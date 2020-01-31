<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ReconciliationItemDataReviewer" CodeBehind="ReconciliationItemDataReviewer.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="ItemDescExplanation" Src="ItemDescriptionExplanation.ascx" %>
<%@ Register Src="VerificationDetailsDocumentConrol.ascx" TagName="VerificationDetailsDocumentConrol"
    TagPrefix="uc3" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">
    .item_highlight {
        color: red !important;
    }

    .highlightAssignedItem {
        padding: 3px 3px 3px 3px;
        background-color: #b2b2b2;
    }

    .highlightDetailBorder {
        border: 3px solid Gray;
    }

    label {
        font-size: 11px !important;
    }
</style>

<div class="msgbox">
    <asp:Label ID="lblMessage" runat="server">
    </asp:Label>
</div>
<div class="section divPendingReview" id="divEditMode" runat="server">
    <h1 class="mhdr nocolps">
        <div id="dvlnkItemName" runat="server">
            <a runat="server" style="text-decoration: none;" href="javascript:void(0);return false;" id="lnkItemName" onclick="OnItemNameClick(this);" class="item_lslnk">
                <asp:Literal ID="litItemName" runat="server"></asp:Literal>
                <asp:Image ID="imageExceptionOff" ImageUrl="~/Resources/Mod/Compliance/icons/ExceptionsOffIcon.png" Visible="false" Style="vertical-align: text-bottom;"
                    runat="server" />
                <asp:Image ID="imageSDEdisabled" ImageUrl="~/Resources/Mod/Compliance/icons/ExceptionsOffIcon-D.png" Visible="false" Style="vertical-align: text-bottom;"
                    runat="server" />
            </a>
        </div>
        <div class="sec_cmds">
            <span id="spniHelp" class="bar_icon ihelp" title="View explanatory notes"></span>
        </div>

    </h1>

    <div class="content">
        <infsu:ItemDescExplanation ID="ucExplanationDescription" runat="server"></infsu:ItemDescExplanation>
        <div id="itemSubmissionDate">
            <span style="color: #bd6a38; font-weight: 700; word-spacing: 2px;" class="cptn">Submission Date</span>
            <asp:Label ID="lblSubmissionDate" runat="server"></asp:Label>
        </div>
        <div id="dvDetailPanel" class="sxform auto" runat="server">
            <div class="sxpnl">
                <asp:CheckBox ID="chkDeleteItem" runat="server" Text="Delete Item"></asp:CheckBox>
                <div class='sxro sx1co'>
                    <div class='sxlb' title="The current verification status for this Item">
                        <span class="cptn">Current Status</span>
                    </div>
                    <div class='sxlm'>
                        <span class="ronly">
                            <asp:Literal ID="litStatus" runat="server"></asp:Literal></span>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <asp:Repeater ID="rpAttributes" runat="server" OnItemDataBound="rpAttributes_ItemDataBound">
                    <ItemTemplate>
                        <div class='sxro sx1co'>
                            <div class='sxlb'>
                                <span class="cptn">
                                    <asp:Literal ID="litLabel" Text='<%# String.IsNullOrEmpty(Convert.ToString (Eval("AttributeLabel"))) ? INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("AttributeName"))):
                                                                                                                                            INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("AttributeLabel"))) %>'
                                        runat="server"></asp:Literal></span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox ID="txtDataType" runat="server" Visible="false">
                                </infs:WclTextBox>
                                <infs:WclDatePicker ID="dtPicker" runat="server" Visible="false">
                                </infs:WclDatePicker>
                                <infs:WclTextBox ID="txtBox" runat="server" Visible="false">
                                </infs:WclTextBox>
                                <infs:WclNumericTextBox ID="numericTextBox" runat="server" Visible="false">
                                </infs:WclNumericTextBox>
                                <div id="optionComboDiv">
                                    <infs:WclComboBox ID="optionCombo" runat="server" Visible="false" OnClientSelectedIndexChanged="setTheValueInTextBox">
                                    </infs:WclComboBox>
                                    <asp:TextBox Style="display: none;" ID="hdnoptionComboValue" Value="" runat="server" AutoPostBack="false" OnTextChanged="textchanged" />
                                </div>
                                <%-- <asp:LinkButton ID="btnPreviewDoc" ToolTip="Click here to view the document" Visible="false"
                                    runat="server" Font-Underline="true" BackColor="Transparent" BorderStyle="None">
                                </asp:LinkButton>--%>
                                <asp:Label ID="lblViewDoc" Visible="false" runat="server" />
                                <asp:HiddenField ID="hdfAttributeId" Value='<%# Eval("ComplianceAttributeId") %>'
                                    runat="server" />
                                <asp:HiddenField ID="hdfAttributeDataId" Value='<%# Eval("ApplAttributeDataId") %>'
                                    runat="server" />
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <!-- Document repater-->
                <uc3:VerificationDetailsDocumentConrol ID="ucVerificationDetailsDocumentConrol" IsException="false"
                    runat="server"></uc3:VerificationDetailsDocumentConrol>
                <div class='sxro monly' id="dvApplicantNotes" runat="server">
                    <div class='sxlb' title="Applicant entered description or notes for this Item are displayed below">
                        <span class="cptn">Applicant's Note</span>
                    </div>
                    <div class='sxroend'>
                    </div>
                    <span class="ronly">
                        <asp:Literal ID="litApplicantNotes" runat="server"></asp:Literal>
                    </span>
                </div>
                <div class='sxro monly'>
                    <div class='sxlb' title="Admin comments for this Item are displayed below">
                        <span id="spnCommentsHistory" runat="server" class="cptn">Admin's Comment History</span>
                    </div>
                    <%--UAT-2407: Allow copying text in complio admin screens from updated chrome version--%>
                    <infs:WclTextBox runat="server" ID="txtVerificationComments" ReadOnly="true" TextMode="MultiLine"
                        Height="50px">
                    </infs:WclTextBox>
                    <div class='sxroend'>
                    </div>
                </div>
                <div id="dvCombined" runat="server">
                    <h1 class="shdr">Administrator Actions</h1>
                    <div class='sxro monly'>
                        <div class='sxlb'>
                            <span class="cptn">Change Status</span>
                        </div>
                        <br />
                        <asp:RadioButtonList ID="rbtnListAction" runat="server" RepeatDirection="Horizontal"
                            CssClass="radio-list">
                        </asp:RadioButtonList>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro monly'>
                        <div class='sxlb'>
                            <span class="cptn">Write a Note</span>
                        </div>
                        <infs:WclTextBox runat="server" ID="txtAdminNote" TextMode="MultiLine" Height="50px">
                        </infs:WclTextBox>
                        <div class='sxroend'>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdfReviewerTenantId" runat="server" />
    <asp:HiddenField ID="hdfReviewerTypeId" runat="server" />
    <asp:HiddenField ID="hdfTPReviewerUserId" runat="server" />
    <asp:HiddenField ID="hdfSelectedState" runat="server" />
    <asp:HiddenField ID="hdfItemStatus" runat="server" />
    <asp:HiddenField ID="hdfRejectionCodeItem" runat="server" />
    <asp:HiddenField ID="hdfComplianceItemId" runat="server" />
    <asp:HiddenField ID="hdfFileUploadExists" runat="server" />
    <asp:HiddenField ID="hdfFileUploadAttributeId" runat="server" />
</div>
<script type="text/javascript">

    function setTheValueInTextBox(obj) {
        var selectedValue = obj._value;
        var hiddenFieldTextBox = $jQuery(obj._element).siblings('[name$=hdnoptionComboValue]');
        hiddenFieldTextBox.val(selectedValue);
    }

    function OnItemNameClick(obj) {
        var unifiedDocumentStartPageID = $jQuery(obj).attr('UnifiedDocumentStartPageID');
        //if (unifiedDocumentStartPageID > 0) {
        //    debugger;
        //    parent.pdfDocViewerChildWnd.ChangePDFSetView(unifiedDocumentStartPageID);
        //}
        ChangePdfDocVwrScroll(unifiedDocumentStartPageID);
        return false;
    }

</script>

