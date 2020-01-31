<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddEditRotation.ascx.cs" Inherits="CoreWeb.ClinicalRotation.Views.AddEditRotation" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<%@ Register TagName="UploadDocuments" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/UploadDocuments.ascx" %>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchyMultiple" Src="~/AgencyHierarchy/UserControls/AgencyHierarchyMultipleSelection.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxClinicalRotationMappingPopup">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Generic/popup.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/ClinicalRotation/ManageRotation.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/AgencyHierarchy/AgencyHierarchyMultipleSelection.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    .controlHidden {
        display: none;
    }
    /*UAT-3221*/
    .agencyHierarchyLinkDisabled {
        color: gray !important;
        text-decoration: none !important;
        cursor: default !important;
    }

    .disabled {
        pointer-events: none !important;
        cursor: default;
        text-decoration: none;
        color: gray !important;
    }
</style>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <asp:Label ID="lblRotation" Text="Add New Rotation" runat="server" />
            </h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div id="DivMsgBox" class="msgbox">
                <asp:Label ID="lblMsg" runat="server" CssClass="info"></asp:Label>
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlEditForm" runat="server">
        <div class="row bgLightGreen">
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3 col-sm-3' title="Select the Institution whose data you want to view">
                        <span class="cptn">Institution</span><span class='reqd'>*</span>
                        <infs:WclComboBox ID="ddlTenant" runat="server" DataTextField="TenantName" AutoPostBack="true"
                            DataValueField="TenantID" Filter="None" OnClientKeyPressing="openCmbBoxOnTab"
                            OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant" Enabled="true"
                                InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                Text="Institution is required." />
                        </div>
                    </div>
                    <div class='form-group col-md-3 col-sm-3' title="Select a Rotation you want to Clone." id="dvCloneRotation" runat="server">
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

                        <infs:WclCheckBox CssClass="form-control" ID="chkCopyStudentusingClone" runat="server" style="font-size:13px" Text="Copy students to new rotation" />
                        
                        <infs:WclButton ID="btnClone" runat="server" Text="Clone Rotation" OnClick="btnClone_Click" AutoSkinMode="false" Skin="Silk" ValidationGroup="grpCloneRotation"></infs:WclButton>
                    </div>
                     
                    <div class="clearfix" id="divClearFix" runat="server"></div>
                    <div class="form-group col-md-6 col-sm-6" title="Select a Agency whose data you want to view." id="divAgencyHierarchy" runat="server">
                        <%--span class="cptn lineHeight">Agency</span><span class="reqd">*</span>
                        <infs:WclComboBox ID="ddlAgency" runat="server" DataTextField="AgencyName" DataValueField="AgencyID" OnSelectedIndexChanged="ddlAgency_SelectedIndexChanged"
                            AutoPostBack="true" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvAgency" ControlToValidate="ddlAgency" Enabled="true"
                                InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                Text="Agency is required." />
                        </div>--%>

                        <uc:AgencyHierarchyMultiple ID="ucAgencyHierarchyMultiple" runat="server" />
                    </div>
                    <div id="dvComplioId" runat="server">
                        <div class='form-group col-md-3 col-sm-3'>
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
                        <%-- <a style="color: blue;" href="#" id="lnkInstitutionHierarchyPB" runat="server"  onclick="OpenInstitutionHierarchyPopupInsideGrid(true);">Select Institution Hierarchy</a><br />--%>
                        <a style="color: blue;" href="javascript:void(0)" id="lnkInstitutionHierarchyPB" runat="server" class="">Select Institution Hierarchy</a><br />
                        <asp:Label ID="lblInstitutionHierarchyPB" runat="server"></asp:Label>
                    </div>
                </div>
            </div>

            <div id="dvInstAvailability" runat="server" class="col-md-12" style="display: none">
                <div class="row">
                    <div class='form-group col-md-9 col-sm-9'>
                        <span class="cptn">Will the school be sending an instructor for this rotation?</span>
                        <asp:RadioButton ID="rdbInstAvailabileYes" runat="server" GroupName="InstAvailabile" Text="Yes" OnCheckedChanged="rdbInstAvailabile_CheckedChanged"  AutoPostBack="true"/>
                        <asp:RadioButton ID="rdbInstAvailabileNo" runat="server" GroupName="InstAvailabile" Text="No" Checked="true" OnCheckedChanged="rdbInstAvailabile_CheckedChanged" AutoPostBack="true"/>
                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3 col-sm-3'>
                        <span class="cptn">Rotation ID/Name</span> <span id="spnRotationName" runat="server" class="controlHidden reqd">*</span>
                        <div id="dvRotationName" runat="server">
                            <infs:WclTextBox ID="txtRotationName" runat="server" Text='<%# Eval("RotationName") %>'
                                Width="100%" CssClass="form-control">
                            </infs:WclTextBox>
                        </div>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvRotationName" ControlToValidate="txtRotationName"
                                Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                Text="Rotation ID/Name is required." />
                        </div>
                    </div>
                    <div class='form-group col-md-3 col-sm-3'>
                        <span class="cptn">Type/Specialty</span> <span id="spnTypeSpecialty" runat="server" class="controlHidden reqd">*</span>
                        <div id="dvType" runat="server">
                            <infs:WclTextBox ID="txtTypeSpecialty" runat="server" MaxLength="256" Text='<%# Eval("TypeSpecialty") %>'
                                Width="100%" CssClass="form-control">
                            </infs:WclTextBox>
                            <infs:WclComboBox ID="cmbTypeSpecialty" runat="server" CheckBoxes="false" DataValueField="MappingID" Filter="Contains"
                                DataTextField="MappingValue" OnClientKeyPressing="openCmbBoxOnTab" AutoPostBack="false"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                        </div>
                        <%-- <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvCombTypeSpecialtyAddEdit" ControlToValidate="cmbTypeSpecialty"
                                Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"  InitialValue="--SELECT--"
                                Text="Type/Specialty is required." />
                        </div>--%>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTypeSpecialtyAddEdit"
                                Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit" InitialValue=""
                                Text="Type/Specialty is required." />
                        </div>
                    </div>
                    <div class='form-group col-md-3 col-sm-3'>
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
                    <div class='form-group col-md-3 col-sm-3'>
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
                    <div class='form-group col-md-3 col-sm-3'>
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
                    <div class='form-group col-md-3 col-sm-3' title="Restrict search results to the selected term">
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
                    <div class='form-group col-md-3 col-sm-3'>
                        <span class="cptn">Unit/Floor or Location</span><span id="spnUnitFloorLocation" runat="server" class="controlHidden reqd">*</span>
                        <div id="dvUnitLoc" runat="server">
                            <infs:WclTextBox ID="txtUnit" runat="server" Text='<%# Eval("UnitFloorLoc") %>' Width="100%" MaxLength="100"
                                CssClass="form-control">
                            </infs:WclTextBox>
                        </div>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvUnitFloorLocation" ControlToValidate="txtUnit"
                                Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                Text="Unit/Floor or Location is required." />
                        </div>
                    </div>
                    <div class='form-group col-md-3 col-sm-3'>
                        <span class="cptn"># of Students</span> <span id="spnStudent" runat="server" class="controlHidden reqd">*</span>
                        <div id="dvStudent" runat="server">
                            <infs:WclNumericTextBox ID="txtStudents" runat="server"
                                text='<%# Eval("Students") %>' NumberFormat-DecimalDigits="0" NumberFormat-AllowRounding="false" Width="100%"
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
                    <div class='form-group col-md-3 col-sm-3'>
                        <span class="cptn"># of Recommended Hours</span><span id="spnRecommendedHrs" runat="server" class="controlHidden reqd">*</span>
                        <div id="dvHour" runat="server">
                            <infs:WclNumericTextBox ID="txtRecommendedHrs" runat="server"
                                text='<%# Eval("RecommendedHours") %>' NumberFormat-DecimalDigits="2" Width="100%"
                                CssClass="form-control">
                            </infs:WclNumericTextBox>
                        </div>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvRecommendedHrs" ControlToValidate="txtRecommendedHrs"
                                Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                Text="Recommended Hour(s) is required." />
                        </div>
                    </div>
                    <div class='form-group col-md-3 col-sm-3'>
                        <span class="cptn lineHeight">Days</span><span id="spnDays" runat="server" class="controlHidden reqd">*</span>
                        <infs:WclComboBox ID="ddlDays" runat="server" OnClientBlur="OnClientItemChecked" CheckBoxes="true" DataValueField="WeekDayID"
                            DataTextField="Name"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvDays" ControlToValidate="ddlDays"
                                Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                Text="Day(s) is required." />
                        </div>
                    </div>
                    <div class='form-group col-md-3 col-sm-3'>
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
                    <div class='form-group col-md-3 col-sm-3'>
                        <span class="cptn lineHeight">Instructor/Preceptor</span><span id="spnInsPre" class="controlHidden reqd" runat="server">*</span>
                        <infs:WclComboBox ID="ddlInstructor" runat="server" CheckBoxes="true" DataValueField="ClientContactID"
                            DataTextField="Name" OnClientBlur="OnClientItemChecked" OnClientItemChecked="OnClientItemChecked" EnableCheckAllItemsCheckBox="false" EmptyMessage="--SELECT--" Filter="Contains"
                            OnClientKeyPressing="openCmbBoxOnTab" AutoPostBack="false"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
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
                <div class="row">
                    <div class='form-group col-md-3 col-sm-3'>
                        <span class="cptn">Time</span><span id="spnTime" class="controlHidden reqd" runat="server">*</span>
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
                        <infs:WclTimePicker ID="tpEndTime" DateInput-EmptyMessage="End Time" runat="server" TimeView-Height="300px"
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
                    <div class="form-group col-md-3 col-sm-3">
                        <span class="cptn">Start Date</span><span class="reqd">*</span>

                        <infs:WclDatePicker ID="dpStartDate" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnPopupOpening="SetMinDate" ClientEvents-OnDateSelected="CorrectStartToEndDate"
                            Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvStartDate" ControlToValidate="dpStartDate"
                                Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                Text="Start Date is required." />
                        </div>
                    </div>
                    <div class="form-group col-md-3 col-sm-3">
                        <span class="cptn">End Date</span><span class="reqd">*</span>

                        <infs:WclDatePicker ID="dpEndDate" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnPopupOpening="SetMinEndDate" Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvEndDate" ControlToValidate="dpEndDate"
                                Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                Text="End Date is required." />
                            <asp:CompareValidator ID="CfEndDate" CssClass="errmsg" ControlToValidate="dpEndDate" ControlToCompare="dpStartDate" runat="server"
                                ErrorMessage="End Date should not be less than Start Date" Operator="GreaterThan" ValidationGroup="grpFormSubmit"></asp:CompareValidator>
                        </div>
                    </div>
                    <div class='form-group col-md-3 col-sm-3'>
                        <span class="cptn lineHeight">Syllabus Document</span><span id="spnSyllabus" class="controlHidden reqd" runat="server">*</span>

                        <div class="bx_uploader" title="Click this button to upload syllabus">
                            <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay"
                                MultipleFileSelection="Disabled" MaxFileInputsCount="1" OnClientFileSelected="onClientFileSelected" OnClientFilesUploaded="OnClientFilesUploadedSyll"
                                OnClientFileUploadRemoved="OnClientFileUploadRemovedSyll"
                                OnClientFileUploaded="onFileUploaded" OnClientValidationFailed="upl_OnClientValidationFailed"
                                Localization-Select="Browse" Localization-Cancel="Cancel" Localization-Remove="Remove" Width="100%" CssClass="form-control marginTop2"
                                AutoSkinMode="true"
                                AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,JPG,gif,tif,tiff,docx,doc,rtf,pdf,odt,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,JPG,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT" />
                        </div>
                        <div style="clear: both; float: left; position: relative;">
                            <asp:Label ID="lblUploadFormName" ClientIDMode="Static" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblUploadFormPath" runat="server" Visible="false"></asp:Label>
                            <%--OnClick="lnkRemove_Click"--%>
                            <asp:LinkButton ID="lnkRemove" runat="server" Text="Remove" OnClick="lnkRemove_Click" Visible="false"
                                ToolTip="Click this button to remove document"></asp:LinkButton>
                        </div>
                        <asp:Label ID="lblSyllabusDocumentError" runat="server" Style="display: none" CssClass="errmsg" Text="Syllabus Document is required."></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-12" style="clear: both;">
                <div class="row">
                    <div class='form-group col-md-3 col-sm-3'>
                        <span class="cptn">Deadline Date</span><span id="spnDeadlineDate" class="controlHidden reqd" runat="server">*</span>
                        <infs:WclDatePicker ID="dpDeadlineDate" runat="server" DateInput-EmptyMessage="Select a date"
                            Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvDeadlineDate" ControlToValidate="dpDeadlineDate"
                                Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                Text="Deadline Date is required." />
                        </div>
                    </div>
                    <div class='form-group col-md-3 col-sm-3' style="display: none;">
                        <span class="cptn">Allow Notification</span>
                        <infs:WclButton runat="server" ID="chkAllowNotification" ToggleType="CheckBox" ButtonType="ToggleButton"
                            AutoPostBack="false" Checked="true" ToolTip="If this setting is checked, then “Rotation Item Submitted for Review” notification is sent to the admin/school admin who creates rotation.">
                            <ToggleStates>
                                <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                <telerik:RadButtonToggleState Text="No" Value="False" />
                            </ToggleStates>
                        </infs:WclButton>
                    </div>
                    <div class='form-group col-md-3 col-sm-3'>
                        <span class="cptn">Editable By</span>
                        <asp:CheckBoxList Style="height: 50px !important;" ID="chkEditPermission" runat="server" CssClass="form-control" RepeatColumns="4" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Client Admin" Value="CA" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Agency User" Value="AGU" Selected="True"></asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                    <div class='form-group col-md-6 col-sm-6'>
                        <span class="cptn lineHeight">Additional Document(s)</span><span id="spnAdditional" class="controlHidden reqd" runat="server">*</span>
                        <div class="AdditionalDocument">
                            <asp:Panel runat="server" ID="PnlAdditionalDocument">
                                <%-- <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="Label2" runat="server" Visible="false"></asp:Label>
                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Remove" OnClick="lnkRemove_Click" Visible="false"
                                ToolTip="Click this button to remove document"></asp:LinkButton>--%>
                            </asp:Panel>
                        </div>
                        <div class="AdditionalDocument"></div>
                        <div class="bx_uploader">

                            <infs:WclAsyncUpload runat="server" ID="UpLoadAdditionalDocuments" HideFileInput="true" Skin="Hay" ClientIDMode="Static"
                                MultipleFileSelection="Automatic" OnClientFileSelected="onClientFileSelected" OnClientFilesUploaded="OnClientFilesUploadedAdditional"
                                OnClientFileUploaded="onFileUploaded" OnClientValidationFailed="upl_OnClientValidationFailed" OnClientFileUploadRemoved="OnClientFileUploadRemovedAdditional"
                                Localization-Select="Browse" Width="100%" CssClass="marginTop2"
                                AutoSkinMode="true" PostbackTriggers="btnSubmit" Height="100%"
                                AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,JPG,gif,tif,tiff,docx,doc,rtf,pdf,odt,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,JPG,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT" />
                        </div>

                        <div class="AdditionalDocument" style="clear: both; float: left;"></div>
                        <asp:Label ID="lblAdditionalDocumentsRequired" runat="server" Style="display: none" CssClass="errmsg" Text="At least one Additional Document is required."></asp:Label>

                    </div>

                </div>
            </div>

        </div>
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Nag Notification Settings</h2>
            </div>
        </div>
        <div id="DivNotificationSettings" class="row bgLightGreen">
            <div class="col-md-6">
                <div class="row">
                    <div class='form-group col-md-3 col-sm-3'>
                        <span class="cptn lineHeight">Days Before</span><span id="spnDaysBefore" class="controlHidden reqd" runat="server">*</span>

                        <infs:WclNumericTextBox ID="txtDaysBefore" runat="server" MaxLength="3" text='<%# Eval("DaysBefore") %>'
                            Width="100%" CssClass="form-control">
                            <NumberFormat AllowRounding="false" DecimalDigits="0" />
                        </infs:WclNumericTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvDaysBefore" ControlToValidate="txtDaysBefore"
                                Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                Text="Days Before is required." />
                        </div>
                    </div>
                    <div class='form-group col-md-3 col-sm-3'>
                        <span class="cptn lineHeight">Frequency</span><span id="spnFrequency" class="controlHidden reqd" runat="server">*</span>
                        <infs:WclNumericTextBox ID="txtFrequency" runat="server" MaxLength="3" text='<%# Eval("Frequency") %>'
                            Width="100%" CssClass="form-control">
                            <NumberFormat AllowRounding="false" DecimalDigits="0" />
                        </infs:WclNumericTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvFrequency" ControlToValidate="txtFrequency"
                                Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                Text="Frequency is required." />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
