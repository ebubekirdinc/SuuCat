namespace Assessment.Domain.Entities;

public class Question : BaseAuditableEntity
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string QuestionText { get; set; }
    public string Answer { get; set; }

    public virtual Category Category { get; set; }
}