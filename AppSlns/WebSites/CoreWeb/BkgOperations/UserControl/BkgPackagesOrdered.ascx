<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BkgPackagesOrdered.ascx.cs"
    Inherits="CoreWeb.BkgOperations.Views.BkgPackagesOrdered" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<%@ Register TagName="ProfileMapping" TagPrefix="uc" Src="~/BkgOperations/UserControl/BkgOrderProfileMapping.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxBkgPackagesOrdered">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style>
    .rgRefresh {
        background-image: url('/Resources/Mod/Dashboard/images/refresh.png') !important;
        background-repeat: no-repeat !important;
        background-position: 1px 1px !important;
    }

        .rgRefresh + a {
            font-family: "Titillium Web",sans-serif !important;
            font-size: 16px !important;
            font-weight: 600;
        }

    /*UAT-2864*/
    .resizetxtbox {
        resize: vertical !important;
        width: 100% !important;
        height: 100%;
        border-width: 1px !important;
        border-color: #6788be !important;
    }
</style>
<div class="col-md-12">
    <div class="row">
        <div class="msgbox" id="divSuccessMsg">
            <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
        </div>
    </div>
</div>

<%--<div class="section">
    <h1 class="mhdr">Packages Ordered
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>

        </div>
        <div id="Div1" class="swrap" runat="server">
            <infs:WclGrid runat="server" ID="grdPackagesOrdered" AutoGenerateColumns="false"
                AllowSorting="True" AutoSkinMode="True" CellSpacing="0"
                GridLines="Both" ShowAllExportButtons="False" ShowExtraButtons="True" OnNeedDataSource="grdPackagesOrdered_NeedDataSource" EnableDefaultFeatures="false">
                <MasterTableView CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false"
                        ShowExportToExcelButton="False" ShowExportToPdfButton="False" ShowExportToCsvButton="False"
                        ShowRefreshButton="False" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="BPA_Name" FilterControlAltText="Filter Name column"
                            HeaderText="Name" SortExpression="BPA_Name" UniqueName="BPA_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PackagePrice" FilterControlAltText="Filter Price column" DataFormatString="{0:c}"
                            HeaderText="Package Price" SortExpression="PackagePrice" UniqueName="PackagePrice">
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
    <div class="dummyBtn" style="display: none;">
        <infs:WclButton runat="server" ID="WclButton1" Text=""
            ButtonType="LinkButton" Height="30px">
        </infs:WclButton>
    </div>
</div>--%>
<div class="row">&nbsp;</div>
<div class="row">
    <div class="col-md-10"></div>
    <div class="col-md-2">
        <infs:WclButton ID="btnAddSvcLineItmMapping" runat="server" AutoPostBack="true" Skin="Silk" AutoSkinMode="false" Text="Add Line Item" Icon-PrimaryIconCssClass="rbSave"
            OnClick="btnAddSvcLineItmMapping_Click">
        </infs:WclButton>
    </div>
</div>


