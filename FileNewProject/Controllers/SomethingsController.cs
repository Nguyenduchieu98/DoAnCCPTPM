using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileNewProject.Controllers
{
    public class SomethingsController : Controller
    {
  
        
            // GET: Somethings
            public ActionResult FAQ()
            {
                return View();
            }
            public ActionResult Gallery()
            {
                return View();
            }
            public ActionResult Services()
            {
                return View();
            }
            public ActionResult Use()
            {
                return View();
            }
        }
    }
