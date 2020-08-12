using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities
{
    public class ProductCost : BaseEntity
    {
        public int ProductID { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
