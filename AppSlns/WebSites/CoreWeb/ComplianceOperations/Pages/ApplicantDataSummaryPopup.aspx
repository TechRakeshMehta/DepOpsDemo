<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApplicantDataSummaryPopup.aspx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ApplicantDataSummaryPopup"
    MasterPageFile="~/Shared/PopupMaster.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
    <style type="text/css">
        .bullet li {
            list-style-position: inside;
            list-style: disc;
            list-style-type: disc;
        }

        .section.mhdr {
            font-size: 80px !important;
        }

        .section {
            margin-bottom: 5px !important;
            margin-top: 5px !important;
        }
    </style>
    <script type="text/javascript">
        function OpenBkgOrderReportWithOrderID(sender) {
            var TenantId = $jQuery("[id$=hdnApplicantTenantId]").val();
            var hdnfOrderID = $jQuery("[id$=hdnfOrderID]").val();
            if (hdnfOrderID != undefined && hdnfOrderID != null && hdnfOrderID != '') {
                var documentType = "ReportDocument";
                var reportType = "OrderCompletion";
                //UAT-2364
                var popupHeight = $jQuery(window).height() * (100 / 100);

                var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID=" + hdnfOrderID + "&DocumentType=" + documentType + "&ReportType=" + reportType + "&tenantId=" + TenantId);
                var win = $window.createPopup(url, {
                    size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize
                    | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal
                },
                    function () {
                        this.set_title("American Databank | Report Detail");
                        this.set_destroyOnClose(true);
                        //this.set_status("");
                    });
                winopen = true;
                return false;
            }
        }

        function SetPopupHeight() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            if (oWindow != undefined && oWindow != null) {
                oWindow.SetHeight(200);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <div class="section" id="dvTrackingRenewSubscription" runat="server" visible="false">
        <h1 class="mhdr">
            <asp:Label ID="lblTrackingRenewSubscription" runat="server" Text="Renew Subscription"></asp:Label>
        </h1>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel ID="pnlTrackingRenewSubscription" CssClass="sxpnl" runat="server">
                    <div></div>
                    <div class='sxro sx2co'>
                        Your subscription is about to expire. 
                      <%--  <asp:LinkButton ID="lnkTrackingRenewSubscription" runat="server" Text="Click here to Renew Existing Tracking Subscription"
                            OnClientClick="return OpenTrackingRenewSubscription(this);"></asp:LinkButton>--%>
                        <asp:LinkButton ID="lnkTrackingRenewSubscription" runat="server" Text="Please click here to renew."
                            OnClick="lnkTrackingRenewSubscription_Click"></asp:LinkButton>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <div id="dvCategoriesParentDiv" runat="server" style="padding-bottom: 5px; margin-bottom: 5px;">
        <div runat="server" class="content">
            <asp:Panel ID="pnlInfoMessage" CssClass="sxpnl" runat="server">
                <div>
                    <div style="font-weight: bold;" id="dvMessage" runat="server">
                        <asp:Label ID="lblMessage" runat="server" CssClass="info"></asp:Label>
                    </div>
                </div>
                <div style="padding-top: 10px;" id="dvCategoriesMain" class="section" runat="server">
                    <h1 class="mhdr">
                        <span style="padding-bottom: 10px; font-weight: bold;">You are still not compliant in the following category(s):</span>
                    </h1>
                    <div class="content">
                        <div class="sxform auto">
                            <asp:Panel ID="pnlCategorySummary" CssClass="sxpnl" runat="server">
                                <div id="divCategoryContainer" runat="server" class="bullet" style="padding-top: 10px; padding-left: 10px; padding-bottom: 10px;">
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div style="padding-top: 10px;" class="section" id="divUpcomingCatExp" runat="server">
                    <h1 class="mhdr">
                        <span style="padding-bottom: 10px; font-weight: bold;">You have following upcoming expiration category(s):</span>
                    </h1>
                    <div class="content">
                        <div class="sxform auto">
                            <asp:Panel ID="pnlUpcomingExp" CssClass="sxpnl" runat="server">
                                <div id="divUpcomingExp" runat="server" class="bullet" style="padding-top: 10px; padding-left: 10px; padding-bottom: 10px;">
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <div class="section" id="dvSvcGroupMain" runat="server" visible="false">
        <h1 class="mhdr">
            <asp:Label ID="lblTitle" runat="server" Text="Service Group Detail"></asp:Label>
        </h1>
        <div class="content">
            <div class="swrap">
                <infs:WclGrid runat="server" ID="grdSvcGroup" AllowPaging="false" AutoGenerateColumns="False" AllowFilteringByColumn="false"
                    AllowSorting="True" AutoSkinMode="True" CellSpacing="0" MasterTableView-AllowSorting="false"
                    EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="false">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        HideStructureColumns="true">
                    </ExportSettings>
                    <MasterTableView CommandItemDisplay="Top">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="OrderNumber" FilterControlAltText="Filter Order Number column"
                                HeaderText="Order Number " SortExpression="OrderNumber" UniqueName="OrderNumber">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ServiceGroupName" FilterControlAltText="Filter Service Group Name column"
                                HeaderText="Service Group Name" SortExpression="ServiceGroupName" UniqueName="ServiceGroupName">
                            </telerik:GridBoundColumn>
                            <%-- <telerik:GridBoundColumn DataField="SvcGrpReviewStatusName" FilterControlAltText="Filter Review Status column"
                                        HeaderText="Review Status" SortExpression="SvcGrpReviewStatusName" UniqueName="SvcGrpReviewStatusName">
                                    </telerik:GridBoundColumn>--%>
                            <%-- <telerik:GridBoundColumn DataField="SvcGrpStatusName" FilterControlAltText="Filter Service Group Status column"
                                        HeaderText="Service Group Status" SortExpression="SvcGrpStatusName" UniqueName="SvcGrpStatusName">
                                    </telerik:GridBoundColumn>--%>
                            <telerik:GridTemplateColumn DataField="SvcGrpStatusName" UniqueName="SvcGrpStatusName"
                                HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSvcGrpStatusName" Text='<%# Eval("SvcGrpStatusName") %>'></asp:Label>
                                    <asp:Image ID="imgStatusServiceGrp" runat="server" ImageUrl='<%# Eval("SvcGrpFlaggedStatusImgPath") %>' Visible="true"
                                        AlternateText='<%# Eval("svcGroupFlaggedStatusAltText") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--<telerik:GridBoundColumn DataField="IsServiceGroupFlagged" FilterControlAltText="Filter Is Service Group Flagged column"
                                        HeaderText="Is Service Group Flagged" SortExpression="IsServiceGroupFlagged" UniqueName="IsServiceGroupFlagged"
                                        DataType="System.Boolean">
                                    </telerik:GridBoundColumn>--%>
                        </Columns>
                        <EditFormSettings EditFormType="Template">
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </infs:WclGrid>
            </div>
            <div class="gclr">
            </div>
        </div>
    </div>

    <div class="section" id="dvBkgOrderReport" runat="server" visible="false">
        <h1 class="mhdr">
            <asp:Label ID="lblBkgOrderReport" runat="server" Text="Background Order Report"></asp:Label>
        </h1>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel ID="Panel3" CssClass="sxpnl" runat="server">
                    <div></div>
                    <div class='sxro sx2co'>
                        <asp:LinkButton ID="lbtnOrderCompletionReport" runat="server" Text="Background report for Order# {0}"
                            OnClientClick="return OpenBkgOrderReportWithOrderID(this);"></asp:LinkButton>
                        <asp:HiddenField ID="hdnfOrderID" runat="server" />
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <div class="section" id="dvApprovedRotations" runat="server" visible="false">
        <h1 class="mhdr">
            <asp:Label ID="Label1" runat="server" Text="Approved Rotation Requirements"></asp:Label>
        </h1>
        <div class="content">
            <div class="swrap">
                <infs:WclGrid runat="server" ID="grdApprovedRotations" AllowPaging="false" AutoGenerateColumns="False" AllowFilteringByColumn="false"
                    AllowSorting="True" AutoSkinMode="True" CellSpacing="0" MasterTableView-AllowSorting="false"
                    EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="false">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        HideStructureColumns="true">
                    </ExportSettings>
                    <MasterTableView CommandItemDisplay="Top">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="AgencyName" HeaderText="Agency Name" SortExpression="AgencyName" UniqueName="AgencyName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Department" HeaderText="Department" SortExpression="Department" UniqueName="Department">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Program" HeaderText="Program" SortExpression="Program" UniqueName="Program">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Course" HeaderText="Course" SortExpression="Course" UniqueName="Course">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <EditFormSettings EditFormType="Template">
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </infs:WclGrid>
            </div>
            <div class="gclr">
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnApplicantTenantId" runat="server" />
</asp:Content>
