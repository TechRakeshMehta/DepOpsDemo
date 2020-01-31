<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientProfile.ascx.cs"
    Inherits="CoreWeb.Search.Views.ClientProfile" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="CustomAttribute" Src="~/ComplianceAdministration/UserControl/CustomAttributeLoader.ascx" %>

<%--UAT-3326--%>
<%@ Register Src="~/SearchUI/UserControl/NewApplicantProfileNotes.ascx" TagPrefix="uc"
    TagName="ApplicantProfileNotes" %>

<infs:WclResourceManagerProxy runat="server" ID="rmpClientProfile">
    <infs:LinkedResource Path="~/Resources/Mod/SearchUI/ClientProfile.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/mapping_pages.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/mapping_pages.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>


<style>
    a.linkStyle {
        float: right;
        padding-right: 3px;
    }
</style>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-10"></div>
            <div class="col-md-2 text-right">
                <div runat="server" id="divBackToConfiguration" visible="false">
                    <infs:WclButton runat="server" ID="btnBackToQueue" Text="Back To Queue" OnClick="btnBackToQueue_Click"
                        Height="30px" AutoSkinMode="false" Skin="Silk" ButtonType="LinkButton" AutoPostBack="true">
                    </infs:WclButton>
                </div>
            </div>
        </div>
    </div>

    <div class="section" runat="server" id="divClientUserDetails">
        <h1 class="mhdr">Client User Details</h1>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlClientProfile">
                    <asp:LinkButton ID="lnkGoBack" runat="server" CssClass="linkStyle" Text="Back to Search"
                        OnClick="lnkGoBack_Click"></asp:LinkButton>
                    <h1 class="shdr">Personal Information</h1>
                    <div class='sxro sx3co'>
                        <div class='sxlb' title="The User Name of this client user">
                            <span class="cptn">User Name</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtUserName" MaxLength="50" Enabled="false">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxlb' title="The First Name of this client user">
                            <span class="cptn">First Name</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtFirstName" MaxLength="256" Enabled="false">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxlb' title="The Last Name of this client user">
                            <span class="cptn">Last Name</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtLastName" MaxLength="50" Enabled="false">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb' title="The Phone Number of this client user">
                            <span class="cptn">Phone Number</span>
                        </div>
                        <div class='sxlm'>
                            <telerik:RadMaskedTextBox Mask="(###)-###-####" runat="server" ID="txtPhone1" MaxLength="50"
                                Enabled="false">
                            </telerik:RadMaskedTextBox>
                            <telerik:RadTextBox runat="server" ID="txtPhoneUnMasking" Enabled="false"></telerik:RadTextBox>
                        </div>
                        <div class='sxlb' title="The Email Address of this client user">
                            <span class="cptn">Email Address</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtEmail" MaxLength="50" Enabled="false">
                            </infs:WclTextBox>
                        </div>

                        <div class='sxlb' title="Allow Message">
                            <span class="cptn">Allow Message</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclButton ID="btnNotifications" runat="server" OnClientClicked="MaangeNotiication">
                            </infs:WclButton>
                            <asp:HiddenField ID="hdfCrntUsrId" runat="server" />
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>

                    <div class='sxro sx3co'>
                        <div class='sxlb' title="The locked status of this client user">
                            <span class="cptn">Lock/Unlock User</span>
                        </div>
                        <div class='sxlm'>
                            <%-- <infs:WclCheckBox Enabled="true" ID="chkUnlockUser" AutoPostBack="true" OnCheckedChanged="chkUnlockUser_CheckedChanged" runat="server" />--%>
                            <infs:WclButton ID="btnUnlockUser" ButtonType="StandardButton" runat="server" AutoPostBack="true"
                                OnClick="btnUnlockUser_Click">
                            </infs:WclButton>
                        </div>
                        <div class='sxlb' title="The Assigned Role(s) of this client user">
                            <span class="cptn">Assigned Role(s)</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtAssignedRoles" Enabled="false">
                            </infs:WclTextBox>
                        </div>
                        <div id="divTwoFactorAuth" runat="server">
                            <div class='sxlb' title="Google Authenticator">
                                <span class="cptn">Two Factor Authentication</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclButton ID="btnTwoFactorAuthentication" runat="server" CausesValidation="false"
                                    OnClientClicked="ManageTwoFactorAuthentication" AutoPostBack="false">
                                </infs:WclButton>
                                <asp:HiddenField ID="hdnUserId" runat="server" />
                                <asp:HiddenField ID="hdnTenantId" runat="server" />
                                <asp:HiddenField ID="hdnCurrentUserId" runat="server" />
                            </div>


                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>

        <telerik:RadCaptcha ID="radCpatchaPassword" runat="server" CaptchaImage-TextChars="LettersAndNumbers"
            CaptchaImage-TextLength="10" Visible="false" Display="Dynamic" />
    </div>
    <div class="row" runat="server" id="divClientUserDetails1">
        <div class="col-md-12">
            <infsu:CommandBar ID="cmdbar_ClientProfile" runat="server" DefaultPanel="pnlClientProfile"
                ExtraButtonText="Reset Password" ExtraButtonIconClass="rbReset" OnExtraClick="cmdbar_ClientProfile_ExtraClick"
                AutoPostbackButtons="Extra" DisplayButtons="Extra" UseAutoSkinMode="false" ButtonSkin="Silk" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <infsu:CustomAttribute ID="caProfileCustomAttributes" Title="Profile Information" runat="server" />
            <infsu:CommandBar ID="cmb_EditProfile" runat="server" SaveButtonIconClass="rbsave" DisplayButtons="Save" ClearButtonText="Edit" ValidationGroup="grpFormSubmit" AutoPostbackButtons="Save"
                SaveButtonText="Save" OnSaveClick="cmb_EditProfile_SaveClick" UseAutoSkinMode="false" ButtonSkin="Silk">
            </infsu:CommandBar>
        </div>
    </div>
    <%--UAT-3326--%>
    <div runat="server" id="divProfileNotes" visible="true">
        <div class='row'>
            <div class='col-md-12'>
                <h2 class="header-color" tabindex="0">Profile Note(s)</h2>
            </div>
        </div>
        <uc:ApplicantProfileNotes ID="ucApplicantNotes" runat="server" Visible="true"></uc:ApplicantProfileNotes>
    </div>
      <div class="row">&nbsp;</div>
    <%--UAT-3326--%>

    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Institution Hierarchy Information - Immunization Compliance</h2>
        </div>
    </div>

    <div class="row">


        <asp:Panel runat="server" ID="pnlHiearchyInfo">
            <infs:WclTreeList ID="treelstHiearachyDetails" runat="server" DataTextField="Value"
                ParentDataKeyNames="ParentNodeID,ParentDataID,ParentNodeCode" DataKeyNames="NodeID,DataID,NodeCode"
                OnNeedDataSource="treelstHiearachyDetails_NeedDataSource" DataMember="Assigned"
                OnItemCreated="treelstHiearachyDetails_ItemCreated" AutoGenerateColumns="false"
                DataValueField="UICode" OnPreRender="treelstHiearachyDetails_PreRender">
                <Columns>
                    <telerik:TreeListBoundColumn DataField="Value" UniqueName="Value" HeaderText="Node Name" />
                    <telerik:TreeListBoundColumn DataField="PermissionName" UniqueName="NodePermission"
                        HeaderText="Hierarchy Permission Type" HeaderStyle-Width="18%" />
                    <telerik:TreeListBoundColumn DataField="ProfilePermissionName" UniqueName="ProfilePermission"
                        HeaderText="Profile Permission Type" HeaderStyle-Width="18%" />
                    <telerik:TreeListBoundColumn DataField="VerificationPermissionName" UniqueName="VerificationPermission"
                        HeaderText="Verification Permission Type" HeaderStyle-Width="18%" />
                    <telerik:TreeListBoundColumn DataField="OrderPermissionName" UniqueName="OrderPermission"
                        HeaderText="Order Permission Type" HeaderStyle-Width="18%" />
                    <telerik:TreeListBoundColumn DataField="PackagePermissionName" UniqueName="PackagePermission"
                        HeaderText="Package Permission Type" HeaderStyle-Width="18%" />
                </Columns>
            </infs:WclTreeList>
        </asp:Panel>

    </div>


    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Institution Hierarchy Information - Background Check</h2>
        </div>
    </div>
    <div class="row">

        <asp:Panel runat="server" ID="pnlBkgHiearchyInfo">
            <infs:WclTreeList ID="treelstBkgHiearachyDetails" runat="server" DataTextField="Value"
                ParentDataKeyNames="ParentNodeID,ParentDataID,ParentNodeCode" DataKeyNames="NodeID,DataID,NodeCode"
                OnNeedDataSource="treelstBkgHiearachyDetails_NeedDataSource" DataMember="Assigned"
                OnItemCreated="treelstBkgHiearachyDetails_ItemCreated" AutoGenerateColumns="false"
                DataValueField="UICode" OnPreRender="treelstBkgHiearachyDetails_PreRender">
                <Columns>
                    <telerik:TreeListBoundColumn DataField="Value" UniqueName="Value" HeaderText="Node Name" />
                    <telerik:TreeListBoundColumn DataField="PermissionName" UniqueName="NodePermission"
                        HeaderText="Hierarchy Permission Type" HeaderStyle-Width="18%" />
                </Columns>
            </infs:WclTreeList>
        </asp:Panel>

    </div>


    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Notification Settings Information</h2>
        </div>
    </div>
    <div class="row">


        <asp:Panel runat="server" ID="pnlNotificationSetup">
            <infs:WclGrid runat="server" ID="grdNotificationSetup" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="True" ShowAllExportButtons="false" ShowExtraButtons="true"
                ShowClearFiltersButton="false"
                GridLines="None" OnNeedDataSource="grdNotificationSetup_NeedDataSource">
                <MasterTableView CommandItemDisplay="Top" AllowPaging="false" AutoGenerateColumns="False"
                    AllowSorting="false"
                    AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="false"
                        ShowExportToPdfButton="false" ShowExportToCsvButton="false" ShowExportToWordButton="false"
                        ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="CommunicationCCMaster.lkpCommunicationSubEvent.Name"
                            FilterControlAltText="Filter Sub Event column"
                            HeaderText="Sub Event Type" UniqueName="SubEvent">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="lkpCopyType.CT_Name" FilterControlAltText="Filter CopyType column"
                            HeaderText="Copy Type" UniqueName="CopyType">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="false" Visible="false" />
                </MasterTableView>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </asp:Panel>

    </div>


    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Feature Action Permission Information</h2>
        </div>
    </div>
    <div class="row">


        <asp:Panel runat="server" ID="pnlFeaturePermissionInfo">
            <infs:WclTreeList ID="treelstFeaturePermissionDetails" runat="server" DataTextField="Name"
                ParentDataKeyNames="ParentDataID" DataKeyNames="DataID"
                OnNeedDataSource="treelstFeaturePermissionDetails_NeedDataSource" DataMember="Assigned"
                OnItemCreated="treelstFeaturePermissionDetails_ItemCreated" AutoGenerateColumns="false"
                DataValueField="Code" OnPreRender="treelstFeaturePermissionDetails_PreRender">
                <Columns>
                    <telerik:TreeListBoundColumn DataField="Name" UniqueName="Name" HeaderText="Feature/Action Name" />
                    <telerik:TreeListBoundColumn DataField="SelectedPermission" UniqueName="SelectedPermission"
                        HeaderText="Permission Type" />
                </Columns>
            </infs:WclTreeList>
        </asp:Panel>

    </div>

    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Granular Permissions Information</h2>
        </div>
    </div>
    <div class="row">


        <asp:Panel runat="server" ID="Panel1">
            <infs:WclGrid runat="server" ID="grdEntityPermissions" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="True" ShowAllExportButtons="false" ShowExtraButtons="true"
                ShowClearFiltersButton="false"
                GridLines="None" OnNeedDataSource="grdEntityPermissions_NeedDataSource">
                <MasterTableView CommandItemDisplay="Top" AllowPaging="false" AutoGenerateColumns="False" DataKeyNames="HierarchyNodeId"
                    AllowSorting="false"
                    AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="false"
                        ShowExportToPdfButton="false" ShowExportToCsvButton="false" ShowExportToWordButton="false"
                        ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="PermissionTypeName" FilterControlAltText="Filter PermissionTypeName column"
                            HeaderText="Permission Type" UniqueName="PermissionTypeName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="HierarchyNodeLabel" FilterControlAltText="Filter HierarchyNodeLabel column"
                            HeaderText="Hierarchy" UniqueName="HierarchyNodeLabel">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PermissionName" FilterControlAltText="Filter PermissionName column"
                            HeaderText="Permission" UniqueName="PermissionName">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="false" Visible="false" />
                </MasterTableView>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </asp:Panel>


    </div>
</div>
