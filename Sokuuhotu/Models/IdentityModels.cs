using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BAW.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;
using System;

namespace Sokuuhotu.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //Name :is our userName That is unique... 
        [Required]
        [Remote("NameExist", "Account")]
        public string Name { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Display]
        public string ProfileImageUrl { get; set; }

        public string Mobile { get; set; }

        public string AboutMe { get; set; }

        public bool AsCompany { get; set; }

        public bool AsUser { get; set; }


        public DateTime AccountCreatedDate { get; set; }

        public string YourWebAddress { get; set; }


        // navigation property

        public List<Product> Products { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<BidRequest> BidRequests { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Contact> ContactUs { get; set; }
        public DbSet<ReportAd> ReportAd { get; set; }
        public DbSet<PaymentReceival> Payments { get; set; }
        public DbSet<RegistrationModel> Registrations { get; set; }





    }
}