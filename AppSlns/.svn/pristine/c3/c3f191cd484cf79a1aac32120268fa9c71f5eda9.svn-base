<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManualServiceForms.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.ManualServiceForms" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagName="InstituteHierarchy" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/ComplianceInstitutionHierarchy.ascx" %>


<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblManualServiceForms" runat="server" Text="Manual Service Forms"></asp:Label></h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlShowFilters">
                <div class='sxro sx3co'>
                    <div id="divTenant" runat="server">
                        <div class='sxlb' title="Select the institution whose data you want to view">
                            <span class='cptn'>Institution</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <%--<infs:WclDropDownList ID="ddlTenantName" runat="server" AutoPostBack="true"
                                DataTextField="TenantName" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_ItemSelected"
                                CausesValidation="false" OnDataBound="ddlTenantName_DataBound">
                            </infs:WclDropDownList>--%>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" AutoPostBack="true"
                                DataTextField="TenantName" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_ItemSelected"
                                CausesValidation="false" OnDataBound="ddlTenantName_DataBound" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpManualForm" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                    <div class='sxlb' title="Select Service to restrict search results to those services" runat="server">
                        <span class='cptn'>Service</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="chkServices" runat="server" DataTextField="BSE_Name" DataValueField="BSE_ID"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" EmptyMessage="--Select--">
                        </infs:WclComboBox>
                    </div>

                    <div runat="server">
                        <div class='sxlb' title="Select form status to restrict search results to that form status" runat="server">
                            <span class='cptn'>Form Status</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="cmbFormStatus" runat="server" DataTextField="SFS_Name" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                DataValueField="SFS_ID" EmptyMessage="--Select--">
                            </infs:WclComboBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>

                    <div class='sxro sx3co'>
                        <div class='sxlb' title="Restrict search results to the entered first name">
                            <span class='cptn'>First Name</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtFirstName" runat="server">
                            </infs:WclTextBox>
                        </div>

                        <div class='sxlb' title="Restrict search results to the entered last name">
                            <span class='cptn'>Last Name</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtLastName" runat="server">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>

                    <div class='sxro sx3co'>
                        <div class='sxlb' title="Click the link and select a node to restrict search results to the selected node">
                            <span class='cptn'>Institution Hierarchy</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <a href="#" id="instituteHierarchy" onclick="openPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                        <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="fsucOrderCmdBar" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
            AutoPostbackButtons="Submit,Save,Cancel" SaveButtonIconClass="rbSearch" SaveButtonText="Search"
            CancelButtonText="Cancel" SubmitButtonText="Reset" SubmitButtonIconClass="rbRefresh"
            OnSubmitClick="CmdBarReset_Click" ValidationGroup="grpManualForm" OnSaveClick="CmdBarSearch_Click"
            OnCancelClick="CmdBarCancel_Click">
        </infsu:CommandBar>
    </div>

    <div class="swrap">
        <infs:WclGrid runat="server" ID="grdManualServiceForms" AutoGenerateColumns="False" AllowSorting="True"
            AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0" EnableCustomExporting="true"
            ShowClearFiltersButton="false" GridLines="Both" OnNeedDataSource="grdManualServiceForms_NeedDataSource"
            ShowAllExportButtons="False" OnItemDataBound="grdManualServiceForms_ItemDataBound" OnItemCreated="grdManualServiceForms_ItemCreated"
            OnItemCommand="grdManualServiceForms_ItemCommand" AllowCustomPaging="true" OnSortCommand="grdManualServiceForms_SortCommand" OnUpdateCommand="grdManualServiceForms_UpdateCommand"
            NonExportingColumns="EditCommandColumn">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="false" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrderServiceFormId,NotificationId,SFStatusId,OrganizationUserId,HierarchyNodeID,OrderNumber,ServiceName,SFName,PackageName,ServiceGroupName"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="true"
                    ShowExportToPdfButton="true" ShowExportToCsvButton="true" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="OrderId" FilterControlAltText="Filter OrderId column"
                        AllowFiltering="false" HeaderText="Order ID" UniqueName="OrderIdTemp" HeaderTooltip="This column displays the Order ID for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantFirstName" FilterControlAltText="Filter ApplicantFirstName column"
                        HeaderText="First Name" SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName"
                        HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantLastName" FilterControlAltText="Filter ApplicantLastName column"
                        HeaderText="Last Name" SortExpression="ApplicantLastName" UniqueName="ApplicantLastName"
                        HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantAddress" FilterControlAltText="Filter ApplicantAddress column"
                        HeaderText="Address" SortExpression="ApplicantAddress" UniqueName="ApplicantAddress" HeaderTooltip="This column displays the applicant Address for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="HierarchyLabel" FilterControlAltText="Filter HierarchyLabel column"
                        AllowFiltering="false" HeaderText="Institution Hierarchy" SortExpression="HierarchyLabel"
                        UniqueName="HierarchyLabel" HeaderTooltip="This column displays the institution hierarchy for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SFName" FilterControlAltText="Filter SFName column"
                        HeaderText="Service Form Name" SortExpression="SFName" UniqueName="SFName"
                        HeaderTooltip="This column displays the Service Form Name for each order">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SFStatus" HeaderText="Service Form Status" UniqueName="SFStatus" SortExpression="SFStatus"
                        HeaderTooltip="This column displays the Service Form Status for each order" FilterControlAltText="Filter SFStatus column">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantEmailAddress" FilterControlAltText="Filter ApplicantEmailAddress column"
                        HeaderText="Email Address" SortExpression="ApplicantEmailAddress" UniqueName="ApplicantEmailAddress"
                        HeaderTooltip="This column displays the applicant's email address for each record in the grid" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" />
                    </telerik:GridEditCommandColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div class="section" runat="server" id="divEditBlock" visible="true">
                            <h1 class="mhdr">
                                <asp:Label ID="lblEHAttr" Text="Update Service Form Status"
                                    runat="server" /></h1>
                            <div class="content">
                                <div class="sxform auto">
                                    <div class="msgbox">
                                        <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                    </div>
                                    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlServiceForm">
                                        <div class='sxro sx3co'>
                                            <div class='sxlb'>
                                                <span class="cptn">Service Form Status</span><span class="reqd">*</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclComboBox ID="cmbSvcFrmStatus" runat="server" DataTextField="SFS_Name" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                                    DataValueField="SFS_ID">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvSvcFrmStatus" ControlToValidate="cmbSvcFrmStatus"
                                                        InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpSvcFrm" CssClass="errmsg"
                                                        Text="Service Form Status is required." />
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <infsu:CommandBar ID="fsucCmdBarSvcFrm" runat="server" GridMode="true" DefaultPanel="pnlSvcFrm"
                                    ValidationGroup="grpSvcFrm" GridInsertText="Save" GridUpdateText="Save" />
                            </div>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>

    <div class="gclr">
    </div>
</div>
<asp:HiddenField ID="hfOrderID" runat="server" />
<asp:HiddenField ID="hfClientOrderStatus" runat="server" />
<asp:HiddenField ID="hfTenantId" runat="server" />
<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
<asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
<asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
<asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
<asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />
<script type="text/javascript" language="javascript">
    var winopen = false;

    function openPopUp() {
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "BackgroundScreen";
        //var tenantId = 2;
        var tenantId = $jQuery("#<%= hfTenantId.ClientID %>").val();
        if (tenantId != "0" && tenantId != "") {
            var DelemittedDeptPrgMapIds = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();  //DepartmentProgramId
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/InstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName=" + screenName + "&DelemittedDeptPrgMapIds=" + DelemittedDeptPrgMapIds);
            var win = $window.createPopup(url, { size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
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
                __doPostBack("<%= btnDoPostBack.ClientID %>", "");
            }
            winopen = false;
        }
    }
</script>
