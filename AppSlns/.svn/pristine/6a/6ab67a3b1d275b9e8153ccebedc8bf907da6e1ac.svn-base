<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateDraftControl.ascx.cs" Inherits="CoreWeb.PlacementMatching.Views.CreateDraftControl" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ClinicalRotation/UserControl/CustomAttributeForm.ascx"
    TagPrefix="infsu" TagName="CustomAttributes" %>
<%@ Register Src="~/ClinicalRotation/UserControl/SharedUserCustomAttributeForm.ascx"
    TagPrefix="uc" TagName="CustomAttributes" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxCreateDraft">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

</infs:WclResourceManagerProxy>
<style type="text/css">
    .DaysColumn {
        width: 12%;
        float: left;
        margin-right: 2px;
    }

    .daysbutton {
        border-style: None;
        height: 100%;
        width: 100%;
        margin-bottom: 0px;
        padding-bottom: 5px;
        text-align: center;
        text-decoration: none;
        font-size: 16px;
        border-radius: 2px;
        margin-right: 2px;
    }
</style>
<div class="row">
    <div class="col-md-6 col-sm-6">
        <h2 class="header-color">
            <asp:Label ID="lblCreateDraft" runat="server" Text="Create Draft"></asp:Label>
        </h2>

    </div>
    <div class="col-md-6 col-sm-6" style="float: right">
        <h2 style="float: right" class="header-color">
            <asp:Label ID="lblStaus" runat="server" Visible="false"></asp:Label>
        </h2>

    </div>
</div>

