/* ------------------------------------------------------------------------------
*
*  # Form validation
*
*  Specific JS code additions for form_validation.html page
*
*  Version: 1.3
*  Latest update: Feb 5, 2016
*
* ---------------------------------------------------------------------------- */

$(function () {


    // Form components
    // ------------------------------

    // Switchery toggles
    var elems = Array.prototype.slice.call(document.querySelectorAll('.switchery'));
    elems.forEach(function (html) {
        var switchery = new Switchery(html);
    });


  


    // Setup validation
  
    // Initialize
    var validator = $(".form-validate-jquery").validate({
        ignore: 'input[type=hidden], .select2-search__field', // ignore hidden fields
        errorClass: 'validation-error-label',
        successClass: 'validation-valid-label',
        highlight: function (element, errorClass) {
            $(element).removeClass(errorClass);
            $(element).addClass('input-validation-error');
        },
        unhighlight: function (element, errorClass) {
            $(element).removeClass(errorClass);
            $(element).removeClass('input-validation-error');
        },

        // Different components require proper error label placement
        errorPlacement: function (error, element) {

            // Styled checkboxes, radios, bootstrap switch
            if (element.parents('div').hasClass("checker") || element.parents('div').hasClass("choice") || element.parent().hasClass('bootstrap-switch-container')) {
                if (element.parents('label').hasClass('checkbox-inline') || element.parents('label').hasClass('radio-inline')) {
                    error.appendTo(element.parent().parent().parent().parent());
                } else {
                    error.appendTo(element.parent().parent().parent().parent().parent());
                }
            }
            // Unstyled checkboxes, radios
            else if (element.parents('div').hasClass('checkbox') || element.parents('div').hasClass('radio')) {
                error.appendTo(element.parent().parent().parent());
            }
            // Input with icons and Select2
            else if (element.parents('div').hasClass('has-feedback') || element.hasClass('select2-hidden-accessible')) {
                error.appendTo(element.parent());
            }
            // Inline checkboxes, radios
            else if (element.parents('label').hasClass('checkbox-inline') || element.parents('label').hasClass('radio-inline')) {
                error.appendTo(element.parent().parent());
            }
            // Input group, styled file input
            else if (element.parent().hasClass('uploader') || element.parents().hasClass('input-group')) {
                error.appendTo(element.parent().parent());
            } else {
                error.insertAfter(element);
            }
        },
        validClass: "validation-valid-label",
        success: function (label) {
            label.addClass("validation-valid-label").text("");
            label.parent().find('.input-validation-error').removeClass('input-validation-error');
        },
        rules: {
            OldPassword: {
                minlength: 6
            },
            Password: {
                minlength: 6
            },
            MobileNumber: {
                //required: true,
                number: true,
                //rangelength: [3, 5],
                minlength: 10
            },
            Pincode: {
                //required: true,
                number: true,
                //rangelength: [3, 5],
                minlength: 6
            },
            CustomerCode: {
                minlength: 6
            },
            Otp: {
                number: true,
                minlength: 6
            },
            AadharCard: {
                number: true,
                minlength: 12
            },
            ConfirmPassword: {
                equalTo: "#Password"
            },
            Email: {
                email: true,
                emailFormatValidation: true
            },
            repeat_email: {
                equalTo: "#email"
            },
            minimum_characters: {
                minlength: 10
            },
            maximum_characters: {
                maxlength: 10
            },
            minimum_number: {
                min: 10
            },
            maximum_number: {
                max: 10
            },
            number_range: {
                range: [10, 20]
            },
            url: {
                url: true
            },
            date: {
                date: true
            },
            date_iso: {
                dateISO: true
            },
            numbers: {
                number: true
            },
            digits: {
                digits: true
            },
            creditcard: {
                creditcard: true
            },
            basic_checkbox: {
                minlength: 2
            },
            styled_checkbox: {
                minlength: 2
            },
            switchery_group: {
                minlength: 2
            },
            switch_group: {
                minlength: 2
            },
            IMEI: {
                number: true,
                minlength: 15
            }
        },
        messages: {
            MobileNumber: {
                // required: "You must enter your zip code",
                number: $("#hdnMobileNumberDigitsOnly").val(),
                //rangelength: "Mobile Number must have between 3 to 5 digits",
                minlength: $("#hdnMobileNumberChracterLong").val()
            },
            Pincode: {
                // required: "You must enter your zip code",
                number: $("#hdnPincodeDigitsOnly").val(),
                //rangelength: "Mobile Number must have between 3 to 5 digits",
                minlength: $("#hdnPincodeChracterLong").val()
            },
            Otp: {
                number: $("#hdnOtpDigitsOnly").val(),
                minlength: $("#hdnOtpChracterLong").val()
            },
            AadharCard: {
                number: $("#hdnAadharCardDigitsOnly").val(),
                minlength: $("#hdnAadharCradChracterLong").val()
            },
            OldPassword: {
                minlength: $("#hdnPasswordCharacterLong").val()
            },
            Password: {
                minlength: $("#hdnPasswordCharacterLong").val()
            },
            Email: {
                email: $("#hdnValidEmailAddress").val()
            },
            ConfirmPassword: {
                equalTo: $("#hdnPasswordMatch").val()
            },
            IMEI: {
                number: $("#hdnIsValidIMEI_msg").val()
            },
            custom: {
                required: "This is a custom error message"
            },
            agree: "Please accept our policy"
        }
    });


    // Reset form
    $('#reset').on('click', function () {
        validator.resetForm();
    });

});

jQuery.validator.addMethod("emailFormatValidation", function (value, element) {
    // allow any non-whitespace characters as the host part
    return this.optional(element) || /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/.test(value);
}, 'Please enter a valid email address.');
