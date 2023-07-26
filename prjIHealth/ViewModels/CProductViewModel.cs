using Microsoft.AspNetCore.Http;
using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CProductViewModel
    {
        public CProductViewModel()
        {
            _prod = new TProduct();
            //TOrderDetails = new HashSet<TOrderDetail>();
            //TProductsImages = new HashSet<TProductsImage>();
            //TTrackLists = new HashSet<TTrackList>();
        }
        private TProduct _prod;

        IHealthContext db = new IHealthContext();

        public IEnumerable<TProduct> ProductList
        {
            get
            {
                var pList = db.TProducts.OrderBy(c => c.FProductId);
                return pList;
            }
        }



        public int FProductId
        {
            get { return _prod.FProductId; }
            set { _prod.FProductId = value; }
        }

        [DisplayName("品名")]
        public string FProductName
        {
            get { return _prod.FProductName; }
            set { _prod.FProductName = value; }
        }
        [DisplayName("種類")]
        public int FCategoryId
        {
            get { return _prod.FCategoryId; }
            set { _prod.FCategoryId = value; }
        }

        [DisplayName("價錢")]
        public decimal FUnitprice
        {
            get { return _prod.FUnitprice; }
            set { _prod.FUnitprice = value; }
        }

        [DisplayName("描述")]
        public string FDescription
        {
            get { return _prod.FDescription; }
            set { _prod.FDescription = value; }
        }

        [DisplayName("上架")]
        public bool FVisible
        {
            get { return _prod.FVisible; }
            set { _prod.FVisible = value; }
        }

        public string FCoverImage
        {
            get { return _prod.FCoverImage; }
            set { _prod.FCoverImage = value; }
        }

        public IFormFile photo { get; set; }

        public TProductCategory FCategoryName { get; set; }

        public virtual TProductCategory FCategory { get; set; }
        public virtual ICollection<TOrderDetail> TOrderDetails { get; set; }
        public virtual ICollection<TProductsImage> TProductsImages { get; set; }
        public virtual ICollection<TTrackList> TTrackLists { get; set; }
    }
}
