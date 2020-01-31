
function ValidateLength(sender, args) {
    var maxContentLength = 500;
    var editor = $jQuery("[id$=rdEditorDescription]")[0];
    text = editor.control.get_text();
    text = text.replace(/(?:\\[rn]|[\r\n]+)+/g, "");
    var textLength = text.length;
    if (text != "" && textLength > maxContentLength)
        return args.IsValid = false;
    else
        return args.IsValid = true;    
}

function ValidatePackageDetailLength(sender, args) {
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

function ValidateDetailsLength(sender, args) {
    var maxContentLength = 500;
    var editor = $jQuery("[id$=rdEditorDetails]")[0];
    text = editor.control.get_text();
    text = text.replace(/(?:\\[rn]|[\r\n]+)+/g, "");
    var textLength = text.length;
    if (text != "" && textLength > maxContentLength)
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

function ValidatePackageBundleDetailLength(sender, args) { 
    var maxTextLength = 500;
    var editor = $jQuery("[id$=rdEditorNotes]")[0];
    text = editor.control.get_text();
    text = text.replace(/(?:\\[rn]|[\r\n]+)+/g, "");
    var textLength = text.length;
    if (text != "" && textLength > maxTextLength)
        return args.IsValid = false;
    else
        return args.IsValid = true;
}

//Commented below code for future use: Sachin Singh[SS]
//var maxTextLength = 400;
//var maxTextLengthNotes = 2048;

//function isAlphaNumericKey(keyCode) {
//    if ((keyCode > 47 && keyCode < 58) || (keyCode > 64 && keyCode < 91)) {
//        return true;
//    }
//    return false;
//}

//function LimitCharacters(editor) {
//    $jQuery('ul.reToolbar').width('auto');
//    editor.attachEventHandler("keydown", function (e) {
//        e = (e == null) ? window.event : e;
//        if (isAlphaNumericKey(e.keyCode)) {
//            text = editor.get_text();
//            text = text.replace(/(?:\\[rn]|[\r\n]+)+/g, "");
//            textLength = text.length;
//            if (textLength >= maxTextLength) {
//                e.returnValue = false;
//                return false;
//            }
//        }
//    });
//}

//function CalculateLength(editor) {
//    text = editor.get_text();
//    text = text.replace(/(?:\\[rn]|[\r\n]+)+/g, "");
//    var textLength = text.length - SelectedTextLength;
//    var clipboardText = window.clipboardData.getData("Text")
//    clipboardText = clipboardText.replace(/(?:\\[rn]|[\r\n]+)+/g, "");
//    var clipboardLength = clipboardText.length;
//    textLength += clipboardLength;
//    return textLength;
//}

//function OnClientPasteHtml(editor, args) {
//    debugger;
//    var commandName = args.get_commandName();
//    var value = args.get_value();
//    if (commandName == "PasteFromWord"
//        || commandName == "PasteFromWordNoFontsNoSizes"
//        || commandName == "PastePlainText"
//        || commandName == "PasteAsHtml"
//        || commandName == "Paste") {
//        var textLength = CalculateLength(editor, value);
//        if (textLength >= maxTextLength) {
//            args.set_cancel(true);

//        }
//    }
//}

//window.setTimeout(function () {
//    var editorIframe = $get(editor.get_id() + "Wrapper").getElementsByTagName("iframe")[0];
//    alert(editorIframe);
//    editorIframe.style.height = "200px";
//    editor.setSize(650, 200);
//}, 100);



