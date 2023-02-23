namespace Subscription.Domain.Entities;

public class Stock : BaseAuditableEntity
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Count { get; set; }
     
}