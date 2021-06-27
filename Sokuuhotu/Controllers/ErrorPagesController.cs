using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sokuuhotu.Controllers
{
    public class ErrorPagesController : Controller
    {
        // GET: ErrorPages
        public ActionResult PageNotFound()
        {
            return View();
        }
        //public ActionResult GeneralError()
        //{
        //    return View();
        //}
    }
}