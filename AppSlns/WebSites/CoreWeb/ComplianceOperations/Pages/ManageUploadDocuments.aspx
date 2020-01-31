<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ManageUploadDocuments" Title="Manage Documents"
    MasterPageFile="~/Shared/DefaultMaster.master" Codebehind="ManageUploadDocuments.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagName="UploadDocuments" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/UploadDocuments.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <style type="text/css">
        .abc .rgEditRow
        {
            background-color: #CFCFCF;
            padding: 10px;
        }
        .abc .rgEditRow td
        {
            padding: 10px;
        }
    </style>
    <div style="padding: 10px; padding-left: 50px; background-color: #efefef;">
       <infsu:UploadDocuments ID="ucUploadDocuments" runat="server">
        </infsu:UploadDocuments>
    </div>
    <br />
    <div class="section">
        <h1 class="mhdr">
            Map Documents</h1>
        <div class="content">
            <div class="swrap abc">
                <infs:WclGrid runat="server" ID="grdMapping" AllowPaging="True" AutoGenerateColumns="False"
                    AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                    EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="False"
                    OnNeedDataSource="grdMapping_NeedDataSource" OnDeleteCommand="grdMapping_DeleteCommand"
                     OnUpdateCommand="grdMapping_UpdateCommand">
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="ApplicantDocumentID">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false" ShowExportToExcelButton="false"
                        ShowExportToPdfButton ="false" ShowRefreshButton="false"/>
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="ApplicantUploadedDocumentID" FilterControlAltText="Filter ApplicantUploadedDocumentID column"
                                HeaderText="ID" SortExpression="ApplicantUploadedDocumentID" UniqueName="ApplicantUploadedDocumentID"
                                Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FileName" FilterControlAltText="Filter FileName column"
                                HeaderText="File Name" SortExpression="FileName" UniqueName="FileName" ReadOnly="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Size" FilterControlAltText="Filter Size column"
                                HeaderText="Size" SortExpression="Size" UniqueName="Size" ReadOnly="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Description">
                                <ItemTemplate>
                                    <%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Description")))%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="padding: 10px;">
                                        <infs:WclTextBox runat="server" ID="txtDescription" Width="100%" Text='<%#Eval("Description")%>'>
                                        </infs:WclTextBox>
                                    </div>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--<telerik:GridTemplateColumn HeaderText="Mapping">
                                <ItemTemplate>
                                   <%-- <%#Eval("MappedDocumentDetails")%>
                                </ItemTemplate>--%>
                                <%--<EditItemTemplate>
                                    <div style="padding: 10px;">
                                        <telerik:RadDropDownTree ID="RadDropDownTree2" runat="server" Width="250px" CheckBoxes="CheckChildNodes"
                                            DefaultMessage="Please select" DataFieldID="ItemID" DataFieldParentID="ParentID"
                                            DataTextField="Name" DataSourceID="xdtsCategories">
                                        </telerik:RadDropDownTree>
                                    </div>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                                <HeaderStyle CssClass="tplcohdr" />
                                <ItemStyle CssClass="MyImageButton" />
                            </telerik:GridEditCommandColumn>                            
                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this record?"
                                Text="Delete" UniqueName="DeleteColumn">
                                <HeaderStyle CssClass="tplcohdr" />
                                <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                            </telerik:GridButtonColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </infs:WclGrid>
            </div>
            <div class="gclr">
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="server">
    <%--<infsu:CommandBar ID="fsucCmdBar1" runat="server" DefaultPanel="pnlName1" DisplayButtons="Save,Cancel" />--%>
</asp:Content>
