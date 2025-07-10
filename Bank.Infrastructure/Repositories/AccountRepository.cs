using Bank.Application.Repositories;
using Bank.Domain.Entities;
using Bank.Infrastructure.Data;
using Bank.Sql.Queries;
using Dapper;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;

namespace Bank.Infrastructure.Repositories
{
    public class AccountRepository : RepositoryBase, IAccountRepository
    {
        public AccountRepository(DbContextOptions<BankContext> options) : base(options) { }

        public async Task<IReadOnlyList<Account>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<Account>(AccountQueries.AllAccounts);
            return result.ToList();
        }

        public async Task<IReadOnlyList<Account>> GetAllByUserAsync(int userId)
        {
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<Account>(AccountQueries.AllByUser, new { UserId = userId });
            return result.ToList();
        }

        public async Task<Account> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Account>(AccountQueries.AccountById, new { accountId = id });
        }

        public async Task<string> AddAsync(Account entity)
        {
            using var connection = CreateConnection();
            var result = await connection.ExecuteAsync(AccountQueries.AddAccount, entity);
            return result.ToString();
        }

        public async Task<string> UpdateAsync(Account entity)
        {
            using var connection = CreateConnection();
            var result = await connection.ExecuteAsync(AccountQueries.UpdateAccount, entity);
            return result.ToString();
        }

        public async Task<string> DeleteAsync(Account entity)
        {
            using var connection = CreateConnection();
            var result = await connection.ExecuteAsync(AccountQueries.DeleteAccount, new { entity.AccountId });
            return result.ToString();
        }
    }
}
