namespace Bank.Infrastructure.DTO
{
    public class BankTransfert
    {
        public long FirstAccountId { get; set; }
        public long SecondAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
