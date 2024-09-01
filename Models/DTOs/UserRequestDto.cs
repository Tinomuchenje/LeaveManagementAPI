using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementAPI.Models
{
    public class UserRequestDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }

        // Leave balances
        public int AnnualLeave { get; set; }
        public int SickLeave { get; set; }
        public int MaternityLeave { get; set; }
    }
}