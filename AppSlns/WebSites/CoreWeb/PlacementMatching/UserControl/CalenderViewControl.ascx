<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalenderViewControl.ascx.cs" Inherits="CoreWeb.PlacementMatching.Views.CalenderViewControl" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<style>
    .space input[type="checkbox"] + label {
        padding-right: 0px !important;
    }

    /*.rsAptDelete {
    display: none;
}*/
    /*.rsContentTable
   {
    display:none !important;
  }
.rsWrap,.rsLastSpacingWrapper
  {
    height:200px !important;
  }*/
    .togglebutton {
        width: 50%;
        height: 100%;
        font-size: 16px;
        color: white;
        float: left;
        text-align: center;
    }

    #box_content p {
        margin: 0px;
    }

    rsWrap rsLastSpacingWrapper {
        height: 250px;
    }

    .container-fluid h1 {
        font-size: large !important;
        color: #8C1921;
        margin: 0px;
        padding-top: 10px;
        font-weight: 600;
    }

    .RadScheduler .rsMonthView .rsAptContent {
        min-height: 15px;
    }

    #box_content i, #box_content em {
        font-style: normal;
    }
</style>

<infs:WclResourceManagerProxy runat="server" ID="rprxCalenderView">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="col-md-1">
                    <span style="font-size: 13px; text-align: center">SHOW:</span>
                </div>
                <div class="col-md-3">
                    <infs:WclCheckBox runat="server" CssClass="space" ID="chkAgencyHierachy" Text="Agency Hierachy" OnCheckedChanged="chkAgencyHierachy_CheckedChanged" AutoPostBack="true" Font-Size="13px" />
                </div>
                <div class="col-md-2">
                    <infs:WclCheckBox runat="server" CssClass="space" ID="chkShift" Text="Shift" OnCheckedChanged="chkShift_CheckedChanged" AutoPostBack="true" Font-Size="13px" />
                </div>
                <div class="col-md-2">
                    <infs:WclCheckBox runat="server" CssClass="space" ID="chkProgram" Text="Program" OnCheckedChanged="chkProgram_CheckedChanged" AutoPostBack="true" Font-Size="13px" />
                </div>
                <div class="col-md-2">
                    <infs:WclCheckBox runat="server" CssClass="space" ID="chkCourse" Text="Course" OnCheckedChanged="chkCourse_CheckedChanged" AutoPostBack="true" Font-Size="13px" />
                </div>
            </div>
            <div class="col-md-1"></div>
            <div class="col-md-3" style="margin-left: 95px;">
                <%-- <asp:Button ID="btnGrid" CssClass="togglebutton" runat="server" Text="Grid" Enabled="true" BackColor="#8C1921" OnClick="btnGrid_Click" />
                <asp:Button ID="btnCalender" CssClass="togglebutton" runat="server" Text="Calender" Enabled="false" BackColor="Gray" />--%>
                <asp:LinkButton ID="btnGrid" CssClass="togglebutton" runat="server" Enabled="true" BackColor="#8C1921" OnClick="btnGrid_Click" Font-Underline="false">
                   <i class="fa fa-th"></i> Grid
                </asp:LinkButton>
                <asp:LinkButton ID="btnCalender" CssClass="togglebutton" runat="server" Enabled="false" BackColor="Gray" Font-Underline="false">
                  <i class="fa fa-calendar"></i> Calender
                </asp:LinkButton>
            </div>
        </div>
    </div>
    <div class="row col-md-12">
        <div class="panel panel-default bgCalander">
            <div class="panel-body" style="padding-left: 0px; padding-top: 0px; padding-bottom: 0px;">
                <div id="rotationWidget">
                    <asp:UpdatePanel ID="upl" UpdateMode="Always" runat="server">
                        <ContentTemplate>
                            <div id="divRadScheduler" class="col-md-7">
                                <div class="row">
                                    <div class="demo-container no-bg">
                                        <infs:WclScheduler ShowAllDayRow="false" Skin="Windows7" runat="server" SelectedView="MonthView" AppointmentStyleMode="Auto"
                                            OnAppointmentDataBound="calenderPlacement_AppointmentDataBound" OnAppointmentClick="calenderPlacement_AppointmentClick"
                                            AllowEdit="true" ReadOnly="true" EnableAdvancedForm="false" TimeSlotContextMenuSettings-EnableDefault="false"
                                            ID="calenderPlacement" TimelineView-UserSelectable="false">
                                            <AppointmentTemplate>
                                                <%# Eval("Subject") %>
                                            </AppointmentTemplate>
                                        </infs:WclScheduler>
                                    </div>
                                </div>
                            </div>
                            <div id="divLstView" class="col-md-5" runat="server">
                                <div class="row col-md-12">
                                    <h1>Placement Details</h1>
                                </div>

                                <hr />
                                <div class="col-md-4">
                                    <span class="cptn" style="font-size: 14px">Institution</span>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="lblInstitution" runat="server" Text=""> </asp:Label>
                                </div>
                                <div class="row"></div>
                                <div class="col-md-4">
                                    <span class="cptn" style="font-size: 14px">Location</span>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="lblLocation" runat="server" Text=""> </asp:Label>
                                </div>
                                <div class="row"></div>
                                <div class="col-md-4">
                                    <span class="cptn" style="font-size: 14px">Unit</span>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="lblUnit" runat="server" Text=""> </asp:Label>
                                </div>
                                <div class="row"></div>
                                <div class="col-md-4">
                                    <span class="cptn" style="font-size: 14px">Department</span>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="lblDepartment" runat="server" Text=""> </asp:Label>
                                </div>
                                <div class="row"></div>
                                <div class="col-md-4">
                                    <span class="cptn" style="font-size: 14px">Specialty</span>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="lblSpecialty" runat="server" Text=""> </asp:Label>
                                </div>
                                <div class="row"></div>
                                <div class="col-md-4">
                                    <span class="cptn" style="font-size: 14px">Student Type</span>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="lblStudentType" runat="server" Text=""> </asp:Label>
                                </div>
                                <div class="row"></div>
                                <div class="col-md-4">
                                    <span class="cptn" style="font-size: 14px">Max #</span>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="lblMax" runat="server" Text=""> </asp:Label>
                                </div>
                                <div class="row"></div>
                                <div class="col-md-4">
                                    <span class="cptn" style="font-size: 14px">Dates Available</span>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="lblDates" runat="server" Text=""> </asp:Label>
                                </div>
                                <div class="row"></div>
                                <div class="col-md-4">
                                    <span class="cptn" style="font-size: 14px">Days</span>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="lblDays" runat="server" Text=""> </asp:Label>
                                </div>
                                <div class="row"></div>
                                <div class="col-md-4">
                                    <span class="cptn" style="font-size: 14px">Shift</span>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="lblShift" runat="server" Text=""> </asp:Label>
                                </div>
                                <div class="row"></div>
                                <div class="col-md-4">
                                    <span class="cptn" style="font-size: 14px">Is Preceptonship</span>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="lblIsPreceptonship" runat="server" Text=""> </asp:Label>
                                </div>
                                <div class="row"></div>
                                <div class="col-md-4">
                                    <span class="cptn" style="font-size: 14px">Contains Float Area</span>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="lblFloatArea" runat="server" Text=""> </asp:Label>
                                </div>
                                <%--<div class="row"></div>--%>
                                <%--<div class="col-md-4">
                                    <span class="cptn" style="font-size: 14px">Unit</span>
                                </div>--%>
                                <%--  <div class="col-md-8">
                                    <asp:Label ID="lblUnit1" runat="server" Text=""> </asp:Label>
                                </div>--%>
                                <%--<div class="row"></div>--%>
                                <%--<div class="col-md-4">
                                    <span class="cptn" style="font-size: 14px">Max #</span>
                                </div>--%>
                                <%--<div class="col-md-8">
                                    <asp:Label ID="lblMax1" runat="server" Text=""> </asp:Label>
                                </div>--%>
                                <div class="row">&nbsp;</div>
                                <div class="row">&nbsp;</div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-5"></div>
                                        <div class="form-group col-md-1">
                                            <infs:WclButton Icon-PrimaryIconCssClass="rbEdit" runat="server" AutoPostBack="true" Skin="Silk" OnClick="btnEdit_Click" AutoSkinMode="false" ID="btnEdit" Text="Edit" Visible="true">
                                            </infs:WclButton>
                                            <asp:HiddenField runat="server" ID="hdnRequestID" />
                                            <asp:HiddenField runat="server" ID="hdnOpportunityID" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>
    </div>
    <div class="col-md-6"></div>

</div>
<script>


    function GetAppointmnet() {
        alert("fdgfd");
    }

</script>
