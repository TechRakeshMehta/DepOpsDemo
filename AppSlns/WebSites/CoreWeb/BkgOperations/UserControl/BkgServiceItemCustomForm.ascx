<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BkgServiceItemCustomForm.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.BkgServiceItemCustomForm" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<%@ Register TagPrefix="uc" TagName="ApplicantDetails" Src="~/BkgOperations/UserControl/ApplicantDetails.ascx" %>
<%@ Register TagPrefix="uc" TagName="ApplicantData" Src="~/BkgOperations/UserControl/ApplicantData.ascx" %>

<uc:ApplicantDetails ID="ucApplicantDetails" runat="server" />

<%--<div id="dvSSNPanel" style="display: none;" runat="server">
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblSSNPanel" runat="server" Text="SSN Results"></asp:Label></h1>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel ID="Panel2" CssClass="sxpnl" runat="server">
                    <infs:WclGrid ID="radGridOrderSSNResults" runat="server" AutoGenerateColumns="false"
                        AllowPaging="False" AllowSorting="false" GroupingEnabled="False" ClientSettings-EnableRowHoverStyle="true"
                        ValidationSettings-EnableValidation="true" AllowAutomaticUpdates="false" AllowAutomaticInserts="false"
                        AllowAutomaticDeletes="false" HeaderStyle-HorizontalAlign="Center" GridLines="None"
                        Visible="true" Enabled="true" EnableDefaultFeatures="false">
                        <MasterTableView EditMode="PopUp" CommandItemDisplay="None">
                            <HeaderStyle />
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="SSN Trace Results" HeaderButtonType="TextButton" DataField="Key" />
                                <telerik:GridBoundColumn HeaderText="Value" HeaderButtonType="TextButton" Visible="false" DataField="Key" />
                            </Columns>
                        </MasterTableView>
                    </infs:WclGrid>
                </asp:Panel>
            </div>
        </div>
    </div>
</div>
<div id="dvNtnlCrimnalSrch" style="display: none;" runat="server">
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="LblNtnlCrimnalSrch" runat="server" Text="Natinal Criminal Search Results"></asp:Label></h1>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel ID="Panel3" CssClass="sxpnl" runat="server">
                    <telerik:RadGrid ID="radGridOrderNationwideCriminalResults" runat="server" AutoGenerateColumns="false"
                        AllowPaging="False" AllowSorting="True" GroupingEnabled="False" ClientSettings-EnableRowHoverStyle="true"
                        ValidationSettings-EnableValidation="true" AllowAutomaticUpdates="false" AllowAutomaticInserts="false"
                        AllowAutomaticDeletes="false" HeaderStyle-HorizontalAlign="Center" GridLines="None"
                        Visible="true" Enabled="true" EnableDefaultFeatures="false">
                        <MasterTableView EditMode="PopUp" CommandItemDisplay="None">
                            <HeaderStyle />
                            <Columns>
                                <telerik:GridBoundColumn SortExpression="Key" HeaderText="Nationwide Criminal Results" HeaderButtonType="TextButton" DataField="Key" />
                                <telerik:GridBoundColumn SortExpression="Value" HeaderText="Value" HeaderButtonType="TextButton" Visible="false" DataField="Key" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </asp:Panel>
            </div>
        </div>
    </div>
</div>--%>

<uc:ApplicantData id="ucApplicantData" runat="server"></uc:ApplicantData>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblVerificationQueue" runat="server" Text=""></asp:Label></h1>
    <div class="content">

        <div class="sxform auto">
            <asp:Panel ID="Panel1" CssClass="sxpnl" runat="server">
                <div class='sxro sx2co'>
                    <div class="sxlb">
                        <span class='cptn'>Select Services</span><span class="reqd">*</span>
                    </div>
                    <div class="sxlm">
                        <%--<infs:WclComboBox ID="cmbServices" EmptyMessage="--Select--" AutoPostBack="true" runat="server" OnSelectedIndexChanged="cmbServices_SelectedIndexChanged"></infs:WclComboBox>--%>
                        <infs:WclComboBox ID="cmbServices" EmptyMessage="--Select--" runat="server" CheckBoxes="true"></infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvServiceName" ControlToValidate="cmbServices"
                                Display="Dynamic" ValidationGroup="grpServc" CssClass="errmsg" Text="Please select Service." />
                        </div>
                    </div>
                   <%-- <div class="sxlb">
                        <span class='cptn'>Select Service Item</span><span class="reqd">*</span>
                    </div>
                    <div class="sxlm">
                        <infs:WclComboBox ID="cmbServiceItem" EmptyMessage="--Select--" CheckBoxes="true" runat="server"></infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbServiceItem"
                                Display="Dynamic" ValidationGroup="grpServc" CssClass="errmsg" Text="Please select Service." />
                        </div>
                    </div>--%>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>

<div style="margin-top: 17px;">
    <infsu:CommandBar ID="fsucCmdBar1" runat="server" ButtonPosition="Center" DisplayButtons="Save,Cancel"
        AutoPostbackButtons="Save,Cancel" CauseValidationOnCancel="true"
        SaveButtonText="Cancel" CancelButtonIconClass="rbNext" CancelButtonText="Start Supplement" SaveButtonIconClass="rbPrevious"
        OnSaveClick="CmdBarCancel_Click" OnCancelClick="CmdBarSave_Click">
    </infsu:CommandBar>
</div>

