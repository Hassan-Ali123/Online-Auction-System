using BAW.Models;
using BAW.Services;
using Microsoft.AspNet.Identity;
using Sokuuhotu.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace BAW.Controllers
{
    [Authorize]

    public class BidController : Controller
    {




        private readonly BidService _service;



        public BidController(BidService service)
        {
            _service = service;
        }


        public BidController() : this(new BidService())
        {

        }


        public async Task<ActionResult> BidReceived(int proId, [Form] int price)
        {
            if (price != 0)
            {

            var userId = User.Identity.GetUserId();

            var mesg = await _service.AcceptBidAsync(proId, userId, price);

            if (mesg != "")
            {
                TempData["Message"] = "only 2 bid";

                return RedirectToAction("ProductDetails", "Store", new { Id = proId });




            }

                return RedirectToAction("BidsHistory");


            }


            return RedirectToAction("ProductDetails","Store",new {id= proId });


        }



        public async Task<ActionResult> BidsHistory()
        {

            var userId = User.Identity.GetUserId();

            var history = await _service.BidsHistoryAsync(userId);

            return View(history);

        }






        public async Task<ActionResult> AllBidsByProduct(int proId)
        {


            //var userId = User.Identity.GetUserId();

            var bidrequest = await _service.GetProductBidsByIdAsync(proId);

            return View(bidrequest);

        }



        //Accept Bid Offer

        public async Task<ActionResult> AcceptBidOffer(int bidId)
        {

            await _service.BidsAcceptAsync(bidId);


            ViewBag.Message = "Bid Accepted";


            return RedirectToAction("Dashboard","Seller");

        }





        public async Task<ActionResult> AcceptedBidsBySeller()
        {
            var userId = User.Identity.GetUserId();
            var result = await _service.BidsAcceptBySellerAsync(userId);



            return View(result);

        }




        public async Task<ActionResult> BuyerBidsAccepted()
        {
            var userId = User.Identity.GetUserId();

            var result = await _service.BuyerBidsAcceptedAsync(userId);


          

            return View(result);

        }







        [ChildActionOnly]
        public  ActionResult Offers()
        {


            var user = User.Identity.GetUserId();

            var history = _service.BidsHistory(user);
            var bidsAceptbyseller = _service.BidsAcceptBySeller(user);
            var buyerBidsAccept = _service.BuyerBidsAccepted(user);

            var model = new UserDashboard()
            {
                SendingBidHistory = history.Count(),
                BidAcceptBySeller = bidsAceptbyseller.Count(),
                BuyerBidAcceptedByMe = buyerBidsAccept.Count()
            };

        
            return PartialView("_Offers",model);

        }








        public async Task<ActionResult> AddNote(string note, int id)
        {

             await _service.AddMsgAsync(note, id);
            return RedirectToAction("BidsHistory");
        }



      






    }
}


