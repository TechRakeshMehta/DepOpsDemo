<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewAgencyJobPosting.ascx.cs" Inherits="CoreWeb.AgencyJobBoard.Views.ViewAgencyJobPosting" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxAdminView">
    <infs:LinkedResource ResourceType="StyleSheet" Path="~/Resources/Themes/Default/colors.css" />
</infs:WclResourceManagerProxy>

<script type="text/javascript">

    function grd_rwDbClick(s, e) {
        var _id = "btnViewDetails";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

</script>

<asp:Panel ID="pnlMain" runat="server" Width="100%" Height="100%">
    <h1 class="mhdr">
        <asp:Label runat="server" ID="lblHeader" Text="Job Opportunities">
        </asp:Label>
    </h1>
    <div class="section">
        <div class="content">
            <div class="msgbox" id="msgBox" runat="server">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <div class="sxform auto">
                <asp:Panel runat="server" CssClass="sxpnl">
                    <div class='sxro sx3co' id="dvInstitution" runat="server" visible="false" title="Restrict search results to the selected institution">
                        <div class='sxlb'>
                            <span class="cptn">Institution</span><span class='reqd'>*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="cmbInstitution" runat="server" Width="100%" AutoPostBack="false"
                                CssClass="form-control" Skin="Silk" AutoSkinMode="false" DataTextField="TenantName"
                                DataValueField="TenantID" EmptyMessage="--SELECT--" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" OnClientKeyPressing="openCmbBoxOnTab">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvInstitution" ControlToValidate="cmbInstitution"
                                    InitialValue="" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class='cptn' title="Job Title">Job Title</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtJobTitle" runat="server" Width="100%" AutoPostBack="false">
                            </infs:WclTextBox>
                            <div class="vldx">
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class='cptn' title="Job Title">Company</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtCompany" runat="server" Width="100%" AutoPostBack="false">
                            </infs:WclTextBox>
                            <div class="vldx">
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class='cptn' title="Job Title">Location</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtLocation" runat="server" Width="100%" AutoPostBack="false">
                            </infs:WclTextBox>
                            <div class="vldx">
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class='cptn'>Job Type</span>
                        </div>
                        <div class='sxlm'>
                            <asp:RadioButtonList ID="rbljobType" runat="server" RepeatDirection="Horizontal" CssClass="radio_list"
                                AutoPostBack="false">
                                <asp:ListItem Text="Internship" Selected="True" Value="AAAA"></asp:ListItem>
                                <asp:ListItem Text="Employment" Value="AAAB"></asp:ListItem>
                            </asp:RadioButtonList>
                            <div class="vldx">
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class='cptn'>Field Type</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="cmbJobFieldType" runat="server" DataTextField="Description" DataValueField="ID">
                            </infs:WclComboBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </asp:Panel>
                <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                    AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbRefresh"
                    SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
                    CancelButtonText="Cancel" OnSubmitClick="fsucCmdBarButton_SubmitClick" OnSaveClick="fsucCmdBarButton_SaveClick"
                    OnCancelClick="fsucCmdBarButton_CancelClick">
                </infsu:CommandBar>
            </div>
        </div>
    </div>
    <infs:WclGrid runat="server" ID="grdAgencyJobs" AllowPaging="True" PageSize="10" AllowFilteringByColumn="false" AutoSkinMode="true"
        AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" ShowAllExportButtons="False" AllowCustomPaging="true"
        OnNeedDataSource="grdAgencyJobs_NeedDataSource" OnItemCommand="grdAgencyJobs_ItemCommand" ShowClearFiltersButton="false"
        OnSortCommand="grdAgencyJobs_SortCommand" OnPreRender="grdAgencyJobs_PreRender">
        <ExportSettings Pdf-PageWidth="300mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
            Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
            ExportOnlyData="true" IgnorePaging="true">
        </ExportSettings>
        <ClientSettings EnableRowHoverStyle="true">
            <ClientEvents OnRowDblClick="grd_rwDbClick" />
            <Selecting AllowRowSelect="true"></Selecting>
        </ClientSettings>
        <MasterTableView CommandItemDisplay="Top" DataKeyNames="AgencyJobID"
            AllowFilteringByColumn="false"
            AllowCustomPaging="true">
            <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"></CommandItemSettings>
            <Columns>
                <telerik:GridBoundColumn DataField="Company" FilterControlAltText="Filter Company column"
                    HeaderText="Company" SortExpression="Company" UniqueName="Company">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column"
                    HeaderText="Location" SortExpression="Location" UniqueName="Location">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="JobTitle" FilterControlAltText="Filter JobTitle column"
                    HeaderText="Job Title" SortExpression="JobTitle" UniqueName="JobTitle">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="JobDescription" FilterControlAltText="Filter JobDescription column"
                    HeaderText="Job Description" SortExpression="JobDescription" UniqueName="JobDescription">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="JobTypeName" FilterControlAltText="Filter JobType column"
                    HeaderText="Job Type" SortExpression="JobTypeName" UniqueName="JobTypeName">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="FieldTypeName" FilterControlAltText="Filter FieldTypeName column"
                    HeaderText="Field Type" SortExpression="FieldTypeName" UniqueName="FieldTypeName">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <telerik:RadButton ID="btnViewDetails" ButtonType="LinkButton" BackColor="Transparent" Font-Underline="true"
                            BorderStyle="None" ForeColor="Black" CommandName="ViewDetail"
                            ToolTip="Click here to view details of Job." runat="server" Text="Detail">
                        </telerik:RadButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
        </MasterTableView>
    </infs:WclGrid>
</asp:Panel>

