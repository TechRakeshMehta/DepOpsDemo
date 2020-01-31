<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageService.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageService" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<style type="text/css">
    /*.rgMasterTable td {
        word-wrap: break-word;
        word-break: break-all;
    }*/
</style>

<div class="section">
    <h1 class="mhdr">Manage Services
    </h1>
    <div class="content">
       
        <div class="swrap" runat="server" id="dvServiceGroup">
            <infs:WclGrid runat="server" ID="grdManageService" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                GridLines="Both" EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                NonExportingColumns="EditCommandColumn,DeleteColumn,MapAttributeGroup,MapCustomForm"
                OnItemCommand="grdManageService_ItemCommand" OnNeedDataSource="grdManageService_NeedDataSource"
                OnItemCreated="grdManageService_ItemCreated" OnItemDataBound="grdManageService_ItemDataBound" >
                
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BSE_ID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Service"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="BSE_Name" FilterControlAltText="Filter Service Name column"
                            HeaderText="Service Name" SortExpression="BSE_Name" UniqueName="BSE_Name">
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="BSE_ConfigurableServiceText" FilterControlAltText="Filter Service Configurable Service Text column"
                            HeaderText="Configurable Service Text" SortExpression="BSE_ConfigurableServiceText" UniqueName="BSE_ConfigurableServiceText">
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="BSE_Description" FilterControlAltText="Filter Service Description column"
                            HeaderText="Service Description" SortExpression="BSE_Description" UniqueName="BSE_Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="lkpBkgSvcType.BST_Name" FilterControlAltText="Filter Service Type column"
                            HeaderText="Service Type" SortExpression="lkpBkgSvcType.BST_Name" UniqueName="lkpBkgSvcType.BST_Name">
                        </telerik:GridBoundColumn>
                       <%-- <telerik:GridTemplateColumn DataField="BSE_IsEditable" FilterControlAltText="Filter IsActive column"
                            HeaderText="Is Editable" SortExpression="BSE_IsEditable" UniqueName="BSE_IsEditable" Display="false">
                            <ItemTemplate>
                                <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("BSE_IsEditable"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                               
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="MapAttributeGroup" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnfIsEditable" runat="server" Value='<%#Eval("BSE_IsEditable")%>'/>
                                <telerik:RadButton ID="btnDetails" ButtonType="LinkButton" CommandName="MapAttributeGroup"
                                    runat="server" Text="Attribute Mapping" BackColor="Transparent" Font-Underline="true" BorderStyle="None" ForeColor="Black">                                     
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="MapCustomForm" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnCustomForm" ButtonType="LinkButton" CommandName="MapCustomForm"
                                    runat="server" Text="Custom Form Mapping" BackColor="Transparent" Font-Underline="true" BorderStyle="None" ForeColor="Black">
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Service?"
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
                                    <asp:Label ID="lblEHServiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Service" : "Update Service" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlServiceGroup">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Service Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtServiceName" Text='<%# Eval("BSE_Name") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvServiceGroupName" ControlToValidate="txtServiceName"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Service Name is required." />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Service Type</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="ddlServiceType" runat="server"
                                                        DataTextField="BST_Name" DataValueField="BST_ID" EmptyMessage="--Select--">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvServiceType" ControlToValidate="ddlServiceType"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Service Type is required." />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Derived From</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="ddlDerivedFromServices" runat="server"
                                                        DataTextField="BSE_Name" DataValueField="BSE_ID">
                                                    </infs:WclComboBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                              <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Configurable Service Text</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <infs:WclTextBox runat="server" ID="txtConfServiceText" Text='<%# Eval("BSE_ConfigurableServiceText") %>'
                                                        MaxLength="500">
                                                    </infs:WclTextBox>
                                                </div>

                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <infs:WclTextBox runat="server" ID="txtSvcDescription" Text='<%# Eval("BSE_Description") %>'
                                                        MaxLength="1024">
                                                    </infs:WclTextBox>
                                                </div>

                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                            <div class="sxroend"></div>
                            <div class="section" visible="true" id="divConfigureSetting" runat="server">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblConfigureSetting" Text="Configure Service Setting"
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="sxpnl">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Show Included in Package Count</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclButton runat="server" ID="chkPackageCount" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>
                                                </div>
                                              <%--  <div class='sxlb'>
                                                    <span class="cptn">Show Number of Years of Residence</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclButton runat="server" ID="chkYear" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>
                                                </div>--%>
                                                <div class='sxlb'>
                                                    <span class="cptn">Show Minimum Occurrences</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclButton runat="server" ID="chkMinOccurences" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Show Maximum Occurrences</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclButton runat="server" ID="chkMaxOccurences" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Show Send Documents to Applicant</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclButton runat="server" ID="chkSendDoc" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Show Is Supplemental</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclButton runat="server" ID="chkIsSupplemental" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <%--<div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Show Ignore Residential History on Supplement</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclButton runat="server" ID="chkIgnore" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>
                                                </div>
                                                <div class="sxroend"></div>
                                            </div>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlServiceGroup"
                                ValidationGroup="grpFormSubmit" GridInsertText="Save" GridUpdateText="Save" ExtraButtonIconClass="icnreset" />
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

