<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Search.Views.ApplicantSearch" CodeBehind="ApplicantSearch.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblApplicantSearch" runat="server" Text="Manage Applicant Search"></asp:Label></h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel ID="pnlSearch" CssClass="sxpnl" runat="server">
                <div class='sxro sx3co'>
                    <div id="divTenant" runat="server">
                        <div class='sxlb' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <%--<infs:WclDropDownList ID="ddlTenantName" runat="server" DataTextField="TenantName"
                                CausesValidation="false" AutoPostBack="true" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                OnDataBound="ddlTenantName_DataBound" Enabled="false">
                            </infs:WclDropDownList>--%>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName"
                                CausesValidation="false" AutoPostBack="true" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                OnDataBound="ddlTenantName_DataBound" Enabled="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                </div>
                <div class='sxroend'>
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
                    <div class='sxlb' title="Restrict search results to the entered User ID">
                        <span class="cptn">User ID</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclNumericTextBox ID="txtUserId" runat="server" MinValue="1" NumberFormat-DecimalDigits="0"
                            NumberFormat-AllowRounding="false" MaxLength="9">
                        </infs:WclNumericTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb' title="Restrict search results to the entered email address">
                        <span class="cptn">Email Address</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtEmail" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div id="divSSN" runat="server">
                        <div class='sxlb' title="Restrict search results to the entered SSN or ID Number">
                            <span class="cptn">SSN/ID Number</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclMaskedTextBox ID="txtSSN" runat="server" MaxLength="10" Mask="aaa-aa-aaaa">
                            </infs:WclMaskedTextBox>
                        </div>
                    </div>
                    <div id="divDOB" runat="server">
                        <div class='sxlb' title="Restrict search results to the entered Date of Birth">
                            <span class="cptn">Date of Birth</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="Select a date"
                                DateInput-DateFormat="MM/dd/yyyy">
                            </infs:WclDatePicker>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb' title="Select &#34All&#34 to view all subscriptions per the other parameters or &#34Archived&#34 to view only archived subscriptions or &#34Active&#34 to view only non archived subscriptions">
                        <span class="cptn">Subscription Archive State</span>
                    </div>
                    <div class='sxlm'>
                        <asp:RadioButtonList ID="rbSubscriptionState" runat="server" RepeatDirection="Horizontal"
                            DataTextField="As_Name" DataValueField="AS_Code" CssClass="radio_list" AutoPostBack="false">
                        </asp:RadioButtonList>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>

                    <%--<div class='sxlb'>
                        Program
                    </div>--%>
                    <div class='sxlm'>
                        <infs:WclDropDownList ID="ddlProgram" runat="server" DataTextField="IN_Name" DataValueField="IN_ID"
                            OnDataBound="ddlProgram_DataBound" Visible="false">
                        </infs:WclDropDownList>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>

            </asp:Panel>
        </div>
        <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
            AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbRefresh" SubmitButtonText="Reset"
            SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
            OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click" OnCancelClick="CmdBarCancel_Click">
        </infsu:CommandBar>
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdApplicantSearchData" AllowCustomPaging="True"
                AutoGenerateColumns="False" AllowSorting="True" AllowFilteringByColumn="True"
                AutoSkinMode="True" CellSpacing="0" GridLines="Both" ShowAllExportButtons="False"
                ShowClearFiltersButton="false" OnNeedDataSource="grdApplicantSearchData_NeedDataSource" OnItemDataBound="grdApplicantSearchData_ItemDataBound"
                OnItemCommand="grdApplicantSearchData_ItemCommand" OnSortCommand="grdApplicantSearchData_SortCommand"
                OnInit="grdApplicantSearchData_Init" EnableLinqExpressions="false" NonExportingColumns="Detail,ApplicantSSN">
                <ClientSettings EnableRowHoverStyle="true">
                    <ClientEvents OnRowDblClick="grd_rwDbClick" />
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserId" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridNumericColumn DataField="OrganizationUserId" HeaderText="User ID" SortExpression="OrganizationUserId"
                            UniqueName="OrganizationUserId" HeaderTooltip="This column displays the User ID for each record in the grid">
                        </telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="ApplicantFirstName" FilterControlAltText="Filter ApplicantFirstName column"
                            HeaderText="Applicant First Name" SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName"
                            HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ApplicantLastName" FilterControlAltText="Filter ApplicantLastName column"
                            HeaderText="Applicant Last Name" SortExpression="ApplicantLastName" UniqueName="ApplicantLastName"
                            HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EmailAddress" FilterControlAltText="Filter EmailAddress column"
                            HeaderText="Email Address" SortExpression="EmailAddress" UniqueName="EmailAddress"
                            HeaderTooltip="This column displays the applicant's email address for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="DateOfBirth" FilterControlAltText="Filter DateOfBirth column"
                            HeaderText="Date of Birth" SortExpression="DateOfBirth" UniqueName="DateOfBirth"
                            HeaderTooltip="This column displays the applicant's date of birth for each record in the grid"
                            DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                        </telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn DataField="ApplicantSSN" HeaderText="SSN/ID Number" SortExpression="ApplicantSSN"
                            HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid"
                            UniqueName="ApplicantSSN">
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="ApplicantSSN" HeaderText="SSN/ID Number" SortExpression="ApplicantSSN" Display="false"
                            HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid"
                            UniqueName="_ApplicantSSN">
                        </telerik:GridBoundColumn>

                        <%-- <telerik:GridTemplateColumn AllowFiltering="false" SortExpression="ApplicantSSN" UniqueName="ApplicantSSN" HeaderText="SSN/ID Number"
                            HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid">
                            <ItemTemplate>
                                <%#String.IsNullOrWhiteSpace(Convert.ToString(Eval("ApplicantSSN"))) || Convert.ToString(Eval("ApplicantSSN")).Length<9 ? "" : Eval("ApplicantSSN").ToString().Substring(0,3)+"-"+Eval("ApplicantSSN").ToString().Substring(3,5)+"-"+Eval("ApplicantSSN").ToString().Substring(5,4)%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridBoundColumn DataField="TenantName" HeaderText="Institution" SortExpression="TenantName"
                            HeaderTooltip="This column displays the Institution for each record in the grid"
                            UniqueName="TenantName">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Detail">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="ViewDetail"
                                    ToolTip="Click here to edit the profile information of the user" runat="server"
                                    Text="Detail" BackColor="Transparent" Font-Underline="true" BorderStyle="None">
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
        </div>
        <div class="gclr">
        </div>
    </div>
</div>

<script type="text/javascript">
    //click on link button while double click on any row of grid.
    function grd_rwDbClick(s, e) {
        var _id = "btnEdit";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

</script>
