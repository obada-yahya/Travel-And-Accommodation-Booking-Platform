using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IAppUserRepository
{
    public Task InsertAsync(AppUser appUser);
    public Task SaveChangesAsync();
}