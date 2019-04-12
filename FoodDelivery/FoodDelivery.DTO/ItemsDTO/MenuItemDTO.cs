﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.DTO.ItemsDTO
{
    public class MenuItemDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}