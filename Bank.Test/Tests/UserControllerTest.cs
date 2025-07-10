using Bank.Api.Controllers;
using Bank.Application.Repositories;
using Bank.Domain.Entities;
using Bank.Infrastructure.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Bank.Test.Tests
{
    [TestClass]
    public sealed class UserControllerTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork = null!;
        private Mock<IUserRepository> mockUserRepository = null!;
        private Mock<IAccountRepository> mockAccountRepository = null!;
        private Mock<IOperationRepository> mockOperationRepository = null!;
        private UserController userController;
        private AccountController accountController;
        private OperationController operationController;

        [TestInitialize]
        public void Setup()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockAccountRepository = new Mock<IAccountRepository>();
            mockOperationRepository = new Mock<IOperationRepository>();

            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.Accounts).Returns(mockAccountRepository.Object);
            mockUnitOfWork.Setup(u => u.Operations).Returns(mockOperationRepository.Object);
            mockUnitOfWork.Setup(u => u.Users).Returns(mockUserRepository.Object);

            userController = new UserController(mockUnitOfWork.Object);
            accountController = new AccountController(mockUnitOfWork.Object);
            operationController = new OperationController(mockUnitOfWork.Object);

            mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
        }

        [TestMethod]
        public async Task ListAccounts()
        {
            // Arrange
            Account account1 = new Account
            {
                UserId = 1,
                AccountId = 1,
                Amount = 100
            };

            Account account2 = new Account
            {
                UserId = 2,
                AccountId = 3,
                Amount = 1000
            };

            Currency currency = new Currency
            {
                AccountId = 1,
                Amount = 50.95M
            };

            List<Account> accounts = new List<Account>
            {
                account1,
                account2
            };

            mockAccountRepository.Setup(a => a.GetByIdAsync(account1.AccountId)).ReturnsAsync(account1);
            mockAccountRepository.Setup(a => a.GetAllByUserAsync(account1.UserId)).ReturnsAsync(accounts);
            mockOperationRepository.Setup(o => o.AddCurrency(account1, currency.Amount)).Returns(Task.CompletedTask);

            // Act
            var result = await operationController.GetAll();

            // Assert
            var res = result.Result as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [TestMethod]
        public async Task AddCurrency()
        {
            // Arrange
            Account account = new Account
            {
                AccountId = 1,
                Amount = 100
            };

            Currency currency = new Currency
            {
                AccountId = 1,
                Amount = 50.95M
            };

            mockAccountRepository.Setup(a => a.GetByIdAsync(account.AccountId)).ReturnsAsync(account);
            mockOperationRepository.Setup(o => o.AddCurrency(account, currency.Amount)).Returns(Task.CompletedTask);

            // Act
            var result = await operationController.AddCurrency(currency);

            // Assert
            var res = result.Result as OkResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [TestMethod]
        public async Task TakeCurrency()
        {
            // Arrange
            Account account = new Account
            {
                AccountId = 1,
                Amount = 100
            };

            Currency currency = new Currency
            {
                AccountId = 1,
                Amount = 50.95M
            };

            mockAccountRepository.Setup(a => a.GetByIdAsync(account.AccountId)).ReturnsAsync(account);
            mockOperationRepository.Setup(o => o.TakeCurrency(account, currency.Amount)).Returns(Task.CompletedTask);

            // Act
            var result = await operationController.TakeCurrency(currency);

            // Assert
            var res = result.Result as OkResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [TestMethod]
        public async Task BankTransfert()
        {
            // Arrange
            Account account1 = new Account
            {
                AccountId = 1,
                Amount = 2000,
            };

            Account account2 = new Account
            {
                AccountId = 2,
                Amount = 3000,
            };

            BankTransfert transfert = new BankTransfert
            {
                FirstAccountId = account1.AccountId,
                SecondAccountId = account2.AccountId,
                Amount = 500
            };

            decimal amount = 500M;

            mockAccountRepository.Setup(a => a.GetByIdAsync(account1.AccountId)).ReturnsAsync(account1);
            mockAccountRepository.Setup(a => a.GetByIdAsync(account2.AccountId)).ReturnsAsync(account2);
            mockOperationRepository.Setup(o => o.BankTransfert(account1, account2, amount)).Returns(Task.CompletedTask);

            // Act
            var result = await operationController.BankTransfert(transfert);

            // Assert
            var res = result.Result as OkResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [TestMethod]
        public async Task Histo()
        {
            // Arrange
            Account account = new Account
            {
                UserId = 1,
                AccountId = 1,
                Amount = 100
            };

            Operation op1 = new Operation
            {
                OperationId = 1,
                Date = new DateTime(2025, 7, 8, 14, 0, 0, 0, DateTimeKind.Local),
                AccountId = account.AccountId,
                Name = "OP1",
                IsCredit = true,
                Amount = 20
            };

            Operation op2 = new Operation
            {
                OperationId = 2,
                Date = new DateTime(2025, 7, 8, 15, 0, 0, 0, DateTimeKind.Local),
                AccountId = account.AccountId,
                Name = "OP2",
                IsCredit = false,
                Amount = 10,
            };

            IReadOnlyList<Operation> operations = new List<Operation>
            {
                op1,
                op2
            };

            mockAccountRepository.Setup(a => a.GetByIdAsync(account.AccountId)).ReturnsAsync(account);
            mockOperationRepository.Setup(o => o.GetHisto(account.AccountId)).ReturnsAsync(operations);

            // Act
            var result = await operationController.GetHisto(account.AccountId);

            // Assert
            var res = result.Result as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [TestMethod]
        public async Task BankTranfertPreventOverdraft()
        {
            // Arrange
            Account account1 = new Account
            {
                AccountId = 1,
                Amount = 2000,
            };

            Account account2 = new Account
            {
                AccountId = 2,
                Amount = 3000,
            };

            BankTransfert transfert = new BankTransfert
            {
                FirstAccountId = account1.AccountId,
                SecondAccountId = account2.AccountId,
                Amount = 5000
            };

            decimal amount = 500M;

            mockAccountRepository.Setup(a => a.GetByIdAsync(account1.AccountId)).ReturnsAsync(account1);
            mockAccountRepository.Setup(a => a.GetByIdAsync(account2.AccountId)).ReturnsAsync(account2);
            mockOperationRepository.Setup(o => o.BankTransfert(account1, account2, amount)).Returns(Task.CompletedTask);

            // Act
            var result = await operationController.BankTransfert(transfert);

            // Assert
            var res = result.Result as UnauthorizedObjectResult;
            Assert.IsNotNull(res);
            Assert.AreEqual("Solde insuffisant", res.Value);
        }

        [TestMethod]
        public async Task TakeCurrencyPreventOverdraft()
        {
            // Arrange
            Account account = new Account
            {
                AccountId = 1,
                AllowedOverdraft = 0,
                Amount = 100
            };

            Currency currency = new Currency
            {
                AccountId = 1,
                Amount = 1000M
            };

            mockAccountRepository.Setup(a => a.GetByIdAsync(account.AccountId)).ReturnsAsync(account);
            mockOperationRepository.Setup(o => o.TakeCurrency(account, currency.Amount)).Returns(Task.CompletedTask);

            // Act
            var result = await operationController.TakeCurrency(currency);

            // Assert
            var res = result.Result as UnauthorizedObjectResult;
            Assert.IsNotNull(res);
            Assert.AreEqual("Solde insuffisant", res.Value);
        }
    }
}
