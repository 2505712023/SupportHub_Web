document.getElementById('select-all').addEventListener('click', function (event) {
    var checkboxes = document.querySelectorAll('.select-checkbox');
    checkboxes.forEach(function (checkbox) {
        checkbox.checked = event.target.checked;
    });
});