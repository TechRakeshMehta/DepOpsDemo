﻿using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface ISchoolNodeAssociationView
    {
        ISchoolNodeAssociationView CurrentViewContext { get; }

        Int32 CurrentUserId { get; }

        Int32 TenantId { get; set; }

        List<SchoolNodeAssociationDataContract> lstSchoolNodeAssociation { get; set; }

        Int32 AgencyHierarchyID { get; set; }

        Int32 SelectedTenantID { get; set; }

        List<TenantDetailContract> lstTenant { get; set; }

        SchoolNodeAssociationContract SchoolNodeAssociationContract { get; set; }
    }
}