<div class="col-md-12 col-sm-12">
    <div class="row">
        <div class="form-group col-md-6 col-sm-6">
            <div class="col-md-4 col-sm-4 row">
                <span class="cptn">Institution</span>
            </div>
            <div class="col-md-8 col-sm-8 row" style="margin-top: 5px">
                <asp:Label ID="lblInstitution" class="form-conrtrol" runat="server" Text="ABC"></asp:Label>
            </div>
        </div>
        <div class="form-group col-md-6 col-sm-6">
            <div class="col-md-4 col-sm-4 row">
                <span class="cptn">Student Type</span>
            </div>
            <div class="col-md-8 col-sm-8 row" style="margin-top: 5px">
                <asp:Label ID="lblStudentType" class="form-conrtrol" runat="server" Text="BSN, Acelerated BSN,LPN"> 
                </asp:Label>
            </div>
        </div>

        <div class="form-group col-md-6 col-sm-6">
            <div class="col-md-4 col-sm-4 row">
                <span class="cptn">Location</span>
            </div>
            <div class="col-md-8 col-sm-8 row" style="margin-top: 5px">
                <asp:Label ID="lblLocation" class="form-conrtrol" runat="server" Text="ABC Clinic"></asp:Label>
            </div>
        </div>
        <div class="form-group col-md-6 col-sm-6">
            <div class="col-md-4 col-sm-4 row">
                <span class="cptn">Max #</span>
            </div>
            <div class="col-md-8 col-sm-8 row" style="margin-top: 5px">
                <asp:Label ID="lblMax" class="form-conrtrol" runat="server" Text="10"></asp:Label>
            </div>
        </div>
        <div class="form-group col-md-6 col-sm-6">
            <div class="col-md-4 col-sm-4 row">
                <span class="cptn">Unit</span>
            </div>
            <div class="col-md-8 col-sm-8 row" style="margin-top: 5px">
                <asp:Label ID="lblUnit" class="form-conrtrol" runat="server" Text="3rd floor"></asp:Label>
            </div>
        </div>
        <div class="form-group col-md-6 col-sm-6">
            <div class="col-md-4 col-sm-4 row">
                <span class="cptn">Dates Available</span>
            </div>
            <div class="col-md-8 col-sm-8 row" style="margin-top: 5px">
                <asp:Label ID="lblDates" class="form-conrtrol" runat="server" Text="January-December"></asp:Label>
            </div>
        </div>
        <div class="form-group col-md-6 col-sm-6">
            <div class="col-md-4 col-sm-4 row">
                <span class="cptn">Department</span>
            </div>
            <div class="col-md-4 col-sm-4 row" style="margin-top: 5px">
                <asp:Label ID="lblDepartment" class="form-conrtrol" runat="server" Text="Nursing"></asp:Label>
            </div>
        </div>
        <div class="form-group col-md-6 col-sm-6">
            <div class="col-md-4 col-sm-4 row">
                <span class="cptn">Days</span>
            </div>
            <div class="col-md-8 col-sm-8 row" style="margin-top: 5px">
                <asp:Label ID="lblDays" class="form-conrtrol" runat="server" Text="M,W,F"></asp:Label>
            </div>
        </div>
        <div class="form-group col-md-6 col-sm-6">
            <div class="col-md-4 col-sm-4 row">
                <span class="cptn">Speciality</span>
            </div>
            <div class="col-md-4 col-sm-4 row" style="margin-top: 5px">
                <asp:Label ID="lblSpeciality" class="form-conrtrol" runat="server" Text="Pediatrics"></asp:Label>
            </div>
        </div>
        <div class="form-group col-md-6 col-sm-6">
            <div class="col-md-4 col-sm-4 row">
                <span class="cptn">Shift</span>
            </div>
            <div class="col-md-8 col-sm-8 row" style="margin-top: 5px">
                <asp:Label ID="lblShift" class="form-conrtrol" runat="server" Text="AM"></asp:Label>
            </div>
        </div>
        <div class="form-group col-md-6 col-sm-6">
        </div>
        <div class="form-group col-md-6 col-sm-6">
            <div class="col-md-4 col-sm-4 row">
                <span class="cptn">IsPreceptonship</span>
            </div>
            <div class="col-md-8 col-sm-8 row" style="margin-top: 5px">
                <asp:Label ID="lblIsPreceptonship" class="form-conrtrol" runat="server"></asp:Label>
            </div>
        </div>
        <div class="form-group col-md-6 col-sm-6">
        </div>
        <div class="form-group col-md-6 col-sm-6">
            <div class="col-md-5 col-sm-5 row">
                <span class="cptn">Contains Float Area</span>
            </div>
            <div class="col-md-1 col-sm-1 row" style="margin-top: 5px; margin-left: -32px">
                <asp:Label ID="lblFloatArea" class="form-conrtrol" runat="server"></asp:Label>
            </div>
            <div class="col-md-3 col-sm-3 row">
                <span class="cptn" runat="server" id="spnFloatArea" visible="false" style="margin-left: 8px">Float Area</span>
            </div>
            <div class="col-md-3 col-sm-3 row" style="margin-top: 5px">
                <asp:Label runat="server" ID="lblFloatAreaText"></asp:Label>
            </div>
        </div>
        <div class="form-group col-md-6 col-sm-6">
        </div>
        <div class="form-group col-md-6 col-sm-6">
            <div class="col-md-4 col-sm-4 row">
            </div>
            <%--     <div class="col-md-8 col-sm-8 row" style="margin-top: 5px">
                <div class="col-md-4 col-sm-4 row">
                    <span class="cptn">Unit</span>
                </div>
                <div class="col-md-4 col-sm-4 row" style="margin-top: 5px">
                    <asp:Label ID="lblUnit1" class="form-conrtrol" runat="server" Text="3rd Floor"></asp:Label>
                </div>
                <div class="col-md-4 col-sm-4 row">
                    <span class="cptn">Max #</span>
                </div>
                <div class="col-md-4 col-sm-4 row" style="margin-top: 5px">
                    <asp:Label ID="lblMax1" class="form-conrtrol" runat="server" Text="10"></asp:Label>
                </div>
            </div>--%>
        </div>
    </div>
