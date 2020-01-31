<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditFeeItemDetail.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.EditFeeItemDetail" %>


<%@ Register TagPrefix="telerik" Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="Commandbar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div>
    <div class="content">
        <h1 class="mhdr">
            <asp:Label ID="lblEHAttr" Text="Update Fee Item"
                runat="server" /></h1>
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlReviewer">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Fee Item Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtFeeItemName" Text="" Enabled="false"
                            MaxLength="256">
                        </infs:WclTextBox>
                    </div>
                    <%--<div class='sxlb'>
                        <span class="cptn">Fee Item Label</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtFeeItemLabel" Text="" Enabled="false"
                            MaxLength="256">
                        </infs:WclTextBox>
                    </div>--%>
                    <div class='sxlb'>
                        <span class="cptn">Select Fee Item Type</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="ddlFeeItemType" runat="server" Enabled="false"
                            DataTextField="SIFT_Name" DataValueField="SIFT_ID" AutoPostBack="true" OnSelectedIndexChanged="ddlFeeItemType_SelectedIndexChanged">
                        </infs:WclComboBox>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ID="rfvCustomFormConfigAttrGroup" ControlToValidate="ddlFeeItemType" InitialValue="--Select--"
                                class="errmsg" ValidationGroup="grpFeeItem" Display="Dynamic" ErrorMessage="Fee Item Type is required." />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Description</span>
                    </div>
                    <div class='sxlm m2spn'>
                        <infs:WclTextBox runat="server" ID="txtDescription" Text="" Enabled="false"
                            MaxLength="1024">
                        </infs:WclTextBox>
                    </div>
                    <div id="divFixedTypeAmount" runat="server" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">Amount</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox runat="server" ID="txtFixedTypeAmount" Enabled="true" NumberFormat-DecimalDigits="2"
                                Type="Currency">
                            </infs:WclNumericTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvFixedTypeAmount" ControlToValidate="txtFixedTypeAmount"
                                    class="errmsg" ValidationGroup="grpFeeItem" Display="Dynamic" ErrorMessage="Fee Item Amount is required." />
                            </div>                            
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>
<div id="divButtonSave" runat="server">
    <div class="sxcbar">
        <div class="sxcmds" style="text-align: right">
            <infs:WclButton ID="btnEdit" runat="server" Text="Edit" OnClick="CmdBarEdit_Click">
                    <Icon PrimaryIconCssClass="rbEdit" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                        PrimaryIconWidth="14" />
                </infs:WclButton>
            <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="CmdBarSave_Click"
                ValidationGroup="grpFeeItem">
                <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                    PrimaryIconWidth="14" />
            </infs:WclButton>
            <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="CmdBarCancel_Click">
                <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                    PrimaryIconWidth="14" />
            </infs:WclButton>
        </div>
    </div>
</div>