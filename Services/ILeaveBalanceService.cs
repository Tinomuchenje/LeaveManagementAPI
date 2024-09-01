using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagementAPI.Services
{
    public interface ILeaveBalanceService
    {
        void InitializeLeaveBalances(ApplicationUser user);
        Task<Dictionary<string, double>> GetLeaveBalancesAsync(string userId);
        Task UpdateLeaveBalancesAsync();
    }
}
