<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.PolicyRegisterControlMappings" Codebehind="PolicyRegisterControlMappings.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Import Namespace="INTSOF.Utils" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxPolicyRegisterControlMappings">
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/PolicyRegisterControlMappings.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<input type="hidden" id="hdnCurrentNode" name="hdnSelectedNode" />
<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblPolicyRegisterControlMapping" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <infs:WclTreeList runat="server" ID="treeListControlPolicy" DataKeyNames="RegisterUserControlID"
            AllowPaging="true" PageSize="10" ParentDataKeyNames="PolicyRegisterUserControl2.RegisterUserControlID"
            AutoGenerateColumns="false" OnNeedDataSource="treeListControlPolicy_NeedDataSource"
            OnDeleteCommand="treeListControlPolicy_DeleteCommand" OnInsertCommand="treeListControlPolicy_InsertCommand"
            OnUpdateCommand="treeListControlPolicy_UpdateCommand" OnItemCreated="treeListControlPolicy_ItemCreated" PagerStyle-Position="Top">
            <Columns>
                <telerik:TreeListEditCommandColumn UniqueName="EditCommandColumn" EditText="Edit"
                    AddRecordText="Add Policy Register Control" ButtonType="ImageButton">
                    <ItemStyle CssClass="MyImageButton" />
                </telerik:TreeListEditCommandColumn>
                <telerik:TreeListBoundColumn DataField="RegisterUserControlID" UniqueName="RegisterUserControlID"
                    Visible="false" HeaderText="RegisterUserControlID" ReadOnly="true" />
                <telerik:TreeListBoundColumn DataField="ControlName" UniqueName="ControlName" HeaderText="Control Name" />
                <telerik:TreeListBoundColumn DataField="DisplayName" UniqueName="DisplayName" HeaderText="Display Name" />
               <%-- <telerik:TreeListBoundColumn DataField="ControlPath" UniqueName="ControlPath" HeaderText="Control Path" />--%>
            </Columns>
            <EditFormSettings EditFormType="Template">
                <FormTemplate>
                    <div class="section">
                        <h1 class="mhdr">
                            <asp:Label ID="lblHeading" Text='<%# (Container is TreeListEditFormInsertItem)
                                 ? "Add Policy Register Control"
                                 : "Edit Policy Register Control"%>' runat="server"></asp:Label></h1>
                        <div class="content">
                            <div class="sxform auto">
                                <div class="msgbox">
                                    <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                </div>
                                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPolicyRegisterControlMapping">
                                    <div class='sxro sx2co'>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="Control Name" CssClass="cptn"></asp:Label><span
                                                class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox Width="97%" Text='<%# Bind("ControlName") %>' ID="txtName" TabIndex="1"
                                                MaxLength="210" runat="server" />
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                                    Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_CONTROL_REQUIRED) %>' />
                                                <asp:RegularExpressionValidator runat="server" ID="revName" ControlToValidate="txtName"
                                                    Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS) %>'
                                                    ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)]{3,50}$"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblDisplayName" runat="server" AssociatedControlID="txtDisplayName" CssClass="cptn"
                                                Text="Display Name"></asp:Label><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox Width="97%" Text='<%# Bind("DisplayName") %>' TabIndex="2" MaxLength="210"
                                                ID="txtDisplayName" runat="server" />
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvDisplayName" ControlToValidate="txtDisplayName"
                                                    Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_DISPLAY_NAME_REQUIRED) %>' />
                                                <asp:RegularExpressionValidator runat="server" ID="revDisplayName" ControlToValidate="txtDisplayName"
                                                    Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS) %>'
                                                    ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)]{3,50}$"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx2co'>
                                        <asp:Label ID="lblUIControlID" runat="server" Style="display: none;" />
                                        <div class='sxlb'>
                                            <asp:Label ID="lblControlPath" runat="server" AssociatedControlID="txtControlPath" CssClass="cptn"
                                                Text="Control Path"></asp:Label><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm m2spn'>
                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td style="padding-right: 2px;">
                                                        <infs:WclTextBox Width="99%" Text='<%# Bind("ControlPath") %>' TabIndex="3" MaxLength="255"
                                                            ID="txtControlPath" runat="server" />
                                                    </td>
                                                    <td width="29px">
                                                        <infs:WclButton ID="btnUIControlID" Text="..." runat="server" TabIndex="4" AutoPostBack="false"
                                                            Style="vertical-align: top;" CausesValidation="False">
                                                        </infs:WclButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblUiCtrError" ForeColor="Red" CssClass="errmsg" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <infsu:CommandBar ID="fsucCmdBarPolicyRegisterControlMapping" runat="server" TabIndexAt="5"
                                TreeListMode="true" DefaultPanel="pnlPolicyRegisterControlMapping" OnSaveClientClick="OnClientClicked" />
                        </div>
                    </div>
                </FormTemplate>
            </EditFormSettings>
            <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric"></PagerStyle>
        </infs:WclTreeList>
    </div>
</div>
