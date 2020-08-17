using eCommerce.Data;
using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace eCommerce.Services.ApiServices
{
    public class UserApiService
    {
        eCommerceContext context = new eCommerceContext();
        public async Task<eCommerceUser> GetUser(string userName , string email)
        {
            return await context.Users.Include(x=>x.Picture).FirstOrDefaultAsync(x => x.UserName == userName || x.Email == email);
        }

        public async Task<eCommerceUser> GetUserById(string Id)
        {
            
            return await context.Users.Include(x => x.Picture).FirstOrDefaultAsync(x => x.Id == Id);
        }


        public async Task<JsonResult> Save()
        {
            JsonResult jResult = new JsonResult();
            using (context.SaveChangesAsync());

             jResult.Data = new  { Success = true, Message = "Chnages Saved Successfly" };

            return jResult;
        }

        public async Task<JsonResult> AddUserPictuer(string UserId , Picture pictuer)
        {
            JsonResult jResult = new JsonResult();
            var NewPictuer = context.Pictures.Add(pictuer);
            await context.SaveChangesAsync();
            var FindUser = context.Users.Find(UserId);

            if(FindUser != null)
            {
                FindUser.PictureID = NewPictuer.ID;
                await context.SaveChangesAsync();
                jResult.Data = new { Success = true, Message = "Photo changed successfly" };

                return jResult;
                
            }
            
                jResult.Data = new { Success = false, Message = "UserId is Not Valid" };
            

          

            return jResult;

        }


        public async Task<JsonResult> UpdateUser(eCommerceUser user)
        {
            JsonResult jResult = new JsonResult();
             context.Users.AddOrUpdate(user);

            await context.SaveChangesAsync();
            jResult.Data = new { Success = true, Messages = string.Format("Your personal information have been updated") };

            return jResult;

        }
    }
}
