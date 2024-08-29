using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveManagementAPI.Models;


namespace LeaveManagementAPI.Data
{
    public class LeaveManagementContext : DbContext
    {
        public LeaveManagementContext(DbContextOptions<LeaveManagementContext> options)
                    : base(options)
        {
        }

        public DbSet<Leave> Leaves { get; set; }
    }
}