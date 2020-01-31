<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ComplianceSearchControl" CodeBehind="ComplianceSearchControl.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearchMultipleNodes.ascx"
    TagName="CustomAttributeLoaderSearch" TagPrefix="uc1" %>
<%@ Register Src="~/CommonControls/UserControl/ColumnsConfiguration.ascx" TagPrefix="infsu" TagName="ColumnsConfiguration" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxComplianceSearch">
    <infs:LinkedResource Path="~/Resources/Generic/ColumnsConfiguration.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style>
    .rbLinkButton {
        display: inline-block;
        height: 25px !important;
        line-height: none !important;
        position: relative;
        border: 1px solid;
        padding: 0 !important;
        cursor: pointer;
        vertical-align: bottom;
        text-decoration: none;
    }


    .height {
        height: auto !important;
    }

    .RadMenu .rmHorizontal .rmText {
        padding: 0 2px 1px 0;
    }

    .rmVertical.rmGroup.rmLevel1 {
        border: none;
    }

    .RadMenu_Default .rmGroup, .RadMenu_Default .rmMultiColumn, .RadMenu_Default .rmGroup .rmVertical {
    }

    .fa-download {
        margin-left: 8px;
    }
</style>
<style type="text/css">
    #menuDiv ul ul li:first-child {
        border-radius: 10px 10px 0px 0px;
    }

    #menuDiv ul ul {
        border-radius: 10px;
    }

        #menuDiv ul ul li:last-child {
            border-radius: 0px 0px 10px 10px;
        }

        #menuDiv ul ul li.rmFirst.rmLast {
            border-radius: 10px 10px 10px 10px;
        }

    .rmVertical.rmGroup {
        background: none;
        background-color: #4382c2;
        border-radius: 10px;
    }

    .btn {
        width: 100%;
        text-align: left;
    }

    .RadMenu .rmGroup .rmText {
        padding: 0px;
        margin: 0px;
    }

    .setZindex {
        z-index: 9 !important;
    }

    .ColConfigBtn {
        padding-bottom: 7px !important;
    }



    /*.RadMenu .rmItem .rmTemplate, .RadToolTip_Default .rtWrapper .rtWrapperTopRight, .RadToolTip_Default .rtWrapper .rtWrapperBottomLeft, .RadToolTip_Default .rtWrapper .rtWrapperBottomRight, .RadToolTip_Default .rtWrapper .rtWrapperTopCenter, .RadToolTip_Default .rtWrapper .rtWrapperBottomCenter, .RadToolTip_Default table.rtShadow .rtWrapperTopLeft, .RadToolTip_Default table.rtShadow .rtWrapperTopRight, .RadToolTip_Default table.rtShadow .rtWrapperBottomLeft, .RadToolTip_Default table.rtShadow .rtWrapperBottomRight, .RadToolTip_Default table.rtShadow .rtWrapperTopCenter, .RadToolTip_Default table.rtShadow .rtWrapperBottomCenter, .RadToolTip_Default .rtCloseButton {
        background-image: none !important;
    }*/
</style>
<%--<style type="text/css">
    .chkAllResults {
        width: 10%;
        float: left;
        margin-top: 10px;
        padding-left: 7px;
    }

    .sxcbar {
        padding-left: 0px !important;
        width: 89%;
    }
