<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicantProfileNotes.ascx.cs"
    Inherits="CoreWeb.SearchUI.Views.ApplicantProfileNotes" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<style>
    /*.BreakGridWord {
        word-wrap: break-word !important;
        word-break: break-all !important;
    }*/
    .headerFont
    {
        padding: 0 0 8px 0;
        margin: 0 !important;
        font-size: 133% !important;
        cursor: pointer;
        font-weight: bold;
    }
</style>

<%--<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>
<div id="divNewNotes" runat="server">
    <h1 class="mhdr">
        <asp:Label ID="lblNewHeading" Text='Add New Note'
            runat="server"></asp:Label></h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblContactSuccess" runat="server"></asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlMNew">

                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblNewNote" Text="Note" runat="server"
                            CssClass="cptn" />
                    </div>
                    <div class='sxlm m2spn'>
                        <infs:WclTextBox ID="txtNewNote" Width="100%" runat="server" TextMode="MultiLine"
                            Height="80px">
                        </infs:WclTextBox>
                        <div class='vldx'>
                            <asp:RegularExpressionValidator runat="server" ID="revNewNote" ControlToValidate="txtNewNote"
                                Display="Dynamic" CssClass="errmsg" ValidationExpression="^[&quot;\w\d\s\-\.\,\[\]\(\)\{\}\:\,\،\、\‒\–\—\―\…\!\‐\-\?\‘\’\“\”\'\'\;\\\<\>\/\~\@]{1,500}$" ValidationGroup='grpValdNotes'
                                ErrorMessage="Invalid character(s)." />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <div id="divBtnAdd" runat="server" style="text-align: center">
        <infs:WclButton runat="server" ID="btnAdd" Text="Add Note" OnClick="CmdBarCancel_Click" ValidationGroup="grpValdNotes">
            <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                PrimaryIconWidth="14" />
        </infs:WclButton>
    </div>
</div>--%>

<%--<div class="section">--%>
<%--<h1 id="hdrNotes" runat="server" class="mhdr">Notes
</h1>--%>

<div class="row">
    <div class="col-md-12">
        <div class="msgbox" tabindex="0">
            <asp:Label ID="Label1" runat="server" CssClass="info">
            </asp:Label>
        </div>
    </div>
