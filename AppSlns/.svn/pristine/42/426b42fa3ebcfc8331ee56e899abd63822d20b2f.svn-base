<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdditionalDocumentSearch.ascx.cs"
    Inherits="CoreWeb.Search.Views.AdditionalDocumentSearch" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxApplicantDisclosureDocumentSearch">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>


<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Additional Document Search
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class='reqd'>*</span>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" AutoPostBack="true"
                                DataValueField="TenantID" OnDataBound="ddlTenantName_DataBound"
                                Enabled="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
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
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered document name">
                        <span class="cptn">Document Name</span>
                        <infs:WclTextBox ID="txtDocumentName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <infsu:CommandBar ID="CmdBarSearch" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Clear,Cancel"
                AutoPostbackButtons="Submit,Save,Clear,Cancel" SubmitButtonIconClass="rbUndo"
                SubmitButtonText="Reset" DefaultPanel="pnlSearch" DefaultPanelButton="Save"
                SaveButtonText="Search" SaveButtonIconClass="rbSearch" ClearButtonText="Export Document(s)"
                CancelButtonText="Cancel" ValidationGroup="grpFormSubmit"
                OnSubmitClick="CmdBarSearch_ResetClick" OnSaveClick="CmdBarSearch_SearchClick" OnClearClick="CmdBarSearch_ExportClick"
                OnCancelClick="CmdBarSearch_CancelClick" ClearButtonIconClass="" UseAutoSkinMode="false"
                ButtonSkin="Silk">
            </infsu:CommandBar>
        </div>
    </div>
    <%--<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
    <asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
    <asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
    <asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />--%>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdAdditionalDocSearch" AutoGenerateColumns="false"
            AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
            ShowAllExportButtons="false" ShowExtraButtons="False" AllowCustomPaging="true"
            ShowClearFiltersButton="false" GridLines="Both" OnNeedDataSource="grdAdditionalDocSearch_NeedDataSource"
            OnItemDataBound="grdAdditionalDocSearch_ItemDataBound" OnItemCommand="grdAdditionalDocSearch_ItemCommand"
            OnSortCommand="grdAdditionalDocSearch_SortCommand">
            <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grd_rwDbClick" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="ApplicantDocumentID,ApplicantID"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false"
                    ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowRefreshButton="true" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="ExportCheckBox" AllowFiltering="false" ShowFilterIcon="false"
                        HeaderStyle-Width="2%" ItemStyle-Width="2%">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectDocument" runat="server" onclick="UnCheckHeader(this)"
                                OnCheckedChanged="chkSelectDocument_CheckedChanged" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="DocumentPath" FilterControlAltText="Filter DocumentPath column"
                        HeaderText="DocumentPath" SortExpression="DocumentPath" UniqueName="DocumentPath"
                        Display="false">
                        <ItemTemplate>
                            <asp:Label ID="lblDocumentPath" runat="server" Text='<%# Convert.ToString(Eval("DocumentPath")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="FileName" FilterControlAltText="Filter DocumentPath column"
                        HeaderText="FileName" SortExpression="FileName" UniqueName="FileName" Display="false">
                        <ItemTemplate>
                            <asp:Label ID="lblFileName" runat="server" Text='<%# Convert.ToString(Eval("FileName")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="ApplicantDocumentID" FilterControlAltText="Filter ApplicantDocumentID column"
                        HeaderText="ID" SortExpression="ApplicantDocumentID" UniqueName="ApplicantDocumentID"
                        Visible="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="FirstName" FilterControlAltText="Filter ApplicantName column"
                        HeaderStyle-Width="350" AllowFiltering="false"
                        HeaderText="Applicant First Name" SortExpression="FirstName" UniqueName="FirstName"
                        ReadOnly="true" HeaderTooltip="This column contains the first name of Applicant">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="LastName" FilterControlAltText="Filter ItemName column"
                        HeaderStyle-Width="350" AllowFiltering="false"
                        HeaderText="Applicant Last Name" SortExpression="LastName" UniqueName="LastName" ReadOnly="true"
                        HeaderTooltip="This column contains the Last Name of Applicant">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="FileName" FilterControlAltText="Filter FileName column"
                        HeaderStyle-Width="350" AllowFiltering="false"
                        HeaderText="Document Name" SortExpression="FileName" UniqueName="FileName" ReadOnly="true"
                        HeaderTooltip="This column contains the name of each uploaded document">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageDocument">
                        <HeaderStyle Width="110" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <a runat="server" id="ancManageDocument" title="Click here to view the document">View
                                Document</a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </infs:WclGrid>
    </div>
    <div class="col-md-12">
        <div class="row text-center">
            <infsu:CommandBar ID="fsucCmdExport" runat="server" ButtonPosition="Center" DisplayButtons="Extra"
                AutoPostbackButtons="Extra" ExtraButtonText="Export Document(s)" OnExtraClick="fsucCmdExport_ExportClick"
                UseAutoSkinMode="false" ButtonSkin="Silk">
            </infsu:CommandBar>
            <iframe id="ifrExportDocument" runat="server" height="0" width="0"></iframe>
        </div>
    </div>
</div>

<script type="text/javascript">

    //click on link button while double click on any row of grid.   
    function grd_rwDbClick(s, e) {
        var _id = "ancManageDocument";
        var b = e.get_gridDataItem().findElement(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
        //findElement findControl
    }

    function CheckAll(id) {
        var masterTable = $find("<%= grdAdditionalDocSearch.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectDocument").checked = isChecked; // for checking the checkboxes
            }
        }
    }
    function UnCheckHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdAdditionalDocSearch.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    } 
</script>
