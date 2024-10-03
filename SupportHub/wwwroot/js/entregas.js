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
    // Guardamos los selects del modal
    var selectIdTipoEntrega = $("#idTipoEntrega").closest("select").prop("outerHTML");
    var selectIdEmpleadoEntrega = $("#idEmpleadoEntrega").closest("select").prop("outerHTML");
    var selectIdEmpleadoRecibe = $("#idEmpleadoRecibe").closest("select").prop("outerHTML");
    var selectIdEquipo = $("#idEquipo").closest("select").prop("outerHTML");

    //Eliminamos los selects del modal ya que los vamos a insertar nuevamente pero en diferente orden
    $("#idTipoEntrega").closest("select").remove();
    $("#idEmpleadoEntrega").closest("select").remove();
    $("#idEmpleadoRecibe").closest("select").remove();
    $("#idEquipo").closest("select").remove();

    if (button === null) {
        if (opcion === "agregar") {
            $("#modalActionButton").text("Agregar");
            $("#modalBody").html(`
                <label class="col-sm-12 col-form-label">Tipo de entrega:</label>
                <div class="col-sm-12">
                    ${selectIdTipoEntrega}
                </div>

                <label class="col-sm-12 col-form-label">Equipo:</label>
                <div class="col-sm-12">
                    ${selectIdEquipo}
                </div>

                <label class="col-sm-12 col-form-label">Cantidad:</label>
                <div class="col-sm-12">
                    <input type="number" class="form-control" name="cantidadEntrega" placeholder="0" />
                </div>

                <label class="col-sm-12 col-form-label">Fecha:</label>
                <div class="col-sm-12">
                    <input type="date" class="form-control" name="fechaEntrega" />
                </div>

                <label class="col-sm-12 col-form-label">Empleado entrega:</label>
                <div class="col-sm-12">
                    ${selectIdEmpleadoEntrega}
                </div>

                <label class="col-sm-12 col-form-label">Empleado recibe:</label>
                <div class="col-sm-12">
                    ${selectIdEmpleadoRecibe}
                </div>

                <label class="col-sm-12 col-form-label">Observaciones:</label>
                <div class="col-sm-12">
                    <textarea class="form-control" name="observacionEntrega" placeholder="Agregar observación (opcional)..."></textarea>
                </div>
            `);
        }
    }
}
