<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ItemDataLoader" CodeBehind="ItemDataLoader.ascx.cs" %>
<%@ Register TagPrefix="infsu" TagName="ItemDataEditMode" Src="ItemDataEditMode.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ItemDataReadOnly" Src="ItemDataReadOnlyMode.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ItemDataException" Src="ItemDataExceptionMode.ascx" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register Src="VerificationDetailsDocumentConrol.ascx" TagName="VerificationDetailsDocumentConrol"
    TagPrefix="uc3" %>
<%@ Register Src="VerificationDetailsUnassignedDocumentConrol.ascx" TagName="VerificationDetailsUnassignedDocumentConrol"
    TagPrefix="uc4" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxCompliancePackageDetails">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/ComplianceVerification.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    .radioButtonSpace {
        padding-right: 5px;
    }

    .auto .sxro .radio-list label {
        font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        font-size: 12px !important;
    }

    a.cat_lslnk {
        height: auto !important;
    }

    .auto .sxro .radio-list {
        margin-left: 0px;
    }

    #uploadControlDiv td a, #tdPrint a, #mappedDocument td span {
        font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        font-size: 11px !important;
    }

    hr {
        display: block;
        margin: 0.5em 10px;
        border-style: inset;
        border-width: 1px;
        color: #dedede;
        border-color: #dedede;
    }

    .resizetxtbox {
        resize: vertical !important;
        width: 100% !important;
        height: 100%;
        border-width: 1px !important;
        border-color: #6788be !important;
    }
    /*td span {
  font-family: "Segoe UI",Arial,Helvetica,sans-serif;
  font-size: 11px !important;
}*/
</style>

