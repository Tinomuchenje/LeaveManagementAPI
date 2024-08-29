using LeaveManagementAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace LeaveManagementAPI.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRoleService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserRole> GetUserRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return UserRole.Employee;

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
                return UserRole.Admin;
            if (roles.Contains("Supervisor"))
                return UserRole.Supervisor;

            return UserRole.Employee;
        }
    }
}