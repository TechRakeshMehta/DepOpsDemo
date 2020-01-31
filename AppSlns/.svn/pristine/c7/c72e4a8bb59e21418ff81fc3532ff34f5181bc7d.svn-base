<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DRDocsEntityMapping.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.DRDocsEntityMapping" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/BkgOperations/UserControl/DocsEntityMappingTemplate.ascx" TagPrefix="infsu" TagName="DocsEntityMappingTemplate" %>


<div class="msgbox" id="divMesssage">
    <asp:Label Text="" ID="lblMessage" runat="server" Visible="false" />
</div>
<div class="section">
    <h1 class="mhdr">Disclosure and Authorization Mapping</h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel CssClass="sxpnl" runat="server" ID="pnlShowFilters">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class='cptn'>Institution</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox runat="server" ID="cmbTenant" OnDataBound="cmbTenant_DataBound" DataTextField="Name" DataValueField="ID"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxlb'>
                        <span class='cptn'>Country</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox runat="server" ID="cmbCountry" OnDataBound="cmbCountry_DataBound" OnClientSelectedIndexChanged="PopulateBindStateDropdown" 
                           Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" DataTextField="Name" DataValueField="ID">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxlb'>
                        <span class='cptn'>State</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox runat="server" ID="cmbState" MaxHeight="220" OnDataBound="cmbState_DataBound" 
                           Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" DataTextField="Name" DataValueField="ID">
                        </infs:WclComboBox>
                    </div>

                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class='cptn'>Service Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox runat="server" ID="cmbService" OnDataBound="cmbService_DataBound" 
                           Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" DataTextField="Name" DataValueField="ID">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxlb'>
                        <span class='cptn'>Regulatory Entity Type</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox runat="server" ID="cmbRegulatoryEntity" OnDataBound="cmbRegulatoryEntity_DataBound" 
                           Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" DataTextField="Name" DataValueField="ID">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxlb'>
                        <span class='cptn'>D&A Document</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox runat="server" ID="cmbDRDocuments" OnDataBound="cmbDRDocuments_DataBound" 
                           Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" DataTextField="Name" DataValueField="ID">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="fsucOrderCmdBar" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
            AutoPostbackButtons="Submit,Save,Cancel" SaveButtonIconClass="rbSearch" SaveButtonText="Search"
            CancelButtonText="Cancel" SubmitButtonText="Reset" SubmitButtonIconClass="rbRefresh" DefaultPanel="pnlShowFilters"
            OnSubmitClick="CmdBarReset_Click" ValidationGroup="grpFormSubmit" OnSaveClick="CmdBarSearch_Click"
            OnCancelClick="CmdBarCancel_Click">
        </infsu:CommandBar>
    </div>
</div>

<div class="swrap">
    <infs:WclGrid runat="server" ID="grdDRDocumentMapping" AutoGenerateColumns="False"
        OnItemCommand="grdResidentialHistory_ItemCommand" OnItemCreated="grdDRDocumentMapping_ItemCreated" AutoSkinMode="True"
        CellSpacing="0" GridLines="Both" ShowClearFiltersButton="false" OnNeedDataSource="grdDRDocumentMapping_NeedDataSource">
        <ClientSettings EnableRowHoverStyle="true">
            <Selecting AllowRowSelect="true"></Selecting>
        </ClientSettings>
        <ValidationSettings ValidationGroup="grpFormEdit" EnableValidation="true" />
        <MasterTableView CommandItemDisplay="Top" DataKeyNames="DisclosureDocumentMappingId" ValidateRequestMode="Enabled" AllowFilteringByColumn="false">
            <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Mapping" ShowRefreshButton="false" />
            <Columns>
                <telerik:GridBoundColumn DataField="CountryName" FilterControlAltText="Filter CountryName column"
                    HeaderText="Country" SortExpression="CountryName" UniqueName="CountryName">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="StateName" FilterControlAltText="Filter StateName column"
                    HeaderText="State" SortExpression="StateName" UniqueName="StateName">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ServiceName" FilterControlAltText="Filter ServiceName column"
                    HeaderText="Service Name" SortExpression="ServiceName" UniqueName="ServiceName">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="RegulatoryEntityType" FilterControlAltText="Filter RegulatoryEntityType column"
                    HeaderText="Regulatory Entity Type" SortExpression="RegulatoryEntityType" UniqueName="RegulatoryEntityType">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DocumentName" FilterControlAltText="Filter DocumentName column"
                    HeaderText="D&A Document" SortExpression="DocumentName" UniqueName="DocumentName">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="TenantName" FilterControlAltText="Filter TenantName column"
                    HeaderText="Institution" SortExpression="TenantName" UniqueName="TenantName">
                </telerik:GridBoundColumn>
                 <telerik:GridBoundColumn DataField="InstitutionHierarchy" FilterControlAltText="Filter InstitutionHierarchy column"
                    HeaderText="Institution Hierarchy" SortExpression="InstitutionHierarchy" UniqueName="InstitutionHierarchy">
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
                                runat="server" /></h1>

                        <infsu:DocsEntityMappingTemplate runat="server" ID="DocsEntityMappingTemplate" />

                    </div>
                    </div>
                </FormTemplate>
            </EditFormSettings>
        </MasterTableView>
    </infs:WclGrid>
