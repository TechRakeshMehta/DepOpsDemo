<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditAttribute.aspx.cs" Inherits="CoreWeb.BkgSetup.Views.EditAttribute"
    Title="Edit Attribute" MasterPageFile="~/Shared/ChildPage.master" %>

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

        function IsRequiredClicked(isReq) {
            //var isReq = $jQuery("[id$=btnIsReq]");
            var isReqDiv = $jQuery("[id$=divIsRequired]");
            var rfvReqdMsg = $jQuery("[id$=rfvReqValMessage]");
            if (isReq._checked) {
                //rfvReqdMsg[0].enabled = true;
                ValidatorEnable(rfvReqdMsg[0], true);
                rfvReqdMsg.hide();
                isReqDiv.show();
            }
            else {
                //rfvReqdMsg[0].enabled = false;
                ValidatorEnable(rfvReqdMsg[0], false);
                isReqDiv.hide();
            }
        }
    </script>
    <div class="section" id="div" runat="server">
        <div class="content">
            <div id="divEditButton" runat="server">
                <div class="page_cmd">
                    <infs:WclButton runat="server" ID="btnEdit" Text="+ Edit Attribute" OnClick="CmdBarEditAttribute_Click"
                        ButtonType="LinkButton" Height="30px">
                    </infs:WclButton>
                    <%-- <infs:WclButton runat="server" ID="btnEditLocally" Text="+ Edit Attribute Locally" OnClick="btnEditLocally_Click"
                        ButtonType="LinkButton" Height="30px">
                    </infs:WclButton>--%>
                </div>
                <%--<infsu:CommandBar ID="fsucCmdBarEditAttribute" runat="server" DefaultPanel="pnlEditAttribute"
                    DisplayButtons="Save" AutoPostbackButtons="Save" SaveButtonText="Edit Attribute"
                    SaveButtonIconClass="rbEdit" ButtonPosition="Center" OnSaveClick="CmdBarEditService_Click">
                </infsu:CommandBar>--%>
            </div>
            <div class="content">
                <div id="divEditAttribute" runat="server">
                    <h1 class="mhdr">
                        <span id="spnAttributeTitle" runat="server">Update Attribute</span>
                    </h1>
                    <div class="sxform auto">
                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAttribute">
                            <div class="sxgrp" id="div1" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Attribute Name</span><span class="reqd">*</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtAttributeName" MaxLength="256">
                                        </infs:WclTextBox>
                                        <div class='vldx'>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvAttributeName" ControlToValidate="txtAttributeName"
                                                class="errmsg" ValidationGroup="grpEditAttribute" Display="Dynamic" ErrorMessage="Attribute Name is required." />
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
                                                class="errmsg" ValidationGroup="grpEditAttribute" Display="Dynamic" ErrorMessage="Attribute Label is required." />
                                        </div>
                                    </div>
                                    <div class="sxlm">
                                        <asp:LinkButton ID="lnkbtnEditLocally" runat="server" Text="Edit Attribute Locally" OnClick="btnEditLocally_Click">
                                        </asp:LinkButton>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>

                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Is Active</span>
                                    </div>
                                    <div class='sxlm'>
                                        <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" IsAutoPostBack="false" />

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
                                        <%-- <div class='vldx'>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvAttributeDescription" ControlToValidate="txtAttributeDescription"
                                                class="errmsg" ValidationGroup="grpEditAttribute" Display="Dynamic" ErrorMessage="Attribute Description is required." />
                                        </div>--%>
                                    </div>
                                </div>
                                <div class='sxro sx3co'>
                                    <%-- <div class='sxlb'>
                                        <span class="cptn">Is Required</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclButton runat="server" ID="chkRequired" ToggleType="CheckBox" ButtonType="ToggleButton"
                                            AutoPostBack="false" OnClientCheckedChanged="IsRequiredClicked">
                                            <ToggleStates>
                                                <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                <telerik:RadButtonToggleState Text="No" Value="False" />
                                            </ToggleStates>
                                        </infs:WclButton>
                                    </div>--%>
                                    <div class='sxlb'>
                                        <span class="cptn">Data Type</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclComboBox ID="cmbDataType" runat="server" AutoPostBack="true" DataTextField="SADT_Name"
                                            DataValueField="SADT_ID" OnSelectedIndexChanged="cmbDataType_SelectedIndexChanged">
                                        </infs:WclComboBox>
                                        <div class="vldx">
                                            <asp:RequiredFieldValidator runat="server" ID="rfvDataType" ControlToValidate="cmbDataType"
                                                InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpAttribute" CssClass="errmsg"
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
                                                    class="errmsg" Display="Dynamic" ErrorMessage="Option is required." ValidationGroup='grpAttribute' />
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
                                                MinValue="0">
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
                                                MinValue="0">
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
                                <div class='sxro sx3co'>
                                    <div id="divIsRequired" runat="server">
                                        <div class='sxlb'>
                                            <span class="cptn">Is Required</span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclButton runat="server" ID="chkIsRequired" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                AutoPostBack="false">
                                                <ToggleStates>
                                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                                </ToggleStates>
                                            </infs:WclButton>
                                        </div>
                                        <div class='sxlb'>
                                            <span class="cptn">Is Display</span><span class="reqd">*</span>
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
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div runat="server" id="divRegexValidate">
                                    <div class="sxro sx3co">
                                        <div class='sxlb'>
                                            <span class="cptn">Regular Expression</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox runat="server" ID="txtRegularExpression" MaxLength="256">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class='sxlb'>
                                            <span class="cptn">Error Message</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox runat="server" ID="txtErrorMessage" MaxLength="256">
                                            </infs:WclTextBox>
                                        </div>
                                    </div>
                                    <div class="sxro sx3co">
                                        <div class='sxlb'>
                                            <span class="cptn">Input Text to Validate</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox runat="server" ID="txtInputToValidate">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class="sxlm" style="width: 40%;">
                                            <infs:WclButton runat="server" ID="btnValidateRegExp" Text="Validate" OnClick="btnValidateRegExp_Click" AutoPostBack="true"></infs:WclButton>
                                            <asp:Label runat="server" ID="lblValidStatus"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <asp:HiddenField ID="hdnIsSystemConfigured" runat="server" Value="" />
                    <asp:HiddenField ID="hdnIsCompleteEdit" runat="server" Value="" />
                    <asp:HiddenField ID="hdnIsEditable" runat="server" Value="" />
                    <asp:HiddenField ID="hdnServiceAttributeMappingID" runat="server" Value="" />
                    <div id="divSaveButton" runat="server" visible="false">
                        <div class="sxcbar">
                            <div class="sxcmds" style="text-align: right">
                                <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="fsucCmdBarCat_SaveClick" OnClientClicking="SaveClick"
                                    ValidationGroup="grpAttribute">
                                    <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                                        PrimaryIconWidth="14" />
                                </infs:WclButton>
                                <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="fsucCmdBarCat_CancelClick">
                                    <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                                        PrimaryIconWidth="14" />
                                </infs:WclButton>
                            </div>
                        </div>
                        <%-- <infsu:CommandBar ID="fsucCmdBarCat" runat="server" DefaultPanel="pnlAttribute" DisplayButtons="Save, Cancel"
                            SubmitButtonIconClass="rbEdit" OnSaveClick="fsucCmdBarCat_SaveClick" OnSaveClientClick="SaveClick"
                            AutoPostbackButtons=" Save, Cancel"
                            OnCancelClick="fsucCmdBarCat_CancelClick" SaveButtonText="Update"
                            ValidationGroup="grpAttribute">
                        </infsu:CommandBar>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
