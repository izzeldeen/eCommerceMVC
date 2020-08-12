using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities
{
    public class CountryCode : BaseEntity
    {
        public string Countryname { get; set; }
        public int Code { get; set; }
    }
}
