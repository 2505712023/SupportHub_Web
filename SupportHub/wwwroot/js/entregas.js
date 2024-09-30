$(document).ready(function () {
    $("#busqueda").on("keyup", function () {
        var filtroSeleccionado = $("#filtros").val();
        var valorAFiltrar = $(this).val().toLowerCase();
        console.log("Entra al evento keyup.")
        $("#tablaEntregas tbody tr").filter(function () {
            var textoColumna = $(this).find('td').eq(filtroSeleccionado).text().toLowerCase();

            $(this).toggle(textoColumna.indexOf(valorAFiltrar) > -1);
        });
    });
});
