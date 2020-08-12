using eCommerce.Data;
using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class CommentsService
    {
        #region Define as Singleton
        private static CommentsService _Instance;

        public static CommentsService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new CommentsService();
                }

                return (_Instance);
            }
        }

        private CommentsService()
        {
        }
        #endregion

        public bool AddComment(Comment comment)
        {
            eCommerceContext context = new eCommerceContext();

            context.Comments.Add(comment);

            return context.SaveChanges() > 0;
        }

        public List<Comment> GetComments(int entityID, int recordID, int recordsSize = 20)
        {
            eCommerceContext context = new eCommerceContext();

            return context.Comments.Where(x => x.EntityID == entityID && x.RecordID == recordID).OrderByDescending(x => x.TimeStamp).Take(recordsSize).ToList();
        }

        public List<Comment> GetComments(string userID, string searchTerm, int entityID, int? pageNo, int recordsSize)
        {
            eCommerceContext context = new eCommerceContext();

            pageNo = pageNo ?? 1;
            var skipCount = (pageNo.Value - 1) * recordsSize;

            var comments = context.Comments.Where(x => x.EntityID == entityID)
                                   .AsQueryable();

            if (!string.IsNullOrEmpty(userID))
            {
                comments = comments.Where(x => x.UserID == userID);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                comments = comments.Where(x => x.Text.ToLower().Contains(searchTerm.ToLower()));
            }

            return comments.OrderByDescending(x => x.TimeStamp)
                           .Skip(skipCount)
                           .Take(recordsSize)
                           .ToList();
        }

        public int GetCommentsTotalCount(string userID, string searchTerm, int entityID)
        {
            eCommerceContext context = new eCommerceContext();

            var comments = context.Comments.Where(x => x.EntityID == entityID)
                                   .AsQueryable();

            if (!string.IsNullOrEmpty(userID))
            {
                comments = comments.Where(x => x.UserID == userID);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                comments = comments.Where(x => x.Text.ToLower().Contains(searchTerm.ToLower()));
            }

            return comments.Count();
        }

        public Comment GetComment(int ID)
        {
            eCommerceContext context = new eCommerceContext();

            return context.Comments.Find(ID);
        }

        public bool DeleteComment(int ID)
        {
            eCommerceContext context = new eCommerceContext();

            var comment = context.Comments.Find(ID);

            if (comment != null)
            {
                context.Entry(comment).State = System.Data.Entity.EntityState.Deleted;
            }
            return context.SaveChanges() > 0;
        }
    }
}
