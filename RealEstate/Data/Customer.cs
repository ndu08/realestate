using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Data
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string Description { get; set; }
        public string Criteria { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public int? AddressId { get; set; }
        public Address Address { get; set; }

        public int? ServiceId { get; set; }
        public Service Service { get; set; }

    }
}
