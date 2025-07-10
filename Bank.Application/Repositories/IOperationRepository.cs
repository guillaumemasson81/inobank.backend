using Bank.Domain.Entities;

namespace Bank.Application.Repositories
{
    public interface IOperationRepository : IRepository<Operation>
    {
        Task<IReadOnlyList<Operation>> GetHisto(long accountId);
        Task AddCurrency(Account account, decimal amount);
        Task TakeCurrency(Account account, decimal amount);
        Task BankTransfert(Account firstAccount, Account secondAccount, decimal amount);
    }
}
