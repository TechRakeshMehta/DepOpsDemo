<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttributeDocumentFieldMapping.ascx.cs" Inherits="CoreWeb.ComplianceAdministration.Views.AttributeDocumentFieldMapping" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<style type="text/css">
    .breakword
    {
        word-break: break-all;
    }

    .rgFilterRow
    {
        display: none !important;
    }
</style>
<div class="msgbox">
    <asp:Label ID="lblMessage" runat="server" CssClass="info">
    </asp:Label>
</div>
<div class="section">
    <h1 class="mhdr">Attribute Document Field Mapping
    </h1>
    <div class="content">
        <div class="sxform auto">
        </div>
        <div id="Div1" class="swrap" runat="server">
            <infs:WclGrid runat="server" ID="grdAttributeFieldMapping" AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowClearFiltersButton="false"
                EnableLinqExpressions="false" AllowSorting="True" AutoSkinMode="True" CellSpacing="0" OnNeedDataSource="grdAttributeFieldMapping_NeedDataSource" OnItemDataBound="grdAttributeFieldMapping_ItemDataBound"
                GridLines="Both" ShowAllExportButtons="False" ShowExtraButtons="True" EnableDefaultFeatures="true" OnUpdateCommand="grdAttributeFieldMapping_UpdateCommand">
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="DFM_ID">
                    <CommandItemSettings ShowAddNewRecordButton="false"
                        ShowExportToExcelButton="False" ShowExportToPdfButton="False" ShowExportToCsvButton="False"
                        ShowRefreshButton="true" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="DFM_FieldName" FilterControlAltText="Filter Field Name column"
                            HeaderText="Field Name" SortExpression="DFM_FieldName" UniqueName="DFM_FieldName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="lkpDocumentFieldType_.DFT_Name" FilterControlAltText="Filter FieldType column"
                            HeaderText="Field Type" SortExpression="Attribute" UniqueName="DFT_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div id="Div2" class="section" visible="true" runat="server">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblEvService" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Attribute Document Field Mapping" : "Update Attribute Document Field Mapping" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlMapping">
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Field Name</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:Label ID="txtFieldName" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: Eval("DFM_FieldName") %>'></asp:Label>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Field Type</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbFieldType" ClientIDMode="Static" DataTextField="DFT_Name" DataValueField="DFT_ID" runat="server"></infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvcmbFieldType" ControlToValidate="cmbFieldType" InitialValue="--SELECT--" Enabled="false"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Field Type is required." />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                            <div class="sxroend"></div>
                            <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlExternalVendor" DisplayButtons="Save" SaveButtonText="Map"
                                ValidationGroup="grpFormSubmit" ExtraButtonIconClass="icnreset" GridUpdateText="Save" />
                        </FormTemplate>
                    </EditFormSettings>
                </MasterTableView>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
        <div id="divCmdButton" runat="server">
            <div class="sxcbar">
                <div class="sxcmds" style="text-align: center">
                    <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false">
                        <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                </div>
            </div>
        </div>
    </div>
</div>
