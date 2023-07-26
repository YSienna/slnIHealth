using Microsoft.EntityFrameworkCore;
using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CSearchCoachViewModel
    {

        public int? CityId { get; set; } //0
        public int? SkillId { get; set; } //0
        public string KeyWord { get; set; } //null
        public int? Sort { get; set; } //1:DESC 0:ORDERBY
        public int? StatusNum { get; set; } //0
        //public List<CCoachViewModel> Coaches
        //{
        //    get
        //    {
        //        IEnumerable<TCoach> tCoaches = null;
        //        if (SkillId != 0)
        //        {
        //            tCoaches = db.TCoachSkills.Include(s=>s.FCoach).Where(cs => cs.FSkillId == SkillId).Select(cs => cs.FCoach);
        //        }
        //        if (Sort == 1)
        //        {
        //            tCoaches = tCoaches.OrderByDescending(c => c.FApplyDate);
        //        }
        //        else if (Sort == 0)
        //        {
        //            tCoaches = tCoaches.OrderBy(c => c.FApplyDate);
        //        }
        //        if (CityId != 0)
        //        {
        //            tCoaches = tCoaches.Where(c => c.FCityId == CityId);
        //        }
        //        if (KeyWord!="null")
        //        {
        //            tCoaches = tCoaches.Where(c => c.FCoachName.Contains(KeyWord) || c.FCoachId.ToString().Contains(KeyWord));
        //        }
        //        if (StatusNum != 0)
        //        {
        //            tCoaches = tCoaches.Where(c => c.FStatusNumber == StatusNum);
        //        }

        //        List<CCoachViewModel> coaches = new List<CCoachViewModel>();
        //        foreach (var c in tCoaches)
        //        {
        //            CCoachViewModel coachViewModel = new CCoachViewModel(db)
        //            {
        //                Coach = c
        //            };
        //            coaches.Add(coachViewModel);
        //        }
        //        return coaches;
        //    }
        //}


    }
}
