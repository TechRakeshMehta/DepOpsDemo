
function OnSelectedExpiryTypeRadioButton(event) {

    //if (event.value == 'AAAF') {
    //    $jQuery("[id$=divExpiresIn]").show();
    //    $jQuery("[id$=divExpiresOn]").hide();
    //    EnableValidator($jQuery("[id$=rfvExpirationValue]")[0].id);
    //    EnableValidator($jQuery("[id$=rfvExpirationValueType]")[0].id);
    //    EnableValidator($jQuery("[id$=rfvDateTypeFields]")[0].id);
    //    DisableValidator($jQuery("[id$=rfvExpiresOn]")[0].id);
    //}
    //else if ((event.value == 'AAAE')) {
    //    $jQuery("[id$=divExpiresIn]").hide();
    //    $jQuery("[id$=divExpiresOn]").show();
    //    EnableValidator($jQuery("[id$=rfvExpiresOn]")[0].id);
    //    DisableValidator($jQuery("[id$=rfvExpirationValue]")[0].id);
    //    DisableValidator($jQuery("[id$=rfvExpirationValueType]")[0].id);
    //    DisableValidator($jQuery("[id$=rfvDateTypeFields]")[0].id);
    //}

    //else {
    //    $jQuery("[id$=divExpiresIn]").hide();
    //    $jQuery("[id$=divExpiresOn]").hide();
    //    DisableValidator($jQuery("[id$=rfvExpiresOn]")[0].id);
    //    DisableValidator($jQuery("[id$=rfvExpirationValue]")[0].id);
    //    DisableValidator($jQuery("[id$=rfvExpirationValueType]")[0].id);
    //    DisableValidator($jQuery("[id$=rfvDateTypeFields]")[0].id);
    //}
}

function EnableValidator(id) {
    if ($jQuery('#' + id)[0] != undefined) {
        ValidatorEnable($jQuery('#' + id)[0], true);
        $jQuery('#' + id).hide();
    }
}

function DisableValidator(id) {
    if ($jQuery('#' + id)[0] != undefined) {
        ValidatorEnable($jQuery('#' + id)[0], false);
    }
}

function OnSelectedVideoRadioButton(event) {
    var id = "[id$=" + event.id + "]";
    var rdButtonListId = "#" + event.id + " input[type=radio]:checked";
    var selectedvalue = $jQuery(rdButtonListId)[0].value;
    if (selectedvalue == 'true') {
        $jQuery("[id$=divVideoDuration]").show();
        EnableValidator($jQuery("[id$=rfvVideoSeconds]")[0].id);
        EnableValidator($jQuery("[id$=rfvVideoMinutes]")[0].id);
    }
    else {
        $jQuery("[id$=divVideoDuration]").hide();
        findRadControlByServerId("txtVideoSeconds").clear();
        findRadControlByServerId("txtVideoMinutes").clear();
        DisableValidator($jQuery("[id$=rfvVideoSeconds]")[0].id);
        DisableValidator($jQuery("[id$=rfvVideoMinutes]")[0].id);
    }
}

function findRadControlByServerId(Id) {
    var buttonId = $find($jQuery("[id$=" + Id + "]").attr('id'));
    return buttonId;
}

//Custom Rule Create Logic
var html = '';
var uiExp = '';
var sqlExp = '';
var accesser = {};
var expression = {};
var newlyAddedNodeIds = [];
var isCustomRuleValid = true;
//var itemLst = [{ id: '-1', text: "--Select--" }, { id: 1, text: "ABC" }, { id: 2, text: "DEF" }, { id: "-2", text: "Discard" }];
var itemLst = [];
var operatorLst = [{ id: 'AND', text: "AND" }, { id: 'OR', text: "OR" }, { id: "-2", text: "Discard" }];
var isCustomRuleItemSelected = false;

//Variables for produce initial rule
var dict = []; // created an empty array
var input = '';//document.getElementById("txt").value;
var levels = '';
var maxlevel = 0;
var tree = {};

