using BAW.Models;
using Sokuuhotu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAW.Models
{
    // All comments related to Product/items  
    public class Comment
    {

        public int CommentId { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }


        //Foriegn Keys
        public int ProductId { get; set; }

        public string UserId { get; set; }

        // Navigation Properties

        public virtual Product Product { get; set; }
        public  ApplicationUser User { get; set; }


    }
}