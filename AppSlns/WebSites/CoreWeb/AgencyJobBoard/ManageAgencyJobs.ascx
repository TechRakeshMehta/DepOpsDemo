<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageAgencyJobs.ascx.cs" Inherits="CoreWeb.AgencyJobBoard.Views.ManageAgencyJobs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>

<style type="text/css">
    .collapsed {
        height: 40px !important;
    }

    .customHeight {
        /*max-height: 82px !important;
        max-width: 200px !important;*/
        /*width: 175px !important;
        height: 73px !important;*/
    }

    .initName {
        font-size: 12px !important;
        line-height: 65px !important;
    }
</style>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Applicant/editprofile.css" ResourceType="StyleSheet" />
     <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <div id="dvSection" runat="server" class="section" style="margin-bottom: 0px;">
                <h1 class="header-color mhdr" id="hAgencyLogo" style="font-size: 22px !important" runat="server">Add/Change Agency Logo</h1>
                <div id="dvContent" runat="server" class="content" style="width: 180px; margin-bottom: 0px;">
                    <div class="bx_image customHeight">
                        <asp:Image runat="server" ID="imgCntrl" class="thumb customHeight" />
                        <asp:Label runat="server" ID="lblNameInitials" Visible="false" Height="73" Width="175" class="initName" />
                    </div>
                    <div class="bx_uploader" title="Click this button to change agency logo." style="margin-left: 0px; width: 160px; float: left;">
                        <infs:WclButton ID="btnValidator" ClientIDMode="Static" CssClass="RadAsyncUpload RadUpload RadUpload_Windows7" Width="80px" Style="float: left;" runat="server" Text="Change" AutoPostBack="false" OnClientClicked="ValidateUploadContrl">
                        </infs:WclButton>
                        <infs:WclAsyncUpload runat="server" ID="uploadControl" Width="80px" Style="float: left; display: none;" HideFileInput="true" Skin="Hay" ClientIDMode="Static"
                            OnClientFileUploaded="onFileUploaded" MultipleFileSelection="Disabled" MaxFileInputsCount="1"
                            AllowedFileExtensions=".jpg,.jpeg,.tiff,.bmp,.bitmap,.png,.JPG,.PNG,.BITMAP,.JPEG,.TIFF,.BMP" OnClientFileSelected="onClientFileSelected"
                            Localization-Select="Change" OnClientValidationFailed="upl_OnClientValidationFailed">
                        </infs:WclAsyncUpload>
                        <infs:WclButton runat="server" ID="btnClearLogo" ClientIDMode="Static" OnClientClicked="ClearLogo" AutoPostBack="false" OnClick="btnClearLogo_Click"
                            ToolTip="Click this button to clear agency logo." CssClass="RadAsyncUpload RadUpload RadUpload_Windows7" Text="Clear" Style="float: left; width: 75px; font-size: 12px;">
                        </infs:WclButton>
                        <div style="display: none">
                            <infs:WclButton ID="btnUpload" runat="server" OnClick="btnUpload_Click">
                            </infs:WclButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Manage Agency Jobs</h2>
        </div>
    </div>
    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-md-12">
            <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="rblJobBoardType" OnSelectedIndexChanged="rblJobBoardType_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Text="Job Template" Value="AAAA" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Job Posting" Value="AAAB"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
    </div>
    <div class="row">&nbsp;</div>
    <div class="row bgLightGreen">
        <infs:WclGrid runat="server" ID="grdManageAgencyJobs" AllowPaging="true" PageSize="10" AllowCustomPaging="false"
            AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" ShowAllExportButtons="False" AllowFilteringByColumn="false"
            NonExportingColumns="EditCommandColumn, DeleteColumn" ValidationGroup="grpValdManageAgencyJobs"
            OnNeedDataSource="grdManageAgencyJobs_NeedDataSource" OnDeleteCommand="grdManageAgencyJobs_DeleteCommand"
            OnPreRender="grdManageAgencyJobs_PreRender" OnItemDataBound="grdManageAgencyJobs_ItemDataBound" ShowClearFiltersButton="false">
            <ExportSettings Pdf-PageWidth="300mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="AgencyJobID,StatusCode" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="true" ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"></CommandItemSettings>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="AssignItems" HeaderTooltip="Click this box to select all agency users."
                        AllowFiltering="false" ShowFilterIcon="false" Visible="true">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectItem" runat="server"
                                onclick="UnCheckHeader(this)" OnCheckedChanged="chkSelectItem_CheckedChanged" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="TemplateName" FilterControlAltText="Filter TemplateName column"
                        HeaderText="Template Name" SortExpression="TemplateName" UniqueName="TemplateName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Company" FilterControlAltText="Filter Company column"
                        HeaderText="Company" SortExpression="Company" UniqueName="Company">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column"
                        HeaderText="Location" SortExpression="Location" UniqueName="Location">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="JobTitle" FilterControlAltText="Filter JobTitle column"
                        HeaderText="Job Title" SortExpression="JobTitle" UniqueName="JobTitle">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="JobDescription" FilterControlAltText="Filter JobDescription column"
                        HeaderText="Job Description" SortExpression="JobDescription" UniqueName="JobDescription">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="FieldTypeName" FilterControlAltText="Filter FieldTypeName column"
                        HeaderText="Field Type" SortExpression="FieldTypeName" UniqueName="FieldTypeName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="JobTypeName" FilterControlAltText="Filter JobTypeName column"
                        HeaderText="Job Type" SortExpression="JobTypeName" UniqueName="JobTypeName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column"
                        HeaderText="Status" SortExpression="Status" UniqueName="Status">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PublishDate" FilterControlAltText="Filter PublishDate column" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="88px"
                        HeaderText="Publish Date" SortExpression="PublishDate" UniqueName="PublishDate">
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif" CommandName="Delete" ConfirmText="Are you sure you want to delete this Agency Job?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <HeaderStyle Width="30px" />
                    </telerik:GridButtonColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif"
                        UniqueName="EditCommandColumn">
                        <HeaderStyle Width="30px" />
                    </telerik:GridEditCommandColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div runat="server" id="divEditBlock" visible="true">
                            <div class="col-md-12">
                                <h2 class="header-color">
                                    <asp:Label ID="lblTitleAgencyJob" Text='<%# (Container is GridEditFormInsertItem && grdManageAgencyJobs.MasterTableView.CommandItemSettings.AddNewRecordText=="Add New Job Template") ? "Add New Agency Job Template"
                                    :((Container is GridEditFormInsertItem && grdManageAgencyJobs.MasterTableView.CommandItemSettings.AddNewRecordText=="Add New Post")? "Add New Job Post"
                                    :(grdManageAgencyJobs.MasterTableView.CommandItemSettings.AddNewRecordText=="Add New Job Template")? "Update Agency Job Template":"Update Agency Job Post") %>'
                                        runat="server" />
                                </h2>
                            </div>
                            <asp:Panel runat="server" CssClass="col-md-12" ID="pnlAgencyJobs">
                                <asp:HiddenField ID="hdnAgencyJobID" runat="server" Value='<%# Eval("AgencyJobID") %>' />
                                <asp:HiddenField ID="hdnAgencyJobStatusCode" runat="server" Value='<%# Eval("StatusCode") %>' />
                                <div class="row bgLightGreen">
                                    <div class="form-group col-md-3">
                                    </div>
                                </div>
                                <div class="row bgLightGreen">
                                    <div class="form-group col-md-3" id="dvTemplateContainer" runat="server">
                                        <div id="dvTemplate" runat="server" visible="false">
                                            <span class="cptn">Template</span>
                                            <infs:WclComboBox ID="drpdwnTemplate" runat="server" DataTextField="TemplateName" DataValueField="AgencyJobID" AutoPostBack="true"
                                                CssClass="form-control" Skin="Silk" Width="100%" AutoSkinMode="false" EmptyMessage="--SELECT--" OnSelectedIndexChanged="drpdwnTemplate_SelectedIndexChanged">
                                            </infs:WclComboBox>
                                        </div>
                                        <div id="dvTemplateText" runat="server">
                                            <span class="cptn">Template Name</span><span class="reqd">*</span>
                                            <infs:WclTextBox ID="txtTemplateName" Width="100%" runat="server" Text='<%# Eval("TemplateName") %>'
                                                CssClass="form-control" MaxLength="50">
                                            </infs:WclTextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvTemplateName" ControlToValidate="txtTemplateName"
                                                class="errmsg" ErrorMessage="Template Name is required." ValidationGroup='grpValdManageAgencyJobs' Display="Dynamic"
                                                Enabled="true" />
                                        </div>
                                    </div>
                                    <div class="form-group col-md-3">
                                        <span class="cptn">Job Title</span><span id="spnReqJobTitle" class="reqd">*</span>
                                        <infs:WclTextBox ID="txtJobTitle" Width="100%" runat="server" Text='<%# Eval("JobTitle") %>'
                                            CssClass="form-control" MaxLength="50">
                                        </infs:WclTextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvJobTitle" ControlToValidate="txtJobTitle"
                                            class="errmsg" ErrorMessage="Job Title is required." ValidationGroup='grpValdManageAgencyJobs' Display="Dynamic"
                                            Enabled="true" />
                                    </div>
                                    <div class="form-group col-md-3">
                                        <span class="cptn">Company</span>
                                        <infs:WclTextBox Width="100%" ID="txtCompany" runat="server" CssClass="form-control"
                                            Text='<%# Eval("Company") %>' MaxLength="255">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class="form-group col-md-3">
                                        <span class="cptn">Location</span>
                                        <infs:WclTextBox Width="100%" ID="txtLocation" runat="server" CssClass="form-control"
                                            Text='<%# Eval("Location") %>' MaxLength="255">
                                        </infs:WclTextBox>
                                    </div>
                                </div>
                                <div class="row bgLightGreen">
                                    <div class="form-group col-md-3">
                                        <span class="cptn">Instructions</span>
                                        <infs:WclTextBox ID="txtInstructions" Width="100%" runat="server" Text='<%# Eval("Instructions") %>'
                                            CssClass="form-control" MaxLength="2048" TextMode="MultiLine">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class="form-group col-md-3">
                                        <span class="cptn">How To Apply</span>
                                        <infs:WclTextBox Width="100%" ID="txtHowToApply" runat="server" CssClass="form-control"
                                            Text='<%# Eval("HowToApply") %>' MaxLength="2048" TextMode="MultiLine">
                                        </infs:WclTextBox>
                                    </div>
                                    <%-- New Dropdown Testing --%>
                                    <div class="form-group col-md-3">
                                        <span class="cptn">Field Type</span><span id="spnJobFieldType" class="reqd">*</span>
                                        <infs:WclComboBox ID="ddlFieldType" runat="server" CheckBoxes="false"
                                            DataTextField="Description" DataValueField="ID"
                                            OnClientKeyPressing="openCmbBoxOnTab" AutoPostBack="false" Width="100%" CssClass="form-control"
                                            Skin="Silk" AutoSkinMode="false">
                                        </infs:WclComboBox>
                                        <div class="vldx">
                                            <asp:RequiredFieldValidator runat="server" ID="rfvJobFieldType" ControlToValidate="ddlFieldType"
                                                CssClass="errmsg" Text="Field type is required." ValidationGroup="grpValdManageAgencyJobs" Display="Dynamic"
                                                InitialValue="--SELECT--" Enabled="true" />
                                        </div>

                                    </div>
                                </div>
                                <div class="row bgLightGreen">
                                    <div class="form-group col-md-3">
                                        <span class="cptn">Job Type</span>
                                        <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="rblJobType" DataValueField='<%# Eval("AgencyJobTypeCode")%>'>
                                            <asp:ListItem Value="AAAA" Text="Internship" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="AAAB" Text="Employment"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="row bgLightGreen">
                                    <div class="form-group col-md-3" style="width: 100%;">
                                        <span class="cptn">Description</span>
                                        <div>
                                            <infs:WclEditor ID="txtDescription" ClientIDMode="Static" runat="server" Width="99.5%" EnableResize="false" Height="500px"
                                                ToolsFile="~/WebSite/Data/Tools.xml"
                                                OnClientLoad="OnClientLoad" content='<%# Eval("JobDescription") %>'>
                                            </infs:WclEditor>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12 textright  bgLightGreen">
                                    <infsu:CommandBar ID="fsucCmdBarAgencyJobs" DisplayButtons="Save,Extra,Submit,Cancel" runat="server" GridMode="false"
                                        DefaultPanel="pnlAgencyJobs" GridInsertText="Save" GridUpdateText="Save" UseAutoSkinMode="False"
                                        AutoPostbackButtons="Save,Extra,Submit,Cancel" CauseValidationOnCancel="false"
                                        OnSaveClick="fsucCmdBarAgencyJobs_SaveClick" SaveButtonText="Preview"
                                        OnExtraClick="fsucCmdBarAgencyJobs_ExtraClick" ExtraButtonText="Draft"
                                        OnSubmitClick="fsucCmdBarAgencyJobs_SubmitClick" SubmitButtonText="Post" SubmitButtonIconClass="rbSave"
                                        OnCancelClick="fsucCmdBarAgencyJobs_CancelClick" CancelButtonText="Cancel"
                                        ValidationGroup="grpValdManageAgencyJobs" ExtraButtonIconClass="rbSave"
                                        ButtonSkin="Silk" />
                                </div>
                            </asp:Panel>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
        </infs:WclGrid>
    </div>
    <div id="Archive" runat="server">
        <div class="row">&nbsp;</div>
        <div class="row">
            <div class="col-md-12" style="text-align: center;">
                <infs:WclButton runat="server" ID="btnArchive" OnClick="btnArchive_Click" Skin="Silk" AutoSkinMode="false"
                    ButtonType="StandardButton" Icon-PrimaryIconCssClass="rbAddNew" Text="Archive Job Post" AutoPostBack="true">
                </infs:WclButton>
            </div>
        </div>
    </div>
