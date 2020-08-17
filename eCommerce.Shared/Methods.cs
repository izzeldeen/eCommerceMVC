using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eCommerce.Shared
{
    public class Methods
    {
        public static List<Category> GetCategoryHierarchy(Category category, List<Category> allCategories)
        {
            if (category != null && allCategories != null && allCategories.Count > 0)
            {
                var categories = new List<Category>() { category };

                Category parentCategory = null;

                var parentCategoryID = category.ParentCategoryID;

                do
                {
                    parentCategory = GetCategoryParent(parentCategoryID, allCategories);

                    if (parentCategory != null)
                    {
                        categories.Add(parentCategory);

                        parentCategoryID = parentCategory.ParentCategoryID;
                    }
                    else
                    {
                        parentCategoryID = null;
                    }
                } while (parentCategory != null);

                categories.Reverse();

                return categories;
            }

            return null;
        }

        public static Category GetCategoryParent(int? parentCategoryID, List<Category> allCategories)
        {
            return parentCategoryID.HasValue ? allCategories.FirstOrDefault(x => x.ID == parentCategoryID) : null;
        }

        public static List<Category> GetAllCategoryChildrens(Category category, List<Category> allCategories)
        {
            if (category != null && allCategories != null && allCategories.Count > 0)
            {
                var categories = new List<Category>() { category };

                var childCategories = GetCategoryChildren(category.ID, allCategories);

                foreach (var childCategory in childCategories)
                {
                    categories.Add(childCategory);

                    GetAllCategoryChildrens(childCategory, allCategories);
                }

                return categories;
            }

            return null;
        }

        public static List<Category> GetCategoryChildren(int parentCategoryID, List<Category> allCategories)
        {
            return allCategories.Where(x => x.ParentCategoryID == parentCategoryID).ToList();
        }

        public string RemoveRegx(string elem)
        {
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("<[^>]*>");

            elem = rx.Replace(elem, "");
            elem = Regex.Replace(elem, @"&lt;.+?&gt;|&nbsp;|\t|\r|\n", "").Trim();

            return elem;
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
