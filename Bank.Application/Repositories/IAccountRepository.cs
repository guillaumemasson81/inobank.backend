using Bank.Domain.Entities;

namespace Bank.Application.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<IReadOnlyList<Account>> GetAllByUserAsync(int userId);
    }
}
