<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageContracts.ascx.cs"
    Inherits="CoreWeb.ContractManagement.Views.ManageContracts" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<%@ Register TagPrefix="uc" TagName="ManageSites" Src="~/ContractManagement/UserControl/ManageSites.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rmManageContracts">
    <infs:LinkedResource Path="~/Resources/Mod/ContractManagement/ManageContract.js"
        ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<script type="text/javascript"> 
    function DownloadForm(url) {
        //debugger;
        location.href = url;
    }
</script>

<asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
<asp:HiddenField ID="hdnTenantIdNew" runat="server" Value="" />
<asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
<asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
<asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />
<asp:HiddenField ID="hdnDPMGridMode" runat="server" Value="" />
<asp:HiddenField ID="hdnInstitutionHierarchyGridMode" runat="server" Value="" />
<asp:HiddenField ID="hdnInstNodeIdGridMode" runat="server" Value="" />
<asp:HiddenField ID="hdnDocumentFileName" runat="server" />

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Manage Contracts
            </h2>
        </div>
    </div>
    <div class="row">
        <div class="msgbox">
            <asp:Label ID="lblMessage" runat="server">
            </asp:Label>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel runat="server" ID="pnlTenant">
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Select the Institution whose data you want to view.">
                        <span class="cptn">Institution</span><span class="reqd">*</span>
                        <infs:WclComboBox ID="ddlTenantName" runat="server" AutoPostBack="true" Enabled="false"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="ddlTenantName"
                                Display="Dynamic" InitialValue="--SELECT--" CssClass="errmsg" Text="Institution is required."
                                ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Master Contract Name</span>
                        <infs:WclTextBox ID="txtAffiliationSrch" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>

                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Site Name</span>
                        <infs:WclTextBox ID="txtSite" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-12'>
                        <span class="cptn">Hierarchy Nodes</span>
                        <a href="#" id="lnkInstituteHierarchySearch" onclick="openPopUp();">Select Institution
                            Hierarchy</a>&nbsp;&nbsp
                        <asp:Label ID="lblInstituteHierarchyName" runat="server"></asp:Label>
                    </div>

                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3'>
                        <span class="cptn">Search Level</span>
                        <asp:RadioButtonList ID="rbSearchLevel" runat="server" RepeatDirection="Horizontal"
                            CssClass="form-control" AutoPostBack="false" Width="100%" Skin="Silk" AutoSkinMode="false">
                            <asp:ListItem Text="Master" Value="MSTR" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Site" Value="SITE"> </asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Contract/Site Renewal Type</span>
                        <infs:WclComboBox ID="ddlRenewalSrch" runat="server" AutoPostBack="false" Width="100%"
                            CssClass="form-control" Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Contract/Site Start Date</span>
                        <infs:WclDatePicker ID="txtSdateSrch" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnPopupOpening="SetMinDate" ClientEvents-OnDateSelected="CorrectStartToEndDate"
                            Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Contract/Site End Date</span>
                        <infs:WclDatePicker ID="txtEdateSrch" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnPopupOpening="SetMinEndDate" Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">

                    <div class='form-group col-md-3'>
                        <span class="cptn">Contract/Site Document Status</span>
                        <infs:WclComboBox ID="cmbDocumentStatus" runat="server" AutoPostBack="false" Width="100%"
                            CssClass="form-control" Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Contract/Site Contract Type</span>
                        <infs:WclComboBox ID="cmbContractType" runat="server" AutoPostBack="false" Width="100%" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            CssClass="form-control" Skin="Silk" AutoSkinMode="false" EmptyMessage="--SELECT--" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>

                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="col-md-12 text-center">
        <infsu:CommandBar ID="CmdBarSearch" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
            AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo" SubmitButtonText="Reset"
            SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
            OnSaveClick="CmdBarSearch_SaveClick" OnSubmitClick="CmdBarSearch_ResetClick"
            OnCancelClick="CmdBarSearch_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk"
            ValidationGroup="grpFormSubmit" DefaultPanelButton="Save" DefaultPanel="pnlTenant">
        </infsu:CommandBar>
    </div>
    <asp:HiddenField ID="hdnDocHtml" Value="" runat="server" />
