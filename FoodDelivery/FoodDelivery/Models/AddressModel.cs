using FoodDelivery.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.DTO.Models
{
    public class AddressModel
    {
        public Region Region { get; set; }
        public string Street { get; set; }
        public int BuildingNum { get; set; }
    }
}
