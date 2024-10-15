document.addEventListener("DOMContentLoaded", function () {
    const rowsPerPage = 10; // Mostrar 10 empleados por página
    const tableBody = document.getElementById("empleadosBody");
    const rows = Array.from(tableBody.querySelectorAll("tr"));
    const paginationList = document.querySelector(".pagination-list");
    const totalPages = Math.ceil(rows.length / rowsPerPage);

    function showPage(pageNumber) {
        // Limpiar las filas actuales
        tableBody.innerHTML = "";

        // Calcular los índices de las filas para la página actual
        const startIndex = (pageNumber - 1) * rowsPerPage;
        const endIndex = Math.min(startIndex + rowsPerPage, rows.length);
        const pageRows = rows.slice(startIndex, endIndex);

        // Agregar las filas de la página actual
        pageRows.forEach(row => {
            tableBody.appendChild(row);
        });

        // Actualizar los puntos de paginación
        updatePagination(pageNumber);
    }

    function updatePagination(currentPage) {
        paginationList.innerHTML = ""; // Limpiar la paginación actual

        for (let i = 1; i <= totalPages; i++) {
            const pageItem = document.createElement("li");
            pageItem.className = "page-item";

            const pageDot = document.createElement("span");
            pageDot.className = "page-dot";

            if (i === currentPage) {
                pageDot.classList.add("active");
            }

      
            pageDot.addEventListener("click", function () {
                showPage(i);
            });

            pageItem.appendChild(pageDot);
            paginationList.appendChild(pageItem);
        }
    }

    // Mostrar la primera página al cargar
    showPage(1);
});

function validarFormularioEmpleado() {
    var nombre = document.getElementById("nombre").value.trim();
    var apellido = document.getElementById("apellido").value.trim();
    var telefono = document.getElementById("telefono").value.trim();
    var email = document.getElementById("email").value.trim();
    var area = document.getElementById('area').value;
    var cargo = document.getElementById('cargo').value;

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (nombre === "") {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Nombre es requerido!"
        });
        return false;
    } else if (apellido === "") {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Apellido es requerido!"
        });
        return false;
    } else if (telefono === "") {

        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Teléfono es requerido!"
        });
        return false;
    } else if (email === "") {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "El campo de correo no puede estar vacío."
        });
        return false;
    } else if (!emailRegex.test(email)) {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Por favor ingresa un correo electrónico válido."
        });
        return false;
    } else if (email.length > 100) {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "El correo no debe exceder los 100 caracteres."
        });
        return false;
    } else if (!area || !cargo) {
        Swal.fire({
            title: "Error",
            text: "Debe seleccionar un Área y un Cargo.",
            icon: "error",
            confirmButtonText: "OK"
        });
        return false;
    }

    // Enviar el formulario si todo está correcto
    document.getElementById('formAgregarEmpleado').submit();
}


function limpiarModalEmpleado() {
    $(".modal #esModificacion").val("false")
    $(".modal h1").text("Agregar Empleados");
    $("#formAgregarEmpleado")[0].reset();


}