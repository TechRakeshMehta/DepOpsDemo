<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModifyShippingInfo.ascx.cs" Inherits="CoreWeb.ComplianceOperations.UserControl.ModifyShippingInfo" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="Location" Src="~/CommonControls/UserControl/LocationInfo.ascx" %>
<div class="section">
    <h1 class="mhdr"><%=Resources.Language.MODIFYSHIPPINGADDRESS%>
    </h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlRegForm">

                <div class='sxro sx3co' id="dvAddress" visible="false" runat="server">
                    <h1 class="shdr"><%=Resources.Language.CONTACTINFO%></h1>
                    <div class='sxro sx3co'>
                        <div class='sxro sx3co sxlb'>
                            <span class='cptn'><%=Resources.Language.ADDRESS%></span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtAddress" MaxLength="100">
                            </infs:WclTextBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvAddress" ControlToValidate="txtAddress"
                                    Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,ADDRESSREQ%>" />
                                <asp:RegularExpressionValidator Display="Dynamic" Enabled="true" ID="revAddress" runat="server"
                                    CssClass="errmsg" ErrorMessage="<%$Resources:Language,ADDRESSASCIIINVALIDCODE%>" ControlToValidate="txtAddress"
                                    ValidationExpression="^[\x01-\x7F]+$" />
                            </div>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                    <uc:Location ID="MailingAddress" ZipTabIndex="6" CityTabIndex="7" runat="server" ControlsExtensionId="AOP"
                        NumberOfColumn="Three" ValidationGroup="grpFormSubmit" IsReverselookupControl="true" />
                    <div class='sxroend'>
                    </div>

                    <div class='sxro sx3co' id="dvMailingOption" runat="server">
                        <h1 class="shdr"><%=Resources.Language.MAILINGOPTION%></h1>
                        <div class="sxro sx3co" runat="server">
                            <div class="sxlb">
                                <span class="cptn"><%=Resources.Language.SELECTMAILINGOPTION%></span>
                            </div>
                            <div class="sxlm">
                                <infs:WclComboBox ID="cmbMailingOption" runat="server" DataValueField="FieldID" DataTextField="DisplayText"></infs:WclComboBox>
                            </div>
                        </div>
                    </div>
                </div>

                <%--                        <div class='sxro sx3co' id="dvOrderPayment" visible="false" runat="server">
                            <uc:OrderPayment ID="OrderPayment" runat="server" />
                            <div class='sxroend'>
                            </div>
                        </div>--%>

                <div class='sxro sx3co' id="dvOrderConfirmation" visible="false" runat="server">
                    <h2 style="width: 80%; padding-top: 40px; margin: 0 auto;"><%=Resources.Language.THANKSORDERCONFIRMD%></h2>
                    <div class='sxroend'>
                    </div>
                    <div id="Div1" style="width: 100%; padding-top: 100px; text-align: center" runat="server">
                        <infs:WclButton runat="server" ID="btnClose" CssClass="padRight2" Visible="true" Icon-PrimaryIconCssClass="rbCancel"
                            Text="<%$ Resources:Language, CLOSE %>" OnClick="btnClose_Click" OnClientClicked="returnToParent">
                        </infs:WclButton>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>

                <div id="dvButtons" style="width: 100%; padding-top: 100px; text-align: center" runat="server" visible="true">
                    <infs:WclButton runat="server" ID="btnPrevious" CssClass="padRight2 marginTop30" Icon-PrimaryIconCssClass="rbPrevious" Visible="false" 
                        Text="<%$ Resources:Language, PREVIOUS %>" OnClick="btnPrevious_Click">
                    </infs:WclButton>
                    <infs:WclButton runat="server" ID="btnNext" CssClass="padRight2 marginTop30" Icon-PrimaryIconCssClass="rbNext" Visible="true" ValidationGroup="grpFormSubmit"
                        Text="<%$ Resources:Language, NEXT %>" OnClick="btnNext_Click">
                    </infs:WclButton>
                    <infs:WclButton runat="server" ID="btnNext2" CssClass="padRight2 marginTop30" Icon-PrimaryIconCssClass="rbNext" Visible="false"
                        Text="<%$ Resources:Language, NEXT %>" OnClick="btnNext2_Click">
                    </infs:WclButton>
                    <infs:WclButton ID="btnCancelOrder" runat="server" Text="<%$ Resources:Language, CNCL %>" CssClass="cancelposition padRight2 marginTop30"
                        AutoPostBack="true" OnClick="btnCancel_Click">
                        <Icon PrimaryIconCssClass="rbCancel" />
                    </infs:WclButton>
                </div>

            </asp:Panel>
            <asp:HiddenField ID="hdnIsLocationTenant" runat="server" />
            <asp:HiddenField ID="hdnSelectedMailingOptionPrice" runat="server" />
            <asp:HiddenField ID="hdnSelectedMailingOptionId" runat="server" />
        </div>
        <div class="gclr">
        </div>
    </div>
</div>
<script type="text/javascript">

    function returnToParent() {


        var oArg = {};
        oArg.Action = "Submit";

        var oWnd = GetRadWindow();
        oWnd.Close(oArg);
    }

    function GetRadWindow() {
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }

    function openCmbBoxOnTab(sender, e) {
        if (!sender.get_dropDownVisible()) sender.showDropDown();
    }

    function returnToParent() {
        var oArg = {};
        oArg.Action = "Submit";

        var oWnd = GetRadWindow();
        oWnd.Close(oArg);
    }

    function GetRadWindow() {
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }

</script>
