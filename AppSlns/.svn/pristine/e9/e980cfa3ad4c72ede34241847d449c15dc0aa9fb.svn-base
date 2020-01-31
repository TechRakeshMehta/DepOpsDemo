<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BkgOrderHistory.ascx.cs"
    Inherits="CoreWeb.BkgOperations.Views.BkgOrderHistory" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<infs:WclResourceManagerProxy runat="server" ID="rprxBkgPackagesOrdered">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color" tabindex="0">Order Event History</h2>
        </div>
    </div>

    <div class="row">
      
            <infs:WclGrid CssClass="removeExtraSpace" runat="server" ID="grdOrderHistory" AllowPaging="false" AutoGenerateColumns="False"
                AllowSorting="false" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="false"
                GridLines="None" OnNeedDataSource="grdOrderHistory_NeedDataSource">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BOEH_ID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="BOEH_CreatedOn" FilterControlAltText="Filter Date column"
                            HeaderText="Date" SortExpression="BOEH_CreatedOn" UniqueName="BOEH_CreatedOn"
                            HeaderStyle-Width="230px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BOEH_OrderEventDetail" FilterControlAltText="Filter Detail column"
                            HeaderText="Detail" SortExpression="BOEH_OrderEventDetail"
                            UniqueName="BOEH_OrderEventDetail">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BOEH_FullName" FilterControlAltText="Filter Full Name"
                            HeaderText="Created By" SortExpression="BOEH_FullName"
                            UniqueName="BOEH_FullName">
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </infs:WclGrid>
        </div>
    
    <div class="col-md-12" style="display: none;">
        <infs:WclButton runat="server" ID="WclButton1" Text="" Width="100%" CssClass="form-control"
            ButtonType="LinkButton" Height="30px">
        </infs:WclButton>
    </div>

</div>

