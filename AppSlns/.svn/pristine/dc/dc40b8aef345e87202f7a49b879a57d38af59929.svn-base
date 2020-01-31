$jQuery = $telerik.$;

var myApi = null;
var isDownload = false;
var currentPage = null;
var _blankField = null;
var DocumentAcroFieldType =
    {
        FullName: "AAAA",
        CurrentDate: "AAAB",
        Signature: "AAAC",
        FirstName: "AAAD",
        MiddleName: "AAAE",
        LastName: "AAAF",
        FullSSN: "AAAG",
        LastFourDigitofSSN: "AAAH",
        DOB: "AAAI",
        PhoneNumber: "AAAJ",
        EmailAddress: "AAAK",
        Gender: "AAAL",
        AliasName: "AAAM",
        Address1: "AAAN",
        Address2: "AAAO",
        City: "AAAP",
        State: "AAAQ",
        PostalCode: "AAAR",
        County: "AAAS",
        Country: "AAAT",
        InstitutionName: "AAAU",
        BlankField: "AAAV"
    };

var DocumentAcroFieldName =
    {
        FullName: "FullName",
        CurrentDate: "CurrentDate",
        Signature: "Signature",
        FirstName: "FName",
        MiddleName: "MName",
        LastName: "LName",
        FullSSN: "FullSSN",
        DOB: "DOB",
        PhoneNumber: "PhoneNumber",
        EmailAddress: "EmailAddress",
        Gender: "Gender",
        AliasName: "aliasName",
        Address1: "Address1",
        Address2: "Address2",
        City: "City",
        State: "State",
        PostalCode: "PostalCode",
        County: "County",
        Country: "Country",
        InstitutionName: "InstitutionName",
        LastFourDigitofSSN: "LastFourDigitofSSN",
        BlankField: "BlankField"

    };

var DocumentFieldUiName =
    {
        FullName: "Full Name",
        CurrentDate: "Current Date",
        Signature: "Signature",
        FirstName: "FName",
        MiddleName: "MName",
        LastName: "LName",
        FullSSN: "Full SSN",
        DOB: "DOB",
        PhoneNumber: "Phone Number",
        EmailAddress: "Email Address",
        Gender: "Gender",
        AliasName: "alias Name",
        Address1: "Address 1",
        Address2: "Address 2",
        City: "City",
        State: "State",
        PostalCode: "Postal Code",
        County: "County",
        Country: "Country",
        InstitutionName: "Institution Name",
        LastFourDigitofSSN: "Last Four Digit of SSN",
        BlankField: "BlankField"
    };

function e_wcload() {
    // Get id   
    var pdfWebControlID = $jQuery("[id$=PdfWebControlDocument]").attr("id");
    // Get api instance
    myApi = new PdfWebControlApi(pdfWebControlID);
    currentPage = myApi.getPageViewed().getPageNumber();
    myApi.addEventListener(
        "viewChanged",
        function (evt) {
            fitPDFDocuments(true);
            if (evt.view.page) {
                currentPage = evt.view.page;
            }
        });

    myApi.addEventListener(
        "rendered",
        function () {
            fitPDFDocuments(true);
            checkForacroFields();
        });
}

//Function to fit the size of PDF documents in RADPDF document viewer.
function fitPDFDocuments(isFitReq) {
    if ((myApi.getView().zoom != 50 && myApi.getView().zoom != 100 && myApi.getView().zoom != 200) || isFitReq) {
        myApi.setView({ "zoom": "fit" });
    }
}

// After rendering the pdf file check if any of the acrofields are already added then provide delete buttons for those acrofields.
function checkForacroFields() {
    if (myApi) {
        var fullNameAcrofield = myApi.getField(DocumentAcroFieldName.FullName);
        var currentDateAcroField = myApi.getField(DocumentAcroFieldName.CurrentDate);
        var signatureAcroField = myApi.getField(DocumentAcroFieldName.Signature);
        if (fullNameAcrofield) {
            $jQuery("[id$=btnDelFullName]").show();
        }
        if (currentDateAcroField) {
            $jQuery("[id$=btnDelCurrentDate]").show();
        }
        if (signatureAcroField) {
            $jQuery("[id$=btnDelSignature]").show();
        }
    }
}

