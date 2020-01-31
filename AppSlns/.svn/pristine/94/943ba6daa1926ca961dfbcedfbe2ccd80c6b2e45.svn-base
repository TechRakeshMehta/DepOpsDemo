<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.RenewalOrder" CodeBehind="RenewalOrder.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div>
    <asp:Panel ID="pnlRenewOrder" runat="server" CssClass="section">
        <h1 class="mhdr">Renewal Order</h1>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlBeforeSubmission">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class='cptn'>Institution Hierarchy</span>
                        </div>
                        <div class='sxlm m3spn'>
                            <asp:Label ID="lblInstitutionHierarchy" runat="server" CssClass="ronly">
                            </asp:Label>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class='cptn'>First Name</span>
                        </div>
                        <div class='sxlm'>
                            <asp:Label ID="lblFirstname" runat="server" CssClass="ronly">
                            </asp:Label>
                        </div>
                        <div class='sxlb'>
                            <span class='cptn'>Last Name</span>
                        </div>
                        <div class='sxlm'>
                            <asp:Label ID="lblLastName" runat="server" CssClass="ronly">
                            </asp:Label>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb' title="The name of the package that you need to purchase">
                            <span class='cptn'>Package</span>
                        </div>
                        <div class='sxlm'>
                            <asp:Label ID="lblPackage" runat="server" CssClass="ronly">
                            </asp:Label>
                        </div>
                        <div class='sxlb' title="Details about your subscription">
                            <span class='cptn'>Details</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <asp:Label ID="lblPackageDetail" runat="server" CssClass="ronly">
                            </asp:Label>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class='cptn'>Renewal Duration (Months)</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtRenewalDuration" Text="">
                                <ClientEvents OnBlur="SetPrice" />
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvRenewalDuration" ControlToValidate="txtRenewalDuration"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Renewal duration is required."
                                    ValidationGroup="grpRenewalOrder" />
                            </div>
                            <div class='vldx'>
                                <asp:RegularExpressionValidator Display="Dynamic" ID="revRenewalDuration" runat="server"
                                    ValidationExpression="^[1-9][0-9]*$" class="errmsg" ValidationGroup="grpRenewalOrder"
                                    ControlToValidate="txtRenewalDuration" ErrorMessage="Zero,decimal and alphabets are not allowed.">
                                </asp:RegularExpressionValidator>
                            </div>
                            <div class='vldx'>
                                <asp:RangeValidator ID="rngvRenewalDuration" runat="server" ControlToValidate="txtRenewalDuration"
                                    Type="Integer" 
                                    ValidationGroup="grpRenewalOrder" Display="Dynamic" CssClass="errmsg">
                                </asp:RangeValidator>
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class='cptn'>Package Price</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" Enabled="false" ID="txtPrice"
                                runat="server" MinValue="0" InvalidStyleDuration="100">
                                <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                            </infs:WclNumericTextBox>
                        </div>
                        <div id="dvRushOrder" runat="server" visible="false">
                            <div class='sxlb'>
                                <span class='cptn'>Rush Order Price</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" Enabled="false" ID="txtRushOrderPrice"
                                    runat="server" MinValue="0" InvalidStyleDuration="100">
                                    <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                                </infs:WclNumericTextBox>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                        <asp:HiddenField ID="hdnTotalPrice" runat="server" />
                    </div>
                </asp:Panel>
            </div>
        </div>
        <div class="cmdButtonMinSize">
            <infsu:CommandBar ID="cmdbarRenewalOrder" runat="server" ButtonPosition="Center" DefaultPanel="pnlRenewOrder" DefaultPanelButton="Extra"
                AutoPostbackButtons="Submit,Save,Extra,Clear" ExtraButtonText="Next" ExtraButtonIconClass="rbNext"
                ClearButtonText="Go To Dashboard" OnExtraClick="cmdbarContinueOrder_Click" SubmitButtonIconClass="rbSave" ClearButtonIconClass=""
                SubmitButtonText="View Details" OnClearClick="cmdbarGoToDashboard_Click" OnSubmitClick="cmdBarViewDetails_Click" SaveButtonText="Cancel" SaveButtonIconClass="rbCancel"
                OnSaveClick="cmdbarCancelOrder_Click" OnExtraClientClick="SetPrice">
            </infsu:CommandBar>
        </div>
    </asp:Panel>
</div>

<script language="javascript" type="text/javascript">
    function SetPrice() {
        var workRenewalMonth = $jQuery("[id$=txtRenewalDuration]").val();
        var totalPrice = $jQuery("[id$=hdnTotalPrice]").val();
        var price = workRenewalMonth * totalPrice;
        var priceId = $jQuery("[id$=txtPrice]")[0].control;
        priceId.set_value(price);
    }
</script>
