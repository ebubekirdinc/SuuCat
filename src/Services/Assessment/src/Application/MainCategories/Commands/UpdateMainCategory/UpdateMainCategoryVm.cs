using Assessment.Application.Common.Mappings;
using Assessment.Domain.Entities;

namespace Assessment.Application.MainCategories.Commands.UpdateMainCategory;

public class UpdateMainCategoryVm : IMapFrom<MainCategory>
{
    public int Id { get; set; }
    public string Name { get; init; }
    public string Description { get; init; }
}