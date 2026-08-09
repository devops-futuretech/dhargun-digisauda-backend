$(function () {

    TrimInputBlur();
    //GetAndUpdateClientTimezone();

    //$('form :input:enabled:visible:not([readonly]):first').focus();
    $("form").find(':text, textarea').filter(":visible:enabled").first().focus();
    $(".accord").click(function () {
        $(this).toggleClass("expanded");
    });
    var currentMenu = $(".currentMenu");
    if (currentMenu.length > 0) {
        $(".sidebar .navigation li").removeClass('active');
        $(".sidebar .navigation li." + currentMenu.val()).addClass('active');

        if (currentMenu.val().toLowerCase().indexOf('tp') > -1) {
            $("#tp").addClass('active');
            $("#tp .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('master') > -1) {
            $("#master").addClass('active');
            $("#master .hidden-ul").css({ 'display': "block" });
        }

        //if (currentMenu.val().toLowerCase().indexOf('pricing') > -1) {
        //    $("#pricing").addClass('active');
        //    $("#pricing .hidden-ul").css({ 'display': "block" });
        //}
       
        if (currentMenu.val().toLowerCase().indexOf('transaction') > -1) {
            $("#transaction").addClass('active');
            $("#transaction .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('dealers') > -1) {
            $("#dealers").addClass('active');
            $("#dealers .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('role') > -1) {
            $("#role").addClass('active');
            $("#role .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('reverseauction') > -1) {
            $("#reverseauction").addClass('active');
            $("#reverseauction .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('tpdiscount') > -1) {
            $("#tpdiscount").addClass('active');
            $("#tpdiscount .hidden-ul").css({ 'display': "block" });
        }
        if (currentMenu.val().toLowerCase().indexOf('ra2Scheme') > -1) {
            $("#ra2Scheme").addClass('active');
            $("#ra2Scheme .hidden-ul").css({ 'display': "block" });
        }
        

        if (currentMenu.val().toLowerCase().indexOf('premium') > -1) {
            $("#premium").addClass('active');
            $("#premium .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('manageuser') > -1) {
            $("#manageuser").addClass('active');
            $("#manageuser .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('configuration') > -1) {
            $("#configuration").addClass('active');
            $("#configuration .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('salestourplan') > -1) {
            $("#salestourplan").addClass('active');
            $("#salestourplan .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('specialityfat') > -1) {
            $("#specialityfat").addClass('active');
            $("#specialityfat .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('sauda') > -1) {
            $("#sauda").addClass('active');
            $("#sauda .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('target') > -1) {
            $("#target").addClass('active');
            $("#target .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('updates') > -1) {
            $("#updates").addClass('active');
            $("#updates .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('sfquantitylimit') > -1) {
            $("#sfquantitylimit").addClass('active');
            $("#sfquantitylimit .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('publish') > -1) {
            $("#publish").addClass('active');
            $("#publish .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('report') > -1) {
            $("#report").addClass('active');
            $("#report .hidden-ul").css({ 'display': "block" });
        }
        
        if (currentMenu.val().toLowerCase().indexOf('finalprice') > -1) {
            $("#finalprice").addClass('active');
            $("#finalprice .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('support') > -1) {
            $("#support").addClass('active');
            $("#support .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('ra2') > -1) {
            $("#ra2").addClass('active');
            $("#ra2 .hidden-ul").css({ 'display': "block" });
        }
        if (currentMenu.val().toLowerCase().indexOf('racustomergroup') > -1) {
            $("#racustomergroup").addClass('active');
            $("#racustomergroup .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('ravolumediscount') > -1) {
            $("#ravolumediscount").addClass('active');
            $("#ravolumediscount .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('raschemediscount') > -1) {
            $("#raschemediscount").addClass('active');
            $("#raschemediscount .hidden-ul").css({ 'display': "block" });
        }
        if (currentMenu.val().toLowerCase().indexOf('raComplaintManagement') > -1) {
            $("#raschemediscount").addClass('active');
            $("#raschemediscount .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('raskudiscount') > -1) {
            $("#raskudiscount").addClass('active');
            $("#raskudiscount .hidden-ul").css({ 'display': "block" });
        }
        if (currentMenu.val().toLowerCase().indexOf('cms') > -1) {
            $("#cms").addClass('active');
            $("#cms .hidden-ul").css({ 'display': "block" });
        }
        if (currentMenu.val().toLowerCase().indexOf('raComplaintManagementMenu') > -1) {
            $("#raComplaintManagementMenu").addClass('active');
            $("#raComplaintManagementMenu .hidden-ul").css({ 'display': "block" });
        }

        if (currentMenu.val().toLowerCase().indexOf('saudaconditionalbooking') > -1) {
            $("#saudaconditionalbooking").addClass('active');
            $("#saudaconditionalbooking .hidden-ul").css({ 'display': "block" });
        }
    }

    // To Make show and hide filter
    $('#chkKendoFilterShowHide').change(function () {
        filterShowHide();
    });

    // To Make show and hide filter
    $('#chkKendoFilterShowHideForSpecificGrid').change(function () {
        filterShowHideForSpecificGrid();
    });

    // To Make filer checkbox checked on clicking its text
    $(".spanClassFilter").on("click", function (event) {
        filterShowHide();
    });

    // To Make checkbox checked on clicking its text
    $(".spanClassCheckbox").on("click", function (event) {
        var target = $(event.target);
        ToggleCheckboxSpan(target);
    });

    // To Make radiobutton checked on clicking its text
    $(".spanClassRadio").on("click", function (event) {
        var target = $(event.target);
        ToggleRadioButtonSpan(target);
    });

    //Method to change text to uppercase
    $("#CaptchaInputText").on("change", function (event) {
        this.value = this.value.toUpperCase();
    });

    $("#CaptchaInputText").keypress(function (event) {
        this.value = this.value.toUpperCase();
    });


    //Trigger submit button when click the enter key
    $('input[type="text"],input[type="password"]').keypress(function (e) {
        var code = e.keyCode || e.which;
        if (code === emamiGlobal.constant.EnterKeyCode) {
            e.preventDefault();
            formSubmit();
        }
    });

    //Allow Decimal Values
    $('.decimalValue').keypress(function (event) {
        if (((event.which !== 46 || (event.which === 46 && $(this).val() === '')) ||
            $(this).val().indexOf('.') !== -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });
    //.on('paste', function (event) {
    //    event.preventDefault();
    //});

    ////Allow Decimal Values with 3 decimal places  
    $('.decimalValueThreeDecimalDigits').keypress(function (event) {
        var $this = $(this);
        if ((event.which !== 46 || $this.val().indexOf('.') !== -1) &&
            ((event.which < 48 || event.which > 57) &&
                (event.which !== 0 && event.which !== 8))) {
            event.preventDefault();
        }

        var text = $(this).val();
        if ((event.which === 46) && (text.indexOf('.') === -1)) {
            setTimeout(function () {
                if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                    $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
                }
            }, 1);
        }

        if ((text.indexOf('.') !== -1) &&
            (text.substring(text.indexOf('.')).length > 3) &&
            (event.which !== 0 && event.which !== 8) &&
            ($(this)[0].selectionStart >= text.length - 2)) {
            event.preventDefault();
        }
    });


    //Allow Positive & Negative Decimal Values
    $('.negativeDecimalValue').keypress(function (event) {
        var regex = /^-?\d*\.?\d{0,6}$/;
        var InputValue = String.fromCharCode(!event.keyCode ? event.which : event.keyCode);
        var key = $(this).val();
        key = key + InputValue;
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });
    //    .on('paste', function (event) {
    //    event.preventDefault();
    //});
});

//Method to show success message
function ShowSuccessMessage(message, divName) {
    $("#dvAlert").removeClass().addClass("alertCont alert alert-success");
    $("#dvAlert").fadeIn();

    if (divName !== "" && typeof (divName) !== "undefined") {
        $("#" + divName).html(message);
        $("#" + divName).css("display", "block");
        setTimeout(function () {
            $("#" + divName).fadeOut('slow');
        }, 10000);
    } else {
        $("#dvAlert").html(message);
        setTimeout(function () {
            $('#dvAlert').fadeOut('slow');
        }, 10000);
    }

}

//Method to show error message
function ShowErrorMessage(message, divName) {
    $("#dvAlert").removeClass().addClass("alertCont alert alert-danger");
    $("#dvAlert").fadeIn();

    if (divName !== "" && typeof (divName) !== "undefined") {
        $("#" + divName).html(message);
        $("#" + divName).css("display", "block");
        setTimeout(function () {
            $("#" + divName).fadeOut('slow');
        }, 10000);
    } else {
        $("#dvAlert").html(message);
        setTimeout(function () {
            $('#dvAlert').fadeOut('slow');
        }, 10000);
    }
}

function ShowSuccessMessageLeftAlign(message, divName) {
    $("#dvAlert").removeClass().addClass("alertContLeftAlign alert alert-success");
    $("#dvAlert").fadeIn();

    if (divName !== "" && typeof (divName) !== "undefined") {
        $("#" + divName).html(message);
        $("#" + divName).css("display", "block");
        setTimeout(function () {
            $("#" + divName).fadeOut('slow');
        }, 10000);
    } else {
        $("#dvAlert").html(message);
        setTimeout(function () {
            $('#dvAlert').fadeOut('slow');
        }, 10000);
    }
}

function ShowErrorMessageLeftAlign(message, divName) {
    $("#dvAlert").removeClass().addClass("alertContLeftAlign alert alert-danger");
    $("#dvAlert").fadeIn();

    if (divName !== "" && typeof (divName) !== "undefined") {
        $("#" + divName).html(message);
        $("#" + divName).css("display", "block");
        setTimeout(function () {
            $("#" + divName).fadeOut('slow');
        }, 10000);
    } else {
        $("#dvAlert").html(message);
        setTimeout(function () {
            $('#dvAlert').fadeOut('slow');
        }, 10000);
    }
}

//More than one message bind
function ErrorSuccessMessageBind(errorMsg, message) {
    if (errorMsg === "") {
        errorMsg = message
    }
    else {
        errorMsg = errorMsg.concat("<br><br>" + message);
    }
    return errorMsg;
}
//Method to check url validation
function isValidUrl(url) {
    var myVariable = url;
    if (/(^|\s)((https?:\/\/)?[\w-]+(\.[\w-]+)+\.?(:\d+)?(\/\S*)?)/.test(myVariable)) {
        return 1;
    } else {
        return -1;
    }
}

//Method to check is number or not
function isNumber(e) {
    //if (e.which > 95 && e.which < 106 || e.which === 9 || e.which === 13)
    //    return true;

    //if the letter is not digit then display error and don't type anything
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        e.preventDefault();
    }
}


//Method to check is alphabet
function isAlphabets(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 8 || charCode == 9 || e.which == 13)
            return true;
        else
            return false;
    }
    catch (err) {
        //(charCode > 64 && charCode < 91) ||
        //alert(err.Description);
    }
}

// Function to check for numeric
function isNumeric(e) {
    var evt = (e) ? e : window.event;
    var key = (evt.keyCode) ? evt.keyCode : evt.which;
    if (key != null) {
        key = parseInt(key, 10);
        if ((key < 48 || key > 57) && (key < 96 || key > 105)) {
            if (!IsUserFriendlyChar(key, "Decimals")) {
                return false;
            }
        }
        else {
            if (evt.shiftKey) {
                return false;
            }
        }
    }
    return true;
}

// Function to check for user friendly keys
function IsUserFriendlyChar(val, step) {
    // Backspace, Tab, Enter, Insert, and Delete  
    if (val == 8 || val == 9 || val == 13 || val == 45 || val == 46) {
        return true;
    }
    // Ctrl, Alt, CapsLock, Home, End, and Arrows  
    if ((val > 16 && val < 21) || (val > 34 && val < 41)) {
        return true;
    }
    if (step == "Decimals") {
        if (val == 190 || val == 110) {  //Check dot key code should be allowed
            return true;
        }
    }
    // The rest  
    return false;
}

//Method to check is alphaNumeric or not
function isAlphaNumeric(e) {

    if (e.which == 8)
        return true;

    var regex = new RegExp("^[a-zA-Z0-9]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    }

    e.preventDefault();
    return false;
}

function isAlphaNumeric2(str) {
    if (/[^a-zA-Z0-9 ]/.test(str)) {
        return false;
    }
    return true;
}

function AllowOnlyAlphaNumeric(e) {
    var key = e.keyCode;
    if (!((key == 8) || (key == 9) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105)) || (event.shiftKey && (event.keyCode >= 48 && event.keyCode <= 57))) {
        e.preventDefault();
    }
}

//Method to check is number or not
function isFocus(e) {
    if (e.which === 9)
        return true;
    else
        return false;
}


//Method to check pincode validation
function PincodeValidation(pincodeValue, errorMsg) {
    var pincodeFlag = false;
    $("#txtPincode-error").css("display", "none");
    if (pincodeValue === "") return true;
    for (var i = 0; i < pincodeValue.length; i++) {
        var ch = pincodeValue.charAt(i);
        if (ch !== '0')
            pincodeFlag = true;
    }
    if (!pincodeFlag) {
        setTimeout(function () {
            $("#txtPincode-error").css("display", "block");
            $("#txtPincode-error").text(errorMsg);
        }, 500);
    }
    return pincodeFlag;
}


function PhoneNoValidation(phoneNoValue, errorMsg) {
    var phoneNoFlag = false;
    $("#txtMobileNumber-error").css("display", "none");
    if (phoneNoValue === "") return true;
    var ch = phoneNoValue.charAt(0);
    mobileString = emamiGlobal.constant.mobileNoValidadtionValue;
    if (mobileString.indexOf(ch) >= 0)
        phoneNoFlag = true;
    else
        phoneNoFlag = false;

    if (!phoneNoFlag) {
        setTimeout(function () {
            $("#txtMobileNumber-error").css("display", "block");
            $("#txtMobileNumber-error").text(errorMsg);
        }, 500);
    }
    return phoneNoFlag;
}

//Method to check alphabetic or not
function isString(stringValue) {
    return stringValue.match("^[a-zA-Z\(\)]+$");
}


//Methdo to trim the input value
function TrimInputBlur() {
    $('input[type=text]:visible,input[type=email]:visible,input[type=number]:visible,textarea:visible')
        .not('input[readonly=readonly],input[disabled=disabled]').on('blur', function () {
            var inputvalue = $.trim($(this).val());
            $(this).val(inputvalue);
        });
}

//Methdo to trim the input value
function TrimInput() {
    $('input[type=text]:visible,input[type=email]:visible,input[type=number]:visible,textarea:visible')
        .not('input[readonly=readonly],input[disabled=disabled]').each(function () {
            var inputvalue = $.trim($(this).val());
            $(this).val(inputvalue);
        });
}


//To Make Radio button checked on clicking its text
function ToggleRadioButtonSpan(target) {
    if (target.prev().is('input:radio') && target.prev().is(':enabled')) {
        if (!target.prev().prop("checked")) {
            target.prev().prop("checked", true).triggerHandler('click');
        }
    }
}



//To Make checkbox checked on clicking its text
function ToggleCheckboxSpan(target) {
    if (target.prev().prev().is('input:checkbox') && target.prev().prev().is(':enabled')) {
        if (!target.prev().prev().prop("checked")) {
            target.prev().prev().prop("checked", true).triggerHandler('click');
        }
        else if (target.prev().prev().prop("checked")) {
            target.prev().prev().prop("checked", false).triggerHandler('click');
        }
    }
}

//Select All Checkbox
function onSelectAllClickEvent(className, chkBoxName) {
    if ($('#' + chkBoxName).is(':checked')) {
        $("." + className).prop('checked', true);
    }
    else {
        $("." + className).prop('checked', false);
    }
}

//Select All Checkbox
function onSelectClickEvent(className, chkBoxName) {
    var a = $("input[type='checkbox']." + className);
    if (a.length == a.filter(":checked").length) {
        $("#" + chkBoxName).prop('checked', true);
    }
    else {
        $("#" + chkBoxName).prop('checked', false);
    }
}

//Select All Checkbox
function onDisableAllCheckboxClickEvent(className, chkBoxName, chkboxAll) {
    $("#" + chkboxAll).prop('checked', false);
    if ($('#' + chkBoxName).is(':checked')) {
        $("." + className).attr("disabled", true);
        $("." + className).prop('checked', false);
    }
    else {
        $("." + className).attr("disabled", false);
    }
}

//Method to show/hide filter
function filterShowHide() {
    if ($(".k-filter-row").length) {
        var gridId = $('.k-grid').length ? $('.k-grid').attr('id') : "";
        if ($(".k-filter-row").is(":visible")) {
            $(".k-filter-row").hide();
            $("#chkKendoFilterShowHide").prop("checked", false);
            if (gridId) {
                var grid = $("#".concat(gridId)).data("kendoGrid");
                grid.filterable = false;
                grid.dataSource.filter("");
                grid.refresh();
            }
        } else {
            $(".k-filter-row").show();
            $("#chkKendoFilterShowHide").prop("checked", true);
        }
    }
}

function filterShowHideForSpecificGrid() {
    if ($(".k-filter-row").length) {
        var gridId = $('.k-grid').length ? $('.k-grid').attr('id') : "";
        if ($(".k-filter-row").is(":visible")) {
            $(".k-filter-row").hide();
            $("#chkKendoFilterShowHideForSpecificGrid").prop("checked", false);
            if (gridId) {
                var grid = $("#".concat(gridId)).data("kendoGrid");
                grid.filterable = false;
                grid.dataSource.filter("");
                grid.refresh();
            }
        } else {
            $(".k-filter-row").show();
            $("#chkKendoFilterShowHideForSpecificGrid").prop("checked", true);
        }
    }
}

//Method used to grid is empty then add empty row.
function SetEmptyRow(e) {
    
    if (!e.sender.dataSource.view().length) {
        $(".k-pager-refresh").hide();
        var colspan = e.sender.thead.find("th:visible").length,
            emptyRow = '<tr><td colspan="'.concat(colspan, '"class="emptyRow"> </td></tr>');
        e.sender.tbody.parent().end().html(emptyRow);
    }
}


//Method to filter 'contains' in kendo grid
function FilterContains() {
    var dropDowns = $(".k-filter-row .k-filtercell").find("[data-role=dropdownlist]");
    //select the first element as defined in the filterable configuration
    setTimeout(function () {
        for (var i = 0; i < dropDowns.length; i++) {
            if (typeof dropDowns.eq(i).data("kendoDropDownList") !== 'undefined') {
                //dropDowns.eq(i).data("kendoDropDownList").select(3); //select the first option
                //dropDowns.eq(i).data("kendoDropDownList").trigger("change"); //trigger the change		
                $(".k-filter-row").hide();
                //$(".k-filter-row").show();
                $("#chkKendoFilterShowHide").prop("checked", false);
                $("#chkKendoFilterShowHideForSpecificGrid").prop("checked", false);
            }
        }
    });

    //To set minimum value of Numeric filter as 0
    if ($(".k-grid-header .k-filter-row .k-filtercell").length) {
        var numbericFields = $(".k-grid-header .k-filter-row .k-filtercell").find("[data-role=numerictextbox]");
        if (numbericFields.length) {
            $.each(numbericFields, function (id, field) {
                if ($(field).length) {
                    var numerictextbox = $(field).data("kendoNumericTextBox");
                    numerictextbox.min(0);
                }
            });
        }
    }

    //To set DateFormat of KendoDatePicker filter  as "dd-MMM-yyyy"
    if ($(".k-grid-header .k-filter-row .k-filtercell").length) {
        var numbericFields = $(".k-grid-header .k-filter-row .k-filtercell").find("[data-role=datepicker]");
        if (numbericFields.length) {
            $.each(numbericFields, function (id, field) {
                if ($(field).length) {

                    var numerictextbox = $(field).data("kendoDatePicker");
                    numerictextbox.options.format = emamiGlobal.constant.DateFormat;
                }
            });
        }
    }

    var gridId = $('.k-grid').length ? $('.k-grid').attr('id') : "";
    var grid = $("#".concat(gridId)).data("kendoGrid");
    grid.thead.kendoTooltip({
        filter: "th",
        content: function (e) {
            var target = e.target;
            if (target.text().trim() == "") {
                e.hide();
            }
            return target.text();
        }
    });

    //grid.tbody.kendoTooltip({        
    //    filter: "td",
    //    show: function (e) {            
    //        if (this.content.text() != "") {
    //            $('[role="tooltip"]').css("visibility", "visible");
    //        }
    //    },
    //    hide: function () {           
    //        $('[role="tooltip"]').css("visibility", "hidden");
    //    },
    //    content: function (e) {            
    //        var element = e.target[0];
    //        if (element.offsetWidth < element.scrollWidth) {
    //            return e.target.text();
    //        } else {
    //            return "";
    //        }
    //    }
    //});
}

//Recheck in grid databound Event
function checkSelectedCheckBoxItems(hiddenId, rowCheckBoxClass, masterCheckBoxClass) {
    $('input:checkbox.' + rowCheckBoxClass).each(function () {
        var selectedIds = $("#" + hiddenId).val().split(",");
        if ($.inArray(this.value, selectedIds) > -1) {

            this.checked = true;
        }
        else {
            this.checked = false;
        }
    });
    checkAllMaster(masterCheckBoxClass, rowCheckBoxClass);
}


//Recheck in grid databound Event
function checkChildSelectedCheckBoxItems(hiddenId, rowCheckBoxClass, masterCheckBoxClass) {
    $('input:checkbox.' + rowCheckBoxClass).each(function () {
        var selectedIds = $("#" + hiddenId).val().split(",");
        if ($.inArray(this.value, selectedIds) > -1) {

            this.checked = true;
        }
        else {
            this.checked = false;
        }
    });
    checkAllMaster(masterCheckBoxClass, rowCheckBoxClass);
}

function disableExpandCollapseColumn(hiddenId) {

    $(".k-hierarchy-cell").hide();
    $(".k-hierarchy-col").remove();

    $('input:checkbox.' + "chkbxAllDealer").on('change', function () {
        //var checkedvalue = $('input:checkbox.' + 'chkbxAllDealer:checked').val();
        $('input:checkbox.' + 'chkbxAllDealer').not(this).prop('checked', false);
        //$("#" + hiddenId).val() = checkedvalue;
    });

}


//Grid row check event
function findCheckedItemsSaveItToHidden(ele, id, hiddenId, masterCheckBoxId, checkBoxClass, lblSelectedCount) {
    var idList = [];
    if ($("#" + hiddenId).val() != "") {
        idList = $("#" + hiddenId).val().split(",");
    }
    var masterCheckBox = $(ele).is(':checked');
    var selectedCount = Number($("#" + lblSelectedCount).html());

    if (masterCheckBox) {
        idList.push(id);
        $("#" + lblSelectedCount).html(selectedCount + 1);
    }
    else {
        idList = jQuery.grep(idList, function (value) {
            return value != id;
        });
        if (selectedCount > 0)
            $("#" + lblSelectedCount).html(selectedCount - 1);
    }
    if (idList) {
        $("#" + hiddenId).val(idList.join(","));
    }
    else {
        $("#" + hiddenId).val("");
    }
    checkAllMaster(masterCheckBoxId, checkBoxClass);
}


// Master checkbox click event
function masterfindCheckedItemsSaveItToHidden(ele, hiddenId, checkBoxClass, lblSelectedCount) {

    var idList = [];
    if ($("#" + hiddenId).val()) {
        idList = $("#" + hiddenId).val().split(",");
    }
    var masterCheckBox = $(ele).is(':checked');
    var selectedCount = Number($("#" + lblSelectedCount).html());
    $('input:checkbox.' + checkBoxClass).each(function () {
        var rValue = this.value;
        if (masterCheckBox) {
            if ($.inArray(rValue, idList) > -1) {

            } else {
                idList.push(rValue);
                selectedCount = selectedCount + 1;
                $("#" + lblSelectedCount).html(selectedCount);
            }
        }
        else {
            if ($.inArray(rValue, idList) > -1) {
                idList = jQuery.grep(idList, function (cVaule) {
                    return cVaule != rValue;
                });
                if (selectedCount > 0) {
                    selectedCount = selectedCount - 1;
                    $("#" + lblSelectedCount).html(selectedCount);
                }
            }
        }
        this.checked = masterCheckBox;
        this['IsChecked'] = masterCheckBox;
    });

    if (idList) {
        $("#" + hiddenId).val(idList.join(","));
    }
    else {
        $("#" + hiddenId).val("");

    }
}


/*Method of master checkbox*/
function checkAllMaster(masterChk, chkClsName) {
    var isChecked = true;
    $('input:checkbox.' + chkClsName).each(function () {
        if (this.checked == false)
            isChecked = false;
    });
    $('input:checkbox#' + masterChk).each(function () {
        this.checked = isChecked;
    });
}

/*Method of master checkbox with checkbox checked status*/
function checkAllSelectedIdsMaster(ele, rowId, hdSelectedId, masterChk, chkClsName) {
    var data = [];
    if ($("#" + hdSelectedId).val() != "") {
        data = $("#" + hdSelectedId).val().split(",");
    }
    var blnCheckAll = $(ele).is(':checked');
    if (blnCheckAll) {
        data.push(rowId);
    } else {
        data = jQuery.grep(data, function (value) {
            return value != rowId;
        });
    }
    if (data != "") {
        $("#" + hdSelectedId).val(data.join(","));
    } else {
        $("#" + hdSelectedId).val("");
    }

    var isChecked = true;
    $('input:checkbox.' + chkClsName).each(function () {
        if (this.checked == false)
            isChecked = false;
    });
    $('input:checkbox#' + masterChk).each(function () {
        this.checked = isChecked;
    });
}

//Get selected item count
function GetSelectedItemCount(hiddenId, lblSelectedCount) {
    var selectedCount = $("#" + hiddenId).val().split(",").length;
    $("#" + lblSelectedCount).html(selectedCount);
}


function roundToTwo(n) {
    //return +(Math.round(num + "e+2") + "e-2");
    var negative = false;
    var digits = emamiGlobal.constant.DefaultDecimalPoints;
    if (digits === undefined) {
        digits = 0;
    }
    if (n < 0) {
        negative = true;
        n = n * -1;
    }
    var multiplicator = Math.pow(10, digits);
    n = parseFloat((n * multiplicator).toFixed(11));
    n = (Math.round(n) / multiplicator).toFixed(2);
    if (negative) {
        n = (n * -1).toFixed(2);
    }
    return n;
}

//Method to popup close event
function popupClose(hiddenId, controlId) {
    if ($("#".concat(hiddenId)).val()) {
        $("#".concat(controlId)).prop('checked', true);
    } else {
        $("#".concat(controlId)).prop('checked', false);
    }
}

// Method used to Export the grid data
function ExportToExcel(exportGridName) {
    window.kendo.ui.progress($("#div_Progress"), true);
    var grid = $("#" + exportGridName + "").data("kendoGrid");
    if (grid.columns[0].title == "Image") {
        grid.hideColumn(0);
    }
    grid.saveAsExcel();
    window.kendo.ui.progress($("#div_Progress"), false);
}

//Method to validate email or mobile number
function ValidateEmailOrMobileNumber() {
    var userName = $("#txtUsername").val();
    if (userName.length > 0) {
        if (isNaN(userName)) {
            var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
            var isValidEmail = emailReg.test(userName);

            if (!isValidEmail)
                ShowErrorMessage(emamiGlobal.errorMessage.EnterValidEmail);

            return isValidEmail;
        } else {
            var isValidMobileNumber;
            var ch = userName.charAt(0);
            mobileString = emamiGlobal.constant.mobileNoValidadtionValue;
            if (mobileString.indexOf(ch) >= 0 && userName.length === 10)
                isValidMobileNumber = true;
            else
                isValidMobileNumber = false;

            if (!isValidMobileNumber)
                ShowErrorMessage(emamiGlobal.errorMessage.EnterValidMobileNumber);

            return isValidMobileNumber;
        }
    }

}

function unCheckAllMasterAndChild(masterChk, chkClsName) {
    $('input:checkbox#' + masterChk).each(function () {
        this.checked = false;
    });
    $('input:checkbox.' + chkClsName).each(function () {
        this.checked = false;
    });
}

function clearCount(lblSelectedCount, selectedValues) {
    $("#" + lblSelectedCount).html("0");
    $("#" + selectedValues).val("");
}

function isFormChanged(isValue) {
    window.localStorage.setItem('frmChanged', isValue);
}

function isChange() {
    var data = window.localStorage.getItem('frmChanged');
    if (data === 'isChanged') {
        return true;
    } else {
        return false;
    }
}

function isFormChangeOrNot(url) {
    if (isChange()) {
        $("#btnDirtyOk").val(url);
        $("#confirmationPopoup").modal('show');
    } else {
        window.location.href = url;
    }
}

//Restrict Space On Password
function RestrictSpaceOnPassword() {
    if (event.keyCode == 32) {
        return false;
    }
    else {
        return true;
    }
}

//Check Given Input Contains only Zero
function CheckValueIsZero(input) {
    var isValid = true;
    var val = input.replace(/[\s\n\r]/g, "");
    if (val == 0 && val != '') {
        isValid = false;
    }
    return isValid;
}

//function AllowOnlyAlphaNumeric(e) {
//    if (e.shiftKey || e.ctrlKey || e.altKey) {
//        e.preventDefault();
//    } else {
//        var key = e.keyCode;
//        if (!((key == 9) || (key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
//            e.preventDefault();
//        }
//    }
//}


function isNumericExceptSubtractOrMinus(e) {
    var evt = (e) ? e : window.event;
    var key = (evt.keyCode) ? evt.keyCode : evt.which;
    if (key != null) {
        key = parseInt(key, 10);
        if ((key < 48 || key > 57) && (key < 96 || key > 105)) {
            if (!IsUserFriendlyCharExceptSubtractOrMinus(key, "Decimals")) {
                return false;
            }
        }
        else {
            if (evt.shiftKey) {
                return false;
            }
        }
    }
    return true;
}


function isNumericWithSubtractAndCopyAndPaste(e) {
    var evt = (e) ? e : window.event;
    var ctrlDown = evt.ctrlKey
    var key = (evt.keyCode) ? evt.keyCode : evt.which;
    if (key != null) {
        key = parseInt(key, 10);
        if ((ctrlDown && key === 67) || (ctrlDown && key === 86)) {
            return true;
        }
        else if ((key < 48 || key > 57) && (key < 96 || key > 105)) {
            if (!IsUserFriendlyCharExceptSubtractOrMinus(key, "Decimals")) {
                return false;
            }
        }
        else {
            if (evt.shiftKey) {
                return false;
            }
        }
    }
    return true;
}


// Function to check for user friendly keys
function IsUserFriendlyCharExceptSubtractOrMinus(val, step) {
    // Backspace, Tab, Enter, Insert, and Delete  
    if (val === 8 || val === 9 || val === 13 || val === 45 || val === 46) {
        return true;
    }
    // Ctrl, Alt, CapsLock, Home, End, and Arrows  
    if ((val > 16 && val < 21) || (val > 34 && val < 41)) {
        return true;
    }
    if (step === "Decimals") {
        if (val === 190 || val === 110) {  //Check dot key code should be allowed
            return true;
        }
        if (val === 109) {  //Check minus(-) key code should be allowed
            return true;
        }
    }
    // The rest  
    return false;
}


//Summary:
//  Click the "Select All" checkbox will get all the dropdown values and assign MultiSelect value() method.
//  UnClick the "Select All" checkbox will remove all the dropdown values.
//
//Parameters:
//  DropDownName or Id,SelectAll Checkbox Name
//
//Note:
//  Dropdown DataValueField name should be SkuId
function SkuMultiSelectSelectAll(DropDownName, CheckBoxName) {
    var dropdown = $("#" + DropDownName + "").data("kendoMultiSelect");
    if (dropdown) {
        var allIds = dropdown.dataSource.data().map(function (dataItem) { return dataItem.SkuId; });            //Select all ids from dropdown
        if ($("#" + CheckBoxName + "").is(":checked")) {
            dropdown.value(allIds);
        } else {
            dropdown.value([]);
        }
    }
}

//Summary:
//  Click the "Select All" checkbox will get all the dropdown values and assign MultiSelect value() method.
//  UnClick the "Select All" checkbox will remove all the dropdown values.
//
//Parameters:
//  DropDownName or Id,SelectAll Checkbox Name
//
//Note:
//  Dropdown DataValueField name should be Id
function MultiSelectSelectAll(DropDownName, CheckBoxName) {
    
    var dropdown = $("#" + DropDownName + "").data("kendoMultiSelect");
    if (dropdown) {
        var allIds = dropdown.dataSource.data().map(function (dataItem) { return dataItem.Id; });            //Select all ids from dropdown
        if ($("#" + CheckBoxName + "").is(":checked")) {
            dropdown.value(allIds);
        } else {
            dropdown.value([]);
        }
    }
}

//Summary:
//  Given dropdown name based get the 1.selected and 2.dropdown-all values.
//  two values is equal "Select All" checkbox is checked.
//
//Parameters:
//  DropDownName or Id,SelectAll Checkbox Name
function MultiSelectIndividualSelect(DropDownName, CheckBoxName) {
    var dropdown = $("#" + DropDownName + "").data("kendoMultiSelect");
    if (dropdown) {
        //Selected value length
        var selectedValueCount = dropdown.value().length;
        //Dropdown total value count
        var totalValueCount = dropdown.dataSource.data().length;
        if (selectedValueCount > 0 && totalValueCount > 0 && selectedValueCount === totalValueCount) {
            $("#" + CheckBoxName + "").prop("checked", true);
        } else {
            $("#" + CheckBoxName + "").prop("checked", false);
        }
    }
}

function IsDateGreaterThanToday(inputDate) {
    var toDate = new Date(inputDate);
    var currDate = new Date();

    currDate = new Date(currDate.getFullYear(), currDate.getMonth(), currDate.getDate());
    toDate = new Date(toDate.getFullYear(), toDate.getMonth(), toDate.getDate());

    if (toDate >= currDate) {
        return true;
    }
    return false;
}


function fromDateRange() {
    var dates = new Date();
    dates.setDate(dates.getDate());

    $("#StartDate").val(kendo.toString(kendo.parseDate(dates), emamiGlobal.constant.DateFormat));
    $("#EndDate").val(kendo.toString(kendo.parseDate(dates), emamiGlobal.constant.DateFormat));

    var endPicker = $("#EndDate").data("kendoDatePicker");
    endPicker.min(dates);
}

function fromDateChange() {
    var endPicker = $("#EndDate").data("kendoDatePicker"),
        startDate = this.value();

    if (startDate) {
        startDate = new Date(startDate);
        $("#EndDate").val(kendo.toString(kendo.parseDate(startDate), emamiGlobal.constant.DateFormat));
        endPicker.min(startDate);
    }
}

function MultiSelectEvents(DropDownName, CheckBoxName) {
    var dropdown = $("#" + DropDownName + "").data("kendoMultiSelect");
    if (dropdown) {
        var selectedValueCount = dropdown.value().length;                                   //Selected value length
        var totalValueCount = dropdown.dataSource.data().length;                            //Dropdown total value count
        if (selectedValueCount > 0 && totalValueCount > 0 && selectedValueCount === totalValueCount) {
            $("#" + CheckBoxName + "").prop("checked", true);
        } else {
            $("#" + CheckBoxName + "").prop("checked", false);
        }
    }
}

function MultiSelectCheckUnCheck(DropDownName, CheckBoxName) {
    
    var dropdown = $("#" + DropDownName + "").data("kendoMultiSelect");
    if (dropdown) {
        var allIds = dropdown.dataSource.data().map(function (dataItem) { return dataItem.Id; });            //Select all ids from dropdown
        if ($("#" + CheckBoxName + "").is(":checked")) {
            dropdown.value(allIds);
        } else {
            dropdown.value([]);
        }
    }
}


//Summary:
//  From date based to date range set
//  based on given numberOfDaysToAdd
//
//Parameters:
//  fromDateName, toDateName and numberOfDaysToAdd
function setToDateRange(fromDateName, toDateName, numberOfDaysToAdd) {

    var startDate = $("#" + fromDateName + "").data("kendoDatePicker").value();
    var endPicker = $("#" + toDateName + "").data("kendoDatePicker");

    if (startDate) {
        startDate = new Date(startDate);

        endPicker.setOptions({
            min: new Date(startDate),
            max: new Date(startDate.setDate(startDate.getDate() + numberOfDaysToAdd)),
            format: emamiGlobal.constant.DateFormat
        });

        startDate = new Date(startDate.setDate(startDate.getDate() - numberOfDaysToAdd));
        $("#" + toDateName + "").val(kendo.toString(kendo.parseDate(startDate), emamiGlobal.constant.DateFormat));
    }
}


//Summary:
//  Page load set default date and date range
//  based on given numberOfDaysToAdd
//
//Parameters:
//  fromDateName, toDateName and numberOfDaysToAdd
function setFromToDateRange(fromDateName, toDateName, numberOfDaysToAdd) {
    var dates = new Date();
    $("#" + toDateName + "").kendoDatePicker({
        min: new Date(dates),
        max: new Date(dates.setDate(dates.getDate() + 30)),
        format: emamiGlobal.constant.DateFormat
    });
    dates = new Date(dates.setDate(dates.getDate() - 30));
    $("#" + fromDateName + "").val(kendo.toString(kendo.parseDate(dates), emamiGlobal.constant.DateFormat));
    $("#" + toDateName + "").val(kendo.toString(kendo.parseDate(dates), emamiGlobal.constant.DateFormat));
}
function setToDateRangeBasedOnFromDate(startDate, toDateName, numberOfDaysToAdd) {
    var startdate = new Date(startDate);
    $("#" + toDateName + "").kendoDatePicker({
        min: new Date(startdate),
        max: new Date(startdate.setDate(startdate.getDate() + 30)),
        format: emamiGlobal.constant.DateFormat
    });
    startdate = new Date(startdate.setDate(startdate.getDate() - 30));
    $("#" + toDateName + "").val(kendo.toString(kendo.parseDate(startdate), emamiGlobal.constant.DateFormat));
}
function IsActiveBasedOnValidTo(ValidTo, IsActive) {
    
    var CurrentDate = new Date();
    CurrentDate.setHours(0, 0, 0, 0);
    if (!IsActive) {
        return false;
    }
    else {
        if (ValidTo >= CurrentDate) {
            return true;
        }
        return false;
    }
}

function checkIsActive(status) {
    if (status) {
        return "<center><i  class='icon-checkbox-checked2 faicon-green' ></i></center>";
    }
    else {
        return "<center><i class='icon-cancel-square2 faicon-red' ></i></center>";
    }
}


function IsActiveBasedOnValidTo(ValidTo, IsActive) {
    var CurrentDate = new Date();
    CurrentDate.setHours(0, 0, 0, 0);
    if (!IsActive) {
        return false;
    } else {
        if (ValidTo >= CurrentDate) {
            return true;
        }
        return false;
    }
}