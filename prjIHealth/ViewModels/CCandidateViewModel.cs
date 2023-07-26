using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CCandidateViewModel
    {
        private IHealthContext db;
        public CCandidateViewModel(IHealthContext context)
        {
            db = context;
        }
        public int FCandidateId { get; set; }
        public int FCoachId { get; set; }
        public string CoachImage 
        {
            get 
            {
                return db.TCoaches.FirstOrDefault(c => c.FCoachId == FCoachId).FCoachImage;
            }
        }
        public string CoachName
        {
            get
            {
                return db.TCoaches.FirstOrDefault(c => c.FCoachId == FCoachId).FCoachName;
            }
        }


    }
}
