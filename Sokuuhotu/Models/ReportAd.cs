using BAW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sokuuhotu.Models
{
    public class ReportAd
    {

        public int ReportAdId { get; set; }

        public string ReportMsg { get; set; }

        public DateTime Date { get; set; }


        public bool IsRead { get; set; }

        //FK
        public int ProductId { get; set; }
        //navigation Property
        public Product Product { get; set; }

    }
}