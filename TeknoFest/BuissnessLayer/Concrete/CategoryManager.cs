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
    public class CategoryManager : ICategoryService
    {
        private ICategoryRepository _repository;

        public CategoryManager(ICategoryRepository repository) { 
        
            _repository = repository;
        }
        public void Create(Category entity)
        {
           _repository.Create(entity);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public List<Category> GetAll()
        {
            return _repository.GetAll();
        }

        public Category GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Update(Category entity)
        {
           _repository.Update(entity);  
        }
    }
}