<script type="text/javascript">

    function chkDeleteOverideStatus_Click(val)
    {       
        var litCurrentCategoryVerSts = $jQuery("[id$=litCurrentCategoryVerSts]");
        if (val)
            {
            litCurrentCategoryVerSts.css("text-decoration", "line-through");
            }
        else
            litCurrentCategoryVerSts.css("text-decoration", "");
    };

    function OnSelectedRadioButton(event) {
        var id = "[id$=" + event.id + "]";
        var rdButtonListId = "#" + event.id + " input[type=radio]:checked";
        var selectedvalue = $jQuery(rdButtonListId)[0].value;
        var dbExpirationdate = $jQuery("[id$=hdnExpirationDate]")[0].value;
        if (selectedvalue == '<%= ApprovedWithExcepStatusCode %>') {
            if (dbExpirationdate != null && dbExpirationdate != "") {
                var dbDate = new Date(dbExpirationdate);
                $jQuery("[id$=dpExpiDateEditMode]")[0].control.set_selectedDate(dbDate);
            }
            $jQuery("[id$=divExpiDateEditMode]")[0].style.display = "block";
            $jQuery("[id$=divExpiDateReadOnly]")[0].style.display = "none";
        }
        else {
            $jQuery("[id$=dpExpiDateEditMode]")[0].control.set_selectedDate(null);
            $jQuery("[id$=divExpiDateEditMode]")[0].style.display = "none";
            $jQuery("[id$=divExpiDateReadOnly]")[0].style.display = "none";

        }
        var adminLoggedIn = "True";
        var rejectionReasonDivID = event.id.replace("rbtnActions", "dvRejectionReason");
        if (selectedvalue == '<%= NotApprovedStatusCode %>' && adminLoggedIn == '<%= IsDefaultTenant %>') {
            $jQuery("[id$=" + rejectionReasonDivID + "]")[0].style.display = "block";
        }
        else { $jQuery("[id$=" + rejectionReasonDivID + "]")[0].style.display = "none"; }
    }


    function OnSelectedRadioButtonCatOverride(event) {
        debugger;
        var id = "[id$=" + event.id + "]";
        var rdButtonListId = "#" + event.id + " input[type=radio]:checked";
        var selectedvalue = $jQuery(rdButtonListId)[0].value;
        var dbExpirationdate = $jQuery("[id$=hdnExpirationDate]")[0].value;
        if (selectedvalue == '<%= ApprovedCatOverrideStatusCode %>') {
            if (dbExpirationdate != null && dbExpirationdate != "") {
                var dbDate = new Date(dbExpirationdate);
                $jQuery("[id$=dpExpteCatOverrideEditMode]")[0].control.set_selectedDate(dbDate);
            }
            $jQuery("[id$=divExpiDateCategoryOvrrideEditMode]")[0].style.display = "block";

            //UAT-1635
            $jQuery(".approvepopup").css("display", "block");
            $window.showDialog($jQuery(".approvepopup").clone().show(), {
                approvebtn: {
                    autoclose: true, text: "Approve", click: function () {
                        $jQuery("[id$=rbtnActionsCatOverride] input:checked").prop('checked', true)
                        $jQuery("[id$=rbtnActionsCatOverride] [value='XXXX']").prop('checked', false);
                    }
                }, closeBtn: {
                    autoclose: true, text: "Cancel", click: function () {
                        $jQuery("[id$=rbtnActionsCatOverride] input:checked").prop('checked', false)
                        $jQuery("[id$=rbtnActionsCatOverride] [value='XXXX']").prop('checked', true);
                        $jQuery("[id$=dpExpteCatOverrideEditMode]")[0].control.set_selectedDate(null);
                        $jQuery("[id$=divExpiDateCategoryOvrrideEditMode]")[0].style.display = "none";
                    }
                }
            }, 475, '&nbsp;');
            $jQuery(".approvepopup").css("display", "none");
            //END UAT-1635 

        }
        else {
            $jQuery("[id$=dpExpteCatOverrideEditMode]")[0].control.set_selectedDate(null);
            $jQuery("[id$=divExpiDateCategoryOvrrideEditMode]")[0].style.display = "none";

        }



    };

    function ViewScreeningDocument(tenantId, documentId) {
        var popupWindowName = "View Document Window";
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/ComplianceOperations/Pages/FormViewer.aspx?documentId=" + documentId + "&tenantId=" + tenantId + "&IsApplicantDocument=" + "true");
        var win = $window.createPopup(url, {
            size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize
            | Telerik.Web.UI.WindowBehaviors.Close
            | Telerik.Web.UI.WindowBehaviors.Move
            | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName
        });
        return false;
    }

    function OpenEmployerDisclosureDocument(tenantId, orgUsrId, documentId, documentType) {
        if ($jQuery("[id$=hdnIsEdsAccepted]").val() == "true") {
            ViewScreeningDocument(tenantId, documentId)
        }
        else {
            var popupWindowName = "Employment Disclosure";
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/ReportEmploymentDisclosure.aspx?DocumentTypeCode=" + documentType + "&TenantID=" + tenantId + "&DocumentId=" + documentId + "&OrganizationUserID=" + orgUsrId);
            var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close, onclose: Close },
                function () {
                    this.set_title(popupWindowName);
                });
            return false;
        }
    }

    function Close(oWnd, args) {
        oWnd.remove_close(Close);
        var arg = args.get_argument();
        if (arg) {
            if (arg.Action == 'success') {
                if ($jQuery("[id$=hdnIsEdsAccepted]")) {
                    $jQuery("[id$=hdnIsEdsAccepted]").val("true");
                }
                ViewScreeningDocument(arg.TenantId, arg.DocumentId)
            }
            return false;
        }
    }

    function OnSelectedIndexChangeCatRejReason(sender, args) {
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
            var hdnSelectedRejectionReasonIDsCatExcep = sender._element.parentElement.id.replace("dvRejectionReason", "hdnSelectedRejectionReasonIDsCatExcep");
            var hdnSelectedRejectionReasonIDsCatExcep = $jQuery("[id$=" + hdnSelectedRejectionReasonIDsCatExcep + "]")[0];
            if (hdnSelectedRejectionReasonIDsCatExcep.value != "") {
                hdnSelectedRejectionReasonIDsCatExcep.value = hdnSelectedRejectionReasonIDsCatExcep.value + "," + sender._value;
            }
            else { hdnSelectedRejectionReasonIDsCatExcep.value = sender._value; }
        }
    }

</script>


<div class="approvepopup" runat="server" style="display: none">
    <span style="font-weight: bold; color: red">Override Compliance Rule</span>
    <br />
    By selecting approved, the compliance rule(s) for this category will be disabled!
</div>
<div class="msgbox">
    <asp:Label ID="lblMessageCatException" runat="server">
    </asp:Label>
</div>
<div class="msgbox">
    <asp:Label ID="lblMessageOverideRule" runat="server">
    </asp:Label>
