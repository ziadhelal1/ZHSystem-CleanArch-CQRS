using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZHSystem.Application.DTOs.UserMangment;
using ZHSystem.Application.Features.Users.Commands;
using ZHSystem.Application.Features.Users.Queries;
using System.Security.Claims;

namespace ZHSystem.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("users")]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            var id = await _mediator.Send(new CreateUserCommand(dto));
            return Ok(id);
        }

        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole(CreateRoleDto dto)
        {
            var id = await _mediator.Send(new CreateRoleCommand(dto));
            return Ok(id);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(AssignRoleDto dto)
        {
            await _mediator.Send(new AssignRoleCommand(dto));
            return NoContent();
        }
        [HttpGet("users")]
        
        public async Task<IActionResult> GetUsers()
        {
            var users = await _mediator.Send(new GetUsersWithRolesQuery());
            return Ok(users);
        }
        [HttpDelete("{userId}/roles/{roleId}")]
        public async Task<IActionResult> RemoveRole(int userId, int roleId)
        {
            await _mediator.Send(new RemoveRoleCommand(userId, roleId));
            return NoContent();
        }
        [HttpGet("UsersPagination")]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query)
            //[FromQuery] int pageNumber = 1,
            //[FromQuery] int pageSize = 10,
            //[FromQuery] string?serchUserName = null,
            //[FromQuery] string? searchEmail = null,
            //[FromQuery] string? searchRole = null,
            //[FromQuery]string? SortBy = null,
            //[FromQuery] bool Descending = false)
            {
            var result = await _mediator.Send(query);
            //var result = await _mediator.Send(new GetUsersWithRolesQuery());
            //return Ok(result);

            return Ok(result);
        }




        #region Test
        //[HttpGet("dashboard")]
        //public IActionResult Dashboard()
        //{
        //    return Ok(new
        //    {
        //        Message = "Welcome Admin 👑",
        //        Time = DateTime.UtcNow
        //    });
        //}
        //[HttpGet("test-role")]
        //[Authorize(Roles = "Admin")]
        //public IActionResult TestAdmin()
        //{
        //    return Ok("You are Admin!");
        //}

        //[HttpGet("test-auth")]
        //[Authorize]
        //public IActionResult TestAuth()
        //{
        //    return Ok($"Hello {User.Identity.Name}, your roles: {string.Join(", ", User.FindAll(ClaimTypes.Role).Select(c => c.Value))}");
        //}
        //[HttpGet("debug-token")]
        //[Authorize]
        //public IActionResult DebugToken()
        //{
        //    var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        //    return Ok(new
        //    {
        //        IsAuthenticated = User.Identity?.IsAuthenticated,
        //        Name = User.Identity?.Name,
        //        Claims = claims
        //    });
        //}
        //[HttpGet("protected")]
        //[Authorize]
        //public IActionResult Protected()
        //{
        //    var userName = User.Identity?.Name ?? "Unknown";
        //    var roles = string.Join(", ", User.FindAll(ClaimTypes.Role).Select(r => r.Value));
        //    return Ok(new
        //    {
        //        Message = $"Hello {userName}! Your roles: {roles}"
        //    });
        //} 
        #endregion
    }
}
