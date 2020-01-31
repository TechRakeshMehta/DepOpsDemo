
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
    public interface IManageGradeView
    {
        /// <summary>
        /// CurrentUserID</summary>
        /// <value>
        /// Gets the value for current user's id.</value>
        Int32 CurrentUserId
        {
            get;
        }

        //    /// <summary>
        //    /// Name</summary>
        //    /// <value>
        //    /// Gets or sets the value for permission's id.</value>
        //    Int32 ParentOrganizationId
        //    {
        //        get;
        //        set;
        //    }

        //    /// <summary>
        //    /// TenantID</summary>
        //    /// <value>
        //    /// Gets or sets the value for tenant's id.</value>
        //    Int32 TenantId
        //    {
        //        get;
        //        set;
        //    }

        /// <summary>
        /// AllGrades</summary>
        /// <value>
        /// Gets or sets the list of all programs.</value>
        IQueryable<lkpGradeLevel> AllGrades
        {
            get;
            set;
        }

        /// <summary>
        /// AllGradeGroups</summary>
        /// <value>
        /// Gets or sets the list of all programs.</value>
        List<lkpGradeLevelGroup> AllGradeGroups
        {
            get;
            set;
        }

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
        ManageGradeContract ViewContract
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

        ///// <summary>
        ///// GroupID</summary>
        ///// <value>
        ///// Gets or sets the value for GroupID.</value>
        //Int16 GroupID
        //{
        //    get;
        //    set;
        //}

        Int32 OrganizationId
        {
            get;
            set;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 ParentOrganizationId
        {
            get;
            set;
        }
    }
}




