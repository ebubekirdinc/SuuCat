namespace Account.Domain.Entities;

public class User : BaseAuditableEntity
{
    public int Id { get; set; } 
    public string UserId { get; set; } 
    public string Email { get; set; }
    public string UserName { get; set; }
    public string SpecialCode { get; set; }
     
}