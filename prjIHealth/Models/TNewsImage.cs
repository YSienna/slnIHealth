using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TNewsImage
    {
        public int FNewsImageId { get; set; }
        public string FImagePath { get; set; }
        public int? FNewsId { get; set; }

        public virtual TNews FNews { get; set; }
    }
}
