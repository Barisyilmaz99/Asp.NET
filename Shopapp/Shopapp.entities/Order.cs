using System;
using System.Collections.Generic;
using System.Text;

namespace Shopapp.Entities
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItems>();
        }
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string OrderNote { get; set; }
        public EnumOrderState OrderState { get; set; }
        public EnumPaymentTypes PaymentTypes { get; set; }
        public List<OrderItems> OrderItems { get; set; }
        public string PaymentToken { get; set; }
        public string PaymentId { get; set; }
        public string ConversationId { get; set; }

    }
    public enum EnumOrderState
    {
        waiting=0,
        Unpaid=1,
        Completed=2
    }
}   public enum EnumPaymentTypes
    {
        CreditCard=0,
        Eft = 1
    }