$jQuery(document).ready(function () {

    if ($jQuery("#dvCustomRule").length > 0) {
        $jQuery("#dvCustomRule").hide();
    }

    $jQuery("#dvRuleUIExpression").hide();

    if ($jQuery("[id$='cmdbarRules_btnExtra']").length > 0) {
        $jQuery("[id$='cmdbarRules_btnExtra']").hide();
    }

    if ($jQuery(".initialRule").length > 0) {
        $jQuery(".initialRule").hide();
    }

    if ($jQuery("[id$='cmdbarRules_btnExtra']").length > 0) {
        $jQuery("[id$='cmdbarRules_btnExtra']").bind('click', function () {
            $jQuery("[id$=txtCustomRule]").val('');
            $jQuery("#lnkInitialRule").show();
            $jQuery(".initialRule").hide();

            expression = { nodetype: 'new', id: 'root', parentId: null };
            initializeObjects();
        });
    }

    if ($jQuery(".categoryRule").length > 0) {
        $jQuery(".categoryRule").bind('click', function (e) {
            if (isCustomRuleOptionSelected()) {
                $jQuery("#dvCustomRule").show();
                $jQuery("#dvRuleUIExpression").show();

                $jQuery("[id$='cmdbarRules_btnExtra']").show();
                expression = { nodetype: 'new', id: 'root', parentId: null };
                initializeObjects();
                isCustomRuleItemSelected = true;
            }
            else {
                $jQuery("#dvCustomRule").hide();
                $jQuery("#dvRuleUIExpression").hide();

                $jQuery("[id$='cmdbarRules_btnExtra']").hide();
                isCustomRuleItemSelected = false;
            }
        });
    }

    if ($jQuery(".initialRule").length > 0) {
        $jQuery("#lnkInitialRule").click(function () {
            $jQuery(this).hide();
            $jQuery(".initialRule").show();
        });
    }

    if ($jQuery("#btnProduceInitialRule").length > 0) {

        $jQuery("#btnProduceInitialRule").click(function () {

            if ($jQuery("[id$=txtCustomRule]").val().length == 0) {
                showAlertMessage('Please provide initial custom rule structure.', 'info', true);
                return false;
            }

            $jQuery("#lnkInitialRule").show();
            $jQuery(".initialRule").hide();

            tree = {};
            dict = [];
            input1 = input = $jQuery("[id$=txtCustomRule]")[0].value;

            //Replacing
            input1 = input = input1.replace('AND(', 'AND (');
            input1 = input = input1.replace('OR(', 'OR (');
            input1 = input = input1.replace(')AND', ') AND');
            input1 = input = input1.replace(')OR', ') OR');
            input1 = input = input1.toUpperCase();
            input1 = input = input1.replace(/\n/g, ' ')


            var bracesCount = 0
            for (var i = 0; i < input1.length; i++) {

                if (input.substr(i, 1) == '(') {
                    bracesCount = bracesCount + 1;
                }

                if (input.substr(i, 1) == ')') {
                    bracesCount = bracesCount - 1;
                }

                if (bracesCount < 0) {
                    $jQuery("#lnkInitialRule").click();
                    showAlertMessage('Rule structure could not be parsed, Please verify your input.', 'info', true);
                    return;
                }
            }

            if (bracesCount != 0) {
                $jQuery("#lnkInitialRule").click();
                showAlertMessage('Rule structure could not be parsed, Please verify your input.', 'info', true);
                return;
            }

            levels = '';
            parseForLevels(input1, 0, ' AND ');
            parseForLevels(input1, 0, ' OR ');

            if (dict.length <= 0) {
                $jQuery("#lnkInitialRule").click();
                showAlertMessage('Initial structure must contains one logical operator.', 'info', true);
                return false;
            }

            levels = '';
            dict.forEach(function (obj, index) { levels += 'op=' + obj.value.operator + ' | level= ' + obj.value.nestinglevel + ' | left=' + obj.value.left + ' | right=' + obj.value.right + '<br/>'; });
            buildTree();
            renderExpression(expression);
        });
    }

    var items = $jQuery("[id$='hdnCategoryItems']").val();

    if (items.length > 0) {
        itemLst = JSON.parse(items);
    }

    isCustomRuleItemSelected = $jQuery("[id$='hdnIsCustomRuleOptionSelected']").val() == "1" ? true : false;
    var isCustomRuleInDisableMode = true;

    if ($jQuery(".categoryRule > input[type='radio']").length > 0) {
        isCustomRuleInDisableMode = false;
    }

    if (isCustomRuleItemSelected) {
        $jQuery("#dvRuleUIExpression").show();

        if ($jQuery("[id$='hdnCustomRuleStringifyObj']").length > 0 && $jQuery("[id$='hdnCustomRuleStringifyObj']").val() != '') {
            expression = JSON.parse($jQuery("[id$='hdnCustomRuleStringifyObj']").val());
            initializeObjects();
        }

        if (isCustomRuleInDisableMode) {
            if ($jQuery("[id$='cmdbarRules_btnExtra']").length > 0) {
                $jQuery("[id$='cmdbarRules_btnExtra']").hide();
            }
            if ($jQuery("#dvCustomRule").length > 0) {
                $jQuery("#dvCustomRule").hide();
            }
        }
        else {
            if ($jQuery("[id$='cmdbarRules_btnExtra']").length > 0) {
                $jQuery("[id$='cmdbarRules_btnExtra']").show();
            }
            if ($jQuery("#dvCustomRule").length > 0) {
                $jQuery("#dvCustomRule").show();
            }
        }
    }
});

