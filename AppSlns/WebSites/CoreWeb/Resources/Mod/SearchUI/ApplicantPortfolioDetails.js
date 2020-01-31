$page.add_pageLoad(function () {
    //Accessibility Code
    //Checkbox added aria attribute
    $jQuery("[id$=chkMiddleNameRequired]").attr('aria-labelledby', $jQuery("[id$=lblChkMiddleName]")[0].id);
    $jQuery("[id$=chkShowHideAlias]").attr('aria-labelledby', $jQuery("[id$=ucPersonAlias_lblChkShowHide]")[0].id);
    $jQuery("[id$=cmbGender_Input]").attr('role', 'combobox');
    $jQuery("[id$=locationTenant_cmbCountry_AEP_Input").attr('role', 'combobox');

    $jQuery("[id$=locationTenant_cmbRSL_State_AEP_Input]").attr('role', 'combobox');
    $jQuery("[id$=locationTenant_cmbRSL_State_AEP_Input]").attr('aria-labelledby', $jQuery("[id$=locationTenant_lblRSL_State_AEP]")[0].id);

    $jQuery("[id$=locationTenant_cmbRSL_ZipId_AEP_Input]").attr('role', 'combobox');
    $jQuery("[id$=locationTenant_cmbRSL_ZipId_AEP_Input]").attr('aria-labelledby', $jQuery("[id$=locationTenant_lblRSL_Zipcode_AEP]")[0].id);

    $jQuery("[id$=locationTenant_cmbRSL_County_AEP_Input]").attr('role', 'combobox');

    if ($jQuery("[id$=lblMoveInDate_AddNewAdd]").length > 0) {
        $jQuery("[id$=dpResFrm_dateInput]").attr('aria-labelledby', $jQuery("[id$=lblMoveInDate_AddNewAdd]")[0].id);
    }

    if ($jQuery("[id$=lblResidentUntil_AddNewAdd]").length > 0) {
        $jQuery("[id$=dpResTill_dateInput]").attr('aria-labelledby', $jQuery("[id$=lblResidentUntil_AddNewAdd]")[0].id);
    }

    //divPersonalAlias -- Repeater
    $jQuery("div[id*=divPersonalAlias] .col-md-3 input, div[id*=locationTenant_dvReverseStateLookup] .col-md-3 input, div[id*=caProfileCustomAttributes] .col-md-3 input").each(function (i, e) {
        var input = $jQuery(this);
        var relatedSpan = $jQuery(input).parents('.col-md-3').children(0);
        var relatedSpanId = $jQuery(relatedSpan).attr('id');

        if ($jQuery(input).attr('type') != 'radio') {
            $jQuery(input).attr('aria-labelledBy', relatedSpanId);
        }
    });

    $jQuery("[id$=gbcDeleteColumn]").attr('title', 'Delete Address');
    $jQuery("tr[id*=grdResidentialHistory] input[id$=EditButton]").attr('title', 'Edit Address');

    //Adding tool-tip to View Portfolio link
    $jQuery("span.rbText:contains('Detail'), span.rbText:contains('Not Applicable')").each(function (i, o) {
        var title = $jQuery(o).parent().attr('title');
        $jQuery(o).append("<span class='sr-only'>" + title + "</span>");
    });
});