using INTSOF.UI.Contract.SearchUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.SearchUI.Views
{
    public interface ISharedUserSearchDetails
    {
        /// <summary>
        /// Represents the list of Invitation Details
        /// </summary>
        List<SharedUserSearchInvitationDetailsContract> SharedUserInvitationDetails
        {
            get;
            set;
        }

        ///// <summary>
        ///// Represensts the Current Context
        ///// </summary>
        //ISharedUserSearchDetails CurrentViewContext
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Represents the Id of the Current Invitation being viewed, during Expand/Collapse
        /// </summary>
        Int32 CurrentInvitationId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the current invitation groupID
        /// </summary>
        Int32 SharedUserID
        {
            get;
            set;
        }

        List<SharedPackages> lstSharedPkgs
        {
            get;
            set;
        }

        String SharedCategoryIDs { get; set; }

        String InvitationSourceCode { get; set; }

        Int32? TenantID { get; set; }
    }
}
