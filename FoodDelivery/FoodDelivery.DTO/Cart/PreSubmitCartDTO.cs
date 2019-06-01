using System.Collections.Generic;

namespace FoodDelivery.DTO.Cart
{
    public class PreSubmitCartDTO
    {
        public List<AddressDTO> SavedAddresses { get; set; }
        public List<string> Regions { get; set; }
        public bool IsSavedAddress { get; set; }
        public bool IsAuthorize { get; set; }
    }
}
