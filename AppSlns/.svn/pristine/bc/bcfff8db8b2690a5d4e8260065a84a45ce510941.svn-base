<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Announcement.ascx.cs" Inherits="CoreWeb.CommonOperations.Views.Announcement" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr">Announcements</h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdAnnouncement" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0" OnNeedDataSource="grdAnnouncement_NeedDataSource"
                EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false" OnItemDataBound="grdAnnouncement_ItemDataBound"
                OnItemCommand="grdAnnouncement_ItemCommand" GridLines="both" EnableLinqExpressions="false" NonExportingColumns="EditCommandColumn,DeleteColumn">
                <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="AnnouncementID" AllowFilteringByColumn="True">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Announcement" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="AnnouncementName" FilterControlAltText="Filter AnnouncementName column"
                            HeaderText="Announcement Name" SortExpression="AnnouncementName" UniqueName="AnnouncementName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AnnouncementText" FilterControlAltText="Filter AnnouncementText column"
                            HeaderText="Announcement Text" SortExpression="AnnouncementText" UniqueName="AnnouncementText">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Record?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section" id="divAddForm" runat="server" visible="true">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblRotation" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Announcement" : "Update Announcement" %>'
                                        runat="server" />
                                </h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlItem">
                                            <div class="msgbox">
                                                <asp:Label ID="lblGridMessage" runat="server">
                                                </asp:Label>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Announcement Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtAnnouncementName" MaxLength="50">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvItemName" ControlToValidate="txtAnnouncementName"
                                                            class="errmsg" Display="Dynamic" ValidationGroup="grpFormSubmit" ErrorMessage="Announcement Name is required." />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>

                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Announcement Text</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm m3spn'>
                                                    <infs:WclEditor ID="rdEditorNotes" ClientIDMode="Static" runat="server" Width="99.5%"
                                                        ToolsFile="~/Templates/Data/Tools.xml" EnableResize="false" OnClientLoad="OnClientLoad">
                                                    </infs:WclEditor>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvContent" ControlToValidate="rdEditorNotes"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Announcement Text is required."
                                                            ValidationGroup="grpFormSubmit" />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarPermission" runat="server" GridMode="true" DefaultPanel="pnlItem" GridInsertText="Save" GridUpdateText="Save"
                                        ValidationGroup="grpFormSubmit" />
                                </div>
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
    </div>
</div>

<script>
    function OnClientLoad(editor, args) {
        $jQuery('ul.reToolbar').width('auto');
    }

</script>
