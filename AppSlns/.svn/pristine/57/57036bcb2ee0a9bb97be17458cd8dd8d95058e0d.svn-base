<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.ShotSeriesShuffleRuleTest" CodeBehind="ShotSeriesShuffleRuleTest.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxShuffle">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
        .BorderStyle {
            border:2px solid grey !important;
            border-radius:5px !important; 
            padding:8px !important;
            margin-bottom:8px !important;
        }
</style>
<div class="BorderStyle" id="divDetails" runat="server">
    <div class="container-fluid" runat="server">
        <div class="row">
            <p class="header-color">
                <asp:Label ID="Label1" Text="Series Rule(s)" runat="server"></asp:Label>
            </p>
        </div>

        <div class="col-md-12">
            <div class="row">
                <asp:Repeater ID="rptShotSeriesData" runat="server" OnItemDataBound="rptShotSeriesData_ItemDataBound">
                    <ItemTemplate>
                        <asp:Label ID="lblRuleMappingDetailID" Text='<%#Eval("RuleMappingDetailID")%>' runat="server" CssClass="form-control" Visible="false">
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
                                    DataValueField="ItemComplianceStatusID" EmptyMessage="--Select--" CheckBoxes="false" Filter="None" CssClass="form-control" OnClientKeyPressing="openCmbBoxOnTab">
                                </infs:WclComboBox>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</div>
