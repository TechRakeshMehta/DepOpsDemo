
function pageLoad(sender) {
(msgupd = function ($) {
        $(".error").each(function () {
            if ($(this).text().replace(/\s/g, "") != "") $(this).parent(".msgbox:last").fadeIn();
        });
        $(".info").each(function () {
            if ($(this).text().replace(/\s/g, "") != "") $(this).parent(".msgbox:last").fadeIn();
        });
        $(".sucs").each(function () {
            if ($(this).text().replace(/\s/g, "") != "") $(this).parent(".msgbox:last").fadeIn();
        });
    })(FSObject.$);
}

function FocusMenu(menu, eventArgs) {
    localStorage.setItem("filterinput", eventArgs._targetElement.id)
    menu.get_items().getItem(1).get_linkElement().focus();
}