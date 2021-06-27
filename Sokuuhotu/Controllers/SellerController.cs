using BAW.Services;
using Microsoft.AspNet.Identity;
using Sokuuhotu.Models;
using Sokuuhotu.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sokuuhotu.Controllers
{
    public class SellerController : Controller
    {


        private readonly StoreServices _service;




        public SellerController(StoreServices service)
        {
            _service = service;

        }


        public SellerController() : this(new StoreServices())
        {

        }



        // GET: Seller    >> here u means UserName
        public async Task<ActionResult> ViewProfile(string u)
        {
          

            var result = await _service.SellerProfileDetails(u);

            return View(result);

        }


        // GET: Seller  products

        public async Task<ActionResult> SellerItems(string userName)
        {

            var result = await _service.GetSellerProductsAsync(userName);

            return View(result);

        }

        [Authorize]
        public async Task<ActionResult> Dashboard()
        {
            var user = User.Identity.GetUserId();

            var result = await _service.DashboardDetails(user);


           

            return View(result);

        }
















    }
}