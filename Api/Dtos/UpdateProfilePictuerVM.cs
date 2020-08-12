using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Dtos
{
    public class UpdateProfilePictuerVM
    {
        public string UserId { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }


    }
}