using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveManagementAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace LeaveManagementAPI.Data
{
    public class LeaveManagementContext : IdentityDbContext<ApplicationUser>
    {
        public LeaveManagementContext(DbContextOptions<LeaveManagementContext> options)
                    : base(options)
        {
        }

        public DbSet<Leave> Leaves { get; set; }
    }
}