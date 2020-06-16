using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileNewProject.Models;
using FileNewProject.Commom;
namespace FileNewProject.Controllers
{
    public class UserController : Controller
    {
        DoChoiCongNgheEntities1 db = new DoChoiCongNgheEntities1();
        // GET: User
        [HttpGet]
        public ActionResult Registration()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Registration(KhachHang kh)
        {
            //BAT LOI BO TRONG
            if (ModelState.IsValid)
            {
                //chen du liệu vào bảng khách hàng
                db.KhachHangs.Add(kh);
                //luu vao csdl
                db.SaveChanges();
                return RedirectToAction("Thank", "User");
            }
            return View();
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            string sTaiKhoan = f["txtTaiKhoan"].ToString();
            string sMatKhau = f["txtMatKhau"].ToString();


            if (String.IsNullOrEmpty(sTaiKhoan))
            {
                ViewData["Loi1"] = "Vui lòng nhập tên đăng nhập";
            }
            if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Loi2"] = "Vui lòng nhập Mật Khẩu";
            }
            else
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);

                if (kh != null)
                {

                    var userSession = new Login();
                    userSession.Username = kh.HoTen;
                    Session.Add(CommonConstants.LOGIN_SESSION, userSession);
                    ViewBag.Thongbao = "Đăng Nhập Thành Công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "Home");


                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }

            return View();
        }


        public ActionResult ProfileUser()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            return View(db.KhachHangs.ToList());
        }


        //Chỉnh sửa sản phẩm
        [HttpGet]
        public ActionResult EditProfile(int MaKhach)
        {
            //Lấy ra đối tượng sách theo mã 
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH == MaKhach);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }


            return View(kh);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditProfile(KhachHang kh, FormCollection f)
        {


            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(kh).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            //Đưa dữ liệu vào dropdownlist


            return RedirectToAction("Warning");

        }

        public ActionResult Warning()
        {
            return View();
        }
        public ActionResult Thank()
        {
            return View();
        }
    }
}