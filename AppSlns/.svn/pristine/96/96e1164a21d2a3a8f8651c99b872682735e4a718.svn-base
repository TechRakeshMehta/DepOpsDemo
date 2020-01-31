<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RotationRequirementDataEntry.ascx.cs" Inherits="CoreWeb.ApplicantRotationRequirement.Views.RotationRequirementDataEntry" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ReqItemForm" Src="~/ApplicantRotationRequirement/UserControl/RequirementItemForm.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/RotationDetails.ascx" TagName="RotationDetails" TagPrefix="uc" %>
<style>
    .textwrap {
        white-space: pre-wrap;
    }

    .msgbox1 .info {
        display: block;
        padding: 15px 10px 20px 53px;
        border-width: 1px;
        margin: 10px;
    }

    .msgbox1 .info {
        color: #3071cd !important;
        background-image: url('../../Resources/Themes/Default/images/info.png');
        background-color: #fffef0;
        background-position: 10px 8px;
        background-repeat: no-repeat;
    }

    a.headerName {
        font-size: 16px;
        font-weight: bold;
        color: #555 !important;
        padding: 5px 0;
        text-decoration: none;
    }

        a.headerName:hover {
            text-decoration: none;
        }
</style>
<script type="text/javascript">
    //function ShowToolTip(e) {
    //    var id = "#" + e.id;
    //    var ToolTipCustom = $jQuery(id).parent().siblings("#ToolTipCustom");
    //    var spnText = $jQuery(id).parent().siblings("#litCatDesc")[0].innerHTML;
    //    var resultHTML = spnText.substring(1, spnText.length - 1);
    //    ToolTipCustom[0].innerHTML = resultHTML;
    //    if (resultHTML != "")
    //        ToolTipCustom.show();
    //}
    //function HideToolTip(e) {
    //    var id = "#" + e.id;
    //    var ToolTipCustom = $jQuery(id).parent().siblings("#ToolTipCustom");
    //    ToolTipCustom.hide();
    //}

    function setFormMode(mode) {
        var control = $jQuery('[id$="hidEditForm"]');
        control.val(mode);

        if (mode == '2' || mode == '3') {
            return confirm('Are you sure you want to delete the selected record ?');

            return true;
        }
    }
</script>