function isCustomRuleOptionSelected() {
    if ($jQuery(".categoryRule > input[type='radio']:checked").attr('value') == 'AAAI') {
        return true;
    }
    return false;
}

function validateCustomRule(sender, args) {
    if (isCustomRuleItemSelected) {
        if (!isCustomRuleValid) {
            showAlertMessage('Custom rule is incomplete!', 'info', true);
            sender.set_autoPostBack(false);
        }
        else {
            $jQuery("[id$='hdnSqlExpression']").val(sqlExp);
            $jQuery("[id$='hdnCustomRuleStringifyObj']").val(JSON.stringify(expression));
            sender.set_autoPostBack(true);
        }
    }
    else {
        sender.set_autoPostBack(true);
    }
}

function initializeObjects() {

    html = '';
    uiExp = '';

    accesser["root"] = expression;
    renderExpression(expression);
}

function renderExpression(expr) {
    //if (!isNaN(parent.ResetTimer)) {
    //    parent.ResetTimer();
    if ($jQuery.isFunction(parent.ResetTimer)) {
        parent.ResetTimer();
    }

    html = '';
    uiExp = '';
    sqlExp = '';
    isCustomRuleValid = true;
    traverse(expr);

    if ($jQuery('#dvRuleExpression').length > 0) {
        $jQuery('#dvRuleExpression').html(html);
    }

    if ($jQuery('#dvUIExpr').length > 0) {
        $jQuery('#dvUIExpr').html(uiExp);
    }

    if ($jQuery('#dvSqlExpr').length > 0) {
        $jQuery('#dvSqlExpr').html(sqlExp);
    }
}

