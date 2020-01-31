<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeBkgOrderColorStatus.aspx.cs" Inherits="CoreWeb.BkgOperations.Views.ChangeBkgOrderColorStatus"
    MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <infs:WclResourceManagerProxy runat="server" ID="rprChangeBkgOrder">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <script type="text/javascript">

        var tabKey = 9;
        // To close the popup.
        function ClosePopup() {
            //top.$window.get_radManager().getActiveWindow().close();
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;

            var hdnSaveStatus = $jQuery("[id$=hdnSaveStatus]");
            var oArg = {};
            if (hdnSaveStatus != undefined && hdnSaveStatus.val() != "" && hdnSaveStatus.val() == "True") {
                oArg.Action = "Submit";
                oArg.IsStatusSaved = true;
            }
            else { oArg.IsStatusSaved = false; }

            oWindow.Close(oArg);
        }

        $jQuery(document).ready(function () {
          //  $jQuery(".rbPrimaryIcon.rbSave").removeClass().addClass("fa fa-floppy-o");
            //For accessibility, we need to prevent focus to go outside after tabbing on last link
            $jQuery(document).on("keydown", "#<%= fsucFeatureActionList.CancelButton.ClientID %>", function (e) {
                if (e.keyCode == tabKey && !e.shiftKey) {
                    e.preventDefault();
                    $jQuery("[id$=rcbInstitutionStatusColorIcons_Input]").focus();
                }
            });

            $jQuery("[id$=rcbInstitutionStatusColorIcons_Input]").focus();

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
            $jQuery(document).on("keydown", "[id$=rcbInstitutionStatusColorIcons_Input]", function (e) {
                if (e.shiftKey && e.keyCode == tabKey) {
                    e.preventDefault();
                    $jQuery("[id$=<%= fsucFeatureActionList.CancelButton.ClientID %>]").focus();
                }
            });
        });

       

        function ValidatorUpdateDisplay(val) {
            if (typeof (val.display) == "string") {
                if (val.display == "None") {
                    return;
                }
                if (val.display == "Dynamic") {
                    val.style.display = val.isvalid ? "none" : "inline";
                    if (!val.isvalid) {
                        $jQuery("[id$=" + val.controltovalidate + "_Input]").focus();
                    }
                    return;
                }

            }
            val.style.visibility = val.isvalid ? "hidden" : "visible";
            if (val.isvalid) {
                document.getElementById(val.controltovalidate).style.border = '1px solid #333';
            }
            else {
                document.getElementById(val.controltovalidate).style.border = '1px solid red';
            }
        }
    </script> 
   
   <%-- <link href="<%= ResolveUrl("~/Resources/Mod/Dashboard/Styles/bootstrap.min.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Resources/Mod/Dashboard/Styles/font-awesome.min.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Resources/Mod/Shared/ApplyNewIcons.js") %>" type="text/javascript" />--%>

    <div class="container-fluid" title="containerDiv" id="containerDiv">
        <%--<div class="row">
            <div class="col-md-8">
                <p class="header-color">
                    <span class="cptn">Update Order Color Flag</span>
                </p>
            </div>
        </div>--%>
        <div class="row" style="padding-top: 10px;">
            <div class="col-md-4">
                <div class="row">
                    <div class='form-group col-md-3'>
                        <%--<span class="cptn">Flag Color</span><span class="reqd">*</span>--%>
                        <label class="cptn" for="<%= rcbInstitutionStatusColorIcons.ClientID %>_Input">Enter Flag Color<span class="reqd">*</span> </label>
                        <infs:WclComboBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="rcbInstitutionStatusColorIcons"
                            runat="server" DataValueField="IOF_ID" DataTextField="OFL_Tooltip" EmptyMessage="--Select--" CssClass="form-control"
                            AllowCustomText="true" EnableAriaSupport="true">
                            <ItemTemplate>
                                <asp:Image ID="imbIcon" runat="server" ImageUrl='<%# String.Format("~/{0}/{1}", Eval("OFL_FilePath"), Eval("OFL_FileName")) %>' />
                                <asp:Label ID="lblTooltip" runat="server" Text='<%# Eval("OFL_Tooltip") %>'></asp:Label>
                            </ItemTemplate>
                        </infs:WclComboBox>

                        <asp:RequiredFieldValidator role="alert" runat="server" ID="rfvInstitutionStatusColor" ControlToValidate="rcbInstitutionStatusColorIcons"
                            Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Flag Color is required." SetFocusOnError="true" />

                    </div>
                </div>
            </div>
        </div>
        <div class="row" style="padding-top: 40px;">
            <div class="col-md-12">
                <infsu:CommandBar ID="fsucFeatureActionList" runat="server" AutoPostbackButtons="Save" OnSaveClick="fsucFeatureActionList_SaveClick" DisplayButtons="Save,Cancel" CancelButtonText="Close"
                    ButtonPosition="Center"  CauseValidationOnCancel="false" OnCancelClientClick="ClosePopup" ValidationGroup="grpFormSubmit" ButtonSkin="Silk" UseAutoSkinMode="false" />
            </div>
        </div>
        <asp:HiddenField ID="hdnSaveStatus" Value="False" runat="server" />
    </div>
</asp:Content>
