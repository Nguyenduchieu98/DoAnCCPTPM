using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileNewProject.Models;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;

namespace FileNewProject.Controllers
{
    public class HomeController : Controller
    {
        DoChoiCongNgheEntities1 db = new DoChoiCongNgheEntities1();
        // GET: Home
        public ActionResult Index(int? page)
        {
            int pageSize = 12;
            //tạo biến số trang
            int pageNumber = (page ?? 1);
            return View(db.SanPhams.ToList().OrderByDescending(n => n.GiaBan).ToPagedList(pageNumber, pageSize));
        }
	public ActionResult ViewDetails(int MaDrone = 0)
        {
            SanPham sanpham = db.SanPhams.SingleOrDefault(n => n.MaSP == MaDrone);
            if(sanpham ==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }
        
	
    }
}