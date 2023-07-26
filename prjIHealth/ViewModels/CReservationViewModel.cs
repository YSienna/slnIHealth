using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CReservationViewModel
    {
        private IHealthContext db;
        public CReservationViewModel(IHealthContext context)
        {
            db = context;
        }
        public int FReservationId { get; set; }
        public int? FCourseId { get; set; }
        public string FCourseTime { get; set; }
        public int? FStatusNumber { get; set; }
        public string Status
        {
            get
            {
                return db.TStatuses.FirstOrDefault(s => s.FStatusNumber == FStatusNumber).FStatus;
            }
        }
    }
}
