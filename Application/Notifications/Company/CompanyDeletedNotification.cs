﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications.Company
{
    public sealed record CompanyDeletedNotification(Guid Id, bool TrackChanges) : INotification;
}
