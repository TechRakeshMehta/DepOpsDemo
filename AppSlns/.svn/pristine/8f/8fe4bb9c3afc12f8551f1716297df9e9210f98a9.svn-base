<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewAgencyJobPosting.ascx.cs"
    Inherits="CoreWeb.AgencyJobBoard.Views.ViewAgencyJobPosting" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxAgencyJobPosting">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
   <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<script type="text/javascript">
    function grdAgencyJobs_rwDbClick(s, e) {
        var _id = "btnEdit";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

</script>

<div id="dvTop" class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Job Board
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' id="dvInstitution" runat="server" visible="false" title="Restrict search results to the selected institution">
                        <span class="cptn">Institution</span><span class='reqd'>*</span>
                        <infs:WclComboBox ID="cmbInstitution" runat="server" Width="100%" AutoPostBack="false"
                            CssClass="form-control" Skin="Silk" AutoSkinMode="false" DataTextField="TenantName"
                            DataValueField="TenantID" EmptyMessage="--SELECT--" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvInstitution" ControlToValidate="cmbInstitution"
                                InitialValue="" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                Text="Institution is required." />
                        </div>
                        <div class="vldx">
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Job Title">
                        <span class="cptn">Job Title</span>
                        <infs:WclTextBox ID="txtJobTitle" runat="server" Width="100%" AutoPostBack="false"
                            CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclTextBox>
                        <div class="vldx">
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Company">
                        <span class="cptn">Company</span>
                        <%--    <infs:WclTextBox ID="txtCompany" runat="server" Width="100%" AutoPostBack="false"
                            CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclTextBox>--%>

                        <infs:WclTextBox ID="txtCompany" runat="server" Width="100%" AutoPostBack="false"
                            CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclTextBox>
                        <div class="vldx">
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Location">
                        <span class="cptn">Location</span>
                        <infs:WclTextBox ID="txtLocation" runat="server" Width="100%" AutoPostBack="false"
                            CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclTextBox>
                        <div class="vldx">
                        </div>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the selected Job Type">
                        <span class="cptn">Job Type</span>
                        <asp:RadioButtonList ID="rbljobType" runat="server" RepeatDirection="Horizontal" CssClass="radio_list"
                            AutoPostBack="false">
                            <asp:ListItem Text="Internship" Selected="True" Value="AAAA"></asp:ListItem>
                            <asp:ListItem Text="Employment" Value="AAAB"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the selected Field Type">
                        <span class="cptn">Field Type</span>
                        <infs:WclComboBox ID="cmbJobFieldType" runat="server" CheckBoxes="false"
                            DataTextField="Description" DataValueField="ID"
                            OnClientKeyPressing="openCmbBoxOnTab" AutoPostBack="false" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                </div>
            </div>

        </asp:Panel>
        <div class="col-md-12">&nbsp;</div>
        <div class="col-md-12">
            <div class="row text-center">
                <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                    AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo"
                    SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch" ValidationGroup="grpFormSubmit"
                    CancelButtonText="Cancel" OnSubmitClick="fsucCmdBarButton_SubmitClick" OnSaveClick="fsucCmdBarButton_SaveClick"
                    OnCancelClick="fsucCmdBarButton_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk">
                </infsu:CommandBar>
            </div>
            <div class="col-md-12">&nbsp;</div>
        </div>
    </div>
    <div class="row allowscroll">
        <infs:WclGrid runat="server" ID="grdAgencyJobs" AllowPaging="True" PageSize="10" AllowFilteringByColumn="false"
            AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" ShowAllExportButtons="False" AllowCustomPaging="true"
            OnNeedDataSource="grdAgencyJobs_NeedDataSource" OnItemCommand="grdAgencyJobs_ItemCommand" ShowClearFiltersButton="false"
            OnSortCommand="grdAgencyJobs_SortCommand" OnPreRender="grdAgencyJobs_PreRender">
            <ExportSettings Pdf-PageWidth="300mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grdAgencyJobs_rwDbClick" />
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
                            <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="ViewDetail"
                                ToolTip="Click here to view details of Job." runat="server" Text="Detail">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
        </infs:WclGrid>
    </div>
</div>
