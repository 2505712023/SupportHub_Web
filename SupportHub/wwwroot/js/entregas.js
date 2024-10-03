$(document).ready(function () {
    $("#busqueda").on("keyup", function () {
        var filtroSeleccionado = $("#filtros").val();
        var valorAFiltrar = $(this).val().toLowerCase();
        console.log("Entra al evento keyup.")
        $("#tablaEntregas tbody tr").filter(function () {
            var textoColumna = $(this).find('td').eq(filtroSeleccionado).text().toLowerCase();

            $(this).toggle(textoColumna.indexOf(valorAFiltrar) > -1);
        });
    });

    $('tr[data-idEntrega]').each(function () {
        var row = $(this);
        var fechaDevolucion = row.find('#fechaDevolucion');
        var devolucion = row.find('#devolucion');

        if (fechaDevolucion.text().trim() === "") {
            devolucion.attr('onclick', "openModal('agregarDevolucion', this)");
        } else {
            devolucion.attr('onclick', "openModal('eliminarDevolucion', this)");
        }
    });
});

function openModal(opcion, button = null) {
    if (button === null) {
        if (opcion === "agregar") {
            $("#modalActionButton").text("Agregar");
            $("select #idTipoEntrega").toggle();
            $("select #idEmpleadoEntrega").toggle();
            $("select #idEmpleadoRecibe").toggle();
            $("select #idEquipo").toggle();
        }
    }
}
