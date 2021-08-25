using FlameAndWax.Data.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public ModeOfPayment ModeOfPayment { get; set; }
        public Courier Courier { get; set; }
        public double TotalCost { get; set; }
        public Constants.OrderDetailStatus OrderStatus{ get; set; }

        public IEnumerable<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
