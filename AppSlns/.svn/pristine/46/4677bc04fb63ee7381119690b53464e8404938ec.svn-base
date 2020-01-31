<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ItemDataExceptionMode" CodeBehind="ItemDataExceptionMode.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="ItemDescExplanation" Src="ItemDescriptionExplanation.ascx" %>
<%@ Register Src="VerificationDetailsDocumentConrol.ascx" TagName="VerificationDetailsDocumentConrol"
    TagPrefix="uc3" %>
<%--<script language="javascript" type="text/javascript">
    //    Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
    //    Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };

    $jQuery(document).ready(function () {

        showHideButton(false);
        // $jQuery("[id$=divNoPreview]").show();
    });

    function onFileUploaded(sender, args) {
        if (sender.getUploadedFiles() != "") {
            showHideButton(true);
        }
    }

    function onFileRemoved(sender, args) {
        if (sender.getUploadedFiles() == "") {
            showHideButton(false)
        }
    }

    function showHideButton(visible) {
        if (visible) {
            $jQuery("[id$=btnUpload]").show();
            $jQuery("[id$=btnCancelUpload]").show();
        }
        else {
            $jQuery("[id$=btnUpload]").hide();
            $jQuery("[id$=btnCancelUpload]").hide();
        }
    }
    function PreventBackspace(e) {
        var evt = e || window.event;
        if (evt) {
            var keyCode = evt.charCode || evt.keyCode;
            if (keyCode === 8) {
                if (evt.preventDefault) {
                    evt.preventDefault();
                } else {
                    evt.returnValue = false;
                }
            }
        }
    }
</script>--%>

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
<div class="section divExceptionMode" id="divExceptionMode" runat="server">
    <h1 class="mhdr nocolps">
        <div id="dvlnkItemName" runat="server">
            <a runat="server" style="text-decoration: none;" href="javascript:void(); return false;" id="lnkItemName" onclick="OnItemNameClick(this);" class="item_lslnk">
                <asp:Literal ID="litItemName" runat="server"></asp:Literal>
                <asp:Image ID="imageExceptionOff" ImageUrl="~/Resources/Mod/Compliance/icons/ExceptionsOffIcon.png" Visible="false" Style="vertical-align: text-bottom;"
                    runat="server" />
                <asp:Image ID="imageSDEdisabled" ImageUrl="~/Resources/Mod/Compliance/icons/ExceptionsOffIcon-D.png" Visible="false" Style="vertical-align: text-bottom;"
                    runat="server" />
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
        <div id="dvDetailPanel" runat="server" class="sxform auto">
            <div class="sxpnl">
                <asp:CheckBox ID="chkDeleteItem" runat="server" Text="Delete Item"></asp:CheckBox>
                <div class='sxro sx1co'>
                    <div class='sxlb' title="The current verification status for this Item">
                        <span class="cptn">Current Status</span>
                    </div>
                    <div class='sxlm'>
                        <span class="ronly">
                            <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                        </span>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx1co' id="dvItemsList" runat="server">
                    <div class='sxlb'>
                        <span class="cptn">Item</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbItems" runat="server" AutoPostBack="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div id="divExpiDateReadOnly" runat="server" style="display: none;">
                    <div class='sxro sx1co'>
                        <div class='sxlb'>
                            <span class="cptn">Expiration Date</span>
                        </div>
                        <div class='sxlm monly'>
                            <infs:WclDatePicker ID="dpExpiDateReadOnly" runat="server" Enabled="false" DateInput-EmptyMessage="Select a date">
                            </infs:WclDatePicker>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </div>
                <div id="divExpirationDate" runat="server" class='sxro sx1co' style="display: none">
                    <div class='sxlb'>
                        <span class="cptn">Expiration Date</span>
                    </div>
                    <div class='sxlm '>
                        <infs:WclDatePicker ID="dpExpirationDate" runat="server" DateInput-EmptyMessage="Select a date">
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
                    <%--<asp:Literal ID="litExceptionReason" runat="server"></asp:Literal>--%>
                </div>
                <uc3:VerificationDetailsDocumentConrol ID="ucVerificationDetailsDocumentConrol" IsException="true"
                    runat="server"></uc3:VerificationDetailsDocumentConrol>
                <div class='sxro monly'>
                    <div class='sxlb' title="Admin comments for this Item are displayed below">
                        <span class="cptn" id="spnComments" runat="server">Admin's Comment History</span>
                    </div>
                    <%--UAT-2407: Allow copying text in complio admin screens from updated chrome version--%>
                    <%--<infs:WclTextBox runat="server" ID="txtVerificationComments" ReadOnly="true" TextMode="MultiLine"
                        Height="150px">
                    </infs:WclTextBox>--%>
                    <asp:TextBox runat="server" ID="txtVerificationComments" TextMode="MultiLine" CssClass="resizetxtbox" ReadOnly="true">
                    </asp:TextBox>
                    <div class='sxroend'>
                    </div>
                </div>
                <div id="dvAdminActions" runat="server">
                    <h1 class="shdr">Administrator Actions</h1>
                    <div class='sxro monly'>
                        <div class='sxlb'>
                            <span class="cptn">Change Status</span>
                        </div>
                        <br />
                        <asp:RadioButtonList ID="rbtnActions" runat="server" onclick="OnRadioButtonClick(this)" RepeatDirection="Horizontal">
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
                            OnClientSelectedIndexChanged="OnSelectedIndexChangeExcep" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" />
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro monly'>
                        <div class='sxlb'>
                            <span class="cptn">Write a Note</span>
                        </div>
                        <%--<infs:WclTextBox runat="server" ID="txtAdminNotes" TextMode="MultiLine" Height="150px">
                        </infs:WclTextBox>--%>
                        <asp:TextBox runat="server" ID="txtAdminNotes" TextMode="MultiLine" CssClass="resizetxtbox">
                        </asp:TextBox>
                        <div class='sxroend'>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdfApplicantItemDataId" runat="server" />
    <asp:HiddenField ID="hdfOrganizationUserId" runat="server" />
    <asp:HiddenField ID="hdfItemExceptionStatus" runat="server" />
    <asp:HiddenField ID="hdfRejectionCodeException" runat="server" />
    <asp:HiddenField ID="hdnExpirationDateDB" runat="server" />
    <!--[SS]:UAT-3951:Addition of option to use preset ADB Admin rejection notes-->
    <asp:HiddenField ID="hdnSelectedRejectionReasonIDsExcep" runat="server" />
