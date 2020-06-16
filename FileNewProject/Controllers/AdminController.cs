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
//Thêm mới 
        [HttpGet]
        public ActionResult CreateProduct()
        {
            //Đưa dữ liệu vào dropdownlist
            ViewBag.MaNSX = new SelectList(db.NoiSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            ViewBag.MaNhaSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNhaSX), "MaNhaSX", "TenNhaSX");
            ViewBag.MaMS = new SelectList(db.MauSacs.ToList().OrderBy(n => n.TenMau), "MaMS", "TenMau");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateProduct(SanPham sp, HttpPostedFileBase fileUpload, HttpPostedFileBase fileUpload1, HttpPostedFileBase fileUpload2, HttpPostedFileBase fileUpload3, HttpPostedFileBase fileUpload4)
        {


            //Đưa dữ liệu vào dropdownlist
            ViewBag.MaNSX = new SelectList(db.NoiSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            ViewBag.MaNhaSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNhaSX), "MaNhaSX", "TenNhaSX");
            ViewBag.MaMS = new SelectList(db.MauSacs.ToList().OrderBy(n => n.TenMau), "MaMS", "TenMau");
            //kiểm tra đường dẫn ảnh bìa
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Chọn hình ảnh";
                return View();
            }
            else
            if (fileUpload1 == null)
            {
                ViewBag.ThongBao = "Chọn hình ảnh";
                return View();
            }
            else
            if (fileUpload2 == null)
            {
                ViewBag.ThongBao = "Chọn hình ảnh";
                return View();
            }
            else
            if (fileUpload3 == null)
            {
                ViewBag.ThongBao = "Chọn hình ảnh";
                return View();
            }
            else
            if (fileUpload4 == null)
            {
                ViewBag.ThongBao = "Chọn hình ảnh";
                return View();
            }

            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                //Lưu tên file
                var fileName = Path.GetFileName(fileUpload.FileName);
                var fileName1 = Path.GetFileName(fileUpload1.FileName);
                var fileName2 = Path.GetFileName(fileUpload2.FileName);
                var fileName3 = Path.GetFileName(fileUpload3.FileName);
                var fileName4 = Path.GetFileName(fileUpload4.FileName);
                //Lưu đường dẫn của file
                var path = Path.Combine(Server.MapPath("~/HinhAnhSP/AnhBia"), fileName);
                var path1 = Path.Combine(Server.MapPath("~/HinhAnhSP/AnhSl"), fileName1);
                var path2 = Path.Combine(Server.MapPath("~/HinhAnhSP/AnhSli"), fileName2);
                var path3 = Path.Combine(Server.MapPath("~/HinhAnhSP/AnhSlid"), fileName3);
                var path4 = Path.Combine(Server.MapPath("~/HinhAnhSP/AnhSlide"), fileName4);
                //Kiểm tra hình ảnh đã tồn tại chưa
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    return View();
                }
                else
                if (System.IO.File.Exists(path1))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    return View();
                }
                else
                if (System.IO.File.Exists(path2))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    return View();
                }
                else
                if (System.IO.File.Exists(path3))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    return View();
                }
                else
                if (System.IO.File.Exists(path4))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    return View();
                }
                else
                {
                    fileUpload.SaveAs(path);
                    fileUpload1.SaveAs(path1);
                    fileUpload2.SaveAs(path2);
                    fileUpload3.SaveAs(path3);
                    fileUpload4.SaveAs(path4);
                }
                sp.AnhBia = fileUpload.FileName;
                sp.AnhSl = fileUpload1.FileName;
                sp.AnhSli = fileUpload2.FileName;
                sp.AnhSlid = fileUpload3.FileName;
                sp.AnhSlide = fileUpload4.FileName;
                db.SanPhams.Add(sp);
                db.SaveChanges();
            }
            return RedirectToAction("Admin","Admin");
        }

    }
}