<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManagePaymentInstructions.ascx.cs" Inherits="CoreWeb.SystemSetUp.Views.ManagePaymentInstructions" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr">Payment Option Instructions
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPaymentInstruction">
                <div class='sxro sx2co'>
                    <div class='sxlb'>
                        <span class="cptn">Choose Payment Option</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbPaymentOption" DataTextField="Name" DataValueField="PaymentOptionID" runat="server" AutoPostBack="true"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnSelectedIndexChanged="cmbPaymentOption_SelectedIndexChanged">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxlm'>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ID="rfvPaymentInstruction" ControlToValidate="cmbPaymentOption" InitialValue="--SELECT--"
                                class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Payment Option is required." />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx2co'>
                    <div class='sxlb '>
                        <span class="cptn">Instruction Text</span>
                    </div>
                    <div class='sxlm m2spn'>
                        <asp:Panel CssClass="content" ID="pnlRadContent" runat="server" Height="250px">
                            <infs:WclEditor ID="radHTMLEditor" ToolsFile="~/WebSite/Data/Tools.xml" runat="server" EditModes="Preview"
                                Width="98%" Height="242px" OnClientLoad="OnClientLoad" EnableResize="false">
                                <%-- <ImageManager ViewPaths="~/InstitutionImages" UploadPaths="~/InstitutionImages" DeletePaths="~/InstitutionImages" MaxUploadFileSize="7100000" />--%>
                            </infs:WclEditor>
                        </asp:Panel>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div id="divButtonSave" runat="server">
            <div class="sxcbar">
                <div class="sxcmds" style="text-align: right">
                    <infs:WclButton ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_Click" ValidationGroup="grpFormSubmit">
                        <Icon PrimaryIconCssClass="rbEdit" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                        ValidationGroup="grpFormSubmit" Visible="false">
                        <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Visible="false">
                        <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    &nbsp;&nbsp;
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function OnClientLoad(editor, args) {
        $jQuery('ul.reToolbar').width('auto');
    }
</script>
