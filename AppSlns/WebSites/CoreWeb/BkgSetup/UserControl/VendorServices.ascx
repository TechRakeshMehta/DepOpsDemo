<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorServices.ascx.cs" Inherits="CoreWeb.BkgSetup.UserControl.Views.VendorServices" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>



<div class="section">
    <h1 class="mhdr">Vendor Services
    </h1>
    <div class="content">
        <%--<div runat="server" id="divImportSvc" visible="false" style="float: right; margin-bottom: 10px;">
            <a href="javascript:void" onclick="openPopUp()" class="RadButton RadButton_Outlook rbLinkButton">+ Import Clearstar Services</a>
        </div>--%>
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server"> </asp:Label>
            </div>

            <infs:WclGrid runat="server" ID="grdExternalBkgSvc" AllowPaging="True" PageSize="50" AutoGenerateColumns="False" AllowFilteringByColumn="true"
                AllowSorting="True" GridLines="Both" ClearFiltersButtonText="Clear filters" ShowClearFiltersButton="true"  AutoSkinMode="true" EnableDefaultFeatures="true"
                ShowAllExportButtons="false" OnNeedDataSource="grdExternalBkgSvc_NeedDataSource" OnDetailTableDataBind="grdExternalBkgSvc_DetailTableDataBind">
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="EBS_ID" AllowFilteringByColumn="true" PageSize="50">
                    <CommandItemSettings  ShowAddNewRecordButton="false" ShowRefreshButton="true" ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="EBS_Name" HeaderText="Service Name"
                            UniqueName="EBS_Name" DataType="System.String"
                            SortExpression="EBS_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EBS_ExternalCode" UniqueName="EBS_ExternalCode"
                            HeaderText="External Service Code">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <DetailTables>
                        <telerik:GridTableView runat="server" CommandItemDisplay="None" DataKeyNames="EBSA_ID"
                            AllowPaging="true" AllowFilteringByColumn="false" Caption="<h6>External Background Service Attributes</h6>"
                            Width="100%">
                            <%--<ParentTableRelation>
                                <telerik:GridRelationFields DetailKeyField="EBSA_ID" MasterKeyField="EBS_ID" />
                            </ParentTableRelation>--%>
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="EBSA_Label"
                                    HeaderText="Label" ReadOnly="true" SortExpression="EBSA_Label"
                                    UniqueName="EBSA_Label" AllowSorting="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="EBSA_Name"
                                    HeaderText="Name" ReadOnly="true" SortExpression="EBSA_Name" UniqueName="EBSA_Name">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="EBSA_LocationField"
                                    HeaderText="Location Field" SortExpression="EBSA_LocationField" UniqueName="EBSA_LocationField">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="EBSA_DefaultValue" HeaderText="DefaultValue" SortExpression="EBSA_DefaultValue"
                                    UniqueName="EBSA_DefaultValue">
                                </telerik:GridBoundColumn>
                                <telerik:GridMaskedColumn DataField="EBSA_IsRequired" HeaderText="Is Required" SortExpression="EBSA_IsRequired"
                                    UniqueName="EBSA_IsRequired">
                                </telerik:GridMaskedColumn>
                                <telerik:GridMaskedColumn DataField="EBSA_IsVisible" HeaderText="Is Visible" SortExpression="EBSA_IsVisible"
                                    UniqueName="EBSA_IsVisible">
                                </telerik:GridMaskedColumn>
                            </Columns>
                        </telerik:GridTableView>
                    </DetailTables>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
            </infs:WclGrid>

        </div>
    </div>
</div>
<div style="width: 100%; text-align: center" id="dvShowBackLink" runat="server">
    <infs:WclButton runat="server" ID="btnImportServices" Text="Import Services" Visible="false" OnClientClicked="openPopUp" AutoPostBack="false">
    </infs:WclButton>
    <infs:WclButton runat="server" ID="btnGoBack" Text="Go Back To Service Vendors" OnClick="CmdBarCancel_Click">
    </infs:WclButton>
</div>
<asp:HiddenField runat="server" ID="hdnVendorId" />
<script type="text/javascript">
    function openPopUp() {
        var composeScreenWindowName = "Import External Services";
        var VendorID = $jQuery("[id$=hdnVendorId]").val();
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/BkgSetup/Pages/ImportClearstarServicesPopup.aspx?VendorID=" + VendorID);
        var win = $window.createPopup(url, { size: "950,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName });
    }
</script>
