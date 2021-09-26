using FlameAndWax.Data.Models;
using FlameAndWax.Employee.Controllers;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Employee.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public CustomerViewModel Customer { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public double TotalCost { get; set; }
        public ModeOfPayment ModeOfPayment { get; set; }
        public Courier Courier { get; set; }
        public OrderStatus Status { get; set; }
        public OrderViewModel(OrderModel orderModel)
        {
            OrderId = orderModel.OrderId;
            Customer = new CustomerViewModel(orderModel.Customer);
            Employee = new EmployeeViewModel(orderModel.Employee);
            TotalCost = orderModel.TotalCost;
            ModeOfPayment = orderModel.ModeOfPayment;
            Courier = orderModel.Courier;
            Status = orderModel.Status;
        }
        public OrderViewModel()
        {
            
        }
    }
}