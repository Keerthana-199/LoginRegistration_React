using LoginRegisterAuthAPI.Models;
using System;
using System.Threading.Tasks;

namespace LoginRegisterAuthAPI.Repositories
{
    public interface IUserRepository
    {
        Task<object?> AuthenticateUserAsync(string email, string password);
        Task<Guid> CreateUserAsync(Users user);
        Task<Users?> GetUserByIdAsync(Guid id);
        Task<bool> UpdateUserAsync(Users user);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
