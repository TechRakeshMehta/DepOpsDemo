<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Reports.Views.RotationStudentsNonComplianceStatus"
    CodeBehind="RotationStudentsNonComplianceStatus.ascx.cs" EnableTheming="true" %>
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
        This report lists students in current rotations who are non-compliant.                        
            <ul class="list">
                <li class="listcontent">Can be filtered by institution, by agency hierarchy and by Category.</li>
                <li class="listcontent">Does not show students in upcoming rotations.</li>
                <li class="listcontent">Only shows profiles with Approved status.</li>
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
                        EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                        OnClientDropDownClosed="FillCategories">
                    </infs:WclComboBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvAgency" ControlToValidate="cmbAgency" ValidationGroup="btnsubmit"
                            Display="Dynamic" CssClass="errmsg" Text="Agency is required." />
                    </div>
                </div>
                <div class="form-group col-md-3">
                    <span class="cptn">Category</span><span class="reqd">*</span>
                    <infs:WclComboBox ID="cmbCategory" Width="100%" CssClass="form-control" runat="server" AllowCustomText="true"
                        AutoPostBack="false" DataTextField="Value" DataValueField="Key" EmptyMessage="--SELECT--"
                        Skin="Silk" AutoSkinMode="false"
                        EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Filter="Contains">
                    </infs:WclComboBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvCategory" ControlToValidate="cmbCategory" ValidationGroup="btnsubmit"
                            Display="Dynamic" CssClass="errmsg" Text="Category is required." />
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
            </div>
            <div class="row">
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
        <asp:Button ID="btnAgencies" runat="server" OnClick="btnAgencies_Click" />
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
                    AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0" ShowGroupPanel="True"
                    ShowAllExportButtons="False" ShowExtraButtons="False" ShowClearFiltersButton="false" 
                    GridLines="Both" PageSize="50" OnNeedDataSource="grdReportData_NeedDataSource" ExportSettings-FileName="Report">
                    
                    <ExportSettings ExportOnlyData="False" FileName="Report" UseItemStyles="true" IgnorePaging="True" OpenInNewWindow="True"
                        Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true">
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" AllowFilteringByColumn="false">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                            ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="TenantName"
                                HeaderText="Institution" SortExpression="TenantName" UniqueName="TenantName"
                                ItemStyle-HorizontalAlign="Left">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Agency"
                                HeaderText="Agency" SortExpression="Agency" UniqueName="Agency" 
                                ItemStyle-HorizontalAlign="Left">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ComplioID"
                                HeaderText="ComplioID" SortExpression="ComplioID" UniqueName="ComplioID"
                                ItemStyle-HorizontalAlign="Left">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StudentName"
                                HeaderText="Name" SortExpression="StudentName" UniqueName="StudentName"
                                ItemStyle-HorizontalAlign="Left">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StudentEmail"
                                AllowFiltering="false" HeaderText="Email" AllowSorting="false"
                                UniqueName="StudentEmail" ItemStyle-HorizontalAlign="Left">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="OutOfComplianceDate"
                                AllowFiltering="false" HeaderText="Out Of Compliance Date" SortExpression="OutOfComplianceDate"
                                UniqueName="OutOfComplianceDate" ItemStyle-HorizontalAlign="Left">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NonComplianceRequirements"
                                HeaderText="Non Compliance Requirements" SortExpression="NonComplianceRequirements" UniqueName="NonComplianceRequirements" ItemStyle-HorizontalAlign="Left">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SharedBy"
                                HeaderText="Shared By" SortExpression="SharedBy" UniqueName="SharedBy" ItemStyle-HorizontalAlign="Left">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SharedByEmail"
                                HeaderText="Shared By Email" SortExpression="SharedByEmail" UniqueName="SharedByEmail" ItemStyle-HorizontalAlign="Left">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ReviewStatus"
                                HeaderText="Review Status" SortExpression="ReviewStatus" UniqueName="ReviewStatus" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-Width="5%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ReviewedBy"
                                HeaderText="Reviewed By" SortExpression="ReviewedBy" UniqueName="ReviewedBy" ItemStyle-HorizontalAlign="Left">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="RotationEndDate" DataFormatString="{0:MM/dd/yyyy}"
                                HeaderText="Rotation End Date" SortExpression="RotationEndDate" UniqueName="RotationEndDate" ItemStyle-HorizontalAlign="Left">
                            </telerik:GridBoundColumn>                            
                             <telerik:GridBoundColumn DataField="UserType" 
                                HeaderText="User Type" SortExpression="UserType" UniqueName="RotationUserType" ItemStyle-HorizontalAlign="Left">
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
<script type="text/javascript">
    function FillAgencies(sender, eventArgs) {
        $jQuery("[id$=btnTenants]").click();  
    }

    function FillCategories(sender, eventArgs) {
        $jQuery("[id$=btnAgencies]").click();
       
    }
</script>
