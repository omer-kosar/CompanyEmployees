using Contracts.Logger;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications.Company
{
    internal sealed class EmailHandler : INotificationHandler<CompanyDeletedNotification>
    {
        private readonly ILoggerManager _logger;

        public EmailHandler(ILoggerManager logger)
        {
            _logger = logger;
        }

        public async Task Handle(CompanyDeletedNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning($"Delete action from the company with id:{notification.Id} has occured.");
            await Task.CompletedTask;
        }
    }
}
