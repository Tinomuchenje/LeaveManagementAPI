using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeaveManagementAPI.Models;
using Microsoft.EntityFrameworkCore; // Add this directive

namespace LeaveManagementAPI.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILeaveBalanceService _leaveBalanceService;


        public UserManagementService(UserManager<ApplicationUser> userManager, ILeaveBalanceService leaveBalanceService)
        {
            _userManager = userManager;
            _leaveBalanceService = leaveBalanceService;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> CreateUserAsync(ApplicationUser user, string password, string role)
        {
            _leaveBalanceService.InitializeLeaveBalances(user);


            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) return false;

            var roleResult = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }


        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> AssignRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<bool> RemoveRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            return result.Succeeded;
        }
    }

}