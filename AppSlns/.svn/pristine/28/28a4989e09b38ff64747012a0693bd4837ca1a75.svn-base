<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgencyLocation.ascx.cs" Inherits="CoreWeb.PlacementMatching.Views.AgencyLocation"  %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="SysXResourceManagerProxy1">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h1 class="header-color">Map Location(s)</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
        </div>
    </div>

    <div id="dvAgencyLocations" class="row" style="margin-left: 10px; margin-right: 10px; width: auto; height: auto">
        <infs:WclGrid runat="server" ID="grdAgencyLocations" AllowPaging="true" AutoGenerateColumns="false" CssClass="gridhover"
            AllowSorting="true" AllowFilteringByColumn="true" AutoSkinMode="true" CellSpacing="0" GridLines="Both" EnableDefaultFeatures="true"
            ShowAllExportButtons="false" ShowExtraButtons="true" NonExportingColumns="DeleteColumn"
            OnNeedDataSource="grdAgencyLocations_NeedDataSource" OnItemCommand="grdAgencyLocations_ItemCommand">
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents />
                <%--OnRowDblClick="grd_rwDbClick"--%>
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <GroupingSettings CaseSensitive="false" />
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="AgencyLocationID" AllowFilteringByColumn="true">
                <CommandItemSettings ShowExportToExcelButton="false"
                    ShowExportToPdfButton="false" ShowExportToCsvButton="false" AddNewRecordText="Add New Location" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column"
                        HeaderText="Location" SortExpression="Location" UniqueName="Location"
                        HeaderTooltip="This column displays the name of the location for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Experience" FilterControlAltText="Filter Experience column"
                        HeaderText="Experience" SortExpression="Experience" UniqueName="Experience"
                        HeaderTooltip="This column displays the experience for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/Resources/Mod/Dashboard/images/CancelGrid.gif"
                        CommandName="Delete" ConfirmText="Are you sure you want to delete this mapping ?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="~/Resources/Mod/Dashboard/images/editGrid.gif"
                        UniqueName="EditCommandColumn">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridEditCommandColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                    <FormTemplate>
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12">
                                    <h2 class="header-color">
                                        <asp:Label ID="lblTitleAgencyLocationMapping" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Location" : "Update Location" %>'
                                            runat="server" /></h2>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="msgbox">
                                        <asp:Label runat="server" ID="lblName" CssClass="info"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <asp:Panel runat="server" CssClass="editForm" ID="pnlAgencyLocationMapping">
                                <div class="row bgLightGreen">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="form-group col-md-3">
                                                <span class="cptn">Location</span><span class='reqd'>*</span>
                                                <div id="dvLocation">
                                                    <infs:WclTextBox ID="txtLocation" runat="server" Text='<%# Eval("Location") %>'
                                                        Width="100%" CssClass="form-control">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvLocation" ControlToValidate="txtLocation"
                                                        Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                        Text="Location is required." />
                                                </div>
                                            </div>
                                            <div class="form-group col-md-3">
                                                <span class="cptn">Experience</span>
                                                <infs:WclNumericTextBox ID="txtExperience" NumberFormat-DecimalDigits="2" Type="Number" runat="server" Text='<%# Eval("Experience") %>'
                                                    Width="100%" CssClass="form-control">
                                                </infs:WclNumericTextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div class="col-md-12 text-right">
                                <infsu:CommandBar ID="fsucCmdBarNode" runat="server" GridMode="true" GridInsertText="Save" GridUpdateText="Save"
                                    ValidationGroup="grpFormSubmit" ExtraButtonIconClass="icnreset" UseAutoSkinMode="false" ButtonSkin="Silk" />
                            </div>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" Position="TopAndBottom" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False"></FilterMenu>
        </infs:WclGrid>
    </div>
</div>

<script type="text/javascript">

    function RefreshAgencyLocation() {
        //debugger;
        var btn = $jQuery('[id$=btnDoPostBack]', $jQuery(parent.theForm));
        btn.click();
    }
</script>
