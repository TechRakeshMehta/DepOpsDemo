<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.ForgotPassword" Codebehind="ForgotPassword.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<infs:WclResourceManagerProxy ID="LoginSysXResourceManager" runat="server">
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/LoginPage.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="fesum">
    <asp:Label ID="lblMessage" runat="server" CssClass="reqd" Style="color: Red;"></asp:Label>
    <asp:ValidationSummary ID="valSumForgotPassword" DisplayMode="List" ValidationGroup="ValgrpForgetpass"
        runat="server" />
</div>
<div class="fgtfrm">
    <asp:Panel ID="pnlForgotPassword" DefaultButton="btnGenerate" runat="server">
        <ul>
            <li><span class="flb"><span class="cptn">
                <%=Resources.Language.RECOVER %>
                <%--Recover--%></span><span class="reqd" style="color: Red;">*</span></span>
                <div style="display: inline-block">
                    <asp:RadioButtonList ID="rdlRecoverOpt" runat="server" RepeatDirection="Horizontal"
                        CssClass="radio_list" RepeatLayout="Flow">
                       <%--Text="Username"--%> 
                         <asp:ListItem 
                            Text="<% $Resources:Language, USERNAME %>" Value="Username"
                            />
                            <%--Text="Password" --%>
                        <asp:ListItem Text="<% $Resources:Language, PASSWORD %>" Value="Password" />
                    </asp:RadioButtonList>
                </div>
                <asp:RequiredFieldValidator ValidationGroup="ValgrpForgetpass" ID="revOption" runat="server"
                    ControlToValidate="rdlRecoverOpt" Display="None" CssClass="LogVldxMsg">
                </asp:RequiredFieldValidator>
            </li>
            <li><span class="flb">
                <%--Text="Email"--%>
                <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail"
                    Text="<% $Resources:Language, EMAIL %>"
                      CssClass="cptn"></asp:Label><span
                    class="reqd" style="color: Red;">*</span> </span>
                <telerik:RadCaptcha ID="radCpatchaVerificationCode" CaptchaImage-TextChars="LettersAndNumbers"
                    CaptchaImage-TextLength="10" runat="server" Visible="false" Display="Dynamic">
                </telerik:RadCaptcha>
                <telerik:RadCaptcha ID="radCpatchaPassword" runat="server" CaptchaImage-TextChars="LettersAndNumbers"
                    Visible="false" CaptchaImage-TextLength="10" Display="Dynamic">
                </telerik:RadCaptcha>
                <infs:WclTextBox runat="server" ID="txtEmail" Width="180px" />
                <asp:RegularExpressionValidator runat="server" ID="revEmail" ValidationGroup="ValgrpForgetpass"
                    ControlToValidate="txtEmail" Display="None" ValidationExpression="\w+([-+.']*\w+)*@\w+([-.]*\w+)*\.\w+([-.]*\w+)*"
                    CssClass="LogVldxMsg" />
                <asp:RequiredFieldValidator ValidationGroup="ValgrpForgetpass" ID="rfvmail" runat="server"
                    ControlToValidate="txtEmail" Display="None" CssClass="LogVldxMsg">
                </asp:RequiredFieldValidator></li>
            <li><span class="flb">
                <%--Text="Verification code"--%>
                <asp:Label ID="lblSpncode" runat="server" AssociatedControlID="Spncode" 
                    Text="<% $Resources:Language, VERIFICATIONCODE %>"
                      CssClass="cptn"></asp:Label><span
                    id="Spncode" visible="false" class="reqd" style="color: Red;" runat="server">*</span></span>
                <infs:WclTextBox runat="server" ID="txtCode" Enabled="false" />
                <asp:RequiredFieldValidator ID="rfvCode" runat="server" Display="None" CssClass="LogVldxMsg"
                    ControlToValidate="txtCode" ValidationGroup="ValgrpForgetpasscode"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ID="revExpTxtCode" CssClass="LogVldxMsg"
                    ValidationGroup="ValgrpForgetpasscode" ControlToValidate="txtCode" Display="None"
                    ValidationExpression="^[^%^</^>]*$" /></li>
        </ul>
    </asp:Panel>
    <br />
    <%--Text="Generate code"--%>
    <infs:WclButton ID="btnGenerate" ValidationGroup="ValgrpForgetpass" runat="server"
        Text="<% $Resources:Language, GENRTCODE %>"
          OnClientClicked="btnGenerate_ClientClicked" OnClick="btnGenerate_Click">
    </infs:WclButton>
    <%-- Text="Submit"--%>
    <infs:WclButton ID="btnSave" runat="server"
        Text="<% $Resources:Language, SUBMIT %>"
         OnClientClicked="btnSave_ClientClicked"
        OnClick="btnSave_Click" Enabled="false" ValidationGroup="ValgrpForgetpasscode">
    </infs:WclButton>
     <%--Text="Cancel"--%>
    <infs:WclButton ID="btnCancel" CausesValidation="false" runat="server" 
        Text="<% $Resources:Language, CNCL %>"
       
        OnClick="btnCancel_Click">
    </infs:WclButton>
</div>
