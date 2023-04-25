using BuissnessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnessLayer.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductRepository _repository; 
        public ProductManager(IProductRepository repository)
        {
            this._repository = repository;
        }

        public void Create(Product entity)
        {
            _repository.Create(entity);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public void Edit(Product product)
        {
            _repository.Edit(product);
        }

        public List<Product> GetAll()
        {
            return _repository.GetAll();
        }

        public Product GetById(int id)
        {
          return  _repository.GetById(id);
        }

        public void Update(Product entity)
        {
          _repository.Update(entity);
        }
    }
}
