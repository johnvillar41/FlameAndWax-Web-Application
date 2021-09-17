using System.Collections.Generic;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Models
{
    public class CartSummaryViewModel : CartViewModel
    {
        public double TotalCost { get; set; }
        public CartSummaryViewModel(
            double totalCost,
            ModeOfPayment modeOfPayment,
            Courier courier,
            List<ProductViewModel> cartProducts) : base(modeOfPayment, courier, cartProducts)
        {
            TotalCost = totalCost;
        }
    }
}