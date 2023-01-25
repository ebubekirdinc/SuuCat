using Assessment.Application.Common.Exceptions;
using Assessment.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Dto;

namespace Assessment.Application.Questions.Commands.DeleteCategory;

public class DeleteQuestionCommand : IRequest<ApiResult<string>>
{
    public int? Id { get; set; }

    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, ApiResult<string>>
    {
        private readonly IApplicationDbContext _context;

        public DeleteQuestionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<string>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(x=>x.Id == request.Id && x.Deleted == null, cancellationToken: cancellationToken);
            if (question == null)
                throw new NotFoundException("Question not found", request.Id);
 
            _context.Questions.Remove(question);

            await _context.SaveChangesAsync(cancellationToken);

            return new ApiResult<string>(true, "Question deleted successfully");
        }
    }
}