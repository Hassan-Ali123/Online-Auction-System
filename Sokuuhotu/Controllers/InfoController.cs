using BAW.Models;
using BAW.Services;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sokuuhotu.Controllers
{
    public class InfoController : Controller
    {





        private readonly StoreServices _service;




        public InfoController(StoreServices service)
        {
            _service = service;

        }


        public InfoController() : this(new StoreServices())
        {

        }




        // GET: Info
        public ActionResult About()
        {

            return View();
        }




        public ActionResult Terms()
        {

            return View();
        }


        public ActionResult Rules()
        {

            return View();
        }



        public ActionResult Contact()
        {

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Contact(Contact model)
        {

            if (ModelState.IsValid)
            {
                  await _service.AddQueryAsync(model);

                return RedirectToAction("Contact");


            }


            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> ReportAd(int proId, string msg)
        {
            //method in Store Controller

            await _service.AdReportAsync(proId, msg);

            return RedirectToAction("Index", "Store");
        }









    }
}