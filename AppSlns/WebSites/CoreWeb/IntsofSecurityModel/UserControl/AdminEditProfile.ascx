<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.AdminEditProfile" CodeBehind="AdminEditProfile.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<%@ Register TagPrefix="infsu" TagName="TwoFactorAuthentication" Src="~/CommonOperations/TwoFactorAuthenticationSettings.ascx" %>
<div class="section">
    <h1 class="mhdr">Update Profile
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox" id="msgBox" runat="server">
                <asp:Label ID="lblMessage" runat="server" CssClass="info"></asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlEditProfile">
                <h1 class="shdr">Account Information
                </h1>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class='cptn'>Username</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtUsername" MaxLength="256">
                        </infs:WclTextBox>&nbsp;
                        <div class="vldx">
                            <span class="errmsg">
                                <asp:Label ID="lblUserNameMessage" runat="server" Text=""></asp:Label>
                            </span>
                        </div>
                    </div>
                    <div class='sxlm'>
                        <infs:WclButton runat="server" ID="btnCheckUsername" Text="Check" OnClick="btnCheckUsername_Click"
                            Skin="Windows7" AutoSkinMode="false" CausesValidation="false">
                        </infs:WclButton>
                    </div>
                    <div class='sxlb'>
                        <span class='cptn'>Change Password</span>
                    </div>
                    <div class='sxlm'>
                        <asp:HyperLink ID="lnkChangePassword" runat="server" CssClass="user" Text="Change Password"> </asp:HyperLink>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <h1 class="shdr">Personal Information
                </h1>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class='cptn'>First Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtFirstName" ReadOnly="true">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        <span class='cptn'>Last Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtLastName" ReadOnly="true">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        <span class='cptn'>Primary Email</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtPrimaryEmail" runat="server" ReadOnly="true">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div id="dvTwoFactorAuthentication" runat="server" style="display: none;">
                    <h1 class="shdr">Two Factor Authentication</h1>
                    <infsu:TwoFactorAuthentication ID="TwoFactorAuthentication" Visible="true" runat="server" />
                </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="cBarMain" runat="server" DisplayButtons="Save,Cancel,Clear" AutoPostbackButtons="Save,Cancel,Clear"
            ButtonPosition="Center" SaveButtonText="Save" DefaultPanel="pnlEditProfile"
            OnSaveClick="fsucCmdBar1_SaveClick" OnCancelClick="fsucCmdBar1_CancelClick"
           ClearButtonText="Link Account" OnClearClick="cBarMain_ClearClick" />
    </div>
</div>
