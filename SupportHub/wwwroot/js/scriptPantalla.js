function confirmDelete(idProveedor) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    });

    swalWithBootstrapButtons.fire({
        title: '¿Está seguro que desea eliminar?',
        text: "¡No podrá revertir esto!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, eliminar!',
        cancelButtonText: 'No, cancelar!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            // Llamada a la eliminación
            fetch('/Proveedor/mostrarProveedor?handler=Eliminar', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ idProveedor: idProveedor })
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            '¡Eliminado!',
                            'Se ha sido eliminado correctamente.',
                            'success'
                        ).then(() => {
                            // Recargar la tabla o la página
                            location.reload();
                        });
                    } else {
                        swalWithBootstrapButtons.fire(
                            'Error',
                            'No se pudo eliminar. ' + data.message,
                            'error'
                        );
                    }
                })
                
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Cancelado',
                ':)',
                'error'
            );
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

function llenarModal(button) {
    var tr = $(button).closest("tr");

    $(".modal #esModificacion").val("true")
    $(".modal h1").text("Modificar Proveedor");
    $(".modal #id").val(tr.data("id"));
    $(".modal #codigo").val(tr.data("codigo"));
    $(".modal #nombre").val(tr.data("nombre"));
    $(".modal #direccion").val(tr.data("direccion"));
    $(".modal #telefono").val(tr.data("telefono"));
}

function limpiarModal() {
    $(".modal #esModificacion").val("false")
    $(".modal h1").text("Agregar Proveedor");
    $("#formAgregarProveedor")[0].reset();
}
