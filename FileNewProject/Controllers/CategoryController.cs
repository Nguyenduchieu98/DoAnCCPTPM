using FileNewProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileNewProject.Controllers
{
    public class CategoryController : Controller
    {
        DoChoiCongNgheEntities1 db = new DoChoiCongNgheEntities1();
        // GET: Category
        public ActionResult Brand()
        {
            return PartialView(db.NhaSanXuats.ToList());
        }
        public ViewResult DroneBrand(int MaSX = 0)
        {
            //Kiểm tra chủ đề tồn tại hay không
            NhaSanXuat cd = db.NhaSanXuats.SingleOrDefault(n => n.MaNhaSX == MaSX);
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Truy xuất danh sách các quyển sách theo chủ đề
            List<SanPham> lstSanPham = db.SanPhams.Where(n => n.MaNhaSX == MaSX).OrderBy(n => n.GiaBan).ToList();
            if (lstSanPham.Count == 0)
            {
                ViewBag.Sach = "Không có sách nào thuộc chủ đề này";
            }
            //Gán danh sách chủ để
            ViewBag.lstChuDe = db.NhaSanXuats.ToList();
            return View(lstSanPham);
        }
    }
}