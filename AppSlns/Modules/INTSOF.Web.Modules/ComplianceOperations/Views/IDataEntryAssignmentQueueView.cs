﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IDataEntryAssignmentQueueView
    {

        #region Properties
        Int32 TenantId
        {
            get;
        }
        #endregion
    }
}