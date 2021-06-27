using Sokuuhotu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BAW.Areas.Admin.Controllers
{

   
    public class DashBoardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/DashBoard
        public ActionResult YourDashBoard()
        {


            return View();
        }


        public ActionResult ViewAds()
        {
            var model = db.Products.OrderByDescending(x => x.ProductId).ToList();

            return View(model);

        }

        
        public ActionResult ItemDelete(int id)
        {
            var result = db.Products.Find(id);
            db.Products.Remove(result);
            db.SaveChangesAsync();

            return RedirectToAction("ViewAds");

        }



        public ActionResult ViewImmoralAd()
        {
            var model = db.ReportAd.Include("Product").OrderByDescending(x => x.ReportAdId).ToList();

            return View(model);

        }


        public ActionResult ContactRequest()
        {

            var model = db.ContactUs.OrderByDescending(x=>x.ContactId).ToList();

            return View(model);

        }




        public ActionResult AllCustomer()
        {

            var model = db.Users.OrderByDescending(x => x.Id).ToList();

            return View(model);

        }

        public ActionResult Companies()
        {

            var model = db.Users.OrderByDescending(x => x.Id).Where(x=> x.AsCompany ==true).ToList();

            return View(model);

        }



    }
}