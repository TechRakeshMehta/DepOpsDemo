<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BkgOrderReviewQueue.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.BkgOrderReviewQueue" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr">Background Order Review Queue</h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel ID="pnlSearch" CssClass="sxpnl" runat="server">
                <div class='sxro sx3co'>
                    <div id="divTenant" runat="server">
                        <div class='sxlb' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class='reqd'>*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" AutoPostBack="true"
                                DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                OnDataBound="ddlTenantName_DataBound" Enabled="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb' title="Select order status types to restrict search results to those order status types">
                        <span class='cptn'>Service Group Review Status</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbSGReviewStatusTypes" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            DataTextField="BSGRS_ReviewStatusType" DataValueField="BSGRS_ID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" EmptyMessage="--Select--">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxlb' title="Select order status types to restrict search results to those order status types">
                        <span class='cptn'>Service Group Status</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbSGStatusTypes" runat="server" DataTextField="BSGS_StatusType" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            DataValueField="BSGS_ID" EmptyMessage="--Select--">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxlb' title="Select a User Group. ">
                        <span class="cptn">Review Criteria</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox runat="server" ID="cmbReviewCriteria" MaxHeight="220" DataTextField="BRC_Name" DataValueField="BRC_ID" EmptyMessage="--Select--"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb' title="Restrict search results to the entered first name">
                        <span class="cptn">Applicant First Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtFirstName" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb' title="Restrict search results to the entered last name">
                        <span class="cptn">Applicant Last Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtLastName" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb' title="Restrict search results to the entered OrderId">
                        <span class='cptn'>Order ID</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtOrderNumber" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb' title="Enter a range of dates to restrict search results to orders placed within that range">
                        <span class='cptn'>Order Created From Date</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclDatePicker ID="dpOdrCrtFrm" runat="server" DateInput-EmptyMessage="Select a date (From)">
                        </infs:WclDatePicker>
                    </div>
                    <div class='sxlb' title="Enter a range of dates to restrict search results to orders placed within that range">
                        <span class='cptn'>Order Created To Date</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclDatePicker ID="dpOdrCrtTo" runat="server" DateInput-EmptyMessage="Select a date (To)"
                            ClientEvents-OnPopupOpening="SetMinDateCrtd">
                        </infs:WclDatePicker>
                    </div>
                    <div class='sxlb' title="">
                        <span class="cptn">Service Group Review Type</span>
                    </div>
                    <div class='sxlm'>
                        <asp:RadioButtonList ID="rblReviewType" runat="server" RepeatDirection="Horizontal"
                            CssClass="radio_list" AutoPostBack="false">
                            <asp:ListItem Text="All" Value="AA"></asp:ListItem>
                            <asp:ListItem Text="Automatic Completed Review" Value="AB" Selected="True"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>

                <div class='sxro sx3co'>

                    <div class='sxlb' title="Enter a range of dates to restrict search results to Service Group Last Updated within that range">
                        <span class='cptn'>Service Group Last Updated From Date</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclDatePicker ID="dpSGUpdtFrm" runat="server" DateInput-EmptyMessage="Select a date (From)">
                        </infs:WclDatePicker>
                    </div>
                    <div class='sxlb' title="Enter a range of dates to restrict search results to Service Group Last Updated within that range">
                        <span class='cptn'>Service Group Last Updated To Date</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclDatePicker ID="dpSGUpdtTo" runat="server" DateInput-EmptyMessage="Select a date (To)"
                            ClientEvents-OnPopupOpening="SetMinDateSGUpdated">
                        </infs:WclDatePicker>
                    </div>
                    <div class='sxlb' title="Select &#34All&#34 to view all subscriptions per the other parameters or &#34Archived&#34 to view only archived subscriptions or &#34Active&#34 to view only non archived subscriptions">
                        <span class="cptn">Subscription Archive State</span>
                    </div>
                    <div class='sxlm'>
                        <asp:RadioButtonList ID="rbSubscriptionState" runat="server" RepeatDirection="Horizontal"
                            DataTextField="As_Name" DataValueField="AS_Code" CssClass="radio_list" AutoPostBack="true">
                        </asp:RadioButtonList>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>

                <div class='sxro sx3co'>
                    <div class='sxlb' title="Click the link and select a node to restrict search results to the selected node">
                        <span class="cptn">Institution Hierarchy</span>
                    </div>
                    <div class='sxlm m2spn'>
                        <a href="#" id="instituteHierarchy" onclick="openPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                     <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>

            </asp:Panel>
        </div>
        <div style="padding-bottom: 5px">
            <infsu:CommandBar ID="CmdBarSearch" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbRefresh" SubmitButtonText="Reset" DefaultPanel="pnlSearch" DefaultPanelButton="Save"
                SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel" ValidationGroup="grpFormSubmit"
                OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click" OnCancelClick="CmdBarCancel_Click">
            </infsu:CommandBar>
        </div>
        <asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
        <asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
        <asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
        <asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
        <asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdBkgOrderReview" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                ShowAllExportButtons="false" ShowExtraButtons="False" AllowCustomPaging="true" ShowClearFiltersButton="false"
                GridLines="Both" OnNeedDataSource="grdBkgOrderReview_NeedDataSource"
                OnItemCommand="grdBkgOrderReview_ItemCommand" OnSortCommand="grdBkgOrderReview_SortCommand" OnItemDataBound="grdBkgOrderReview_ItemDataBound"
                NonExportingColumns="OrderIdTemp">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <ClientEvents OnRowDblClick="grd_rwDbClick" />
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="OrderPackageSvcGrpID,OrderID,OrderNumber,SupplementAutomationStatusID" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true"
                        ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="OrderNumber" FilterControlAltText="Filter OrderID column"
                            HeaderText="OrderID" SortExpression="OrderNumber" UniqueName="OrderID" Display="false"
                            HeaderTooltip="This column displays the total OrderID for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Order ID" ItemStyle-Width="55px" DataField="OrderNumber" UniqueName="OrderIdTemp"
                            FilterControlAltText="Filter OrderId column" SortExpression="OrderID" DataType="System.String" HeaderStyle-Width="5%" HeaderTooltip="This column displays the order number for each record in the grid">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbOrderID" runat="server" Text='<%#Eval("OrderNumber")%>' CommandName="ViewDetail" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="ApplicantFirstName" FilterControlAltText="Filter ApplicantFirstName column"
                            HeaderText="First Name" SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName"
                            HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ApplicantLastName" FilterControlAltText="Filter ApplicantLastName column"
                            HeaderText="Last Name" SortExpression="ApplicantLastName" UniqueName="ApplicantLastName"
                            HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="HierarchyLabel" FilterControlAltText="Filter HierarchyLabel column"
                            AllowFiltering="false" HeaderText="Institution Hierarchy" SortExpression="HierarchyLabel"
                            UniqueName="HierarchyLabel" HeaderTooltip="This column displays the institution hierarchy for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="OrderCreatedDate" FilterControlAltText="Filter OrderCreatedDate column"
                            HeaderText="Order Date" SortExpression="OrderCreatedDate" UniqueName="OrderCreatedDate" DataFormatString="{0:d}">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SvcgrpName" FilterControlAltText="Filter Service Group Name column"
                            HeaderText="Service Group Name" SortExpression="SvcgrpName" UniqueName="SvcgrpName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CustomAttributes" FilterControlAltText="Filter CustomAttributes column"
                            AllowFiltering="false" HeaderText="Custom Attributes" AllowSorting="false"
                            UniqueName="CustomAttributes" HeaderTooltip="This column displays the Custom Attributes for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SvcGrpReviewStatusName" FilterControlAltText="Filter Service Group Review Status column"
                            HeaderText="Service Group Review Status" SortExpression="SvcGrpReviewStatusName" UniqueName="SvcGrpReviewStatusName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SvcGrpStatusName" FilterControlAltText="Filter Service Group Status column"
                            HeaderText="Service Group Status" SortExpression="SvcGrpStatusName" UniqueName="SvcGrpStatusName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SvcGrpLastUpdatedDate" FilterControlAltText="Filter Service Group Last Updated Date column"
                            HeaderText="Service Group Last Updated Date" SortExpression="SvcGrpLastUpdatedDate" UniqueName="SvcGrpLastUpdatedDate" DataFormatString="{0:d}">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsServiceGroupFlagged" FilterControlAltText="Filter Is Service Group Flagged column"
                            HeaderText="Is Service Group Flagged" SortExpression="IsServiceGroupFlagged" UniqueName="IsServiceGroupFlagged">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbGridOrderPkgSvcID" runat="server" Text="View Details" CommandName="ViewDetail" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                    </Columns>
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
<script type="text/javascript">
    var winopen = false;
    var minDate = new Date("01/01/1980");
   
    function openPopUp() {
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "BackgroundScreen";
        //var tenantId = 2;
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);
        var tenantId = $jQuery("[id$=hdnTenantId]").val();
        if (tenantId != "0" && tenantId != "") {
            var DelemittedDeptPrgMapIds = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();
            var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName=" + screenName + "&DelemittedDeptPrgMapIds=" + DelemittedDeptPrgMapIds);
            var win = $window.createPopup(url, { size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
        }
        return false;
    }

    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                $jQuery("[id$=hdnDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
                $jQuery("[id$=hdnHierarchyLabel]").val(arg.HierarchyLabel);
                $jQuery("[id$=hdnInstitutionNodeId]").val(arg.InstitutionNodeId);
                __doPostBack("<%= btnDoPostBack.ClientID %>", "");
            }
            winopen = false;
        }
    }
    function SetMinDateCrtd(picker) {

        var date = $jQuery("[id$=dpOdrCrtFrm]")[0].control.get_selectedDate();
        if (date != null) {
            picker.set_minDate(date);
        }
        else {
            picker.set_minDate(minDate);
        }
    }

    function SetMinDateSGUpdated(picker) {

        var date = $jQuery("[id$=dpSGUpdtFrm]")[0].control.get_selectedDate();
        if (date != null) {
            picker.set_minDate(date);
        }
        else {
            picker.set_minDate(minDate);
        }
    }

    function grd_rwDbClick(s, e) {
        //debugger;
        var _id = "lbOrderID";
        var b = e.get_gridDataItem().findElement(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

</script>
