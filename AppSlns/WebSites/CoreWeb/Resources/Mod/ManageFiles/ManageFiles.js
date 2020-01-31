

function ClientBeforeUpload(source, e) {
    FSObject.$("[id$=txtDocument]").val('Text');
}


function CheckValidFileExtensions(source, arguments) {
    var isvalid = true;
    $findByKey("investorfile", function () {
        isvalid = this.validateExtensions();

    });
    arguments.IsValid = isvalid;
    var cvDocument = $getByKey("cvDocumentFileExt", function () {
        this.innerHTML = "Invalid file extension.";
        if (arguments.IsValid) {
            //this.style = "display:none;"
            this.style.display = "none";
        }
        else {
            //this.style = "display:block;"
            this.style.display = "block";
        }
    });


}

function validateFileUpload(source, e) {
    e.IsValid = false;
    
    var upload = $findByKey("fupDocument");
    var inputs = upload.getFileInputs();
    for (var i = 0; i < inputs.length; i++) {
        //check for empty string or invalid extension
        if (inputs[i].value != "") {
            e.IsValid = true;
            break;
        }
    }
}   
