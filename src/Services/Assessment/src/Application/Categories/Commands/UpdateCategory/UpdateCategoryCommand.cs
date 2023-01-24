using Assessment.Application.Common.Exceptions;
using Assessment.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Dto;

namespace Assessment.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest<ApiResult<UpdateCategoryVm>>
{
    public int? Id { get; set; }
    public string Name { get; init; }
    public string Description { get; init; }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ApiResult<UpdateCategoryVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<UpdateCategoryVm>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(x=>x.Id == request.Id && x.Deleted == null, cancellationToken: cancellationToken);
            if (category == null)
                throw new NotFoundException("Category not found", request.Id);

            category.Name = request.Name;
            category.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            return new ApiResult<UpdateCategoryVm>(true, _mapper.Map<UpdateCategoryVm>(category));
        }
    }
}