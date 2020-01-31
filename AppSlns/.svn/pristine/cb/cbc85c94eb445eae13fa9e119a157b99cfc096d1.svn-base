<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.SubscriptionPackage" Title="SubscriptionPackage"
    MasterPageFile="~/Shared/ChildPage.master" CodeBehind="SubscriptionPackage.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/PackagePaymentOptions.ascx" TagPrefix="infsu" TagName="PackagePaymentOptions" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/AdditionalDocumentsMapping.ascx" TagPrefix="infsu" TagName="AdditionalDocumentsMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="msgbox" id="pageMsgBox">
        <asp:Label CssClass="info" EnableViewState="false" runat="server" ID="lblError" Text="Note: The Custom (Monthly) Subscription Option is used only in pro-rata calculation
                            for renewal order and change program."></asp:Label>
    </div>
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblSubscription" runat="server" Text=""></asp:Label></h1>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel ID="pnlSubscription" CssClass="sxpnl" runat="server">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Subscription Option</span>
                        </div>
                        <div class='sxlm m3spn'>
                            <asp:CheckBoxList ID="chkSubscriptionOption" DataTextField="Label" RepeatDirection="Horizontal"
                                DataValueField="SubscriptionOptionID" runat="server">
                            </asp:CheckBoxList>
                            <div class='vldx'>
                                <asp:CustomValidator ID="cvSubscriptionOption" CssClass="errmsg" Display="Dynamic"
                                    runat="server" EnableClientScript="true" ErrorMessage="At least one Subscription Option is required in addition to Custom (Monthly)."
                                    ValidationGroup="grpFormSubmit" ClientValidationFunction="ValidateSubscriptionOption">
                                </asp:CustomValidator>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Price Model</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <%--<infs:WclDropDownList ID="ddlPriceModel" runat="server" DataTextField="Name" DataValueField="PriceModelID"
                                OnDataBound="ddlPriceModel_DataBound">
                            </infs:WclDropDownList>--%>
                            <infs:WclComboBox ID="ddlPriceModel" runat="server" DataTextField="Name" DataValueField="PriceModelID"
                                OnDataBound="ddlPriceModel_DataBound">
                            </infs:WclComboBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvPriceModel" ControlToValidate="ddlPriceModel"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please select Price Model."
                                    InitialValue="--SELECT--" />
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Priority</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox ID="txtPriority" Type="Number" runat="server" MinValue="0" MaxLength="9">
                                <NumberFormat AllowRounding="false" DecimalDigits="0" DecimalSeparator="." />
                            </infs:WclNumericTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvPriority" ControlToValidate="txtPriority"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Priority is required." />
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Available For Order</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclButton runat="server" ID="chkAvailableForOrder" ToggleType="CheckBox" ButtonType="ToggleButton"
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
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Auto Renew Invoice Order</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclButton runat="server" ID="chkAutoRenewInvoiceOrder" ToggleType="CheckBox" ButtonType="ToggleButton"
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
            <div class='sxroend'>
            </div>
            <asp:Panel ID="pnlPkgPaymentOptions" runat="server">
                <infsu:PackagePaymentOptions ID="ucPkgPaymentOptions" runat="server" />
            </asp:Panel>
            <infsu:CommandBar ID="fsucCmdBarSubscription" runat="server" ButtonPosition="Right" DisplayButtons="Submit"
                AutoPostbackButtons="Submit" SubmitButtonIconClass="rbSave" SubmitButtonText="Save"
                ValidationGroup="grpFormSubmit" OnSubmitClick="CmdBarSave_Click" OnSubmitClientClick="ConfirmSave">
            </infsu:CommandBar>
            <asp:HiddenField ID="hdnSavedPriceModelId" runat="server" />
            <div class="gclr">
            </div>
        </div>
    </div>

    <hr style="border-bottom: solid 1px #c0c0c0;" />
    <div class="section" id="dvAdditionalDocuments" runat="server">
        <h1 class="mhdr">
            <asp:Label ID="Label4" runat="server" Text="Map Additional Documents"></asp:Label>
        </h1>
        <div class="content">
            <infsu:AdditionalDocumentsMapping runat="server" ID="AdditionalDocumentsMapping" />
        </div>
    </div>

    <script type="text/javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
        });

        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }

        // Function to validate Subscription Options
        function ValidateSubscriptionOption(sender, args) {
            var options = $jQuery("[id$=chkSubscriptionOption] input:checked");
            //Loop start from 1 not from 0, because at least one Subscription Option is required in addition to Custom (Monthly).
            for (var i = 1; i < options.length; i++) {
                if (options[i].checked) {
                    args.IsValid = true;
                    return false;
                }
            }
            args.IsValid = false;
        }

        function ConfirmSave(sender, eventArgs) {
            var comboBox = $find("<%= ddlPriceModel.ClientID %>");
            var comboBoxValue = comboBox.get_value();
            if ($jQuery('[id$=hdnSavedPriceModelId]').val() > 0 && $jQuery('[id$=hdnSavedPriceModelId]').val() != comboBoxValue) {
                var IsOKClicked = confirm('The Price Model is changing. Prices will be reset for all subscriptions in this package. Do you want to continue?');
                if (IsOKClicked)
                    sender.set_autoPostBack(true);
                else
                    sender.set_autoPostBack(false);
            }
            else {
                sender.set_autoPostBack(true);
                return true;
            }
        }

    </script>
</asp:Content>
