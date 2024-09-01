using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LeaveManagementAPI.Services
{
    public class LeaveBalanceService : ILeaveBalanceService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public LeaveBalanceService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Initialize leave balances for a new user
        public void InitializeLeaveBalances(ApplicationUser user)
        {
            user.AnnualLeaveBalance = 22.5;
            user.SickLeaveBalance = 14;
            user.EducationLeaveBalance = 14;
            user.MaternityLeaveBalance = user.Gender == "Female" ? 90 : 0;
            user.LastLeaveUpdate = DateTime.UtcNow;
        }

        // Get leave balances for a specific user
        public async Task<Dictionary<string, double>> GetLeaveBalancesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var leaveBalances = new Dictionary<string, double>
            {
                { "AnnualLeaveBalance", user.AnnualLeaveBalance },
                { "SickLeaveBalance", user.SickLeaveBalance },
                { "EducationLeaveBalance", user.EducationLeaveBalance },
                { "MaternityLeaveBalance", user.MaternityLeaveBalance }
            };

            return leaveBalances;
        }

        // Update leave balances at the end of the year
        public async Task UpdateLeaveBalancesAsync()
        {
            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                if (DateTime.UtcNow.Year > user.LastLeaveUpdate.Year)
                {
                    // Accrue Annual Leave
                    user.AnnualLeaveBalance += 22.5;

                    // Reset other leaves
                    user.SickLeaveBalance = 14;
                    user.EducationLeaveBalance = 14;
                    user.MaternityLeaveBalance = user.Gender == "Female" ? 90 : 0;

                    user.LastLeaveUpdate = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);
                }
            }
        }
    }
}
