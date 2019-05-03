using FoodDelivery.DTO.Purchase;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IPurchaseService
    {
        List<PurchaseDTO> GetListOfPurchases(string userName);
        List<PurchaseItemDTO> GetPurchaseItems(string purchaseId);

    }
}
