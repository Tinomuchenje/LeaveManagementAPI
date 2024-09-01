using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementAPI.Models
{
    public class RoleRequestDto
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}