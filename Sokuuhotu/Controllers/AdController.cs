using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BAW.Models;
using BAW.Services;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web.Hosting;

namespace BAW.Controllers
{


    [Authorize]
    //It Handels all activity related to post an "Ad"   
    public class AdController : Controller
    {



        private readonly StoreServices _service;

      



        public AdController(StoreServices service)
        {
            _service = service;

        }


        public AdController() : this(new StoreServices())
        {

        }







        public async Task<ActionResult> PostedProduct()
        {


            var userId = User.Identity.GetUserId();

            var products = await _service.GetPostedProductsAsync(userId);
            //var totalbids = 0;

            //foreach (var item in products)
            //{
            //    totalbids += item.Bids.Where(b => b.FirstPrice != 0).Count() +
            //        item.Bids.Where(b => b.SecondPrice != 0).Count();

            //}

            //ViewBag.AllBids = totalbids;
            return View(products);

        }

        
        public async Task<ActionResult> ItemsInAuction(bool status )
        {


            var userId = User.Identity.GetUserId();

            var products = await _service.GetProductsByStatusAsync(status, userId);

            return View(products);

        }


        [Authorize]
        // GET: Ad/Create
        public async Task<ActionResult> Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "1 Day", Value = "1" });
            items.Add(new SelectListItem { Text = "3 Days ", Value = "3" });
            items.Add(new SelectListItem { Text = "1 Week", Value = "7", });
            items.Add(new SelectListItem { Text = "2 Weeks", Value = "14", });



            ViewBag.closeDate = items;
            ViewBag.Categorylist =  new  SelectList(await _service.CategoriesAsync(), "CategoryId", "CategoryName");
            //ViewBag.areas = Areas().OrderBy(x=>x.Value); // function at bottom
            return View();
        }


        //Accepts product Detail(MainImage also), and Multiple Images of product

         [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product,List<HttpPostedFileBase> files, HttpPostedFileBase mainImage , int closeDate,string areas)
        {
          var  userId = User.Identity.GetUserId();



            int offset=0;
            DateTime clientDate= DateTime.UtcNow ;
            if (Request.Cookies["OffsetHelppohutto"] != null)
            {
                var value = Request.Cookies["OffsetHelppohutto"].Value;
                var offsetminutes = int.Parse(value);
                clientDate = DateTime.UtcNow.AddMinutes(offsetminutes);
                offset = offsetminutes;
            }
           

            if (ModelState.IsValid)
            {
                if (closeDate != 0)
                {
                    //await _service.AddProductImagesAsync(product.ProductId, files);

                    await _service.AddProductAsync(product, files, mainImage, userId, closeDate,clientDate, offset);


                    return RedirectToAction("PostedProduct");
                }
               
            }






            //Closing days Static list

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "1 Day", Value = "1" });
            items.Add(new SelectListItem { Text = "3 Days", Value = "3" });
            items.Add(new SelectListItem { Text = "1 Week", Value = "7", });
            items.Add(new SelectListItem { Text = "2 Weeks", Value = "14", });

            ViewBag.closeDate = items;
            //ViewBag.areas = Areas(); // function at bottom

            ViewBag.Categorylist = new SelectList(await _service.CategoriesAsync(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Ad/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = _service.FindProduct(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "1 Day", Value = "1" });
            items.Add(new SelectListItem { Text = "2 Days", Value = "2" });
            items.Add(new SelectListItem { Text = "1 week", Value = "7", });
            items.Add(new SelectListItem { Text = "2 weeks", Value = "14", });

            ViewBag.closeDate = items;

            //ViewBag.areas = Areas(); // function at bottom

            ViewBag.Categorylist = new SelectList(await _service.CategoriesAsync(), "CategoryId", "CategoryName");
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product product, int closeDate)
        {
            if (ModelState.IsValid)
            {
                await _service.ProductEditAsync(product, closeDate);
                return RedirectToAction("PostedProduct");
            }


            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "1 Day", Value = "1" });
            items.Add(new SelectListItem { Text = "2 Days", Value = "2" });
            items.Add(new SelectListItem { Text = "1 week", Value = "7", });
            items.Add(new SelectListItem { Text = "2 weeks", Value = "14", });

            ViewBag.closeDate = items;

            //ViewBag.areas = Areas(); // function at bottom

            ViewBag.Categorylist = new SelectList(await _service.CategoriesAsync(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }


        // Delete

        public async Task<ActionResult> Delete(int proId)
        {
           
            await _service.ItemDelete(proId);

            return RedirectToAction("PostedProduct");

        }






        //Finland Areas
        public IEnumerable<SelectListItem> Areas()
        {

            List<SelectListItem> collection = new List<SelectListItem>();
            collection.Add(new SelectListItem { Text = "Ahvenanmaa", Value = "Ahvenanmaa", });
            collection.Add(new SelectListItem { Text = "Etelä-Karjala", Value = "Etelä-Karjala", });
            collection.Add(new SelectListItem { Text = "Etelä-Savo", Value = "Etelä-Savo", });
            collection.Add(new SelectListItem { Text = "Häme", Value = "Häme", });

            collection.Add(new SelectListItem { Text = "Uusimaa", Value = "Uusimaa" });
            collection.Add(new SelectListItem { Text = "Varsinais-Suomi", Value = "Varsinais-Suomi" });
            collection.Add(new SelectListItem { Text = "Satakunta", Value = "Satakunta", });
            collection.Add(new SelectListItem { Text = "Pirkanmaa", Value = "Pirkanmaa" });
            collection.Add(new SelectListItem { Text = "Päijät-Häme", Value = "Päijät-Häme" });
            collection.Add(new SelectListItem { Text = "Kymenlaakso", Value = "Kymenlaakso", });
             collection.Add(new SelectListItem { Text = "Pohjois-Savo", Value = "Pohjois-Savo", });
            collection.Add(new SelectListItem { Text = "Pohjois-Karjala", Value = "Pohjois-Karjala", });
            collection.Add(new SelectListItem { Text = "Keski-Suomi", Value = "Keski-Suomi", });
            collection.Add(new SelectListItem { Text = "Etelä-Pohjanmaa", Value = "Etelä-Pohjanmaa", });
            collection.Add(new SelectListItem { Text = "Pohjanmaa", Value = "Pohjanmaa", });
            collection.Add(new SelectListItem { Text = "Keski-Pohjanmaa", Value = "Keski-Pohjanmaa", });
            collection.Add(new SelectListItem { Text = "Pohjois-Pohjanmaa", Value = "Pohjois-Pohjanmaa", });
            collection.Add(new SelectListItem { Text = "Kainuu", Value = "Kainuu", });
            collection.Add(new SelectListItem { Text = "Lappi", Value = "Lappi", });


            return collection;


        }




    }
}
