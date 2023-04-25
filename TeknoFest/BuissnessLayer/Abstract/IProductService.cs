using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnessLayer.Abstract
{
    public interface IProductService
    {
        void Create(Product entity);
        void Delete(int id);
        void Update(Product entity);
        Product GetById(int id);
        List<Product> GetAll();
        void Edit(Product product);
    }
}
