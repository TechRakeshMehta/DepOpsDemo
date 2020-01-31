<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReviewCriteriaHierarchyMapping.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ReviewCriteriaHierarchyMapping" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<script type="text/javascript">
    //Function to validate Review Criteria checkbox selection
    function ValidateReviewCriteria(Sender, args) {
        var checkedItems = $jQuery("[id$=ddlReviewCriteria]")[0].control.get_checkedItems();
        if (checkedItems.length > 0) {
            args.IsValid = true;
            return false;
        }
        args.IsValid = false;
    }

</script>

<div class="swrap">
    <infs:WclGrid runat="server" ID="grdMappedReviewCriteria" AllowPaging="True" PageSize="10"
        AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" OnNeedDataSource="grdMappedReviewCriteria_NeedDataSource"
        OnItemCreated="grdMappedReviewCriteria_ItemCreated" OnInsertCommand="grdMappedReviewCriteria_InsertCommand"
        OnDeleteCommand="grdMappedReviewCriteria_DeleteCommand" 
        ShowAllExportButtons="False" NonExportingColumns="EditCommandColumn, DeleteColumn">
        <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
            Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
        </ExportSettings>
        <ClientSettings EnableRowHoverStyle="true">
            <Selecting AllowRowSelect="true"></Selecting>
        </ClientSettings>
        <MasterTableView CommandItemDisplay="Top" DataKeyNames="BRCHM_ID">
            <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Map Review Criteria"
                ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"></CommandItemSettings>
            <Columns>
                <telerik:GridBoundColumn DataField="BkgReviewCriteria.BRC_Name" FilterControlAltText="Filter ReviewCriteriaName column"
                    HeaderText="Review Criteria Name" SortExpression="BkgReviewCriteria.BRC_Name" UniqueName="ReviewCriteriaName" 
                    HeaderStyle-Width="130">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="BkgReviewCriteria.BRC_Description" FilterControlAltText="Filter ReviewCriteriaDescription column"
                    HeaderText="Review Criteria Description" SortExpression="BkgReviewCriteria.BRC_Description" UniqueName="ReviewCriteriaDescription" 
                    HeaderStyle-Width="130">
                </telerik:GridBoundColumn>
                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this mapping?"
                    Text="Delete" UniqueName="DeleteColumn">
                    <HeaderStyle Width="30px" />
                </telerik:GridButtonColumn>
            </Columns>
            <EditFormSettings EditFormType="Template">
                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                </EditColumn>
                <FormTemplate>
                    <div class="section">
                        <h1 class="mhdr">
                            <asp:Label ID="lblHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Map Review Criteria" : "Edit Review Criteria"%>'
                                runat="server"></asp:Label></h1>
                        <div class="content">
                            <div class="sxform auto">
                                <div class="msgbox">
                                    <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                </div>
                                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlReviewCriteria">
                                    <div class='sxro sx2co'>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblReviewCriteria" runat="server"  Text="Review Criteria" CssClass="cptn">                                                        
                                            </asp:Label><span class='reqd '>*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclComboBox ID="ddlReviewCriteria" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" AutoPostBack="false" DataTextField="BRC_Name"
                                                MaxHeight="200px" DataValueField="BRC_ID">
                                            </infs:WclComboBox>
                                            <div class='vldx'>
                                                <asp:CustomValidator ID="rfvReviewCriteria" CssClass="errmsg" Display="Dynamic" runat="server"
                                                    ErrorMessage="Review Criteria is required." EnableClientScript="true" ValidationGroup="grpValreviewCriteria"
                                                    ClientValidationFunction="ValidateReviewCriteria">
                                                </asp:CustomValidator>
                                            </div>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <infsu:CommandBar ID="fsucCmdBarMUser" runat="server" GridMode="true" DefaultPanel="pnlMUser" GridInsertText="Save" GridUpdateText="Save"
                                ExtraButtonIconClass="icnreset" ValidationGroup="grpValreviewCriteria" />
                        </div>
                    </div>
                </FormTemplate>
            </EditFormSettings>
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
        </MasterTableView>
    </infs:WclGrid>
</div>
<div class="gclr">
</div>
