using FlameAndWax.Data.Constants;
using System.Collections.Generic;

namespace FlameAndWax.Models
{
    public class CartViewModel
    {
        public double TotalCost { get; set; }
        public Constants.ModeOfPayment ModeOfPayment { get; set; }
        public Constants.Courier Courier { get; set; }

        public List<ProductViewModel> CartProducts { get; set; }
    }
}