</style>--%>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <asp:Label ID="lblVerificationQueue" runat="server" Text=""></asp:Label>
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen" id="divSearchPanel">
        <asp:Panel ID="Panel1" runat="server">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class="reqd">*</span>
                            <%--<infs:WclDropDownList ID="ddlTenantName" runat="server" DataTextField="TenantName"
                                CausesValidation="false" AutoPostBack="true" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                OnDataBound="ddlTenantName_DataBound" Enabled="false">
                            </infs:WclDropDownList>--%>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName"
                                CausesValidation="false" AutoPostBack="true" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                OnDataBound="ddlTenantName_DataBound" Enabled="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <uc1:CustomAttributeLoaderSearch ID="ucCustomAttributeLoader" runat="server" />
            <div class='sxroend'>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Select one or more Category Statuses to restrict search results to those statuses">
                        <span class="cptn">Category Status(s)</span>
                        <infs:WclComboBox ID="chkCategoryStatus" runat="server" CheckBoxes="true" DataTextField="Name"
                            DataValueField="CategoryComplianceStatusID" EmptyMessage="--Select--" Width="100%"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select one or more Item Statuses to restrict search results to those statuses">
                        <span class="cptn">Item Status(s)</span>
                        <infs:WclComboBox ID="chkItemStatus" runat="server" CheckBoxes="true" DataTextField="Name"
                            DataValueField="ItemComplianceStatusID" EmptyMessage="--Select--" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select an Overall Compliance Status to restrict search results to that status">
                        <span class="cptn">Overall Compliance Status(s)</span>
                        <infs:WclComboBox ID="chkOverAllStatus" runat="server" CheckBoxes="true" DataTextField="Name"
                            DataValueField="PackageComplianceStatusID" EmptyMessage="--Select--" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered first name">
                        <span class="cptn">Applicant First Name</span>
                        <infs:WclTextBox ID="txtFirstName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered last name">
                        <span class="cptn">Applicant Last Name</span>
                        <infs:WclTextBox ID="txtLastName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div id="divSSN" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to the entered SSN or ID Number">
                            <span class="cptn">SSN/ID Number</span>
                            <infs:WclMaskedTextBox ID="txtSSNNumber" runat="server" MaxLength="10" Mask="aaa-aa-aaaa"
                                Width="100%" CssClass="form-control">
                            </infs:WclMaskedTextBox>
                        </div>
                    </div>
                    <div id="divDOB" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to the entered Date of Birth">
                            <span class="cptn">Date of Birth</span>
                            <infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="Select a date" Width="100%" CssClass="form-control">
                            </infs:WclDatePicker>
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Order ID">
                        <span class="cptn">Order ID</span>
                        <infs:WclTextBox ID="txtOrderId" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                        <%--<infs:WclTextBox ID="txtOrderId" runat="server">
                        </infs:WclTextBox>--%>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div id="dvRecords" runat="server" visible="false">
                        <div class='form-group col-md-3'>
                            <span class="cptn">Records</span>
                            <asp:RadioButtonList ID="rblOrderState" runat="server" RepeatDirection="Horizontal"
                                DataTextField="As_Name" DataValueField="AS_Code" CssClass="radio_list" Width="100%">
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Select a User Group to restrict search results to that group">
                        <span class="cptn">User Group</span>
                        <infs:WclComboBox ID="ddlUserGroup" runat="server" DataTextField="UG_Name" DataValueField="UG_ID" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            OnDataBound="ddlUserGroup_DataBound" Width="100%" CssClass="form-control" Skin="Silk"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" EmptyMessage="--Select--" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select &#34All&#34 to view all subscriptions per the other parameters or &#34Archived&#34 to view only archived subscriptions or &#34Active&#34 to view only non archived subscriptions">
                        <span class="cptn">Subscription Archive State</span>
                        <asp:RadioButtonList ID="rbSubscriptionState" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbSubscriptionState_SelectedIndexChanged"
                            DataTextField="As_Name" DataValueField="AS_Code" CssClass="radio_list" AutoPostBack="true">
                        </asp:RadioButtonList>
                    </div>
                     <div class='form-group col-md-3' >
                        <span class="cptn">Subscription Expiry State</span>
                        <asp:RadioButtonList ID="rbSubscriptionExpiryState" runat="server" RepeatDirection="Horizontal">
                          <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                          <asp:ListItem Text="Expired " Value="0"></asp:ListItem>
                          <asp:ListItem Text="All" Value="ZZ"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group col-md-3">
                <div class="row">
                    <asp:CheckBox ID="chkSelectAllResults" Text="Select All Results" runat="server" OnCheckedChanged="chkSelectAllResults_CheckedChanged" AutoPostBack="true"
                        Width="100%" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group col-md-9">
                <div class="row" id="trailingText">
                    <%--            <infsu:CommandBar ID="fsucCmdBar1" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel,Clear,Extra"
                        AutoPostbackButtons="Submit,Save,Cancel,Clear,Extra" SubmitButtonIconClass="rbUndo" SubmitButtonText="Reset"
                        SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
                        OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click" OnCancelClick="CmdBarCancel_Click"
                        ClearButtonText="Send Message" OnClearClick="btnSendMessage_Click" ExtraButtonText="Passport Report" OnExtraClick="btnViewReport_Click"
                        ClearButtonIconClass="rbEnvelope" ExtraButtonIconClass="rbPassport" UseAutoSkinMode="false" ButtonSkin="Silk">
                                  <ExtraCommandButtons>
                                  <infs:WclButton ID="btnArchieve" runat="server" Text="Archive" Enabled="true" OnClick="btnArchieve_Click" AutoSkinMode="false" Skin="Silk">
                                <Icon PrimaryIconCssClass="rbArchive"></Icon>
                            </infs:WclButton>
                        </ExtraCommandButtons>
                    </infsu:CommandBar>--%>

                    <div id="menuDiv" runat="server">
                        <infs:WclMenu ID="cmd" runat="server" Skin="Default" AutoSkinMode="false" CssClass="setZindex">
                            <Items>
                                <telerik:RadMenuItem Text="Searchmun">
                                    <ItemTemplate>
                                        <infs:WclButton runat="server" Text="Search" ID="btnSearch" Icon-PrimaryIconCssClass="rbSearch" OnClick="CmdBarSearch_Click" ToolTip="Click to search orders per the criteria entered above" Skin="Silk" AutoSkinMode="false" ButtonPosition="Center">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem Text="Resetmun">
                                    <ItemTemplate>
                                        <infs:WclButton runat="server" Text="Reset" ID="btnReset" Icon-PrimaryIconCssClass="rbUndo" OnClick="CmdBarReset_Click" ToolTip="Click to remove all values entered in the search criteria above" Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" CausesValidation="false" CssClass="btn">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                </telerik:RadMenuItem>
                                <%-- <telerik:RadMenuItem Text="PassportReportmun">
                                    <ItemTemplate>
                                        <infs:WclButton runat="server" Text="Passport Report(All Items)" ID="btnPassportReport" Icon-PrimaryIconCssClass="rbPassport" OnClick="btnViewReport_Click" Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" Visible="false" CssClass="btn">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                </telerik:RadMenuItem>
                                   <telerik:RadMenuItem Text="PassportReportmunApprovedItems">
                                    <ItemTemplate>
                                        <infs:WclButton runat="server" Text="Passport Report(Approved Items)" ID="btnPassportReportApprovedItems" Icon-PrimaryIconCssClass="rbPassport" OnClick="btnPassportReportApprovedItems_Click" Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" Visible="false" CssClass="btn">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                </telerik:RadMenuItem>--%>

                                <telerik:RadMenuItem Text="PassportReportmun">
                                    <ItemTemplate>
                                        <div id="dvPassprotReport" runat="server" class="col-md-4" visible="false">
                                            <infs:WclMenu ID="cmd" runat="server" Skin="Default" AutoSkinMode="false">
                                                <Items>
                                                    <telerik:RadMenuItem Text="Passport Report">
                                                        <ItemTemplate>
                                                            <infs:WclButton runat="server" Text="Passport Report" ID="btnPassport" Icon-PrimaryIconCssClass="fa fa-download download-color"
                                                                Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" AutoPostBack="false">
                                                                <Icon PrimaryIconCssClass="rbSummary"></Icon>
                                                            </infs:WclButton>
                                                        </ItemTemplate>
                                                        <Items>
                                                            <telerik:RadMenuItem>
                                                                <ItemTemplate>
                                                                    <infs:WclButton runat="server" Text="All Items" ID="btnPassportReport" Icon-PrimaryIconCssClass="btnPlane" CssClass="btn" 
                                                                        Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnViewReport_Click">
                                                                        <Icon PrimaryIconCssClass="rbSummary"></Icon>
                                                                    </infs:WclButton>
                                                                </ItemTemplate>
                                                            </telerik:RadMenuItem>
                                                            <telerik:RadMenuItem>
                                                                <ItemTemplate>
                                                                      <infs:WclButton runat="server" Text="Approved Items" ID="btnPassportReportApprovedItems" Icon-PrimaryIconCssClass="btnPlane" CssClass="btn"
                                                                        Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnPassportReportApprovedItems_Click">
                                                                        <Icon PrimaryIconCssClass="rbSummary"></Icon>
                                                                    </infs:WclButton>
                                                                </ItemTemplate>
                                                            </telerik:RadMenuItem>
                                                        </Items>
                                                    </telerik:RadMenuItem>
                                                </Items>
                                            </infs:WclMenu>
                                        </div>
                                    </ItemTemplate>
                                </telerik:RadMenuItem>

                                <telerik:RadMenuItem Text="Cancelmun">
                                    <ItemTemplate>
                                        <infs:WclButton runat="server" Text="Cancel" ID="btnCancel" Icon-PrimaryIconCssClass="rbCancel" OnClick="CmdBarCancel_Click" ToolTip="Click to cancel. Any data entered will not be saved" Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" CssClass="btn" CausesValidation="false">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                </telerik:RadMenuItem>

                                <telerik:RadMenuItem Text="SendMessagemun">
                                    <ItemTemplate>
                                        <infs:WclButton runat="server" Text="Send Message" ID="btnSendMessage" Icon-PrimaryIconCssClass="rbEnvelope" OnClick="btnSendMessage_Click" Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" Visible="false" CssClass="btn">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                </telerik:RadMenuItem>
                               
                                
                                <telerik:RadMenuItem runat="server" id="btnUnArch" Visible="false" Text="UnArchive">
                                    <ItemTemplate>
                                        <infs:WclButton ID="btnUnA" runat="server" Text="UnArchive" Icon-PrimaryIconCssClass="rbArchive" cssClass="btn"
                                            Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnUnArchive_Click" >

                                        </infs:WclButton>

                                    </ItemTemplate>
                                </telerik:RadMenuItem>

                                <telerik:RadMenuItem runat="server" id="radMenubtnArchUnArch" Text="Archivemun">
                                    <ItemTemplate>
                                        <infs:WclButton runat="server" Text="Archive" ID="btnArchive" Icon-PrimaryIconCssClass="rbArchive" CssClass="btn"
                                            Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnArchieve_Click">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                    <Items>
                                        <%--      <telerik:RadMenuItem>
                                            <ItemTemplate>
                                                <infs:WclButton runat="server" Text="Archive" ID="btnArchive" Icon-PrimaryIconCssClass="rbArchive" CssClass="btn"
                                                    Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnArchieve_Click">
                                                </infs:WclButton>
                                            </ItemTemplate>
                                        </telerik:RadMenuItem>--%>
                                        <telerik:RadMenuItem>
                                            <ItemTemplate>
                                                <infs:WclButton runat="server" Text="UnArchive" ID="btnUnArchive" Icon-PrimaryIconCssClass="rbArchive" CssClass="btn"
                                                    Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnUnArchive_Click">
                                                </infs:WclButton>
                                            </ItemTemplate>
                                        </telerik:RadMenuItem>
                                    </Items>
                                </telerik:RadMenuItem>
                            </Items>
                        </infs:WclMenu>

                    </div>

                </div>
            </div>
        </div>
    </div>
    <div id="dvColumnsConfiguration" style="display: none">
        <infsu:ColumnsConfiguration runat="server" ID="ColumnsConfiguration" />
    </div>
    <%--<infsu:CommandBar ID="fsucCmdBar2" runat="server" DisplayButtons="Submit" AutoPostbackButtons="Submit"
            SubmitButtonText="Search" DefaultPanel="pnlRegForm" OnSubmitClick="fsucCmdBar1_SubmitClick" />--%>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdItemData" AllowPaging="True" CssClass="gridhover containsColumnsConfiguration" AutoGenerateColumns="False" AllowFilteringByColumn="false"
            AllowSorting="True" AutoSkinMode="True" CellSpacing="0" GridLines="Both" ShowAllExportButtons="False"
            OnNeedDataSource="grdItemData_NeedDataSource" OnItemCommand="grdItemData_ItemCommand"
            ShowClearFiltersButton="false" AllowCustomPaging="true" OnSortCommand="grdItemData_SortCommand"
            OnInit="grdItemData_Init" OnItemDataBound="grdItemData_ItemDataBound" NonExportingColumns="Detail,DataEntry,SelectUsers,Notes"
            EnableLinqExpressions="false">
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grd_rwDbClick" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="false" IgnorePaging="true">
            </ExportSettings>
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ApplicantComplianceItemID,PackageSubscriptionID,ComplianceCategoryID,CompliancePackageID,ApplicantId"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="SelectUsers" HeaderTooltip="Click this box to select all users on the active page"
                        AllowFiltering="false" ShowFilterIcon="false" ItemStyle-Width="10px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectUser" runat="server"
                                onclick="UnCheckHeader(this)" OnCheckedChanged="chkSelectUser_CheckedChanged" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdfCatId" runat="server" Value='<%# Eval("ComplianceCategoryID") %>' />
                            <asp:HiddenField ID="hdfPackSubscriptionId" runat="server" Value='<%# Eval("PackageSubscriptionID") %>' />
                            <asp:HiddenField ID="hdfPermissionCode" runat="server" Value='<%# Eval("PermissionCode") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="OrderNumber" FilterControlAltText="Filter OrderId column"
                        HeaderText="Order ID" SortExpression="OrderNumber" UniqueName="OrderId" ItemStyle-Width="50px"
                        HeaderTooltip="This column displays the order number for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantLastName" FilterControlAltText="Filter ApplicantLastName column"
                        HeaderText="Applicant Last Name" SortExpression="ApplicantLastName" UniqueName="ApplicantLastName"
                        ItemStyle-Width="100px" HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantFirstName" FilterControlAltText="Filter ApplicantFirstName column"
                        HeaderText="Applicant First Name" SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName"
                        ItemStyle-Width="100px" HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="InstitutionHierarchy" FilterControlAltText="Filter InstitutionHierarchy column"
                        HeaderText="Institution Hierarchy" SortExpression="InstitutionHierarchy" UniqueName="InstitutionHierarchy" ItemStyle-Width="130px"
                        HeaderTooltip="This column displays the institution hierarchy for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CustomAttributes" FilterControlAltText="Filter CustomAttributes column"
                        AllowFiltering="false" HeaderText="Custom Attributes" AllowSorting="false" ItemStyle-Width="130px"
                        UniqueName="CustomAttributes" HeaderTooltip="This column displays the Custom Attributes for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CustomAttributes" AllowFiltering="false" HeaderText="Custom Attributes" AllowSorting="false" ItemStyle-Width="300px"
                        UniqueName="CustomAttributesTemp" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="UserGroupName" FilterControlAltText="Filter UserGroupName column"
                        HeaderText="User Group" SortExpression="UserGroupName" UniqueName="UserGroupName"
                        ItemStyle-Width="130px" HeaderTooltip="This column displays any user group(s) to which the applicant belongs for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter PackageName column"
                        HeaderText="Package Name" SortExpression="PackageName" UniqueName="PackageName"
                        ItemStyle-Width="100px" HeaderTooltip="This column displays the name of the package for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="DateOfBirth" FilterControlAltText="Filter DateOfBirth column"
                        HeaderText="Date of Birth" SortExpression="DateOfBirth" UniqueName="DateOfBirth"
                        ItemStyle-Width="75px" DataFormatString="{0:d}" FilterControlWidth="100px" HeaderTooltip="This column displays the applicant's date of birth for each record in the grid">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="CompliancePackageStatus" FilterControlAltText="Filter CompliancePackageStatus column"
                        HeaderText="Verification Status" SortExpression="CompliancePackageStatus" UniqueName="VerificationStatus"
                        ItemStyle-Width="100px" HeaderTooltip="This column displays the applicant's overall compliance status for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SubscriptionExpirationDate" HeaderText="Subscription Expiration Date"
                        SortExpression="SubscriptionExpirationDate" UniqueName="SubscriptionExpirationDate"
                        ItemStyle-Width="75px" HeaderTooltip="This column displays the Subscription Expiration Date  for each record in the grid"
                        FilterControlAltText="Filter SubscriptionExpirationDate column">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Notes" HeaderText="Notes"
                        SortExpression="Notes" UniqueName="NotesTemp" Display="false"
                        ItemStyle-Width="75px" HeaderTooltip="Notes">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="NonCompliantCategories" HeaderText="Non-Compliant Categories"
                        UniqueName="NonCompliantCategories" Display="false"
                        ItemStyle-Width="75px" AllowFiltering="false" AllowSorting="false" HeaderTooltip="This column displays the Non-Compliant Categories for each record in the grid">
                    </telerik:GridBoundColumn>
                    <%--<telerik:GridBoundColumn DataField="SystemStatus" FilterControlAltText="Filter SystemStatus column"
                            HeaderText="System Status" SortExpression="SystemStatus" UniqueName="SystemStatus">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AssignedUserName" FilterControlAltText="Filter AssignedToUser column"
                            HeaderText="Assigned To User" SortExpression="AssignedUserName" UniqueName="AssignedUserName">
                        </telerik:GridBoundColumn>--%>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Detail" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <%--  <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="ViewDetail"
                                    runat="server" Text="Detail" BackColor="Transparent" Font-Underline="true" BorderStyle="None">
                                </telerik:RadButton>--%>
                            <telerik:RadButton ID="btnDetails" ButtonType="LinkButton" CommandName="VerificationDetails"
                                runat="server" Text="Detail" ToolTip="Click to open the verification screen for this Item.">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DataEntry" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnDataEntry" ButtonType="LinkButton" CommandName="DataEntry"
                                runat="server" Text="Applicant's View" ToolTip="Click here for a read-only view of the applicant's data entry screen">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Notes" UniqueName="Notes" ItemStyle-Width="170px">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnAddShowNotes" ButtonType="LinkButton" CommandName="Notes" runat="server" Text="Add/View Notes" CssClass="height">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <%--                <FilterMenu EnableImageSprites="False">
                </FilterMenu>--%>
        </infs:WclGrid>
    </div>
