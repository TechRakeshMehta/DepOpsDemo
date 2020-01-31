<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Search.Views.ApplicantPortfolioDetails" CodeBehind="ApplicantPortfolioDetails.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/SearchUI/UserControl/ApplicantPortfolioProfile.ascx" TagPrefix="infsu"
    TagName="ApplicantPortfolioProfile" %>
<%@ Register Src="~/SearchUI/UserControl/ApplicantPortfolioCustomAttributes.ascx"
    TagPrefix="infsu" TagName="ApplicantPortfolioCustomAttributes" %>
<%@ Register Src="~/SearchUI/UserControl/ApplicantPortfolioOrderHistory.ascx" TagPrefix="infsu"
    TagName="ApplicantPortfolioOrderHistory" %>
<%@ Register Src="~/SearchUI/UserControl/ApplicantPortfolioSubscription.ascx" TagPrefix="infsu"
    TagName="ApplicantPortfolioSubscription" %>

<%@ Register Src="~/ComplianceOperations/UserControl/NewApplicantCommunicationGridControl.ascx" TagPrefix="infsu"
    TagName="ApplicantCommunicationGridControl" %>
<%@ Register Src="~/ComplianceOperations/UserControl/ApplicantOrderNotificationHistoryGridControl.ascx" TagPrefix="infsu"
    TagName="ApplicantOrderNotificationHistoryGridControl" %>

<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>
<%@ Register Src="~/SearchUI/UserControl/ApplicantRequirementRotations.ascx" TagPrefix="infsu" TagName="ApplicantRequirementRotations" %>
<%@ Register Src="~/SearchUI/UserControl/ApplicantPortfolioIntegration.ascx" TagPrefix="infsu" TagName="ApplicantPortfolioIntegration" %>


