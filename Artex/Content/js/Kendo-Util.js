$(function () {


    $(".kendo-select").kendoDropDownList({
        filter: "contains",
      //  optionLabel: "Seleccione una opción...",
     //   footerTemplate: $(" Total <strong>#: instance.dataSource.total() #</strong> items found").html(),
    });
    $(".kendo-select2").kendoDropDownList();

    $(".kendo-dateTime").kendoDateTimePicker({
        format: "{0:dd/MM/yyyy HH:mm:ss}",

        dateInput: true
    });

    $(".kendo-date").kendoDatePicker({
        format: "{0:dd/MM/yyyy}"

    });


    
  
});


