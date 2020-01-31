<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginCenterPanel.ascx.cs" Inherits="CoreWeb.Shell.Views.LoginCenterPanel" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<div class="login_wrap">
    <div class="mascot">
    </div>
    <asp:Panel ID="pnlLogin" DefaultButton="btnSubmit" runat="server" CssClass="login-form">
        <ul>
            <li>
                <%--<asp:Label ID="lblUserName" runat="server" Text="Username" AssociatedControlID="txtUserName"
                    CssClass="lbl"></asp:Label></li>--%>
                <asp:Label ID="lblUserName" runat="server" Text="<%$Resources:Language,USERNAME %>" AssociatedControlID="txtUserName"
                    CssClass="lbl"></asp:Label></li>
            <li class="loginput">
                <infs:WclTextBox ID="txtUserName" runat="server" Width="100%" />
                <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName"
                    Display="None" ForeColor="#FF0000" ValidationGroup="grpLogin"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ID="revUserName" ValidationGroup="grpLogin"
                    Display="None" ControlToValidate="txtUserName" ForeColor="#FF0000" ValidationExpression="^[\.\@a-zA-Z0-9_-]{4,50}$" /></li>
            <li>
                <%--   <asp:Label ID="lblPassword" runat="server" Text="Password" AssociatedControlID="txtPassword"
                    CssClass="lbl"></asp:Label>--%>
                <asp:Label ID="lblPassword" runat="server" Text="<%$Resources:Language,PASSWORD %>" AssociatedControlID="txtPassword"
                    CssClass="lbl"></asp:Label>
            </li>
            <%--Alert: Caps Lock is on--%>
            <li class="loginput">
                <infs:WclTextBox ID="txtPassword" runat="server" TextMode="Password" Width="100%" onkeyup="checkCapsWarning(event)" onfocus="checkCapsWarning(event)" onblur="removeCapsWarning()" />
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                    Display="None" ForeColor="#FF0000" ValidationGroup="grpLogin"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ID="revPassword" ValidationGroup="grpLogin"
                    ControlToValidate="txtPassword" Display="None" ForeColor="#FF0000" ValidationExpression="^[^&*.;=<>|`]*$" />
                <div style="display: none; color: red; font-size: 12PX; padding-top: 5PX; text-align: center" id="caps"><%=Resources.Language.CAPSLOCKON %></div>
            </li>
            <li class="commands">
                <%--<infs:WclButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" OnClientClicked="btnSubmit_ClientClicked"
                    Text="Sign in" CssClass="button" UseSubmitBehavior="false" ValidationGroup="grpLogin">
                </infs:WclButton>--%>
                <infs:WclButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" OnClientClicked="btnSubmit_ClientClicked"
                    Text="<%$Resources:Language,SIGNIN %>" CssClass="button" UseSubmitBehavior="false" ValidationGroup="grpLogin">
                </infs:WclButton>
                <div class="reset">
                </div>
            </li>
        </ul>
    </asp:Panel>
</div>
<asp:HiddenField runat="server" ID="hdnAccountVerificationPopup" />
<asp:HiddenField runat="server" ID="hdnVerificationCode" />
