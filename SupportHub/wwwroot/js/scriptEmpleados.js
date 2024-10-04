function validarFormularioEmpleado() {
    var nombre = document.getElementById("nombre").value.trim();
    var apellido = document.getElementById("apellido").value.trim();
    var telefono = document.getElementById("telefono").value.trim();
    var email = document.getElementById("email").value.trim();
    var area = document.getElementById("area").value.trim();
    var cargo = document.getElementById("cargo").value.trim();

    if (nombre === "") {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Código es requerido!"
        });
        return false;
    } else if (apellido === "") {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Nombre es requerido!"
        });
        return false;
    } else if (telefono === "") {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Dirección es requerido!"
        });
        return false;
    } else if (email === "") {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Dirección solo permite 100 caracteres!"
        });
        return false;
    } else if (area === "") {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Teléfono es requerido!"
        });
        return false;
    }
    else if (cargo == "") {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Teléfono debe estar entre 8 y 12 caracteres!"
        });
        return false;
    }
    submitForm();
}


function submitForm() {
    document.getElementById("formAgregarEmpleado").submit();
}


function limpiarModalEmpleado() {
    $(".modal #esModificacion").val("false")
    $(".modal h1").text("Agregar Empleados");
    $("#formAgregarEmpleado")[0].reset();


}