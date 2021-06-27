using Sokuuhotu.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI;

namespace Sokuuhotu
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            timer_Elapsed();

        }


        

        private void timer_Elapsed()
        {
            ApplicationDbContext _db = new ApplicationDbContext();
            /*.Where(p=>p.StartDate.Date == )*/
            var products = _db.Products.ToList();

            foreach (var item in products)
            {
                if (item.EndDate <= DateTime.Now)
                {
                    item.AuctionEnd = true;
                    _db.Entry(item).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }
        }



    }
    }


