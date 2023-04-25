using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Yorum
    {
        public int Id { get; set; }
        public string CommentTitle { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public string UserPP { get; set; }
        public DateTime UserDate { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; }

    }
}
