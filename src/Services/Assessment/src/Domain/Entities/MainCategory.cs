namespace Assessment.Domain.Entities;

public class MainCategory : BaseAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}