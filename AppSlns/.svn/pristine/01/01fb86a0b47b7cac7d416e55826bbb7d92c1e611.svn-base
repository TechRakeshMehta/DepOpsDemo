<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceOperations.Views.ItemDetails" Codebehind="ItemDetails.ascx.cs" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagName="AttributeControl" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/AttributeControl.ascx" %>
<%@ Register TagName="RowControl" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/RowControl.ascx" %>
<%@ Import Namespace="CoreWeb.ComplianceOperations.Views" %>
<%--
<asp:Panel runat="server" ID="ReportPanel" Visible="false">
    <div class="section">
        <h1 class="mhdr">
            <asp:Label Text="Manage Item" runat="server" ID="lblReportItemName" />
            <asp:Image ID="Image1" ImageUrl="~/Resources/Mod/Compliance/icons/yes16.png" runat="server" />
        </h1>
        <div class="content">
            <div>
                <span>Compliance Status</span>&nbsp;<span style="font-weight: bold; color: green">Compliant</span></div>
            <div>
                <span>Start Date</span>&nbsp;<span style="font-weight: bold">May 5, 2013</span></div>
            <div>
                <span>Provider Name</span>&nbsp;<span style="font-weight: bold">Dr. ABC</span></div>
        </div>
    </div>
</asp:Panel>--%>
<asp:Panel runat="server" ID="FormPanel" Visible="true">
    <div class="section">
        <h1 class="mhdr">
            <asp:Label Text="Manage Item" runat="server" ID="lblFormItemName" /></h1>
        <%--<div class="content">
            <div class="sxform auto">
                <div class="sxpnl">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            Start Date
                        </div>
                        <div class='sxlm'>
                            <infs:WclDatePicker ID="dpkrDate" runat="server" AutoPostBack="true" DateInput-EmptyMessage="Select a date">
                                <Calendar>
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                    </SpecialDays>
                                </Calendar>
                            </infs:WclDatePicker>
                        </div>
                        <div class='sxlb'>
                            Provider Name
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtName">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            Documents
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="WclComboBox1" runat="server" DataSourceID="xdtsDocuments" DataTextField="Name"
                                AllowCustomText="false" HighlightTemplatedItems="true" ShowMoreResultsBox="true"
                                CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
                                <HeaderTemplate>
                                    Existing Documents
                                </HeaderTemplate>
                            </infs:WclComboBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </div>
            </div>
            <infsu:CommandBar ID="fsucCmdBar1" runat="server" DefaultPanel="pnlName1" DisplayButtons="Save"
                ButtonPosition="Center" />
        </div>--%>
        <div class="content">
            <div class="sxform auto">
                <div class="sxpnl">
                    <asp:Panel ID="pnl" runat="server">
                    </asp:Panel>
                </div>
            </div>
            <infsu:CommandBar ID="fsucCmdBar1"  OnSaveClick="btnSave_Click" runat="server" DefaultPanel="pnlName1"
                DisplayButtons="Save" ButtonPosition="Center" AutoPostbackButtons="Save" />
        </div>
    </div>

     <asp:Label ID="lbl" runat="server"> </asp:Label>
     <asp:HiddenField ID="hdfApplicantItemDataId" runat="server" />
</asp:Panel>
