using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using prjIHealth.Models;
using prjIHealth.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace prjIHealth.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    public class ProductManageController : Controller
    {
        IHealthContext db = new IHealthContext();
        private IWebHostEnvironment _enviroment;
        public ProductManageController(IWebHostEnvironment p)
        {
            _enviroment = p;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProductList(int? page)
        {
            var pageNumber = page ?? 1;
            var proid = db.TProducts.OrderBy(n => n.FProductId).ToList();
            //var pro = db.TProducts.Include(p => p.FCategory).ToList();
            //IEnumerable<TProduct> datas = null; */
             var pro = (from p in db.TProducts
                        join c in db.TProductCategories
                        on p.FCategoryId equals c.FCategoryId
                        select new CProductViewModel()
                        {
                            FProductId = p.FProductId,
                            FProductName = p.FProductName,
                            FCategoryId = p.FCategoryId,
                            FCategoryName = p.FCategory,
                            FUnitprice = p.FUnitprice,
                            FDescription = p.FDescription,
                            FVisible = p.FVisible,
                            FCoverImage = p.FCoverImage
                        }).ToList();
            var onePageOfPro = pro.ToPagedList(pageNumber, 10);
            ViewBag.onePageOfPro = onePageOfPro;
            return View(onePageOfPro);
        }

        public IActionResult ProductEdit(int? id)
        {
            var pro = (from p in db.TProducts
                       where p.FProductId == id
                       join c in db.TProductCategories
                       on p.FCategoryId equals c.FCategoryId
                       select new CProductViewModel()
                       {
                           FProductId = p.FProductId,
                           FProductName = p.FProductName,
                           FCategoryId = p.FCategoryId,
                           FCategoryName = p.FCategory,
                           FUnitprice = p.FUnitprice,
                           FDescription = p.FDescription,
                           FVisible = p.FVisible,
                           FCoverImage = p.FCoverImage
                       }).ToList();
            if (pro == null)
            {
                return RedirectToAction("ProductList");
            }
            return View(pro);
        }
        [HttpPost]
        public IActionResult ProductEdit(CProductViewModel c)
        {
            IHealthContext db = new IHealthContext();
            TProduct pd = db.TProducts.FirstOrDefault(f => f.FProductId == c.FProductId);
            var pro = (from p in db.TProducts
                       where p.FProductId == c.FProductId
                       join t in db.TProductCategories
                       on p.FCategoryId equals t.FCategoryId
                       select new CProductViewModel()
                       {
                           FProductId = p.FProductId,
                           FProductName = p.FProductName,
                           FCategoryId = t.FCategoryId,
                           FCategoryName = p.FCategory,
                           FUnitprice = p.FUnitprice,
                           FDescription = p.FDescription,
                           FVisible = p.FVisible,
                           FCoverImage = p.FCoverImage
                       }).FirstOrDefault();
            if (pro != null)
            {
                if (c.photo != null)
                {
                    string pname = Guid.NewGuid().ToString() + ".jpg";
                    c.photo.CopyTo(new FileStream(
                        _enviroment.WebRootPath + "/img/product/" + pname, FileMode.Create));
                    pro.FCoverImage = pname;
                    TProductsImage PI = new TProductsImage();
                    PI.FProductId = c.FProductId;
                    PI.FImage = pname;
                    db.TProductsImages.Add(PI);
                }               
                pro.FProductName = c.FProductName;
                pro.FCategoryName = c.FCategoryName;
                pro.FCategoryId = c.FCategoryId;
                pro.FDescription = c.FDescription;
                pro.FUnitprice = c.FUnitprice;
                pro.FVisible = c.FVisible;
                pd.FProductName = pro.FProductName;
                pd.FCategoryId = pro.FCategoryId;
               // pd.FCategory.FCategoryId = pro.FCategoryId;
                pd.FDescription = pro.FDescription;
                pd.FUnitprice = pro.FUnitprice;
                pd.FVisible = pro.FVisible;
                pd.FCoverImage = pro.FCoverImage;
                db.SaveChanges();
            }

            return RedirectToAction("ProductList");
        }
        public IActionResult ProductCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ProductCreate(CProductViewModel p)
        {
            IHealthContext db = new IHealthContext();
            TProduct tp = new TProduct();
            tp.FProductId = p.FProductId;
            tp.FProductName = p.FProductName;
            tp.FCategoryId = p.FCategoryId;
            //tp.FCategory.FCategoryName = p.FCategoryName.FCategoryName;
            tp.FUnitprice = p.FUnitprice;
            tp.FDescription = p.FDescription;
            tp.FVisible = p.FVisible;
            tp.FCoverImage = p.FCoverImage;
            if (p.photo != null)
            {               
                string pic = Guid.NewGuid().ToString() + ".jpg";
                p.photo.CopyTo(new FileStream(_enviroment.WebRootPath +
                    "/img/product/" + pic, FileMode.Create));              
                tp.FCoverImage = pic;             
            }          
            db.TProducts.Add(tp);
            db.SaveChanges();
            return RedirectToAction("ProductList");
        }

        public IActionResult ProductImgList(int? id)
        {
            TProductsImage pm = db.TProductsImages.FirstOrDefault(p => p.FProductId == id);
            var pro = (from n in db.TProductsImages
                       where n.FProductId == id
                       join p in db.TProducts
                       on n.FProductId equals p.FProductId
                       select new CProductImageViewModel()
                       {
                           FImage = n.FImage,
                           FProductId = n.FProductId,
                           FProductImageId = n.FProductImageId,
                       }).ToList();

            if (pro == null)
            {
                return RedirectToAction("ProductList");
            }
            return View(pro);
        }
        [HttpPost]
        public IActionResult ProductImgList(CProductImageViewModel p)
        {
            IHealthContext db = new IHealthContext();
            var pro = (from n in db.TProductsImages
                       where n.FProductId == p.FProductId
                       join Tp in db.TProducts
                       on n.FProductId equals Tp.FProductId
                       select new CProductImageViewModel()
                       {
                           FImage = n.FImage,
                           FProductId = n.FProductId,
                           FProductImageId = n.FProductImageId,
                       }).ToList();
            //=====================================================
            TProductsImage ti = new TProductsImage();
            ti.FProductId = p.FProductId;
            ti.FImage = p.FImage;
            if (p.photo != null)
            {
                string pic = Guid.NewGuid().ToString() + ".jpg";
                p.photo.CopyTo(new FileStream(_enviroment.WebRootPath +
                    "/img/product/" + pic, FileMode.Create));
                ti.FImage = pic;
            }
            db.TProductsImages.Add(ti);
            db.SaveChanges();
            if (pro == null)
            {
                return RedirectToAction("ProductList");
            }
            return RedirectToAction("ProductImgList");
        }

        public IActionResult ProductImgDelete(int? id)
        {
            IHealthContext db = new IHealthContext();
            TProductsImage prod = db.TProductsImages.FirstOrDefault(t => t.FProductImageId == id);
            if (prod != null)
            {
                db.TProductsImages.Remove(prod);
                db.SaveChanges();
            }
            return RedirectToAction("ProductList");
        }
        //AJAX
        public IActionResult Categoryselect(int id)
        {
            IHealthContext db = new IHealthContext();
            var pro = (from p in db.TProducts
                       join c in db.TProductCategories
                       on p.FCategoryId equals c.FCategoryId
                       where p.FCategoryId == id
                       select new CProductViewModel()
                       {
                           FProductId = p.FProductId,
                           FProductName = p.FProductName,
                           FUnitprice = p.FUnitprice,
                           FDescription = p.FDescription,
                           FVisible = p.FVisible,
                           FCoverImage = p.FCoverImage,
                           FCategoryId = p.FCategoryId,
                           FCategoryName = p.FCategory
                       }).ToList();
       
            return Json(pro);
        }
    }
}
