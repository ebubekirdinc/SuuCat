using Assessment.Application.Common.Mappings;
using Assessment.Domain.Entities;

namespace Assessment.Application.Categories.Commands.CreateCategory;

public class CreateCategoryVm : IMapFrom<Category>
{
    public int Id { get; set; }
    public int ParentCategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}