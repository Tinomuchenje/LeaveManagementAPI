using LeaveManagementAPI.Models;

namespace LeaveManagementAPI.Services
{
    public interface ILeaveManagementService
    {
        Task<IEnumerable<Leave>> GetAllLeavesAsync();
        Task<Leave?> GetLeaveByIdAsync(int id);
        Task<Leave> AddLeaveAsync(Leave leave);
        Task<bool> UpdateLeaveAsync(Leave leave);
        Task<bool> DeleteLeaveAsync(int id);
    }
}
