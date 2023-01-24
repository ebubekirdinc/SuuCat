using FluentValidation;

namespace Assessment.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(v => v.ParentCategoryId).NotEmpty();
        RuleFor(v => v.Name).MaximumLength(100).NotEmpty();
        RuleFor(v => v.Description).MaximumLength(200).When(x => string.IsNullOrEmpty(x.Description));
    }
}