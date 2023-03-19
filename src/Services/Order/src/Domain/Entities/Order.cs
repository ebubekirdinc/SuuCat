using Order.Domain.Enums;

namespace Order.Domain.Entities;

public class Order : BaseAuditableEntity
{
    public Order()
    {
        OrderItemList = new List<OrderItem>();
    }
    public int Id { get; set; }
    public string CustomerId { get; set; }
    public string PaymentAccountId { get; set; }
    public OrderStatus Status { get; set; }
    public string ErrorMessage { get; set; }
    
    public virtual List<OrderItem> OrderItemList { get; set; } 
}