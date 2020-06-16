using FileNewProject.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileNewProject.Controllers
{
    public class TimKiemMoiController : Controller
    {
        // GET: TimKiemMoi
        DoChoiCongNgheEntities1 db = new DoChoiCongNgheEntities1();
        [HttpGet]
        public ActionResult KetQuaTimkiem()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KetQuaTimkiem(FormCollection f, int? page)
        {


            var sGia = decimal.Parse(f["Gia"].ToString());
            //decimal sGia = Convert.ToDecimal(f["GiaBan"].ToString());
            //int sGia = Int32.Parse(f["Gia"].ToString());
            var sMausac = f["txtMausac"].ToString();
            var sXuatsu = f["txtXuatxu"].ToString();
            List<SanPham> lstKQTK = db.SanPhams.Where(n => n.Mau.Contains(sMausac) && n.GiaBan >= sGia && n.XuatXu.Contains(sXuatsu)).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 5;




            if (String.IsNullOrEmpty(sXuatsu) && String.IsNullOrEmpty(sXuatsu) && sGia == 0)
            {
                ViewBag.ThongBao = "Bạn phải chọn  đủ điều kiện";

            }
            else
           if (String.IsNullOrEmpty(sMausac))
            {
                ViewBag.ThongBao = "Bạn phải chọn màu";
            }

            else
            if (sGia == 0)
            {
                ViewBag.ThongBao = "Bạn phải chọn giá.";

            }
            else
            if (String.IsNullOrEmpty(sXuatsu))
            {
                ViewBag.ThongBao = "Bạn phải chọn màu";
            }

            else
       if (lstKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sản phẩm nào nào";
            }
            else
            {
                ViewBag.ThongBao = "Ðã tìm thấy " + " " + lstKQTK.Count + " " + " kết quả";
            }
            return View(lstKQTK.OrderBy(n => n.GiaBan).ToPagedList(pageNumber, pageSize));
        }
    }
}