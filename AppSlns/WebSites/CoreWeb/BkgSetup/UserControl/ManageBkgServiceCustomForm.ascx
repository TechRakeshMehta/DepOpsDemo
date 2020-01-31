<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageBkgServiceCustomForm.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageBkgServiceCustomForm" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblServiceName" runat="server"></asp:Label></h1>
    <div class="content">
        <infs:WclGrid runat="server" ID="grdCusfomFormmapping" AllowPaging="True"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" AutoGenerateColumns="False"
            AllowFilteringByColumn="false" AllowSorting="True"
            ShowAllExportButtons="False" ShowClearFiltersButton="false" OnNeedDataSource="grdCusfomFormmapping_NeedDataSource" OnInsertCommand="grdCusfomFormmapping_InsertCommand" OnItemDataBound="grdCusfomFormmapping_ItemDataBound" OnDeleteCommand="grdCusfomFormmapping_DeleteCommand">
            <ExportSettings Pdf-PageWidth="300mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="CustomFormID,SvcFormMappingID" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Mapping"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"></CommandItemSettings>
                <Columns>
                    <telerik:GridBoundColumn DataField="CustomFormName" HeaderText="Name"
                        SortExpression="CustomFormName" UniqueName="CustomFormName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CustomFormDesc" HeaderText="Description"
                        SortExpression="CustomFormDesc" UniqueName="CustomFormDesc">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="" FilterControlAltText="Filter IsActive column"
                        HeaderText="" SortExpression="" UniqueName="" Visible="false">
                        <ItemTemplate>
                            <%-- <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("IsEditable"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>--%>
                            <%-- <asp:HiddenField ID="hdnfIsEditable" runat="server" Value='<%#Eval("IsEditable")%>' />--%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn" Display="false">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                    <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Custom Form Mapping?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div class="section" visible="true" id="divEditFormBlock" runat="server">
                            <h1 class="mhdr">
                                <asp:Label ID="lblEHServiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Mapping" : "Update Mapping" %>'
                                    runat="server" /></h1>
                            <div class="content">
                                <div class="sxform auto">
                                    <div class="msgbox">
                                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                    </div>
                                    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlCustomForms">
                                        <div class='sxro sx3co'>
                                            <div class='sxlb'>
                                                <span class="cptn">Custom form</span><span class="reqd">*</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclComboBox ID="ddlCustomForm" runat="server" CheckBoxes="true"
                                                    DataTextField="CF_Name" DataValueField="CF_ID" EmptyMessage="--Select--">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvCustomForm" ControlToValidate="ddlCustomForm"
                                                        Display="Dynamic" CssClass="errmsg" Text="Custom Form is required." ValidationGroup="grpFormSubmit" />
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </div>

                                    </asp:Panel>
                                </div>
                                <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlCategory"
                                    ValidationGroup="grpFormSubmit" GridInsertText="Save" GridUpdateText="Save" ExtraButtonIconClass="icnreset" />
                            </div>
                        </div>
                    </FormTemplate>
                </EditFormSettings>

                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
        </infs:WclGrid>
        <div class="gclr">
        </div>
    </div>
</div>

<div style="width: 100%; text-align: center" id="dvShowBackLink" runat="server">
    <infs:WclButton runat="server" ID="btnGoBack" Text="Go Back To Service Queue" OnClick="CmdBarCancel_Click">
    </infs:WclButton>
</div>
