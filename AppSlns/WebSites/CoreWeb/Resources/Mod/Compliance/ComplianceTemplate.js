function openCreateExprPopup(complianceCategoryComplianceItemID, complianceItemAttributeID) {
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    $window.createPopup($page.url.create("~/ComplianceAdministration/Pages/ManageExpression.aspx?complianceCategoryItemID=" + complianceCategoryComplianceItemID + "&complianceItemAttributeID=" + complianceItemAttributeID), { size: "800,"+popupHeight });
    return false;
}