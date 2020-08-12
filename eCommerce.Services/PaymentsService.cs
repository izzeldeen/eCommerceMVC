using eCommerce.Data;
using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class PaymentsService
    {
        #region Define as Singleton
        private static PaymentsService _Instance;

        public static PaymentsService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new PaymentsService();
                }

                return (_Instance);
            }
        }

        private PaymentsService()
        {
        }
        #endregion

        public Payment GetUserPayment(string userId)
        {
            eCommerceContext context = new eCommerceContext();

            return context.Payment.FirstOrDefault(x => x.UserId == userId);
        }

        public Payment AddUserPayment(Payment payment)
        {
            eCommerceContext context = new eCommerceContext();

            var paymentToAdd = context.Payment.Add(payment);
            context.SaveChanges();

            return paymentToAdd;
        }
    }
}
