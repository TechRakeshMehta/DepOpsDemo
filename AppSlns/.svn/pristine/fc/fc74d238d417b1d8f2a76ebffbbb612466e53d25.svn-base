<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequirementVerificationQueue.ascx.cs"
    Inherits="CoreWeb.ClinicalRotation.Views.RequirementVerificationQueue"  %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxVerificationQueue">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/ClinicalRotation/RotationMemberSearch.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

</infs:WclResourceManagerProxy>


<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 runat="server" id="header" class="header-color">Rotation Requirement Verification queue</h2>
        </div>
    </div>
    <div class="row bgLightGreen" id="divSearchPanel">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class='reqd'>*</span>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" AutoPostBack="true"
                                DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                OnDataBound="ddlTenantName_DataBound" Enabled="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Select a Agency whose data you want to view.">
                        <span class="cptn">Agency</span>
                        <infs:WclComboBox ID="cmbAgency" runat="server" DataTextField="AgencyName" DataValueField="AgencyID"
                            AutoPostBack="false" OnDataBound="cmbAgency_DataBound" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered first name">
                        <span class="cptn">Applicant First Name</span>
                        <infs:WclTextBox ID="txtFirstName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered last name">
                        <span class="cptn">Applicant Last Name</span>
                        <infs:WclTextBox ID="txtLastName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class="form-group col-md-3" title="Restrict search results to the entered rotation start date">
                        <span class="cptn">Rotation Start Date</span>
                        <infs:WclDatePicker ID="dpRotationStartDate" runat="server" DateInput-EmptyMessage="Select a date"
                            DateInput-DateFormat="MM/dd/yyyy" ClientEvents-OnPopupOpening="SetMinDate" ClientEvents-OnDateSelected="CorrectStartToEndDate"
                            Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class="form-group col-md-3" title="Restrict search results to the entered rotation end date">
                        <span class="cptn">Rotation End Date</span>
                        <infs:WclDatePicker ID="dpRotationEndDate" runat="server" DateInput-EmptyMessage="Select a date"
                            DateInput-DateFormat="MM/dd/yyyy" ClientEvents-OnPopupOpening="SetMinEndDate"
                            Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered submission date">
                        <span class="cptn">Submission Date</span>
                        <infs:WclDatePicker ID="dpSubmissionDate" runat="server" DateInput-EmptyMessage="Select a date"
                            DateInput-DateFormat="MM/dd/yyyy" ClientEvents-OnPopupOpening="SetMinSubmissionDate"
                            Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the selected Requirement Package Type" runat="server" id="divRequirementPackageType">
                        <span class="cptn">Requirement Package Type</span><%--<span class='reqd'>*</span>--%><infs:WclComboBox ID="cmbRequirementPackageType" runat="server" DataValueField="ID" CheckBoxes="true" EmptyMessage="--SELECT--"
                            DataTextField="Name" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" 
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" AutoPostBack="false" >
                        </infs:WclComboBox>
                        <%--<div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvRequirementPackageType" ControlToValidate="cmbRequirementPackageType"
                                InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                Text="Requirement package type is required." />
                        </div>--%>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the selected user type">
                        <span class="cptn">User Type</span>
                        <infs:WclComboBox ID="ddlUserType" runat="server" CheckBoxes="true" EmptyMessage="--SELECT--"
                            AutoPostBack="false" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false" DataTextField="Value" DataValueField="Key">
                        </infs:WclComboBox>
                    </div>

                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class="form-group col-md-3" title="search results to the Requirement Category Label">
                        <span class="cptn">Requirement Category Label</span>
                        <infs:WclComboBox ID="ddlCategoryLabel" runat="server" DataTextField="RequirementCategoryName" DataValueField="RequirementCategoryIDs"
                            OnDataBound="ddlCategoryLabel_DataBound" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientBlur="" OnClientKeyPressing="openCmbBoxOnTab" AutoPostBack="true" OnSelectedIndexChanged="ddlCategoryLabel_SelectedIndexChanged">
                        </infs:WclComboBox>
                        <%--<infs:WclTextBox ID="txtCategoryLabel" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>--%>
                    </div>
                    <div class="form-group col-md-3" title="search results to the Requirement Item Label">
                        <span class="cptn">Requirement Item Label</span>
                        <infs:WclComboBox ID="ddlItemLabel" runat="server" DataTextField="RequirementItemName" DataValueField="RequirementItemIDs"
                            AutoPostBack="false" OnDataBound="ddlItemLabel_DataBound" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                        <%--<infs:WclTextBox ID="txtItemLabel" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>--%>
                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <div class="row">

                    <div class='form-group col-md-3' title="Restrict search results to the Current Rotation records">
                        <span class="cptn">Current Rotation Only</span>
                        <infs:WclCheckBox ID="chkCurrentRotation" runat="server" />
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the selected Requirement Package" runat="server" id="divPackage">
                        <span class="cptn">Package</span>
                        <infs:WclComboBox ID="cmbPackage" runat="server" DataValueField="RequirementPackageID" EnableCheckAllItemsCheckBox="true"
                            DataTextField="RequirementPackageName" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" CheckBoxes="true" EmptyMessage="--SELECT--"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <div class="col-md-12 text-center">
            <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo"
                SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
                CancelButtonText="Cancel"
                OnSaveClick="fsucCmdBarButton_SearchClick" OnSubmitClick="fsucCmdBarButton_ResetClick"
                OnCancelClick="fsucCmdBarButton_CancelClick"
                ValidationGroup="grpFormSubmit" UseAutoSkinMode="false" ButtonSkin="Silk">
            </infsu:CommandBar>
        </div>
     &nbsp;</div>
    </div>
    <div class="row">
        <%--Note:-Submission Date in Rotation Verification Queue is wrong.So, We may have ticket in future to fix - submission date issue then save and next and save and previous navigation should be test and updated as well.--%>
        <infs:WclGrid runat="server" ID="grdRequirementVerificationQueue" AllowCustomPaging="true"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            EnableLinqExpressions="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            ShowClearFiltersButton="false"
            OnNeedDataSource="grdRequirementVerificationQueue_NeedDataSource" OnItemCommand="grdRequirementVerificationQueue_ItemCommand"
            OnSortCommand="grdRequirementVerificationQueue_SortCommand" NonExportingColumns="ViewDetail">
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grd_rwDbClick" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserID,ClinicalRotationID,RequirementPackageSubscriptionID,RequirementPackageTypeID,RequirementItemId,ApplicantRequirementItemId"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="ApplicantFirstName" HeaderText="Applicant First Name"
                        SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName" HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantLastName" HeaderText="Applicant Last Name"
                        SortExpression="ApplicantLastName" UniqueName="ApplicantLastName" HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AgencyName" HeaderText="Agency" SortExpression="AgencyName"
                        HeaderTooltip="This column displays the Agency name for each record in the grid"
                        UniqueName="AgencyName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="UserType" HeaderText="User Type" SortExpression="UserType"
                        HeaderTooltip="This column displays the user type for each record in the grid"
                        UniqueName="UserType">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RequirementPackageName" HeaderText="Package Name" SortExpression="RequirementPackageName"
                        HeaderTooltip="This column displays the Requirement Package Name for each record in the grid"
                        UniqueName="RequirementPackageName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationStartDate" HeaderText="Rotation Start Date"
                        AllowSorting="true" SortExpression="RotationStartDate" DataFormatString="{0:MM/dd/yyyy}"
                        UniqueName="RotationStartDate" HeaderTooltip="This column displays the Rotation Start Date for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationEndDate" HeaderText="Rotation End Date"
                        AllowSorting="true" SortExpression="RotationEndDate" DataFormatString="{0:MM/dd/yyyy}"
                        UniqueName="RotationEndDate" HeaderTooltip="This column displays the Rotation End Date for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="SubmissionDate" HeaderText="Submission Date"
                        SortExpression="SubmissionDate"
                        UniqueName="SubmissionDate" HeaderTooltip="This column displays the Submission Date for each record in the grid"
                        DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="ReqReviewByDesc" HeaderText="Review By" SortExpression="ReqReviewByDesc"
                        HeaderTooltip="This column displays the Requirement Package Review By for each record in the grid"
                        UniqueName="ReqReviewByDesc">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ReqCategoryLabel" HeaderText="Category Label" SortExpression="ReqCategoryLabel"
                        HeaderTooltip="This column displays the Requirement Category Label for each record in the grid" AllowSorting="true"
                        UniqueName="ReqCategoryLabel">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ReqItemLabel" HeaderText="Item Label" SortExpression="ReqItemLabel"
                        HeaderTooltip="This column displays the Requirement Item Label for each record in the grid" AllowSorting="true"
                        UniqueName="ReqItemLabel">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-ForeColor="Red" UniqueName="IsCurrentRotation">
                        <ItemTemplate>
                            <%#  Convert.ToBoolean(Eval("IsCurrentRotation")) == true ? "Current Rotation" : string.Empty %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdfReqCatId" runat="server" Value='<%# Eval("RequirementCategoryID") %>' />
                            <asp:HiddenField ID="hdfReqPackSubscriptionId" runat="server" Value='<%# Eval("RequirementPackageSubscriptionID") %>' />
                            <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="ViewDetail"
                                ToolTip="Click here to view details of verification."
                                runat="server" Text="Detail">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                    Position="TopAndBottom" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
