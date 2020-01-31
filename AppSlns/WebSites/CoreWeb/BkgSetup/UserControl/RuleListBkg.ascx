<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RuleListBkg.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.RuleListBkg"
    Title="RuleList" %>


<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagName="RuleDetail" TagPrefix="infsu" Src="~/BkgSetup/UserControl/RuleInfoBkg.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/mapping_pages.css" ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>
<div class="page_cmd">
    <infs:WclButton runat="server" ID="btnAdd" Text="+ Add Rule"
        Height="30px" ButtonType="LinkButton" OnClick="btnAdd_Click">
    </infs:WclButton>
</div>
<div class="section" id="divAddForm" runat="server" visible="false">
    <infsu:RuleDetail ID="ucRuleDetail" runat="server"></infsu:RuleDetail>
</div>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblTitle" Text="Rules" runat="server"></asp:Label>
    </h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdRules" AllowPaging="false" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="false"
                GridLines="None" OnNeedDataSource="grdRules_NeedDataSource" OnItemCommand="grdRules_ItemCommand">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BRLM_ID">
                    <CommandItemSettings ShowAddNewRecordButton="false" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Rule Name" DataField="BRLM_Name" SortExpression="BRLM_Name" UniqueName="BRLM_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Rule Type" DataField="lkpBkgRuleType.BRLT_Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Return Type" DataField="BkgRuleTemplate.lkpBkgRuleResultType.BRSL_Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Action Type" DataField="lkpBkgRuleActionType.BACT_Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="BRLM_IsActive" HeaderText="Is Active"
                            SortExpression="BRLM_IsActive" UniqueName="BRLM_IsActive">
                            <ItemTemplate>
                                <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("BRLM_IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Rule?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                    </EditFormSettings>
                </MasterTableView>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>

