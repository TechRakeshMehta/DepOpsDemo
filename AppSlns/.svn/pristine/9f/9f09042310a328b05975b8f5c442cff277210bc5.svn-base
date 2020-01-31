<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ItemDataReadOnlyMode" CodeBehind="ItemDataReadOnlyMode.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="ItemDescExplanation" Src="ItemDescriptionExplanation.ascx" %>
<%@ Register Src="VerificationDocumentControlReadOnlyMode.ascx" TagName="VerificationDocumentControlReadOnlyMode"
    TagPrefix="uc3" %>
<asp:Panel ID="pnlNoItemData" runat="server" CssClass="section divNoItemDataRO">
    <h1 class="mhdr nocolps">
        <a runat="server" style="text-decoration: none;" href="javascript:void(0);return false;" id="lnkItemName" onclick="OnItemNameClick(this);">
            <asp:Literal ID="litItemName" runat="server"></asp:Literal>
             <asp:Image ID="imageSDEdisabled" ImageUrl="~/Resources/Mod/Compliance/icons/ExceptionsOffIcon-D.png" Visible="false" Style="vertical-align: text-bottom;"
                runat="server"/>
            <asp:Image ID="imageAutoApprove" ImageUrl="~/Resources/Mod/Compliance/icons/Auto-Approve.png" Visible="false" Style="vertical-align: text-bottom;"
                runat="server"/>
        </a>
        <div class="sec_cmds">
            <span id="spniHelp" class="bar_icon ihelp" title="View explanatory notes"></span>
        </div>
    </h1>
    <div class="content">
        <infsu:ItemDescExplanation ID="ucExplanationDescription" runat="server"></infsu:ItemDescExplanation>
        <div class="sxform auto">
            <div class="sxpnl">
                <div class='sxro sx1co'>
                    <div class='sxlb' title="The current verification status for this Item">
                        <span class="cptn">Current Status</span>
                    </div>
                    <div class='sxlm'>
                        <span class="ronly">
                            <asp:Literal ID="litStatusNoData" Text="Incomplete" runat="server"></asp:Literal>
                        </span>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div id="divItemPaymentPanel" runat="server" style="display: none;">
                    <div class='sxro sx1co'>
                        <div class='sxlb' title="The amount of item to be paid.">
                            <span class="cptn">Amount</span>
                        </div>
                        <div class='sxlm'>
                            <span class="ronly">
                                <asp:Literal ID="litItemAmount" runat="server"></asp:Literal></span>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx1co'>
                        <div class='sxlb' title="Item payment current status">
                            <span class="cptn">Payment Status</span>
                        </div>
                        <div class='sxlm'>
                            <span class="ronly">
                                <asp:Literal ID="litItemPaymentStatus" runat="server"></asp:Literal></span>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="pnlExceptionData" runat="server" CssClass="section divExceptionDataRO">
    <h1 class="mhdr">
        <asp:Literal ID="litItemNameExceptionData" runat="server"></asp:Literal>
        <div class="sec_cmds">
            <span id="spniHelp" class="bar_icon ihelp" title="View explanatory notes"></span>
        </div>
    </h1>
    <div class="content">
        <infsu:ItemDescExplanation ID="ItemDescExplanation1" runat="server"></infsu:ItemDescExplanation>
        <div id="itemExceptionDataSubmissionDate">
            <span style="color: #bd6a38; font-weight: 700; word-spacing: 2px;" class="cptn">Submission Date</span>
            <asp:Label ID="lblExceptionDataSubmissionDate" runat="server"></asp:Label>
        </div>
        <div class="sxform auto">
            <div class="sxpnl">
                <div class='sxro sx1co'>
                    <div class='sxlb' title="The current verification status for this Item">
                        <span class="cptn">Current Status</span>
                    </div>
                    <div class='sxlm'>
                        <span class="ronly">
                            <asp:Literal ID="litStatusExceptionApproved" runat="server"></asp:Literal>
                        </span>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                 <div id="divExpirationDate" runat="server" class='sxro sx1co' style="display: none">
                    <div class='sxlb'>
                        <span class="cptn">Expiration Date</span>
                    </div>
                    <div class='sxlm monly'>
                        <infs:WclDatePicker ID="dpExpirationDate" enabled="false" runat="server"  DateInput-EmptyMessage="Select a date">
                        </infs:WclDatePicker>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro monly'>
                    <div class='sxlb'>
                        <span class="cptn">Exception Reason</span>
                    </div>
                    <div class='sxroend'>
                    </div>
                    <span class="ronly">
                        <asp:Literal ID="litExceptionReason" runat="server"></asp:Literal>
                    </span>
                </div>
                <uc3:VerificationDocumentControlReadOnlyMode ID="ucReadOnlyModeException" IsException="true"
                    runat="server"></uc3:VerificationDocumentControlReadOnlyMode>
                <div class='sxro monly'>
                    <div class='sxlb'>
                        <span class="cptn">Comments</span>
                    </div>
                    <infs:WclTextBox Enabled="false" runat="server" ID="txtAdminNotes" TextMode="MultiLine"
                        Height="50px" Width="125">
                    </infs:WclTextBox>
                    <div class='sxroend'>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="pnlItemData" runat="server" Visible="false" CssClass="section divItemDataRO">
    <h1 class="mhdr">
        <asp:Literal ID="liteItemNameItemData" runat="server"></asp:Literal>
        <div class="sec_cmds">
            <span id="spniHelp" class="bar_icon ihelp" title="View explanatory notes"></span>
        </div>
    </h1>
    <div class="content">
        <infsu:ItemDescExplanation ID="ItemDescExplanation2" runat="server"></infsu:ItemDescExplanation>
        <div id="itemDataSubmissionDate">
            <span style="color: #bd6a38; font-weight: 700; word-spacing: 2px;" class="cptn">Submission Date</span>
            <asp:Label ID="lblItemDataSubmissionDate" runat="server"></asp:Label>
        </div>
        <div id="pnlItem" class="sxform auto" runat="server">
            <div class="sxpnl">
                <div class='sxro sx1co'>
                    <div class='sxlb' title="The current verification status for this Item">
                        <span class="cptn">Current Status</span>
                    </div>
                    <div class='sxlm'>
                        <span class="ronly">
                            <asp:Literal ID="litStatusDataEntered" runat="server"></asp:Literal>
                        </span>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro monly'>
                    <div class='sxlb'>
                        <span class="cptn">Comments</span>
                    </div>
                    <infs:WclTextBox Enabled="false" runat="server" ID="txtCommentsDataEntered" TextMode="MultiLine"
                        Height="50px" Width="125">
                    </infs:WclTextBox>
                    <div class='sxroend'>
                    </div>
                </div>

                <asp:Repeater ID="rpAttributes" runat="server" OnItemDataBound="rpAttributes_ItemDataBound">
                    <ItemTemplate>
                        <div class='sxro sx1co'>
                            <div class='sxlb'>
                                <span class="cptn">
                                    <asp:Literal ID="litLabel" Text='<%# String.IsNullOrEmpty(Convert.ToString (Eval("AttributeLabel"))) ? INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("AttributeName"))): INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("AttributeLabel"))) %>'
                                        runat="server"></asp:Literal></span>
                            </div>
                            <div class='sxlm'>
                                <span class="ronly">
                                    <asp:Literal ID="litAttributeValues" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("AttributeValue"))) %>' runat="server"></asp:Literal>
                                    <asp:Literal ID="litOptionsText" runat="server"></asp:Literal>
                                    <div id="divScreeningDocuments" runat="server" visible="false"></div>
                                </span>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <uc3:VerificationDocumentControlReadOnlyMode ID="ucReadOnlyMode" IsException="false"
                    runat="server"></uc3:VerificationDocumentControlReadOnlyMode>
            </div>
        </div>
    </div>
</asp:Panel>
<script type="text/javascript">
    function OnItemNameClick(obj) {
        var unifiedDocumentStartPageID = $jQuery(obj).attr('UnifiedDocumentStartPageID');
        if (unifiedDocumentStartPageID > 0) {
            ChangePdfDocVwrScroll(unifiedDocumentStartPageID);
        }
        return false;
    }

</script>
