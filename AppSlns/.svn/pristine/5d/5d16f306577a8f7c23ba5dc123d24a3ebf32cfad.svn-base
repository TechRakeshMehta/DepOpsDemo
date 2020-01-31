<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientServiceVendor.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ClientServiceVendor" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<script type="text/javascript" language="javascript">



    function DisableStatedropdown(sender, args) {
        var chkAllState = $jQuery("[id$=chkAllState]");
        var dvStateDropdown = $jQuery("[id$=dvStateDropdown]");
        if (sender._checked) {
            dvStateDropdown.hide();
        }
        else {
            //chkAllState[0].control.set_checked(false);
            dvStateDropdown.show();
        }
    }
</script>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblApplicantPortFolioSearch" runat="server" Text="Client Service Vendor"></asp:Label></h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info"> </asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="cptn"></asp:Label>
                    </div>
                    <div class='sxlm'>
                       <%-- <infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
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


        <div class="swrap" runat="server" id="dvClientSvcVendor">
            <infs:WclGrid runat="server" ID="grdClientSvcVendor" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                GridLines="Both" EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                NonExportingColumns="EditCommandColumn,DeleteColumn" OnNeedDataSource="grdClientSvcVendor_NeedDataSource"
                OnItemCreated="grdClientSvcVendor_ItemCreated" OnItemDataBound="grdClientSvcVendor_ItemDataBound"
                OnInsertCommand="grdClientSvcVendor_InsertCommand" OnUpdateCommand="grdClientSvcVendor_UpdateCommand" OnPreRender="grdClientSvcVendor_PreRender" OnDeleteCommand="grdClientSvcVendor_DeleteCommand">

                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BkgServiceID,ExtServiceID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Mapping"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="BkgServiceName" FilterControlAltText="Filter External Service column"
                            HeaderText="Background Service" SortExpression="BkgServiceName" UniqueName="BkgServiceName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ExtServiceName" FilterControlAltText="Filter External Service column"
                            HeaderText="External Service" SortExpression="ExtServiceName" UniqueName="ExtServiceName" AllowSorting="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ExtServiceCode" FilterControlAltText="Filter External Service Code column"
                            HeaderText="External Service Code" SortExpression="ExtServiceCode" UniqueName="ExtServiceCode" AllowSorting="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="State" FilterControlAltText="Filter State column"
                            HeaderText="State" SortExpression="State" UniqueName="State" AllowSorting="false">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridTemplateColumn DataField="" FilterControlAltText="Filter IsActive column"
                            HeaderText="Is Editable" SortExpression="BSE_IsEditable" UniqueName="BSE_IsEditable" Display="false">
                            <ItemTemplate>
                                <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("BSE_IsEditable"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                <asp:HiddenField ID="hdnfIsEditable" runat="server" Value='<%#Eval("BSE_IsEditable")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>


                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Mapping?"
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
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlServiceGroup">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Background Service</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbBkgServices" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbBkgServices_SelectedIndexChanged"
                                                        DataTextField="BSE_Name" DataValueField="BSE_ID" EmptyMessage="--Select--">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvcmbBkgServices" ControlToValidate="cmbBkgServices"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Background Service is required." />
                                                    </div>
                                                </div>

                                                <div class='sxlb'>
                                                    <span class="cptn">External Service</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbExtBkgServices" runat="server" OnSelectedIndexChanged="cmbExtBkgServices_SelectedIndexChanged"
                                                        DataTextField="EBS_Name" DataValueField="EBS_ID" EmptyMessage="--Select--" AutoPostBack="true">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvcmbExtBkgServices" ControlToValidate="cmbExtBkgServices"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="External Service is required." />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">External Service Code</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:Label ID="lblExtSvcCode" runat="server"></asp:Label>

                                                </div>
                                            <div class='sxroend'>
                                            </div>
                                    </div>
                                    <div class='sxro sx3co'>
                                        <div id="dvStateDropdown" runat="server">
                                            <div class='sxlb'>
                                                <span class="cptn">State</span>
                                            </div>
                                            <div class='sxlm'>
                                                <%--<infs:WclComboBox ID="cmbState" runat="server" CheckBoxes="true" EmptyMessage="--Select--"
                                                        DataTextField="StateName" DataValueField="StateID">
                                                    </infs:WclComboBox>--%>
                                                <infs:WclComboBox ID="cmbState" runat="server" CheckBoxes="true"
                                                    DataTextField="StateName" DataValueField="StateID" EmptyMessage="--Select--">
                                                </infs:WclComboBox>


                                                <%--  <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvcmbState" ControlToValidate="cmbState"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="State is required." />
                                                    </div>--%>
                                            </div>
                                        </div>
                                        <div class='sxlb'>
                                            <span class="cptn">All States</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclButton runat="server" ID="chkAllState" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                OnClientClicked="DisableStatedropdown" AutoPostBack="false" OnClientLoad="DisableStatedropdown">
                                                <ToggleStates>
                                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                                </ToggleStates>
                                            </infs:WclButton>
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
