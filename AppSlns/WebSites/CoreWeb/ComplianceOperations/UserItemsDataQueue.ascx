<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.UserItemsDataQueue" CodeBehind="UserItemsDataQueue.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%--<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearch.ascx" TagName="CustomAttributeLoaderSearch" TagPrefix="uc1" %>--%>
<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearchMultipleNodes.ascx"
    TagName="CustomAttributeLoaderNodeSearch" TagPrefix="uc" %>


<infs:WclResourceManagerProxy runat="server" ID="rprxUserItemsDataQueue">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    
<infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
       var column = null;
        var columnName = null;
        function MenuShowing(sender, args) {
            
            if (column == null) return;
            var menu = sender; var items = menu.get_items();
            
            //column.get_uniqueName() == "uniqueName"
            if (columnName == 'PackageName' || columnName == 'CategoryName' || columnName == 'ItemName' 
            ||  columnName == 'AssignedUserName')
            {
              
               var j = 0;
                while (j < items.get_count()) {
                    if (!(items.getItem(j).get_value() in { 'NoFilter': '', 'Contains': '','DoesNotContain':'','IsEmpty':'' })) {
                        var item = items.getItem(j);
                        if (item != null)
                            item.set_visible(false);
                    }  else {
                        var item = items.getItem(j);
                        if (item != null)
                            item.set_visible(true);
                    } j++;
                }
            }
            else if (columnName == 'ReviewLevel')
            {
               
                var K = 0;
                while (K < items.get_count()) {
                    debugger;
                    if (!(items.getItem(K).get_value() in { 'NoFilter': '', 'EqualTo': '' })) {

                        var item = items.getItem(K);
                        if (item != null)
                            item.set_visible(false);
                    } else {
                        var item = items.getItem(K);
                        if (item != null)
                            item.set_visible(true);
                    } K++;
                }
            }
           else if (column.get_dataType() == "System.String" && columnName != 'ReviewLevel') {
                var i = 0;
                while (i < items.get_count()) {
                    if (!(items.getItem(i).get_value() in { 'NoFilter': '', 'Contains': '','IsEmpty':'' })) {
                        var item = items.getItem(i);
                        if (item != null)
                            item.set_visible(false);
                    }
                    else {
                        var item = items.getItem(i);
                        if (item != null)
                            item.set_visible(true);
                    } i++;
                }
            }
            
            column = null;
            columnName = null;
            menu.repaint();
        }
        function filterMenuShowing(sender, eventArgs) {
            
            column = eventArgs.get_column();
            columnName = eventArgs.get_column().get_uniqueName();
        }
    </script>
