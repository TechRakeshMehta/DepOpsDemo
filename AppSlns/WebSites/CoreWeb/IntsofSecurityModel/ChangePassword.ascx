<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.ChangePassword" CodeBehind="ChangePassword.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<infs:WclResourceManagerProxy ID="LoginSysXResourceManager" runat="server">
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/LoginPage.js"
        ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/shared/changepassword.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/shared/changepwd.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="note-box">
    <asp:Label CssClass="info" EnableViewState="false" runat="server" ID="lblError"><span
        class="highlight"><%=Resources.Language.NOTE %></span>
        <asp:Label ID="lblChangedPwdTopMsgPart1" runat="server" Text=""></asp:Label></asp:Label>
    <%=Resources.Language.ITEMSWITH %> <span class="reqd">*</span> <%=Resources.Language.TXTREQ %>
</div>
<div class="fesum">
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <asp:ValidationSummary ID="valSumChangePassword" DisplayMode="List" ValidationGroup="grpValdChngPassword"
        runat="server" />
</div>
<div class="fgtfrm">
    <asp:Panel ID="pnlChangePassword" DefaultButton="btnUpdate" runat="server">
        <ul class="no-list-style no-box">
            <li id="ilOldPswd" runat="server" visible="false">
                <span class="flb">
                    <asp:Label ID="lblOldPassword" runat="server" AssociatedControlID="txtOldPassword"
                        Text="<%$Resources:Language,OLDPASWRD %>" CssClass="cptn"></asp:Label><span class="reqd">*</span></span>
                <infs:WclTextBox runat="server" ID="txtOldPassword" Width="180px" TextMode="Password">
                </infs:WclTextBox>
                <div class="valdx">
                    <asp:RequiredFieldValidator runat="server" ID="rfvOldPassword" CssClass="LogVldxMsg"
                        ControlToValidate="txtOldPassword" Display="None" ValidationGroup="grpValdChngPassword" />
                    <%--<asp:RegularExpressionValidator runat="server" ID="revOldPassword" CssClass="LogVldxMsg"
                        ValidationGroup="grpValdChngPassword" ControlToValidate="txtOldPassword" Display="None"
                        ValidationExpression="^[^%^</^>]*$" />--%>
                </div>

            </li>
            <li><span class="flb">
                <asp:Label ID="lblNewPassword" runat="server" AssociatedControlID="txtNewPassword"
                    Text="<%$Resources:Language,NEWPASWRD %>" CssClass="cptn"></asp:Label><span class="reqd">*</span> </span>
                <infs:WclTextBox runat="server" ID="txtNewPassword" Width="180px" TextMode="Password"
                    MaxLength="15" autocomplete="off" ClientEvents-OnLoad="txtpwd_load">
                </infs:WclTextBox>
                <div class="valdx">
                    <asp:RequiredFieldValidator runat="server" ID="rfvNewPassword" CssClass="LogVldxMsg"
                        ControlToValidate="txtNewPassword" Display="None" ValidationGroup="grpValdChngPassword" />
                    <asp:CompareValidator ID="cmpvalNewWithOldPwd" runat="server" CssClass="LogVldxMsg"
                        ControlToCompare="txtOldPassword" Type="String" Operator="NotEqual" ControlToValidate="txtNewPassword"
                        Display="None" ValidationGroup="grpValdChngPassword"></asp:CompareValidator>
                    <asp:RegularExpressionValidator runat="server" ID="revNewPassword" CssClass="LogVldxMsg"
                        ControlToValidate="txtNewPassword" Display="None" ValidationExpression="(?=^.{8,15}$)^(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^_+~!?\\\/\'\:\,\(\)\{\}\[\]\-])[a-zA-Z0-9@#$%^_+~!?\\\/\'\:\,\(\)\{\}\[\]\-]{8,}$"
                        ValidationGroup="grpValdChngPassword" />
                </div>
                <%-- "(?=^.{8,15}$)(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*_+~])(?!.*\s)(^[^%^</^>^\s]).*$"--%>
            </li>
            <li><span class="flb">
                <asp:Label ID="lblCmpPassword" runat="server" AssociatedControlID="txtCmpPassword"
                    Text="<%$Resources:Language,CNFMPASSWORD %>" CssClass="cptn"></asp:Label><span class="reqd">*</span> </span>
                <infs:WclTextBox runat="server" ID="txtCmpPassword" Width="180px" TextMode="Password">
                </infs:WclTextBox>
                <div class="valdx">
                    <asp:RequiredFieldValidator runat="server" ID="rfvCmpPassword" CssClass="LogVldxMsg"
                        ControlToValidate="txtCmpPassword" Display="None" ValidationGroup="grpValdChngPassword" />
                    <asp:CompareValidator ID="cmpvalCmpPassword" runat="server" CssClass="LogVldxMsg"
                        ControlToCompare="txtNewPassword" ControlToValidate="txtCmpPassword" Display="None"
                        ValidationGroup="grpValdChngPassword"></asp:CompareValidator>
                </div>
            </li>
            <li><span class="flb">
                <asp:Label ID="lblSxBlocks" runat="server" Text="User Type" AssociatedControlID="cmbSxBlocks" CssClass="cptn"></asp:Label></span>
                <infs:WclComboBox ID="cmbSxBlocks" runat="server" DataValueField="SysXBlockId" DataTextField="Name"
                    Width="180px" MarkFirstMatch="true" Style="z-index: 7002;" />
            </li>
        </ul>
    </asp:Panel>
    <div class="cmds">
        <infs:WclButton ID="btnUpdate" runat="server" Text="<%$Resources:Language,SAVE %>" OnClientClicked="btnUpdate_ClientClicked"
            OnClick="btnUpdate_Click" ValidationGroup="grpValdChngPassword">
        </infs:WclButton>
        <infs:WclButton ID="btnCancel" runat="server" Text="<%$Resources:Language,CNCL %>" OnClick="btnCancel_Click">
        </infs:WclButton>
    </div>
</div>
<infs:WclToolTip runat="server" ID="tltpCatExplanation" TargetControlID="txtNewPassword"
    Width="380px" ShowEvent="OnFocus" ManualClose="true" RelativeTo="Element" Position="MiddleRight"
    ClientKey="pwdTip" Skin="Windows7" AutoSkinMode="false" EnableShadow="true">
    <div class="pwd_hint">
        <span style="font-weight: bold"><%=Resources.Language.PASSWORDCRITERIA %></span>
        <ul>
            <%--<li class="white yes">Should not have blank spaces</li>--%>
            <li class="white yes"><%=Resources.Language.NOBLANKSPACE %></li>
            <%--<li class="digit no">Should have at least one digit [0-9]</li>--%>
            <li class="digit no"><%=Resources.Language.ATLEASTONEDIGIT %></li>
            <%--<li class="char no">Should have at least one capital letter [A-Z]</li>--%>
            <li class="char no"><%=Resources.Language.ATLEASTONECAPLETTER %></li>
            <%--<li class="sym no">Should have at least one special character [@#$%^_+~!?\':/,(){}[]-]</li>--%>
            <li id="specialchar" runat="server" class="sym no"><%=Resources.Language.ATLEASTONESPECIALCHAR %></li>
            <li id="specialcharcbi" runat="server" class="sym no"><%=Resources.Language.ATLEASTONESPECIALCHARCBI %></li>
            <%--<li class="len no">Should have 8 to 15 characters.</li>--%>
            <li class="len no"><%=Resources.Language.PSWDCHARRANGE %></li>
        </ul>
    </div>
</infs:WclToolTip>
