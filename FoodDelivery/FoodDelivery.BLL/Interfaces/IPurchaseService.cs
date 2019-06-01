﻿using FoodDelivery.DTO;
using FoodDelivery.DTO.Purchase;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IPurchaseService
    {
        List<PurchaseDTO> GetListOfPurchases(string userName);
        IEnumerable<PurchaseItemDTO> GetPurchaseItems(string purchaseId);
        IEnumerable<PurchaseItemDTO> GetPurchaseItemsByFilters(FilterMenuItem filter, string purchaseId);
        double GetPriceOfPurchaseItems(string purchaseId);
    }
}
