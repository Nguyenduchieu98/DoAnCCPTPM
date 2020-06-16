using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileNewProject.Models;
using PagedList;
using PagedList.Mvc;
namespace FileNewProject.Controllers
{
    public class AdminQLDONHANGController : Controller
    {
        // GET: AdminQLDONHANG

        DoChoiCongNgheEntities1 db = new DoChoiCongNgheEntities1();

        public ActionResult QLdonhang(int? page)
        {

            int pageNumber = (page ?? 1);
            int pageSize = 5;

            return View(db.ChiTietDonHangs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
        }

    }
}