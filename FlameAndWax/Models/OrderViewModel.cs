using System;
using System.Collections.Generic;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public double TotalCost { get; set; }
        public ModeOfPayment ModeOfPayment { get; set; }
        public Courier Courier { get; set; }
   
        public OrderStatus Status { get; set; }
        public IEnumerable<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
