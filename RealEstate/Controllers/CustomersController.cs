using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RealEstate.Data;
using RealEstate.Models;

namespace RealEstate.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _hostEnvironment;

        public CustomersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IMapper mapper, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost]
        public IActionResult Search([FromForm]string search)
        {
            return RedirectToAction(nameof(Index), new { search });
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]string search)
        {
            // this line will show all customers on index page, was created by default.
            //var applicationDbContext = _context.Customers.Include(c => c.Address);

            List<Customer> customers = new List<Customer>();

            if (!string.IsNullOrEmpty(search))
            {
                customers = await _context.Customers
                    .Include(c => c.Address)
                    .Include(c => c.Service)
                    .Where(c => c.IsActive && (c.LastName.Contains(search) ||
                                               c.FirstName.Contains(search) ||
                                               c.Address.Street.Contains(search) ||
                                               c.Phone.Contains(search)))
                    .OrderByDescending(c => c.Id)
                    .ToListAsync();
            }
            else
            {
                customers = await _context.Customers
                 .Where(c => c.IsActive)
                 .Include(c => c.Address)
                 .Include(c => c.Service)
                 .OrderByDescending(c => c.Id)
                 .ToListAsync();
            }

            List<CustomerViewModel> customerViewModels = _mapper.Map<List<Customer>, List<CustomerViewModel>>(customers);

            return View(customerViewModels);
        }


        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var customer = await _context.Customers
                .Include(c => c.Address)
                .Include(c => c.Service)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }



        [HttpGet]
        // this id is coming from Route
        public IActionResult Attachments(int id)
        {
            // bringing list of customer files, data from data base
            List<CustomerFile> customerFiles = _context.CustomerFiles.Where(cf => cf.CustomerId == id).ToList();

            List<AttachmentViewModel> attachmentViewModels = _mapper.Map<List<CustomerFile>, List<AttachmentViewModel>>(customerFiles);

            ViewBag.CustomerId = id;
            return View(attachmentViewModels);
           
        }

        [HttpPost]    
        public IActionResult Attachments(int id, IFormFile file)
        {
            //   save image to wwwroot / uploads
            if (file != null)
            {
                string uniqueFilename = GetUniqueFileName(file.FileName);
                var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, file.FileName);

                file.CopyTo(new FileStream(filePath, FileMode.Create));

                _context.CustomerFiles.Add(new CustomerFile
                {
                    UniqueFileName = uniqueFilename,
                    FileName = file.FileName,
                    CustomerId = id
                });

                _context.SaveChanges();
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult Attachment(AttachmentViewModel attachmentViewModel)
        {
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", attachmentViewModel.FileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                //TODO : Remove from file directory and database
            }

            CustomerFile customerFile = _context.CustomerFiles.Find(attachmentViewModel.CustomerId);

            if (customerFile == null)
            {
                return NotFound();
            }
            _context.CustomerFiles.Remove(customerFile);
            _context.SaveChanges();

            return RedirectToAction(nameof(Attachments));

            //return RedirectToAction("Attachments", new { id = attachmentViewModel.CustomerId });

            //if (_hostEnvironment.ContentRootPath + "/uploads/" + attachmentViewModel.FileName)
            //{

            //}
        }


        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            return Path.GetFileNameWithoutExtension(fileName) +
                "_" +
                Guid.NewGuid().ToString().Substring(0, 4) +
                Path.GetExtension(fileName);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel customerVm)
        {
            if (ModelState.IsValid)
            {
                Customer customer = _mapper.Map<CustomerViewModel, Customer>(customerVm);
                customer.DateCreated = DateTime.Now;
                customer.IsActive = true;
                customer.Service = null;

                _context.Add(customer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(customerVm);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Address)
                .Include(c => c.Service)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            CustomerViewModel customerVm = _mapper.Map<Customer, CustomerViewModel>(customer);
            return View(customerVm);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerViewModel customerVm)
        {
            if (id != customerVm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Customer customer = _mapper.Map<CustomerViewModel, Customer>(customerVm);
                    customer.Service = null;
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customerVm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customerVm);
        }
        //public async Task<IActionResult> Edit(int id, Customer customer)
        //{
        //    if (id != customer.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(customer);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CustomerExists(customer.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(customer);
        //}


        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Address)
                .Include(c => c.Service)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            customer.IsActive = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
