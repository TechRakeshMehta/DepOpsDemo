<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceFormInstitutionMapping.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ServiceFormInstitutionMapping" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%--<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearch.ascx"
    TagName="CustomAttributeLoaderSearch" TagPrefix="uc" %>--%>
<%@ Register Src="~/BkgSetup/UserControl/ServiceFormMappingTemplate.ascx" TagPrefix="infsu" TagName="ServiceFormMappingTemplate" %>

<div class="msgbox" id="divMesssage">
    <asp:Label Text="" ID="lblMessage" runat="server" Visible="false" />
</div>
<div class="section">
    <h1 class="mhdr">Service Form Institution Mapping</h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel CssClass="sxpnl" runat="server" ID="pnlShowFilters">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class='cptn'>Mapping Type</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
 
                        <infs:WclComboBox runat="server" ID="cmbMappingType" CausesValidation="false" AutoPostBack="true" OnDataBound="cmbMappingType_DataBound"
                            Filter="contains" OnClientKeyPressing="openCmbBoxOnTab" DataTextField="MT_Name" DataValueField="MT_ID" OnSelectedIndexChanged="cmbMappingType_SelectedIndexChanged">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvMappingType" ControlToValidate="cmbMappingType"
                                InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Mapping Type is required." />
                        </div>
                    </div>
                    <div class='sxlb'>
                        <span class='cptn'>Service Form Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox runat="server" ID="cmbServiceForm" OnDataBound="cmbServiceForm_DataBound" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            DataTextField="SF_Name" DataValueField="SF_ID">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxlb'>
                        <span class='cptn'>Service Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox runat="server" ID="cmbService" OnDataBound="cmbService_DataBound" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            DataTextField="BSE_Name" DataValueField="BSE_ID">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div id="divTenant" runat="server" visible="false">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class='cptn'>Institution</span>
                        </div>
                        <div class='sxlm'>
                            <%--<infs:WclDropDownList runat="server" ID="cmbTenant" OnDataBound="cmbTenant_DataBound" CausesValidation="false" AutoPostBack="true"
                                OnSelectedIndexChanged="cmbTenant_SelectedIndexChanged" DataTextField="Name" DataValueField="ID">
                            </infs:WclDropDownList>--%>
                            <infs:WclComboBox runat="server" ID="cmbTenant" OnDataBound="cmbTenant_DataBound" CausesValidation="false" AutoPostBack="true"
                                OnSelectedIndexChanged="cmbTenant_SelectedIndexChanged" DataTextField="Name" DataValueField="ID">
                            </infs:WclComboBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb' title="Click the link and select a node to restrict search results to the selected node">
                            <span class='cptn'>Institution Hierarchy</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <a href="#" id="instituteHierarchyParent" onclick="openPopUp(false);">Select Institution Hierarchy</a>&nbsp;&nbsp
                            <asp:Label ID="lblParentinstituteHierarchy" runat="server"></asp:Label>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <%--<uc:CustomAttributeLoaderSearch ID="ucCustomAttributeLoaderSearch" runat="server" />--%>
                </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="fsucOrderCmdBar" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
            AutoPostbackButtons="Submit,Save,Cancel" SaveButtonIconClass="rbSearch" SaveButtonText="Search"
            CancelButtonText="Cancel" SubmitButtonText="Reset" SubmitButtonIconClass="rbRefresh" DefaultPanel="pnlShowFilters"
            OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click"
            OnCancelClick="CmdBarCancel_Click">
        </infsu:CommandBar>
    </div>
    <div class="swrap">
        <infs:WclGrid runat="server" ID="grdServiceFormInstitutionMapping" AutoGenerateColumns="False" AutoSkinMode="True" AllowPaging="true" AllowSorting="false"
            CellSpacing="0" GridLines="Both" ShowClearFiltersButton="false" OnItemCreated="grdServiceFormInstitutionMapping_ItemCreated"
            OnNeedDataSource="grdServiceFormInstitutionMapping_NeedDataSource" OnItemCommand="grdServiceFormInstitutionMapping_ItemCommand">
            <ValidationSettings ValidationGroup="grpFormEdit" EnableValidation="true" />
            <MasterTableView CommandItemDisplay="Top" AllowPaging="false" PagerStyle-Visible="true" AllowSorting="false" AllowFilteringByColumn="false"
                DataKeyNames="SFM_ID,SF_ID,BSE_ID,SAFHM_ID,DPM_ID">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Mapping" ShowRefreshButton="false" />
                <Columns>
                    <telerik:GridBoundColumn DataField="ServiceFormName" FilterControlAltText="Filter ServiceFormName column"
                        HeaderText="Service Form Name" SortExpression="ServiceFormName" UniqueName="ServiceFormName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ServiceName" FilterControlAltText="Filter ServiceName column"
                        HeaderText="Service Name" SortExpression="ServiceName" UniqueName="ServiceName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="DPM_Label" FilterControlAltText="Filter DPM_Label column"
                        HeaderText="Institution Hierarchy" SortExpression="DPM_Label" UniqueName="DPM_Label">
                    </telerik:GridBoundColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" />
                    </telerik:GridEditCommandColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this mapping?"
                        Text="Delete" UniqueName="DeleteColumn" HeaderStyle-Width="5%">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div class="section" runat="server" visible="true" id="divEditFormBlock">
                            <h1 class="mhdr">
                                <asp:Label ID="lblEHPrevAddress" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Mapping" : "Update Mapping" %>'
                                    runat="server" />
                            </h1>
                            <infsu:ServiceFormMappingTemplate runat="server" ID="ServiceFormMappingTemplate" />
                        </div>
                    </FormTemplate>
                </EditFormSettings>
            </MasterTableView>
        </infs:WclGrid>
    </div>
    <div class="gclr">
    </div>

