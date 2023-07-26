using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjiHealth.Models;
using prjiHealth.ViewModels;
using prjIHealth.Controllers;
using prjIHealth.Models;
using prjIHealth.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiHealth.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly IHealthContext dblIHealth;
        public ShoppingController(IHealthContext db)
        {
            dblIHealth = db;
        }

        public IActionResult ShoppingCartList()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Shopped_Items))
            {
                string jsonCart = HttpContext.Session.GetString(CDictionary.SK_Shopped_Items);
                List<CShoppingCartItem> cart = JsonSerializer.Deserialize<List<CShoppingCartItem>>(jsonCart);
                return View(cart);
            }
            else
                return RedirectToAction("ShowShoppingMall");
        }

        public IActionResult CheckOut()
        {
            //第三方支付用變數
            string itemName = "";
            int totalAmount = 0;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Shopped_Items))
            {
                string jsonCart = HttpContext.Session.GetString(CDictionary.SK_Shopped_Items);
                List<CShoppingCartItem> cart = JsonSerializer.Deserialize<List<CShoppingCartItem>>(jsonCart);
                for (int i = 0; i < cart.Count; i++)
                {
                    if (cart[i].count == 0 || cart[i].count == -1)
                        cart.Remove(cart[i]);
                    itemName += cart[i].product.FProductName + " " + "X" + cart[i].count + "#";
                    totalAmount += (int)cart[i].price;
                }
                totalAmount = (totalAmount - (int)cart[0].discount);
                //第三方支付用變數
                string randomNumber = Guid.NewGuid().ToString();//產生編號亂數
                string merchantTradeNo = randomNumber.Substring(0, 24).Replace("-", "");//取訂單編號格式二十碼和去掉-符號
                ViewBag.MerchantTradeNo = merchantTradeNo;//輸出至前端
                string merchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");//現在時間格式
                ViewBag.TradeDate = merchantTradeDate;//輸出至前端
                string checkMacValue1 = $"HashKey=5294y06JbISpM5x9&ChoosePayment=Credit&ClientBackURL=" +
                $"{ Request.Scheme}://{Request.Host}/Shopping/TPPSessionToDB&CreditInstallment=&EncryptType=1&" +
                $"InstallmentAmount=&ItemName={itemName}&MerchantID=2000132&MerchantTradeDate={merchantTradeDate}&" +
                $"MerchantTradeNo={merchantTradeNo}&PaymentType=aio&Redeem=&ReturnURL={Request.Scheme}://{Request.Host}/Shopping/TPPSessionToDB" +
                $"&StoreID=&TotalAmount={totalAmount}&TradeDesc=建立信用卡測試訂單&" +
                $"HashIV=v77hoKGq4kWxNNIS";
                string checkMacValue2 = System.Web.HttpUtility.UrlEncode(checkMacValue1, Encoding.UTF8).ToLower();//轉為UTF-8的編碼UrlEncode完轉小寫
                                                                                                                  //string checkMacValue3 = checkMacValue2.ToLower();//轉為小寫

                using var hashCode = SHA256.Create(); //建立SHA256個體
                var hashingCode = hashCode.ComputeHash(Encoding.UTF8.GetBytes(checkMacValue2));//計算指定雜湊值

                string checkMacValue3 = Convert.ToHexString(hashingCode).ToUpper();//轉換為使用大寫十六進位字元編碼的相等字串表示法，轉為大寫
                                                                                   //string checkMacValue5 = checkMacValue4.ToUpper(); //轉為大寫最後一步
                ViewBag.CheckMacValue = checkMacValue3;//輸出至前端
                ViewBag.ItemName = itemName;//輸出至前端
                ViewBag.TotalAmout = totalAmount;//輸出至前端
                ViewBag.Url = $"{Request.Scheme}://{Request.Host}/Shopping/TPPSessionToDB";
                ViewBag.UrlBack = $"{ Request.Scheme}://{Request.Host}/Member/OrderList/1";
                return View(cart);
            }
            else
                return RedirectToAction("ShowShoppingMall");
        }
        [HttpPost]
        public IActionResult CheckOut(CShoppingCartItem vmodel)
        {
            int userID = TakeMemberID();
            if (ModelState.IsValid)
            {
                IHealthContext db = new IHealthContext();
                TOrder order = new TOrder();
                order.FPaymentCategoryId = vmodel.FPaymentCategoryId;
                order.FDate = vmodel.FDate;
                order.FMemberId = userID;
                order.FAddress = vmodel.FAddress;
                order.FContact = vmodel.FContact;
                order.FPhone = vmodel.FPhone;
                order.FRemarks = vmodel.FRemarks;
                order.FStatusNumber = vmodel.FStatusNumber;
                db.TOrders.Add(order);

                db.SaveChanges();

                //第一次寫入資料庫產生orderid後，開啟第二dbcontext處理orderdetail，取用session的list進行迴圈寫入
                IHealthContext dbod = new IHealthContext();
                TOrderDetail orderdetail = new TOrderDetail();
                string jsonCart = "";
                List<CShoppingCartItem> list = null;
                jsonCart = HttpContext.Session.GetString(CDictionary.SK_Shopped_Items);
                list = JsonSerializer.Deserialize<List<CShoppingCartItem>>(jsonCart);
                var orderid = (from p in dbod.TOrders
                               where p.FMemberId == userID
                               orderby p.FOrderId descending
                               select p.FOrderId).FirstOrDefault();
                for (int i = 0; i < list.Count; i++)
                {
                    //每次迴圈時識別欄位值會異動，故重新設為0
                    orderdetail.FOrderDetailsId = 0;
                    orderdetail.FOrderId = orderid;
                    orderdetail.FProductId = list[i].productId;
                    orderdetail.FQuantity = list[i].count;
                    orderdetail.FUnitprice = (int)list[i].price;
                    if (list[i].discountID == 0)
                    {
                        orderdetail.FDiscountId = 10;
                    }
                    else
                    {
                        orderdetail.FDiscountId = list[i].discountID;
                    }
                    dbod.TOrderDetails.Add(orderdetail);
                    dbod.SaveChanges();
                    HttpContext.Session.Remove(CDictionary.SK_Shopped_Items);
                    HttpContext.Session.Remove(CDictionary.SK_Third_Party_Payment);
                }
            }
            else
            {
                return RedirectToAction("CheckOut");
            }
            return RedirectToAction("OrderList", "Member", 1);
        }
        //將第三方金流用資料存入session
        public IActionResult ThirdPartyPaymentSession(TOrder vmodel)
        {
            int userID = TakeMemberID();
            string jsonCart = "";
            List<TOrder> list = null;

            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_Third_Party_Payment))
            {
                list = new List<TOrder>();
            }
            else
            {
                jsonCart = HttpContext.Session.GetString(CDictionary.SK_Third_Party_Payment);
                list = JsonSerializer.Deserialize<List<TOrder>>(jsonCart);
            }
            TOrder item = new TOrder()
            {
                FPaymentCategoryId = vmodel.FPaymentCategoryId,
                FDate = vmodel.FDate,
                FMemberId = userID,
                FAddress = vmodel.FAddress,
                FContact = vmodel.FContact,
                FPhone = vmodel.FPhone,
                FRemarks = vmodel.FRemarks,
                FStatusNumber = vmodel.FStatusNumber
            };
            list.Add(item);
            jsonCart = JsonSerializer.Serialize(list);
            HttpContext.Session.SetString(CDictionary.SK_Third_Party_Payment, jsonCart);
            //歐付寶目前未知原因不會自動導向存入DB的ACTION故手動改為在此導向，正常應等交易完成後歐付寶自動導向
            //return RedirectToAction("TPPSessionToDB");
            return Json("ok");
        }
        //交易完成後，將session中的第三方金流資料存入db
        public IActionResult TPPSessionToDB()
        {
            int userID = TakeMemberID();

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Third_Party_Payment))
            {
                string jsonTPP = HttpContext.Session.GetString(CDictionary.SK_Third_Party_Payment);
                List<TOrder> cart = JsonSerializer.Deserialize<List<TOrder>>(jsonTPP);
                IHealthContext db = new IHealthContext();
                if (cart[0].FAddress == null || cart[0].FContact == null || cart[0].FPhone == null)
                {
                    return RedirectToAction("CheckOut");
                }
                else
                {
                    TOrder order = new TOrder();
                    order.FPaymentCategoryId = cart[0].FPaymentCategoryId;
                    order.FDate = cart[0].FDate;
                    order.FMemberId = userID;
                    order.FAddress = cart[0].FAddress;
                    order.FContact = cart[0].FContact;
                    order.FPhone = cart[0].FPhone;
                    order.FRemarks = cart[0].FRemarks;
                    order.FStatusNumber = cart[0].FStatusNumber;
                    db.TOrders.Add(order);

                    db.SaveChanges();


                    //第一次寫入資料庫產生orderid後，開啟第二dbcontext處理orderdetail，取用session的list進行迴圈寫入
                    IHealthContext dbod = new IHealthContext();
                    TOrderDetail orderdetail = new TOrderDetail();
                    string jsonCart = "";
                    List<CShoppingCartItem> list = null;
                    jsonCart = HttpContext.Session.GetString(CDictionary.SK_Shopped_Items);
                    list = JsonSerializer.Deserialize<List<CShoppingCartItem>>(jsonCart);
                    var orderid = (from p in dbod.TOrders
                                   where p.FMemberId == userID
                                   orderby p.FOrderId descending
                                   select p.FOrderId).FirstOrDefault();
                    for (int i = 0; i < list.Count; i++)
                    {
                        //每次迴圈時識別欄位值會異動，故重新設為0
                        orderdetail.FOrderDetailsId = 0;
                        orderdetail.FOrderId = orderid;
                        orderdetail.FProductId = list[i].productId;
                        orderdetail.FQuantity = list[i].count;
                        orderdetail.FUnitprice = (int)list[i].price;
                        if (list[i].discountID == 0)
                        {
                            orderdetail.FDiscountId = 10;
                        }
                        else
                        {
                            orderdetail.FDiscountId = list[i].discountID;
                        }
                        dbod.TOrderDetails.Add(orderdetail);
                        dbod.SaveChanges();
                        HttpContext.Session.Remove(CDictionary.SK_Shopped_Items);
                        HttpContext.Session.Remove(CDictionary.SK_Third_Party_Payment);
                    }
                }
            }
            else
            {
                return RedirectToAction("CheckOut");
            }
            //return Json("交易完成");
            return RedirectToAction("OrderList", "Member", 1);
        }
        //商城主頁界面
        public IActionResult ShowShoppingMall()
        {
            return View();
        }

        public int TakeMemberID()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string loginSession = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                TMember loginUser = JsonSerializer.Deserialize<TMember>(loginSession);
                int userID = loginUser.FMemberId;
                return userID;
            }
            return 0;
        }

        [HttpPost]
        public IActionResult GetProduct(CShoppingFeatureViewModel vModel)
        {
            IEnumerable<TProduct> dataShoppingItems = null;
            //預設顥示商品
            dataShoppingItems = from t in dblIHealth.TProducts
                                    //where t.FUnitprice >= vModel.minPrice && t.FUnitprice <= vModel.maxPrice
                                select t;

            //以價格排序
            if (vModel.sort != null)
            {
                if (vModel.sort == "asc")
                {
                    dataShoppingItems = dataShoppingItems.OrderBy(t => t.FUnitprice);
                }
                else if (vModel.sort == "desc")
                {
                    dataShoppingItems = dataShoppingItems.OrderByDescending(t => t.FUnitprice);
                }
            }
            //以類別排序
            if (vModel.categoryID != null)
            {
                if (vModel.categoryID != 0)
                {
                    dataShoppingItems = dataShoppingItems.Where(t => t.FCategoryId == vModel.categoryID);
                }
            }
            //以關鍵字搜尋
            if (!string.IsNullOrEmpty(vModel.txtKeyword))
            {
                if (vModel.txtKeyword != "")
                {
                    dataShoppingItems = dataShoppingItems.Where(t => t.FProductName.ToLower().Contains(vModel.txtKeyword.ToLower()));
                }
            }
            return Json(dataShoppingItems);
        }

        //傳入產品、會員ID 把商品加入到資料庫當中
        public IActionResult AddToTrack(int? id)
        {
            int userID = TakeMemberID();
            var p = new CProductViewModel().ProductList.FirstOrDefault(t => t.FProductId == id);

            var q = (from a in dblIHealth.TTrackLists
                     where a.FMemberId == userID && a.FProductId == id
                     select a).Count();

            if (q != 0)
            {
                int trackNum = dblIHealth.TTrackLists.Where(t => t.FMemberId == userID).Select(t => t).Count();
                return Json(trackNum);
            }
            else
            {
                if (p != null)
                {
                    if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
                    {
                        TTrackList trackList = new TTrackList()
                        {
                            FMemberId = userID,
                            FProductId = Convert.ToInt32(id)
                        };
                        dblIHealth.TTrackLists.Add(trackList);
                        dblIHealth.SaveChanges();

                        Dictionary<string, int> trackCount = new Dictionary<string, int>();
                        int trackNum = dblIHealth.TTrackLists.Where(t => t.FMemberId == userID).Select(t => t).Count();
                        trackCount.Add("trackNum", trackNum);
                        return Json(trackNum);
                    }
                }
                return RedirectToAction("ShowShoppingMall");
            }
        }

        //產品明細界面
        public ActionResult ShowProductDetail(int? id)
        {
            TProduct prod = dblIHealth.TProducts.FirstOrDefault(t => t.FProductId == id);
            if (prod == null)
            {
                return RedirectToAction("ShowShoppingMall");
            }
            return View(prod);
        }

        [HttpPost]
        public ActionResult ShowProductDetail(CAddToCartViewModel vModel)
        {
            IHealthContext db = new IHealthContext();
            TProduct prod = db.TProducts.FirstOrDefault(t => t.FProductId == vModel.txtFid);
            if (prod == null)
            {
                return RedirectToAction("ShowShoppingMall");
            }
            string jsonCart = "";
            List<CShoppingCartItem> list = null;
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_Shopped_Items))
            {
                list = new List<CShoppingCartItem>();
            }
            else
            {
                jsonCart = HttpContext.Session.GetString(CDictionary.SK_Shopped_Items);
                list = JsonSerializer.Deserialize<List<CShoppingCartItem>>(jsonCart);
            }

            CShoppingCartItem item = new CShoppingCartItem()
            {
                count = vModel.txtCount,
                price = (decimal)prod.FUnitprice,
                productId = vModel.txtFid,
                product = prod
            };

            if (list.Count == 0)
            {
                list.Add(item);
            }
            else
            {
                bool sameproduct = false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].productId == item.productId)
                    {
                        list[i].count += item.count;
                        sameproduct = true;
                    }
                }
                if (!sameproduct)
                {
                    list.Add(item);
                }
            }
            jsonCart = JsonSerializer.Serialize(list);
            HttpContext.Session.SetString(CDictionary.SK_Shopped_Items, jsonCart);
            int cartNum = list.Count();
            return Json(cartNum);
        }

        //以ProductID搜對應的圖片顯示在ProductDetail(沒用到先放著)
        public ActionResult ShowProductImages(int? id)
        {
            var images = from p in dblIHealth.TProducts
                         join i in dblIHealth.TProductsImages
                         on p.FProductId equals i.FProductId
                         where p.FProductId == id
                         select i.FImage;
            return Json(images);
        }

        //顯示各類別前3名圖片(沒用到先放著)
        public ActionResult ShowTop3Product(int? id)
        {
            // var top3PopularProduct = null;
            ArrayList list = new ArrayList();
            var top3PopularProduct = (from od in dblIHealth.TOrderDetails.Include(od => od.FProduct).ThenInclude(p => p.FCategory).AsEnumerable()
                                      where od.FProduct.FCategoryId == id
                                      group od by new
                                      {
                                          od.FProductId,
                                          od.FProduct.FProductName,
                                          od.FProduct.FCoverImage,
                                          od.FProduct.FUnitprice
                                      }
                                      into g
                                      select new
                                      {
                                          Key = g.Key,
                                          Count = g.Sum(od => od.FQuantity),
                                          Photo = g.Key.FCoverImage,
                                          UnitPrice = g.Key.FUnitprice
                                      }).OrderByDescending(p => p.Count).Take(3).ToList();

            list.Add(top3PopularProduct);

            return Json(list);
        }

        //隨機推薦4項商品
        public ActionResult SuggestProduct(int? id)
        {
            ArrayList list = new ArrayList();
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            int count = dblIHealth.TProducts.Count();
            int num = 0;
            List<int> items = new List<int>();
            for (int i = 0; i < 4;)
            {
                num = rd.Next(1, count);

                if (items.Contains(num) || num == id)
                {
                    continue;
                }
                else
                {
                    TProduct rdProduct = (from t in dblIHealth.TProducts
                                          where t.FProductId == num
                                          select t).FirstOrDefault();
                    list.Add(rdProduct);
                    i++;
                    items.Add(num);
                }
            }
            return Json(list);
        }
        public IActionResult CheckDiscount(string code)
        {
            IHealthContext db = new IHealthContext();
            var discount = db.TDiscounts.Where(t => t.FDiscountCode == code).Select(t => t).Distinct();
            return Json(discount);
        }

        public ActionResult ShoppingCartSession(CAddToCartViewModel vModel)
        {
            IHealthContext db = new IHealthContext();
            TProduct prod = db.TProducts.FirstOrDefault(t => t.FProductId == vModel.txtFid);
            if (prod == null)
            {
                return RedirectToAction("ShowShoppingMall");
            }
            string jsonCart = "";
            List<CShoppingCartItem> list = null;
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_Shopped_Items))
            {
                list = new List<CShoppingCartItem>();
            }
            else
            {
                jsonCart = HttpContext.Session.GetString(CDictionary.SK_Shopped_Items);
                list = JsonSerializer.Deserialize<List<CShoppingCartItem>>(jsonCart);
            }
            CShoppingCartItem item = new CShoppingCartItem()
            {
                count = vModel.txtCount,
                price = (decimal)prod.FUnitprice,
                productId = vModel.txtFid,
                discount = vModel.discountValue,
                product = prod,
                discountID = vModel.discountID
            };

            if (list.Count == 0)
            {
                list.Add(item);
            }
            else
            {
                bool sameproduct = false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].productId == item.productId)
                    {
                        list[i].count = item.count;
                        sameproduct = true;
                    }
                    list[i].discount = item.discount;
                    list[i].discountID = item.discountID;
                }
                if (!sameproduct)
                {
                    list.Add(item);
                }
            }
            jsonCart = JsonSerializer.Serialize(list);
            HttpContext.Session.SetString(CDictionary.SK_Shopped_Items, jsonCart);
            return RedirectToAction("ShoppingCartList");
        }
        public ActionResult ShoppingCartDelete(CAddToCartViewModel vModel)
        {
            IHealthContext db = new IHealthContext();
            TProduct prod = db.TProducts.FirstOrDefault(t => t.FProductId == vModel.txtFid);
            if (prod == null)
            {
                return RedirectToAction("ShowShoppingMall");
            }
            string jsonCart = "";
            List<CShoppingCartItem> list = null;
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_Shopped_Items))
            {
                list = new List<CShoppingCartItem>();
            }
            else
            {
                jsonCart = HttpContext.Session.GetString(CDictionary.SK_Shopped_Items);
                list = JsonSerializer.Deserialize<List<CShoppingCartItem>>(jsonCart);
            }
            CShoppingCartItem item = new CShoppingCartItem()
            {
                count = vModel.txtCount,
                price = (decimal)prod.FUnitprice,
                productId = vModel.txtFid,
                discount = vModel.discountValue,
                product = prod
            };
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].productId == vModel.txtFid)
                    list.Remove(list[i]);
            }
            jsonCart = JsonSerializer.Serialize(list);
            HttpContext.Session.SetString(CDictionary.SK_Shopped_Items, jsonCart);
            return RedirectToAction("ShoppingCartList");
        }
        public ActionResult ShoppingCartCheckZero(CAddToCartViewModel vModel)
        {
            IHealthContext db = new IHealthContext();
            TProduct prod = db.TProducts.FirstOrDefault(t => t.FProductId == vModel.txtFid);
            if (prod == null)
            {
                return RedirectToAction("ShowShoppingMall");
            }
            string jsonCart = "";
            List<CShoppingCartItem> list = null;
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_Shopped_Items))
            {
                list = new List<CShoppingCartItem>();
            }
            else
            {
                jsonCart = HttpContext.Session.GetString(CDictionary.SK_Shopped_Items);
                list = JsonSerializer.Deserialize<List<CShoppingCartItem>>(jsonCart);
            }
            CShoppingCartItem item = new CShoppingCartItem()
            {
                count = vModel.txtCount,
                price = (decimal)prod.FUnitprice,
                productId = vModel.txtFid,
                discount = vModel.discountValue,
                product = prod
            };
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].count == 0 || list[i].count == -1)
                    list.Remove(list[i]);
            }
            jsonCart = JsonSerializer.Serialize(list);
            HttpContext.Session.SetString(CDictionary.SK_Shopped_Items, jsonCart);
            return RedirectToAction("CheckOut");
        }
        public ActionResult AttachDiscount(CAddToCartViewModel vModel)
        {
            IHealthContext db = new IHealthContext();
            TProduct prod = db.TProducts.FirstOrDefault(t => t.FProductId == vModel.txtFid);
            if (prod == null)
            {
                return RedirectToAction("ShowShoppingMall");
            }
            string jsonCart = "";
            List<CShoppingCartItem> list = null;
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_Shopped_Items))
            {
                list = new List<CShoppingCartItem>();
            }
            else
            {
                jsonCart = HttpContext.Session.GetString(CDictionary.SK_Shopped_Items);
                list = JsonSerializer.Deserialize<List<CShoppingCartItem>>(jsonCart);
            }
            CShoppingCartItem item = new CShoppingCartItem()
            {
                count = vModel.txtCount,
                price = (decimal)prod.FUnitprice,
                productId = vModel.txtFid,
                discount = vModel.discountValue,
                product = prod,
                discountID = vModel.discountID
            };
            for (int i = 0; i < list.Count; i++)
            {
                list[i].discount = item.discount;
                list[i].discountID = item.discountID;
            }
            jsonCart = JsonSerializer.Serialize(list);
            HttpContext.Session.SetString(CDictionary.SK_Shopped_Items, jsonCart);
            return RedirectToAction("ShoppingCartList");
        }
        public IActionResult ShowCartCount()
        {
            string jsonCart = "";
            List<CShoppingCartItem> list = null;
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_Shopped_Items))
            {
                list = new List<CShoppingCartItem>();
            }
            else
            {
                jsonCart = HttpContext.Session.GetString(CDictionary.SK_Shopped_Items);
                list = JsonSerializer.Deserialize<List<CShoppingCartItem>>(jsonCart);
            }
            int cartNum = list.Count();
            return Json(cartNum);

        }
        public IActionResult DEMO()
        {
            //快速新增五筆訂單改變前三名推薦商品
            for(int i=0; i<=5;i++)
            {
                IHealthContext db = new IHealthContext();
                TOrder order = new TOrder();
                order.FPaymentCategoryId = 2;
                order.FDate = DateTime.Now.ToString("yyyy/MM/dd").Replace("/","-");
                order.FMemberId = 9;
                order.FAddress = "台北市大安區復興南路";
                order.FContact = "管理者";
                order.FPhone = "0912345678";
                order.FRemarks = "DEMO";
                order.FStatusNumber = 70;
                db.TOrders.Add(order);

                db.SaveChanges();

                //第一次寫入資料庫產生orderid後，開啟第二dbcontext處理orderdetail
                IHealthContext dbod = new IHealthContext();
                TOrderDetail orderdetail = new TOrderDetail();
                var orderid = (from p in dbod.TOrders
                               where p.FMemberId == 9
                               orderby p.FOrderId descending
                               select p.FOrderId).FirstOrDefault();
                orderdetail.FOrderDetailsId = 0;
                orderdetail.FOrderId = orderid;
                orderdetail.FProductId = 1;
                orderdetail.FQuantity = 1;
                orderdetail.FUnitprice = 1999;
                orderdetail.FDiscountId = 10;

                dbod.TOrderDetails.Add(orderdetail);
                dbod.SaveChanges();
            }

            return RedirectToAction("ShowShoppingMall");
        }
    }
}
