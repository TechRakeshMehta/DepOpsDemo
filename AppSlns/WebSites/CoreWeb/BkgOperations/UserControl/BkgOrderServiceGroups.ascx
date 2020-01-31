<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BkgOrderServiceGroups.ascx.cs"
    Inherits="CoreWeb.BkgOperations.Views.BkgOrderServiceGroups" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxManageInvitationExpiration">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style>
    .hlink
    {
        cursor: pointer;
    }
</style>
<script type="text/javascript">

    var winopen = false;

    function openReportWithServiceGroupID(sender) {
      
        var btnID = sender.id;
        //UAT-1923
        window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "-1");
        $jQuery("[id$=hdnCurrentClicked]").val(btnID);
        //var containerID = btnID.substr(0, btnID.indexOf("btnNotificationPdf"));
        var TenantId = $jQuery("#<%= hfTenantId.ClientID %>").val()
        var hfOrderID = $jQuery("#<%= hfOrderID.ClientID %>").val();
        var containerID = btnID.substr(0, btnID.indexOf("hlPackageGroupDocument"));
        var hdnfServiceGroupID = $jQuery("[id$=" + containerID + "hdnfServiceGroupID]").val();
        var hdnfBkgPkgSvcGrpID = $jQuery("[id$=" + containerID + "hdnfBkgPkgSvcGrpID]").val();
        var isAdmin = $jQuery("[id$=hdnIsAdmin]").val();
        if ((isAdmin == "true") || ($jQuery("[id$=hdnIsEdsAccepted]").val() == "true") || ($jQuery("[id$=hdn_IsEmploymentType]").val() == "False")) {
            var documentType = "ReportDocument";
            var reportType = "OrderCompletion";
            var composeScreenWindowName = "Filterd Report Detail";
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID=" + hfOrderID + "&DocumentType=" + documentType + "&ServiceGroupID=" + hdnfServiceGroupID + "&ReportType=" + reportType + "&tenantId=" + TenantId + "&BkgPkgSvcGrpID=" + hdnfBkgPkgSvcGrpID);
            var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
            winopen = true;
            return false;
        }
        else {
            var documentType = $jQuery("[id$=hdnEmployementDiscTypeCode]").val();
            var orgUsrId = $jQuery("[id$=hdnOrgUsrId]").val();
            var popupType = "servicegroup";
            OpenEmployerDisclosureDocument(TenantId, orgUsrId, documentType, hfOrderID, popupType, hdnfServiceGroupID, hdnfBkgPkgSvcGrpID)
        }
    }

    function openReportWithOrderID(sender) {
        var btnID = sender.id;
        //UAT-1923
        window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "-1");
        $jQuery("[id$=hdnCurrentClicked]").val(btnID);

        var containerID = btnID.substr(0, btnID.indexOf("imgPDF"));
        var TenantId = $jQuery("#<%= hfTenantId.ClientID %>").val()
        var hfOrderID = $jQuery("#<%= hfOrderID.ClientID %>").val();

           var documentType = $jQuery("[id$=hdnEmployementDiscTypeCode]").val();
           var orgUsrId = $jQuery("[id$=hdnOrgUsrId]").val();
           var isAdmin = $jQuery("[id$=hdnIsAdmin]").val();
           if (isAdmin == "true" || $jQuery("[id$=hdnIsEdsAccepted]").val() == "true") {
               var documentType = "ReportDocument";
               var reportType = "OrderCompletion";
               var composeScreenWindowName = "Report Detail";
               //UAT-2364
               var popupHeight = $jQuery(window).height() * (100 / 100);

               var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID=" + hfOrderID + "&DocumentType=" + documentType + "&ReportType=" + reportType + "&tenantId=" + TenantId);
               var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
               winopen = true;
               return false;
           }
           else {

               var popupType = "report";
               OpenEmployerDisclosureDocument(TenantId, orgUsrId, documentType, hfOrderID, popupType)
           }
       }



       function OnClientClose(oWnd, args) {
           //debugger;
           oWnd.get_contentFrame().src = ''; //This is added for fixing pop-up close issue in Safari browser.
           oWnd.remove_close(OnClientClose);
           if (winopen) {
               winopen = false;
           }
           //UAT-1923
           window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "0");
           var currentLinkFocus = $jQuery("[id$=hdnCurrentClicked]").val();
           if (currentLinkFocus != undefined && currentLinkFocus != null && currentLinkFocus != "") {
               setTimeout(function () { $jQuery("[id$=" + currentLinkFocus + "]").focus(); }, 1500);
               $jQuery("[id$=hdnCurrentClicked]").val("");
           }
       }


       function OpenEmployerDisclosureDocument(tenantId, orgUsrId, documentType, orderID, popupType, serviceGroupID,BkgPkgSvcGrpID) {
           var popupWindowName = "Employment Disclosure";
           //UAT-2364
           var popupHeight = $jQuery(window).height() * (100 / 100);

           var url = $page.url.create("~/ComplianceOperations/Pages/ReportEmploymentDisclosure.aspx?DocumentTypeCode=" + documentType + "&TenantID=" + tenantId + "&OrganizationUserID=" + orgUsrId + "&OrderId=" + orderID + "&PopupType=" + popupType + "&ServiceGroupID=" + serviceGroupID + "&BkgPkgSvcGrpID=" + BkgPkgSvcGrpID);
           var win = $window.createPopup(url, { size: "900, " + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close, onclose: Close },
                                               function () {
                                                   this.set_title(popupWindowName);
                                               });
           return false;
       }

       function Close(oWnd, args) {
           //debugger;
           oWnd.remove_close(Close);
           var arg = args.get_argument();
           if (arg) {
               if (arg.Action == 'success') {
                   if ($jQuery("[id$=hdnIsEdsAccepted]")) {
                       $jQuery("[id$=hdnIsEdsAccepted]").val("true");
                   }
                   //UAT-2364
                   var popupHeight = $jQuery(window).height() * (100 / 100);

                   if (arg.PopupType == "report") {
                       var documentType = "ReportDocument";
                       var reportType = "OrderCompletion";
                       var composeScreenWindowName = "Report Detail";
                       var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID=" + arg.OrderId + "&DocumentType=" + documentType + "&ReportType=" + reportType + "&tenantId=" + arg.TenantId);
                       var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
                       winopen = true;
                   }
                   else if (arg.PopupType == "servicegroup") {
                       var documentType = "ReportDocument";
                       var reportType = "OrderCompletion";
                       var composeScreenWindowName = "Filterd Report Detail";
                       var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID=" + arg.OrderId + "&DocumentType=" + documentType + "&ServiceGroupID=" + arg.ServiceGroupID + "&ReportType=" + reportType + "&tenantId=" + arg.TenantId + "&BkgPkgSvcGrpID=" + arg.BkgPkgSvcGrpID);
                       var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
                       winopen = true;
                       return false;
                   }
               }
               else {
                   //UAT-1923
                   window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "0");
                   var currentLinkFocus = $jQuery("[id$=hdnCurrentClicked]").val();
                   if (currentLinkFocus != undefined && currentLinkFocus != null && currentLinkFocus != "") {
                       setTimeout(function () { $jQuery("[id$=" + currentLinkFocus + "]").focus(); }, 1000);
                       $jQuery("[id$=hdnCurrentClicked]").val("");
                   }
               }
               return false;
           }
           //UAT-1923
           window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "0");
           var currentLinkFocus = $jQuery("[id$=hdnCurrentClicked]").val();
           if (currentLinkFocus != undefined && currentLinkFocus != null && currentLinkFocus != "") {
               setTimeout(function () { $jQuery("[id$=" + currentLinkFocus + "]").focus(); }, 500);
               $jQuery("[id$=hdnCurrentClicked]").val("");
           }
       }

