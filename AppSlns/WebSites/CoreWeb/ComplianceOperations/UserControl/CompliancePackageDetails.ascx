<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.CompliancePackageDetails" CodeBehind="CompliancePackageDetails.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ItemForm" Src="~/ComplianceOperations/UserControl/ItemForm.ascx" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>
<%@ Register TagName="AttributeControl" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/AttributeControl.ascx" %>
<%@ Register TagName="RowControl" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/RowControl.ascx" %>
<%--<%@ Register TagName="UploadDocuments" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/UploadDocuments.ascx" %>--%>
<infs:WclResourceManagerProxy runat="server" ID="rprxCompliancePackageDetails">
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/main.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/CompliancePackageDetails.js" ResourceType="JavaScript" />
    <%--<infs:LinkedResource Path="~/Resources/Mod/ComplianceDataEntry/Scripts/UploadDocuments.js" ResourceType="JavaScript" />--%>
</infs:WclResourceManagerProxy>
<style>
    .tooltipCustom {
        margin: -20px 20px 20px 150px;
        border: none;
        background-color: white;
        box-shadow: 0 10px 10px rgba(0, 0, 0, 0.2);
        position: absolute;
        z-index: 2;
        padding: 5px;
        max-width: 400px;
        max-height: 100px;
        overflow: auto;
    }

    .bullet ul {
        margin-left: 10px;
        padding-left: 10px !important;
    }

    .bullet li {
        list-style-position: inside;
        list-style: disc;
    }

    .tooltipCustom ul {
        margin-left: 10px;
        padding-left: 10px !important;
    }

    .tooltipCustom li {
        list-style-position: inside;
        list-style: disc;
    }

    .bullet ol {
        list-style-type: decimal;
        margin-left: 10px;
        padding-left: 10px;
    }

        .bullet ol li {
            list-style: decimal;
        }

    .tooltipCustom ol {
        list-style-type: decimal;
        margin-left: 10px;
        padding-left: 10px;
    }

        .tooltipCustom ol li {
            list-style: decimal;
        }

    .RadTreeList .rtlRemove {
        background-image: Url('/App_Themes/Default/images/Delete.gif');
        background-repeat: no-repeat;
        padding-left: 18px;
    }

    .RadTreeList .rtlAddException {
        background-image: Url('/Resources/Mod/Compliance/icons/applyexceptn.png');
        background-repeat: no-repeat;
        padding-left: 18px;
    }

    .expNotes.msgbox .info {
        color: black !important;
    }

    .expNotes {
        color: black !important;
    }

    .subdate {
        float: right;
    }

    .attr_grp1 table td {
        font-size: 13px;
    }

    td.td-one, td.td-two, td.td-three {
        font-size: 11px !important;
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

    .box__dragndrop,
    .box__uploading,
    .box__success,
    .box__error {
        display: none;
    }

    .issue-drop-zone {
        /*border: 1px dashed #ccc;*/
        border-top-color: rgb(204, 204, 204);
        border-right-color: rgb(204, 204, 204);
        border-bottom-color: rgb(204, 204, 204);
        border-left-color: rgb(204, 204, 204);
        border-radius: 0;
        padding: 7px;
        padding-left: 7px;
    }

    .issue-drop-zoneItemForm {
        border: 1px dashed #ccc;
        border-top-color: rgb(204, 204, 204);
        border-right-color: rgb(204, 204, 204);
        border-bottom-color: rgb(204, 204, 204);
        border-left-color: rgb(204, 204, 204);
        border-radius: 0;
        background-color: white;
        padding: 7px;
        transition: background-color .01s linear .01s;
        position: relative;
        margin-right: -10.6%;
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

    .issue-drop-zone__drop-icon {
        position: relative;
    }

    .issue-drop-zone button {
        color: #0052cc;
    }

    .issue-drop-zone__button {
        position: relative;
        cursor: pointer;
        color: #3572b0;
        background: transparent;
        padding: 0;
        border: 0;
        font-family: inherit;
        font-size: inherit;
    }

        .issue-drop-zone__button:hover {
            text-decoration: underline;
        }

    .k-button {
        background-color: #8C1921 !important;
        background-image: none !important;
        border: medium none !important;
        border-radius: 7px !important;
        color: #fff !important;
        height: 30px !important;
        line-height: 30px !important;
        padding: 0 15px !important;
    }
</style>
<script>

    function FocusSetFunction() {
        var ActiveElement = document.activeElement;
        var FocusId = '';
        var container = $jQuery("div[id$=pnLower]");
        if ($jQuery('#hdnEnterRequirement').val() == 'true') {
            $jQuery('#txtInserted').focus();
            FocusId = $jQuery('#txtInserted');
        }
        else if ($jQuery('#hdnEnterRequirement').val() == 'ApplyException') {
            $jQuery('#txtApplyException').focus();
            FocusId = $jQuery('#txtApplyException');
        }
        else {
            if (ActiveElement.id != '') {
                FocusId = $jQuery('#' + ActiveElement.id);
            }
        }
        if (container != '' && FocusId != '' && FocusId.offset() != undefined) {
            container.animate({
                scrollTop: FocusId.offset().top - container.offset().top + container.scrollTop()
            }, 1);
        }
    }
    function OpenItemPaymentForm(url) {
        var popupWindowName = "Item Payment";
        var widht = (window.screen.width) * (90 / 100);
        var height = (window.screen.height) * (80 / 100);
        var popupsize = widht + ',' + height;
        var url = $page.url.create(url + "&popupHeight=" + parseInt(height));
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

    function ExceptionAlertPopUp() {
        $window.showDialog($jQuery("[id$=ExceptionAlertPopUpDiv]").clone().show(), {
            closeBtn: {
                autoclose: true, text: "Close", click: function () {
                    $jQuery("[id$=lblExceptionMsg]")[0].innerText = "";
                }
            }
        }, 475, 'Alert');
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
<div onclick="setPostBackSourceDE(event, this);" style="overflow-y: visible;">
    <asp:HiddenField ID="hdnShowHide" runat="server" Value="true" />
    <asp:TextBox ID="hdnPostbacksource" class="postbacksource" runat="server" Style="display: none;" />
    <div id="vermod_cmds">
        <%--<asp:LinkButton Text="Back to Search" runat="server" ID="lnkBackSearch" OnClick="lnkBackSearch_click"
        Visible="false" />--%>
        <a runat="server" id="lnkBackSearch" visible="false" onclick="Page.showProgress('Processing...');">Back to Search</a>
        &nbsp;&nbsp;
    <%--    <asp:LinkButton Text="Go to Verification Details" runat="server" ID="lnkVerificationDetails"
        OnClick="lnkVerificationDetails_click" Visible="false" />--%>
        <a runat="server" id="lnkVerificationDetails" visible="false" onclick="Page.showProgress('Processing...');">Go to Verification Details</a>
    </div>
    <div class="pack_info">
        <div class="left">
            <div class="status_box" runat="server" id="dvComplianceStatus">
                <span class="fln">Overall Compliance Status&nbsp;<span class="not"><asp:Label ID="lblComplianceStatus"
                    runat="server" Text=""></asp:Label></span>&nbsp;
                <asp:Image ID="imgPackageComplianceStatus" runat="server" CssClass="img_status" />&nbsp;&nbsp;<asp:Label ID="lblComplianceCategoryStatus"
                    runat="server" Visible="false"></asp:Label></span>
            </div>
        </div>
        <div class="right">
            <div class="pkg_info" id="dvPkgInfo" runat="server">
                <asp:Image ID="Image1" ImageUrl="~/App_Themes/Default/images/icons/pkg.gif" runat="server" />
                <asp:Label ID="lblPackageName" runat="server" Text="" CssClass="strong"></asp:Label>
            </div>
            <a href="#" style="display: none;">My Contact Info</a>
        </div>
        <div style="clear: both">
        </div>
    </div>
    <table class="topbar" runat="server" id="tblComplBar">
        <tr>
            <td class="tltl"></td>
            <td>
                <div class="abut">
                    <a href="javascript:void" onclick="openPopUp()">
                        <%--id="ancStartHere" runat="server">--%>
                        <div class="ico">
                            <asp:Image ID="Image2" ImageUrl="~/Resources/Mod/Compliance/icons/help.png" runat="server" />
                        </div>
                        <div class="cont" title="Click to view the details for what is required to reach compliance">
                            <span class="fln">Start Here</span> <span class="ln">Know how to become compliant?</span>
                        </div>
                    </a>
                </div>
            </td>
            <td class="tsep"></td>
            <td>
                <div class="abut">
                    <a id="ancUpload" runat="server">
                        <div class="ico">
                            <asp:Image ImageUrl="~/Resources/Mod/Compliance/icons/docs.png" runat="server" />
                        </div>
                        <div class="cont" title="Click to view, upload, or edit your documents">
                            <span class="fln">Upload Documents</span> <span class="ln">View or upload your documents.</span>
                        </div>
                    </a>
                </div>
            </td>
            <td class="tsep"></td>
            <td>
                <div class="abut">
                    <a href="#" onclick="return false" id="alnkSummary">
                        <div class="ico">
                            <asp:Image ID="Image3" ImageUrl="~/Resources/Mod/Compliance/icons/download.png" runat="server" />
                        </div>
                        <div class="cont" title="Click to view a summary of your immunization record">
                            <span class="fln">Compliance Summary</span> <span class="ln">Save or print your Immunization
                            record.</span>
                        </div>
                    </a>
                </div>
            </td>
            <td class="tsep"></td>
            <td>
                <div class="abut">
                    <a href="#" onclick="return false" id="ancViewSubscription" runat="server">
                        <div class="ico">
                            <asp:Image ID="Image4" ImageUrl="~/Resources/Mod/Compliance/icons/subscribe.png"
                                runat="server" />
                        </div>
                        <div class="cont">
                            <span class="fln">View Subscription</span> <span class="ln"><span class="heavy">
                                <asp:Literal ID="litDaysLeft" runat="server"></asp:Literal>
                            </span></span>
                        </div>
                    </a>
                </div>
            </td>
            <td class="trtl"></td>
        </tr>
    </table>
    <div class="section nobg">
        <div class="content" style="overflow: visible">
            <div id="divApplicantName" runat="server" style="display: none;">
                <span style="font-weight: bold; font-size: 16px; color: #555 !important;">Applicant Name: </span>
                <label id="lblApplicantName" style="font-size: 16px;" runat="server"></label>
            </div>

            <infs:WclTreeList runat="server" ID="tlistComplianceData" AllowPaging="false" DataKeyNames="NodeID"
                ParentDataKeyNames="ParentNodeID" AutoGenerateColumns="false" ShowTreeLines="false"
                OnNeedDataSource="tlistComplianceData_NeedDataSource" ClientDataKeyNames="NodeID"
                OnItemCommand="tlistComplianceData_ItemCommand" Skin="Office2007" AutoSkinMode="true"
                OnItemDataBound="tlistComplianceData_ItemDataBound" OnItemCreated="tlistComplianceData_ItemCreated">
                <ClientSettings Resizing-AllowColumnResize="true">
                </ClientSettings>
                <Columns>
                    <telerik:TreeListTemplateColumn DataField="Category.CategoryName" UniqueName="CategoryName"
                        HeaderText="Compliance Category/Item">
                        <HeaderTemplate>
                            <span>Compliance Category/Item</span> (<asp:LinkButton ID="LinkButton1" runat="server"
                                Text="Expand" CommandName="ExpandAll" CssClass="lnkToggleButton" ToolTip="Click to expand the details for all Categories below" />&nbsp;/&nbsp;<asp:LinkButton
                                    ID="LinkButton2" runat="server" Text="Collapse" CommandName="CollapseAll" CssClass="lnkToggleButton"
                                    ToolTip="Click to collapse the details for all Categories below" />)
                                                    &nbsp;&nbsp;&nbsp;
                             <infs:WclButton ID="btnDownloadTrackingRequirementReport" AutoPostBack="true" Text="Requirement Explanation" runat="server" CssClass="k-button" ForeColor="White"
                                 Skin="Silk" AutoSkinMode="false" OnClick="btnDownloadTrackingRequirementReport_Click">
                             </infs:WclButton>
                            <iframe id="iframe" runat="server" height="3"></iframe>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox runat="server" Style="height: 0px; width: 0px;" ID="TxtCategory"></asp:TextBox>
                            <br />
                            <div id="ToolTipCustom" style="display: none" class="tooltipCustom" onmouseover="ToolTipMouseEnter(this)" onmouseout="ToolTipMouseOut(this)"></div>
                            <asp:Image ID="imgStatus" ImageUrl='<%# Eval("ImageReviewStatus") %>' runat="server"
                                CssClass="img_cat_stat" Visible='<%# ( (Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_REQUIRED_CATEGORY_NODE 
                                                                     || Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_OPTIONAL_CATEGORY_NODE ))   
                                                                      ? true : false %>'
                                ToolTip='<%# Convert.ToString(Eval("ImgReviewStatus")) %>' />

                            <span id="spnHeader" class="spnCatName">
                                <asp:LinkButton ID="lnlSectionText" runat="server" Enabled="false"
                                    Text='<%# Eval("Name").ToString() %>' Visible='<%# Convert.ToString(Eval("ParentNodeID")).Trim() == String.Empty 
                                                                                     ? true : false %>'
                                    CssClass="headerName" />
                            </span>

                            <span id="spnCatName" class="spnCatName" runat="server">
                                <asp:LinkButton ID="LinkButton3" runat="server" Enabled='<%# Eval("IsCategoryDataEntered") %>' onmouseover="ShowToolTip(this);" onmouseout="HideToolTip(this);"
                                    Text='<%# Eval("Name").ToString() %>' Visible='<%# Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_REQUIRED_CATEGORY_NODE  
                                                                                    || Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_OPTIONAL_CATEGORY_NODE  
                                                                ? true : false %>'
                                    CssClass="catname" CommandName="ExpandCollapse" />
                            </span>
                            <span id="litCatDesc" style="display: none" class="litCatDesc">'<%# Eval("ExplanatoryNotes")!=null?Eval("ExplanatoryNotes").ToString():String.Empty %>'</span>
                            <div id="errorMessageBox" class="msgbox" runat="server">
                                <asp:Label ID="lblSeriesErrorMsg" runat="server" CssClass="error" Text="">
                                </asp:Label>
                            </div>
                            <%--  <infs:WclToolTip runat="server" ID="tltpCatDesc" TargetControlID="spnCatName" Width="300px" ClientIDMode="Static" Text='<%# Eval("Category.Description").ToString() %>'
                            Position="TopRight" Visible='<%# Eval("Category.Description").ToString().Trim()==String.Empty ? false : true %>'>
                        </infs:WclToolTip>--%>
                            <div class="item_name">
                                <%--style="width:0px; height:0px;"--%>
                                <asp:TextBox runat="server" Text="" Style="width: 0px; height: 0px;" ID="txtEditTime"></asp:TextBox>
                                <br />
                                <asp:Image ID="Image5" ImageUrl="~/App_Themes/Default/images/icons/itm.gif" runat="server"
                                    Visible='<%# Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_REQUIRED_CATEGORY_NODE  
                                              || Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_OPTIONAL_CATEGORY_NODE 
                                              || Eval("ParentNodeID").ToString().Trim() == String.Empty ? false : true %>' />

                                <asp:Label ID="lblReqName" Text='<%# Eval("Name").ToString() %>' runat="server" CssClass='reqname'
                                    Visible='<%# Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_REQUIRED_CATEGORY_NODE 
                                              || Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_OPTIONAL_CATEGORY_NODE  
                                              ||Eval("ParentNodeID").ToString().Trim() == String.Empty ? false : true %>'
                                    ToolTip="" />

                                <asp:Label ID="lblSubmissionDate" Text='<%# Convert.ToString( Eval("SubmissionDate"))  %>' runat="server" CssClass='reqname subdate'
                                    Visible='<%# Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_REQUIRED_CATEGORY_NODE 
                                              || Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_OPTIONAL_CATEGORY_NODE   
                                              || Eval("ParentNodeID").ToString().Trim()==String.Empty ? false : true %>'
                                    ToolTip="" />

                            </div>
                            <table class="tbl-data" runat="server" id="tblData" visible='<%# (Eval("ParentNodeID").ToString().Trim()==String.Empty 
                                              || Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_REQUIRED_CATEGORY_NODE 
                                              || Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_OPTIONAL_CATEGORY_NODE  ) 
                                                                                    ? false : true %>'>
                                <tr>
                                    <td class="tbl-data-col col-one">
                                        <div title="This section displays the information that has been submitted for this requirement">
                                            <h2 class="col-hdr">Submitted Data
                                            </h2>
                                        </div>
                                        <div class="attr_grp1">
                                            <%#Eval("AttributeHtml") == null ? Eval("AttributeHtmlItem") : Eval("AttributeHtml")%>
                                        </div>
                                        <div class="attr_details">
                                            <%#Eval("AttributeHtmlDescription")%>
                                        </div>
                                        <div class="attr_grp2">
                                            <%#Eval("AttributeHtml") == null?String.Empty:Eval("AttributeHtmlItem")%>
                                        </div>
                                    </td>
                                    <td class="tbl-data-col col-two">
                                        <div title="This section displays any notes submitted by the student for this item">
                                            <h2 class="col-hdr">Submitted Comments
                                            </h2>
                                        </div>
                                        <div class="usernote">
                                            <%#Eval("Notes")%>
                                            <asp:Label ID="lblExceptionComments" Text='<%# Eval("ExceptionReason") %>' runat="server" />
                                        </div>
                                    </td>
                                    <td class="tbl-data-col col-three">
                                        <div title="This section displays any notes submitted by an administrator for this item">
                                            <h2 class="col-hdr">Administrator's Comments
                                            </h2>
                                        </div>
                                        <div class="adm_notes">
                                            <asp:Label ID="lblVerificationCmts" Text='<%# Eval("StatusComments") %>' runat="server"
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
                            <%--Text='<%# Eval("EnterRequirementText") %>'--%>
                            <%--<asp:TextBox runat="server" Text="" ID="txtEditTime" ></asp:TextBox>--%>
                            <div id="Div2" class="cmdbox" runat="server" visible='<%# Eval("ShowAddRequirement") %>'>
                                <asp:LinkButton ID="btnAddReq" CommandArgument='<%# Eval("NodeID") %>'
                                    CssClass="lnkEnterRequirement" OnClick="btnAddReq_Click" OnClientClick="setFormMode(0)" runat="server"
                                    CommandName="InitInsert" Visible='<%# Eval("ShowAddRequirement") %>' ToolTip='<%# Eval("EnterRequirementToolTip") %>'>
                                 <span class="rtlAdd icon_block" >&nbsp;</span><%# Eval("EnterRequirementText") %>
                                </asp:LinkButton>
                            </div>
                            <div id="Div1" class="cmdbox" runat="server" visible='<%# Eval("ShowApplyException") %>'>
                                <asp:LinkButton ID="lnkApplyForException" CommandArgument='<%# Eval("NodeID") %>'
                                    CssClass="lnkEnterRequirement" OnClientClick="setFormMode(0)" runat="server"
                                    CommandName="InitInsert" OnClick="lnkApplyForException_Click" Visible='<%# Eval("ShowApplyException") %>'>
                                 <span class="rtlAdd icon_block" >&nbsp;</span>Apply For Exception
                                </asp:LinkButton>
                            </div>

                            <div id="Div3" runat="server" class="cmdbox" visible='<%# !Convert.ToBoolean(Eval("IsParent")) %>'>
                                <asp:LinkButton ID="btnUpdReq" runat="server" CommandName="Edit" OnClick="btnUpdReq_Click" OnClientClick="setFormMode(0)" ForeColor='<%# (Convert.ToBoolean(Eval("IsItemAboutToExpire"))||Convert.ToBoolean(Eval("IsItemExpired")))?System.Drawing.Color.Red:System.Drawing.Color.Black %>'
                                    CommandArgument='<%# Eval("NodeID") %>' Visible='<%# (Convert.ToBoolean(Eval("ShowItemEditDelete"))  && !Convert.ToBoolean(Eval("IsSeriesItem")))? true : false %> '
                                    ToolTip="Click here to edit the information for this requirement">
                                    <span class="rtlEdit icon_block"></span>
                                    <%# Eval("UpdateButtonText") %>
                                    <asp:HiddenField ID="hdnItemID" runat="server" Value='<%# Eval("NodeID") + "," + Eval("Category.ComplianceCategoryID") %>' />
                                    <%--UAT-5220--%>
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnDeleteItem" CommandArgument='<%# Eval("ApplicantComplianceItemId") %>'
                                    OnClientClick="javascript:return setFormMode(2)" OnClick="lnkbtnDeleteItem_Click" Text="Delete" runat="server"
                                    CommandName="Delete" Visible='<%# Eval("ShowItemDelete") %>' ToolTip="Click here to delete the entered data for this requirement">
                                    <span class="rtlRemove icon_block">&nbsp</span>Delete
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnUpdateException" CommandArgument='<%# Eval("ApplicantComplianceItemId") %>'
                                    OnClientClick="setFormMode(1)" runat="server" CommandName="Edit" Visible='<%# ((Convert.ToBoolean(Eval("IsUpdateButtonNeedForException")) && Convert.ToBoolean(Eval("ShowExceptionEditDelete"))) || Convert.ToBoolean(Eval("ShowExceptionAllTimeUpdate"))) ? true : false %>'
                                    ToolTip="Click here to edit the information for this requirement">
                                     <span class="rtlEdit icon_block"></span>
                                    <%# Eval("UpdateExceptionText").ToString() %>
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnDelete" CommandArgument='<%# Eval("ApplicantComplianceItemId") %>'
                                    OnClientClick="javascript:return setFormMode(3)" Text="Delete" runat="server"
                                    CommandName="Delete" Visible='<%# Eval("ShowExceptionEditDelete") %>' ToolTip="Click here to delete the entered data for this requirement">
                                 <span class="rtlRemove icon_block">&nbsp</span>Delete
                                </asp:LinkButton>
                            </div>
                            <div id="Div4" class="cmdbox" runat="server" visible='<%# Eval("ShowEnterData") %>'>
                                <asp:LinkButton ID="lnkbtnEnterData" CommandArgument='<%# Eval("ParentNodeID") %>' Text="Enter Data"
                                    CssClass="lnkEnterRequirement" OnClientClick="setFormMode(4)" runat="server"
                                    CommandName="InitInsert" Visible='<%# Eval("ShowEnterData") %>' ToolTip="Click to enter data for this item.">
                                 <span class="rtlAdd icon_block" >&nbsp;</span>Enter Data
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </telerik:TreeListTemplateColumn>
                    <telerik:TreeListTemplateColumn DataField="ReviewStatus" UniqueName="ReviewStatus"
                        HeaderText="Status" HeaderTooltip="This column displays the status for each Category and Item for which you have entered data">
                        <HeaderStyle Width="130" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="lblReviewStatus" Text='<%# Eval("ReviewStatus").ToString() %>' runat="server" />
                        </ItemTemplate>
                    </telerik:TreeListTemplateColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <asp:Panel runat="server" ID="pnlExceptionForm">
                            <div class="section">
                                <asp:TextBox runat="server" ID="txtApplyException" Style="height: 0px; width: 0px;" ClientIDMode="Static"></asp:TextBox>
                                <h1 class="mhdr">

                                    <asp:Label ID="lblExceHrdr" Text='<%# !IsEditing(Container) ? "Apply for an Exception: Please note-all exceptions are reviewed by your school on a case to case basis.The turnaround may vary." : "Update Exception: Please note-all exceptions are reviewed by your school on a case to case basis.The turnaround may vary." %>'
                                        runat="server" />
                                </h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblExceptionMsg" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlException">
                                            <div id="divApplyfor" runat="server">
                                                <div class='sxro sx3co'>
                                                    <div class='sxlb'>
                                                        <span class='cptn'>Applying for</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclButton runat="server" Checked="True" ID="rdbCategoryLevel" OnClick="rdbCategoryLevel_Click"
                                                            ToggleType="Radio" Value="1" ButtonType="ToggleButton" GroupName="Group1" Text="Category" />
                                                        <infs:WclButton runat="server" ID="rdbItemLevel" OnClick="rdbItemLevel_Click" ToggleType="Radio"
                                                            Value="0" ButtonType="ToggleButton" GroupName="Group1" Text="Item" />
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:Panel ID="pnlExceptionItems" Visible="False" runat="server" CssClass="sxro sx3co">
                                                <div class='sxlb'>
                                                    <span class='cptn'>Item</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox runat="server" CheckBoxes="True" ID="cmbExceptionItems" EnableItemCaching="false" DataValueField="ComplianceItemID" Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab"
                                                        ChangeTextOnKeyBoardNavigation="false" DataTextField="ItemLabel" />
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvExceptionItems" ControlToValidate="cmbExceptionItems"
                                                            InitialValue="" ValidationGroup="ExceptionValidation" CssClass="errmsg" Display="Dynamic"
                                                            ErrorMessage="Compliance Item is required." />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </asp:Panel>
                                            <%--<div id="pnlSupportingDocs" visible="false" runat="server">--%>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb' title="Click Browse to upload additional documents to be associated with this requirement">
                                                    <span class='cptn'>Exception Supporting Document</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay"
                                                        MultipleFileSelection="Automatic" OnClientFileSelected="clientFileSelected" OnClientValidationFailed="upl_OnClientValidationFailed" OnClientFileUploading="OnClientFileUploading"
                                                        OnClientFileUploaded="OnClientFileUploaded" CssClass="complioFileUpload" OnClientFileUploadRemoved="onFileRemoved"
                                                        AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,JPG,gif,tif,tiff,docx,doc,rtf,pdf,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,JPG,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,TXT">
                                                        <Localization Select="Browse" DropZone="" />
                                                    </infs:WclAsyncUpload>
                                                    <%--                                                AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,gif,tif,tiff,docx,doc,rtf,pdf,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,TXT"
                                                    MaxFileSize="<%#MaxFileSize %>"--%>
                                                </div>
                                                <div id="<%=DropzoneID%>" class="dvApplicantDocumentDropzone sxro sx2co mod-content issue-drop-zoneItemForm drgndrop_border_class ">
                                                    <div class="issue-drop-zone -dui-type-parsed">
                                                        <div class="issue-drop-zone__target"></div>
                                                        <span class="issue-drop-zone__text"><span class="issue-drop-zone__drop-icon"></span>Drop files to attach, or
                                                        <input type="button" class="issue-drop-zone__button" value="Browse" onclick="OpenDialog()" />
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class='cptn'>Uploaded Documents</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox runat="server" CheckBoxes="True" ID="cmbUploadDocument" Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab" EmptyMessage="-- SELECT --" OnClientDropDownClosed="BindDocumentForPreview" />
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <asp:Panel ID="pnlDocumentPreviewForException" runat="server">
                                                <div id="dvDocumentPreview" class='sxro sx3co' style="display: none" runat="server">
                                                    <div class='sxlb' title="Click to View document Preview">
                                                        <span class='cptn'>Preview Documents</span>
                                                    </div>
                                                    <div class='sxlm m2spn'>
                                                        <asp:Panel ID="pnlDocumentPreview" runat="server">
                                                        </asp:Panel>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <div class='sxro monly'>
                                                <div class='sxlb'>
                                                    <span class='cptn'>Reason for Exception (min 10 characters)</span><span class="reqd">*</span>
                                                </div>
                                                <infs:WclTextBox runat="server" ID="txtExceptionComments" TextMode="MultiLine" MaxLength="500"
                                                    Height="50px">
                                                </infs:WclTextBox>
                                                <asp:RequiredFieldValidator ID="RangeValidator1" runat="server" ControlToValidate="txtExceptionComments"
                                                    ValidationGroup="ExceptionValidation" CssClass="errmsg" ErrorMessage="Exception reason is required."></asp:RequiredFieldValidator>
                                                <asp:Label runat="server" CssClass="errmsg" ID="lblExceptionComments"></asp:Label>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <%--</div>--%>
                                        </asp:Panel>
                                    </div>
                                    <asp:HiddenField runat="server" ID="hdnApplicantComplianceItemId" Value="0" />
                                    <asp:HiddenField runat="server" ID="hdnComplianceCategoryId" Value="0" />
                                    <asp:HiddenField runat="server" ID="hdnIsCategoryView" Value="1" />
                                    <asp:HiddenField runat="server" ID="hdnComplianceItemId" Value="0" />
                                    <infsu:CommandBar ID="fsucCmdBar1" runat="server" ButtonPosition="Right" ValidationGroup="ExceptionValidation" GridUpdateText="Save"
                                        TreeListMode="true" DefaultPanel="pnlException" GridInsertText="Submit">
                                    </infsu:CommandBar>
                                </div>
                            </div>
                        </asp:Panel>
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
                                <asp:TextBox runat="server" ID="txtInserted" Style="width: 0px; height: 0px;" ClientIDMode="Static" Text="Inserted Time"></asp:TextBox>
                                <h1 class="mhdr">

                                    <asp:HiddenField runat="server" ID="HdnFields1"></asp:HiddenField>

                                    <asp:Label ID="lblEDTFormHdr" Text='<%# !IsEditing(Container) ? "Add New Requirement" : "Update Requirement" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <!-- Note: Please donot insert anything here. There should be nothing between content and form divs -->
                                    <div class="sxform auto">
                                        <asp:HiddenField ID="hdnItemComplianceStatus" Value='<%# Eval("ReviewStatus").ToString() %>' runat="server"></asp:HiddenField>
                                        <div id="Div4" class="msgbox bullet" runat="server" visible='<%# IsEditing(Container) ? false : true %>'>
                                            <%--'<%# IsEditing(Container) ? (hdnIsExpiredItemSelected.Value=="1")? true: false : true %>'>--%>
                                            <asp:Label ID="lblForm" runat="server" CssClass="info">
                                    <%--<span class="expl-title"><%# Eval("Name").ToString() %></span> <span class="expl-dur">
                                                    One Time.</span> <%# Eval("ExplanatoryNotes")%>--%>
                                            </asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl main_pnl" ID="pnlName1">
                                            <div id="Div5" class='sxro sx3co' runat="server" visible='<%# IsEditing(Container) ? false : true %>'>
                                                <%--'<%# IsEditing(Container)? (hdnIsExpiredItemSelected.Value=="1")? true: false : true %>'> --%>
                                                <div id="divSelectRequirement" runat="server">
                                                    <div class='sxlb' title="Choose the requirement for which you will be entering data">
                                                        <span class='cptn'>Select a requirement</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <%--   <infs:WclDropDownList runat="server" ID="cmbRequirement" AutoPostBack="true" DataValueField="ComplianceItemID"
                                                    DataTextField="ItemLabel">
                                                </infs:WclDropDownList>--%>
                                                        <infs:WclComboBox runat="server" ID="cmbRequirement" AutoPostBack="true" DataValueField="CompItemID" Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab"
                                                            DataTextField="ItemLabel" OnClientSelectedIndexChanging="CheckExpiredItemOnSelection" />
                                                        <infs:WclToolTip runat="server" ID="tltpCatExplanation" TargetControlID="cmbRequirement"
                                                            Width="300px" ManualClose="false" RelativeTo="Element" Position="TopCenter">
                                                            Select an item you wish to add. Read the instruction above to enter the minimum
                                                    number of items required to make you compliant.
                                                        </infs:WclToolTip>
                                                    </div>
                                                </div>
                                                <div class='sxlm m2spn' id="dvApplyException" runat="server" style="padding-left: 100px;">
                                                    <asp:LinkButton ID="btnAddException"  ForeColor="Blue" Text="Apply For Exception" OnClientClick="setFormMode(1)" runat="server" OnClick="btnAddException_Click"
                                                        ToolTip="Apply for exception">                                
                                                    </asp:LinkButton>
                                                </div>
                                                <%--<div class='sxlm' style="padding-left: 50px;">
                                                <asp:LinkButton ID="btnDeleteCatException" Visible="false" Enabled="false" ForeColor="Blue" Text="Delete Category Exception" runat="server" OnClick="btnDeleteCatException_Click"
                                                    ToolTip="Delete Category Exception">                                
                                                </asp:LinkButton>
                                            </div>--%>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <asp:Panel ID="pnlItemInfo" runat="server" Visible="false">
                                                <div class="item_info_bottom">
                                                </div>
                                                <div class="item_info">
                                                    <div id="ItemToolTipCustom" runat="server" style="display: none" class="tooltipCustom"></div>
                                                    <span id="spnItemInfo" runat="server" onmouseover="ShowItemToolTip(this);" onmouseout="HideItemToolTip(this);">Complete the below fields for 
                                                    <asp:Label runat="server" ID="lblItemName" />
                                                    </span>
                                                    <span id="spnItemInfoData" runat="server" visible="false" onmouseover="ShowItemToolTip(this);" onmouseout="HideItemToolTip(this);">fill the form below for
                                                    <asp:Label runat="server" ID="lblDocViewItemName" />
                                                    </span>
                                                    <%--<infs:WclToolTip runat="server" ID="tltpItemDesc" TargetControlID="spnItemInfo" Width="300px"
                                                    ManualClose="false" RelativeTo="Mouse" Position="TopRight">
                                                </infs:WclToolTip>--%>
                                                </div>
                                            </asp:Panel>
                                            <infsu:ItemForm ID="itemForm" runat="server" ReadOnly="true" Visible="true" />
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="cmdBar" runat="server" TreeListMode="true" DefaultPanel="pnlName1" GridUpdateText="Save"
                                        Visible="false" ButtonPosition="Right" GridInsertText="Submit" />
                                    <div style="text-align: right; padding-top: 10px;">
                                        <infs:WclButton ID="btnCancelExpNotes" Text="Cancel" CommandName="Cancel" Icon-PrimaryIconCssClass="rbCancel" Visible="false" runat="server"></infs:WclButton>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <%-- <asp:Panel runat="server" ID="pnlMessage">
                        No Item found in this category.
                    </asp:Panel>--%>
                    </FormTemplate>
                </EditFormSettings>
            </infs:WclTreeList>
        </div>
    </div>
    <infs:WclToolTip runat="server" ID="tltReqHelp" ShowEvent="FromCode" TargetControlID="TOOLTIP100"
        ManualClose="false" IsClientID="true" RelativeTo="Element" AutoCloseDelay="20000"
        VisibleOnPageLoad="false" Position="TopCenter" OffsetY="-10" ShowCallout="true"
        ClientKey="step2tooltip" Animation="Slide">
        To begin, click <span class="heavy">Enter Requirements</span> below
    </infs:WclToolTip>
    <div style="display: none;">
        <infs:WclAsyncUpload runat="server" ID="hiddenuploader" HideFileInput="true" Skin="Hay" OnClientFileUploading="OnClientFileUploading"
            MultipleFileSelection="Automatic" OnClientValidationFailed="upl_OnClientValidationFailed">
            <Localization Select="Browse" />
        </infs:WclAsyncUpload>
    </div>
    <%--  AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,gif,tif,tiff,docx,doc,rtf,pdf,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,TXT"
        MaxFileSize="<%#MaxFileSize %>"--%>
    <asp:HiddenField ID="hidEditForm" runat="server" />
    <asp:HiddenField ID="hdnApplyForException" runat="server" />
    <infs:WclToolTipManager ID="WclToolTipManager1" runat="server" AutoTooltipify="true"
        ShowCallout="false" RelativeTo="Mouse" Position="TopRight" Width="200px" AutoCloseDelay="20000">
    </infs:WclToolTipManager>
    <asp:HiddenField ID="hdfComplianceCategoryId" runat="server" />
    <asp:HiddenField ID="hdfComplianceItemId" runat="server" />
    <asp:HiddenField ID="hdfTenantId" runat="server" />
    <asp:HiddenField ID="hdfPackageId" runat="server" />
    <asp:HiddenField ID="hdfPackageSubscriptionID" runat="server" />
    <asp:HiddenField runat="server" ID="hdnPackageName" />
    <asp:HiddenField runat="server" ID="hdnCategoryName" />
    <asp:HiddenField runat="server" ID="hdnCmbName" />
    <asp:HiddenField runat="server" ID="hdnSignatureMinLengh" Value="" />
    <%-- <asp:HiddenField ID="hdfInvoiceNumber" runat="server"  />
    <asp:HiddenField ID="hdfOrgUserProfileID" runat="server" />--%>
    <%--<asp:HiddenField runat="server" ID="hdnIsExpiredItemSelected" Value="0" />--%>
    <asp:HiddenField ID="hdnNeedToHideDocumentAttributeSection" ClientIDMode="Static" Value="false" runat="server" />
    <asp:HiddenField ID="hdfAppItemDataId" runat="server" />

    <asp:HiddenField ID="hdfDocViewed" runat="server" />
    <asp:HiddenField ID="hdfViewedDocPath" runat="server" />
    <asp:HiddenField ID="hdfDocFileName" runat="server" />
    <asp:HiddenField ID="hdfapplicantDocID" runat="server" />

    <asp:HiddenField ID="hdnItemNotes" runat="server" />
    <asp:HiddenField ID="hdnfIsAutoSubmitTriggerForItem" runat="server" />
    <asp:HiddenField ID="hdnItemComplianceReviewStatus" runat="server" />
    <asp:HiddenField ID="hdnAllowedExtension" runat="server" />
    <asp:HiddenField ID="hdnIsAnyRestrictedFileUploaded" runat="server" Value="false" />
    <asp:HiddenField ID="hdnSelectedNodeIds" runat="server" />
    <asp:HiddenField ID="hdnEnterRequirement" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnApplyException" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdnCompliancePackageDetails" ClientIDMode="Static" runat="server" />

    <div style="display: none">
        <asp:Button ID="btnRefeshPage" runat="server" OnClick="btnRefeshPage_Click"></asp:Button>
    </div>
    <%--UAT-3248--%>
    <div id="nextStepPopUpDiv" class="nextStepPopUp" runat="server" style="overflow-y: auto; max-height: 400px; display: none">
        <asp:Label ID="lblMsg" runat="server"></asp:Label>
    </div>
    <div id="ItemExpiredAlertPopUpDiv" class="ItemExpiredAlertPopUp" runat="server" style="max-height: 400px; display: none">
        <asp:Label ID="lblExpiryItemMsg" runat="server"></asp:Label>
    </div>
    <div id="ExceptionAlertPopUpDiv" class="ItemExpiredAlertPopUp" runat="server" style="max-height: 400px; display: none">
        <asp:Label ID="lblExceptionMsg" runat="server"></asp:Label>
    </div>
    <asp:Button ID="btnAutoSubmit" runat="server" CssClass="buttonHidden" OnClick="btnAutoSubmit_Click" />
</div>