<script>


    function OpenItemPaymentForm(url) {
        var popupWindowName = "Item Payment";
        var widht = (window.screen.width) * (90 / 100);
        var height = (window.screen.height) * (80 / 100);
        var popupsize = widht + ',' + height;
        var url = $page.url.create(url + "?popupHeight=" + parseInt(height));
        var win = $window.createPopup(url, { size: popupsize, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnClose }
        );

        return false;
    }

    function OnClose(oWnd, args) {
        oWnd.remove_close(OnClose);
        win = false;
        ItemPaymentRefreshClick();
    }
    // To close the popup.
    function ClosePopup() {
        top.$window.get_radManager().getActiveWindow().close();
    }




    function showHideBrowseButton() {
        if ($jQuery(".dvApplicantDocumentDropzone").length > 0) {
            $jQuery(".RadUpload .ruBrowse[value='Hidden']").remove();
            $jQuery(".ruButton .ruBrowse[value='Hidden']").remove();
            $jQuery(".ruButton .ruBrowse").remove();
            $jQuery(".ruDropZone").remove();
        }
    }

    function SetDropZone() {
        window.addEventListener("dragover", function (e) {
            e = e || event;
            e.preventDefault();
        }, false);
        window.addEventListener("drop", function (e) {
            e = e || event;
            e.preventDefault();
        }, false);
        var dropZone1 = $jQuery(".dvApplicantDocumentDropzone");
        if (dropZone1.length == 0) return;
        showHideBrowseButton();
        if (!Telerik.Web.UI.RadAsyncUpload.Modules.FileApi.isAvailable()) {
            var dropZone1 = $jQuery(".dvApplicantDocumentDropzone").innerHtml("<strong>Your browser does not support Drag and Drop. Please take a look at the info box for additional information.</strong>");
        }
        else {
            $jQuery(document).bind({ "drop": function (e) { e.preventDefault(); } });
            dropZone1.bind({ "dragenter": function (e) { dragEnterHandler(e, dropZone1); } })
                .bind({ "dragleave": function (e) { dragLeaveHandler(e, dropZone1); } })
                .bind({ "drop": function (e) { dropHandler(e, dropZone1); } });
        }
    };

    function dropHandler(e, dropZone1) {
        dropZone1[0].style.backgroundColor = "#ffffff";
    }

    function dragEnterHandler(e, dropZone1) {
        dropZone1[0].style.backgroundColor = "#f0f0f6";
    }

    function dragLeaveHandler(e, dropZone1) {
        dropZone1[0].style.backgroundColor = "#ffffff";
    }

    $jQuery(document).ready(function () {       
        showHideButton(false);
        var IsDocLinkExist = $jQuery('[id$="hdnDocLinkExist"]').val();
        if (IsDocLinkExist != null && IsDocLinkExist) {
            ShowHideRequiredDocumentTooltip(IsDocLinkExist);
        }
    });

    $page.add_pageLoad(function () {
        SetDropZone();
    });


    function ReqClientFileSelected(sender, args) {
        showHideBrowseButton();
        uploader = sender;
        selectedFileIndex = args.get_rowIndex();

        var fileInputs = sender._selectedFilesCount;
        var completeFileName = args.get_fileName();
        var selectedFileName = "";

        if (completeFileName.indexOf("\\") != -1)
            selectedFileName = completeFileName.substring(completeFileName.lastIndexOf("\\") + 1);
        else
            selectedFileName = completeFileName;

    }

    function onClientFileSelectedDandADocument(sender, args) {
        selectedFileIndex = args.get_rowIndex();

    }


    function OnClientFileUploadedReq(sender, args) {
        showHideBrowseButton();
        var fileSize = args.get_fileInfo().ContentLength;
        var completeFileName = args.get_fileName();
        var selectedFileName = "";
        var organizationUserId = "";
        if (completeFileName.indexOf("\\") != -1)
            selectedFileName = completeFileName.substring(completeFileName.lastIndexOf("\\") + 1);
        else
            selectedFileName = completeFileName;

        var hdnOrganizationUserId = $jQuery('[id$="hdfOrganizationUserId"]');
        if (hdnOrganizationUserId != undefined && hdnOrganizationUserId.length > 0) {
            organizationUserId = hdnOrganizationUserId.val();
        }

        //Added minimum file size check regarding UAT-862: WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 byte
        if (fileSize > 0) {
            //To check if duplicate file is uploading
            var isDuplicateFile = false;
            var uploadedFilesCount = $jQuery(sender._uploadedFiles).toArray().length;
            if (uploadedFilesCount > 1) {
                for (var fileindex = 0; fileindex < uploadedFilesCount - 1; fileindex++) {
                    if (sender._uploadedFiles[fileindex].fileInfo.FileName == selectedFileName && sender._uploadedFiles[fileindex].fileInfo.ContentLength == fileSize) {
                        isDuplicateFile = true;
                    }
                }
            }
            if (isDuplicateFile) {
                sender.deleteFileInputAt(selectedFileIndex);
                isDuplicateFile = false;
                sender.updateClientState();
                showHideBrowseButton();
                alert("You have already updated this document.");
                showHideBrowseButton();
                return;
            }
            //Check if document is already uploaded.
            PageMethods.IsDocumentAlreadyUploaded(selectedFileName, fileSize, organizationUserId, checkCallBack);
        }
        else {
            sender.deleteFileInputAt(selectedFileIndex);
            sender.updateClientState();
            alert("File size should be more than 0 byte.");
            return;
        }
        showHideBrowseButton();
    }

    //callBack after the check has been done
    function checkCallBack(result) {
        if (result) {
            showHideBrowseButton();
            alert('You have already updated this document.');
            uploader.deleteFileInputAt(selectedFileIndex);
            uploader.updateClientState();
            showHideBrowseButton();
        }
    }

    //Code changes for UAT 2128- create a label with css as a combobox.
    function CreateSelect(inputId, inputname, inputHiddenid) {
        var inputbuttonControl = document.createElement("label");
        inputbuttonControl.innerHTML = 'Select Requirement(s)';
        inputbuttonControl.setAttribute("id", inputId);
        inputbuttonControl.setAttribute("name", inputname);
        inputbuttonControl.style.border = "1px solid #ccc";
        inputbuttonControl.style.height = "28px";
        inputbuttonControl.style.marginBottom = "5px";
        inputbuttonControl.style.lineHeight = "28px";
        inputbuttonControl.style.verticalAlign = "middle";
        inputbuttonControl.style.background = "linear-gradient(to bottom, #e6e6e6 0%,#ffffff 100%)";
        inputbuttonControl.style.marginLeft = "20px";
        inputbuttonControl.style.width = "40%";
        inputbuttonControl.style.textAlign = "left";
        inputbuttonControl.style.fontSize = "13px";
        inputbuttonControl.style.borderRadius = "4px";
        inputbuttonControl.style.fontFamily = "Titillium Web , sans-serif";
        inputbuttonControl.setAttribute("class", "rcbArrowCell");
        inputbuttonControl.setAttribute("onclick", "callComboBox('" + inputHiddenid + "','" + inputId + "')");
        return inputbuttonControl;
    }

    //Code changes for UAT 2128- create a hideen field to save the selected dropdown items id.
    function createInputHiddenFeild(inputType, inputId, inputName) {
        var inputhidden = document.createElement("input");
        inputhidden.setAttribute("type", inputType);
        inputhidden.setAttribute("id", inputId);
        inputhidden.setAttribute("name", inputName);
        return inputhidden;
    }

    //Code changes for UAT 2128- call the combo box and show its drop down list under the label.
    function callComboBox(sender, args) {
        var cbmitems = $find($jQuery("[id$=cmbItems]")[0].id);
        var dropdownlabel = $jQuery("[id$=" + args + "]")[0];
        cbmitems.set_offsetY(dropdownlabel.getClientRects()['0'].top + 28);
        cbmitems.set_offsetX(dropdownlabel.offsetLeft + 80);
        cbmitems._dropDownWidth = dropdownlabel.offsetWidth;


        cbmitems.trackChanges();
        if (window.hidden_id != sender) {
            window.hidden_id = sender;
            window.hidden1_id = args;


            var items = $jQuery('#' + window.hidden_id).val();
            cbmitems.get_checkedItems().forEach(function (item) { item.set_checked(false); });
            items.split(',').forEach(function (val) {
                var item = cbmitems.findItemByValue(val);
                if (item != null) { if (!item.get_enabled()) { item.enable(); } item.set_checked(true); }
            });
            enabledisableitems();
        }
        cbmitems.showDropDown();
        cbmitems.commitChanges();
    }
    function OnDropdownClosed(sender, eventArgs) {
        var idlist = '';
        //var idtext = '';
        var cbmitems = $find($jQuery("[id$=cmbItems]")[0].id);
        cbmitems.get_checkedItems().forEach(function (item) {

            idlist = idlist.concat(item.get_value(), ',');
            // idtext += idtext.concat(item.get_text(), ',');
        });
        $jQuery('#' + window.hidden_id).val(idlist.substr(0, idlist.length - 1));
        var dropdownlabel = $jQuery("[id$=" + window.hidden1_id + "]")[0];
        if (cbmitems.get_checkedItems().length > 0) {
            dropdownlabel.innerHTML = cbmitems.get_checkedItems().length + ' item selected';
        }
        else {
            dropdownlabel.innerHTML = 'Select Requirement(s)';
        }
    }

    //function createInputLabel(attr) {
    //    var inputLabel = document.createElement("label");
    //    inputLabel.setAttribute("for", attr);
    //    inputLabel.innerHTML = "Description: ";
    //    return inputLabel;
    //}

    function onFileRemoved(sender, args) {
        showHideBrowseButton();
        window.setTimeout(function () { showHideBrowseButton(); }, 0);
        window.setTimeout(function () { showHideBrowseButton(); }, 100);
    }

    function showHideButton(visible) {
        if (visible) {
            $jQuery("[id$=btnUploadAll]").show();
            $jQuery("[id$=btnUploadCancel]").show();
        }
        else {
            $jQuery("[id$=btnUploadAll]").hide();
            $jQuery("[id$=btnUploadCancel]").hide();
        }
    }

    var upl_OnClientValidationFailedReq = function (s, a) {
        var error = false;
        var errorMsg = "";
        var extn = a.get_fileName().substring(a.get_fileName().lastIndexOf('.') + 1, a.get_fileName().length);

        if (a.get_fileName().lastIndexOf('.') != -1) {
            if (s.get_allowedFileExtensions().indexOf(extn) == -1) {
                error = true;
                errorMsg = "Error: Unsupported File Format";
            }
            else {
                error = true;
                errorMsg = "Error: File size exceeds 20 MB";
            }
        }
        else {
            error = true;
            errorMsg = "Error: Unrecognized File Format";
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

    function OnClientFileUploadingReq(sender, args) {
        if (args.get_fileName().length > 100) {
            args.set_cancel(true);

            //UAT-3181
            var row = args.get_row();
            smsg = document.createElement("span");
            smsg.innerHTML = "Document name is not valid. Please make sure document name should not exceed 100 characters and should not contain special character(s).";
            smsg.setAttribute("class", "ruFileWrap");
            smsg.setAttribute("style", "color:red;padding-left:10px;");
            row.appendChild(smsg);
        }
        else {
            args.set_cancel(false);
        }
    }

    function ShowCallBackMessage(docMessage) {
        if (docMessage != '') {
            alert(docMessage);
        }
    }
    $page.showAlertMessageWithTitle = function (msg, msgtype, headerText, overriderErrorPanel) {
        if (typeof (msg) == "undefined") return;
        var c = typeof (msgtype) != "undefined" ? msgtype : "";
        if (overriderErrorPanel) {
            $jQuery("#pageMsgBoxPkgCompletion").children("span")[0].innerHTML = msg;
            $jQuery("#pageMsgBoxPkgCompletion").children("span").attr("class", msgtype);

            c = headerText;

            $jQuery("[id$=pnlErrorPkgCompletion]").hide();

            $window.showDialog($jQuery("#pageMsgBoxPkgCompletion").clone().show(), { closeBtn: { autoclose: true, text: "OK" } }, 400, c);
        }
        else {
            $jQuery("#pageMsgBoxPkgCompletion").fadeIn().children("span")[0].innerHTML = msg;
            $jQuery("#pageMsgBoxPkgCompletion").fadeIn().children("span").attr("class", msgtype);

        }
    }
</script>
<asp:UpdatePanel ID="pnlErrorPkgCompletion" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="msgbox" id="pageMsgBoxPkgCompletion" style="overflow-y: auto; max-height: 400px">
            <asp:Label CssClass="info" EnableViewState="false" runat="server" ID="lblError"></asp:Label>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<infs:WclResourceManagerProxy runat="server" ID="rprxCompliancePackageDetails">
    <infs:LinkedResource Path="~/Resources/Mod/ApplicantRotationRequirement/main.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/ApplicantRotationRequirement/ApplicantRequirementDataEntry.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div onclick="setPostBackSourceDE(event, this);" style="overflow-y: visible;">
    <asp:HiddenField ID="hdnShowHide" runat="server" Value="true" />
    <asp:TextBox ID="hdnPostbacksource" class="postbacksource" runat="server" Style="display: none;" />
    <%--<uc:RotationDetails id="ucRotationDetails" runat="server"></uc:RotationDetails>--%>
    <asp:Panel ID="pnlRotationDetails" runat="server"></asp:Panel>
    <div class="right" id="dvRotationComplianceStatus" runat="server">
        <div class="status_box" runat="server" id="dvComplianceStatus">
            <span class="fln">Rotation Compliance Status&nbsp;<span class="not"><asp:Label ID="lblComplianceStatus"
                runat="server" Text=""></asp:Label></span>&nbsp;
                <asp:Image ID="imgPackageComplianceStatus" runat="server" CssClass="img_status" />&nbsp;&nbsp;<asp:Label ID="lblComplianceCategoryStatus"
                    runat="server" Visible="false"></asp:Label></span>
        </div>
    </div>
    <div class="section nobg">
        <div class="content" style="overflow: visible;">
            <div id="divApplicantName" runat="server" style="display: none;">
                <span style="font-weight: bold; font-size: 16px; color: #555 !important;">Applicant Name: </span>
                <label id="lblApplicantName" style="font-size: 16px;" runat="server"></label>
            </div>
            <div id="divInstructorName" runat="server" style="display: none;">
                <span style="font-weight: bold; font-size: 16px; color: #555 !important;">Instructor/Preceptor Name: </span>
                <label id="lblInstructorName" style="font-size: 16px;" runat="server"></label>
            </div>

            <infs:WclTreeList runat="server" ID="tlistRequirementData" ClientKey="tlistRequirementData" AllowPaging="false" DataKeyNames="NodeID"
                ParentDataKeyNames="ParentNodeID" AutoGenerateColumns="false" ShowTreeLines="false"
                OnNeedDataSource="tlistRequirementData_NeedDataSource" ClientDataKeyNames="NodeID"
                OnItemCommand="tlistRequirementData_ItemCommand" Skin="Office2007" AutoSkinMode="true"
                OnItemDataBound="tlistRequirementData_ItemDataBound" OnItemCreated="tlistRequirementData_ItemCreated">
                <ClientSettings Resizing-AllowColumnResize="true">
                </ClientSettings>
                <Columns>
                    <telerik:TreeListTemplateColumn DataField="Category.CategoryName" UniqueName="CategoryName"
                        HeaderText="Compliance Category/Item">
                        <HeaderTemplate>
                            <span>Compliance Category/Item</span> (<asp:LinkButton ID="LinkButton1" runat="server"
                                Text="Expand" CommandName="ExpandAll" CssClass="lnkToggleButton" ToolTip="Click to expand the details for all Categories below" />&nbsp;/
                        <asp:LinkButton
                            ID="LinkButton2" runat="server" Text="Collapse" CommandName="CollapseAll" CssClass="lnkToggleButton"
                            ToolTip="Click to collapse the details for all Categories below" />)
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%-- <div id="ToolTipCustom" style="display: none" class="tooltipCustom"></div>--%>
                            <asp:Image ID="imgStatus" ImageUrl='<%# Eval("ImageReviewStatusPath") %>' runat="server"
                                CssClass="img_cat_stat" Visible='<%# Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_REQUIRED_REQUIREMENT_CATEGORY_NODE  
                                                                                    || Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_OPTIONAL_REQUIREMENT_CATEGORY_NODE 
                                                                ? true : false %>'
                                ToolTip='<%# Convert.ToString(Eval("ImgReviewStatus")) %>' />

                            <span id="spnHeader" class="spnCatName">
                                <asp:LinkButton ID="lnlSectionText" runat="server" Enabled="false"
                                    Text='<%#  INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Name"))) %>' Visible='<%# Convert.ToString(Eval("ParentNodeID")).Trim() == String.Empty 
                                                                                     ? true : false %>'
                                    CssClass="headerName" />
                            </span>

                            <span id="spnCatName" class="spnCatName" runat="server">
                                <asp:LinkButton ID="LinkButton3" runat="server" Enabled='<%# Eval("IsCategoryDataEntered") %>'
                                    Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Name"))) %>' Visible='<%# Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_REQUIRED_REQUIREMENT_CATEGORY_NODE  
                                                                                    || Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_OPTIONAL_REQUIREMENT_CATEGORY_NODE 
                                                                ? true : false %>'
                                    CssClass="catname" CommandName="ExpandCollapse" />
                            </span>
                            <div class="item_name">
                                <asp:Image ID="Image5" ImageUrl="~/App_Themes/Default/images/icons/itm.gif" runat="server"
                                    Visible='<%# Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_REQUIRED_REQUIREMENT_CATEGORY_NODE  
                                              || Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_OPTIONAL_REQUIREMENT_CATEGORY_NODE 
                                              || Eval("ParentNodeID").ToString().Trim() == String.Empty ? false : true %>' />

                                <asp:Label ID="lblReqName" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Name"))) %>' runat="server" CssClass='reqname'
                                    Visible='<%# Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_REQUIRED_REQUIREMENT_CATEGORY_NODE  
                                              || Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_OPTIONAL_REQUIREMENT_CATEGORY_NODE 
                                              || Eval("ParentNodeID").ToString().Trim() == String.Empty ? false : true %>'
                                    ToolTip="" />
                            </div>
                            <table class="tbl-data" runat="server" id="tblData"
                                visible='<%# Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_REQUIRED_REQUIREMENT_CATEGORY_NODE  
                                              || Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_OPTIONAL_REQUIREMENT_CATEGORY_NODE 
                                              || Eval("ParentNodeID").ToString().Trim() == String.Empty ? false : true %>'>
                                <tr>
                                    <td class="tbl-data-col col-one">
                                        <div title="This section displays the information that has been submitted for this requirement">
                                            <h2 class="col-hdr">Submitted Data
                                            </h2>
                                        </div>
                                        <div class="attr_grp1">
                                            <%#Eval("FieldHtml") == null ? Eval("FieldHtmlItem") : Eval("FieldHtml")%>
                                        </div>
                                        <div class="attr_details">
                                            <%#Eval("fieldHtmlDescription")%>
                                        </div>
                                        <div class="attr_grp2">
                                            <%#Eval("FieldHtml") == null?String.Empty:Eval("FieldHtmlItem")%>
                                        </div>
                                    </td>
                                    <td class="tbl-data-col col-two">
                                        <div title="This section displays any rejection reason submitted by an administrator for this item">
                                            <h2 class="col-hdr">Rejection Reason
                                            </h2>
                                        </div>
                                        <div class="rej_reason">
                                            <asp:Label ID="lblRejectionReason" Text='<%# Eval("ItemRejectionReason") %>' runat="server"
                                                CssClass="info"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:TreeListTemplateColumn>
                    <telerik:TreeListTemplateColumn HeaderText="" UniqueName="ItemDataColumn">
                        <HeaderTemplate>
                            <div id="TOOLTIP100" class="tooltip_hint">
                                &nbsp;
                            </div>
                        </HeaderTemplate>
                        <HeaderStyle Width="200" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <!-- Item Template for Enter requirements Column links -->
                            <div id="dvAddRequirement" class="cmdbox" runat="server" visible='<%# Eval("ShowAddRequirement") %>'>
                                <asp:LinkButton ID="btnAddReq" CommandArgument='<%# Eval("NodeID") %>' Text="Enter Requirement"
                                    CssClass="lnkEnterRequirement" OnClientClick="setFormMode(0)" runat="server"
                                    CommandName="InitInsert" Visible='<%# Eval("ShowAddRequirement") %>' ToolTip="Click to enter data for a requirement in this category">
                                 <span class="rtlAdd icon_block" >&nbsp;</span>Enter Requirement
                                </asp:LinkButton>
                            </div>
                            <div id="dvViewRequirement" class="cmdbox" runat="server" visible='<%# Eval("ShowViewRequirement") %>'>
                                <asp:LinkButton ID="btnViewReq" CommandArgument='<%# Eval("NodeID") %>' Text="View Requirement"
                                    CssClass="lnkEnterRequirement" OnClientClick="setFormMode(0)" runat="server" OnCommand="btnViewReq_Command"
                                    CommandName="InitInsert" Visible='<%# Eval("ShowViewRequirement") %>' ToolTip="Click to view data for a requirement in this category">
                                 <span class="rtlAdd icon_block" >&nbsp;</span>View Requirement
                                </asp:LinkButton>
                            </div>
                            <div id="dvUpdateRequirement" runat="server" class="cmdbox" visible='<%# !Convert.ToBoolean(Eval("IsParent")) %>'>
                                <asp:LinkButton ID="btnUpdReq" Text="Update" runat="server" CommandName="Edit" OnClientClick="setFormMode(1)"
                                    CommandArgument='<%# Eval("NodeID") %>' Visible='<%# (Convert.ToBoolean(Eval("ShowItemEditDelete")))? true : false %>'
                                    ToolTip="Click here to edit the information for this requirement">
                                    <span class="rtlEdit icon_block">&nbsp;</span>&nbsp;Update
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnDeleteItem" CommandArgument='<%# Eval("ApplicantRequirementItemDataId") %>'
                                    OnClientClick="javascript:return setFormMode(2)" Text="Delete" runat="server"
                                    CommandName="Delete" Visible='<%# Eval("ShowItemDelete") %>' ToolTip="Click here to delete the entered data for this requirement">
                                    <span class="rtlRemove icon_block">&nbsp</span>Delete
                                </asp:LinkButton>


                            </div>
                        </ItemTemplate>
                    </telerik:TreeListTemplateColumn>
                    <telerik:TreeListTemplateColumn DataField="ReviewStatus" UniqueName="ReviewStatus"
                        HeaderText="Status" HeaderTooltip="This column displays the status for each Category and Item for which you have entered data">
                        <HeaderStyle Width="130" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="lblReviewStatus" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ReviewStatus"))) %>' runat="server" />
                        </ItemTemplate>
                    </telerik:TreeListTemplateColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <asp:Panel runat="server" ID="pnlEntryForm">
                            <div id="dvExplanatoryNotesItem" runat="server">
                                <asp:Repeater ID="rptExplanatoryNotes" runat="server">
                                    <ItemTemplate>
                                        <div class="content">
                                            <div class="sxform auto">
                                                <div id="dvMainExpNotes" class="expNotes">
                                                    <div id="dvMsgBox" class="msgbox bullet expNotes" runat="server">
                                                        <asp:Label ID="lblExplanatoryNotes" Text='<%# Container.DataItem %>' runat="server" CssClass="info">
                                                        </asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="section" id="dvAddNewRequirement" runat="server">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblEDTFormHdr" Text='<%# !IsEditing(Container) ? "Add New Requirement" : "Update Requirement" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <!-- Note: Please donot insert anything here. There should be nothing between content and form divs -->
                                    <div class="sxform auto">
                                        <div id="dvBox" class="msgbox1 bullet " runat="server" visible='<%# IsEditing(Container) ? false : true %>'>
                                            <asp:Label ID="lblForm" runat="server" CssClass="info">
                                            </asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl main_pnl" ID="pnlName1">
                                            <div id="Div5" class='sxro sx3co' runat="server" visible='<%# IsEditing(Container) ? false : true %>'>
                                                <div id="divSelectRequirement" runat="server">
                                                    <div class='sxlb' title="Choose the requirement for which you will be entering data">
                                                        <span class='cptn'>Select a requirement</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclComboBox runat="server" ID="cmbRequirement" AutoPostBack="true" DataValueField="RequirementItemID" Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab"
                                                            DataTextField="RequirementItemName" />
                                                        <%--<infs:WclToolTip runat="server" ID="tltpCatExplanation" TargetControlID="cmbRequirement"
                                                            Width="300px" ManualClose="false" RelativeTo="Element" Position="TopCenter">
                                                            Select an item you wish to add. Read the instruction above to enter the minimum
                                                    number of items required to make you compliant.
                                                        </infs:WclToolTip>--%>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <%-- <asp:Panel ID="pnlItemInfo" runat="server" Visible="false">
                                                <div class="item_info_bottom">
                                                </div>
                                                <div class="item_info">
                                                     <div id="ItemToolTipCustom" runat="server" style="display: none" class="tooltipCustom"></div> onmouseover="ShowItemToolTip(this);" onmouseout="HideItemToolTip(this);"
                                                    <span id="spnItemInfo" runat="server">fill the form below for
                                                    <asp:Label runat="server" ID="lblItemName" />
                                                    </span>
                                                </div>
                                            </asp:Panel>--%>
                                            <infsu:ReqItemForm ID="itemForm" runat="server" ReadOnly="true" Visible="true" />
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="cmdBar" runat="server" TreeListMode="true" DefaultPanel="pnlName1" GridUpdateText="Save" ClientIDMode="Static"
                                        Visible="false" ButtonPosition="Right" GridInsertText="Submit" />
                                    <div style="text-align: right; padding-top: 10px;">
                                        <infs:WclButton ID="btnCancel" Text="Close" CommandName="Cancel" Icon-PrimaryIconCssClass="rbCancel" Visible="false" runat="server"></infs:WclButton>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </FormTemplate>
                </EditFormSettings>
            </infs:WclTreeList>
            <div id="divMainNotes" runat="server">
                <div id="divNotes" runat="server" style="padding-top: 17px;">
                    <div>
                        <h1 class="mhdr" style="cursor: default;">Notes</h1>
                    </div>
                    <infs:WclTextBox ID="txtNotes" runat="server" TextMode="MultiLine" Height="100px" CssClass="textwrap" Width="100%"></infs:WclTextBox>
                    <div class="vldx">
                        <%--<asp:RequiredFieldValidator runat="server" ID="rfvNotes" ControlToValidate="txtNotes"
                            Display="Dynamic" CssClass="errmsg" Text="Notes is required." ValidationGroup="grpSubmitNotes" />--%>
                        <asp:CustomValidator runat="server" ErrorMessage="Please don't enter more than 1000 characters." ID="cvNotes"
                            ClientValidationFunction="ValidateLength" CssClass="errmsg" ControlToValidate="txtNotes"
                            Display="Dynamic" ValidationGroup="grpSubmitNotes"></asp:CustomValidator>
                    </div>
                </div>
                <div style="text-align: center; margin-top: 5px">
                    <infs:WclButton ID="btnSaveNotes" Icon-PrimaryIconCssClass="rbSave" AutoPostBack="true" Text="Save Notes" ValidationGroup="grpSubmitNotes" runat="server" OnClick="btnSaveNotes_Click"></infs:WclButton>
                </div>
            </div>
        </div>
    </div>
    <infsu:CommandBar ID="fsucCmdBar" runat="server" Visible="false" ButtonPosition="Center" DisplayButtons="Submit" SubmitButtonIconClass=""
        SubmitButtonText="Return To Manage Invitations" AutoPostbackButtons="Submit" OnSubmitClick="fsucCmdBar_SubmitClick">
    </infsu:CommandBar>
</div>
<div id="ItemExpiredAlertPopUpDiv" class="ItemExpiredAlertPopUp" runat="server" style="max-height: 400px; display: none">
    <asp:Label ID="lblExpiryItemMsg" runat="server"></asp:Label>
</div>
<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
<asp:Button ID="btnAutoSubmit" runat="server" CssClass="buttonHidden" OnClick="btnAutoSubmit_Click" />
<asp:HiddenField ID="hidEditForm" runat="server" />
<asp:HiddenField ID="hdRequirementCategoryId" runat="server" />
<asp:HiddenField ID="hdRequirementPackageId" runat="server" />
<asp:HiddenField ID="hdfRequirementItemId" runat="server" />
<asp:HiddenField ID="hdfTenantId" runat="server" />
<asp:HiddenField ID="hdfRequirementPkgSubscriptionID" runat="server" />
<asp:HiddenField ID="hdnClinicalRotationID" Value="0" runat="server" />
<asp:HiddenField ID="hdnOrganizationUserId" runat="server" />
<asp:HiddenField ID="hdnFieldDataType" runat="server" />
<asp:HiddenField ID="hdnreqItemFieldId" runat="server" />
<asp:HiddenField ID="hdnReqFieldAttributeValue" runat="server" />
<asp:HiddenField ID="hdnReqFieldDataTypeCode" runat="server" />
<asp:HiddenField ID="hdnSignature" runat="server" />
<asp:HiddenField ID="hdfReqItemDataId" runat="server" />

<asp:HiddenField ID="hdfIsReqDocViewed" runat="server" />
<asp:HiddenField ID="hdfReqViewedDocPath" runat="server" />
<asp:HiddenField ID="hdfReqDocFileName" runat="server" />
<asp:HiddenField ID="hdnfReqIsAutoSubmitTriggerForItem" runat="server" />
<!-- Document and video type fields value -->
<%--<asp:HiddenField ID="hdfIsViewVideoRequired" runat="server" Value="0" />
    <asp:HiddenField ID="hdfIsViewDocumentRequired" runat="server" Value="0"/>
    <asp:HiddenField ID="hdfIsVideoViewed" runat="server"  Value="0"/>
    <asp:HiddenField ID="hdfIsDocumentViewed" runat="server" Value="0"/>
    <asp:HiddenField ID="hdfVideoViewedTime" runat="server" Value="0" />--%>

<asp:HiddenField runat="server" ID="hdnCategoryName" />
<asp:HiddenField runat="server" ID="hdnCmbName" />

<asp:HiddenField runat="server" ID="hdnSignatureMinLengh" Value="" />
<asp:HiddenField runat="server" ID="hdnDocLinkExist" Value="" />

