using BAW.Models;
using BAW.Services;
using BAW.ViewModels;
using Hangfire;
using Microsoft.AspNet.Identity;
using NodaTime;
using NodaTime.Text;
using Sokuuhotu.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace BAW.Controllers
{
    [HandleError]
    public class StoreController : Controller
    {


        private readonly StoreServices _service;

       


        public StoreController(StoreServices service)
        {
            _service = service;

        }


        public StoreController() : this(new StoreServices())
        {

        }
        public ActionResult CommingSoon()
        {
            //string zoneId = "Eastern European Summer Time";
            //TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
            //DateTime australianNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);




            //  DateTime datetime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local,
            //TimeZoneInfo.FindSystemTimeZoneById("Europe/Helsinki"));



            //// the time zone to convert to
            //var timeZone = DateTimeZoneProviders.Tzdb["Europe/Helsinki"];
            //// the date as UTC - this could be from a data store
            //var fakeUtcDate = DateTime.UtcNow;
            //// convert to instant from UTC - see http://stackoverflow.com/questions/20807799/using-nodatime-how-to-convert-an-instant-to-the-corresponding-systems-zoneddat
            //var instant = Instant.FromDateTimeUtc(DateTime.SpecifyKind(fakeUtcDate, DateTimeKind.Utc));
            //var result = instant.InZone(timeZone).ToDateTimeUnspecified();


            //TimeZoneInfo cet = TimeZoneInfo.FindSystemTimeZoneById("Central European Summer Time");

            //DateTimeOffset offset = TimeZoneInfo.ConvertTime(DateTime.Now, cet);

            //var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);

            //var date = DateTime.UtcNow.AddHours(offset.Hours+1);

            TimeZone zone = TimeZone.CurrentTimeZone;
            // Get offset.
            TimeSpan offset = zone.GetUtcOffset(DateTime.UtcNow);

            
            var date = DateTime.UtcNow.AddHours(offset.Hours);

            //ViewBag.offset = offset;
            //ViewBag.Date = date;



            if (Request.Cookies["OffsetHelppohutto"] != null)
            {
                var value = Request.Cookies["OffsetHelppohutto"].Value;
                var d = int.Parse(value);
                var date1 = DateTime.UtcNow.AddMinutes(d);

                ViewBag.offset = d;
                ViewBag.Date = date1;

            }




            return View();
        }

        

        // GET: Store/products
        public async Task<ActionResult> Index()
        {



            var products = await _service.ProductsAsync();

            var categories = await _service.CategoriesAsync();


            var vm = new CategoryProductVM()
            {
                Categories = categories,
                Products = products.Take(20)
            };

            // get cateogries list and Areas list

            ViewBag.Categorylist = new SelectList(await _service.CategoriesAsync(), "CategoryId", "CategoryName");

            ViewBag.areas = Areas().OrderBy(x=>x.Value); // function at bottom

            return View(vm);
        }








        // Get: Store/productdetail
        public async Task<ActionResult> ProductDetails(int id)
        {
           

            var productDetails = await _service.ProductByIdAsync(id);

            if (productDetails.SingleProduct !=null)
            {
                var t = TempData["Message"]; // moving data action to action

            if (t != null)

                ViewData["Message"] = t; // pass data to Vew

            return View(productDetails);
            }


            return RedirectToAction("PageNotFound","ErrorPages");

        }


        public async Task<ActionResult> ProductsByCategory(int CategoryId)
        {
            var products = await _service.ProductsByCategoryIdAsync(CategoryId);


            return View(products);

        }




        // ***********  Comments ***********
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddComment(ProductDetailsVM model,int proId)
        {
           var userId = User.Identity.GetUserId();
             await _service.AddCommentsAsync(model.Comment, userId, proId);


            return RedirectToAction("ProductDetails",new { id = proId });

        }








        // ***********  Search ***********


        //[HttpGet]
        //public ActionResult Search()
        //{

        //    return PartialView("_SearchBarPartial");
        //}



        [ChildActionOnly]
        public ActionResult Search()
        {
            var id = User.Identity.GetUserId();
            ApplicationUser currentuser;
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                currentuser = db.Users.Where(u=> u.Id == id)
                    .SingleOrDefault();
            }
            return PartialView("_SearchBarPartial", currentuser);
        }




        [HttpGet]
        public async Task<ActionResult> PostSearch(string search)
        {
            if (search != null)
            {
                try
                {
                    var searchlist = await _service.SearchAsync(search);

                    return PartialView("ProductsByCategory", searchlist);
                }
                catch (Exception e)
                {
                    // handle exception
                }
            }
            return RedirectToAction("PageNotFound", "ErrorPages");
        }


        //*******Advance Search *********

        [HttpPost]
        public async Task<ActionResult> Index(string keywords, string areas, int? CategoryId)
        {

            var result = await _service.AdvanceSearchAsync(keywords, areas, CategoryId);
            return PartialView("ProductsByCategory", result);

        }


        //Function
        // Get list of Finladn Areas

        public IEnumerable<SelectListItem> Areas()
        {

            List<SelectListItem> collection = new List<SelectListItem>();
            collection.Add(new SelectListItem { Text = "Uusimaa", Value = "Uusimaa" });
            collection.Add(new SelectListItem { Text = "Varsinais-Suomi", Value = "Varsinais-Suomi" });
            collection.Add(new SelectListItem { Text = "Satakunta", Value = "Satakunta", });
            collection.Add(new SelectListItem { Text = "Häme", Value = "Häme", });
            collection.Add(new SelectListItem { Text = "Pirkanmaa", Value = "Pirkanmaa" });
            collection.Add(new SelectListItem { Text = "Päijät-Häme", Value = "Päijät-Häme" });
            collection.Add(new SelectListItem { Text = "Kymenlaakso", Value = "Kymenlaakso", });
            collection.Add(new SelectListItem { Text = "Etelä-Karjala", Value = "Etelä-Karjala", });
            collection.Add(new SelectListItem { Text = "Etelä-Savo", Value = "Etelä-Savo", });
            collection.Add(new SelectListItem { Text = "Pohjois-Savo", Value = "Pohjois-Savo", });
            collection.Add(new SelectListItem { Text = "Pohjois-Karjala", Value = "Pohjois-Karjala", });
            collection.Add(new SelectListItem { Text = "Keski-Suomi", Value = "Keski-Suomi", });
            collection.Add(new SelectListItem { Text = "Etelä-Pohjanmaa", Value = "Etelä-Pohjanmaa", });
            collection.Add(new SelectListItem { Text = "Pohjanmaa", Value = "Pohjanmaa", });
            collection.Add(new SelectListItem { Text = "Keski-Pohjanmaa", Value = "Keski-Pohjanmaa", });
            collection.Add(new SelectListItem { Text = "Pohjois-Pohjanmaa", Value = "Pohjois-Pohjanmaa", });
            collection.Add(new SelectListItem { Text = "Kainuu", Value = "Kainuu", });
            collection.Add(new SelectListItem { Text = "Lappi", Value = "Lappi", });
            collection.Add(new SelectListItem { Text = "Ahvenanmaa", Value = "Ahvenanmaa", });


            return collection;


        }





       
      
        }









    }




