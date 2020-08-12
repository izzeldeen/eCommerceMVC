using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities
{
    public class Supplier : BaseEntity
    {

        public string SupplierName { get; set; }

        public string Address { get; set; }

        public string Phonenumbher { get; set; }
    }
}