</div>
<div id="Div2" class="row" runat="server">
    <infs:WclGrid runat="server" ID="grdNotes" AutoGenerateColumns="false"
        AllowSorting="false" AutoSkinMode="True" CellSpacing="0" EnableAriaSupport="true"
        GridLines="Both" ShowAllExportButtons="False" ShowExtraButtons="True" ShowClearFiltersButton="false" OnItemDataBound ="grdNotes_ItemDatabound" 
        OnItemCreated="grdNotes_ItemCreated" OnNeedDataSource="grdNotes_NeedDataSource"
        OnInsertCommand="grdNotes_InsertCommand" OnUpdateCommand="grdNotes_UpdateCommand"
        OnDeleteCommand="grdNotes_DeleteCommand" AllowPaging="false">
        <MasterTableView CommandItemDisplay="Top" DataKeyNames="APN_ID,TempId" PagerStyle-Visible="false"
            AllowFilteringByColumn="false" AllowPaging="false">
            <ItemStyle Wrap="true" />
            <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Note"
                ShowExportToExcelButton="False" ShowExportToPdfButton="False" ShowExportToCsvButton="False"
                ShowRefreshButton="False" />
            <Columns>
                <telerik:GridBoundColumn DataField="APN_ProfileNote" FilterControlAltText="Filter Node Text column"
                    ItemStyle-Width="150px"
                    HeaderText="Note Text" SortExpression="APN_ProfileNote" AllowSorting="false"
                    UniqueName="APN_ProfileNote">
                    <ItemStyle Wrap="true" Width="70%" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="CreatedBy" FilterControlAltText="Filter Created By column"
                    HeaderText="Created By" SortExpression="CreatedBy" UniqueName="CreatedBy">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn DataField="APN_CreatedOn" FilterControlAltText="Filter Created On column"
                    HeaderText="Created On" SortExpression="APN_CreatedOn" UniqueName="APN_CreatedOn">
                    <ItemTemplate>
                        <asp:Label ID="lblCreatedOn" runat="server" Text='<%# Convert.ToString(Convert.ToDateTime(Eval("APN_CreatedOn")))  %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <%--Start UAT-5052--%>
                <telerik:GridTemplateColumn DataField="APN_IsVisibleToClientAdmin" Visible="false" SortExpression="APN_IsVisibleToClientAdmin"
                    HeaderText="Visible to Client Admin" UniqueName="APN_IsVisibleToClientAdmin"> 
                   <%-- <ItemTemplate>
                        <asp:Label ID="lblVisibleClientAdmin" runat="server" Text='<%# Eval("APN_IsVisibleToClientAdmin") %>' />
                    </ItemTemplate>--%>
                </telerik:GridTemplateColumn>
                <%--End UAT-5052--%>
                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this note?"
                    Text="Delete" UniqueName="DeleteColumn" ImageUrl="~/Resources/Mod/Dashboard/images/CancelGrid.gif"> 
                    <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                </telerik:GridButtonColumn>
                <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="~/Resources/Mod/Dashboard/images/editGrid.gif"
                    UniqueName="EditCommandColumn">
                    <HeaderStyle CssClass="tplcohdr" />
                    <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                </telerik:GridEditCommandColumn> 
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
                                    <asp:Label ID="lblTitleProfileNote"  Text='<%# (Container is GridEditFormInsertItem) ? "Add Note " : "Update Note " %>'
                                        runat="server" />
                                    <span id="spnMessage" runat="server">- Please note that the notes will not be retained
                                        unless you click update button at the bottom of the page. </span>
                                </h2>
                            </div>
                        </div>
                        <div class="row bgLightGreen">
                            <div class="msgbox">
                                <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                            </div>
                            <asp:Panel runat="server" ID="pnlProfileNote">

                                <infs:WclTextBox runat="server" Text='<%# Eval("APN_ID") %>' ID="txtAPN_ID"
                                    Visible="false" Width="100%" CssClass="form-control">
                                </infs:WclTextBox>
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class='form-group col-md-6'>
                                            <span class="cptn">Note</span><span class="reqd">*</span>
                                            <infs:WclTextBox ID="txtProfileNote" Width="100%" CssClass="borderTextArea" TextMode="MultiLine"
                                                runat="server" Text='<%# Eval("APN_ProfileNote") %>'>
                                            </infs:WclTextBox>
                                            <div class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvNewNote" ControlToValidate="txtProfileNote"
                                                    Display="Dynamic" class="errmsg" ErrorMessage="Note is required." ValidationGroup='grpValdNotes' />
                                                <asp:RegularExpressionValidator runat="server" ID="revNewNote" ControlToValidate="txtProfileNote"
                                                    Display="Dynamic" CssClass="errmsg" ValidationExpression="^[&quot;\w\d\s\-\.\,\[\]\(\)\{\}\:\,\،\、\‒\–\—\―\…\!\‐\-\?\‘\’\“\”\'\'\;\\\<\>\/\~\@]*$"
                                                    ValidationGroup='grpValdNotes'
                                                    ErrorMessage="Invalid character(s)." />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="col-md-12 text-right">
                            <infsu:CommandBar ID="fsucCmdBarProfileNote" runat="server" GridMode="true" DefaultPanel="pnlProfileNote"
                                ValidationGroup="grpValdNotes" ExtraButtonIconClass="icnreset" GridInsertText="Save"
                                GridUpdateText="Save" UseAutoSkinMode="false" ButtonSkin="Silk" />
                        </div>
                        <div class="col-md-12">&nbsp;</div>
                    </div>
                </FormTemplate>
            </EditFormSettings>
        </MasterTableView>
    </infs:WclGrid>
</div>
<%--</div>--%>