</div>

<asp:HiddenField ID="hdnSubscriptionIds" runat="server" />
<asp:HiddenField ID="hdMessageSent" runat="server" Value="new" />

<script type="text/javascript">
    //click on link button while double click on any row of grid.    
    function grd_rwDbClick(s, e) {
        var _id = "btnDetails";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

    function CheckAll(id) {
        var masterTable = $find("<%= grdItemData.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectUser").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectUser").checked = isChecked; // for checking the checkboxes
            }
        }
    }

    function UnCheckHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdItemData.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectUser").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectUser").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }

    function OpenPopup(sender, eventArgs) {
        var composeScreenWindowName = "composeScreen";
        var fromScreenName = "ComplianceSearch";
        var communicationTypeId = 'CT01';
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/Messaging/Pages/WriteMessage.aspx?cType=" + communicationTypeId + "&SName=" + fromScreenName);
        var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName, onclose: OnMessagePopupClose });
        return false;
    }

    function OpenReportPopup() {
        //debugger;
        var composeScreenWindowName = "Report Viewer";
        var fromScreenName = "PortfolioSearch";
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $jQuery('[id$="hdnSubscriptionIds"]').val();
        var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName });
        return false;
    }

    //This event fired when multiple subscription popup closed.
    function OnMessagePopupClose(oWnd, args) {
        oWnd.remove_close(OnMessagePopupClose);
        var arg = args.get_argument();
        if (arg) {
            if (arg.MessageSentStatus == "sent") {
                $jQuery("[id$=hdMessageSent]").val("sent");
                var masterTable = $find("<%= grdItemData.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        }
    }

    function openAddNotesPopUp(pkgSubscriptionId, selectedTenantId) {
        openAddNotesPopUp
        var composeScreenWindowName = "Copliance Search Note";
        var width = (window.screen.width) * (60 / 100);
        var height = (window.screen.height) * (50 / 100);
        var popupsize = width + ',' + height;
        var url = $page.url.create("~/ComplianceOperations/Pages/ComplianceSearchNotesPopUp.aspx?pkgSubscriptionId=" + pkgSubscriptionId + "&selectedTenantId=" + selectedTenantId);
        var win = $window.createPopup(url, {
            size: popupsize, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize
                | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close
                | Telerik.Web.UI.WindowBehaviors.Modal, name: composeScreenWindowName, onclose: function () {
                    var masterTable = $find("<%= grdItemData.ClientID %>").get_masterTableView();
                    masterTable.rebind();
                }

        }
                );
            winopen = true;
            return false;
        }

        function pageLoad() {


            SetDefaultButtonForSection("divSearchPanel", "btnSearch", true);
        }

</script>