</div>
<script type="text/javascript">
    function OnItemNameClick(obj) {
        var unifiedDocumentStartPageID = $jQuery(obj).attr('UnifiedDocumentStartPageID');
        ChangePdfDocVwrScroll(unifiedDocumentStartPageID);
        return true;
    }

    function OnRadioButtonClick(event) {
        var id = "[id$=" + event.id + "]";
        var rdButtonListId = "#" + event.id + " input[type=radio]:checked";
        var actionId = $jQuery(id)[0].getAttribute("exActionItemId");
        var divExpirationId = "[id$=divExpirationDate" + actionId + "]";
        var divExpiDateReadOnly = "[id$=divExpiDateReadOnly" + actionId + "]";
        var dpExpirationDate = "[id$=dpExpirationDate" + actionId + "]";
        var hdnExpirationDateDB = "[id$=hdnExpirationDateDB" + actionId + "]";
        var selectedvalue = $jQuery(rdButtonListId)[0].value;
        var dbExpirationdate = $jQuery(hdnExpirationDateDB)[0].value;
        if (selectedvalue == '<%= ApprovedWithExcepStatusCode %>') {
            if (dbExpirationdate != null && dbExpirationdate != "") {
                var dbDate = new Date(dbExpirationdate);
                $jQuery(dpExpirationDate)[0].control.set_selectedDate(dbDate);
            }
            $jQuery(divExpirationId)[0].style.display = "block";
            $jQuery(divExpiDateReadOnly)[0].style.display = "none";
        }
        else {
            $jQuery(dpExpirationDate)[0].control.set_selectedDate(null);
            $jQuery(divExpirationId)[0].style.display = "none";
            $jQuery(divExpiDateReadOnly)[0].style.display = "none";
        }

        var adminLoggedIn = "True";
        var rejectionReasonDivID = event.id.replace("rbtnActions", "dvRejectionReason");
        if (selectedvalue == '<%= NotApprovedStatusCode %>' && adminLoggedIn == '<%= IsAdminLoggedIn %>') {
            $jQuery("[id$=" + rejectionReasonDivID + "]")[0].style.display = "block";
        }
        else { $jQuery("[id$=" + rejectionReasonDivID + "]")[0].style.display = "none"; }
    }


    function SetMinDate(sender, args) {
        var minDate = new Date('<%=MinExpirationDate%>');
        var datePickerId = "[id$=" + sender._element.id + "]";
        var date = $jQuery(datePickerId)[0].control.get_selectedDate();
        if (date != null) {
            sender.set_minDate(date);
        }
        else {
            sender.set_minDate(minDate);
        }
    }

    /*[SS]:UAT-3951:Addition of option to use preset ADB Admin rejection notes*/
    function OnSelectedIndexChangeExcep(sender, args) {
        if (sender._value > 0) {
            var selectedReason = args._item._attributes._data.RR_ReasonText;
            var txtAdminNoteID = sender._element.parentElement.id.replace("dvRejectionReason", "txtAdminNotes");
            var txtAdminNote = $jQuery("[id$=" + txtAdminNoteID + "]")[0];
            var updatedNote = "";

            if (txtAdminNote.value != "") {
                updatedNote = txtAdminNote.value + "\n" + selectedReason;
            }
            else {
                updatedNote = selectedReason;
            }

            txtAdminNote.value = updatedNote;
            var hdnSelectedRejectionReasonIDsExcep = sender._element.parentElement.id.replace("dvRejectionReason", "hdnSelectedRejectionReasonIDsExcep");
            var hdnSelectedRejectionReasonIDsExcep = $jQuery("[id$=" + hdnSelectedRejectionReasonIDsExcep + "]")[0];
            if (hdnSelectedRejectionReasonIDsExcep.value != "") {
                hdnSelectedRejectionReasonIDsExcep.value = hdnSelectedRejectionReasonIDsExcep.value + "," + sender._value;
            }
            else { hdnSelectedRejectionReasonIDsExcep.value = sender._value; }
        }
    }

</script>
