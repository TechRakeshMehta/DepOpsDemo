#region Namespace

#region System Defined

using System;
using System.Linq;
using System.Collections.Generic;

#endregion

#region Application Specific

using Entity;
using INTSOF.UI.Contract.IntsofSecurityModel;
using INTSOF.UI.Contract.SysXSecurityModel;

#endregion

#endregion


namespace CoreWeb.IntsofSecurityModel.Views
{
    public interface IManageProgramsView
    {


        IQueryable<lkpGradeLevel> AllGrades
        {
            get;
            set;
        }

        /// <summary>
        /// CurrentUserID</summary>
        /// <value>
        /// Gets the value for current user's id.</value>
        Int32 CurrentUserId
        {
            get;
        }


        /// <summary>
        /// OrganizationPrograms</summary>
        /// <value>
        /// Gets or sets the list of all programs.</value>
        //IQueryable<DeptProgramMapping> OrganizationPrograms
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// ErrorMessage</summary>
        /// <value>
        /// Gets or sets the value for error message.</value>
        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// IsAdmin</summary>
        /// <value>
        /// Gets the value for IsAdmin.</value>
        Boolean IsAdmin
        {
            get;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ManageProgramsContract ViewContract
        {
            get;
        }

        /// <summary>
        /// SuccessMessage</summary>
        /// <value>
        /// Gets or sets the value for Success message.</value>
        String SuccessMessage
        {
            get;
            set;
        }
        /// <summary>
        /// TenantID.
        /// </summary>
        /// <value>
        /// Gets or sets the value for tenant's id.
        /// </value>
        Int32 TenantId
        {
            get;
            set;
        }

        Int32 DepProgramMappingId
        { get; set; }

        Int32 SelectedOrganizationId
        { get; set; }

        /// <summary>
        /// Get or Set Payment Options
        /// </summary>
        IQueryable<Entity.ClientEntity.lkpPaymentOption> AllPaymentOption
        {
            get;
            set;
        }

        /// <summary>
        /// Collection of selected payment option.
        /// </summary>
        List<Int32> SelectedPaymentOptionIds
        {
            get;
            set;
        }
    }
}




