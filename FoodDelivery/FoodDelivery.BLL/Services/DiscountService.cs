using System;
using System.Collections.Generic;
using System.Linq;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DTO.Menu;

namespace FoodDelivery.BLL.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiscountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(DiscountDTO discount)
        {
            _unitOfWork.DiscountsRepository.Create(
                new Discount
                {
                    Id = discount.Id,
                    Percentage = discount.Percentage,
                    Description = discount.Description,
                });
            _unitOfWork.SaveChanges();
        }

        public void Delete(DiscountDTO discount)
        {
            _unitOfWork.DiscountsRepository.Delete(discount.Id);
            _unitOfWork.SaveChanges();
        }

        public DiscountDTO Get(string id)
        {
            Discount discount = _unitOfWork.DiscountsRepository.Get(id);
            return new DiscountDTO
            {
                Id = discount.Id,
                Percentage = discount.Percentage,
                Description = discount.Description
            };
        }

        public IEnumerable<DiscountDTO> GetAll()
        {
            var discounts = _unitOfWork.DiscountsRepository.GetQuery();
            if (discounts != null)
            {
                return discounts.Select(d => new DiscountDTO
                {
                    Id = d.Id,
                    Percentage = d.Percentage,
                    Description = d.Description
                });

            }
            return new List<DiscountDTO>();
        }

        public void Update(DiscountDTO discount)
        {
            var c = _unitOfWork.DiscountsRepository.Get(discount.Id);
            c.Id = discount.Id;
            c.Percentage = discount.Percentage;
            c.Description = discount.Description;
            _unitOfWork.DiscountsRepository.Update(c);
            _unitOfWork.SaveChanges();
        }
    }
}
