using FluentValidation;

namespace Assessment.Application.Questions.Commands.CreateQuestion;

public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(v => v.CategoryId).NotEmpty();
        RuleFor(v => v.QuestionText).MaximumLength(1000).NotEmpty();
        RuleFor(v => v.Answer).MaximumLength(1).NotEmpty();
    }
}