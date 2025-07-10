namespace Bank.Sql.Queries
{
    public static class UserQueries
    {
        public static string AllUsers => "SELECT * FROM [User] (NOLOCK)";

        public static string UserById => "SELECT * FROM [User] (NOLOCK) WHERE [UserId] = @UserId";

        public static string AddUser =>
            @"INSERT INTO [User] ([UserName], [FirstName], [LastName], [Email], [PhoneNumber]) 
            VALUES (@UserName, @FirstName, @LastName, @Email, @PhoneNumber)";

        public static string UpdateUser =>
            @"UPDATE [User] 
        SET [UserName] = @UserName, 
            [FirstName] = @FirstName, 
            [LastName] = @LastName, 
            [Email] = @Email, 
            [PhoneNumber] = @PhoneNumber
        WHERE [UserId] = @UserId";

        public static string DeleteUser => "DELETE FROM [User] WHERE [UserId] = @UserId";
    }
}
