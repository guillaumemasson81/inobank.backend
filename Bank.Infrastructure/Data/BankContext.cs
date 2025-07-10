using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Data
{
    public class BankContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Operation> Operations { get; set; }

        public BankContext(DbContextOptions<BankContext> options) : base(options) { }
    }
}
