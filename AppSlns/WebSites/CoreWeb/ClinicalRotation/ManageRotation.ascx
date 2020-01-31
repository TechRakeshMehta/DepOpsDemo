<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageRotation.ascx.cs"
    Inherits="CoreWeb.ClinicalRotation.Views.ManageRotation" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ClinicalRotation/UserControl/CustomAttributeForm.ascx"
    TagPrefix="infsu" TagName="CustomAttributes" %>
<%@ Register Src="~/ClinicalRotation/UserControl/SharedUserCustomAttributeForm.ascx"
    TagPrefix="uc" TagName="CustomAttributes" %>
<%--<%@ Register TagPrefix="uc" TagName="AgencyHierarchy" Src="~/AgencyHierarchy/UserControls/AgencyHierarchySelection.ascx" %>--%>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchyMultiple" Src="~/AgencyHierarchy/UserControls/AgencyHierarchyMultipleSelection.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/ColumnsConfiguration.ascx" TagPrefix="infsu" TagName="ColumnsConfiguration" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/ClinicalRotation/ManageRotation.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/AgencyHierarchy/AgencyHierarchyMultipleSelection.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Generic/ColumnsConfiguration.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
       <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>


<style type="text/css">
    .groupBtn {
    margin-top:2px;
    }

    .rmItem.rmLast .rmText {
    padding-right:3px;
    }
    .btnSeprate {
    position:absolute;
    right:15px;
    }
    .controlHidden {
        display: none;
    }

    .disabled {
        pointer-events: none;
        cursor: default;
        text-decoration: none;
        color: gray !important;
    }

    .highlightGrid {
        color: red !important;
    }

    .highlight {
        /*border:2px solid #ff5608 !important;*/
        border: 2px solid red !important;
        border-radius: 5px !important;
    }
    /*UAT-3138*/
    .top3 {
        top: 3px !important;
    }

    .btn {
       
        text-align: left;
    }

    .RadMenu .rmGroup .rmText {
        padding: 0px;
        margin: 0px;
    }

    .rmVertical.rmGroup.rmLevel1 {
        border: none;
    }

    /*UAT-3221*/
    .agencyHierarchyLinkDisabled {
        color: gray !important;
        text-decoration: none !important;
        cursor: default !important;
    }
</style>


