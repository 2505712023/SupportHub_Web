function eliminarProveedor(codProveedor) {
    if (confirm("¿Está seguro de que desea eliminar este proveedor?")) {
        fetch(`/Proveedor/eliminarProveedor?codProveedor=${codProveedor}`, {
            method: 'DELETE'
        })
            .then(response => {
                if (response.ok) {
                    // Elimina la fila del proveedor si la solicitud fue exitosa
                    document.getElementById(`fila-${codProveedor}`).remove();
                } else {
                    alert("Hubo un problema al eliminar el proveedor.");
                }
            })
            .catch(error => console.error('Error:', error));
    }
}
