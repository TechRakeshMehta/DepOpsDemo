<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackingAutoAssignmentConfigurationDetail.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.TrackingAutoAssignmentConfigurationDetail" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

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


<script type="text/javascript">

  

    var minDate = new Date("01/01/1980");
    function SetMinDate(picker) {
        picker.set_minDate(minDate);
    }

    function CorrectStartToEndDate(picker) {
        //debugger;
        var rpItemCount = $jQuery('[id$=tblAdminConfig] >tbody >tr').length;
        var fromDate = $jQuery(picker)[0].get_selectedDate();
        var pickerId = $jQuery(picker)[0]._element.id;

        for (var i = 0; i < rpItemCount; i++) {
            if ($jQuery("[id$=dpDateFrom]")[i].id == pickerId) {
                //var alreadySelectedEnddate = $jQuery("[id$=dpDateTo]")[i].control.get_selectedDate();
                //if (fromDate > alreadySelectedEnddate) {
                //    $jQuery("[id$=dpDateTo]")[0].control.set_selectedDate(null);
                //}
                if (fromDate != null) {
                    $jQuery("[id$=dpDateTo]")[i].control.set_minDate(fromDate);
                    break;
                }
                else {
                    $jQuery("[id$=dpDateTo]")[i].control.set_minDate(minDate);
                    break;
                }
            }
        }
    }

    function OnKeyPress(sender, args) {
        //debugger;
        if (args.get_keyCode() == 13) {
            args.get_domEvent().preventDefault();
            args.get_domEvent().stopPropagation();
            //$jQuery("[id$=btnSaveAbdReturnToQueue]").click();
        }
    }

    function ObjectPriorityClick(sender) {
        //debugger;
        $jQuery("[id$=btnObjectPriority]").click();
    }
</script>

