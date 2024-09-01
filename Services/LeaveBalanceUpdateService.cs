using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementAPI.Services
{
    //A hosted service to handle year-end leave balance updates.
    public class LeaveBalanceUpdateService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public LeaveBalanceUpdateService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateLeaveBalances, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private void UpdateLeaveBalances(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var leaveBalanceService = scope.ServiceProvider.GetRequiredService<ILeaveBalanceService>();
                leaveBalanceService.UpdateLeaveBalancesAsync().Wait();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}