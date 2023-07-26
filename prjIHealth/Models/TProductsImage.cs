using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TProductsImage
    {
        public int FProductImageId { get; set; }
        public int FProductId { get; set; }
        public string FImage { get; set; }

        public virtual TProduct FProduct { get; set; }
    }
}
