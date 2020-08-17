using eCommerce.Data;
using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;

namespace eCommerce.Services.ApiServices
{
    public class CommentApiService
    {
       
        public CommentApiService()
        {

        }
       

        public async Task<JsonResult> AddComment(Comment comment)
        {
            eCommerceContext context = new eCommerceContext();
           JsonResult jResult = new JsonResult();

            context.Comments.Add(comment);
          await context.SaveChangesAsync();
          jResult.Data = new { Success = true, Message = "The Comment has been add" };

            return jResult;

        }

        public async Task<Comment> GetCommentById(int Id)
        {
            eCommerceContext context = new eCommerceContext();
            return await context.Comments.Include(x => x.User).FirstOrDefaultAsync(x => x.ID == Id);
        }

        public async Task<List<Comment>> GetUserComments(string userId)
        {
            if (userId == null || userId.IsEmpty()) return null;
            eCommerceContext context = new eCommerceContext();

            return await context.Comments.Include(x => x.User).Where(x => x.UserID == userId).ToListAsync();

        }

        public async Task<List<Comment>> GetComments(int? entityId , int? recoredId)
        {
            eCommerceContext context = new eCommerceContext();

            return await context.Comments.Include(x => x.User).Where(a => a.EntityID == entityId || a.RecordID == recoredId).ToListAsync();
        }

       
    }
}
