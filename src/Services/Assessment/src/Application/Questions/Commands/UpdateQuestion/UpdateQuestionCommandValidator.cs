using FluentValidation;

namespace Assessment.Application.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.QuestionText).MaximumLength(1000).NotEmpty();
        RuleFor(v => v.Answer).MaximumLength(1).NotEmpty();
    }
}