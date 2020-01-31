<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientContactProfile.ascx.cs" Inherits="CoreWeb.ClinicalRotation.Views.ClientContactProfile" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%--<%@ Register Src="~/ClinicalRotation/UserControl/ManageSharedUserDocument.ascx" TagPrefix="infsu1" TagName="UploadDocument" %>--%>

<infs:WclResourceManagerProxy runat="server" ID="rprxClientContactProfile">
    <infs:LinkedResource Path="~/Resources/Mod/ClinicalRotation/ClientContact.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Instructor/Preceptor User Details</h2>
        </div>
    </div>
    <div class="row">&nbsp;</div>
    <div class="row bgLightGreen">
        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlClientContactProfile">
            <div class="col-md-12">&nbsp;</div>
            <div class="col-md-12" style="display: none;">
                <div class="row">
                    <div class="form-group col-md-3">
                        <span class="cptn">Institution</span><span class="reqd">*</span>
                        <infs:WclComboBox ID="cmbTenant" runat="server" Width="100%" AutoSkinMode="false" Skin="Silk" CssClass="form-control" AutoPostBack="true" OnDataBound="cmbTenant_DataBound" DataTextField="TenantName"
                            OnSelectedIndexChanged="cmbTenant_SelectedIndexChanged" DataValueField="TenantID" Filter="None" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>


                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class="form-group col-md-3" title="The Email Address of this client user">
                        <span class="cptn">Email Address</span>
                        <infs:WclTextBox runat="server" Width="100%" CssClass="form-control" ID="txtEmail" MaxLength="50" Enabled="false">
                        </infs:WclTextBox>
                    </div>
                    <div class="form-group col-md-3" title="The first name of this client user">
                        <span class="cptn">First Name</span><span class="reqd">*</span>
                        <infs:WclTextBox runat="server" ID="txtFirstName" MaxLength="256" Width="100%" CssClass="form-control" Enabled="true">
                        </infs:WclTextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName"
                            Display="Dynamic" CssClass="errmsg" Text="First name is required." ValidationGroup="grpFormSubmit" />
                    </div>
                    <div class="form-group col-md-3" title="The middle name of this client user">
                        <span class="cptn">Middle Name</span>
                        <infs:WclTextBox runat="server" ID="txtMiddleName" Width="100%" CssClass="form-control" MaxLength="50" Enabled="true">
                        </infs:WclTextBox>
                    </div>
                    <div class="form-group col-md-3" title="The last name of this client user">
                        <span class="cptn">Last Name</span><span class="reqd">*</span>
                        <infs:WclTextBox runat="server" ID="txtLastName" Width="100%" CssClass="form-control" MaxLength="50" Enabled="true">
                        </infs:WclTextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                            Display="Dynamic" CssClass="errmsg" Text="Last name is required." ValidationGroup="grpFormSubmit" />
                    </div>

                </div>
                <div class="row">
                    <div class="form-group col-md-3" title="The SSN/ID of this client user">
                        <span class="cptn">Last 4 Digits of SSN</span>
                        <infs:WclMaskedTextBox runat="server" ID="txtSSN" Mask="####" AutoPostBack="false" Width="100%" />
                        <asp:RegularExpressionValidator ID="revSSN" ControlToValidate="txtSSN" ValidationExpression="^\d{4}$" runat="server"
                            ValidationGroup="grpFormSubmit" Display="Dynamic" CssClass="errmsg" Text="Please enter last 4 digits of SSN."></asp:RegularExpressionValidator>
                    </div>

                    <div class="form-group col-md-3" title="Redirect to Change Password">
                        <span class='cptn'><%=Resources.Language.CHGEPASSWRD %></span>
                        <asp:HyperLink ID="lnkChangePassword" runat="server" Visible="true" CssClass="user" Text="<%$Resources:Language,CHGEPASSWRD %>"> </asp:HyperLink>
                    </div>
                </div>
            </div>

        </asp:Panel>
    </div>
    <infsu:CommandBar ID="fsucCmdBar" runat="server" ButtonPosition="Center" DisplayButtons="Save,Cancel,Clear" DefaultPanel="pnlClientContactProfile" UseAutoSkinMode="false" ButtonSkin="Silk"
        AutoPostbackButtons="Save,Cancel,Clear" CancelButtonText="Cancel" OnCancelClick="fsucCmdBar_CancelClick" ValidationGroup="grpFormSubmit" SaveButtonIconClass="rbSave"
        ClearButtonText="Link Account" OnClearClick="fsucCmdBar_ClearClick"
        SaveButtonText="Update Profile" OnSaveClick="fsucCmdBar_SaveClick">
    </infsu:CommandBar>
</div>
<%--Upload Document Control--%>
<%--<infsu1:UploadDocument ID="ucUploadDocument" runat="server"></infsu1:UploadDocument>--%>
<div class="container-fluid" style="display: none">
    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Syllabus Document(s) </h2>
        </div>
    </div>
    <div class="row">

        <infs:WclGrid runat="server" ID="grdRotationDocuments" AllowPaging="true" Visible="false"
            PageSize="10" AutoGenerateColumns="False" AllowSorting="True" EnableDefaultFeatures="true" GridLines="Both" ShowClearFiltersButton="false"
            MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false"
            GridViewMode="AutoAddOnly" OnItemDataBound="grdRotationDocuments_ItemDataBound" OnNeedDataSource="grdRotationDocuments_NeedDataSource" OnItemCommand="grdRotationDocuments_ItemCommand"
            ShowAllExportButtons="false">
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="DocumentID,ComplioID" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                    ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                <Columns>
                    <telerik:GridBoundColumn DataField="FileName" FilterControlAltText="Filter DocumentName column"
                        HeaderText="Document Name" SortExpression="FileName" UniqueName="DocumentName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationName" FilterControlAltText="Filter RotationName column"
                        HeaderText="Rotation Name" SortExpression="RotationName" UniqueName="RotationName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Department" FilterControlAltText="Filter Department column"
                        HeaderText="Department" SortExpression="Department" UniqueName="Department">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Program" FilterControlAltText="Filter Program column"
                        HeaderText="Program" SortExpression="Program" UniqueName="Program">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Course" FilterControlAltText="Filter Course column"
                        HeaderText="Course" SortExpression="Course" UniqueName="Course">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn UniqueName="ViewDocument">
                        <ItemTemplate>
                            <asp:LinkButton Visible="true" Text="View Document" runat="server" ID="lnkRotationDoc"></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle CssClass="tplcohdr" />
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </infs:WclGrid>
    </div>
    <div class="row">&nbsp;</div>
    <div class="row">&nbsp;</div>
    <div class="row">&nbsp;</div>
</div>

<asp:HiddenField ID="hfTenantId" runat="server" />



<script src="../Resources/Mod/Dashboard/Scripts/bootstrap.min.js" type="text/javascript"></script>
