using FluentValidation;

namespace Assessment.Application.MainCategories.Commands.CreateMainCategory;

public class CreateMainCategoryCommandValidator : AbstractValidator<CreateMainCategoryCommand>
{
    public CreateMainCategoryCommandValidator()
    {
        RuleFor(v => v.Name).MaximumLength(100).NotEmpty();
        RuleFor(v => v.Description).MaximumLength(200).When(x => string.IsNullOrEmpty(x.Description));
    }
}