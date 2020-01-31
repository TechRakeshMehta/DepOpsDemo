function updateBookmarkStatus(sender, obj) {
    if (sender.attributes.Enabled != undefined && sender.attributes.Enabled.value == "false") {
        return false;
    }
    var currentOrgUserID = $jQuery("#hdnCurrentOrgUserID").val();
    var tenantProductSysXBlockID = $jQuery(sender).attr('TenantProductSysXBlockID');
    var productFeatureID = $jQuery(sender).attr('ProductFeatureID');
    var isBookmarkedStr = $jQuery(sender).attr('IsFeatureBookmarked');
    var isBookmarked = !(isBookmarkedStr.toLowerCase().trim() == "true" ? true : false);

    var dataString = "tenantProductSysXBlockID : '" + tenantProductSysXBlockID + "', productFeatureID : '" + productFeatureID + "', orgUserID : '" + currentOrgUserID + "', isBookmarked : '" + isBookmarked + "'";
    var urltoPost = "/PersonalSettings/Default.aspx/SaveUpdateFeatureBookmark";

    $jQuery.ajax
     (
      {
          type: "POST",
          url: urltoPost,
          data: "{ " + dataString + " }",
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          success: function (result) {
              var data = JSON.parse(result.d);
              var status = data.Status;

              if (status) {
                  if (isBookmarked) {
                      $jQuery(sender)[0].title = "Remove bookmark."
                      $jQuery(sender).attr('class', 'star fa fa-star');
                      $jQuery(sender).attr('IsFeatureBookmarked', 'True');
                  }
                  else {
                      $jQuery(sender)[0].title = "Bookmark this feature."
                      $jQuery(sender).attr('class', 'star fa fa-star-o');
                      $jQuery(sender).attr('IsFeatureBookmarked', 'False');
                  }
              }
          }
      });

    return false;
}