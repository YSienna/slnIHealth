using Microsoft.AspNetCore.Http;
using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CCoachViewModel
    {
        public TCoach Coach;
        public CCoachViewModel()
        {
            Coach = new TCoach();
        }
        public CCoachViewModel(TCoach c)
        {
            Coach = c;
        }
        static public List<CCoachViewModel> CoachList(List<TCoach> coachList)
        {
            List<CCoachViewModel> list = new List<CCoachViewModel>();
            foreach(var c in coachList)
            {
                CCoachViewModel vModel = new CCoachViewModel(c);
                list.Add(vModel);
            }
            return list;
        }
        public int FCoachId
        {
            get { return Coach.FCoachId; }
        }
        public int? FMemberId
        {
            get { return Coach.FCoachId; }
            set { Coach.FCoachId = (int)value; }
        }
        public bool? Gender
        {
            get { return Coach.FMember.FGender; }
            set { Coach.FMember.FGender = value; }
        }
        public string FCoachName
        {
            get { return Coach.FCoachName; }
            set { Coach.FCoachName = value; }
        }
        public int? FCityId
        {
            get { return Coach.FCityId; }
            set { Coach.FCityId = value; }
        }
        public string CityName
        {
            get { return Coach.FCity.FCityName; }
            set { Coach.FCity.FCityName = value; }
        }
        public string FCoachImage
        {
            get { return Coach.FCoachImage; }
            set { Coach.FCoachImage = value; }
        }
        public int? FCoachFee
        {
            get { return Coach.FCoachFee; }
            set { Coach.FCoachFee = value; }
        }       
        public string FApplyDate
        {
            get { return Coach.FApplyDate; }
            set { Coach.FApplyDate = value; }
        }
        public int? FStatusNumber
        {
            get { return Coach.FStatusNumber; }
            set { Coach.FStatusNumber = value; }
        }
        public bool? FVisible
        {
            get { return Coach.FVisible; }
            set { Coach.FVisible = value; }
        }
        public int? FCourseCount
        {
            get { return Coach.FCourseCount; }
            set { Coach.FCourseCount = value; }
        }
        public string FCoachDescription
        {
            get { return Coach.FCoachDescription; }
            set { Coach.FCoachDescription = value; }
        }
        public string FSlogan
        {
            get { return Coach.FSlogan; }
            set { Coach.FSlogan = value; }
        }
        public int RateCount
        {
            get { return Coach.TCoachRates.Count(); }
        }        
    }
}
