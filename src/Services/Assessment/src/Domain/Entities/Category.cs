namespace Assessment.Domain.Entities;

public class Category : BaseAuditableEntity
{
    public int Id { get; set; }
    public int ParentCategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public virtual List<Question> Questions { get; set; }
}