using BAW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sokuuhotu.ViewModels
{
    public class UserDashboard
    {

        public int  SendingBidHistory { get; set; }

        public int BidAcceptBySeller { get; set; }

        public int BuyerBidAcceptedByMe { get; set; }



    }
}