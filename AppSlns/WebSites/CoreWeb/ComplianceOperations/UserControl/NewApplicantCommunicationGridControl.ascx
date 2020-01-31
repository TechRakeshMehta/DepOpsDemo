<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicantCommunicationGridControl.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ApplicantCommunicationGridControl" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">
    a.lnkViewAllRecentMessages
    {
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

        a.lnkViewAllRecentMessages:hover
        {
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
<div class="row">
    <div class='col-md-12'>
        <h2 class="header-color" tabindex="0">
            <asp:Label ID="lblCommunicationGrid" runat="server" Text="User Communication Grid"></asp:Label></h2>
    </div>
</div>
<div class="row">
    <div id="dvGoToComnCntr" runat="server">
        <%--<a href="#" id="lnkViewAllRecentMessages" class="lnkViewAllRecentMessages" title="Click here to go to the Communication Center" runat="server">Go To Communication Center</a>--%>
        <asp:LinkButton ID="lnkViewAllRecentMessages" Text="Go To Communication Center" runat="server" CssClass="lnkViewAllRecentMessages" ToolTip="Click here to go to the Communication Center" />
        <div class='sxroend'>
        </div>
    </div>
</div>
<div class="row">
    <div class='col-md-12'>
        <asp:RadioButtonList ID="rdlCommunicationMode" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" TabIndex="0"
            OnSelectedIndexChanged="rdlCommunicationMode_SelectedIndexChanged">
            <asp:ListItem Selected="True" Value="1" Text="Communication Center"></asp:ListItem>
            <asp:ListItem Value="2" Text="E-Mail"></asp:ListItem>
        </asp:RadioButtonList>
    </div>
</div>
<%--<div class="sxform auto">--%>
<div class="row">
    <div id="divCommMessageGrid" runat="server">
        <infs:WclGrid runat="server" ID="grdUserCommunicationGrid" AutoGenerateColumns="False" ShowClearFiltersButton="false" EnableAriaSupport="true" 
            MasterTableView-CommandItemSettings-ShowRefreshButton="false"
            AllowSorting="True" MasterTableView-AllowFilteringByColumn="false" MasterTableView-RowIndicatorColumn-ShowFilterIcon="false" 
            AutoSkinMode="True" CellSpacing="0" GridLines="Both" OnNeedDataSource="grdUserCommunicationGrid_NeedDataSource" 
            ShowAllExportButtons="False" AllowCustomPaging="true" OnSortCommand="grdUserCommunicationGrid_SortCommand">
            <GroupingSettings CaseSensitive="false" />
            <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="false">
                <Selecting AllowRowSelect="true"></Selecting>
                <ClientEvents OnRowDblClick="e_viewMessage" />
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" ClientDataKeyNames="MessageDetailID,IsHighImportant,CommunicationTypeCode" CommandItemSettings-ShowAddNewRecordButton="false">
                <Columns>
                    <telerik:GridBoundColumn DataField="From" HeaderText="From" SortExpression="From" UniqueName="From"
                        HeaderTooltip="This column displays the sender name">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Subject" HeaderText="Subject" SortExpression="Subject"
                        UniqueName="Subject" HeaderTooltip="This column displays the subject of the message">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ReceivedDateFormat" HeaderText="Receive Date" SortExpression="ReceivedDateFormat" UniqueName="ReceivedDateFormat"
                        HeaderTooltip="This column displays the Receive Date of the message">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="IsHighImportant" Display="false" DataField="IsHighImportant">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FromUserId" Display="false" DataField="FromUserId">
                    </telerik:GridBoundColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
        </infs:WclGrid>
    </div>

    <div id="divCommEmailGrid" runat="server">
        <infs:WclGrid runat="server" ID="grdUserEmailGrid" MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" AutoGenerateColumns="False" ShowClearFiltersButton="false" EnableAriaSupport="true" MasterTableView-CommandItemSettings-ShowRefreshButton="false"
            AllowSorting="True" MasterTableView-AllowFilteringByColumn="false" MasterTableView-RowIndicatorColumn-ShowFilterIcon="false" AutoSkinMode="True"
            CellSpacing="0" GridLines="Both" OnNeedDataSource="grdUserEmailGrid_NeedDataSource" OnItemCommand="grdUserEmailGrid_ItemCommand"
            ShowAllExportButtons="False" AllowCustomPaging="true" OnSortCommand="grdUserEmailGrid_SortCommand">
            <GroupingSettings CaseSensitive="false" />
            <ClientSettings EnableRowHoverStyle="false" EnablePostBackOnRowClick="false">
                <Selecting AllowRowSelect="true"></Selecting>
                <ClientEvents OnRowDblClick="grdUserEmailGrid_Command" OnCommand="grdUserEmailGrid_Command" />
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="SystemCommunicationId,EmailType,Subject,DispatchedDate" 
                ClientDataKeyNames="SystemCommunicationId,EmailType,DispatchedDate" AllowFilteringByColumn="false">
                <Columns>
                    <telerik:GridBoundColumn DataField="EmailType" HeaderText="Email Type" SortExpression="EmailType" UniqueName="EmailType"
                        HeaderTooltip="This column displays the Email Type">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Subject" HeaderText="Subject" SortExpression="Subject"
                        UniqueName="Subject" HeaderTooltip="This column displays the subject of the message">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="DispatchedDate" HeaderText="Dispatched Date" SortExpression="DispatchedDate" UniqueName="DispatchedDate"
                        HeaderTooltip="This column displays the Dispatched Date of the message">
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="ViewContent" Text="View Content" UniqueName="Resend">
                    </telerik:GridButtonColumn>
                    <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="Resend" Text="Resend" UniqueName="Resend">
                    </telerik:GridButtonColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
        </infs:WclGrid>
    </div>
</div>
<%--</div>--%>
<div id="Div1" class="approvepopup" runat="server" style="display: none">
    <div style="float: left; width: 50px">
        <img src="../../Resources/Themes/Default/images/info.png" />
    </div>
</div>
<script type="text/javascript">

    var e_viewMessage = function (sender, args) {
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

    var grdUserEmailGrid_Command = function (sender, args) {        
        if (args._domEvent != undefined && args._domEvent.type != undefined && args._domEvent.type == "dblclick") {
            var selectedRow = sender.get_masterTableView().get_dataItems()[args.get_itemIndexHierarchical()];
            var selectedId = args._dataKeyValues.SystemCommunicationId;
            var dispatchDate = sender.get_masterTableView().getCellByColumnUniqueName(selectedRow, "DispatchedDate").innerText; //sender.get_masterTableView().get_dataItems()[args.get_commandArgument()].getDataKeyValue("DispatchedDate");
            var url = $page.url.create("~/Messaging/Pages/EmailViewer.aspx?sysCommId=" + selectedId + "&Date=" + dispatchDate);
            var popupHeight = $jQuery(window).height() * (100 / 100);
            var win = $window.createPopup(url, { size: "650," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload || Telerik.Web.UI.WindowBehaviors.Resize });
        }
        else
            if (args.get_commandName().toLowerCase() == "viewcontent") {
                args.set_cancel(true);
                var selectedId = sender.get_masterTableView().get_dataItems()[args.get_commandArgument()].getDataKeyValue("SystemCommunicationId");
                var dispatchDate = sender.get_masterTableView().get_dataItems()[args.get_commandArgument()].getDataKeyValue("DispatchedDate");
                var url = $page.url.create("~/Messaging/Pages/EmailViewer.aspx?sysCommId=" + selectedId + "&Date=" + dispatchDate);
                var popupHeight = $jQuery(window).height() * (100 / 100);
                var win = $window.createPopup(url, { size: "650," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload || Telerik.Web.UI.WindowBehaviors.Resize });
            }
    }

    function ShowCallBackMessage(msg, msgClass) {
        if (typeof (msg) == "undefined") return;
        var c = typeof (msgClass) != "undefined" ? msgClass : "";
        if ($jQuery(".approvepopup").length > 0)
        {
            $jQuery("#pageMsgBox").children("span").text(msg).attr("class", msgClass);            
            if (c == 'sucs') {
                c = "Success";
            }
            else (c = c.toUpperCase());
            $jQuery("#pnlError").hide();
            $window.showDialog($jQuery("#pageMsgBox").clone().show(), {
                closeBtn: {
                    autoclose: true, text: "Ok", click: function () {
                        $jQuery("#pageMsgBox").children("span").text('').attr("class", "");
                        $jQuery("#pnlError").show();
                    }
                }
            }, 400, c);
        }
    }
</script>
