﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public interface IBkgOrderHistoryView
    {
        Int32 SelectedTenantId { get;}
        Int32 CurrentLoggedInUserId { get; }
        Int32 OrderID { get; }
        List<OrderEventHistoryContract> lstOrderEventHistory { get; set; }
    }
}