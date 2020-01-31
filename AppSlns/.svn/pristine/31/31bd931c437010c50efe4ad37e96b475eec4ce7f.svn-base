<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.SearchControl" CodeBehind="SearchControl.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%--<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearch.ascx" TagName="CustomAttributeLoaderSearch" TagPrefix="uc1" %>--%>
<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearchMultipleNodes.ascx"
    TagName="CustomAttributeLoaderNodeSearch" TagPrefix="uc" %>
<%--<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblVerificationQueue" runat="server" Text=""></asp:Label></h1>--%>
<%--<div class="content">--%>
<script src="../../Resources/Mod/Shared/ApplyNewIcons.js" type="text/javascript"></script>
<style type="text/css">
    .chkAllResults {
        width: 10%;
        float: left;
        margin-top: 10px;
        padding-left: 7px;
    }

    .fsucCmdBarStyle {
        padding-left: 0px !important;
        width: 89%;
    }
</style>

<div class="container-fluid">
    <div class="row bgLightGreen">
        <asp:Panel runat="server">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span>
                            <%--<infs:WclDropDownList ID="ddlTenantName" runat="server" DataTextField="TenantName"
                        AutoPostBack="true" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                        OnDataBound="ddlTenantName_DataBound" Enabled ="false">
                    </infs:WclDropDownList>--%>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" Enabled="false"
                                AutoPostBack="true" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                OnDataBound="ddlTenantName_DataBound" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
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
            <%--  <uc1:CustomAttributeLoaderSearch ID="ucCustomAttributeLoader" runat="server" />--%>
            <uc:CustomAttributeLoaderNodeSearch ID="ucCustomAttributeLoaderNodeSearch" runat="server" />

            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Select a Package to restrict items in the grid to the selected Package">
                        <span class="cptn">Package</span>
                        <infs:WclComboBox ID="ddlPackage" runat="server" CheckBoxes="true" AutoPostBack="false" DataTextField="PackageName"
                            DataValueField="CompliancePackageID" EnableCheckAllItemsCheckBox="true"  AllowCustomText="true" 
                             Width="100%" CssClass="form-control" Skin="Silk" EmptyMessage="--SELECT--"
                            Localization-CheckAllString="Check All" 
                            AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnClientDropDownClosed="BtnClickPostBack" >
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select a Category to restrict items in the grid to the selected Category">
                        <span class="cptn">Category</span>
                        <infs:WclComboBox ID="ddlCategory" runat="server" DataTextField="CategoryName" CheckBoxes="true" AllowCustomText="true"
                            EnableCheckAllItemsCheckBox="true" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            DataValueField="ComplianceCategoryID" OnDataBound="ddlCategory_DataBound" EmptyMessage="--Select--"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered first name">
                        <span class="cptn">Applicant First Name</span>
                        <infs:WclTextBox ID="txtFirstName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered last name">
                        <span class="cptn">Applicant Last Name</span>
                        <infs:WclTextBox ID="txtLastName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div id="divDOB" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to the entered Date of Birth">
                            <span class="cptn">Date of Birth</span>
                            <infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="Select a date"
                                Width="100%" CssClass="form-control">
                            </infs:WclDatePicker>
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Restrict the search results to the entered Item Label">
                        <span class="cptn">Item Label</span>
                        <infs:WclTextBox ID="txtItemLabel" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Select a User Group to restrict search results to that group">
                        <span class="cptn">User Group</span>
                        <infs:WclComboBox ID="ddlUserGroup" runat="server" DataTextField="UG_Name" DataValueField="UG_ID"
                            CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            Width="100%" CssClass="form-control" Skin="Silk" EmptyMessage="--Select--" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select &#34All&#34 to view all subscriptions per the other parameters or &#34Archived&#34 to view only archived subscriptions or &#34Active&#34 to view only non archived subscriptions">
                        <span class="cptn">Subscription Archive State</span>
                        <asp:RadioButtonList ID="rbSubscriptionState" runat="server" RepeatDirection="Horizontal"
                            DataTextField="As_Name" DataValueField="AS_Code" CssClass="form-control" AutoPostBack="false">
                        </asp:RadioButtonList>
                    </div>

                </div>
            </div>

            <div class="col-md-12" id="divAdminDataItemSearch" runat="server">
                <div class="row">
                    <div id="div1" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to Submission Date greater than or equal to the entered date">
                            <span class="cptn">Submission Date From</span>
                            <infs:WclDatePicker ID="dpkrSubmissionDateFrom" runat="server" DateInput-EmptyMessage="Select a date"
                                Width="100%" CssClass="form-control">
                            </infs:WclDatePicker>
                        </div>
                    </div>
                    <div id="div2" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to Submission Date less than or equal to the entered date">
                            <span class="cptn">Submission Date To</span>
                            <infs:WclDatePicker ID="dpkrSubmissionDateTo" runat="server" DateInput-EmptyMessage="Select a date"
                                Width="100%" CssClass="form-control">
                            </infs:WclDatePicker>
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Restrict the search results to the entered Assigned User">
                        <span class="cptn">Assigned to User</span>
                        <infs:WclTextBox ID="txtAssignedUser" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict the search results to the entered System Status">
                        <span class="cptn">System Status</span>
                        <asp:RadioButtonList ID="rbSystemStatus" runat="server" RepeatDirection="Horizontal">
                          <asp:ListItem Text="All" Value="AA" Selected="True"></asp:ListItem>
                          <asp:ListItem Text="Passed " Value="Passed"></asp:ListItem>
                          <asp:ListItem Text="Failed" Value="Failed"></asp:ListItem>
                          <asp:ListItem Text="Error" Value="Error"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' >
                        <span class="cptn">Subscription Expiry State</span>
                        <asp:RadioButtonList ID="rbSubscriptionExpiryState" runat="server" RepeatDirection="Horizontal">
                          <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                          <asp:ListItem Text="Expired " Value="0"></asp:ListItem>
                          <asp:ListItem Text="All" Value="ZZ"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class='form-group col-md-9' title="If one or more status is selected, the search results will only display Items whose status matches one of the selections">
                        <span class="cptn">Status</span>
                        <asp:CheckBoxList ID="chkItemStatus" DataTextField="Name" RepeatDirection="Horizontal"
                            DataValueField="ItemComplianceStatusID" runat="server" CssClass="form-control"
                            Width="100%">
                        </asp:CheckBoxList>
                    </div>
                </div>
            </div>


            <div class='col-md-12' style="display: none">
                <div class="row">
                    <%--<div class='sxlb'>
                        Program
                    </div>--%>
                    <div class='form-group col-md-3'>
                        <infs:WclComboBox ID="ddlProgram" runat="server" DataTextField="ProgramStudy"
                            DataValueField="AdminProgramStudyID" OnDataBound="ddlProgram_DataBound" Visible="false">
                        </infs:WclComboBox>
                    </div>
                    <%-- <div class='sxlb'>
                        Status
                    </div>
                    <div class='sxlm'>
                        <infs:WclListBox ID="chkItemStatus" CheckBoxes="true" DataTextField="Name" DataValueField="ItemComplianceStatusID"
                            runat="server">
                        </infs:WclListBox>
                    </div>--%>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group col-md-2">
                <div class="row" id="divSelectAllResults" runat="server">
                    <asp:CheckBox ID="chkSelectAllResults" Text="Select All Results" runat="server" OnCheckedChanged="chkSelectAllResults_CheckedChanged"
                        AutoPostBack="true"
                        Width="100%" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group col-md-10">
                <div id="trailingText" class="row">
                    <infsu:CommandBar ID="fsucCmdBar1" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel,Clear"
                        AutoPostbackButtons="Submit,Save,Cancel,Clear" SubmitButtonIconClass="rbUndo"
                        SubmitButtonText="Reset"
                        SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
                        ClearButtonText="Send Message"
                        OnClearClick="btnSendMessage_Click" ClearButtonIconClass="rbEnvelope" ValidationGroup="grpFormSubmit"
                        OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click" OnCancelClick="CmdBarCancel_Click"
                        UseAutoSkinMode="false" ButtonSkin="Silk">
                    </infsu:CommandBar>
                </div>
            </div>
        </div>
    </div>
    <%--<infsu:CommandBar ID="fsucCmdBar2" runat="server" DisplayButtons="Submit" AutoPostbackButtons="Submit"
            SubmitButtonText="Search" DefaultPanel="pnlRegForm" OnSubmitClick="fsucCmdBar1_SubmitClick" />--%>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdItemData" AllowPaging="True" AutoGenerateColumns="False"
            AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
            GridLines="Both" ShowAllExportButtons="False" OnNeedDataSource="grdItemData_NeedDataSource"
            OnItemCommand="grdItemData_ItemCommand" AllowCustomPaging="true" OnSortCommand="grdItemData_SortCommand"
            OnItemDataBound="grdItemData_ItemDataBound" OnPreRender="grdItemData_PreRender"
            OnInit="grdItemData_Init" NonExportingColumns="Detail,CustomAttributes,UserGroups" EnableLinqExpressions="false">
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grd_rwDbClick" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ApplicantComplianceItemID,ApplicantId">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="SelectUsers" HeaderTooltip="Click this box to select all users on the active page"
                        AllowFiltering="false" ShowFilterIcon="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectUser" runat="server"
                                onclick="UnCheckHeader(this)" OnCheckedChanged="chkSelectUser_CheckedChanged" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdfCatId" runat="server" Value='<%# Eval("CategoryID") %>' />
                            <asp:HiddenField ID="hdfPackSubscriptionId" runat="server" Value='<%# Eval("PackageSubscriptionID") %>' />
                            <asp:HiddenField ID="hdnPkgId" runat="server" Value='<%# Eval("PackageID") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                  <%--  <telerik:GridBoundColumn DataField="ApplicantName" FilterControlAltText="Filter ApplicantName column"
                        HeaderText="Applicant Name" SortExpression="ApplicantName" UniqueName="ApplicantName"
                        HeaderTooltip="This column displays the applicant's name for each record in the grid">
                    </telerik:GridBoundColumn>--%>
                       <telerik:GridBoundColumn DataField="FirstName" FilterControlAltText="Filter FirstName column"
                        HeaderText="First Name" SortExpression="FirstName" UniqueName="FirstName"
                        HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                    </telerik:GridBoundColumn>
                       <telerik:GridBoundColumn DataField="LastName" FilterControlAltText="Filter LastName column"
                        HeaderText="Last Name" SortExpression="LastName" UniqueName="LastName"
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
                    <telerik:GridBoundColumn DataField="VerificationStatus" FilterControlAltText="Filter VerificationStatus column"
                        HeaderText="Verification Status" SortExpression="VerificationStatus" UniqueName="VerificationStatus"
                        HeaderTooltip="This column displays the applicant's overall compliance status for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SystemStatus" FilterControlAltText="Filter SystemStatus column"
                        HeaderText="System Status" SortExpression="SystemStatus" UniqueName="SystemStatus"
                        HeaderTooltip="This column displays the system suggested Item Compliance, if a compliance rule has been applied at the Item level">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AssignedUserName" FilterControlAltText="Filter AssignedToUser column"
                        HeaderText="Assigned To User" SortExpression="AssignedUserName" UniqueName="AssignedUserName"
                        HeaderTooltip="This column displays the user, if any,  who has been assigned to complete the verification for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CustomAttributes" FilterControlAltText="Filter CustomAttributes column"
                        AllowFiltering="false" HeaderText="Custom Attributes" AllowSorting="false" ItemStyle-Width="200px"
                        UniqueName="CustomAttributes" HeaderTooltip="This column displays the Custom Attributes for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CustomAttributes" AllowFiltering="false" HeaderText="Custom Attributes"
                        AllowSorting="false" ItemStyle-Width="300px"
                        UniqueName="CustomAttributesTemp" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="UserGroups" FilterControlAltText="Filter UserGroups column"
                        HeaderText="User Group" SortExpression="UserGroups" UniqueName="UserGroups" AllowFiltering="false" AllowSorting="false"
                        ItemStyle-Width="130px" HeaderTooltip="This column displays any user group(s) to which the applicant belongs for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="UserGroups" AllowFiltering="false" HeaderText="User Group"
                        AllowSorting="false" ItemStyle-Width="300px" UniqueName="UserGroupsTemp" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Detail">
                        <ItemTemplate>
                            <%--  <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="ViewDetail"
                                    runat="server" Text="Detail" BackColor="Transparent" Font-Underline="true" BorderStyle="None">
                                </telerik:RadButton>--%>
                            <telerik:RadButton ID="btnDetails" ButtonType="LinkButton" CommandName="VerificationDetails"
                                ToolTip="Click to open the verification screen for this Item" runat="server"
                                Text="Detail">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
        <asp:HiddenField ID="hdMessageSent" runat="server" Value="new" />
    </div>
