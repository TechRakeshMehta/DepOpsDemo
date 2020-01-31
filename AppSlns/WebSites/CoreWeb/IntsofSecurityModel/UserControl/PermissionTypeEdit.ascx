<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.PermissionTypeEdit" Codebehind="PermissionTypeEdit.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register Src="~/Shared/Controls/CommandBar.ascx" TagName="Commandbar" TagPrefix="infsu" %>
<%@ Import Namespace="INTSOF.Utils" %>
<%@ Import Namespace="Telerik.Web.UI" %>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Add Permission Type" : "Edit Permission Type"%>'
            runat="server"></asp:Label></h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox error">
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="errmsg"></asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlManagePermissionTypes">
                <div class='sxro sx2co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblTypeName" runat="server" AssociatedControlID="txtTypeName" Text="Permission Type Name" CssClass="cptn"></asp:Label><span
                            class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox Width="100%" Text='<%#Bind("Name")%>' MaxLength="216" TabIndex="1"
                            ID="txtTypeName" runat="server" />
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTypeName" ControlToValidate="txtTypeName"
                                Display="Dynamic" ValidationGroup="grpValdPermissionTypeEdit" CssClass="errmsg"
                                ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_PERMISSION_TYPE_REQUIRED)%>' />
                            <asp:RegularExpressionValidator runat="server" ID="revTypeName" ControlToValidate="txtTypeName"
                                Display="Dynamic" ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)]{3,216}$" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                ValidationGroup="grpValdPermissionTypeEdit" CssClass="errmsg"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx2co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblDescription" runat="server" AssociatedControlID="txtDescription" CssClass="cptn"
                            Text="Description"></asp:Label>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox Width="100%" Text='<%#Bind("Description") %>' TabIndex="2" ID="txtDescription"
                            runat="server" MaxLength="246" />
                        <div class="vldx">
                            <asp:RegularExpressionValidator runat="server" ID="revDescription" ControlToValidate="txtDescription"
                                Display="Dynamic" ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)]{3,255}$" ValidationGroup="grpValdPermissionTypeEdit"
                                CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(
    ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="fsucCmdBarValdPermissionTypeEdit" runat="server" TabIndexAt="3" GridInsertText="Save" GridUpdateText="Save"
            ValidationGroup="grpValdPermissionTypeEdit" GridMode="true" DefaultPanel="pnlManagePermissionTypes" />
    </div>
</div>
