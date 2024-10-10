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

    var cantidad = parseInt($("#formCantidadEntrega").val());
    console.log("Cantidad disponible según el modal: " + parseInt($("#formIdEquipo option:selected").data("disponible")));

    if (!isNaN(cantidad) && parseInt($("#formIdEquipo option:selected").data("disponible")) < cantidad) {
        $("#alertaCantidadDisponible").show();
        $("#modalActionButton").attr("disabled", true);
    } else {
        $("#alertaCantidadDisponible").hide();
        $("#modalActionButton").attr("disabled", false);
    }

});

function openModal(opcion, button = null) {
    //console.log(button);

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
            mostrarSelects();
        }
    } else {
        if (opcion === "modificar") {

            $("#modalActionButton").text("Guardar");
            $("#modalBody").html(`
                <input type="hidden" id="idEntrega" name="idEntrega" value="" />

                <input type="hidden" id="esModificacion" name="esModificacion" value="true" />

                <label class="col-sm-12 col-form-label">Código:</label>
                <div class="col-sm-12">
                    <input type="text" class="form-control" id="codEntrega" name="codEntrega" >
                </div>

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
            mostrarSelects();

            var tr = $(button).closest("tr");
            //console.log(tr.data());
            $(".modal h1").text("Modificar Entrega");
            $(".modal #idEntrega").val(tr.data("idEntrega"));
            $(".modal #codEntrega").val(tr.data("codEntrega"));
            $(".modal #codEntrega").prop("readonly", true);
            $(".modal #formIdTipoEntrega").val(tr.data("idTipoEntrega"));
            $(".modal #formIdEquipo").val(tr.data("idEquipo"));
            $(".modal #formCantidadEntrega").val(tr.data("cantidadEntrega"));
            $(".modal #formFechaEntrega").val(tr.data("fechaEntrega"));
            $(".modal #formIdEmpleadoEntrega").val(tr.data("idEmpleadoEntrega"));
            $(".modal #formIdEmpleadoRecibe").val(tr.data("idEmpleadoRecibe"));
            $(".modal #formObservacionEntrega").val(tr.data("observacionEntrega"));

            // Obteniendo la cantidad anterior y actualizando el nuevo disponible
            var equipoSeleccionado = $(".modal #formIdEquipo option:selected");
            var cantidadAnterior = parseInt($(".modal #formCantidadEntrega").val(), 10) || 0;
            console.log("Cantidad entrega antes de comenzar la modificación: " + cantidadAnterior);

            var cantidadDisponibleActual = parseInt(equipoSeleccionado.data("disponible"), 10) || 0;
            console.log("Cantidad de data-disponible obtenida del option:selected al cargar el modal: " + cantidadDisponibleActual);

            var nuevaCantidad = cantidadAnterior + cantidadDisponibleActual;
            console.log("Nueva cantidad disponible calculada a partir de la cantidad de entrega antes de modificar y la cantidad de data-disponible en el option:selected: " + nuevaCantidad);

            // Obteniendo el texto anterior y actualizando el nuevo texto
            var textoAnterior = $(".modal #formIdEquipo option:selected").text();
            var nuevoTexto = textoAnterior.replace(/\(\d+ disponibles\)/, '(' + nuevaCantidad + ' disponibles)');

            // Actualizando valores del select luego de haber calculado la nueva cantidad y haber definido el nuevo texto
            $(".modal #formIdEquipo option:selected").attr("data-disponible", nuevaCantidad).data("disponible", nuevaCantidad);
            $(".modal #formIdEquipo option:selected").text(nuevoTexto);

            // Guardando cambios en localStorage por si se necesitan revertir posteriormente sin guardar una modificación de entrega
            localStorage.setItem('cantidadEntregaAntesDeModificar', cantidadAnterior);
            localStorage.setItem('textoEquipoAntesDeModificar', textoAnterior);
            console.log("Cantidad anterior que se guardó en el localStorage: " + cantidadAnterior);
            console.log("Texto anterior que se guardó en el localStorage: " + textoAnterior); 

        } else if (opcion === "eliminar") {
            $("#modalActionButton").text("Eliminar");
            $("#modalActionButton").removeClass("btn-primary");
            $("#modalActionButton").addClass("btn-danger");
            $("#modalBody").html(`
                <p>¿Seguro que desea eliminar una entrega?</p>
                <input type="hidden" name="codEntrega" id="codEntrega" value="" />
                <input type="hidden" name="esEliminacion" id="esEliminacion" value="true" />
                ${selectIdTipoEntrega}
                ${selectIdEquipo}
                ${selectIdEmpleadoEntrega}
                ${selectIdEmpleadoRecibe}
            `);

            var tr = $(button).closest("tr");
            //console.log(tr.data());
            $(".modal h1").text("Eliminar Entrega");
            $(".modal #codEntrega").val(tr.data("codEntrega"));
            ocultarSelects();
        }
    }
}

function submitFormEntregas() {
    if ($(".modal #esEliminacion").val() === "true") {
        $("#formEntregas").submit();
    } else {
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
}

function limpiarFiltroEntregas() {
    $("#busqueda").val("");
    $("#tablaEntregas tbody tr").show();
    $("#btn-limpiar-filtro").hide();
    $("#busqueda").focus().select();
}

function limpiarModalEntregas() {
    if ($(".modal #esEliminacion").val() === "true") {
        $("#modalActionButton").removeClass("btn-danger");
        $("#modalActionButton").addClass("btn-primary");
    }

    if ($(".modal #esModificacion").val() === "true") {
        // Buscamos los valores que se guardaron en localStorage
        cantidadAnterior = localStorage.getItem("cantidadEntregaAntesDeModificar");
        console.log("Cantidad guardada en localStorage y se asigna denuevo al cerrar el modal: " + localStorage.getItem("cantidadEntregaAntesDeModificar"));
        textoAnterior = localStorage.getItem("textoEquipoAntesDeModificar");
        console.log("Texto guardado en localStorage y se asigna denuevo al cerrar el modal: " + localStorage.getItem("textoEquipoAntesDeModificar"));

        // Guardamos el option:selected
        var opcionSeleccionada = $(".modal #formIdEquipo option:selected");

        // Creamos una nueva opción modificada en base a lo anterior
        let opcionNueva = $('<option>')
            .attr("value", opcionSeleccionada.val())  // Copia el valor original
            .attr("data-disponible", cantidadAnterior)  // Actualiza data-disponible
            .text(textoAnterior);  // Actualiza el texto

        // Intercambiamos la opción seleccionada por la nueva opción
        opcionSeleccionada.replaceWith(opcionNueva);

        // Detonamos el refresco de la UI
        $(".modal #formIdEquipo").val(opcionNueva.val()).trigger("change");
    }

    $("#modalEntregas h1").text("Agregar Entrega");
    $("#formEntregas")[0].reset();
}

function mostrarSelects() {
    $(".modal #formIdTipoEntrega").show();
    $(".modal #formIdEquipo").show();
    $(".modal #formIdEmpleadoEntrega").show();
    $(".modal #formIdEmpleadoRecibe").show();
}

function ocultarSelects() {
    $(".modal #formIdTipoEntrega").hide();
    $(".modal #formIdEquipo").hide();
    $(".modal #formIdEmpleadoEntrega").hide();
    $(".modal #formIdEmpleadoRecibe").hide();
}
