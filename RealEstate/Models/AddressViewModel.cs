using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Models
{
    public class AddressViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Street { get; set; }
        [Required]
        [StringLength(50)]
        public string City { get; set; }
        [Required]
        [StringLength(10)]
        public string State { get; set; }

        [Required]
        [StringLength(9, MinimumLength = 5)]
        public string Zip { get; set; }
       
    }
}
