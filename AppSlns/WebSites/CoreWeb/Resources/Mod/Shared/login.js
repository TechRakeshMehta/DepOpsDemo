
if (typeof (parent) != "undefined") {
    if (typeof (parent.Page) != "undefined") {
        parent.Page.postBack();
    }
}

FSObject.onReady(function ($) {
    $('#loginbox').fadeIn();
  });


// Checks the caps lock is on or not.
function capLock(e) {
    kc = e.keyCode ? e.keyCode : e.which;
    sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
    if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk)) {
        document.getElementById('divCapsLock').style.visibility = 'visible';
    }
    else {
        document.getElementById('divCapsLock').style.visibility = 'hidden';
    }
 }

FSObject.onReady(function () {
if ((screen.width <= 1024) && (screen.height <= 768)) {
    FSObject.$(".logo").removeClass("LtColPnl").addClass("LtColPnlSmall");
  }
else {
    FSObject.$(".logo").addClass("LtColPnl");
     }
});