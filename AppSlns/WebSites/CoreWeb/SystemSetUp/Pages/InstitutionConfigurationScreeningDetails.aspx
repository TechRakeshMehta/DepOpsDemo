<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstitutionConfigurationScreeningDetails.aspx.cs"
    Inherits="CoreWeb.SystemSetUp.Views.InstitutionConfigurationScreeningDetails"
    MaintainScrollPositionOnPostback="true"
    MasterPageFile="~/Shared/ChildPage.master" Title="InstitutionConfigurationScreeningDetails" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .PdfLink
        {
            padding-left: 2px;
            padding-top: 2px !important;
        }

        .PdfLabel
        {
            padding-left: 20px;
            padding-top: 2px !important;
        }

        .hlink
        {
            cursor: pointer;
            padding-top: 2px !important;
        }
    </style> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
        });

        function KeyPress(sender, args) {
            if (args.get_keyCharacter() == sender.get_numberFormat().DecimalSeparator || args.get_keyCharacter() == '-') {
                args.set_cancel(true);
            }
        }

    </script>

    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/mapping_pages.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <div class="container-fluid">
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-10">&nbsp;</div>
                <div class="col-md-2 text-right">
                    <infs:WclButton runat="server" ID="btnBackToQueue" Text="Back To Queue" OnClick="btnBackToQueue_Click"
                        ButtonType="LinkButton" Height="30px" AutoSkinMode="false" Skin="Silk">
                    </infs:WclButton>
                </div>
            </div>
        </div>
        <div id="divHierarchyNodePackage" runat="server">
            <div class="col-md-12">
                <div class="msgbox">
                    <asp:Label ID="lblMessage" runat="server" CssClass="info"> </asp:Label>
                </div>
            </div>
            <div class="bgLightGreen">
                <div class="col-md-12">
                    <div class="row">
                        <infs:WclButton runat="server" ID="btnDummy"></infs:WclButton>
                      <h2 class="heighAuto">
                            <asp:Label ID="lblNodeTitle" runat="server" CssClass="header-color"></asp:Label>
                        </h2>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="row">
                        <infs:WclGrid runat="server" ID="grdServiceGrp" AllowPaging="false" PageSize="10"
                            AutoGenerateColumns="False" AllowSorting="false" GridLines="Both" ShowAllExportButtons="False"
                            AllowFilteringByColumn="false"
                            OnNeedDataSource="grdServiceGrp_NeedDataSource"
                            ShowClearFiltersButton="false" EnableDefaultFeatures="false" OnItemCommand="grdServiceGrp_ItemCommand">
                            <ClientSettings EnableRowHoverStyle="true">
                                <Selecting AllowRowSelect="true"></Selecting>
                            </ClientSettings>
                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ServiceGroupID,PackageSvcGrpID"
                                AllowFilteringByColumn="false" HierarchyDefaultExpanded="true">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                                    ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"
                                    ShowExportToWordButton="false"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="ServiceGroupName" UniqueName="ServiceGroupName"
                                        HeaderText="Service Group Name">
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <NestedViewTemplate>
                                    <div class="swrap">
                                        <infs:WclGrid runat="server" ID="grdServices" AllowPaging="false"
                                            PageSize="10" AutoGenerateColumns="False" AllowSorting="false" GridLines="Both"
                                            ShowClearFiltersButton="false"
                                            OnNeedDataSource="grdServices_NeedDataSource" OnItemCommand="grdServices_ItemCommand"
                                            ShowAllExportButtons="false" AllowFilteringByColumn="false" PagerStyle-ShowPagerText="false"
                                            EnableDefaultFeatures="false" OnItemDataBound="grdServices_ItemDataBound">
                                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ServiceID"
                                                AllowFilteringByColumn="false" HierarchyDefaultExpanded="true">
                                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                                                    ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="ServiceName"
                                                        HeaderText="Service Name" UniqueName="ServiceName" HeaderStyle-Width="400px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Electronic Service Form(s)" ItemStyle-CssClass="breakword"
                                                        ItemStyle-Width="400px"
                                                        UniqueName="ServiceFormsElectronic">
                                                        <ItemTemplate>
                                                            <asp:Repeater ID="rptServiceForms" runat="server" OnItemDataBound="rptServiceForms_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <div style="text-align: Left;">
                                                                        <asp:ImageButton ID="imgPDF" OnClick="imgPDF_Click" CssClass="hlink" AlternateText="PDF"
                                                                            runat="server" ImageUrl='<%# ImagePath + "/pdf.gif" %>' />
                                                                        <asp:LinkButton Text="Service Form" runat="server" CssClass="PdfLink" ID="lnkDownldSvcFrm"
                                                                            OnClick="hlViewServiceForm_Click"></asp:LinkButton>
                                                                        <asp:HiddenField runat="server" ID="hdnServiceForm" Value='<%#Eval("SystemDocumentID")%>' />
                                                                    </div>
                                                                </ItemTemplate>
                                                                <SeparatorTemplate>
                                                                    <div style="clear: both;">
                                                                    </div>
                                                                </SeparatorTemplate>
                                                            </asp:Repeater>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Manual Service Form(s)" ItemStyle-CssClass="breakword"
                                                        ItemStyle-Width="400px"
                                                        UniqueName="ServiceFormsManual">
                                                        <ItemTemplate>
                                                            <asp:Repeater ID="rptServiceFormsManual" runat="server" OnItemDataBound="rptServiceFormsManual_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <div style="text-align: Left;">
                                                                        <asp:Label ID="lblServiceForm" runat="server"></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                                <SeparatorTemplate>
                                                                    <div style="clear: both;">
                                                                    </div>
                                                                </SeparatorTemplate>
                                                            </asp:Repeater>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                                <NestedViewTemplate>
                                                    <div class="swrap">
                                                        <infs:WclGrid runat="server" ID="grdServiceItems" AllowPaging="false"
                                                            PageSize="10" AutoGenerateColumns="False" AllowSorting="false" GridLines="Both"
                                                            ShowClearFiltersButton="false"
                                                            OnNeedDataSource="grdServiceItems_NeedDataSource"
                                                            ShowAllExportButtons="false" AllowFilteringByColumn="false" PagerStyle-ShowPagerText="false"
                                                            EnableDefaultFeatures="false" OnItemDataBound="grdServiceItems_ItemDataBound">
                                                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="PackageServiceItemID"
                                                                AllowFilteringByColumn="false" HierarchyDefaultExpanded="true">
                                                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                                                                    ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="PackageServiceItemName"
                                                                        HeaderText="Service Item Name" UniqueName="PackageServiceItemName" HeaderStyle-Width="20%">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="QuantityGroup" HeaderStyle-Width="10%"
                                                                        HeaderText="Quantity Group" UniqueName="QuantityGroup">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="QuantityIncluded" HeaderStyle-Width="10%"
                                                                        HeaderText="Quantity Included" UniqueName="QuantityIncluded">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="AdditionalOccurencePriceType" HeaderStyle-Width="10%"
                                                                        HeaderText="Additional Occurrence Price Type" UniqueName="AdditionalOccurencePriceType">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="AdditionalOccurencePriceAmount" HeaderStyle-Width="10%"
                                                                        HeaderText="Additional Occurrence Price Amount" UniqueName="AdditionalOccurencePriceAmount">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Global Fee" ItemStyle-CssClass="breakword"
                                                                        UniqueName="GlobalFeeName" ItemStyle-Width="20%">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton Visible="true" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("GlobalFeeName")))%>' runat="server" ID="lnkGlobalFeeName"
                                                                                OnClick="lnkGlobalFeeName_Click"></asp:LinkButton>
                                                                            <asp:HiddenField runat="server" ID="hdnGlobalFeeName" Value='<%#Eval("GlobalFeeName")%>' />
                                                                            <asp:HiddenField runat="server" ID="hdnPackageServiceItemFeeID" Value='<%#Eval("PackageServiceItemFeeID")%>' />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Local Fee(s)" ItemStyle-CssClass="breakword"
                                                                        ItemStyle-Width="20%"
                                                                        UniqueName="LocalFees">
                                                                        <ItemTemplate>
                                                                            <asp:Repeater ID="rptLocalFees" runat="server" OnItemDataBound="rptLocalFees_ItemDataBound">
                                                                                <ItemTemplate>
                                                                                    <div style="text-align: Left;">
                                                                                        <asp:LinkButton Visible="false" runat="server" ID="lnkLocalFees" OnClick="lnkLocalFees_Click"></asp:LinkButton>
                                                                                        <asp:Label ID="lblLocalFees" runat="server" Visible="false"></asp:Label>
                                                                                        <asp:HiddenField runat="server" ID="hdnLocalFeeName" Value='<%#Eval("FeeItemType")%>' />
                                                                                        <asp:HiddenField runat="server" ID="hdnPackageServiceItemFeeID" Value='<%#Eval("PackageServiceItemFeeID")%>' />
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                                <SeparatorTemplate>
                                                                                    <div style="clear: both;">
                                                                                    </div>
                                                                                </SeparatorTemplate>
                                                                            </asp:Repeater>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                        </infs:WclGrid>
                                                    </div>
                                                </NestedViewTemplate>
                                            </MasterTableView>
                                        </infs:WclGrid>
                                    </div>
                                </NestedViewTemplate>

                            </MasterTableView>
                        </infs:WclGrid>
                    </div>
                </div>
            </div>
        </div>

        <iframe id="ifrExportDocument" runat="server" height="0" width="0"></iframe>
    </div>
</asp:Content>