<div class="col-md-12">&nbsp;</div>
<div class="col-md-12">
    <div class="row">
        <infsu:CommandBar ID="fsucCmdBarRotation" runat="server" DisplayButtons="Submit,Cancel" DefaultPanel="pnlEditForm" AutoPostbackButtons="Submit"
            SubmitButtonText="Save" OnSubmitClick="fsucCmdBarRotation_SaveClick" SubmitButtonIconClass="rbSave" OnSubmitClientClick="ValidatePageControls" OnCancelClientClick="ClosePopup" CancelButtonText="Cancel"
            ValidationGroup="grpFormSubmit" UseAutoSkinMode="false" ButtonSkin="Silk" />
    </div>
</div>
<%--UAT-4147--%>
<div id="divExistingRotationMembers" class="acknowledgeMessagePopup" title="Complio" runat="server" style="display: none">
    <p style="text-align: left;">One or more of the selected Instructor/Preceptor is already added as Applicant. Please review your selection: </p>
    <div>&nbsp;</div>
    <asp:Panel ID="pnlExistingRotationMembers" runat="server">
    </asp:Panel>
</div>
<div class="col-md-12">&nbsp;</div>
<asp:HiddenField ID="hdnTempAdditionalDocumentIds" runat="server" />
<asp:HiddenField ID="hdnFileRemoved" runat="server" />
<asp:HiddenField ID="hdnDepartmentProgmapNew" runat="server" Value="" />
<asp:HiddenField ID="hdnInstNodeLabel"  runat="server" Value="" />
<asp:HiddenField ID="hdnInstNodeIdNew" runat="server" Value="" />
<asp:HiddenField ID="hdnSelectRootNodeID" runat="server" Value="" />
<asp:HiddenField ID="hdnIsFromStudentBulkAssignmentScreen" runat="server" Value="1" />
<asp:HiddenField ID="hdnValidateFileUploadControl" runat="server" Value="" />
<asp:HiddenField ID="hdnValidateAdditionalControl" runat="server" Value="" />
<asp:HiddenField ID="hdnchkRotationFieldOptionValidator" runat="server" Value="" />
<asp:HiddenField ID="hdnIsEditableByClientAdmin" runat="server" Value="" />
<asp:HiddenField ID="hdnIsEditableByAgencyUser" runat="server" Value="" />
<asp:HiddenField ID="hdnAgencyId" runat="server" Value="" />
<asp:HiddenField ID="hdnClinicalRotationId" runat="server" Value="" />
<asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
<asp:HiddenField ID="hdnIsApplicantPkgNotAssignedThroughCloning" runat="server" Value="" />
<asp:HiddenField ID="hdnIsInstructorPkgNotAssignedThroughCloning" runat="server" Value="" />
<asp:HiddenField ID="hdnIsInstAvailabilityDefined" runat="server" Value="" />
<%--UAT-3241--%>
<div id="multipleHierarchyPopUpDiv" class="msgbox" runat="server" style="display: none">
    <asp:Label CssClass="info" ID="lblAttention" runat="server"></asp:Label>
