<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Search.Views.ApplicantPortfolioSubscription" CodeBehind="ApplicantPortfolioSubscription.ascx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style type="text/css">
    div.RadToolTip_Default table.rtWrapper td.rtWrapperContent {
        background: -webkit-linear-gradient(white, #E4E5F0); /* For Safari 5.1 to 6.0 */
        background: -o-linear-gradient(white, #E4E5F0); /* For Opera 11.1 to 12.0 */
        background: -moz-linear-gradient(white, #E4E5F0); /* For Firefox 3.6 to 15 */
        background: linear-gradient(white, #E4E5F0); /* Standard syntax (must be last) */
        padding-left: 2.5px;
        padding-right: 2.5px;
        padding-top: 2.5px;
        padding-bottom: 2.5px;
        border: 2px solid #8D8D8D;
        color: black;
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='white', endColorstr='#E4E5F0', GradientType=0); /*For IE7-8-9*/
    }

    @-moz-document url-prefix() {
        div .RadToolTip_Default table.rtWrapper td.rtWrapperContent;

    {
        border: 1px solid #8D8D8D;
    }

    }

    @media screen and (-webkit-min-device-pixel-ratio:0) {
        div.RadToolTip_Default table.rtWrapper td.rtWrapperContent {
            border: 1px solid #8D8D8D;
        }
    }

    div.rtCallout.rtCalloutTopCenter {
        visibility: hidden !important;
    }

    .RadToolTip_Default .rtWrapper .rtWrapperLeftMiddle, .RadToolTip_Default .rtWrapper .rtWrapperRightMiddle, .RadToolTip_Default table.rtShadow .rtWrapperLeftMiddle, .RadToolTip_Default table.rtShadow .rtWrapperRightMiddle {
        background-image: none !important;
    }

    .RadToolTip_Default .rtWrapper .rtWrapperTopLeft, .RadToolTip_Default .rtWrapper .rtWrapperTopRight, .RadToolTip_Default .rtWrapper .rtWrapperBottomLeft, .RadToolTip_Default .rtWrapper .rtWrapperBottomRight, .RadToolTip_Default .rtWrapper .rtWrapperTopCenter, .RadToolTip_Default .rtWrapper .rtWrapperBottomCenter, .RadToolTip_Default table.rtShadow .rtWrapperTopLeft, .RadToolTip_Default table.rtShadow .rtWrapperTopRight, .RadToolTip_Default table.rtShadow .rtWrapperBottomLeft, .RadToolTip_Default table.rtShadow .rtWrapperBottomRight, .RadToolTip_Default table.rtShadow .rtWrapperTopCenter, .RadToolTip_Default table.rtShadow .rtWrapperBottomCenter, .RadToolTip_Default .rtCloseButton {
        background-image: none !important;
    }
</style>


<script type="text/javascript">
    $jQuery(document).ready(function () {
        //debugger;
        $jQuery("[id$=treeListDetail]").find("th").each(function (element) {
           // debugger;
            if ($jQuery(this).text() != "" && $jQuery(this).text() != undefined && $jQuery(this).text().length > 1) {
                $jQuery(this).attr("tabindex", "0");
            }
        });
    });

</script>
<div class="row">
    <div class='col-md-12'>
        <h2 class="header-color" title="Details pertaining to the student's subscription are displayed in this section" tabindex="0">Subscription Details
        </h2>
    </div>
</div>
<%-- <div>
        <input type="text" id="txtTest" style="box-shadow: 10px 10px 5px #888888; width:200px;" />
        </div>
    <div>fsdfkj</div>--%>
<div class="row">
    <infs:WclTreeList ID="treeListDetail" runat="server" DataTextField="Value" TabIndex="0"
        ParentDataKeyNames="ParentNodeID,ParentDataID,ParentNodeCode" DataKeyNames="NodeID,DataID,NodeCode"
        OnNeedDataSource="treeListDetail_NeedDataSource" DataMember="Assigned" EnableAriaSupport="true"
        OnItemCreated="treeListDetail_ItemCreated" AutoGenerateColumns="false"
        DataValueField="UICode" OnPreRender="treeListDetail_PreRender" OnItemCommand="treeListDetail_ItemCommand" ClientIDMode="Static">
        <Columns>
            <telerik:TreeListBoundColumn DataField="Value" UniqueName="Value" HeaderText="Name" HeaderTooltip="The Package or Category name is displayed in each row" />
            <telerik:TreeListBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description" HeaderTooltip="The description, if any, for each row is displayed in this column" />
            <telerik:TreeListBoundColumn DataField="ComplianceStatus" UniqueName="ComplianceStatus" HeaderText="Compliance Status" HeaderTooltip="For Package-level rows, this column displays the student's overall compliance status. For Category-level rows, this column displays the verification status for that category" />
            <telerik:TreeListBoundColumn DataField="SubscriptionStatus" UniqueName="SubscriptionStatus" HeaderText="Subscription Status" HeaderTooltip="The number of days remaining in the student's subscription" />
            <telerik:TreeListBoundColumn DataField="InstituteHierarchy" UniqueName="InstituteHierarchy" HeaderText="Institution Hierarchy" HeaderTooltip="Click the link and select a note to restrict search results to the selected node" />
            <telerik:TreeListButtonColumn ButtonType="LinkButton" Text="Detail" CommandName="ViewDetail" UniqueName="ViewDetail"></telerik:TreeListButtonColumn>
            <telerik:TreeListBoundColumn DataField="OrganizationUserID" UniqueName="OrganizationUserID" HeaderText="UserID" Display="false" />
            <telerik:TreeListBoundColumn DataField="PackageSubscriptionID" UniqueName="PackageSubscriptionID" HeaderText="SubscriptionID" Display="false" />
            <%-- <asp:HiddenField ID="hdfOrgUserId" runat="server" Value='<%# Eval("OrganizationUserID") %>' />
                <asp:HiddenField ID="hdfPackSubscriptionId" runat="server" Value='<%# Eval("PackageSubscriptionID") %>' />--%>
        </Columns>
    </infs:WclTreeList>
    <infs:WclToolTipManager ID="RadToolTipManager1" runat="server" RelativeTo="Element" ToolTipZoneID="treeListDetail" TabIndex="0"
        AutoTooltipify="true" ContentScrolling="Default" Position="BottomCenter" AutoCloseDelay="20000" />
    <%-- <telerik:RadToolTip ID="RadToolTip1" runat="server" TargetControlID="treeListDetail" IsClientID="true">
        </telerik:RadToolTip>--%>
</div>
<br />


