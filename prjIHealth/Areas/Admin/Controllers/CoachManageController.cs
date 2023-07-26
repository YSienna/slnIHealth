using Microsoft.AspNetCore.Mvc;
using prjIHealth.Models;
using prjIHealth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    public class CoachManageController : Controller
    {
        private IHealthContext db;
        public CoachManageController(IHealthContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CoachList()
        {
            List<CCoachResumeViewModel> coaches = new List<CCoachResumeViewModel>();
            foreach(var c in db.TCoaches.OrderByDescending(c=>c.FApplyDate))
            {
                CCoachResumeViewModel coachViewModel = new CCoachResumeViewModel(db)
                {
                    FCoachId=c.FCoachId,
                    FCoachName=c.FCoachName,
                    FMemberId=c.FMemberId,
                    FCityId=c.FCityId,
                    FCoachImage=c.FCoachImage,
                    FCoachFee=c.FCoachFee,
                    FCoachDescription=c.FCoachDescription,
                    FApplyDate=c.FApplyDate,
                    FStatusNumber=c.FStatusNumber,
                    FVisible=c.FVisible,
                    FSlogan=c.FSlogan
                };
                coaches.Add(coachViewModel);
            }
            return View(coaches);
        }
        public IActionResult getCoach(int? id)
        {
            var c = db.TCoaches.FirstOrDefault(c => c.FCoachId == id);
            CCoachResumeViewModel coachViewModel = new CCoachResumeViewModel(db)
            {
                FCoachId = c.FCoachId,
                FCoachName = c.FCoachName,
                FMemberId = c.FMemberId,
                FCityId = c.FCityId,
                FCoachImage = c.FCoachImage,
                FCoachFee = c.FCoachFee,
                FCoachDescription = c.FCoachDescription,
                FApplyDate = c.FApplyDate,
                FStatusNumber = c.FStatusNumber,
                FVisible = c.FVisible,
                FSlogan = c.FSlogan
            };

            return Json(coachViewModel);
        }
        public IActionResult passResume(int? id)
        {
            var theCoach = db.TCoaches.FirstOrDefault(c => c.FCoachId == id);
            theCoach.FStatusNumber = 66;
            theCoach.FVisible = true;
            db.SaveChanges();
            return Content("");
        }
        public IActionResult returnResume(int? id)
        {
            var theCoach = db.TCoaches.FirstOrDefault(c => c.FCoachId == id);
            theCoach.FStatusNumber = 67;
            theCoach.FVisible = false;
            db.SaveChanges();
            return Content("");
        }

        public IActionResult loadCoach(CSearchCoachViewModel searchCoachViewModel)
        {
            IEnumerable<TCoach> tCoaches = null;
            if (searchCoachViewModel.SkillId != 0)
                tCoaches = db.TCoachSkills.Where(cs => cs.FSkillId == searchCoachViewModel.SkillId).Select(cs => cs.FCoach);
            else
                tCoaches = db.TCoaches;

            if (searchCoachViewModel.Sort == 1)
                tCoaches = tCoaches.OrderByDescending(c => c.FApplyDate);
            else
                tCoaches = tCoaches.OrderBy(c => c.FApplyDate);

            if (searchCoachViewModel.CityId != 0)
                tCoaches = tCoaches.Where(c => c.FCityId == searchCoachViewModel.CityId);
            if (!String.IsNullOrEmpty(searchCoachViewModel.KeyWord))
                tCoaches = tCoaches.Where(c => c.FCoachName.ToLower().Contains(searchCoachViewModel.KeyWord.ToLower()) || c.FCoachId.ToString().Contains(searchCoachViewModel.KeyWord));
            if (searchCoachViewModel.StatusNum != 0)
                tCoaches = tCoaches.Where(c => c.FStatusNumber == searchCoachViewModel.StatusNum);

            List<CCoachResumeViewModel> coaches = null;
            if (tCoaches.Count() != 0)
            {
                coaches = new List<CCoachResumeViewModel>();
                foreach (var c in tCoaches)
                {
                    CCoachResumeViewModel coachViewModel = new CCoachResumeViewModel(db)
                    {
                        FCoachId = c.FCoachId,
                        FCoachName = c.FCoachName,
                        FMemberId = c.FMemberId,
                        FCityId = c.FCityId,
                        FCoachImage = c.FCoachImage,
                        FCoachFee = c.FCoachFee,
                        FCoachDescription = c.FCoachDescription,
                        FApplyDate = c.FApplyDate,
                        FStatusNumber = c.FStatusNumber,
                        FVisible = c.FVisible,
                        FSlogan = c.FSlogan
                    };
                    coaches.Add(coachViewModel);
                }
            }
            return Json(coaches);
        }
        public IActionResult RateList()
        {
            var tRates = db.TCoachRates.OrderByDescending(r=>r.FRateDate);
            List<CCoachRateViewModel> rates = new List<CCoachRateViewModel>();
            foreach (var r in tRates)
            {
                CCoachRateViewModel coachRateViewModel = new CCoachRateViewModel(db)
                {
                    FRateId = r.FRateId,
                    FMemberId = r.FMemberId,
                    FCoachId = r.FCoachId,
                    FRateStar = r.FRateStar,
                    FRateText = r.FRateText,
                    FRateDate = r.FRateDate,
                    FVisible = r.FVisible
                };
                rates.Add(coachRateViewModel);
            }
            return View(rates);
        }
        public IActionResult editVisible(int? flag,int? FRateId)
        {
            var theRate = db.TCoachRates.FirstOrDefault(r => r.FRateId == FRateId);
            if (flag == 1)
                theRate.FVisible = true;
            else
                theRate.FVisible = false;
            db.SaveChanges();
            return Content("");
        }

        public IActionResult loadRate(int? IsDesc , string Keyword)
        {
            IEnumerable<TCoachRate> tRates;
            if (IsDesc == 0)
                tRates = db.TCoachRates.OrderBy(r => r.FRateDate);
            else if (IsDesc == 2)
                tRates = db.TCoachRates.OrderByDescending(r => r.FRateStar);
            else if (IsDesc == 3)
                tRates = db.TCoachRates.OrderBy(r => r.FRateStar);
            else
                tRates = db.TCoachRates.OrderByDescending(r => r.FRateDate);
            if (!String.IsNullOrEmpty(Keyword))
                tRates = tRates.Where(r => r.FRateText.ToLower().Contains(Keyword.ToLower()));
            if (tRates.Count() != 0)
            {
                List<CCoachRateViewModel> rates = new List<CCoachRateViewModel>();
                foreach (var r in tRates)
                {
                    CCoachRateViewModel coachRateViewModel = new CCoachRateViewModel(db)
                    {
                        FRateId = r.FRateId,
                        FMemberId = r.FMemberId,
                        FCoachId = r.FCoachId,
                        FRateStar = r.FRateStar,
                        FRateText = r.FRateText,
                        FRateDate = r.FRateDate,
                        FVisible = r.FVisible
                    };
                    rates.Add(coachRateViewModel);
                }
                return Json(rates);
            }
            else
                return Json(null);
        }




    }
}
