using prjiHealth.Models;
using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiHealth.ViewModels
{
    public class CBodyRecordViewModel
    {
        public CBodyRecordViewModel() { }

        private IHealthContext db;
        public CBodyRecordViewModel(IHealthContext context)
        {
            db = context;
        }

        public int FBodyRecordId { get; set; }
        public int? FMemberId { get; set; }
        public string FRecordDate { get; set; }
        public double? FWorkload { get; set; }
        public double? FHeight { get; set; }
        public double? FWeight { get; set; }
        public virtual TMember FMember { get; set; }
        private int? age;
        public int? Age
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
                    return age;
            }
            set
            {
                age = value;
            }
        }
        private int? gender;
        public int? Gender 
        { 
            get 
            {
                if (FMemberId != null)
                {
                    var theMember = db.TMembers.FirstOrDefault(m => m.FMemberId == FMemberId);
                    return ((bool)(theMember.FGender))?1:0;
                }
                else
                    return gender;
            }
            set
            {
                gender = value;
            } 
        }
        public double NumBMI
        {
            get
            {
                return Math.Round((double)FWeight / Math.Pow((double)(FHeight / 100), 2),1);
            }
        }
        public double NumFat
        {
            get
            {
                if (Age != null&& Gender!=null) 
                { 
                double numBMI = (double)FWeight / Math.Pow((double)(FHeight / 100), 2);
                return Math.Round(1.2 * numBMI + (0.23 * (double)Age) - 5.4 - (10.8 * (double)Gender), 2);
                }
                else
                {
                    return 0;
                }
            }
        }
        public double NumTDEE
        {
            get
            {
                if (Age != null && Gender != null)
                {
                    if (Gender == 1)
                    {
                        return Math.Round((((10 * (double)FWeight) + (6.25 * (double)FHeight) - (5 * (double)Age) + 5) * (double)FWorkload), 0);
                    }
                    else
                    {
                        return Math.Round((((10 * (double)FWeight) + (6.25 * (double)FHeight) - (5 * (double)Age) - 161) * (double)FWorkload), 0);
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
