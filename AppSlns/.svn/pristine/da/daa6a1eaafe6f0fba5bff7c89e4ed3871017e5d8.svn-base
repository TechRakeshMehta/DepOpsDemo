<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageServiceGroup.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageServiceGroup" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="../Shared/Controls/IsActiveToggle.ascx" TagName="IsActiveToggle"
    TagPrefix="uc1" %>
<div class="section">
    <h1 class="mhdr">Manage Service Groups
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="cptn"></asp:Label>
                    </div>
                    <div class='sxlm'>
                        <%--<infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" DefaultMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged">
                        </infs:WclDropDownList>--%>
                        <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID"  EmptyMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">  
                        </infs:WclComboBox> 
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="swrap" runat="server" id="dvServiceGroup">
            <infs:WclGrid runat="server" ID="grdServiceGroup" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                GridLines="Both" EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                NonExportingColumns="EditCommandColumn,DeleteColumn" OnNeedDataSource="grdServiceGroup_NeedDataSource"
                OnItemCommand="grdServiceGroup_ItemCommand"
                OnItemDataBound="grdServiceGroup_ItemDataBound">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BSG_ID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Service Group"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="BSG_Name" FilterControlAltText="Filter Service Group Name column"
                            HeaderText="Service Group Name" SortExpression="BSG_Name" UniqueName="BSG_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BSG_Description" FilterControlAltText="Filter Service Group Description column"
                            HeaderText="Service Group Description" SortExpression="BSG_Description" UniqueName="BSG_Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BSG_Active" FilterControlAltText="Filter IsActive column"
                            HeaderText="Is Active" SortExpression="BSG_Active" UniqueName="BSG_Active">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn Display="false">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnfIsEditable" runat="server" Value='<%#Eval("BSG_IsEditable")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn DataField="BSG_Active" FilterControlAltText="Filter IsActive column"
                            HeaderText="Is Active" SortExpression="BSG_Active" UniqueName="BSG_Active">
                            <ItemTemplate>
                                <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("BSG_Active"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                <asp:HiddenField ID="hdnfIsEditable" runat="server" Value='<%#Eval("BSG_IsEditable")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Service Group?"
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
                                    <asp:Label ID="lblEHServiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Service Group" : "Update Service Group" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlServiceGroup">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Service Group Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtServiceGroupName" Text='<%# Eval("BSG_Name") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvServiceGroupName" ControlToValidate="txtServiceGroupName"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Service Group Name is required." />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Is Active</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <uc1:IsActiveToggle runat="server" ID="chkActive" IsAutoPostBack="false" IsActiveEnable="true" Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("BSG_Active") %>' />
                                                
                                                    <%--<infs:WclButton runat="server" ID="chkActive" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                        AutoPostBack="false" Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("BSG_Active") %>'>
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>--%>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>

                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <infs:WclTextBox runat="server" ID="txtDescription" Text='<%# Eval("BSG_Description") %>'
                                                        MaxLength="1024">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlServiceGroup"
                                        ValidationGroup="grpFormSubmit" GridInsertText="Save" GridUpdateText="Save" ExtraButtonIconClass="icnreset" />
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
        <div class="gclr">
        </div>
    </div>
</div>
