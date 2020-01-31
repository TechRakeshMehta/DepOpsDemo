<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BkgOrderClientStatus.aspx.cs" Inherits="CoreWeb.BkgOperations.Views.BkgOrderClientStatus"
    MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        // To close the popup.
        function ClosePopup() {
            top.$window.get_radManager().getActiveWindow().close();
        }
    </script>
    <div id="divClientStatus" runat="server">
        <div class="content">
            <div class="sxform auto">
                <asp:Panel ID="pnlPackageName" CssClass="sxpnl" runat="server">
                    <div class="msgbox">
                        <asp:Label ID="lblMessage" runat="server" CssClass="info">
                        </asp:Label>
                    </div>
                    <div class='sxro sx1co'>
                        <div class='sxlb'>
                            <span class="cptn">Status</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="rcOrderClientStatusType" runat="server" DataTextField="BOCS_OrderClientStatusTypeName" DataValueField="BOCS_ID" OnSelectedIndexChanged="selected_IndexChanged" AutoPostBack="true"></infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvOrderClientStatus" ControlToValidate="rcOrderClientStatusType"
                                    Display="Dynamic" ValidationGroup="saveClientStatus" CssClass="errmsg"  InitialValue="--Select--"
                                    Text="Client Status is required." />
                            </div>
                        </div>
                    </div>
                    <div class='sxro sx1co'>
                        <div class='sxlb'>
                            <span class="cptn">Notes</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox TextMode="MultiLine" ID="txtNotes" MaxLength="1026" runat="server" Height="100px" Width="100px"></infs:WclTextBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvNotes" ControlToValidate="txtNotes"
                                    Display="Dynamic" ValidationGroup="saveClientStatus" CssClass="errmsg"
                                    Text="Notes is required." />
                            </div>
                        </div>

                    </div>

                    <div class='sxro sx1co'>
                        <div class='sxlb'>
                            <span class="cptn">Notes History</span>
                        </div>
                    </div>

                    <infs:WclGrid runat="server" ID="grdBkgOrderClientStatus" AllowPaging="false" AutoGenerateColumns="False"
                        AllowSorting="false" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0" PagerStyle-AlwaysVisible="false"
                        GridLines="Both" EnableDefaultFeatures="false" ShowAllExportButtons="False" ShowExtraButtons="false"
                        NonExportingColumns="EditCommandColumn,DeleteColumn" OnNeedDataSource="grdBkgOrderClientStatus_NeedDataSource">
                        <%-- <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                            HideStructureColumns="true" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                            Pdf-PageRightMargin="20mm">
                            <Excel AutoFitImages="true" />
                        </ExportSettings>--%>
                        <%--  <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                        </ClientSettings>--%>
                        <%--   <GroupingSettings CaseSensitive="false" />--%>
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="OCSH_ID" PagerStyle-AlwaysVisible="false">
                            <CommandItemSettings ShowAddNewRecordButton="false"
                                ShowExportToCsvButton="False" ShowExportToExcelButton="False" ShowExportToPdfButton="False"
                                ShowRefreshButton="False" />
                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="OCSH_Notes" AllowFiltering="false" AllowSorting="false"
                                    HeaderText="Notes" SortExpression="OCSH_Notes" UniqueName="Notes">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </infs:WclGrid>
                </asp:Panel>
            </div>
            <div>
                <infsu:CommandBar ID="fsucFeatureActionList" runat="server" AutoPostbackButtons="Save" OnSaveClick="fsucFeatureClientStatus_SaveClick" DisplayButtons="Save,Cancel" CancelButtonText="Close"
                    ButtonPosition="Right" CauseValidationOnCancel="false" OnCancelClientClick="ClosePopup" ValidationGroup="saveClientStatus" />
            </div>
        </div>
    </div>

</asp:Content>

