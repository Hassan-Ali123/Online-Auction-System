using BAW.Models;
using BAW.ViewModels;
using Sokuuhotu.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BAW.Services
{
    public class BidService
    {

        private readonly ApplicationDbContext _db;

        public BidService(ApplicationDbContext context)
        {
            _db = context;
        }

        public BidService() : this(new ApplicationDbContext())
        {

        }



        public async Task<string> AcceptBidAsync(int proId, string userId, int price)
        {



            var message = string.Empty;


            var bidProduct = await _db.BidRequests.Where(p => p.ProductId == proId && p.UserId == userId).SingleOrDefaultAsync();


            if (bidProduct ==null)
            {

                var bidRequest = new BidRequest
                {
                    ProductId = proId,
                    FirstPrice = price,
                    UserId = userId,
                    BidCount = 1,
                    BidDate = DateTime.Now,
                };
                _db.BidRequests.Add(bidRequest);
            }

            else if (bidProduct.BidCount == 1)
            {

                bidProduct.SecondPrice = price;
                bidProduct.BidDate = DateTime.Now;
                bidProduct.BidCount = 2;

                _db.Entry(bidProduct).State = EntityState.Modified;
            }



            else
            {
                message = "More than 2 bids";
            }
            await _db.SaveChangesAsync();


            return message;


        }




        public async Task<IEnumerable<BidRequest>> BidsHistoryAsync(string userId)
        {

            var model = await _db.BidRequests.Where(p => p.UserId == userId).OrderByDescending(p => p.BidDate).ToArrayAsync();
            //var bidVM = new BidsHistoryVM()
            //{
            //    //FirstTimeBid = firstTime,
            //    //SecondTimeBid = secondTime
            //};
            return model;

        }




        public async Task<IEnumerable<Product>> GetPostedProductsAsync(string userId)
        {
            return await _db.Products.Where(p => p.UserId == userId).OrderByDescending(p => p.StartDate).ToArrayAsync();

        }




        public async Task<BidsByProduct> GetProductBidsByIdAsync(int proId)
        {
            var bids = await _db.BidRequests.Include("User").Where(p => p.ProductId == proId).OrderByDescending(p=> p.BidDate).ToArrayAsync();

            //var firstTimebids =  bids.Where(p => p.BidTimes == 1).ToList();

            //var secondTimebids = bids.Where(p => p.BidTimes == 2).ToList();

            // Show mAx bid price of Product
            decimal maxBid = 0;
            try
            {
                var firstBid = bids.Max(p => p.FirstPrice);
                var secondBid = bids.Max(p => p.SecondPrice);
                if (firstBid > secondBid)
                {
                    maxBid = firstBid;
                }
                else
                    maxBid = secondBid;




            }
            catch (Exception e)
            {

            }


            var vm = new BidsByProduct()
            {

                BidsOfProduct = bids,
                MaxBid = maxBid
            };

            return vm;

        }




        //Accept Bid
        public async Task BidsAcceptAsync(int bidId)
        {

            // find Bid 
            var bid = await _db.BidRequests.Include("User").Where(p => p.BidRequestId == bidId).SingleOrDefaultAsync();

            // Modifies Accept to true

            if (bid != null)
            {

                bid.Accepted = true;

            }

            _db.Entry(bid).State = EntityState.Modified;
            await _db.SaveChangesAsync();



        }

        
        public async Task<IEnumerable<BidRequest>> BidsAcceptBySellerAsync(string userId)
        {
         return await  _db.BidRequests.Include(m=> m.Product.User).Where(x => x.UserId == userId && x.Accepted == true).OrderByDescending(p => p.BidDate).ToArrayAsync();

        }
        

        public async Task<IEnumerable<BidRequest>> BuyerBidsAcceptedAsync(string userId)
        {
            //var allProductsbySeller = await _db.Products.Where(u=>u.UserId ==userId && u.AuctionEnd == true).ToArrayAsync();
            var offerAccepted = await _db.BidRequests.Include(m=>m.User).Where(x => x.Product.UserId == userId && x.Accepted == true).OrderByDescending(p => p.BidDate).ToArrayAsync();


            return offerAccepted;


        }










        public async Task AddMsgAsync(string msg, int id)
        {
            var bid = _db.BidRequests.SingleOrDefault(x => x.BidRequestId == id);
            bid.Note = msg;
            _db.Entry(bid).State = EntityState.Modified;
            await _db.SaveChangesAsync();


        }



        // non async Method because of[child action only]



        public IEnumerable<BidRequest> BidsHistory(string userId)
        {

            var model = _db.BidRequests.Where(p => p.UserId == userId).ToArray();
            return model;
        }

        public IEnumerable<BidRequest> BidsAcceptBySeller(string userId)
        {
            return  _db.BidRequests.Include("Product").Where(x => x.UserId == userId && x.Accepted == true).ToArray();

        }


        public  IEnumerable<BidRequest> BuyerBidsAccepted(string userId)
        {
            //var allProductsbySeller = await _db.Products.Where(u=>u.UserId ==userId && u.AuctionEnd == true).ToArrayAsync();
            var offerAccepted =  _db.BidRequests.Include("User").Where(x => x.Product.UserId == userId && x.Accepted == true).ToArray();


            return offerAccepted;


        }

















    }
}
