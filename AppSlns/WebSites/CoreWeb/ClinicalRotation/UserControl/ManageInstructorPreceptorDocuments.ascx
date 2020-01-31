<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageInstructorPreceptorDocuments.ascx.cs" Inherits="CoreWeb.ClinicalRotation.UserControl.ManageInstructorPreceptorDocuments" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ClinicalRotation/UserControl/ManageSharedUserDocument.ascx" TagPrefix="uc" TagName="SharedUserDocuments" %>
<%@ Register TagName="UploadDocuments" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/UploadDocuments.ascx" %>


<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
     <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/upload.css" ResourceType="StyleSheet" />

</infs:WclResourceManagerProxy>
<style>
    #modcmd_bar {
        position: relative !important;
        z-index: 9999999999;
    }

    .Category label > input {
        display: none;
    }

    .Category label {
        color: black;
        background-color: white;
        padding-left: 10px !important;
    }

    #cmbItems li:not(.rcbSeparator) {
        padding: 5px 0px 0px 40px !important;
    }


    .box__dragndrop,
    .box__uploading,
    .box__success,
    .box__error {
        display: none;
    }

    .issue-drop-zone {
        border: 1px dashed #ccc;
        border-top-color: rgb(204, 204, 204);
        border-right-color: rgb(204, 204, 204);
        border-bottom-color: rgb(204, 204, 204);
        border-left-color: rgb(204, 204, 204);
        border-radius: 0;
        padding: 7px;
        padding-left: 7px;
    }

    .issue-drop-zone__drop-icon {
        position: relative;
    }

    .issue-drop-zone__drop-icon {
        position: relative;
    }

    .adg3 #attachmentmodule .issue-drop-zone .issue-drop-zone__text {
        width: 53%;
        font-family: -apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,Oxygen,Ubuntu,'Fira Sans','Droid Sans','Helvetica Neue',sans-serif;
        color: #172b4d;
        font-size: 14px;
        font-weight: 400;
        font-style: normal;
        line-height: 20px;
    }

    .issue-drop-zone__text {
        text-align: center;
    }

    .issue-drop-zone:not(.mod-content) {
        text-align: center;
    }
</style>

<div class="col-md-12">
    <ul class="nav nav-tabs">
        <li runat="server" id="liDocuments"><a runat="server" id="tabDocuments" onserverclick="tabDocuments_Click">Manage Requirement Documents</a></li>
        <li runat="server" id="liIPDocuments"><a runat="server" id="tabIPDocuments" onserverclick="tabIPDocuments_ServerClick">Manage Professional Documents</a></li>
    </ul>
