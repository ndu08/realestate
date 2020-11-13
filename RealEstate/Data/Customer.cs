using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Data
{
  
    public class Customer
    {
        public Customer()
        {
            CustomerFiles = new HashSet<CustomerFile>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Criteria { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }

        // one to one relations
        public int? AddressId { get; set; }
        public Address Address { get; set; }

        // this column is from dqatabase
        public int? ServiceId { get; set; }  
        public Service Service { get; set; }
        // virtual property that we are adding foe Entity Framework, because we have ServiceId, but in VM we don't need
        //     this property already that is why we are adding ServiceId prop & public string ServiceType { get; set; }

         // one to many relations
        public ICollection<CustomerFile> CustomerFiles { get; set; }
    }
}
