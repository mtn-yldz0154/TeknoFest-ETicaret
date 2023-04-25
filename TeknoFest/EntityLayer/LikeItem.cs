using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class LikeItem
    {
        public int Id { get; set; }
        public Like Like { get; set; }
        public int LikeId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
       
    }
}
