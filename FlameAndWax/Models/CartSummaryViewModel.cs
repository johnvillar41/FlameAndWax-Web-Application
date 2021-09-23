using FlameAndWax.Data.Models;

namespace FlameAndWax.Customer.Models
{
    public class CartSummaryViewModel : CartViewModel
    {
        public double TotalCost { get; set; }
        public CustomerViewModel Customer { get; set; }
        public ShippingAddressViewModel ShippingAddress { get; set; }
        public CartSummaryViewModel(
            double totalCost,
            CartViewModel cartViewModel,
            ShippingAddressModel shippingAddressModel) : base(cartViewModel.ModeOfPayment, cartViewModel.Courier, cartViewModel.CartProducts)
        {
            TotalCost = totalCost;
            ShippingAddress = new ShippingAddressViewModel(shippingAddressModel);
        }
    }
}