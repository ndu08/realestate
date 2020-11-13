using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
       
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
       
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
       
        public DateTime DOB { get; set; }
       
        [Required]
        [StringLength(10)]
        //[DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [StringLength(1000)]
        public string Criteria { get; set; }
        public AddressViewModel Address { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceType { get; set; }
        public DateTime DateCreated { get; set; }
        public string DOBFormatted {
            get {
                return DOB.ToString("MM-dd-yyyy"); 
            } 
        }
        public bool IsActive { get; set; }
    }
}
