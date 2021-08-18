using System.Collections.Generic;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Data.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public int QuantityPerUnit { get; set; }
        public double UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsInOrder { get; set; }
        public Category Category { get; set; }

        public IEnumerable<ProductGalleryModel> ProductGallery { get; set; }
    }
}
