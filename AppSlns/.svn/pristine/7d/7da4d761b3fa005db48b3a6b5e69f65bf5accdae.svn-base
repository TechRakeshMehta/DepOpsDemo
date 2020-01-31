<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Messaging.Views.Messaging"
    Title="Messaging" MasterPageFile="~/Messaging/Masters/MessagingMaster.master" CodeBehind="Messaging.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/Messaging/SubscriptionSetting.ascx" TagPrefix="uc" TagName="Settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageContent" runat="server">
    <input type="hidden" id="hdnLastGetData" />
    <asp:HiddenField id="hdnFocusFolder" runat="server" />
    <div id="messagingGrid">
        <infs:WclSplitter ID="WclSplitter1" runat="server" LiveResize="true" Orientation="Vertical"
            Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
            <infs:WclPane ID="WclPane2" runat="server" MinWidth="200" Height="100%" Scrolling="Y"
                Collapsed="false">
                <div id="m_msgsearch">
                    <div class="section">
                        <h1 class="mhdr">Search for mails</h1>
                        <div class="content">
                            <div class="sxform auto">
                                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlName1">
                                    <div class='sxro sx2co'>
                                        <div class='sxlb'>
                                            <span class='cptn'>From</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox runat="server" ID="txtFrom">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class='sxlb'>
                                            <span class='cptn'>Body</span>  
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox runat="server" ID="txtBody">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx2co'>
                                        <div class='sxlb'>
                                            <span class='cptn'>Subject</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox runat="server" ID="txtSubject">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class='sxlb'>
                                            <span class='cptn'>To</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox runat="server" ID="txtTo">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div style="margin-top: 10px; text-align: right;">
                                <infs:WclButton runat="server" ID="btnSearch" Text="Search" OnClientClicked="e_PerformSearch"
                                    AutoPostBack="false">
                                </infs:WclButton>
                                <infs:WclButton runat="server" ID="WclButton2" Text="Close" AutoPostBack="false"
                                    OnClientClicked="e_closeserachbox">
                                </infs:WclButton>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:XmlDataSource ID="xmlMessages" runat="server" DataFile="~/App_Data/DB.xml" XPath="//Messaging/Inbox/*"></asp:XmlDataSource>
                <asp:UpdatePanel ID="UpdatePanelMessageLst" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="swrap">
                            <div class="messaging" style="display: none">
                                <infs:WclGrid ID="RadGrid1" EnableViewState="false" runat="server" AllowPaging="true"
                                    AutoGenerateColumns="false" GridLines="None" Height="100%" BorderWidth="0px"
                                    AllowSorting="True" EnableDefaultFeatures="false" Style="outline: none" AllowFilteringByColumn="false"
                                    AutoSkinMode="True" ShowGroupPanel="false" CellSpacing="0" AllowMultiRowSelection="True"
                                    ClientKey="grdmessage" ShowExtraButtons="False" ShowAllExportButtons="false"
                                    OnItemCommand="RadGrid1_ItemCommand" OnItemCreated="RadGrid1_ItemCreated">
                                    <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="true" ClientEvents-OnRowSelected=""
                                        EnablePostBackOnRowClick="false">
                                        <Selecting AllowRowSelect="true"></Selecting>
                                        <Scrolling UseStaticHeaders="true" />
                                        <ClientEvents OnCommand="RadGrid1_Command" OnRowDblClick="e_viewMessage" OnRowClick="RemoveBold"
                                            OnRowDataBound="RadGrid1_RowDataBound" OnRowDeselected="RowDeselected" OnRowDeselecting="RowDeselecting"
                                            OnRowSelected="RowSelected" />
                                    </ClientSettings>
                                    <MasterTableView EnableColumnsViewState="false" AllowFilteringByColumn="false" TableLayout="Fixed"
                                        AllowMultiColumnSorting="false" EnableHeaderContextMenu="false">
                                        <Columns>
                                            <telerik:GridBoundColumn UniqueName="MessageDetailID" Display="false" DataField="MessageDetailID">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn UniqueName="HasAttachment" Groupable="False" HeaderText=""
                                                AllowFiltering="false">
                                                <HeaderStyle Width="30px"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:Image ID="imgHasAttachment" BorderWidth="0px" ImageUrl="~/Resources/Mod/Messaging/icons/attachment.png"
                                                        AlternateText="Has attachment(s)" CssClass="on" Style="cursor: pointer;" runat="server"></asp:Image>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Important" Groupable="False" HeaderText=""
                                                AllowFiltering="false">
                                                <HeaderStyle Width="30px"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:Image ID="imgImportantMailButton" BorderWidth="0px" ImageUrl="~/Resources/Mod/Messaging/icons/important.png"
                                                        AlternateText="Important Message" CssClass="on" Style="cursor: pointer;" runat="server"></asp:Image>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn UniqueName="From" SortExpression="From" HeaderText="From"
                                                DataField="From">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="To" SortExpression="To" HeaderText="To" DataField="To"
                                                Display="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="Subject" SortExpression="Subject" HeaderText="Subject"
                                                DataField="Subject" HeaderStyle-Width="200px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="Received" SortExpression="ReceivedDate" HeaderText="Date"
                                                DataFormatString="{0:MM/dd/yyyy H:mm:ss}" FilterListOptions="VaryByDataType"
                                                DataField="ReceivedDate">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn UniqueName="flag" Groupable="False" HeaderText="" AllowFiltering="false">
                                                <HeaderStyle Width="30px"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgMailFlagImageButton" BorderWidth="0px" CommandName="ChangeFlag"
                                                        ImageUrl="~/Resources/Mod/Messaging/Images/mailFlagRed.gif" CssClass="on" AlternateText="Change Flag"
                                                        Style="cursor: pointer;" runat="server" Visible="true"></asp:ImageButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Restore" Groupable="False" HeaderText=""
                                                AllowFiltering="false">
                                                <HeaderStyle Width="30px"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgRestoreButton" BorderWidth="0px" ImageUrl="~/Resources/Mod/Messaging/Images/restore.png"
                                                        CssClass="on" AlternateText="Restore Message" Style="cursor: pointer;" runat="server"
                                                        Visible="true"></asp:ImageButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn UniqueName="IsUnread" Display="false" DataField="IsUnread">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="IsFollowUp" Display="false" DataField="IsFollowUp">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="IsHighImportant" Display="false" DataField="IsHighImportant">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="TotalRecords" Display="false" DataField="TotalRecords">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="FromUserId" Display="false" DataField="FromUserId">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat="{4} {5} Item(s) in {1} page(s)" />
                                </infs:WclGrid>
                            </div>
                        </div>
                        <div class="gclr">
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </infs:WclPane>
            <infs:WclSplitBar ID="WclSplitBar1" runat="server" CollapseMode="Both">
            </infs:WclSplitBar>
            <infs:WclPane ID="WclPaneContent" runat="server" MinWidth="300" Height="100%" Scrolling="Both"
                Collapsed="false">
                <div id="msgBody" class="textual">
                </div>
            </infs:WclPane>
        </infs:WclSplitter>
    </div>
    <div id="suscriptionSetting" style="display: none">
        <uc:Settings ID="Settings1" runat="server" />
    </div>
    <asp:HiddenField runat="server" id="hdnIsRead" value="0">
    </asp:HiddenField>
</asp:Content>