</div>
<asp:HiddenField runat="server" ID="hdnIsCollapsed" ClientIDMode="Static" Value="0" />

<script type="text/javascript">

    function pageLoad() {
     
        $jQuery('[id$=dvSection]').unbind('click');

        $jQuery('[id$=dvSection]').bind('click', function () {
            setTimeout(function () {
                if ($jQuery('[id$=dvSection]').hasClass('collapsed')) {
                    $jQuery('#hdnIsCollapsed').val(1);
                }
                else {
                    $jQuery('#hdnIsCollapsed').val(0);
                }
            }, 700);
        });

        ////$jQuery('[id$=uploadControl]').unbind('click');
        //var openFile = false;
        //$jQuery('[id$=uploadControl]').on('click', function (e) {
        //    debugger;

        //    if ($jQuery('[id$=btnCancel]').length > 0) {
        //        if (openFile == false) {
        //           // e.preventDefault();
        //            $confirm(messageTempJobData, function (res) {
        //                if (!res) {
        //                    alert(1);
        //                    openFile = true;
        //                    $jQuery('[id$=uploadControl]').click();
        //                }
        //            }, 'Complio', true);
        //        }
        //        else {
        //            openFile = false;
        //        }
        //    }
        //});

        $jQuery('[id$=btnClearLogo]').attr('class', 'RadAsyncUpload RadUpload RadUpload_Windows7');
        $jQuery('[id$=btnClearLogo] input[type="submit"]').attr('class', 'ruButton ruBrowse');
        $jQuery('[id$=btnClearLogo] input[type="submit"]').attr('style', 'font-size:12px;');
        $jQuery('[id$=btnValidator]').attr('class', 'RadAsyncUpload RadUpload RadUpload_Windows7');
        $jQuery('[id$=btnValidator] input[type="submit"]').attr('class', 'ruButton ruBrowse');
        $jQuery('[id$=btnValidator] input[type="submit"]').attr('style', 'font-size:12px;');

    }

    function ValidateUploadContrl(sender, args) {
        if ($jQuery('[id$=btnCancel]').length > 0) {
            $confirm(messageTempJobData, function (res) {
                if (res) {
                    $jQuery('[id$=uploadControlfile0]').click();
                }
            }, 'Complio', true);
        }
        else {
            $jQuery('[id$=uploadControlfile0]').click();
        }
        return false;
    }

    var messageTempJobData = "Please save job template/post and then change/clear agency logo otherwise job template/post data will be lost. Would you like to change/clear logo?";

    var selectedFileIndex;

    function onClientFileSelected(sender, args) {
        selectedFileIndex = args.get_rowIndex();
    }

    var upl_OnClientValidationFailed = function (s, a) {
        var error = false;
        var errorMsg = "";

        var extn = a.get_fileName().substring(a.get_fileName().lastIndexOf('.') + 1, a.get_fileName().length);

        if (a.get_fileName().lastIndexOf('.') != -1) {
            if (s.get_allowedFileExtensions().indexOf(extn) == -1) {
                error = true;
                errorMsg = "! Error: Unsupported File Format";
            }
            else {
                error = true;
                errorMsg = "! Error: File size exceeds 5MB";
            }
        }
        else {
            error = true;
            errorMsg = "! Error: Unrecognized File Format";
        }

        if (error) {
            var row = a.get_row();
            smsg = document.createElement("span");

            smsg.innerHTML = errorMsg;
            smsg.setAttribute("class", "ruFileWrap");
            smsg.setAttribute("style", "color:red;padding-left:10px;");

            row.appendChild(smsg);
        }
    }


    function onFileUploaded(sender, args) {
        var fileSize = args.get_fileInfo().ContentLength;

        if (fileSize > 0) {
            if (sender.getUploadedFiles() != "") {
                $jQuery("[id$=btnUpload]").click();
            }
        }
        else {
            sender.deleteFileInputAt(selectedFileIndex);
            sender.updateClientState();
            alert("File size should be more than 0 byte.");
            return;
        }
    }

    function ShowCallBackMessage(docMessage) {
        if (docMessage != '') {
            alert(docMessage);
        }
    }

    function CheckAll(id) {
        var masterTable = $find("<%= grdManageAgencyJobs.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectItem").checked = isChecked; // for checking the checkboxes
            }
        }
    }

    function UnCheckHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdManageAgencyJobs.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }
    var messageTempJobData = "Please save job template/post and then change/clear agency logo otherwise job template/post data will be lost. Would you like to change/clear logo?";
    var message = "Are you sure, you want to delete agency logo?"
    function ClearLogo(sender, eventArgs) {
        if ($jQuery('[id$=btnCancel]').length > 0) {
            $confirm(messageTempJobData, function (res) {
                if (!res) {
                    return false;
                }
                else {
                    $confirm(message, function (res) {
                        if (res) {
                            __doPostBack("<%= btnClearLogo.UniqueID %>", "");
                            return true;
                        }
                        else {
                            return false;
                        }
                    }, 'Complio', true);
                }
            }, 'Complio', true);
        }
        else {
            $confirm(message, function (res) {
                if (res) {
                    __doPostBack("<%= btnClearLogo.UniqueID %>", "");
                    return true;
                }
                else {
                    return false;
                }
            }, 'Complio', true);
        }
        return false;
    }

    function openPopUp(pageTitle) {
        // debugger;
        var packageCopyScreenWindowName = "previewAgencyJobPost";
        //ResetTimer();
        var url = $page.url.create("~/AgencyJobBoard/Pages/PreviewAgencyJobPost.aspx");
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (80 / 100);

        var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: packageCopyScreenWindowName, onclose: OnClientClose }
             , function () {
                 this.set_title(pageTitle);
             });

        winopen = true;
    }

    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        winopen = false;
    }

</script>