//Add new field on pdf file.
function AddNewField(sender, args, acroFieldCode) {
    if (myApi) {
        var acroFieldObjProperties = GetAcroFieldProperties(acroFieldCode);
        if (acroFieldObjProperties) {
            var _fieldWidth = 500;
            var _fieldHeight = 100;
            var _fieldXAxis = acroFieldObjProperties.fieldXAxis;
            var _fieldYAxis = acroFieldObjProperties.fieldYAxis;
            var _fieldName = acroFieldObjProperties.fieldName;
            var _fieldUiName = acroFieldObjProperties.fieldUiName;

            //var fFileName = myApi.getField(_fieldName);
            // if (fFileName == null) {
            //Adding ne acroField on current page of pdf File
            //Create new object.
            var acroFieldsObj = myApi.getPage(myApi.getView().page).addObject(myApi.ObjectType.TextField, _fieldXAxis, _fieldYAxis, _fieldWidth, _fieldHeight);
            var customData = "";
            switch (acroFieldCode) {
                case DocumentAcroFieldType.FullName:
                    customData = "FullName";
                    break;
                case DocumentAcroFieldType.CurrentDate:
                    customData = "CurrentDate";
                    break;
                case DocumentAcroFieldType.Signature:
                    customData = "Signature";
                    break;
            }
            SetAcroFieldObjectProp(acroFieldsObj, _fieldName, acroFieldObjProperties.color, customData);

            var addedAcroFields = $jQuery("[id$=hdnAddedAcroFields]").val();
            if (addedAcroFields == "") {
                addedAcroFields = acroFieldCode;
            }
            else {
                addedAcroFields = addedAcroFields + "," + acroFieldCode;
            }

            $jQuery("[id$=hdnAddedAcroFields]").val(addedAcroFields);

            switch (acroFieldCode) {
                case DocumentAcroFieldType.FullName:
                    $jQuery("[id$=btnDelFullName]").show();
                    break;
                case DocumentAcroFieldType.CurrentDate:
                    $jQuery("[id$=btnDelCurrentDate]").show();
                    break;
                case DocumentAcroFieldType.Signature:
                    $jQuery("[id$=btnDelSignature]").show();
                    break;
            }
            //}
            //else {
            //    alert(_fieldUiName + ' field has already added. Please remove the existing field to add new field.');
            //    //return false;
            //}
        }
    }
    return false;
}

