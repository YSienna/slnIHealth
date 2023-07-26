using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjiHealth.Models;
using prjIHealth.Controllers;
using prjIHealth.Models;
using prjIHealth.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiHealth.Controllers
{
    public class CoachController : Controller
    {
        private IHealthContext db;
        private readonly IHealthContext _context;
        private IWebHostEnvironment _environment;
        public CoachController(IHealthContext context, IWebHostEnvironment environment)
        {
            db = context;
            _context = context;
            _environment = environment;
        }               

        //取得教練所有排課
        public IActionResult GetAllReservation()
        {
            int userId = 11;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                userId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }
            var coachId = _context.TCoaches.FirstOrDefault(c => c.FMemberId == userId).FCoachId;
            var reservations = _context.TReservations
                .Include(r => r.FCourse).ThenInclude(c=>c.FCoachContact).ThenInclude(c => c.FMember)
                .Where(r=>r.FCourse.FCoachContact.FCoachId == coachId).OrderBy(r=>r.FCourseTime).ToList();

            var reservationList=CCalendarViewModel.ReservationList(reservations);
            return Json(reservationList);
        }
        //取得所有有排課的MemberId
        public IActionResult GetReservationMemId()
        {
            int userId = 11;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                userId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }
            var coachId = _context.TCoaches.FirstOrDefault(c => c.FMemberId == userId).FCoachId;
            var memIdList = _context.TReservations
                .Include(r => r.FCourse).ThenInclude(c => c.FCoachContact).ThenInclude(c => c.FMember)
                .Where(r => r.FCourse.FCoachContact.FCoachId == coachId)
                .Select(r=>r.FCourse.FCoachContact.FMemberId).Distinct().ToList();
            return Json(memIdList);
        }

        //public IActionResult GetDayTime(string date)
        //{
        //    var dayTime = _context.TReservations.Where(r => r.FCourseTime.Substring(0, 8) == date).ToList();            
        //    return Json(dayTime);
        //}

        //教課列表
        public IActionResult TeachingList()
        {
            int userId = 11;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                userId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }
            var coach = _context.TCoaches.Where(c => c.FStatusNumber == 66).FirstOrDefault(c => c.FMemberId == userId);
            if (coach == null)
                return RedirectToAction("CreateResume");

            var data = _context.TCourses
                .Include(c => c.FCoachContact).ThenInclude(cc=>cc.FMember)
                .Include(c=>c.TReservations)
                .Where(c => c.FCoachContact.FCoachId == coach.FCoachId).ToList();
            return View(CTeachingListViewModel.CourseList(data));
        }
        
        //完成排課
        public IActionResult ReservationDone(int id)
        {
            var reservation = _context.TReservations.FirstOrDefault(r => r.FReservationId == id);
            reservation.FStatusNumber = 61;
            _context.SaveChanges();

            //若Reservation皆結束，即修改課程狀態為「已結束」
            int courseId = (int)reservation.FCourseId;
            if (_context.TReservations.Where(r => r.FCourseId == courseId).Select(r => r.FStatusNumber).ToList().All(num=>num==61))
            {
                var thisCourse = _context.TCourses.FirstOrDefault(c => c.FCourseId == courseId);
                thisCourse.FStatusNumber = 56;
            }
            _context.SaveChanges();
            return Content("Success", "text/plain");
        }
        //更改時間
        public IActionResult EditReservation(int id,string date,string time)
        {
            //取得該教練排課
            int userId = 11;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                userId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }
            var coachId = _context.TCoaches.FirstOrDefault(c => c.FMemberId == userId).FCoachId;
            var reservations = _context.TReservations.Include(r => r.FCourse).ThenInclude(c => c.FCoachContact)
                .Where(r => r.FCourse.FCoachContact.FCoachId == coachId);

            //比對教練該時段是否已額滿
            var occupied = reservations.Where(r => r.FCourseTime.Substring(0, 8) == date.Replace("-", ""))
                .Select(r => Convert.ToInt32(r.FCourseTime.Substring(8,2))).ToList();
            if (occupied.Contains(Convert.ToInt32(time)))
                return Content("Fail", "text/plain");            
            else
            {
                var reservation = _context.TReservations.FirstOrDefault(r => r.FReservationId == id);
                string newDate = date.Replace("-", "");
                string newTime = time.Length == 1 ? "0" + time : time;
                reservation.FCourseTime = newDate + newTime + "00";
                _context.SaveChanges();
                return Content("Success", "text/plain");
            }            
        }
        //取得進行中課程
        public IActionResult GetCourseInProcess(int id)
        {
            var courses = _context.TCourses.Where(c => c.FStatusNumber == id).Select(c => c.FCourseId).ToList();
            return Json(courses);
        }
        //取得已結束課程
        public IActionResult GetCourseDone(int id)
        {
            var courses = _context.TCourses.Where(c => c.FStatusNumber == id).Select(c => c.FCourseId).ToList();
            return Json(courses);
        }
       
        //填寫履歷
        public IActionResult CreateResume()
        {
            int userId = 8; //備用帳號
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                userId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }
            TCoach c = _context.TCoaches.FirstOrDefault(c => c.FMemberId == userId);
            if (c != null)
                return RedirectToAction("EditResume");
            CCoachViewModel vModel = new CCoachViewModel
            {
                Coach = new TCoach()
            };
            return View(vModel);
        }

        [HttpPost] //送出履歷
        public IActionResult CreateResume(TCoach c, IFormFile File, int[] fCoachSkill, int[] fCoachTime, string[] fExperience, string[] fLicense)
        {
            int userId = 8;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                userId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }
            c.FMemberId = userId;
            c.FStatusNumber = 65;
            c.FVisible = false;
            c.FApplyDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            _context.TCoaches.Add(c);
            _context.SaveChanges();
            if (File != null)
            {
                string photoName = Guid.NewGuid().ToString() + ".jpg";
                File.CopyTo(new FileStream(_environment.WebRootPath + "/img/coach/coachImage/" + photoName, FileMode.Create));
                c.FCoachImage = photoName;
            }

            //新增Skills            
            foreach (int skillId in fCoachSkill)
            {
                TCoachSkill newSkill = new TCoachSkill
                {
                    FCoachId = c.FCoachId,
                    FSkillId = skillId
                };
                _context.TCoachSkills.Add(newSkill);
            }

            //新增AvailableTime            
            foreach (int timeId in fCoachTime)
            {
                TCoachAvailableTime newTime = new TCoachAvailableTime
                {
                    FCoachId = c.FCoachId,
                    FAvailableTimeId = timeId
                };
                _context.TCoachAvailableTimes.Add(newTime);
            }

            //新增Experience            
            foreach (string Exp in fExperience)
            {
                if (Exp != null)
                {
                    TCoachExperience newExp = new TCoachExperience
                    {
                        FCoachId = c.FCoachId,
                        FExperience = Exp.Trim()
                    };
                    _context.TCoachExperiences.Add(newExp);
                }
            }

            //新增License           
            foreach (string Lic in fLicense)
            {
                if (Lic != null)
                {
                    TCoachLicense newLic = new TCoachLicense
                    {
                        FCoachId = c.FCoachId,
                        FLicense = Lic.Trim()
                    };
                    _context.TCoachLicenses.Add(newLic);
                }
            }
            _context.SaveChanges();
            return Content("Success", "text/plain");
        }
        
        //修改履歷
        public IActionResult EditResume()
        {
            int userId = 11;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                userId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }
            TCoach data = _context.TCoaches
                .Include(c => c.TCoachSkills)
                .Include(c => c.TCoachAvailableTimes)
                .Include(c => c.TCoachExperiences)
                .Include(c => c.TCoachLicenses)
                .FirstOrDefault(c => c.FMemberId == userId);
            CCoachViewModel vModel = new CCoachViewModel
            {
                Coach = data
            };
            return View(vModel);
        }

        [HttpPost]  //送出修改
        public IActionResult EditResume(TCoach c, IFormFile File, int[] fCoachSkill, int[] fCoachTime, string[] fExperience, string[] fLicense)
        {
            int userId = 11;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                userId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }
            TCoach coach = _context.TCoaches.FirstOrDefault(c => c.FMemberId == userId);
            if (coach != null)
            {
                if (File != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    File.CopyTo(new FileStream(_environment.WebRootPath + "/img/coach/coachImage/" + photoName, FileMode.Create));
                    coach.FCoachImage = photoName;
                }
                coach.FCityId = c.FCityId;
                coach.FCoachFee = c.FCoachFee;
                coach.FCoachDescription = c.FCoachDescription;
                coach.FSlogan = c.FSlogan;

                //新增Skills
                var currentSkills = _context.TCoachSkills.Where(cs => cs.FCoachId == coach.FCoachId).ToList();
                foreach (var skill in currentSkills)
                    _context.TCoachSkills.Remove(skill);
                foreach (int skillId in fCoachSkill)
                {
                    TCoachSkill newSkill = new TCoachSkill
                    {
                        FCoachId = coach.FCoachId,
                        FSkillId = skillId
                    };
                    _context.TCoachSkills.Add(newSkill);
                }

                //新增AvailableTime
                var currentTime = _context.TCoachAvailableTimes.Where(at => at.FCoachId == coach.FCoachId).ToList();
                foreach (var time in currentTime)
                    _context.TCoachAvailableTimes.Remove(time);
                foreach (int timeId in fCoachTime)
                {
                    TCoachAvailableTime newTime = new TCoachAvailableTime
                    {
                        FCoachId = coach.FCoachId,
                        FAvailableTimeId = timeId
                    };
                    _context.TCoachAvailableTimes.Add(newTime);
                }

                //新增Experience
                var currentExp = _context.TCoachExperiences.Where(e => e.FCoachId == coach.FCoachId).ToList();
                foreach (var exp in currentExp)
                    _context.TCoachExperiences.Remove(exp);
                foreach (string Exp in fExperience)
                {
                    if (Exp != null)
                    {
                        TCoachExperience newExp = new TCoachExperience
                        {
                            FCoachId = coach.FCoachId,
                            FExperience = Exp.Trim()
                        };
                        _context.TCoachExperiences.Add(newExp);
                    }
                }

                //新增License
                var currentLic = _context.TCoachLicenses.Where(e => e.FCoachId == coach.FCoachId).ToList();
                foreach (var lic in currentLic)
                    _context.TCoachLicenses.Remove(lic);
                foreach (string Lic in fLicense)
                {
                    if (Lic != null)
                    {
                        TCoachLicense newLic = new TCoachLicense
                        {
                            FCoachId = coach.FCoachId,
                            FLicense = Lic.Trim()
                        };
                        _context.TCoachLicenses.Add(newLic);
                    }
                }
            }
            _context.SaveChanges();
            return Content("Success", "text/plain");
        }
        //切換履歷顯示狀態
        public IActionResult ToggleVisible(int id)
        {
            var coach = _context.TCoaches.FirstOrDefault(c => c.FCoachId == id);
            if (coach.FVisible == true)
                coach.FVisible = false; //設為不公開 
            else
                coach.FVisible = true;  //設為公開
            _context.SaveChanges();
            return Content(coach.FVisible.ToString(), "text/plain");
        }
        
        //教練專區-招生紀錄==================================================
        public IActionResult RecruitmentList()
        {
            //取得登入者ID
            int theMemberId = 11;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                theMemberId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }

            var contacts = db.TCoachContacts.Where(c =>c.FCoach.FMemberId==theMemberId).OrderByDescending(c=>c.FContactDate);
            List<CContactViewModel> contactViewModels = new List<CContactViewModel>();
            foreach(var c in contacts)
            {
                CContactViewModel contactViewModel = new CContactViewModel(db)
                {
                    FCoachContactId = c.FCoachContactId,
                    FMemberId = c.FMemberId,
                    FStatusNumber = c.FStatusNumber,
                    FAvailableTimeNum = c.FAvailableTimeNum
                };
                contactViewModels.Add(contactViewModel);
            }

            return View(contactViewModels);
        } 
        //教練專區-招生紀錄:改變聯繫狀態
        public IActionResult changeContactStatus(TCoachContact contact)
        {
            db.TCoachContacts.FirstOrDefault(c => c.FCoachContactId == contact.FCoachContactId).FStatusNumber = contact.FStatusNumber;
            db.SaveChanges();
            return Content("");
        } 
        //教練專區-招生紀錄:顯示學員資料
        public IActionResult showMember(int? memberId)
        {
            CContactViewModel member = new CContactViewModel(db)
            {
                FMemberId = memberId
            };

            return Json(member);
        } 
        //教練專區-招生紀錄:新增課程&排課
        public IActionResult createCourse(TCourse course)
        {
            //加入課程
            course.FStatusNumber = 55;
            course.FVisible = true;
            db.TCourses.Add(course);

            //改變聯繫狀態
            db.TCoachContacts.FirstOrDefault(c => c.FCoachContactId == course.FCoachContactId).FStatusNumber = 52;

            db.SaveChanges(); 

            //計算排課起始日
            int day = 0;
            switch (DateTime.Now.DayOfWeek.ToString())
            {
                case "Monday":
                    day = 1;
                    break;
                case "Tuesday":
                    day = 2;
                    break;
                case "Wednesday":
                    day = 3;
                    break;
                case "Thursday":
                    day = 4;
                    break;
                case "Friday":
                    day = 5;
                    break;
                case "Saturday":
                    day = 6;
                    break;
                case "Sunday":
                    day = 7;
                    break;
            }
            int courseDay = int.Parse(course.FAvailableTimeNum.ToString().Substring(0, 1));
            DateTime date;
            if ((courseDay - day) > 0)
            {
                int interval = courseDay - day;
                date = DateTime.Now.AddDays(interval);
            }
            else
            {
                int interval = 7 - (day - courseDay);
                date = DateTime.Now.AddDays(interval);
            }
            //計算上課時間
            string courseTime = course.FAvailableTimeNum.ToString().Substring(1, course.FAvailableTimeNum.ToString().Length - 1);
            if (courseTime.Length < 2)
                courseTime = $"0{courseTime}00";
            else
                courseTime = $"{courseTime}00";

            //加入排課
            for(int i = 0; i < course.FCourseTotal; i++)
            {
                TReservation reservation = new TReservation()
                {
                    FCourseId=course.FCourseId,
                    FCourseTime=date.ToString("yyyyMMdd")+courseTime,
                    FStatusNumber=60
                };
                db.TReservations.Add(reservation);
                date = date.AddDays(7);
            }

            //增加教練課程數量
            var courseCount = db.TCoaches.FirstOrDefault(c => c.FCoachId == course.FCoachContact.FCoachId).FCourseCount;
            db.TCoaches.FirstOrDefault(c => c.FCoachId == course.FCoachContact.FCoachId).FCourseCount = courseCount + 1;
            
            db.SaveChanges();

            return Content("");
        } 
        //教練專區-招生紀錄:依聯繫時間排序
        public IActionResult loadContact(int? flag, int? statusNum)    
        {
            //取得登入者ID
            int theCoachMemberId = 11;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                theCoachMemberId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }

            IEnumerable<TCoachContact> contacts = null;
            if (statusNum == 0)
            {
                if (flag == 1)
                {
                    contacts = db.TCoachContacts.Where(c => c.FCoach.FMemberId == theCoachMemberId).OrderByDescending(c => c.FContactDate);
                }
                else
                {
                    contacts = db.TCoachContacts.Where(c => c.FCoach.FMemberId == theCoachMemberId).OrderBy(c => c.FContactDate);
                }
            }
            else
            {
                if (flag == 1)
                {
                    contacts = db.TCoachContacts.Where(c => c.FCoach.FMemberId == theCoachMemberId && c.FStatusNumber == statusNum).OrderByDescending(c => c.FContactDate);
                }
                else
                {
                    contacts = db.TCoachContacts.Where(c => c.FCoach.FMemberId == theCoachMemberId && c.FStatusNumber == statusNum).OrderBy(c => c.FContactDate);
                }
            }
            List<CContactViewModel> contactViewModels = new List<CContactViewModel>();
            foreach (var c in contacts)
            {
                CContactViewModel contactViewModel = new CContactViewModel(db)
                {
                    FCoachContactId = c.FCoachContactId,
                    FMemberId = c.FMemberId,
                    FStatusNumber = c.FStatusNumber,
                    FAvailableTimeNum = c.FAvailableTimeNum
                };
                contactViewModels.Add(contactViewModel);
            }
            return Json(contactViewModels);
        }
        //教練專區-招生紀錄:顯示訊息
        public IActionResult loadChatText(int? id)
        {
            var tContactTexts = db.TContactTexts.Where(t => t.FCoachContactId == id).OrderBy(t => t.FContactTextTime);
            List<CContactTextViewModel> texts = new List<CContactTextViewModel>();
            if (tContactTexts.Count() != 0)
            {
                foreach (var t in tContactTexts)
                {
                    CContactTextViewModel contactTextViewModel = new CContactTextViewModel(db);
                    contactTextViewModel.TcontactText = t;
                    texts.Add(contactTextViewModel);
                }
            }
            return Json(texts);
        }
        //教練專區-招生紀錄:傳送訊息
        public IActionResult saveText(TContactText contactText)
        {
            contactText.FContactTextTime = DateTime.Now.ToString("yyyyMMddHHmm");
            contactText.FIsCoach = true;
            db.TContactTexts.Add(contactText);
            db.SaveChanges();
            return Content("");
        }

    }
}
