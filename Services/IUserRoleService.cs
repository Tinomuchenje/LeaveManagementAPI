using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveManagementAPI.Models;

namespace LeaveManagementAPI.Services
{
    public interface IUserRoleService
    {
        Task<UserRole> GetUserRoleAsync(string userId);
    }
}