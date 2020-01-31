<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RotationDocuments.ascx.cs" Inherits="CoreWeb.ClinicalRotation.Views.RotationDocuments" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>


<infs:WclResourceManagerProxy runat="server" ID="rprxRotationDetails">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

</infs:WclResourceManagerProxy>

<div class="container-fluid">
    <div id="dvSection">
        <div class="row">
            <div class="col-md-12">
                <div id="modcmd_bar">
                    <div id="vermod_cmds">
                        <asp:LinkButton Text="Back to Rotation Details" runat="server" ID="lnkGoBack" OnClick="lnkGoBack_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <div class="msgbox">
                <asp:Label ID="lblInfoMsg" runat="server" CssClass="info"></asp:Label>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <h1 class="header-color">Rotation Document(s)</h1>
        </div>
    </div>
    <div class="row bgLightGreen">
        <div class='col-md-12'>
            <div class="row">
                <div class='form-group col-md-3'>
                    <span class="cptn">Requirement Category</span><span class='reqd'>*</span>
                    <infs:WclComboBox ID="ddlReqCat" runat="server" DataTextField="RequirementCategoryName" AutoPostBack="false"
                        EmptyMessage="--SELECT--" DataValueField="RequirementCategoryID" Filter="Contains" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                        OnClientKeyPressing="openCmbBoxOnTab" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                    </infs:WclComboBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvReqCat" ControlToValidate="ddlReqCat"
                            InitialValue="" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                            Text="Requirement Category is required." />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h1 class="header-color">Rotation Members</h1>
        </div>
    </div>
    <div class="row bgLightGreen">
        <div class='col-md-12'>
            <div class="row">
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
                <div class='form-group col-md-3' title="Restrict search results to the entered email address">
                    <span class="cptn">Email Address</span>
                    <infs:WclTextBox ID="txtEmail" runat="server" Width="100%" CssClass="form-control">
                    </infs:WclTextBox>
                </div>

                <div class='form-group col-md-3' title="Restrict search results to the entered SSN or ID Number">
                    <span class="cptn">SSN/ID Number</span>
                    <infs:WclMaskedTextBox runat="server" ID="txtSSN" Mask="aaa-aa-aaaa" Width="100%"
                        CssClass="form-control" />
                </div>
            </div>
        </div>
        <div class='col-md-12'>
            <div class="row">
                <div class='form-group col-md-3' title="Restrict search results to the entered Date of Birth">
                    <span class="cptn">Date of Birth</span>
                    <infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="Select a date"
                        DateInput-DateFormat="MM/dd/yyyy" Width="100%" CssClass="form-control">
                    </infs:WclDatePicker>
                </div>
            </div>
        </div>
        <div class='col-md-12 text-center'>
            <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                AutoPostbackButtons="Submit,Save,Cancel,Clear" SubmitButtonIconClass="rbUndo"
                SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
                CancelButtonText="Cancel" OnSaveClick="fsucCmdBarButton_SearchClick" OnSubmitClick="fsucCmdBarButton_ResetClick"
                OnCancelClick="fsucCmdBarButton_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk">
            </infsu:CommandBar>
        </div>
    </div>
    <div class='row'>
        <infs:WclGrid runat="server" ID="grdApplicants" AllowCustomPaging="true"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            EnableLinqExpressions="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            ShowClearFiltersButton="false"
            OnNeedDataSource="grdApplicants_NeedDataSource" OnItemCommand="grdApplicants_ItemCommand"
            OnSortCommand="grdApplicants_SortCommand" OnItemDataBound="grdApplicants_ItemDataBound"
            NonExportingColumns="AssignItems,SSN">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserId"
                AllowFilteringByColumn="false">

                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="AssignItems" HeaderTooltip="Click this box to select all users on the active page"
                        AllowFiltering="false" ShowFilterIcon="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAllApplicants" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectApplicant" runat="server" CssClass="uncheck" OnCheckedChanged="chkSelectApplicant_CheckedChanged"
                                onclick='<% # "UnCheckHeader(this,\"" + Eval("OrganizationUserId") + "\" );"%>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="ApplicantFirstName" HeaderText="Applicant First Name"
                        SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName" HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantLastName" HeaderText="Applicant Last Name"
                        SortExpression="ApplicantLastName" UniqueName="ApplicantLastName" HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="EmailAddress" HeaderText="Email Address" SortExpression="EmailAddress"
                        UniqueName="EmailAddress" HeaderTooltip="This column displays the applicant's email address for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="DateOfBirth" HeaderText="Date of Birth" SortExpression="DateOfBirth"
                        UniqueName="DateOfBirth" HeaderTooltip="This column displays the applicant's date of birth for each record in the grid"
                        DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN"
                        UniqueName="SSN" HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN"
                        Display="false"
                        UniqueName="_SSN" HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid">
                    </telerik:GridBoundColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                    Position="TopAndBottom" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
