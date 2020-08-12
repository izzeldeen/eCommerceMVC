using eCommerce.Entities;
using eCommerce.Services;
using System;
using System.Collections.Generic;
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
        [System.Web.Http.HttpGet]
        public Comment GetCommentById(int Id) => CommentsService.Instance.GetComment(Id);
        [System.Web.Http.HttpGet]
        public List<Comment> GetComments(int entityID, int recoredID, int recoredSize) => CommentsService.Instance.GetComments(entityID, recoredID, recoredSize).ToList();
        //[System.Web.Http.HttpGet]
        // public List<Comment> GetComments(string userID, string searchTerm, int entityID, int? pageNo, int recoredsSize) => CommentsService.Instance.GetComments(userID, searchTerm, entityID, pageNo, recoredsSize);
        //[System.Web.Http.HttpGet]
        //public int GetTotalComment(string userID, string searchTerm, int entityID) => CommentsService.Instance.GetCommentsTotalCount(userID, searchTerm, entityID);





    }
}
