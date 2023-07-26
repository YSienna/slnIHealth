
using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjiHealth.ViewModels;
using HealthyLifeApp;
using Microsoft.AspNetCore.Http;
using prjiHealth.Models;
using System.Text.Json;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using prjIHealth.ViewModels;
using X.PagedList;

namespace prjIHealth.Controllers
{
    public class MemberController : Controller
    {
        private readonly IHealthContext _context;
        private IWebHostEnvironment _environment;

        public MemberController(IHealthContext context, IWebHostEnvironment iwhe)
        {
            _context = context;
            _environment = iwhe;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(CLoginViewModel vModel)
        {
            if (vModel.fUserName == null || vModel.fPassword == null)
            {
                return Content("empty", "text/plain", System.Text.Encoding.UTF8);
            }

            var q = _context.TMembers.FirstOrDefault(tm => tm.FUserName == vModel.fUserName);
            if (q != null)
            {
                if (q.FPassword == utilities.getCryptPWD(vModel.fPassword, vModel.fUserName))
                {
                    string loginSession = JsonSerializer.Serialize(q);
                    HttpContext.Session.SetString(CDictionary.SK_Logined_User, loginSession);
                    TMember loginUser = JsonSerializer.Deserialize<TMember>(loginSession);
                    int authorId = (int)loginUser.FAuthorityId;
                    if (authorId <5)
                    {
                        string admin = "admin" + loginUser.FUserName;
                        return Content(admin, "text/plain", System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        return Content(loginUser.FUserName, "text/plain", System.Text.Encoding.UTF8);
                    }
                }
            }
            return Content("false", "text/plain", System.Text.Encoding.UTF8);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(CDictionary.SK_Logined_User);
            HttpContext.Session.Remove(CDictionary.SK_Shopped_Items);
            HttpContext.Session.Remove(CDictionary.SK_Third_Party_Payment);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Edit()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                var memberEdit = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                TMember loginUser = JsonSerializer.Deserialize<TMember>(memberEdit);
                var q = _context.TMembers.FirstOrDefault(m => m.FMemberId == loginUser.FMemberId);
                return View(q);
            }
            else
            {
                var q = _context.TMembers.FirstOrDefault(m => m.FMemberId == 8);
                return View(q);
            }
        }
        [HttpPost]
        public IActionResult Edit( CLoginViewModel vModel)
        {
            var q = _context.TMembers.FirstOrDefault(m => m.FMemberId == vModel.fMemberId);
            if (q != null)
            {
                if (vModel.photo != null)
                {
                    string pName = Guid.NewGuid().ToString() + ".jpg";
                    vModel.photo.CopyTo(new FileStream(_environment.WebRootPath + "/img/member/" + pName, FileMode.Create));
                    q.FPicturePath = pName;
                }
                q.FMemberName = vModel.fMemberName;
                q.FBirthday = vModel.fBirthday;
                q.FAddress = vModel.fAddress;
                q.FPhone = vModel.fPhone;
                q.FEmail = vModel.fEmail;
                q.FRemarks = vModel.fRemarks;
                q.FPhone = vModel.fPhone;
                _context.SaveChanges();
                return RedirectToAction("Edit", "Member");
            }
            else { return RedirectToAction("Edit", "Member");}
        
           
            
        }
        // GET: MemberRegister
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(CLoginViewModel vModel, TMember tm)
        {
            if (vModel.fPassword == null || vModel.fUserName == null || vModel.fEmail==null)
            {
                return Content("empty", "text/plain", System.Text.Encoding.UTF8);
            }
            else if (vModel.fPassword == vModel.confirmPassword)
            {
                var q = _context.TMembers.FirstOrDefault(m => m.FUserName == vModel.fUserName);
                if (q != null)
                {
                    return Content("user", "text/plain", System.Text.Encoding.UTF8);
                }
                else
                {
                    if (vModel.fMemberName == null) { tm.FMemberName = tm.FUserName; }
                    tm.FPassword = utilities.getCryptPWD(vModel.fPassword, vModel.fUserName);
                    _context.Add(tm);
                    _context.SaveChanges();
                    return Content("true", "text/plain", System.Text.Encoding.UTF8);
                }
            }
            else
            {
                return Content("FCerror", "text/plain", System.Text.Encoding.UTF8);
            }
        }
        public IActionResult getUserName([Bind("fUserName")] CLoginViewModel vModel)
        {
            var q = _context.TMembers.FirstOrDefault(m => m.FUserName == vModel.fUserName);
            if (q != null)
            {
                return Content("true", "text/plain", System.Text.Encoding.UTF8);
            }
            else
            {
                return Content("false", "text/plain", System.Text.Encoding.UTF8);
            }
        }
        public IActionResult getEmail([Bind("fEmail")] CLoginViewModel vModel)
        {
            if (vModel.fEmail != null)
            {
                var q = _context.TMembers.FirstOrDefault(m => m.FEmail == vModel.fEmail);
                if (q != null)
                {
                    return Content("true", "text/plain", System.Text.Encoding.UTF8);
                }
                else
                {
                    return Content("false", "text/plain", System.Text.Encoding.UTF8);
                }
            }
            else
            {
                return Content("false", "text/plain", System.Text.Encoding.UTF8);
            };
          
        }
        public IActionResult getPassword([Bind("fEmail,fPassword")] CLoginViewModel vModel)
        {
            var q = _context.TMembers.FirstOrDefault(m => m.FEmail == vModel.fEmail);
            if (vModel.fPassword != null && q != null)
            {
                if (q.FPassword == utilities.getCryptPWD(vModel.fPassword, q.FUserName))
                {
                    return Content("true", "text/plain", System.Text.Encoding.UTF8);
                }
                else
                {
                    return Content("false", "text/plain", System.Text.Encoding.UTF8);
                };
            }
            else
            {
                return Content("false2", "text/plain", System.Text.Encoding.UTF8);
            }
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword([Bind("fEmail,")] CLoginViewModel vModel)
        {
            if (vModel.fEmail == null) { return Content("empty", "text/plain", System.Text.Encoding.UTF8); }
            else
            {
                var q = _context.TMembers.FirstOrDefault(m => m.FEmail == vModel.fEmail);
                if (q != null)
                {
                    //============================================================= 
                    string newPassword = utilities.RandomString(8);
                    var DateAndTime = DateTime.Now.ToString("yyyy/MM/dd: HH:mm:ss");
                    utilities.sendMail(q.FUserName, newPassword, q.FEmail, DateAndTime);
                    q.FPassword = utilities.getCryptPWD(newPassword, q.FUserName);
                    _context.SaveChanges();
                    return Content(q.FUserName.ToString(), "text/plain", System.Text.Encoding.UTF8);
                }
                else
                {
                    return Content("false", "text/plain", System.Text.Encoding.UTF8);
                }
            }
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(CLoginViewModel vmodel)
        {
            if (vmodel.fEmail == null || vmodel.fPassword==null || vmodel.firstPassword==null)
            {
                return Content("empty", "text/plain", System.Text.Encoding.UTF8);
            }
            else
            {
                var q = _context.TMembers.FirstOrDefault(m => m.FEmail == vmodel.fEmail);
                if (q != null)
                {
                    if (q.FPassword == utilities.getCryptPWD(vmodel.fPassword, q.FUserName))
                    {
                        if (vmodel.firstPassword == vmodel.confirmPassword)
                        {
                            q.FPassword = utilities.getCryptPWD(vmodel.firstPassword, q.FUserName);
                            _context.SaveChanges();
                            return Content(q.FUserName.ToString(), "text/plain", System.Text.Encoding.UTF8);
                        }
                        else { return Content("ConfirmPasswordError", "text/plain", System.Text.Encoding.UTF8); }
                    }
                    else
                    {
                        return Content("PasswordError", "text/plain", System.Text.Encoding.UTF8);
                    }
                }
                else
                {
                    return Content("false", "text/plain", System.Text.Encoding.UTF8);
                }
            }
        }
       
        //========================追蹤清單===========================    

        public IActionResult ShowTrackList()
        {
            return View();
        }
        public IActionResult ShowTrackList2()
        {
            return View();
        }

        public IActionResult ShowTrackProduct(int? id)//MemberID
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var showProducts = from a in _context.TTrackLists
                                   join b in _context.TProducts
                                   on a.FProductId equals b.FProductId
                                   where a.FMemberId == id
                                   select b;
                return Json(showProducts);
            }
        }

        public IActionResult ShowTrackCount(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                int trackNum = _context.TTrackLists.Where(t => t.FMemberId == id).Select(t => t).Count();
                return Json(trackNum);
            }
        }

