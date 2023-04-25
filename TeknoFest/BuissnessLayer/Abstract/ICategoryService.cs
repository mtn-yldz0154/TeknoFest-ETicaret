using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnessLayer.Abstract
{
    public interface ICategoryService
    {
        void Create(Category entity);
        void Delete(int id);
        void Update(Category entity);
        Category GetById(int id);
        List<Category> GetAll();

    }
}
