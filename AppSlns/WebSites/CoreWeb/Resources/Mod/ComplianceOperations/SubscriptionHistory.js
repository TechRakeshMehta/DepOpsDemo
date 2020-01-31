//click on link button while double click on any row of grid.
function grd_rwDbClick(s, e) {
    var _id = "btnEnterData";
    var b = e.get_gridDataItem().findControl(_id);
    if (b && typeof (b.click) != "undefined") { b.click(); }
}