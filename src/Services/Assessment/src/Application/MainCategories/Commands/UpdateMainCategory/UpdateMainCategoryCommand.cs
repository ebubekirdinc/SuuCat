using Assessment.Application.Common.Exceptions;
using Assessment.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Dto;

namespace Assessment.Application.MainCategories.Commands.UpdateMainCategory;

public class UpdateMainCategoryCommand : IRequest<ApiResult<UpdateMainCategoryVm>>
{
    public int? Id { get; set; }
    public string Name { get; init; }
    public string Description { get; init; }

    public class UpdateMainCategoryCommandHandler : IRequestHandler<UpdateMainCategoryCommand, ApiResult<UpdateMainCategoryVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateMainCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<UpdateMainCategoryVm>> Handle(UpdateMainCategoryCommand request, CancellationToken cancellationToken)
        {
            var mainCategory = await _context.MainCategories.SingleOrDefaultAsync(x=>x.Id == request.Id && x.Deleted == null, cancellationToken: cancellationToken);
            if (mainCategory == null)
                throw new NotFoundException("MainCategory not found", request.Id);

            mainCategory.Name = request.Name;
            mainCategory.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            return new ApiResult<UpdateMainCategoryVm>(true, _mapper.Map<UpdateMainCategoryVm>(mainCategory));
        }
    }
}