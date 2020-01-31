<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequirementRotations.ascx.cs" Inherits="CoreWeb.ApplicantRotationRequirement.Views.RequirementRotations" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Import Namespace="INTSOF.Utils" %>

<style>
    .sxlm .radio_list input, .sxlm .radio_list label
    {
        padding: 0!important;
        margin-top: 1px !important;
        vertical-align: top!important;
    }

    .rbLinkButton
    {
        height: auto !important;
    }
</style>
<script type="text/javascript">
    //function alertmessage() {
    //    alert(1);
    //}
    //click on link button while double click on any row of grid.
    function grd_rwDbClick(s, e) {
        var _id = "btnViewDetails";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }
</script>
<asp:Panel ID="pnlMain" runat="server" Width="100%" Height="100%">
    <h1 class="mhdr">
        <asp:Label runat="server" ID="lblHeader" Text="Clinical Rotations">
        </asp:Label>
    </h1>
    <div class="section">
        <div class="content">
            <div class="msgbox" id="msgBox" runat="server">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <div class="sxform auto">
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlRotations">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class='cptn' title="Agency">Agency</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="cmbAgency" runat="server"
                                DataTextField="AgencyName" DataValueField="AgencyID">
                            </infs:WclComboBox>
                            <div class="vldx">
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class='cptn'>Department</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtDepartment" runat="server"></infs:WclTextBox>
                            <div class="vldx">
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class='cptn'>Program</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtProgram" runat="server"></infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>

                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class='cptn'>Course</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtCourse" runat="server"></infs:WclTextBox>
                        </div>

                        <div class='sxlb'>
                            <span class='cptn' title="Instructor/Preceptor">Instructor/Preceptor</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="cmbClientContacts" runat="server" DataValueField="ClientContactID" DataTextField="Name"></infs:WclComboBox>
                            <div class="vldx">
                            </div>
                        </div>
                        <div class='sxlb'>
                            Status
                        </div>
                        <div class='sxlm'>
                            <asp:RadioButtonList ID="rbtnListStatus" runat="server" CssClass="radio_list" RepeatDirection="Horizontal" Width="175px" AutoPostBack="false">
                                <asp:ListItem Text="All" Value="all" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Active" Value="active"></asp:ListItem>
                                <asp:ListItem Text="In-active" Value="inactive"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>

                </asp:Panel>
                <infsu:CommandBar ID="cmdBarSearch" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save"
                    AutoPostbackButtons="Submit,Save" SubmitButtonIconClass="rbRefresh" SubmitButtonText="Reset"
                    SaveButtonText="Search" SaveButtonIconClass="rbSearch"
                    OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click">
                </infsu:CommandBar>
            </div>


        </div>
    </div>
    <asp:Panel ID="pnlInvitationsList" runat="server">
        <infs:WclGrid ID="grdRequirementRotations" AutoGenerateColumns="false" runat="server" AutoSkinMode="True" CellSpacing="0"
            EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="true" ShowClearFiltersButton="false"
            GridLines="None" OnNeedDataSource="grdRequirementRotations_NeedDataSource" AllowCustomPaging="false"
            OnItemCommand="grdRequirementRotations_ItemCommand" MasterTableView-DataKeyNames="RotationId,PkgSubscriptionId">
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grd_rwDbClick" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="False" AllowSorting="false"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                <Columns>
                    <telerik:GridBoundColumn DataField="AgencyName" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="125px" HeaderText="Agency Name" UniqueName="AgencyName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ComplioID" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="125px" HeaderText="Complio ID" UniqueName="ComplioID">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Department" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="125px" HeaderText="Department" UniqueName="Department">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Program" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="125px" HeaderText="Program" UniqueName="Program">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Course" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="125px" HeaderText="Course" UniqueName="Course">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="UnitFloorLoc" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="125px" HeaderText="Unit/Floor/Location" UniqueName="UnitFloorLoc">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RecommendedHours" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="125px" HeaderText="No of Hours" UniqueName="RecommendedHours">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="DaysName" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="125px" HeaderText="Days" UniqueName="DaysName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Shift" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="125px" HeaderText="Shift" UniqueName="Shift">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="Time" UniqueName="Time" ItemStyle-Width="140px">
                        <ItemTemplate>
                            <asp:Label ID="lblFromTime" runat="server" Text='<%# 
                             String.IsNullOrEmpty(Convert.ToString(Eval("StartTime"))) ? String.Empty:  DateTime.Parse(Convert.ToString(Eval("StartTime"))).ToShortTimeString() %>' />
                            <span>- </span>
                            <asp:Label ID="lblToTime" runat="server" Text='<%# 
                             String.IsNullOrEmpty(Convert.ToString(Eval("EndTime"))) ? String.Empty:  DateTime.Parse(Convert.ToString(Eval("EndTime"))).ToShortTimeString() %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Start Date" UniqueName="Time">
                        <ItemTemplate>
                            <asp:Label ID="lblStartDate" runat="server" Text='<%# Convert.ToString(Eval("StartDate", "{0:d}"))  %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="Time">
                        <ItemTemplate>
                            <asp:Label ID="lblEndDate" runat="server" Text='<%# Convert.ToString(Eval("EndDate", "{0:d}"))  %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="ContactNames" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="100px" HeaderText="Instructor/Preceptor" UniqueName="ContactNames">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField ="ReviewStatus" UniqueName ="ReviewStatus" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="125px" HeaderText="Review Status">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" ItemStyle-Width="130px">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnViewDetails" ButtonType="LinkButton" CommandName="ViewDetail" Visible='<%# 
                             Eval("PkgSubscriptionId")== null? false: true %>'
                                runat="server" Text="Detail" BackColor="Transparent" Font-Underline="true" BorderStyle="None">
                            </telerik:RadButton>
                            <telerik:RadButton ID="btnNotApplicable" ButtonType="LinkButton" Visible='<%#Eval("PkgSubscriptionId")== null? true: false %>'
                                ToolTip="You do not currently have any additional requirements for this rotation. If additional requirements are assigned, you will receive an email notification."
                                runat="server" Text="Not Applicable" BackColor="Transparent" Font-Underline="true" BorderStyle="None" Enabled="false">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Compliance Status">
                        <ItemTemplate>
                            <%# !String.IsNullOrEmpty(Convert.ToString(Eval("RequirementPackageStatusCode"))) 
                            ? String.Compare(RequirementPackageStatus.REQUIREMENT_COMPLIANT.GetStringValue(), Convert.ToString(Eval("RequirementPackageStatusCode"))) == 0 ? "<img src='../../Resources/Mod/Compliance/icons/yes16.png' alt='X' style='vertical-align: text-bottom; width: 12; height: 10' />" :"<img src='../../Resources/Mod/Compliance/icons/no16.png' alt='X' style='vertical-align: text-bottom; width: 12; height: 10' />"
                            : String.Empty
                            %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </infs:WclGrid>
    </asp:Panel>
</asp:Panel>