</div>
<%-- UAT523:Show exception at category level--%>
<asp:Panel ID="pnlCategoryException" Visible="false" runat="server">

    <div class="section">
        <div class="content">
            <div class="sxform auto">
                <div class="sxpnl">
                    <asp:CheckBox ID="chkDeleteCatException" runat="server" Visible="false" Text="Delete Category Exception"></asp:CheckBox>
                    <div class='sxro sx1co'>
                        <div class='sxlb' title="The current verification status for this Category">
                            <span class="cptn">Overall Category Status</span>
                        </div>
                        <div class='sxlm'>
                            <span class="ronly">
                                <asp:Literal ID="litCurrentCategoryStatus" runat="server"></asp:Literal></span>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx1co'>
                        <div class='sxlb' title="The Category Exception status for this Category">
                            <span class="cptn">Category Exception Status</span>
                        </div>
                        <div class='sxlm'>
                            <span class="ronly">
                                <asp:Literal ID="litCategoryExceptionStatus" runat="server"></asp:Literal></span>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx1co'>
                        <div class='sxlb' title="The Category Exception Submission Date for this Category">
                            <span class="cptn">Category Exception Submission Date</span>
                        </div>
                        <div class='sxlm'>
                            <span class="ronly">
                                <asp:Literal ID="litCategoryExceptionSubmissionDate" runat="server"></asp:Literal></span>
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
                    <div id="divExpiDateEditMode" runat="server" style="display: none;">
                        <div class='sxro sx1co'>
                            <div class='sxlb'>
                                <span class="cptn">Expiration Date</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclDatePicker ID="dpExpiDateEditMode" runat="server" DateInput-EmptyMessage="Select a date">
                                </infs:WclDatePicker>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <!-- UAT-819: WB: Category Exception enhancements -->
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
                    <%--<div id="divReadOnly" runat="server">
                         <uc2:VerificationDocumentControlReadOnlyMode ID="ucReadOnlyMode" IsException="true"
                            runat="server"></uc2:VerificationDocumentControlReadOnlyMode>
                         <div class='sxro monly'>
                            <div class='sxlb'>
                                <span class="cptn">Comments</span>
                            </div>
                            <infs:WclTextBox Enabled="false" runat="server" ID="txtReadOnlyNotes" TextMode="MultiLine"
                                Height="50px" Width="125">
                            </infs:WclTextBox>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>--%>
                    <div id="divEditMode" runat="server">
                        <!-- UAT-819: WB: Category Exception enhancements -->
                        <uc3:VerificationDetailsDocumentConrol ID="ucVerificationDetailsDocumentConrol" IsException="true"
                            runat="server"></uc3:VerificationDetailsDocumentConrol>


                        <div class='sxro monly'>
                            <div class='sxlb' title="Admin comments for this Item are displayed below">
                                <span class="cptn" id="spnComments" runat="server">Admin's Comment History</span>
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
                        <div id="dvAdminActions" runat="server" visible="false">
                            <h1 class="shdr">Administrator Actions</h1>
                            <div class='sxro monly'>
                                <div class='sxlb'>
                                    <span class="cptn">Change Status</span>
                                </div>
                                <br />
                                <asp:RadioButtonList ID="rbtnActions" CssClass="radio-list" onclick="OnSelectedRadioButton(this)"
                                    runat="server" RepeatDirection="Horizontal">
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
                                    OnClientSelectedIndexChanged="OnSelectedIndexChangeCatRejReason" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" />
                                <div class='sxroend'>
                                </div>
                            </div>
                            <!-- UAT-819: WB: Category Exception enhancements -->
                            <div class='sxro monly'>
                                <div class='sxlb'>
                                    <span class="cptn">Write a Note</span>
                                </div>
                                <%--<infs:WclTextBox runat="server" ID="txtAdminNotes" TextMode="MultiLine" Height="50px">
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
        </div>
    </div>

    <asp:HiddenField ID="hdfCatRejectionCodeException" runat="server" />
    <!--[SS]:UAT-3951:Addition of option to use preset ADB Admin rejection notes-->
    <asp:HiddenField ID="hdnSelectedRejectionReasonIDsCatExcep" runat="server" />
</asp:Panel>


<%-- UAT523 END Show exception at category level--%>


<div style="margin-bottom: 40px;">
</div>

<asp:Panel ID="pnlItems" runat="server">
</asp:Panel>