</div>
<asp:Panel runat="server" ID="pnlCreateDraft">
    <div class="col-md-12 col-sm-12">
        <div class="row">
            <div class="col-md-6 col-sm-6">
                <%--    <div class="form-group col-md-12 col-sm-12" title="Click the link and select a node to restrict search results to the selected node">
                    <div class="row">
                        <label class="cptn">Institution Hierarchy</label>
                        <a href="#" id="instituteHierarchy" onclick="openPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                        <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
                    </div>
                </div>--%>
                <div class="row">
                    <div class="form-group col-md-6 col-sm-6">
                        <span class="cptn">Course</span><span class='reqd'>*</span>
                        <infs:WclTextBox ID="txtCourse" Width="100%" CssClass="form-control" runat="server">
                        </infs:WclTextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtCourse" ID="rfvCourse" runat="server" Text="Please enter the Course" ForeColor="Red" ValidationGroup="grpFormSubmit"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-6 col-sm-6">
                        <span class="cptn">Number Of Students</span><span class='reqd'>*</span>
                        <infs:WclNumericTextBox DataType="Integer" ID="txtStudents" Width="100%" MinValue="1" NumberFormat-DecimalDigits="0" CssClass="form-control" runat="server">
                        </infs:WclNumericTextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtStudents" ID="rfvStudents" runat="server" ForeColor="Red" ErrorMessage="Please enter the number of Students" ValidationGroup="grpFormSubmit"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-6 col-sm-6">
                        <span class="cptn">Start Date</span><span class='reqd'>*</span>
                        <%--     <infs:WclDatePicker ID="dteStartDate" Width="100%" CssClass="form-control" runat="server">
                        </infs:WclDatePicker>--%>
                        <telerik:RadDatePicker ID="dteStartDate" Width="100%" CssClass="form-control" runat="server" MinDate="2009/1/1"></telerik:RadDatePicker>
                        <asp:RequiredFieldValidator runat="server" ID="rfvdteStartDate" ControlToValidate="dteStartDate" ErrorMessage="Please enter valid date" ForeColor="Red" ValidationGroup="grpFormSubmit"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-md-6 col-sm-6">
                        <span class="cptn">End Date</span><span class='reqd'>*</span>
                        <infs:WclDatePicker ID="dteEndDate" Width="100%" CssClass="form-control" runat="server">
                        </infs:WclDatePicker>
                        <asp:RequiredFieldValidator runat="server" ID="rfvdteEndDate" ControlToValidate="dteEndDate" ValidationGroup="grpFormSubmit" ForeColor="Red" ErrorMessage="Please enter  valid date"></asp:RequiredFieldValidator>
                    </div>
                    <asp:CompareValidator ID="cvDate" runat="server" ControlToValidate="dteEndDate" ControlToCompare="dteStartDate" Operator="GreaterThanEqual" ErrorMessage="End date should be greater than Start date" ForeColor="Red" ValidationGroup="grpFormSubmit"></asp:CompareValidator>
                </div>
                <div class="row">
                    <div class="form-group col-md-5 col-sm-5" style="width: 45%">
                        <span class="cptn">Shift</span><span class='reqd'>*</span>
                        <infs:WclComboBox ID="ddlShift" runat="server" DataTextField="Shift" EmptyMessage="--Select--" AutoPostBack="true"
                            AllowCustomText="true" CausesValidation="false" DataValueField="ClinicalInventoryShiftID" CheckBoxes="false" EnableCheckAllItemsCheckBox="true"
                            Filter="Contains" OnSelectedIndexChanged="ddlShift_SelectedIndexChanged" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                        <asp:RequiredFieldValidator runat="server" ID="rfvddlShift" ControlToValidate="ddlShift" ForeColor="Red" ErrorMessage="Please select the Shift" ValidationGroup="grpFormSubmit"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-md-7 col-sm-7 ">
                        <span class="cptn" style="width: 100%; float: left;">Days</span>
                        <div class="DaysColumn">
                            <asp:Button ID="btnSunday" Enabled="false" CssClass="daysbutton" Text="S" runat="server" OnClick="btnSunday_Click" />
                        </div>
                        <div class="DaysColumn">

                            <asp:Button ID="btnMonday" Enabled="false" CssClass="daysbutton" Text="M" runat="server" OnClick="btnMonday_Click" />
                        </div>
                        <div class="DaysColumn">

                            <asp:Button ID="btnTuesday" OnClick="btnTuesday_Click" Enabled="false" CssClass="daysbutton" Text="T" runat="server" />
                        </div>
                        <div class="DaysColumn">

                            <asp:Button ID="btnWednesday" Enabled="false" CssClass="daysbutton" Text="W" runat="server" OnClick="btnWednesday_Click" />
                        </div>
                        <div class="DaysColumn">

                            <asp:Button ID="btnThursday" Enabled="false" CssClass="daysbutton" Text="T" runat="server" OnClick="btnThursday_Click" />
                        </div>
                        <div class="DaysColumn">

                            <asp:Button ID="btnFriday" Enabled="false" CssClass="daysbutton" Text="F" runat="server" OnClick="btnFriday_Click" />
                        </div>
                        <div class="DaysColumn">
                            <asp:Button ID="btnSaturday" Enabled="false" CssClass="daysbutton" Text="S" runat="server" OnClick="btnSaturday_Click" />
                        </div>
                        <asp:CustomValidator ID="cvlDaySelect" runat="server" ClientIDMode="Static" ClientValidationFunction="DayValidateFunction" ValidationGroup="grpFormSubmit" ForeColor="Red" ErrorMessage="Please select atleast one day"></asp:CustomValidator>
                    </div>


                </div>

                <div class="row">
                    <div class="form-group col-md-12 col-sm-12">
                        <span class="cptn">Notes</span>
                        <infs:WclTextBox ID="txtNotes" Width="100%" CssClass="form-control" runat="server" TextMode="MultiLine" Columns="3">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <asp:HiddenField runat="server" ID="hdnDepartmntPrgrmMppng" />
            <asp:HiddenField runat="server" ID="hdnTenantId" />
            <asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
            <asp:HiddenField ID="hdnHierarchyLabel" runat="server" />

            <div class="col-md-6 col-sm-6">
                <div class="form-group col-md-12 col-sm-12">
                    <span class="cptn">Audit Log</span>
                    <infs:WclTextBox runat="server" ID="txtAuditLogs" TextMode="MultiLine" Enabled="false" DisabledStyle-Font-Italic="true"
                        Width="100%" CssClass="resizetxtbox borderTextArea" Rows="20" ReadOnly="true" Height="100%">
                    </infs:WclTextBox>
                </div>
            </div>

        </div>
    </div>
    <div class='col-md-12 col-sm-12'>
        <%-- <asp:Panel ID="pnlCustomAttributes" runat="server"></asp:Panel>--%>
        <uc:CustomAttributes ID="caCustomAttributesID" EnableViewState="false" runat="server" />
    </div>
