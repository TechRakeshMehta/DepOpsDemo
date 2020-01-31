<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.ManagePermissionType" Codebehind="ManagePermissionType.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblManagePermissionType" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdPermissionType" DataSourceID="" AllowPaging="True"
                PageSize="10" AutoGenerateColumns="False" AllowSorting="True" GridLines="Both"
                OnNeedDataSource="grdPermissionType_NeedDataSource" OnDeleteCommand="grdPermissionType_DeleteCommand"
                OnInsertCommand="grdPermissionType_InsertCommand" OnUpdateCommand="grdPermissionType_UpdateCommand"
                OnItemCommand="grdPermissionType_ItemCommand" NonExportingColumns="EditCommandColumn, DeleteColumn">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="PermissionTypeId">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Permission Type" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="Name" HeaderText="Permission Type">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description" HeaderText="Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                            <HeaderStyle Width="30px" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Permission Type?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle Width="30px" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings UserControlName="UserControl\PermissionTypeEdit.ascx" EditFormType="WebUserControl">
                        <EditColumn UniqueName="EditCommandColumnManagePermissionType">
                        </EditColumn>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat="{4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
           </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>
