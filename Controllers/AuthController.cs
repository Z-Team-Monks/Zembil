using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zembil.ErrorHandler;
using Zembil.Services;
using Zembil.Views;

namespace Zembil.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] UserCred userCred)
        {
            var token = await _accountService.Authenticate(userCred.Username, userCred.Password);

            if (token == null)
                throw new CustomAppException(new ErrorDetail() { StatusCode = 401, Message = "Wrong user credentials!", Status = "fail" });

            var authDto = new AuthDto
            {
                token = token
            };
            return Ok(authDto);
        }

    }
}