function traverse(jsonObj) {

    accesser[jsonObj.id] = jsonObj;

    var styleHtml = "";
    var parentObj = accesser[jsonObj.parentId];

    if (jsonObj.left != undefined) {

        if (jsonObj.operatorid == undefined) {
            jsonObj.operatorid = "AND";
        }

        if (parentObj != undefined && !((jsonObj.operatorid.trim().toUpperCase() == "AND" && parentObj.operatorid.trim().toUpperCase() == "AND") || (jsonObj.operatorid.trim().toUpperCase() == "OR" && parentObj.operatorid.trim().toUpperCase() == "OR"))) {
            html += '<span class="inputBraces" style="color:' + jsonObj.bracesColor + '">(</span>'
            uiExp += '&nbsp;&nbsp;<span class="uiInputBraces" style="color:' + jsonObj.bracesColor + '">(</span>'
            sqlExp += ' ( ';
        }
        traverse(jsonObj.left);
    }

    if (typeof jsonObj == "object") {
        if (jsonObj.nodetype == 'new') {

            isCustomRuleValid = false;

            if (newlyAddedNodeIds.indexOf(jsonObj.id) >= 0) {
                styleHtml = 'style="color:green;font-weight:bold;"';
            }

            html += ' ';
            html += '<input type="button" title="Expression" class="inputButton RadButton RadButton_Silk rbSkinnedButton" ' + styleHtml + ' id="' + jsonObj.id + '-expr" value="Expr" onclick="addExpression(\'' + jsonObj.id + '\')"/>';
            html += '<input type="button" title="Item" class="inputButton RadButton RadButton_Silk rbSkinnedButton" ' + styleHtml + ' id="' + jsonObj.id + '-item" value="Item"  onclick="addItem(\'' + jsonObj.id + '\')"/>';
            html += '';

            uiExp += "&nbsp;&nbsp;<span class='uiExp' " + styleHtml + ">E/I</span>";
            sqlExp += " E/I "
        }
        else if (jsonObj.nodetype == 'item') {

            html += '   ';
            var itmTextForUiExp = "Itm";
            var itmTextForSqlExp = "$-1#"

            var newItem = document.createElement("select");

            for (i = 0; i < itemLst.length; i++) {
                var op = new Option();
                op.value = itemLst[i].id;
                op.text = itemLst[i].text;

                if (op.value == jsonObj.itemid) {
                    op.setAttribute("selected", "selected");

                    if (op.value != -1) {
                        itmTextForUiExp = op.text;
                        itmTextForSqlExp = "$" + op.value + "#";
                    }
                    else {
                        isCustomRuleValid = false;
                    }
                }

                newItem.options.add(op);
            }

            $jQuery(newItem).attr("onchange", "itemSelected(\'" + jsonObj.id + "\', this.value)");
            $jQuery(newItem).attr('class', 'RadComboBox RadComboBox_Silk form-control inputComboBox');

            if (newlyAddedNodeIds.indexOf(jsonObj.id) >= 0) {
                $jQuery(newItem).attr('style', 'color:green;font-weight:bold;');;
                uiExp += "&nbsp;&nbsp;<span class='uiExp' style='color:green;font-weight:bold;'>" + itmTextForUiExp + "</span>";
            }
            else {
                uiExp += "&nbsp;&nbsp;<span class='uiExp'>" + itmTextForUiExp + "</span>";
            }

            sqlExp += " " + itmTextForSqlExp + " ";

            html += newItem.outerHTML;
            html += '   ';
        }
        else if (jsonObj.nodetype == 'expression') {
            html += '   ';
            var operatorTextForUiExp = "Optr";

            var newItem = document.createElement("select");
            for (i = 0; i < operatorLst.length; i++) {
                var op = new Option();
                op.value = operatorLst[i].id;
                op.text = operatorLst[i].text;

                if (op.value == jsonObj.operatorid.trim()) {
                    op.setAttribute("selected", "selected");
                    if (op.value != "-1") {
                        operatorTextForUiExp = op.text;
                    }
                    else {
                        isCustomRuleValid = false;
                    }
                }
                newItem.options.add(op);
            }

            $jQuery(newItem).attr("onchange", "operatorSelected(\'" + jsonObj.id + "\', this.value)");
            $jQuery(newItem).attr('class', 'RadComboBox RadComboBox_Silk form-control inputComboBox optrComboBox');

            if (newlyAddedNodeIds.indexOf(jsonObj.id) >= 0) {
                $jQuery(newItem).attr('style', 'color:green;font-weight:bold;');
                uiExp += "&nbsp;&nbsp;<span class='uiOptr' style='color:green;font-weight:bold;'>" + operatorTextForUiExp + "</span>";
            }
            else {
                uiExp += "&nbsp;&nbsp;<span class='uiOptr'>" + operatorTextForUiExp + "</span>";
            }

            sqlExp += " " + operatorTextForUiExp + " ";

            html += newItem.outerHTML;
            html += '   ';
        }

        if (jsonObj.right != undefined) {

            traverse(jsonObj.right);

            if (parentObj != undefined && !((jsonObj.operatorid.trim().toUpperCase() == "AND" && parentObj.operatorid.trim().toUpperCase() == "AND") || (jsonObj.operatorid.trim().toUpperCase() == "OR" && parentObj.operatorid.trim().toUpperCase() == "OR"))) {
                html += '<span class="inputBraces" style="color:' + jsonObj.bracesColor + '">)</span>'
                uiExp += '&nbsp;&nbsp;<span class="uiInputBraces" style="color:' + jsonObj.bracesColor + '">)</span>'
                sqlExp += " ) ";
            }
        }
    }
}

function addItem(nodeid) {
    newlyAddedNodeIds = [];
    newlyAddedNodeIds.push(nodeid);

    var node = accesser[nodeid];
    node.nodetype = 'item';
    node.itemid = -1;

    renderExpression(expression);
}

function operatorSelected(nodeid, id) {
    newlyAddedNodeIds = [];
    newlyAddedNodeIds.push(nodeid);

    var node = accesser[nodeid];
    node.operatorid = id;

    if (id == "-2") {
        node.left = null;
        node.right = null;
        node.nodetype = "new";
        node.operatorid = "AND";
    }

    renderExpression(expression);
}

