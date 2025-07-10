using Microsoft.AspNetCore.Mvc;
using Bank.Domain.Entities;
using Bank.Application.Repositories;
using Bank.Logging;

namespace Bank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            IEnumerable<User> users;

            try
            {
                users = await unitOfWork.Users.GetAllAsync();
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(long id)
        {
            User user;

            try
            {
                user = await unitOfWork.Users.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Add(User user)
        {
            try
            {
                await unitOfWork.Users.AddAsync(user);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception", ex);
                return BadRequest("An unexpected error occurred");
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(User user)
        {
            try
            {
                await unitOfWork.Users.UpdateAsync(user);
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
        public async Task<IActionResult> Delete(User user)
        {
            try
            {
                await unitOfWork.Users.DeleteAsync(user);
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