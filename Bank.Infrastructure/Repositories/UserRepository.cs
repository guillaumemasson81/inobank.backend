using Bank.Application.Repositories;
using Bank.Domain.Entities;
using Bank.Infrastructure.Data;
using Bank.Sql.Queries;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(DbContextOptions<BankContext> options) : base(options) { }

        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<User>(UserQueries.AllUsers);
            return result.ToList();
        }

        public async Task<User> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(UserQueries.UserById, new { userId = id });
        }

        public async Task<string> AddAsync(User entity)
        {
            using var connection = CreateConnection();
            var result = await connection.ExecuteAsync(UserQueries.AddUser, entity);
            return result.ToString();
        }

        public async Task<string> UpdateAsync(User entity)
        {
            using var connection = CreateConnection();
            var result = await connection.ExecuteAsync(UserQueries.UpdateUser, entity);
            return result.ToString();
        }

        public async Task<string> DeleteAsync(User entity)
        {
            using var connection = CreateConnection();
            var result = await connection.ExecuteAsync(UserQueries.DeleteUser, new { entity.UserId });
            return result.ToString();
        }
    }
}
