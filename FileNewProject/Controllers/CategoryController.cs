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
	public ActionResult Color()
        {
            return PartialView(db.MauSacs.ToList());
        }
        public ViewResult DroneColor(int MaMauSac = 0)
        {
            //Kiểm tra chủ đề tồn tại hay không
            MauSac ms = db.MauSacs.SingleOrDefault(n => n.MaMS == MaMauSac);
            if (ms == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Truy xuất danh sách các quyển sách theo chủ đề
            List<SanPham> lstMauSac = db.SanPhams.Where(n => n.MaMS == MaMauSac).OrderBy(n => n.GiaBan).ToList();
            if (lstMauSac.Count == 0)
            {
                ViewBag.Sach = "Không có sách nào thuộc chủ đề này";
            }
            //Gán danh sách chủ để
            ViewBag.lstChuDe = db.MauSacs.ToList();
            return View(lstMauSac);
        }

        public ActionResult Place()
        {
            return PartialView(db.NoiSanXuats.ToList());
        }
        public ViewResult DronePlace(int MaNoiSanXuat = 0)
        {
            //Kiểm tra chủ đề tồn tại hay không
            NoiSanXuat nsx = db.NoiSanXuats.SingleOrDefault(n => n.MaNSX == MaNoiSanXuat);
            if (nsx == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Truy xuất danh sách các quyển sách theo chủ đề
            List<SanPham> lstNoiSX = db.SanPhams.Where(n => n.MaNSX == MaNoiSanXuat).OrderBy(n => n.GiaBan).ToList();
            if (lstNoiSX.Count == 0)
            {
                ViewBag.Sach = "Không có sách nào thuộc chủ đề này";
            }
            //Gán danh sách chủ để
            ViewBag.lstChuDe = db.NoiSanXuats.ToList();
            return View(lstNoiSX);
        }
    }
}