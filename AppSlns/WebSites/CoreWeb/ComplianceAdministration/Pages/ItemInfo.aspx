<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.ItemInfo"
    Title="ItemInfo" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="ItemInfo.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>
<%@ Register TagPrefix="uc2" TagName="CategoriesItemsNodes" Src="~/Shared/Controls/CategoriesItemsNodes.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <style type="text/css">
        .reEditorModes a {
            display: none;
        }

        .reToolZone {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
            //UAT-3077
            HideShowPanel(null, true);
        });

        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }

        function RefrshTreeOnDissociate(data) {
            if (data != undefined && data != '') {
                var hdnStoredData = $jQuery('[id$=hdnStoredData]', $jQuery(parent.theForm))
                if (hdnStoredData != undefined) {
                    hdnStoredData.val(data);
                }
            }
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }

        function HideShowPanel(args, isPageReady) {
            var divAmount = $jQuery('[id$=divAmount]');
            var txtAmount = $jQuery('[id$=txtAmount]');
            var rfvAmount = $jQuery("[id$=rfvAmount]");
            var isChecked;

            if (isPageReady != undefined && isPageReady == true) {
                var chkPaymentType = $jQuery('[id$=chkPaymentType]');
                isChecked = chkPaymentType[0].checked;
            }
            else {
                isChecked = args.checked;
            }

            if (isChecked) {
                divAmount[0].style.display = "block";
                ValidatorEnable(rfvAmount[0], true);
                $jQuery(rfvAmount[0]).hide();
            }
            else {
                txtAmount.val("");
                divAmount[0].style.display = "none";
                ValidatorEnable(rfvAmount[0], false);
            }
        }

    </script>
    <div class="page_cmd">
        &nbsp;
    </div>
    <div class="section">
        <h1 class="mhdr">Item Information</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlItem">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Item Name</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtName" MaxLength="100">
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvItemName" ControlToValidate="txtName"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Item Name is required." />
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Item Label</span><%--<span class="reqd">*</span>--%>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtLabel" MaxLength="100">
                            </infs:WclTextBox>
                            <%--   <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvLabel" ControlToValidate="txtLabel"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Item Label is required." /></div>--%>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Screen Label</span><%--<span class="reqd">*</span>--%>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtScreenLabel" MaxLength="100">
                            </infs:WclTextBox>
                            <%--  <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvScreenLabel" ControlToValidate="txtScreenLabel"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Screen Label is required." /></div>--%>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Is Active</span>
                        </div>
                        <div class='sxlm'>
                            <%-- <infs:WclButton runat="server" ID="chkActive" ToggleType="CheckBox" ButtonType="ToggleButton"
                                AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>--%>
                            <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" IsAutoPostBack="false" />
                        </div>
                        <div id="divEffectiveDate" runat="server">
                            <div class='sxlb'>
                                <span class="cptn">Effective Date</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclDatePicker ID="dpkrEffectiveDate" runat="server" DateInput-EmptyMessage="Select a date">
                                    <%--<Calendar>
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                    </SpecialDays>
                                </Calendar>--%>
                                </infs:WclDatePicker>
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Display Order</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox ShowSpinButtons="false" Type="Number" ID="txtDisplayOrder"
                                MaxValue="2147483647" runat="server" MinValue="0" InvalidStyleDuration="100"
                                NumberFormat-DecimalDigits="0" MaxLength="4">
                            </infs:WclNumericTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Is a Payment Item</span>
                        </div>
                        <div class='sxlm'>
                            <asp:CheckBox ID="chkPaymentType" runat="server" Text="" onclick="HideShowPanel(this);" />
                        </div>
                        <div id="divAmount" runat="server" style="display: none;">
                            <div class='sxlb'>
                                <span class="cptn">Amount</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclNumericTextBox ID="txtAmount" Type="Currency" runat="server" NumberFormat-DecimalDigits="2" MinValue="0" MaxLength="9">
                                </infs:WclNumericTextBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvAmount" ControlToValidate="txtAmount"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Amount is required." />
                                </div>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div id="dvMappingHierarchy" runat="server" visible="false" class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Item Nodes</span>
                            </div>
                            <uc2:CategoriesItemsNodes runat="server" ID="ucCategoriesItemsNodes" />
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <%-- <div class='sxlb'>
                            <span class="cptn">Universal Item</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="cmbUniversalItem" runat="server" ToolTip="Select universal category to map"
                                DataTextField="UI_Name" DataValueField="UI_ID" OnSelectedIndexChanged="cmbUniversalItem_SelectedIndexChanged"
                                AutoPostBack="true">
                            </infs:WclComboBox>
                        </div>--%>
                        <telerik:RadButton ID="btnCopyItemData" ToolTip="If there are any active rotation subscription then data will be copied from tracking package to rotation package." runat="server" Text="Copy Item Data to Rotation" Visible="true" OnClick="btnCopyItemData_ExtraClick">
                        </telerik:RadButton>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Description</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclEditor ID="rdEditorDescription" ClientIDMode="Static" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" Width="99.3%" EnableResize="false"
                                Height="150px">
                            </infs:WclEditor>
                        </div>
                        <div class='vldx'>
                            <asp:CustomValidator runat="server" ID="cstValEditorDescription" ControlToValidate="rdEditorDescription" ClientValidationFunction="ValidateLength"
                                class="errmsg" Display="Dynamic" ErrorMessage="Please don't enter more than 500 characters." />
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Details</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclEditor ID="rdEditorDetails" ClientIDMode="Static" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" Width="99.3%" EnableResize="false"
                                Height="150px">
                            </infs:WclEditor>
                        </div>
                        <div class='vldx'>
                            <asp:CustomValidator runat="server" ID="cstValEditorDetails" ControlToValidate="rdEditorDetails" ClientValidationFunction="ValidateDetailsLength"
                                class="errmsg" Display="Dynamic" ErrorMessage="Please don't enter more than 500 characters." />
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Explanatory Notes</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclEditor ID="rdEditorEcplanatoryNotes" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" Width="99.3%" EnableResize="false"
                                Height="150px">
                            </infs:WclEditor>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <%-- <div class='sxro sx3co monly'>
                        <div class='sxlb'>
                            <span class="cptn">Explanatory Notes</span>
                        </div>
                    </div>
                    <div class='sxro sx1co'>
                        <infs:WclEditor ID="rdEditorEcplanatoryNotes" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" Width="99.3%" EnableResize="false"
                            Height="100px">
                        </infs:WclEditor>
                        <div class='sxroend'>
                        </div>
                    </div>--%>
                </asp:Panel>
            </div>
            <div class="sxpnl">
                <div style="float: right;">
                    <infsu:CommandBar ID="fsucCmdBarItem" runat="server" DefaultPanel="pnlItem" SubmitButtonIconClass="rbEdit"
                        OnSaveClick="fsucCmdBarItem_SaveClick" SubmitButtonText="Edit" AutoPostbackButtons="Submit, Save, Cancel"
                        OnSubmitClick="fsucCmdBarItem_SubmitClick" OnCancelClick="fsucCmdBarItem_CancelClick"
                        SaveButtonText="Save">
                    </infsu:CommandBar>
                </div>
                <div id="dvDisassociate" runat="server" style="padding-top: 10px;">
                    <div class='sxro sx2co' style="clear: none">
                        <div class='sxlb' style="border-color: transparent">
                            <span class="cptn">Dissociate With</span>
                        </div>
                        <div class='sxlm' style="border-color: transparent">
                            <infs:WclComboBox ID="cmbAssociatedCategories" runat="server" ToolTip="Select Category to Dissociate the Item"
                                DataTextField="CategoryName" DataValueField="ComplianceCategoryID" CheckBoxes="true" Width="70%" EmptyMessage="--Select--"
                                AutoPostBack="false">
                            </infs:WclComboBox>
                            <telerik:RadButton ID="btnDissociateItem" ToolTip="Click here to dissociate Item" runat="server" Text="Dissociate" Visible="false" OnClick="btnDissociateItem_Click">
                            </telerik:RadButton>
                        </div>
                    </div>
                </div>
            </div>

            <%--<infsu:CommandBar ID="fsucCmdBarItem" runat="server" DefaultPanel="pnlItem" SubmitButtonIconClass="rbEdit"
                OnSaveClick="fsucCmdBarItem_SaveClick" SubmitButtonText="Edit" AutoPostbackButtons="Submit, Save, Cancel"
                OnSubmitClick="fsucCmdBarItem_SubmitClick" OnCancelClick="fsucCmdBarItem_CancelClick"
                SaveButtonText="Save">
            </infsu:CommandBar>--%>
            <%--<infsu:CommandBar ID="fsucCmdBarItem" runat="server" DefaultPanel="pnlItem" DisplayButtons="Submit, Save, Cancel"
                OnCancelClick="fsucCmdBarItem_CancelClick" OnSubmitClick="fsucCmdBarItem_SubmitClick"
                SaveButtonText="Update" SubmitButtonText="Edit" SubmitButtonIconClass="rbEdit"
                AutoPostbackButtons="Submit, Save, Cancel" OnSaveClick="fsucCmdBarItem_SaveClick">
            </infsu:CommandBar>--%>
        </div>
    </div>
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblTitle" runat="server" Text="Attributes"></asp:Label>
        </h1>
        <div class="content">
            <div class="swrap">
                <infs:WclGrid runat="server" ID="grdAttributes" AllowPaging="True" AutoGenerateColumns="False"
                    AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                    EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false"
                    GridLines="None" OnNeedDataSource="grdAttributes_NeedDataSource" OnDeleteCommand="grdAttributes_DeleteCommand">
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="CIA_ID">
                        <CommandItemSettings ShowAddNewRecordButton="false" AddNewRecordText="Add new Category" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="ComplianceAttribute.Name" FilterControlAltText="Filter Name column"
                                HeaderText="Attribute Name" SortExpression="ComplianceAttribute.Name" UniqueName="Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ComplianceAttribute.AttributeLabel" FilterControlAltText="Filter AttributeLabel column"
                                HeaderText="Attribute Label" SortExpression="ComplianceAttribute.AttributeLabel"
                                UniqueName="AttributeLabel">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ComplianceAttribute.ScreenLabel" FilterControlAltText="Filter ScreenLabel column"
                                HeaderText="Screen Label" SortExpression="ComplianceAttribute.ScreenLabel" UniqueName="ScreenLabel">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ComplianceAttribute.lkpComplianceAttributeType.Name"
                                FilterControlAltText="Filter AttributeType column" HeaderText="Attribute Type"
                                SortExpression="ComplianceAttribute.lkpComplianceAttributeType.Name" UniqueName="AttributeType">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ComplianceAttribute.lkpComplianceAttributeDatatype.Name"
                                FilterControlAltText="Filter DataType column" HeaderText="Data Type" SortExpression="ComplianceAttribute.lkpComplianceAttributeDatatype.Name"
                                UniqueName="DataType">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ComplianceAttribute.Description" FilterControlAltText="Filter Description column"
                                HeaderText="Description" SortExpression="ComplianceAttribute.Description" UniqueName="Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ComplianceAttribute.TenantName" FilterControlAltText="Filter TenantName column"
                                HeaderText="Tenant" SortExpression="TenantName" UniqueName="TenantName">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="ComplianceAttribute.IsActive" FilterControlAltText="Filter IsActive column" DataType="System.Boolean"
                                HeaderText="Is Active" SortExpression="ComplianceAttribute.IsActive" UniqueName="IsActive">
                                <ItemTemplate>
                                    <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("ComplianceAttribute.IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                    <asp:HiddenField ID="hdnfComplianceAttributeID" runat="server" Value='<%#Eval("ComplianceAttribute.ComplianceAttributeID")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="CIA_DisplayOrder" FilterControlAltText="Filter Display Order column" DataType="System.Int32"
                                HeaderText="Display Order" SortExpression="CIA_DisplayOrder" UniqueName="CIA_DisplayOrder">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                                Text="Delete" UniqueName="DeleteColumn">
                                <HeaderStyle CssClass="tplcohdr" />
                                <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                            </telerik:GridButtonColumn>
                        </Columns>
                        <EditFormSettings EditFormType="Template">
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </infs:WclGrid>
            </div>
            <div class="gclr">
            </div>
        </div>
    </div>
</asp:Content>
