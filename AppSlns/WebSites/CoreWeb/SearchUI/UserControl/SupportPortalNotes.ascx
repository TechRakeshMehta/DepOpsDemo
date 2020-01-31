<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupportPortalNotes.ascx.cs" Inherits="CoreWeb.SearchUI.Views.SupportPortalNotes" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/SearchUI/UserControl/NewApplicantProfileNotes.ascx" TagPrefix="uc"
    TagName="ApplicantProfileNotes" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxSupportPortalNotes">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="../Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Accessibility/Grid-Accessibility.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style>
    .headerFont {
        padding: 0 0 8px 0;
        margin: 0 !important;
        font-size: 133% !important;
        cursor: pointer;
        font-weight: bold;
    }
</style>
<div class="container-fluid">
    <asp:Panel ID="Panel1" runat="server">
        <div class="row">
            <div class="col-md-12" style="padding-left: 50px;">
                <h2 class="header-color">Portfolio Notes
                </h2>
            </div>
        </div>
        <uc:ApplicantProfileNotes ID="ucApplicantNotes" runat="server" Visible="true"></uc:ApplicantProfileNotes>
    </asp:Panel>
    <div class=" row col-md-12">&nbsp;</div>
    <asp:Panel ID="pnlBackgroundNotes" runat="server">
        <div class="row">
            <div class="col-md-12" style="padding-left: 50px;">
                <h2 class="header-color">Background Notes
                </h2>
            </div>
        </div>
        <div class="row allowscroll">
            <infs:WclGrid ID="grdBkgOrderNotes" runat="server" AutoGenerateColumns="false" AllowSorting="false" AutoSkinMode="true" CellSpacing="0" AllowPaging="true"
                GridLines="Both" ShowAllExportButtons="False" ShowExtraButtons="True" ShowClearFiltersButton="false" OnNeedDataSource="grdBkgOrderNotes_NeedDataSource" OnItemCreated="grdBkgOrderNotes_ItemCreated"
                OnInsertCommand="grdBkgOrderNotes_InsertCommand" OnUpdateCommand="grdBkgOrderNotes_UpdateCommand" OnDeleteCommand="grdBkgOrderNotes_DeleteCommand" OnItemDataBound="grdBkgOrderNotes_ItemDataBound" EnableAriaSupport="true">
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="NotesID,OrderID,OrderNumber" PagerStyle-Visible="false" AllowFilteringByColumn="false" AllowPaging="true">
                    <ItemStyle Wrap="true" />
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Note"
                        ShowExportToExcelButton="False" ShowExportToPdfButton="False" ShowExportToCsvButton="False"
                        ShowRefreshButton="False" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="OrderNumber" FilterControlAltText="Filter OrderNumber Text column"
                            HeaderText="Order Number" SortExpression="OrderNumber" AllowSorting="false" UniqueName="OrderNumber">
                            <ItemStyle Wrap="true" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Note" FilterControlAltText="Filter Note Text column"
                            HeaderText="Note Text" SortExpression="Note" AllowSorting="false" UniqueName="Note">
                            <ItemStyle Wrap="true" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserName" FilterControlAltText="Filter Created By column"
                            HeaderText="Created By" SortExpression="UserName" UniqueName="UserName">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="CreatedOn" FilterControlAltText="Filter Created On column"
                            HeaderText="Created On" SortExpression="CreatedOn" UniqueName="CreatedOn">
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedOn" runat="server" Text='<%# Convert.ToString(Convert.ToDateTime(Eval("CreatedOn")))  %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" Width="3%" />
                        </telerik:GridEditCommandColumn>--%>
                        <%-- <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this note?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" Width="3%" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>--%>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="container-fluid" runat="server" id="divEditBlock" visible="true">
                                <%-- <h1 class="mhdr">--%>
                                <div class="row">
                                    <div class="col-md-12">
                                        <h2 class="header-color">
                                            <asp:Label ID="lblTitleProfileNote" Text='<%# (Container is GridEditFormInsertItem) ? "Add Note " : "Update Note " %>'
                                                runat="server" />
                                            <span id="spnMessage" runat="server"></span>
                                        </h2>
                                    </div>
                                </div>
                                <div class="row bgLightGreen">
                                    <div class="msgbox">
                                        <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                    </div>
                                    <asp:Panel runat="server" ID="pnlSupportPortalNote">
                                        <infs:WclTextBox runat="server" Text='<%# Eval("NotesID") %>' ID="txtNotesID"
                                            Visible="false">
                                        </infs:WclTextBox>
                                        <infs:WclTextBox runat="server" Text='<%# Eval("OrderID") %>' ID="txtOrderID"
                                            Visible="false">
                                        </infs:WclTextBox>

                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3' title="Select an Order.">
                                                    <span class="cptn">Order Number</span><span class="reqd">*</span>
                                                    <infs:WclComboBox ID="cmbOrderNumber" runat="server" Filter="Contains" DataValueField="OrderID" DataTextField="OrderNumber"
                                                        OnClientKeyPressing="openCmbBoxOnTab" CssClass="form-control" AutoPostBack="false"
                                                        Skin="Silk" AutoSkinMode="false">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvOrderID" ControlToValidate="cmbOrderNumber"
                                                            InitialValue="" Display="Dynamic" ValidationGroup="grpValdNotesForm" CssClass="errmsg"
                                                            Text="Order Number is required." />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-6'>
                                                    <span class="cptn">Note</span><span class="reqd">*</span>
                                                    <infs:WclTextBox ID="txtNotes" Width="100%" TextMode="MultiLine" MaxLength="1012" CssClass="borderTextArea" runat="server" EnableAriaSupport="true" Text='<%# Eval("Note") %>'>
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvNote" ControlToValidate="txtNotes"
                                                            Display="Dynamic" class="errmsg" ErrorMessage="Note is required." ValidationGroup='grpValdNotesForm' />
                                                        <asp:RegularExpressionValidator runat="server" ID="revNote" ControlToValidate="txtNotes"
                                                            Display="Dynamic" CssClass="errmsg" ValidationExpression="^[&quot;\w\d\s\-\.\,\[\]\(\)\{\}\:\,\،\、\‒\–\—\―\…\!\‐\-\?\‘\’\“\”\'\'\;\\\<\>\/\~\@]*$" ValidationGroup='grpValdNotes'
                                                            ErrorMessage="Invalid character(s)." />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div class="col-md-12 text-right">
                                    <infsu:CommandBar ID="fsucCmdBarProfileNote" runat="server" GridMode="true" DefaultPanel="pnlSupportPortalNote" ValidationGroup="grpValdNotesForm"
                                        ExtraButtonIconClass="icnreset" GridInsertText="Save" GridUpdateText="Save" UseAutoSkinMode="false" ButtonSkin="Silk" />
                                </div>
                                <div class="col-md-12">&nbsp;</div>
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                </MasterTableView>
            </infs:WclGrid>
        </div>
    </asp:Panel>
</div>

