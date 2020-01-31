<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharedUserSearchDetails.ascx.cs" Inherits="CoreWeb.SearchUI.Views.SharedUserSearchDetails" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxCompliancePackageDetails">
    <infs:LinkedResource Path="~/Resources/Mod/SearchUI/SharedUserSearchDetail.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblHeader" runat="server" Text="Invitation Details"></asp:Label>
    </h1>
    <div class="content">
        <asp:Panel ID="pnlSharedUserInvitationDetails" runat="server">
            <div class="sxform auto">
                <infs:WclGrid runat="server" ID="grdInvitationDetails" AllowPaging="false" PageSize="10"
                    AutoGenerateColumns="False" AllowSorting="false" GridLines="Both" ShowAllExportButtons="False" AllowFilteringByColumn="false"
                    OnNeedDataSource="grdInvitationDetails_NeedDataSource" ShowClearFiltersButton="false" EnableDefaultFeatures="false"
                    OnItemCommand="grdInvitationDetails_ItemCommand" OnItemDataBound="grdInvitationDetails_ItemDataBound">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="InvitationID,TenantID" AllowFilteringByColumn="false">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true"
                            ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"
                            ShowExportToWordButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="InvitationID" HeaderText="Invitation ID" UniqueName="InvitationID">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TenantName" HeaderText="Institution Name" UniqueName="TenantName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AgencyName" HeaderText="Agency Name" UniqueName="AgencyName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ApplicantUserID" HeaderText="Applicant User ID" UniqueName="ApplicantUserID">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ApplicantName" HeaderText="Applicant Name" UniqueName="ApplicantName">
                            </telerik:GridBoundColumn>
                            <telerik:GridDateTimeColumn DataField="InvitationDate" HeaderText="Invitation Date" UniqueName="InvitationDate" DataFormatString="{0:MM/dd/yyyy}">
                            </telerik:GridDateTimeColumn>
                            <telerik:GridDateTimeColumn DataField="InvitationSourceCode" HeaderText="Invite Type" UniqueName="InviteType">
                            </telerik:GridDateTimeColumn>

                            <telerik:GridTemplateColumn DataField="InvitationSentStatus" HeaderText="Invitation Sent Status" UniqueName="InvitationSentStatus">
                                <ItemTemplate>
                                    <%# Convert.ToBoolean(Eval("InvitationSentStatus")) ? "Sent" : "Failed"  %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="ViewedStatus" HeaderText="Viewed Status" UniqueName="ViewedStatus">
                                <ItemTemplate>
                                    <%# Convert.ToBoolean(Eval("ViewedStatus")) ? "Viewed" : "Not Viewed"  %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Attestation Document" UniqueName="AttestationDocument">
                                <ItemTemplate>
                                    <div id="dvAttestationDoc" style="text-align: Left;">
                                        <asp:LinkButton runat="server" Visible='<%# Eval("InvitationSourceCode").ToString().Trim()!= ApplicantInvitationSourceCode ? true : false %>' ID="btnAttestationDocument" Text="Attestation Document"
                                            AutoPostBack="false" OnClientClick="DownloadAttestationDocument(this); return false;"
                                            invitationid='<%# Eval("InvitationID") %>' BackColor="Transparent" Font-Underline="true"
                                            BorderStyle="None" ForeColor="Black">
                                        </asp:LinkButton>
                                        <asp:HiddenField ID="hdnTenantID" runat="server" Value='<%# Eval("TenantID") %>' />
                                    </div>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <NestedViewTemplate>
                            <div class="swrap">
                                <infs:WclGrid runat="server" ID="grdSharedPackages" AllowPaging="false"
                                    AutoGenerateColumns="False" AllowSorting="false" GridLines="Both" ShowClearFiltersButton="false"
                                    OnNeedDataSource="grdSharedPackages_NeedDataSource" OnItemDataBound="grdSharedPackages_ItemDataBound"
                                    ShowAllExportButtons="false" AllowFilteringByColumn="false"
                                    PagerStyle-ShowPagerText="false" EnableDefaultFeatures="false" OnItemCommand="grdSharedPackages_ItemCommand">
                                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrderID,PackageID,PackageIdentifier, PackageTypeCode"
                                        AllowFilteringByColumn="false">
                                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                                            ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="OrderNumber" HeaderText="Order ID" UniqueName="OrderID">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="PackageName" HeaderText="Package Name" UniqueName="PackageName">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="PackageTypeCode" HeaderText="Package Type" UniqueName="PackageTypeCode">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn HeaderText="Passport Report" UniqueName="PassportReport">
                                                <ItemTemplate>
                                                    <div style="text-align: Left;">
                                                        <asp:LinkButton runat="server" ID="btnPassportReport" Text="Passport/Summary Report"
                                                            CommandName="ViewPassportReport" AutoPostBack="false" BackColor="Transparent" Font-Underline="true"
                                                            BorderStyle="None" ForeColor="Black" packagesubscriptionid='<%# Eval("PackageSubscriptionID") %>'
                                                            snapshotid='<%# Eval("SnapShotID") %>'>
                                                        </asp:LinkButton>
                                                        <%--<asp:HiddenField ID="hdnPackageSubscriptionID" runat="server" Value='<%# Eval("PackageSubscriptionID") %>' />--%>
                                                    </div>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Background Order Status">
                                                <ItemTemplate>
                                                    <div class="icon flag" style="text-align: left">
                                                        <asp:Image ID="imgOrderFlag" Visible='<%# Eval("ColorFlagPath") == null ? false : true %>' ImageUrl=' <%# Eval("ColorFlagPath") == null ? "" : Eval("ColorFlagPath").ToString()  %>' runat="server" Height="20px" Width="20px" />
                                                        <asp:Label ID="lblFlagText" Visible='<%# Convert.ToBoolean(Eval("ShowFlagText")) ? true : false %>' runat="server" CssClass="imageText" Text="N/A"></asp:Label>
                                                        <asp:Image ID="imgFlagStatus" Visible='<%# Eval("FlagStatusImagePath") == null ? false : true %>' ImageUrl=' <%# Eval("FlagStatusImagePath") == null ? "" : Eval("FlagStatusImagePath").ToString() %>' runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                        <NestedViewTemplate>
                                            <div class="swrap">
                                                <infs:WclGrid runat="server" ID="grdSharedEntity" AllowPaging="false"
                                                    AutoGenerateColumns="False" AllowSorting="false" GridLines="Both" ShowClearFiltersButton="false"
                                                    OnNeedDataSource="grdSharedEntity_NeedDataSource" OnItemCommand="grdSharedEntity_ItemCommand"
                                                    ShowAllExportButtons="false" AllowFilteringByColumn="false" PagerStyle-ShowPagerText="false" EnableDefaultFeatures="false">
                                                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="SharedEntityID"
                                                        AllowFilteringByColumn="false">
                                                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                                                            ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="SharedEntityName"
                                                                HeaderText="Shared Entity Name" UniqueName="SharedEntityName">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn UniqueName="ResultReport">
                                                                <ItemTemplate>
                                                                    <div id="dvResultReport" style="text-align: Left;">
                                                                        <asp:LinkButton runat="server" ID="btnResultReport" Text="Result Report"
                                                                            AutoPostBack="false" Visible='<%# Eval("IsResultReportVisible") %>'
                                                                            CommandName="ResultReport"
                                                                            BackColor="Transparent" Font-Underline="true"
                                                                            BorderStyle="None" ForeColor="Black">
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>

                                                    </MasterTableView>
                                                </infs:WclGrid>
                                            </div>
                                        </NestedViewTemplate>
                                    </MasterTableView>
                                </infs:WclGrid>
                            </div>
                        </NestedViewTemplate>
                    </MasterTableView>
                </infs:WclGrid>
                <div class="gclr">
                </div>
            </div>
        </asp:Panel>
        <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Cancel"
            AutoPostbackButtons="Cancel" CancelButtonText="Back to Search" ValidationGroup="grpFormSubmit"
            OnCancelClick="fsucCmdBarButton_CancelClick" CancelButtonIconClass="rbPrevious">
        </infsu:CommandBar>
    </div>
    <iframe id="ifrExportDocument" runat="server" height="0" width="0" src=""></iframe>
</div>

