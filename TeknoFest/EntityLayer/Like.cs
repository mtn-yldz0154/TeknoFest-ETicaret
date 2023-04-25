using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Like
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<LikeItem> LikeItems { get; set; }


    }
}
