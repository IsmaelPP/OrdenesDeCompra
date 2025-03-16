
$(document).ready(function () {
    var table = $('#OrdenesDeCompras').DataTable({
        language: {
            search: "", // Oculta la barra de búsqueda predeterminada
            decimal: ",",
            thousands: ".",
            searchPlaceholder: "Ingrese el número de orden...", // Texto dentro del input
            emptyTable: "No hay órdenes de compra registradas.",
            info: "Mostrando _START_ a _END_ de _TOTAL_ órdenes",
            infoEmpty: "Mostrando 0 a 0 de 0 órdenes",
            infoFiltered: "(filtrado de _MAX_ órdenes totales)",
            lengthMenu: "Mostrar _MENU_ órdenes",
            loadingRecords: "Cargando...",
            processing: "Procesando...",
            search: "Buscar:",
            zeroRecords: "No se encontraron órdenes de compra coincidentes",
            aria: {
                sortAscending: ": activar para ordenar la columna en orden ascendente",
                sortDescending: ": activar para ordenar la columna en orden descendente"
            }
        },
        responsive: true,
        autoWidth: false,
        lengthChange: true,
        searching: true,
        pageLength: 10,
        order: [[0, 'asc']],
        columnDefs: [
            { orderable: false, targets: [6] },
            {
                targets: 4, // Indicar/seleccionar indice de la columna que vamos a ordenar
                type: 'num', // Definirla como tipo numerica
                render: function (data, type, row) {
                    if (type === 'display') {
                        var numericValue = parseFloat(data.replace(/[^\d.-]/g, ''));
                        if (!isNaN(numericValue)) {
                            return `$${numericValue.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
                        }
                        return data; // Si no es un número válido, se devuelve tal cual
                    }
                    return parseFloat(data.replace(/[^\d.-]/g, '')) || 0; // Retornar el valor numérico limpio
                }
            }
        ]
    });

    // Conectar el campo de búsqueda personalizado
    $('#customSearch').on('keyup', function () {
        table.search(this.value).draw();
    });


    $('.activarDesactivarBtn').on('click', function (e) {
        e.preventDefault();

        let id = $(this).data('id');
        let status = $(this).data('status');
        let form = $(this).closest('form');

        Swal.fire({
            title: `Modificar estatus`,
            text: `¿Está seguro de que desea ${status} esta orden de compra?`,
            icon: 'question',
            showCancelButton: true,
            cancelButtonColor: '#6c757d',
            confirmButtonText: `Sí, ${status}`,
            cancelButtonText: 'Cancelar',
            customClass: {
                confirmButton: status === 'activar' ? 'custom-confirm-alert' : 'custom-decline-alert',  // Cambiar el estilo del botón de confirmación
                cancelButton: 'custom-cancel-alert'    // Cambiar el estilo del botón de cancelación
            }
        }).then((result) => {
            if (result.isConfirmed) {
                form.submit(); // Enviar el formulario después de la confirmación
            }
        });
    });
});