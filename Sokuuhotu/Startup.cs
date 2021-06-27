using Microsoft.Owin;
using Owin;
using Hangfire;
using System.Web;
using BAW.Services;
using Sokuuhotu.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Hangfire.Dashboard;

[assembly: OwinStartupAttribute(typeof(Sokuuhotu.Startup))]
namespace Sokuuhotu
{
    // Here i Will use hangfire Lib to Recurring task like
    //    update auction status by checking after evey one minute

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            ConfigureAuth(app);
            createRolesandUsers();

            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");

            var result = new DashboardOptions { AppPath = VirtualPathUtility.ToAbsolute("/Services/StoreService.cs") };



            app.UseHangfireDashboard("/AutofunctionDash", new DashboardOptions
            {
                Authorization = new[] { new HangFireAuthorizationFilter() }
            });
            app.UseHangfireServer();



        }

        // Authentication For using HangFire DashBoared
        public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                // In case you need an OWIN context, use the next line, `OwinContext` class
                // is the part of the `Microsoft.Owin` package.
                var owinContext = new OwinContext(context.GetOwinEnvironment());

                // Allow all authenticated users to see the Dashboard (potentially dangerous).
                return owinContext.Authentication.User.Identity.IsAuthenticated;
            }
        }












        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


         // In Startup iam creating first Admin Role and creating a default Admin User    
        //    if (!roleManager.RoleExists("Admin"))
        //    {

        //        first we create Admin rool
        //       var role = new IdentityRole();
        //        role.Name = "Admin";
        //        roleManager.Create(role);

        //        Here we create a Admin super user who will maintain the website


        //        var result1 = UserManager.AddToRole("1974e151-b6a4-416f-ad27-9c3bf7539277", "Admin");

        //    }
        //}

        // creating Creating Manager role    
        //if (!roleManager.RoleExists("Manager"))
        //{
        //    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
        //    role.Name = "Manager";
        //    roleManager.Create(role);

        //}

        // creating Creating Employee role    
        //if (!roleManager.RoleExists("Employee"))
        //{
        //    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
        //    role.Name = "Employee";
        //    roleManager.Create(role);

        //}
    }

    }
}


 