</div>
<div class="tab-content">
    <div class="row">&nbsp;</div>
    <div id="dvUploadSharedUserPersonalDocuments" runat="server">
        <uc:SharedUserDocuments ID="ucSharedUserDocuments" runat="server" Visible="true" />
    </div>

    <div id="dvManageUploadDocument" runat="server">

        <div class="container-fluid">
            <div class="row bgLightGreen" id="dvInstitution" runat="server">
                <div class="col-md-12">&nbsp;</div>
                <div class="col-md-12">
                    <div class="row">
                        <div class="form-group col-md-3">
                            <span class="cptn">Institution</span><span class="reqd">*</span>
                            <infs:WclComboBox ID="cmbTenant" runat="server" Width="100%" AutoSkinMode="false"
                                Skin="Silk" CssClass="form-control" AutoPostBack="true" DataTextField="TenantName"
                                OnSelectedIndexChanged="cmbTenant_SelectedIndexChanged" OnDataBound="cmbTenant_DataBound"
                                DataValueField="TenantID" Filter="None" OnClientKeyPressing="openCmbBoxOnTab">
                            </infs:WclComboBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">&nbsp;</div>

        </div>

        <div id="dvDocumentsGrid" runat="server">
            <div class="upload-box-header">
                <h1>Upload Documents
                </h1>

            </div>

            <div class="upload-box">
                <infsu:UploadDocuments ID="ucUploadDocuments" runat="server" isDropZoneEnabled="false"></infsu:UploadDocuments>
            </div>



            <div class="row">
                <div class="col-md-12">
                    <h2 class="header-color">Map Documents</h2>
                </div>
            </div>
            <div class="row">

                <infs:WclGrid runat="server" ID="grdMapping" AllowPaging="True" AutoGenerateColumns="False" ShowClearFiltersButton="false"
                    AllowSorting="True" AllowFilteringByColumn="false"  AutoSkinMode="true" CellSpacing="0" PagerStyle-Position="TopAndBottom"
                     ShowAllExportButtons="false" ShowExtraButtons="False" GridLines="Both"
                    OnNeedDataSource="grdMapping_NeedDataSource" OnDeleteCommand="grdMapping_DeleteCommand" OnUpdateCommand="grdMapping_UpdateCommand"
                    OnItemCommand="grdMapping_ItemCommand" OnItemDataBound="grdMapping_ItemDataBound">
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true">
                        <ClientEvents OnRowDblClick="grd_rwDbClick" />
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="ApplicantDocumentID,DocumentTypeCode" AllowFilteringByColumn="false" PagerStyle-Position="TopAndBottom">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false"
                            ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowRefreshButton="true" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridTemplateColumn UniqueName="ExportCheckBox" AllowFiltering="false" ShowFilterIcon="false" HeaderStyle-Width="2%" ItemStyle-Width="2%">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelectDocument" runat="server" onclick="UnCheckHeader(this)"
                                        OnCheckedChanged="chkSelectDocument_CheckedChanged" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="ApplicantUploadedDocumentID" FilterControlAltText="Filter ApplicantUploadedDocumentID column"
                                HeaderText="ID" SortExpression="ApplicantUploadedDocumentID" UniqueName="ApplicantUploadedDocumentID" AllowFiltering="false"
                                Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FileName" FilterControlAltText="Filter FileName column" HeaderStyle-Width="350" AllowFiltering="false"
                                HeaderText="File Name" SortExpression="FileName" UniqueName="FileName" ReadOnly="true" HeaderTooltip="This column contains the name of each uploaded document">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FileType" FilterControlAltText="Filter FileType column" HeaderStyle-Width="80" AllowFiltering="false"
                                HeaderText="File Type" SortExpression="FileType" UniqueName="FileType" ReadOnly="true" HeaderTooltip="This column contains the type of each uploaded document">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Size" FilterControlAltText="Filter Size column" AllowFiltering="false"
                                HeaderText="Size (KB)" SortExpression="Size" UniqueName="Size" ReadOnly="true" HeaderTooltip="This column contains the file size of each uploaded document">
                                <HeaderStyle Width="80" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UploadedBy" FilterControlAltText="Filter UploadedBy column" HeaderStyle-Width="120" AllowFiltering="false"
                                HeaderText="Uploaded By" SortExpression="UploadedBy" UniqueName="UploadedBy" ReadOnly="true" HeaderTooltip="This column contains the uploader of uploaded document">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UploadedOn" FilterControlAltText="Filter UploadedOn column" HeaderStyle-Width="120" DataType="System.DateTime" DataFormatString="{0:d}" HeaderText="Uploaded On" SortExpression="UploadedOn" UniqueName="UploadedOn" ReadOnly="true" HeaderTooltip="This column contains the upload date of uploaded document" AllowFiltering="false">
                            </telerik:GridBoundColumn>

                            <telerik:GridTemplateColumn HeaderStyle-Width="300" HeaderText="Mapped Items" UniqueName="ItemName" Visible="false" HeaderTooltip="This column contains the mapped items of uploaded document" AllowFiltering="false">
                                <ItemTemplate>
                                    <asp:Label ID="hrefItem" runat="server" Text='<%# Convert.ToString(Eval("ItemName")).Length > 50 ? INTSOF.Utils.Extensions.HtmlEncode( Convert.ToString(Eval("ItemName")).Substring(0,30))+"...." : INTSOF.Utils.Extensions.HtmlEncode( Convert.ToString(Eval("ItemName"))) %>'></asp:Label>

                                    <%--<a id="hrefItem" runat="server">
                                        <%# Convert.ToString(Eval("ItemName")).Length > 50 ? Convert.ToString(Eval("ItemName")).Substring(0,30)+"...." : Eval("ItemName") %></a>--%>
                                    <infs:WclToolTip runat="server" ID="tltpItem" TargetControlID="hrefItem" Width="300px"
                                        Text='<%# INTSOF.Utils.Extensions.HtmlEncode( Convert.ToString( Eval("ItemName"))) %>' ManualClose="false" RelativeTo="Element"
                                        Position="TopRight" Visible='<%# Eval("ItemName").ToString().Length < 30? false : true %>'>
                                    </infs:WclToolTip>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderStyle-Width="300" HeaderText="Description" HeaderTooltip="This column displays the description of each uploaded document" AllowFiltering="false">
                                <ItemTemplate>
                                    <%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString( Eval("Description")))%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="padding: 10px;">
                                        <infs:WclTextBox runat="server" ID="txtDescription" Width="100%" Text='<%# Eval("Description").ToString()%>'>
                                        </infs:WclTextBox>
                                    </div>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>


                           <%-- <telerik:GridTemplateColumn HeaderStyle-Width="300" HeaderText="Description" HeaderTooltip="This column displays the description of each uploaded document">
                                <ItemTemplate>
                                    <a id="hrefDesc" runat="server">
                                        <%# Convert.ToString(Eval("Description")).Length > 50 ? Convert.ToString(Eval("Description")).Substring(0,30)+"...." : Eval("Description") %></a>
                                    <infs:WclToolTip runat="server" ID="tltpCatDesc" TargetControlID="hrefDesc" Width="300px"
                                        Text='<%# Eval("Description").ToString() %>' ManualClose="false" RelativeTo="Element"
                                        Position="TopRight" Visible='<%# Eval("Description").ToString().Length < 30? false : true %>'>
                                    </infs:WclToolTip>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <infs:WclTextBox runat="server" ID="txtDescription" Width="100%" Text='<%#Eval("Description")%>' MaxLength="500">
                                    </infs:WclTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                            <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageDocument">
                                <HeaderStyle Width="280" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <a runat="server" id="ancManageDocument" title="Click here to view the document">View Document</a>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <infs:WclComboBox ID="cmbItems" runat="server" Width="100%" CheckBoxes="true"
                                        OnItemDataBound="cmbItems_ItemDataBound"
                                        Filter="None" DataValueField="CategoryItemsID" DataTextField="ItemsName"
                                        OnClientItemChecked="OncmbItemChecked" OnClientKeyPressing="openCmbBoxOnTab">
                                    </infs:WclComboBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Click here to edit the description of the document." UniqueName="EditCommandColumn">
                                <HeaderStyle CssClass="tplcohdr" Width="25" />
                                <ItemStyle CssClass="MyImageButton" />
                            </telerik:GridEditCommandColumn>
                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this document?"
                                Text="Click here to delete this document from your uploaded documents list." UniqueName="DeleteColumn">
                                <HeaderStyle CssClass="tplcohdr" Width="30" />
                                <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                            </telerik:GridButtonColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </infs:WclGrid>
            </div>
            <div class="row">&nbsp;</div>

            <infsu:CommandBar ID="fsucCmdExport" runat="server" ButtonPosition="Center" DisplayButtons="Extra" UseAutoSkinMode="false" ButtonSkin="Silk"
                AutoPostbackButtons="Extra" ExtraButtonText="Print Document(s)" ExtraButtonIconClass="rbPrint" OnExtraClick="btnPrint_Click">
            </infsu:CommandBar>

        </div>
        <asp:HiddenField ID="hdnPrintDocumentURL" runat="server" />
        <asp:HiddenField ID="hdnDocumentAssociationSettingEnabled" runat="server" />
        <asp:HiddenField ID="hdnAlreadySelectedAppDocItemAssociationID" runat="server" />
        <asp:HiddenField ID="hfTenantId" runat="server" />
    </div>
