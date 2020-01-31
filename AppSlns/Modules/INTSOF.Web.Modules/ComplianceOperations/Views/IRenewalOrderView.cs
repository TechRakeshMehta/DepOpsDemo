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
    public interface IRenewalOrderView
    {

        #region Variables

        #region Private Variables
        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties

        Int32 CurrentLoggedInUserId
        {
            get;
        }
        Int32 TenantId
        {
            get;
        }
        Int32 OrderId
        {
            get;
            set;

        }

        String FirstName
        {
            //get;
            set;
        }

        String LastName
        {
            //get;
            set;
        }

        String InstitutionHierarchy
        {
            //get;
            set;
        }

        /// <summary>
        /// DeptProgramId of the selected Node hierarchy, to get the Custom attribute values
        /// </summary>
        Int32 SelectedDeptProgramId
        {
            get;
            set;
        }

        String PackageName
        {
            //get;
            set;
        }
        String PackageDetail
        {
            //get;
            set;
        }

        Int32? RenewalDuration
        {
            get;
            set;
        }

        Decimal? Price
        {
            get;
            set;
        }
        Decimal? RushOrderPrice
        {
            get;
            set;
        }
        Decimal? TotalPrice
        {
            get;
            set;
        }

        Int32? ProgramDuration
        {
            get;
            set;
        }

        Order OrderDetail
        { get; set; }

        Int32 DPPS_Id
        { get; set; }

        /// <summary>
        /// Get and set next page path.
        /// </summary>
        String NextPagePath
        {
            get;
            set;
        }

        Int32 NodeId
        {
            get;
            set;
        }

        Int32 Dpp_Id
        {
            get;
            set;
        }

        Boolean ShowRushOrder
        {
            get;
            set;
        }

        Boolean ViewDetails
        {
            get;
            set;
        }

        Int32 MaximumAllowedDuration
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}




