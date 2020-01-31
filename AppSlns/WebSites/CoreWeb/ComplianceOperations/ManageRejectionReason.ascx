<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageRejectionReason.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ManageRejectionReason" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>


<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Scripts/bootstrap.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style type="text/css">
    .resizetxtbox {
        resize: vertical !important;
        width: 100% !important;
        height: 100%!important;
        border-width: 1px !important;
        border-color: #6788be !important;
    }
       .textBoxHeight {
        height:80px !important;
    }
</style>
<div id="dvTop" class="container-fluid">

    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Manage Rejection Reason
            </h2>
        </div>
    </div>

    <div class="row allowscroll">
        <infs:WclGrid runat="server" ID="grdRejectionReason" AllowPaging="True" PageSize="10" AutoSkinMode="true" AllowFilteringByColumn="true"
            AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" ShowAllExportButtons="False"
            NonExportingColumns="EditCommandColumn, DeleteColumn" ValidationGroup="grpValdInstitutionNodeType"
            OnNeedDataSource="grdRejectionReason_NeedDataSource" OnItemCommand="grdRejectionReason_ItemCommand"
            OnInsertCommand="grdRejectionReason_InsertCommand" OnUpdateCommand="grdRejectionReason_UpdateCommand"
            OnDeleteCommand="grdRejectionReason_DeleteCommand" OnItemCreated="grdRejectionReason_ItemCreated">
            <ExportSettings Pdf-PageWidth="300mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="RR_ID">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Rejection Reason"
                    ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"></CommandItemSettings>
                <Columns>
                    <telerik:GridBoundColumn DataField="RR_Name" FilterControlAltText="Filter RR_Name column"
                        HeaderText="Name" SortExpression="RR_Name" UniqueName="RR_Name">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RR_ReasonText" FilterControlAltText="Filter RR_ReasonText column"
                        HeaderText="Reason Text" SortExpression="RR_ReasonText" UniqueName="RR_ReasonText">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="lkpRejectionReasonCategory.LRRC_Name" FilterControlAltText="Filter Category column"
                        HeaderText="Category" SortExpression="lkpRejectionReasonCategory.LRRC_Name" UniqueName="Category">
                    </telerik:GridBoundColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif" UniqueName="EditCommandColumn">
                        <HeaderStyle Width="30px" />
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridEditCommandColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif" CommandName="Delete" ConfirmText="Are you sure you want to delete this Rejection Reason?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <HeaderStyle Width="30px" />
                    </telerik:GridButtonColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12">
                                    <h2 class="header-color">
                                        <asp:Label ID="lblTitleRejectionReason" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Rejection Reason" : "Update Rejection Reason" %>'
                                            runat="server" />
                                    </h2>
                                </div>
                            </div>


                            <asp:Panel ID="pnlEditForm" runat="server">
                                <div class="row bgLightGreen">
                                    <div class="col-md-12">
                                        <infs:WclTextBox runat="server" Text='<%# Eval("RR_ID") %>' ID="txtInstitutionNodeTypeId"
                                            Visible="false">
                                        </infs:WclTextBox>
                                        <div class="row">
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Name</span><span class="reqd">*</span>
                                                <infs:WclTextBox ID="txtName" Width="100%" runat="server" Text='<%# Eval("RR_Name") %>'
                                                    MaxLength="200">
                                                </infs:WclTextBox>
                                                <div id="Div1" class='vldx'>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                                        class="errmsg" ErrorMessage="Name is required." ValidationGroup='grpValdRejectionReason'
                                                        Enabled="true" />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Category</span><span class="reqd">*</span>
                                                <infs:WclComboBox ID="cmbReasonCategory" runat="server" MarkFirstMatch="true" Width="100%" AutoPostBack="false"
                                                    DataTextField="LRRC_Name" CssClass="form-control" Skin="Silk" AutoSkinMode="false" DataValueField="LRRC_ID" />
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvReasonCategory" ControlToValidate="cmbReasonCategory"
                                                        Display="Dynamic" InitialValue="--SELECT--" CssClass="errmsg" ErrorMessage="Category is required."
                                                        ValidationGroup="grpValdRejectionReason" />
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class='form-group col-md-4'>
                                                <span class="cptn">Reason Text</span><span class="reqd">*</span>

                                                <infs:WclTextBox ID="txtReasonText" Width="100%" runat="server" Text='<%# Eval("RR_ReasonText") %>'
                                                    MaxLength="2048" TextMode="MultiLine" CssClass="textBoxHeight">
                                                </infs:WclTextBox>
                                                <div id="dvLabel" class='vldx'>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvLabel" ControlToValidate="txtReasonText"
                                                        class="errmsg" ErrorMessage="Reason Text is required." ValidationGroup='grpValdRejectionReason'
                                                        Enabled="true" />
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <infsu:CommandBar ID="fsucCmdBarPriceAdjustment" runat="server" GridMode="true" DefaultPanel="pnlPriceAdjustment" GridInsertText="Save" GridUpdateText="Save"
                                ValidationGroup="grpValdRejectionReason" UseAutoSkinMode="false" ButtonSkin="Silk" />

                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
        </infs:WclGrid>
    </div>
    <div class="gclr">
    </div>
</div>


