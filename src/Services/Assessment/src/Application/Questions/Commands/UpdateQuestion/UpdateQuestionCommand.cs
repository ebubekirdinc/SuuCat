using Assessment.Application.Common.Exceptions;
using Assessment.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Dto;

namespace Assessment.Application.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommand : IRequest<ApiResult<UpdateQuestionVm>>
{
    public int? Id { get; set; }
    public string QuestionText { get; init; }
    public string Answer { get; init; }

    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, ApiResult<UpdateQuestionVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateQuestionCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<UpdateQuestionVm>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(x=>x.Id == request.Id && x.Deleted == null, cancellationToken: cancellationToken);
            if (question == null)
                throw new NotFoundException("Question not found", request.Id);

            question.QuestionText = request.QuestionText;
            question.Answer = request.Answer.ToUpper();

            await _context.SaveChangesAsync(cancellationToken);

            return new ApiResult<UpdateQuestionVm>(true, _mapper.Map<UpdateQuestionVm>(question));
        }
    }
}