<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OtherAccountLinking.ascx.cs"
    Inherits="CoreWeb.IntsofSecurityModel.Views.OtherAccountLinking" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style type="text/css"></style>

<div class="section">
    <h1 class="mhdr"><%=Resources.Language.LINKACCOUNT %></h1>
    <div class="content">
        <div class="msgbox" id="msgBox" runat="server">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </div>
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlRegForm">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class='cptn'><%=Resources.Language.EMAILADD %></span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtEmailAddress" MaxLength="100">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvEmailAddress" ControlToValidate="txtEmailAddress"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,EMAILADDRESSREQ %>" ValidationGroup="grpLinkAccount" />
                            <asp:RegularExpressionValidator ID="VldtxtEmail" runat="server" Display="Dynamic" ValidationGroup="grpLinkAccount"
                                ErrorMessage="<%$Resources:Language,EMAILNOTVALID %>" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                ControlToValidate="txtEmailAddress" CssClass="errmsg"> 
                            </asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>
<infsu:CommandBar ID="fsucCmdBar" runat="server" DisplayButtons="Submit,Cancel" AutoPostbackButtons="Submit,Cancel"
    ButtonPosition="Center" SubmitButtonText="<%$Resources:Language,SEARCH %>" SubmitButtonIconClass="rbSearch" OnSubmitClick="fsucCmdBar_SubmitClick"  
    CancelButtonText="<%$Resources:Language,CNCL%>" CancelButtonIconClass="" OnCancelClick="fsucCmdBar_CancelClick" ValidationGroup="grpLinkAccount">
</infsu:CommandBar>
<div id="dvlstExistingUsers" runat="server" visible="false">
    <div class="section">
        <h1 class="mhdr"><%=Resources.Language.MATCHEDUSER %></h1>
        <%-- Matched User(s)--%>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel runat="server" CssClass="sxpnl" ID="Panel1">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <asp:Label ID="lblMatchedUserName" runat="server" Text=""></asp:Label><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox TabIndex="8" runat="server" ID="txtPassword" TextMode="Password" MaxLength="15"
                                autocomplete="off">
                            </infs:WclTextBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvPassword" ControlToValidate="txtPassword"
                                    ValidationGroup="grpCredentialsSubmit" Display="Dynamic" ErrorMessage="<%$Resources:Language,PASSWORDREQ %>" CssClass="errmsg" />
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>

    </div>
    <infsu:CommandBar ID="cmbUserCredentials" runat="server" DisplayButtons="Submit,Cancel" AutoPostbackButtons="Submit,Cancel"
        ButtonPosition="Center" SubmitButtonText="<%$Resources:Language,VALIDATE %>" SubmitButtonIconClass="" OnSubmitClick="cmbUserCredentials_SubmitClick"
         CancelButtonText="<%$Resources:Language,CNCL%>" CancelButtonIconClass="" OnCancelClick="fsucCmdBar_CancelClick" ValidationGroup="grpCredentialsSubmit">
    </infsu:CommandBar>
</div>
<div id="dvUserDataSelection" runat="server" visible="false">
    <div class="section">
        <h1 class="mhdr"><%=Resources.Language.SELECTUSERTORETAIN %></h1>  <%--Select the User for which you wish to retain username and email address.--%>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlUserDataSelection">
                    <div class="content">
                        <div class="sxform auto">
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                     <asp:RadioButton runat="server" ID="rbSelectedUser1" GroupName="rblGroup"></asp:RadioButton>
                                    <span class='cptn'><%=Resources.Language.USERNAME %></span><span class="reqd">*</span> <%--User Name, using Username--%> 
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblUserName1" runat="server" CssClass="info"><</asp:Label>
                                    <%--Text='<%# Eval("UserName") %>'--%>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.EMAILADD %></span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblEmailAddress1" runat="server" CssClass="info"><</asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="sxform auto">
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <asp:RadioButton runat="server" ID="rbSelectedUser2" GroupName="rblGroup"></asp:RadioButton>
                                    <span class='cptn'><%=Resources.Language.USERNAME %></span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblUserName2" runat="server" CssClass="info"><</asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.EMAILADD %></span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblEmailAddress2" runat="server" CssClass="info"><</asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <infsu:CommandBar ID="cmbLinkAccount" runat="server" DisplayButtons="Submit,Cancel" AutoPostbackButtons="Submit,Cancel"
        ButtonPosition="Center" SubmitButtonText="<%$Resources:Language,LINKACCOUNT%>" SubmitButtonIconClass="rbSave" OnSubmitClick="cmbLinkAccount_SubmitClick"
        CancelButtonText="<%$Resources:Language,CNCL%>" CancelButtonIconClass="" OnCancelClick="fsucCmdBar_CancelClick">
    </infsu:CommandBar>
</div>
