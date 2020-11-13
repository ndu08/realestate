using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Models
{
    public class AttachmentViewModel
    {
        public IFormFile FormFile { get; set; }
        public int CustomerId { get; set; }
        public string FileName { get; set; }
        public string UniqueFileName { get; set; }
    }
}