</div>
<%-- </div>--%>
<%--</div>--%>
<asp:Button ID="btnPackageChecked" runat="server" OnClick="btnPackageChecked_Click" />
<script type="text/javascript">
    //click on link button while double click on any row of grid.
    function grd_rwDbClick(s, e) {
        var _id = "btnDetails";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }


    function CheckAll(id) {
        var masterTable = $find("<%= grdItemData.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectUser").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectUser").checked = isChecked; // for checking the checkboxes
            }
        }
    }

    function UnCheckHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdItemData.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectUser").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectUser").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }

    function OpenPopup(sender, eventArgs) {
        var composeScreenWindowName = "composeScreen";
        var fromScreenName = "AdminDataItemSearch";
        var communicationTypeId = 'CT01';
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/Messaging/Pages/WriteMessage.aspx?cType=" + communicationTypeId + "&SName=" + fromScreenName);
        var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName, onclose: OnMessagePopupClose });
        return false;
    }

    //This event fired when Send Message popup closed.
    function OnMessagePopupClose(oWnd, args) {
        oWnd.remove_close(OnMessagePopupClose);
        var arg = args.get_argument();
        if (arg) {
            if (arg.MessageSentStatus == "sent") {
                $jQuery("[id$=hdMessageSent]").val("sent");
                var masterTable = $find("<%= grdItemData.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        }
    }

    $jQuery(document).ready(function () {
        var needSendMessageFunctionality = '<%= needSendMessageFunctionality %>';
        if (needSendMessageFunctionality.toLowerCase() == "true") {
            var obj = $jQuery("div[id*=fsucCmdBar1]").addClass("fsucCmdBarStyle");
        }
        else {

        }
    });

    function BtnClickPostBack()
    {
        $jQuery("[id$=btnPackageChecked]").click();
    }

   

</script>
