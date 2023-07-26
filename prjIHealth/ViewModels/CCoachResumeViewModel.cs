using Microsoft.EntityFrameworkCore;
using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CCoachResumeViewModel
    {
        private IHealthContext db;
        public CCoachResumeViewModel(IHealthContext context)
        {
            db = context;
        }
        public int FCoachId { get; set; }
        public string FCoachName { get; set; }
        public int? FMemberId { get; set; }
        public int? FCityId { get; set; }
        public string FCoachImage { get; set; }
        public int? FCoachFee { get; set; }
        public string FCoachDescription { get; set; }
        public string FApplyDate { get; set; }
        public int? FStatusNumber { get; set; }
        public bool? FVisible { get; set; }
        public int? FCourseCount { get; set; }
        public string FSlogan { get; set; }
        public string CityName
        {
            get
            {
                if (FCityId != null)
                    return db.TCities.FirstOrDefault(c => c.FCityId == FCityId).FCityName;
                else
                    return null;

            }
        }
        public string ApplyDate
        {
            get
            {
                if (!String.IsNullOrEmpty(FApplyDate))
                {
                    string fApplyDate = FApplyDate;
                    string yyyy = fApplyDate.Substring(0, 4);
                    string MM = fApplyDate.Substring(4, 2);
                    string dd = fApplyDate.Substring(6, 2);
                    string hh = fApplyDate.Substring(8, 2);
                    string mm = fApplyDate.Substring(10, 2);
                    return $"{yyyy}-{MM}-{dd} {hh}:{mm}";
                }
                else
                    return null;
            }
        }
        public string Status
        {
            get
            {
                if (FStatusNumber != null)
                    return db.TStatuses.FirstOrDefault(s => s.FStatusNumber == FStatusNumber).FStatus;
                else
                    return null;
            }
        }
        public string Visible
        {
            get
            {
                if (FVisible != null)
                {
                    if ((bool)(FVisible))
                        return "公開";
                    else
                        return "未公開";
                }
                else
                    return null;
            }
        }

        public IEnumerable<string> Skills
        {
            get
            {
                if (FCoachId != null && FCoachId != 0)
                    return db.TCoachSkills.Where(cs => cs.FCoachId == FCoachId).Include(cs => cs.FSkill).Select(cs => cs.FSkill.FSkillName);
                else
                    return null;
            }
        }
        public IEnumerable<string> Experiences
        {
            get
            {
                if (FCoachId != null && FCoachId != 0)
                    return db.TCoachExperiences.Where(e => e.FCoachId == FCoachId).Select(e => e.FExperience);
                else
                    return null;
            }
        }
        public IEnumerable<string> Licenses
        {
            get
            {
                if (FCoachId != null && FCoachId != 0)
                    return db.TCoachLicenses.Where(l => l.FCoachId == FCoachId).Select(l => l.FLicense);
                else
                    return null;
            }
        }
    }
}