<div class="container-fluid" id="dvDefault" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">External Vendor Services
            </h2>
        </div>
    </div>
    <div class="row" runat="server">
        <infs:WclGrid runat="server" ID="grdExternalVendorServices" AllowPaging="false" AutoGenerateColumns="False"
            AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="false" CellSpacing="0" BorderWidth="0px" BorderStyle="NotSet"
            OnNeedDataSource="grdExternalVendorServices_NeedDataSource" EnableDefaultFeatures="false"
            GridLines="None" OnItemDataBound="grdExternalVendorServices_ItemDataBound" OnUpdateCommand="grdExternalVendorServices_UpdateCommand" OnItemCommand="grdExternalVendorServices_ItemCommand">
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="BOR_ID,PSLI_ID" CommandItemSettings-ShowAddNewRecordButton="false">
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="BSE_Name" FilterControlAltText="Filter Service Name column"
                        HeaderText="Service Name" SortExpression="BSE_Name" UniqueName="BSE_Name">
                        <ItemStyle Wrap="true" Width="300px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="VendorStatus" FilterControlAltText="Filter Vendor Status column"
                        HeaderText="Vendor Status" SortExpression="VendorStatus" UniqueName="VendorStatus">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PSLI_ID" FilterControlAltText="Filter PSLI_ID column"
                        Visible="false"
                        HeaderText="PSLI_ID" SortExpression="PSLI_ID" UniqueName="PSLI_ID">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="Flagged" FilterControlAltText="Filter Flagged column"
                        HeaderText="Flagged" SortExpression="Flagged" UniqueName="Flagged">
                        <ItemTemplate>
                            <asp:CheckBox ID="IsFlagged" runat="server" Enabled="false" Checked='<%# Convert.ToBoolean(Eval("Flagged"))== true ? true :false %>' />
                            <asp:Label ID="lblFlaggedText" runat="server" Text='<%# Convert.ToBoolean(Eval("Flagged"))== true ? INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("FlaggedText")) ) :String.Empty %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="EVOD_VendorProfileID" FilterControlAltText="Filter Vendor Profile ID column"
                        HeaderText="Vendor Profile ID" SortExpression="EVOD_VendorProfileID" UniqueName="EVOD_VendorProfileID">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="VendorCode" FilterControlAltText="Filter Vendor Code column"
                        HeaderText="Vendor Service Code" SortExpression="VendorCode" UniqueName="VendorCode">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="VendorOrderID" FilterControlAltText="Filter Vendor Order ID column"
                        HeaderText="Vendor Order ID" SortExpression="VendorOrderID" UniqueName="VendorOrderID">
                    </telerik:GridBoundColumn>

                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                        Visible="true">
                        <%--   <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" />--%>
                    </telerik:GridEditCommandColumn>

                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="LinkProfile">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnLinkProfile" ButtonType="LinkButton" CommandName="LinkProfile"
                                ToolTip="Click here to link/edit profile for line item." runat="server" Text="Link Profile">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div class="col-md-12">
                            <div class="row bgLightGreen" visible="true" runat="server">
                                <%-- <h1 class="mhdr">
                                    <asp:Label ID="lblEvService" Text='<%# (Container is GridEditFormInsertItem) ? "Add New External Vendor Service" : "Edit External Vendor Service" %>'
                                        runat="server" /></h1>--%>

                                <div class='col-md-12'>
                                    <div class="row">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <asp:Panel runat="server" ID="pnlExternalVendor">
                                    <div class='col-md-12'>
                                        <div class="row">
                                            <div class='form-group col-md-6'>
                                                <span class="cptn">Vendor Service Results</span>
                                                <%-- <infs:WclTextBox runat="server" ID="txtVendorServiceResults" TextMode="MultiLine"
                                                    Width="100%" CssClass="resizetxtbox borderTextArea"  ReadOnly="true">
                                                </infs:WclTextBox>--%>

                                                <asp:TextBox runat="server" ID="txtVendorServiceResults" TextMode="MultiLine" CssClass="resizetxtbox borderTextArea" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class='col-md-12'>
                                        <div class="row">
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Is Vendor Flagged</span>
                                                <asp:CheckBox ID="IsVendorFlagged" runat="server" Enabled="false" />
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Vendor Result Status</span>
                                                <asp:Label ID="lblVendorResultStatus" CssClass="form-control" runat="server" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("VendorStatus"))) %>'></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12 text-center">
                                        <div class="row">
                                            <infs:WclButton runat="server" ID="btnCopy" Text="Copy Vendor Results to ADB Results"
                                                AutoPostBack="false" OnClientClicked="CopyTextToADBResult" AutoSkinMode="false"
                                                Skin="Silk">
                                            </infs:WclButton>
                                        </div>
                                    </div>
                                    <div class='col-md-12'>
                                        <div class="row">
                                            <div class='form-group col-md-6'>
                                                <span class="cptn">ADB Service Results</span><span class="reqd">*</span>
                                                <%--  <infs:WclTextBox runat="server" ID="txtADBServiceResults" TextMode="MultiLine" Width="100%"
                                                    CssClass="resizetxtbox borderTextArea">
                                                </infs:WclTextBox>--%>
                                                <asp:TextBox runat="server" ID="txtADBServiceResults" TextMode="MultiLine" CssClass="resizetxtbox borderTextArea"></asp:TextBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvADBServiceResults" ControlToValidate="txtADBServiceResults"
                                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="ADB Service Results is required."
                                                        ValidationGroup="grpFormSubmit" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class='col-md-12'>
                                        <div class="row">
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">ADB Flagged</span>
                                                <asp:CheckBox ID="chkADBFlagged" runat="server" />
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Service Status</span>
                                                <infs:WclComboBox ID="cmbServiceStatus" DataTextField="LIRS_NAME" Enabled="false"
                                                    DataValueField="LIRS_ID" runat="server" Width="100%" CssClass="form-control"
                                                    Skin="Silk" AutoSkinMode="false">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvcmbServiceStatus" ControlToValidate="cmbServiceStatus"
                                                        InitialValue="--SELECT--"
                                                        Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Service Status is required." />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="col-md-12 text-right">
                            <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlExternalVendor"
                                ValidationGroup="grpFormSubmit" GridInsertText="Save" GridUpdateText="Save" ExtraButtonIconClass="icnreset"
                                UseAutoSkinMode="false" ButtonSkin="Silk" />
                        </div>
                    </FormTemplate>
                </EditFormSettings>
            </MasterTableView>
        </infs:WclGrid>
    </div>
</div>


<%--<div id="dvSvcLineItemMapping" runat="server" visible="false">
    <uc:ProfileMapping ID="ucSvcLineItemMapping" runat="server" />
</div>--%>
<asp:HiddenField ID="hdnIsSavedSuccessfully" runat="server" />
<asp:HiddenField ID="hdnIsUpdatesSuccessfully" runat="server" />
<asp:Button ID="btnDoPostback" runat="server" CssClass="buttonHidden" OnClick="btnDoPostback_Click" />

<script>
    function CopyTextToADBResult(sender, args) {
        var txtVendorServiceResults = $jQuery("[id$=txtVendorServiceResults]").val();
        $jQuery("[id$=txtADBServiceResults]").val(txtVendorServiceResults);
    }

    //UAT-4004
    function OpenProfileMappingPopup(pkgSivcLineItemID, isLinkProfile) {
        //debugger;
        var popupWindowName = "Profile Mapping";

        winopen = true;
        var url = "BkgOperations/Pages/BkgOrderProfileMappingPopup.aspx?PackageServiceLineItemID=" + pkgSivcLineItemID + "&IsLinkProfile=" + isLinkProfile;
        var popupHeight = $jQuery(window).height() * (100 / 100);
        var win = $window.createPopup(url, { size: "1100," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnProfileMappingPopupClose }, function () { this.set_title(popupWindowName)});
        return false;
    }

    function OnProfileMappingPopupClose(oWnd, args) {
       // debugger;
        oWnd.remove_close(OnProfileMappingPopupClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                //do button postback to show success message.
                $jQuery("[id$=hdnIsSavedSuccessfully]").val(arg.IsSavedSuccessfully);
                $jQuery("[id$=hdnIsUpdatesSuccessfully]").val(arg.IsUpdatesSuccessfully);
                // 1
                $jQuery("#<%=btnDoPostback.ClientID %>").trigger('click');
            }
            winopen = false;
        }
    }
    //End UAT-4004

</script>
