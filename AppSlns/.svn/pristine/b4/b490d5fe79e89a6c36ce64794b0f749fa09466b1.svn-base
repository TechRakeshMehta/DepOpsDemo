<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Messaging.Views.CommunicationSummary" CodeBehind="CommunicationSummary.ascx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxName1">
    <infs:LinkedResource Path="~/Resources/Mod/Communication/CommunicationSummary.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="section">

    <h1 class="mhdr">Communication History
    </h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel ID="pnlSearch" CssClass="sxpnl" runat="server">
                <div class='sxro sx3co'>
                    <div class='sxlb' title="Restrict search results to the entered Email Type">
                        <span class="cptn">Email Type</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtEmailType" runat="server">
                        </infs:WclTextBox> 
                    </div>
                    <div class='sxlb' title="Restrict search results to the entered Receiver">
                        <span class="cptn">Receiver</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtReceiver" runat="server">
                        </infs:WclTextBox> 
                    </div>
                    <div class='sxlb' title="Restrict search results to the entered Receiver Email-Id">
                        <span class="cptn">Receiver Email-Id</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtReceiverEmailId" runat="server">
                        </infs:WclTextBox>
                          <div class="vldx">
                            <asp:RegularExpressionValidator ID="revReceiverEmailId" runat="server" ErrorMessage="Enter AlphaNumeric Characters Only"
                                ValidationExpression="^[a-zA-Z0-9@._-]+$" ValidationGroup="grpFormSubmit" Enabled="true"
                                Display="Dynamic" ControlToValidate="txtReceiverEmailId" CssClass="errmsg">
                            </asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb' title="Restrict search results to the entered Recipient Type">
                        <span class="cptn">Recipient Type</span>
                    </div>
                    <div class='sxlm'>
                        <asp:RadioButtonList ID="rblRecipientType" runat="server" CssClass="radio_list" RepeatDirection="Horizontal" AutoPostBack="false">
                            <asp:ListItem Text="To" Value="To"></asp:ListItem>
                            <asp:ListItem Text="Bcc" Value="Bcc"></asp:ListItem>
                            <asp:ListItem Text="Cc" Value="Cc"></asp:ListItem>
                        </asp:RadioButtonList>

                    </div>
                    <div id="divSSN" runat="server">
                        <div class='sxlb' title="Restrict search results to the entered Dispatch Date ">
                            <span class="cptn">Dispatch Date</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclDatePicker ID="dpkrDispatchDate" runat="server" DateInput-EmptyMessage="Select a date" ClientEvents-OnPopupOpening="SetMinDate"
                                DateInput-DateFormat="MM/dd/yyyy">
                            </infs:WclDatePicker>
                        </div>
                    </div>
                    <div id="divDOB" runat="server">
                        <div class='sxlb' title="Restrict search results to the entered Subject">
                            <span class="cptn">Subject</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtSubject" runat="server">
                            </infs:WclTextBox> 
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="CmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Save,Cancel,Clear"
            AutoPostbackButtons="Submit,Save,Cancel,Clear" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
            ValidationGroup="grpFormSubmit"  OnSaveClick="CmdBarButton_SaveClick" CancelButtonText="Cancel" 
            OnCancelClick="CmdBarButton_CancelClick" ClearButtonIconClass="rbRefresh"
            ClearButtonText="Reset" OnClearClick="CmdBarButton_ClearClick">
        </infsu:CommandBar>

        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdCommunicationSummary" AllowPaging="True" AllowCustomPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0" GridLines="Both"
                ShowExtraButtons="False" NonExportingColumns="EmailSelectColumn,ViewContent" ShowAllExportButtons="false"
                AllowMultiRowSelection="true" MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false"
                MasterTableView-CommandItemSettings-ShowRefreshButton="false" OnNeedDataSource="grdCommunicationSummary_NeedDataSource"
                OnItemCommand="grdCommunicationSummary_ItemCommand" OnItemDataBound="grdCommunicationSummary_ItemDataBound" OnSortCommand="grdCommunicationSummary_SortCommand"
                OnInit="grdCommunicationSummary_Init"
                EnableLinqExpressions="true">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm"
                    Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="false">
                    <Selecting AllowRowSelect="true"></Selecting>
                    <ClientEvents OnCommand="grdCommunicationSummary_Command" OnRowCreated="grdCommunicationSummary_RowCreated"
                        OnRowSelected="grdCommunicationSummary_RowSelected" OnRowDeselected="grdCommunicationSummary_RowDeselected"
                        OnGridCreated="GridCreated" />
                </ClientSettings>
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView CommandItemDisplay="None" DataKeyNames="SystemCommunicationID,SystemCommunicationDeliveryID"
                    ClientDataKeyNames="SystemCommunicationID,SystemCommunicationDeliveryID">
                    <CommandItemSettings ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false" ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridClientSelectColumn UniqueName="EmailSelectColumn">
                        </telerik:GridClientSelectColumn>
                        <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter email type column"
                            HeaderText="Email Type" SortExpression="Name" UniqueName="Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SenderEmailID" FilterControlAltText="Filter sender email-id column"
                            HeaderText="Sender Email-Id" SortExpression="SenderEmailID" UniqueName="SenderEmailID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SenderName" FilterControlAltText="Filter sender name column"
                            HeaderText="Sender" SortExpression="SenderName" UniqueName="SenderName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RecieverEmailID" FilterControlAltText="Filter receiver email-id column"
                            HeaderText="Receiver Email-Id" SortExpression="RecieverEmailID" UniqueName="RecieverEmailID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RecieverName" FilterControlAltText="Filter receiver name column"
                            HeaderText="Receiver" SortExpression="RecieverName" UniqueName="RecieverName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RecipientType" FilterControlAltText="Filter RecipientType column"
                            HeaderText="Recipient Type" SortExpression="RecipientType" UniqueName="RecipientType">
                        </telerik:GridBoundColumn>

                        <telerik:GridDateTimeColumn DataField="DispatchedDate" FilterControlAltText="Filter DispatchDate date column"
                            HeaderText="Dispatch Date" SortExpression="DispatchedDate" UniqueName="DispatchedDate" DataFormatString="{0:d}" FilterControlWidth="100px">
                        </telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn HeaderText="Subject" DataField="Subject" FilterControlAltText="Filter subject column" SortExpression="Subject" UniqueName="Subject">
                        <%-- <ItemTemplate>
                         <asp:Label ID="lblSubject" ToolTip='<%#Eval("Subject") %>' runat="server" Text='<%# Convert.ToString(Eval("Subject")).Length > 75 ? Convert.ToString(Eval("Subject")).Substring(0,75) + "..." : Eval("Subject")%>'></asp:Label>
                         </ItemTemplate>--%>
                        </telerik:GridBoundColumn>
                        <telerik:GridButtonColumn ButtonType="LinkButton" UniqueName="ViewContent" CommandName="ViewContent" Text="View Content">
                        </telerik:GridButtonColumn>
                    </Columns>
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
    <infsu:CommandBar ID="fsucCmdBar1" runat="server" OnSubmitClick="btnSubmit_Click"
        OnSubmitClientClick="ValidateSelection" DisplayButtons="Submit" DefaultPanel="pnlName1"
        SubmitButtonText="Re-Send Email" ButtonPosition="Center">
    </infsu:CommandBar>
    <asp:HiddenField ID="hdfSelectedIds" runat="server" />
</div>

