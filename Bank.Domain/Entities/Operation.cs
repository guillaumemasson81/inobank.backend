namespace Bank.Domain.Entities
{
    public class Operation
    {
        public long OperationId { get; set; }
        public DateTime Date { get; set; }
        public bool IsCredit { get; set; }
        public string OperationType { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Origin { get; set; }
        public long AccountId { get; set; }
        public Account? Account { get; set; }

        public Operation()
        {
            Date = DateTime.Now;
            IsCredit = false;
            OperationType = string.Empty;
            Name = string.Empty;
            Amount = 0;
            Origin = string.Empty;
        }
    }
}
