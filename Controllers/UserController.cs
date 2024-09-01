using LeaveManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LeaveManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Only Admins can manage users
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        // GET: api/UserManagement
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagementService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/UserManagement/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManagementService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        // POST: api/UserManagement
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email
            };

            var result = await _userManagementService.CreateUserAsync(user, request.Password, request.Role);

            if (result)
                return Ok("User created and role assigned successfully.");

            return BadRequest("Failed to create user or assign role.");
        }

        // PUT: api/UserManagement/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request)
        {
            var user = await _userManagementService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            user.Email = request.Email;
            user.UserName = request.Username;

            var result = await _userManagementService.UpdateUserAsync(user);

            if (result)
                return Ok("User updated successfully.");

            return BadRequest("Failed to update user.");
        }

        // DELETE: api/UserManagement/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userManagementService.DeleteUserAsync(id);

            if (result)
                return Ok("User deleted successfully.");

            return BadRequest("Failed to delete user.");
        }

        // POST: api/UserManagement/assign-role
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] RoleRequest request)
        {
            var result = await _userManagementService.AssignRoleAsync(request.UserId, request.Role);

            if (result)
                return Ok("Role assigned successfully.");

            return BadRequest("Failed to assign role.");
        }

        // POST: api/UserManagement/remove-role
        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRole([FromBody] RoleRequest request)
        {
            var result = await _userManagementService.RemoveRoleAsync(request.UserId, request.Role);

            if (result)
                return Ok("Role removed successfully.");

            return BadRequest("Failed to remove role.");
        }
    }
}
