using Microsoft.AspNetCore.Mvc;
using Bank.Domain.Entities;
using Bank.Application.Repositories;
using Bank.Infrastructure.DTO;

namespace Bank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public OperationController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operation>>> GetAll()
        {
            IEnumerable<Operation> operations;

            try
            {
                operations = await unitOfWork.Operations.GetAllAsync();
            }
            catch (Exception ex)
            {
                Logging.Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok(operations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Operation>> GetById(long id)
        {
            Operation operation;

            try
            {
                operation = await unitOfWork.Operations.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                Logging.Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok(operation);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Operation operation)
        {
            try
            {
                var account = await unitOfWork.Accounts.GetByIdAsync(operation.AccountId);
                if (operation.IsCredit)
                {
                    account.Amount += operation.Amount;
                }
                else
                {
                    if (operation.Amount < (account.Amount + account.AllowedOverdraft))
                    {
                        account.Amount -= operation.Amount;
                    }
                    else
                    {
                        return Unauthorized("Solde insuffisant");
                    }
                }

                await unitOfWork.Operations.AddAsync(operation);

                await unitOfWork.Accounts.UpdateAsync(account);

                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logging.Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Operation operation)
        {
            try
            {
                await unitOfWork.Operations.UpdateAsync(operation);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logging.Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Operation operation)
        {
            try
            {
                await unitOfWork.Operations.DeleteAsync(operation);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logging.Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok();
        }

        [HttpPost("addCurrency")]
        public async Task<ActionResult<string>> AddCurrency(Currency currency)
        {
            try
            {
                var account = await unitOfWork.Accounts.GetByIdAsync(currency.AccountId);
                await unitOfWork.Operations.AddCurrency(account, currency.Amount);
                account.Amount += currency.Amount;
                await unitOfWork.Accounts.UpdateAsync(account);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logging.Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok();
        }

        [HttpPost("takeCurrency")]
        public async Task<ActionResult<string>> TakeCurrency(Currency currency)
        {
            try
            {
                var account = await unitOfWork.Accounts.GetByIdAsync(currency.AccountId);
                if (currency.Amount <= (account.Amount + account.AllowedOverdraft))
                {
                    await unitOfWork.Operations.TakeCurrency(account, currency.Amount);
                    account.Amount -= currency.Amount;
                    await unitOfWork.Accounts.UpdateAsync(account);
                    await unitOfWork.SaveChangesAsync();
                }
                else
                {
                    return Unauthorized("Solde insuffisant");
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok();
        }

        [HttpPost("transfert")]
        public async Task<ActionResult<string>> BankTransfert(BankTransfert transfert)
        {
            try
            {
                var firstAccount = await unitOfWork.Accounts.GetByIdAsync(transfert.FirstAccountId);
                var secondAccount = await unitOfWork.Accounts.GetByIdAsync(transfert.SecondAccountId);
                if (transfert.Amount <= (firstAccount.Amount + firstAccount.AllowedOverdraft))
                {
                    firstAccount.Amount -= transfert.Amount;
                    secondAccount.Amount += transfert.Amount;
                    await unitOfWork.Operations.BankTransfert(firstAccount, secondAccount, transfert.Amount);
                    await unitOfWork.SaveChangesAsync();
                }
                else
                {
                    return Unauthorized("Solde insuffisant");
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok();
        }

        [HttpGet("histo/{accountId}")]
        public async Task<ActionResult<IEnumerable<Operation>>> GetHisto(long accountId)
        {
            IEnumerable<Operation> operations;

            try
            {
                operations = await unitOfWork.Operations.GetHisto(accountId);
            }
            catch (Exception ex)
            {
                Logging.Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok(operations);
        }
    }
}
