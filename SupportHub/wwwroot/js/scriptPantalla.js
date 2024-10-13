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
    } else if (iti && !iti.isValidNumber()) {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Teléfono es incorrecto!"
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
//funciones de la página mi_informacion
function ModalModificarContraseña(button) {
    var tr = $(button).closest("tr");

    $(".modal h1").text("Modificar Contraseña");
    $("#id").val(tr.data("id"));
    $("#usuario").val(tr.data("usuario"));
    $("#nombre").val(tr.data("nombre"));
    $("#apellido").val(tr.data("apellido"));
    $("#usuario").prop("readonly", true);

    // ocultando los input y label que no se van a usar en esta operación
    $("#nombre").attr("type", "hidden");
    $("#apellido").attr("type", "hidden");
    $('label[for="nombre"]').hide();
    $('label[for="apellido"]').hide();

    // mostrando los campos que sí se van a usar
    $("#nContra").attr("type", "password");
    $("#CnContra").attr("type", "password");
    $('label[for="nContra"]').show();
    $('label[for="CnContra"]').show();
}

function ModalModificarUsuario(button) {
    var tr = $(button).closest("tr");

    $(".modal h1").text("Modificar Mi Información");
    $("#id").val(tr.data("id"));
    $("#usuario").val(tr.data("usuario"));
    $("#nombre").val(tr.data("nombre"));
    $("#apellido").val(tr.data("apellido"));
    $("#usuario").prop("readonly", true);

    // ocultando los input y label que no se van a usar en esta operación
    $("#nContra").attr("type", "hidden");
    $("#CnContra").attr("type", "hidden");
    $('label[for="nContra"]').hide();
    $('label[for="CnContra"]').hide();

    //  mostrando los campos que sí se van a usar
    $("#nombre").attr("type", "text");
    $("#apellido").attr("type", "text");
    $('label[for="nombre"]').show();
    $('label[for="apellido"]').show();
}

function validarIformacionUsuario() {
    var nombre = document.getElementById("nombre").value.trim();
    var apellido = document.getElementById("apellido").value.trim();
    var contra = document.getElementById("contraA").value.trim();
    var contraLogueada = sessionStorage.getItem("contra");
    var nuevaContra = document.getElementById("nContra").value.trim();
    var confirmarNuevaContra = document.getElementById("CnContra").value.trim();
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
    } else if (contra != contraLogueada) {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "La contraseña actual es incorrecta!"
        });
        return false;
    } else if (nuevaContra != "" && confirmarNuevaContra == "" || nuevaContra == "" && confirmarNuevaContra != "") {
        Swal.fire({
            icon: "error", 
            title: "Oops...",
            text: "Por favor, complete ambos campos de nueva contraseña."
        });
        return false;
    } else if (nuevaContra !== confirmarNuevaContra) {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "La nueva contraseña no coincide!"
        });
        return false;
    } else if (confirmarNuevaContra != "" && !/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/.test(nuevaContra)) {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "La nueva contraseña debe tener al menos 8 caracteres, incluyendo mayúsculas, minúsculas, números y caracteres especiales."
        });
        return false;
    }
    submitFormModificarInfoUsuario();
}

function submitFormModificarInfoUsuario() {
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
