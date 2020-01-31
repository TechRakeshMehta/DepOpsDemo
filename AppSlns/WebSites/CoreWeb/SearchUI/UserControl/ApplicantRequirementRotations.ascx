<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicantRequirementRotations.ascx.cs" Inherits="CoreWeb.Search.Views.ApplicantRequirementRotations" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Import Namespace="INTSOF.Utils" %>

<style>
    .sxlm .radio_list input, .sxlm .radio_list label {
        padding: 0!important;
        margin-top: 1px !important;
        vertical-align: top!important;
    }
</style>
<script type="text/javascript">

    //click on link button while double click on any row of grid.
    function grd_rwDbClick(s, e) {
        var _id = "btnViewDetails";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

    $jQuery(document).ready(function () {

        $jQuery("[id$=grdRequirementRotations]").find("th").each(function (element) {
            if ($jQuery(this).text() != "" && $jQuery(this).text() != undefined && $jQuery(this).text().length > 1) {
                $jQuery(this).attr("tabindex", "0");
            }
        });
    });
</script>

<div class="row">
    <div class='col-md-12'>
        <h2 class="header-color">Clinical Rotations
        </h2>
    </div>
</div>

<div class="row  allowscroll">
    <div id="dvRequirementRotations" runat="server">
        <infs:WclGrid ID="grdRequirementRotations" AutoGenerateColumns="false" runat="server" AutoSkinMode="True" CellSpacing="0"
            EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="true" ShowClearFiltersButton="false"
            GridLines="None" OnNeedDataSource="grdRequirementRotations_NeedDataSource" AllowCustomPaging="false" EnableAriaSupport="true"
            OnItemCommand="grdRequirementRotations_ItemCommand" MasterTableView-DataKeyNames="RotationId,PkgSubscriptionId">
            <ClientSettings EnableRowHoverStyle="true" ClientEvents-OnGridCreated="onGridCreated">
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
                    <telerik:GridBoundColumn DataField="HierarchyNodes" HeaderText="Hierarchy" SortExpression="HierarchyNodes"
                        HeaderTooltip="This column displays the Hierarchy for each record in the grid" HeaderStyle-Width="250px"
                        UniqueName="HierarchyNodes">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ComplioID" HeaderText="Complio ID" SortExpression="ComplioID"
                        UniqueName="ComplioID" HeaderTooltip="This column displays the Complio ID for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationName" HeaderText="Rotation ID/Name" SortExpression="RotationName"
                        UniqueName="RotationName" HeaderTooltip="This column displays the Location for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TypeSpecialty" HeaderText="Type/Specialty" SortExpression="TypeSpecialty"
                        HeaderStyle-Width="100px"
                        UniqueName="TypeSpecialty" HeaderTooltip="This column displays the Type/Specialty for each record in the grid">
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
                    <telerik:GridBoundColumn DataField="Term" HeaderText="Term" SortExpression="Term"
                        UniqueName="Term" HeaderTooltip="This column displays the term for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="UnitFloorLoc" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="125px" HeaderText="Unit/Floor/Location" UniqueName="UnitFloorLoc">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Students" HeaderText="# of Students"
                        SortExpression="Students" HeaderStyle-Width="100px"
                        UniqueName="Students" HeaderTooltip="This column displays the # of Students for each record in the grid">
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
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" ItemStyle-Width="130px">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnViewDetails" ButtonType="LinkButton" CommandName="ViewDetail" Visible='<%# 
                             Eval("PkgSubscriptionId") == null ? false: true %>'
                                ToolTip='<%# String.Concat("Click to view rotation detail, For Complio ID: ", Eval("ComplioID")) %>'
                                runat="server" Text="Detail" BackColor="Transparent" Font-Underline="true" BorderStyle="None">
                            </telerik:RadButton>
                            <telerik:RadButton ID="btnNotApplicable" ButtonType="LinkButton" Visible='<%#Eval("PkgSubscriptionId") == null ? true: false %>'
                                ToolTip='<%# String.Concat(" You do not currently have any additional requirements for this rotation. If additional requirements are assigned, you will receive an email notification. For Complio Id: ", Eval("ComplioID")) %>'
                                runat="server" Text="Not Applicable" BackColor="Transparent" Font-Underline="true" BorderStyle="None" Enabled="false">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Compliance Status">
                        <ItemTemplate>
                            <%# !String.IsNullOrEmpty(Convert.ToString(Eval("RequirementPackageStatusCode"))) 
                            ? String.Compare(RequirementPackageStatus.REQUIREMENT_COMPLIANT.GetStringValue(), Convert.ToString(Eval("RequirementPackageStatusCode"))) == 0 ? "<img src='../../Resources/Mod/Compliance/icons/yes16.png' alt='Complio ID " + Convert.ToString(Eval("ComplioID"))+ " is Compliant' style='vertical-align: text-bottom; width: 12; height: 10' />" :"<img src='../../Resources/Mod/Compliance/icons/no16.png' alt='Complio ID " + Convert.ToString(Eval("ComplioID"))+ " is not Compliant' style='vertical-align: text-bottom; width: 12; height: 10' />"
                            : String.Empty
                            %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="Review Status" UniqueName="SharedUserInvitationReviewStatusName" DataField="SharedUserInvitationReviewStatusName">
                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <%--<telerik:GridTemplateColumn HeaderText="Compliance Status">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnViewDetails" ButtonType="LinkButton" CommandName="ViewDetail" runat="server"
                                Text='<%# Eval("RequirementPackageStatusDesc") %>' Visible='<%# Eval("RequirementPackageStatusDesc") == null? false: true %>'
                                BackColor="Transparent" Font-Underline="true" BorderStyle="None"
                                ToolTip="Click here to view details of verification.">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                </Columns>
            </MasterTableView>
        </infs:WclGrid>
    </div>
    <div class="gclr">
    </div>
</div>
