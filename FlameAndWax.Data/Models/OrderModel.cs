using System;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Data.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public CustomerModel Customer { get; set; }
        public EmployeeModel Employee { get; set; }
        public OrderDetailModel OrderDetails { get; set; }
        public DateTime Date { get; set; }
        public ModeOfPayment ModeOfPayment { get; set; }
        public Courier Courier { get; set; }
    }
}
