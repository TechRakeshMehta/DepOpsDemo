///<reference path="/Resources/core/Ref.js" />

//Global Methods

var messaging = {
    defaultPopup: {
        size: "800, 600",
        behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize | Telerik.Web.UI.WindowBehaviors.Reload
    }
}

var btntoolbar_clicked = function (a, args) {
    var button = args.get_item();
    top.button = button;

    if (button == null) return;

    var command = button.get_commandName().toLowerCase();

    switch (command) {
        case "newmessage":
            var url = $page.url.create("Pages/Compose.aspx");

            var win = $window.createPopup(url, messaging.defaultPopup);
            win.add_command(function (sender, args) {
                if (typeof (sender.get_contentFrame().contentWindow.e_onWindowCommand) == "function") {
                    sender.get_contentFrame().contentWindow.e_onWindowCommand(sender, args);
                }
            });
        default:

    }

}


var fn_adjustHeight = function (container) {

    container = container || "body";
    var no_adjust = ".no-adjust";
    var adjust = ".adjust";

    $jQuery(container).find(".adjust-height").each(function () {

        var cheight = $jQuery(this).height();
        var nheight = 0;
        var alength = $jQuery(this).children(no_adjust).length;

        $jQuery(this).children(no_adjust).each(function () {
            nheight = nheight + $jQuery(this).height();
        });

        nheight = (cheight - nheight) / alength;

        $jQuery(this).children(adjust).each(function () {
            $jQuery(this).height(nheight);
        });
    });
}

$jQuery(document).ready(function () {
    fn_adjustHeight();
});


var e_resize = function (a) {
    fn_adjustHeight(a.get_element());
}