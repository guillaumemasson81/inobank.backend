namespace Bank.Domain.Entities
{
    public class Account
    {
        public long AccountId { get; set; }
        public string Currency { get; set; }
        public string AccountType { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal AllowedOverdraft { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }

        public Account()
        {
            Currency = "euro";
            AccountType = string.Empty;
            Name = string.Empty;
            Amount = 0;
            AllowedOverdraft = 0;

            Operations = new List<Operation>();
        }
    }
}