</div>
<div class="container-fluid">
    <div class="row">
        <infs:WclGrid runat="server" ID="grdContract" AllowPaging="True" AllowCustomPaging="true"
            AutoGenerateColumns="False"
            AllowSorting="True" ShowClearFiltersButton="false" AutoSkinMode="False" CellSpacing="0"
            PageSize="2"
            EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false"
            OnNeedDataSource="grdContract_NeedDataSource"
            GridLines="both" EnableLinqExpressions="false" OnItemCommand="grdContract_ItemCommand"
            OnItemDataBound="grdContract_ItemDataBound"
            OnSortCommand="grdContract_SortCommand" OnDeleteCommand="grdContract_DeleteCommand">
            <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ContractId" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Contract" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="AffiliationName" FilterControlAltText="Filter AffiliationName column" HeaderStyle-Width="15%"
                        HeaderText="Master Contract Name" SortExpression="AffiliationName" UniqueName="AffiliationName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="StartDate" ItemStyle-CssClass="breakword" FilterControlAltText="Filter StartDate column" HeaderStyle-Width="8%"
                        HeaderText="Contract's Start Date" SortExpression="StartDate" UniqueName="StartDate" DataFormatString="{0:MM/dd/yyyy}">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EndDate" ItemStyle-CssClass="breakword" FilterControlAltText="Filter EndDate column" HeaderStyle-Width="8%"
                        HeaderText="Contract's End Date" SortExpression="EndDate" UniqueName="EndDate" DataFormatString="{0:MM/dd/yyyy}">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="HierarchyNodes" ItemStyle-CssClass="breakword" HeaderStyle-Width="15%"
                        FilterControlAltText="Filter HierarchyNodes column"
                        HeaderText="Hierarchy Nodes" SortExpression="HierarchyNodes" UniqueName="HierarchyNodes">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Contacts" FilterControlAltText="Filter Contacts column"
                        HeaderStyle-Width="15%"
                        HeaderText="Contract's Contact(s)" SortExpression="Contacts" UniqueName="Contacts">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Sites" FilterControlAltText="Filter Sites column"
                        HeaderStyle-Width="15%"
                        HeaderText="Sites" SortExpression="Sites" UniqueName="Sites">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ContractTypeNames" FilterControlAltText="Filter ContractType(s) column"
                        HeaderStyle-Width="15%"
                        HeaderText="Contract’s Contract Type(s)" SortExpression="ContractTypeNames" AllowSorting="false" UniqueName="ContractTypeNames">
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Record?"
                        Text="Delete" UniqueName="DeleteColumn" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                        EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif">
                    </telerik:GridEditCommandColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDocument" Visible="true"
                        ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnViewDocument" runat="server" Visible="true" Text="View Document"
                                CommandName="ViewDocument" CommandArgument='<%# Eval("AffiliationName") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div id="divAddForm" runat="server" visible="true">
                            <div class="col-md-12">
                                <div class="row bgLightGreen">
                                    <div class="col-md-12">
                                        <h2 class="header-color">
                                            <asp:Label ID="lblTitleContract" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Contract" : "Update Contract" %>'
                                                runat="server" />
                                        </h2>
                                    </div>
                                </div>
                            </div>
                            <asp:Panel runat="server" ID="pnlItem">
                                <div class="col-md-12">
                                    <div class="msgbox">
                                        <asp:Label ID="lblGridMessage" runat="server">
                                        </asp:Label>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="row bgLightGreen">
                                        <div class='form-group col-md-3'>
                                            <span class="cptn">Institution</span><span class="reqd">*</span>
                                            <infs:WclComboBox ID="cmbTenantGridMode" runat="server" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                                DataTextField="TenantName" AutoPostBack="true" DataValueField="TenantID"
                                                OnSelectedIndexChanged="ddlTenantSaveMode_SelectedIndexChanged" Width="100%"
                                                CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                            </infs:WclComboBox>
                                            <div class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="cmbTenantGridMode"
                                                    Display="Dynamic" InitialValue="--SELECT--" CssClass="errmsg" Text="Institution is required."
                                                    ValidationGroup="grpContract" />
                                            </div>
                                        </div>
                                        <div class='form-group col-md-3'>
                                            <span class="cptn">Master Contract Name</span><span class="reqd">*</span>
                                            <infs:WclTextBox ID="txtAffiliation" Skin="Silk" AutoSkinMode="false" runat="server"
                                                ValidationGroup="grpContract" Width="100%" CssClass="form-control">
                                            </infs:WclTextBox>
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator ID="rfvAffiliationName" runat="server" ValidationGroup="grpContract"
                                                    ControlToValidate="txtAffiliation"
                                                    Display="Dynamic" CssClass="errmsg" Text="Master Contract Name is required."></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class='form-group col-md-3'>
                                            <span class="cptn">Start Date</span>
                                            <infs:WclDatePicker ID="dpStartDate" runat="server" ClientEvents-OnPopupOpening="SetMinDate"
                                                DateInput-EmptyMessage="Select a date" ClientEvents-OnDateSelected="CorrectStartToEndDateOnAdd"
                                                Width="100%" CssClass="form-control">
                                            </infs:WclDatePicker>
                                            <div class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvStartDate" ControlToValidate="dpStartDate"
                                                    Enabled="false"
                                                    Display="Dynamic" CssClass="errmsg" Text="Start Date is required."
                                                    ValidationGroup="grpContract" />
                                            </div>
                                        </div>
                                        <div class='form-group col-md-3'>
                                            <span class="cptn">End Date</span>
                                            <infs:WclDatePicker ID="dpEdate" runat="server" DateInput-EmptyMessage="Select a date"
                                                ClientEvents-OnPopupOpening="SetMinEndDateOnAdd" Width="100%" CssClass="form-control">
                                            </infs:WclDatePicker>
                                            <div class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvEndDate" ControlToValidate="dpEdate"
                                                    Enabled="false"
                                                    Display="Dynamic" CssClass="errmsg" Text="End Date is required."
                                                    ValidationGroup="grpContract" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class='col-md-12'>
                                    <div class="row">
                                        <div class='form-group col-md-12'>
                                            <span class="cptn">Hierarchy Nodes</span>
                                            <a href="#" id="lnkInstitutionHierarchyGridMode" runat="server" class="blue" onclick="OpenInstitutionHierarchyGridMode();">Select Institution
                            Hierarchy</a>&nbsp;&nbsp
                                        <asp:Label ID="lblInstitutionHierarchyGridMode" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div id="trailingText" class="row bgLightGreen">
                                        <div class='form-group col-md-3'>
                                            <span class="cptn lineHeight18">Days(before)</span>
                                            <infs:WclTextBox ID="txtDaysBefore" runat="server" MaxLength="150" Width="100%" CssClass="form-control">
                                            </infs:WclTextBox>
                                            <div class="vldx">
                                                <asp:RegularExpressionValidator Display="Dynamic" ID="revBeforeExpiry" runat="server"
                                                    ValidationExpression="^[0-9,]*$" class="errmsg" ControlToValidate="txtDaysBefore"
                                                    ErrorMessage="Only numeric value is allowed.">
                                                </asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class='form-group col-md-3'>
                                            <span class="cptn lineHeight18">Frequency (after)</span>
                                            <infs:WclNumericTextBox ID="txtFrequencyAfter" runat="server" MaxLength="3"
                                                MinValue="0" NumberFormat-DecimalDigits="0" Width="100%" CssClass="form-control">
                                            </infs:WclNumericTextBox>
                                        </div>
                                        <div class='form-group col-md-3'>
                                            <span class="cptn lineHeight18">Renewal Type</span>
                                            <infs:WclComboBox ID="cmbRenewalType" runat="server" AutoSkinMode="false" Skin="Silk" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                                AutoPostBack="true" OnSelectedIndexChanged="cmbRenewalType_SelectedIndexChanged"
                                                Width="100%" CssClass="form-control">
                                            </infs:WclComboBox>
                                        </div>
                                        <asp:Panel ID="pnlCriteria" runat="server" Visible="false">
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">
                                                    <asp:Literal ID="litLabel" runat="server"></asp:Literal></span>
                                                <span id="spnRequired" runat="server" class="reqd">*</span>
                                                <infs:WclDatePicker ID="dpExpirationDate" runat="server" Visible="false" Width="100%"
                                                    CssClass="form-control">
                                                </infs:WclDatePicker>
                                                <infs:WclNumericTextBox ID="txtTerms" runat="server" Visible="false" NumberFormat-DecimalDigits="0"
                                                    Width="100%" CssClass="form-control">
                                                </infs:WclNumericTextBox>
                                                <div class='vldx'>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvExpirationDate" ControlToValidate="dpExpirationDate"
                                                        Display="Dynamic" CssClass="errmsg" Text="Expiration Date is required."
                                                        ValidationGroup="grpContract" />
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvTerms" ControlToValidate="txtTerms"
                                                        Display="Dynamic" CssClass="errmsg" Text="No. of Months is required."
                                                        ValidationGroup="grpContract" />
                                                    <asp:RangeValidator ID="rngvTerms" runat="server" ControlToValidate="txtTerms" MinimumValue="1"
                                                        Type="Integer" MaximumValue="999"
                                                        ValidationGroup="grpContract" Display="Dynamic" CssClass="errmsg" Text="No. of Months should be greater than 0."></asp:RangeValidator>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class='col-md-12'>
                                    <div class="row bgLightGreen">
                                        <div class='form-group col-md-6'>
                                            <span class="cptn">Notes</span>
                                            <infs:WclTextBox runat="server" ID="txtNotes" TextMode="MultiLine" MaxLength="500"
                                                Width="100%" CssClass="borderTextArea form-control">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class='form-group col-md-3'>
                                            <span class="cptn">Contract Type</span>
                                            <infs:WclComboBox ID="cmbContractType" runat="server" AutoPostBack="false" Width="100%" CheckBoxes="true"
                                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" EnableCheckAllItemsCheckBox="true"
                                                CssClass="form-control" Skin="Silk" AutoSkinMode="false" EmptyMessage="--SELECT--">
                                            </infs:WclComboBox>
                                        </div>
                                    </div>
                                </div>
                                <div class='col-md-12'>&nbsp;</div>
                            </asp:Panel>
                            <div class='col-md-12'>
                                <div class="row">
                                    <infs:WclGrid runat="server" ID="grdContractDocuments" CssClass="gridStyleChange"
                                        AllowPaging="True" AutoGenerateColumns="False"
                                        AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="true" CellSpacing="0"
                                        ShowClearFiltersButton="false"
                                        GridLines="Both" EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="false"
                                        PageSize="10" OnInsertCommand="grdContractDocuments_InsertCommand" OnUpdateCommand="grdContractDocuments_UpdateCommand"
                                        OnDeleteCommand="grdContractDocuments_DeleteCommand"
                                        OnNeedDataSource="grdContractDocuments_NeedDataSource" OnItemDataBound="grdContractDocuments_ItemDataBound">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="true"></Selecting>
                                        </ClientSettings>
                                        <GroupingSettings CaseSensitive="false" />
                                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="TempDocID" AllowFilteringByColumn="false">
                                            <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Document"
                                                ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                                                ShowRefreshButton="false" />
                                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                            </RowIndicatorColumn>
                                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                            </ExpandCollapseColumn>
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="DocumentName"
                                                    HeaderText="Name" SortExpression="DocumentName" UniqueName="DocumentName">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="DocumentTypeName"
                                                    HeaderText="Type" SortExpression="DocumentTypeName" UniqueName="DocumentTypeName">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="DocStatusName"
                                                    HeaderText="Agreement Status" SortExpression="DocStatusName" UniqueName="DocStatusName">
                                                </telerik:GridBoundColumn>
                                                <%-- <telerik:GridBoundColumn DataField="DocStartDate" DataFormatString="{0:MM//dd/yyyy}"
                                                        HeaderText="Start Date" SortExpression="DocStartDate" UniqueName="DocStartDate">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="DocEndDate" DataFormatString="{0:MM//dd/yyyy}"
                                                        HeaderText="End Date" SortExpression="DocEndDate" UniqueName="DocEndDate">
                                                    </telerik:GridBoundColumn>--%>
                                                <telerik:GridTemplateColumn UniqueName="ViewDocument">
                                                    <ItemTemplate>
                                                        <a id="lnkDoc" runat="server" class="linkView">View Document</a>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Document?"
                                                    Text="Delete" UniqueName="DeleteColumn" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif">
                                                    <HeaderStyle CssClass="tplcohdr" />
                                                    <ItemStyle CssClass="MyImageButton" Width="3%" HorizontalAlign="Center" />
                                                </telerik:GridButtonColumn>
                                                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                                    EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif">
                                                    <ItemStyle Width="3%" />
                                                </telerik:GridEditCommandColumn>
                                            </Columns>
                                            <EditFormSettings EditFormType="Template">
                                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                </EditColumn>
                                                <FormTemplate>
                                                    <div runat="server" id="divEditBlock" visible="true">
                                                        <div class="col-md-12">
                                                            <div class="row bgLightGreen">
                                                                <div class="col-md-12">
                                                                    <h2 class="header-color">
                                                                        <asp:Label ID="lblTitleNode" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Document" : "Update Document" %>'
                                                                            runat="server" /></h2>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div class="header-color">
                                                                        <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <asp:Panel runat="server" ID="pnlNode">
                                                            <asp:HiddenField runat="server" Value='<%# Eval("ContractDocumentMappingID") %>'
                                                                ID="hdnDocumentID"></asp:HiddenField>

                                                            <div class="col-md-12">
                                                                <div class="row bgLightGreen">
                                                                    <div class='form-group col-md-3'>
                                                                        <span class="cptn">Document Type</span><span class="reqd">*</span>
                                                                        <infs:WclComboBox ID="cmbDocType" runat="server" AutoPostBack="false" Width="100%"
                                                                            CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                                        </infs:WclComboBox>
                                                                        <div class="vldx">
                                                                            <asp:RequiredFieldValidator runat="server" ID="rfvDocType" ControlToValidate="cmbDocType"
                                                                                InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpDocuments" CssClass="errmsg"
                                                                                Text="Document Type is required." />
                                                                        </div>
                                                                    </div>
                                                                    <div class='form-group col-md-3'>
                                                                        <span class="cptn">Document Name</span>
                                                                        <infs:WclTextBox ID="txtDocumentName" Width="100%" runat="server" Text='<%# Eval("DocumentName") %>'
                                                                            MaxLength="150" ValidationGroup="grpDocuments" CssClass="form-control">
                                                                        </infs:WclTextBox>
                                                                    </div>
                                                                    <div class='form-group col-md-3'>
                                                                        <span class="cptn">Agreement Status</span>
                                                                        <infs:WclComboBox ID="cmbDocStatus" runat="server" AutoPostBack="false" Width="100%"
                                                                            CssClass="form-control" Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                                                                        </infs:WclComboBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="row bgLightGreen">
                                                                    <div class='form-group col-md-3'>
                                                                        <span class="cptn">Upload Document</span><span class="reqd">*</span>
                                                                        <div id="divUploadDoc" runat="server">
                                                                            <%--title="Click this button to upload document"--%>
                                                                            <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true"
                                                                                MultipleFileSelection="Disabled" MaxFileInputsCount="1" OnClientFileSelected="onClientFileSelected"
                                                                                OnClientFileUploaded="onFileUploaded" OnClientValidationFailed="upl_OnClientValidationFailed"
                                                                                Localization-Select="Browse"
                                                                                AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,JPG,gif,tif,tiff,docx,doc,rtf,pdf,odt,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,JPG,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT"
                                                                                CssClass="form-control" AutoSkinMode="false" />
                                                                        </div>
                                                                        <div>
                                                                            <asp:Label ID="lblUploadFormName" runat="server" Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblUploadFormPath" runat="server" Visible="false"></asp:Label>
                                                                            <%--<asp:LinkButton ID="lnkRemove" runat="server" Text="Remove" Visible="false" OnClick="lnkRemove_Click" ToolTip="Click this button to remove document"></asp:LinkButton>--%>
                                                                        </div>
                                                                        <div class='vldx'>
                                                                            <asp:Label ID="lblUploadFormMsg" class="errmsg" runat="server" Visible="false">Upload Document is required.</asp:Label>
                                                                        </div>
                                                                    </div>
                                                                    <div runat="server" id="divDocVersion" style="display: none">
                                                                        <div class="form-group col-md-3">
                                                                            <span class="cptn">Create Version</span>
                                                                            <infs:WclButton runat="server" ID="chkCreateVersion" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                                                AutoPostBack="false"
                                                                                OnClientCheckedChanged="CreatePackageVersionClicked" CssClass="form-control">
                                                                                <ToggleStates>
                                                                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                                                                </ToggleStates>
                                                                            </infs:WclButton>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </asp:Panel>
                                                        <div class="col-md-12">
                                                            <div class="row">&nbsp;</div>
                                                            <div class="row text-center">
                                                                <infsu:CommandBar ID="fsucCmdBarSite" runat="server" ValidationGroup="grpDocuments"
                                                                    GridMode="true" DefaultPanel="pnlNode"
                                                                    GridInsertText="Save" GridUpdateText="Save" UseAutoSkinMode="false" ButtonSkin="Silk" />
                                                            </div>
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
                            </div>
                            <div class='col-md-12'>
                                <div class="row">
                                    <infs:WclGrid runat="server" ID="grdSites" AllowPaging="True"
                                        AutoGenerateColumns="False"
                                        AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                                        ShowClearFiltersButton="false"
                                        GridLines="Both" EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="false"
                                        PageSize="10" OnInsertCommand="grdSites_InsertCommand" OnUpdateCommand="grdSites_UpdateCommand"
                                        OnDeleteCommand="grdSites_DeleteCommand" OnItemCommand="grdSites_ItemCommand"
                                        OnItemDataBound="grdSites_DataBound"
                                        OnNeedDataSource="grdSites_NeedDataSource">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="true"></Selecting>
                                        </ClientSettings>
                                        <GroupingSettings CaseSensitive="false" />
                                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="TempSiteId,ContractSiteMappingId,SiteId"
                                            AllowFilteringByColumn="false">
                                            <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Site"
                                                ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                                                ShowRefreshButton="false" />
                                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                            </RowIndicatorColumn>
                                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                            </ExpandCollapseColumn>
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="SiteName"
                                                    HeaderText="Name" SortExpression="SiteName" UniqueName="SiteName">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="SiteAddress" FilterControlAltText="Filter Site Address column"
                                                    HeaderText="Address" SortExpression="SiteAddress" UniqueName="SiteAddress">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="StartDate" ItemStyle-CssClass="breakword" FilterControlAltText="Filter StartDate column"
                                                    HeaderText="Start Date" SortExpression="StartDate" UniqueName="StartDate" DataFormatString="{0:MM/dd/yyyy}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="EndDate" ItemStyle-CssClass="breakword" FilterControlAltText="Filter EndDate column"
                                                    HeaderText="End Date" SortExpression="EndDate" UniqueName="EndDate" DataFormatString="{0:MM/dd/yyyy}">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Site?"
                                                    Text="Delete" UniqueName="DeleteColumn" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif">
                                                    <HeaderStyle CssClass="tplcohdr" />
                                                    <ItemStyle CssClass="MyImageButton" Width="3%" HorizontalAlign="Center" />
                                                </telerik:GridButtonColumn>
                                                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                                    EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif">
                                                    <ItemStyle Width="3%" />
                                                </telerik:GridEditCommandColumn>
                                                <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDocument" Visible="true"
                                                    ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnViewDocument" runat="server" Visible="true" Text="View Document"
                                                            CommandName="ViewDocument" CommandArgument='<%# Eval("SiteName") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                            <EditFormSettings EditFormType="Template">
                                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                </EditColumn>
                                                <FormTemplate>

                                                    <div runat="server" id="divEditBlock" visible="true">
                                                        <div class="col-md-12">
                                                            <div class="row bgLightGreen">
                                                                <div class="col-md-12">
                                                                    <h2 class="header-color">
                                                                        <asp:Label ID="lblTitleNode" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Site" : "Update Site" %>'
                                                                            runat="server" />
                                                                    </h2>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="header-color">
                                                            <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <asp:Panel runat="server" ID="pnlNode">
                                                        <asp:HiddenField runat="server" Value='<%# Eval("SiteID") %>' ID="hdnSiteId"></asp:HiddenField>
                                                        <div class='col-md-12'>
                                                            <div class="row bgLightGreen">
                                                                <div class='form-group col-md-3'>
                                                                    <span class="cptn">Site Name</span><span class="reqd">*</span>
                                                                    <infs:WclTextBox ID="txtSiteName" Width="100%" runat="server" Text='<%# Eval("SiteName") %>'
                                                                        MaxLength="250" ValidationGroup="grpSites" CssClass="form-control">
                                                                    </infs:WclTextBox>
                                                                    <div id="Div1" class='vldx'>
                                                                        <asp:RequiredFieldValidator runat="server" ID="rfvSiteName" ControlToValidate="txtSiteName"
                                                                            Display="Dynamic" class="errmsg" ErrorMessage="Site Name is required."
                                                                            Enabled="true" ValidationGroup="grpSites" />
                                                                    </div>
                                                                </div>
                                                                <div class='form-group col-md-3'>
                                                                    <span class="cptn">Site Address</span>
                                                                    <infs:WclTextBox ID="txtSiteAddress" runat="server" Text='<%# Eval("SiteAddress") %>'
                                                                        MaxLength="500" Width="100%" CssClass="form-control">
                                                                    </infs:WclTextBox>
                                                                </div>
                                                                <div class='form-group col-md-3'>
                                                                    <span class="cptn">Start Date</span>
                                                                    <infs:WclDatePicker ID="dpSiteStartDate" runat="server" ClientEvents-OnPopupOpening="SetMinDate"
                                                                        DateInput-EmptyMessage="Select a date" ClientEvents-OnDateSelected="CorrectStartToEndDateOnAdd"
                                                                        Width="100%" CssClass="form-control">
                                                                    </infs:WclDatePicker>
                                                                    <div class='vldx'>
                                                                        <asp:RequiredFieldValidator runat="server" ID="rfvStartDate" ControlToValidate="dpSiteStartDate"
                                                                            Enabled="false"
                                                                            Display="Dynamic" CssClass="errmsg" Text="Start Date is required."
                                                                            ValidationGroup="grpSites" />
                                                                    </div>
                                                                </div>
                                                                <div class='form-group col-md-3'>
                                                                    <span class="cptn">End Date</span>
                                                                    <infs:WclDatePicker ID="dpSiteEdate" runat="server" DateInput-EmptyMessage="Select a date"
                                                                        ClientEvents-OnPopupOpening="SetMinEndDateOnAdd" Width="100%" CssClass="form-control">
                                                                    </infs:WclDatePicker>
                                                                    <div class='vldx'>
                                                                        <asp:RequiredFieldValidator runat="server" ID="rfvEndDate" ControlToValidate="dpSiteEdate"
                                                                            Enabled="false"
                                                                            Display="Dynamic" CssClass="errmsg" Text="End Date is required."
                                                                            ValidationGroup="grpSites" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div id="trailingText" class="row bgLightGreen">
                                                                <div class='form-group col-md-3'>
                                                                    <span class="cptn lineHeight18">Days(before)</span>
                                                                    <infs:WclTextBox ID="txtSiteDaysBefore" runat="server" MaxLength="150" Width="100%" CssClass="form-control">
                                                                    </infs:WclTextBox>
                                                                    <div class="vldx">
                                                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revBeforeExpiry" runat="server"
                                                                            ValidationExpression="^[0-9,]*$" class="errmsg" ControlToValidate="txtSiteDaysBefore"
                                                                            ErrorMessage="Only numeric value is allowed.">
                                                                        </asp:RegularExpressionValidator>
                                                                    </div>
                                                                </div>
                                                                <div class='form-group col-md-3'>
                                                                    <span class="cptn lineHeight18">Frequency (after)</span>
                                                                    <infs:WclNumericTextBox ID="txtSiteFrequencyAfter" runat="server" MaxLength="3"
                                                                        MinValue="0" NumberFormat-DecimalDigits="0" Width="100%" CssClass="form-control">
                                                                    </infs:WclNumericTextBox>
                                                                </div>
                                                                <div class='form-group col-md-3'>
                                                                    <span class="cptn lineHeight18">Renewal Type</span>
                                                                    <infs:WclComboBox ID="cmbSiteRenewalType" runat="server" AutoSkinMode="false" Skin="Silk"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="cmbSiteRenewalType_SelectedIndexChanged"
                                                                        Width="100%" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" CssClass="form-control">
                                                                    </infs:WclComboBox>
                                                                </div>
                                                                <asp:Panel ID="pnlCriteria" runat="server" Visible="false">
                                                                    <div class='form-group col-md-3'>
                                                                        <span class="cptn">
                                                                            <asp:Literal ID="litLabel" runat="server"></asp:Literal></span>
                                                                        <span id="spnRequired" runat="server" class="reqd">*</span>
                                                                        <infs:WclDatePicker ID="dpSiteExpirationDate" runat="server" Visible="false" Width="100%"
                                                                            CssClass="form-control">
                                                                        </infs:WclDatePicker>
                                                                        <infs:WclNumericTextBox ID="txtSiteTerms" runat="server" Visible="false" NumberFormat-DecimalDigits="0"
                                                                            Width="100%" CssClass="form-control">
                                                                        </infs:WclNumericTextBox>
                                                                        <div class='vldx'>
                                                                            <asp:RequiredFieldValidator runat="server" ID="rfvExpirationDate" ControlToValidate="dpSiteExpirationDate"
                                                                                Display="Dynamic" CssClass="errmsg" Text="Expiration Date is required."
                                                                                ValidationGroup="grpSites" />
                                                                            <asp:RequiredFieldValidator runat="server" ID="rfvTerms" ControlToValidate="txtSiteTerms"
                                                                                Display="Dynamic" CssClass="errmsg" Text="No. of Months is required."
                                                                                ValidationGroup="grpSites" />
                                                                            <asp:RangeValidator ID="rngvTerms" runat="server" ControlToValidate="txtSiteTerms" MinimumValue="1"
                                                                                Type="Integer" MaximumValue="999"
                                                                                ValidationGroup="grpSites" Display="Dynamic" CssClass="errmsg" Text="No. of Months should be greater than 0."></asp:RangeValidator>
                                                                        </div>
                                                                    </div>
                                                                </asp:Panel>
                                                            </div>
                                                        </div>
                                                        <div class='col-md-12'>
                                                            <div class="row bgLightGreen">
                                                                <div class='form-group col-md-6'>
                                                                    <span class="cptn">Notes</span>
                                                                    <infs:WclTextBox runat="server" ID="txtSiteNotes" TextMode="MultiLine" MaxLength="500"
                                                                        Width="100%" CssClass="borderTextArea form-control">
                                                                    </infs:WclTextBox>
                                                                </div>
                                                                <div class='form-group col-md-3'>
                                                                    <span class="cptn">Contract Type</span>
                                                                    <infs:WclComboBox ID="cmbContractType" runat="server" AutoPostBack="false" Width="100%" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                                                        CssClass="form-control" Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" EmptyMessage="--SELECT--">
                                                                    </infs:WclComboBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <div class='col-md-12'>
                                                        <div class="row">
                                                            <infs:WclGrid runat="server" ID="grdSiteDocuments" CssClass="gridStyleChange"
                                                                AllowPaging="True"
                                                                AutoGenerateColumns="False" AllowSorting="True" AllowFilteringByColumn="false"
                                                                CellSpacing="0"
                                                                ShowClearFiltersButton="false" GridLines="Both" AutoSkinMode="true" EnableDefaultFeatures="True"
                                                                ShowAllExportButtons="False" ShowExtraButtons="false" PageSize="10" OnNeedDataSource="grdSiteDocuments_NeedDataSource"
                                                                OnInsertCommand="grdSiteDocuments_InsertCommand" OnUpdateCommand="grdSiteDocuments_UpdateCommand"
                                                                OnDeleteCommand="grdSiteDocuments_DeleteCommand" OnItemDataBound="grdSiteDocuments_ItemDataBound">
                                                                <ClientSettings EnableRowHoverStyle="true">
                                                                    <Selecting AllowRowSelect="true"></Selecting>
                                                                </ClientSettings>
                                                                <GroupingSettings CaseSensitive="false" />
                                                                <MasterTableView CommandItemDisplay="Top" DataKeyNames="TempDocID" AllowFilteringByColumn="false">
                                                                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Site Document"
                                                                        ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                                                                        ShowRefreshButton="false" />
                                                                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                                                    </RowIndicatorColumn>
                                                                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                                                    </ExpandCollapseColumn>
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="DocumentName"
                                                                            HeaderText="Name" SortExpression="DocumentName" UniqueName="DocumentName1">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="DocumentTypeName"
                                                                            HeaderText="Type" SortExpression="DocumentTypeName" UniqueName="DocumentTypeName1">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="DocStatusName"
                                                                            HeaderText="Agreement Status" SortExpression="DocStatusName" UniqueName="DocStatusName1">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridTemplateColumn UniqueName="ViewDocument">
                                                                            <ItemTemplate>
                                                                                <a id="lnkDoc" runat="server" class="linkView">View Document</a>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Document?"
                                                                            Text="Delete" UniqueName="DeleteColumn" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif">
                                                                            <HeaderStyle CssClass="tplcohdr" />
                                                                            <ItemStyle CssClass="MyImageButton" Width="3%" HorizontalAlign="Center" />
                                                                        </telerik:GridButtonColumn>
                                                                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                                                            EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif">
                                                                            <ItemStyle Width="3%" />
                                                                        </telerik:GridEditCommandColumn>
                                                                    </Columns>
                                                                    <EditFormSettings EditFormType="Template">
                                                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                                        </EditColumn>
                                                                        <FormTemplate>
                                                                            <div runat="server" id="divEditBlock" visible="true">
                                                                                <div class="col-md-12">
                                                                                    <div class="row bgLightGreen">
                                                                                        <div class="col-md-12">
                                                                                            <h2 class="header-color">
                                                                                                <asp:Label ID="lblTitleNode" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Site Document" : "Update Site Document" %>'
                                                                                                    runat="server" />
                                                                                            </h2>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div class="row">
                                                                                        <div class="col-md-12">
                                                                                            <div class="header-color">
                                                                                                <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <asp:Panel runat="server" ID="pnlNode">
                                                                                    <asp:HiddenField runat="server" Value='<%# Eval("SiteDocumentMappingID") %>'
                                                                                        ID="hdnSiteDocumentID"></asp:HiddenField>
                                                                                    <div class="col-md-12">
                                                                                        <div class="row bgLightGreen">
                                                                                            <div class='form-group col-md-3'>
                                                                                                <span class="cptn">Document Type</span><span class="reqd">*</span>
                                                                                                <infs:WclComboBox ID="cmbDocType" runat="server" AutoPostBack="false" Width="100%"
                                                                                                    CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                                                                </infs:WclComboBox>
                                                                                                <div class="vldx">
                                                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvDocType" ControlToValidate="cmbDocType"
                                                                                                        InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpSiteDocuments"
                                                                                                        CssClass="errmsg"
                                                                                                        Text="Document Type is required." />
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class='form-group col-md-3'>
                                                                                                <span class="cptn">Document Name</span>
                                                                                                <infs:WclTextBox ID="txtDocumentName" Width="100%" runat="server" Text='<%# Eval("DocumentName") %>'
                                                                                                    MaxLength="150" ValidationGroup="grpSiteDocuments" CssClass="form-control">
                                                                                                </infs:WclTextBox>
                                                                                            </div>
                                                                                            <div class='form-group col-md-3'>
                                                                                                <span class="cptn">Agreement Status</span>
                                                                                                <infs:WclComboBox ID="cmbDocStatus" runat="server" AutoPostBack="false" Width="100%"
                                                                                                    Filter="Contains" OnClientKeyPressin="openCmbBoxOnTab" CssClass="form-control"
                                                                                                    Skin="Silk" AutoSkinMode="false">
                                                                                                </infs:WclComboBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-12">
                                                                                        <div class="row bgLightGreen">
                                                                                            <div class='form-group col-md-3'>
                                                                                                <span class="cptn">Upload Document</span><span class="reqd">*</span>
                                                                                                <div id="divUploadDoc" runat="server">
                                                                                                    <%--title="Click this button to upload document"--%>
                                                                                                    <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true"
                                                                                                        MultipleFileSelection="Disabled" MaxFileInputsCount="1" OnClientFileSelected="onClientFileSelected"
                                                                                                        OnClientFileUploaded="onFileUploaded" OnClientValidationFailed="upl_OnClientValidationFailed"
                                                                                                        Localization-Select="Browse"
                                                                                                        AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,JPG,gif,tif,tiff,docx,doc,rtf,pdf,odt,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,JPG,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT"
                                                                                                        CssClass="form-control" AutoSkinMode="false" />
                                                                                                </div>
                                                                                                <div>
                                                                                                    <asp:Label ID="lblUploadFormName" runat="server" Visible="false"></asp:Label>
                                                                                                    <asp:Label ID="lblUploadFormPath" runat="server" Visible="false"></asp:Label>
                                                                                                </div>
                                                                                                <div class='vldx'>
                                                                                                    <asp:Label ID="lblUploadFormMsg" class="errmsg" runat="server" Visible="false">Upload Document is required.</asp:Label>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div runat="server" id="divDocVersion" style="display: none">
                                                                                                <div class="form-group col-md-3">
                                                                                                    <span class="cptn">Create Version</span>
                                                                                                    <infs:WclButton runat="server" ID="chkCreateVersion" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                                                                        AutoPostBack="false"
                                                                                                        OnClientCheckedChanged="CreatePackageVersionClicked" CssClass="form-control">
                                                                                                        <ToggleStates>
                                                                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                                                                        </ToggleStates>
                                                                                                    </infs:WclButton>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </asp:Panel>
                                                                                <div class="col-md-12">
                                                                                    <div class="row">&nbsp;</div>
                                                                                    <div class="row text-center">
                                                                                        <infsu:CommandBar ID="fsucCmdBarSite" runat="server" ValidationGroup="grpSiteDocuments"
                                                                                            GridMode="true" DefaultPanel="pnlNode"
                                                                                            GridInsertText="Save" GridUpdateText="Save" UseAutoSkinMode="false" ButtonSkin="Silk" />
                                                                                    </div>
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
                                                    </div>
                                                    <div class='col-md-12'>
                                                        <div class="row">
                                                            <infs:WclGrid runat="server" ID="grdSiteContacts" AllowPaging="True" AutoGenerateColumns="False"
                                                                AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="false" CellSpacing="0"
                                                                ShowClearFiltersButton="false" GridLines="Both" EnableDefaultFeatures="True" ShowAllExportButtons="False"
                                                                ShowExtraButtons="false" PageSize="10" OnInsertCommand="grdSiteContacts_InsertCommand"
                                                                OnUpdateCommand="grdSiteContacts_UpdateCommand" OnItemDataBound="grdSiteContacts_ItemDataBound"
                                                                OnDeleteCommand="grdSiteContacts_DeleteCommand" OnNeedDataSource="grdSiteContacts_NeedDataSource">
                                                                <ClientSettings EnableRowHoverStyle="true">
                                                                    <Selecting AllowRowSelect="true"></Selecting>
                                                                </ClientSettings>
                                                                <GroupingSettings CaseSensitive="false" />
                                                                <MasterTableView CommandItemDisplay="Top" DataKeyNames="TempContactId,ContractContactMappingId"
                                                                    AllowFilteringByColumn="false">
                                                                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Site Contact"
                                                                        ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                                                                        ShowRefreshButton="false" />
                                                                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                                                    </RowIndicatorColumn>
                                                                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                                                    </ExpandCollapseColumn>
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="FirstName"
                                                                            HeaderText="First Name" SortExpression="FirstName" UniqueName="FirstName">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="LastName"
                                                                            HeaderText="Last Name" SortExpression="LastName" UniqueName="LastName">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="Title"
                                                                            HeaderText="Title" SortExpression="Title" UniqueName="Title">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="Phone"
                                                                            HeaderText="Phone" SortExpression="Phone" UniqueName="Phone">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="Email"
                                                                            HeaderText="Email" SortExpression="Email" UniqueName="Email">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Contact?"
                                                                            Text="Delete" UniqueName="DeleteColumn" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif">
                                                                            <HeaderStyle CssClass="tplcohdr" />
                                                                            <ItemStyle CssClass="MyImageButton" Width="3%" HorizontalAlign="Center" />
                                                                        </telerik:GridButtonColumn>
                                                                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                                                            EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif">
                                                                            <ItemStyle Width="3%" />
                                                                        </telerik:GridEditCommandColumn>
                                                                    </Columns>
                                                                    <EditFormSettings EditFormType="Template">
                                                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                                        </EditColumn>
                                                                        <FormTemplate>
                                                                            <div runat="server" id="divEditBlock" visible="true">
                                                                                <div class="col-md-12">
                                                                                    <div class="row bgLightGreen">
                                                                                        <div class="col-md-12">
                                                                                            <h2 class="header-color">
                                                                                                <asp:Label ID="lblTitleNode" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Site Contact" : "Update Site Contact" %>'
                                                                                                    runat="server" />
                                                                                            </h2>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div class="header-color">
                                                                                        <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                                <asp:Panel runat="server" ID="pnlNode">
                                                                                    <asp:HiddenField runat="server" Value='<%# Eval("ContactId") %>' ID="hdnSiteId"></asp:HiddenField>
                                                                                    <div class='col-md-12'>
                                                                                        <div class="row bgLightGreen">
                                                                                            <div class='form-group col-md-3'>
                                                                                                <span class="cptn">First Name</span><span class="reqd">*</span>
                                                                                                <infs:WclTextBox ID="txtFirstName" Width="100%" runat="server" Text='<%# Eval("FirstName") %>'
                                                                                                    MaxLength="250" ValidationGroup="grpSiteContacts" CssClass="form-control">
                                                                                                </infs:WclTextBox>
                                                                                                <div id="Div1" class='vldx'>
                                                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName"
                                                                                                        Display="Dynamic" class="errmsg" ErrorMessage="First Name is required."
                                                                                                        Enabled="true" ValidationGroup="grpSiteContacts" />
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class='form-group col-md-3'>
                                                                                                <span class="cptn">Last Name</span>
                                                                                                <infs:WclTextBox ID="txtLastName" Width="100%" runat="server" Text='<%# Eval("LastName") %>'
                                                                                                    MaxLength="250" ValidationGroup="grpSiteContacts" CssClass="form-control">
                                                                                                </infs:WclTextBox>
                                                                                                <%-- <div class='vldx'>
                                                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                                                                                                        Display="Dynamic" class="errmsg" ErrorMessage="Last Name is required."
                                                                                                        Enabled="true" ValidationGroup="grpSiteContacts" />
                                                                                                </div>--%>
                                                                                            </div>
                                                                                            <div class='form-group col-md-3'>
                                                                                                <span class="cptn">Title</span>
                                                                                                <infs:WclTextBox ID="txtTitle" Width="100%" runat="server" Text='<%# Eval("Title") %>'
                                                                                                    MaxLength="250" ValidationGroup="grpContacts" CssClass="form-control">
                                                                                                </infs:WclTextBox>
                                                                                                <%-- <div class='vldx'>
                                                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvTitle" ControlToValidate="txtTitle"
                                                                                                        Display="Dynamic" class="errmsg" ErrorMessage="Title is required."
                                                                                                        Enabled="true" ValidationGroup="grpSiteContacts" />
                                                                                                </div>--%>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class='col-md-12'>
                                                                                        <div class="row bgLightGreen">
                                                                                            <div class='form-group col-md-3'>
                                                                                                <span class="cptn">Email</span>
                                                                                                <infs:WclTextBox ID="txtEmail" Width="100%" runat="server" Text='<%# Eval("Email") %>'
                                                                                                    MaxLength="250" ValidationGroup="grpContacts" CssClass="form-control">
                                                                                                </infs:WclTextBox>
                                                                                                <div class='vldx'>
                                                                                                    <asp:RegularExpressionValidator runat="server" ID="revEmail" ControlToValidate="txtEmail"
                                                                                                        Display="Dynamic" class="errmsg" ErrorMessage="Email is not valid."
                                                                                                        Enabled="true" ValidationGroup="grpSiteContacts" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class='form-group col-md-3'>
                                                                                                <span class="cptn">Phone</span>
                                                                                                <div class="row">
                                                                                                    <div class="form-group col-md-11" style="padding-right: 0px;">
                                                                                                        <infs:WclMaskedTextBox ID="txtPhone" Width="100%" Mask="(###)-###-####" runat="server"
                                                                                                            Text='<%# Eval("Phone") %>'
                                                                                                            MaxLength="250" ValidationGroup="grpSiteContacts" CssClass="form-control">
                                                                                                        </infs:WclMaskedTextBox>
                                                                                                        <infs:WclTextBox ID="txtInternationalPhone" Width="100%" runat="server" MaxLength="15" CssClass="form-control" Text='<%# Eval("Phone") %>' />
                                                                                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobilePrmyNonMasking" runat="server"
                                                                                                            ValidationGroup="grpSites" CssClass="errmsg" ErrorMessage="Invalid phone number. This field only contains +, - and numbers." ControlToValidate="txtInternationalPhone"
                                                                                                            ValidationExpression="(\d?)+(([-+]+?\d+)?)+([-+]?)+" />
                                                                                                    </div>
                                                                                                    <div class="form-group col-md-1">
                                                                                                        <infs:WclCheckBox runat="server" AutoPostBack="true" ID="chkInternationalPhone" ToolTip="Check this box if you do not have an US Number." OnCheckedChanged="chkInternationalPhone_CheckedChanged" CssClass="PhoneCheck" Checked='<%# (Container is GridEditFormInsertItem) ? false: Convert.ToBoolean(Eval("IsInternationalPhone")) %>'></infs:WclCheckBox>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                </asp:Panel>
                                                                                <div class="col-md-12">
                                                                                    <div class="row">
                                                                                        <infsu:CommandBar ID="fsucCmdBarContact" runat="server" ValidationGroup="grpSiteContacts"
                                                                                            GridMode="true" DefaultPanel="pnlNode" GridInsertText="Save" GridUpdateText="Save"
                                                                                            UseAutoSkinMode="false" ButtonSkin="Silk" />
                                                                                    </div>
                                                                                </div>
                                                                                <div style="clear: both;"></div>
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
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="row">&nbsp;</div>
                                                        <div class="row text-center">
                                                            <infsu:CommandBar ID="fsucCmdBarSite" runat="server" ValidationGroup="grpSites" GridMode="true"
                                                                DefaultPanel="pnlNode" GridInsertText="Save" GridUpdateText="Save" UseAutoSkinMode="false"
                                                                ButtonSkin="Silk" />
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
                            </div>
                            <div class='col-md-12'>
                                <div class="row">
                                    <infs:WclGrid runat="server" ID="grdContacts" AllowPaging="True"
                                        AutoGenerateColumns="False"
                                        AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="false" CellSpacing="0"
                                        ShowClearFiltersButton="false"
                                        GridLines="Both" EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="false"
                                        PageSize="10" OnInsertCommand="grdContacts_InsertCommand" OnUpdateCommand="grdContacts_UpdateCommand"
                                        OnItemDataBound="grdContacts_ItemDataBound"
                                        OnDeleteCommand="grdContacts_DeleteCommand" OnNeedDataSource="grdContacts_NeedDataSource">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="true"></Selecting>
                                        </ClientSettings>
                                        <GroupingSettings CaseSensitive="false" />
                                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="TempContactId,ContractContactMappingId"
                                            AllowFilteringByColumn="false">
                                            <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Contact"
                                                ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                                                ShowRefreshButton="false" />
                                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                            </RowIndicatorColumn>
                                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                            </ExpandCollapseColumn>
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="FirstName"
                                                    HeaderText="First Name" SortExpression="FirstName" UniqueName="FirstName">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="LastName"
                                                    HeaderText="Last Name" SortExpression="LastName" UniqueName="LastName">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Title"
                                                    HeaderText="Title" SortExpression="Title" UniqueName="Title">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Phone"
                                                    HeaderText="Phone" SortExpression="Phone" UniqueName="Phone">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Email"
                                                    HeaderText="Email" SortExpression="Email" UniqueName="Email">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Contact?"
                                                    Text="Delete" UniqueName="DeleteColumn" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif">
                                                    <HeaderStyle CssClass="tplcohdr" />
                                                    <ItemStyle CssClass="MyImageButton" Width="3%" HorizontalAlign="Center" />
                                                </telerik:GridButtonColumn>
                                                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                                    EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif">
                                                    <ItemStyle Width="3%" />
                                                </telerik:GridEditCommandColumn>
                                            </Columns>
                                            <EditFormSettings EditFormType="Template">
                                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                </EditColumn>
                                                <FormTemplate>

                                                    <div runat="server" id="divEditBlock" visible="true">
                                                        <div class="col-md-12">
                                                            <div class="row bgLightGreen">
                                                                <div class="col-md-12">
                                                                    <h2 class="header-color">
                                                                        <asp:Label ID="lblTitleNode" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Contact" : "Update Contact" %>'
                                                                            runat="server" />
                                                                    </h2>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div class="header-color">
                                                                <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <asp:Panel runat="server" ID="pnlNode">
                                                            <asp:HiddenField runat="server" Value='<%# Eval("ContactId") %>' ID="hdnSiteId"></asp:HiddenField>
                                                            <div class='col-md-12'>
                                                                <div class="row bgLightGreen">
                                                                    <div class='form-group col-md-3'>
                                                                        <span class="cptn">First Name</span><span class="reqd">*</span>
                                                                        <infs:WclTextBox ID="txtFirstName" Width="100%" runat="server" Text='<%# Eval("FirstName") %>'
                                                                            MaxLength="250" ValidationGroup="grpContacts" CssClass="form-control">
                                                                        </infs:WclTextBox>
                                                                        <div id="Div1" class='vldx'>
                                                                            <asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName"
                                                                                Display="Dynamic" class="errmsg" ErrorMessage="First Name is required."
                                                                                Enabled="true" ValidationGroup="grpSites" />
                                                                        </div>
                                                                    </div>
                                                                    <div class='form-group col-md-3'>
                                                                        <span class="cptn">Last Name</span>
                                                                        <infs:WclTextBox ID="txtLastName" Width="100%" runat="server" Text='<%# Eval("LastName") %>'
                                                                            MaxLength="250" ValidationGroup="grpContacts" CssClass="form-control">
                                                                        </infs:WclTextBox>
                                                                        <div class='vldx'>
                                                                            <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                                                                                Display="Dynamic" class="errmsg" ErrorMessage="Last Name is required."
                                                                                Enabled="true" ValidationGroup="grpContacts" />
                                                                        </div>
                                                                    </div>
                                                                    <div class='form-group col-md-3'>
                                                                        <span class="cptn">Title</span>
                                                                        <infs:WclTextBox ID="txtTitle" Width="100%" runat="server" Text='<%# Eval("Title") %>'
                                                                            MaxLength="250" ValidationGroup="grpContacts" CssClass="form-control">
                                                                        </infs:WclTextBox>
                                                                        <div class='vldx'>
                                                                            <asp:RequiredFieldValidator runat="server" ID="rfvTitle" ControlToValidate="txtTitle"
                                                                                Display="Dynamic" class="errmsg" ErrorMessage="Title is required."
                                                                                Enabled="true" ValidationGroup="grpContacts" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class='col-md-12'>
                                                                <div class="row bgLightGreen">
                                                                    <div class='form-group col-md-3'>
                                                                        <span class="cptn">Email</span>
                                                                        <infs:WclTextBox ID="txtEmail" Width="100%" runat="server" Text='<%# Eval("Email") %>'
                                                                            MaxLength="250" ValidationGroup="grpContacts" CssClass="form-control">
                                                                        </infs:WclTextBox>
                                                                        <div class='vldx'>
                                                                            <asp:RegularExpressionValidator runat="server" ID="revEmail" ControlToValidate="txtEmail"
                                                                                Display="Dynamic" class="errmsg" ErrorMessage="Email is not valid."
                                                                                Enabled="true" ValidationGroup="grpContacts" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                                        </div>
                                                                    </div>
                                                                    <div class='form-group col-md-3'>
                                                                        <span class="cptn">Phone</span>
                                                                        <div class="row">
                                                                            <div class="form-group col-md-11" style="padding-right: 0px;">
                                                                                <infs:WclMaskedTextBox ID="txtPhone1" Width="100%" Mask="(###)-###-####" runat="server"
                                                                                    Text='<%# Eval("Phone") %>'
                                                                                    MaxLength="250" ValidationGroup="grpContacts" CssClass="form-control">
                                                                                </infs:WclMaskedTextBox>
                                                                                <infs:WclTextBox ID="txtInternationalPhone1" Width="100%" runat="server" MaxLength="15" ValidationGroup="grpContacts" CssClass="form-control" Text='<%# Eval("Phone") %>' />
                                                                                <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobilePrmyNonMasking1" runat="server"
                                                                                    ValidationGroup="grpSites" CssClass="errmsg" ErrorMessage="Invalid phone number. This field only contains +, - and numbers." ControlToValidate="txtInternationalPhone1"
                                                                                    ValidationExpression="(\d?)+(([-+]+?\d+)?)+([-+]?)+" />
                                                                            </div>
                                                                            <div class="form-group col-md-1">
                                                                                <infs:WclCheckBox runat="server" AutoPostBack="true" ID="chkInternationalPhone1" ToolTip="Check this box if you do not have an US Number." OnCheckedChanged="chkInternationalPhone1_CheckedChanged" CssClass="PhoneCheck1" Checked='<%# (Container is GridEditFormInsertItem) ? false: Convert.ToBoolean(Eval("IsInternationalPhone")) %>'></infs:WclCheckBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                        <div class="col-md-12">

                                                            <div class="row">
                                                                <infsu:CommandBar ID="fsucCmdBarContact" runat="server" ValidationGroup="grpSites"
                                                                    GridMode="true" DefaultPanel="pnlNode" GridInsertText="Save" GridUpdateText="Save"
                                                                    UseAutoSkinMode="false" ButtonSkin="Silk" />
                                                            </div>
                                                        </div>
                                                        <div style="clear: both;"></div>
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
                            </div>
                        </div>
                        <div class="col-md-12">
                            <infsu:CommandBar ID="fsucCmdBarPermission" runat="server" GridMode="true" DefaultPanel="pnlItem"
                                GridInsertText="Save" GridUpdateText="Save"
                                ValidationGroup="grpContract" UseAutoSkinMode="false" ButtonSkin="Silk" />
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
        <div style="clear: both;"></div>
    </div>
