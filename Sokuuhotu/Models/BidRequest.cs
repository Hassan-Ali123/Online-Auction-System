using Sokuuhotu.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAW.Models
{
    public class BidRequest
    {

        [Key]
        public int BidRequestId { get; set; }

        public DateTime BidDate { get; set; }

        public decimal FirstPrice { get; set; }

        public decimal SecondPrice { get; set; }

        public bool Accepted { get; set; }
        public int BidCount { get; set; }


        public string Note { get; set; }

        public string UserId { get; set; }


        public int ProductId { get; set; }


        // navigation property

        public virtual Product Product { get; set; }


        public ApplicationUser User { get; set; }




    }
}