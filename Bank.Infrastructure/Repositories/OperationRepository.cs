using Bank.Application.Repositories;
using Bank.Domain.Entities;
using Bank.Infrastructure.Data;
using Bank.Sql.Queries;
using Dapper;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;

namespace Bank.Infrastructure.Repositories
{
    public class OperationRepository : RepositoryBase, IOperationRepository
    {
        public OperationRepository(DbContextOptions<BankContext> options) : base(options) { }

        public async Task<IReadOnlyList<Operation>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<Operation>(OperationQueries.AllOperations);
            return result.ToList();
        }

        public async Task<Operation> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Operation>(OperationQueries.OperationById, new { operationId = id });
        }

        public async Task<string> AddAsync(Operation entity)
        {
            using var connection = CreateConnection();
            var result = await connection.ExecuteAsync(OperationQueries.AddOperation, entity);
            return result.ToString();
        }

        public async Task<string> UpdateAsync(Operation entity)
        {
            using var connection = CreateConnection();
            var result = await connection.ExecuteAsync(OperationQueries.UpdateOperation, entity);
            return result.ToString();
        }

        public async Task<string> DeleteAsync(Operation entity)
        {
            using var connection = CreateConnection();
            var result = await connection.ExecuteAsync(OperationQueries.DeleteOperation, new { entity.OperationId });
            return result.ToString();
        }

        public async Task AddCurrency(Account account, decimal amount)
        {
            using var connection = CreateConnection();

            Operation op = new Operation
            {
                AccountId = account.AccountId,
                Amount = amount,
                Date = DateTime.UtcNow,
                IsCredit = true,
                Name = "Dépôt devise",
                OperationType = "Dépôt",
                Origin = ""
            };

            await AddAsync(op);
        }

        public async Task TakeCurrency(Account account, decimal amount)
        {
            using var connection = CreateConnection();

            Operation op = new Operation
            {
                AccountId = account.AccountId,
                Amount = amount,
                Date = DateTime.UtcNow,
                IsCredit = false,
                Name = "Retrait devise",
                OperationType = "Retrait",
                Origin = ""
            };

            await AddAsync(op);
        }

        public async Task BankTransfert(Account firstAccount, Account secondAccount, decimal amount)
        {
            using var connection = CreateConnection();

            Operation firstOp = new Operation
            {
                AccountId = firstAccount.AccountId,
                Amount = amount,
                Date = DateTime.UtcNow,
                IsCredit = false,
                Name = "Origine Transfert",
                OperationType = "Retrait",
                Origin = ""
            };

            Operation secondOp = new Operation
            {
                AccountId = secondAccount.AccountId,
                Amount = amount,
                Date = DateTime.UtcNow,
                IsCredit = true,
                Name = "Destination Transfert",
                OperationType = "Retrait",
                Origin = ""
            };

            await AddAsync(firstOp);
            await AddAsync(secondOp);
        }

        public async Task<IReadOnlyList<Operation>> GetHisto(long accountId)
        {
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<Operation>(OperationQueries.Histo, new { AccountId = accountId });
            return result.ToList();
        }
    }
}
