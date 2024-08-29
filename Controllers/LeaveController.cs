using Microsoft.AspNetCore.Mvc;
using LeaveManagementAPI.Services;
using LeaveManagementAPI.Models;

namespace LeaveManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveManagementService _leaveService;

        public LeaveController(ILeaveManagementService leaveService)
        {
            _leaveService = leaveService;
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
    }
}
