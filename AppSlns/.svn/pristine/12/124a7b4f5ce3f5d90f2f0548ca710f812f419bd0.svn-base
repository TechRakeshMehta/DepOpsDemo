<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageSrvcItemEntityRecord.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageSrvcItemEntityRecord" %>


<%@ Register TagPrefix="telerik" Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="Commandbar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="page_cmd">
    <infs:WclButton runat="server" ID="btnAdd" Text="+ Add Attribute Value " OnClick="btnAdd_Click"
        Height="30px" ButtonType="LinkButton">
    </infs:WclButton>
</div>
<div class="section" id="divAddForm" runat="server" visible="false">
    <h1 class="mhdr">
        <asp:Label ID="lblAddEntity" Text="Entity Record"
            runat="server" /></h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlReviewer">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Select attribute</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclDropDownList ID="ddlAttributeList" runat="server"
                            DataTextField="AttributeName" DataValueField="AttributeGroupMappingId" OnDataBound="ddlAttributeList_DataBound"
                            OnSelectedIndexChanged="ddlAttributeList_SelectedIndexChanged" AutoPostBack="true">
                        </infs:WclDropDownList>
                    </div>
                    <div class='sxlb'></div>
                    <div class='sxlm'></div>
                    <div class='sxlb'>
                        <span class="cptn">All Occurences</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <asp:CheckBox ID="chkAllOccurences" runat="server"></asp:CheckBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div id="dvState" runat="server" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">Select State</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="ddlStateList" runat="server"  DataTextField="StateName"
                                DataValueField="StateID" OnDataBound="ddlStateList_DataBound" OnSelectedIndexChanged="ddlStateList_SelectedIndexChanged">
                            </infs:WclComboBox>
                        </div>
                    </div>
                    <div id="dvCounty" runat="server" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">Select County</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="ddlCountyList" runat="server"
                                DataTextField="CountyName" DataValueField="CountyID" OnDataBound="ddlCountyList_DataBound">
                            </infs:WclComboBox>
                        </div>
                    </div>
                    <div id="dvAttribteValue" runat="server" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">Enter Attribute Value</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtAttributeValue" runat="server">
                            </infs:WclTextBox>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="sxcbar">
            <div class="sxcmds" style="text-align: right">
                <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="grpFormSubmit">
                    <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                        PrimaryIconWidth="14" />
                </infs:WclButton>
                <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click">
                    <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                        PrimaryIconWidth="14" />
                </infs:WclButton>
            </div>
        </div>
    </div>
</div>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblServiceName" runat="server" Text="Service Item Attribute Value"></asp:Label></h1>
    <div class="content">
        <infs:WclGrid runat="server" ID="grdManageSvcItemEntity" AllowPaging="True" PageSize="10"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" AutoGenerateColumns="False"
            AllowFilteringByColumn="false" AllowSorting="True"
            ShowAllExportButtons="False" ShowClearFiltersButton="false" OnNeedDataSource="grdManageSvcItemEntity_NeedDataSource" OnItemCommand="grdManageSvcItemEntity_ItemCommand">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ServiceItemEntityId" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="true" ShowExportToPdfButton="true"
                    ShowExportToCsvButton="true"></CommandItemSettings>
                <Columns>
                    <telerik:GridBoundColumn DataField="AttributeName" HeaderText="Attribute Name"
                        SortExpression="AttributeName" UniqueName="AttributeName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Value" HeaderText="Attribute Value"
                        SortExpression="Value" UniqueName="Value">
                    </telerik:GridBoundColumn>
                   <%-- <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" VerticalAlign="Bottom" />
                    </telerik:GridEditCommandColumn>--%>
                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Attribute Value?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" Width="3%" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
        </infs:WclGrid>
    </div>
</div>
