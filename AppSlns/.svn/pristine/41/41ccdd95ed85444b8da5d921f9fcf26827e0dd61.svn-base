<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.AttributeList"
    Title="AttributeList" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="AttributeList.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/mapping_pages.css" ResourceType="StyleSheet" />
    </infs:WclResourceManagerProxy>
    <script type="text/javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
        });

        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }
        //13/02/2014 Changes done for - "Attribute listing screen : Show splash screen on save"
        function SaveClick(sender, args) {
            var selecteditem = $find($jQuery("[id$=cmbMaster]")[0].id).get_selectedItem().get_value();
            if (selecteditem == "" || selecteditem == "0") {
                if (Page_Validators != undefined && Page_Validators != null) {
                    var i;
                    for (i = 0; i < Page_Validators.length; i++) {
                        var val = Page_Validators[i];
                        if (!val.isvalid) {
                            return
                        }
                    }
                }
            }
            Page.showProgress("Processing...");
            args.set_cancel(false);
        }

        var winopen = false;

        function openPdfPopUp(sender) {
            var btnID = sender.get_id();
            var hdnfSystemDocumentId = $jQuery("[id$=hdnfSystemDocumentId]").val();
            var documentType = "ClientSystemDocument";
            var tenantID = $jQuery("[id$=hdnfTenantId]").val();
            var composeScreenWindowName = "Service Form Details";
            if (hdnfSystemDocumentId == "0" || hdnfSystemDocumentId == "" || hdnfSystemDocumentId == 0 || hdnfSystemDocumentId == null || hdnfSystemDocumentId == undefined) {
                alert("No document exists for current attribute.");
            }
            else {
                //UAT-2364
                var popupHeight = $jQuery(window).height() * (100 / 100);

                var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?systemDocumentId=" + hdnfSystemDocumentId + "&DocumentType=" + documentType + "&tenantId=" + tenantID);
                var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
                winopen = true;
            }
            return false;
        }

        function OnClientClose(oWnd, args) {
            oWnd.get_contentFrame().src = ''; //This is added for fixing pop-up close issue in Safari browser.
            oWnd.remove_close(OnClientClose);
            if (winopen) {
                winopen = false;
            }
        }
    </script>
    <div class="page_cmd">
        <infs:WclButton runat="server" ID="btnAdd" Text="+ Add an Attribute" OnClick="btnAdd_Click"
            Height="30px" ButtonType="LinkButton">
        </infs:WclButton>
    </div>
    <div class="section" id="divAddForm" runat="server" visible="false">
        <h1 class="mhdr">Add Attribute</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblAddMessage" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlCategory" DefaultButton="btnSave">
                    <div class="sxgrp" id="divSelect" runat="server" visible="true">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Select Attribute</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbMaster" runat="server" ToolTip="Select from a master list OR create new"
                                    DataValueField="ComplianceAttributeID" DataTextField="Name" OnSelectedIndexChanged="cmbMaster_SelectedIndexChanged"
                                    AutoPostBack="true" OnDataBound="cmbMaster_DataBound" OnItemDataBound="cmbMaster_ItemDataBound">
                                </infs:WclComboBox>
                            </div>
                            <%-- <div class='sxlm'>
                                <infs:WclButton runat="server" ID="btnCreate" Text="Create New">
                                    <Icon PrimaryIconCssClass="rbAdd" />
                                </infs:WclButton>
                            </div>--%>
                            <div id="divDocPreviewExisting" runat="server" visible="false">
                                <telerik:RadButton ID="btnPreviewDocExisting" ToolTip="Click here to view the document" OnClientClicked="openPdfPopUp" AutoPostBack="false"
                                    ButtonType="LinkButton" runat="server" Font-Underline="true" Text="Preview Document" BackColor="Transparent" BorderStyle="None">
                                </telerik:RadButton>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <div class="sxgrp" runat="server" id="divCreate" visible="true">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Attribute Name</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtAttrName" MaxLength="500">
                                </infs:WclTextBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvAttributeName" ControlToValidate="txtAttrName"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Attribute Name is required." ValidationGroup='grpAttribute' />
                                </div>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Attribute Label</span>
                                <%--<span class="reqd">*</span>--%>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtAttrLabel" MaxLength="500">
                                </infs:WclTextBox>
                                <%--  <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvAttributeLabel" ControlToValidate="txtAttrLabel"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Attribute Label is required."
                                        ValidationGroup='grpAttribute' /></div>--%>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Screen Label</span><%-- <span class="reqd">*</span>--%>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtScreenLabel" MaxLength="100">
                                </infs:WclTextBox>
                                <%--  <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvScreenLabel" ControlToValidate="txtScreenLabel"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Screen Label is required." ValidationGroup='grpAttribute' /></div>--%>
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
                            <div class='sxlb'>
                                <span class="cptn">Description</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclTextBox runat="server" ID="txtDescription" MaxLength="250">
                                </infs:WclTextBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Attribute Type</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbAttrType" runat="server" AutoPostBack="true" DataTextField="Name"
                                    DataValueField="ComplianceAttributeTypeID" OnSelectedIndexChanged="cmbAttrType_SelectedIndexChanged">
                                </infs:WclComboBox>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Data Type</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbDataType" runat="server" AutoPostBack="true" DataTextField="Name"
                                    DataValueField="ComplianceAttributeDatatypeID" OnSelectedIndexChanged="cmbDataType_SelectedIndexChanged">
                                </infs:WclComboBox>
                            </div>
                            <div id="divAttributeGroup" runat="server" visible="true">
                                <div class='sxlb'>
                                    <span class="cptn">Attribute Group</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclComboBox ID="cmbAttributeGroup" runat="server" DataTextField="CAG_Name"
                                        DataValueField="CAG_ID">
                                    </infs:WclComboBox>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>

                        <div class='sxro sx3co'>

                            <div class='sxlb'>
                                <span class="cptn">Display Order</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclNumericTextBox ShowSpinButtons="false" Type="Number" ID="txtDisplayOrder"
                                    MaxValue="2147483647" runat="server" MinValue="0" InvalidStyleDuration="100"
                                    NumberFormat-DecimalDigits="0" MaxLength="4">
                                </infs:WclNumericTextBox>
                            </div>
                            <div id="divOption" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Options</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox runat="server" ID="txtOptOptions" EmptyMessage="E.g. Positive=1|Negative=2" AutoPostBack="true" OnTextChanged="txtOptOptions_TextChanged">
                                    </infs:WclTextBox>
                                    <div class='vldx'>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvOptions" ControlToValidate="txtOptOptions"
                                            class="errmsg" Display="Dynamic" ErrorMessage="Option is required." ValidationGroup='grpAttribute' />
                                    </div>
                                </div>
                            </div>
                            <div id="divCharacters" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Maximum Characters</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="ntxtTextMaxChars"
                                        MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                        MinValue="1">
                                        <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                    </infs:WclNumericTextBox>
                                    <div class='vldx'>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvMaximumCharacters" ControlToValidate="ntxtTextMaxChars"
                                            class="errmsg" Display="Dynamic" ErrorMessage="Maximum Character is required."
                                            ValidationGroup='grpAttribute' />
                                    </div>
                                </div>
                            </div>
                            <div id="divDocuments" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Document</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclComboBox ID="cmbDocument" runat="server" DataTextField="CSD_FileName" OnSelectedIndexChanged="cmbDocument_SelectedIndexChanged"
                                        DataValueField="CSD_ID" AutoPostBack="true">
                                    </infs:WclComboBox>
                                </div>
                            </div>
                            <div id="divDocPreview" runat="server" visible="false">
                                <telerik:RadButton ID="btnPreviewDoc" ToolTip="Click here to view the document" OnClientClicked="openPdfPopUp" AutoPostBack="false"
                                    runat="server" Font-Underline="true" BackColor="Transparent" BorderStyle="None" ButtonType="LinkButton" Text="Preview Document">
                                </telerik:RadButton>
                            </div>
                            <div id="dvAdditionalDocuments" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Additional Document(s)</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclComboBox ID="cmbAdditionalDocument" runat="server" DataTextField="FileName" DataValueField="SystemDocumentID"
                                        AutoPostBack="false" CausesValidation="false" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Filter="Contains"
                                        OnClientKeyPressing="openCmbBoxOnTab" AllowCustomText="true" EmptyMessage="--SELECT--">
                                        <Localization CheckAllString="All" />
                                    </infs:WclComboBox>
                                </div>
                            </div>

                            <div id="divInstructionText" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Instruction Text</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox runat="server" MaxLength="200" ID="txtInstructionText">
                                    </infs:WclTextBox>
                                    <div class='vldx'>
                                        <%-- <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtInstructionText"
                                            class="errmsg" Display="Dynamic" ErrorMessage="Instruction text is required." ValidationGroup='grpAttribute' />--%>
                                    </div>
                                </div>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Trigger(s) Reconciliation</span>
                            </div>
                            <div class='sxlm'>
                                <uc1:IsActiveToggle runat="server" ID="chkTriggerRecon" IsActiveEnable="true" IsAutoPostBack="false" />
                                <%--Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IsTriggersReconciliation") %>' />--%>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                        <%--<div class='sxro sx3co'>
                            <div id="divOption" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Options</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox runat="server" ID="txtOptOptions" EmptyMessage="E.g. Positive=1,Negative=2">
                                    </infs:WclTextBox>
                                    <div class='vldx'>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvOptions" ControlToValidate="txtOptOptions"
                                            class="errmsg" Display="Dynamic" ErrorMessage="Option is required." ValidationGroup='grpAttribute' />
                                    </div>
                                </div>
                            </div>
                            <div id="divCharacters" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Maximum Characters</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="ntxtTextMaxChars"
                                        MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                        MinValue="1">
                                        <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                    </infs:WclNumericTextBox>
                                    <div class='vldx'>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvMaximumCharacters" ControlToValidate="ntxtTextMaxChars"
                                            class="errmsg" Display="Dynamic" ErrorMessage="Maximum Character is required."
                                            ValidationGroup='grpAttribute' />
                                    </div>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>--%>
                        <div id="dvUniversalAttrAndInputType" class='sxro sx3co' runat="server">
                            <div class='sxlb'>
                                <span class="cptn">Universal Attribute</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbUniversalAttribute" runat="server" ToolTip="Select universal Attribute to map"
                                    DataTextField="UF_Name" DataValueField="UF_ID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnSelectedIndexChanged="cmbUniversalAttribute_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </infs:WclComboBox>
                            </div>

                            <div class='sxlb'>
                                <span class="cptn">Input Type</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbInputAttribute" runat="server" ToolTip="Select Input Type" CheckBoxes="true" EmptyMessage="--SELECT--"
                                    DataTextField="UF_Name" DataValueField="UF_ID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnSelectedIndexChanged="cmbInputAttribute_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </infs:WclComboBox>
                            </div>
                            <div runat="server" id="divIsSendForIntegration">
                                <div class='sxlb'>
                                    <span class="cptn">Override Date to send for Integration</span>
                                </div>
                                <div class='sxlm'>
                                    <uc1:IsActiveToggle runat="server" ID="ChkIsSendForIntegration" IsActiveEnable="true" IsAutoPostBack="false" />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div id="dvSelectedInputType" runat="server" class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Selected Input Type</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <asp:Repeater ID="rptrInputTypeAttribute" runat="server">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table id="mytable" cellspacing="0" width="100%" align="center">
                                            <tr>
                                                <td style="width: 20%;">
                                                    <span>
                                                        <%#Eval("Name")%></span>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:HiddenField ID="hdnUA_ID" Value='<%#Eval("ID")%>'
                                                        runat="server" />
                                                </td>
                                                <td style="width: 5%;">
                                                    <%--<span>Input Priority</span>--%>
                                                </td>
                                                <td id="tdRadioAttribute" runat="server">
                                                    <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="txtNumericInputPriority" Value='<%#Convert.ToInt32(Eval("InputPriority"))>0?Convert.ToInt32(Eval("InputPriority")):1%>'
                                                        MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter input Priority"
                                                        MinValue="1" Enabled='<%#Eval("Enabled")%>'>
                                                        <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                                    </infs:WclNumericTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div id="dvOptionAttributeMapping" runat="server" class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Option Mapping</span>
                            </div>
                            <div class='sxlm '>
                                <asp:Repeater ID="rptOptionMapping" runat="server" OnItemDataBound="rptOptionMapping_ItemDataBound">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table id="mytable" cellspacing="0" width="100%" align="center">
                                            <tr>
                                                <td style="width: 20%;">
                                                    <span>
                                                        <%#Eval("OptionText")%></span>
                                                    <asp:HiddenField ID="hdnMappedUniversalOption_ID" Value='<%#Eval("MappedUniversalOptionID")%>'
                                                        runat="server" />
                                                    <asp:HiddenField ID="hdn_OptionText" Value='<%#Eval("OptionText")%>'
                                                        runat="server" />
                                                </td>
                                                <td style="width: 30%; padding-left: 10px;">
                                                    <%--<infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="txtNumericInputPriority" Value='<%#Convert.ToInt32(Eval("InputPriority"))>0?Convert.ToInt32(Eval("InputPriority")):1%>'
                                                    MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter input Priority"
                                                    MinValue="1" Enabled='<%#Eval("Enabled")%>'>
                                                    <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                                </infs:WclNumericTextBox>--%>
                                                    <infs:WclComboBox ID="cmbUniversalOptions" runat="server" ToolTip="Select universal option to map"
                                                        DataTextField="Value" DataValueField="Key" AutoPostBack="false">
                                                    </infs:WclComboBox>
                                                </td>
                                            </tr>
                                            <tr style="padding-top: 2px;"></tr>
                                        </table>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co monly'>
                            <div class='sxlb'>
                                <span class="cptn">Explanatory Notes</span>
                            </div>
                            <infs:WclTextBox runat="server" ID="txtExplanatoryNotes" TextMode="MultiLine" Height="50px">
                            </infs:WclTextBox>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>

                </asp:Panel>
            </div>
            <div class="sxcbar">
                <div class="sxcmds" style="text-align: right">
                    <%-- //13/02/2014 Changes done for - "Attribute listing screen : Show splash screen on save"--%>
                    <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="fsucCmdBarPackage_SaveClick"
                        OnClientClicking="SaveClick" ValidationGroup="grpAttribute">
                        <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="fsucCmdBarPackage_CancelClick">
                        <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                </div>
            </div>
            <%-- <infsu:CommandBar ID="fsucCmdBarPackage" runat="server" DefaultPanel="pnlPackage"
                DisplayButtons="Save,Cancel" OnSaveClick="fsucCmdBarPackage_SaveClick" AutoPostbackButtons="Save,Cancel"
                OnCancelClick="fsucCmdBarPackage_CancelClick" ValidationGroup="grpAttribute">
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
                    GridLines="None" OnNeedDataSource="grdAttributes_NeedDataSource" OnDeleteCommand="grdAttributes_DeleteCommand"
                    EnableLinqExpressions="false">
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
                            <telerik:GridBoundColumn DataField="ComplianceAttribute.ComplianceAttributeGroup.CAG_Name"
                                FilterControlAltText="Filter AttributeGroup column" HeaderText="Attribute Group"
                                SortExpression="ComplianceAttribute.ComplianceAttributeGroup.CAG_Name" UniqueName="AttributeGroup">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ComplianceAttribute.Description" FilterControlAltText="Filter Description column"
                                HeaderText="Description" SortExpression="ComplianceAttribute.Description" UniqueName="Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ComplianceAttribute.TenantName" FilterControlAltText="Filter TenantName column"
                                HeaderText="Tenant" SortExpression="ComplianceAttribute.TenantName" UniqueName="TenantName">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="ComplianceAttribute.IsActive" FilterControlAltText="Filter Active column" DataType="System.Boolean"
                                HeaderText="Is Active" SortExpression="ComplianceAttribute.IsActive" UniqueName="CIA_IsActive">
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
    <asp:HiddenField ID="hdnfTenantId" runat="server" />
    <asp:HiddenField ID="hdnfSystemDocumentId" runat="server" />
</asp:Content>
