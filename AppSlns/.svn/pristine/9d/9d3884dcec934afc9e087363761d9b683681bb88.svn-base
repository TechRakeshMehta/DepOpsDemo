<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.RuleAssociationViewer"
    Title="Rule Association" MasterPageFile="~/Shared/PopupMaster.master" CodeBehind="RuleAssociationViewer.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">

    <infs:WclResourceManagerProxy runat="server" ID="rprxSharingPackages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/mapping_pages.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    </infs:WclResourceManagerProxy>
    <script type="text/javascript">
        function pageLoad() {
            SetSelectAllCheckboxState()
            BindEvents();
        }

        function RebindCheckboxesEvents() {
            $jQuery("[id*='chkCompliancePackage'] input[type='checkbox']").unbind("click");
            $jQuery("#chkSelectAll").unbind("click");

            $jQuery("#chkSelectAll").prop('checked', false);
            $jQuery("#lblSelection").text('Select All');
            BindEvents();
        }

        function BindEvents() {
            $jQuery("[id*='chkCompliancePackage'] input[type='checkbox']").click(function () {
                SetSelectAllCheckboxState();
            });


            $jQuery("#chkSelectAll").click(function () {
                var compliancePkgs = $jQuery("[id*='chkCompliancePackage'] input[type='checkbox']");

                var isChkSelectAllStateChecked = $jQuery("#chkSelectAll").is(":checked");

                if (isChkSelectAllStateChecked) {
                    $jQuery(compliancePkgs).prop('checked', 'checked');
                    $jQuery("#lblSelection").text('Deselect All');
                }
                else {
                    $jQuery(compliancePkgs).prop('checked', false);
                    $jQuery("#lblSelection").text('Select All');
                }
            });
        }

        function SetSelectAllCheckboxState() {

            var totalCompliancePkgs = $jQuery("[id*='chkCompliancePackage'] input[type='checkbox']").length;
            var selectedCompPkgs = $jQuery("[id*='chkCompliancePackage'] input[type='checkbox']:checked").length;

            if (totalCompliancePkgs == selectedCompPkgs) {
                $jQuery("#lblSelection").text('Deselect All');
                $jQuery("#chkSelectAll").prop('checked', 'checked');
            }
            else {
                $jQuery("#lblSelection").text('Select All');
                $jQuery("#chkSelectAll").prop('checked', false);
            }
        }


        function closePreview() {

            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;

            var oArg = {};

            oWindow.Close(oArg);

            //var window = GetPreviewWindow();
            //window.Close();
            //debugger;
            //if ($jQuery('[id$=btnRuleDeletionDoPostBack]').length > 0)
            //    $jQuery('[id$=btnRuleDeletionDoPostBack]')[0].click();
        }
        function closePopup() {        
              var window = GetPreviewWindow();
            window.Close();
        }
        function GetPreviewWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
    </script>
    <%--  <div class="section">
        <h1 class="mhdr">Rules</h1>
        <div class="content">
            <asp:Repeater id="rptRuleAssociation" runat="server">
                <ItemTemplate>
                    <infs:WclCheckBox ID="cmbShareBetween" runat="server" EnableCheckAllItemsCheckBox="true" EmptyMessage="--SELECT--" CheckBoxes="true" Height="100px"  DataTextField="PackageName" DataValueField="CompliancePackageId"   >
                    </infs:WclCheckBox>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>--%>
    <div class="container-fluid">
        <div class="msgbox" id="divSuccessMsg">
            <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
        </div>
        <div class="row">
            <div class="col-md-12">&nbsp;</div>
        </div>
        <div class="section">
            <div class="row bgLightGreen">
                <div class="col-md-12 form-group">
                    <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server" />
                    <label for="chkSelectAll" id="lblSelection">Deselect All</label>
                </div>
            </div>

            <div id="divCompliance" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-12">
                        <h2 class="header-color">Rule Association Packages</h2>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-10">&nbsp;</div>
                </div>
                <div class="row bgLightGreen">
                    <div class="col-md-12">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:CheckBoxList ID="chkCompliancePackage" runat="server"
                                    RepeatColumns="4">
                                </asp:CheckBoxList>
                                <asp:HiddenField ID="hdnPkgId" runat="server" Value='<%# Eval("PackageId") %>' />
                            </div>
                        </div>
                        <div class="row">&nbsp;</div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-10">&nbsp;</div>
                </div>
            </div>

            <infsu:CommandBar ID="cmdBar"  runat="server"  SaveButtonIconClass="fa fa-trash-o" DisplayButtons="Save,Cancel" SaveButtonText="Delete Rule Association" CancelButtonText="Cancel"
                ButtonPosition="Center" OnCancelClientClick="closePopup" CauseValidationOnCancel="false" AutoPostbackButtons="Save" OnSaveClick="cmdBar_SaveClick">
            </infsu:CommandBar>
        </div>
    </div>
</asp:Content>



