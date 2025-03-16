using Microsoft.EntityFrameworkCore;
using LoginRegisterAuthAPI.Data;
using LoginRegisterAuthAPI.Models;
using System;
using System.Threading.Tasks;
using BCrypt.Net;

namespace LoginRegisterAuthAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContextClass _context;
    private readonly JwtTokenGenerator _tokenGenerator;
    public UserRepository(ApplicationDbContextClass context, JwtTokenGenerator tokenGenerator)
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<object?> AuthenticateUserAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            return null;
        var token = _tokenGenerator.GenerateToken(user.Id, user.Email);
        
        return new 
        {
            token,
            user = new 
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                phone = user.Phone,
                isActive = user.isActive
            }
        };
    }
    public async Task<Guid> CreateUserAsync(Users user)
    {
        // Hash password before storing
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        var idParameter = new Microsoft.Data.SqlClient.SqlParameter("@Id", System.Data.SqlDbType.UniqueIdentifier) 
        { 
            Direction = System.Data.ParameterDirection.Output 
        };
        var nameParameter = new Microsoft.Data.SqlClient.SqlParameter("@Name", user.Name);
        var emailParameter = new Microsoft.Data.SqlClient.SqlParameter("@Email", user.Email);
        var passwordParameter = new Microsoft.Data.SqlClient.SqlParameter("@Password", user.Password);
        var phoneParameter = new Microsoft.Data.SqlClient.SqlParameter("@Phone", user.Phone ?? (object)DBNull.Value);
        var isActiveParameter = new Microsoft.Data.SqlClient.SqlParameter("@isActive", user.isActive);

        await _context.Database.ExecuteSqlRawAsync("EXEC sp_CreateUser @Id OUTPUT, @Name, @Email, @Password, @Phone, @isActive",
            idParameter, nameParameter, emailParameter, passwordParameter, phoneParameter, isActiveParameter);

        return (Guid)idParameter.Value;
    }

    public async Task<Users?> GetUserByIdAsync(Guid id)
    {
        var idParameter = new Microsoft.Data.SqlClient.SqlParameter("@Id", id);
        var user = await _context.Users.FromSqlRaw("EXEC sp_GetUserById @Id", idParameter).ToListAsync();
        return user.FirstOrDefault();
    }

    public async Task<bool> UpdateUserAsync(Users user)
    {
        var parameters = new[]
        {
            new Microsoft.Data.SqlClient.SqlParameter("@Id", user.Id),
            new Microsoft.Data.SqlClient.SqlParameter("@Name", user.Name),
            new Microsoft.Data.SqlClient.SqlParameter("@Email", user.Email),
            new Microsoft.Data.SqlClient.SqlParameter("@Phone", user.Phone ?? (object)DBNull.Value),
            new Microsoft.Data.SqlClient.SqlParameter("@isActive", user.isActive)
        };

        var affectedRows = await _context.Database.ExecuteSqlRawAsync("EXEC sp_UpdateUser @Id, @Name, @Email, @Phone, @isActive", parameters);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var idParameter = new Microsoft.Data.SqlClient.SqlParameter("@Id", id);
        var affectedRows = await _context.Database.ExecuteSqlRawAsync("EXEC sp_DeleteUser @Id", idParameter);
        return affectedRows > 0;
    }
}

