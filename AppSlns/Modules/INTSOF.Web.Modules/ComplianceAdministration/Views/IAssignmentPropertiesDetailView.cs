using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IAssignmentPropertiesDetailView
    {
        List<lkpEditableBy> lstEditableBy
        {
            get;
            set;
        }

        Int32 ApplicantEditableByID
        {
            get;
            set;
        }

        List<lkpReviewerType> lstReviewerType
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId { get; }

        Int32 TenantId { get; set; }

        Int32 CurrentDataID
        {
            get;
            set;
        }

        Int32 ParentCategoryDataID
        {
            get;
            set;
        }

        Int32 ParentPackageDataID
        {
            get;
            set;
        }

        Int32 ParentItemDataID
        {
            get;
            set;
        }

        String CurrentRuleSetTreeTypeCode
        {
            get;
            set;
        }

        AssignmentProperty AssignmentPropertyDetails
        {
            get;
            set;
        }

        List<Tenant> LstThirdPartyReviewer
        {
            get;
            set;
        }

        Int32 selectedReviewerId
        {
            get;
            set;
        }

        List<Entity.OrganizationUser> LstThirdPartyUser
        {
            get;
            set;
        }
    }
}




