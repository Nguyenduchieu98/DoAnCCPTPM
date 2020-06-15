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





    }
}