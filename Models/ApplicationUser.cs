using Microsoft.AspNetCore.Identity;


namespace LeaveManagementAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }

        // Leave balances
        public int AnnualLeaveBalance { get; set; }
        public int SickLeaveBalance { get; set; }
        public int MaternityLeaveBalance { get; set; }

    }
}