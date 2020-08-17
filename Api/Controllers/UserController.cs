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
using eCommerce.Services.ApiServices;
using System.Security.Cryptography;
using System.Web.Helpers;
using com.sun.org.apache.bcel.@internal.generic;
using eCommerceMVC.ViewModels;

namespace Api.Controllers
{

    public class UserController : ApiController
    {
        private eCommerceContext context;
        private  eCommerceUserManager _userManager;
        private readonly HttpPostedFileBase _httPostFileBase;
        UserApiService userApiService = new UserApiService();





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
            eCommerceUser result = await userApiService.GetUser(model.UserName, model.Email);


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
            var user = await userApiService.GetUser(username , email);
            if (user != null)
            {
                eCommerceContext context = new eCommerceContext();

             var res =   VerifyHashedPassword(user.PasswordHash , password);
                if(res == true)
                {
                    if (user.Picture != null)
                    {
                        if (user.Picture.URL.Contains("api") != true)
                        {
                            user.Picture.URL = "http://jomlahjo.com/Content/images/" + user.Picture.URL;
                        }
                        else
                        {
                            user.Picture.URL = "http://" + user.Picture.URL;
                        }

                    }

                    return user;
                }
            }

         
            return null;
         }

     
        [System.Web.Http.HttpPost]
        public async Task<JsonResult> ResetPassword(ResetPasswordVM ResetVM)
        {
            eCommerceContext context = new eCommerceContext();

            var UserManager = new UserManager<eCommerceUser>(new UserStore<eCommerceUser>(context));
           

            JsonResult jResult = new JsonResult();


            eCommerceUser user = await userApiService.GetUserById(ResetVM.UserId);



            if (user != null)
            {
               
                var result = await UserManager.ChangePasswordAsync(ResetVM.UserId, ResetVM.OldPassword, ResetVM.NewPassword);
                if(result.Succeeded)
                {
                    await userApiService.Save();
                    jResult.Data = new { Success = true, Messages = "Your password has been reset. Please login with your updated credentials now." };

                    return jResult;
                }

                else
                {
                    jResult.Data = new { Success = false, Messages = "Unable to reset password." };
                }



            }
          


            return jResult;
        }

        [System.Web.Http.HttpPost]
        public async Task<JsonResult> UploadPhoto(string Userid)
        {
            JsonResult jResult = new JsonResult();
            var file = HttpContext.Current.Request.Files.Count > 0 ?
            HttpContext.Current.Request.Files[0] :  null;
            if (file == null)
            {
                jResult.Data = new { Success = false, Message = "Photo is empty or null" };

                return jResult;
            }
           // eCommerceContext context = new eCommerceContext();
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
           
            
            
            return await userApiService.AddUserPictuer(Userid,picture);
          
        }

        [System.Web.Http.HttpPost]
        public async Task<JsonResult> UpdateProfile(eCommerceUser model)
        {
            JsonResult jResult = new JsonResult();
            eCommerceContext context = new eCommerceContext();

            if (model != null)
            {
                var UserManager = new UserManager<eCommerceUser>(new UserStore<eCommerceUser>(context));
                var user = await UserManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    user.FullName = model.FullName;
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Country = model.Country;
                    user.City = model.City;
                    user.Address = model.Address;
                    user.ZipCode = model.ZipCode;

                    var result = await UserManager.UpdateAsync(user);

                    jResult.Data = new { Success = result.Succeeded, Message = string.Join("\n", result.Errors) };

                    return jResult;
                }
            }
            else
            {
                jResult.Data = new { Success = false, Message = "Invalid User" };
            }

            return jResult;
        }
     
        
        
        public async Task<JsonResult> SendForgetPasswordEmail(string UserName , string email)
        {
            eCommerceContext context = new eCommerceContext();
            JsonResult jResult = new JsonResult();
            eCommerceUser user = await userApiService.GetUser(UserName , email);
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




        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }


        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        public static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }


    }
}


