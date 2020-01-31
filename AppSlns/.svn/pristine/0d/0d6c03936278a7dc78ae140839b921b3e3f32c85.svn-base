<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackingPackageRequiredDocumnets.ascx.cs" Inherits="CoreWeb.ComplianceAdministration.UserControl.TrackingPackageRequiredDocumnets" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>
<%@ Register TagPrefix="uc2" TagName="CategoriesItemsNodes" Src="~/Shared/Controls/CategoriesItemsNodes.ascx" %>
<%@ Register TagPrefix="uc" TagName="DocumentUrlInfo" Src="~/Shared/Controls/DocumentUrlInfo.ascx" %>


<infs:WclResourceManagerProxy runat="server" ID="SysXResourceManagerProxy1">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

</infs:WclResourceManagerProxy>

<asp:XmlDataSource ID="xdtsCategory" runat="server" DataFile="~/App_Data/DB.xml"
    XPath="//MasterCompliance/MasterCategory/*"></asp:XmlDataSource>

<style type="text/css">
    .reEditorModes a {
        display: none;
    }

    .reToolZone {
        display: none;
    }

    .bullet ul {
        margin-left: 10px;
        padding-left: 10px !important;
    }

    .bullet li {
        list-style-position: inside;
        list-style: disc;
    }

    .bullet ol {
        list-style-type: decimal;
        margin-left: 10px;
        padding-left: 10px;
    }

        .bullet ol li {
            list-style: decimal;
        }
    .test {
    display:none;
    }
