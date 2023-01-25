using FluentValidation;

namespace Assessment.Application.Questions.Commands.DeleteCategory;

public class DeleteQuestionCommandValidator : AbstractValidator<DeleteQuestionCommand>
{
    public DeleteQuestionCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
    }
}