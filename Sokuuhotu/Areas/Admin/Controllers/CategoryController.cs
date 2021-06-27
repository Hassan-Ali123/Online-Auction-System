using BAW.Models;
using BAW.ViewModels;
using Sokuuhotu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BAW.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Admin/Category // Add and View Categories in single page.
        public ActionResult MainCategories()
        {
            var vm = new CategoriesVM()
            {
                AllCategories = _db.Categories.ToList()
            };
            return View(vm);
        }



        [HttpPost]
        public ActionResult MainCategories(CategoriesVM model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    CategoryName = model.CreateCategory.CategoryName
                };
                _db.Categories.Add(category);
                _db.SaveChanges();

                return RedirectToAction("MainCategories");

            }

            return View(model);
        }


        // Delet: Admin/Delete category

        public ActionResult Delete(int id)
        {

           var result= _db.Categories.Find(id);
            _db.Categories.Remove(result);
            _db.SaveChanges();
            return RedirectToAction("MainCategories");
        }

    }
}