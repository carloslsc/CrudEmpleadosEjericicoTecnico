var dataTable;

$(document).ready(
    function () {
        cargarDatatable();
    }
);

function cargarDatatable() {
    //Llamar Datatable
    dataTable = $("#tablaEmpleados").DataTable({
        language: {
            "decimal": "",
            "emptyTable": "No hay información",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 a 0 de 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "columnDefs": [
            {
                "searchable": false,
                "targets": [0, 2, 3, 4, 5]
            },
            {
                "targets": [5],
                "className": "dt-body-right"
            }
        ],
        "width": "100%"
    });
}