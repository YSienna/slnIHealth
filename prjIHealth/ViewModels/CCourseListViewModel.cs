using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CCourseListViewModel
    {
        private IHealthContext db;
        public CCourseListViewModel(IHealthContext context)
        {
            db = context;
        }
        public int FCourseId { get; set; }
        public int? FCoachContactId { get; set; }
        public int? CoachId
        {
            get
            {
                return db.TCoachContacts.FirstOrDefault(c => c.FCoachContactId == FCoachContactId).FCoachId;
            }
        }
        public string CoachName
        {
            get
            {
                return db.TCoaches.FirstOrDefault(c => c.FCoachId == CoachId).FCoachName;
            }
        }
        public string FCourseName { get; set; }
        public int? FCourseTotal { get; set; }
        //public int? CourseRemaining
        //{
        //    get
        //    {
        //        //TODO 計算已完成排課
        //        var finishCourseCount = 1;

        //        return FCourseTotal - finishCourseCount;
        //    }
        //}
        public int? FStatusNumber { get; set; }
        public string Status
        {
            get
            {
                return db.TStatuses.FirstOrDefault(s => s.FStatusNumber == FStatusNumber).FStatus;
            }
        }
        //public bool? FVisible { get; set; }
        public int? FAvailableTimeNum { get; set; }
        public string AvailableTime
        {
            get
            {
                return db.TAvailableTimes.FirstOrDefault(a => a.FAvailableTimeNum == FAvailableTimeNum).FAvailableTime;
            }
        }

        public IEnumerable<CReservationViewModel> Reservations
        {
            get
            {
                var tReservations = db.TReservations.Where(r => r.FCourseId == FCourseId);
                List<CReservationViewModel> reservations = new List<CReservationViewModel>();
                foreach (var r in tReservations)
                {
                    CReservationViewModel reservationViewModel = new CReservationViewModel(db)
                    {
                        FReservationId = r.FReservationId,
                        FCourseId = r.FCourseId,
                        FCourseTime = r.FCourseTime,
                        FStatusNumber = r.FStatusNumber
                    };
                    reservations.Add(reservationViewModel);
                }
                return reservations;
            }
        }

    }
}
