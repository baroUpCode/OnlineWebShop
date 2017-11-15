using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineWebShop.Models
{
    public class ShoppingCartModels
    {
        dbOnlineWebShopDataContext db = new dbOnlineWebShopDataContext();
        /// <summary>
        /// Giỏ hàng bao gồm các thuộc tính của sản phẩm, nhận giá trị sản phẩm truyền vào từ hàm ShoppingCarts
        /// </summary>
        ///
        public int _productID { get; set; }
        public string _productImages { get; set; }
        public string _productName { get; set; }
        public int _quantity { get; set; }
        public bool _status { get; set; }
        public Double _unitPrice { get; set; }
        public Double _total { get { return _quantity * _unitPrice; } }
        /// <summary>
        /// Lấy các thông tin sản phẩm (các biến từ View truyền vào) gán vào thuộc tính (Sản phẩm) của giỏ hàng 
        /// Class giỏ hàng được tạo ra khi cần nhận và hiển thị tạm thời các thông tin(biến từ View) truyền vào mà chưa cập nhật xuống 
        /// DB, và có thể cập nhật xuống DB khi Thanh Toán (Cập nhật sản phẩm về table Order- hóa đơn)
        /// </summary>
        /// <param name="proID"></param>
        public ShoppingCartModels(int modelID)
        {

            _productID = modelID;
           var price = from p in db.Details where p.ProductID == _productID select p.UnitPrice;
           Product product = db.Products.Single(x=>x.ProductID==_productID);// lấy đơn thông tin sản phẩm từ db 
            _productName= product.Name;
            _productImages= product.ProductImages;
            _quantity = 1;
            _status = Convert.ToBoolean(product.Status);
            //_unitPrice =Convert.ToDouble((from p in db.Details where p.ProductID==_productID select p.UnitPrice).ToString());// lấy đơn giá của sản phẩm trong Details
            _unitPrice = Convert.ToDouble(product.Price);

        }
    }
    
}