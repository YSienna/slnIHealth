using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using X.PagedList;
using Microsoft.AspNetCore.Hosting;
using prjIHealth.Models;
using Microsoft.AspNetCore.Http;
using prjiHealth.ViewModels;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace prjIHealth.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    public class NewsManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private IHealthContext _db;
        private IWebHostEnvironment _enviroment;
        public NewsManageController(IWebHostEnvironment n, IHealthContext context)
        {
            _enviroment = n;
            _db = context;
        }
        //專欄表單生成，冠宇是神
        public IActionResult List(CNewsViewModel vModel/*, int? page*/)
        {
            IHealthContext db = new IHealthContext();
            IEnumerable<TNews> datas = null;

            if (string.IsNullOrEmpty(vModel.txtKeyword))
            {
                datas = db.TNews.Include(t => t.FNewsCategory)
                    .Include(n => n.FMember)
                    .Select(t => t)
                    .OrderBy(t => t.FNewsId);
                //int datascount = 0;
                //datascount = datas.Count();

                //ViewBag.SearchResult = "您搜尋的資料共(" + datascount + ")";
                //.ToPagedList(page ?? 1, 5);
            }
            else
            {
                datas = db.TNews
                    .Include(t => t.FNewsCategory)
                    .Include(n => n.FMember)
                    .Where(t => t.FTitle.Contains(vModel.txtKeyword));
                //int datascount = 0;
                //datascount = datas.Count();
                //ViewBag.SearchResult = "您搜尋的資料共(" + datascount + ")";
                if (datas.Count() == 0)
                {
                    datas = db.TNews.Include(t => t.FNewsCategory)
                   .Include(n => n.FMember)
                   .Select(t => t)
                   .OrderBy(t => t.FNewsId);
                    //ViewBag.SearchResult = "沒有您所蒐尋的資料";
                }
                //.ToPagedList(page ?? 1, 5);
            }

            List<CNewsViewModel> news = new List<CNewsViewModel>();
            foreach (var n in datas)
            {
                CNewsViewModel newsViewModel = new CNewsViewModel(_db)
                {
                    _news = n,
                    //newsCategory = n.FNewsCategory,
                    //getMember = n.FMember
                };
                news.Add(newsViewModel);
            }
            //ViewBag.OnePageOfNews = datas;
            //var news = CNewsViewModel.List(datas.ToList());
            return View(news);
        }

        public IActionResult Search(string keyWord)
        {
            IHealthContext db = new IHealthContext();
            IEnumerable<TNews> tNews = null;

            if (!String.IsNullOrEmpty(keyWord))
                tNews = db.TNews.Where(c => c.FTitle.ToLower().Contains(keyWord.ToLower()));

            List<CNewsViewModel> vModel = null;
            //if (tNews.Count() != 0)
            //{
            vModel = new List<CNewsViewModel>();
            foreach (var n in tNews)
            {
                CNewsViewModel viewModel = new CNewsViewModel(db)
                {
                    FNewsId = n.FNewsId,
                    FTitle = n.FTitle,
                    FNewsDate = n.FNewsDate,
                    FContent = n.FContent,
                    FThumbnailPath = n.FThumbnailPath,
                    FNewsCategoryId = n.FNewsCategoryId,
                    FViews = n.FViews,
                    FVideoUrl = n.FVideoUrl,
                    FMemberId = n.FMemberId,
                    newsCategory = n.FNewsCategory,
                    getMember = n.FMember
                };
                vModel.Add(viewModel);
                //}
            }
            //IHealthContext db = new IHealthContext();

            //var selCateID = (from n in db.TNews
            //                 join c in db.TNewsCategories
            //                 on n.FNewsCategoryId equals c.FNewsCategoryId
            //                 where n.FTitle == keyWord
            //                 join m in db.TMembers
            //                 on n.FMemberId equals m.FMemberId
            //                 select new CNewsViewModel()
            //                 {
            //                     FNewsId = n.FNewsId,
            //                     FTitle = n.FTitle,
            //                     FNewsDate = n.FNewsDate,
            //                     FContent = n.FContent,
            //                     FThumbnailPath = n.FThumbnailPath,
            //                     FNewsCategoryId = n.FNewsCategoryId,
            //                     FViews = n.FViews,
            //                     FVideoUrl = n.FVideoUrl,
            //                     FMemberId = n.FMemberId,
            //                     newsCategory = n.FNewsCategory,
            //                     getMember = n.FMember
            //                 }).ToList();
            return Json(vModel);
        }

        //專欄後台詳細內容顯示
        public IActionResult Details(int? id)
        {
            IHealthContext db = new IHealthContext();
            TNews news = db.TNews.FirstOrDefault(t => t.FNewsId == id);
            if (news == null)
                return RedirectToAction("List");
            return View(news);
        }
        //專欄新增，利用viewmodel存入圖檔至根目錄
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CNewsViewModel vModel)
        {
            if (string.IsNullOrEmpty(vModel.FTitle))
            {
                ViewBag.AlertTitle = "請輸入專欄標題";
            }
            else if (string.IsNullOrEmpty(vModel.FContent))
            {
                ViewBag.AlertContent = "請輸入文章內容";
            }
            else if (vModel.FNewsCategoryId > 5 || vModel.FNewsCategoryId <= 0)
            {
                ViewBag.AlertCategory = "請輸入專欄標題";
            }
            else
            {
                IHealthContext db = new IHealthContext();
                TNews news = new TNews();
                news.FTitle = vModel.FTitle;
                news.FContent = vModel.FContent;
                news.FNewsDate = vModel.FNewsDate;
                news.FNewsCategoryId = vModel.FNewsCategoryId;
                news.FVideoUrl = vModel.FVideoUrl;
                news.FViews = vModel.FViews;
                news.FMemberId = vModel.FMemberId;

                if (vModel.photo != null)
                {
                    string nName = Guid.NewGuid().ToString() + ".jpg";
                    vModel.photo.CopyTo(new FileStream(
                        _enviroment.WebRootPath + "/img/blog/" + nName, FileMode.Create));
                    news.FThumbnailPath = nName;
                }
                db.Add(news);
                db.SaveChanges();
                ViewBag.Succes = "專欄已新增成功";
            }
            return RedirectToAction("List");
        }
        //專欄編輯，利用viewmodel帶過categoryname
        public IActionResult Edit(int? id)
        {
            IHealthContext db = new IHealthContext();
            TNews news = db.TNews.FirstOrDefault(t => t.FNewsId == id);
            if (news == null)
                return RedirectToAction("List");
            return View(news);
        }
        [HttpPost]
        public IActionResult Edit(CNewsViewModel n)
        {
            IHealthContext db = new IHealthContext();
            TNews news = db.TNews.FirstOrDefault(t => t.FNewsId == n.FNewsId);
            if (news != null)
            {
                if (string.IsNullOrEmpty(n.FTitle))
                {
                    ViewBag.AlertTitle = "請輸入專欄標題";
                }
                else if (string.IsNullOrEmpty(n.FContent))
                {
                    ViewBag.AlertContent = "請輸入文章內容";
                }
                else if (n.FNewsCategoryId > 5 || n.FNewsCategoryId <= 0)
                {
                    ViewBag.AlertCategory = "請輸入專欄標題";
                }
                else
                {
                    if (n.photo != null)
                    {
                        string nName = Guid.NewGuid().ToString() + ".jpg";
                        n.photo.CopyTo(new FileStream(
                            _enviroment.WebRootPath + "/img/blog/" + nName, FileMode.Create));
                        news.FThumbnailPath = nName;
                    }
                    news.FTitle = n.FTitle;
                    news.FNewsDate = n.FNewsDate;
                    news.FContent = n.FContent;
                    news.FNewsCategoryId = n.FNewsCategoryId;
                    news.FVideoUrl = n.FVideoUrl;
                }
            }
            db.SaveChanges();
            return RedirectToAction("List");
        }
        //專欄刪除
        public IActionResult Delete(int? id)
        {
            IHealthContext db = new IHealthContext();
            var comments = db.TNewsComments.Where(c => c.FNewsId == id);
            var news = db.TNews.FirstOrDefault(t => t.FNewsId == id);//
            //var news = db.TNews.Where(t => t.FNewsId == id).Include(c => c.TNewsComments).FirstOrDefault();
            if (comments != null)
            {
                foreach (var c in comments)
                {
                    db.TNewsComments.Remove(c);
                }
                db.SaveChanges();
                if (news != null)
                {
                    db.TNews.Remove(news);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }

        //選取類別api
        public IActionResult SelectCategoryIDAPI(int id)
        {
            IHealthContext db = new IHealthContext();

            var selCateID = (from n in db.TNews
                             join c in db.TNewsCategories
                             on n.FNewsCategoryId equals c.FNewsCategoryId
                             where n.FNewsCategoryId == id
                             join m in db.TMembers
                             on n.FMemberId equals m.FMemberId
                             select new CNewsViewModel()
                             {
                                 FNewsId = n.FNewsId,
                                 FTitle = n.FTitle,
                                 FNewsDate = n.FNewsDate,
                                 FContent = n.FContent,
                                 FThumbnailPath = n.FThumbnailPath,
                                 FNewsCategoryId = n.FNewsCategoryId,
                                 FViews = n.FViews,
                                 FVideoUrl = n.FVideoUrl,
                                 FMemberId = n.FMemberId,
                                 newsCategory = n.FNewsCategory,
                                 getMember = n.FMember
                             }).ToList();

            return Json(selCateID);
        }
        //重置類別選項api
        public IActionResult ResetList()
        {
            IHealthContext db = new IHealthContext();
            var clickReset = (from n in db.TNews
                              join c in db.TNewsCategories
                              on n.FNewsCategoryId equals c.FNewsCategoryId
                              join m in db.TMembers
                              on n.FMemberId equals m.FMemberId
                              select new CNewsViewModel()
                              {
                                  FNewsId = n.FNewsId,
                                  FTitle = n.FTitle,
                                  FNewsDate = n.FNewsDate,
                                  FContent = n.FContent,
                                  FThumbnailPath = n.FThumbnailPath,
                                  FNewsCategoryId = n.FNewsCategoryId,
                                  FViews = n.FViews,
                                  FVideoUrl = n.FVideoUrl,
                                  FMemberId = n.FMemberId,
                                  newsCategory = n.FNewsCategory,
                                  getMember = n.FMember
                              }).ToList();

            return Json(clickReset);
        }
    }
}
