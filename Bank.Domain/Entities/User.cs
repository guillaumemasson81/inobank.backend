namespace Bank.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }

        public User()
        {
            UserName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Accounts = new List<Account>();
        }
    }
}
