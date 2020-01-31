<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Reports.Views.RotationStudentDetailsReport"
    CodeBehind="RotationStudentDetails.ascx.cs" EnableTheming="true" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/ColumnsConfiguration.ascx" TagPrefix="infsu" TagName="ColumnsConfiguration" %>

<link href="../../Resources/Mod/Dashboard/Styles/bootstrap.min.css" rel="stylesheet" />
<link href='https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700' rel='stylesheet' type='text/css'>
<link href="../../Resources/Mod/Dashboard/Styles/font-awesome.min.css" rel="stylesheet" />
<link href="../../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" rel="stylesheet" />
<link href="../../App_Themes/Default/core.css" rel="stylesheet" />

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="~/Resources/Generic/ColumnsConfiguration.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style>
    .setZindex {
        z-index: auto !important;
        padding-top: 18px;
    }

    .list {
        padding-left: 15px;
    }

    .listcontent {
        list-style: disc;
    }

    html {
        overflow: auto !important;
    }
</style>
<script type="text/javascript">
    function FillAgencies(sender, eventArgs) {
        $jQuery("[id$=btnTenants]").click();
    }
</script>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Save Search Criteria
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <div class="col-md-12">
            <div class="row">
                <div class="form-group col-md-3">
                    <span class="cptn">Saved Report Searches</span>
                    <infs:WclComboBox ID="cmbSavedReportSearches" Width="100%" CssClass="form-control" runat="server" AllowCustomText="true"
                        AutoPostBack="true" DataTextField="Value" DataValueField="Key" EmptyMessage="--SELECT--"
                        Skin="Silk" AutoSkinMode="false" OnSelectedIndexChanged="cmbSavedSearches_SelectedIndexChanged"
                        Filter="Contains">
                    </infs:WclComboBox>
                </div>
                <div class="form-group col-md-3">
                    <span class="cptn">Search Name</span><span class="reqd">*</span>
                    <infs:WclTextBox ID="txtSearchName" Width="100%" CssClass="form-control" runat="server">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvSearchName" ControlToValidate="txtSearchName" ValidationGroup="btnSearchSave"
                            Display="Dynamic" CssClass="errmsg" Text="Name is required." />
                    </div>
                </div>
                <div class="form-group col-md-3">
                    <span class="cptn">Search Description</span>
                    <infs:WclTextBox ID="txtSearchDescription" Width="100%" CssClass="form-control" runat="server">
                    </infs:WclTextBox>
                </div>
                <div class="form-group col-md-3">
                    <infs:WclMenu ID="WclSearchSave" runat="server" Skin="Default" AutoSkinMode="false" CssClass="setZindex">
                        <Items>
                            <telerik:RadMenuItem Text="Searchmun">
                                <ItemTemplate>
                                    <infs:WclButton runat="server" Text="Save" ID="btnSearchSave" Icon-PrimaryIconCssClass="rbSave" CssClass="form-control" OnClick="CmdBarSearchSave_Click"
                                        ToolTip="Click to save the search criteria" ValidationGroup="btnSearchSave"
                                        Skin="Silk" AutoSkinMode="false" ButtonPosition="Left">
                                    </infs:WclButton>
                                </ItemTemplate>
                            </telerik:RadMenuItem>
                        </Items>
                    </infs:WclMenu>
                </div>
            </div>
        </div>
    </div>
    <div class="row">&nbsp;</div>
    <div class='description'>
        <asp:Label ID="lblRepDescription" runat="server" Text="Report Definition" CssClass="cptn"></asp:Label>
        This report compiles personal information for each student whose profile has been shared, along with information pertaining to the student’s rotation.  This report can be filtered by institution, by agency hierarchy, and by rotation dates. The same student may appear on the report multiple times, if placed in multiple rotations.                       
            <ul class="list">
                <li class="listcontent">Can be filtered by institution, agency hierarchy, rotation dates, and profile review status.</li>
                <li class="listcontent">Shows students, instructors, or both.</li>
            </ul>
    </div>
    <div class="row">&nbsp;</div>
    <div class="row bgLightGreen">
        <div class="col-md-12">
            <div class="row">
                <div class="form-group col-md-3">
                    <span class="cptn">Institution</span><span class="reqd">*</span>
                    <infs:WclComboBox ID="cmbInstitute" Width="100%" CssClass="form-control" runat="server" AllowCustomText="true"
                        AutoPostBack="false" DataTextField="Value" DataValueField="Key" EmptyMessage="--SELECT--"
                        Skin="Silk" AutoSkinMode="false" EnableCheckAllItemsCheckBox="true"
                        CheckBoxes="true" Filter="Contains" CausesValidation="false" OnClientKeyPressing="openCmbBoxOnTab"
                        OnClientDropDownClosed="FillAgencies">
                    </infs:WclComboBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvInstitution" ControlToValidate="cmbInstitute" ValidationGroup="btnsubmit"
                            Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                    </div>
                </div>
                <div class="form-group col-md-3">
                    <span class="cptn">Agency</span><span class="reqd">*</span>
                    <infs:WclComboBox ID="cmbAgency" Width="100%" CssClass="form-control" runat="server" AllowCustomText="true"
                        AutoPostBack="false" DataTextField="Value" DataValueField="Key" EmptyMessage="--SELECT--"
                        Skin="Silk" AutoSkinMode="false"
                        EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                    </infs:WclComboBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvAgency" ControlToValidate="cmbAgency" ValidationGroup="btnsubmit"
                            Display="Dynamic" CssClass="errmsg" Text="Agency is required." />
                    </div>
                </div>
                <div class="form-group col-md-3">
                    <span class="cptn">User Type</span><span class="reqd">*</span>
                    <infs:WclComboBox ID="cmbUserType" Width="100%" CssClass="form-control" runat="server" AllowCustomText="true"
                        AutoPostBack="false" DataTextField="Value" DataValueField="Key" EmptyMessage="--SELECT--"
                        Skin="Silk" AutoSkinMode="false"
                        EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Filter="Contains" CausesValidation="false">
                    </infs:WclComboBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvUserType" ControlToValidate="cmbUserType" ValidationGroup="btnsubmit"
                            Display="Dynamic" CssClass="errmsg" Text="User Type is required." />
                    </div>
                </div>
                <div class="form-group col-md-3">
                    <span class="cptn">Review Status</span><span class="reqd">*</span>
                    <infs:WclComboBox ID="cmbReviewStatus" Width="100%" CssClass="form-control" runat="server" AllowCustomText="true"
                        AutoPostBack="false" DataTextField="Value" DataValueField="Key" EmptyMessage="--SELECT--"
                        Skin="Silk" AutoSkinMode="false"
                        EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Filter="Contains" CausesValidation="false">
                    </infs:WclComboBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvReviewStatus" ControlToValidate="cmbReviewStatus" ValidationGroup="btnsubmit"
                            Display="Dynamic" CssClass="errmsg" Text="Review Status is required." />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="form-group col-md-3">
                    <span class="cptn">From Date</span>
                    <infs:WclDatePicker ID="dpSubmissionStartDate" Width="100%" CssClass="form-control" runat="server">
                    </infs:WclDatePicker>
                </div>
                <div class="form-group col-md-3">
                    <span class="cptn">To Date</span>
                    <infs:WclDatePicker ID="dpSubmissionEndDate" Width="100%" CssClass="form-control" runat="server">
                    </infs:WclDatePicker>
                </div>
                <div class="form-group col-md-3">
                    <span class="cptn">Include Undefined Date Shares</span>
                    <asp:RadioButtonList runat="server" ID="rblIncludeUndefined" RepeatDirection="Horizontal" CssClass="form-control">
                        <asp:ListItem Text="True" Value="True" Selected="False">
                        </asp:ListItem>
                        <asp:ListItem Text="False" Value="False" Selected="True">
                        </asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnTenants" runat="server" OnClick="btnTenants_Click" />
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group col-md-4">
                <div class="row">
                    &nbsp;
                </div>
            </div>
            <div class="form-group col-md-8">
                <div class="row" id="trailingText">
                    <div id="archiveMenuDiv" runat="server">
                        <infs:WclMenu ID="MainCommandBar" runat="server" Skin="Default" AutoSkinMode="false" CssClass="setZindex">
                            <Items>
                                <telerik:RadMenuItem Text="Searchmun">
                                    <ItemTemplate>
                                        <infs:WclButton runat="server" Text="Search" ID="btnSearch" Icon-PrimaryIconCssClass="rbSearch" OnClick="CmdBarSearch_Click"
                                            ToolTip="Click to search as per the criteria entered above" ValidationGroup="btnsubmit"
                                            Skin="Silk" AutoSkinMode="false" ButtonPosition="Center">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem Text="Resetmun">
                                    <ItemTemplate>
                                        <infs:WclButton runat="server" Text="Reset" ID="btnReset" Icon-PrimaryIconCssClass="rbUndo"
                                            OnClick="CmdBarReset_Click" ToolTip="Click to remove all values entered in the search criteria above" Skin="Silk"
                                            AutoSkinMode="false" ButtonPosition="Center" CausesValidation="false" CssClass="btn">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                </telerik:RadMenuItem>
                            </Items>
                        </infs:WclMenu>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="row">
                <infs:WclGrid runat="server" ID="grdReportData" AutoGenerateColumns="False"
                    AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                    ShowAllExportButtons="False" ShowExtraButtons="False" ShowClearFiltersButton="false"
                    GridLines="Both" PageSize="50" OnNeedDataSource="grdReportData_NeedDataSource" ExportSettings-FileName="Report">
                    <ExportSettings ExportOnlyData="False" FileName="Report" UseItemStyles="true" IgnorePaging="True" OpenInNewWindow="True"
                        Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true">
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="ID" AllowFilteringByColumn="false" HeaderStyle-CssClass="left">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                            ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="TenantName"
                                HeaderText="Institution" SortExpression="TenantName"
                                ItemStyle-HorizontalAlign="Left" UniqueName="TenantName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StudentFirstName"
                                HeaderText="First Name" SortExpression="StudentFirstName"
                                ItemStyle-HorizontalAlign="Left" UniqueName="StudentFirstName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StudentLastName"
                                HeaderText="Last Name" SortExpression="StudentLastName"
                                ItemStyle-HorizontalAlign="Left" UniqueName="StudentLastName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StudentEmailAddress"
                                HeaderText="Email Address" SortExpression="StudentEmailAddress"
                                ItemStyle-HorizontalAlign="Left" UniqueName="StudentEmailAddress">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StudentPhoneNumber"
                                HeaderText="Phone Number" SortExpression="StudentPhoneNumber"
                                ItemStyle-HorizontalAlign="Left" UniqueName="StudentPhoneNumber">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StudentDOB"
                                HeaderText="Date of Birth" SortExpression="StudentDOB"
                                ItemStyle-HorizontalAlign="Left" UniqueName="StudentDOB">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Address"
                                HeaderText="Address" SortExpression="Address"
                                ItemStyle-HorizontalAlign="Left" UniqueName="Address">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AgencyName"
                                HeaderText="Agency" SortExpression="AgencyName"
                                ItemStyle-HorizontalAlign="Left" UniqueName="AgencyName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ComplioID"
                                HeaderText="Complio ID" SortExpression="ComplioID"
                                ItemStyle-HorizontalAlign="Left" UniqueName="ComplioID">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="RotationID"
                                HeaderText="Rotation ID/Name" SortExpression="RotationID"
                                ItemStyle-HorizontalAlign="Left" UniqueName="RotationID">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TypeSpecialty"
                                HeaderText="Type/Speciality" SortExpression="TypeSpecialty"
                                ItemStyle-HorizontalAlign="Left" UniqueName="TypeSpecialty">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Department"
                                HeaderText="Department" SortExpression="Department"
                                ItemStyle-HorizontalAlign="Left" UniqueName="Department">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Program"
                                HeaderText="Program" SortExpression="Program"
                                ItemStyle-HorizontalAlign="Left" UniqueName="Program">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Course"
                                HeaderText="Course" SortExpression="Course"
                                ItemStyle-HorizontalAlign="Left" UniqueName="Course">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Term"
                                HeaderText="Term" SortExpression="Term"
                                ItemStyle-HorizontalAlign="Left" UniqueName="Term">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UnitFloorLoc"
                                HeaderText="Unit/Floor" SortExpression="UnitFloorLoc"
                                ItemStyle-HorizontalAlign="Left" UniqueName="UnitFloorLoc">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Days"
                                HeaderText="Days" SortExpression="Days"
                                ItemStyle-HorizontalAlign="Left" UniqueName="Days">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="RotationShift"
                                HeaderText="Shift" SortExpression="RotationShift"
                                ItemStyle-HorizontalAlign="Left" UniqueName="RotationShift">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Times"
                                HeaderText="Time" SortExpression="Times"
                                ItemStyle-HorizontalAlign="Left" UniqueName="Times">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn  DataField="RotationStartDate" DataFormatString="{0:MM/dd/yyyy}"
                                HeaderText="Start Date" SortExpression="RotationStartDate"
                                ItemStyle-HorizontalAlign="Left" UniqueName="RotationStartDate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="RotationEndDate" DataFormatString="{0:MM/dd/yyyy}"
                                HeaderText="End Date" SortExpression="RotationEndDate"
                                ItemStyle-HorizontalAlign="Left" UniqueName="RotationEndDate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CustomAttributes"
                                HeaderText="Custom Attributes" SortExpression="CustomAttributes"
                                ItemStyle-HorizontalAlign="Left" UniqueName="CustomAttributes">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="InvitationReviewStatusName"
                                HeaderText="Review Status" SortExpression="InvitationReviewStatusName"
                                ItemStyle-HorizontalAlign="Left" UniqueName="InvitationReviewStatusName">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </infs:WclGrid>
            </div>
        </div>
    </div>
</div>
