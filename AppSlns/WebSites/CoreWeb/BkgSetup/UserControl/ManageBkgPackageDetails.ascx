<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageBkgPackageDetails.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageBkgpackageDetails" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<%@ Register Src="~/ComplianceAdministration/UserControl/PackagePaymentOptions.ascx" TagPrefix="infsu" TagName="PackagePaymentOptions" %>

<style type="text/css">
    .paymentOption {
        background-color: #EEEEEE !important;
        opacity: 1 !important;
    }
</style>

<script type="text/jscript">
    function KeyPress(sender, args) {
        if (args.get_keyCharacter() == sender.get_numberFormat().DecimalSeparator || args.get_keyCharacter() == '-') {
            args.set_cancel(true);
        }
    }

    //UAT-3268
    function ManageAdditionalPrice(sender) {
        //debugger;
        var selectedValue = $jQuery("[id$=rblAdditionalPrice] [type='radio']:checked").val();

        $jQuery("[id$=hdnIsAdditionalPriceAvailable]").val(selectedValue);

        if (selectedValue == "True") {
            $jQuery("[id$=dvAdditionalPrice]").attr('style', 'display:block;');
            EnableValidator($jQuery("[id$=rfvAdditionalPrice]")[0].id);
            EnableValidator($jQuery("[id$=rfvMinPrice]")[0].id);
            EnableValidator($jQuery("[id$=rfvAdditionalPricePaymentOption]")[0].id);
        }
        else {
            // debugger;
            $jQuery("[id$=dvAdditionalPrice]").attr('style', 'display:none;');
            $jQuery("[id$=txtAdditionalPrice]").val("");
            $jQuery("[id$=rbtnAdditionalPricePaymentOption] [type='radio']:checked").removeAttr('checked');
            DisableValidator($jQuery("[id$=rfvAdditionalPrice]")[0].id);
            DisableValidator($jQuery("[id$=rfvMinPrice]")[0].id);
            DisableValidator($jQuery("[id$=rfvAdditionalPricePaymentOption]")[0].id);

        }
    }

    function EnableValidator(id) {
        if ($jQuery('#' + id)[0] != undefined) {
            ValidatorEnable($jQuery('#' + id)[0], true);
            $jQuery('#' + id).hide();
        }
    }

    function DisableValidator(id) {
        if ($jQuery('#' + id)[0] != undefined) {
            ValidatorEnable($jQuery('#' + id)[0], false);
        }
    }

 <%--   function changeValueForAdminEntry(args) {
        //debugger;
        //alert('package selected Admin Entry');
        var button = $find("<%= chkHRPortal.ClientID %>");
        if (args.get_checked()) {
            button.set_checked(false);
        }
        else {
            button.set_checked(true);
        }
    }

    function changeValueForAvailableOrder(args) {
        //debugger;
        //    alert('package selected available order');
        var button = $find("<%= chkAvailableForOrder.ClientID %>");
        if (args.get_checked()) {
            button.set_checked(false);
        }
        else {
            button.set_checked(true);
        }
    }--%>


