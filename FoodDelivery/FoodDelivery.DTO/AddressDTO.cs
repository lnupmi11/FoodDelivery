using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.DTO
{
    public class AddressDTO
    {
        public string AddressId { get; set; }
        public string Region { get; set; }
        public string Street { get; set; }
        public int BuildingNumber { get; set; }
    }
}
