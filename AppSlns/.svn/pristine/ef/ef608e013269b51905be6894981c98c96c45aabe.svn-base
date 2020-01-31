$jQuery(document).ready(function () {
    var folderCode = null;
    var fromUserId = null;
    var IsApplicantAllowToSendMessages = null;
    if (parent.pageFrame == undefined) {
        var folderCode = 'MSGINB';
        var fromUserId = fromID;
    }
    else {
        var folderCode = parent.pageFrame.window.folderCode;
        var fromUserId = parent.pageFrame.window.sendFromUserId;
        var IsApplicantAllowToSendMessages = parent.pageFrame.window.hdnIsApplicantAllowToSendMessages
    }
    if ((folderCode == "MSGSNT") && $jQuery("[id$=WclToolBar1]")[0] != undefined) {
        var toolbar = $find($jQuery("[id$=WclToolBar1]")[0].id);
        var replyButton = toolbar.findItemByText("Reply");
        var replyAllButton = toolbar.findItemByText("Reply All");

        if (replyButton != undefined) {
            replyButton.set_enabled(false);
        }

        if (replyAllButton != undefined) {
            replyAllButton.set_enabled(false);
        }
    }
    else if (folderCode == undefined) {
        if ($find($jQuery("[id$=WclToolBar1]")) != null) {
            var toolbar = $find($jQuery("[id$=WclToolBar1]")[0].id);
            var replyButton = toolbar.findItemByText("Reply");
            var replyAllButton = toolbar.findItemByText("Reply All");
            var forwardButton = toolbar.findItemByText("Forward");
            var deleteButton = toolbar.findItemByText("Delete");
            var attachmentButton = toolbar.findItemByText("Attachment(s)");
            if (replyButton != undefined) {
                replyButton.set_enabled(false);
            }

            if (replyAllButton != undefined) {
                replyAllButton.set_enabled(false);
            }
            if (forwardButton != undefined) {
                forwardButton.set_enabled(false);
            }

            if (deleteButton != undefined) {
                deleteButton.set_enabled(false);
            }
            if (attachmentButton != undefined) {
                attachmentButton.set_enabled(false);
            }
        }
    }
    if (IsApplicantAllowToSendMessages != null && IsApplicantAllowToSendMessages != "" && IsApplicantAllowToSendMessages == "false") {
        if ($jQuery("[id$=WclToolBar1]") != null) {
            var toolbar = $find($jQuery("[id$=WclToolBar1]")[0].id);
            var replyButton = toolbar.findItemByText("Reply");
            var replyAllButton = toolbar.findItemByText("Reply All");
            var forwardButton = toolbar.findItemByText("Forward");

            if (replyButton != undefined) {
                replyButton.set_enabled(false);
            }

            if (replyAllButton != undefined) {
                replyAllButton.set_enabled(false);
            }
            if (forwardButton != undefined) {
                forwardButton.set_enabled(false);
            } 
        }
    }
});




var btntoolbar_clicked = function (sender, args) {
    var button = args.get_item();
    top.button = button;
    if (button == null) return;

    var composeScreenWindowName = "composeScreen";
    var messageId = $jQuery("[id*=hdnMessageId]")[0].value;
    var command = button.get_commandName().toLowerCase();
    var current_window = $page.get_window();
    switch (command) {

        case "reply":
            parent.pageFrame.window.OpenWriteMessageWindow(messageId, queueType, currentUserID, 0, communicationType, 2, composeScreenWindowName, current_window);
            break;
        case "replyall":
            parent.pageFrame.window.OpenWriteMessageWindow(messageId, queueType, currentUserID, 0, communicationType, 3, composeScreenWindowName, current_window);
            break;
        case "forward":
            if (messageId <= 0) break;
            parent.pageFrame.window.OpenWriteMessageWindow(messageId, queueType, currentUserID, 0, communicationType, 4, composeScreenWindowName, current_window);
            break;
        case "delete":
            if (messageId <= 0) break;
            /*Bug fix - Confirm box not at the top*/
            var win = $page.get_window();
            var timer = new IWeb.Timer(500);
            timer.add_tick(function () {
                if (win && win.isMaximized()) {
                    win.restore();
                }
                $confirm("Are you sure you want to delete the selected messages(s). " + " <br/><span style='font-weight:bolder'>Do you want to continue?</span><br/>&nbsp;", function (cr) {
                    if (cr) {
                        parent.pageFrame.window.DeleteMessage(1);
                        $page.get_window().set_destroyOnClose(true);
                        $page.get_window().close();
                    }
                }, 'Confirm Delete', true);
            });
            timer.startOnce();
            break;
        case 'print':
            {
                var pageContent = GetRadWindow().GetContentFrame().contentWindow;
                var printDocument = pageContent.document;
                printDocument.execCommand("Print");
            }
        default:
    }
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}