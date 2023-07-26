using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CAddToCartViewModel
    {
        public int txtFid { get; set; }
        public int txtCount { get; set; }
        public int discountValue { get; set;}
        public int discountID { get; set; }
    }
}
