using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HealthyLifeApp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using prjiHealth.ViewModels;
using prjIHealth.Models;
using X.PagedList;

namespace prjIHealth.Areas.Admin.Controllers
{
    [Area("Admin")]
        public class MemberManageController : Controller
    {
        private IWebHostEnvironment _environment;
        private readonly IHealthContext _context;
        public MemberManageController(IHealthContext context, IWebHostEnvironment IWHE)
        {
            _context = context;
            _environment = IWHE;
        }

        // GET: Admin/MemberManage
        public IActionResult Index(int? page, CKeywordViewModel vModel)
        {
            IEnumerable<TMember> q = null;
            if (string.IsNullOrEmpty(vModel.txtKeyword))
            {
                q = _context.TMembers.Include(t => t.FAuthority).ToList();

                //q = _context.TMembers.Include(t => t.FAuthority);
            }
            else
            {
                q = _context.TMembers.Include(t => t.FAuthority).Where(m => m.FUserName.Contains(vModel.txtKeyword)
                   || m.FMemberName.Contains(vModel.txtKeyword) || m.FEmail.Contains(vModel.txtKeyword)||m.FPhone.Contains(vModel.txtKeyword)).ToList();
            }
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfMembers = q.ToPagedList(pageNumber, 11); // will only contain 6 items max because of the pageSize
            ViewBag.onePageOfMembers = onePageOfMembers;
            return View(onePageOfMembers);
        }

        // GET: Admin/MemberManage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tMember = await _context.TMembers
                .Include(t => t.FAuthority)
                .FirstOrDefaultAsync(m => m.FMemberId == id);
            if (tMember == null)
            {
                return NotFound();
            }

            return View(tMember);
        }

        // GET: Admin/MemberManage/Create
        public IActionResult Create()
        {
            ViewData["FAuthorityId"] = new SelectList(_context.TAuthorities, "FAutorityId", "FAuthorityName");
            return View();
        }

        // POST: Admin/MemberManage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FMemberId,FMemberName,FPassword,FBirthday,FGender,FPicturePath,FUserName,FAddress,FPhone,FEmail,FRegisterDate,FAuthorityId,FDisabled,FRemarks")] TMember tMember)
        {
            if (ModelState.IsValid)
            {
                var password = utilities.RandomString(8);
                var DateAndTime = DateTime.Now.ToString("yyyy-MM-dd: HH:mm:ss");
                utilities.sendMail(tMember.FUserName, password, tMember.FEmail, DateAndTime);
                tMember.FPassword = utilities.getCryptPWD(password, tMember.FUserName);
                _context.Add(tMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FAuthorityId"] = new SelectList(_context.TAuthorities, "FAutorityId", "FAuthorityName", tMember.FAuthorityId);
            return View(tMember);
        }

        // GET: Admin/MemberManage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tMember = await _context.TMembers.FindAsync(id);
            if (tMember == null)
            {
                return NotFound();
            }
            ViewData["FAuthorityId"] = new SelectList(_context.TAuthorities, "FAutorityId", "FAuthorityName", tMember.FAuthorityId);
            return View(tMember);
        }

        // POST: Admin/MemberManage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("FMemberId,FMemberName,FPassword,FBirthday,FGender,FPicturePath,FUserName,FAddress,FPhone,FEmail,FRegisterDate,FAuthorityId,FDisabled,FRemarks")] TMember tMember)
        public IActionResult Edit(CLoginViewModel vModel)
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
                if (vModel.fPassword == null) { q.FPassword = utilities.getCryptPWD("jay0513@1388", q.FUserName); }
                q.FMemberName = vModel.fMemberName;
                q.FBirthday = vModel.fBirthday;
                q.FAddress = vModel.fAddress;
                q.FPhone = vModel.fPhone;
                q.FEmail = vModel.fEmail;
                q.FRemarks = vModel.fRemarks;
                q.FPhone = vModel.fPhone;
                q.FGender = vModel.FGender;
                q.FDisabled = vModel.FDisabled;
                q.FAuthorityId = vModel.FAuthorityId;

            }
            _context.SaveChanges();
            return RedirectToAction("Index", "MemberManage");
        }

        // GET: Admin/MemberManage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tMember = await _context.TMembers
                .Include(t => t.FAuthority)
                .FirstOrDefaultAsync(m => m.FMemberId == id);
            if (tMember == null)
            {
                return NotFound();
            }
            //return Content(id.ToString(), "text/plain", System.Text.Encoding.UTF8);

            return View(tMember);
        }

        // POST: Admin/MemberManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TMember tm)
        {
            try
            {
                var tMember = await _context.TMembers.FindAsync(tm.FMemberId);
                _context.TMembers.Remove(tMember);
                await _context.SaveChangesAsync();
                return Content("true", "text/plain", System.Text.Encoding.UTF8);
            }
            catch (Exception e)
            {
                return Content("false", "text/plain", System.Text.Encoding.UTF8);
            }

        }

        private bool TMemberExists(int id)
        {
            return _context.TMembers.Any(e => e.FMemberId == id);
        }
    }
}
