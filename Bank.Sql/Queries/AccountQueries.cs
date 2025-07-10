namespace Bank.Sql.Queries
{
    public static class AccountQueries
    {
        public static string AllAccounts => "SELECT * FROM [Account] (NOLOCK)";
        public static string AllByUser => "SELECT * FROM [Account] (NOLOCK) WHERE [UserId] = @UserId";

        public static string AccountById => "SELECT * FROM [Account] (NOLOCK) WHERE [AccountId] = @AccountId";

        public static string AddAccount =>
            @"INSERT INTO [Account] ([Currency], [Name], [AccountType], [Amount], [AllowedOverdraft], [UserId]) 
            VALUES (@currency, @name, @accountType, @amount, @allowedOverdraft, @userId)";

        public static string UpdateAccount =>
            @"UPDATE [Account] 
        SET [Currency] = @currency, 
            [AccountType] = @accountType, 
            [Amount] = @amount, 
            [Name] = @name, 
            [AllowedOverdraft] = @allowedOverdraft, 
            [userId] = @userId
        WHERE [AccountId] = @accountId";

        public static string DeleteAccount => "DELETE FROM [Account] WHERE [AccountId] = @AccountId";
    }
}
