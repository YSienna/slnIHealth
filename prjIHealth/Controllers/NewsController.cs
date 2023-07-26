using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using prjIHealth.Models;
using prjiHealth.ViewModels;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using prjIHealth.ViewModels;
using System.Text.Json;
using prjiHealth.Models;
using Microsoft.AspNetCore.Http;
using System.Collections;

namespace prjiHealth.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private IHealthContext _db;
        private IWebHostEnvironment _enviroment;
        public NewsController(IWebHostEnvironment n, IHealthContext context)
        {
            _enviroment = n;
            _db = context;
        }


        public IActionResult Blog(CNewsViewModel vModel, int? page)
        {
            IHealthContext db = new IHealthContext();
            IEnumerable<TNews> datas = null;

            var pageNumber = page ?? 1;
            var news = db.TNews.OrderBy(n => n.FNewsId).ToList();
            var testNews = db.TNews.Include(t => t.FNewsCategory).ToList();
            var onePageOfBlogs = news.ToPagedList(pageNumber, 6);
            var category0Count = db.TNews.Select(t => t).Count();
            ViewBag.Category0Count = category0Count;
            var category1Count = db.TNews.Where(t => t.FNewsCategoryId == 1).Count();
            ViewBag.Category1Count = category1Count;
            var category2Count = db.TNews.Where(t => t.FNewsCategoryId == 2).Count();
            ViewBag.Category2Count = category2Count;
            var category3Count = db.TNews.Where(t => t.FNewsCategoryId == 3).Count();
            ViewBag.Category3Count = category3Count;
            var category4Count = db.TNews.Where(t => t.FNewsCategoryId == 4).Count();
            ViewBag.Category4Count = category4Count;

            if (string.IsNullOrEmpty(vModel.txtKeyword))
            {
                datas = (from t in db.TNews
                         select t).Include(c => c.TNewsComments);
                onePageOfBlogs = datas.ToPagedList(pageNumber, 6);
                //int datascount = 0;
                //datascount = datas.Count();

                //ViewBag.SearchResult = "您搜尋的資料共(" + datascount + ")";
            }
            else
            {
                datas = db.TNews.Where(t => t.FTitle.Contains(vModel.txtKeyword))
                    .Include(c => c.TNewsComments);
                onePageOfBlogs = datas.ToPagedList(pageNumber, 6);
                //int datascount = 0;
                //datascount = datas.Count();

                //ViewBag.SearchResult = "您搜尋的資料共(" + datascount + ")";
                if (datas.Count() == 0)
                {
                    datas = (from t in db.TNews
                             select t).Include(c => c.TNewsComments);
                    onePageOfBlogs = datas.ToPagedList(pageNumber, 6);
                }
            }
            //List<CNewsViewModel> news = new List<CNewsViewModel>();
            //foreach (var n in datas)
            //{
            //    CNewsViewModel newsViewModel = new CNewsViewModel(_db)
            //    {
            //        _news = n,
            //        //newsCategory = n.FNewsCategory,
            //        //getMember = n.FMember
            //    };
            //    news.Add(newsViewModel);
            //}
            ViewBag.onePageOfBlogs = onePageOfBlogs;
            return View(onePageOfBlogs);
        }


        public IActionResult BlogCategory(int? id)
        {
            IHealthContext db = new IHealthContext();
            var category0Count = db.TNews.Select(t => t).Count();
            ViewBag.Category0Count = category0Count;
            var category1Count = db.TNews.Where(t => t.FNewsCategoryId == 1).Count();
            ViewBag.Category1Count = category1Count;
            var category2Count = db.TNews.Where(t => t.FNewsCategoryId == 2).Count();
            ViewBag.Category2Count = category2Count;
            var category3Count = db.TNews.Where(t => t.FNewsCategoryId == 3).Count();
            ViewBag.Category3Count = category3Count;
            var category4Count = db.TNews.Where(t => t.FNewsCategoryId == 4).Count();
            ViewBag.Category4Count = category4Count;
            if (id == 1)
            {
                ViewBag.AddClass1 = "selectedCategory";
            }
            if (id == 2)
            {
                ViewBag.AddClass2 = "selectedCategory";
            }
            if (id == 3)
            {
                ViewBag.AddClass3 = "selectedCategory";
            }
            if (id == 4)
            {
                ViewBag.AddClass4 = "selectedCategory";
            }
            var selCateID = (from n in db.TNews
                             join c in db.TNewsCategories
                             on n.FNewsCategoryId equals c.FNewsCategoryId
                             where n.FNewsCategoryId == id
            
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
                             }).ToList();
            return View(selCateID);
        }

        public IActionResult BlogDetail(int? id)
        {
            IHealthContext db = new IHealthContext();

            var category0Count = db.TNews.Select(t => t).Count();
            ViewBag.Category0Count = category0Count;
            var category1Count = db.TNews.Where(t => t.FNewsCategoryId == 1).Count();
            ViewBag.Category1Count = category1Count;
            var category2Count = db.TNews.Where(t => t.FNewsCategoryId == 2).Count();
            ViewBag.Category2Count = category2Count;
            var category3Count = db.TNews.Where(t => t.FNewsCategoryId == 3).Count();
            ViewBag.Category3Count = category3Count;
            var category4Count = db.TNews.Where(t => t.FNewsCategoryId == 4).Count();
            ViewBag.Category4Count = category4Count;

            TNews news = db.TNews.FirstOrDefault(t => t.FNewsId == id);
            //var news = db.TMembers.Include(n=>n.TNews).FirstOrDefault(t=>t.)
            if (news == null)
                return RedirectToAction("Blog");

            return View(news);
        }
        [HttpPost]
        public IActionResult ReplyComment(TNewsComment comment)
        {
            IHealthContext db = new IHealthContext();
            int userID = TakeMemberID();
            TNewsComment newsComment = new TNewsComment()
            {
                FMemberId = userID,
                FNewsReply = comment.FNewsReply,
                FNewsId = comment.FNewsId
            };
            if (userID == 0)
            {
                newsComment.FMemberId = comment.FMemberId;
            }
            else
            {
                newsComment.FMemberId = userID;
            }

            db.TNewsComments.Add(newsComment);
            db.SaveChanges();
            return RedirectToAction("BlogDetail", "News", new { id = comment.FNewsId });
            //return Content("謝謝你哦", "text/plain", System.Text.Encoding.UTF8);
        }
        public IActionResult BlogSelectCategoryAPI(int? id)
        {
            IHealthContext db = new IHealthContext();
            
            if (id == null)
            {
                var selCateID = db.TNews.Include(c => c.TNewsComments).OrderBy(t => t.FNewsId).Select(t => new
                {
                    t.FNewsId,
                    t.FThumbnailPath,
                    t.FNewsDate,
                    commentCount = t.TNewsComments.Count,
                    t.FTitle,
                    t.FContent
                });
                return Json(selCateID);
            }
            else
            {
                var selCateID = db.TNews.Include(c => c.TNewsComments).Where(t => t.FNewsCategoryId == id).OrderBy(t => t.FNewsId).Select(t=>new
                {
                    t.FNewsId,
                    t.FThumbnailPath,
                    t.FNewsDate,
                    commentCount = t.TNewsComments.Count,
                    t.FTitle,
                    t.FContent
                });
                return Json(selCateID);
            }
            
        }

        public IActionResult LoadComment(int id)
        {
            IHealthContext db = new IHealthContext();
            var comment = db.TNewsComments.Where(t => t.FNewsId == id).OrderByDescending(m => m.FNewsCommentId)
                .Select(vModel => new CNewsCommentViewModel()
                {
                    FMemberId = vModel.FMemberId,
                    FNewsId = vModel.FNewsId,
                    FNewsReply = vModel.FNewsReply,
                    MemberName = vModel.FMember,
                    FNewsCommentId = vModel.FNewsCommentId
                }).ToList();
            var commentCount = db.TNewsComments.Where(t => t.FNewsId == id).OrderByDescending(m => m.FNewsCommentId)
           .Select(vModel => new CNewsCommentViewModel()
           {
               FMemberId = vModel.FMemberId,
               FNewsId = vModel.FNewsId,
               FNewsReply = vModel.FNewsReply,
               MemberName = vModel.FMember,
               FNewsCommentId = vModel.FNewsCommentId
           }).Count();
            ViewBag.CommentCount = commentCount;
            return PartialView(comment);
        }

        public IActionResult CommentAPI(int? id)
        {
            IHealthContext db = new IHealthContext();
            var comment = db.TNewsComments.Where(t => t.FNewsId == id).Select(vModel => new CNewsCommentViewModel()
            {
                FMemberId = vModel.FMemberId,
                FNewsId = vModel.FNewsId,
                FNewsReply = vModel.FNewsReply,
                MemberName = vModel.FMember
            }).ToList();
            return Json(comment);
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

        public ActionResult RandomBlog(int? id)
        {
            IHealthContext db = new IHealthContext();
            ArrayList list = new ArrayList();
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            int count = db.TNews.Count();
            int num = 0;
            List<int> items = new List<int>();
            for (int i = 0; i < 3;)
            {
                num = rd.Next(1, count);

                if (items.Contains(num) || num == id)
                {
                    continue;
                }
                else
                {
                    TNews rdNews = (from t in db.TNews
                                    where t.FNewsId == num
                                    select t).FirstOrDefault();
                    list.Add(rdNews);
                    i++;
                    items.Add(num);
                }
            }
            return Json(list);
        }

        //public IActionResult CommentCount(int? id)
        //{
        //    IHealthContext db = new IHealthContext();
        //    IEnumerable<TNewsComment> newsComments = null;
        //    int q=0;
        //    if (id == null)
        //    {
        //        newsComments = from t in db.TNewsComments
        //                       select t;
        //        q = newsComments.Count();
        //    }
        //    if (id != null)
        //    {
        //        newsComments = from t in db.TNewsComments
        //                       where t.FNewsId == id
        //                       select t;
        //        q = newsComments.Count();
        //    }
        //    return Json(q);
        //}
    }
}