</asp:Panel>

<div class="row ">
    <div class="col-md-12 col-sm-12">
        <div class="form-group col-md-3 col-sm-3"></div>
        <div class="col-md-5 col-sm-9"></div>

        <infs:WclButton CssClass="" ValidationGroup="grpFormSubmit" runat="server" AutoPostBack="true" Skin="Silk" OnClick="btnPublish_Click" AutoSkinMode="false" ID="btnPublish" Text="Publish" Visible="false">
            <Icon PrimaryIconCssClass="fa fa-share-square-o" PrimaryIconLeft="15" PrimaryIconTop="10" PrimaryIconHeight="14"
                PrimaryIconWidth="14" />
        </infs:WclButton>
        <infs:WclButton CssClass="" ValidationGroup="grpFormSubmit" runat="server" AutoPostBack="true" Skin="Silk" OnClick="btnSave_Click" AutoSkinMode="false" ID="btnSave" Text="Save" Visible="false" Icon-PrimaryIconCssClass="rbSave">
        </infs:WclButton>

        <infs:WclButton CssClass="" runat="server" AutoPostBack="true" Skin="Silk" OnClick="btnArchive_Click" AutoSkinMode="false" ID="btnArchive" Text="Archive" Visible="false" Icon-PrimaryIconCssClass="rbArchive">
        </infs:WclButton>

        <infs:WclButton CssClass="" runat="server" AutoPostBack="true" Skin="Silk" OnClick="btnEdit_Click" AutoSkinMode="false" ID="btnEdit" Text="Edit" Visible="false" Icon-PrimaryIconCssClass="rbEdit">
        </infs:WclButton>

        <infs:WclButton CssClass="" runat="server" AutoPostBack="true" Skin="Silk" OnClick="btnApprove_Click" AutoSkinMode="false" ID="btnApprove" Text="Approve" Visible="false">
            <Icon PrimaryIconCssClass="fa fa-check" PrimaryIconLeft="15" PrimaryIconTop="9" PrimaryIconHeight="14"
                PrimaryIconWidth="14" />
        </infs:WclButton>

        <infs:WclButton CssClass="" runat="server" AutoPostBack="true" Skin="Silk" OnClick="btnReject_Click" AutoSkinMode="false" ID="btnReject" Text="Reject" Visible="false">
            <Icon PrimaryIconCssClass="fa fa-times" PrimaryIconLeft="15" PrimaryIconTop="9" PrimaryIconHeight="14"
                PrimaryIconWidth="14" />
        </infs:WclButton>

        <infs:WclButton CssClass="" runat="server" AutoPostBack="true" Skin="Silk" OnClick="btnCancelRequest_Click" AutoSkinMode="false" ID="btnCancelRequest" Text="Cancel" Visible="false" Icon-PrimaryIconCssClass="rbCancel">
        </infs:WclButton>

        <infs:WclButton CssClass="" runat="server" AutoPostBack="true" Skin="Silk" OnClick="btnCancel_Click" AutoSkinMode="false" ID="btnCancel" Text="Close" Visible="false">
            <Icon PrimaryIconCssClass="fa fa-times-circle" PrimaryIconLeft="15" PrimaryIconTop="9" PrimaryIconHeight="14"
                PrimaryIconWidth="14" />
        </infs:WclButton>

    </div>
