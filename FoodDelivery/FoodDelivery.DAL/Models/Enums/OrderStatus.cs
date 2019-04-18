using System.ComponentModel; 

namespace FoodDelivery.DAL.Models.Enums
{
    public enum OrderStatus
    {
        [Description("Waiting response")]
        WaitingResponse,
        [Description("Accepted")]
        Accepted,
        [Description("Rejected")]
        Rejected,
        [Description("Cooking")]
        Cooking,
        [Description("Cooked")]
        Cooked,
        [Description("In delivery")]
        InDelivery,
        [Description("Delivered")]
        Delivered,
        [Description("Cancelled")]
        Cancelled,
        [Description("Something unexpected")]
        Unexpected
    }
}
