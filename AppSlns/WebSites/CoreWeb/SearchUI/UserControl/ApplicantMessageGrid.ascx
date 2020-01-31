<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicantMessageGrid.ascx.cs" Inherits="CoreWeb.Search.Views.ApplicantMessageGrid" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr" title="Details pertaining to the student's subscription are displayed in this section">Message Details
    </h1>

    <div class="content">    
            <div Style="float:right; margin-bottom:10px;">
                <a runat="server" id="lnkGoBack">Back to portfolio Search</a>
                &nbsp;&nbsp;&nbsp;
                <a runat="server" id="lnkDetail">Back to portfolio detail</a>
            </div>
        
        
        <infs:WclGrid runat="server" ID="grdLstMsg" AutoGenerateColumns="False" AllowCustomPaging="true" AllowAutomaticInserts="false"
            AllowSorting="True" AutoSkinMode="True" CellSpacing="0" AllowFilteringByColumn="false" ShowClearFiltersButton="false"
            GridLines="Both" OnNeedDataSource="grdVerificationItemData_NeedDataSource">
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grd_rwDbClick" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" AllowFilteringByColumn="false" RowIndicatorColumn-ShowFilterIcon="false"
                CommandItemSettings-ShowAddNewRecordButton="false">

                <Columns>
                    <telerik:GridBoundColumn DataField="FromMessage" HeaderText="From" UniqueName="FromMessage">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Subject" HeaderText="Subject" UniqueName="Subject">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ReceivedDate" HeaderText="Received Date" UniqueName="ReceivedDate">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="MessageId" Display="false" DataField="MessageId"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdfCatId" runat="server" Value='<%# Eval("MessageId") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
        </infs:WclGrid>
    </div>
</div>

<script type="text/javascript">
    var messaging = {
        defaultPopup: {
            size: "800, 600",
            behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize | Telerik.Web.UI.WindowBehaviors.Reload
        }
    }
    function grd_rwDbClick(s, e) {
        var messageId = s.get_masterTableView().get_selectedItems()[0].get_cell("MessageId").innerHTML;
        var FromMessage = s.get_masterTableView().get_selectedItems()[0].get_cell("FromMessage").innerHTML;
        var Date = s.get_masterTableView().get_selectedItems()[0].get_cell("ReceivedDate").innerHTML;

        var url = $page.url.create("~/Messaging/Pages/MessageViewer.aspx?messageID=" + messageId + "&cType=CT01 &isImportant=false&From=" + FromMessage + "&Date=" + Date);

        var win = $window.createPopup(url, messaging.defaultPopup, function () { this.set_destroyOnClose(true); });
    }

</script>
