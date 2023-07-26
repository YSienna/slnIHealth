using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CTeachingListViewModel
    {
        public TCourse Course;
        public CTeachingListViewModel()
        {
            Course = new TCourse();
        }
        public CTeachingListViewModel(TCourse c)
        {
            Course = c;
        }
        static public List<CTeachingListViewModel> CourseList(List<TCourse> courseList)
        {
            List<CTeachingListViewModel> list = new List<CTeachingListViewModel>();
            foreach (var c in courseList)
            {
                CTeachingListViewModel vModel = new CTeachingListViewModel(c);
                list.Add(vModel);
            }
            return list;
        }
        public int FCourseId
        {
            get { return Course.FCourseId; }
        }
        public int? FCoachContactId
        {
            get { return Course.FCoachContactId; }
            set { Course.FCoachContactId = value; }
        }
        public int? MemberId
        {
            get { return Course.FCoachContact.FMember.FMemberId; }
            set { Course.FCoachContact.FMember.FMemberId = (int)value; }
        }
        [DisplayName("學員")]
        public string MemberName
        {
            get { return Course.FCoachContact.FMember.FMemberName; }
            set { Course.FCoachContact.FMember.FMemberName = value; }
        }
        public string MemberPhone
        {
            get { return Course.FCoachContact.FMember.FPhone; }
            set { Course.FCoachContact.FMember.FPhone = value; }
        }
        [DisplayName("堂數")]
        public int? FCourseTotal
        {
            get { return Course.FCourseTotal; }
            set { Course.FCourseTotal = value; }
        }
        [DisplayName("課程名稱")]
        public string FCourseName
        {
            get { return Course.FCourseName; }
            set { Course.FCourseName = value; }
        }
        [DisplayName("預設上課時段")]
        public int? FAvailableTimeNum
        {
            get { return Course.FAvailableTimeNum; }
            set { Course.FAvailableTimeNum = value; }
        }        
        public int? FStatusNumber
        {
            get { return Course.FStatusNumber; }
            set { Course.FStatusNumber = value; }
        }
        [DisplayName("狀態")]
        public string Status
        {
            get { return Course.FStatusNumber == 55 ? "進行中" : "已結束"; }
        }
        public bool? FVisible
        {
            get { return Course.FVisible; }
            set { Course.FVisible = value; }
        }

        //排課紀錄
        public List<CCalendarViewModel> Reservations
        {
            get
            {
                var reservations = Course.TReservations.AsEnumerable();
                List<CCalendarViewModel> list = new List<CCalendarViewModel>();
                foreach (var r in reservations)
                {
                    CCalendarViewModel vModel = new CCalendarViewModel(r)
                    {
                        Reservation = r
                    };
                    list.Add(vModel);
                }
                return list;
            }
        }
    }
}
