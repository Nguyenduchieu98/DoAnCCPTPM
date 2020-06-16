using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileNewProject.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace FileNewProject.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        DoChoiCongNgheEntities1 db = new DoChoiCongNgheEntities1();
        public ActionResult Admin(int? page)
        {
            //if (Session["TaiKhoan"] == null)
            //{  
            //    return RedirectToAction("Login", "User");
            //}
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(db.SanPhams.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }

    }
}