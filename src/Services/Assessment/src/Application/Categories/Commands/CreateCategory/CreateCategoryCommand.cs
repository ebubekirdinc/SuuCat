using Assessment.Application.Common.Exceptions;
using Assessment.Application.Common.Interfaces;
using Assessment.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Dto;

namespace Assessment.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<ApiResult<CreateCategoryVm>>
{
    public int ParentCategoryId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ApiResult<CreateCategoryVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<CreateCategoryVm>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var mainCategory = await _context.MainCategories.SingleOrDefaultAsync(x=>x.Id == request.ParentCategoryId && x.Deleted == null, cancellationToken: cancellationToken);
            if (mainCategory == null)
                throw new NotFoundException("MainCategory not found", request.ParentCategoryId);
            
            var entity = new Category
            {
                Name = request.Name,
                Description = request.Description,
                ParentCategoryId = request.ParentCategoryId
            };
             
            await _context.Categories.AddAsync(entity, cancellationToken);
            
            await _context.SaveChangesAsync(cancellationToken);

            var category = _mapper.Map<CreateCategoryVm>(entity);
            
            return new ApiResult<CreateCategoryVm>(true, category);
        }
    }
}