
$(document).ready(function () {
    var table = $('#OrdenesDeCompras').DataTable({
        language: {
            search: "",
            decimal: ".",
            thousands: ",",
            searchPlaceholder: "Ingrese el número de orden...",
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
                            return `$${numericValue.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
                        }
                        return data; // Si no es un número válido, se devuelve tal cual
                    }
                    return parseFloat(data.replace(/[^\d.-]/g, '')) || 0; // Retornar el valor numérico limpio
                }
            }
        ]
    });
    $('#loader').hide();

    $('#customSearch').on('keyup', function () {
        var searchValue = this.value.trim();
        table.search(searchValue).draw();
    });


    
    $(document).on('click', '.activarDesactivarBtn', function (e) {
        e.preventDefault();
        console.log("Entró al botón");

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
                confirmButton: status === 'activar' ? 'custom-confirm-alert' : 'custom-decline-alert',
                cancelButton: 'custom-cancel-alert'
            }
        }).then((result) => {
            if (result.isConfirmed) {
                form.submit();
            }
        });
    });


    $('#generarOrdenRandomBtn').on('click', function () {
        Swal.fire({
            title: 'Generar orden de compra',
            text: '¿Cuántas órdenes de compra aleatorias deseas generar? (Entre 1 y 30)',
            icon: 'question',
            input: 'select',
            inputOptions: {
                '10': '10 órdenes',
                '20': '20 órdenes',
                '30': '30 órdenes'
            },
            inputPlaceholder: 'Selecciona la cantidad de órdenes',
            showCancelButton: true,
            confirmButtonText: 'Generar',
            cancelButtonText: 'Cancelar',
            customClass: {
                input: 'custom-select',
                confirmButton: 'custom-confirm-alert',
                cancelButton: 'custom-cancel-alert'
            },
            preConfirm: (cantidad) => {
                if (cantidad < 1 || cantidad > 30) {
                    Swal.showValidationMessage('Por favor, selecciona una cantidad.');
                    return false; 
                }
                return cantidad;
            }
        }).then((result) => {
            if (result.isConfirmed) {
                const cantidad = result.value;
                $('#loader').show();
                setTimeout(function () {
                    $.ajax({
                        url: '/OrdenDeCompra/GenerarOrdenRandom',
                        type: 'POST',
                        data: { cantidad: cantidad },
                        success: function (response) {
                            $('#loader').hide();
                            if (response.success) {
                                Swal.fire({
                                    title: 'Éxito',
                                    text: 'La órdenes de compra se ha generado exitosamente.',
                                    icon: 'success',
                                    confirmButtonText: 'Aceptar',
                                    customClass: {
                                        confirmButton: 'custom-confirm-alert'
                                    }
                                }).then(() => {
                                    location.reload();
                                });
                            } else {
                                Swal.fire({
                                    title: 'Error',
                                    text: 'Ocurrió un error al generar la orden.',
                                    icon: 'error',
                                    confirmButtonText: 'Aceptar',
                                    customClass: {
                                        confirmButton: 'custom-confirm-alert'
                                    }
                                });
                            }
                        },
                        error: function () {
                            $('#loader').hide();
                            Swal.fire({
                                title: 'Error',
                                text: 'Ocurrió un error al generar la orden.',
                                icon: 'error',
                                confirmButtonText: 'Aceptar',
                                customClass: {
                                    confirmButton: 'custom-confirm-alert'
                                }
                            });
                        }
                    });
                }, 2000);


                
            }
        });
    });

});