        public IActionResult DeleteTrackList(int? id) //ProductID
        {
            int userID = 8;//演示用ID
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                var memberEdit = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                TMember loginUser = JsonSerializer.Deserialize<TMember>(memberEdit);
                userID = loginUser.FMemberId;
            }
            var trackList = (from t in _context.TTrackLists
                             where t.FMemberId == /*1*/userID && t.FProductId == id
                             select t).FirstOrDefault();
            if (trackList != null)
            {
                _context.TTrackLists.Remove(trackList);
                _context.SaveChanges();
            }
            return RedirectToAction("ShowTrackList");
        }

        //public IActionResult Delete(int? id)
        //{
        //    IHealthContext dblIHealth = new IHealthContext();
        //    TTrackList trackList = dblIHealth.TTrackLists.FirstOrDefault(t => t.FProductId == id);
        //    if (trackList != null)
        //    {
        //        dblIHealth.TTrackLists.Remove(trackList);
        //        dblIHealth.SaveChanges();
        //    }
        //    return RedirectToAction("ShowTrackList");
        //}

        //===================購買紀錄=============================
        public IActionResult OrderList(int? page)
        {
            int userID = 8;//演示用ID
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                var memberEdit = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                TMember loginUser = JsonSerializer.Deserialize<TMember>(memberEdit);
                userID = loginUser.FMemberId;
            }
            var pro = (from o in _context.TOrders
                       join p in _context.TPaymentCategories
                       on o.FPaymentCategoryId equals p.FPaymentCategoryId
                       join s in _context.TStatuses
                       on o.FStatusNumber equals s.FStatusNumber
                       join m in _context.TMembers
                       on o.FMemberId equals m.FMemberId
                       where o.FMemberId == userID
                       select new COrderViewModel()
                       {
                           FOrderId = o.FOrderId,
                           FPaymentCategoryId = o.FPaymentCategoryId,
                           fPayment = o.FPaymentCategory,
                           FDate = o.FDate,
                           FAddress = o.FAddress,
                           FMemberId = userID,
                           fmember = o.FMember,
                           FContact = o.FContact,
                           FPhone = o.FPhone,
                           FRemarks = o.FRemarks,
                           FStatusNumber = o.FStatusNumber,
                           fstatus = o.FStatusNumberNavigation
                       }).OrderByDescending(a => a.FDate).ToList();
            var pageNumber = page ?? 1;
            var onePageOfPro = pro.ToPagedList(pageNumber, 5);
            ViewBag.onePageOfPro = onePageOfPro;
            return View(onePageOfPro);
        }
        public IActionResult OrderDetailList(int? id)
        {
            IHealthContext db = new IHealthContext();
            var odt = (from o in db.TOrderDetails
                       where o.FOrderId == id
                       //where o.FOrderDetailsId == id
                       join or in db.TOrders
                       on o.FOrderId equals or.FOrderId
                       join d in db.TDiscounts
                       on o.FDiscountId equals d.FDiscountId
                       join p in db.TProducts
                       on o.FProductId equals p.FProductId
                       select new COrderDetailViewModel()
                       {
                           FOrderId = o.FOrderId,
                           FOrderDetailsId = o.FOrderDetailsId,
                           FQuantity = o.FQuantity,
                           FUnitprice = o.FUnitprice,
                           fdiscount = o.FDiscount,
                           FDiscountId = o.FDiscountId,
                           FProductId = o.FProductId,
                           fproduct = o.FProduct,
                           forder=o.FOrder,
                           pay=or.FPaymentCategory,
                           sta=or.FStatusNumberNavigation
                       }).ToList();
            if (odt == null)
            {
                return RedirectToAction("OrderList");
            }
            return View(odt);
        }

    }
}

