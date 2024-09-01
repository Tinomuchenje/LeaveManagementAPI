using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveManagementAPI.Models;

namespace LeaveManagementAPI.Services
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(string username, string password);
        Task LogoutAsync();
        Task<ApplicationUser> GetCurrentUserAsync();
    }
}