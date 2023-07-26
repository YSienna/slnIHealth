using Microsoft.EntityFrameworkCore;
using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CContactTextViewModel
    {
        private IHealthContext db;
        public CContactTextViewModel(IHealthContext context)
        {
            db = context;
        }
        public TContactText TcontactText;
        public bool? FIsCoach 
        {
            get
            {
                if (TcontactText != null)
                    return TcontactText.FIsCoach;
                else
                    return null;
            }
        }
        public string FContactText 
        {
            get
            {
                if (TcontactText != null)
                    return TcontactText.FContactText;
                else
                    return null;
            }
        }
        public string CoachName
        {
            get
            {
                if ((bool)FIsCoach)
                {
                    return db.TCoachContacts.Include(c => c.FCoach).FirstOrDefault(c => c.FCoachContactId == TcontactText.FCoachContactId).FCoach.FCoachName;
                }
                else
                    return null;
            }
        }
        public string MemberName
        {
            get
            {
                if (!(bool)FIsCoach)
                {
                    return db.TCoachContacts.Include(c => c.FMember).FirstOrDefault(c => c.FCoachContactId == TcontactText.FCoachContactId).FMember.FMemberName;
                }
                else
                    return null;
            }
        }
        public string Time
        {
            get
            {
                if (TcontactText.FContactTextTime != null)
                {
                    string time = TcontactText.FContactTextTime;
                    return $"{time.Substring(8,2)}:{time.Substring(10,2)}";
                }
                else
                    return null;
            }
        }
    }
}
