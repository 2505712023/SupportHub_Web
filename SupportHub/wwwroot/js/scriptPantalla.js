function checkSearch(input) {
    if (input.value.trim() === '') {
        input.form.submit(); // Envía el formulario si está vacío
    }
}
function confirmDelete(id) {

    // Mostrar la ventana emergente de confirmación
    if (confirm("¿Estás seguro de que deseas eliminar este proveedor?")) {
        // Si el usuario confirma, redirigir a la URL de eliminación
        window.location.href = '/Proveedor/Eliminar?id=' + id;
    }
}