<hr>
<%-- UAT-845  Admin Category Override--%>
<asp:Panel ID="pnlCategoryOverride" Visible="false" runat="server">

    <div style="padding-left: 10px;">

        <span style="font-weight: bold; color: red">Override
        Compliance Rule</span>
    </div>
    <div class="section">
        <div class="content">
            <div class="sxform auto">
                <div class="sxpnl">
                    <div class='sxro sx1co'>
                        <div class='sxlb' title="The current verification status for this Category">
                            <span class="cptn">Current Status</span>
                        </div>
                        <div class='sxlm'>
                            <span class="ronly">
                                <asp:Label ID="litCurrentCategoryVerSts" runat="server"></asp:Label></span>
                        </div>
                
                        <div class='sxroend'>
                        </div>
                                 <asp:CheckBox ID="chkDeleteOverideStatus" ForeColor="Red" runat="server" Visible="false" 
                                     Text="Delete Override Status" OnClick="chkDeleteOverideStatus_Click(this.checked)" ></asp:CheckBox>
                            <br />

                    </div>
                    <%-- <div id="divExpiDateCategoryOvrrideReadOnly" runat="server" style="display: none;">
                        <div class='sxro sx1co'>
                            <div class='sxlb'>
                                <span class="cptn">Expiration Date</span>
                            </div>
                            <div class='sxlm monly'>
                                <infs:WclDatePicker ID="dpExpteCatOverrideReadOnly" runat="server" Enabled="false" DateInput-EmptyMessage="Select a date">
                                </infs:WclDatePicker>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>--%>
                    <div id="divExpiDateCategoryOvrrideEditMode" runat="server" style="display: none;">
                        <div class='sxro sx1co'>
                            <div class='sxlb'>
                                <span class="cptn">Override Expiration Date</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclDatePicker ID="dpExpteCatOverrideEditMode" runat="server" DateInput-EmptyMessage="Select a date">
                                </infs:WclDatePicker>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <div id="dvAdminActionsCatOverride" runat="server" visible="false">
                        <h1 class="shdr">Administrator Actions</h1>
                        <div class='sxro monly'>
                            <div class='sxlb'>
                                <span class="cptn">Change Status</span>
                            </div>
                            <br />
                            <asp:RadioButtonList ID="rbtnActionsCatOverride" onchange="OnSelectedRadioButtonCatOverride(this)"
                                CssClass="radio-list" runat="server" RepeatDirection="Horizontal">
                            </asp:RadioButtonList>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <div class='sxro monly'>
                        <div class='sxlb'>
                            <span class="cptn">Category Override Notes</span>
                        </div>
                        <%-- <infs:WclTextBox runat="server" ID="txtOverrideNotes" TextMode="MultiLine" CssClass="resizetxtbox" MaxLength="1000">
                        </infs:WclTextBox>--%>
                        <asp:TextBox runat="server" ID="txtOverrideNotes" TextMode="MultiLine" CssClass="resizetxtbox" MaxLength="1000">
                        </asp:TextBox>
                        <div class='sxroend'>
                        </div>
                    </div>

                </div>

            </div>
        </div>
    </div>




</asp:Panel>
<div class="section" id="unassignedDocs" runat="server">
    <h2 class="header-color category">Un-assigned Document(s)
            <div class="sec_cmds">
                <span class="bar_icon ihelp" title="View Un-assigned documents"></span>
            </div>
    </h2>
    <div class="content">
        <div class="tab-block">
            <div class="tabs">
                <span class="tab1 focused">Applicant's Document(s)</span>
            </div>
            <div class="tab-content tab1 focused">
                <uc4:VerificationDetailsUnassignedDocumentConrol ID="ucVerificationDetailsUnassignedDocumentConrol" IsException="true" runat="server"></uc4:VerificationDetailsUnassignedDocumentConrol>
            </div>
        </div>
    </div>
</div>
<%-- END UAT-845  Admin Category Override--%>
<div class="page_cmd_main">
    <%--    <infsu:CommandBar ID="btnSave" runat="server" DisplayButtons="Save" ButtonPosition="Center"
        Visible="false" SaveButtonText="Save Changes" OnSaveClick="Save" OnSaveClientClick="verifyRejection">
    </infsu:CommandBar>--%>
    <%--    <infs:WclButton runat="server" Visible="false" OnClick="Save" OnClientClicked="verifyRejection"
                ID="btnSave" Text="Save Changes">
                <Icon PrimaryIconCssClass="rbSave" />
            </infs:WclButton>--%>
</div>

<asp:HiddenField ID="hdfSaveResultMessage" Value="Test Message" runat="server" />
<asp:HiddenField ID="hdnExpirationDate" runat="server" />
<asp:HiddenField ID="hdnIsEdsAccepted" runat="server" Value="" />
<asp:HiddenField ID="hdnPackageSubscriptionId" runat="server" Value="" />
<asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
