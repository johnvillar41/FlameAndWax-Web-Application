using FlameAndWax.Data.Models;
using System.Linq;

namespace FlameAndWax.Customer.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductSubTotalPrice { get; set; }
        public double ProductPrice { get; set; }
        public string PhotoLink { get; set; }
        public int StockQuantity { get; set; }
        public int QuantityPerUnit { get; set; }       
        public int QuantityOrdered { get; set; }
        public ProductViewModel(ProductModel productModel)
        {
            ProductId = productModel.ProductId;
            ProductName = productModel.ProductName;
            ProductDescription = productModel.ProductDescription;
            ProductPrice = productModel.ProductPrice;
            ProductSubTotalPrice = productModel.ProductPrice;
            PhotoLink = productModel.ProductGallery.FirstOrDefault().ProductPhotoLink;
            StockQuantity = (productModel.QuantityPerUnit * productModel.UnitsInStock) - productModel.UnitsInOrder;
            QuantityPerUnit = productModel.QuantityPerUnit;
            QuantityOrdered = 1;
        }
        public ProductViewModel()
        {

        }
    }
}