function GetAcroFieldProperties(acroFieldCode) {

    var acroFieldPositionProperties = {};
    switch (acroFieldCode) {
        case DocumentAcroFieldType.FullName:
            acroFieldPositionProperties.color = 'red';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 50;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.FullName;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.FullName;
            break;
        case DocumentAcroFieldType.CurrentDate:
            acroFieldPositionProperties.color = 'blue';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 170;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.CurrentDate;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.CurrentDate;
            break;
        case DocumentAcroFieldType.Signature:
            acroFieldPositionProperties.color = 'green';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.Signature;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.Signature;
            break;
        case DocumentAcroFieldType.FirstName:
            acroFieldPositionProperties.color = 'Gray';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.FirstName;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.FirstName;
            break;
        case DocumentAcroFieldType.MiddleName:
            acroFieldPositionProperties.color = 'yellow';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.MiddleName;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.MiddleName;
            break;
        case DocumentAcroFieldType.LastName:
            acroFieldPositionProperties.color = 'pink';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.LastName;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.LastName;
            break;
        case DocumentAcroFieldType.FullSSN:
            acroFieldPositionProperties.color = 'maroon';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.FullSSN;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.FullSSN;
            break;
        case DocumentAcroFieldType.DOB:
            acroFieldPositionProperties.color = 'navy';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.DOB;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.DOB;
            break;
        case DocumentAcroFieldType.PhoneNumber:
            acroFieldPositionProperties.color = 'teal';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.PhoneNumber;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.PhoneNumber;
            break;
        case DocumentAcroFieldType.EmailAddress:
            acroFieldPositionProperties.color = 'Purple';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.EmailAddress;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.EmailAddress;
            break;
        case DocumentAcroFieldType.Gender:
            acroFieldPositionProperties.color = 'Silver';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.Gender;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.Gender;
            break;
        case DocumentAcroFieldType.AliasName:
            acroFieldPositionProperties.color = 'Fuchsia';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.AliasName;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.AliasName;
            break;
        case DocumentAcroFieldType.Address1:
            acroFieldPositionProperties.color = 'Olive';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.Address1;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.Address1;
            break;
        case DocumentAcroFieldType.Address2:
            acroFieldPositionProperties.color = 'Lime';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.Address2;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.Address2;
            break;
        case DocumentAcroFieldType.City:
            acroFieldPositionProperties.color = 'Aqua';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.City;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.City;
            break;
        case DocumentAcroFieldType.State:
            acroFieldPositionProperties.color = 'RosyBrown';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.State;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.State;
            break;
        case DocumentAcroFieldType.PostalCode:
            acroFieldPositionProperties.color = 'Orange';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.PostalCode;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.PostalCode;
            break;
        case DocumentAcroFieldType.County:
            acroFieldPositionProperties.color = 'LightCoral';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.County;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.County;
            break;
        case DocumentAcroFieldType.Country:
            acroFieldPositionProperties.color = 'Gold';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.Country;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.Country;
            break;
        case DocumentAcroFieldType.InstitutionName:
            acroFieldPositionProperties.color = 'Brown';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.InstitutionName;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.InstitutionName;
            break;
        case DocumentAcroFieldType.LastFourDigitofSSN:
            acroFieldPositionProperties.color = 'SkyBlue';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = DocumentAcroFieldName.LastFourDigitofSSN;
            acroFieldPositionProperties.fieldUiName = DocumentFieldUiName.LastFourDigitofSSN;
            break;
        case DocumentAcroFieldType.BlankField:
            _blankField = "blankField" + "_" + Math.floor(Math.random() * 100);
            acroFieldPositionProperties.color = 'Tan';
            acroFieldPositionProperties.fieldXAxis = 300;
            acroFieldPositionProperties.fieldYAxis = 290;
            acroFieldPositionProperties.fieldName = _blankField;
            acroFieldPositionProperties.fieldUiName = _blankField;

    }

    return acroFieldPositionProperties;
}

function SetAcroFieldObjectProp(acroFieldObj, fieldName, color, customData) {
    acroFieldObj.setProperties(
        {
            "resizable": true,
            "opacity": 50,
            "stylable": false,
            "deletable": true,
            "customData": customData,
            "name": fieldName,
            "font":
            {
                "name": "Arial",
                "color": "black",
                "size": 12,
                "bold": false,
                "italic": false,
                "underline": false
            },
            "hideFocusOutline": false,
            "fillColor": color,
            "hideHighlight": true
        });
}

