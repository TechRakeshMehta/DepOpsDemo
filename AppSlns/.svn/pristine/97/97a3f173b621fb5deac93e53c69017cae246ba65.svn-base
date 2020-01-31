using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Text;
using CoreWeb.ComplianceOperations.Views;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;


namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ItemDetails : BaseUserControl, IItemDetailsView
    {
        private ItemDetailsPresenter _presenter=new ItemDetailsPresenter();
        private Int32 _attributesPerRow;
        private ApplicantComplianceAttributeDataContract _viewContract;

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        public Boolean ReadOnly
        {
            get;
            set;
        }

        public String ItemName
        {
            get;
            set;
        }

        public List<Entity.ClientEntities.ClientComplianceItemAttribute> ClientItemAttributes
        {
            get;
            set;
        }

        public IItemDetailsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 ItemId
        {
            get;
            set;
        }

        public ApplicantComplianceAttributeDataContract ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ApplicantComplianceAttributeDataContract();
                }
                return _viewContract;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public Int32 TenantId
        {
            get;
            set;
        }

        public List<ApplicantComplianceAttributeDataContract> lstAttributesData
        {
            get;
            set;
        }

        public Int32 PackageSubscriptionId
        {
            get;
            set;
        }

        public ApplicantComplianceItemDataContract ItemDataContract
        {
            get;
            set;
        }

        public ApplicantComplianceCategoryDataContract CategoryDataContract
        {
            get;
            set;
        }

        public Int32 ComplianceCategoryId
        {
            get;
            set;
        }

        public Dictionary<Int32, Int32> AttributeDocuments
        {
            get;
            set;
        }

        public Boolean SaveStatus
        {
            get;
            set;
        }

        public Entity.ClientEntities.ApplicantComplianceItemData ApplicantItemData
        {
            get;
            set;
        }

        public Int32 ApplicantComplianceCategoryIdGenerated
        {
            get;
            set;
        }

        public Int32 ApplicantComplianceItemIdGenerated
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region Variables

        #region Private Variables

        #endregion

        #region Public Variables

        #endregion

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.IsPostBack)
            //{
            //    this._presenter.OnViewInitialized();
            //    if (CurrentViewContext.ApplicantItemData.IsNotNull())
            //        hdfApplicantItemDataId.Value = Convert.ToString(CurrentViewContext.ApplicantItemData.ApplicantComplianceItemDataID);
            //}
            //this._presenter.OnViewLoaded();

            //_attributesPerRow = 2;

            //lblFormItemName.Text = ItemName;
            //CurrentViewContext.ClientItemAttributes = ClientItemAttributes.OrderBy(att => att.DisplayOrder).ToList();
            //List<Entity.ClientEntities.ClientComplianceItemAttribute> lst =
            //    CurrentViewContext.ClientItemAttributes.Take(_attributesPerRow).ToList();

            //List<Int32> lstTemporary = new List<int>();
            ////lstTemporary.AddRange(lst.Select(attribute => attribute.ClientComplianceItemAttributeID));

            //int attributesAdded = 0;
            //for (int i = 1; i <= Math.Ceiling(Convert.ToDecimal(CurrentViewContext.ClientItemAttributes.Count()) / _attributesPerRow); i++)
            //{
            //    if (attributesAdded == _attributesPerRow)
            //    {
            //        attributesAdded = 0;
            //        lst = new List<Entity.ClientEntities.ClientComplianceItemAttribute>();

            //        foreach (var att in CurrentViewContext.ClientItemAttributes)
            //        {
            //            if (!lstTemporary.Contains(att.ClientComplianceItemAttributeID))
            //            {
            //                lst.Add(att);
            //            }
            //        }
            //    }
            //    lst = lst.Take(_attributesPerRow).ToList();
            //    lstTemporary.AddRange(lst.Select(att => att.ClientComplianceItemAttributeID));
            //    AddRow(lst);
            //    attributesAdded += _attributesPerRow;
            //}
            //fsucCmdBar1.ValidationGroup = "vGroup_" + ItemId.ToString();


        }
        private void AddRow(List<Entity.ClientEntities.ClientComplianceItemAttribute> lstAttributes)
        {
            //System.Web.UI.Control attributeRow = Page.LoadControl("~\\ComplianceOperations\\UserControl\\RowControl.ascx");
            //(attributeRow as RowControl).ClientItemAttributes = lstAttributes;
            //(attributeRow as RowControl).NoOfAttributesPerRow = _attributesPerRow;
            //(attributeRow as RowControl).ItemId = CurrentViewContext.ItemId;
            //(attributeRow as RowControl).TenantId = CurrentViewContext.TenantId;

            //if (CurrentViewContext.ApplicantItemData.IsNotNull() && CurrentViewContext.ApplicantItemData.ApplicantComplianceAttributeDatas.Count() > 0)
            //{
            //    (attributeRow as RowControl).ApplicantComplianceItemId = CurrentViewContext.ApplicantItemData.ApplicantComplianceItemDataID;
            //    (attributeRow as RowControl).ApplicantAttributeData = CurrentViewContext.ApplicantItemData.ApplicantComplianceAttributeDatas.ToList();
            //}

            //pnl.Controls.Add(attributeRow);
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            //HiddenField hdf = GetHiddenField();

            //CurrentViewContext.CategoryDataContract = new ApplicantComplianceCategoryDataContract();
            //CurrentViewContext.CategoryDataContract.PackageSubscriptionId = CurrentViewContext.PackageSubscriptionId;
            //CurrentViewContext.CategoryDataContract.ComplianceCategoryId = CurrentViewContext.ComplianceCategoryId;
            //CurrentViewContext.CategoryDataContract.ReviewStatusTypeCode = ApplicantCategoryComplianceStatus.Incomplete.GetStringValue();
            //CurrentViewContext.CategoryDataContract.Notes = "Test Category Notes ";
            //CurrentViewContext.CategoryDataContract.ApplicantComplianceCategoryId = String.IsNullOrEmpty(hdf.Value) ? AppConsts.NONE : Convert.ToInt32(hdf.Value); ;

            //CurrentViewContext.ItemDataContract = new ApplicantComplianceItemDataContract();
            //CurrentViewContext.ItemDataContract.ApplicantComplianceItemId = String.IsNullOrEmpty(hdfApplicantItemDataId.Value) ? AppConsts.NONE : Convert.ToInt32(hdfApplicantItemDataId.Value); ;
            //CurrentViewContext.ItemDataContract.ComplianceItemId = CurrentViewContext.ItemId;
            //CurrentViewContext.ItemDataContract.ReviewStatusTypeCode = ApplicantItemComplianceStatus.Pending_Review.GetStringValue();
            //CurrentViewContext.ItemDataContract.Notes = "Test Item Notes";

            //lstAttributesData = new List<ApplicantComplianceAttributeDataContract>();

            //foreach (Control rowControl in pnl.Controls)
            //{
            //    if (rowControl.GetType().BaseType == typeof(RowControl))
            //    {
            //        foreach (var attributeControl in rowControl.Controls)
            //        {
            //            if (attributeControl.GetType().BaseType == typeof(HtmlContainerControl))
            //            {
            //                foreach (var ctrl in (attributeControl as HtmlContainerControl).Controls)
            //                {
            //                    if (ctrl.GetType().BaseType == typeof(AttributeControl))
            //                    {
            //                        Panel attrPanel = ((ctrl as AttributeControl).FindControl("pnlControls") as Panel);
            //                        if (attrPanel != null)
            //                        {

            //                            foreach (var ctrlType in attrPanel.Controls)
            //                            {
            //                                Type baseControlType = ctrlType.GetType();
            //                                String attributeValue = String.Empty;

            //                                if (baseControlType == typeof(WclTextBox))
            //                                {
            //                                    attributeValue = (ctrlType as WclTextBox).Text;
            //                                    AddDataToList(ctrl, attributeValue);
            //                                }
            //                                else if (baseControlType == typeof(WclNumericTextBox))
            //                                {
            //                                    attributeValue = (ctrlType as WclNumericTextBox).Text;
            //                                    AddDataToList(ctrl, attributeValue);
            //                                }
            //                                else if (baseControlType == typeof(WclComboBox))
            //                                {
            //                                    if (!(ctrlType as WclComboBox).CheckBoxes)
            //                                    {
            //                                        attributeValue = (ctrlType as WclComboBox).SelectedValue;
            //                                        AddDataToList(ctrl, attributeValue);
            //                                    }
            //                                    else
            //                                    {
            //                                        CurrentViewContext.AttributeDocuments = new Dictionary<Int32, Int32>();
            //                                        foreach (var checkedDocument in (ctrlType as WclComboBox).CheckedItems)
            //                                        {
            //                                            CurrentViewContext.AttributeDocuments.Add(Convert.ToInt32(checkedDocument.Value), (ctrl as AttributeControl).ClientItemAttributes.ClientComplianceItemAttributeID);
            //                                        }
            //                                        AddDataToList(ctrl, String.Empty);
            //                                    }
            //                                }
            //                                else if (baseControlType == typeof(WclDatePicker))
            //                                {
            //                                    attributeValue = Convert.ToString((ctrlType as WclDatePicker).SelectedDate);
            //                                    AddDataToList(ctrl, attributeValue);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //Presenter.SaveApplicantComplianceAttributeData();

            //// Save & assign the ApplicantCategoryDataId & ApplicantComplianceItemIdGenerated for the further operations, by other items on page
            //hdf.Value = Convert.ToString(CurrentViewContext.ApplicantComplianceCategoryIdGenerated);
            //hdfApplicantItemDataId.Value = Convert.ToString(CurrentViewContext.ApplicantComplianceItemIdGenerated);

            //if (CurrentViewContext.SaveStatus)
            //{
            //    lbl.Text = "Item has been saved successfully.";
            //}
            //else
            //{
            //    lbl.Text = "Could not save the Item data.";
            //}
            //this.Page.GetType().InvokeMember("GenerateForm", System.Reflection.BindingFlags.InvokeMethod, null, this.Page, new object[] { true });
        }

        private void AddDataToList(object ctrl, String attributeValue)
        {
            //// Get the 'ApplicantComplianceAttributeId' whuile updating 
            //Control ucAttributes = (ctrl as AttributeControl) as Control;
            //Control hdfAttribute = DAL.Extensions.FindControlRecursive(ucAttributes, "hdfClientAttributeDataId");

            //lstAttributesData.Add(new ApplicantComplianceAttributeDataContract
            //{
            //    ApplicantComplianceAttributeId = String.IsNullOrEmpty((hdfAttribute as HiddenField).Value) ? AppConsts.NONE : Convert.ToInt32((hdfAttribute as HiddenField).Value),
            //    ApplicantComplianceItemId = CurrentViewContext.ItemId,
            //    ComplianceItemAttributeId = (ctrl as AttributeControl).ClientItemAttributes.ClientComplianceItemAttributeID,
            //    AttributeValue = attributeValue
            //});
        }
        private HiddenField GetHiddenField()
        {
            HiddenField hdf = null;
            Control ctl = this.Parent;
            while (true)
            {
                hdf = (HiddenField)ctl.FindControl("hdf");
                if (hdf.IsNull())
                {
                    if (ctl.Parent == null)
                        return hdf;
                    ctl = ctl.Parent;
                    continue;
                }
                return hdf;
            }
        }
        
        public ItemDetailsPresenter Presenter
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

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	

    }
}


