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
        IEnumerable<PurchaseItemDTO> GetPurchaseItemsByFilters(int page, string searchWord, string filterOpt, string categoryId, int itemPerPage, string purchaseId);
    }
}
