<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageOrderColorStatus.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageOrderColorStatus" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr">Manage Order Status Color
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
                      <%--  <infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" DefaultMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged">
                        </infs:WclDropDownList>--%>
                        <infs:WclComboBox  ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID"  EmptyMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                             Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"></infs:WclComboBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="swrap" runat="server" id="dvOrderColorStatus">
            <infs:WclGrid runat="server" ID="grdOrderColorStatus" AllowPaging="False" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                GridLines="Both" EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                NonExportingColumns="EditCommandColumn,DeleteColumn" OnNeedDataSource="grdOrderColorStatus_NeedDataSource"
                OnItemCommand="grdOrderColorStatus_ItemCommand"
                OnItemDataBound="grdOrderColorStatus_ItemDataBound">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="IOF_ID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Order Status Color"
                        ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>                        
                        <telerik:GridTemplateColumn HeaderText="Color / Flag" ItemStyle-VerticalAlign="Middle" AllowFiltering="false" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left">                            
                            <ItemTemplate>
                                <asp:Image ID="imgIcon" runat="server" ImageUrl='<%# String.Format("~/{0}/{1}", Eval("lkpOrderFlag.OFL_FilePath"), Eval("lkpOrderFlag.OFL_FileName")) %>' /><br />
                                [<%# Eval("lkpOrderFlag.OFL_Tooltip") %>]
                            </ItemTemplate> 
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="IOF_Description" FilterControlAltText="Filter Description column"
                            HeaderText="Description" SortExpression="IOF_Description" UniqueName="IOF_Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="IOF_IsSuccessIndicator" FilterControlAltText="Filter IOF_IsSuccessIndicator column"
                            HeaderText="Is Success Indicator" SortExpression="IOF_IsSuccessIndicator" UniqueName="IOF_IsSuccessIndicator">
                            <ItemTemplate>
                                <asp:Label ID="lblIsSuccessIndicator" runat="server" Text='<%# Convert.ToBoolean(Eval("IOF_IsSuccessIndicator"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                <%--<asp:HiddenField ID="hdnfIsEditable" runat="server" Value='<%#Eval("BSG_IsEditable")%>' />--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Order Status Color?"
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
                                    <asp:Label ID="lblEHOrderStatusColor" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Order Status Color" : "Update Order Status Color" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlOrderStatusColor">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Order Status Flag</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <telerik:RadComboBox ID="rcbInstitutionStatusColorIcons" runat="server" EmptyMessage="--Select--" AllowCustomText="true" ValidationGroup="grpFormSubmit" >
                                                        <ItemTemplate>
                                                            <asp:Image ID="imbIcon" runat="server" ImageUrl='<%# String.Format("~/{0}/{1}", Eval("OFL_FilePath"), Eval("OFL_FileName")) %>' />
                                                            <asp:Label ID="lblTooltip" runat="server" Text='<%# Eval("OFL_Tooltip") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:RadComboBox>
                                                    <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvInstitutionStatusColor" ControlToValidate="rcbInstitutionStatusColorIcons"
                                                Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"  Text="Order Status Color/Flag is required." />
                                                        </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Is Success Indicator</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclButton runat="server" ID="chkIsSuccessIndicator" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                        AutoPostBack="false" Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IOF_IsSuccessIndicator")==null? false:Eval("IOF_IsSuccessIndicator") %>'>
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
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <infs:WclTextBox runat="server" ID="txtDescription" Text='<%# Eval("IOF_Description") %>'
                                                        MaxLength="1024">
                                                    </infs:WclTextBox>
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
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>
