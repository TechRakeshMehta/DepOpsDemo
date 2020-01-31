<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharedUserRotationRequirementDataEntry.ascx.cs" Inherits="CoreWeb.ApplicantRotationRequirement.Views.SharedUserRotationRequirementDataEntry" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ReqItemForm" Src="~/ApplicantRotationRequirement/UserControl/SharedUserRequirementItemForm.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/RotationDetails.ascx" TagName="RotationDetails" TagPrefix="uc" %>
<style>
    .RadComboBox_Silk .rcbInputCell {
        background-color: none !important;
    }

    .attr_grp1 table td {
        font-size: 13px;
    }

    label {
        font-size: 13px !important;
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

    .collapsed {
        height: 40px !important;
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
    function ValidateSignature(sender, args) {
        var reqItemFieldId = sender.getAttribute("ReqItemFieldId");
        var signatureLength = $jQuery('[id$=hdnSignatureMinLengh]');// sender.getAttribute("SignatureMinLengh");

        var hdfSignature = $jQuery('[id$="' + "hiddenOutput_" + reqItemFieldId + '"]')[0];

        if ($jQuery(hdfSignature).length > 0 && $jQuery(hdfSignature).val().length > 0) {
            args.IsValid = true;

            if (signatureLength.length > 0) {
                var MinSignLengthValue = parseInt(signatureLength[0].value);
                var ActualSignLengthValue = parseInt($jQuery(hdfSignature).val().length);
                if (MinSignLengthValue > ActualSignLengthValue) {
                    args.IsValid = false;
                    sender.innerHTML = "Provided text does not qualify as valid Signature. Please provide valid Signature.";
                }
            }
        }
        else {
            args.IsValid = false;
            sender.innerHTML = "Signature is Required.";
        }
    }
    function CheckMinLengthSignature(sender, args) {
        var reqItemFieldId = sender.getAttribute("ReqItemFieldId");
        var signatureLength = $jQuery('[id$=hdnSignatureMinLengh]');// sender.getAttribute("SignatureMinLengh");

        var hdfSignature = $jQuery('[id$="' + "hiddenOutput_" + reqItemFieldId + '"]')[0];

        if ($jQuery(hdfSignature).length > 0 && $jQuery(hdfSignature).val().length > 0) {
            args.IsValid = true;

            if (signatureLength.length > 0) {
                var MinSignLengthValue = parseInt(signatureLength[0].value);
                var ActualSignLengthValue = parseInt($jQuery(hdfSignature).val().length);
                if (MinSignLengthValue > ActualSignLengthValue) {
                    args.IsValid = false;
                    sender.innerHTML = "Provided text does not qualify as valid Signature. Please provide valid Signature.";
                }
            }
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
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/ApplicantRotationRequirement/main.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/ApplicantRotationRequirement/SharedUserRequirementDataEntry.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div onclick="setPostBackSourceDE(event, this);" style="overflow-y: visible;">
    <asp:HiddenField ID="hdnShowHide" runat="server" Value="true" />
    <asp:TextBox ID="hdnPostbacksource" class="postbacksource" runat="server" Style="display: none;" />
    <%--<uc:RotationDetails id="ucRotationDetails" runat="server"></uc:RotationDetails>--%>
    <div class="container-fluid">
        <div class="col-md-12">
            <div id="dvSection" runat="server">
                <asp:Literal ID="litRotationDetails" runat="server"></asp:Literal>
                <div id="dvContent" class="content" runat="server">
                    <asp:Panel ID="pnlRotationDetails" runat="server"></asp:Panel>
                </div>
            </div>
        </div>
    </div>


    <div class="container-fluid">
        <div class="col-md-12">
            <div class="row">
                <div class="row">&nbsp;</div>
                <div class="form-group col-md-12 text-right">
                    <div class="row">
                        <div runat="server" id="dvComplianceStatus">
                            <span class="blacktext">Rotation Compliance Status&nbsp;<span class="redalret"><asp:Label ID="lblComplianceStatus"
                                runat="server" Text=""></asp:Label></span>&nbsp;
                <asp:Image ID="imgPackageComplianceStatus" runat="server" />&nbsp;&nbsp;<asp:Label ID="lblComplianceCategoryStatus"
                    runat="server" Visible="false"></asp:Label></span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">&nbsp;</div>
            <div class="row">
                <div class="col-md-12" id="gridBorder">
                    <div class="row">
                        <%--<div class="content" style="overflow: visible;">--%>
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
                                                Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Name"))) %>' Visible='<%# Convert.ToString(Eval("ParentNodeID")).Trim() == String.Empty 
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
                                        <table class="tbl-data" runat="server" id="tblData" visible='<%# Eval("ParentNodeID") == INTSOF.Utils.AppConsts.DATA_ENTRY_REQUIRED_REQUIREMENT_CATEGORY_NODE  
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
                                                        <asp:Label ID="lblRejectionReason" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ItemRejectionReason"))) %>' runat="server"
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
                                        <div id="dvUpdateRequirement" runat="server" class="cmdbox" visible='<%# !Convert.ToBoolean(Eval("IsParent")) %>'>
                                            <asp:LinkButton ID="btnUpdReq" Text="Update" runat="server" CommandName="Edit" OnClientClick="setFormMode(1)"
                                                CommandArgument='<%# Eval("NodeID") %>' Visible='<%# Eval("ShowItemEditDelete") %>'
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
                                        <asp:Label ID="lblReviewStatus" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString( Eval("ReviewStatus"))) %>' runat="server" />
                                    </ItemTemplate>
                                </telerik:TreeListTemplateColumn>
                            </Columns>
                            <EditFormSettings EditFormType="Template">
                                <FormTemplate>
                                    <asp:Panel runat="server" ID="pnlEntryForm">
                                        <div class="container-fluid">
                                            <div id="dvExplanatoryNotesItem" runat="server">
                                                <asp:Repeater ID="rptExplanatoryNotes" runat="server">
                                                    <ItemTemplate>
                                                        <div class="row">
                                                            <div class="col-md-12">
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
                                            <div class="row" id="dvAddNewRequirement" runat="server">
                                                <div class="col-md-12">
                                                    <div class="row">
                                                        <h2 class="header-color">
                                                            <asp:Label ID="lblEDTFormHdr" Text='<%# !IsEditing(Container) ? "Add New Requirement" : "Update Requirement" %>'
                                                                runat="server" />
                                                        </h2>
                                                    </div>
                                                </div>

                                                <!-- Note: Please donot insert anything here. There should be nothing between content and form divs -->
                                                <div class="col-md-12">
                                                    <div class="row">
                                                        <div id="dvBox" class="msgbox1 bullet" runat="server" visible='<%# IsEditing(Container) ? false : true %>'>
                                                            <asp:Label ID="lblForm" runat="server" CssClass="info">
                                                            </asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <asp:Panel runat="server" CssClass="sxpnl main_pnl" ID="pnlName1">
                                                    <div id="Div5" class='col-md-12' runat="server" visible='<%# IsEditing(Container) ? false : true %>'>
                                                        <div id="divSelectRequirement" runat="server" class="row">
                                                            <div class='form-group col-md-3' title="Choose the requirement for which you will be entering data">
                                                                <span class='cptn'>Select a requirement</span>
                                                                <infs:WclComboBox runat="server" ID="cmbRequirement" AutoPostBack="true" DataValueField="RequirementItemID" Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab"
                                                                    DataTextField="RequirementItemName" Skin="Silk" AutoSkinMode="false" Width="100%" CssClass="form-control removebg" />
                                                                <%--<infs:WclToolTip runat="server" ID="tltpCatExplanation" TargetControlID="cmbRequirement"
                                                            Width="300px" ManualClose="false" RelativeTo="Element" Position="TopCenter">
                                                            Select an item you wish to add. Read the instruction above to enter the minimum
                                                    number of items required to make you compliant.
                                                        </infs:WclToolTip>--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <%-- <asp:Panel ID="pnlItemInfo" runat="server" Visible="false">
                                                        <div class="row">
                                                        </div>
                                                        <div class="col-md-12">
                                                             <div id="ItemToolTipCustom" runat="server" style="display: none" class="tooltipCustom"></div> onmouseover="ShowItemToolTip(this);" onmouseout="HideItemToolTip(this);"
                                                            <span id="spnItemInfo" runat="server">fill the form below for
                                                    <asp:Label runat="server" ID="lblItemName" />
                                                            </span>
                                                        </div>
                                                        <div class="row">
                                                        </div>
                                                    </asp:Panel>--%>
                                                    <infsu:ReqItemForm ID="itemForm" runat="server" ReadOnly="true" Visible="true" />
                                                </asp:Panel>

                                                <infsu:CommandBar ID="cmdBar" runat="server" TreeListMode="true" DefaultPanel="pnlName1" GridUpdateText="Save" ClientIDMode="Static"
                                                    Visible="false" ButtonPosition="Right" GridInsertText="Submit" UseAutoSkinMode="false" ButtonSkin="Silk" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </FormTemplate>
                            </EditFormSettings>
                        </infs:WclTreeList>
                        <%--</div>--%>
                        <div class="row">&nbsp;</div>
                        <div class="col-md-12" id="trailingText">
                            <infsu:CommandBar ID="fsucCmdBar" runat="server" Visible="false" ButtonPosition="Center" DisplayButtons="Submit" SubmitButtonIconClass="rbReturn" ButtonSkin="Silk" UseAutoSkinMode="false"
                                SubmitButtonText="Return To Manage Invitations" AutoPostbackButtons="Submit" OnSubmitClick="fsucCmdBar_SubmitClick">
                            </infsu:CommandBar>
                        </div>
                        <div class="row">&nbsp;</div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hidEditForm" runat="server" />
            <asp:HiddenField ID="hdRequirementCategoryId" runat="server" />
            <asp:HiddenField ID="hdRequirementPackageId" runat="server" />
            <asp:HiddenField ID="hdfRequirementItemId" runat="server" />
            <asp:HiddenField ID="hdfTenantId" runat="server" />
            <asp:HiddenField ID="hdfOrganizationUserId" runat="server" />
            <asp:HiddenField ID="hdfRequirementPkgSubscriptionID" runat="server" />
            <asp:HiddenField ID="hdnClinicalRotationID" Value="0" runat="server" />
            <!-- Document and video type fields value -->
            <%--<asp:HiddenField ID="hdfIsViewVideoRequired" runat="server" Value="0" />
    <asp:HiddenField ID="hdfIsViewDocumentRequired" runat="server" Value="0"/>
    <asp:HiddenField ID="hdfIsVideoViewed" runat="server"  Value="0"/>
    <asp:HiddenField ID="hdfIsDocumentViewed" runat="server" Value="0"/>
    <asp:HiddenField ID="hdfVideoViewedTime" runat="server" Value="0" />--%>

            <asp:HiddenField runat="server" ID="hdnCategoryName" />
            <asp:HiddenField runat="server" ID="hdnCmbName" />
            <asp:HiddenField runat="server" ID="hdnSignatureMinLengh" Value="" />

            <asp:HiddenField ID="hdfIsReqDocViewed" runat="server" />
            <asp:HiddenField ID="hdfReqViewedDocPath" runat="server" />
            <asp:HiddenField ID="hdfReqDocFileName" runat="server" />
            <asp:HiddenField ID="hdnfReqIsAutoSubmitTriggerForItem" runat="server" />
            <asp:HiddenField ID="hdfReqItemDataId" runat="server" />

        </div>
    </div>
</div>
<script src="../Resources/Mod/Dashboard/Scripts/bootstrap.min.js" type="text/javascript"></script>

<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
<asp:Button ID="btnAutoSubmit" runat="server" CssClass="buttonHidden" OnClick="btnAutoSubmit_Click" />