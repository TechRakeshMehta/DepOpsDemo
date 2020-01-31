<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstitutionConfigurationLocalFeeDetails.aspx.cs" Inherits="CoreWeb.SystemSetUp.Views.InstitutionConfigurationLocalFeeDetails" MaintainScrollPositionOnPostback="true"
    MasterPageFile="~/Shared/ChildPage.master" Title="InstitutionConfigurationLocalFeeDetails" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    </infs:WclResourceManagerProxy>

    <div class="page_cmd">
        <infs:WclButton runat="server" ID="btnBackToQueue" Text="Back To Package Detail" OnClick="btnBackToQueue_Click"
            Height="30px" ButtonType="LinkButton">
        </infs:WclButton>
    </div>


    <div id="divHierarchyNodePackage" runat="server">
        <div class="msgbox">
            <asp:Label ID="lblMessage" runat="server" CssClass="info"> </asp:Label>
        </div>

        <div class="section">
            <h1 class="mhdr">
                <asp:Label ID="lblFeeItem" runat="server"></asp:Label>
            </h1>
            <div class="content">
                <div class="swrap">
                    <infs:WclGrid runat="server" ID="grdFeeRecord" AllowPaging="false"
                        AutoSkinMode="true" CellSpacing="0" GridLines="Both" AutoGenerateColumns="False"
                        AllowFilteringByColumn="false" AllowSorting="True" EnableDefaultFeatures="false"
                        ShowAllExportButtons="False" ShowClearFiltersButton="false"
                        OnNeedDataSource="grdFeeRecord_NeedDataSource"
                        EnableLinqExpressions="false">
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="LocalSFRID">
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowExportToExcelButton="false" ShowExportToCsvButton="false"
                                ShowRefreshButton="true" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="StateName" HeaderText="State" UniqueName="State"
                                    SortExpression="StateName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CountyName" HeaderText="County" UniqueName="County"
                                    SortExpression="CountyName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LocalSFRAmount" HeaderText="Amount($)" DataType="System.Decimal"
                                    SortExpression="LocalSFRAmount" UniqueName="LocalSFRAmount" DataFormatString="{0:c}">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="GlobalFeeAmount" HeaderText="Global Fee Amount($)"
                                    SortExpression="GlobalFeeAmount" UniqueName="GlobalFeeAmount" DataFormatString="{0:c}">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </infs:WclGrid>
                </div>
                <div class="gclr">
                </div>
            </div>
        </div>
    </div>
</asp:Content>



