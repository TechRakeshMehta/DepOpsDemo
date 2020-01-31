<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ReconciliationItemDataReadOnlyMode" CodeBehind="ReconciliationItemDataReadOnlyMode.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="ItemDescExplanation" Src="ItemDescriptionExplanation.ascx" %>
<%@ Register Src="VerificationDocumentControlReadOnlyMode.ascx" TagName="VerificationDocumentControlReadOnlyMode"
    TagPrefix="uc3" %>
<asp:Panel ID="pnlNoItemData" runat="server" CssClass="section divNoItemDataRO">
    <h1 class="mhdr nocolps">
        <a runat="server" style="text-decoration: none;" href="javascript:void(0);return false;" id="lnkItemName" onclick="OnItemNameClick(this);">
            <asp:Literal ID="litItemName" runat="server"></asp:Literal>
        </a>
        <div class="sec_cmds">
            <span id="spniHelp" class="bar_icon ihelp" title="View explanatory notes"></span>
        </div>
        <h1></h1>
        <div class="content">
            <infsu:ItemDescExplanation ID="ucExplanationDescription" runat="server" />
            <div class="sxform auto">
                <div class="sxpnl">
                    <div class="sxro sx1co">
                        <div class="sxlb" title="The current verification status for this Item">
                            <span class="cptn">Current Status</span>
                        </div>
                        <div class="sxlm">
                            <span class="ronly">
                            <asp:Literal ID="litStatusNoData" runat="server" Text="Incomplete"></asp:Literal>
                            </span>
                        </div>
                        <div class="sxroend">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </h1>
</asp:Panel>
<asp:Panel ID="pnlExceptionData" runat="server" CssClass="section divExceptionDataRO">
    <h1 class="mhdr">
        <asp:Literal ID="litItemNameExceptionData" runat="server"></asp:Literal>
        <div class="sec_cmds">
            <span id="Span1" class="bar_icon ihelp" title="View explanatory notes"></span>
        </div>
        <h1></h1>
        <div class="content">
            <infsu:ItemDescExplanation ID="ItemDescExplanation1" runat="server" />
            <div id="itemExceptionDataSubmissionDate">
                <span class="cptn" style="color: #bd6a38; font-weight: 700; word-spacing: 2px;">Submission Date</span>
                <asp:Label ID="lblExceptionDataSubmissionDate" runat="server"></asp:Label>
            </div>
            <div class="sxform auto">
                <div class="sxpnl">
                    <div class="sxro sx1co">
                        <div class="sxlb" title="The current verification status for this Item">
                            <span class="cptn">Current Status</span>
                        </div>
                        <div class="sxlm">
                            <span class="ronly">
                            <asp:Literal ID="litStatusExceptionApproved" runat="server"></asp:Literal>
                            </span>
                        </div>
                        <div class="sxroend">
                        </div>
                    </div>
                    <div id="divExpirationDate" runat="server" class="sxro sx1co" style="display: none">
                        <div class="sxlb">
                            <span class="cptn">Expiration Date</span>
                        </div>
                        <div class="sxlm monly">
                            <infs:WclDatePicker ID="dpExpirationDate" runat="server" DateInput-EmptyMessage="Select a date" enabled="false">
                            </infs:WclDatePicker>
                        </div>
                        <div class="sxroend">
                        </div>
                    </div>
                    <div class="sxro monly">
                        <div class="sxlb">
                            <span class="cptn">Exception Reason</span>
                        </div>
                        <div class="sxroend">
                        </div>
                        <span class="ronly">
                        <asp:Literal ID="litExceptionReason" runat="server"></asp:Literal>
                        </span>
                    </div>
                    <uc3:VerificationDocumentControlReadOnlyMode ID="ucReadOnlyModeException" runat="server" IsException="true" />
                    <div class="sxro monly">
                        <div class="sxlb">
                            <span class="cptn">Comments</span>
                        </div>
                        <infs:WclTextBox ID="txtAdminNotes" runat="server" Enabled="false" Height="50px" TextMode="MultiLine" Width="125">
                        </infs:WclTextBox>
                        <div class="sxroend">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </h1>
</asp:Panel>
<asp:Panel ID="pnlItemData" runat="server" Visible="false" CssClass="section divItemDataRO">
    <h1 class="mhdr">
        <asp:Literal ID="liteItemNameItemData" runat="server"></asp:Literal>
         <asp:Image ID="imageAutoApprove" ImageUrl="~/Resources/Mod/Compliance/icons/Auto-Approve.png" Visible="false" Style="vertical-align: text-bottom;"
                runat="server"/>
        <div class="sec_cmds">
            <span id="Span2" class="bar_icon ihelp" title="View explanatory notes"></span>
        </div>
        <h1></h1>
        <div class="content">
            <infsu:ItemDescExplanation ID="ItemDescExplanation2" runat="server" />
            <div id="itemDataSubmissionDate">
                <span class="cptn" style="color: #bd6a38; font-weight: 700; word-spacing: 2px;">Submission Date</span>
                <asp:Label ID="lblItemDataSubmissionDate" runat="server"></asp:Label>
            </div>
            <div id="pnlItem" runat="server" class="sxform auto">
                <div class="sxpnl">
                    <div class="sxro sx1co">
                        <div class="sxlb" title="The current verification status for this Item">
                            <span class="cptn">Current Status</span>
                        </div>
                        <div class="sxlm">
                            <span class="ronly">
                            <asp:Literal ID="litStatusDataEntered" runat="server"></asp:Literal>
                            </span>
                        </div>
                        <div class="sxroend">
                        </div>
                    </div>
                    <div class="sxro monly">
                        <div class="sxlb">
                            <span class="cptn">Comments</span>
                        </div>
                        <infs:WclTextBox ID="txtCommentsDataEntered" runat="server" Enabled="false" Height="50px" TextMode="MultiLine" Width="125">
                        </infs:WclTextBox>
                        <div class="sxroend">
                        </div>
                    </div>
                    <asp:Repeater ID="rpAttributes" runat="server" OnItemDataBound="rpAttributes_ItemDataBound">
                        <ItemTemplate>
                            <div class="sxro sx1co">
                                <div class="sxlb">
                                    <span class="cptn">
                                    <asp:Literal ID="litLabel" runat="server" Text=' <%# String.IsNullOrEmpty(Convert.ToString (Eval("AttributeLabel"))) ? INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("AttributeName"))):
  INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("AttributeLabel"))) %>'></asp:Literal>
                                    </span>
                                </div>
                                <div class="sxlm">
                                    <span class="ronly">
                                    <asp:Literal ID="litAttributeValues" runat="server" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("AttributeValue"))) %>'></asp:Literal>
                                    <asp:Literal ID="litOptionsText" runat="server"></asp:Literal>
                                    <div id="divScreeningDocuments" runat="server">
                                    </div>
                                    </span>
                                </div>
                                <div class="sxroend">
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <uc3:VerificationDocumentControlReadOnlyMode ID="ucReadOnlyMode" runat="server" IsException="false" />
                </div>
            </div>
        </div>
        <h1></h1>
    </h1>
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
