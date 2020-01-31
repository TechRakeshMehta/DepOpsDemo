/*

IE 7 SUPPORT FILE (MVP)
Version: 2013.1.8.1530
 
Log--
2013.1.8.1515
    1. Initially created (blank)

2013.1.1530
    1. sxlb and sxlm box-sizing fix
*/


$page.add_pageReady(function ($) {
    $(window).resize(function () { _fxformie7($, true); });
});

$page.add_pageLoad(function () {
    _fxformie7($jQuery);
});

var _fxformie7 = function ($, r) {

    //Preventing multiple resize events
    if (r) {
        if (!DocWidthChanged($jQuery)) return;
    }

    //Preventing top window to execute this code
    if (top === self) return;

    //Resizing form elements    

    $("div.sxro").each(function (a) {        
        var tWid = $(this).width();
        var xWid = 0;

        $(this).children(".sxlb").each(function (b) {
            var cWid = parseFloat($(this).css("width"));
            _adjustWidth(this, tWid, $);
            xWid = xWid + $(this).outerWidth();
        });

        $(this).children(".sxlm").each(function (b) {
            var cWid = parseFloat($(this).css("width"));
            _adjustWidth(this, tWid, $);
            xWid = xWid + $(this).outerWidth();

        });

    });

}

//Return
var DocWidthChanged = function ($) {

    var _yes = false;
    var _attr = "_infsResize";
    var _cTime = Date().valueOf();
    var _cWidth = $(document).width();

    var _pVal = $("body").attr(_attr);

    if (_pVal) {
        _pVal = _pVal.split("/");
        var _pValt = _pVal[0] || 1;
        var _pValw = _pVal[1] || 1;
    }

    if (_cTime == _pValt || _cWidth == _pValw) return false;
    else {
        $("body").attr(_attr, _cTime + "/" + _cWidth)
        return true;
    }
}

var _adjustWidth = function (that, roomWidth, $) {

    var _attr = "_adjustWidth";
    var _space = 0;
    //Get original style
    var pW = $(that).attr(_attr);
    if (!pW) {
        //First time exec., set attr
        pW = $(that).get(0).currentStyle.width;
        $(that).attr(_attr, pW);
    }

    nW = parseFloat(pW) * roomWidth / 100;
    var val = parseFloat($(that).css("border-left-width"))
    _space = _space + (isNaN(val) ? 0 : val);
    val = parseFloat($(that).css("border-right-width"))
    _space = _space + (isNaN(val) ? 0 : val);
    val = parseFloat($(that).css("padding-right"))
    _space = _space + (isNaN(val) ? 0 : val);
    val = parseFloat($(that).css("padding-left"))
    _space = _space + (isNaN(val) ? 0 : val);

    $(that).width(nW - _space);

}