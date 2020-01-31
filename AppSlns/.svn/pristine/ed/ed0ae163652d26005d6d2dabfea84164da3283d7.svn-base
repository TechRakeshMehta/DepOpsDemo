#region Namespaces

#region System Defined
using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Collections;
using System.Linq;

#endregion

#region UserDefined
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using Newtonsoft.Json;
using System.Web.UI;
#endregion
#endregion

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class AgencyHierarchyMultipleSelection : BaseUserControl, IAgencyHierarchyMultipleSelectionView
    {
        #region Variables

        #region Private Variables
        private AgencyHierarchyMultipleSelectionPresenter _presenter = new AgencyHierarchyMultipleSelectionPresenter();
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties
        public IAgencyHierarchyMultipleSelectionView CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        public Dictionary<String, Object> AgencyHierarchyCollection
        {
            get
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return String.IsNullOrEmpty(hdnAgencyHierarchyJsonObj.Value) ? new Dictionary<String, Object>() : serializer.Deserialize<Dictionary<String, Object>>(hdnAgencyHierarchyJsonObj.Value);
            }
        }
        public AgencyHierarchyMultipleSelectionPresenter Presenter
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
        public Int32 TenantId
        {
            get
            {
                return String.IsNullOrEmpty(hdnTenantId.Value) ? 0 : Convert.ToInt32(hdnTenantId.Value);
            }
            set
            {
                hdnTenantId.Value = value.ToString();
            }

        }
        public Int32 CurrentOrgUserId
        {
            get
            {
                return Convert.ToInt32(hdnCurrentOrgUserID.Value);
            }
            set
            {

                hdnCurrentOrgUserID.Value = value.ToString();
            }
        }
        public Boolean AgencyHierarchyNodeSelection
        {
            get
            {
                return String.IsNullOrEmpty(hdnAgencyHierarchyNodeSelection.Value) ? false : Convert.ToBoolean(hdnAgencyHierarchyNodeSelection.Value);
            }
            set
            {

                hdnAgencyHierarchyNodeSelection.Value = value.ToString();
            }
        }
        public Boolean NodeHierarchySelection
        {
            get
            {
                return String.IsNullOrEmpty(hdnNodeHierarchySelection.Value) ? false : Convert.ToBoolean(hdnNodeHierarchySelection.Value);
            }
            set
            {
                hdnNodeHierarchySelection.Value = value.ToString();
            }
        }
        public String AgencyHierarchyIds
        {
            get
            {
                return String.IsNullOrEmpty(AgencyHierarchyNodeIds.Value) ? String.Empty : Convert.ToString(AgencyHierarchyNodeIds.Value);
            }
            set
            {
                AgencyHierarchyNodeIds.Value = value.ToString();
            }
        }
        public Boolean IsInDisableMode
        {
            get
            {
                return String.IsNullOrEmpty(hdnIsDisabledMode.Value) ? false : Convert.ToBoolean(hdnIsDisabledMode.Value);
            }
            set
            {
                hdnIsDisabledMode.Value = value.ToString();
            }
        }
        public Int32 SelectedRootNodeId
        {
            get
            {
                return String.IsNullOrEmpty(hdnSelectedRootNodeId.Value) ? 0 : Convert.ToInt32(hdnSelectedRootNodeId.Value);
            }
            set
            {
                hdnSelectedRootNodeId.Value = value.ToString();
            }
        }
        public String SelectedNodeIds
        {
            get
            {
                return String.IsNullOrEmpty(hdnSelectedNodeIds.Value) ? String.Empty : Convert.ToString(hdnSelectedNodeIds.Value);
            }
            set
            {
                hdnSelectedNodeIds.Value = value.ToString();
            }
        }
        public String SelectedAgecnyIds
        {
            get
            {
                return String.IsNullOrEmpty(hdnSelectedAgecnyIds.Value) ? String.Empty : Convert.ToString(hdnSelectedAgecnyIds.Value);
            }
            set
            {
                hdnSelectedAgecnyIds.Value = value.ToString();
            }
        }
        public String Hierarchylabel
        {
            get
            {
                return Convert.ToString(ViewState["Hierarchylabel"]).HtmlEncode();
            }
            set
            {
                ViewState["Hierarchylabel"] = value;
            }
        }
        public Boolean IsAllNodeDisabledMode
        {
            get
            {
                return String.IsNullOrEmpty(hdnIsAllNodeDisabledMode.Value) ? false : Convert.ToBoolean(hdnIsAllNodeDisabledMode.Value);
            }
            set
            {
                hdnIsAllNodeDisabledMode.Value = value.ToString();
            }
        }
        public Boolean IsParentDisable
        {
            get
            {
                return String.IsNullOrEmpty(hdnIsParentDisable.Value) ? false : Convert.ToBoolean(hdnIsParentDisable.Value);
            }
            set
            {
                hdnIsParentDisable.Value = value.ToString();
            }
        }
        public Dictionary<Int32, String> SelectedAgencyHierarchyDetails { get; set; }
        public Boolean IsInstitutionHierarchyRequired
        {
            get
            {
                return String.IsNullOrWhiteSpace(hdnIsInstitutionHierarchyRequired.Value) ? false : Convert.ToBoolean(hdnIsInstitutionHierarchyRequired.Value);
            }
            set
            {
                hdnIsInstitutionHierarchyRequired.Value = value.ToString();
            }
        }
        public String SelectedInstitutionNodeIds
        {
            get
            {
                return String.IsNullOrEmpty(hdnInstitutionNodeIds.Value) ? String.Empty : Convert.ToString(hdnInstitutionNodeIds.Value);
            }

            set
            {
                hdnInstitutionNodeIds.Value = value.ToString();
            }
        }

        public Boolean IsAgencyNodeCheckable
        {
            get
            {
                return String.IsNullOrWhiteSpace(hdnIsAgencyNodeCheckable.Value) ? false : Convert.ToBoolean(hdnIsAgencyNodeCheckable.Value);
            }
            set
            {
                hdnIsAgencyNodeCheckable.Value = value.ToString();
            }
        }

        //UAT-3494
        public Boolean IsRotationPkgCopyFromAgencyHierarchy
        {
            get
            {
                return String.IsNullOrWhiteSpace(hdnIsRotationPkgCopyFromAgencyHierarchy.Value) ? false : Convert.ToBoolean(hdnIsRotationPkgCopyFromAgencyHierarchy.Value);
            }
            set
            {
                hdnIsRotationPkgCopyFromAgencyHierarchy.Value = value.ToString();
            }
        }
        public Boolean IsChildTreeNodeChecked
        {
            get
            {
                return String.IsNullOrWhiteSpace(hdnIsChildTreeNodeChecked.Value) ? false : Convert.ToBoolean(hdnIsChildTreeNodeChecked.Value);
            }
            set
            {
                hdnIsChildTreeNodeChecked.Value = value.ToString();
            }
        }

        public Boolean IsChildBackButtonDisabled
        {
            get
            {
                return String.IsNullOrEmpty(hdnIsChildBackButtonDisabled.Value) ? false : Convert.ToBoolean(hdnIsChildBackButtonDisabled.Value);
            }
            set
            {
                hdnIsChildBackButtonDisabled.Value = value.ToString();
            }
        }

        public Boolean AddDisableStyle
        {
            set
            {
                hdnAddDisabledStyle.Value = Convert.ToString(value);
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events
        protected void Page_PreRender(object sender, EventArgs e)
        {
            BindTree();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
            GetAgencyHierarchyCollection();
            lblAgencyHierarchy.Text = Hierarchylabel;
        }
        #endregion

        #endregion

        #region Methods

        #region Public Methods
        /// <summary>
        /// GetAgencyHierarchyCollection
        /// </summary>
        /// <returns></returns>
        public AgencyhierarchyCollection GetAgencyHierarchyCollection()
        {
            Dictionary<String, Object> dictionayObj = CurrentViewContext.AgencyHierarchyCollection;
            AgencyhierarchyCollection agencyHierarchyList = new AgencyhierarchyCollection();
            agencyHierarchyList.agencyhierarchy = new List<Agencyhierarchy>();
            if (dictionayObj.Count > 0)
            {
                foreach (KeyValuePair<String, Object> item in dictionayObj)
                {
                    String AgencyHierarchyLabel = String.Empty;
                    Int32 AgencyID = 0;
                    Int32 AgencyNodeID = 0;
                    Object obj = (Object)item.Value;

                    if (obj is IList)
                    {
                        foreach (var objList in (System.Collections.ArrayList)obj)
                        {
                            Agencyhierarchy agencyHierarchy = new Agencyhierarchy();
                            foreach (KeyValuePair<String, Object> dataItem in (Dictionary<String, Object>)objList)
                            {
                                AgencyHierarchyLabel = dataItem.Key.Contains("AgencyHierarchyLabel") ? Convert.ToString(dataItem.Value) : AgencyHierarchyLabel;
                                AgencyID = dataItem.Key.Contains("AgencyID") ? Convert.ToInt32(dataItem.Value) : AgencyID;
                                AgencyNodeID = dataItem.Key.Contains("AgencyNodeID") ? Convert.ToInt32(dataItem.Value) : AgencyNodeID;
                            }
                            agencyHierarchy.AgencyHierarchyLabel = AgencyHierarchyLabel;
                            agencyHierarchy.AgencyID = AgencyID;
                            agencyHierarchy.AgencyNodeID = AgencyNodeID;
                            agencyHierarchyList.agencyhierarchy.Add(agencyHierarchy);
                        }

                    }
                    else
                    {
                        Agencyhierarchy agencyHierarchy = new Agencyhierarchy();
                        foreach (KeyValuePair<String, Object> dataItem in (Dictionary<String, Object>)obj)
                        {
                            AgencyHierarchyLabel = dataItem.Key.Contains("AgencyHierarchyLabel") ? Convert.ToString(dataItem.Value) : AgencyHierarchyLabel;
                            AgencyID = dataItem.Key.Contains("AgencyID") ? Convert.ToInt32(dataItem.Value) : AgencyID;
                            AgencyNodeID = dataItem.Key.Contains("AgencyNodeID") ? Convert.ToInt32(dataItem.Value) : AgencyNodeID;
                        }
                        agencyHierarchy.AgencyHierarchyLabel = AgencyHierarchyLabel;
                        agencyHierarchy.AgencyID = AgencyID;
                        agencyHierarchy.AgencyNodeID = AgencyNodeID;
                        agencyHierarchyList.agencyhierarchy.Add(agencyHierarchy);
                    }
                }
            }
            if (agencyHierarchyList.agencyhierarchy.Count > 0)
            {
                Hierarchylabel = String.Join(", ", agencyHierarchyList.agencyhierarchy.Select(x => x.AgencyHierarchyLabel).Distinct());
                if (IsRotationPkgCopyFromAgencyHierarchy)
                {
                    if (!IsChildTreeNodeChecked)
                    {
                        Hierarchylabel = String.Empty;
                        agencyHierarchyList.agencyhierarchy = new List<Agencyhierarchy>();
                    }
                }
            }
            else
            {
                Hierarchylabel = String.Empty;
            }
            return agencyHierarchyList;
        }
        /// <summary>
        /// BindTree
        /// </summary>
        public void BindTree()
        {
            if (!String.IsNullOrEmpty(SelectedNodeIds)
                && (SelectedRootNodeId.IsNullOrEmpty()
                    || SelectedRootNodeId == AppConsts.NONE))
            {
                String rootNodeId = Presenter.GetAgencyHierarchyParent();
                SelectedRootNodeId = Convert.ToInt32(rootNodeId);
            }
            if (!String.IsNullOrEmpty(SelectedNodeIds))
            {
                String jsonObj = String.Empty;
                if (!SelectedAgecnyIds.IsNullOrEmpty())
                {
                    if (Session["AgencySelected"] == null
                        || !Convert.ToString(Session["AgencySelected"]).IsNullOrEmpty())
                    {
                        SelectedAgecnyIds = Convert.ToString(Session["AgencySelected"]);
                    }
                    if (Session["NodeSelected"] == null
                        || !Convert.ToString(Session["NodeSelected"]).IsNullOrEmpty())
                    {
                        SelectedNodeIds = Convert.ToString(Session["NodeSelected"]);
                    }
                }
                String HierarchySelectionType = SelectedAgecnyIds.IsNullOrEmpty() ? "NHS" : "AHNS";
                String resultXML = Presenter.GetAgencyDetailByMultipleNodeIds(SelectedNodeIds, SelectedAgecnyIds, HierarchySelectionType);
                if (!String.IsNullOrEmpty(resultXML))
                {
                    System.Xml.Linq.XDocument input = System.Xml.Linq.XDocument.Parse(resultXML);
                    jsonObj = JsonConvert.SerializeXNode(input, Newtonsoft.Json.Formatting.Indented, true);
                }
                hdnAgencyHierarchyJsonObj.Value = jsonObj;
                GetAgencyHierarchyCollection();
                if (IsRotationPkgCopyFromAgencyHierarchy)
                {
                    if (!IsChildTreeNodeChecked)
                    {
                        lblAgencyHierarchy.Text = Hierarchylabel;
                    }
                    else
                    {
                        //lblAgencyHierarchy.Text = String.Empty;

                    }
                }
                else
                    lblAgencyHierarchy.Text = Hierarchylabel;
            }

        }
        /// <summary>
        /// Rebind
        /// </summary>
        public void Rebind()
        {
            GetAgencyHierarchyCollection();
            lblAgencyHierarchy.Text = Hierarchylabel;
        }
        /// <summary>
        /// Reset
        /// </summary>
        public void Reset()
        {
            hdnSelectedAgecnyIds.Value = String.Empty;
            hdnSelectedNodeIds.Value = String.Empty;
            hdnSelectedRootNodeId.Value = String.Empty;
            AgencyHierarchyNodeIds.Value = String.Empty;
            lblAgencyHierarchy.Text = String.Empty;
            hdnAgencyHierarchyJsonObj.Value = String.Empty;
        }

        #region UAT-2916

        public List<String> GetAgencyHierarchyAgencyCollection()
        {
            return Presenter.GetAgencyHierarchyAgencyByMultipleNodeIds(SelectedNodeIds);
        }
        #endregion

        #endregion

        #endregion
    }
}
