function confirmDelete(idProveedor) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: "btn btn-success",
            cancelButton: "btn btn-danger"
        },
        buttonsStyling: false
    });

    swalWithBootstrapButtons.fire({
        title: "¿Está seguro que desea eliminar?",
        text: "¡No podrás revertir esto!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, Eliminar!",
        cancelButtonText: "No, Cancelar!",
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            // Redirige al método OnPostEliminar en mostrarProveedor.cshtml.cs
            const form = document.createElement('form');
            form.method = 'post';
            form.action = '/Proveedor/mostrarProveedor'; // Cambia esta ruta según tu estructura
            form.innerHTML = `<input type="hidden" name="idProveedor" value="${idProveedor}" />`;
            document.body.appendChild(form);
            form.submit();
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire({
                title: "Cancelado",
                text: ":)",
                icon: "error"
            });
        }
    });
}


function validarFormulario() {

    var codigo = document.getElementById("codigo").value.trim();
    var nombre = document.getElementById("nombre").value.trim();
    var direccion = document.getElementById("direccion").value.trim();
    var telefono = document.getElementById("telefono").value.trim();


    if (codigo === "" ) {
  
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
    } else if (telefono === "") {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Teléfono es requerido!"
        });
        return false;
    }


    submitForm();
}
function submitForm() {
    document.getElementById("formAgregarProveedor").submit();
}
function checkSearch(input) {
    if (input.value.trim() === '') {
        input.form.submit();
    }
}


