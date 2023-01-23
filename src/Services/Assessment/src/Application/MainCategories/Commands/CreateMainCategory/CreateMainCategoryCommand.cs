using Assessment.Application.Common.Interfaces;
using Assessment.Domain.Entities;
using AutoMapper;
using MediatR;
using Shared.Dto;

namespace Assessment.Application.MainCategories.Commands.CreateMainCategory;

public class CreateMainCategoryCommand : IRequest<ApiResult<CreateMainCategoryVm>>
{
    public string Name { get; init; }
    public string Description { get; init; }

    public class CreateMainCategoryCommandHandler : IRequestHandler<CreateMainCategoryCommand, ApiResult<CreateMainCategoryVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateMainCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<CreateMainCategoryVm>> Handle(CreateMainCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new MainCategory
            {
                Name = request.Name,
                Description = request.Description
            };
             
            await _context.MainCategories.AddAsync(entity, cancellationToken);
            
            await _context.SaveChangesAsync(cancellationToken);

            var mainCategory = _mapper.Map<CreateMainCategoryVm>(entity);
            
            return new ApiResult<CreateMainCategoryVm>(true, mainCategory);
        }
    }
}