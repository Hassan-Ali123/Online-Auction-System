using BAW.Models;
using Sokuuhotu.Models;
using BAW.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Data.SqlClient;
using Hangfire;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace BAW.Services
{
    public class StoreServices
    {


        private readonly ApplicationDbContext _db;

        public StoreServices(ApplicationDbContext context)
        {
            _db = context;



        }

        public StoreServices() : this(new ApplicationDbContext())
        {
            
        }




        public async Task<IEnumerable<Category>> CategoriesAsync()
        {

            AutocallFunction();
            return await _db.Categories.OrderBy(x=>x.CategoryName).ToArrayAsync();

        }



        public async Task AddProductAsync(Product model, List<HttpPostedFileBase> files, HttpPostedFileBase mainImage, string userId, int closeDate, DateTime clientDate,int clientOffset)
        {
            // <Main Image Upload> Code:


            var image = ImageSaving(mainImage);

          

            
            
          


            // initializing Product table entry
            var product = new Product
            {
                CategoryId = model.CategoryId,
                ProductName = model.ProductName,
                ProductDescription = model.ProductDescription,
                StartDate = clientDate,
                EndDate = clientDate.AddDays(closeDate),
                SelectedArea = null,
                City = model.City,
                MainImageUrl = "~/img/" + image,
                UserId = userId,
                AuctionEndDate = clientDate,
                UserTimeOffset = clientOffset // offset of every user
            };



            _db.Products.Add(product);

            await _db.SaveChangesAsync();


            // Code : Multiple Images added of related ProductId 

            foreach (var item in files) // listing eah image
            {
                if (item != null)
                {

                    if (item.ContentLength > 0) // cheking image length 
                    {

                        var nameOfFile = ImageSaving(item);

                        var images = new ProductImage
                        {
                            ProductId = product.ProductId,
                            ImageUrl = "~/img/" + nameOfFile

                        };
                        _db.ProductImages.Add(images);


                    }
                }
            }
            await _db.SaveChangesAsync();

        }







        public async Task<IEnumerable<Product>> ProductsAsync()
        {
            return await _db.Products.Where(p=>p.AuctionEnd==false).OrderByDescending(x=>x.ProductId).ToArrayAsync();
        }


        public async Task<ProductDetailsVM> ProductByIdAsync(int proId)
        {
            

           Product product = await _db.Products.Include("User").SingleOrDefaultAsync(p => p.ProductId == proId);

            var images = await _db.ProductImages.Include("Product").Where(p => p.ProductId == proId).ToArrayAsync();

            var comments = await _db.Comments.Include("User").Where(p => p.ProductId == proId).OrderByDescending(x=>x.CommentId).ToArrayAsync();

            var vm = new ProductDetailsVM
            {
                SingleProduct = product,
                Images = images,
                Comments= comments
            };

            return vm;
        }


        public async Task<IEnumerable<Product>> ProductsByCategoryIdAsync(int categoryId)
        {
            return await _db.Products.Include("Category").Where(p => p.CategoryId == categoryId && p.AuctionEnd == false)
                .ToArrayAsync();
        }




        public async Task<IEnumerable<Product>> GetPostedProductsAsync(string userId)
        {
            return await _db.Products.Include("Bids").Where(p => p.UserId == userId).OrderByDescending(x=>x.StartDate).ToArrayAsync();
        }

        

        public async Task<IEnumerable<Product>> GetProductsByStatusAsync(bool auctionEnd, string userid)
        {
            return await _db.Products.Include("Bids").Where(p =>p.UserId==userid && p.AuctionEnd == auctionEnd ).OrderByDescending(x => x.StartDate).ToArrayAsync();
        }




        // Edit function

        public async Task ProductEditAsync(Product model, int closeinDays)
        {

            model.EndDate = model.EndDate.AddDays(closeinDays);

            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();


        }

        // Product/ Ads Delete

        public async Task ItemDelete(int id)
        {
            var result = _db.Products.Find(id);
            _db.Products.Remove(result);
            await _db.SaveChangesAsync();

        }




        // ********Product Comments********


        public async Task AddCommentsAsync(Comment model, string userId,int proId)
        {

            var saveComment = new Comment
            {
                Message = model.Message,
                ProductId = proId,
                Date = DateTime.Now,
                UserId = userId
            };

            _db.Comments.Add(saveComment);
           await _db.SaveChangesAsync();

        }

       
        
        //*********************Add Immoral Ad report***************

        public async Task AdReportAsync(int ProId, string msg)
        {

            var model = new ReportAd
            {
                ProductId = ProId,
                ReportMsg = msg,
                Date = DateTime.Now,
                IsRead = false
            };
            _db.ReportAd.Add(model);
           await _db.SaveChangesAsync();


        }


        //*****************Dashboard***************
        // With userId or userName( for Viewrs)


        // By Id
        public async Task<ApplicationUser> DashboardDetails(string userId)
        {
           

            var sellerHistory = await _db.Users.Include(x => x.Products).SingleOrDefaultAsync(u=> u.Id == userId);
            return sellerHistory;

        }
        // By UserName

        public async Task<ApplicationUser> SellerProfileDetails(string userName)
        {


            var sellerProfile = await _db.Users.Include(x => x.Products).SingleOrDefaultAsync(u => u.Name == userName);
            return sellerProfile;

        }

        //Seller Products

        public async Task<IEnumerable<Product>> GetSellerProductsAsync(string userName)
        {
            return await _db.Products.Where(p => p.User.Name == userName && p.AuctionEnd ==false).OrderByDescending(x => x.StartDate).ToArrayAsync();
        }




        //*************** Add Query Contact page ********

        


         public async Task AddQueryAsync(Contact model)
        {

            var query = new Contact
            {
                Name = model.Name,
                Email = model.Email,
                Message = model.Message,
                PostedDate = DateTime.Now,
                IsRead = false
            };

            _db.ContactUs.Add(query);

            await _db.SaveChangesAsync();

        }




        // ********Searching********


        public async Task<IEnumerable<Product>> SearchAsync(string search)
        {
            return await _db.Products.Include("Category").Where(p => p.AuctionEnd==false && p.ProductName.Contains(search.ToLower())).OrderByDescending(x => x.StartDate)
                .ToArrayAsync();
        }


        public async Task<IEnumerable<Product>> AdvanceSearchAsync(string query, string area, int? categoryId)
        {


            var model =  _db.Products.Include("Category");

            IEnumerable<Product> result=null;

            if (!String.IsNullOrWhiteSpace(query) && !String.IsNullOrEmpty(area) && categoryId !=null)
            {
                result = await model.Where(p => p.AuctionEnd==false &&
                p.CategoryId == categoryId && 
                p.SelectedArea == area &&
                p.ProductName.Contains(query.ToLower())).ToArrayAsync();
                goto done ;
            }

            if (!String.IsNullOrWhiteSpace(query) && !String.IsNullOrEmpty(area) )
            {
                result = await model.Where(p => p.AuctionEnd == false &&
                p.SelectedArea == area
                && p.ProductName.Contains(query.ToLower())).ToArrayAsync();
                goto done;

            }

            if (!String.IsNullOrWhiteSpace(query) && categoryId != null)
            {
                result = await model.Where(p => p.AuctionEnd == false
                && p.CategoryId == categoryId && 
                p.ProductName.Contains(query.ToLower())).ToArrayAsync();
                goto done;

            }

            if (!String.IsNullOrEmpty(area) && categoryId != null)
            {
                result = await model.Where(p => p.AuctionEnd == false 
                && p.CategoryId == categoryId &&
                p.SelectedArea == area).ToArrayAsync();
                goto done;

            }

            if (categoryId != null)
            {
                result = await model.Where(p => p.AuctionEnd == false && p.CategoryId == categoryId).ToArrayAsync();
                goto done;

            }


            if (!String.IsNullOrEmpty(area))
            {
                result = await model.Where(p => p.AuctionEnd == false && p.SelectedArea == area).ToArrayAsync();
                goto done;

            }


            if (!String.IsNullOrWhiteSpace(query))
            {
                result = await SearchAsync(query);
                goto done;

            }


            // if all empty
            if (String.IsNullOrWhiteSpace(query) && String.IsNullOrEmpty(area) && categoryId == null)
            {
                result = await model.Where(p=> p.AuctionEnd == false).ToArrayAsync();
                goto done;

            }



        done:   return result;


        }


        

        //Find Product by ID /For Edit Action
        public Product FindProduct(int? productId)
        {
            return _db.Products.Find(productId);
        }

        //****************** method for image saving in folder**************8

        private string ImageSaving(HttpPostedFileBase item)
        {

                var path = "";
            //var filename = Guid.NewGuid().ToString() + Path.GetFileName(item.FileName);
            var filename = Guid.NewGuid().ToString() + Path.GetFileName(item.FileName);
            var extension = Path.GetExtension(filename).ToLower();
            if (extension == ".jpg" || extension == ".png" || extension == ".jpeg") //checkinh extension
            {
              
                path = HostingEnvironment.MapPath(Path.Combine("~/img/", filename));


                Stream strm = item.InputStream;
                var targetFile = path;

                ReduceImageSize(0.5, strm, targetFile);



                return filename;

                //item.SaveAs(path);

            }

            return null;





        }




        private void ReduceImageSize(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight );
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }



     


        ////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////
        ///////////Bacground Jobs With hangFire///////////////





        public void AutocallFunction()
        {


            RecurringJob.AddOrUpdate<StoreServices>("ID_Status", x => x.Status(), Cron.Minutely);


        }



        //hit after every minute
        public async Task Status()
        {
            var model = await _db.Products.Where(p => p.AuctionEnd == false).ToListAsync();

            
            //TimeZone zone = TimeZone.CurrentTimeZone;

            //DateTime local = zone.ToLocalTime(DateTime.UtcNow);

            //var ddate = local.AddMinutes(60);


            foreach (var item in model)
            {
                //DateTime.UtcNow.AddHours(3)
               var date = DateTime.UtcNow.AddMinutes(item.UserTimeOffset);
                if (item.EndDate <= date)
                {
                    item.AuctionEnd = true;
                    item.AuctionEndDate = date;
                    _db.Entry(item).State = EntityState.Modified;
                }

            }

          await  _db.SaveChangesAsync();


        }






















    }
}