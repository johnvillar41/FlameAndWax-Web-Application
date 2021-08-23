using FlameAndWax.Data.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
