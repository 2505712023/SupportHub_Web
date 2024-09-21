function checkSearch(input) {
    if (input.value.trim() === '') {
        input.form.submit(); // Envía el formulario si está vacío
    }
}
