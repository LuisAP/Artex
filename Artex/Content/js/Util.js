$(function () {

    $(".textMoneda").focusout(function () {

        amount = $(this).val(); 
        amount = parseFloat(amount.replace(/[^0-9\.]/g, '')); // elimino cualquier cosa que no sea numero o punto

        var decimals = 2; // numero de decimales

        // si no es un numero o es igual a cero retorno el mismo cero
        if (isNaN(amount) || amount === 0) {
            $(this).val(parseFloat(0).toFixed(decimals));
        }
        else {
            // si es mayor o menor que cero retorno el valor formateado como numero
            amount = '' + amount.toFixed(decimals);

            var amount_parts = amount.split('.'),
                regexp = /(\d+)(\d{3})/;

            while (regexp.test(amount_parts[0]))
                amount_parts[0] = amount_parts[0].replace(regexp, '$1' + ',' + '$2');

            $(this).val("$" + amount_parts.join('.'));

        }


    });
    $(".decimaFormat").focusout(function () {

        amount = $(this).val();
        amount = parseFloat(amount.replace(/[^0-9\.]/g, '')); // elimino cualquier cosa que no sea numero o punto

        var decimals = 2; // numero de decimales

        // si no es un numero o es igual a cero retorno el mismo cero
        if (isNaN(amount) || amount === 0) {
            $(this).val(parseFloat(0));
        }
        else {
            // si es mayor o menor que cero retorno el valor formateado como numero
            amount = '' + amount.toFixed(decimals);

            var amount_parts = amount.split('.'),
                regexp = /(\d+)(\d{3})/;

            while (regexp.test(amount_parts[0]))
                amount_parts[0] = amount_parts[0].replace(regexp, '$1' + ',' + '$2');

            $(this).val( amount_parts.join('.'));

        }


    });

    $(".decimaFormatNullable").focusout(function () {

        amount = $(this).val();
        amount = parseFloat(amount.replace(/[^0-9\.]/g, '')); // elimino cualquier cosa que no sea numero o punto

        var decimals = 2; // numero de decimales

        // si no es un numero o es igual a cero retorno el mismo cero
        if (isNaN(amount)) {
            $(this).val("");
        }
        else if (amount=== 0) {
            $(this).val(0);
        }
        else {
            // si es mayor o menor que cero retorno el valor formateado como numero
            amount = '' + amount.toFixed(decimals);

            var amount_parts = amount.split('.'),
                regexp = /(\d+)(\d{3})/;

            while (regexp.test(amount_parts[0]))
                amount_parts[0] = amount_parts[0].replace(regexp, '$1' + ',' + '$2');

            $(this).val(amount_parts.join('.'));

        }


    });

});


function toDecimalFormat(num) {

    num += ''; // por si pasan un numero en vez de un string
    num = parseFloat(num.replace(/[^0-9\.]/g, '')); // elimino cualquier cosa que no sea numero o punto

    var decimals = 2; // numero de decimales

    // si no es un numero o es igual a cero retorno el mismo cero
    if (isNaN(num) || num === 0)
        return parseFloat(0).toFixed(decimals);

    // si es mayor o menor que cero retorno el valor formateado como numero
    num = '' + num.toFixed(decimals);

    var num_parts = num.split('.'),
        regexp = /(\d+)(\d{3})/;

    while (regexp.test(num_parts[0]))
        num_parts[0] = num_parts[0].replace(regexp, '$1' + ',' + '$2');

    return num_parts.join('.');
}