function itemSelected(nodeid, itemid) {
    newlyAddedNodeIds = [];
    newlyAddedNodeIds.push(nodeid);

    var node = accesser[nodeid];
    node.itemid = itemid;

    if (itemid == -2) {
        node.left = null;
        node.right = null;
        node.nodetype = "new";
        node.operatorid = "AND";
    }
    renderExpression(expression);
}

function addExpression(nodeid) {
    newlyAddedNodeIds = [];
    var node = accesser[nodeid];
    var leftid = guid();
    var rightid = guid();
    node.nodetype = "expression";
    node.bracesColor = getRandomColor();
    node.left = { nodetype: 'new', id: leftid, parentId: nodeid };
    node.right = { nodetype: 'new', id: rightid, parentId: nodeid };
    newlyAddedNodeIds.push(leftid);
    newlyAddedNodeIds.push(rightid);
    renderExpression(expression);
}

function getRandomColor() {
    var letters = '0123456789ABCDEF'.split('');
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}

function parseForLevels(str, seed, op) {
    var ANDIndex = str.indexOf(op) + seed;
    if (ANDIndex >= seed) {
        var count = 0

        for (i = 0; i <= ANDIndex; i++) {
            if (input.substr(i, 1) == '(') {
                count = count + 1;
            }
            else
                if (input.substr(i, 1) == ')') {
                    count = count - 1;
                }
        }

        dict[ANDIndex] = {
            key: ANDIndex,
            value: { nestinglevel: count, operator: op, left: getOperand(input.substr(0, ANDIndex), count, 'left', ANDIndex), right: getOperand(input.substr(ANDIndex + op.length), count, 'right', ANDIndex + op.length), leftindex: ANDIndex, rightIndex: ANDIndex + op.length }
        };

        parseForLevels(input.substr(ANDIndex + op.length), ANDIndex + op.length, op);
    }
}

function getSiblingIndex(level, sourceindex, orinetation) {
    var result = 0;
    if (orinetation == 'left') {
        result = 0;
        dict.forEach(function (item, index) {
            if (item.value.nestinglevel == level) {
                if (item.value.rightIndex > result && sourceindex > item.value.rightIndex) {
                    result = item.value.rightIndex;
                }

            }

        })
    }
    else
        if (orinetation == 'right') {
            result = input.length;
            dict.forEach(function (item, index) {
                if (item.value.nestinglevel == level) {
                    if (item.value.rightIndex < result && sourceindex < item.value.leftindex) {
                        result = item.value.rightIndex;
                    }

                }

            })
        }

    return result;
}

function getOperand(Str, levels, Orient, sourceindex) {
    var lastindex = getSiblingIndex(levels, sourceindex, Orient);
    if (Orient == 'left') {
        var leftStrLength = 0;
        var count = levels;
        for (i = Str.length; i >= 0; i--) {
            leftStrLength++;
            if (Str.substr(i, 1) == '(') {
                count = count - 1;
            }
            else
                if (Str.substr(i, 1) == ')') {
                    count = count + 1;
                }
            if ((count < levels) || lastindex > i) {
                leftStrLength = leftStrLength - 1;
                break;
            }
        }
        return Str.substr(Str.length + 1 - leftStrLength, leftStrLength + 1);
    }
    else
        if (Orient == 'right') {
            var rightStrLength = 0;;
            var count = levels;
            for (i = 0 ; i <= Str.length; i++) {
                rightStrLength++;
                if (Str.substr(i, 1) == '(') {
                    count = count + 1;
                }
                else
                    if (Str.substr(i, 1) == ')') {
                        count = count - 1;
                    }
                if ((count < levels) || lastindex <= i + sourceindex) {
                    rightStrLength = rightStrLength - 1;
                    break;
                }
            }
            return Str.substr(0, rightStrLength);
        }
}

function buildTree() {
    dict.forEach(function (item, index) {
        if (item.value.nestinglevel > maxlevel) maxlevel = item.value.nestinglevel;
    });

    expression = processTreeLevel(0, null, 0, input.length);
}

