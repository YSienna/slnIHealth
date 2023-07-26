using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TCalorieIntake
    {
        public int FCalorieIntakeId { get; set; }
        public string FIntakeTime { get; set; }
        public int? FMemberId { get; set; }
        public int? FFoodId { get; set; }
        public double? FQuantity { get; set; }
        public string FMeal { get; set; }
        public string FRemarks { get; set; }

        public virtual TFoodCalory FFood { get; set; }
        public virtual TMember FMember { get; set; }
    }
}
