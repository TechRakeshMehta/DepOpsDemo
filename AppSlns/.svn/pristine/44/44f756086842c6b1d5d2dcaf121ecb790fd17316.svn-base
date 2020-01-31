<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ReconciliationItemDataEditMode" CodeBehind="ReconciliationItemDataEditMode.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="ItemDescExplanation" Src="ItemDescriptionExplanation.ascx" %>
<%--<%@ Register Src="VerificationDetailsDocumentConrol.ascx" TagName="VerificationDetailsDocumentConrol"
    TagPrefix="uc3" %>--%>
<%@ Register Src="VerificationDocumentControlReadOnlyMode.ascx" TagName="VerificationDocumentControlReadOnlyMode"
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

    .literalClassGreen {
        background-color: #32CD32;
        padding: 2px;
        border: solid!important;
        border-color: black!important;
        border-width: 1px!important;
        height: 25px!important;
    }

    .literalClassYellow {
        background-color: #FFFF33;
        padding: 2px;
        border: solid!important;
        border-color: black!important;
        border-width: 1px!important;
        height: 25px!important;
    }

    .resizetxtbox {
        resize: vertical !important;
        width: 100% !important;
        height: 100%;
        border-width: 1px !important;
        border-color: #6788be !important;
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
                runat="server"/>
                <asp:Image ID="imageAutoApprove" ImageUrl="~/Resources/Mod/Compliance/icons/Auto-Approve.png" Visible="false" Style="vertical-align: text-bottom;"
                runat="server"/>
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
                <div id="divStatus" runat="server">
                </div>
                <asp:Repeater ID="rpAttributes" runat="server" OnItemDataBound="rpAttributes_ItemDataBound">
                    <ItemTemplate>
                        <div id="divMainDiv" style="margin-top: 20px;" runat="server">
                        </div>
                        <div class='sxro sx1co'>
                            <div class='sxlb'>
                                <span class="cptn">
                                    <asp:Literal ID="litLabel" Text='<%# String.IsNullOrEmpty(Convert.ToString (Eval("AttributeLabel"))) ? INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("AttributeName"))): INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("AttributeLabel"))) %>'
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
                                <asp:Label ID="lblViewDoc" Visible="false" runat="server" />
                                <div id="divScreeningDocuments" runat="server"></div>
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
                <div class='sxro sx1co' style="margin-top: 20px;">
                    <div class='sxlb'>
                        <span class="cptn">Documents</span>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <!-- Document repater-->
                <uc3:VerificationDocumentControlReadOnlyMode ID="ucVerificationDocumentControlReadOnlyMode" IsException="false"
                    runat="server">
                </uc3:VerificationDocumentControlReadOnlyMode>
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
                    <%--<infs:WclTextBox runat="server" ID="txtVerificationComments" ReadOnly="true" TextMode="MultiLine"
                        Height="50px">
                    </infs:WclTextBox>--%>
                    <asp:TextBox runat="server" ID="txtVerificationComments" TextMode="MultiLine" CssClass="resizetxtbox" ReadOnly="true">
                    </asp:TextBox>
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
                        <asp:RadioButtonList ID="rbtnListAction" runat="server" RepeatDirection="Horizontal" onclick="OnRadioButtonSelect(this)"
                            CssClass="radio-list">
                        </asp:RadioButtonList>
                        <div class='sxroend'>
                        </div>
                    </div>
                     <!--UAT-3951: Addition of option to use preset ADB Admin rejection notes -->
                       <!--Do not change the HTML structure of dvRejectionReason, if changed then verif the client methods of dropdown-->
                    <div id="dvRejectionReason" runat="server" style="display: none;" class='sxro monly'>
                        <div class='sxlb'>
                            <span class="cptn">Rejection Reason</span>
                        </div>
                        <br />
                        <infs:WclComboBox ID="cmbRejectionReason" runat="server" MarkFirstMatch="true" Width="100%" AutoPostBack="false"
                            DataTextField="RR_Name" CssClass="form-control" DataValueField="RR_ID" OnItemDataBound="cmbRejectionReason_ItemDataBound"
                            OnClientSelectedIndexChanged="OnSelectedIndexChange" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" />
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro monly'>
                        <div class='sxlb'>
                            <span class="cptn">Write a Note</span>
                        </div>
                        <%--<infs:WclTextBox runat="server" ID="txtAdminNote" TextMode="MultiLine" Height="50px">
                        </infs:WclTextBox>--%>
                        <asp:TextBox runat="server" ID="txtAdminNote" TextMode="MultiLine" CssClass="resizetxtbox">
                        </asp:TextBox>
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
     <!--[SS]:UAT-3951:Addition of option to use preset ADB Admin rejection notes-->
    <asp:HiddenField ID="hdnSelectedRejectionReasonIDs" runat="server" />
</div>
<script type="text/javascript">

    function setTheValueInTextBox(obj) {
        var selectedValue = obj._value;
        var hiddenFieldTextBox = $jQuery(obj._element).siblings('[name$=hdnoptionComboValue]');
        hiddenFieldTextBox.val(selectedValue);
    }

    function OnItemNameClick(obj) {
        var unifiedDocumentStartPageID = $jQuery(obj).attr('UnifiedDocumentStartPageID');
        ChangePdfDocVwrScroll(unifiedDocumentStartPageID);
        return false;
    }

    function ViewScreeningDocument(tenantId, documentId) {
        var popupWindowName = "View Document Window";
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/ComplianceOperations/Pages/FormViewer.aspx?tenantId=" + tenantId + "&documentId=" + documentId + "&IsApplicantDocument=" + "true");
        var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close },
                                            function () {
                                                this.set_title(popupWindowName);
                                                this.set_destroyOnClose(true);
                                            });
        return false;
    }

    /*[SS]:UAT-3951:Addition of option to use preset ADB Admin rejection notes*/
    function OnRadioButtonSelect(event) {

        var id = "[id$=" + event.id + "]";
        var rejectionReasonDivID = event.id.replace("rbtnListAction", "dvRejectionReason");
        var rdButtonListId = "#" + event.id + " input[type=radio]:checked";
        var selectedvalue = $jQuery(rdButtonListId)[0].value;

        var adminLoggedIn = "True";
        if (selectedvalue == '<%= NotApprovedStatusCode %>' && adminLoggedIn == '<%= IsAdminLoggedIn %>') {
            $jQuery("[id$=" + rejectionReasonDivID + "]")[0].style.display = "block";
        }
        else { $jQuery("[id$=" + rejectionReasonDivID + "]")[0].style.display = "none"; }
    }

    function OnSelectedIndexChange(sender, args) {
        if (sender._value > 0) {
            var selectedReason = args._item._attributes._data.RR_ReasonText;
            var txtAdminNoteID = sender._element.parentElement.id.replace("dvRejectionReason", "txtAdminNote");
            var txtAdminNote = $jQuery("[id$=" + txtAdminNoteID + "]")[0];
            var updatedNote = "";

            if (txtAdminNote.value != "") {
                updatedNote = txtAdminNote.value + "\n" + selectedReason;
            }
            else {
                updatedNote = selectedReason;
            }

            txtAdminNote.value = updatedNote;
            var hdnSelectedRejectionReasonIDs = sender._element.parentElement.id.replace("dvRejectionReason", "hdnSelectedRejectionReasonIDs");
            var hdnSelectedRejectionReasonIDs = $jQuery("[id$=" + hdnSelectedRejectionReasonIDs + "]")[0];
            if (hdnSelectedRejectionReasonIDs.value != "") {
                hdnSelectedRejectionReasonIDs.value = hdnSelectedRejectionReasonIDs.value + "," + sender._value;
            }
            else { hdnSelectedRejectionReasonIDs.value = sender._value; }
        }
    }
</script>

