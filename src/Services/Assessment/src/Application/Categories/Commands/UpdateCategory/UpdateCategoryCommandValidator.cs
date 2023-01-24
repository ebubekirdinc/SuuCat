using FluentValidation;

namespace Assessment.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.Name).MaximumLength(100).NotEmpty();
        RuleFor(v => v.Description).MaximumLength(200).When(x => string.IsNullOrEmpty(x.Description));
    }
}