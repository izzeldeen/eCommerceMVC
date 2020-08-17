using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Services.ApiServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
namespace Api.Controllers
{
    public class CommentController : ApiController
    {
        CommentApiService CommentApiService = new CommentApiService();


        [System.Web.Http.HttpGet]
        public async Task<Comment> GetCommentById(int Id) => await CommentApiService.GetCommentById(Id);
        [System.Web.Http.HttpGet]
        public async Task<List<Comment>> GetUserComments(string userId) => await CommentApiService.GetUserComments(userId);

        [System.Web.Http.HttpGet]
        public async Task<List<Comment>> GetComments(int? entityID, int? recoredID) => await CommentApiService.GetComments(entityID, recoredID);



        [System.Web.Http.HttpPost]
        public async Task<JsonResult> AddComment(Comment comment)
        {
            JsonResult jResult = new JsonResult();
            if (ModelState.IsValid)
            {
                return await CommentApiService.AddComment(comment);   
               
             }

            jResult.Data = new { Success = false, Messages = string.Format("Failed to save comment") };

            return jResult;
        }


      


    }
}
