document.addEventListener('DOMContentLoaded', function () {
    var selectAllCheckbox = document.getElementById('select-all');
    var individualCheckboxes = document.querySelectorAll('.select-checkbox');
    var deleteButton = document.getElementById('btn-eliminar-seleccionados');
    var btnAdd = document.querySelector('.btn-add');
    var btnEdit = document.querySelectorAll('.btn-edit');
    var btnDelete = document.querySelectorAll('.btn-delete');

    // Función para verificar si hay al menos dos elementos seleccionados
    function toggleDeleteButton() {
        var selectedCount = Array.from(individualCheckboxes).filter(function (checkbox) {
            return checkbox.checked;
        }).length;

        if (selectedCount >= 2) {
            deleteButton.style.display = 'block'; // Mostrar botón de eliminar
            btnAdd.classList.add('btn-disabled'); // Deshabilitar agregar
            btnEdit.forEach(btn => btn.classList.add('btn-disabled')); // Deshabilitar modificar
            btnDelete.forEach(btn => btn.classList.add('btn-disabled')); // Deshabilitar eliminar individual
        } else {
            deleteButton.style.display = 'none'; // Ocultar botón si menos de 2 seleccionados
            btnAdd.classList.remove('btn-disabled'); // Habilitar agregar
            btnEdit.forEach(btn => btn.classList.remove('btn-disabled')); // Habilitar modificar
            btnDelete.forEach(btn => btn.classList.remove('btn-disabled')); // Habilitar eliminar individual
        }
    }

    // Evento para seleccionar o deseleccionar todos los checkboxes
    selectAllCheckbox.addEventListener('click', function (event) {
        var checkboxes = document.querySelectorAll('.select-checkbox');
        checkboxes.forEach(function (checkbox) {
            checkbox.checked = event.target.checked;
        });
        toggleDeleteButton(); // Actualizar el botón de eliminar seleccionados
    });

    // Evento para cada checkbox individual
    individualCheckboxes.forEach(function (checkbox) {
        checkbox.addEventListener('change', function () {
            var allChecked = Array.from(individualCheckboxes).every(function (cb) {
                return cb.checked;
            });
            selectAllCheckbox.checked = allChecked;
            toggleDeleteButton(); // Actualizar el botón de eliminar seleccionados
        });
    });

    // Función para deshabilitar los botones cuando hay 2 o más seleccionados
    function updateButtonStates() {
        var selectedCount = Array.from(individualCheckboxes).filter(function (checkbox) {
            return checkbox.checked;
        }).length;

        if (selectedCount >= 2) {
            btnAdd.classList.add('btn-disabled');
            btnEdit.forEach(btn => btn.classList.add('btn-disabled'));
            btnDelete.forEach(btn => btn.classList.add('btn-disabled'));
        } else {
            btnAdd.classList.remove('btn-disabled');
            btnEdit.forEach(btn => btn.classList.remove('btn-disabled'));
            btnDelete.forEach(btn => btn.classList.remove('btn-disabled'));
        }
    }

    // Inicializar el estado de los botones
    updateButtonStates();
});
