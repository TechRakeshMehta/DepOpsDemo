using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;


namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RequirementVerificationReadOnlyCategoryControl : BaseUserControl, IRequirementVerificationReadOnlyCategoryControlView
    {
        #region Variables

        #endregion

        #region Properties

        /// <summary>
        /// CategoryID
        /// </summary>
        Int32 IRequirementVerificationReadOnlyCategoryControlView.CategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// ApplicantRequirementCategoryDataID
        /// </summary>
        Int32 IRequirementVerificationReadOnlyCategoryControlView.ApplReqCatDataId
        {
            get;
            set;
        }

        /// <summary>
        /// Contains the category level data i.e. Category and all its items
        /// </summary>
        List<RequirementVerificationDetailContract> IRequirementVerificationReadOnlyCategoryControlView.lstCategoryLevelData
        {
            get;
            set;
        }

        /// <summary>
        /// List for 'lkpRequirementItemStatus' entity
        /// </summary>
        List<RequirementItemStatusContract> IRequirementVerificationReadOnlyCategoryControlView.lstReqItemStatusTypes
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the current Context
        /// </summary>
        public IRequirementVerificationReadOnlyCategoryControlView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// prefix for the Item Control
        /// </summary>
        String IRequirementVerificationReadOnlyCategoryControlView.ItemControlIdPrefix
        {
            get
            {
                return "ucItemControl_" + CurrentViewContext.CategoryId + "_";
            }
        }

        /// <summary>
        /// SelectedTenantId
        /// </summary>
        Int32 IRequirementVerificationReadOnlyCategoryControlView.SelectedTenantId
        {
            get;
            set;
        }

        //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes)
        String IRequirementVerificationReadOnlyCategoryControlView.RequirementDocumentLink
        {
            get;
            set;
        }

        //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes)
        String IRequirementVerificationReadOnlyCategoryControlView.CategoryExplanatoryNotes
        {
            get;
            set;
        }

        //UAT-3161
        String IRequirementVerificationReadOnlyCategoryControlView.RequirementDocumentLinkLabel
        {
            get;
            set;
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentViewContext.lstCategoryLevelData.IsNullOrEmpty())
            {
                var _currentCategory = CurrentViewContext.lstCategoryLevelData.First();
                litCatName.Text = _currentCategory.CatName.HtmlEncode();

                # region UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes)
                StringBuilder sb = new StringBuilder();
                //String cat = _currentCategory.CategoryExplanatoryNotes;
                if (!_currentCategory.CategoryExplanatoryNotes.IsNullOrEmpty())
                {
                    sb.Append(_currentCategory.CategoryExplanatoryNotes);
                }

                //Commeneted in UAT-4254
                //if (!_currentCategory.RequirementDocumentLink.IsNullOrEmpty())
                //{
                //    String DocLinkLabel = _currentCategory.RequirementDocumentLinkLabel.IsNullOrEmpty() ? "More Information" : _currentCategory.RequirementDocumentLinkLabel; //UAT-3161
                //    sb.Append("&nbsp");
                //    sb.Append("<br /><a href=\"" + _currentCategory.RequirementDocumentLink + "\" onclick=\"\" target=\"_blank\");'>" + "\r<p>" + DocLinkLabel + "&nbsp" + "</a>");

                //}

                //Added in UAT-4254
                if (!_currentCategory.lstReqCatDocUrls.IsNullOrEmpty() && _currentCategory.lstReqCatDocUrls.Count > AppConsts.NONE)
                {
                    foreach (RequirementCategoryDocUrl catUrl in _currentCategory.lstReqCatDocUrls)
                    {
                        sb.Append("&nbsp");
                        sb.Append("<br /><a href=\"" + catUrl.RequirementCatDocUrl + "\" onclick=\"\" target=\"_blank\");'>" + "\r<p>" + catUrl.RequirementCatDocUrlLabel + "&nbsp" + "</a>");
                    }
                }
                //END

                _currentCategory.CategoryExplanatoryNotes = sb.ToString();
                if (!_currentCategory.CategoryExplanatoryNotes.IsNullOrEmpty())
                {
                    litExplanatoryNotes.Text = String.Format("<span class='expl-title'></span><span class='expl-dur'></span>{0}", _currentCategory.CategoryExplanatoryNotes);

                }
                else
                {
                    litExplanatoryNotes.Text = String.Empty;
                }
                #endregion

                CurrentViewContext.ApplReqCatDataId = CurrentViewContext.lstCategoryLevelData.First().ApplReqCatDataId;
                imgCatStatus.ImageUrl = GetImagePathCategoryStatus(_currentCategory.CatStatusCode);
                imgCatStatus.ToolTip = _currentCategory.CatStatusName;
            }
            GenerateItemControls();
        }

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// Generate the Item level controls
        /// </summary>
        private void GenerateItemControls()
        {
            var _distinctItems = CurrentViewContext.lstCategoryLevelData.Select(vdd => vdd.ItemId).Distinct().ToList();

            foreach (var itemId in _distinctItems)
            {
                System.Web.UI.Control _itemControl = Page.LoadControl("~/ClinicalRotation/UserControl/RequirementVerificationReadOnlyItemControl.ascx");
                (_itemControl as IRequirementVerificationReadOnlyItemControlView).lstItemLevelData = CurrentViewContext.lstCategoryLevelData.Where(vdd => vdd.ItemId == itemId).ToList();
                (_itemControl as IRequirementVerificationReadOnlyItemControlView).lstReqItemStatusTypes = CurrentViewContext.lstReqItemStatusTypes;
                (_itemControl as IRequirementVerificationReadOnlyItemControlView).ItemId = itemId;
                (_itemControl as IRequirementVerificationReadOnlyItemControlView).SelectedTenantId = CurrentViewContext.SelectedTenantId;
                (_itemControl as RequirementVerificationReadOnlyItemControl).ID = CurrentViewContext.ItemControlIdPrefix + itemId;
                pnlItemContainer.Controls.Add(_itemControl);
            }
        }

        /// <summary>
        /// Get the path of the image to be displayed for category
        /// </summary>
        /// <param name="reviewStatus"></param>
        /// <returns></returns>
        private String GetImagePathCategoryStatus(String reviewStatus)
        {
            if (!String.IsNullOrEmpty(reviewStatus))
            {
                if (RequirementCategoryStatus.APPROVED.GetStringValue().Equals(reviewStatus))
                {
                    return "~/Resources/Mod/Compliance/icons/yes16.png";
                }
                else if (RequirementCategoryStatus.INCOMPLETE.GetStringValue().Equals(reviewStatus))
                {
                    return "~/Resources/Mod/Compliance/icons/no16.png";
                }
                else if (RequirementCategoryStatus.PENDING_REVIEW.GetStringValue().Equals(reviewStatus))
                {
                    return "~/Resources/Mod/Compliance/icons/attn16.png";
                }
            }
            return "~/Resources/Mod/Compliance/icons/no16.png";
        }

        #endregion

        #region Public Methods
        #endregion

        #endregion
    }
}