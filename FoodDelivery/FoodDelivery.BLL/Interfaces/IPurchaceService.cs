using FoodDelivery.DTO.Purchace;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IPurchaceService
    {
        List<PurchaceDTO> GetListOfPurchaces(string userName);
        List<PurchaceItemDTO> GetPurchaceItems(string purchaceId);

    }
}