//Delete added field
function DeleteField(sender, args, acroFieldCode) {
    if (myApi) {
        //myApi.getFields()[2].getProperties().customData
        //myApi.getFields()[3].getProperties().name
        var getAllAcroField = myApi.getFields();
        $jQuery.each(getAllAcroField, function (index, value) {
            var _fieldName = "";
            switch (acroFieldCode) {
                case DocumentAcroFieldType.FullName:
                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.FullName) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.CurrentDate:
                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.CurrentDate) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.Signature:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.Signature) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                    
                case DocumentAcroFieldType.FirstName:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.FirstName) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.LastName:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.LastName) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.MiddleName:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.MiddleName) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.FullSSN:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.FullSSN) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.DOB:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.DOB) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.PhoneNumber:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.PhoneNumber) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.EmailAddress:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.EmailAddress) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.Gender:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.Gender) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.AliasName:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.AliasName) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.Address1:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.Address1) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.Address2:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.Address2) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.City:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.City) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.State:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.State) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.PostalCode:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.PostalCode) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.County:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.County) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.Country:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.Country) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.InstitutionName:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.InstitutionName) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.LastFourDigitofSSN:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != DocumentAcroFieldName.LastFourDigitofSSN) {
                        _fieldName = "";
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
                case DocumentAcroFieldType.BlankField:

                    if (value.getProperties().customData != "")
                        _fieldName = value.getProperties().customData
                    else {
                        _fieldName = value.getProperties().name;
                    }
                    if (_fieldName != _blankField) {
                        if (_fieldName.indexOf("blankField") >= 0) {
                            ResetAcroFieldObjectProp(value);
                            value.deleteOption();
                            value.deleteObject();
                        }
                        else {
                            _fieldName = "";
                        }
                    }
                    else {
                        ResetAcroFieldObjectProp(value);
                        value.deleteOption();
                        value.deleteObject();
                    }
                    break;
            }

            if (_fieldName != "") {
                var fFileName = myApi.getField(_fieldName);
                if (fFileName == null) { }
                else {
                    fFileName.deleteObject();
                }
            }

        });

        switch (acroFieldCode) {
            case DocumentAcroFieldType.FullName:
                $jQuery("[id$=btnDelFullName]").hide();
                break;
            case DocumentAcroFieldType.CurrentDate:
                $jQuery("[id$=btnDelCurrentDate]").hide();
                break;
            case DocumentAcroFieldType.Signature:
                $jQuery("[id$=btnDelSignature]").hide();
        }

        //var _fieldName;
        //var _fieldUiName;

        //switch (acroFieldCode) {
        //    case DocumentAcroFieldType.FullName:
        //        _fieldName = DocumentAcroFieldName.FullName;
        //        _fieldUiName = DocumentFieldUiName.FullName;
        //        break;
        //    case DocumentAcroFieldType.CurrentDate:

        //        _fieldName = DocumentAcroFieldName.CurrentDate;
        //        _fieldUiName = DocumentFieldUiName.CurrentDate;
        //        break;
        //    case DocumentAcroFieldType.Signature:
        //        _fieldName = DocumentAcroFieldName.Signature;
        //        _fieldUiName = DocumentFieldUiName.Signature;
        //}

        //var fFileName = myApi.getField(_fieldName);
        //if (fFileName == null) { alert('Object already deleted'); }
        //else {
        //    // fFileName.deleteObject();
        //    switch (acroFieldCode) {
        //        case DocumentAcroFieldType.FullName:
        //            $jQuery("[id$=btnDelFullName]").hide();
        //            //var btnFullName = $find($jQuery("[id$=btnFullName]").attr('id'));
        //            //btnFullName.set_enabled(true);
        //            break;
        //        case DocumentAcroFieldType.CurrentDate:
        //            $jQuery("[id$=btnDelCurrentDate]").hide();
        //            //var btnCurrentDate = $find($jQuery("[id$=btnCurrentDate]").attr('id'));
        //            //btnCurrentDate.set_enabled(true);
        //            break;
        //        case DocumentAcroFieldType.Signature:
        //            //$jQuery("[id$=btnDelSignature]").hide();

        //            //var i = 1;
        //            //while (i = 1) {
        //            //    var signatureAcroField = myApi.getField(DocumentAcroFieldName.Signature);
        //            //    signatureAcroField.deleteObject();
        //            //    //  
        //            //    //var btnSignature = $find($jQuery("[id$=btnSignature]").attr('id'));

        //            //    //btnSignature.set_enabled(true);
        //            //    var fFileName = myApi.getField(DocumentAcroFieldName.Signature);
        //            //    if (fFileName == null)
        //            //        break;
        //            //}
        //    }
        //}
    }
}

