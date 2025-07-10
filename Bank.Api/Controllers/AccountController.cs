using Microsoft.AspNetCore.Mvc;
using Bank.Domain.Entities;
using Bank.Application.Repositories;
using Bank.Logging;

namespace Bank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAll()
        {
            IEnumerable<Account> accounts;
            try
            {
                accounts = await unitOfWork.Accounts.GetAllAsync();
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok(accounts);
        }

        [HttpGet("byuser/{userId}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAll(int userId)
        {
            IEnumerable<Account> accounts;
            try
            {
                accounts = await unitOfWork.Accounts.GetAllByUserAsync(userId);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetById(long id)
        {
            Account account;
            try
            {
                account = await unitOfWork.Accounts.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Account account)
        {
            try
            {
                await unitOfWork.Accounts.AddAsync(account);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Account account)
        {
            try
            {
                await unitOfWork.Accounts.UpdateAsync(account);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Account account)
        {
            try
            {
                await unitOfWork.Accounts.DeleteAsync(account);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok();
        }
    }
}