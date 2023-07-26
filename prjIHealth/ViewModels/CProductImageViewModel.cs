using Microsoft.AspNetCore.Http;
using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CProductImageViewModel
    {
        public CProductImageViewModel()
        {
            _prod = new TProductsImage();
        }
        private TProductsImage _prod;
        public TProductsImage productimg
        {
            get { return _prod; }
            set { _prod = value; }
        }
        public int FProductImageId
        {
            get { return _prod.FProductImageId; }
            set { _prod.FProductImageId = value; }
        }
        public int FProductId
        {
            get { return _prod.FProductId; }
            set { _prod.FProductId = value; }
        }
        public string FImage
        {
            get { return _prod.FImage; }
            set { _prod.FImage = value; }
        }
        public IFormFile photo { get; set; }
        public TProduct product { get; set; }
    }
}
