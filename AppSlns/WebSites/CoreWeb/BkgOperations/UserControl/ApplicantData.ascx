<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicantData.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.ApplicantData" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script type="text/javascript">
    function GridCreated(sender, args) {
        var scrollArea = sender.GridDataDiv;
        var scrollHeightStr = sender.ClientSettings.Scrolling.ScrollHeight;
        var scrollHeight = scrollHeightStr.substring(0, scrollHeightStr.lastIndexOf("px"));
        var dataHeight = sender.get_masterTableView().get_element().clientHeight;
        if (dataHeight < scrollHeight) {
            scrollArea.style.height = dataHeight + 5 + "px";
        }
    }
    function ShowResultDetail() {
        var composeScreenWindowName = "Nationwide Criminal Results";
        var url = $page.url.create("~/BkgOperations/Pages/ShowResultData.aspx");
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var win = $window.createPopup(url, {
            size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnClose
        },
                                                     function () {
                                                         this.set_title(composeScreenWindowName);
                                                     });
        winopen = true;
        return false;
    }

    function OnClose(oWnd, args)
    {
        oWnd.remove_close(OnClientClose);
        winopen = false;
    }
</script>

<asp:Panel ID="pnlAlias" runat="server" Visible="false">
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="Label1" runat="server" Text="Applicant's Alias/Maiden"></asp:Label></h1>
        <div class="content">
            <div class="sxform auto">
                <infs:WclGrid runat="server" ID="grdAliasNames" AllowPaging="false" PageSize="10"
                    AutoGenerateColumns="False" AllowSorting="false" GridLines="Both" ShowAllExportButtons="False" AllowFilteringByColumn="false"
                    ShowClearFiltersButton="false" EnableDefaultFeatures="false">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="None" AllowFilteringByColumn="false">
                        <Columns>
                            <telerik:GridTemplateColumn DataField="FirstName" UniqueName="FirstName"
                                HeaderText="Alias/Maiden First Name">
                                <ItemTemplate>
                                    <asp:Literal ID="litFirstName" runat="server" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("FirstName")) )%>'></asp:Literal>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="MiddleName" UniqueName="MiddleName"
                                HeaderText="Alias/Maiden Middle Name">
                                <ItemTemplate>
                                    <asp:Literal ID="litMiddleName" runat="server" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("MiddleName")) )%>'></asp:Literal>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="LastName" UniqueName="LastName"
                                HeaderText="Alias/Maiden Last Name">
                                <ItemTemplate>
                                    <asp:Literal ID="litLastName" runat="server" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("LastName")) )%>'></asp:Literal>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="IsUsed" UniqueName="IsUsed"
                                HeaderText="Is Alias Search">
                                <ItemTemplate>
                                    <asp:Literal ID="litIsUsed" runat="server" Text='<%# Convert.ToBoolean(Eval("IsUsed")) ? "Yes": "No"%>'></asp:Literal>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </infs:WclGrid>
            </div>
        </div>
    </div>
</asp:Panel>

<asp:Panel ID="pnlResidentialHistory" runat="server" Visible="false">
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="Label2" runat="server" Text="Applicant's Residential History(s)"></asp:Label></h1>
        <div class="content">
            <div class="sxform auto">
                <infs:WclGrid runat="server" ID="grdResidentialHistory" AllowPaging="false" PageSize="10"
                    AutoGenerateColumns="False" AllowSorting="false" GridLines="Both" ShowAllExportButtons="False" AllowFilteringByColumn="false"
                    ShowClearFiltersButton="false" EnableDefaultFeatures="false">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="None" AllowFilteringByColumn="false">
                        <Columns>
                            <telerik:GridTemplateColumn DataField="StateName" UniqueName="StateName"
                                HeaderText="State Name">
                                <ItemTemplate>
                                    <asp:Literal ID="litStateName" runat="server" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("StateName")) )%>'></asp:Literal>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="CountyName" UniqueName="CountyName"
                                HeaderText="County Name">
                                <ItemTemplate>
                                    <asp:Literal ID="litCountyName" runat="server" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CountyName")) )%>'></asp:Literal>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="IsStateSearch" UniqueName="IsStateSearch"
                                HeaderText="Is State search">
                                <ItemTemplate>
                                    <asp:Literal ID="litStateSearch" runat="server" Text='<%# Convert.ToBoolean(Eval("IsStateSearch")) ? "Yes": "No"%>'></asp:Literal>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="IsCountySearch" UniqueName="IsCountySearch"
                                HeaderText="Is County search">
                                <ItemTemplate>
                                    <asp:Literal ID="litCountySearch" runat="server" Text='<%# Convert.ToBoolean(Eval("IsCountySearch")) ? "Yes": "No"%>'></asp:Literal>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </infs:WclGrid>
            </div>
        </div>
    </div>