</div>
<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
<asp:HiddenField ID="hdnParentTenantId" runat="server" Value="" />
<asp:HiddenField ID="hdnParentDepartmntPrgrmMppng" runat="server" Value="" />
<asp:HiddenField ID="hdnParentHierarchyLabel" runat="server" Value="" />

<script type="text/javascript">
    var winopen = false;
    var set = false;
    function openPopUp(setValue) {
        set = setValue;
        var composeScreenWindowName = "Institution Hierarchy";
        if (set) {
            var tenantId = $jQuery("[id$=hdnChildTenantId]").val();
        }
        else {
            var tenantId = $jQuery("[id$=hdnParentTenantId]").val();
        }
        if (tenantId != "0" && tenantId != "") {
            if (set) {
                //value for the Child Page
                var DepartmentProgramId = $jQuery("[id$=hdnChildDepartmntPrgrmMppng]").val();
            }
            else {
                //value for the parent page
                var DepartmentProgramId = $jQuery("[id$=hdnParentDepartmntPrgrmMppng]").val();
            }
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/InstitutionHierarchyList.aspx?TenantId=" + tenantId + "&DepartmentProgramId=" + DepartmentProgramId);
            var win = $window.createPopup(url, { size: "500,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
        }
        return false;
    }

    function OnClientClose(oWnd, args) {

        oWnd.remove_close(OnClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                if (set == true) {
                    //value for the Child Page
                    $jQuery("[id$=hdnChildDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
                    $jQuery("[id$=hdnChildHierarchyLabel]").val(arg.HierarchyLabel);
                    $jQuery("[id$=lblChildinstituteHierarchy]")[0].innerHTML = arg.HierarchyLabel;
                }
                else {
                    //value for the Parent Page
                    $jQuery("[id$=hdnParentDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
                    $jQuery("[id$=hdnParentHierarchyLabel]").val(arg.HierarchyLabel);
                    $jQuery("[id$=lblParentinstituteHierarchy]")[0].innerHTML = arg.HierarchyLabel;
                }

                //__doPostBack("<%= btnDoPostBack.ClientID %>", "");

                //$jQuery("[id$=hdnDepartmntProgramMapping]").val(arg.DepPrgMappingId);
                //$jQuery("[id$=hdnNodeHierarchyLabel]").val(arg.HierarchyLabel);
                //$jQuery("[id$=hdnHrchyInstitutionNodeId]").val(arg.InstitutionNodeId);
                //$jQuery("[id$=lblNodeinstituteHierarchy")[0].innerHTML = arg.HierarchyLabel;
            }
            winopen = false;
        }
    }
</script>