</script>
<div id="divPackage" runat="server">
    <h1 class="mhdr">
        <asp:Label ID="lblPackageDetailTitle" runat="server" Text="Package Detail"></asp:Label>
    </h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPackage">
                <div class="sxgrp" id="divSelect" runat="server">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Package Name</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtPkgName" runat="server">
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvPackageName" ControlToValidate="txtPkgName"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Package Name is required." />
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Package Label</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtPkgLabel" runat="server">
                            </infs:WclTextBox>
                        </div>

                        <div class='sxlb'>
                            <span class="cptn">PackageType</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="cmbBkgPackageType" OnDataBound="cmbBkgPackageType_DataBound" runat="server" DataTextField="BPT_Name" AutoPostBack="false"
                                DataValueField="BPT_Id">
                            </infs:WclComboBox>
                            <div class='vldx'>
                            </div>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Base Price</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox ID="txtPrice" Type="Currency" runat="server" MinValue="0" MaxLength="9">
                            </infs:WclNumericTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvPrice" ControlToValidate="txtPrice"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Price is required." />
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Is Exclusive</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <asp:RadioButtonList ID="rbtnExclusive" runat="server" RepeatDirection="Horizontal"
                                CssClass="radio_list" AutoPostBack="false">
                                <asp:ListItem Text="Yes" Value="True" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="No" Value="False"></asp:ListItem>
                            </asp:RadioButtonList>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvExclusive" ControlToValidate="rbtnExclusive"
                                    Display="Dynamic" CssClass="errmsg" Text="Is Exclusive is required." ValidationGroup="grpFormSubmit" />
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Transmit To Vendor</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclButton runat="server" ID="chkTransmitToVendor" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Does Require First Review</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclButton runat="server" ID="chkFirstReview" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="true"
                                Value="false" OnToggleStateChanged="chkFirstReview_ToggleStateChanged">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Supplemental Type</span><span id="spSupptype" class="reqd" runat="server"
                                visible="false">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="cmbSupplementalType" runat="server" Enabled="false"
                                DataTextField="PST_Name" DataValueField="PST_ID" EmptyMessage="--SELECT--">
                            </infs:WclComboBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvSupplementalType" ControlToValidate="cmbSupplementalType"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Please select Supplemental Type." Enabled="false " ValidationGroup="grpFormSubmit" />
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Instructions</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclTextBox runat="server" ID="txtInstruction"
                                MaxLength="1024">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Available For Order</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclButton runat="server" ID="chkAvailableForOrder" ToggleType="CheckBox" ButtonType="ToggleButton"
                                AutoPostBack="false" >
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>
                        </div>

                        <div class='sxroend'>
                        </div>
                    </div>

                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Price Text</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclTextBox runat="server" ID="txtPriceText"
                                MaxLength="1024">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Available For Admin Entry</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclButton runat="server" ID="chkHRPortal" ToggleType="CheckBox" ButtonType="ToggleButton"
                                AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>
                        </div>


                        <%-- <div id="divMaxYearForResidence" runat="server" visible="false">
                            <div class='sxlb'>
                                <span class="cptn">Maximum Number Of Years For Residence</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="txtMaxNumberOfYearforResidence"
                                    MaxValue="100" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                    MinValue="0">
                                    <NumberFormat AllowRounding="false" DecimalDigits="0" />
                                    <ClientEvents OnKeyPress="KeyPress" />
                                </infs:WclNumericTextBox>
                            </div>
                        </div>--%>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div id="Div1" runat="server">
                            <div class='sxlb'>
                                <span class="cptn">Passcode</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox ID="txtPasscode" runat="server" MaxLength="6">
                                </infs:WclTextBox>
                                <div class='vldx'>
                                    <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator3" ControlToValidate="txtPasscode"
                                        Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d]{1,50}$" ValidationGroup="grpFormSubmit"
                                        ErrorMessage="Invalid Characters." />
                                </div>
                            </div>
                        </div>

                        <div id="divMaxYearForResidence" runat="server" visible="false">
                            <div class='sxlb'>
                                <span class="cptn">Maximum Number Of Years For Residence</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="txtMaxNumberOfYearforResidence"
                                    MaxValue="100" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                    MinValue="0">
                                    <NumberFormat AllowRounding="false" DecimalDigits="0" />
                                    <ClientEvents OnKeyPress="KeyPress" />
                                </infs:WclNumericTextBox>
                            </div>
                        </div>

                        <%--   <div  id="hrPortalDiv">                    
                        <div class='sxlb'>
                            <span class="cptn">Available For Admin Entry</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclButton runat="server" ID="chkHRPortal" ToggleType="CheckBox" ButtonType="ToggleButton" 
                                AutoPostBack="false" onclienttogglestatechanged="changeValueForAvailableOrder">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>
                        </div>                     
                    </div>--%>

                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div id="dvIsAdditionalPriceAvailable" runat="server">
                            <div class='sxlb'>
                                <span class="cptn">Include Additional Price</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rblAdditionalPrice" runat="server" RepeatDirection="Horizontal"
                                    CssClass="radio_list" AutoPostBack="false" onclick="ManageAdditionalPrice(this);">
                                    <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="False" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div id="dvAdditionalPrice" style="display: none" runat="server">

                            <div class='sxlb'>
                                <span class="cptn">Additional Price</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclNumericTextBox ID="txtAdditionalPrice" Type="Currency" runat="server" MinValue="0" MaxLength="9">
                                </infs:WclNumericTextBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvAdditionalPrice" ControlToValidate="txtAdditionalPrice" Enabled="false"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Additional Price is required." />
                                    <asp:CompareValidator runat="server" ID="rfvMinPrice" ValueToCompare="0" Type="Currency" Operator="GreaterThan" ControlToValidate="txtAdditionalPrice" Enabled="false"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Additional Price should be greater than zero." />
                                </div>
                            </div>
                            <div class='sxlb paymentOption'>
                                <span class="cptn">Payment Option</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rbtnAdditionalPricePaymentOption" RepeatDirection="Horizontal" runat="server" DataTextField="Name" DataValueField="PaymentOptionID">
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator runat="server" ID="rfvAdditionalPricePaymentOption" ControlToValidate="rbtnAdditionalPricePaymentOption" Enabled="false"
                                    Display="Dynamic" CssClass="errmsg" Text="Payment option is required." ValidationGroup="grpFormSubmit" />
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>

                    <div id="dvAutpmaticPackageInvitation" class='sxro sx3co'>
                        <h2 style="color: #bd6a38" class="mhdr">Automatic Package Invitation</h2>
                        <div class='sxlb'>
                            <span class="cptn">Automatic Package Invitation Setting</span>
                        </div>
                        <div class='sxlm'>
                            <asp:RadioButton ID="rbtnTriggerAutomaticPackageInvitationYes" OnCheckedChanged="rbtnTriggerAutomaticPackageInvitationYes_CheckedChanged" AutoPostBack="true" runat="server" GroupName="TriggerAutomaticPackageInvitation" Text="Yes" />
                            <asp:RadioButton ID="rbtnTriggerAutomaticPackageInvitationNo" OnCheckedChanged="rbtnTriggerAutomaticPackageInvitationNo_CheckedChanged" AutoPostBack="true" runat="server" GroupName="TriggerAutomaticPackageInvitation" Text="No" />
                        </div>


                        <div runat="server" id="dvShowSettings">
                            <div class='sxlb'>
                                <span class="cptn">Month(s)</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="txtAutomaticInvitationMonth"
                                    MaxValue="99" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a month"
                                    MinValue="1">
                                    <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                </infs:WclNumericTextBox>
                                <div style="color: red" class='vldx'>
                                    <asp:Label ID="lblErrorMsg" Visible="false" Style="color: red" runat="server" Text="Month is required."></asp:Label>
                                </div>
                            </div>
                            <div class="sxlb">
                                <span class="cptn">Package(s)</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="chkBkgInvitationPackages" runat="server" Width="277px" CheckBoxes="true" EmptyMessage="--SELECT--"
                                    AutoPostBack="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                                    <Localization CheckAllString="Select All" />
                                </infs:WclComboBox>
                                <div style="color: red" class='vldx'>
                                    <asp:Label ID="lblSeletedPackageError" Visible="false" Style="color: red" runat="server" Text="Package is required."></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <asp:HiddenField ID="hdnIsAdditionalPriceAvailable" runat="server" Value="" />
                </div>
            </asp:Panel>
        </div>
        <asp:Panel runat="server" ID="pnlPkgPaymentOptions">
            <infsu:PackagePaymentOptions ID="ucPkgPaymentOptions" runat="server" />
        </asp:Panel>
        <infsu:CommandBar ID="fsucCmdBarSaveNode" runat="server" DefaultPanel="pnlPackage"
            DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel" ValidationGroup="grpFormSubmit"
            ButtonPosition="Right" OnSaveClick="CmdBarSave_Click" OnCancelClick="CmdBarCancel_Click">
        </infsu:CommandBar>
    </div>
</div>
<div id="divSaveButton" runat="server">
</div>
