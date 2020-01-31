<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.ComplianceRuleTest"
    Title="ComplianceRuleTest" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="ComplianceRuleTest.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .BorderStyle {
            border: 1px solid grey !important;
            border-radius: 5px !important;
            padding: 8px !important;
            margin-bottom: 8px !important;
        }

        .MaxHeight {
            height: 100% !important;
            line-height: 28px !important;
            padding: 0px;
            border: none;
            font-size: 14px;
            color: #555 !important;
            background-color: #fff;
        }
    </style>
    <script type="text/javascript">
        function pageLoad() {
            $jQuery("[id$=lblRuleExpression]").removeClass().addClass("MaxHeight");
            var lblExpressionText = $jQuery("[id$=lblExpressionText]");
            var divRuleExpression = $jQuery("[id$=divRuleExpression]");
            var divResultExpression = $jQuery("[id$=divResultExpression]");
            if (lblExpressionText != undefined && lblExpressionText != null && lblExpressionText.length > 0) {
                lblExpressionText.removeClass().addClass("MaxHeight");
            }
            //if (divRuleExpression != undefined && divRuleExpression != null && divRuleExpression.length > 0) {
            //    divRuleExpression.removeClass();
            //}
            //if (divRuleExpression != undefined && divRuleExpression != null && divRuleExpression.length > 0) {
            //    divRuleExpression.removeClass();
            //}
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxShuffle">
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
         <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <div class="container-fluid">
        <div class="col-md-12">
            <div class="row">
                <p class="header-color">
                    <asp:Label ID="lblHeader" Text="Compliance Rules Test" runat="server"></asp:Label>
                </p>
            </div>
            <div class="BorderStyle" id="divDetails" runat="server">
                <div class="container-fluid">
                    <div class="row">
                        <p class="header-color">
                            <asp:Label ID="lblSeries" Text="Compliance Rule" runat="server"></asp:Label>
                        </p>
                        <div class="col-md-12">
                            <div class="row bgLightGreen">
                                <div style="font-weight: bold">
                                    <asp:Label ID="Label1" Text="Rule Expression:" runat="server" CssClass="form-control" Height="100%">
                                    </asp:Label>
                                </div>
                                <div>
                                    <asp:Label ID="lblRuleExpression" runat="server" CssClass="form-control">
                                    </asp:Label>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="row">&nbsp;</div>
                            </div>
                            <asp:Repeater ID="rptComplianceRuleData" runat="server" OnItemDataBound="rptComplianceRuleData_ItemDataBound">
                                <ItemTemplate>
                                    <asp:Label ID="lblPlaceHolderName" Text='<%#Eval("PlaceHolderName")%>' runat="server" CssClass="form-control" Visible="false">
                                    </asp:Label>
                                    <asp:Label ID="lblObjectMappingTypeCode" Text='<%#Eval("ObjectMappingTypeCode")%>' runat="server" CssClass="form-control" Visible="false">
                                    </asp:Label>
                                    <asp:Label ID="lblComplianceAttributeDataTypeCode" Text='<%#Eval("ComplianceAttributeDataTypeCode")%>' runat="server" CssClass="form-control" Visible="false">
                                    </asp:Label>
                                    <div class="row bgLightGreen">
                                        <div class="form-group col-md-6 col-sm-6" runat="server" id="divObjectName">
                                            <asp:Label ID="lbObjectName" runat="server" CssClass="form-control" Width="100%">
                                            </asp:Label>
                                        </div>
                                        <div class="form-group col-md-6 col-sm-6" runat="server" id="divConstant" visible="false">
                                            <asp:Label ID="lblConstant" runat="server" CssClass="form-control">
                                            </asp:Label>
                                        </div>
                                        <div class="form-group col-md-6 col-sm-6" runat="server" id="divAttribute" visible="false">
                                            <infs:WclDatePicker ID="dtPicker" runat="server" Visible="false" CssClass="form-control" Width="48%" DateInput-EmptyMessage="Select a date">
                                            </infs:WclDatePicker>
                                            <infs:WclTextBox ID="txtBox" runat="server" Visible="false" CssClass="form-control" Width="48%">
                                            </infs:WclTextBox>
                                            <infs:WclNumericTextBox ID="numericTextBox" runat="server" Visible="false" CssClass="form-control" Width="48%">
                                            </infs:WclNumericTextBox>
                                            <div id="optionComboDiv">
                                                <infs:WclComboBox ID="optionCombo" runat="server" Visible="false" CssClass="form-control" Skin="Silk" AutoSkinMode="false" Width="48%">
                                                </infs:WclComboBox>
                                            </div>
                                        </div>
                                        <div class="form-group col-md-6 col-sm-6" runat="server" visible="false" id="divItem">
                                            <infs:WclComboBox ID="ddlItemStatus" runat="server" AutoPostBack="false" DataTextField="Name" Skin="Silk" AutoSkinMode="false" Width="48%"
                                                DataValueField="ItemComplianceStatusID" CheckBoxes="false" Filter="None" CssClass="form-control" OnClientKeyPressing="openCmbBoxOnTab">
                                            </infs:WclComboBox>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>

            </div>
            <div class="row">
                <div class="container-fluid">
                    <div class="col-md-12" style="position: relative; bottom: 2px;">
                        <infsu:CommandBar ButtonPosition="Center" ID="cmdbarTestRule" runat="server" DisplayButtons="Submit" ValidationGroup="grpFrmSubmit" AutoPostbackButtons="Submit"
                            SubmitButtonIconClass="rbOk" SubmitButtonText="Test Rule(s)" ButtonSkin="Silk" UseAutoSkinMode="false" OnSubmitClick="cmdbarTestRule_SubmitClick">
                        </infsu:CommandBar>
                    </div>
                </div>
            </div>
            <div id="dvRuleResult" runat="server" visible="false" class="BorderStyle">
                <div class="container-fluid">
                    <div class="row">
                        <p class="header-color">
                            <asp:Label ID="lblShuffledData" Text="Rule(s) Result" runat="server"></asp:Label>
                        </p>
                        <div class="col-md-12">
                            <div class="row bgLightGreen">
                                <div class="form-group col-md-6 col-sm-6">
                                    <asp:Label ID="lblMessage" Text="Message:" runat="server" CssClass="form-control">
                                    </asp:Label>
                                </div>
                                <div class="form-group col-md-6 col-sm-6">
                                    <asp:Label ID="lblMessageText" runat="server" CssClass="form-control">
                                    </asp:Label>
                                </div>
                            </div>
                            <div class="row bgLightGreen">
                                <div class="form-group col-md-6 col-sm-6">
                                    <asp:Label ID="lblResult" Text="Result:" runat="server" CssClass="form-control">
                                    </asp:Label>
                                </div>
                                <div class="form-group col-md-6 col-sm-6">
                                    <asp:Label ID="lblResultText" runat="server" CssClass="form-control">
                                    </asp:Label>
                                </div>
                            </div>
                            <div class="row bgLightGreen">
                                <div style="font-weight: bold">
                                    <asp:Label ID="lblExpression" Text="Expression:" runat="server" CssClass="form-control">
                                    </asp:Label>
                                </div>
                                <div>
                                    <asp:Label ID="lblExpressionText" runat="server" CssClass="form-control">
                                    </asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
