using eCommerce.Data;
using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class CountryCodesService
    {
        #region Define as Singleton
        private static CountryCodesService _Instance;

        public static CountryCodesService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new CountryCodesService();
                }

                return (_Instance);
            }
        }

        private CountryCodesService()
        {
        }
        #endregion

        public List<CountryCode> GetCountryCodes()
        {

            eCommerceContext context = new eCommerceContext();

            var codes = context.CountryCodes.ToList();

            return codes;
        }


    }
}
