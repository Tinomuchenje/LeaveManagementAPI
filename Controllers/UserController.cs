using LeaveManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LeaveManagementAPI.Models;
using LeaveManagementAPI.Models.DTOs;

namespace LeaveManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Only Admins can manage users
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        private readonly ILeaveBalanceService _leaveBalanceService;


        public UserManagementController(IUserManagementService userManagementService, , ILeaveBalanceService leaveBalanceService)
        {
            _userManagementService = userManagementService;
            _leaveBalanceService = leaveBalanceService;

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
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDto request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                FullName = request.FullName,
                Gender = request.Gender,
                Age = request.Age,
            };

            var result = await _userManagementService.CreateUserAsync(user, request.Password, request.Role, request.AnnualLeave, request.SickLeave, request.MaternityLeave);

            if (result)
                return Ok("User created successfully with role and leave balances.");

            return BadRequest("Failed to create user or assign role.");
        }

        // PUT: api/UserManagement/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserRequestDto request)
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
        public async Task<IActionResult> AssignRole([FromBody] RoleRequestDto request)
        {
            var result = await _userManagementService.AssignRoleAsync(request.UserId, request.Role);

            if (result)
                return Ok("Role assigned successfully.");

            return BadRequest("Failed to assign role.");
        }

        // POST: api/UserManagement/remove-role
        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRole([FromBody] RoleRequestDto request)
        {
            var result = await _userManagementService.RemoveRoleAsync(request.UserId, request.Role);

            if (result)
                return Ok("Role removed successfully.");

            return BadRequest("Failed to remove role.");
        }

        [HttpGet("leave-balances/{userId}")]
        public async Task<IActionResult> GetLeaveBalances(string userId)
        {
            var leaveBalances = await _leaveBalanceService.GetLeaveBalancesAsync(userId);
            if (leaveBalances == null)
                return NotFound("User not found.");

            return Ok(leaveBalances);
        }

        [HttpPost("reset-leave-balances")]
        public async Task<IActionResult> ResetLeaveBalances()
        {
            await _leaveBalanceService.UpdateLeaveBalancesAsync();
            return Ok("Leave balances updated.");
        }
    }
}