</div>

<script type="text/javascript">

    var minDate = new Date("01/01/1900");
    function SetMinDate(picker) {
        //var date = picker.get_selectedDate();
        //if (date != null) {
        //    picker.set_minDate(date);
        //}
        //else {
        //    picker.set_minDate(minDate);
        //}
        picker.set_minDate(minDate);
    }

    function SetMinEndDate(picker) {
        //var date = $jQuery("[id$=dpRotationStartDate]")[0].control.get_selectedDate();
        picker.set_minDate(minDate);
    }

    function SetMinSubmissionDate(picker) {
        picker.set_minDate(minDate);
    }

    function CorrectStartToEndDate(picker) {
        var date1 = $jQuery("[id$=dpRotationStartDate]")[0].control.get_selectedDate();
        var date2 = $jQuery("[id$=dpRotationEndDate]")[0].control.get_selectedDate();
        if (date1 != null && date2 != null) {
            if (date1 > date2)
                $jQuery("[id$=dpRotationEndDate]")[0].control.set_selectedDate(null);
        }
    }

    function grd_rwDbClick(s, e) {
        var _id = "btnEdit";
        var b = e.get_gridDataItem().findElement(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

    function pageLoad() {

        SetDefaultButtonForSection("divSearchPanel", "fsucCmdBarButton_btnSave", true);
    }
</script>