</div>


<script type="text/javascript">

    var CountOfSyllabusFiles = 0;
    var CountOfAdditionalFiles = 0;
    function OnClientFileUploadRemovedAdditional(sender) {
        CountOfAdditionalFiles = sender._uploadedFiles.length;
    }
    function OnClientFilesUploadedAdditional(sender) {
        CountOfAdditionalFiles = sender._uploadedFiles.length;
    }
    function OnClientFileUploadRemovedSyll(sender) {
        CountOfSyllabusFiles = sender._uploadedFiles.length;
    }
    function OnClientFilesUploadedSyll(sender) {
        CountOfSyllabusFiles = sender._uploadedFiles.length;
    }
    function RemoveRecords(id, e, lblId) {
        var TempValue = '';
        var TempValues = $jQuery("[id$=hdnTempAdditionalDocumentIds]").val();
        e.remove()
        $jQuery('#' + lblId).remove();
        var str_array = TempValues.split(',');
        for (var i = 0; i < str_array.length - 1; i++) {
            if (str_array[i] === id) {
            }
            else {
                TempValue += str_array[i] + ',';
            }
        }
        $jQuery("[id$=hdnTempAdditionalDocumentIds]").val(TempValue);
    }
    function openCmbBoxOnTab(sender, e) {
        if (!sender.get_dropDownVisible()) sender.showDropDown();
    }

    function IsAdditionalDocumentsthere() {
        var PnlAdditionalDocument = $jQuery("[id$=PnlAdditionalDocument]");
        if (PnlAdditionalDocument[0].textContent.trim() != '')
            return true;
        else
            return false;
    }
    var submitclicked = true;
    var ValidateInstitution = true;
    var ValidateucAgencyHierarchyMultiple = false;
    var ValidateucInstitution = false;

    function ValidateInstitutionAndAgencyHier() {
        ValidateucAgencyHierarchyMultiple = false;
        ValidateucInstitution = false;
        if ($jQuery("[id$=hdnInstNodeIdNew]").val() != '' && $jQuery("[id$=hdnInstNodeIdNew]").val() != undefined) {
            ValidateucAgencyHierarchyMultiple = true;
            $jQuery("[id$=DivMsgBox]").hide();
        }

        else {
            $jQuery("[id$=lblMsg]").text('Please select Institution Hierarchy.').addClass('error');
            $jQuery("[id$=DivMsgBox]").show();
            return;
        }
        if ($jQuery("[id$=hdnAgencyHierarchyJsonObj]").val() != '' && $jQuery("[id$=hdnAgencyHierarchyJsonObj]").val() != undefined) {
            ValidateucInstitution = true;
            $jQuery("[id$=DivMsgBox]").hide();
        }
        else {
            $jQuery("[id$=DivMsgBox]").show();
            $jQuery("[id$=lblMsg]").text('Please select Agency Hierarchy.').addClass('error');

        }
    }

    function ValidatePageControls(s, e) {
        ValidateInstitutionAndAgencyHier();
        var IsAdditionalAndSyallbusValidate = true;
        if ($jQuery("[id$=hdnValidateFileUploadControl]").val() == 'true' && CountOfSyllabusFiles == 0 && $jQuery("[id$=lblUploadFormName]").length == 0 && ($jQuery("[id$=lblUploadFormName]").val() == '' || $jQuery("[id$=lblUploadFormName]").val() == undefined)) {
            $jQuery("[id$=lblSyllabusDocumentError]").show();
            IsAdditionalAndSyallbusValidate = false;
        }
        else { $jQuery("[id$=lblSyllabusDocumentError]").hide(); }
        if ($jQuery("[id$=hdnValidateAdditionalControl]").val() == 'true' && CountOfAdditionalFiles == 0 && !IsAdditionalDocumentsthere()) {
            $jQuery("[id$=lblAdditionalDocumentsRequired]").show();
            IsAdditionalAndSyallbusValidate = false;
        }
        else { $jQuery("[id$=lblAdditionalDocumentsRequired]").hide(); }
        s.set_autoPostBack(false);
        if (Page_IsValid && IsAdditionalAndSyallbusValidate && ValidateucAgencyHierarchyMultiple && ValidateucInstitution) {
            s.set_autoPostBack(true);
        }
    }

    function ResetValidator() {
        //debugger;
        if ($jQuery("[id$=rfvRotationName]")[0] != "" && $jQuery("[id$=rfvRotationName]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvRotationName]")[0], false);
        }
        if ($jQuery("[id$=rfvTypeSpecialtyAddEdit]")[0] != "" && $jQuery("[id$=rfvTypeSpecialtyAddEdit]")[0] != undefined) {
            ValidatorEnable($jQuery("[id$=rfvTypeSpecialtyAddEdit]")[0], false);
        }
        //if ($jQuery("[id$=rfvCombTypeSpecialtyAddEdit]")[0] != "" && $jQuery("[id$=rfvCombTypeSpecialtyAddEdit]")[0] != undefined) {
        //    ValidatorEnable($jQuery("[id$=rfvCombTypeSpecialtyAddEdit]")[0], false);
        //}

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
       // debugger;
        var IsInstAvailabilityDefined = "false";
        hdnIsInstAvailabilityDefined = $jQuery("[id$=hdnIsInstAvailabilityDefined]");
        if (hdnIsInstAvailabilityDefined != undefined && hdnIsInstAvailabilityDefined != null)
            IsInstAvailabilityDefined = hdnIsInstAvailabilityDefined.val();

        if (IsInstAvailabilityDefined != "True" && IsInstAvailabilityDefined != "true") {
            if ($jQuery("[id$=rfvInsPre]")[0] != "" && $jQuery("[id$=rfvInsPre]")[0] != undefined) {
                ValidatorEnable($jQuery("[id$=rfvInsPre]")[0], false);
            }
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
    function pageLoad() {
        //debugger;
        $jQuery("span#spnAgencyHierarchy").removeClass('buttonHidden');
        ResetValidator();
    }

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

    function CloseAddEditPopup() {
        var oArg = {};
        oArg.Action = "CloseAddEditPopup";
        oArg.IsEditableByClientAdmin = $jQuery("[id$=hdnIsEditableByClientAdmin]").val();
        oArg.IsEditableByAgencyUser = $jQuery("[id$=hdnIsEditableByAgencyUser]").val();
        oArg.AgencyId = $jQuery("[id$=hdnAgencyId]").val();

        oArg.TenantId = $jQuery("[id$=hdnTenantId]").val();
        oArg.ClinicalRotationId = $jQuery("[id$=hdnClinicalRotationId]").val();
        oArg.IsApplicantPkgNotAssignedThroughCloning = $jQuery("[id$=hdnIsApplicantPkgNotAssignedThroughCloning]").val();
        oArg.IsInstructorPkgNotAssignedThroughCloning = $jQuery("[id$=hdnIsInstructorPkgNotAssignedThroughCloning]").val();

        top.$window.get_radManager().getActiveWindow().close(oArg);
    }

    function ClosePopup() {
        var oArg = {};
        oArg.Action = "Cancel";
        top.$window.get_radManager().getActiveWindow().close();
    }

    function refreshParentGrid() {
        ClosePopup();
        var parentWindow = null;
        for (var i = 0; i < window.parent.length; i++) {
            if (window.parent[i].$page.url.toString().indexOf('ClinicalRotationMapping.aspx') > 0) {
                parentWindow = window.parent[i];
                break;
            }
        }
        if (parentWindow != null) {
            parentWindow.refreshRotationMappingGrid();
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

    function BindInstitutionLabel() {
        setTimeout(function () {
            var InstNodeLabel = $jQuery("[id$=hdnInstNodeLabel]").val();
            $jQuery($jQuery("[id$=lblInstitutionHierarchyPB]")[0]).text(InstNodeLabel);
        }, 1000);
    }

    function OnClientItemChecked(sender, args) {

        if (sender.get_checkedItems().length == 0) {

            sender.clearSelection();

            sender.set_emptyMessage("--SELECT--");

        }

    }
    function markAgencyHierarchyLinkDisabled() {
        setTimeout(function () {
            $jQuery("[id=AgencyHierarchy]").attr('class', 'agencyHierarchyLinkDisabled');
            $jQuery("[id=titleForAgencyHierarchy]").removeAttr('title');
        }, 200);
    }
    var IsParentMaximizedWindow = false;
    var IsParentzIndex = 0;
    $jQuery(document).ready(function () {
        $jQuery(document).on('click', '[id$=lnkInstitutionHierarchyPB]', function (e) {
            if (!$jQuery(this).hasClass('disabled')) {
                var win = $page.get_window();
                var IsRequestFromAddRotationScreen = 'Yes';
                var TempDIV = top.$window.get_radManager().getActiveWindow();//.get_popupElement();//.css('z-index','90001 !important');
                if (TempDIV._popupElement && Sys.UI.DomElement.containsCssClass(TempDIV._popupElement, "rwMaximizedWindow")) {
                    IsParentzIndex = TempDIV._popupElement.style.zIndex;
                    IsParentMaximizedWindow = true;
                }
                else {
                    IsParentMaximizedWindow = false;
                    IsParentzIndex = 0;
                }
                OpenInstitutionHierarchyPopupInsideGrid(false, IsRequestFromAddRotationScreen);
            }
        });
    });

    //UAT-4147
    function ShowExistingRotationMembers() {
        // debugger;
        var dialog = $window.showDialog($jQuery("[id$=divExistingRotationMembers]").clone().show(), {
            approvebtn: {
                autoclose: true, text: "Ok", click: function () {
                    return false;
                }
            }
        }, 550, 'Notice');
    }
</script>
