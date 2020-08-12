using eCommerce.Data;
using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class SharedService
    {
        #region Define as Singleton
        private static SharedService _Instance;

        public static SharedService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SharedService();
                }

                return (_Instance);
            }
        }

        private SharedService()
        {
        }
        #endregion

        public int SavePicture(Picture picture)
        {
            eCommerceContext context = new eCommerceContext();

            context.Pictures.Add(picture);

            context.SaveChanges();

            return picture.ID;
        }

        public eCommerceUser GetUserByUsername(string Username)
        {
            eCommerceContext context = new eCommerceContext();

            var user = context.Users.Find(Username);

            return user;
        }

        public eCommerceUser GetUserById(string Id)
        {
            eCommerceContext context = new eCommerceContext();

            var user = context.Users.Find(Id);

            return user;
        }
    }
}
