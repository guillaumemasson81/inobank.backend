namespace Bank.Application.Repositories
{
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; }
        IOperationRepository Operations { get; }
        IUserRepository Users { get; }

        Task<int> SaveChangesAsync();
    }
}
