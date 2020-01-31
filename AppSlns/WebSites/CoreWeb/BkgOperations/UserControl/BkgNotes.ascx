<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BkgNotes.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.BkgNotes" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxManageInvitationExpiration">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
 
<div class="container-fluid">
    <div class="msgbox" id="divSuccessMsg">
        <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
    </div> 
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <asp:Label ID="lblNewHeading" Text='Add New Note'
                    runat="server"></asp:Label></h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <div class="msgbox">
            <asp:Label ID="lblContactSuccess" runat="server"></asp:Label>
        </div>
        <asp:Panel runat="server" ID="pnlMNew">
            <div class='form-group col-md-12'>
                <asp:Label ID="lblNewNote" Text="New Note" runat="server" CssClass="cptn" /><span
                    class="reqd">*</span>
                <infs:WclTextBox ID="txtNewNote" CssClass="borderTextArea" Width="100%" runat="server"
                    TextMode="MultiLine" MaxLength="500">
                </infs:WclTextBox>
                <div class='vldx'>
                    <asp:RequiredFieldValidator runat="server" ID="rfvNewNote" ControlToValidate="txtNewNote"
                        Display="Dynamic" class="errmsg" ErrorMessage="New Note is required." ValidationGroup='grpValdNotes' />
                    <asp:RegularExpressionValidator runat="server" ID="revNewNote" ControlToValidate="txtNewNote"
                        Display="Dynamic" CssClass="errmsg" ValidationExpression="^[&quot;\w\d\s\-\.\,\[\]\(\)\{\}\:\,\،\、\‒\–\—\―\…\!\‐\-\?\‘\’\“\”\'\'\;\\\<\>\/\~\@]{1,500}$"
                        ValidationGroup='grpValdNotes'
                        ErrorMessage="Invalid character(s)." />
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="row text-center">
        <infs:WclButton runat="server" Skin="Silk" AutoSkinMode="false" ID="btnAdd" Text="Add Note"
            OnClick="CmdBarCancel_Click" ValidationGroup="grpValdNotes">
            <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                PrimaryIconWidth="14" />
        </infs:WclButton>
    </div>
    <div class="row">
        <div class="col-md-12">
            <h1 class="header-color">Notes
            </h1>
        </div>
    </div>
    <div class="row">
        <div class="msgbox">
            <asp:Label ID="Label1" runat="server" CssClass="info">
            </asp:Label>
        </div>
        <div id="Div2" runat="server">
            <infs:WclGrid runat="server" ID="grdNotes" CssClass="removeExtraSpace" AutoGenerateColumns="false"
                AllowSorting="True" AutoSkinMode="True" CellSpacing="0"
                GridLines="Both" ShowAllExportButtons="False" ShowExtraButtons="True" OnNeedDataSource="grdNotes_NeedDataSource"
                AllowPaging="false" EnableDefaultFeatures="false">
                <MasterTableView CommandItemDisplay="Top">
                    <ItemStyle Wrap="true" />
                    <CommandItemSettings ShowAddNewRecordButton="false"
                        ShowExportToExcelButton="False" ShowExportToPdfButton="False" ShowExportToCsvButton="False"
                        ShowRefreshButton="False" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="Note" FilterControlAltText="Filter Node Text column"
                            DataFormatString="<nobr>{0}</nobr>" ItemStyle-Width="150px"
                            HeaderText="Note Text" SortExpression="Note" UniqueName="Note">
                            <ItemStyle Wrap="true" Width="600px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserName" FilterControlAltText="Filter Created By column"
                            HeaderText="Created By" SortExpression="UserName" UniqueName="UserName">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="CreatedOnDate" FilterControlAltText="Filter Created On column"
                            HeaderText="Created On" SortExpression="CreatedOnDate" UniqueName="CreatedOnDate">
                            <ItemTemplate>
                                <asp:Label ID="lblGridOrderStatus" runat="server" Text='<%# Convert.ToString(Convert.ToDateTime(Eval("CreatedOnDate")))  %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </infs:WclGrid>
        </div>
    </div>
</div>

