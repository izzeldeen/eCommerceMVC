using eCommerce.Entities;
using eCommerce.Services;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using eCommerce.Data;
using System.Linq;
using Microsoft.AspNet.Identity.Owin;
using System;
using Api.Dtos;
using System.Data.Entity.Migrations;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.DataProtection;
using System.IO;
using System.Web.Hosting;
using Microsoft.AspNetCore.Http.Internal;
using com.sun.corba.se.spi.activation;

namespace Api.Controllers
{

    public class UserController : ApiController
    {
        private eCommerceContext context;

        private  eCommerceUserManager _userManager;

        private readonly HttpPostedFileBase _httPostFileBase;






        public UserController(eCommerceContext db ,eCommerceUserManager userManager , HttpPostedFileBase httpPostedFileBase )
        {
            context = db;
            _userManager = userManager;
            _httPostFileBase = httpPostedFileBase;
        }

        public UserController()
        {
        }
        [System.Web.Http.HttpPost]
        public async Task<JsonResult> Register([System.Web.Http.FromBody] eCommerceUser model)
        {
            JsonResult jsonResult = new JsonResult();
            eCommerceContext context = new eCommerceContext();

           


            var fullName = model.FirstName + " " + model.MiddleName + " " + model.LastName;

            if (!ModelState.IsValid)
            {
                jsonResult.Data = new { Success = false, Messages = string.Format("ModelState is not valid") };

                return jsonResult;


            }
            eCommerceUser result = context.Users.FirstOrDefault(x=>x.UserName == model.UserName || x.Email == model.Email);


            var UserManager = new UserManager<eCommerceUser>(new UserStore<eCommerceUser>(context));

            if (result == null)
            {
                var NewUser = new eCommerceUser
                {
                    FullName = fullName,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    PhoneNumber = model.PhoneNumber,
                    LastName = model.LastName,
                    City = model.City,
                    CountryCode = model.CountryCode,
                    ZipCode = model.ZipCode,
                    Email = model.Email,
                    UserName = model.UserName,
                    PasswordHash = model.PasswordHash,
                    EmailConfirmed = false,
                    TwoFactorEnabled = false,
                    PhoneNumberConfirmed=false,
                    LockoutEndDateUtc=null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                    
                    };
                var CreateNewUser = await UserManager.CreateAsync(NewUser, model.PasswordHash);

                await context.SaveChangesAsync();

                UserManager.AddToRole( NewUser.Id, "User");

               
                jsonResult.Data = new { Success = true, RequiresVerification = false };
                return jsonResult;
            }
        
            else
            {
                if(result.UserName == model.UserName)
                
                    jsonResult.Data = new { Success = false, Message = "User Name is exist" };
                

                else

                    jsonResult.Data = new { Success = false, Message = "Email is exist" };
                  }
              return jsonResult;
                }
        [System.Web.Http.HttpGet]
        public async Task<eCommerceUser> Login(string username,string email ,  string password)
        {
            eCommerceContext context = new eCommerceContext();
            var res = context.Users.Where(x => x.UserName == username);

            
            var user = await context.Users.Include(x=>x.Picture).FirstOrDefaultAsync(u => u.UserName == username || u.Email == email);
            if (user != null)
            {   
                user.PasswordHash = password;

                return   user;
            }

            return user;
  }

        [System.Web.Http.HttpGet]
        public async Task<eCommerceUser> UserName(string username)
        {
            var user = await context.Users.Include(x=>x.Picture).FirstOrDefaultAsync(u => u.UserName == username);


            return user;   
        }





        [System.Web.Http.HttpPost]
        public async Task<JsonResult> ResetPassword(ResetPasswordVM ResetVM)
        {
            eCommerceContext context = new eCommerceContext();

           
          JsonResult jResult = new JsonResult();

          var user =  context.Users.Where(x=>x.Id == ResetVM.UserId).FirstOrDefault();

            if (user != null)
            {

                if(user.PasswordHash == ResetVM.OldPassword)
                {
                    user.PasswordHash = ResetVM.NewPassword;

                    await context.SaveChangesAsync();
                    jResult.Data = new { Success = true, Messages = "Your password has been reset. Please login with your updated credentials now." };

                    return jResult;
                }
                 }
            else
            {
                jResult.Data = new { Success = false, Messages = "Unable to reset password." };
            }

            return jResult;
        }

        [System.Web.Http.HttpPost]
        public async Task<JsonResult> UploadPhoto(string Userid)
        {
            var file = HttpContext.Current.Request.Files.Count > 0 ?
            HttpContext.Current.Request.Files[0] : null;
            eCommerceContext context = new eCommerceContext();
            JsonResult jResult = new JsonResult();
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extesntion = Path.GetExtension(file.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extesntion;
            // System.Web.Hosting.HostingEnvironment.MapPath(path);
            string pathApi =Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/images/"), fileName);
            if (file.FileName.Length > 0)
            {
                file.SaveAs(pathApi);
              }
            Picture picture = new Picture()
            { URL = "api.jomlahjo.com/Content/images/" + fileName,
                ModifiedOn = DateTime.Now
            };
            context.Pictures.Add(picture);
            var user = context.Users.FirstOrDefault(x => x.Id == Userid);
            if(user != null)
            {
                user.PictureID = picture.ID;
                await context.SaveChangesAsync();
                jResult.Data = new { Success = false, Message = "failed To Upload image" };

                return jResult;
            }
            jResult.Data = new { Success = true, Message = "Photo Upload" };
              return jResult;
              }

        [System.Web.Http.HttpPost]
        public  JsonResult UpdatePersonailInformation(eCommerceUser model)
        {
            JsonResult jsonResult = new JsonResult();
            eCommerceContext context = new eCommerceContext();


            if (!ModelState.IsValid)
            {
                jsonResult.Data = new { Success = false, Messages = string.Format("ModelState is not valid") };

                return jsonResult;    }

            var res =  context.Users.Find(model.Id);
            if(res != null)
            {

                context.Users.AddOrUpdate(model);
                context.SaveChanges();
                jsonResult.Data = new { Success = true, Messages = string.Format("Your personal information have been updated") };

                return jsonResult;

            }

            jsonResult.Data = new { Success = false, Messages = string.Format("user Id is wrong") };

            return jsonResult;

        }

        public JsonResult SendForgetPasswordEmail(string UserName)
        {
            eCommerceContext context = new eCommerceContext();
            JsonResult jResult = new JsonResult();
            var user = context.Users.Where(x => x.UserName == UserName).FirstOrDefault();
            var UserManager = new UserManager<eCommerceUser>(new UserStore<eCommerceUser>(context));
            var provider = new DpapiDataProtectionProvider("SampleAppName");
            UserManager.UserTokenProvider = new DataProtectorTokenProvider<eCommerceUser>(
                                                 provider.Create("SampleTokenName"));

            string Code = UserManager.GeneratePasswordResetToken(user.Id);

            var callbackUrl = "http://jomlahjo.com/reset-password?userId=" + user.Id + "&code=" + Code;

           UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");


         //For more security alawys return True

         jResult.Data = new { Success = true, Message = "Check your Email for reset your password" }; 

            return jResult;
      }

       
       
    }
}


