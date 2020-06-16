using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;
using FileNewProject.Models;
namespace FileNewProject.Controllers
{
    public class ShoppingCartController : Controller
    {
        DoChoiCongNgheEntities1 db = new DoChoiCongNgheEntities1();
        private PayPal.Api.Payment payment;
        // GET: ShoppingCart
        #region Giỏ hàng
        public List<ShoppingCart> TakeShoppingCart()
        {
            List<ShoppingCart> lstShoppingCart = Session["ShoppingCart"] as List<ShoppingCart>;
            if (lstShoppingCart == null)
            {
                //Nếu giỏ hàng chưa tồn tại thì mình tiến hành khởi tao list giỏ hàng (sessionGioHang)
                lstShoppingCart = new List<ShoppingCart>();
                Session["ShoppingCart"] = lstShoppingCart;
            }
            return lstShoppingCart;
        }
        //Thêm giỏ hàng
        public ActionResult InsertShoppingCart(int iMaDrone, string strURL)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == iMaDrone);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy ra session giỏ hàng
            List<ShoppingCart> lstShoppingCart = TakeShoppingCart();
            //Kiểm tra sách này đã tồn tại trong session[giohang] chưa
            ShoppingCart sanpham = lstShoppingCart.Find(n => n.iMaDrone == iMaDrone);
            if (sanpham == null)
            {
                sanpham = new ShoppingCart(iMaDrone);
                //Add sản phẩm mới thêm vào list
                lstShoppingCart.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }
        //Cập nhật giỏ hàng 
        public ActionResult UpdateShoppingCart(int iMaSP, FormCollection f)
        {
            //Kiểm tra masp
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == iMaSP);
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<ShoppingCart> lstShoppingCart = TakeShoppingCart();
            //Kiểm tra sp có tồn tại trong session["GioHang"]
            ShoppingCart sanpham = lstShoppingCart.SingleOrDefault(n => n.iMaDrone == iMaSP);
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(f["txtSoLuong"].ToString());

            }
            return RedirectToAction("ShoppingCart");
        }
        //Xóa giỏ hàng
        public ActionResult DeleteShoppingCart(int iMaSP)
        {
            //Kiểm tra masp
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == iMaSP);
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<ShoppingCart> lstShoppingCart = TakeShoppingCart();
            ShoppingCart sanpham = lstShoppingCart.SingleOrDefault(n => n.iMaDrone == iMaSP);
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                lstShoppingCart.RemoveAll(n => n.iMaDrone == iMaSP);

            }
            if (lstShoppingCart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("ShoppingCart");
        }
        //Xây dựng trang giỏ hàng
        public ActionResult ShoppingCart()
        {
            if (Session["ShoppingCart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<ShoppingCart> lstShoppingCart = TakeShoppingCart();
            return View(lstShoppingCart);
        }
        //Tính tổng số lượng và tổng tiền
        //Tính tổng số lượng
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<ShoppingCart> lstGioHang = Session["ShoppingCart"] as List<ShoppingCart>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        //Tính tổng thành tiền
        private double TongTien()
        {
            double dTongTien = 0;
            List<ShoppingCart> lstGioHang = Session["ShoppingCart"] as List<ShoppingCart>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.ThanhTien);
            }
            return dTongTien;
        }

        //tạo partial giỏ hàng
        public ActionResult ShoppingCartPartial()
        {

            if (TongSoLuong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.SumWishlist = SumWishlist();
            return PartialView();
        }
        //Xây dựng 1 view cho người dùng chỉnh sửa giỏ hàng
        public ActionResult EditShoppingCart()
        {
            if (Session["ShoppingCart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<ShoppingCart> lstGioHang = TakeShoppingCart();
            return View(lstGioHang);

        }

        public ActionResult Checkout()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            if (Session["ShoppingCart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<ShoppingCart> lstShoppingCart = TakeShoppingCart();
            return View(lstShoppingCart);
        }

        public ActionResult Notification()
        {
            return View();
        }
        #endregion

        #region Đặt hàng
        //Xây dựng chức năng đặt hàng
        [HttpPost]
        public ActionResult Order()
        {
            //Kiểm tra đăng đăng nhập
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            //Kiểm tra giỏ hàng
            if (Session["ShoppingCart"] == null)
            {
                RedirectToAction("Index", "Home");
            }
            //Thêm đơn hàng
            DonHang ddh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<ShoppingCart> gh = TakeShoppingCart();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            db.DonHangs.Add(ddh);
            db.SaveChanges();
            //Thêm chi tiết đơn hàng
            foreach (var item in gh)
            {
                ChiTietDonHang ctDH = new ChiTietDonHang();
                ctDH.MaDonHang = ddh.MaDonHang;
                ctDH.MaSP = item.iMaDrone;
                ctDH.SoLuong = item.iSoLuong;
                ctDH.DonGia = (decimal)item.dDonGia;
                db.ChiTietDonHangs.Add(ctDH);
            }
            db.SaveChanges();
            return RedirectToAction("Notification", "ShoppingCart");
        }
        #endregion


        public List<Wishlist> TakeWishlist()
        {
            List<Wishlist> lstWishlist = Session["Wishlist"] as List<Wishlist>;
            if (lstWishlist == null)
            {
                //Nếu giỏ hàng chưa tồn tại thì mình tiến hành khởi tao list giỏ hàng (sessionGioHang)
                lstWishlist = new List<Wishlist>();
                Session["Wishlist"] = lstWishlist;
            }
            return lstWishlist;
        }

        private int SumWishlist()
        {
            int iSumWishlist = 0;
            List<Wishlist> lstWishlist = Session["Wishlist"] as List<Wishlist>;
            if (lstWishlist != null)
            {
                iSumWishlist = lstWishlist.Sum(n => n.iSoLuong);
            }
            return iSumWishlist;
        }
        public ActionResult Wishlist()
        {
            if (Session["Wishlist"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Wishlist> lstWishlist = TakeWishlist();
            return View(lstWishlist);
        }

        public ActionResult InsertWishlist(int iMaDrone, string strURL)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == iMaDrone);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy ra session giỏ hàng
            List<Wishlist> lstWishlist = TakeWishlist();
            //Kiểm tra sách này đã tồn tại trong session[giohang] chưa
            Wishlist sanpham = lstWishlist.Find(n => n.iMaDrone == iMaDrone);
            if (sanpham == null)
            {
                sanpham = new Wishlist(iMaDrone);
                //Add sản phẩm mới thêm vào list
                lstWishlist.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }

        public ActionResult DeleteWishlist(int iMaSP)
        {
            //Kiểm tra masp
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == iMaSP);
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<Wishlist> lstWishlist = TakeWishlist();
            Wishlist sanpham = lstWishlist.SingleOrDefault(n => n.iMaDrone == iMaSP);
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                lstWishlist.RemoveAll(n => n.iMaDrone == iMaSP);

            }
            if (lstWishlist.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Wishlist");
        }









        public ActionResult PaymentWithPaypal()
        {

            //getting the apiContext as earlier 
            APIContext apiContext = Configuration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class 

                    // Creating a payment 
                    // baseURL is the url on which paypal sendsback the data. 
                    // So we have provided URL of this controller only 
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/ShoppingCart/PaymentWithPayPal?";

                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution


                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url 
                    //on which payer is redirected for paypal account payment 

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function      call


                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = string.Empty;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {


                            paypalRedirectUrl = lnk.href;
                        }
                    }


                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {


                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error" + ex.Message);
                return View("FailureView");
            }

            return View("SuccessView");
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            var itemList = new ItemList() { items = new List<Item>() };
            List<ShoppingCart> listCarts = Session["ShoppingCart"] as List<ShoppingCart>;

            foreach (var cart in listCarts)
            {
                itemList.items.Add(new Item()
                {
                    name = cart.sTenDrone,
                    currency = "USD",
                    price = cart.dDonGia.ToString(),
                    quantity = cart.iSoLuong.ToString(),
                    sku = "sku"
                });
            }
            var payer = new Payer() { payment_method = "paypal" };
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            var details = new Details()
            {
                tax = "1",
                shipping = "2",
                subtotal = listCarts.Sum(x => x.iSoLuong * x.dDonGia).ToString()
            };
            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(),
                details = details
            };
            var transactionList = new List<Transaction>();
            //Tất cả thông tin thanh toán cần đưa vào transaction
            transactionList.Add(new Transaction()
            {
                description = "Shop Testing",
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = itemList
            });
            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return this.payment.Create(apiContext);
        }
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }
    }
}