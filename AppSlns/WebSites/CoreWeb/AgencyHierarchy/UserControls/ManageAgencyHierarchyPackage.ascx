<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageAgencyHierarchyPackage.ascx.cs" Inherits="CoreWeb.AgencyHierarchy.Views.ManageAgencyHierarchyPackage" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchyMultiple" Src="~/AgencyHierarchy/UserControls/AgencyHierarchyMultipleSelection.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxManageAgencyHierarchyPackages">
    <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>


<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Packages</h2>
        </div>
    </div>
    <div class="row">
        <%--<div class="col-md-12">--%>
        <infs:WclGrid runat="server" ID="grdAgencyHirarchyPackage" AllowCustomPaging="true" OnInit="grdAgencyHirarchyPackage_Init"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false" NonExportingColumns="EditCommandColumn,DeleteColumn,PackageCategoryCount"
            OnDeleteCommand="grdAgencyHirarchyPackage_DeleteCommand"
            OnNeedDataSource="grdAgencyHirarchyPackage_NeedDataSource" OnItemCommand="grdAgencyHirarchyPackage_ItemCommand" OnItemDataBound="grdAgencyHirarchyPackage_ItemDataBound"
            OnSortCommand="grdAgencyHirarchyPackage_SortCommand" EnableLinqExpressions="false" ShowClearFiltersButton="false" EnableAriaSupport="true">
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="RequirementPackageID, AgencyHierarchyPackageID, IsNewPackage,RequirementPackageCodeType,TempPackageCategoryCount"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Package" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="RequirementPackageName" HeaderText="Package Name" ItemStyle-Width="19%"
                        SortExpression="RequirementPackageName" UniqueName="RequirementPackageName" HeaderTooltip="This column displays the Requirement Package Name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RequirementPackageType" HeaderText="Package Type" ItemStyle-Width="15%"
                        SortExpression="RequirementPackageType" UniqueName="RequirementPackageType" HeaderTooltip="This column displays the Requirement Package Type for each record in the grid">
                    </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PackageArchiveState" HeaderText="Package Archive State" ItemStyle-Width="13%"
                        SortExpression="PackageArchiveState" UniqueName="PackageArchiveState" HeaderTooltip="This column displays the Requirement Package Archive State for each record in the grid">
                    </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn Display="false" DataField="TempPackageCategoryCount" FilterControlAltText="Filter PackageArchiveState column"
                                        HeaderText="Associated Categories Count" SortExpression="TempPackageCategoryCount" UniqueName="TempPackageCategoryCount">
                                    </telerik:GridBoundColumn>
                     <telerik:GridTemplateColumn DataField="PackageCategoryCount" HeaderStyle-Width="13%" HeaderText="Associated Categories Count"
                                        SortExpression="PackageCategoryCount" UniqueName="PackageCategoryCount" HeaderTooltip="This column displays the Requirement Package Categories Count for each record in the grid">
                                        <ItemTemplate>
                                            <a href="javascript:void(0)" style="color: blue;" runat="server" onclick="ShowPackageCategoriesPopUp(this);" id="lnkPackageCategoryName"></a>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="EffectiveStartDate" HeaderText="Package Effective Start Date" ItemStyle-Width="17%" DataFormatString="{0:MM/dd/yyyy}"
                        SortExpression="EffectiveStartDate" UniqueName="EffectiveStartDate" HeaderTooltip="This column displays the Requirement Package Effective Start Date for each record in the grid">
                    </telerik:GridBoundColumn>

                 <%--   <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="PackageReplace" ItemStyle-Width="7%">
                        <ItemTemplate>
                            <telerik:RadButton ID="btbPackageCopy" ButtonType="LinkButton" CommandName="PackageReplace"
                                runat="server" Text="Replace Package">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                    <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/Resources/Mod/Dashboard/images/CancelGrid.gif" ItemStyle-Width="1%"
                        CommandName="Delete" ConfirmText="Are you sure you want to delete this Requirement Package?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn" ItemStyle-Width="1%">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" />
                    </telerik:GridEditCommandColumn>

                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12">
                                    <h2 class="header-color">
                                        <asp:Label ID="lblEHServiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Package" : "Update Package" %>'
                                            runat="server" />
                                    </h2>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="msgbox">
                                        <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <asp:Panel ID="pnlEditForm" runat="server">
                                <div class="row bgLightGreen">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class='form-group col-md-3' title="Select a package.">
                                                <span class="cptn">Select Package</span><span class="reqd">*</span>

                                                <infs:WclComboBox ID="cmbPackage" runat="server" Filter="Contains" DataTextField="RequirementPackageName"
                                                    DataValueField="RequirementPackageID"
                                                    AutoPostBack="false" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvPackage" ControlToValidate="cmbPackage"
                                                        InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpManageAgencyHierarchyPackageFormSubmit"
                                                        Text="Package is required." />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <infsu:CommandBar ID="fsucCmdBarRotation" runat="server" GridMode="true"
                                GridInsertText="Save" GridUpdateText="Save" SaveButtonIconClass="rbSave"
                                ValidationGroup="grpManageAgencyHierarchyPackageFormSubmit" UseAutoSkinMode="false" ButtonSkin="Silk" />
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
        <%--</div>--%>
    </div>
</div>
<%--<div style="display: none">
    <asp:Button ID="btnRebindGrid" runat="server" OnClick="btnRebindGrid_Click" />
</div>--%>

<script type="text/javascript">

    var winopen = false;
    
    function RedirectControls(url) {
        self.parent.location = url;
    }

  
    //function OpenPopUpToReplacePackage(packageID, agencyHierarchyId, IsRotationPkgCopyFromAgencyHierarchy) {
    //    var popupWindowName = "Replace Requirement Package";      
    //    var url = $page.url.create("").replace("/AgencyHierarchy/Pages/", "") + "/RotationPackages/Pages/RotationPackageCopy.aspx?RequirementPackageID=" + packageID + '&IsRotationPkgCopyFromAgencyHierarchy=' + IsRotationPkgCopyFromAgencyHierarchy + '&AgencyHierarchyId=' + agencyHierarchyId;
    //    var popupHeight = $jQuery(window).height() * (100 / 100);

    //    winopen = true; 
    //    var win = $window.createPopup(url, { size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName, onclose: OnClose }
    //       );
    //    return false;
    //}
    //function OnClose(oWnd, args) {
    //    oWnd.remove_close(OnClose);
    //    if (winopen) {
    //        if (args.get_argument().Action.toLowerCase() == "submit") {
    //            $jQuery("[id$=btnRebindGrid]").click();
    //        }
    //        winopen = false;
    //    }
    //}


    function ShowPackageCategoriesPopUp(obj) {
        var args = $jQuery(obj).attr('args');
         if (args != null && args != "") {
            var popupHeight = $jQuery(window).height() * (100 / 100);
            var url = $page.url.create("~/AgencyHierarchy/Pages/PackageCategoryDetailPopUp.aspx?args=" + args);
            var win = $window.createPopup(url, { size: "625," + popupHeight / 1.5, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize | Telerik.Web.UI.WindowBehaviors.Modal });
            return false;
        }
    }

</script>
<script src="../../Resources/Mod/Dashboard/Scripts/bootstrap.min.js" type="text/javascript"></script>
