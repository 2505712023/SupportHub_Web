document.getElementById('codigoempleado').addEventListener('change', function () {
    var CodigoEmpleado = this.value;

    // Realizar una solicitud al servidor con el ID seleccionado
    fetch(`/getPersonData?id=${CodigoEmpleado}`)
        .then(response => response.json())
        .then(data => {
            document.getElementById('nombre').value = data.nombre;
            document.getElementById('apellido').value = data.apellido;
        })
        .catch(error => console.error('Error:', error));
});