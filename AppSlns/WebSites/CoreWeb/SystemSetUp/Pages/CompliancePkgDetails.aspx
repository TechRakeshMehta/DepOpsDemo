<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompliancePkgDetails.aspx.cs"
    Inherits="CoreWeb.SystemSetUp.Views.CompliancePkgDetails"
    MasterPageFile="~/Shared/ChildPage.master" Title="InstitutionConfigurationDetails"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
        <div class="co-md-12">
            <div class="row">&nbsp;</div>
        </div>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-10"></div>
                <div class="col-md-2 text-right">
                    <infs:WclButton runat="server" ID="btnBackToQueue" Text="Back To Queue"
                        OnClick="btnBackToQueue_Click" Height="30px" ButtonType="LinkButton" Skin="Silk"
                        AutoSkinMode="false">
                    </infs:WclButton>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div id="divRefund" runat="server">
                    <h2 class="header-color heighAuto">
                        <asp:Label ID="lblNodeTitle" runat="server"></asp:Label>
                    </h2>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3 class="header-color">Subscription Option(s)
                </h3>
            </div>
        </div>
        <div class="row bgLightGreen">
            <asp:Panel runat="server" ID="pnlRefund">
                <asp:Repeater ID="rptSubscriptionOption" runat="server">
                    <ItemTemplate>
                        <%--<div class="section">--%>
                        <div class="col-md-12">
                            <div class="row">
                                <div class='form-group col-md-3'>
                                    <span class='cptn'>Subscription Option</span>
                                    <infs:WclTextBox runat="server" ID="txtSubscriptionOption" Enabled="false" Width="100%"
                                        CssClass="form-control" Text='<%# Eval("SubscriptionOptionLabel")%>'>
                                    </infs:WclTextBox>
                                </div>
                                <div class='form-group col-md-3'>
                                    <span class='cptn'>Price</span>
                                    <infs:WclNumericTextBox runat="server" ID="txtTotalRefund" Width="100%" CssClass="form-control"
                                        Enabled="false" Text='<%# Eval("Price")%>'
                                        NumberFormat-DecimalDigits="2" Type="Currency">
                                    </infs:WclNumericTextBox>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>
        </div>

        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color" title="Details pertaining to the Review Type displayed in this section">
                    Review Type
                </h2>
            </div>
        </div>
        <div class="row">
            <infs:WclTreeList ID="treeListDetail" runat="server" DataTextField="Value" ClientIDMode="Static"
                ParentDataKeyNames="ParentAssignmentHierarchyId" DataKeyNames="AssignmentHierarchyID"
                OnNeedDataSource="treeListDetail_NeedDataSource" OnPreRender="treeListDetail_PreRender"
                AutoGenerateColumns="false">
                <Columns>
                    <telerik:TreeListBoundColumn DataField="ObjectName" UniqueName="ObjectName" HeaderText="Compliance Category/Item"
                        HeaderTooltip="The Package or Category name is displayed in each row" />
                    <telerik:TreeListBoundColumn DataField="ReviewerType" UniqueName="ReviewerType" HeaderText="Reviewed By"
                        HeaderTooltip="Reviewed by, if any, for each row is displayed in this column" />
                </Columns>
            </infs:WclTreeList>
        </div>
    </div>
</asp:Content>