</script>

<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color" tabindex="0">Service Groups
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <div id="divSendMail" runat="server" style="display: none">
            <asp:Panel runat="server" ID="pnlSendMail">
                <div class='form-group col-md-3'>
                    <span class='cptn'>Order Status</span>
                </div>
                <div class='form-group col-md-3'>
                    <asp:Image ID="imgOrderStatus" runat="server" Visible="true" ImageUrl="../../images/medium/pdf.gif" />
                    <asp:HyperLink ID="hlOrderDocument" runat="server" Enabled="true"
                        Visible="true" Target="_blank" onclick="openReportWithOrderID(this)" CssClass="hlink">
                        <asp:Image ID="imgOrderPDF" runat="server"
                            AlternateText='<%# String.Concat("Click here to view Order result PDF for Order number ",Convert.ToString((Eval("OrderNumber")))) %>' Visible="true" />
                    </asp:HyperLink>
                </div>
                <div class='form-group col-md-3'>
                    <infs:WclButton ID="btnSendToClient" runat="server" Text="Send Result To Client"
                        AutoPostBack="true" OnClick="btnSendReport_Click" AutoSkinMode="false" Skin="Silk" />
                </div>
                <div class='form-group col-md-3'>
                    <infs:WclButton ID="btnSendToStudent" runat="server" Text="Send Result To Student"
                        AutoPostBack="true" OnClick="btnSendReport_Click" AutoSkinMode="false" Skin="Silk" />
                </div>
            </asp:Panel>
        </div>
    </div>

    <div class="row">
        <infs:WclGrid runat="server" ID="grdServiceGrp" AllowPaging="false" CssClass="removeExtraSpace"
            PageSize="10" AutoGenerateColumns="False" AllowSorting="false" GridLines="Both"
            ShowAllExportButtons="False" AllowFilteringByColumn="false" EnableAriaSupport="true"
            OnNeedDataSource="grdServiceGrp_NeedDataSource" OnItemDataBound="grdServiceGrp_ItemDataBound"
            ShowClearFiltersButton="false" EnableDefaultFeatures="false" OnItemCommand="grdServiceGrp_ItemCommand">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ServiceGroupID,PackageServiceGroupID,ServiceGroupName"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                    ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"
                    ShowExportToWordButton="false"></CommandItemSettings>
                <Columns>
                    <telerik:GridTemplateColumn DataField="ServiceGroupName" UniqueName="ServiceGroupName"
                        HeaderText="Service Group Name">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlPackageGroupDocument" runat="server" onclick="openReportWithServiceGroupID(this);"
                                Visible="true" Target="_blank" CssClass="hlink">
                                <asp:Image ID="imgServiceGroupPDF" runat="server" ImageUrl='<%# ImagePath + "/pdf.gif" %>'
                                     AlternateText='<%# String.Concat("Click here to view ", INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ServiceGroupName")) )," service group result PDF") %>' Visible="true" />
                            </asp:HyperLink>
                            <asp:Image ID="imgStatusServiceGrp" runat="server" ImageUrl='<%# ImagePath + "/blank.gif" %>'
                                Visible="true" />
                            <asp:HiddenField ID="hdn_IsEmploymentType" runat="server" Value='<%#Eval("IsEmployment") %>'/>
                            <asp:Label runat="server" ID="lblServiceGroupName" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ServiceGroupName")) )%>'></asp:Label>
                            <asp:HiddenField ID="hdnfServiceGroupID" runat="server" Value='<%#Eval("ServiceGroupID") %>' />
                            <asp:HiddenField ID="hdnfBkgPkgSvcGrpID" runat="server" Value ='<%#Eval("PackageServiceGroupID") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="SvcGrpReviewStatusType"
                        HeaderText="Review Status" UniqueName="SvcGrpReviewStatusType">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SvcGrpStatusType"
                        HeaderText="Status" UniqueName="SvcGrpStatusType">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn UniqueName="SendReportToClient">
                        <ItemTemplate>
                            <infs:WclButton ID="btnSendSvcGrpRepToClient" runat="server" Text="Send Result To Client"
                                AutoPostBack="true" AutoSkinMode="false" Skin="Silk" CommandName="SendReportToClient" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="SendReportToStudent">
                        <ItemTemplate>
                            <infs:WclButton ID="btnSendSvcGrpRepToStudent" runat="server" Text="Send Result To Student"
                                AutoPostBack="true" AutoSkinMode="false" Skin="Silk" CommandName="SendReportToStudent" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <NestedViewTemplate>
                    <div class="swrap">
                        <infs:WclGrid runat="server" ID="grdLineItems" AllowPaging="false"
                            PageSize="10" AutoGenerateColumns="False" AllowSorting="false" GridLines="Both"
                            ShowClearFiltersButton="false" EnableAriaSupport="true"
                            OnNeedDataSource="grdLineItems_NeedDataSource" OnItemDataBound="grdLineItems_ItemDataBound"
                            ShowAllExportButtons="false" AllowFilteringByColumn="false" PagerStyle-ShowPagerText="false"
                            EnableDefaultFeatures="false" OnItemCommand="grdLineItems_ItemCommand">
                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="LineItemID,ServiceGroupID,HierarchyNodeID"
                                AllowFilteringByColumn="false">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                                    ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                                <Columns>
                                    <telerik:GridTemplateColumn DataField="ServiceName"
                                        HeaderText="Line Item Name" UniqueName="ServiceName">
                                        <ItemTemplate>
                                            <asp:Image ID="imgStatus" runat="server" ImageUrl='<%# ImagePath + "/blank.gif" %>'
                                                Visible="true" />
                                            <asp:Label runat="server" ID="lblLineItemName" Text='<%# Eval("ServiceName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="LineDescription"
                                        HeaderText="Description" UniqueName="LineDescription">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="VendorStatus"
                                        HeaderText="Vendor Status" UniqueName="VendorStatus">
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <NestedViewTemplate>
                                    <div class="swrap">
                                        <infs:WclGrid runat="server" ID="grdCustomText" AllowPaging="false"
                                            PageSize="10" AutoGenerateColumns="False" AllowSorting="false" GridLines="Both"
                                            ShowClearFiltersButton="false" EnableAriaSupport="true"
                                            OnNeedDataSource="grdCustomText_NeedDataSource" OnItemDataBound="grdCustomText_ItemDataBound"
                                            ShowAllExportButtons="false" AllowFilteringByColumn="false" PagerStyle-ShowPagerText="false"
                                            EnableDefaultFeatures="false">
                                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ServiceID"
                                                AllowFilteringByColumn="false">
                                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                                                    ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="Label"
                                                        HeaderText="Field" UniqueName="Label">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="CustomValue"
                                                        HeaderText="Value" UniqueName="CustomValue">
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </infs:WclGrid>
                                    </div>
                                </NestedViewTemplate>
                            </MasterTableView>
                        </infs:WclGrid>
                    </div>
                </NestedViewTemplate>

            </MasterTableView>
        </infs:WclGrid>
    </div>

</div>
<asp:HiddenField ID="hfTenantId" runat="server" />
<asp:HiddenField ID="hfOrderID" runat="server" />
<asp:HiddenField ID="hdnIsEdsAccepted" runat="server" Value="" />
<asp:HiddenField ID="hdnEmployementDiscTypeCode" runat="server" />
<asp:HiddenField ID="hdnOrgUsrId" runat="server" />
<asp:HiddenField ID="hdnIsAdmin" runat="server" />
<asp:HiddenField ID="hdnCurrentClicked" runat="server" Value=""/>

<script type="text/javascript">
    function pageLoad() {

       
        $jQuery("[id$=hlPackageGroupDocument]").attr("tabindex", "0");
        //$jQuery("[id$=hlPackageGroupDocument]").attr("onkeypress", "openReportWithServiceGroupID(this)");
        $jQuery('[id$="hlPackageGroupDocument"]').keyup(function (e) { if (e.keyCode == 13) { openReportWithServiceGroupID(this); } });
    }
</script>