function processLowerNodes(node, level, parentid, startindex, stopindex, i) {
    if (level <= maxlevel) {
        var nextlevelforLeft = 99999;
        var nextlevelforRight = 99999;
        for (var j = startindex; j < i - 1; j++) {
            if (dict[j] != undefined && dict[j].value.nestinglevel < nextlevelforLeft) nextlevelforLeft = dict[j].value.nestinglevel;
        }
        for (var j = i + 1; j < stopindex; j++) {
            if (dict[j] != undefined && dict[j].value.nestinglevel < nextlevelforRight) nextlevelforRight = dict[j].value.nestinglevel;
        }

        if (nextlevelforLeft != 99999) {
            node.left = processTreeLevel(nextlevelforLeft, node.id, startindex, i - 1);
        }
        else {
            var itemId_ = -1;
            var itemText = dict[i].value.left;
            itemText = itemText.trim().toLowerCase();

            for (var j = 0; j < itemLst.length; j++) {
                if (itemText == itemLst[j].text.trim().toLowerCase()) {
                    itemId_ = itemLst[j].id;
                    break;
                }
            }

            node.left = {
                source: null,
                id: guid(),
                parentId: node.id,
                left: null,
                right: null,
                nodetype: 'item',
                operatorid: dict[i].value.op,
                itemid: itemId_,
                bracesColor: null
            }
        }

        if (nextlevelforRight != 99999) {
            node.right = processTreeLevel(nextlevelforRight, node.id, i + 1, stopindex);
        }
        else {

            var itemId_ = -1;
            var itemText = dict[i].value.right;
            itemText = itemText.trim().toLowerCase();

            for (var j = 0; j < itemLst.length; j++) {
                if (itemText == itemLst[j].text.trim().toLowerCase()) {
                    itemId_ = itemLst[j].id;
                    break;
                }
            }

            node.right = {
                source: null,
                id: guid(),
                parentId: node.id,
                left: null,
                right: null,
                nodetype: 'item',
                operatorid: null,
                itemid: itemId_,
                bracesColor: null
            }
        }
    }
    //else {
    //    node.left = {
    //        source: null,
    //        id: guid(),
    //        parentId: node.id,
    //        left: null,
    //        right: null,
    //        nodetype: 'item',
    //        operatorid: null,
    //        itemid: dict[i].value.left,
    //        bracesColor: null
    //    };

    //    node.right = {
    //        source: null,
    //        id: guid(),
    //        parentId: node.id,
    //        left: null,
    //        right: null,
    //        nodetype: 'item',
    //        operatorid: null,
    //        itemid: dict[i].value.right,
    //        bracesColor: null
    //    };
    //}

}

function processTreeLevel(level, parentid, startindex, stopindex) {
    var node = {};
    var secondlevel = 9999999;
    for (var i = startindex; i <= stopindex; i++) {
        if (dict[i] != undefined && dict[i].value.nestinglevel >= level && dict[i].value.nestinglevel < secondlevel) {
            secondlevel = dict[i].value.nestinglevel;
        }
    }
    level = secondlevel;
    for (var i = startindex; i <= stopindex; i++) {
        if (dict[i] != undefined && dict[i].value.nestinglevel == level) {
            var nid = guid(); if (parentid == null) nid = 'root';
            node = {
                source: dict[i].value,
                id: nid,
                parentId: parentid,
                left: null,
                right: null,
                nodetype: 'expression',
                operatorid: dict[i].value.operator,
                itemid: -1,
                bracesColor: null
            }
            processLowerNodes(node, level, parentid, startindex, stopindex, i);
            break;
        }

    }

    return node;
}

function showAlertMessage(msg, msgtype, overriderErrorPanel) {
    if (typeof (msg) == "undefined") return;
    var c = typeof (msgtype) != "undefined" ? msgtype : "";
    if (overriderErrorPanel) {
        $jQuery("#pageMsgBoxSchuduleInv").children("span")[0].innerHTML = msg;
        $jQuery("#pageMsgBoxSchuduleInv").children("span").attr("class", msgtype);
        if (c == 'sucs') {
            c = "Success";
        }
        else (c = "Validation Message(s)");

        $jQuery("[id$=pnlErrorSchuduleInv]").hide();

        $window.showDialog($jQuery("#pageMsgBoxSchuduleInv").clone().show(), { closeBtn: { autoclose: true, text: "Ok" } }, 500, c);
    }
    else {
        $jQuery("#pageMsgBoxSchuduleInv").fadeIn().children("span")[0].innerHTML = msg;
        $jQuery("#pageMsgBoxSchuduleInv").fadeIn().children("span").attr("class", msgtype);
    }
}
