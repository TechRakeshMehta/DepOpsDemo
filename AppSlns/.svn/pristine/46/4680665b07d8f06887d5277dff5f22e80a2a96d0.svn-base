using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using INTSOF.Utils;
using Business;
using CoreWeb.Shell;

/// <summary>
/// Summary description for ComplianceOperations
/// </summary>
public class ComplianceOperationsVerifications
{
    public static void RedirectToVerificationDetailScreen(Int32 selectedTenantId, Int32 itemDataId_Global, WorkQueueType workQueue,
        Int32 packageId, Int32 categoryId, Int32 UserGroupId, Boolean includeCompleteItems, Int32 packageSubscriptionId, Int32 selectedComplianceCategoryId,
        String childControl, String viewType, Boolean ShowOnlyRushOrders, Boolean IsException, Boolean IsEscalationRecords, String SelectedComplianceItemReconciliationDataID, String userMessage = "")
    {
        Dictionary<String, String> queryString = new Dictionary<String, String>();
        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString( selectedTenantId) },
                                                                    { "Child", childControl},
                                                                    { "ItemDataId",Convert.ToString( itemDataId_Global)},
                                                                    {"WorkQueueType",Convert.ToString(workQueue)},
                                                                    {"PackageId",Convert.ToString(packageId) },
                                                                    {"CategoryId",Convert.ToString(categoryId)}, 
                                                                    {"IncludeIncompleteItems",Convert.ToString(includeCompleteItems) },
                                                                    {"SelectedPackageSubscriptionId",Convert.ToString(packageSubscriptionId) },
                                                                    {"SelectedComplianceCategoryId",Convert.ToString(selectedComplianceCategoryId) },
                                                                    {"IsPostBack","1" },
                                                                    {"ShowOnlyRushOrders" , ShowOnlyRushOrders.ToString()},
                                                                    {"UserMessage" , userMessage},
                                                                    {"IsException",IsException.ToString()},
                                                                    { "UserGroupId",Convert.ToString( UserGroupId)},
                                                                    {"IsEscalationRecords", Convert.ToString(IsEscalationRecords)},
                                                                    //UAT-3744
                                                                    {"ComplianceItemReconciliationDataID",SelectedComplianceItemReconciliationDataID}
                                                                 };
        SysXWebSiteUtils.ExceptionService.HandleDebug("The Response. redirect called at" + DateTime.Now.ToShortTimeString());
        HttpContext.Current.Response.Redirect(String.Format("Default.aspx?ucid={0}&args={1}", viewType, queryString.ToEncryptedQueryString()));
    }

    public static String GetVerificationDetailScreenQueryString(Int32 selectedTenantId, Int32 itemDataId_Global, WorkQueueType workQueue,
        Int32 packageId, Int32 categoryId, Int32 UserGroupId, Boolean includeCompleteItems, Int32 packageSubscriptionId, Int32 selectedComplianceCategoryId,
        String childControl, String viewType, Boolean ShowOnlyRushOrders, Boolean IsException, Int32 ApplicantId_Global, String navigationActionType,
        Boolean IsEscalationRecords, String SelectedArchiveStateCode, String SelectedComplianceItemReconciliationDataID)
    {

        Dictionary<String, String> queryString = new Dictionary<String, String>();
        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString( selectedTenantId) },
                                                                    { "Child", childControl},
                                                                    { "ItemDataId",Convert.ToString( itemDataId_Global)},
                                                                    {"WorkQueueType",Convert.ToString(workQueue)},
                                                                    {"PackageId",Convert.ToString(packageId) },
                                                                    {"CategoryId",Convert.ToString(categoryId)}, 
                                                                    {"IncludeIncompleteItems",Convert.ToString(includeCompleteItems) },
                                                                    {"SelectedPackageSubscriptionId",Convert.ToString(packageSubscriptionId) },
                                                                    {"SelectedComplianceCategoryId",Convert.ToString(selectedComplianceCategoryId) },
                                                                    {"IsPostBack","1" },
                                                                    {"ShowOnlyRushOrders" , ShowOnlyRushOrders.ToString()},                                                                   
                                                                    {"IsException",IsException.ToString()},
                                                                    { "UserGroupId",Convert.ToString( UserGroupId)},
                                                                    {"IsCategoryClick","true"},
                                                                    {"ApplicantId",ApplicantId_Global.ToString()},
                                                                    {"ActionType",navigationActionType.ToString()},
                                                                    {"IsEscalationRecords",Convert.ToString(IsEscalationRecords)},
                                                                    {"SelectedArchiveStateCode",SelectedArchiveStateCode},
                                                                    //UAT-3744
                                                                    {"ComplianceItemReconciliationDataID",SelectedComplianceItemReconciliationDataID}
                                                                  // ,{"allowedFileExtensions", String.Join(",",allowedFileExtensions)} //UAT-4067
                                                                 };
        SysXWebSiteUtils.ExceptionService.HandleDebug("The Response. redirect called at" + DateTime.Now.ToShortTimeString());
        //return String.Format("Default.aspx?ucid={0}&args={1}", viewType, queryString.ToEncryptedQueryString());
        return queryString.ToEncryptedQueryString();
    }

}