using Microsoft.AspNetCore.Mvc;
using LeaveManagementAPI.Services;
using LeaveManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LeaveManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveManagementService _leaveService;
        private readonly UserRoleService _userRoleService;

        public LeaveController(ILeaveManagementService leaveService, UserRoleService userRoleService)
        {
            _leaveService = leaveService;
            _userRoleService = userRoleService;
        }

        // GET: api/Leave
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Leave>>> GetLeaves()
        {
            return Ok(await _leaveService.GetAllLeavesAsync());
        }

        // GET: api/Leave/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Leave>> GetLeave(int id)
        {
            var leave = await _leaveService.GetLeaveByIdAsync(id);

            if (leave == null)
            {
                return NotFound();
            }

            return Ok(leave);
        }

        // POST: api/Leave
        [HttpPost]
        public async Task<ActionResult<Leave>> PostLeave(Leave leave)
        {
            var createdLeave = await _leaveService.AddLeaveAsync(leave);
            return CreatedAtAction(nameof(GetLeave), new { id = createdLeave.Id }, createdLeave);
        }

        // PUT: api/Leave/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeave(int id, Leave leave)
        {
            if (id != leave.Id)
            {
                return BadRequest();
            }

            var result = await _leaveService.UpdateLeaveAsync(leave);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Leave/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeave(int id)
        {
            var result = await _leaveService.DeleteLeaveAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> ApproveLeave(int id)
        {
            UserRole? currentUserRole = await GetCurrentUserRole();

            if (currentUserRole == null)
            {
                return Unauthorized();
            }

            var result = await _leaveService.ApproveLeaveAsync(id, currentUserRole.Value);
            if (!result)
                return NotFound(); // Changed to NotFound as it's more appropriate for a non-existent resource

            return NoContent();
        }

        private async Task<UserRole?> GetCurrentUserRole()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return new UserRole();
            }
            return await _userRoleService.GetUserRoleAsync(currentUserId);
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> RejectLeave(int id)
        {
            UserRole? currentUserRole = await GetCurrentUserRole();

            if (currentUserRole == null)
            {
                return Unauthorized();
            }

            var result = await _leaveService.RejectLeaveAsync(id, currentUserRole.Value);

            if (!result)
                return Forbid(); // Or return NotFound() if leave is not found

            return NoContent();
        }

    }
}
