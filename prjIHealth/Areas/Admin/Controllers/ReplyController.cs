using Microsoft.AspNetCore.Mvc;
using prjiHealth.ViewModels;
using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    public class ReplyController : Controller
    {
        public IActionResult ProblemReplyList()
        {
            IHealthContext db = new IHealthContext();
            var datafix = (from t in db.TProblems
                           join p in db.TProblemCategroies
                           on t.FProblemCategoryId equals p.FProblemCategoryId
                           join s in db.TStatuses
                           on t.FStatusNumber equals s.FStatusNumber
                           orderby t.FProblemTime descending
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
                               Status = t.FStatusNumberNavigation,
                               FFilePath=t.FFilePath
                           }).ToList();
            return View(datafix);
        }
        public IActionResult Reply(int? id)
        {
            IHealthContext db = new IHealthContext();
            var prob = (from t in db.TProblems
                        join p in db.TProblemCategroies
                        on t.FProblemCategoryId equals p.FProblemCategoryId
                        join s in db.TStatuses
                        on t.FStatusNumber equals s.FStatusNumber
                        where t.FProblemId == id
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
                            Status = t.FStatusNumberNavigation,
                            FFilePath=t.FFilePath
                        }).ToList();
            if (prob == null)
            {
                return RedirectToAction("ProblemReplyList");
            }
            return View(prob);
        }
        [HttpPost]
        public IActionResult Reply(CProblemViewModel p)
        {
            DateTime date = DateTime.Now;
            ViewBag.Time = date.ToString("yyyy/MM/dd HH:mm:ss");
            if (ModelState.IsValid)
            {
                IHealthContext db = new IHealthContext();
                TProblem prob = db.TProblems.FirstOrDefault(t => t.FProblemId == p.FProblemId);
                prob.FStatusNumber = p.FStatusNumber;
                TReply reply = new TReply();
                reply.FProblemId = p.FProblemId;
                reply.FReplyTime = p.FReplyTime;
                reply.FReplyContent = p.FReplyContent;
                reply.FReplierId = p.FReplierId;
                reply.FReplyType = p.FReplyType;
                db.TReplies.Add(reply);

                db.SaveChanges();
            }
            else
            {
                return RedirectToAction("Reply","Reply",p.FProblemId);
            }
            return RedirectToAction("ProblemReplyList");
        }
        //問題類別篩選ACTION
        public IActionResult SelectByProblemCategoryID(int id)
        {
            IHealthContext db = new IHealthContext();

            var cid = (from t in db.TProblems
                       join p in db.TProblemCategroies
                       on t.FProblemCategoryId equals p.FProblemCategoryId
                       join s in db.TStatuses
                       on t.FStatusNumber equals s.FStatusNumber
                       where t.FProblemCategoryId==id
                       orderby t.FProblemTime descending
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
                           Status = t.FStatusNumberNavigation,
                           FFilePath = t.FFilePath
                       }).ToList();
            return Json(cid);
        }
        //處理狀態篩選ACTION
        public IActionResult SelectByStatus(int id)
        {
            IHealthContext db = new IHealthContext();

            var sid = (from t in db.TProblems
                       join p in db.TProblemCategroies
                       on t.FProblemCategoryId equals p.FProblemCategoryId
                       join s in db.TStatuses
                       on t.FStatusNumber equals s.FStatusNumber
                       where t.FStatusNumber == id
                       orderby t.FProblemTime descending
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
                           Status = t.FStatusNumberNavigation,
                           FFilePath = t.FFilePath
                       }).ToList();
            return Json(sid);
        }
        //清空篩選ACTION
        public IActionResult Reset()
        {
            IHealthContext db = new IHealthContext();

            var all = (from t in db.TProblems
                       join p in db.TProblemCategroies
                       on t.FProblemCategoryId equals p.FProblemCategoryId
                       join s in db.TStatuses
                       on t.FStatusNumber equals s.FStatusNumber
                       orderby t.FProblemTime descending
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
                           Status = t.FStatusNumberNavigation,
                           FFilePath = t.FFilePath
                       }).ToList();
            return Json(all);
        }
        //日期篩選ACTION
        public IActionResult SelectByDate(string date)
        {
            IHealthContext db = new IHealthContext();
            string dateparse = date.Replace('-','/').Substring(0, 10);
            var day = (from t in db.TProblems
                       join p in db.TProblemCategroies
                       on t.FProblemCategoryId equals p.FProblemCategoryId
                       join s in db.TStatuses
                       on t.FStatusNumber equals s.FStatusNumber
                       where t.FProblemTime.Contains(dateparse)
                       orderby t.FProblemTime descending
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
                           Status = t.FStatusNumberNavigation,
                           FFilePath = t.FFilePath
                       }).ToList();
            return Json(day);
        }
    }
}
