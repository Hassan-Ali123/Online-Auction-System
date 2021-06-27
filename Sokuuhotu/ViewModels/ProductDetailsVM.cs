using BAW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAW.ViewModels
{
    public class ProductDetailsVM
    {
        public Product SingleProduct { get; set; }

        public IEnumerable<ProductImage> Images { get; set; }

        public Comment Comment { get; set; }

        public IEnumerable<Comment> Comments { get; set; }


    }
}