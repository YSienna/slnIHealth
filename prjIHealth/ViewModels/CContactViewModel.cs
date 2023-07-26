using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CContactViewModel
    {
        private IHealthContext db;
        public CContactViewModel(IHealthContext context)
        {
            db = context;
        }

        public int? FCoachContactId { get; set; }
        public int? FMemberId { get; set; }
        public string MemberName
        {
            get
            {
                if (FMemberId != null)
                    return db.TMembers.FirstOrDefault(m => m.FMemberId == FMemberId).FMemberName;
                else
                    return null;
            }
        }
        public string MemberGender
        {
            get
            {
                if (FMemberId != null)
                {
                    if ((bool)(db.TMembers.FirstOrDefault(m => m.FMemberId == FMemberId).FGender))
                        return "男";
                    else
                        return "女";
                }
                else
                    return null;
            }
        }
        public int MemberAge
        {
            get
            {
                if (FMemberId != null)
                {
                    var theMember = db.TMembers.FirstOrDefault(m => m.FMemberId == FMemberId);
                    string theBirthday = $"{theMember.FBirthday.Substring(0, 4)}/{theMember.FBirthday.Substring(4, 2)}/{theMember.FBirthday.Substring(6, 2)}";
                    DateTime bday = DateTime.Parse(theBirthday);
                    return (bday > DateTime.Today.AddYears(-(DateTime.Today.Year - bday.Year))) ? DateTime.Today.Year - bday.Year - 1 : DateTime.Today.Year - bday.Year;
                }
                else
                    return 0;
            }
        }
        public string MemberPicturePath
        {
            get
            {
                if (FMemberId != null)
                    return db.TMembers.FirstOrDefault(m => m.FMemberId == FMemberId).FPicturePath;
                else
                    return null;
            }
        }
        public string MemberPhone
        {
            get
            {
                if (FMemberId != null)
                    return db.TMembers.FirstOrDefault(m => m.FMemberId == FMemberId).FPhone;
                else
                    return null;
            }
        }
        public string MemberEmail
        {
            get
            {
                if (FMemberId != null)
                    return db.TMembers.FirstOrDefault(m => m.FMemberId == FMemberId).FEmail;
                else
                    return null;
            }
        }
        public int? FCoachId { get; set; }
        public string CoachName 
        {
            get 
            {
                if (FCoachId != null)
                {
                    return db.TCoaches.FirstOrDefault(c => c.FCoachId == FCoachId).FCoachName;
                }
                else
                    return null;
            } 
        }
        public string ContactDate
        {
            get
            {
                if (FCoachContactId != null)
                    return db.TCoachContacts.FirstOrDefault(c => c.FCoachContactId == FCoachContactId).FContactDate;
                else
                    return null;
            }
        }
        public string StrContactDate 
        {
            get
            {
                if (FCoachContactId != null)
                {
                    var dateTime = db.TCoachContacts.FirstOrDefault(c => c.FCoachContactId == FCoachContactId).FContactDate;
                    string yyyy = dateTime.Substring(0, 4);
                    string MM = dateTime.Substring(4, 2);
                    string dd = dateTime.Substring(6, 2);
                    string hh = dateTime.Substring(8, 2);
                    string mm = dateTime.Substring(10, 2);
                    //string ss = dateTime.Substring(12, 2);
                    return $"{yyyy}-{MM}-{dd}  {hh}:{mm}";
                }
                else
                    return null;
            } 
        }
        public int? FStatusNumber { get; set; }
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

        public int? FAvailableTimeNum { get; set; }

        public string AvailableTime
        {
            get
            {
                if (FAvailableTimeNum != null)
                    return db.TAvailableTimes.FirstOrDefault(a => a.FAvailableTimeNum == FAvailableTimeNum).FAvailableTime;
                else
                    return null;
            }
        }

    }
}
