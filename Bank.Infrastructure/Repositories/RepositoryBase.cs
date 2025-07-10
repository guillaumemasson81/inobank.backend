using Microsoft.EntityFrameworkCore;
using System.Data;
using Bank.Infrastructure.Data;

namespace Bank.Infrastructure.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly DbContextOptions<BankContext> options;

        protected RepositoryBase(DbContextOptions<BankContext> options)
        {
            this.options = options;
        }

        protected IDbConnection CreateConnection()
        {
            var context = new BankContext(options);
            return context.Database.GetDbConnection();
        }
    }
}
