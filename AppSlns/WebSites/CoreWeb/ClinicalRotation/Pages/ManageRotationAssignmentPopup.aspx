<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageRotationAssignmentPopup.aspx.cs" Inherits="CoreWeb.ClinicalRotation.Views.ManageRotationAssignmentPopup"
    MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="../Resources/Mod/ClinicalRotation/ManageRotationAssignment.js" ResourceType="JavaScript" />

        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>

    <script type="text/javascript">
        // To close the popup.
        function ClosePopup() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            var hdnSavedStatus = $jQuery("[id$=hdnSavedStatus]");
            var oArg = {};
            if (hdnSavedStatus != undefined && hdnSavedStatus.val() != "" && hdnSavedStatus.val() == "True") {
                oArg.Action = "Submit";
                oArg.IsStatusSaved = true;
            }
            else { oArg.IsStatusSaved = false; }
            oWindow.Close(oArg);
        }

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
    <div class="container-fluid" style="padding-top: 20px;">
        <%--<div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Instructor/Preceptor Rotation Package</h2>
            </div>
        </div>--%>
        <div id="dvInstrüctorPkg" runat="server" visible="false">
            <asp:Panel ID="Panel1" runat="server">

                <div id="dvAddUpdateInstPackage" runat="server">
                    <div class='col-md-4'>
                        <div class="row">
                            <div class='form-group col-md-3' title="Select a package.">
                                <span class="cptn">Select Instructor/Preceptor Package</span><span class="reqd">*</span>

                                <infs:WclComboBox ID="cmbInstPackage" runat="server" DataTextField="RequirementPackageName"
                                    DataValueField="RequirementPackageID" OnItemDataBound="cmbInstPackage_ItemDataBound"
                                    AutoPostBack="false" OnDataBound="cmbInstPackage_DataBound" Width="100%" CssClass="form-control" MaxHeight="200"
                                    Skin="Silk" AutoSkinMode="false">
                                </infs:WclComboBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvInstPackage" ControlToValidate="cmbInstPackage"
                                        InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpAssignInstPackage"
                                        Text="Instructor/Preceptor Package is required." />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>


        <%-- <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Rotation Requirement Packages</h2>
            </div>
        </div>--%>

        <div id="divStudentPackages" runat="server" visible="false">

            <asp:Panel ID="pnlPackages" CssClass="" runat="server">

                <div id="dvAddUpdatePackage" runat="server">
                    <div class='col-md-4'>
                        <div class="row">
                            <div class='form-group col-md-3' title="Select a package.">
                                <span class="cptn">Select Student Package</span><span class="reqd">*</span>

                                <infs:WclComboBox ID="cmbPackage" runat="server" DataTextField="RequirementPackageName"
                                    DataValueField="RequirementPackageID"
                                    AutoPostBack="false" OnDataBound="cmbPackage_DataBound" OnItemDataBound="cmbPackage_ItemDataBound"
                                    Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" MaxHeight="200">
                                </infs:WclComboBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvPackage" ControlToValidate="cmbPackage"
                                        InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpAssignStudentPackage"
                                        Text="Student Package is required." />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>


        <div id="divAssignPreceptor" runat="server" visible="false">
            <asp:Panel ID="Panel2" runat="server">
                <div class='col-md-4'>
                    <div class="row">
                        <div class='form-group col-md-3'>
                            <span class="cptn lineHeight">Instructor/Preceptor</span><span class="reqd">*</span>
                            <infs:WclComboBox ID="ddlInstructor" runat="server" CheckBoxes="true" DataValueField="ClientContactID"
                                DataTextField="Name" AutoPostBack="false" EmptyMessage="--SELECT--"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" MaxHeight="200">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:CustomValidator ClientValidationFunction="ValidateCheckbox" ID="rfvInstPreceptor" runat="server"
                                    ErrorMessage="Instructor/Preceptor is required." CssClass="errmsg" EnableClientScript="true" ValidationGroup="grpAssignInstructor"
                                    Display="Dynamic"></asp:CustomValidator>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>


        <div id="divUploadSyllabus" runat="server" visible="false">
            <%-- class="row bgLightGreen"--%>
            <asp:Panel ID="Panel3" runat="server">
                <div class='col-md-4'>
                    <div class="row">
                        <div class='form-group col-md-3'>
                            <span class="cptn lineHeight">Syllabus Document</span><span class="reqd">*</span>

                            <div class="bx_uploader" title="Click this button to upload syllabus">
                                <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay"
                                    MultipleFileSelection="Disabled" MaxFileInputsCount="1" OnClientFileSelected="onClientFileSelected"
                                    OnClientFileUploaded="onFileUploaded" OnClientValidationFailed="upl_OnClientValidationFailed"
                                    Localization-Select="Browse" Width="100%" CssClass="form-control marginTop2"
                                    AutoSkinMode="true"
                                    AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,JPG,gif,tif,tiff,docx,doc,rtf,pdf,odt,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,JPG,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT" />
                                <div class="vldx">
                                    <asp:CustomValidator ClientValidationFunction="ValidateUploadControl" ID="cvUploadControl" runat="server"
                                        ErrorMessage="Syllabus Document is required." CssClass="errmsg" EnableClientScript="true" ValidationGroup="grpSyllabusDocument"
                                        Display="Dynamic"></asp:CustomValidator>
                                </div>
                            </div>
                            <div style="clear: both; float: left; position: relative;">
                                <asp:Label ID="lblUploadFormName" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblUploadFormPath" runat="server" Visible="false"></asp:Label>
                                <%--<asp:LinkButton ID="lnkRemove" runat="server" Text="Remove" Visible="false" OnClick="lnkRemove_Click"
                                    ToolTip="Click this button to remove document"></asp:LinkButton>--%>
                            </div>
                            <div class='vldx'>
                                <asp:Label ID="lblUploadFormMsg" class="errmsg" runat="server" Visible="false">Syllabus Document is required.</asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>


        <div class="row" style="padding-top: 40px;">
            <div class="col-md-12">
                <infsu:CommandBar ID="cmdSaveAssignments" runat="server" AutoPostbackButtons="Save" DisplayButtons="Save,Cancel" CancelButtonText="Close"
                    OnSaveClick="cmdSaveAssignments_SaveClick"
                    ButtonPosition="Center" CauseValidationOnCancel="false" OnCancelClientClick="ClosePopup" ButtonSkin="Silk" UseAutoSkinMode="false" />
            </div>
        </div>
        <asp:HiddenField ID="hdnSavedStatus" runat="server" Value="False" />

        <%--UAT-4147--%>
        <div id="divExistingRotationMembers" class="acknowledgeMessagePopup" title="Complio" runat="server" style="display: none">
            <p style="text-align: left;">One or more of the selected Instructor/Preceptor is already added as Applicant. Please review your selection: </p>
            <div>&nbsp;</div>
            <asp:Panel ID="pnlExistingRotationMembers" runat="server">
            </asp:Panel>
        </div>
    </div>
</asp:Content>
