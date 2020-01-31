<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgencyHierarchyRootNodeSetting.ascx.cs" Inherits="CoreWeb.AgencyHierarchy.Views.AgencyHierarchyRootNodeSetting" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rmpHierarchyControls">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~//Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Scripts/bootstrap.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="container-fluid" tabindex="-1" id="dvAgencyHierarchySetting" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Rotation must specify Instructor availability</h2>
        </div>
    </div>
    <div class="clearfix"></div>
    <div id="dvOptnsInstAvailability" runat="server" class="row">
        <div class="col-md-6">
            <div id="Div3" runat="server" class="table-responsive">
                <table class="table table-bordered">
                    <tbody>
                        <tr>
                            <td><span class="cptn">Specify Instructor Availability</span></td>
                            <td>
                                <asp:RadioButton ID="rdbOptionsInstAvailabilityYes" runat="server" GroupName="OptionsInstAvailability" Text="Yes" />
                                <asp:RadioButton ID="rdbOptionsInstAvailabilityNo" runat="server" GroupName="OptionsInstAvailability" Text="No" Checked="true" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="form-group col-md-12">
        <div class=" pull-right">
            <infsu:CommandBar ID="cmdInstructorAvailablity" runat="server" ButtonPosition="Right" DisplayButtons="Submit" UseAutoSkinMode="false" ButtonSkin="Silk"
                AutoPostbackButtons="Submit" OnSubmitClick="cmdInstructorAvailablity_SubmitClick" SubmitButtonIconClass="rbSave" SubmitButtonText="Save" ValidationGroup="grpFormAgencyHierarchySettingSubmit">
            </infsu:CommandBar>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Display drop down for Type/Speciality field of Rotation</h2>
        </div>
    </div>
    <div class="clearfix"></div>
    <div id="dvAutomaticallyArchivedSetting" runat="server" class="row">
        <div class="col-md-6">
            <div id="Div1" runat="server" class="table-responsive">
                <table class="table table-bordered">
                    <tbody>
                        <tr>
                            <td><span class="cptn">Options For Rotation Type/Specialty Field</span></td>
                            <td>
                                <asp:RadioButton ID="rdbOptionsTypeSpecialtyYes" runat="server" GroupName="OptionsTypeSpecialtyRot" Text="Yes" />
                                <asp:RadioButton ID="rdbOptionsTypeSpecialtydNo" runat="server" GroupName="OptionsTypeSpecialtyRot" Text="No" Checked="true" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    <div class="clearfix"></div>
    <div class="form-group col-md-12">
        <div class=" pull-right">
            <infsu:CommandBar ID="fsucCmdBarArchivedRotation" runat="server" ButtonPosition="Right" DisplayButtons="Submit" UseAutoSkinMode="false" ButtonSkin="Silk"
                AutoPostbackButtons="Submit" OnSubmitClick="fsucCmdBarArchivedRotation_SubmitClick" SubmitButtonIconClass="rbSave" SubmitButtonText="Save" ValidationGroup="grpFormAgencyHierarchySettingSubmit">
            </infsu:CommandBar>
        </div>
    </div>
    <div id="showTypeSpecialityOptionDiv" runat="server" visible="false">
        <infs:WclGrid runat="server" ID="grdRotTypeSpecialtyOptions" AllowCustomPaging="false" AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false" NonExportingColumns="EditCommandColumn,DeleteColumn"
            OnDeleteCommand="grdRotTypeSpecialtyOptions_DeleteCommand"
            OnNeedDataSource="grdRotTypeSpecialtyOptions_NeedDataSource" OnItemCommand="grdRotTypeSpecialtyOptions_ItemCommand"
            OnSortCommand="grdRotTypeSpecialtyOptions_SortCommand" EnableLinqExpressions="false" ShowClearFiltersButton="false" EnableAriaSupport="true">
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="MappingID"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Type/Specialty Option" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="MappingValue" HeaderText="Type/Specialty Option" ItemStyle-Width="30%"
                        SortExpression="MappingValue" UniqueName="MappingValue" HeaderTooltip="This column displays the requirement type/specialty option name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/Resources/Mod/Dashboard/images/CancelGrid.gif" ItemStyle-Width="1%"
                        CommandName="Delete" ConfirmText="Are you sure want to delete this type/specialty option?"
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
                                        <asp:Label ID="lblEHServiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Type/Specialty Option" : "Update Type/Specialty Option" %>'
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
                                                <span class="cptn">Option Value</span><span class="reqd">*</span>
                                                <infs:WclTextBox ID="txtOptionValue" runat="server" Text='<%# Eval("MappingValue") %>' Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false"></infs:WclTextBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvPackage" ControlToValidate="txtOptionValue"
                                                        Display="Dynamic" CssClass="errmsg" ValidationGroup="grdRotTypeSpecialtyOptionsFormSubmit"
                                                        Text="Option is required." />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <infsu:CommandBar ID="fsucCmdBarRotation" runat="server" GridMode="true"
                                GridInsertText="Save" GridUpdateText="Save" SaveButtonIconClass="rbSave"
                                ValidationGroup="grdRotTypeSpecialtyOptionsFormSubmit" UseAutoSkinMode="false" ButtonSkin="Silk" />
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
</div>
