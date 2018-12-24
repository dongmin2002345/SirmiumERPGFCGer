﻿using ServiceInterfaces.Messages.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Employees
{
    public interface IPhysicalPersonDocumentService
    {
        PhysicalPersonDocumentListResponse Sync(SyncPhysicalPersonDocumentRequest request);
    }
}
