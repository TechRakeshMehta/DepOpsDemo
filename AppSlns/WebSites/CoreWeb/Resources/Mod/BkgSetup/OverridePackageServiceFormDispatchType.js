
$jQuery(document).ready(function () {
    $jQuery("[id*=Panel3]").each(function () {
        UpdateSts(this);
    });
});

function ManageSts(selectedElement) {
    var selectedDiv = $jQuery(selectedElement).parents('.rootDiv');
    UpdateSts(selectedDiv);
}

function UpdateSts(selectedDiv) {

    var _div = $jQuery(selectedDiv);
    var _checkBokxSts = _div.find('input:checkbox');

    if (_checkBokxSts[0].checked) {
        _div.find('#spnDispatchMode').text('None');
    }
    else {
        var selectedText = _div.find('input:radio:checked').val();
        if (selectedText == 'AAAA') {
            var _sts = _div.find('[id*=hdnInheritedStatus]').val();
            _div.find('#spnDispatchMode').text(_sts);
        }
        else if (selectedText == 'AAAB') {
            _div.find('#spnDispatchMode').text('Automatic');
        }
        else if (selectedText == 'AAAC') {
            _div.find('#spnDispatchMode').text('Manual');
        }
    }
}