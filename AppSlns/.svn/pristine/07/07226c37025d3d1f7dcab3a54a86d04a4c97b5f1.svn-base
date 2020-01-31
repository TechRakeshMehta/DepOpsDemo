

function ValidateBKGPackageDetailLength(sender, args) {
    var maxTextLength = 1000;
    var editor = $jQuery("[id$=rdEditorPackageDetail]")[0];
    text = editor.control.get_text();
    text = text.replace(/(?:\\[rn]|[\r\n]+)+/g, "");
    var textLength = text.length;
    if (text != "" && textLength > maxTextLength)
        return args.IsValid = false;
    else
        return args.IsValid = true;
}

function OnClientLoad(editor, args) {
    $jQuery('ul.reToolbar').width('auto');

    $jQuery('.reEditorModes a').css("display", "none");
    $jQuery('.reToolZone').css("display", "none");
}

function SetFocus(sender, args) {
    sender.focus();
}