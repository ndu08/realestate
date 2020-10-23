using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Models
{
    public class AttachmentViewModel
    {
        public string Description { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
