using EntityLayer;
using System.Collections.Generic;

namespace ElektronikWebUI.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public string ProductLongName { get; set; }
        public string ProductDespcription { get; set; }
        public string ProductImageUrl { get; set; }
        public string ProductImageUrl2 { get; set; }
        public string ProductImageUrl3 { get; set; }
        public string ProductSmallImageUrl { get; set; }
        public double ProductPrice { get; set; }
        public double SaleProductPrice { get; set; }
        public int ProductStock { get; set; }
        public int StarNumber { get; set; }
        public string ProductOzellik { get; set; }
        public int CategoryId { get; set; }
        public List<Product> BenzerUrunler { get; set; }
        public List<Product> OnerilenUrunler { get; set; }
        public List<Yorum> Yorums { get; set; }

        public Category Category { get; set; }
    }
}
