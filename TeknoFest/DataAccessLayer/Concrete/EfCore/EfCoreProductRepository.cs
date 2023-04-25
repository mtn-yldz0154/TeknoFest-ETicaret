using DataAccessLayer.Abstract;
using EntityLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product, Context>, IProductRepository
    {
        Context context = new Context();

        public void Edit(Product product)
        {
                var entity=context.Products.Include(a=>a.Category).Where(i=>i.ProductId==product.ProductId).FirstOrDefault();

                entity.ProductDespcription = product.ProductDespcription;
                entity.ProductPrice = product.ProductPrice;
                entity.ProductStock = product.ProductStock;
                entity.ProductName = product.ProductName;
                entity.ProductLongName = product.ProductLongName;
                entity.CategoryId = product.CategoryId;
                entity.ProductOzellik = product.ProductOzellik;

                


            
        }
    }
}
