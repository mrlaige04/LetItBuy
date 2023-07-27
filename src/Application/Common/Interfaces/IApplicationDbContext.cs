using Application.Common.Models;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<IUser> AppUsers { get; }
    DbSet<Product> Products { get; }
    DbSet<Cart> Carts { get; }
    DbSet<ProductPhoto> Photos { get; }
    DbSet<UserNotification> UserNotifications { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}