</telerik:RadCodeBlock>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <asp:Label ID="lblVerificationQueue" runat="server" Text=""></asp:Label>
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel runat="server" ID="pnlShowFilters">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class='cptn'>Institution</span>
                            <%--<infs:WclDropDownList ID="ddlTenantName" runat="server" AutoPostBack="true" OnItemSelected="ddlTenantName_ItemSelected"  Enabled="false"
                                DataTextField="TenantName" DataValueField="TenantID" OnDataBound="ddlTenantName_DataBound">
                            </infs:WclDropDownList>--%>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTenantName_ItemSelected"
                                Enabled="false"
                                DataTextField="TenantName" DataValueField="TenantID" OnDataBound="ddlTenantName_DataBound"
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" CssClass="form-control"
                                Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    ValidationGroup="grpFormSubmit"
                                    InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%-- <uc1:CustomAttributeLoaderSearch ID="ucCustomAttributeLoader" runat="server" />--%>
            <uc:CustomAttributeLoaderNodeSearch ID="ucCustomAttributeLoaderNodeSearch" runat="server" />
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Select a User Group. The selected group's members' checkboxes will be marked in the grid below">
                        <span class='cptn'>User Group</span>
                        <infs:WclComboBox ID="ddlUserGroup" runat="server" DataTextField="UG_Name" DataValueField="UG_ID"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            OnDataBound="ddlUserGroup_DataBound" OnSelectedIndexChanged="ddlUserGroup_SelectedIndexChanged"
                            AutoPostBack="true" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select a Package to restrict items in the grid to the selected Package">
                        <span class='cptn'>Package</span>
                        <infs:WclComboBox ID="ddlPackage" runat="server" AutoPostBack="true" DataTextField="PackageName"
                            DataValueField="CompliancePackageID" OnSelectedIndexChanged="ddlPackage_SelectedIndexChanged"
                            OnDataBound="ddlPackage_DataBound" Width="100%" CssClass="form-control" Skin="Silk"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select a Category to restrict items in the grid to the selected Category">
                        <span class='cptn'>Category</span>
                        <infs:WclComboBox ID="ddlCategory" runat="server" AutoPostBack="true" DataTextField="CategoryName"
                            DataValueField="ComplianceCategoryID" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                            OnDataBound="ddlCategory_DataBound" Width="100%" CssClass="form-control" Skin="Silk"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Show only Rush Orders</span>
                        <asp:CheckBox ID="chkShowRushOrders" runat="server" Checked="false" OnCheckedChanged="chkShowRushOrders_CheckedChanged"
                            AutoPostBack="true" Width="100%" CssClass="form-control" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="row">&nbsp;</div>
    <infsu:CommandBar ID="fsucCmdBar1" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
        AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo" SubmitButtonText="Reset"
        SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
        ValidationGroup="grpFormSubmit" OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click"
        OnCancelClick="CmdBarCancel_Click" UseAutoSkinMode="false" ButtonSkin="Silk">
    </infsu:CommandBar>
    <div runat="server" id="pnlVerification" visible="false">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">
                    <asp:Label Text="Data Verification Queue" runat="server" ID="lblPageHdr" />
                </h2>
            </div>
        </div>
        <div class="row allowscroll" id="linkHover">
            <infs:WclGrid runat="server" ID="grdVerificationItemData" AllowPaging="true" AutoGenerateColumns="false"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="true" CellSpacing="0"
                NonExportingColumns="ViewDetail,CustomAttributes,IsUiRulesViolate" GridLines="Both" OnNeedDataSource="grdVerificationItemData_NeedDataSource"
                OnItemCommand="grdVerificationItemData_ItemCommand" ShowAllExportButtons="False"
                OnSortCommand="grdVerificationItemData_SortCommand"
                OnItemDataBound="grdVerificationItemData_ItemDataBound"
                AllowCustomPaging="true" OnInit="grdVerificationItemData_Init">
                <ClientSettings EnableRowHoverStyle="true">
                    <ClientEvents OnRowDblClick="grd_rwDbClick" OnFilterMenuShowing="filterMenuShowing"/>
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <ExportSettings Pdf-PageWidth="350mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="ApplicantComplianceItemID,ApplicantId">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="true"
                        ShowExportToPdfButton="true" ShowExportToCsvButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                       <%-- <telerik:GridBoundColumn DataField="ApplicantName" FilterControlAltText="Filter ApplicantName column"
                            HeaderText="Applicant Name" SortExpression="ApplicantName" UniqueName="ApplicantName"
                            HeaderTooltip="This column displays the applicant's name for each record in the grid">
                        </telerik:GridBoundColumn>--%>
                         <telerik:GridBoundColumn DataField="ApplicantFirstName" FilterControlAltText="Filter ApplicantFirstName column"
                            HeaderText="Applicant First Name" SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName"
                            HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn DataField="ApplicantLastName" FilterControlAltText="Filter ApplicantLastName column"
                            HeaderText="Applicant Last Name" SortExpression="ApplicantLastName" UniqueName="ApplicantLastName"
                            HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemName" FilterControlAltText="Filter ItemName column"
                            HeaderText="Item Name" SortExpression="ItemName" UniqueName="ItemName" HeaderTooltip="This column displays the name of the Item for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CategoryName" FilterControlAltText="Filter CategoryName column"
                            HeaderText="Category Name" SortExpression="CategoryName" UniqueName="CategoryName"
                            HeaderTooltip="This column displays the name of the Category for each record">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter PackageName column"
                            HeaderText="Package Name" SortExpression="PackageName" UniqueName="PackageName"
                            HeaderTooltip="This column displays the name of the package for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="SubmissionDate" FilterControlAltText="Filter SubmissionDate column"
                            HeaderText="Submission Date" SortExpression="SubmissionDate" UniqueName="SubmissionDate"
                            HeaderTooltip="This column displays the date the applicant submitted the Item for review"
                            DataFormatString="{0:d}" FilterControlWidth="100px">
                        </telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn DataField="VerificationStatus" FilterControlAltText="Filter VerificationStatus column" AllowSorting ="false" AllowFiltering="false"
                            HeaderText="Verification Status" SortExpression="VerificationStatus" UniqueName="VerificationStatus"
                            HeaderTooltip="This column displays the applicant's overall compliance status for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ReviewLevel" FilterControlAltText="Filter ReviewLevel column" AllowSorting ="true" 
                            HeaderText="Review Level" SortExpression="ReviewLevel" UniqueName="ReviewLevel"
                            Visible="false"
                            HeaderTooltip="This column displays the Review Level for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SystemStatus" FilterControlAltText="Filter SystemStatus column" AllowSorting ="false" AllowFiltering="false"
                            HeaderText="System Status" SortExpression="SystemStatus" UniqueName="SystemStatus"
                            HeaderTooltip="This column displays the system suggested Item Compliance, if a compliance rule has been applied at the Item level">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RushOrderStatus" FilterControlAltText="Filter RushOrderStatus column" AllowSorting ="false" AllowFiltering="false"
                            HeaderText="Rush Order" SortExpression="RushOrderStatus" UniqueName="RushOrderStatus"
                            HeaderTooltip="This column displays the Rush Order, if any, for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CustomAttributes" FilterControlAltText="Filter CustomAttributes column"
                            AllowFiltering="false" HeaderText="Custom Attributes" AllowSorting="false" ItemStyle-Width="200px"
                            UniqueName="CustomAttributes" HeaderTooltip="This column displays the Custom Attributes for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CustomAttributes" AllowFiltering="false" HeaderText="Custom Attributes"
                            AllowSorting="false" ItemStyle-Width="300px"
                            UniqueName="CustomAttributesTemp" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsUiRulesViolate" AllowFiltering="false" HeaderText="IsUiRulesViolate" AllowSorting="false" ItemStyle-Width="300px"
                            UniqueName="IsUiRulesViolate" Display="false">
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfCatId" runat="server" Value='<%# Eval("CategoryId") %>' />
                                <asp:HiddenField ID="hdfPackSubscriptionId" runat="server" Value='<%# Eval("PackageSubscriptionId") %>' />
                                <%--<a id="ancItemDataDetail" runat="server" title="Click to open the verification screen for this Item">
                                    Detail</a>--%>
                                <telerik:RadButton ID="btnEditNew" ButtonType="LinkButton" CommandName="ViewDetail"
                                    ToolTip="Click to open the verification screen for this Item" runat="server"
                                    Text="Detail">
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False" OnClientShowing="MenuShowing">
                </FilterMenu>
            </infs:WclGrid>
        </div>
    </div>
</div>
<script type="text/javascript">
    //click on link button while double click on any row of grid.
    function grd_rwDbClick(s, e) {
        var _id = "btnEditNew";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

  
</script>
