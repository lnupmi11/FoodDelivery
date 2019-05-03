using FoodDelivery.DTO.Menu;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.BLL.Interfaces
{
    public interface ICategoryService
    {
        CategoryDTO Get(string id);
        IEnumerable<CategoryDTO> GetAll();
        void Add(CategoryDTO category);
        void Update(CategoryDTO category);
        void Delete(CategoryDTO category);
    }
}
