using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileNewProject.Models;
namespace FileNewProject.Controllers
{
    public class StatisticalController : Controller
    {
        DoChoiCongNgheEntities1 db = new DoChoiCongNgheEntities1();
        // GET: Statistical
        public ActionResult Index()
        {
            ViewBag.ThongKeDoanhThu = ThongKeDoanhThu();// thống kê doanh thu
            ViewBag.ThongKeDonHang = ThongKeDonHang(); // thống kê đơn hàng
            ViewBag.ThongKeThanhVien = ThongKeThanhVien(); // thống kê thành viên
            return View();
        }
        public decimal ThongKeDoanhThu()
        {
            decimal TongDoanhThu = db.ChiTietDonHangs.Sum(n => n.SoLuong * n.DonGia).Value;
            return TongDoanhThu; // thống kê tổng doanh thu
        }
        public double ThongKeDonHang()
        {
            // đếm đơn đặt hàng
            double slddh = db.DonHangs.Count();
            return slddh;

            //int ddh = 0;
            //var lstDDH = db.DonDatHangs;
            //if (lstDDH.Count() > 0)
            //{
            //    ddh = lstDDH.Count();
            //}
            //return ddh;
        }
        public double ThongKeThanhVien()
        {
            // đếm đơn đặt hàng
            double sltv = db.KhachHangs.Count();
            return sltv;
        }
    }
}