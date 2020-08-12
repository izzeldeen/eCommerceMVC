using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities
{
    public class Payment : BaseEntity
    {
        public string UserId { get; set; }
        public virtual eCommerceUser User { get; set; }
        public string TapUserId { get; set; }
    }
}
