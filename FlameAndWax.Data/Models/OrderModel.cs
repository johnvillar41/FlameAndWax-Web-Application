using System;
using System.Collections.Generic;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Data.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public CustomerModel Customer { get; set; }
        public EmployeeModel Employee { get; set; }        
        public DateTime DateOrdered { get; set; }
        public double TotalCost { get; set; }
        public ModeOfPayment ModeOfPayment { get; set; }
        public Courier Courier { get; set; }       

        public OrderStatus Status { get; set; }
        public IEnumerable<OrderDetailModel> OrderDetails { get; set; }
    }
}
