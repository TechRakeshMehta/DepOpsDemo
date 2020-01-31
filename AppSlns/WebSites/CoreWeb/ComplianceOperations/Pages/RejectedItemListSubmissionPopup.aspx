<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Shared/PopupMaster.master" CodeBehind="RejectedItemListSubmissionPopup.aspx.cs" Inherits="CoreWeb.ComplianceOperations.Pages.RejectedItemListSubmissionPopup" %>


<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxAdminCreateOrderSearch">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />

        <infs:LinkedResource Path="../Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Accessibility/Grid-Accessibility.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <style>
        .left {
            float: left !important;
        }

        .width {
            width: auto!important;
        }

        .height {
            height: auto!important;
        }
    </style>
    <script type="text/javascript">

      


        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function CloseRejectedSubmissionPopup() {
            var oWnd = GetRadWindow();
            oWnd.Close();
        }

        function checkAll(e) {
            // debugger;
            var table = $jQuery(e).closest('table');
            if (e.checked) {
                $jQuery('td input:checkbox', table).prop('checked', true);
            }
            else {
                $jQuery('td input:checkbox', table).prop('checked', false);
            }
        }

        function SelectcheckAll() {
            //debugger;
            if ($jQuery('td input:checkbox') != undefined) {
                var TotalCount = $jQuery('td input:checkbox').length;
                if (TotalCount > 0) {
                    //debugger;
                    var checkedCount = 0;
                    for (var i = 1; i < TotalCount; i++) {
                        if ($jQuery('td input:checkbox')[i] != undefined) {
                            if ($jQuery('td input:checkbox')[i].checked) {
                                checkedCount = checkedCount + 1;
                            }
                        }
                    }
                    if (TotalCount - 1 == checkedCount) {
                        $jQuery('td input:checkbox')[0].checked = true;
                    }
                    else {
                        $jQuery('td input:checkbox')[0].checked = false;
                    }

                }
            }
        }

    </script>
    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlRejectedItems">
        <div id="dvRejectedItems" class="section">
            <h1 class="mhdr">Requeue Rejected Items</h1>
            <div class="content">
                <div class="msgbox" id="dvMsgBox" runat="server">
                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                    <asp:Label ID="lblSuccessMessage" runat="server" CssClass="sucs"></asp:Label>
                </div>
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">&nbsp;</div>
                        <%--<div class="col-md-12 text-left form-group">--%>
                        <div class="col-md-12 form-control text-left">
                            <label id="lblSelection">You have successfully added an alias name to your profile. If any of the following items were rejected due to a mismatched name, please select the items for resubmission.</label>
                        </div>
                    </div>

                    <div class="col-md-12">&nbsp;</div>
                    <div class="clearfix"></div>
                    <div class="col-md-12">
                        <asp:Panel ID="pnlRejectedItemList" runat="server">
                            <div id="dvRejectedItemList" runat="server" class="table-responsive" style="overflow-y: auto; max-height: 63vh;">
                                <table id="tblAdminConfig" class="table table-bordered">
                                    <tbody>
                                        <tr style="background-color: #efefef; word-wrap: hyphenate;">
                                            <td style="text-align: center; vertical-align: middle; width: 5%;">
                                                <asp:CheckBox ID="chkAllItems" runat="server" onclick="checkAll(this);" ToolTip="Click to check all items" />
                                            </td>
                                            <td style="text-align: center; width: 15%;">
                                                <span class="form-control">Package Name</span>
                                            </td>
                                            <td style="text-align: center; width: 15%;">
                                                <span class="form-control">Category Name</span>
                                            </td>
                                            <td style="text-align: center; width: 15%;">
                                                <span class="form-control">Item Name</span>

                                            </td>
                                            <td style="text-align: center; width: 50%;">
                                                <span class="form-control">Rejection Notes</span>
                                            </td>
                                        </tr>
                                        <asp:Repeater ID="rptrRejectedItemList" runat="server">
                                            <ItemTemplate>
                                                <tr style="background-color: #fefdfd">
                                                    <asp:HiddenField ID="hdnApplicantComplianceItemId" runat="server" Value='<%#Eval("ApplicantComplianceItemID") %>' />
                                                    <td style="text-align: center; vertical-align: middle;">
                                                        <asp:CheckBox ID="chkItem" runat="server" onclick="SelectcheckAll();" />
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" CssClass="form-control left width height" ID="lblPackageName"><%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("PackageName"))) %></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" CssClass="form-control left width height" ID="lblCategoryName"><%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CategoryName"))) %></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" CssClass="form-control left width height" ID="lblItemName"><%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ItemName"))) %></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" CssClass="form-control left width height" ID="Label1"><%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("RejectionNotes"))) %></asp:Label>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                            <div class="row">&nbsp;</div>
                            <div class="col-md-12">
                                <div class="text-center">
                                    <infsu:CommandBar ID="fsucCommandBar" runat="server" ButtonPosition="Center" DisplayButtons="Save,Cancel"
                                        AutoPostbackButtons="Save,Cancel" OnSaveClick="fsucCommandBar_SaveClick" OnCancelClientClick="CloseRejectedSubmissionPopup"
                                        SaveButtonText="Requeue items for review" SaveButtonIconClass="rbSave" CancelButtonIconClass="rbCancel" CancelButtonText="Cancel" UseAutoSkinMode="false" ButtonSkin="Silk">
                                    </infsu:CommandBar>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="server">
</asp:Content>
