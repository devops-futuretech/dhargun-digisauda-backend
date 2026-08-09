function checkSelections() {
    const oilTypeSTSelected = $("#ddlOilTypeForST").data("kendoMultiSelect").value().length > 0;
    const bdoSelected = $("#ddlBdo").data("kendoMultiSelect").value().length > 0;
    const oilTypeZTSelected = $("#ddlOilTypeForZT").data("kendoMultiSelect").value().length > 0;
    const zonalHeadSelected = $("#ddlZonalHead").data("kendoMultiSelect").value().length > 0;
    const distributorSelected = $("#ddlDistributor").data("kendoMultiSelect").value().length > 0;

    toggleVisibility("#cbZonalInactive", oilTypeZTSelected && zonalHeadSelected);
    toggleVisibility("#cbStateInactive", oilTypeSTSelected && bdoSelected);
    toggleVisibility("#cbdistributorInactive", distributorSelected);
}

function toggleVisibility(checkboxSelector, condition) {
    const element = $(checkboxSelector).closest(".col-lg-1.col-sm-6.mb-3");
    condition ? element.show() : $(checkboxSelector).prop("checked", false).closest(".col-lg-1.col-sm-6.mb-3").hide();
}
function fromChange() {
    const endPicker = $("#dpToDate").data("kendoDateTimePicker");
    let startDate = this.value();

    if (startDate) {
        startDate = new Date(startDate);
        $("#dpToDate").val(kendo.toString(kendo.parseDate(startDate), emamiGlobal.constant.DateTimeFormat));
        endPicker.min(startDate);
    }
}
function onOilTypeChange() {
    MultiSelectEvents("ddlOilType", "oilTypeSelectAll");
}
function OnOilTypeDataBound() {
    MultiSelectEvents("ddlOilType", "oilTypeSelectAll");
}

function onOilTypeChangeST() {
    MultiSelectEvents("ddlOilType", "oilTypeSTSelectAll");
}
function OnOilTypeDataBoundST() {
    MultiSelectEvents("ddlOilType", "oilTypeSTSelectAll");
}

function onOilTypeChangeZT() {
    MultiSelectEvents("ddlOilTypeForZT", "oilTypeZTSelectAll");
}
function OnOilTypeDataBoundZT() {
    MultiSelectEvents("ddlOilTypeForZT", "oilTypeZTSelectAll");
}
function ResetFieldsData() {
    $("#StartDate").data("kendoDateTimePicker").value(new Date());
    $("#ddlDistributor, #ddlBdo, #ddlZonalHead, #ddlOilType").each(function () {
        $(this).data("kendoMultiSelect").value([]);
    });
    $("#cbdDisInactive, #cbdInactive").prop("checked", false);
}

function OnRoleChange() {
    const role = $("#ddlRole").data("kendoDropDownList").value();
    ResetFieldsData();
    loadFieldsBasedOnRoles(role);
}
function OnDistributorDataBound() {
    const ddl = $("#ddlDistributor").data("kendoMultiSelect");
    ddl.value(ddl.value());
}
