using FluentValidation;

namespace Assessment.Application.MainCategories.Commands.UpdateMainCategory;

public class UpdateMainCategoryCommandValidator : AbstractValidator<UpdateMainCategoryCommand>
{
    public UpdateMainCategoryCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.Name).MaximumLength(100).NotEmpty();
        RuleFor(v => v.Description).MaximumLength(200).When(x => string.IsNullOrEmpty(x.Description));
    }
}