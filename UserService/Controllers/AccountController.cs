using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Dtos;
using UserService.Entities;

namespace UserService.Controllers
{
    /// <summary>
    /// Contoller with endopoints for checking accounts when autenticating users 
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AccountInfo> userManager;

        public AccountController(UserManager<AccountInfo> userManager)
      {
            this.userManager = userManager;
        }

        /// <summary>
        /// Checks if login info are valid
        /// </summary>
        /// <returns>Information about username and password being valid</returns>
        /// <response code="200">Information for principal</response>
        ///<response code="400">Wrong information for principal</response>
        /// <response code="500">Error on the server while checking principal</response>
        [HttpPost("checkPrincipal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<CheckAccountResponse>> CheckAccount([FromBody] CheckAccountRequest requestBody)
        {
            try
            {
                CheckAccountResponse response;
                AccountInfo account = await userManager.FindByEmailAsync(requestBody.Email);
                if (account == null)
                {
                    response = new CheckAccountResponse(false, "Email or password are wrong", null);
                    return BadRequest(response);
                }
                var passwordValid = await userManager.CheckPasswordAsync(account, requestBody.Password);
                if (!passwordValid)
                { 
                   response = new CheckAccountResponse(false, "Email or password are wrong", null);
                    return BadRequest(response);

                }
                if (!account.AccountIsActive)
                {
                    response = new CheckAccountResponse(false, "Account is not activate", null);
                    return BadRequest(response);

                }
                List<string> roles = userManager.GetRolesAsync(account).Result.ToList();
                AccountInfoDto accountDto = new AccountInfoDto(roles[0], account.Id);
                response = new CheckAccountResponse(true, "Successful check", accountDto);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occured");

            }

        } 

    }
}
