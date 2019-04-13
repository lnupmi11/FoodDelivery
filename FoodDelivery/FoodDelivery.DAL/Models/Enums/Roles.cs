using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FoodDelivery.DAL.Models.Enums
{
    public enum Roles
    {
        [Description("Admin")]
        Admin,
        [Description("Order Manager")]
        OrderManager,
        [Description("User")]
        User,
    }
}