</div>


<script type="text/javascript">
    //click on link button while double click on any row of grid.   

    function grd_rwDbClick(s, e) {
        var _id = "ancManageDocument";
        var b = e.get_gridDataItem().findElement(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
        //findElement findControl
    }

    function CheckAll(id) {
        var masterTable = $find("<%= grdMapping.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectDocument").checked = isChecked; // for checking the checkboxes
            }
        }
    }
    function UnCheckHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdMapping.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }

    function openApplicantDocumentToPrint() {
        
        var hdnPrintDocumentURL = $jQuery("[id$=hdnPrintDocumentURL]").val();
        var documentType = "Applicant Document";
        var url = $page.url.create(hdnPrintDocumentURL);
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var win = $window.createPopup(url,
                                         {
                                             size: "900," + popupHeight,
                                             behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize
                                         },
                                         function () {
                                             this.set_title("Instructor/Preceptor Document");
                                             this.set_destroyOnClose(true);
                                             this.set_status("");
                                         });

        parent.$jQuery('.rwMaximizeButton').on('click', function () {
            setTimeout(function () {
                parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
            }, 50);
        });

        setTimeout(function () {
            parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
        }, 650);

        parent.$jQuery('.rwTitlebarControls').on('click', function () {
            setTimeout(function () {
                parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
            }, 40);
        });
        return false;


        //winopen = true;
        //return false;
    }



    function OncmbItemChecked(sender, eventArgs) {
        var CheckedItem = eventArgs.get_item();
        if (CheckedItem.get_text() == 'Exception') {

            var exceptions = [];
            var FixedCategoryId = $jQuery("[id$=hdnAlreadySelectedAppDocItemAssociationID]").val();
            var lstFixedCategoryId = [];
            $jQuery.each(FixedCategoryId.split(","), function () { lstFixedCategoryId.push($jQuery.trim(this)); });

            if (CheckedItem.get_value().indexOf('_0') >= 0) {
                exceptions[CheckedItem.get_value()] = CheckedItem.get_checked();
            }
            sender.get_items().forEach(function (item) {

                if (item.get_value().indexOf('_-1') >= 0 || item.get_value().indexOf('_0') >= 0) return;

                var excvalue = item.get_value().substring(0, item.get_value().indexOf('_')).concat('_0');

                if (exceptions[excvalue] == undefined) {
                    var items = sender.findItemByValue(excvalue);
                    if (items != undefined) {
                        exceptions[excvalue] = sender.findItemByValue(excvalue).get_checked();
                    }
                    else {
                        exceptions[excvalue] = false;
                    }
                }

                if (exceptions[excvalue] == true) {

                    if (item.get_enabled()) {
                        item.set_checked(false);
                        item.disable();
                    }
                }
                else if (lstFixedCategoryId.indexOf(item.get_value()) < 0) {
                    item.enable();
                }

            });
        }
    }

    function onClientFileUploaded(radAsyncUpload, args) {
        showHideBrowseButton();
        var documentAssociationEnabled = $jQuery("[id$=hdnDocumentAssociationSettingEnabled]");
        var row = args.get_row(),
     inputName = radAsyncUpload.getAdditionalFieldID("TextBox"),
     inputType = "text",
     inputId = inputName,
     input = createInputControl(inputType, inputId, inputName),
        label = createInputLabel(inputId),
         br = document.createElement("br");
        //Code changes for UAT 2128- Added functionality to map the document with items in the category
        //Drop down will be shown when Document Association setting is enable.
        if (documentAssociationEnabled.val() == "1") {
            var inputHiddenType = "hidden",
                inputHidden = radAsyncUpload.getAdditionalFieldID("HiddedFeild"),
                 inputName2 = radAsyncUpload.getAdditionalFieldID("DropDown");
            var HiddenFeild = createInputHiddenFeild(inputHiddenType, inputHidden, inputHidden),
                dropdown = CreateSelect(inputName2, inputName2, inputHidden);
        }

        var IgnoreAlreadyUploadedDoc = false;
        var hdfIgnoreAlreadyUploadedDoc = $jQuery('[id$="hdfIgnoreAlreadyUploadedDoc"]');
        if (hdfIgnoreAlreadyUploadedDoc != undefined && hdfIgnoreAlreadyUploadedDoc.length > 0) {
            IgnoreAlreadyUploadedDoc = hdfIgnoreAlreadyUploadedDoc.val();
        }

        var organizationUserId = "";
        var hdnOrganizationUserId = $jQuery('[id$="hdfOrganizationUserId"]');
        if (hdnOrganizationUserId != undefined && hdnOrganizationUserId.length > 0) {
            organizationUserId = hdnOrganizationUserId.val();
        }


        row.appendChild(br);
        row.appendChild(label);
        row.appendChild(input);


        //Code changes for UAT 2128- Added functionality to map the document with items in the category
        //Drop down will be shown when Document Association setting is enable.
        if (documentAssociationEnabled.val() == "1") {
            row.appendChild(HiddenFeild);
            row.appendChild(dropdown);
        }

        if (radAsyncUpload.getUploadedFiles() != "") {
            showHideButton(true);
        }

        //Code changes for UAT 531- As a student, I should not be able to upload a duplicate document.
        var fileSize = args.get_fileInfo().ContentLength;
        var completeFileName = args.get_fileName();
        var selectedFileName = "";

        if (completeFileName.indexOf("\\") != -1)
            selectedFileName = completeFileName.substring(completeFileName.lastIndexOf("\\") + 1);
        else
            selectedFileName = completeFileName;

        //Added minimum file size check regarding UAT-862: WB: As a student or an admin, I should not be allowed to upload documents with a size of 0
        if (fileSize > 0) {
            //To check if duplicate file is uploading
            var isDuplicateFile = false;
            var uploadedFilesCount = $jQuery(radAsyncUpload._uploadedFiles).toArray().length;
            if (uploadedFilesCount > 1) {
                for (var fileindex = 0; fileindex < uploadedFilesCount - 1; fileindex++) {
                    if (radAsyncUpload._uploadedFiles[fileindex].fileInfo.FileName == selectedFileName && radAsyncUpload._uploadedFiles[fileindex].fileInfo.ContentLength == fileSize) {
                        isDuplicateFile = true;
                    }
                }
            }
            if (isDuplicateFile) {
                radAsyncUpload.deleteFileInputAt(selectedFileIndex);
                isDuplicateFile = false;
                radAsyncUpload.updateClientState();
                showHideBrowseButton();
                alert("You have already updated this document.");
                showHideBrowseButton();
                return;
            }
            //UAT-3593
            var tenantId = "";
            var hdnSelectedTenantId = $jQuery('[id$="hfTenantId"]');
            if (hdnSelectedTenantId != undefined && hdnSelectedTenantId.length > 0) {
                tenantId = hdnSelectedTenantId.val();
            }


            //Check if document is already uploaded.
            if (IgnoreAlreadyUploadedDoc == false) {
                //var isPersonalDocumentScreen = false;//parseInt($jQuery("[id$=hdnSelectedTab]").val()) == 1 ? false : true;
                PageMethods.IsReqDocumentAlreadyUploaded(selectedFileName, fileSize, organizationUserId, tenantId, checkCallBack);
            }
        }
        else {
            radAsyncUpload.deleteFileInputAt(selectedFileIndex);
            isDuplicateFile = false;
            radAsyncUpload.updateClientState();
            alert("File size should be more than 0 byte.");
        }
        showHideBrowseButton();
        return;
    }

</script>
