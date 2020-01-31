<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminEntryAcctSettingPermissions.ascx.cs" Inherits="CoreWeb.ComplianceAdministration.UserControl.AdminEntryAcctSettingPermissions" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<script type="text/javascript">
</script>


<%--   <h1 class="mhdr">
        <asp:Label ID="lblNodeTitle" runat="server" Text="fdfdf"></asp:Label>
    </h1>--%>
<%--<div class="content">--%>
<%-- <div class="sbsection">
        <h1 class="sbhdr">
            <asp:Label ID="lblAdminEntryTitle" runat="server" Text="Vendor Account Settings"></asp:Label>
        </h1>
        <div class="sbcontent">--%>
<div class="sxform auto">
    <asp:Panel ID="pnlAdminEntryAcctSetting" CssClass="sxpnl" runat="server">
        <div class='sxro sx3co'>

            <div class='sxlb'>
                <asp:Label ID="lblApplicantInviteStatus" runat="server" AssociatedControlID="ddlApplicantInviteStatus" Text="Applicant Invite- Submit Status" CssClass="cptn">                                                        
                </asp:Label><span class='reqd'>*</span>
            </div>
            <div class='sxlm'>
                <infs:WclComboBox ID="ddlApplicantInviteStatus" runat="server" DataTextField="AISST_Name" CausesValidation="false" OnDataBound="ddlApplicantInviteStatus_DataBound"
                    DataValueField="AISST_ID">
                </infs:WclComboBox>
                <div class='vldx'>
                    <asp:RequiredFieldValidator runat="server" ID="rfvApplicantInviteStatus" ControlToValidate="ddlApplicantInviteStatus"
                        InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpVendorAcctSettings" ErrorMessage="Status is required." />
                </div>
            </div>

            <div class='sxlb'>
                <asp:Label ID="lblDays" runat="server" AssociatedControlID="ntxtDays" Text="Auto- Archive Timeline" CssClass="cptn">                                                        
                </asp:Label><%--<span class='reqd'>*</span>--%>
            </div>
            <div class='sxlm'>
                <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="ntxtDays"
                    MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                    MinValue="0">
                    <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                </infs:WclNumericTextBox>
                <%--  <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvDays" ControlToValidate="ntxtDays"
                                    class="errmsg" Display="Dynamic" ValidationGroup='grpVendorAcctSettings' ErrorMessage="Minimum Character is required." />
                            </div>--%>
            </div>

            <div class='sxlb'>
                <span class="cptn">Add Applicant Invite- Active</span>
            </div>
            <div class='sxlm'>
                <asp:RadioButtonList ID="rbtnAppInviteActive" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                </asp:RadioButtonList>
            </div>

            <div class='sxroend'>
            </div>
        </div>

        <div class='sxro sx3co'>
            <div class='sxlb'>
                <span class="cptn">Add New Order- Active</span>
            </div>
            <div class='sxlm'>
                <asp:RadioButtonList ID="rbtnOrderActive" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                </asp:RadioButtonList>
            </div>

            <div class='sxlb'>
                <span class="cptn">On Hold Status</span>
            </div>
            <div class='sxlm'>
                <asp:RadioButtonList ID="rbtnHoldStatus" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
    </asp:Panel>
</div>
<%--  </div>
    </div>--%>
<infsu:CommandBar ID="fsucCmdBarVendorAcctSettings" runat="server" DefaultPanel="pnlAdminEntryAcctSetting"
    DisplayButtons="Save" AutoPostbackButtons="Save" ValidationGroup="grpVendorAcctSettings"
    ButtonPosition="Right" OnSaveClick="fsucCmdBarVendorAcctSettings_SaveClick">
</infsu:CommandBar>
<%--</div>--%>
