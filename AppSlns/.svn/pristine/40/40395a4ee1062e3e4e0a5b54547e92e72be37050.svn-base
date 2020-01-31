using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml.Linq;
using Business.RepoManagers;
using CoreWeb.Shell;
using Entity;
using Entity.ClientEntity;
using ExternalVendors.ClearStarVendor;
using INTSOF.Contracts;
using INTSOF.UI.Contract;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class ClinicalRotationDefault : BasePage, IDefaultView
    {
        #region VARIABLES

        #region Private Variables

        private DefaultViewPresenter _presenter = new DefaultViewPresenter();

        #endregion

        #endregion

        #region PROPERTIES

        public DefaultViewPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }


        #endregion

        #region EVENTS

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
        }

        /// <summary>
        /// Page OnInitComplete event.
        /// </summary>
        /// <param name="e">Event</param>
        protected override void OnInitComplete(EventArgs e)
        {

            try
            {
                base.dynamicPlaceHolder = phDynamic;
                base.OnInitComplete(e);
                SetModuleTitle("Clinical Rotation");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region PUBLIC METHODS

        [WebMethod]
        public static String GetSingleDocumentForPDFViewer(String documentId, String tenantId)
        {
            if (!documentId.IsNullOrEmpty() && !tenantId.IsNullOrEmpty())
            {
                Dictionary<String, String> requestSingleDocViewerArgs = new Dictionary<String, String>();
                requestSingleDocViewerArgs = new Dictionary<String, String>
                                                                 { 
                                                                    {"SelectedTenantID", tenantId},
                                                                    {"DocumentID",documentId},
                                                                 };
                String _redirectUrl = String.Format(@"/ClinicalRotation/Pages/RequirementVerificationDocViewer.aspx?args={0}", requestSingleDocViewerArgs.ToEncryptedQueryString());
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                return javaScriptSerializer.Serialize(new { redirectUrl = _redirectUrl });
            }
            return null;
        }


        [WebMethod]
        public static String IsInstructorPreceptorRequiredForAgency(String strTenantId, String strAgencyID)
        {
            Boolean isRequired = true;
            Int32 tenantID = 0;
            Int32 agencyID = 0;

            Int32.TryParse(strTenantId, out tenantID);
            Int32.TryParse(strAgencyID, out agencyID);

            if (tenantID > 0 && agencyID > 0)
                isRequired = ClinicalRotationManager.IsPreceptorRequiredForAgency(tenantID, agencyID);

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            return javaScriptSerializer.Serialize(new { IsRequired = isRequired });
        }

        [WebMethod]
        public static String GetAgencyHierarchyRotationFieldOptionSetting(String strTenantId, String strhierarchyId)
        {
            AgencyHierarchyRotationFieldOptionContract requiredFieldSettings = new AgencyHierarchyRotationFieldOptionContract();
            Int32 tenantID = 0;
            Int32 hierarchyID = 0;

            Int32.TryParse(strTenantId, out tenantID);
            //Int32.TryParse(strhierarchyId, out hierarchyID);

            if (tenantID > 0 && !String.IsNullOrEmpty(strhierarchyId))
                requiredFieldSettings = ClinicalRotationManager.GetAgencyHierarchyRotationFieldOptionSetting(tenantID, strhierarchyId);

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            var res = javaScriptSerializer.Serialize(new { RequiredFieldSettings = requiredFieldSettings });
            return javaScriptSerializer.Serialize(new { RequiredFieldSettings = requiredFieldSettings });
        }
        #endregion

        #region performance monitoring
        protected override void InitializeCulture()
        {
            AddLogging("Before Initialize culture");
            base.InitializeCulture();
            AddLogging("After Initialize Culture");

        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            AddLogging("ClinicalRotationDefault pre init");
        }

        public ClinicalRotationDefault()
        {
            AddLogging("ClinicalRotationDefault constructor");
        }

        protected override System.Collections.Specialized.NameValueCollection DeterminePostBackMode()
        {
            AddLogging("Before DeterminePostBackMode");
            var ret = base.DeterminePostBackMode();
            AddLogging("After DeterminePostBackMode");
            return ret;
        }

        private void AddLogging(String step)
        {
            if (HttpContext.Current.Items["LogRequestTime"] == null) return;//logging would work only if it has been initialised by perftracker module.

            String requestLogInfo = String.Empty;
            var currentRequestFullUrl = HttpContext.Current.Request.Url;
            String requestUri = String.Empty;

            if (currentRequestFullUrl.Query.IsNullOrEmpty())
            {
                requestUri = currentRequestFullUrl.AbsoluteUri;
            }
            else
            {
                requestUri = currentRequestFullUrl.AbsoluteUri.Substring(0, currentRequestFullUrl.AbsoluteUri.IndexOf("?"));
            }

            requestLogInfo = HttpContext.Current.Items["LogRequestTime"].ToString() + " " + step + ": " + DateTime.Now.ToString("MMddyyyy hh.mm.ss.ffffff");
            HttpContext.Current.Items["LogRequestTime"] = requestLogInfo;

        }
        #endregion

        [WebMethod]
        public static List<Tuple<Int32, String, String>> GetCategoryUpdatedUrl(String RequirementSubscriptionId, String TenantId, String ClinicalRotationId)
        {
            Int32 RequirementSubscriptionID = Convert.ToInt32(RequirementSubscriptionId);
            Int32 TenantID = Convert.ToInt32(TenantId);
            Int32 ClinicalRotationID = Convert.ToInt32(ClinicalRotationId);

            if (RequirementSubscriptionID <= AppConsts.NONE || TenantID <= AppConsts.NONE || ClinicalRotationID <= AppConsts.NONE)
            {
                return new List<Tuple<int, string, string>>();
            }

            List<Tuple<Int32, String, String>> result = new List<Tuple<Int32, String, String>>();

            List<INTSOF.ServiceDataContracts.Modules.ClinicalRotation.RequirementVerificationDetailContract> lstReqPkgSubData = RequirementVerificationManager.GetRequirementPackageCategoryData(RequirementSubscriptionID, TenantID, ClinicalRotationID);
            
            if (!lstReqPkgSubData.IsNullOrEmpty())
            {
                Int32 reqPackageID = lstReqPkgSubData.FirstOrDefault().PkgId;

                Dictionary<Int32, Boolean> dicComplianceRequiredCategories = ClinicalRotationManager.GetComplianceRequiredRotCatForPackage(TenantID, reqPackageID);

                lstReqPkgSubData = dicComplianceRequiredCategories.IsNullOrEmpty() ? lstReqPkgSubData :
                                                                                         lstReqPkgSubData.Select(slctCat =>
                                                                                         {
                                                                                             slctCat.IsComplianceRequired = dicComplianceRequiredCategories
                                                                                             .FirstOrDefault(crMatch => crMatch.Key == slctCat.CatId).Value; return slctCat;
                                                                                         }
                                                                                         ).ToList();
            }

            foreach (var categoryDetails in lstReqPkgSubData)
            {
                if (!categoryDetails.CatStatusName.IsNullOrEmpty() && !categoryDetails.CatStatusCode.IsNullOrEmpty())
                {
                    var catComplianceStatusName = categoryDetails.CatStatusName;
                    var catComplianceStatus = categoryDetails.CatStatusCode;

                    string url = "";

                    if (catComplianceStatus.IsNullOrEmpty() || catComplianceStatus == RequirementCategoryStatus.INCOMPLETE.GetStringValue())
                    {
                        url = String.Format("../Resources/Mod/Compliance/icons/no16.png");
                    }
                    else if (catComplianceStatus == RequirementCategoryStatus.APPROVED.GetStringValue())
                    {
                        url = String.Format("../Resources/Mod/Compliance/icons/yes16.png");
                    }
                    else if (catComplianceStatus == RequirementCategoryStatus.PENDING_REVIEW.GetStringValue())
                    {
                        url = String.Format("../Resources/Mod/Compliance/icons/attn16.png");
                    }

                    Boolean isComplianceRequired = categoryDetails.IsComplianceRequired;
                    String CategoryRuleStatusID = categoryDetails.CategoryRuleStatusID;

                    if (!isComplianceRequired)
                    {
                        if (CategoryRuleStatusID.Trim() == AppConsts.STR_ONE)
                        {
                            url = String.Format("../Resources/Mod/Compliance/icons/yes16.png");
                        }
                        else
                        {
                            url = String.Format("../Resources/Mod/Compliance/icons/optional.png");
                        }
                    }

                    result.Add(new Tuple<Int32, String, String>(categoryDetails.CatId, url, catComplianceStatusName));
                }
            }

            return result;
        }

        #region UAT-3593
        [WebMethod]
        public static Boolean IsReqDocumentAlreadyUploaded(String documentName, String documentSize, String organizationUserId, Int32 tenantId)
        {
            Int32 _organizationUserID = organizationUserId.IsNullOrEmpty() ? 0 : Convert.ToInt32(organizationUserId);
            if (_organizationUserID > AppConsts.NONE && tenantId > AppConsts.NONE)
            {
                return ComplianceDataManager.IsReqDocumentAlreadyUploaded(documentName, Convert.ToInt32(documentSize), _organizationUserID, tenantId);

            }
            return true;
        }



        #endregion

    }
}
