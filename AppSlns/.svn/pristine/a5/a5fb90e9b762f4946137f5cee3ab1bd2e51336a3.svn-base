<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BkgOrderNote.aspx.cs" Inherits="CoreWeb.BkgOperations.Views.BkgOrderNote"
    MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var tabKey = 9;
        // To close the popup.
        function ClosePopup() {
            top.$window.get_radManager().getActiveWindow().close();
        }

        function pageLoad() {
            $jQuery("[id$=txtNotes]").focus();

        

            //For accessibility, we need to prevent focus to go outside after tabbing on last link
            $jQuery("a,button,:input:not([type=hidden]),[tabindex='0']").last().keydown(function (e) {
                if (e.keyCode == tabKey && !e.shiftKey) {
                    e.preventDefault();
                    $jQuery("#lblNotes").focus();
                }
            });

            if ($jQuery('#MsgBox').css('display') != 'none') {
                $jQuery("#lblError").attr("tabindex", 0).focus();
                $jQuery('#lblError').on("keydown", function (e) {
                    if (e.shiftKey && e.keyCode == tabKey) {
                        e.preventDefault();
                        $jQuery("[id$=<%= fsucFeatureActionList.CancelButton.ClientID %>]").focus();
                    }
                });
            }
            //For accessibility, we need to prevent focus to go outside after shift tab on firstmost element
            $jQuery(document).on("keydown", "[id$=txtNotes]", function (e) {
                if (e.shiftKey && e.keyCode == tabKey) {
                    e.preventDefault();
                    $jQuery("[id$=<%= fsucFeatureActionList.CancelButton.ClientID %>]").focus();
                    }
                });
            }

    </script>
    <%--<link href="<%= ResolveUrl("~/Resources/Mod/Dashboard/Styles/bootstrap.min.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Resources/Mod/Dashboard/Styles/font-awesome.min.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css") %>" rel="stylesheet" type="text/css" />--%>

      <infs:WclResourceManagerProxy runat="server" ID="rprBkgOrderNote">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <p class="header-color">
                    <span class="cptn">Background Order Note</span>
                    <%-- <label class="cptn">Background Order Note<span class="reqd">*</span> </label>--%>
                </p>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <%--  <span class="cptn">Note</span><span class="reqd">*</span>--%>
                <label class="cptn" for="<%= txtNotes.ClientID %>" id="lblNotes">Enter Notes<span class="reqd">*</span> </label>
                <infs:WclTextBox TextMode="MultiLine" ID="txtNotes" MaxLength="1012" runat="server" Height="200px" Width="100%" CssClass="form-control borderTextArea" EnableAriaSupport="true"></infs:WclTextBox>
                <div class="vldx">
                    <asp:RequiredFieldValidator runat="server" ID="rfvNotes" ControlToValidate="txtNotes" role="alert"
                        Display="Dynamic" ValidationGroup="saveNotes" CssClass="errmsg" SetFocusOnError="true"
                        Text="Note is required." />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <infsu:CommandBar ID="fsucFeatureActionList" runat="server" AutoPostbackButtons="Save" OnSaveClick="fsucFeatureClientStatus_SaveClick" DisplayButtons="Save,Cancel" CancelButtonText="Close"
                    ButtonPosition="Right" CauseValidationOnCancel="false" OnCancelClientClick="ClosePopup" ValidationGroup="saveNotes" ButtonSkin="Silk" UseAutoSkinMode="false" />
            </div>
        </div>

        <div class="row">
            <div class="msgbox">
                <asp:Label ID="Label1" runat="server" CssClass="info">
                </asp:Label>
            </div>
            <div id="divNotes" runat="server">
                <infs:WclGrid runat="server" ID="grdNotes" CssClass="removeExtraSpace" AutoGenerateColumns="false"
                    AllowSorting="True" AutoSkinMode="True" CellSpacing="0"
                    GridLines="Both" ShowAllExportButtons="False" ShowExtraButtons="True" OnNeedDataSource="grdNotes_NeedDataSource"
                    AllowPaging="false" EnableDefaultFeatures="false" EnableAriaSupport="true">
                    <MasterTableView CommandItemDisplay="Top">
                        <ItemStyle Wrap="true" />
                        <CommandItemSettings ShowAddNewRecordButton="false"
                            ShowExportToExcelButton="False" ShowExportToPdfButton="False" ShowExportToCsvButton="False"
                            ShowRefreshButton="False" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="Note"
                                HeaderText="Note" UniqueName="Note">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UserName"
                                HeaderText="Created By" UniqueName="UserName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CreatedOnDate"
                                HeaderText="Created On" UniqueName="CreatedOnDate">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </infs:WclGrid>
            </div>
        </div>

    </div>
</asp:Content>

