<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ManageSeriesDose.aspx.cs"
    Inherits="CoreWeb.ComplianceAdministration.Views.ManageSeriesDose" MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript">
        //Validate Fucntion for Key Attribute
        //function ValidateKeyAttribute(sender, args) {
        //    ValidateComboBox("cmbKeyAttributes", args);
        //}
        //Validate Function for Items
        function ValidateItems(sender, args) {
            ValidateComboBox("cmbComplianceItems", args);
        }
        //Validate function for Attributes
        function ValidateAttribute(sender, args) {
            ValidateComboBox("cmbAttributes", args);
        }
        //Validate the ComboBox
        function ValidateComboBox(comboBoxId, args) {
            var checkedItems = $jQuery("[id$=" + comboBoxId + "]")[0].control.get_checkedItems();
            if (checkedItems.length > 0) {
                args.IsValid = true;
                return false;
            }
            args.IsValid = false;
        }
    </script>
    <asp:Panel ID="pnlDose" runat="server">
        <div class="section">
            <h1 class="mhdr">Manage Item</h1>
            <div class="content">
                <div class="sxform auto">
                    <div class="msgbox">
                        <asp:Label ID="lblMessage" runat="server" CssClass="info"></asp:Label>
                    </div>
                    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlItem">
                        <div class='sxro sx2co'>
                            <div class='sxlb'>
                                <span class="cptn">Select Items</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbComplianceItems" AutoPostBack="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput"
                                    OnSelectedIndexChanged="cmbComplianceItems_SelectedIndexChanged" DataTextField="Name" DataValueField="ComplianceItemID"
                                    runat="server" EmptyMessage="--SELECT--" CheckBoxes="true">
                                </infs:WclComboBox>
                                <div class="vldx">
                                    <asp:CustomValidator ClientValidationFunction="ValidateItems" ID="rfvitems" runat="server"
                                        ErrorMessage="Item is required." CssClass="errmsg" EnableClientScript="true" ValidationGroup="grpSave"
                                        Display="Dynamic"></asp:CustomValidator>
                                    <%--<asp:RequiredFieldValidator runat="server" ID="rfvitems" ControlToValidate="cmbComplianceItems"
                                        Display="Dynamic" CssClass="errmsg" Text="Item is required." ValidationGroup="grpSave" />--%>
                                </div>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Select Attributes</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbAttributes" runat="server" OnSelectedIndexChanged="cmbAttributes_SelectedIndexChanged"
                                    AutoPostBack="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="--SELECT--"
                                    DataValueField="ComplianceAttributeID" DataTextField="Name" CheckedItemsTexts="DisplayAllInInput">
                                </infs:WclComboBox>

                                <div class='vldx'>
                                    <asp:CustomValidator ClientValidationFunction="ValidateAttribute" ID="rfvAttribute" runat="server"
                                        ErrorMessage="Attribute is required." CssClass="errmsg" EnableClientScript="true" ValidationGroup="grpSave"
                                        Display="Dynamic"></asp:CustomValidator>
                                    <%--<asp:RequiredFieldValidator runat="server" ID="rfvAttributes" ControlToValidate="cmbAttributes" ValidationGroup="grpSave"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Attribute is required." />--%>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx2co'>
                            <div class='sxlb'>
                                <span class="cptn">Key Attribute</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbKeyAttributes" runat="server" DataTextField="Text" DataValueField="Value"
                                    EmptyMessage="--SELECT--" OnSelectedIndexChanged="cmbKeyAttributes_SelectedIndexChanged" AutoPostBack="true">
                                </infs:WclComboBox>
                                <div class='vldx'>
                                    <%--<asp:CustomValidator ClientValidationFunction="ValidateKeyAttribute" ID="rfvKeyAttributes" runat="server"
                                        ErrorMessage="Key Attribute is required." CssClass="errmsg" EnableClientScript="true" ValidationGroup="grpSave"
                                        Display="Dynamic"></asp:CustomValidator>--%>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvKeyAttributes" ControlToValidate="cmbKeyAttributes" ValidationGroup="grpSave"
                                        InitialValue="--SELECT--" class="errmsg" Display="Dynamic" ErrorMessage="Key Attribute is required." />
                                </div>
                            </div>
                            <%-- <div class='sxlb'>
                                <span class="cptn">Allow entry after all items are approved</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rbtnAlloEntry" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Selected="True" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>--%>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>

        </div>
        <infsu:CommandBar ID="fsucCmdBar" runat="server" DefaultPanel="pnlDose"
            DisplayButtons="Save" OnSaveClick="fsucCmdBar_SaveClick" SaveButtonText="Initialize Mapping"
            AutoPostbackButtons="Save" ValidationGroup="grpSave">
        </infsu:CommandBar>

    </asp:Panel>

    <asp:Panel ID="pnl" runat="server">
    </asp:Panel> 

</asp:Content>
