using Bank.Application.Repositories;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContextOptions<BankContext> options;

        public IUserRepository Users { get; set; }
        public IAccountRepository Accounts { get; set; }

        public IOperationRepository Operations { get; set; }

        public UnitOfWork(DbContextOptions<BankContext> options)
        {
            this.options = options;
            this.Users = new UserRepository(options);
            this.Accounts = new AccountRepository(options);
            this.Operations = new OperationRepository(options);
        }

        public async Task<int> SaveChangesAsync()
        {
            using var context = new BankContext(options);
            return await context.SaveChangesAsync();
        }
    }
}
