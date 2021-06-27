using BAW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAW.ViewModels
{
    // Add categories and View Categories
    public class CategoriesVM
    {
        // for creating
        public Category CreateCategory { get; set; }
        // for Views all categories
        public IEnumerable<Category> AllCategories { get; set; }
    }
}