</div>
<div class="gclr">
</div>
<asp:HiddenField ID="hdnValidateInstHierarchy" runat="server" />
<script type="text/javascript">

    function PopulateBindStateDropdown(e) {
        getFreshData(e._value, 'State', 'cmbState', null, null);
    }

    function getFreshData(value, type, controlId, cityId, stateId) {
        if (this.Page != undefined)
            Page.showProgress('Please wait...');
        $jQuery.ajax({
            type: "POST",
            url: '/CommonControls/Default.aspx/GetDataForAddressDropdowns',
            data: "{'searchId': '" + value + "', type: '" + type + "', cityId: '" + cityId + "', stateId: '" + stateId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {
                if (Result != undefined) {
                    BindListToCombo(controlId, Result.d);
                }
            },
            error: function (Result) {
                alert("Error");
            }
        });
        if (this.Page != undefined)
            Page.hideProgress();
    }

    function BindListToCombo(controlId, result) {
        var control = $jQuery("[id$=" + controlId + "]")[0];
        if (control != undefined || control != null) {
            var combo = $find(control.id);
            if (combo != undefined || combo != null) {
                combo.trackChanges();
                var selectedItem = combo.get_selectedItem();
                var items = combo.get_items();
                items.clear();
                //UAT-2833
                var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                comboItem.set_text('--Select--');
                comboItem.set_value(0);
                items.add(comboItem);

                if (result.length > 0) {
                    for (var i = 0; i < result.length; i++) {
                        var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                        comboItem.set_text(result[i].Name);
                        comboItem.set_value(result[i].ID);
                        items.add(comboItem);
                    }
                }
                combo.clearSelection();
                combo.commitChanges();
                //UAT-2833
                var defaultItem = combo.findItemByText("--Select--");
                defaultItem.select();

            }
        }
    }

    function PopulateBindMappingStateDropdown(e) {
        getFreshData(e._value, 'State', 'cmbMapState', null, null);
    }

    function openPopUp() {
        var composeScreenWindowName = "Institution Hierarchy";
        var tenantId = $jQuery("[id$=hdnTenantId]").val();
        if (tenantId != "0" && tenantId != "") {
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/InstitutionHierarchyList.aspx?TenantId=" + tenantId + "&DepartmentProgramId=" + DepartmentProgramId);
            var win = $window.createPopup(url, { size: "500," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
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
                $jQuery("[id$=hdnDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
                $jQuery("[id$=hdnHierarchyLabel]").val(arg.HierarchyLabel);
                $jQuery("[id$=hdnInstitutionNodeId]").val(arg.InstitutionNodeId);
                $jQuery("[id$=lblinstituteHierarchy]")[0].innerHTML = arg.HierarchyLabel;
            }
            winopen = false;
        }
    }

    //UAT-915 - function to validate Institution Hierarchy.
    function pageLoad() {
        var lblValidationMsg = $jQuery("#lblInstHierarchyError");
        if (lblValidationMsg != undefined && lblValidationMsg.length > 0) {
            $jQuery("#lblInstHierarchyError").text("");
        }
        var data = $jQuery("[id$=hdnValidateInstHierarchy]")[0].value;
        if (data != undefined && data == "true") {
            $jQuery("[id$=lblinstituteHierarchy]")[0].textContent = "";
            $jQuery("#lblInstHierarchyError").text("Please select Institution Hierarchy.");
        }
        $jQuery("[id$=hdnValidateInstHierarchy]")[0].value = "";
    }
</script>
