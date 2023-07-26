using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TCoachLicense
    {
        public int FLicenseId { get; set; }
        public int? FCoachId { get; set; }
        public string FLicense { get; set; }

        public virtual TCoach FCoach { get; set; }
    }
}
