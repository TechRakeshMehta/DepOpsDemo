<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditServiceItemDetail.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.EditServiceItemDetail" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<script type="text/javascript">
    $jQuery(document).ready(function () {
        HideShowEditableByValidator();
    });
    function HideShowEditableByValidator() {
        if ($jQuery("[id$=cmbPriceType]").length > 0) {
            var cmbPriceTypeSelectedValue = $jQuery("[id$=cmbPriceType]")[0].value;
            if (cmbPriceTypeSelectedValue != "--SELECT--") {
                $jQuery("[id$=rfvAdditionalOccrPrice]")[0].enabled = true;
                $jQuery("[id$=AddOccPrcReqd]")[0].style.display = "block";
            }
            else {
                $jQuery("[id$=rfvAdditionalOccrPrice]")[0].enabled = false;
                $jQuery("[id$=AddOccPrcReqd]")[0].style.display = "none";
            }
        }
    }
</script>
<div class="section" runat="server" id="divEditBlock">
    <h1 class="mhdr">
        <asp:Label ID="lblEHAttr" Text="Update Service Item" runat="server" /></h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlServiceItem">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Service Item Name</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtServiceItemName" MaxLength="50">
                        </infs:WclTextBox>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ID="rfvServiceItemName" ControlToValidate="txtServiceItemName"
                                class="errmsg" ValidationGroup="grpServiceItem" Display="Dynamic" ErrorMessage="Service Item Name is required." />
                        </div>
                    </div>
                    <%--<div class='sxlb'>
                        <span class="cptn">Service Item Label</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtServiceItemLabel" MaxLength="200">
                        </infs:WclTextBox>
                    </div>--%>
                    <div class='sxlb'>
                        <span class="cptn">Service Item Type</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbServiceItemType"
                            runat="server" DataTextField="SIT_Name" DataValueField="SIT_ID">
                        </infs:WclComboBox>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ID="rfvServiceItemType" ControlToValidate="cmbServiceItemType"
                                class="errmsg" ValidationGroup="grpServiceItem" Display="Dynamic" ErrorMessage="Service Item Type is required."
                                InitialValue="--SELECT--" />
                        </div>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Parent Service Item</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbParentFeeItem" runat="server" DataTextField="PSI_ServiceItemName"
                            DataValueField="PSI_ID">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Attribute Group</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbAttributeGroup" runat="server" DataTextField="BSAD_Name"
                            DataValueField="BSAD_ID" AutoPostBack="true" OnSelectedIndexChanged="cmbAttributeGroup_SelectedIndexChanged">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvAttributeGroup" ControlToValidate="cmbAttributeGroup"
                                InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpServiceItem" CssClass="errmsg"
                                Text="Attribute Group is required." />
                        </div>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Global Fee Item</span><%--<span class="reqd">*</span>--%>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbGlobalFeeItem" runat="server" DataTextField="PSIF_Name"
                            DataValueField="PSIF_ID">
                        </infs:WclComboBox>
                        <%-- <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvGlobalFeeItem" ControlToValidate="cmbGlobalFeeItem"
                                InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpServiceItem" CssClass="errmsg"
                                Text="Global Fee Item is required." />
                        </div>--%>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Additional occurence price type</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbPriceType" DataTextField="SIPT_Name" DataValueField="SIPT_ID" runat="server" OnClientSelectedIndexChanged="HideShowEditableByValidator"></infs:WclComboBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Additional Occurence Price</span><span id="AddOccPrcReqd" runat="server" style="display: none" class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclNumericTextBox ShowSpinButtons="True" Type="Currency" ID="ntxtAddOccPrice"
                            MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                            MinValue="0">
                            <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                        </infs:WclNumericTextBox>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator Enabled="false" runat="server" ID="rfvAdditionalOccrPrice" ControlToValidate="ntxtAddOccPrice"
                                class="errmsg" Display="Dynamic" ErrorMessage="Additional Occurence Price is required."
                                ValidationGroup='grpServiceItem' />
                        </div>
                    </div>
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
                        <span class="cptn">Is Supplemental</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclButton runat="server" ID="chkSupplement" ToggleType="CheckBox" ButtonType="ToggleButton"
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
                        <span class="cptn">Quantity Group</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbQuantityGroups" DataTextField="PSI_ServiceItemName" DataValueField="PSI_ID" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="cmbQuantityGroups_SelectedIndexChanged">
                        </infs:WclComboBox>
                    </div>
                    <asp:Panel ID="pnl" runat="server">
                        <div class='sxlb'>
                            <span class="cptn">Quantity Included</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox ID="txtQuantityIncluded" runat="server"></infs:WclNumericTextBox>
                        </div>
                    </asp:Panel>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co' id="divSettings" runat="server" style="display: none;">
                    <div id="divMinOcc" runat="server" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">Min Occurrences</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtMinOccurrences" Text="" MaxLength="200">
                            </infs:WclTextBox>
                            <div class="vldx">
                                <asp:RegularExpressionValidator ID="revMinOccurrences" runat="server" ControlToValidate="txtMinOccurrences"
                                    class="errmsg" ValidationGroup="grpServiceItem" Display="Dynamic" ErrorMessage="Please enter valid number."
                                    ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </div>
                    <div id="divMaxOcc" runat="server" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">Max Occurrences</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtMaxOccurrences" Text="" MaxLength="200">
                            </infs:WclTextBox>
                            <div class="vldx">
                                <asp:RegularExpressionValidator ID="revMaxOccurrences" runat="server" ControlToValidate="txtMaxOccurrences"
                                    class="errmsg" ValidationGroup="grpServiceItem" Display="Dynamic" ErrorMessage="Please enter valid number."
                                    ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                <asp:CompareValidator ID="cvMaxOcc" runat="server" Operator="GreaterThan" Type="Integer" ControlToCompare="txtMinOccurrences" ControlToValidate="txtMaxOccurrences"
                                    class="errmsg"  ValidationGroup="grpServiceItem" Display="Dynamic" ErrorMessage="Max Occurrence should be greater than Min Occurence." />

                            </div>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Service Item Description</span>
                    </div>
                    <div class='sxlm m2spn'>
                        <infs:WclTextBox runat="server" ID="txtServiceItemDescription" MaxLength="200">
                        </infs:WclTextBox>
                    </div>                    
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <div id="divSaveButton" runat="server">
        <div class="sxcbar">
            <div class="sxcmds" style="text-align: right">
                <infs:WclButton ID="btnCreateRule" runat="server" Text="Create Rule" Visible="false" OnClick="btnCreateRule_Click">
                    <Icon PrimaryIconCssClass="" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                        PrimaryIconWidth="14" />
                </infs:WclButton>
                <infs:WclButton ID="btnEdit" runat="server" Text="Edit" OnClick="CmdBarEdit_Click">
                    <Icon PrimaryIconCssClass="rbEdit" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                        PrimaryIconWidth="14" />
                </infs:WclButton>
                <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="CmdBarUpdate_Click"
                    ValidationGroup="grpServiceItem">
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
</div>
