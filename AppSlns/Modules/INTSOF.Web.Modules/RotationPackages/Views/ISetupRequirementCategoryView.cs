using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.RotationPackages.Views
{
    public interface ISetupRequirementCategoryView
    {
        /// <summary>
        /// Requirement CategoryId
        /// </summary>
        Int32 RequirementCategoryID { get; set; }
        /// <summary>
        /// Requirement Item ID
        /// </summary>
        Int32 RequirementItemID { get; set; }
        /// <summary>
        /// Set or get Item Name.
        /// </summary>
        String CategoryName { get; set; }

        String ErrorMessage { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        Boolean IsEditMode { get; set; }
        List<RequirementCategoryContract> LstCategories { get; set; }
        List<RequirementItemContract> LstCategoryItems { get; set; }
        Boolean IsVersionRequired { get; set; }
        Int32 RequirementPackageId { get; set; }
        String RequirementDocumentLink
        {
            get;
            set;
        }

        String RequirementDocumentLinkLabel { get; set; } //UAT-3161
        String ExplanatoryNotes
        {
            get;
            set;
        }


        bool IsViewOnly { get; set; }

        #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
        Boolean IsComplianceRequired { get; set; }
        DateTime? ComplianceReqStartDate { get; set; }
        DateTime? ComplianceReqEndDate { get; set; }
        #endregion

        #region UAT-2305

        //List<UniversalCategoryContract> lstUniversalCategory { set; }

        //Int32 UniversalCategoryID { get; set; }

        //UniversalCategoryContract UniversalCategoryData { get; set; }

        #endregion

        #region UAT-3805
        Boolean SendItemDocOnApproval { get; set; }
        #endregion
    }
}
