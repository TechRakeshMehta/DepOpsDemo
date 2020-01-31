<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.AttributeInfo"
    Title="AttributeInfo" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="AttributeInfo.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
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
        &nbsp;
    </div>
    <div class="section">
        <h1 class="mhdr">Attribute Information</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAttribute">
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
                            <%--    <div class='vldx'>
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
                            <%-- <div class='vldx'>
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
                            <%--<infs:WclButton runat="server" ID="chkActive" ToggleType="CheckBox" ButtonType="ToggleButton"
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
                    <div class='sxro sx3co' id="divDoc" runat="server" visible="false">
                        <div id="divDocuments" runat="server" visible="false">
                            <div class='sxlb'>
                                <span class="cptn">Document</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbDocument" runat="server" DataTextField="CSD_FileName" OnSelectedIndexChanged="cmbDocument_SelectedIndexChanged"
                                    AutoPostBack="true" DataValueField="CSD_ID">
                                </infs:WclComboBox>
                            </div>
                        </div>
                        <div id="divDocPreview" runat="server" visible="false">
                            <telerik:RadButton ID="btnPreviewDoc" ToolTip="Click here to view the document" OnClientClicked="openPdfPopUp" AutoPostBack="false"
                                runat="server" Font-Underline="true" BackColor="Transparent" BorderStyle="None" ButtonType="LinkButton" Text="Preview Document">
                            </telerik:RadButton>
                        </div>
                    </div>

                    <div class='sxro sx3co' id="dvAdditionalDocuments" runat="server" visible="false">

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
                                        class="errmsg" Display="Dynamic" ErrorMessage="Option is required." ValidationGroup='grpAttribute' /></div>
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
                                        ValidationGroup='grpAttribute' /></div>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>--%>
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
                                <infs:WclTextBox runat="server" ID="txtOptOptions" EmptyMessage="E.g. Positive=1,Negative=2" AutoPostBack="true" OnTextChanged="txtOptOptions_TextChanged">
                                </infs:WclTextBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvOptions" ControlToValidate="txtOptOptions"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Option is required." ValidationGroup='grpAttribute' />
                                </div>
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
                        <div class='sxlb'>
                            <span class="cptn">Trigger(s) Reconciliation</span>
                        </div>
                        <div class='sxlm'>
                            <uc1:IsActiveToggle runat="server" ID="chkTriggerRecon" IsActiveEnable="true" IsAutoPostBack="false" />
                            <%--Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IsTriggersReconciliation") %>' />--%>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div id="dvUniversalAttrAndInputType" class='sxro sx3co' runat="server">
                        <div class='sxlb'>
                            <span class="cptn">Universal Attribute</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="cmbUniversalAttribute" runat="server" ToolTip="Select universal Attribute to map"
                                DataTextField="UF_Name" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" DataValueField="UF_ID" OnSelectedIndexChanged="cmbUniversalAttribute_SelectedIndexChanged"
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
                                <uc1:IsActiveToggle runat="server" ID="ChkSendForIntegration" IsActiveEnable="true" IsAutoPostBack="false" />
                                <%--Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IsTriggersReconciliation") %>' />--%>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div id="dvSelectedInputType" runat="server" class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Selected Input Type</span>
                        </div>
                        <div class='sxlm '>
                            <asp:Repeater ID="rptrInputTypeAttribute" runat="server">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table id="mytable" cellspacing="0" width="100%" align="center">
                                        <tr>
                                            <td style="width: 20%;">
                                                <span>
                                                    <%#Eval("Name")%></span>
                                                <asp:HiddenField ID="hdnUA_ID" Value='<%#Eval("ID")%>'
                                                    runat="server" />
                                            </td>
                                            <td style="width: 10%; padding-left: 10px;">
                                                <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="txtNumericInputPriority" Value='<%#Convert.ToInt32(Eval("InputPriority"))>0?Convert.ToInt32(Eval("InputPriority")):1%>'
                                                    MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter input Priority"
                                                    MinValue="1" Enabled='<%#Eval("Enabled")%>'>
                                                    <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                                </infs:WclNumericTextBox>
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
                </asp:Panel>
            </div>

            <div>
                <div style="float: right;">
                    <infsu:CommandBar ID="fsucCmdBarCat" runat="server" DefaultPanel="pnlAttribute" DisplayButtons="Clear, Save, Cancel"
                        SubmitButtonIconClass="rbEdit" OnSaveClick="fsucCmdBarCat_SaveClick" ClearButtonText="Edit"
                        AutoPostbackButtons="Clear, Save, Cancel" OnClearClick="fsucCmdBarCat_SubmitClick"
                        ClearButtonIconClass="rbEdit" OnCancelClick="fsucCmdBarCat_CancelClick" SaveButtonText="Save"
                        ValidationGroup="grpAttribute">
                    </infsu:CommandBar>
                </div>
                <div class="sxpnl">
                    <div id="dvDisassociate" runat="server" style="padding-top: 10px;">
                        <div class='sxro sx2co' style="clear: none">
                            <div class='sxlb' style="border-color: transparent">
                                <span class="cptn">Dissociate With</span>
                            </div>
                            <div class='sxlm' style="border-color: transparent">
                                <infs:WclComboBox ID="cmbAssociatedAttributes" runat="server" ToolTip="Select Attributes to Dissociate the Item"
                                    DataTextField="Name" DataValueField="ComplianceItemID" CheckBoxes="true" Width="70%" EmptyMessage="--Select--"
                                    AutoPostBack="false">
                                </infs:WclComboBox>
                                <telerik:RadButton ID="btnDissociateAttribute" ToolTip="Click here to dissociate Attribute" runat="server" Text="Dissociate" OnClick="btnDissociateAttribute_Click">
                                </telerik:RadButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%--<infsu:CommandBar ID="fsucCmdBarCat" runat="server" DefaultPanel="pnlAttribute" DisplayButtons="Clear, Save, Cancel"
                SubmitButtonIconClass="rbEdit" OnSaveClick="fsucCmdBarCat_SaveClick" ClearButtonText="Edit"
                AutoPostbackButtons="Clear, Save, Cancel" OnClearClick="fsucCmdBarCat_SubmitClick"
                ClearButtonIconClass="rbEdit" OnCancelClick="fsucCmdBarCat_CancelClick" SaveButtonText="Save"
                ValidationGroup="grpAttribute">
            </infsu:CommandBar>--%>
        </div>
    </div>
    <asp:HiddenField ID="hdnfTenantId" runat="server" />
    <asp:HiddenField ID="hdnfSystemDocumentId" runat="server" />
</asp:Content>


