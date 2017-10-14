using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineWebShop.Models;

namespace OnlineWebShop.Models
{
    public class DetailsModels
    {
        dbOnlineWebShopDataContext db = new dbOnlineWebShopDataContext();
        public int productID { get; set; }
        public string productName { get; set; }
        public int productPrice { get; set; }
        public string productImages { get; set; }
        public string producerName { get; set; }
        public int productQuantity { get; set; }
        public bool proStatus { get; set; }
        public int productWarranty { get; set; }
        public int producerID { get; set; }
        public DetailsModels(int id)
        {
            productID = id;
            Product product = db.Products.Single(x => x.ProductID == productID);// lấy đơn thông tin sản phẩm từ db 
            productName = product.Name;
            productImages = product.ProductImages;
            productPrice = Convert.ToInt32(product.Price);
            productWarranty = Convert.ToInt32(product.Warranty);
            producerID = product.ProducerID;
            proStatus = Convert.ToBoolean(product.Status);
            Producer nsx = db.Producers.Single(x => x.ProducerID == producerID);
            producerName = nsx.Name;
            // _unitPrice =Convert.ToDouble(from p in db.Details where p.ProductID==_productID select p.UnitPrice);// lấy đơn giá của sản phẩm trong Details

        }

    }
}