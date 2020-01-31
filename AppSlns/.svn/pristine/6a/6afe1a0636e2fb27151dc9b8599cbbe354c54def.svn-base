<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PublicPageMaster.master" AutoEventWireup="true" CodeBehind="ExternalViewDocument.aspx.cs" Inherits="CoreWeb.ExternalViewDocument" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Assembly="RadPdf" Namespace="RadPdf.Web.UI" TagPrefix="radPdf" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadContent" runat="server">
    <style type="text/css">
        #divClientLogo {
            display: none;
        }

        .content-wrapper {
            width: 100% !important;
        }

        .customLbl {
            font-weight: normal;
        }

        .shw_dv {
            display: block;
        }

        .hide_dv {
            display: none;
        }

        .msgbox {
            display: block !important;
        }

        .rgCommandTable {
            display: none;
        }
        /*.height {
         height: 68vh !important;
        }
        .heightcustom {
         height: 100% !important;
        }*/
        table .rgMasterTable {
            min-height: 0px !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">

    <infs:wclresourcemanagerproxy runat="server" id="rprxAddUserInformation">
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource ResourceType="StyleSheet" Path="~/Resources/Themes/Default/colors.css" />
        <infs:LinkedResource Path="~/Resources/Mod/ClinicalRotation/RequirementDocumentViewer.js" />
        <infs:LinkedResource ResourceType="StyleSheet" Path="~/Resources/Mod/Shared/public_pages/core.css" />
         <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:wclresourcemanagerproxy>

    <div id="dvTop" class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <div class="msgbox" runat="server" visible="false" id="pageMsgBox">
                    <asp:label enableviewstate="false" cssclass="info" runat="server" id="lblError">
                    </asp:label>
                </div>
            </div>
        </div>
        <div id="dvDocsSection" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <h2 class="header-color">View Documents
                    </h2>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div id="divMainContainer" runat="server">
                        <infs:WclSplitter ID="sptrMain" runat="server" LiveResize="true" Width="100%" ClientIDMode="Static" CssClass="splitterCustom" Orientation="Vertical" ResizeWithParentPane="true">
                           <infs:WclPane ID="pnLeft" runat="server" Style="width: 49%" CssClass="" Scrolling="None">
                                <infs:WclSplitter ID="sptrLeftPanel" runat="server" LiveResize="true" Width="100%" CssClass="" Orientation="Horizontal" ResizeWithParentPane="true">
                                    <infs:WclPane ID="paneDocumentList" ClientIDMode="Static" runat="server" Width="100%" Scrolling="Both">
                                        <infs:WclGrid ID="grdDocuments" AutoGenerateColumns="false" runat="server" AutoSkinMode="True" CellSpacing="0"
                                            EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="true" ShowClearFiltersButton="false"
                                            GridLines="None" OnNeedDataSource="grdDocuments_NeedDataSource" AllowCustomPaging="false"
                                            OnItemCommand="grdDocuments_ItemCommand" MasterTableView-DataKeyNames="PdfDocPath,DocumentPath">
                                            <ClientSettings EnableRowHoverStyle="true">
                                                <Selecting AllowRowSelect="true"></Selecting>
                                            </ClientSettings>
                                            <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="False" AllowSorting="false"
                                                AllowFilteringByColumn="false">
                                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                                                <HeaderStyle Height="20px" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="FileName" FilterControlAltText="Filter FileName column" HeaderText="File Name"
                                                        SortExpression="FileName" UniqueName="FileName" ReadOnly="true" HeaderTooltip="This column contains the name of each uploaded document">
                                                    </telerik:GridBoundColumn>
                                                    <%-- <telerik:GridTemplateColumn HeaderText="File Type" HeaderTooltip="This column displays the file type of each uploaded document">
                                                    <ItemTemplate>
                                                        <label class="customLbl"><%#  !String.IsNullOrEmpty(Convert.ToString(Eval("PdfDocPath"))) ? "pdf File" : Convert.ToString(Eval("FileName")).Length>0 ? string.Concat( Convert.ToString(Eval("FileName")).Split('.')[Convert.ToString(Eval("FileName")).Split('.').Length - 1], " file")  : "pdf File" %></label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Size (KB)" HeaderTooltip="This column displays the size of each uploaded document">
                                                    <ItemTemplate>
                                                        <label class="customLbl"><%#  String.IsNullOrEmpty(Convert.ToString(Eval("Size"))) ? "0" : Convert.ToString(Convert.ToInt32(Eval("Size")) / 1024) %></label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>--%>
                                                    <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                                                        HeaderText="Description" SortExpression="Description" UniqueName="Description" ReadOnly="true" HeaderTooltip="This column contains the description of each uploaded document">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" ItemStyle-Width="130px">
                                                        <ItemTemplate>
                                                            <telerik:RadButton ID="btnViewDetails" ButtonType="LinkButton" CommandName="ViewDocument"
                                                                runat="server" Text="View Document" BackColor="Transparent" Font-Underline="true" BorderStyle="None">
                                                            </telerik:RadButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </infs:WclGrid>
                                    </infs:WclPane>
                                </infs:WclSplitter>
                            </infs:WclPane>
                            <infs:WclSplitBar runat="server" Height="100%" CollapseMode="Both" ID="spltBar"></infs:WclSplitBar>
                            <infs:WclPane ID="pnRight" runat="server" Width="49%" CssClass="heightcustom" Scrolling="None">
                                <div class="msgbox" id="dvMsgBox" runat="server">
                                    <asp:Label ID="lblPdfMessage" runat="server">                             
                                    </asp:Label>
                                </div>
                                <div id="dvPdfDocuViewer" runat="server">
                                    <asp:HiddenField runat="server" ID="hdnUnifiedDoc_value" />
                                    <radPdf:PdfWebControl ID="PdfWebControl1" ClientIDMode="Static" runat="server" CssClass="heightcustom"
                                        Width="100%" OnClientLoad="e_wcload();"
                                        HideBottomBar="false"
                                        HideThumbnails="true"
                                        HideBookmarks="true"
                                        CollapseTools="true"
                                        HideSearchText="True"
                                        HideEditMenu="true"
                                        HideObjectPropertiesBar="true"
                                        HideToolsPageTab="true"
                                        HideToolsAnnotateTab="true"
                                        HideToolsInsertTab="true"
                                        HideSideBar="true"
                                        HideToolsMenu="true"
                                        ViewerPageLayoutDefault="SinglePageContinuous" />
                                </div>
                            </infs:WclPane>
                        </infs:WclSplitter>
                    </div>
                </div>
            </div>
            <div class="row text-center" style="padding: 10px 0px;">
                <infsu:commandbar id="fsucCmdExport" runat="server" buttonposition="Center" displaybuttons="Extra"
                    autopostbackbuttons="Extra" extrabuttontext="Export Document(s)" onextraclick="fsucCmdExport_ExtraClick"
                    useautoskinmode="false" buttonskin="Silk">
                </infsu:commandbar>
                <iframe id="ifrExportDocument" runat="server" height="0" width="0"></iframe>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentOutsideForm" runat="server">
    <script type="text/javascript">

        $jQuery(document).ready(function () {
            $jQuery('.content-wrapper:first').attr('style', 'padding: 0 0 0 15px;');
            var height = $jQuery(window).height();
            var heightDynamic = (60 * height) / 100;
            heightDynamic = parseInt(heightDynamic) + 'px';
            $jQuery("[id$='divMainContainer']").css('min-height', heightDynamic);
            $jQuery("[id$='sptrMain']").css('min-height', heightDynamic);
            $jQuery("table").css('min-height', heightDynamic);
            $jQuery("[id$='paneDocumentList']").css('min-height', heightDynamic);
            //$jQuery("#RAD_SPLITTER_PANE_CONTENT_paneDocumentList").css('min-height', heightDynamic);
            $jQuery("[id$='pnLeft']").css('min-height', heightDynamic);
            $jQuery("[id$='PdfWebControl1']").css('min-height', heightDynamic);
            // $jQuery("#RAD_SPLITTER_PANE_CONTENT_ctl00_DefaultContent_pnLeft").css('min-height', heightDynamic);
        });

    </script>
</asp:Content>
