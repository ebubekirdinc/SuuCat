using Assessment.Application.Common.Exceptions;
using Assessment.Application.Common.Interfaces;
using Assessment.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Dto;

namespace Assessment.Application.Questions.Commands.CreateQuestion;

public class CreateQuestionCommand : IRequest<ApiResult<CreateQuestionVm>>
{
    public int CategoryId { get; init; }
    public string Answer { get; init; }
    public string QuestionText { get; init; }

    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, ApiResult<CreateQuestionVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateQuestionCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<CreateQuestionVm>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(x=>x.Id == request.CategoryId && x.Deleted == null, cancellationToken: cancellationToken);
            if (category == null)
                throw new NotFoundException("Category not found", request.CategoryId);
            
            var entity = new Question
            {
                CategoryId = request.CategoryId,
                QuestionText = request.QuestionText,
                Answer = request.Answer.ToUpper()
            };
             
            await _context.Questions.AddAsync(entity, cancellationToken);
            
            await _context.SaveChangesAsync(cancellationToken);

            var question = _mapper.Map<CreateQuestionVm>(entity);
            
            return new ApiResult<CreateQuestionVm>(true, question);
        }
    }
}