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

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManageInstitutionNodeTypeView
    {
        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        List<Tenant> ListTenants
        {
            set;
            get;
        }

        Int32 SelectedTenantID
        {
            set;
            get;
        }

        Int32 CurrentUserId
        {
            get;
        }

        Int32 TenantId
        {
            set;
            get;
        }


        IQueryable<InstitutionNodeType> ListInstitutionNodeType
        {
            get;
            set;
        }

        IManageInstitutionNodeTypeView CurrentViewContext
        {
            get;

        }
        String InstitutionNodeTypeName
        {
            get;
            set;
        }

        String InstitutionNodeTypeDescription
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }
        Int32 InstitutionNodeTypeId
        {
            get;
            set;
        }
        String LastCode
        {
            get;
            set;
        }
        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        //IManageInstitutionNodeTypeView CurrentViewContext
        //{
        //    get;

        //}

        //UAT-3032
        Int32 PreferredSelectedTenantID { get; set; }
        #endregion

        #endregion
    }
}




