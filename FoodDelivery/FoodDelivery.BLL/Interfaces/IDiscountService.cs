using System;
using System.Collections.Generic;
using FoodDelivery.DTO.Menu;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IDiscountService
    {
        DiscountDTO Get(string id);
        IEnumerable<DiscountDTO> GetAll();
        void Add(DiscountDTO discount);
        void Update(DiscountDTO discount);
        void Delete(DiscountDTO discount);
    }
}
