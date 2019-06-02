using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.DAL.Models
{
    public class AdditionalInfoAboutMenuItem
    {
        public int AdditionalInfoAboutMenuItemId { get; set; }
        public string  AdditionalInfo { get; set; }
        public string MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}
