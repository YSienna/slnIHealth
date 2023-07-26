using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjiHealth.Models;
using prjiHealth.ViewModels;
using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiHealth.Controllers
{
    public class ProblemController : Controller
    {
        private IWebHostEnvironment _enviroment;

        public ProblemController(IWebHostEnvironment p)
        {
            _enviroment = p;
        }
        public IActionResult ReplyProblem()
        {
            DateTime date = DateTime.Now;
            ViewBag.Time = date.ToString("yyyy/MM/dd HH:mm:ss");
            return View();
        }
        [HttpPost]
        public IActionResult ReplyProblem(CProblemViewModel p)
        {
            DateTime date = DateTime.Now;
            ViewBag.Time = date.ToString("yyyy/MM/dd HH:mm:ss");
            if (string.IsNullOrEmpty(p.FContactPhone) && string.IsNullOrEmpty(p.FEmail))
            {
                ViewBag.Message_EP = "請輸入EMail或電話";
            }
            else if (string.IsNullOrEmpty(p.FProblemContent))
            {
                ViewBag.Message_PC = "請輸入問題內容";
            }
            else if (p.FProblemCategoryId <= 0 || p.FProblemCategoryId > 6)
            {
                ViewBag.Message_PCID = "請選擇問題類別";
            }
            else
            {
                int userID = TakeMemberID();
                TProblem prob = new TProblem();
                IHealthContext db = new IHealthContext();
                if (p.photo != null)
                {
                    string pName = Guid.NewGuid().ToString() + ".jpg";
                    p.photo.CopyTo(new FileStream(_enviroment.WebRootPath + "/img/problem/" + pName, FileMode.Create));
                    prob.FFilePath = pName;
                }
                prob.FProblemTime = p.FProblemTime;
                prob.FProblemCategoryId = p.FProblemCategoryId;
                prob.FProblemContent = p.FProblemContent;
                if (userID == 0)
                {
                    prob.FMemberId = p.FMemberId;
                }
                else
                {
                    prob.FMemberId = userID;
                }
                prob.FOrderId = p.FOrderId;
                prob.FEmail = p.FEmail;
                prob.FContactPhone = p.FContactPhone;
                prob.FStatusNumber = p.FStatusNumber;
                db.TProblems.Add(prob);
                db.SaveChanges();
                ViewBag.Message_SUCCESS = "意見送出成功";
            }

            return View();
        }
        //依狀態篩選內容
        public IActionResult SelectByStatus(int id)
        {
            int userID = TakeMemberID();
            IHealthContext db = new IHealthContext();
            List<CProblemViewModel> sid = null;
            if (id == 0)
            {
                sid = (from t in db.TProblems
                       join p in db.TProblemCategroies
                       on t.FProblemCategoryId equals p.FProblemCategoryId
                       join s in db.TStatuses
                       on t.FStatusNumber equals s.FStatusNumber
                       where t.FMemberId== userID
                       select new CProblemViewModel()
                       {
                           FProblemId = t.FProblemId,
                           FProblemTime = t.FProblemTime,
                           FProblemCategory = t.FProblemCategory,
                           FProblemContent = t.FProblemContent,
                           FMemberId = userID,
                           FOrderId = t.FOrderId,
                           FEmail = t.FEmail,
                           FContactPhone = t.FContactPhone,
                           Status = t.FStatusNumberNavigation,
                           FFilePath = t.FFilePath
                       }).ToList();
            }
            else {
            sid = (from t in db.TProblems
                       join p in db.TProblemCategroies
                       on t.FProblemCategoryId equals p.FProblemCategoryId
                       join s in db.TStatuses
                       on t.FStatusNumber equals s.FStatusNumber
                       where t.FStatusNumber == id && t.FMemberId == userID
                       select new CProblemViewModel()
                       {
                           FProblemId = t.FProblemId,
                           FProblemTime = t.FProblemTime,
                           FProblemCategory = t.FProblemCategory,
                           FProblemContent = t.FProblemContent,
                           FMemberId = userID,
                           FOrderId = t.FOrderId,
                           FEmail = t.FEmail,
                           FContactPhone = t.FContactPhone,
                           Status = t.FStatusNumberNavigation,
                           FFilePath = t.FFilePath
                       }).ToList();
            }
            return Json(sid);
        }

        public IActionResult CheckReply()
        {
            int userID = TakeMemberID();
            IHealthContext db = new IHealthContext();
            var datafix = (from t in db.TProblems
                           join p in db.TProblemCategroies
                           on t.FProblemCategoryId equals p.FProblemCategoryId
                           join s in db.TStatuses
                           on t.FStatusNumber equals s.FStatusNumber
                           where t.FMemberId==userID
                           select new CProblemViewModel()
                           {
                               FProblemId = t.FProblemId,
                               FProblemTime = t.FProblemTime,
                               FProblemCategory = t.FProblemCategory,
                               FProblemContent = t.FProblemContent,
                               FMemberId = t.FMemberId,
                               FOrderId = t.FOrderId,
                               FEmail = t.FEmail,
                               FContactPhone = t.FContactPhone,
                               Status = t.FStatusNumberNavigation
                           }).ToList();
            return View(datafix);
        }
        //讀取客服回覆內容
        public IActionResult LoadReply(int id)
        {
            IHealthContext db = new IHealthContext();
            var data = (from t in db.TReplies
                        where t.FProblemId == id
                        select t.FReplyContent).FirstOrDefault();
            return Json(data);
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
    }

}
