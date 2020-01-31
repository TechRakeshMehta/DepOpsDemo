#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using System.Linq;
using System;

#endregion

#region UserDefined

using Entity.ClientEntity;

#endregion

#endregion

namespace CoreWeb.ContractManagement.Views
{
    public interface IManageContractTypesView
    {
        #region Properties

        #region Public Properties

        IManageContractTypesView CurrentViewContext { get; }
        List<Tenant> ListTenants { get; set; }
        Int32 TenantId { get; set; }
        Int32 SelectedTenantID { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        IQueryable<ContractType> ListContractTypes { get; set; }
        String ContractTypeName { get; set; }
        String ContractTypeLabel  { get; set; }
        String ContractTypeDescription { get; set; }
        String ErrorMessage { get; set; }
        String SuccessMessage { get; set; }       
        Int32 ContractTypeId { get; set; }
        String LastCode { get; set; }

        #endregion

        #endregion
    }
}