</div>
<div class="container-fluid">
    <div class="col-md-12">
        <div class="row">
        </div>
    </div>
    <div class="col-md-12">
        <div class="row">
            <div class='form-group col-md-3'>
            </div>
            <div class='form-group col-md-3'>
                <infs:WclComboBox runat="server" ID="cmbExport" AutoSkinMode="false" Skin="Silk" Width="100%"
                    CssClass="form-control">
                    <Items>
                        <telerik:RadComboBoxItem Text="Affiliation Only" Value="ContractExportContractOnly" />
                        <telerik:RadComboBoxItem Text="Site Only" Value="ContractExportSiteOnly" />
                        <telerik:RadComboBoxItem Text="Both" Value="ContractExportBoth" />
                    </Items>
                </infs:WclComboBox>
            </div>
            <div class='form-group col-md-3'>
                <infsu:CommandBar ID="cbExport" runat="server" ButtonPosition="Left" DisplayButtons="Submit" OnSubmitClick="cbExport_SubmitClick"
                    AutoPostbackButtons="Submit" UseAutoSkinMode="false" ButtonSkin="Silk" SubmitButtonIconClass="resultReport"
                    SubmitButtonText="Export">
                </infsu:CommandBar>
            </div>
        </div>
    </div>
</div>
<iframe id="ifrExportDocument" runat="server" height="0" width="0"></iframe>

