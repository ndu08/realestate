using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Data
{
    public class CustomerFile
    {
        public int CustomerFileId { get; set; }
        public string FileName { get; set; }

        public string UniqueFileName { get; set; }
        // this column is from DB
        public int CustomerId { get; set; }

        // this is virtual property
        public Customer Customer { get; set; }
    }
}