</div>

<div class='col-md-12 text-center' style="padding-top: 20px;">
    <infsu:CommandBar ID="cmdBarViewDoc" runat="server" ButtonPosition="Center" DisplayButtons="Submit"
        AutoPostbackButtons="Submit" SubmitButtonIconClass="rbSearch"
        SubmitButtonText="View Document(s)" OnSubmitClick="cmdBarViewDoc_SubmitClick"
        ValidationGroup="grpFormSubmit" UseAutoSkinMode="false" ButtonSkin="Silk">
    </infsu:CommandBar>
</div>

<div class="container-fluid">
    <div class='row'>
        <infs:WclGrid runat="server" ID="grdDocuments" AllowCustomPaging="true"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            EnableLinqExpressions="false" AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            ShowClearFiltersButton="false"
            OnNeedDataSource="grdDocuments_NeedDataSource" OnItemCommand="grdDocuments_ItemCommand"
            OnSortCommand="grdDocuments_SortCommand" OnItemDataBound="grdDocuments_ItemDataBound"
            NonExportingColumns="DocumentView">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ApplicantDocumentID,ReqCatID"
                AllowFilteringByColumn="false">

                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="AssignItems" HeaderTooltip="Click this box to select all users on the active page"
                        AllowFiltering="false" ShowFilterIcon="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAllDocuments" runat="server" onclick="CheckAllDocuments(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectDocument" runat="server" CssClass="uncheck" OnCheckedChanged="chkSelectDocument_CheckedChanged"
                                onclick='<% # "UnCheckHeaderDocuments(this,\"" + Eval("ApplicantDocumentID") + "\" );"%>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="FirstName" HeaderText="Applicant First Name"
                        SortExpression="FirstName" UniqueName="ApplicantFirstName" HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="LastName" HeaderText="Applicant Last Name"
                        SortExpression="LastName" UniqueName="ApplicantLastName" HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ReqCatName" HeaderText="Requirement Category" SortExpression="ReqCatName"
                        UniqueName="ReqCatName" HeaderTooltip="This column displays the requirement category for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName"
                        UniqueName="DocumentName" HeaderTooltip="This column displays the document name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DocumentView" HeaderText="Download" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnView" ButtonType="LinkButton" CommandName="DocumentView" Visible="true"
                                ToolTip="Click here to download document." runat="server" Text="View Document">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                    Position="TopAndBottom" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
</div>

<div class='col-md-12 text-center' style="padding-top: 20px;">
    <infsu:CommandBar ID="cmdDownloadDoc" runat="server" ButtonPosition="Center" DisplayButtons="Submit"
        AutoPostbackButtons="Submit" SubmitButtonIconClass=""
        SubmitButtonText="Download Document(s)" OnSubmitClick="cmdDownloadDoc_SubmitClick"
        ValidationGroup="grpFormSubmit" UseAutoSkinMode="false" ButtonSkin="Silk">
    </infsu:CommandBar>
</div>

<iframe id="ifrExportDocument" runat="server" height="0" width="0"></iframe>

<script type="text/javascript">

  
    function CheckAll(id) {
        var masterTable = $find("<%= grdApplicants.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectApplicant").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectApplicant").checked = isChecked; // for checking the checkboxes
                //var studentId = row[i].getDataKeyValue("OrganizationUserId");
                //AddStudentIDsInList(isChecked, studentId);
            }
        }
    }

    function UnCheckHeader(id, studentId) {
        var checkHeader = true;
        var masterTable = $find("<%= grdApplicants.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        //AddStudentIDsInList(id.checked, studentId);
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectApplicant").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectApplicant").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAllApplicants]')[0].checked = checkHeader;
    }


    function CheckAllDocuments(id) {
        var masterTable = $find("<%= grdDocuments.ClientID %>").get_masterTableView();
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

    function UnCheckHeaderDocuments(id, studentId) {
        var checkHeader = true;
        var masterTable = $find("<%= grdDocuments.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAllDocuments]')[0].checked = checkHeader;
    }
</script>