<div class="container-fluid" id="dvConfiguration">
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="row">
                    <h2 class="header-color" tabindex="0">
                        <asp:Label ID="lblAutoAssignmentConfigDetails" runat="server" Text="Auto Assignment Configuration" CssClass="page-heading"></asp:Label>
                    </h2>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row text-right">
                    <asp:LinkButton runat="server" ID="lnkGoBack" Text="Back to configuration list" OnClick="lnkGoBack_Click"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel runat="server" ID="pnlAdmins">
            <div class="col-md-12">
                <div class="row">
                    <div id="dvAdmins">
                        <div class='form-group col-md-3' title="Select the admins whose data you want to configure for auto assigment">
                            <span class="cptn">Select Admin(s)</span><span class="reqd">*</span>
                            <infs:WclComboBox ID="cmbAdmins" runat="server" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Filter="Contains"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false"
                                OnClientKeyPressing="openCmbBoxOnTab" EmptyMessage="--Select--" DataTextField="FirstName" DataValueField="OrganizationUserID">
                                <Localization CheckAllString="All" />
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvAdmin" ControlToValidate="cmbAdmins"
                                    Display="Dynamic" CssClass="errmsg" Text="Admin is required." ValidationGroup="grpFormSubmit" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-md-12">
                <div class="form-group col-md-12">
                    <div class="row text-center">
                        <infsu:CommandBar ID="fsucCommandBar" runat="server" ButtonPosition="Center" DisplayButtons="Save,Extra,Cancel"
                            AutoPostbackButtons="Save,Cancel,Extra" OnSaveClick="fsucCommandBar_SaveClick" OnCancelClick="fsucCommandBar_CancelClick" OnExtraClick="fsucCommandBar_ExtraClick"
                            SaveButtonText="Next" SaveButtonIconClass="rbNext" ExtraButtonText="Reset" ExtraButtonIconClass="rbUndo"
                            CancelButtonIconClass="rbCancel" CancelButtonText="Cancel" UseAutoSkinMode="false" ButtonSkin="Silk" ValidationGroup="grpFormSubmit">
                        </infsu:CommandBar>
                    </div>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="col-md-12">
            <asp:Panel ID="pnlAdminConfig" Visible="false" runat="server">
                <div id="dvAdminConfigTable" runat="server" class="table-responsive" style="overflow-y: auto; max-height: 60vh;">
                    <table id="tblAdminConfig" class="table table-bordered">
                        <tbody>
                            <asp:Repeater ID="rptrAdminConfig" runat="server">
                                <ItemTemplate>
                                    <tr style="background-color: #efefef">
                                        <asp:HiddenField ID="hdnAdminID" runat="server" Value='<%#Eval("OrganizationUserID") %>' />
                                        <td><span class="cptn">Name</span></td>
                                        <td>
                                            <infs:WclTextBox ID="txtAdminName" runat="server" ClientEvents-OnKeyPress="OnKeyPress" ReadOnly="true" Enabled="false" Text='<%#Eval("FirstName") %>'></infs:WclTextBox>

                                        </td>
                                        <td><span class="cptn">Assignment Count</span><%--<span class="reqd">*</span></td>--%>
                                        <td>
                                            <infs:WclNumericTextBox ID="txtAssignmentCount" runat="server" MaxLength="5" NumberFormat-DecimalDigits="0" Value="0" MinValue="0" ClientEvents-OnKeyPress="OnKeyPress"></infs:WclNumericTextBox>
                                            <%--<div class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvAssignmentCount" ControlToValidate="txtAssignmentCount"
                                                    Display="Dynamic" class="errmsg" ErrorMessage="Assignment count is required." ValidationGroup='grpAdminConfig'
                                                    Enabled="true" />
                                            </div>--%>
                                        </td>
                                        <%--  COMMENTED CODE for UAT-3075 --%>
                                        <%-- <td><span class="cptn">Submission Start Date</span><span class="reqd">*</span></td>
                                        <td>
                                            <infs:WclDatePicker ID="dpDateFrom" runat="server" ClientEvents-OnPopupOpening="SetMinDate"
                                                ClientEvents-OnDateSelected="CorrectStartToEndDate" DateInput-ClientEvents-OnKeyPress="OnKeyPress">
                                            </infs:WclDatePicker>
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvStartDate" ControlToValidate="dpDateFrom"
                                                    Display="Dynamic" CssClass="errmsg" ValidationGroup="grpAdminConfig"
                                                    Text="Start Date is required." />
                                            </div>
                                        </td>
                                        <td><span class="cptn">Submission End Date</span><span class="reqd">* </span></td>
                                        <td>
                                            <infs:WclDatePicker ID="dpDateTo" runat="server" DateInput-ClientEvents-OnKeyPress="OnKeyPress"></infs:WclDatePicker>
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvEndDate" ControlToValidate="dpDateTo"
                                                    Display="Dynamic" CssClass="errmsg" ValidationGroup="grpAdminConfig"
                                                    Text="End Date is required." />
                                                <asp:CompareValidator ID="cvEndDate" runat="server" Operator="GreaterThanEqual" Type="Date" ControlToCompare="dpDateFrom" ControlToValidate="dpDateTo"
                                                    class="errmsg" ValidationGroup="grpAdminConfig" Display="Dynamic" ErrorMessage="Submission end date should be greater than submission start date." />
                                            </div>
                                        </td>--%>
                                        <td><span class="cptn">Days From</span><span class="reqd">*</span></td>
                                        <td>
                                            <infs:WclNumericTextBox ID="txtDaysFrom" runat="server" MaxLength="5" NumberFormat-DecimalDigits="0" MinValue="0" ClientEvents-OnKeyPress="OnKeyPress"></infs:WclNumericTextBox>
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvDaysFrom" ControlToValidate="txtDaysFrom"
                                                    Display="Dynamic" CssClass="errmsg" ValidationGroup="grpAdminConfig"
                                                    Text="Days from is required." />
                                            </div>
                                        </td>
                                        <td><span class="cptn">Days To</span><span class="reqd">*</span></td>
                                        <td>
                                            <infs:WclNumericTextBox ID="txtDaysTo" runat="server" MaxLength="5" NumberFormat-DecimalDigits="0" MinValue="0" ClientEvents-OnKeyPress="OnKeyPress"></infs:WclNumericTextBox>
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvDaysTo" ControlToValidate="txtDaysTo"
                                                    Display="Dynamic" CssClass="errmsg" ValidationGroup="grpAdminConfig"
                                                    Text="Days to is required." />
                                            </div>
                                        </td>
                                        <td><span class="cptn">Compliance Object</span></td>
                                        <td>
                                            <infs:WclComboBox ID="ddlObjects" runat="server" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Filter="Contains"
                                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false"
                                                OnClientKeyPressing="openCmbBoxOnTab" EmptyMessage="--Select--" DataTextField="CPO_Name" DataValueField="CPO_ID"
                                                OnClientDropDownClosed="ObjectPriorityClick">
                                                <Localization CheckAllString="All" />
                                            </infs:WclComboBox>
                                        </td>
                                        <asp:HiddenField ID="hdnOldSelectedObjects" runat="server" />
                                    </tr>

                                    <%--<tr id="trObjectrptr" style="background-color: #efefef" runat="server">
                                        <table id="tblAdminConfig" class="table table-bordered">
                                            <tbody>--%>
                                    <div>
                                        <asp:Repeater ID="rptrObjectPriority" runat="server">
                                            <ItemTemplate>
                                                <tr style="background-color: #efefef">
                                                    <asp:HiddenField ID="hdnObjId" runat="server" Value='<%#Eval("CPO_ID") %>' />
                                                    <td><span class="cptn">Object Name</span></td>
                                                    <td>
                                                        <infs:WclTextBox ID="txtObjectName" runat="server" ClientEvents-OnKeyPress="OnKeyPress" ReadOnly="true" Enabled="false" Text='<%#Eval("CPO_Name") %>'></infs:WclTextBox>
                                                    </td>
                                                    <td><span class="cptn">Priority</span><span class="reqd">*</span></td>
                                                    <td>
                                                        <infs:WclNumericTextBox ID="txtPriority" runat="server" MaxLength="5" NumberFormat-DecimalDigits="0" MinValue="1" EnableViewState="true" ClientEvents-OnKeyPress="OnKeyPress"></infs:WclNumericTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvPriority" ControlToValidate="txtPriority"
                                                                Display="Dynamic" CssClass="errmsg" ValidationGroup="grpAdminConfig"
                                                                Text="Priority is required." />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <%--   </tbody>
                                        </table>
                                    </tr>--%>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <div class="row">&nbsp;</div>
                <div class="col-md-12">
                    <div class="text-center">
                        <infs:WclButton ID="btnSaveAbdReturnToQueue" AutoSkinMode="false" Skin="Silk" CssClass="form-control" Width="250px" Icon-PrimaryIconCssClass="rbReturn"
                            runat="server" Text="Save and Return to Queue" OnClick="btnSaveAbdReturnToQueue_Click" ValidationGroup="grpAdminConfig">
                        </infs:WclButton>
                    </div>
                    <%-- <div class="text-center">
                        <infs:WclButton ID="btnNext" AutoSkinMode="false" Skin="Silk" CssClass="form-control" Width="100px" Icon-PrimaryIconCssClass="rbNext"
                            runat="server" Text="Next" OnClick="btnSaveAbdReturnToQueue_Click" ValidationGroup="grpAdminConfig">
                        </infs:WclButton>
                    </div>--%>
                </div>
            </asp:Panel>
        </div>

    </div>
    <asp:Button runat="server" ID="btnObjectPriority" OnClick="btnObjectPriority_Click" />
</div>



