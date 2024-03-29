﻿using Assessment.Application.Common.Exceptions;
using Assessment.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Dto;

namespace Assessment.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest<ApiResult<string>>
{
    public int? Id { get; set; }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResult<string>>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(x=>x.Id == request.Id && x.Deleted == null, cancellationToken: cancellationToken);
            if (category == null)
                throw new NotFoundException("Category not found", request.Id);

            var questions = await _context.Questions.Where(x => x.CategoryId == request.Id && x.Deleted == null).ToListAsync(cancellationToken: cancellationToken);
            if (questions.Any())
                throw new BadRequestException("Category has questions, delete them first");
            
            _context.Categories.Remove(category);

            await _context.SaveChangesAsync(cancellationToken);

            return new ApiResult<string>(true, "Category deleted successfully");
        }
    }
}