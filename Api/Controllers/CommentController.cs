using eCommerce.Data;
using eCommerce.Entities;
using eCommerce.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using WebGrease.Css.Ast;
namespace Api.Controllers
{
    public class CommentController : ApiController
    {
        JsonResult jsonReuslt = new JsonResult();

        [System.Web.Http.HttpGet]
        public JsonResult AddComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                var reuslt = CommentsService.Instance.AddComment(comment);
                if (reuslt == true)
                {
                    jsonReuslt.Data = new { Success = true, RequiresVerification = false };

                    return jsonReuslt;
                 }
             }
            jsonReuslt.Data = new { Success = false, Messages = string.Format("Failed to save comment") };

            return jsonReuslt;
        }
        
        public Comment GetCommentById(int Id) 
            {
            if (Id == null ) return null;
            eCommerceContext context = new eCommerceContext();

            var comment = context.Comments.Include(x => x.User).Include(x=>x.User.Picture).FirstOrDefault(x => x.ID == Id);
            return comment;
             }
        [System.Web.Http.HttpGet]
        public List<Comment> GetComments(int entityID, int recoredID, int recoredSize) => CommentsService.Instance.GetComments(entityID, recoredID, recoredSize).ToList();

       public List<Comment> GetUserComments(string userId)
        {
            eCommerceContext context = new eCommerceContext();
            var UserComments = context.Comments.Include(x=>x.User).Where(x => x.UserID == userId).ToList();
            return UserComments;
        }
       






    }
}
