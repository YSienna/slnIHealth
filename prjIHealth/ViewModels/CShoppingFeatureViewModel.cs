using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CShoppingFeatureViewModel
    {
        public int? categoryID { get; set; }
        public string sort { get; set; }
        public string url { get; set; }
        public int page { get; set; }
        public string txtKeyword { get; set; }
        //public int minPrice { get; set; }
        //public int maxPrice { get; set; }
    }
}