</asp:Panel>

<asp:Panel ID="pnlSSN" runat="server" Visible="false">
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblSSNPanel" runat="server" Text="SSN Results"></asp:Label></h1>
        <div class="content">
            <div class="sxform auto" id="gridContainer">
                <infs:WclGrid ID="grdSSNResults" runat="server" AutoGenerateColumns="false" OnItemDataBound="grdSSNResults_ItemDataBound"
                    AllowPaging="False" AllowSorting="false" ClientSettings-EnableRowHoverStyle="false"
                    HeaderStyle-HorizontalAlign="Center" GridLines="None"
                    Visible="true" Enabled="true" EnableDefaultFeatures="false">
                    <ClientSettings EnableRowHoverStyle="false">
                        <Selecting AllowRowSelect="false"></Selecting>
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="150px" />
                        <ClientEvents OnGridCreated="GridCreated" />
                    </ClientSettings>
                    <MasterTableView EditMode="PopUp" CommandItemDisplay="None">
                        <HeaderStyle />
                        <Columns>
                            <telerik:GridBoundColumn HeaderText="SSN Trace Results" HeaderButtonType="TextButton" DataField="Key" />
                            <telerik:GridBoundColumn HeaderText="Value" HeaderButtonType="TextButton" Visible="false" DataField="Key" />
                        </Columns>
                    </MasterTableView>
                </infs:WclGrid>
            </div>
        </div>
    </div>
</asp:Panel>

<asp:Panel ID="pnlNationwideCriminalSrch" runat="server" Visible="false">
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="LblNtnlCrimnalSrch" runat="server" Text="National Criminal Search Results"></asp:Label></h1>
        <div class="content">
            <div class="sxform auto">
                <telerik:RadGrid ID="grdNationwideCriminalSrchResults" runat="server" AutoGenerateColumns="false"
                    AllowPaging="False" AllowSorting="false" ClientSettings-EnableRowHoverStyle="true"
                    HeaderStyle-HorizontalAlign="Center" GridLines="None"
                    Visible="true" Enabled="true" EnableDefaultFeatures="false">
                    <MasterTableView CommandItemDisplay="None">
                        <HeaderStyle />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Nationwide Criminal Results" HeaderButtonType="TextButton" DataField="Key">
                                <ItemTemplate>
                                    <asp:Literal ID="lit" runat="server" Text='<%#Eval("Key") %>'></asp:Literal>
                                    <%--<div id="divText" runat="server">'<%#Eval("Key") %>'</div>--%>
                                    <%--<textarea id="txtArea" runat="server" style="width:100%;height:50px;">'<%#Eval("Key") %>'</textarea>--%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--<telerik:GridBoundColumn HeaderText="Nationwide Criminal Results" HeaderButtonType="TextButton" DataField="Key" />--%>
                            <telerik:GridBoundColumn HeaderText="Value" HeaderButtonType="TextButton" Visible="false" DataField="Key" />
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="150px" />
                        <ClientEvents OnGridCreated="GridCreated" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
        </div>
        <asp:LinkButton ID="lnkBtnResultDetail" runat="server" Text="View Results" OnClientClick="ShowResultDetail();return false;"></asp:LinkButton>
    </div>
</asp:Panel>
