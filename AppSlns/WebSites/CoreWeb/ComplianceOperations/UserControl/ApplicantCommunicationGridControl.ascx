<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicantCommunicationGridControl.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ApplicantCommunicationGridControl" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">
    a.lnkViewAllRecentMessages {
        background-color: #D2D3D5;
        border: thin solid #FFFFFF;
        border-radius: 3px;
        text-decoration: none;
        float: right;
        font-weight: bold;
        padding-bottom: 10px;
        padding-top: 10px;
        text-align: center;
        width: 20%;
        color: #fff;
    }

        a.lnkViewAllRecentMessages:hover {
            background-color: #8C1921;
            border: thin solid #FFFFFF;
            border-radius: 3px;
            text-decoration: none;
            float: right;
            font-weight: bold;
            padding-bottom: 10px;
            padding-top: 10px;
            text-align: center;
            width: 20%;
            color: #fff;
        }
</style>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblCommunicationGrid" runat="server" Text="<%$ Resources:Language, USRCOMMGRD %>"></asp:Label></h1>
    <div class="content">
        <div id="dvGoToComnCntr" runat="server">
            <%--<a href="#" id="lnkViewAllRecentMessages" class="lnkViewAllRecentMessages" title="Click here to go to the Communication Center" runat="server">Go To Communication Center</a>--%>
            <asp:LinkButton ID="lnkViewAllRecentMessages" Text="<%$ Resources:Language, GOTOCOMMCNTR %>" runat="server" CssClass="lnkViewAllRecentMessages" 
                ToolTip="<%$ Resources:Language, CLKTOGOCOMCNTR %>" />
            <div class='sxroend'>
            </div>
        </div>
        <%--<div class="sxform auto">--%>
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdUserCommunicationGrid" AutoGenerateColumns="False" ShowClearFiltersButton="false"
                AllowSorting="True" MasterTableView-AllowFilteringByColumn="false" MasterTableView-RowIndicatorColumn-ShowFilterIcon="false" 
                AutoSkinMode="True" CellSpacing="0" GridLines="Both" OnNeedDataSource="grdUserCommunicationGrid_NeedDataSource"
                ShowAllExportButtons="False" AllowCustomPaging="true" OnSortCommand="grdUserCommunicationGrid_SortCommand">
                <GroupingSettings CaseSensitive="false" />
                <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="false" >
                    <Selecting AllowRowSelect="true"></Selecting>
                    <ClientEvents OnRowDblClick="e_viewMessage" />
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" CommandItemSettings-RefreshText="<%$Resources:Language,REFRESH %>" ClientDataKeyNames="MessageDetailID,IsHighImportant,CommunicationTypeCode" CommandItemSettings-ShowAddNewRecordButton="false">
                    <Columns>
                        <telerik:GridBoundColumn DataField="From" HeaderText="<%$ Resources:Language, FROM %>" SortExpression="From" UniqueName="From"
                            HeaderTooltip="<%$ Resources:Language, DSPLYSNDRNAME %>">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Subject" HeaderText="<%$ Resources:Language, SUBJECT %>" SortExpression="Subject"
                            UniqueName="Subject" HeaderTooltip="<%$ Resources:Language, DSPLYSBJCTMSG %>">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ReceivedDateFormat" HeaderText="<%$ Resources:Language, RCVDATE %>" SortExpression="ReceivedDateFormat" UniqueName="ReceivedDateFormat"
                            HeaderTooltip="<%$ Resources:Language, DSPLYRCVDTMSG %>">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn UniqueName="IsHighImportant" Display="false" DataField="IsHighImportant">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn UniqueName="FromUserId" Display="false" DataField="FromUserId">
                        </telerik:GridBoundColumn>
                       
                    </Columns>
                    <PagerStyle Mode="NextPrevAndNumeric" NextPagesToolTip="<%$Resources:Language,NXTPAGE %>" PrevPagesToolTip="<%$Resources:Language,PREVPAGE %>" FirstPageToolTip="<%$Resources:Language,FIRSTPAGE %>" LastPageToolTip="<%$Resources:Language,LSTPAGE %>" AlwaysVisible="true" PageSizeLabelText="<%$Resources:Language,PAGESIZE %>" PagerTextFormat=" {4} {5} <%$Resources:Language,ITEMS%> <%$Resources:Language,IN%> {1} <%$Resources.Language.PAGE%>" />
                </MasterTableView>
                 <%--PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"--%>
                <%--PagerTextFormat="{4} {5} <%$Resources:Language,ITEMS%> <%$Resources:Language,IN%> {1} <%$Resources.Language.PAGE%>" />--%>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            </infs:WclGrid>
        </div>
        <%--</div>--%>
    </div>
</div>


<script type="text/javascript">

    var e_viewMessage = function (sender, args) {
        //debugger;
        var selectedRow = sender.get_masterTableView().get_dataItems()[args.get_itemIndexHierarchical()];
        var messageFrom = sender.get_masterTableView().getCellByColumnUniqueName(selectedRow, "From").innerText;
        var messageDate = sender.get_masterTableView().getCellByColumnUniqueName(selectedRow, "ReceivedDateFormat").innerText;
        var communicationTypeCode = args._dataKeyValues.CommunicationTypeCode;
        var messageImportance = args._dataKeyValues.IsHighImportant;

        // $jQuery("[id$=hdnSelectedMessage]").val(args._dataKeyValues.MessageDetailID);
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/Messaging/Pages/MessageViewer.aspx?messageID=" + args._dataKeyValues.MessageDetailID + "&isImportant=" + messageImportance +
        "&From=" + messageFrom + "&Date=" + messageDate + "&isDashboardMessage=" + true + "&cType=" + communicationTypeCode);
        var win = $window.createPopup(url, { size: "800," + popupHeight, onclose: OnClientClose });
    }

    function OnClientClose() {
        //top.location.href = top.location.href;
    }

</script>
