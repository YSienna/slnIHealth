using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TFoodCalory
    {
        public TFoodCalory()
        {
            TCalorieIntakes = new HashSet<TCalorieIntake>();
        }

        public int FFoodId { get; set; }
        public string FFoodName { get; set; }
        public string FUnit { get; set; }
        public double? FCalories { get; set; }

        public virtual ICollection<TCalorieIntake> TCalorieIntakes { get; set; }
    }
}
