<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadCategoryDocument.aspx.cs" Inherits="CoreWeb.LoadCategoryDocument" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .changeHeight {
            height: 100vh;
            width: 100%;
        }

        @media (min-width: 1200px) and (max-width: 1400px) {

            .changeHeight {
                height: 100vh;
                width: 100%;
            }
        }

        @media (min-width: 1401px) and (max-width:1500px) {

            .changeHeight {
                height: 100vh;
                width: 100%;
            }
        }

        @media (min-width: 1501px) and (max-width:1600px) {

            .changeHeight {
                height: 100vh;
                width: 100%;
            }
        }

        @media (min-width: 1601px) and (max-width:1800px) {

            .changeHeight {
                height: 100vh;
                width: 100%;
            }
        }

        @media (min-width: 1801px) and (max-width:2000px ) {

            .changeHeight {
                height: 100vh;
                width: 100%;
            }
        }

        @media (min-width: 2001px) and (max-width:2600px ) {

            .changeHeight {
                height: 100vh;
                width: 100%;
            }
        }

        @media (min-width: 2601px) and (max-width:3000px ) {

            .changeHeight {
                width: 100%;
                height: 100vh;
            }
        }

        @media (min-width: 3001px) and (max-width:4000px ) {

            .changeHeight {
                height: 100vh;
                width: 100%;
            }
        }

        @media (min-width: 4001px) and (max-width:10000px ) {

            .changeHeight {
                height: 100vh;
                width: 100%;
            }
        }
    </style>
</head>
<body>
    <form id="frmDocument" runat="server">
        <iframe id="iframeCtgryDocumentViewer" runat="server" class="changeHeight"></iframe>
    </form>
</body>
</html>
