﻿using System.Collections.Generic;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Models
{
    public class CartViewModel
    {
        public ModeOfPayment ModeOfPayment { get; set; }
        public Courier Courier { get; set; }
        public IList<ProductViewModel> CartProducts { get; set; }
        public CartViewModel(ModeOfPayment modeOfPayment, Courier courier, IList<ProductViewModel> cartProducts)
        {
            ModeOfPayment = modeOfPayment;
            Courier = courier;
            CartProducts = cartProducts;
        }
        public CartViewModel()
        {

        }
    }
}
