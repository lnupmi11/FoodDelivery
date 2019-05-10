using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DTO.Menu;
using FoodDelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FoodDelivery.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(CategoryDTO category)
        {
            _unitOfWork.CategoriesRepository.Create(
                new Category
                {
                    Id=category.Id,
                    CategoryName = category.CategoryName,
                    Description = category.Description
                });
            _unitOfWork.SaveChanges();
        }

        public void Delete(CategoryDTO category)
        {
            _unitOfWork.CategoriesRepository.Delete(category.Id);
            _unitOfWork.SaveChanges();
        }

        public CategoryDTO Get(string id)
        {
            Category category = _unitOfWork.CategoriesRepository.Get(id);
            return new CategoryDTO
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Description = category.Description
            };
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            var categories = _unitOfWork.CategoriesRepository.GetQuery();
            if (categories != null)
            {
                return categories.Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName,
                    Description = c.Description
                });

            }
            return new List<CategoryDTO>();
        }

        public void Update(CategoryDTO category)
        {
            var c = _unitOfWork.CategoriesRepository.Get(category.Id);
            c.Id = category.Id;
            c.CategoryName = category.CategoryName;
            c.Description = category.Description;
            _unitOfWork.CategoriesRepository.Update(c);
            _unitOfWork.SaveChanges();
        }
    }
}
