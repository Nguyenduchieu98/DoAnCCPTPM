using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileNewProject.Models
{
    public class ShoppingCart
    {
        DoChoiCongNgheEntities1 db = new DoChoiCongNgheEntities1();
        public int iMaDrone { get; set; }
        public string sTenDrone { get; set; }
        public string sAnhBia { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public double ThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }
        //Hàm tạo cho giỏ hàng
        public ShoppingCart(int MaDrone)
        {
            iMaDrone = MaDrone;
            SanPham sp = db.SanPhams.Single(n => n.MaSP == iMaDrone);
            sTenDrone = sp.TenSP;
            sAnhBia = sp.AnhBia;
            dDonGia = double.Parse(sp.GiaBan.ToString());
            iSoLuong = 1;
        }
    }
}