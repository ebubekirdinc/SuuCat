using Account.Domain.Entities;
using Account.Infrastructure.Persistence;
using EventBus.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Consumers;

public class UserCreatedEventComsumer : IConsumer<UserCreatedEvent>
{
    private readonly ApplicationDbContext _context;

    public UserCreatedEventComsumer(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        if (string.IsNullOrEmpty(context.Message.UserId))
        {
            return;
        }

        var userExists = await _context.Users.AnyAsync(x => x.UserId == context.Message.UserId);
        if (userExists)
        {
            return;
        }

        var user = new User {
            Email = context.Message.Email, 
            UserId = context.Message.UserId,
            UserName = context.Message.UserName
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}