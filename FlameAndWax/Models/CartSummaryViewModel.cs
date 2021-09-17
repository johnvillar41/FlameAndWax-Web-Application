using System.Collections.Generic;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Models
{
    public class CartSummaryViewModel : CartViewModel
    {
        public double TotalCost { get; set; }
        public CartSummaryViewModel(
            double totalCost,
            CartViewModel cartViewModel) : base(cartViewModel.ModeOfPayment, cartViewModel.Courier, cartViewModel.CartProducts)
        {
            TotalCost = totalCost;
        }
    }
}