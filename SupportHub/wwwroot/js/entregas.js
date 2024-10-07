$(document).ready(function () {
    console.log("Document ready!");
    $("#busqueda").focus().select();

    if (localStorage.getItem("filtro-entregas") != null) {
        $("#filtros").val(localStorage.getItem("filtro-entregas"));
    }

    $("#busqueda").on("keyup", function () {
        ejecutarBusqueda();
    });

    $("#busqueda").on("input", function () {
        if ($("#busqueda").val() === '') {
            $("#btn-limpiar-filtro").hide();
        } else {
            $("#btn-limpiar-filtro").show();
        }
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
    
    $("#filtros").on("change", function () {
        localStorage.setItem("filtro-entregas", $("#filtros").val());
        $("#busqueda").focus().select();
        ejecutarBusqueda();
    });

});

function ejecutarBusqueda() {
    var filtroSeleccionado = $("#filtros").val();
    var valorAFiltrar = $("#busqueda").val().toLowerCase();
    $("#tablaEntregas tbody tr").filter(function () {
        var textoColumna = $(this).find('td').eq(filtroSeleccionado).text().toLowerCase();

        $(this).toggle(textoColumna.indexOf(valorAFiltrar) > -1);
    });
}

$(document).on('input', '#formCantidadEntrega', function () {

    var equipoSeleccionado = $("#formIdEquipo option:selected");
    var disponible = parseInt(equipoSeleccionado.data("disponible"));
    var cantidad = parseInt($("#formCantidadEntrega").val());

    if (!isNaN(cantidad) && disponible < cantidad) {
        $("#alertaCantidadDisponible").show();
        $("#modalActionButton").attr("disabled", true);
    } else {
        $("#alertaCantidadDisponible").hide();
        $("#modalActionButton").attr("disabled", false);
    }

});

function openModal(opcion, button = null) {
    // Guardamos los selects del modal
    var selectIdTipoEntrega = $("#formIdTipoEntrega").closest("select").prop("outerHTML");
    var selectIdEmpleadoEntrega = $("#formIdEmpleadoEntrega").closest("select").prop("outerHTML");
    var selectIdEmpleadoRecibe = $("#formIdEmpleadoRecibe").closest("select").prop("outerHTML");
    var selectIdEquipo = $("#formIdEquipo").closest("select").prop("outerHTML");

    //Eliminamos los selects del modal ya que los vamos a insertar nuevamente pero en diferente orden
    $("#formIdTipoEntrega").closest("select").remove();
    $("#formIdEmpleadoEntrega").closest("select").remove();
    $("#formIdEmpleadoRecibe").closest("select").remove();
    $("#formIdEquipo").closest("select").remove();

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
                    <input type="number" class="form-control" name="cantidadEntrega" placeholder="0" id="formCantidadEntrega" />
                </div>

                <div class="alert alert-danger mt-3 mb-0" role="alert" id="alertaCantidadDisponible" style="display: none;">
                        <i class="bi bi-exclamation-diamond-fill"></i> No hay suficiente cantidad disponible de ese equipo!
                </div>

                <label class="col-sm-12 col-form-label">Fecha:</label>
                <div class="col-sm-12">
                    <input type="date" class="form-control" name="fechaEntrega" id="formFechaEntrega" />
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
                    <textarea class="form-control" name="observacionEntrega" placeholder="Agregar observación (opcional)..." id="formObservacionEntrega" rows="3" ></textarea>
                </div>
            `);
        }
    }
}

function submitFormEntregas() {
    if ($("#formIdTipoEntrega").val().trim() === "") {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Tipo de entrega es requerido!"
        });
        return false;

    } else if ($("#formIdEquipo").val().trim() === "") {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Equipo es requerido!"
        });
        return false;

    } else if ($("#formCantidadEntrega").val() <= 0) {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Cantidad debe ser mayor a 0!"
        });
        return false;

    } else if ($("#formFechaEntrega").val().trim() === "") {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Fecha entrega es requerido!"
        });
        return false;

    } else if ($("#formIdEmpleadoEntrega").val().trim() === "") {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Empleado entrega es requerido!"
        });
        return false;

    } else if ($("#formIdEmpleadoRecibe").val().trim() === "") {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Empleado recibe es requerido!"
        });
        return false;

    }

    $("#formEntregas").submit();
}

function limpiarFiltroEntregas() {
    $("#busqueda").val("");
    $("#tablaEntregas tbody tr").show();
    $("#btn-limpiar-filtro").hide();
    $("#busqueda").focus().select();
}

function limpiarModalEntregas() {
    $("#modalEntregas h1").text("Agregar Entrega");
    $("#formEntregas")[0].reset();
}
