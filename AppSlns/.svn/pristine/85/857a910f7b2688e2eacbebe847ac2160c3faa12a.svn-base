<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Shell.Views.ToolBar" Codebehind="ToolBar.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxName1">
    <infs:LinkedResource Path="~/Resources/Mod/Shared/mod.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/mod.css" ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>
<asp:UpdatePanel ID="updQuickSearch" runat="server">
    <ContentTemplate>
        <infs:WclToolBar ID="tlbMain" runat="server" OnButtonClick="tblMain_ButtonClick">
            <Items>
                <telerik:RadToolBarButton EnableImageSprite="true" Text="" Group="Align" CheckOnClick="false"
                    Enabled="false">
                    <ItemTemplate>
                        <div class="tlbficon">
                            &nbsp;</div>
                    </ItemTemplate>
                </telerik:RadToolBarButton>
                <telerik:RadToolBarSplitButton CommandName="SearchOptionBar" EnableDefaultButton="true" DropDownWidth="180%">
                </telerik:RadToolBarSplitButton>
                <telerik:RadToolBarButton CommandName="SearchCriteria" runat="server">
                    <ItemTemplate>
                        <infs:WclTextBox ID="txtSearchCriteria" runat="server" EmptyMessage="Enter Criteria">
                            <ClientEvents OnLoad="navigateSearch" OnKeyPress="OnKeyPress" />
                        </infs:WclTextBox>
                    </ItemTemplate>
                </telerik:RadToolBarButton>
                <telerik:RadToolBarSplitButton EnableDefaultButton="true">
                    <Buttons>
                        <telerik:RadToolBarButton Text="Search" CommandName="QuickSearch" ToolTip="Quick Search" />
                        <telerik:RadToolBarButton Text="Advanced Search" CommandName="AdvanceSearch" ToolTip="Advanced Search" />
                    </Buttons>
                </telerik:RadToolBarSplitButton>
                <telerik:RadToolBarButton IsSeparator="true" />
                <telerik:RadToolBarButton>
                    <ItemTemplate>
                        <span style="font-style: italic;">&nbsp;Switch to:</span>
                    </ItemTemplate>
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton CommandName="LineOfBusiness" runat="server">
                    <ItemTemplate>
                        <infs:WclComboBox runat="server" DataTextField="Name" AutoPostBack="true" MarkFirstMatch="true" OnSelectedIndexChanged="cmbLineOfBusiness_Change"
                            DataValueField="SysXBlockId" ID="cmbLineOfBusiness">
                        </infs:WclComboBox>
                    </ItemTemplate>
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton IsSeparator="true" />
                <telerik:RadToolBarButton ImageUrl="~/Resources/Mod/Shared/images/note.png" Text="Notes" CommandName="Notes" Group="Align" />
                <telerik:RadToolBarButton IsSeparator="true" />
                <telerik:RadToolBarButton ImageUrl="~/Resources/Mod/Shared/images/tools.png" CommandName="Personalize" Text="Personalize"></telerik:RadToolBarButton>
                <telerik:RadToolBarButton ImageUrl="~/Resources/Mod/Shared/images/help.png" Text="Help" Group="Align" ToolTip="Help" CommandName="Help" runat="server" />                                
            </Items>
        </infs:WclToolBar>
        <asp:HiddenField ID="queryField" runat="server" EnableViewState="false" />
    </ContentTemplate>
</asp:UpdatePanel>
