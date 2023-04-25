using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Order
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string OrderStatu { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public string PaymentId { get; set; }
        public string ConverstionId { get; set; }
       
        public DateTime OrderDate { get; set; }
        public OrderStateEnum StateEnumOrder { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }

    public enum OrderStateEnum
    {
        waitig=0,
        unpaid=1,
        complated=2
    }
}
