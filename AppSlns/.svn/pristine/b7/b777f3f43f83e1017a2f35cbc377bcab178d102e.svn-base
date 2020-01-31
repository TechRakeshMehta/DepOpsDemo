$page.add_pageInitComplete(function () {
    var treeView = $find(FSObject.$("[id$=treePersonalFolders]")[0].id);
    var moveToFolderID = $jQuery("#hdnMoveTofolderID");
    moveToFolderID.val("1#MSGPNF");
    var node = treeView.findNodeByValue("1#MSGPNF");
    node.select();
});

function e_nodeclicked(sender, args) {
    var moveToFolderID = $jQuery("#hdnMoveTofolderID");

    if (args.get_node().get_value().split('#')[1] == "MSGJNK" || args.get_node().get_value().split('#')[1] == "MSGINB") {
        $find($jQuery("[id$=txtFolderName]")[0].id).disable(true);
    }
    else {
        $find($jQuery("[id$=txtFolderName]")[0].id).enable(true);
    }
    if (args.get_node().get_enabled()) {
        $jQuery("#foldername").text(args.get_node().get_text());
        moveToFolderID.val(args.get_node().get_value());
    }

}
function cancelClick(sender, args) {
    returnToParent();
}
var FolderMovementVars = [];
function saveClick(sender, args) {
    var sourceFolderCode = parent.pageFrame.window.folderCode;
    var moveToFolderID = $jQuery("#hdnMoveTofolderID");
    if (moveToFolderID.val().split('#')[1] == sourceFolderCode) {
        $alert("Destination folder cannot be same as the Source folder.");
        return false;
    }

    var parentFolderID = $jQuery("#hdnParentID");
    FolderMovementVars.x = $jQuery("#foldername").text();
    FolderMovementVars.tableView = parent.pageFrame.window.tableviewMessageMove;
    FolderMovementVars.folderId = parent.pageFrame.window.folderId;
    FolderMovementVars.folderCode = parent.pageFrame.window.folderCode;
    txtbox = $find($jQuery("[id$=txtFolderName]")[0].id);
    FolderMovementVars.moveToFolderCode = moveToFolderID.val().split('#')[1];
    FolderMovementVars.newMoveToFolderID = moveToFolderID.val().split('#')[0];
    FolderMovementVars.userGroup = 0;
    if (txtbox._text != "") {
        parentFolderID.val(moveToFolderID.val());
        moveToFolderID.val("");
        AddFolder();
    }
    else {
        MoveToFolderAndUpdateResult(FolderMovementVars.newMoveToFolderID, FolderMovementVars.moveToFolderCode);

    }


}
function MoveToFolderAndUpdateResult(newMoveToFolderID, moveToFolderCode) {
    var from = parent.pageFrame.window.from;
    var to = parent.pageFrame.window.to;
    var subject = parent.pageFrame.window.subject;
    var messageBody = parent.pageFrame.window.body;
    for (var i = 0; i < FolderMovementVars.tableView.get_selectedItems().length; i++) {
        var cellID = FolderMovementVars.tableView.get_selectedItems()[i]._dataItem.MessageDetailID
        PageMethods.SetMoveToFolderAndUpdateResult(0, FolderMovementVars.tableView.get_pageSize(), FolderMovementVars.tableView.get_sortExpressions().toString(), FolderMovementVars.tableView.get_filterExpressions().toList(), FolderMovementVars.folderId, FolderMovementVars.folderCode, newMoveToFolderID, moveToFolderCode, cellID, currentUserID, FolderMovementVars.userGroup, queueType, from, to, subject, messageBody, updateGrid);
    }
    FolderMovementVars = [];
}
function updateGrid(result) {
    //    var tableView = parent.pageFrame.window.tableviewMessageMove;
    //    var folderId = parent.pageFrame.window.folderId;
    //    var folderCode = parent.pageFrame.window.folderCode;
    //    var selectedFolderName = parent.pageFrame.window.selectedFolderName;
    //    parent.pageFrame.window.updateMessagingGrid(tableView, folderId, folderCode, currentUserID, 0, queueType);
    //    parent.pageFrame.window.updateFolderCount(selectedFolderName, folderId, folderCode, currentUserID);
    //    parent.pageFrame.window.updateFolderCount(FolderMovementVars.x, FolderMovementVars.moveToFolderCode, FolderMovementVars.moveToFolderCode, currentUserID);
    // 
    returnToParent();
    top.location.href = top.location.href;
    //  __doPostBack();
}

function AddFolder() {
    var moveToFolderID = $jQuery("#hdnMoveTofolderID");
    var parentFolderID = $jQuery("#hdnParentID");
    var textBox = $find($jQuery("[id$=txtFolderName]")[0].id);
    newFolderName = textBox.get_value();
    var checkDuplicatenode = false;
    var count = 0;
    treeView = $find(FSObject.$("[id$=treePersonalFolders]")[0].id);
    for (var i = 0; i < treeView.get_allNodes().length; i++) {
        var node = treeView.get_allNodes()[i].get_textElement().innerHTML;
        if (node.toUpperCase().trim() == newFolderName.toUpperCase().trim()) {
            checkDuplicatenode = true;
        }
    }
    // if (count > 1) {
    //     checkDuplicatenode = true;
    // }
    if (checkDuplicatenode) {
        $alert("A folder with this name already exists. Use another name.");
        return false;
    }

    var node = new Telerik.Web.UI.RadTreeNode();
    node.set_text(newFolderName);
    if (typeof (__newFolderImg) != "undefined" && __newFolderImg) {
        node.set_imageUrl(__newFolderImg);
    }

    treeView.trackChanges();
    treeView.get_selectedNode().get_nodes().add(node); ;
    treeView.commitChanges();
    //Page.showProgress("Processing...");
    var userGroup = 0;
    //    if (($jQuery("[id$=cmbUsergroup]")[0]) != null) {
    //        userGroup = ($jQuery("[id$=cmbUsergroup]")[0]).control.get_value();
    //    }
    //    if (userGroup == "") {
    //        userGroup = 0;
    //    }
    var parentFolderid = parentFolderID.val().split('#')[0];
    PageMethods.AddNewFolder(newFolderName.trim(), currentUserID, userGroup, parentFolderid, UpdateFolder);
    //    textBox.clear();
    //    __doPostBack();
    return false;
}

function UpdateFolder(data) {
    data = $jQuery.parseJSON(data);
    if (data.result == "success") {
        $jQuery("#hdnMoveTofolderID").val(data.folderName);
        var moveToFolderCode = data.folderName.split('#')[1];
        var newMoveToFolderID = data.folderName.split('#')[0];
        MoveToFolderAndUpdateResult(newMoveToFolderID, moveToFolderCode);
    }
    else if (data.result == "duplicate") {
        $alert("A folder with this name previously created, please use another name.");
    }

}

function returnToParent() {
    $page.get_window().set_destroyOnClose(false);
    $page.get_window().close();
}