</style>
<div class="section">
    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Manage Compliance Package Document URL
                </h2>
            </div>
        </div>
        <div class="row bgLightGreen"> 
            <div>
                <asp:UpdatePanel runat="server" ID="UpdatePanelPackage" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                            <div id="divTenant" runat="server">
                                <div class='form-group col-md-3'>
                                    <div class="row">
                                        <div class='form-group col-md-3'>
                                            <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="cptn"></asp:Label>
                                        </div>
                                    </div>
                                    <infs:WclComboBox Width="100%" ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                                        DataValueField="TenantID" CssClass="form-control" Skin="Silk" AutoSkinMode="false" EmptyMessage="--Select--" 
                                        OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                                    </infs:WclComboBox>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPackage">
                            <div class=' form-group col-md-3'>
                                <div class="row">
                                    <div class='form-group col-md-3'>
                                        <asp:Label ID="lblPackage" runat="server" Text="Package" CssClass="cptn"></asp:Label>
                                    </div>
                                </div>
                                <infs:WclComboBox Width="100%" ID="ddlPackage" runat="server" AutoPostBack="false" DataTextField="PackageName"
                                    DataValueField="CompliancePackageID" CssClass="form-control" Skin="Silk" AutoSkinMode="false" EmptyMessage="--SELECT--"
                                    Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" CheckBoxes="true" Localization-CheckAllString="Select ALL">
                                </infs:WclComboBox>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlSearchBtn">
                            <div class='col-md-3'>
                                <div class="row">
                                    <div class='form-group col-md-3'>
                                        &nbsp;
                                    </div>
                                </div>
                                <div style="float: left;">
                                    <infsu:CommandBar runat="server" ID="fsucSearch" ButtonPosition="Center" DisplayButtons="Submit,Extra"
                                        AutoPostbackButtons="Submit,Extra" SubmitButtonText="Search" SubmitButtonIconClass="rbSearch"
                                        ExtraButtonText="Reset" ExtraButtonIconClass="rbUndo" OnExtraClick="fsucSearch_ExtraClick"
                                        OnSubmitClick="fsucSearch_SubmitClick" OnSubmitClientClick="BindGrid" UseAutoSkinMode="false" ButtonSkin="Silk">
                                    </infsu:CommandBar>
                                    <asp:HiddenField ID="hdnIsSearchClicked" runat="server" Value="" />
                                </div>
                            </div>                            
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlTenant" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <%--<div class="row">
                <div class="col-md-12">
                    <div class="form-group col-md-12">
                        <div class="row text-center">
                            <infsu:CommandBar runat="server" ID="fsucSearch" ButtonPosition="Center" DisplayButtons="Submit,Extra"
                                AutoPostbackButtons="Submit,Extra" SubmitButtonText="Search" SubmitButtonIconClass="rbSearch"
                                ExtraButtonText="Reset" ExtraButtonIconClass="rbUndo" OnExtraClick="fsucSearch_ExtraClick"
                                OnSubmitClick="fsucSearch_SubmitClick" OnSubmitClientClick="BindGrid" UseAutoSkinMode="false" ButtonSkin="Silk">
                            </infsu:CommandBar>
                            <asp:HiddenField ID="hdnIsSearchClicked" runat="server" Value="" />
                        </div>
                    </div>
                </div>
            </div>--%>
            <div class="swrap">                
                        <infs:WclGrid runat="server" ID="grdTrackingPackage" CssClass="gridhover" AllowPaging="True" AutoGenerateColumns="False"
                            AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                            GridLines="Both" EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                            NonExportingColumns="EditCommandColumn,DeleteColumn,CountOfAssociated"
                            OnNeedDataSource="grdTrackingPackage_NeedDataSource"
                            OnItemCommand="grdTrackingPackage_ItemCommand" OnItemCreated="grdTrackingPackage_ItemCreated"
                            OnItemDataBound="grdTrackingPackage_ItemDataBound" OnPdfExporting="grdTrackingPackage_PdfExporting" OnGridExporting="grdTrackingPackage_GridExporting">
                            <ExportSettings ExportOnlyData="true" IgnorePaging="True" OpenInNewWindow="True"
                                Pdf-PageWidth="500mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm" FileName="Manage Compliance Required Document">
                            </ExportSettings>
                            <ClientSettings EnableRowHoverStyle="true">
                                <Selecting AllowRowSelect="true"></Selecting>
                            </ClientSettings>
                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="trackingPackageRequiredDOCURLId">
                                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Document URL"
                                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"
                                    ShowRefreshButton="true" />
                                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="SampleDocFormURL" HeaderStyle-Width="50%" FilterControlAltText="Filter ScreenLabel column"
                                        HeaderText="Document URL" SortExpression="SampleDocFormURL" UniqueName="SampleDocFormURL">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ScreenName" FilterControlAltText="Filter CategoryName column"
                                        HeaderText="Document URL Name" SortExpression="ScreenName" UniqueName="ScreenName">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Label" FilterControlAltText="Filter CategoryLabel column"
                                        HeaderText="Document URL Label" SortExpression="Label" UniqueName="Label">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn Display="false" DataField="TempCountOfAssociated" FilterControlAltText="Filter CategoryLabel column"
                                        HeaderText="No of Packages associated" SortExpression="Label" UniqueName="TempCountOfAssociated">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridTemplateColumn DataField="CountOfAssociated" HeaderStyle-Width="15%" HeaderText="No of Packages associated"
                                        SortExpression="CountOfAssociated" UniqueName="CountOfAssociated" HeaderTooltip="This column displays the name of package associated">
                                        <ItemTemplate>
                                            <a href="javascript:void(0)" style="color: blue;" runat="server" onclick="ShowNamesOfPackagesPopUp(this);" id="lnkNameOfPackages"></a>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="PackageIds" Display="false"
                                        UniqueName="PackageIds">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                                        <HeaderStyle CssClass="tplcohdr" />
                                        <ItemStyle CssClass="MyImageButton" />
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Document URL?"
                                        Text="Delete" UniqueName="DeleteColumn">
                                        <HeaderStyle CssClass="tplcohdr" />
                                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                                    </telerik:GridButtonColumn>
                                </Columns>
                                <EditFormSettings EditFormType="Template">
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                    <FormTemplate>
                                        <div class="container-fluid">

                                            <div class="row">
                                                <div class="col-md-12">
                                                    <h2 class="header-color">
                                                        <asp:Label ID="lblEHCategory" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Document URL" : "Update Document URL" %>'
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
                                            <asp:Panel runat="server" CssClass="sxpnl pnlEnroller" ID="pnlCategory">
                                                <div class="row bgLightGreen">
                                                    <div class="row">
                                                        <div class='col-md-12'>
                                                            <div class='form-group col-md-4'>
                                                                <span class="cptn">Document URL</span><span class="reqd">*</span>
                                                                <infs:WclTextBox Width="100%" runat="server" CssClass="form-control" ID="txtURL" ClientEvents-OnLoad="SetFocus" Text='<%# Eval("SampleDocFormURL") %>'
                                                                    MaxLength="100">
                                                                </infs:WclTextBox>
                                                                <div class="vldx">
                                                                    <asp:RegularExpressionValidator ID="RvftxtURL" runat="server" ControlToValidate="txtURL"
                                                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please enter a valid Url."
                                                                        ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$">
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvURL" ControlToValidate="txtURL"
                                                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Document URL is required." />
                                                                </div>
                                                            </div>

                                                            <div class="form-group col-md-4">
                                                                <span class="cptn">Document URL Name</span><span class="reqd">*</span>
                                                                <infs:WclTextBox Width="100%" runat="server" CssClass="form-control" ID="txtScreenName" Text='<%# Eval("ScreenName") %>'
                                                                    MaxLength="100">
                                                                </infs:WclTextBox>
                                                                <div class="vldx">
                                                                    <asp:RequiredFieldValidator runat="server" ID="frvScreenLabel" ControlToValidate="txtScreenName"
                                                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Document URL Name is required." />
                                                                </div>

                                                            </div>

                                                            <div class='form-group col-md-4'>
                                                                <span class="cptn">Document URL Label</span>
                                                                <infs:WclTextBox Width="100%" runat="server" CssClass="form-control" ID="txtScreenLabel" Text='<%# Eval("Label") %>'
                                                                    MaxLength="100">
                                                                </infs:WclTextBox>

                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class='form-group col-md-4'>
                                                                <span class="cptn">Packages</span><span class="reqd">*</span>
                                                                <infs:WclComboBox ID="ddlPackage" CssClass="form-control" Skin="Silk" AutoSkinMode="false" runat="server" DataTextField="PackageName" AutoPostBack="false" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                                                    DataValueField="CompliancePackageID" Filter="Contains" EmptyMessage="--SELECT--" Width="100%">
                                                                </infs:WclComboBox>
                                                                <div class="vldx">
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvPackage" ControlToValidate="ddlPackage"
                                                                        Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" InitialValue=""
                                                                        Text="Please select at least one package." />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>

                                            <infsu:CommandBar UseAutoSkinMode="false" ButtonSkin="Silk" ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlCategory" GridInsertText="Save" GridUpdateText="Save"
                                                ValidationGroup="grpFormSubmit" ExtraButtonIconClass="icnreset" />


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
            <div class="gclr">
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">

    function ShowNamesOfPackagesPopUp(obj) {
        var args = $jQuery(obj).attr('args');
        if (args != null && args != "") {
            var popupHeight = $jQuery(window).height() * (100 / 100);
            var url = $page.url.create("~/ComplianceAdministration/Pages/TrackingPackageDetailPopUp.aspx?args=" + args);
            var win = $window.createPopup(url, { size: "625," + popupHeight / 3, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize | Telerik.Web.UI.WindowBehaviors.Modal });
            return false;
        }
    }

    function BindGrid(e) {
        $jQuery("[id$=hdnIsSearchClicked]").val('true');
        return true;
    }

</script>

