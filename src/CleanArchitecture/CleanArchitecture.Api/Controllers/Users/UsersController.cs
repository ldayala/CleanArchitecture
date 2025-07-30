
using CleanArchitecture.Application.Users.GetUserPagination;
using CleanArchitecture.Application.Users.LoginUser;
using CleanArchitecture.Application.Users.RegisterUser;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleanArchitecture.Api.Controllers.Users
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginUserRequest request,
            CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request.Email, request.Password);
            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return Unauthorized(result.Error);
            }
            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserRequest request,
            CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(request.Nombre, request.Apellidos, request.Email, request.Password);
            var result = await _sender.Send(command, cancellationToken);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            //return CreatedAtAction(nameof(Login), new { id = result.Value }, result.Value);
            return Ok(result.Value);

        }

        [AllowAnonymous]
        [HttpGet("getPagination", Name = "PaginationUser")]
        [ProducesResponseType(typeof(PaginationResult<User, UserId>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationResult<User, UserId>>> GetPagination(
            [FromQuery] GetUserPaginationQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(query, cancellationToken);
            
            return Ok(result);
        }


    }
}
