using LeaveManagementAPI.Data;
using LeaveManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using LeaveManagementAPI.Models;

namespace LeaveManagementAPI.Services
{
    public class LeaveManagementService : ILeaveManagementService
    {
        private readonly LeaveManagementContext _context;

        public LeaveManagementService(LeaveManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Leave>> GetAllLeavesAsync()
        {
            return await _context.Leaves.ToListAsync();
        }

        public async Task<Leave?> GetLeaveByIdAsync(int id)
        {
            return await _context.Leaves.FindAsync(id);
        }

        public async Task<Leave> AddLeaveAsync(Leave leave)
        {
            _context.Leaves.Add(leave);
            await _context.SaveChangesAsync();
            return leave;
        }

        public async Task<bool> UpdateLeaveAsync(Leave leave)
        {
            _context.Entry(leave).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveExists(leave.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteLeaveAsync(int id)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null)
            {
                return false;
            }

            _context.Leaves.Remove(leave);
            await _context.SaveChangesAsync();

            return true;
        }

        private bool LeaveExists(int id)
        {
            return _context.Leaves.Any(e => e.Id == id);
        }

        public async Task<bool> ApproveLeaveAsync(int id, UserRole currentUserRole)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null || leave.Status != LeaveStatus.Pending)
                return false;

            // If the leave was submitted by a Supervisor, only an Admin can approve/reject
            if (leave.SubmittedByRole == UserRole.Supervisor && currentUserRole != UserRole.Admin)
                return false;

            leave.Status = LeaveStatus.Approved;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectLeaveAsync(int id, UserRole currentUserRole)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null || leave.Status != LeaveStatus.Pending)
                return false;

            // If the leave was submitted by a Supervisor, only an Admin can approve/reject
            if (leave.SubmittedByRole == UserRole.Supervisor && currentUserRole != UserRole.Admin)
                return false;

            leave.Status = LeaveStatus.Rejected;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
