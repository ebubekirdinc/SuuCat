using FluentValidation;

namespace Assessment.Application.MainCategories.Commands.DeleteMainCategory;

public class DeleteMainCategoryCommandValidator : AbstractValidator<DeleteMainCategoryCommand>
{
    public DeleteMainCategoryCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
    }
}