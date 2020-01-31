<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgencyReviewQueue.ascx.cs" Inherits="CoreWeb.ClinicalRotation.Views.AgencyReviewQueue" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<%@ Register TagPrefix="uc" TagName="AgencySearch" Src="~/CommonControls/UserControl/AgencySearch.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="manageUploadDocument">
    <infs:LinkedResource Path="~/Resources/Mod/ClinicalRotation/AgencyReviewQueue.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

</infs:WclResourceManagerProxy>

<script type="text/javascript">

    function pageLoad() {
      
        SetDefaultButtonForSection("divSearchPanel", "cmdBar_btnSave", true);
    }
</script>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Agency Review Queue
            </h2>
        </div>
    </div>

    <div class="col-md-12">
        <div class="row">
            <div class='form-group col-md-3'>
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
        </div>
    </div>
    <div id="divSearchPanel">
        <div class="row bgLightGreen">
            <asp:Panel runat="server" ID="pnlShowFilters">


                <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                    <span class='cptn'>
                        <asp:Label ID="lblTenant" runat="server" Text="Institution"></asp:Label></span><span class="reqd">*</span>
                    <infs:WclComboBox ID="cmbTenant" runat="server" AutoPostBack="false" DataTextField="TenantName"
                        DataValueField="TenantID" EmptyMessage="--Select--" EnableCheckAllItemsCheckBox="true" CheckBoxes="true"
                        ValidationGroup="grpSrchFilter" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                    </infs:WclComboBox>
                    <div class="vldx">
                        <asp:CustomValidator ID="rfvTenants" ErrorMessage="Institution is required" ValidateEmptyText="true"
                            ClientValidationFunction="ValidateCheckBoxSelection" ControlToValidate="cmbTenant" CssClass="errmsg"
                            Display="Dynamic" ValidationGroup="grpSrchFilter" runat="server"></asp:CustomValidator>
                    </div>
                </div>
                <div class='form-group col-md-3'>&nbsp;</div>
                <div class='form-group col-md-3'>
                    <span class='cptn'>
                        <asp:Label ID="Label1" runat="server" Text="Review Status"></asp:Label></span>
                    <infs:WclComboBox ID="cmbSearchStatus" runat="server" AutoPostBack="false" DataTextField="SearchStatusName"
                        DataValueField="SearchStatusCode" EmptyMessage="--Select--" CheckBoxes="true"
                        Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                    </infs:WclComboBox>
                </div>


            </asp:Panel>
        </div>
        <div class="row">
            <infsu:CommandBar ID="cmdBar" runat="server" DefaultPanel="pnlSrchFilter" AutoPostbackButtons="Save,Cancel,Submit"
                SaveButtonIconClass="rbSearch" OnCancelClick="cmdBar_CancelClick" SaveButtonText="Search" SubmitButtonText="Reset" CancelButtonText="Cancel" SubmitButtonIconClass="rbUndo"
                ValidationGroup="grpSrchFilter" DisplayButtons="Save,Cancel,Submit" CauseValidationOnCancel="false"
                OnSaveClick="cmdBar_SaveClick" ButtonPosition="Center" OnSubmitClick="cmdBar_SubmitClick" UseAutoSkinMode="false" ButtonSkin="Silk" />
        </div>
        <div class="row">&nbsp; </div>

        <div class="row bgLightGreen">
            <asp:Panel runat="server" ID="Panel1">


                <div class='form-group col-md-6' title="Select Search Agency ">
                    <span class="cptn">Search Agency</span>
                    <uc:AgencySearch ID="ucAgencySrch" runat="server" />

                </div>
            </asp:Panel>
        </div>
    </div>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdAgencies" AutoGenerateColumns="False" AllowSorting="True"
            AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
            ShowClearFiltersButton="false" GridLines="Both" OnNeedDataSource="grdAgencies_NeedDataSource"
            AllowCustomPaging="true" OnSortCommand="grdAgencies_SortCommand">
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="AgencyId" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="false"
                    ShowExportToPdfButton="false" ShowExportToCsvButton="false" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chk" runat="server" onclick="UnCheckHeader(this)" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="tplcohdr" />
                    </telerik:GridTemplateColumn>
                    <telerik:GridDateTimeColumn DataField="AgencyName" HeaderText="Name" SortExpression="AgencyName"
                        UniqueName="AgencyName" HeaderTooltip="This column displays the name of the Agency for each record in the grid">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridDateTimeColumn DataField="NpiNumber" HeaderText="NPI Number" SortExpression="NpiNumber" ItemStyle-Width="150px"
                        UniqueName="NpiNumber" HeaderTooltip="This column displays the NPI Number of the Agency for each record in the grid">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridDateTimeColumn DataField="FullAddress" HeaderText="Address" SortExpression="FullAddress"
                        UniqueName="FullAddress" HeaderTooltip="This column displays the Full Address of the Agency for each record in the grid">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridDateTimeColumn DataField="InstitutionName" HeaderText="Institution Name" SortExpression="InstitutionName"
                        UniqueName="InstitutionName" HeaderTooltip="This column displays the Institution Name of the Agency for each record in the grid">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="ReviewStatus"
                        HeaderText="Status" UniqueName="ReviewStatus"
                        HeaderTooltip="This column displays the Review Status for each Agency">
                    </telerik:GridBoundColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
        <infsu:CommandBar ID="cmdStatusUpdate" runat="server" Visible="false"
            SaveButtonIconClass="rbSave" SaveButtonText="Reviewed" SubmitButtonText="Make Available" AutoPostbackButtons="Save,Submit"
            ValidationGroup="grpSrchFilter" DisplayButtons="Save,Submit" SubmitButtonIconClass="rbOk"
            OnSaveClick="cmdStatusUpdate_SaveClick" OnSubmitClick="cmdStatusUpdate_SubmitClick" ButtonPosition="Center" UseAutoSkinMode="false" ButtonSkin="Silk" />
        <div class="col-md-12">&nbsp;</div>
    </div>
</div>
