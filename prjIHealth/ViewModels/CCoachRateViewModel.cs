using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CCoachRateViewModel
    {
        private IHealthContext db;
        public CCoachRateViewModel(IHealthContext context)
        {
            db = context;
        }
        public int FRateId { get; set; }
        public int? FMemberId { get; set; }
        public string MemberName
        {
            get
            {
                return db.TMembers.FirstOrDefault(m => m.FMemberId == FMemberId).FMemberName;
            }
        }
        public int? FCoachId { get; set; }
        public string CoachName
        {
            get
            {
                var coachMemberId = db.TCoaches.FirstOrDefault(c => c.FCoachId == FCoachId).FMemberId;
                return db.TMembers.FirstOrDefault(m => m.FMemberId == coachMemberId).FMemberName;
            }
        }
        public int? FRateStar { get; set; }
        public string FRateText { get; set; }
        public string FRateDate { get; set; }
        public string RateDate
        {
            get
            {
                string yyyy = FRateDate.Substring(0, 4);
                string MM = FRateDate.Substring(4, 2);
                string dd = FRateDate.Substring(6, 2);
                return $"{yyyy}年{MM}月{dd}日";
            }
        }
        public bool? FVisible { get; set; }
        public string Visible
        {
            get
            {
                if ((bool)FVisible)
                    return "是";
                else
                    return "否";
            }
        }
    }
}
