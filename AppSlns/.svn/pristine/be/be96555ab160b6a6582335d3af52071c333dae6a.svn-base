<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MapUserInstitution.ascx.cs" Inherits="CoreWeb.IntsofSecurityModel.Views.MapUserInstitution" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblMapUserInstitution" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblSuffix" runat="server" Text=""></asp:Label>
    </h1>
    <div class="content">
        <div class="swrap">
            <asp:Panel runat="server" ID="pnlMapUserInstitution">
                <infs:WclGrid runat="server" ID="grdMapUserInstitution"
                    AutoSkinMode="true" CellSpacing="0" GridLines="Both" AutoGenerateColumns="False"
                    AllowFilteringByColumn="false" AllowSorting="True" OnNeedDataSource="grdMapUserInstitution_NeedDataSource"
                    OnItemCommand="grdMapUserInstitution_ItemCommand" OnItemCreated="grdMapUserInstitution_ItemCreated">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="">
                        <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Institution Mapping"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="TenantName" HeaderText="Institution Name"
                                SortExpression="TenantName" UniqueName="TenantName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TenantDesc" HeaderText="Description"
                                SortExpression="TenantDesc" UniqueName="TenantDesc">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <EditFormSettings EditFormType="Template">
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                            <FormTemplate>
                                <div class="section" visible="true" id="divEditFormBlock" runat="server">
                                    <h1 class="mhdr">
                                        <asp:Label ID="lblInstitution" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Mapping" : "Update Mapping" %>'
                                            runat="server" /></h1>
                                    <div class="content">
                                        <div class="sxform auto">
                                            <div class="msgbox">
                                                <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                            </div>
                                            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlInstitution">
                                                <div class='sxro sx2co'>
                                                    <div id="divAttribute" runat="server">
                                                        <div class='sxlb'>
                                                            <span class="cptn">Institute</span><span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclComboBox ID="cmbOrganization" runat="server" DataTextField="TenantName" 
                                                                DataValueField="TenantID">
                                                            </infs:WclComboBox>
                                                            <div class="vldx">
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvOrganization" ControlToValidate="cmbOrganization"
                                                                    InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>

                                            </asp:Panel>
                                        </div>
                                        <infsu:CommandBar ID="fsucCmdBarTenant" runat="server" GridMode="true" DefaultPanel="pnlTenant"
                                            ValidationGroup="grpFormSubmit" GridInsertText="Save" GridUpdateText="Save" ExtraButtonIconClass="icnreset" />
                                    </div>
                                </div>
                            </FormTemplate>
                        </EditFormSettings>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                </infs:WclGrid>
            </asp:Panel>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>
