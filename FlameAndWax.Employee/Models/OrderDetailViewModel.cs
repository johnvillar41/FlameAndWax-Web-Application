using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Employee.Models
{
    public class OrderDetailViewModel
    {
        public int OrderDetailsId { get; set; }
        public int OrderId { get; set; }
        public ProductViewModel Product { get; set; }
        public double TotalPrice { get; set; }
        public int Quantity { get; set; }
        public OrderStatus Status { get; set; }
        public OrderDetailViewModel(OrderDetailModel orderDetailModel)
        {
            OrderDetailsId = orderDetailModel.OrderDetailsId;
            OrderId = orderDetailModel.OrderId;
            Product = new ProductViewModel(orderDetailModel.Product);
            TotalPrice = orderDetailModel.TotalPrice;
            Quantity = orderDetailModel.Quantity;
            Status = orderDetailModel.Status;
        }
        public OrderDetailViewModel()
        {
            
        }
    }
}