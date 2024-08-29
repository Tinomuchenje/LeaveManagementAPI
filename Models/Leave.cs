using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementAPI.Models
{

    public enum UserRole
    {
        Employee,
        Supervisor,
        Admin
    }

    public class Leave
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required string Reason { get; set; }
        public LeaveType Type { get; set; }
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
        public UserRole SubmittedByRole { get; set; } // Track who submitted the leave
    }

    public enum LeaveType
    {
        Sick,
        Casual,
        Annual
    }

    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }
}