function SavePdfChanges() {
    if (myApi) {
        if (myApi.saveAndWait()) {
            alert("Document saved successfully. System will close this popup window.");
        }
        else {
            alert("An error occurred while saving PDF document. Please try again or Contact system administrator.");
            return false;
        }
    }
    else {
        return false;
    }
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function CloseDocumentPreviewWindow(isSaved) {
    var oArg = {};
    if (isSaved == true || isSaved == "True" || isSaved == "true") {
        oArg.pdfAcroFields = $jQuery("[id$=hdnAddedAcroFields]").val();
        oArg.IsDocumentSaved = true;
    }
    else {
        oArg.pdfAcroFields = "";
        oArg.IsDocumentSaved = false;
    }
    var oWnd = GetRadWindow();
    oWnd.Close(oArg);
}


function ResetAcroFieldObjectProp(acroFieldObj) {
    acroFieldObj.setProperties(
        {
            "resizable": true,
            "opacity": 50,
            "stylable": false,
            "deletable": true,
            "customData": "",
            "name": "",
            "font":
            {
                "name": "Arial",
                "color": "black",
                "size": 12,
                "bold": false,
                "italic": false,
                "underline": false
            },
            "hideFocusOutline": false,
            "fillColor": "",
            "hideHighlight": false
        });
}

//UAT 4001 Add additional fields to be able to be mapped for rotation complete document 

function AddField(sender, args, acroFieldCode) {
    if (myApi) {
  
        var acroFieldObjProperties = GetAcroFieldProperties(acroFieldCode);
        if (acroFieldObjProperties) {
            var _fieldWidth = 500;
            var _fieldHeight = 100;
            var _fieldXAxis = acroFieldObjProperties.fieldXAxis;
            var _fieldYAxis = acroFieldObjProperties.fieldYAxis;
            var _fieldName = acroFieldObjProperties.fieldName;
            var _fieldUiName = acroFieldObjProperties.fieldUiName;

            //var fFileName = myApi.getField(_fieldName);
            // if (fFileName == null) {
            //Adding ne acroField on current page of pdf File
            //Create new object.
            var acroFieldsObj = myApi.getPage(myApi.getView().page).addObject(myApi.ObjectType.TextField, _fieldXAxis, _fieldYAxis, _fieldWidth, _fieldHeight);
            var customData = "";
            switch (acroFieldCode) {
                case DocumentAcroFieldType.FullName:
                    customData = "FullName";
                    break;
                case DocumentAcroFieldType.CurrentDate:
                    customData = "CurrentDate";
                    break;
                case DocumentAcroFieldType.Signature:
                    customData = "Signature";
                    break;
                case DocumentAcroFieldType.FirstName:
                    customData = "FName";
                    break;
                case DocumentAcroFieldType.MiddleName:
                    customData = "MName";
                    break;
                case DocumentAcroFieldType.LastName:
                    customData = "LName";
                    break;
                case DocumentAcroFieldType.FullSSN:
                    customData = "FullSSN";
                    break;
                case DocumentAcroFieldType.DOB:
                    customData = "DOB";
                    break;
                case DocumentAcroFieldType.PhoneNumber:
                    customData = "PhoneNumber";
                    break;
                case DocumentAcroFieldType.EmailAddress:
                    customData = "EmailAddress";
                    break;
                case DocumentAcroFieldType.Gender:
                    customData = "Gender";
                    break;
                case DocumentAcroFieldType.AliasName:
                    customData = "aliasName";
                    break;
                case DocumentAcroFieldType.Address1:
                    customData = "Address1";
                    break;
                case DocumentAcroFieldType.Address2:
                    customData = "Address2";
                    break;
                case DocumentAcroFieldType.City:
                    customData = "City";
                    break;
                case DocumentAcroFieldType.State:
                    customData = "State";
                    break;
                case DocumentAcroFieldType.PostalCode:
                    customData = "PostalCode";
                    break;
                case DocumentAcroFieldType.County:
                    customData = "County";
                    break;
                case DocumentAcroFieldType.Country:
                    customData = "Country";
                    break;
                case DocumentAcroFieldType.InstitutionName:
                    customData = "InstitutionName";
                    break;
                case DocumentAcroFieldType.LastFourDigitofSSN:
                    customData = "LastFourDigitofSSN";
                    break;
                case DocumentAcroFieldType.BlankField:
                    customData = _blankField;
                    break;
            }
            SetAcroFieldObjectProp(acroFieldsObj, _fieldName, acroFieldObjProperties.color, customData);

            var addedAcroFields = $jQuery("[id$=hdnAddedAcroFields]").val();
            if (addedAcroFields == "") {
                addedAcroFields = acroFieldCode;
            }
            else {
                addedAcroFields = addedAcroFields + "," + acroFieldCode;
            }

            $jQuery("[id$=hdnAddedAcroFields]").val(addedAcroFields);

            //switch (acroFieldCode) {
            //    case DocumentAcroFieldType.FullName:
            //        $jQuery("[id$=btnDelFullName]").show();
            //        break;
            //    case DocumentAcroFieldType.CurrentDate:
            //        $jQuery("[id$=btnDelCurrentDate]").show();
            //        break;
            //    case DocumentAcroFieldType.Signature:
            //        $jQuery("[id$=btnDelSignature]").show();
            //        break;
            //}
            //}
            //else {
            //    alert(_fieldUiName + ' field has already added. Please remove the existing field to add new field.');
            //    //return false;
            //}
        }
    }
    return false;
}

