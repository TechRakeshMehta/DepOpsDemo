<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdditionalDocumentsMapping.ascx.cs" Inherits="CoreWeb.ComplianceAdministration.UserControl.Views.AdditionalDocumentsMapping" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="swrap">
    <infs:WclGrid runat="server" ID="grdAdditionalDocuments" AllowPaging="True" PageSize="10" ShowExtraButtons="false"
        AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" OnNeedDataSource="grdAdditionalDocuments_NeedDataSource"
        OnInsertCommand="grdAdditionalDocuments_InsertCommand" OnDeleteCommand="grdAdditionalDocuments_DeleteCommand" OnItemDataBound="grdAdditionalDocuments_ItemDataBound"
        ShowAllExportButtons="False" NonExportingColumns="EditCommandColumn, DeleteColumn">
        <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
            Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
        </ExportSettings>
        <ClientSettings EnableRowHoverStyle="true">
            <Selecting AllowRowSelect="true"></Selecting>
        </ClientSettings>
        <MasterTableView CommandItemDisplay="Top" DataKeyNames="SystemDocMappingID">
            <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Map Addtional Documents"
                ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"></CommandItemSettings>
            <Columns>
                <telerik:GridBoundColumn DataField="DocumentFileName" FilterControlAltText="Filter File Name column"
                    HeaderText="File Name" SortExpression="DocumentFileName" UniqueName="DocumentFileName"
                    HeaderStyle-Width="130">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="IsOperational" FilterControlAltText="Filter IsOperational column"
                    HeaderText="Is Operational" SortExpression="IsOperational" UniqueName="IsOperational"
                    HeaderStyle-Width="130">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="SendToStudent" FilterControlAltText="Filter SendToStudent column"
                    HeaderText="Send To Student" SortExpression="SendToStudent" UniqueName="SendToStudent" ReadOnly="true"  HeaderStyle-Width="130">
                </telerik:GridBoundColumn>
                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete?"
                    Text="Delete" UniqueName="DeleteColumn">
                    <HeaderStyle Width="30px" />
                </telerik:GridButtonColumn>
            </Columns>
            <EditFormSettings EditFormType="Template">
                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                </EditColumn>
                <FormTemplate>
                    <div class="section">
                        <h1 class="mhdr">
                            <asp:Label ID="lblHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Map Addtional Documents" : "Edit Addtional Documents"%>'
                                runat="server"></asp:Label></h1>
                        <div class="content">
                            <div class="sxform auto">
                                <div class="msgbox">
                                    <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                </div>
                                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAddAddtionalDocument">
                                    <div class='sxro sx2co'>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblUserName" runat="server" AssociatedControlID="cmbAdditionalDocuments" Text="Select Documents" CssClass="cptn">                                                        
                                            </asp:Label><span class='reqd <%# (Container is GridEditFormInsertItem) ? "" : "nodisp" %>'>*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclComboBox ID="cmbAdditionalDocuments" runat="server" AutoPostBack="false" DataTextField="FileName" EmptyMessage="--Select--"
                                                CheckBoxes="true" EnableCheckAllItemsCheckBox="true" MaxHeight="200px" DataValueField="SystemDocumentID">
                                            </infs:WclComboBox>
                                            <div class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvHierPerUser" ControlToValidate="cmbAdditionalDocuments"
                                                    class="errmsg" ValidationGroup="grpValdAddDocuments" Display="Dynamic" ErrorMessage="User is required."
                                                    InitialValue="--SELECT--" />
                                            </div>
                                            <infs:WclTextBox Enabled="false" runat="server" Visible="false" ID="txtHierUser">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <infsu:CommandBar ID="fsucCmdBarMUser" runat="server" GridMode="true" DefaultPanel="pnlMUser" GridInsertText="Save" GridUpdateText="Save"
                                ExtraButtonIconClass="icnreset" ValidationGroup="grpValdAddDocuments" />
                        </div>
                    </div>
                </FormTemplate>
            </EditFormSettings>
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
        </MasterTableView>
    </infs:WclGrid>
</div>
<div class="gclr">
</div>

