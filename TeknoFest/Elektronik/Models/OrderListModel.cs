using EntityLayer;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ElektronikWebUI.Models
{
    public class OrderListModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStateEnum StateEnumOrder { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }

        public double TotalPrice()
        {
            return OrderItems.Sum(i=>i.Price*i.Quantity);
        }
    }

    public class OrderItemModel
    {
        public int OrderItemId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public string Ozellik { get; set; }
        public int Quantity { get; set; }
    }
}
