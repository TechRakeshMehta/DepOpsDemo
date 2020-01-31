<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LocationImages.aspx.cs" Inherits="CoreWeb.LocationImages" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <%--<link rel="stylesheet" href="Resources/Mod/Dashboard/Styles/bootstrap.min.css" />--%>
    <%--<script src="Resources/Mod/Dashboard/Scripts/Kendo/jquery.min.js"></script>--%>
    <%--<script src="Resources/Mod/Dashboard/Scripts/bootstrap.min.js"></script>--%>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
</head>
<body>
    <style>
        .noImgMsg {
            text-align: center;
            margin: 100px 100px 100px 100px;
            font-size: 22px;
            font-weight: 500;
            font-family: initial;
            color: red;
        }

        .btnUpdate {
            color: #fff !important;
            background-color: #8C1921;
            display: inline-block;
            padding: 6px 12px;
            margin-bottom: 0;
            font-size: 14px;
            font-weight: 400;
            line-height: 1.42857143;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            -ms-touch-action: manipulation;
            cursor: pointer;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            background-image: none;
            border: 1px solid transparent;
            border-radius: 4px;
            margin: 1px;
            text-decoration: none !important;
        }

        #divImgLoadr {
            position: absolute;
            top: 0px;
            right: 0px;
            width: 100%;
            height: 100%;
            background-color: #666;
            background-image: url('Resources/Mod/Dashboard/Styles/Default/loading-image.gif');
            background-repeat: no-repeat;
            background-position: center;
            z-index: 10000000;
            opacity: 0.4;
            filter: alpha(opacity=40); /* For IE8 and earlier */
        }

        .lblLocNameHeader {
            font-size: x-large;
            font-weight: bold;
        }

        #lblLocationNameTxt {
            font-size: x-large;
        }
    </style>
    <script type="text/javascript">
        debugger
        // $('#imgLoadr').hide();
        var slideIndex = 1;
        $(document).ready(function () {
            $('#divImgLoadr').show();
            setTimeout(function () {
                debugger
                $('#divImgLoadr').hide();
            }, 6000);
            showDivs(slideIndex);


        })

        // showDivs(slideIndex);

        function plusDivs(n) {
            showDivs(slideIndex += n);
        }
        function selectImage(n) {
            slideIndex = n;
            showDivs(slideIndex);
        }
        function showDivs(n) {

            var i;
            var x = document.getElementsByClassName("mySlides");
            if (x.length > 1) {
                $('#leftArrow')[0].hidden = false;
                $('#rightArrow')[0].hidden = false;
            }
            else {
                $('#leftArrow')[0].hidden = true;
                $('#rightArrow')[0].hidden = true;
            }
            var indicators = document.getElementsByClassName("mySlidesIndicators");
            if (n > x.length) { slideIndex = 1 }
            if (n < 1) { slideIndex = x.length }
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
                indicators[i].classList.remove("active");
            }

            indicators[slideIndex - 1].classList.add("active");
            x[slideIndex - 1].style.display = "block";
        }

    </script>
    <div class="col-md-12" id="divImgLoadr">
        <span>Loading...</span>
        <%--<img src="Resources/Mod/Dashboard/Styles/Default/loading-image.gif" />--%>
    </div>
    <div class="row">
        <div class="container col-md-12" style="display: flex; justify-content: center; max-height: 1000px;">
            <%--<h2>Carousel Example</h2>--%>
            <div id="divLocationImages" runat="server" class="carousel slide" data-ride="carousel">
                <!-- Indicators -->
                <ol class="carousel-indicators" style="bottom: 7%">
                    <asp:Repeater ID="rptrImgIndicator" runat="server">
                        <ItemTemplate>
                            <li id="imgIndicator" data-target="#divLocationImages" onclick="selectImage(<%#Container.ItemIndex + 1%>)" class="active mySlidesIndicators"></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ol>

                <!-- Wrapper for slides -->
                <div class="carousel-inner">
                    <asp:Repeater ID="rptrImgView" runat="server">
                        <ItemTemplate>
                            <div class="mySlides item active">
                                <asp:Image ID="imgLocation" AlternateText='<%#Eval("FPLI_FileName") %>' runat="server" onerror="this.src='Resources/Mod/Dashboard/images/errPreviewImage.png'" ImageUrl='<%# String.Format("~/ComplianceOperations/UserControl/DocumentViewer.aspx?DocumentType=LocationImageFile&LocationImageFilePath={0}&LocationImageFileName={1}",Eval("FPLI_FilePath"),Eval("FPLI_FileName")) %>' Width="100%" />

                            </div>
                            <%--<asp:Image runat="server" Width="100%" CssClass="mySlides centerImage" AlternateText='<%#Eval("FPLI_FileName") %>' ImageUrl='<%# String.Format("~/ComplianceOperations/UserControl/DocumentViewer.aspx?DocumentType=LocationImageFile&LocationImageFilePath={0}&LocationImageFileName{1}",Eval("FPLI_FilePath"),Eval("FPLI_FileName")) %>' />--%>
                        </ItemTemplate>
                    </asp:Repeater>


                    <%--<div class="item">
                    <img src="images/adb/building_a.jpg" alt="Chicago" style="width: 100%; height: 60%;" />
                </div>

                <div class="item">
                    <img src="images/adb/business_1.jpg" alt="New york" style="width: 100%; height: 60%;" />
                </div>--%>
                </div>

                <!-- Left and right controls -->
                <a class="left carousel-control" id="leftArrow" onclick="plusDivs(-1);return false;"><%--data-slide="prev">--%>
                    <span class="glyphicon glyphicon-chevron-left"></span>
                    <span class="sr-only">Previous</span>
                </a>
                <a class="right carousel-control" id="rightArrow" onclick="plusDivs(+1);return false;"><%-- href="#divLocationImages" data-slide="next">--%>
                    <span class="glyphicon glyphicon-chevron-right"></span>
                    <span class="sr-only">Next</span>
                </a>
            </div>
            <div id="divNoImg" class="noImgMsg" runat="server">
                <asp:Label ID="lblNoImageAvailable" runat="server"><%=Resources.Language.IMGSNOTAVAIL%></asp:Label>
            </div>
        </div>
        <div class="col-md-12" style="text-align: center; margin-top: 1%;">
            <asp:Label ID="lblLocationName" CssClass="lblLocNameHeader" runat="server"><%=Resources.Language.LOCATIONNAME%>:</asp:Label>
            <asp:Label ID="lblLocationNameTxt" runat="server"></asp:Label>
        </div>

    </div>
</body>
</html>
