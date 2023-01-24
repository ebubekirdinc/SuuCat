using Assessment.Application.Common.Exceptions;
using Assessment.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Dto;

namespace Assessment.Application.MainCategories.Commands.DeleteMainCategory;

public class DeleteMainCategoryCommand : IRequest<ApiResult<string>>
{
    public int? Id { get; set; }

    public class DeleteMainCategoryCommandHandler : IRequestHandler<DeleteMainCategoryCommand, ApiResult<string>>
    {
        private readonly IApplicationDbContext _context;

        public DeleteMainCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<string>> Handle(DeleteMainCategoryCommand request, CancellationToken cancellationToken)
        {
            var mainCategory = await _context.MainCategories.SingleOrDefaultAsync(x=>x.Id == request.Id && x.Deleted == null, cancellationToken: cancellationToken);
            if (mainCategory == null)
                throw new NotFoundException("MainCategory not found", request.Id);

            // todo check if main category has any sub categories
            
            _context.MainCategories.Remove(mainCategory);

            await _context.SaveChangesAsync(cancellationToken);

            return new ApiResult<string>(true, "MainCategory deleted successfully");
        }
    }
}