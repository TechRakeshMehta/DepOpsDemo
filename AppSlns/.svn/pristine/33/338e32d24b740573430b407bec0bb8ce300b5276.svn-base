<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ExceptionDetail" CodeBehind="ExceptionDetail.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<script language="javascript" type="text/javascript">
    Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
    Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };

    $jQuery(document).ready(function () {

        showHideButton(false);
        // $jQuery("[id$=divNoPreview]").show();
    });
    var selectedFileIndex;

    function clientFileSelected(sender, args) {
        selectedFileIndex = args.get_rowIndex();

    }
    function onFileUploaded(sender, args) {
        var fileSize = args.get_fileInfo().ContentLength;
        //Added minimum file size check regarding UAT-862: WB: As a student or an admin, I should not be allowed to upload documents with a size of 0
        if (fileSize > 0) {
            if (sender.getUploadedFiles() != "") {
                showHideButton(true);
            }
        }
        else {
            sender.deleteFileInputAt(selectedFileIndex);
            sender.updateClientState();
            alert("File size should be more than 0 byte.");
            return;
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
                errorMsg = "! Error: File size exceeds 20 MB";
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
</script>
<style type="text/css">
    html, body, #frmmod, #UpdatePanel1, #box_content {
        height: 100% !important;
    }
</style>
<infs:WclSplitter ID="sptrMain" runat="server" LiveResize="true" Orientation="Horizontal"
    Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
    <%-- <infs:WclPane ID="pnToolbar" runat="server" MinHeight="80" Height="80" Width="100%"
        Scrolling="None" Collapsed="false">
        <div style="font-size: 11px">
            <div id="modwrapo">
                <div id="modwrapi">
                    <div id="breadcmb">
                        <infsu:breadcrumb ID="breadcrum" runat="server" />
                    </div>
                    <div id="modhdr">
                        <h1>
                            <asp:Label Text="" runat="server" ID="lblModHdr" />&nbsp;<asp:Label Text="" runat="server"
                                ID="lblPageHdr" CssClass="phdr" /></h1>
                    </div>
                </div>
                <div id="modcmds">
                </div>
            </div>
        </div>
    </infs:WclPane>--%>
    <infs:WclPane ID="pnLower" runat="server" Scrolling="None" Width="100%">
        <infs:WclSplitter ID="sptrVeriOuter" runat="server" LiveResize="true" Orientation="Vertical"
            Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true"
            ClientKey="sptrVerification">
            <infs:WclPane ID="pnLeft" runat="server" Height="100%" Width="50%" MinWidth="200"
                Scrolling="Y" Collapsed="false">
                <div class="section">
                    <div class="content">
                        <div class="sxform auto">
                            <asp:Panel CssClass="sxpnl" runat="server" ID="pnlApplicantDetail">
                                <div class='sxro sx2co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Applicant Name</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtApplicantName" Enabled="false">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxlb'>
                                        <span class="cptn">DOB</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtDOB" Enabled="false">
                                        </infs:WclTextBox>
                                    </div>
                                    <%--<div class='sxlb'>
                                        Gender
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtGender" Enabled="false">
                                        </infs:WclTextBox>
                                    </div>--%>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel CssClass="sxpnl" runat="server" ID="pnlItemDetail">
                                <div class='sxro sx2co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Gender</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtGender" Enabled="false">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxlb'>
                                        <span class="cptn">Phone</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtPhone" Enabled="false">
                                        </infs:WclTextBox>
                                    </div>
                                    <%-- <div class='sxlb'>
                                        Email
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtEmail" Enabled="false">
                                        </infs:WclTextBox>
                                    </div>--%>
                                    <%-- <div class='sxlb'>
                                        Client Name
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtTenantName" Enabled="false">
                                        </infs:WclTextBox>
                                    </div>--%>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel CssClass="sxpnl" runat="server" ID="Panel2">
                                <div class='sxro sx2co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Email</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtEmail" Enabled="false">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxlb'>
                                        <span class="cptn">Client Name</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtTenantName" Enabled="false">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel CssClass="sxpnl" runat="server" ID="pnlClientDetail">
                                <div class='sxro sx2co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Item</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclComboBox ID="cmbItemName" runat="server" AutoPostBack="false" DataTextField="Name"
                                            DataValueField="ComplianceItemID">
                                        </infs:WclComboBox>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div id="Div1" class="section" runat="server">
                    <div class="sxform auto">
                        <div class="content">
                            <asp:Panel CssClass="sxpnl" runat="server" ID="Panel1">
                                <div class='sxro sx2co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Upload Document</span>
                                    </div>
                                    <div class='sxlm m2spn'>
                                        <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay"
                                            AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,gif,tif,tiff,docx,doc,rtf,pdf,odt,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT"
                                            UploadedFilesRendering="BelowFileInput" MultipleFileSelection="Automatic" OnClientFileUploaded="onFileUploaded" OnClientFileSelected="clientFileSelected"
                                            OnClientFileUploadRemoved="onFileRemoved" OnClientValidationFailed="upl_OnClientValidationFailed">
                                            <Localization Select="Browse" />
                                        </infs:WclAsyncUpload>
                                        <infs:WclButton ID="btnUpload" Text="Upload" runat="server" OnClick="btnUpload_Click">
                                        </infs:WclButton>
                                        <infs:WclButton ID="btnCancelUpload" runat="server" Text="Cancel" OnClick="btnCancelUpload_Click">
                                        </infs:WclButton>
                                    </div>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <h1 class="mhdr">Documents</h1>
                    <div class="content">
                        <div class="swrap">
                            <infs:WclGrid runat="server" ID="grdDocuments" AutoGenerateColumns="False" AutoSkinMode="true"
                                AllowMultiRowSelection="true" AllowPaging="true" CellSpacing="0" EnableDefaultFeatures="false"
                                AllowFilteringByColumn="false" OnNeedDataSource="grdDocuments_NeedDataSource"
                                GridLines="Both" ShowAllExportButtons="false" ShowExtraButtons="false" ShowHeader="True"
                                ShowFooter="True" OnItemDataBound="grdDocuments_ItemDataBound" OnItemCommand="grdDocuments_ItemCommand">
                                <ClientSettings EnableRowHoverStyle="true">
                                    <Selecting AllowRowSelect="true"></Selecting>
                                </ClientSettings>
                                <MasterTableView CommandItemDisplay="Top" DataKeyNames="ApplicantDocumentID,DocumentPath"
                                    AllowPaging="True">
                                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" ShowExportToCsvButton="false"
                                        ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Document Name" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDocument" runat="server" Text='<%# Eval("FileName")%>' CommandName="DocumentViewer"></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRemove" runat="server" OnCheckedChanged="chkSelectItem_CheckedChanged" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <NoRecordsTemplate>
                                        No Documents found.
                                    </NoRecordsTemplate>
                                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                                </MasterTableView>
                                <FilterMenu EnableImageSprites="False">
                                </FilterMenu>
                            </infs:WclGrid>
                        </div>
                        <div class="gclr">
                        </div>
                    </div>
                </div>
                <div class="section">
                    <div class="content">
                        <div class="sxform auto">
                            <asp:Panel CssClass="sxpnl" runat="server" ID="pnlComments">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Exception Reason</span>
                                    </div>
                                    <div class='sxlm m2spn'>
                                        <infs:WclTextBox runat="server" ID="txtExpReason" ReadOnly="true" TextMode="MultiLine"
                                            Height="50px" CssClass="txt2ro">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Verification Comments</span>
                                    </div>
                                    <div class='sxlm m2spn'>
                                        <infs:WclTextBox runat="server" ID="txtVerificationComments" ReadOnly="true" TextMode="MultiLine"
                                            Height="50px" CssClass="txt2ro">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Add Comments</span>
                                    </div>
                                    <div class='sxlm m2spn'>
                                        <infs:WclTextBox runat="server" ID="txtComments" Enabled="true" TextMode="MultiLine"
                                            Height="50px" CssClass="txt2ro">
                                        </infs:WclTextBox>
                                        <div class='vldx'>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvComments" Display="Dynamic" ControlToValidate="txtComments"
                                                class="errmsg" ErrorMessage="Comments are required." ValidationGroup="grpFormSubmit" />
                                        </div>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <infsu:CommandBar ID="cbbuttons" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                    AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbRemove" SaveButtonIconClass="rbOk"
                    SubmitButtonText="Reject" SaveButtonText="Approve" CancelButtonText="Cancel"
                    OnSubmitClick="CmdBarReject_Click" OnSaveClick="CmdBarApprove_Click" OnCancelClick="CmdBarCancel_Click">
                </infsu:CommandBar>
            </infs:WclPane>
            <infs:WclSplitBar ID="WclSplitBar1" runat="server" CollapseMode="forward">
            </infs:WclSplitBar>
            <infs:WclPane ID="pnRight" runat="server" Scrolling="Both" Width="100%">
                <iframe id="iframe" runat="server" width="99%" height="98%"></iframe>
                <%--  <div id="divNoPreview" style="margin: 0 auto; margin-top: 100px; width: 105px;  color: #adadad">
                    Nothing to preview
                </div>--%>
            </infs:WclPane>
        </infs:WclSplitter>
    </infs:WclPane>
</infs:WclSplitter>