<script>
    //$page.add_pageLoad(function () {
    //    var $ = $jQuery;
    //    $(".grdCmdBar .RadButton").each(function () {
    //        //console.log($(this).find(".rbText").text());
    //        if ($(this).text().toLowerCase() == "refresh") {
    //            $(this).attr("title", "Click to reload the data in the grid");
    //        }
    //        if ($(this).text().toLowerCase() == "clear filters") {
    //            $(this).attr("title", "Click to clear any information entered in the filters");
    //        }
    //        if ($(this).text().toLowerCase() == "download") {
    //            $(this).attr("title", "Click to export the data displayed in the grid");
    //        }
    //    });
    //});




    $page.add_pageLoaded(function () {
        var $ = $jQuery;
       

        if($('.groupBtn span') != undefined || $('.groupBtn span') != null)
            $('.groupBtn span').removeClass('rbPrimaryIcon');
        if (Telerik.Web.UI.RadAsyncUpload != null && Telerik.Web.UI.RadAsyncUpload != undefined) {
            Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
            Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };
        }
        // $jQuery("[id$=AgencyHierarchy]").removeClass("buttonHidden");
        
        $(".grdCmdBar .RadButton").each(function () {
            //console.log($(this).find(".rbText").text());
            if ($(this).text().toLowerCase() == "download") {
                //debugger;
                //var columnNames = $jQuery(["id$=grdRotations"]).jqGrid('getCol', 'CustomAttributesGrd');
            }
        });
    });

    function ValidateStartEndTime(sender, args) {
        var tpStartTime = $jQuery("[id$=tpStartTime]")[0].control.get_timeView().getTime();
        var tpEndTime = $jQuery("[id$=tpEndTime]")[0].control.get_timeView().getTime();
        if (tpEndTime != null && tpStartTime == null) {
            sender.innerText = 'Rotation "Start Time" is required.'
            args.IsValid = false;
        }
        if (tpStartTime != null && tpEndTime == null) {
            sender.innerText = 'Rotation "End Time" is required.'
            args.IsValid = false;
        }
    }
    function ValidateDaysBeforeFrequency(sender, args) {
        //var txtDaysBefore = $jQuery("[id$=txtDaysBefore]").innerText();
        //var txtFrequency = $jQuery("[id$=txtFrequency]").innerText();

        //if (txtDaysBefore != null && txtFrequency == null) {
        //    sender.innerText = 'Rotation "Frequency " is required.'
        //    args.IsValid = false;
        //}
        //if (txtDaysBefore == null && txtFrequency != null) {
        //    sender.innerText = 'Rotation "Days Before" is required.'
        //    args.IsValid = false;
        //}
    }
    function pageLoad() {

         SetDefaultButtonForSection("pnlSearch", "fsucCmdBarButton_btnSave", true);

        var hdnHierarchyLabel = $jQuery("[id$=hdnHierarchyLabel]");
        var hdnIsPagePostBack = $jQuery("[id$=hdnIsPagePostBack]");
        if (hdnHierarchyLabel.val() != "" && hdnIsPagePostBack.val() == "Focus Set") {
            setTimeout(function () { $jQuery("[id$=instituteHierarchy]").focus(); }, 100);
            hdnIsPagePostBack.val("");
        }
        //$jQuery("[id$=ucAgencyHierarchyMultipleToSearchRotation]").find("[id$=spnAgencyHierarchy]").removeClass('buttonHidden');
        //$jQuery("div#ucAgencyHierarchyAddEditRotation #spnAgencyHierarchy").removeClass('buttonHidden');

    }

    //UAT-2034
    function CheckAll(id) {
        var masterTable = $find("<%= grdRotations.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectRotation").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectRotation").checked = isChecked; // for checking the checkboxes
            }
        }
    }
    function UnCheckHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdRotations.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectRotation").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectRotation").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }

    function OpenPopupForManageRotAssignments(agencyId, selectedRotationIds, rotationAssignmentTypeCode, tenantID) {
        var popupWindowName = "Manage Rotation Assignments";
        //UAT-2364
        var popupHeight350 = $jQuery(window).height() * (80 / 100);

        var url = $page.url.create("~/ClinicalRotation/Pages/ManageRotationAssignmentPopup.aspx?TenantID=" + tenantID + "&AgencyId=" + agencyId + "&SelectedRotationIds=" + selectedRotationIds + "&RotationAssignmentTypeCode=" + rotationAssignmentTypeCode);
        var win = $window.createPopup(url, { size: "500," + popupHeight350, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnClose }
            , function () {
                this.set_title(popupWindowName);
            });
        return false;
    }

    function OnClose(oWnd, args) {
        oWnd.remove_close(OnClose);
        var arg = args.get_argument();
        if (arg) {
            if (arg.IsStatusSaved) {
                var masterTable = $find("<%= grdRotations.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        }
        if (winopen) {
            winopen = false;
        }
    }
    function CheckForInsPreceptor(sender, args) {
        args.IsValid = false;
        var idlist = '';
        var cbmitems = $find($jQuery("[id$=ddlInstructor]")[0].id);

        cbmitems.get_checkedItems().forEach(function (item) {

            idlist = idlist.concat(item.get_value(), ',');
        });
        if (idlist != undefined && idlist != '') {
            args.IsValid = true;
        }
    }

    function ResetAgencyHierarchySelection() {
        //$jQuery("[id$=ucAgencyHierarchyAddEditRotation_hdnHierarchyLabel").val('');
        //$jQuery("[id$=ucAgencyHierarchyAddEditRotation_hdnAgencyHierarchyNodeIds").val('');
        //$jQuery("[id$=ucAgencyHierarchyAddEditRotation_hdnInstitutionNodeIds").val('');
        //$jQuery("[id$=ucAgencyHierarchyAddEditRotation_hdnAgencyName").val('');
        //$jQuery("[id$=ucAgencyHierarchyAddEditRotation_hdnselectedRootNodeId").val('');
        $jQuery($jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_lblAgencyHierarchy")[0]).text('');

        //$jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnTenantId").val('');
        $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_AgencyHierarchyNodeIds").val('');
        // $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnAgencyHierarchyNodeSelection").val('');
        // $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnNodeHierarchySelection").val('');
        $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnSelectedAgecnyIds").val('');
        //$jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnNodeHierarchySelection").val('');
        $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnSelectedRootNodeId").val('');

    }

    function BindInstitutionLabel() {
        setTimeout(function () {
            var InstNodeLabel = $jQuery("[id$=hdnInstNodeLabel]").val();
            $jQuery($jQuery("[id$=lblInstitutionHierarchyPB]")[0]).text(InstNodeLabel);
        }, 1100);
    }

    $jQuery(document).ready(function () {
        SetDefaultButtonForSection("pnlSearch", "fsucCmdBarButton_SearchClick", true);

        $jQuery(document).on('click', '[id$=lnkInstitutionHierarchyPB]', function (e) {
            if (!$jQuery(this).hasClass('disabled')) {
                OpenInstitutionHierarchyPopupInsideGrid(false);
            }
        });

    });

    //function validateUpload(source, args) {
    //    debugger;
    //    $jQuery("[id$=lblSyllabusDocumentError]").css('display', 'none');
    //    args.IsValid = false;
    //    var upload = $find($jQuery("[id$=uploadControl]")[0].id);
    //    if (upload._uploadedFiles.length == 0) {
    //        if ($jQuery("[id$=hdnValidateFileUploadControl]").val() == "true") {
    //            //$jQuery("[id$=lblSyllabusDocumentError]").css('display', 'block');
    //        }
    //    }
    //    else {
    //        $jQuery("[id$=hdnValidateFileUploadControl]").val() == "false";
    //        args.IsValid = true;
    //    }
    //}
    function ResetValidator() {

        if ($jQuery("[id$=rfvRotationName]")[0] != "" && $jQuery("[id$=rfvRotationName]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvRotationName]")[0], false);
        }
        if ($jQuery("[id$=rfvTypeSpecialtyAddEdit]")[0] != "" && $jQuery("[id$=rfvTypeSpecialtyAddEdit]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvTypeSpecialtyAddEdit]")[0], false);
        }
        if ($jQuery("[id$=rfvTerm]")[0] != "" && $jQuery("[id$=rfvTerm]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvTerm]")[0], false);
        }
        if ($jQuery("[id$=rfvUnitFloorLocation]")[0] != "" && $jQuery("[id$=rfvUnitFloorLocation]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvUnitFloorLocation]")[0], false);
        }
        if ($jQuery("[id$=rfvStudent]")[0] != "" && $jQuery("[id$=rfvStudent]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvStudent]")[0], false);
        }
        if ($jQuery("[id$=rfvRecommendedHrs]")[0] != "" && $jQuery("[id$=rfvRecommendedHrs]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvRecommendedHrs]")[0], false);
        }
        if ($jQuery("[id$=rfvDays]")[0] != "" && $jQuery("[id$=rfvDays]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvDays]")[0], false);
        }
        if ($jQuery("[id$=rfvShift]")[0] != "" && $jQuery("[id$=rfvShift]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvShift]")[0], false);
        }
        if ($jQuery("[id$=rfvInsPre]")[0] != "" && $jQuery("[id$=rfvInsPre]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvInsPre]")[0], false);
        }
        if ($jQuery("[id$=rfvStartTime]")[0] != "" && $jQuery("[id$=rfvStartTime]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvStartTime]")[0], false);
        }
        if ($jQuery("[id$=rfvEndTime]")[0] != "" && $jQuery("[id$=rfvEndTime]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvEndTime]")[0], false);
        }
        if ($jQuery("[id$=rfvDeadlineDate]")[0] != "" && $jQuery("[id$=rfvDeadlineDate]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvDeadlineDate]")[0], false);
        }
        if ($jQuery("[id$=rfvDaysBefore]")[0] != "" && $jQuery("[id$=rfvDaysBefore]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvDaysBefore]")[0], false);
        }
        if ($jQuery("[id$=rfvFrequency]")[0] != "" && $jQuery("[id$=rfvFrequency]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvFrequency]")[0], false);
        }

    }

    function ClearDefaultValidator() {

        if ($jQuery("[id$=rfvDepartment]")[0] != "" && $jQuery("[id$=rfvDepartment]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvDepartment]")[0], false);
        }
        if ($jQuery("[id$=rfvProgram]")[0] != "" && $jQuery("[id$=rfvProgram]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvProgram]")[0], false);
        }
        if ($jQuery("[id$=rfvTerm]")[0] != "" && $jQuery("[id$=rfvTerm]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvTerm]")[0], false);
        }
        if ($jQuery("[id$=rfvCourse]")[0] != "" && $jQuery("[id$=rfvCourse]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvCourse]")[0], false);
        }
    }
    function OnKeyPress(sender, args) {
        if (args.get_keyCode() == 13) {
            args.get_domEvent().preventDefault();
            args.get_domEvent().stopPropagation();
        }
    }
    function OnClientItemChecked(sender, args) {
        if (sender.get_checkedItems().length == 0) {
            sender.clearSelection();
            sender.set_emptyMessage("--SELECT--");
        }
    }
    function ConfirmationMessage(msg, msgClass) {
        if (typeof (msg) == "undefined") return;
        var c = typeof (msgClass) != "undefined" ? msgClass : "";
        if ($jQuery(".approvepopup").length > 0) {
            $jQuery("#pageMsgBox").children("span").text(msg).attr("class", msgClass);
            if (c == 'sucs') {
                c = "Success";
            }
            else (c = c.toUpperCase());

            $jQuery("#pnlError").hide();

            $window.showDialog($jQuery("#pageMsgBox").clone().show(), {
                closeBtn: {
                    autoclose: true, text: "Ok"
                }
            }, 400, c);
        }
    }


    function openPopUp() {
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "CommonScreen";
        //var tenantId = 2;
        var tenantId = $jQuery("[id$=hdnTenantId]").val();
        if (tenantId != "0" && tenantId != "" && tenantId > 0) {
            $jQuery("[id$=hdnIsPagePostBack]").val("Focus Set");
            $jQuery("[id$=instituteHierarchy]").focusout();
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();

            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName=" + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);
            var win = $window.createPopup(url, { size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
        }
        return false;
    }

    function OnClientClose(oWnd, args) {
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
	
	function OpenAddEditRotationPopupUprPnl() {
        $jQuery("[id$=hdnRotationId]").val("-1");
        return OpenAddEditRotationPopup();
    }
    function OpenAddEditRotationPopup() {


        var tenantID = $jQuery("[id$=hdnTenantId]").val();
        if (tenantID != "0" && tenantID != "" && tenantID > 0) {

            var rotationId = $jQuery("[id$=hdnRotationId]").val();
            var popupWindowName = "Update Rotation";
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/AddEditRotation.aspx?TenantID=" + tenantID + "&ScreenMode=" + "" + "&RotationID=" + rotationId);
            var win = $window.createPopup(url, { size: "1000," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnCloseAddEditRotationPopup }
            );
        }
        else {
            alert("Please select Institution.");
        }
        return false;
    }
    function OnCloseAddEditRotationPopup(oWnd, args) {
        
        oWnd.remove_close(OnCloseAddEditRotationPopup);
        var arg = args.get_argument();
        if (arg) {
            if (arg.Action == "CloseAddEditPopup") { 
                $jQuery("[id$=hdnIsEditableByClientAdmin]").val(arg.IsEditableByClientAdmin);
                $jQuery("[id$=hdnIsEditableByAgencyUser]").val(arg.IsEditableByAgencyUser);
                $jQuery("[id$=hdnAgencyId]").val(arg.AgencyId);

                $jQuery("[id$=hdnClinicalRotationId]").val(arg.ClinicalRotationId);

                $jQuery("[id$=hdnTenantId]").val(arg.TenantId);

                $jQuery("[id$=hdnIsApplicantPkgNotAssignedThroughCloning]").val(arg.IsApplicantPkgNotAssignedThroughCloning);
                $jQuery("[id$=hdnIsInstructorPkgNotAssignedThroughCloning]").val(arg.IsInstructorPkgNotAssignedThroughCloning);


                $jQuery("[id$=btnRelod]").click();            
            }
        }
    }

</script>


<div id="dvTop" class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Manage Existing Rotations
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class='col-md-12'>
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class='reqd'>*</span>

                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" AutoPostBack="true"
                                DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                OnDataBound="ddlTenantName_DataBound" Enabled="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSearch" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
                        </div>
                        <%--  <div class='form-group col-md-3'>
                                &nbsp;
                            </div>--%>
                        <div class='form-group col-md-3' title="Select a Agency whose data you want to view.">
                            <%--  <span class="cptn">Agency</span>--%><%--<span class="reqd">*</span>--commented for UAT-2034--%>

                            <%--  <infs:WclComboBox ID="ddlAgency" runat="server" DataTextField="AgencyName" DataValueField="AgencyID"
                                AutoPostBack="false" OnDataBound="ddlAgency_DataBound" Width="100%" CssClass="form-control"
                                Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                            </infs:WclComboBox>--%>
                            <div style="margin-top: 5%">
                                <uc:AgencyHierarchyMultiple ID="ucAgencyHierarchyMultipleToSearchRotation" runat="server" />
                            </div>
                            <%--<div class="vldx">
                                 <asp:RequiredFieldValidator runat="server" ID="rfvAgency" ControlToValidate="ddlAgency"
                                    InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSearch"
                                    Text="Agency is required." />
                            </div>--%>
                        </div>
                    </div>
                </div>

            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='col-md-12' title="Click the link and select a node to restrict search results to the selected node">
                        <label class="cptn">Institution Hierarchy</label>
                        <a href="#" id="instituteHierarchy" onclick="openPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                        <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered Complio ID">
                        <span class="cptn">Complio ID</span>

                        <infs:WclTextBox ID="txtComplioId" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered rotation name">
                        <span class="cptn">Rotation ID/Name</span>

                        <infs:WclTextBox ID="txtRotationName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Type/Specialty">
                        <span class='cptn'>Type/Specialty</span>

                        <infs:WclTextBox ID="txtTypeSpecialty" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered department">
                        <span class="cptn">Department</span>

                        <infs:WclTextBox ID="txtDepartment" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered program">
                        <span class="cptn">Program</span>

                        <infs:WclTextBox ID="txtProgram" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the entered course">
                        <span class="cptn">Course</span>

                        <infs:WclTextBox ID="txtCourse" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the selected term">
                        <span class="cptn">Term</span>

                        <infs:WclTextBox ID="txtTerm" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the entered Unit/Floor or Location">
                        <span class="cptn">Unit/Floor or Location</span>

                        <infs:WclTextBox ID="txtUnit" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered Students">
                        <span class="cptn"># of Students</span>

                        <infs:WclNumericTextBox ID="txtStudents" runat="server" NumberFormat-DecimalDigits="2"
                            Width="100%" CssClass="form-control">
                        </infs:WclNumericTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Recommended Hours">
                        <span class="cptn"># of Recommended Hours</span>

                        <infs:WclNumericTextBox ID="txtRecommendedHrs" runat="server" NumberFormat-DecimalDigits="2"
                            Width="100%" CssClass="form-control">
                        </infs:WclNumericTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the selected days">
                        <span class="cptn">Days</span>

                        <infs:WclComboBox ID="ddlDays" runat="server" CheckBoxes="true" EmptyMessage="--SELECT--"
                            DataValueField="WeekDayID" DataTextField="Name" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the entered shift">
                        <span class="cptn">Shift</span>

                        <infs:WclTextBox ID="txtShift" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>


                </div>

            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered time range">
                        <span class="cptn">Time</span>

                        <infs:WclTimePicker ID="tpStartTime" runat="server" TimeView-Height="300px" Width="100%"
                            CssClass="form-control">
                            <TimeView CssClass="calanderFontSetting" Interval="00:15:00"></TimeView>
                        </infs:WclTimePicker>
                        <div class="gclrPad"></div>
                        <infs:WclTimePicker ID="tpEndTime" runat="server" TimeView-Height="300px" Width="100%"
                            CssClass="form-control">
                            <TimeView CssClass="calanderFontSetting" Interval="00:15:00"></TimeView>
                        </infs:WclTimePicker>
                    </div>
                    <div class="form-group col-md-3" title="Restrict search results to the entered start date">
                        <span class="cptn">Start Date</span>

                        <infs:WclDatePicker ID="dpStartDate" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnPopupOpening="SetMinDate" ClientEvents-OnDateSelected="CorrectStartToEndDate"
                            Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                    <div class="form-group col-md-3" title="Restrict search results to the entered end date">
                        <span class="cptn">End Date</span>

                        <infs:WclDatePicker ID="dpEndDate" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnPopupOpening="SetMinEndDate" Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the selected Instructor/Preceptor">
                        <span class="cptn">Instructor/Preceptor</span>

                        <infs:WclComboBox ID="ddlContacts" runat="server" EmptyMessage="--SELECT--" CheckBoxes="true"
                            DataValueField="ClientContactID" DataTextField="Name" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false" Filter="Contains">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select &#34All&#34 to view all rotations per the other parameters or &#34Archived&#34 to view only archived rotations or &#34Active&#34 to view only non archived rotations">
                        <span class="cptn">Rotation Archive Status</span>
                        <asp:RadioButtonList ID="rbArchiveStatus" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbArchiveStatus_SelectedIndexChanged"
                            DataTextField="As_Name" DataValueField="AS_Code" CssClass="radio_list" AutoPostBack="true">
                        </asp:RadioButtonList>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the selected Rotation Review Status(s)">
                        <span class="cptn">Rotation Review Status</span>
                        <infs:WclComboBox ID="ddlRotationReviewStatus" runat="server" DataTextField="SURRS_Name"
                            DataValueField="SURRS_ID" EmptyMessage="--Select--" CheckBoxes="true"
                            Filter="Contains" Width="100%" CssClass="form-control" Skin="Silk"
                            AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <%-- <asp:Panel ID="pnlCustomAttributes" runat="server"></asp:Panel>--%>
                <uc:CustomAttributes ID="caCustomAttributesID" EnableViewState="false" runat="server" />
            </div>
        </asp:Panel>
        <div class="col-md-12">&nbsp;</div>
        <div class="col-md-12">
            <div class="row ">
                <div style="float: left; width:50%;">
                    <%--UAT-3138--%>
                    <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Right" DisplayButtons="Submit,Save,Cancel"
                        AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo"
                        SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
                        CancelButtonText="Cancel" OnSubmitClick="fsucCmdBarButton_ResetClick" OnSaveClick="fsucCmdBarButton_SearchClick"
                        OnCancelClick="fsucCmdBarButton_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk">
                        <ExtraCommandButtons>
                            <%--UAT-3138
                            <infs:WclButton ID="btnArchive" runat="server" Text="Archive" Enabled="true" OnClick="btnArchive_Click" AutoSkinMode="false" Skin="Silk">
                            <Icon PrimaryIconCssClass="rbArchive"></Icon>
                        </infs:WclButton>--%>						
						            		
                        </ExtraCommandButtons>
                    </infsu:CommandBar>
                </div>
                <%--UAT-3138--%>
                <infs:WclMenu ID="cmdArchive" runat="server" Skin="Default" CssClass="top3" AutoSkinMode="false">
                    <Items>
                        <telerik:RadMenuItem Text="Archivemun">
                            <ItemTemplate>
                                <infs:WclButton runat="server" Text="Archive" ID="btnArchive" Icon-PrimaryIconCssClass="rbArchive" CssClass="btn"
                                    Skin="Silk" ButtonPosition="Center" AutoSkinMode="false" OnClick="btnArchive_Click">
                                </infs:WclButton>
                            </ItemTemplate>
                            <Items>
                                <telerik:RadMenuItem>
                                    <ItemTemplate>
                                        <infs:WclButton runat="server" Text="UnArchive" ID="btnUnArchive" Icon-PrimaryIconCssClass="rbUnArchive" CssClass="btn"
                                            Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnUnArchive_Click">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                </telerik:RadMenuItem>
                            </Items>
                        </telerik:RadMenuItem>
                    </Items>
                </infs:WclMenu>	
                	<infs:WclButton ID="btnAddNewRotationUprPnl" runat="server" CssClass="groupBtn btnSeprate" AutoSkinMode="false" Skin="Silk" 
								Text="Add New Rotation" Enabled="true" OnClientClicked="OpenAddEditRotationPopupUprPnl">
                        <Icon PrimaryIconCssClass="fa fa-plus-circle" SecondaryIconCssClass="" />
							</infs:WclButton>   
            </div>
            <div class="col-md-12">&nbsp;</div>
        </div>
    </div>
    <div id="dvColumnsConfiguration" style="display: none">
        <infsu:ColumnsConfiguration runat="server" ID="ColumnsConfiguration" />
    </div>
    <div class="row allowscroll">
        <div id="dvGrdRotations" runat="Server">
            <infs:WclGrid runat="server" GridCode="AAAC" ID="grdRotations" CssClass="gridhover containsColumnsConfiguration" AllowCustomPaging="true"
                AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
                AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
                OnNeedDataSource="grdRotations_NeedDataSource" OnItemCommand="grdRotations_ItemCommand"
                OnSortCommand="grdRotations_SortCommand" OnItemDataBound="grdRotations_ItemDataBound"
                OnItemCreated="grdRotations_ItemCreated"
                NonExportingColumns="ViewDetail,EditCommandColumn,DeleteColumn" EnableLinqExpressions="false"
                ShowClearFiltersButton="false">
                <ClientSettings EnableRowHoverStyle="true">
                    <ClientEvents OnKeyPress="OnKeyPress" />
                    <ClientEvents OnRowDblClick="grdRotation_rwDbClick" />
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="RotationID,AgencyID,AgencyHierarchyID,RootNodeID,AgencyIDs,IsEditableByClientAdmin,IsEditableByAgencyUser"
                    AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Rotation"
                        ShowExportToCsvButton="true"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                    <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="AssignCheckBox" AllowFiltering="false" ShowFilterIcon="false">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectRotation" runat="server" onclick="UnCheckHeader(this)"
                                    OnCheckedChanged="chkSelectRotation_CheckedChanged" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="AgencyName" HeaderText="Agency" SortExpression="AgencyName"
                            HeaderTooltip="This column displays the Agency name for each record in the grid"
                            UniqueName="AgencyName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="HierarchyNodes" HeaderText="Hierarchy" SortExpression="HierarchyNodes"
                            HeaderTooltip="This column displays the Hierarchy for each record in the grid" HeaderStyle-Width="250px"
                            UniqueName="HierarchyNodes">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ComplioID" HeaderText="Complio ID" SortExpression="RotationID"
                            UniqueName="ComplioID" HeaderTooltip="This column displays the Complio ID for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RotationName" HeaderText="Rotation ID/Name" SortExpression="RotationName"
                            UniqueName="RotationName" HeaderTooltip="This column displays the Rotation ID/Name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RotationReviewStatusName" HeaderText="Rotation Review Status" SortExpression="RotationReviewStatusName"
                            UniqueName="RotationReviewStatusName" HeaderTooltip="This column displays the Rotation Review Status for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DroppedDate" HeaderText="Dropped Date"
                        SortExpression="DroppedDate" DataFormatString="{0:MM/dd/yyyy}"
                        HeaderTooltip="This column displays the dropped date for each record in the grid"
                        UniqueName="DroppedDate">
                    </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TypeSpecialty" HeaderText="Type/Specialty" SortExpression="TypeSpecialty"
                            HeaderStyle-Width="100px"
                            UniqueName="TypeSpecialty" HeaderTooltip="This column displays the Type/Specialty for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Department" HeaderText="Department" SortExpression="Department"
                            HeaderStyle-Width="100px"
                            UniqueName="Department" HeaderTooltip="This column displays the department name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Program" HeaderText="Program"
                            SortExpression="Program" UniqueName="Program" HeaderTooltip="This column displays the program name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Course" HeaderText="Course" SortExpression="Course"
                            UniqueName="Course" HeaderTooltip="This column displays the course for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Term" HeaderText="Term" SortExpression="Term"
                            UniqueName="Term" HeaderTooltip="This column displays the term for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UnitFloorLoc" HeaderText="Unit/Floor" SortExpression="UnitFloorLoc"
                            UniqueName="unit" HeaderTooltip="This column displays the Unit/Floor for each record in the grid"
                            DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="Students" HeaderText="# of Students"
                            SortExpression="Students" HeaderStyle-Width="100px"
                            UniqueName="Students" HeaderTooltip="This column displays the # of Students for each record in the grid">
                            <ItemTemplate>
                               <a href="javascript:void(0)" style="color:blue;" runat="server" onclick="ShowNumberOfStudentsPopUp(this);" id="lnkNumberOfStudents"></a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="RecommendedHours" HeaderText="# of Recommended Hours"
                            SortExpression="RecommendedHours" HeaderStyle-Width="100px"
                            UniqueName="RecommendedHours" HeaderTooltip="This column displays the # of Recommended Hours for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DaysName" HeaderText="Days" AllowSorting="false"
                            UniqueName="Days" HeaderTooltip="This column displays days for each record in the grid">
                            <ItemStyle Wrap="true" Width="10px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Shift" HeaderText="Shift" SortExpression="Shift"
                            UniqueName="Shift" HeaderTooltip="This column displays shift for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Time" HeaderText="Time" AllowSorting="false"
                            HeaderStyle-Width="100px"
                            UniqueName="Time" HeaderTooltip="This column displays the time for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Created Date"
                            AllowSorting="true" SortExpression="CreatedDate" DataFormatString="{0:MM/dd/yyyy}"
                            UniqueName="CreatedDate" HeaderTooltip="This column displays the Created Date for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="StartDate" HeaderText="Start Date"
                            AllowSorting="true" SortExpression="StartDate" DataFormatString="{0:MM/dd/yyyy}"
                            UniqueName="StartDate" HeaderTooltip="This column displays the Start Date for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EndDate" HeaderText="End Date"
                            AllowSorting="true" SortExpression="EndDate" DataFormatString="{0:MM/dd/yyyy}"
                            UniqueName="EndDate" HeaderTooltip="This column displays the End Date for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ContactNames" HeaderText="Instructor/Preceptor"
                            AllowSorting="false" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Justify"
                            UniqueName="ContactName" HeaderTooltip="This column displays the Instructor/Preceptor for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CustomAttributes" FilterControlAltText="Filter CustomAttributes column"
                            AllowFiltering="false" HeaderText="Custom Attributes" AllowSorting="false" ItemStyle-Width="200px"
                            UniqueName="CustomAttributesGrd" HeaderTooltip="This column displays the Custom Attributes for each record in the grid">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="CustomAttributes" FilterControlAltText="Filter CustomAttributes column"
                            AllowFiltering="false" HeaderText="Custom Attributes" AllowSorting="false" ItemStyle-Width="200px"
                            UniqueName="CustomAttributes" HeaderTooltip="This column displays the Custom Attributes for each record in the grid">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="CustomAttributes" AllowFiltering="false" HeaderText="Custom Attributes"
                            AllowSorting="false" ItemStyle-Width="300px"
                            UniqueName="CustomAttributesTemp" Display="false">
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy"
                            HeaderTooltip="This column displays the Created By for each record in the grid"
                            UniqueName="CreatedBy">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="DeadlineDate" HeaderText="Deadline Date" Visible="false"
                            AllowSorting="false" SortExpression="DeadlineDate" DataFormatString="{0:MM/dd/yyyy}"
                            UniqueName="DeadlineDate" HeaderTooltip="This column displays the Deadline Date for each record in the grid">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="ViewDetail"
                                    ToolTip="Click here to view details of rotation." runat="server" Text="Detail">
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--  <telerik:GridButtonColumn ButtonType="LinkButton" HeaderTooltip="Click here to view details of rotation."
                            CommandName="ViewDetail" Text="Detail" 
                            ItemStyle-Width="100px" UniqueName="ViewDetail">
                        </telerik:GridButtonColumn>--%>
                        <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif"
                            CommandName="Delete" ConfirmText="Are you sure you want to delete this rotation?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif"
                            UniqueName="EditCommandColumn">
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridEditCommandColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-12">
                                        <h2 class="header-color">
                                            <asp:Label ID="lblRotation" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Rotation" : "Update Rotation" %>'
                                                runat="server" />

                                        </h2>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="msgbox">
                                            <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <asp:Panel ID="pnlEditForm" CssClass="editForm" runat="server">
                                    <div class="row bgLightGreen">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                                                    <span class="cptn">Institution</span><span class='reqd'>*</span>

                                                    <infs:WclComboBox ID="ddlTenant" runat="server" DataTextField="TenantName" AutoPostBack="true"
                                                        DataValueField="TenantID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                                        OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                                                        Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant"
                                                            InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                                            Text="Institution is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3' style="display: <%# (Container is GridEditFormInsertItem) ? "Block" : "none" %>" title="Select a Rotation you want to Clone.">
                                                    <span class="cptn lineHeight">Rotation</span>
                                                    <infs:WclComboBox ID="ddlRotation" runat="server" DataTextField="ComplioID" DataValueField="RotationID" ValidationGroup="grpCloneRotation"
                                                        AutoPostBack="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvRotation" ControlToValidate="ddlRotation"
                                                            InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpCloneRotation"
                                                            Text="Rotation is Required for Cloning." />
                                                    </div>
                                                    <div class="gclrPad"></div>
                                                    <infs:WclButton ID="btnClone" runat="server" Text="Clone Rotation" OnClick="btnClone_Click" AutoSkinMode="false" Skin="Silk" ValidationGroup="grpCloneRotation"></infs:WclButton>
                                                </div>

                                                <div class='form-group col-md-3' id="titleForAgencyHierarchy" title="Select a Agency whose data you want to view.">
                                                    <%--   <span class="cptn lineHeight">Agency</span><span class="reqd">*</span>
                                                    <infs:WclComboBox ID="ddlAgency" runat="server" DataTextField="AgencyName" DataValueField="AgencyID" OnSelectedIndexChanged="ddlAgency_SelectedIndexChanged"
                                                        AutoPostBack="true" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                    </infs:WclComboBox>
                                                    3<div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvAgency" ControlToValidate="ddlAgency"
                                                            InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Agency is required." />
                                                    </div>
                                                        <uc:AgencyHierarchyMultiple id="ucAgencyHierarchyAddRotationMultiple" runat="server"/>
                                                    --%>
                                                    <%-- <div style="margin-top: 5%">
                                                      <uc:AgencyHierarchy ID="ucAgencyHierarchyAddEditRotation" runat="server" />
                                                        <uc:AgencyHierarchyMultiple ID="ucAgencyHierarchyAddRotationMultiple" runat="server" />
                                                    </div>--%>

                                                    <div style="margin-top: 5%;" id="divAgencyHierarchyMulti" runat="server">
                                                        <uc:AgencyHierarchyMultiple runat="server" ID="ucAgencyHierarchyAddRotationMultiple" />
                                                        <asp:Label CssClass="errmsg" ID="lblAgencyErr" Text="Agency is required." Visible="false" runat="server" />
                                                    </div>
                                                </div>
                                                <div id="dvComplioId" runat="server">
                                                    <div class='form-group col-md-3'>
                                                        <span class="cptn">Complio ID</span>
                                                        <infs:WclTextBox CssClass="form-control" Width="100%" ID="txtComplioId" runat="server"
                                                            Text='<%# Eval("ComplioID") %>' ReadOnly="true">
                                                        </infs:WclTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-12'>
                                                    <span class="cptn">Hierarchy Nodes</span><span class='reqd'>*</span>
                                                    <a style="color: blue;" href="javascript:void(0)" id="lnkInstitutionHierarchyPB" runat="server" class="">Select Institution Hierarchy</a><br />
                                                    <asp:Label ID="lblInstitutionHierarchyPB" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn">Rotation ID/Name</span> <span id="spnRotationName" runat="server" class="controlHidden reqd">*</span>
                                                    <div id="dvRotationName" runat="server">
                                                        <infs:WclTextBox ID="txtClassification" runat="server" Text='<%# Eval("RotationName") %>'
                                                            Width="100%" CssClass="form-control">
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvRotationName" ControlToValidate="txtClassification"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Rotation ID/Name is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn">Type/Specialty</span> <span id="spnTypeSpecialty" runat="server" class="controlHidden reqd">*</span>
                                                    <div id="dvType" runat="server">
                                                        <infs:WclTextBox ID="txtTypeSpecialty" runat="server" MaxLength="256" Text='<%# Eval("TypeSpecialty") %>'
                                                            Width="100%" CssClass="form-control">
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvTypeSpecialtyAddEdit" ControlToValidate="txtTypeSpecialty"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Type/Specialty is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn">Department</span><span id="spnDepartment" class="reqd">*</span>
                                                    <div id="dvDepartment" runat="server">
                                                        <infs:WclTextBox ID="txtDepartment" runat="server" Text='<%# Eval("Department") %>'
                                                            Width="100%" CssClass="form-control">
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvDepartment" ControlToValidate="txtDepartment"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Department is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn">Program</span><span id="spnProgram" class="reqd">*</span>
                                                    <div id="dvProgram" runat="server">
                                                        <infs:WclTextBox ID="txtProgram" runat="server" Text='<%# Eval("Program") %>' Width="100%"
                                                            CssClass="form-control">
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvProgram" ControlToValidate="txtProgram"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Program is required." />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn">Course</span><span id="spnCourse" class="reqd">*</span>
                                                    <div id="dvCourse" runat="server">
                                                        <infs:WclTextBox ID="txtCourse" runat="server" Text='<%# Eval("Course") %>' Width="100%"
                                                            CssClass="form-control">
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvCourse" ControlToValidate="txtCourse"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Course is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3' title="Restrict search results to the selected term">
                                                    <span class="cptn">Term</span><span id="spnTerm" runat="server" class="controlHidden reqd">*</span>
                                                    <div id="dvTerm" runat="server">
                                                        <infs:WclTextBox ID="txtTerm" Text='<%# Eval("Term") %>' runat="server" Width="100%"
                                                            CssClass="form-control">
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvTerm" ControlToValidate="txtTerm"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Term is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn">Unit/Floor or Location</span><span id="spnUnitFloorLocation" runat="server" class="controlHidden reqd">*</span>
                                                    <div id="dvUnitLoc" runat="server">
                                                        <infs:WclTextBox ID="txtUnit" runat="server" Text='<%# Eval("UnitFloorLoc") %>' Width="100%"
                                                            CssClass="form-control">
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvUnitFloorLocation" ControlToValidate="txtUnit"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Unit/Floor or Location is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn"># of Students</span> <span id="spnStudent" runat="server" class="controlHidden reqd">*</span>
                                                    <div id="dvStudent" runat="server">
                                                        <infs:WclNumericTextBox ID="txtStudents" runat="server"
                                                            Text='<%# Eval("Students") %>' NumberFormat-DecimalDigits="2" Width="100%"
                                                            CssClass="form-control">
                                                        </infs:WclNumericTextBox>
                                                    </div>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvStudent" ControlToValidate="txtStudents"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Student(s) is required." />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn"># of Recommended Hours</span> <span id="spnRecommendedHrs" runat="server" class="controlHidden reqd">*</span>
                                                    <div id="dvHour" runat="server">
                                                        <infs:WclNumericTextBox ID="txtRecommendedHrs" runat="server"
                                                            Text='<%# Eval("RecommendedHours") %>' NumberFormat-DecimalDigits="2" Width="100%"
                                                            CssClass="form-control">
                                                        </infs:WclNumericTextBox>
                                                    </div>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvRecommendedHrs" ControlToValidate="txtRecommendedHrs"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Recommended Hour(s) is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn lineHeight">Days</span> <span id="spnDays" runat="server" class="controlHidden reqd">*</span>
                                                    <infs:WclComboBox ID="ddlDays" runat="server" OnClientItemChecked="OnClientItemChecked" OnClientBlur="OnClientItemChecked" CheckBoxes="true" DataValueField="WeekDayID"
                                                        DataTextField="Name"
                                                        Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvDays" ControlToValidate="ddlDays"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Day(s) is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn">Shift</span> <span id="spnShift" runat="server" class="controlHidden reqd">*</span>
                                                    <div id="dvShift" runat="server">
                                                        <infs:WclTextBox ID="txtShift" runat="server" Text='<%# Eval("Shift") %>' Width="100%"
                                                            CssClass="form-control">
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvShift" ControlToValidate="txtShift"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Shift is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn lineHeight">Instructor/Preceptor</span><span id="spnInsPre" class="controlHidden reqd" runat="server">*</span>
                                                    <infs:WclComboBox ID="ddlInstructor" runat="server" CheckBoxes="true" DataValueField="ClientContactID" EnableCheckAllItemsCheckBox="false"
                                                        DataTextField="Name" EmptyMessage="--SELECT--" Filter="Contains" OnClientBlur="OnClientItemChecked" OnClientItemChecked="OnClientItemChecked" OnClientKeyPressing="openCmbBoxOnTab" AutoPostBack="false"
                                                        Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" Localization-CheckAllString="Check All">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvInsPre" ControlToValidate="ddlInstructor"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Instructor/Preceptor is required." />
                                                    </div>
                                                    <%-- <div class='vldx'>
                                                        <asp:Label ID="lblError" runat="server" Style="display: none" CssClass="errmsg" Text="Instructor/Preceptor is required."></asp:Label>
                                                        <asp:CustomValidator ID="cstValidator" runat="server" ErrorMessage="Instructor/Preceptor is required." CssClass="errmsg" Display="Dynamic"
                                                            ClientValidationFunction="CheckForInsPreceptor" ClientIDMode="Static" ValidationGroup="grpFormSubmit" Enabled="false"></asp:CustomValidator>
                                                    </div>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <%--  <div class="sxlb">
                                                    <span class="cptn">Agency</span>
                                                </div>
                                                <div class="sxlm">
                                                   <asp:Label ID="lblAgencyName" runat="server"></asp:Label>
                                                </div>--%>
                                            <div class="row">
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn">Time</span> <span id="spnTime" class="controlHidden reqd" runat="server">*</span>
                                                    <infs:WclTimePicker ID="tpStartTime" runat="server" DateInput-EmptyMessage="Start Time"
                                                        TimeView-Height="300px" Width="100%" CssClass="form-control">
                                                        <TimeView Interval="00:30:00"></TimeView>
                                                    </infs:WclTimePicker>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvStartTime" ControlToValidate="tpStartTime"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Start Time is required." />
                                                    </div>
                                                    <div class="gclrPad"></div>
                                                    <infs:WclTimePicker ID="tpEndTime" DateInput-EmptyMessage="End Time" runat="server"
                                                        Width="100%" CssClass="form-control">
                                                        <TimeView Interval="00:30:00"></TimeView>
                                                    </infs:WclTimePicker>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvEndTime" ControlToValidate="tpEndTime"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="End Time is required." />
                                                    </div>
                                                    <div class="vldx">
                                                        <asp:CustomValidator ID="cstStartFrm" runat="server" ErrorMessage="EndTime Is Required."
                                                            ValidationGroup="grpFormSubmit"
                                                            CssClass="errmsg" Display="Dynamic" ClientValidationFunction="ValidateStartEndTime"
                                                            ClientIDMode="Static" SetFocusOnError="True">
                                                        </asp:CustomValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group col-md-3">
                                                    <span class="cptn">Start Date</span><span class="reqd">*</span>

                                                    <infs:WclDatePicker ID="dpStartDate" runat="server" DateInput-EmptyMessage="Select a date"
                                                        ClientEvents-OnPopupOpening="SetMinDate" ClientEvents-OnDateSelected="CorrectStartToEndDateOnAdd"
                                                        Width="100%" CssClass="form-control">
                                                    </infs:WclDatePicker>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvStartDate" ControlToValidate="dpStartDate"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Start Date is required." />

                                                    </div>
                                                </div>
                                                <div class="form-group col-md-3">
                                                    <span class="cptn">End Date</span><span class="reqd">*</span>

                                                    <infs:WclDatePicker ID="dpEndDate" runat="server" DateInput-EmptyMessage="Select a date"
                                                        ClientEvents-OnPopupOpening="SetMinEndDateOnAdd" Width="100%" CssClass="form-control">
                                                    </infs:WclDatePicker>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvEndDate" ControlToValidate="dpEndDate"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="End Date is required." />
                                                        <asp:CompareValidator ID="CfvEndDate" CssClass="errmsg" ControlToValidate="dpEndDate" ControlToCompare="dpStartDate" runat="server"
                                                            ErrorMessage="End Date should not be less than Start Date" Operator="GreaterThan" ValidationGroup="grpFormSubmit"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn lineHeight">Syllabus Document</span> <span id="spnSyllabus" class="controlHidden reqd" runat="server">*</span>
                                                    <div class="bx_uploader" title="Click this button to upload syllabus">
                                                        <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay"
                                                            MultipleFileSelection="Disabled" MaxFileInputsCount="1" OnClientFileSelected="onClientFileSelected"
                                                            OnClientFileUploaded="onFileUploaded" OnClientValidationFailed="upl_OnClientValidationFailed"
                                                            Localization-Select="Browse" Width="100%" CssClass="form-control marginTop2"
                                                            AutoSkinMode="true"
                                                            AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,JPG,gif,tif,tiff,docx,doc,rtf,pdf,odt,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,JPG,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT" />
                                                    </div>
                                                    <div style="clear: both; float: left; position: relative;">
                                                        <asp:Label ID="lblUploadFormName" runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblUploadFormPath" runat="server" Visible="false"></asp:Label>
                                                        <asp:LinkButton ID="lnkRemove" runat="server" Text="Remove" Visible="false" OnClick="lnkRemove_Click"
                                                            ToolTip="Click this button to remove document"></asp:LinkButton>
                                                    </div>
                                                    <asp:Label ID="lblSyllabusDocumentError" runat="server" Style="display: none" CssClass="errmsg" Text="Syllabus Document is required."></asp:Label>
                                                    <%--  <asp:CustomValidator ID="cstValidator" runat="server" ErrorMessage="Syllabus Document is required." CssClass="errmsg" Display="Dynamic"
                                                        ClientValidationFunction="validateUpload" ClientIDMode="Static" ValidationGroup="grpFormSubmit" Enabled="false"></asp:CustomValidator>--%>
                                                    <%--<asp:CustomValidator runat="server" ID="CustomSyllabusDocumentValidator" ClientValidationFunction="validateUpload" ErrorMessage="Syllabus is required">
                                                    </asp:CustomValidator>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn">Deadline Date</span> <span id="spnDeadlineDate" class="controlHidden reqd" runat="server">*</span>
                                                    <infs:WclDatePicker ID="dpDeadlineDate" runat="server" DateInput-EmptyMessage="Select a date"
                                                        Width="100%" CssClass="form-control">
                                                    </infs:WclDatePicker>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvDeadlineDate" ControlToValidate="dpDeadlineDate"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Deadline Date is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3' style="display: none;">
                                                    <span class="cptn">Allow Notification</span>
                                                    <infs:WclButton runat="server" ID="chkAllowNotification" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                        AutoPostBack="false" Checked="true" ToolTip="If this setting is checked, then “Rotation Item Submitted for Review” notification is sent to the admin/school admin who creates rotation.">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn">Editable By</span>
                                                    <asp:CheckBoxList Style="height: 50px !important;" ID="chkEditPermission" runat="server" CssClass="form-control" RepeatColumns="4" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="Client Admin" Value="CA" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Agency User" Value="AGU" Selected="True"></asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h2 class="header-color">Nag Notification Settings</h2>
                                        </div>
                                    </div>
                                    <div class="row bgLightGreen">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn lineHeight">Days Before</span> <span id="spnDaysBefore" class="controlHidden reqd" runat="server">*</span>

                                                    <infs:WclNumericTextBox ID="txtDaysBefore" runat="server" MaxLength="3" Text='<%# Eval("DaysBefore") %>'
                                                        Width="100%" CssClass="form-control">
                                                        <NumberFormat AllowRounding="false" DecimalDigits="0" />
                                                    </infs:WclNumericTextBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvDaysBefore" ControlToValidate="txtDaysBefore"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Days Before is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn lineHeight">Frequency</span> <span id="spnFrequency" class="controlHidden reqd" runat="server">*</span>

                                                    <infs:WclNumericTextBox ID="txtFrequency" runat="server" MaxLength="3" Text='<%# Eval("Frequency") %>'
                                                        Width="100%" CssClass="form-control">
                                                        <NumberFormat AllowRounding="false" DecimalDigits="0" />
                                                    </infs:WclNumericTextBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvFrequency" ControlToValidate="txtFrequency"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Frequency is required." />
                                                    </div>
                                                </div>
                                                <%--<div class="vldx">
                                                    <asp:CustomValidator ID="cstFrequenc" runat="server"
                                                        ValidationGroup="grpFormSubmit" ControlToValidate="txtFrequency"
                                                        CssClass="errmsg" Display="Dynamic" ClientValidationFunction="ValidateDaysBeforeFrequency"
                                                        ClientIDMode="Static" SetFocusOnError="True">
                                                    </asp:CustomValidator>
                                                </div>--%>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <infsu:CommandBar ID="fsucCmdBarRotation" runat="server" GridMode="true" DefaultPanel="pnlCategory"
                                    GridInsertText="Save" GridUpdateText="Save"
                                    ValidationGroup="grpFormSubmit" UseAutoSkinMode="false" ButtonSkin="Silk" />
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                        Position="TopAndBottom" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
    </div>
    <div class="row text-center" style="padding-top: 10px;">
        <infsu:CommandBar ID="cmdAssignBtns" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel,Extra"
            AutoPostbackButtons="Submit,Save,Cancel,Extra" SubmitButtonIconClass="" ExtraButtonText="Upload Syllabus" ExtraButtonIconClass=""
            SubmitButtonText="Assign Student Package" SaveButtonText="Assign Preceptor Package" SaveButtonIconClass=""
            CancelButtonText="Assign Preceptor" CancelButtonIconClass="" UseAutoSkinMode="false" ButtonSkin="Silk"
            OnSubmitClick="cmdAssignBtns_AssignStudentPkgClick" OnSaveClick="cmdAssignBtns_AssignPreceptorPkgClick" OnCancelClick="cmdAssignBtns_AssignPreceptorClick"
            OnExtraClick="cmdAssignBtns_UploadSyllabusClick">
        </infsu:CommandBar>
    </div>
    <asp:HiddenField ID="hdnFileRemoved" runat="server" />

    <asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
    <asp:HiddenField ID="hdnTenantIdNew" runat="server" Value="" />
    <asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
    <asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />
    <asp:HiddenField ID="hdnDepartmentProgmapNew" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstNodeIdNew" runat="server" Value="" />
    <asp:HiddenField ID="hdnCurrentRotStartDate" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstNodeLabel" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstituteNodelabel" runat="server" Value="" />
    <asp:HiddenField ID="hdnValidateFileUploadControl" runat="server" Value="" />
    <asp:HiddenField ID="hdnRotationId" runat="server" Value="" />
    <asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
      <div style="display: none">
        <asp:Button ID="btnRelod" runat="server" OnClick="btnRelod_Click"></asp:Button>
    </div>
    <asp:HiddenField ID="hdnIsEditableByClientAdmin" runat="server" Value="" />
<asp:HiddenField ID="hdnIsEditableByAgencyUser" runat="server" Value="" />
<asp:HiddenField ID="hdnAgencyId" runat="server" Value="" />
    <asp:HiddenField ID="hdnClinicalRotationId" runat="server" Value="" />
<asp:HiddenField ID="HiddenField1" runat="server" Value="" />
<asp:HiddenField ID="hdnIsApplicantPkgNotAssignedThroughCloning" runat="server" Value="" />
<asp:HiddenField ID="hdnIsInstructorPkgNotAssignedThroughCloning" runat="server" Value="" />
    <asp:HiddenField ID="hdnIsPagePostBack" runat="server" Value="" />
    <div id="dvInfoPopUp" class="approvepopup" runat="server" style="display: none">
    </div>
</div>

<%--UAT-3241--%>
<div id="multipleHierarchyPopUpDiv" class="msgbox" runat="server" style="display: none">
    <asp:Label CssClass="info" ID="lblAttention" runat="server"></asp:Label>
</div>


<%--UAT-3221--%>
<script type="text/javascript">
    function markAgencyHierarchyLinkDisabled() {
        setTimeout(function () {
            $jQuery(".editForm [id=AgencyHierarchy]").attr('class', 'agencyHierarchyLinkDisabled');
            $jQuery(".editForm [id=titleForAgencyHierarchy]").removeAttr('title');
        }, 200);
    }

    //UAT-3682
    function ShowNumberOfStudentsPopUp(obj) {
        var args = $jQuery(obj).attr('args');
        if (args != null && args != "") {
            var popupHeight = $jQuery(window).height() * (100 / 100);
            var url = $page.url.create("~/ClinicalRotation/Pages/RotationStudentDetailPopup.aspx?args=" + args);
            var win = $window.createPopup(url, { size: "625," + popupHeight / 3, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize | Telerik.Web.UI.WindowBehaviors.Modal });
            return false;
        }
    }

</script>
