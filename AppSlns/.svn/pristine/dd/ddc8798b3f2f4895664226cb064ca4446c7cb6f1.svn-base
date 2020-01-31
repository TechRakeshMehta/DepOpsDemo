$page.add_pageLoad(function () {

    $jQuery(".rule_element").each(function () {
        $jQuery(this).click(function (e) {

            $jQuery(this).parents("#rule_board").find(".selected").removeClass("selected");
            $jQuery(this).addClass("selected");

            var _collectionID = $jQuery(this).attr("_infsCollectionCode");

            $jQueryByKey("CollectionID", function () {
                console.log(_collectionID);
                $jQuery(this).val(_collectionID);
            });

            $findByKey("cmdRemoveSelected", function () {
                this.set_enabled(true);
            });

            e.stopPropagation();
        });
    });


    $jQuery("#rule_board").click(function () {
        $jQuery(this).find(".selected").removeClass("selected");
        $findByKey("cmdRemoveSelected", function () {
            this.set_enabled(false);
        });

        $jQueryByKey("CollectionID", function () {
            $jQuery(this).val(0);
            console.log($jQuery(this).val(0));
        });

    });
});

