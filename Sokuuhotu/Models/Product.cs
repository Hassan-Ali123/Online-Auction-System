using Sokuuhotu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BAW.Models
{
    public class Product
    {
      
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public string MainImageUrl { get; set; }


        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

       
        public string City { get; set; }


        // Area FinLand
        public string SelectedArea { get; set; }



        public bool AuctionEnd { get; set; }



        public DateTime AuctionEndDate { get; set; }

        public int UserTimeOffset { get; set; }


        //Forign Key
        public int CategoryId { get; set; }


        
        public string UserId { get; set; }

    

    // Navigation property

    public virtual Category Category { get; set; }

        public ApplicationUser User { get; set; }

        public List<BidRequest> Bids { get; set; }



    }
}