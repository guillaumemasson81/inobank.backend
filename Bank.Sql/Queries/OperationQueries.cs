namespace Bank.Sql.Queries
{
    public static class OperationQueries
    {
        public static string AllOperations => "SELECT * FROM [Operation] (NOLOCK)";

        public static string Histo => "SELECT * FROM [Operation] (NOLOCK) WHERE [AccountId] = @accountId";

        public static string OperationById => "SELECT * FROM [Operation] (NOLOCK) WHERE [OperationId] = @OperationId";

        public static string AddOperation =>
            @"INSERT INTO [Operation] ([Date], [OperationType], [IsCredit], [Name], [Amount], [Origin], [AccountId]) 
            VALUES (@Date, @OperationType, @IsCredit, @Name, @Amount, @Origin, @AccountId)";

        public static string UpdateOperation =>
            @"UPDATE [Operation] 
        SET [Date] = @Date, 
            [OperationType] = @OperationType, 
            [Name] = @Name, 
            [Amount] = @Amount, 
            [IsCredit] = @IsCredit, 
            [Origin] = @Origin, 
            [AccountId] = @AccountId
        WHERE [OperationId] = @OperationId";

        public static string DeleteOperation => "DELETE FROM [Operation] WHERE [OperationId] = @OperationId";
    }
}