<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="../Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Grid-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/SearchUI/ApplicantPortfolioDetails.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="container-fluid">

    <div class="row">
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="row">
                    <h2 class="header-color" tabindex="0">
                        <asp:Label ID="lblApplicantPortFolioSearch" runat="server" Text="Applicant Portfolio" ToolTip="Details pertaining to the student's user account are displayed in this section"></asp:Label>
                    </h2>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row text-right">
                    <a runat="server" id="lnkGoBack" onclick="openInPageFrame(this); return false;">Back to Search</a>
                </div>
            </div>
        </div>
    </div>
    <infsu:ApplicantPortfolioProfile ID="ucApplicantPortfolioProfile" runat="server" />
    &nbsp;&nbsp;
        <%--<infsu:ApplicantPortfolioCustomAttributes ID="ucApplicantPortfolioCustomAttributes"
            runat="server" />--%>
    <div id="divUserKeyAttributes" runat="server">
        <telerik:RadCaptcha ID="radCpatchaPassword" runat="server" CaptchaImage-TextChars="LettersAndNumbers"
            CaptchaImage-TextLength="10" Visible="false" Display="Dynamic" />
        <div class="row bgLightGreen">
            <asp:Panel runat="server" ID="pnlMUser">
                <div class='col-md-12'>
                    <div class="row">
                        <div class='form-group col-md-3 noUnderline'>
                            <span class="cptn">Active</span>
                            <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" IsAutoPostBack="false" />
                        </div>
                        <div class="form-group col-md-3">
                            <span class="cptn">Locked</span>

                            <infs:WclButton runat="server" ID="chkLocked" CssClass="noUnderline" ToggleType="CheckBox" ButtonType="ToggleButton"
                                AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>
                        </div>
                        <div id="divtwofactorAuthentication" runat="server" class='form-group col-md-3 noUnderline'>
                            <span class="cptn">Two Factor Authentication Setting</span>
                            <%--<asp:RadioButtonList ID="rdbTwoFactorAuthentication" runat="server"
                                RepeatDirection="Horizontal">
                                <asp:ListItem Text="Enabled &nbsp;" Value="True"></asp:ListItem>
                                <asp:ListItem Text="Disabled" Value="False"></asp:ListItem>
                            </asp:RadioButtonList>--%>
                            <%--<asp:HiddenField ID="hdnIsTwoFactorAuthenticationPrevious" runat="server" Value="False" />--%>
                            <asp:RadioButtonList ID="rdbSpecifyAuthentication" Width="350px" ClientIDMode="Static" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Disable &nbsp;" Value="NONE"></asp:ListItem>
                                <asp:ListItem Text="Google Authenticator &nbsp;" Value="AAAA" Enabled="false"></asp:ListItem>
                                <asp:ListItem Text="Text Message" Value="AAAB" Enabled="false"></asp:ListItem>
                            </asp:RadioButtonList>
                            <span id="spnIsTwoFactorAuthVerified" runat="server"></span>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="fsucCmdBarPortfolio" runat="server" DefaultPanel="pnlMUser"
            ExtraButtonText="Reset Password" ExtraButtonIconClass="rbReset" OnExtraClick="fsucCmdBarPortfolio_ExtraClick"
            OnSaveClick="fsucCmdBarPortfolio_SaveClick"
            AutoPostbackButtons="Extra, Save" UseAutoSkinMode="false" ButtonSkin="Silk" />
        <%--DisplayButtons="Save,Extra"--%>
    </div>
    <div runat="server" id="divIntegrationSection">
        <infsu:ApplicantPortfolioIntegration runat="server" ID="ucApplicantPortfolioIntegration" />
    </div>
    <infsu:ApplicantPortfolioSubscription ID="ucApplicantPortfolioSubscription" runat="server" />
    <infsu:ApplicantPortfolioOrderHistory ID="ucApplicantPortfolioOrderHistory" runat="server" />
    <infsu:ApplicantCommunicationGridControl ID="ucApplicantCommunicationGridControl" runat="server" />
    <infsu:ApplicantOrderNotificationHistoryGridControl ID="ucApplicantOrderNotification" runat="server" />
    <infsu:ApplicantRequirementRotations ID="ucApplicantRequirementRotations" runat="server" />

    <asp:HiddenField ID="hdnIsLocationTenant" runat="server" Value="false" />
    <asp:Panel ID="pnlInvitationsList" runat="server">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Manage Invitations
                </h2>
            </div>
        </div>
        <div class="row  allowscroll">
            <infs:WclGrid ID="grdInvitations" AutoGenerateColumns="false" runat="server" AutoSkinMode="True"
                AllowSorting="True" PageSize="10" CellSpacing="0" ShowAllExportButtons="false" MasterTableView-DataKeyNames="ID"
                ShowExtraButtons="true" ShowClearFiltersButton="false" EnableLinqExpressions="false" GridLines="None"
                OnItemDataBound="grdInvitations_ItemDataBound" OnNeedDataSource="grdInvitations_NeedDataSource" OnItemCommand="grdInvitations_ItemCommand">
                <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="False" AllowSorting="false"
                    AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter InviteeName column" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Invitee Name" SortExpression="Name" UniqueName="Name"
                            HeaderTooltip="This column displays the Invitee's name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EmailAddress" FilterControlAltText="Filter InviteeEmail column" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Email" SortExpression="EmailAddress" UniqueName="EmailAddress"
                            HeaderTooltip="This column displays the Invitee's email for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Phone" FilterControlAltText="Filter Invitee Phone column" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Phone" SortExpression="Phone" UniqueName="Phone"
                            HeaderTooltip="This column displays the Invitee's phone for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Agency" FilterControlAltText="Filter Invitee Agency column" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Agency" SortExpression="Agency" UniqueName="Agency"
                            HeaderTooltip="This column displays the Invitee's Agency for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiration Date" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblExpirationDate" runat="server" Text='<%# Eval("ExpirationDate", "{0:d}") %>' Visible='<%# Eval("IsExpirationDateVisible") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Views Remaining" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblViewsLeft" runat="server" Text='<%# Eval("ViewsRemaining") %>' Visible='<%# Eval("IsExpirationCountVisible") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="InvitationDate" FilterControlAltText="Filter Invitation Date column" DataFormatString="{0:d}"
                            HeaderText="Invitation Date" SortExpression="InvitationDate" UniqueName="InvitationDate" HeaderStyle-HorizontalAlign="Center"
                            HeaderTooltip="This column displays the Invitee's invitation date for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LastViewedDate" FilterControlAltText="Filter Invitee Last Viewed column" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Last Viewed" SortExpression="LastViewedDate" UniqueName="LastViewedDate"
                            HeaderTooltip="This column displays the Invitee's last view for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewAttestation" runat="server" Text="View Attestation(s)" CommandName="ViewAttestation" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </infs:WclGrid>
        </div>
    </asp:Panel>
    <%--</div>--%>
</div>
<iframe id="ifrExportDocument" runat="server" height="0" width="0"></iframe>
 
