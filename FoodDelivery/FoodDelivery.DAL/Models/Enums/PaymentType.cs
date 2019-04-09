using System.ComponentModel; 

namespace FoodDelivery.DAL.Models.Enums
{
    public enum PaymentType
    {
        [Description("Payment in cash")]
        Cash,
        [Description("Debit card payment")]
        DebitCard
    }
}
