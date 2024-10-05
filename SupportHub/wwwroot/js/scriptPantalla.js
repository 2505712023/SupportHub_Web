function validarFormulario() {
    var codigo = document.getElementById("codigo").value.trim();
    var nombre = document.getElementById("nombre").value.trim();
    var direccion = document.getElementById("direccion").value.trim();
    var telefono = document.getElementById("telefono").value.trim();

    if (codigo === "") {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Código es requerido!"
        });
        return false;
    } else if (nombre === "") {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Nombre es requerido!"
        });
        return false;
    } else if (direccion === "") {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Dirección es requerido!"
        });
        return false;
    } else if (direccion.length > 100) {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Dirección solo permite 100 caracteres!"
        });
        return false;
    }else if (telefono === "") {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Teléfono es requerido!"
        });
        return false;
    }
    else if (telefono.length < 8 || telefono.length > 12) {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Teléfono debe estar entre 8 y 12 caracteres!"
        });
        return false;
    }
    submitForm();
}


function validarFormularioEliminar() {
    if ($(".modal#Eliminar #idProveedor").val().trim() === "") {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "idProveedor es requerido!"
        });
        return false;
    }
    submitFormEliminar()
}

function validarIformacionUsuario() {
    var nombre = document.getElementById("nombre").value.trim();
    var apellido = document.getElementById("apellido").value.trim();
    var contra = document.getElementById("contraA").value.trim();   
    var contraLogueada = sessionStorage.getItem("contra");
    if (nombre === "") {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "El nombre no puede estar vacío!"
        });
        return false;
    } else if (apellido === "") {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "El apellido no puede estar vacío!"
        });
        return false;
    }
    else if (contra != contraLogueada) {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "La contraseña actual es incorrecta!"
        });
        return false;
    }
    document.getElementById("ModificarinfoUsuario").submit();
}

function submitForm() {
    document.getElementById("formAgregarProveedor").submit();
}

function submitFormEliminar() {
    document.getElementById("formEliminarProveedor").submit();
}

function checkSearch(input) {
    if (input.value.trim() === '') {
        input.form.submit();
    }
}

function llenarModal(button) {
    var tr = $(button).closest("tr");

    $(".modal #esModificacion").val("true")
    $(".modal h1").text("Modificar Proveedores");
    $(".modal #id").val(tr.data("id"));
    $(".modal #codigo").val(tr.data("codigo"));
    $(".modal #nombre").val(tr.data("nombre"));
    $(".modal #direccion").val(tr.data("direccion"));
    $(".modal #telefono").val(tr.data("telefono"));

    $(".modal #codigo").prop("readonly", true);

}


function limpiarModal() {
    $(".modal #esModificacion").val("false")
    $(".modal h1").text("Agregar Proveedor");
    $("#formAgregarProveedor")[0].reset();

    $(".modal #codigo").prop("readonly", false);
}

function llenarModalEliminar(button) {
    var tr = $(button).closest("tr");

    $(".modal#Eliminar #idProveedor").val(tr.data("id"));
    $(".modal#Eliminar #esEliminacion").val("true");
    $("#Eliminar").on("shown.bs.modal", function () {
        $("#btnEliminar").focus();
    });
}

function limpiarModalEliminar() {
    $(".modal#Eliminar #esEliminacion").val("false");
    $("#formEliminarProveedor")[0].reset();
}
