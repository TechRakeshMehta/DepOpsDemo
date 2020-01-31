<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetUpAttributeForAttributeGroup.aspx.cs" Inherits="CoreWeb.BkgSetup.Views.SetUpAttributeForAttributeGroup"
    Title="Attribute " MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/Shared/Controls/IsActiveToggle.ascx" TagName="IsActiveToggle"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/mapping_pages.css" ResourceType="StyleSheet" />
    </infs:WclResourceManagerProxy>
    <script type="text/javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
            parent.Page.hideProgress();
        });

        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }

        //13/02/2014 Changes done for - "Package listing screen : Show splash screen on save"
        function SaveClick(sender, args) {
            if (Page_Validators != undefined && Page_Validators != null) {
                var i;
                for (i = 0; i < Page_Validators.length; i++) {
                    var val = Page_Validators[i];
                    if (!val.isvalid) {
                        return
                    }
                }
            }

            Page.showProgress("Processing...");
            args.set_cancel(false);
        }

        var minDate = new Date("01/01/1980");
        function CorrectMinMaxDate(picker) {
            var date1 = $jQuery("[id$=dpkrMinDateValue]")[0].control.get_selectedDate();
            var date2 = $jQuery("[id$=dpkrMaxDateValue]")[0].control.get_selectedDate();
            if (date1 != null && date2 != null) {
                if (date1 > date2)
                    $jQuery("[id$=dpkrMaxDateValue]")[0].control.set_selectedDate(null);
            }
        }

        function SetMinDate(picker) {
            var date = $jQuery("[id$=dpkrMinDateValue]")[0].control.get_selectedDate();
            if (date != null) {
                picker.set_minDate(date);
            }
            else {
                picker.set_minDate(minDate);
            }
        }
    </script>
    <div class="dummyBtn" style="display: none;">
        <infs:WclButton runat="server" ID="WclButton1" Text=""
            ButtonType="LinkButton" Height="30px">
        </infs:WclButton>
    </div>
    <div class="page_cmd">
        <infs:WclButton runat="server" ID="btnEdit" Text="+ Add Attribute" OnClick="CmdBarAddAttribute_Click"
            ButtonType="LinkButton" Height="30px">
        </infs:WclButton>
    </div>
    <div class="content">
        <div id="divAddAttribute" runat="server" visible="false">
            <h1 class="mhdr">
                <span>Select Attribute </span>
            </h1>
            <div class="sxform auto">
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAttribute">
                    <div class="sxgrp" id="divAddExistingAttribute" runat="server">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Select Attribute</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbChkAttribute" AutoPostBack="true"
                                    runat="server" DataTextField="AttributeName" DataValueField="AttributeId" OnSelectedIndexChanged="cmbChkAttribute_OnSelectedIndexChanged">
                                </infs:WclComboBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                            <asp:HiddenField ID="hdnIsExistingAttributeAdd" runat="server" Value="False" />
                        </div>
                    </div>
                    <div class="sxgrp" id="divAddNewAttribute" runat="server">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Attribute Name</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtAttributeName" MaxLength="256">
                                </infs:WclTextBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvAttributeName" ControlToValidate="txtAttributeName"
                                        class="errmsg" ValidationGroup="grpAttribute" Display="Dynamic" ErrorMessage="Attribute Name is required." />
                                </div>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Attribute Label</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtAttributeLabel" MaxLength="256">
                                </infs:WclTextBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvAttributeLabel" ControlToValidate="txtAttributeLabel"
                                        class="errmsg" ValidationGroup="grpAttribute" Display="Dynamic" ErrorMessage="Attribute Label is required." />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>

                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Is Active</span>
                            </div>
                            <div class='sxlm'>
                                <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" Checked="true" IsAutoPostBack="false" />
                                <%--<infs:WclButton runat="server" ID="chkActive" ToggleType="CheckBox" ButtonType="ToggleButton"
                                    AutoPostBack="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                    </ToggleStates>
                                </infs:WclButton>--%>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Attribute Description</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclTextBox runat="server" ID="txtAttributeDescription" MaxLength="1024">
                                </infs:WclTextBox>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Data Type</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbDataType" runat="server" AutoPostBack="true" DataTextField="SADT_Name"
                                    DataValueField="SADT_ID" OnSelectedIndexChanged="cmbDataType_SelectedIndexChanged">
                                </infs:WclComboBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvDataType" ControlToValidate="cmbDataType"
                                        InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpAttribute" CssClass="errmsg"
                                        Text="Data Type is required." />
                                </div>
                            </div>
                            <div id="divOption" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Options</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox runat="server" ID="txtOptOptions" EmptyMessage="E.g. Positive=1,Negative=2">
                                    </infs:WclTextBox>
                                    <div class='vldx'>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvOptions" ControlToValidate="txtOptOptions"
                                            class="errmsg" Display="Dynamic" ErrorMessage="Options is required." ValidationGroup='grpAttribute' />
                                    </div>
                                </div>
                            </div>
                            <div id="divCharacters" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Minimum Length</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="nTxtMinLength"
                                        MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                        MinValue="1">
                                        <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                    </infs:WclNumericTextBox>
                                    <div class='vldx'>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvMinimumLength" ControlToValidate="nTxtMinLength"
                                            class="errmsg" Display="Dynamic" ErrorMessage="Minimum Length is required."
                                            ValidationGroup='grpAttribute' />
                                    </div>
                                </div>
                                <div class='sxlb'>
                                    <span class="cptn">Maximum Length</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="ntxtMaxChars"
                                        MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                        MinValue="1">
                                        <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                    </infs:WclNumericTextBox>
                                    <div class='vldx'>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvMaximumLength" ControlToValidate="ntxtMaxChars"
                                            class="errmsg" Display="Dynamic" ErrorMessage="Maximum Length is required."
                                            ValidationGroup='grpAttribute' />
                                    </div>
                                </div>
                            </div>
                            <div id="divDateType" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Minimum Date Value</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclDatePicker ID="dpkrMinDateValue" runat="server" DateInput-EmptyMessage="Select a date">
                                        <ClientEvents OnDateSelected="CorrectMinMaxDate" />
                                    </infs:WclDatePicker>
                                    <div class='vldx'>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvMaxDate" ControlToValidate="dpkrMinDateValue"
                                            class="errmsg" Display="Dynamic" ErrorMessage="Minimum Date is required."
                                            ValidationGroup='grpAttribute' />
                                    </div>
                                </div>
                                <div class='sxlb'>
                                    <span class="cptn">Maximum Date Value</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclDatePicker ID="dpkrMaxDateValue" runat="server" DateInput-EmptyMessage="Select a date">
                                        <ClientEvents OnPopupOpening="SetMinDate" />
                                    </infs:WclDatePicker>
                                    <div class='vldx'>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvMinDate" ControlToValidate="dpkrMaxDateValue"
                                            class="errmsg" Display="Dynamic" ErrorMessage="Maximum Date is required."
                                            ValidationGroup='grpAttribute'>

                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div id="divIntegerType" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Minimum Integer Value</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="nTxtMinimunIntegerValue"
                                        MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                        MinValue="1">
                                        <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                    </infs:WclNumericTextBox>
                                    <div class='vldx'>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvMinimumIntegerValue" ControlToValidate="nTxtMinimunIntegerValue"
                                            class="errmsg" Display="Dynamic" ErrorMessage="Minimum Integer Value is required."
                                            ValidationGroup='grpAttribute' />
                                    </div>
                                </div>
                                <div class='sxlb'>
                                    <span class="cptn">Maximum Integer Value</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="nTxtMaxIntegerValue"
                                        MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                        MinValue="1">
                                        <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                    </infs:WclNumericTextBox>
                                    <div class='vldx'>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvMaxIntegerValue" ControlToValidate="nTxtMaxIntegerValue"
                                            class="errmsg" Display="Dynamic" ErrorMessage="Maximum Integer Value is required."
                                            ValidationGroup='grpAttribute' />
                                    </div>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Is Required</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclButton runat="server" ID="chkRequired" ToggleType="CheckBox" ButtonType="ToggleButton"
                                AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Is Display</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclButton runat="server" ID="chkIsDisplay" ToggleType="CheckBox" ButtonType="ToggleButton"
                                AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>
                        </div>
                         <div class='sxlb'>
                            <span class="cptn">Is Hidden From UI</span>
                        </div>
                           <div class='sxlm'>
                            <infs:WclButton runat="server" ID="chkIsHiddenFromUI" ToggleType="CheckBox" ButtonType="ToggleButton"
                                AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <div id="divSaveButton" runat="server" visible="false">
        <div class="sxcbar">
            <div class="sxcmds" style="text-align: right">
                <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="CmdBarSave_Click"
                    ValidationGroup="grpAttribute">
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
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblAttributeGroupGridTitle" runat="server" Text="Attributes"></asp:Label>
        </h1>
        <div class="content">
            <div class="swrap">
                <infs:WclGrid runat="server" ID="grdMappedAttribute" AllowPaging="false" AutoGenerateColumns="False"
                    AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                    EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="false" OnRowDrop="grdMappedAttribute_RowDrop"
                    GridLines="None" OnNeedDataSource="grdMappedAttribute_NeedDataSource" OnDeleteCommand="grdMappedAttribute_DeleteCommand"
                    EnableLinqExpressions="false">
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true" AllowRowsDragDrop="true" AllowAutoScrollOnDragDrop="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="AttributeID,AttributeGroupID,BkgPackageSvcAttributeMappingId,BkgAttributeGroupMappingId">
                        <CommandItemSettings ShowAddNewRecordButton="false" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="AttributeName" FilterControlAltText="Filter AttributeName column"
                                HeaderText="Name" SortExpression="AttributeName" UniqueName="Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AttributeLabel" FilterControlAltText="Filter AttributeLabel column"
                                HeaderText="Label" SortExpression="AttributeLabel"
                                UniqueName="Label">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AttributeDescription" FilterControlAltText="Filter AttributeDescription column"
                                HeaderText="Description" SortExpression="AttributeDescription"
                                UniqueName="Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AttributeDataType" FilterControlAltText="Filter AttributeDataType column"
                                HeaderText="Data Type" SortExpression="AttributeDataType"
                                UniqueName="DataType">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DisplayOrder" FilterControlAltText="Filter DisplayOrder column"
                                HeaderText="Display Order" SortExpression="DisplayOrder"
                                UniqueName="DisplayOrder">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="Required" FilterControlAltText="Filter Required column"
                                HeaderText="Is Required" SortExpression="Required" UniqueName="Required">
                                <ItemTemplate>
                                    <asp:Label ID="IsRequired" runat="server" Text='<%# Convert.ToBoolean(Eval("Required"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%-- <telerik:GridBoundColumn DataField="Required" FilterControlAltText="Filter Required column"
                                HeaderText="Is Required" SortExpression="Required"
                                UniqueName="Required">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridTemplateColumn DataField="IsDisplay" FilterControlAltText="Filter IsDisplay column"
                                HeaderText="Is Display" SortExpression="IsDisplay" UniqueName="IsDisplay">
                                <ItemTemplate>
                                    <asp:Label ID="IsDisplay" runat="server" Text='<%# Convert.ToBoolean(Eval("IsDisplay"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>

                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="IsHdnFrmUI" FilterControlAltText="Filter IsHdnFrmUI column"
                        HeaderText="Is Hidden From UI" SortExpression="IsHdnFrmUI" UniqueName="IsHdnFrmUI">
                        <ItemTemplate>
                            <asp:Label ID="IsHdnFrmUI" runat="server" Text='<%# Convert.ToBoolean(Eval("IsHiddenFromUI"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                            <%-- <telerik:GridBoundColumn DataField="IsDisplay" FilterControlAltText="Filter IsDisplay column"
                                HeaderText="Is Display" SortExpression="IsDisplay"
                                UniqueName="IsDisplay">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridTemplateColumn DataField="Active" FilterControlAltText="Filter Active column"
                                HeaderText="Is Active" SortExpression="Active" UniqueName="Active">
                                <ItemTemplate>
                                    <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("Active"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                    <asp:HiddenField ID="hdnfActive" runat="server" Value='<%#Eval("Active")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this record?"
                                Text="Delete" UniqueName="DeleteColumn">
                                <HeaderStyle CssClass="tplcohdr" />
                                <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                            </telerik:GridButtonColumn>
                        </Columns>
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