</div>

<asp:HiddenField ID="hdnRequestSavedSuccessfully" runat="server" />
<asp:HiddenField ID="hdnRequestCancelledSuccessfully" runat="server" />
<asp:HiddenField ID="hdnRequestRejectedSuccessfully" runat="server" />
<asp:HiddenField ID="hdnRequestArchivedSuccessfully" runat="server" />
<asp:HiddenField ID="hdnRequestApprovedSuccessfully" runat="server" />
<asp:HiddenField ID="hdnRequestPublishedSuccessfully" runat="server" />
<asp:HiddenField runat="server" ID="hdnOpportunityId" />
<script>

    function openPopUp() {
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "CommonScreen";
        //var tenantId = 2;
        var tenantId = $jQuery("[id$=hdnTenantId]").val();
        if (tenantId != "0" && tenantId != "" && tenantId > 0) {
            //$jQuery("[id$=hdnIsPagePostBack]").val("Focus Set");
            $jQuery("[id$=instituteHierarchy]").focusout();
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();

            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName=" + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);
            var win = $window.createPopup(url, { size: "500," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
        }
        return false;
    }

    function OnClientClose(oWnd, args) {
        //   debugger;
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                $jQuery("[id$=hdnDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
                $jQuery("[id$=hdnHierarchyLabel]").val(arg.HierarchyLabel);
                $jQuery("[id$=hdnInstitutionNodeId]").val(arg.InstitutionNodeId);
                __doPostBack("<%= btnDoPostBack.ClientID %>", "");
            }
            winopen = false;

            setTimeout(function () { $jQuery("[id$=instituteHierarchy]").focus(); }, 100);
        }
    }
    function ChangeColor() {

    }
    function GetRadWindow() {
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }

    function returnToParent() {
        //  debugger;
        var hdnRequestSavedSuccessfully = $jQuery("[id$=hdnRequestSavedSuccessfully]")[0].value;
        var hdnRequestCancelledSuccessfully = $jQuery("[id$=hdnRequestCancelledSuccessfully]")[0].value;
        var hdnRequestRejectedSuccessfully = $jQuery("[id$=hdnRequestRejectedSuccessfully]")[0].value;
        var hdnRequestArchivedSuccessfully = $jQuery("[id$=hdnRequestArchivedSuccessfully]")[0].value;
        var hdnRequestApprovedSuccessfully = $jQuery("[id$=hdnRequestApprovedSuccessfully]")[0].value;
        var hdnRequestPublishedSuccessfully = $jQuery("[id$=hdnRequestPublishedSuccessfully]")[0].value;

        var oArg = {};

        oArg.RequestSavedSuccessfully = hdnRequestSavedSuccessfully;
        oArg.RequestCancelledSuccessfully = hdnRequestCancelledSuccessfully;
        oArg.RequestRejectedSuccessfully = hdnRequestRejectedSuccessfully;
        oArg.RequestArchivedSuccessfully = hdnRequestArchivedSuccessfully;
        oArg.RequestApprovedSuccessfully = hdnRequestApprovedSuccessfully;
        oArg.RequestPublishedSuccessfully = hdnRequestPublishedSuccessfully;

        var oWnd = GetRadWindow();
        oWnd.Close(oArg);
    }

    function DayValidateFunction(sender, args) {
        var color = '#add8e6';
        // debugger;
        args.IsValid = false;
        if (hexc($jQuery("[id$=btnMonday]").css('backgroundColor')) == color
            || hexc($jQuery("[id$=btnTuesday]").css('backgroundColor')) == color
            || hexc($jQuery("[id$=btnWednesday]").css('backgroundColor')) == color
            || hexc($jQuery("[id$=btnThursday]").css('backgroundColor')) == color
            || hexc($jQuery("[id$=btnFriday]").css('backgroundColor')) == color
            || hexc($jQuery("[id$=btnSaturday]").css('backgroundColor')) == color
            || hexc($jQuery("[id$=btnSunday]").css('backgroundColor')) == color) {
            args.IsValid = true;
        }
    }
    function hexc(colorval) {
        var parts = colorval.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
        delete (parts[0]);
        for (var i = 1; i <= 3; ++i) {
            parts[i] = parseInt(parts[i]).toString(16);
            if (parts[i].length == 1) parts[i] = '0' + parts[i];
        }
        return '#' + parts.join('');
    }

</script>
