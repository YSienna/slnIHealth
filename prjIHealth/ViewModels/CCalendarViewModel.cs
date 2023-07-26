using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CCalendarViewModel
    {
        public TReservation Reservation;
        public CCalendarViewModel(TReservation r)
        {
            Reservation = r;
        }
        static public List<CCalendarViewModel> ReservationList(List<TReservation> reservations)
        {
            List<CCalendarViewModel> list = new List<CCalendarViewModel>();
            foreach(var r in reservations)
            {
                CCalendarViewModel vModel = new CCalendarViewModel(r);
                list.Add(vModel);
            }
            return list;
        }
        public int FReservationId
        {
            get { return Reservation.FReservationId; }
        }
        public int? FCourseId
        {
            get { return Reservation.FCourseId; }
        }
        public string FCourseTime
        {
            get { return Reservation.FCourseTime; }
            set { Reservation.FCourseTime = value; }
        }
        public int MemberId
        {
            get { return (int)Reservation.FCourse.FCoachContact.FMemberId; }
            set { Reservation.FCourse.FCoachContact.FMemberId = value; }
        }
        public string MemberName
        {
            get { return Reservation.FCourse.FCoachContact.FMember.FMemberName; }
            set { Reservation.FCourse.FCoachContact.FMember.FMemberName = value; }
        }
        public int CoachId
        {
            get { return (int)Reservation.FCourse.FCoachContact.FCoachId; }
            set { Reservation.FCourse.FCoachContact.FCoach.FCoachId = value; }
        }
        public string CoachName
        {
            get { return Reservation.FCourse.FCoachContact.FCoach.FCoachName; }
            set { Reservation.FCourse.FCoachContact.FCoach.FCoachName = value; }
        }
        public string CourseDate
        {
            get { return Reservation.FCourseTime.Substring(0,8); }
        }
        public string CourseTime
        {
            get { return Reservation.FCourseTime.Substring(8, 4); }
        }
        public int? FStatusNumber
        {
            get { return Reservation.FStatusNumber; }
            set { Reservation.FStatusNumber = value; }
        }
        public string Status
        {
            get { return Reservation.FStatusNumber == 60 ? "未完成" : "已完成"; }
        }
    }
}
