
$(document).ready(function () {
    $('#loader').hide();

    const fechaInput = document.getElementById('Fecha');
    const fechaPicker = document.getElementById('FechaPicker');

    
    const [day, month, year] = fechaInput.value.split('/');
    const fechaOrden = new Date(year, month - 1, day);


    function formatDate(date) {
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        return `${day}/${month}/${year}`;
    }

    fechaPicker.setAttribute('value', fechaOrden.toISOString().split('T')[0]);

    // Capturar el cambio de fecha del calendario
    fechaPicker.addEventListener('change', (event) => {
        const selectedDate = new Date(event.target.value); 
        fechaInput.value = formatDate(selectedDate);  
        fechaPicker.style.display = 'none';
    });

    fechaInput.addEventListener('click', (event) => {
        const rect = fechaInput.getBoundingClientRect();
        fechaPicker.style.left = `${rect.left}px`;  
        fechaPicker.style.top = `${rect.bottom + window.scrollY}px`;  
        fechaPicker.style.display = 'block';
    });

    document.addEventListener('click', (e) => {
        if (!fechaInput.contains(e.target) && !fechaPicker.contains(e.target)) {
            fechaPicker.style.display = 'none';
        }
    });


    Inputmask("numeric", {
        groupSeparator: ",",
        autoGroup: true,
        digits: 2,
        digitsOptional: false,
        prefix: "",
        rightAlign: false,
        unmaskAsNumber: true
    }).mask("#MontoTotal");

    $('#editarOrdenForm').on('submit', function (event) {
        event.preventDefault();

        if (!$('#editarOrdenForm').valid()) {
            return;
        }

        $('#loader').show();

        // Simular un retraso o un proceso de guardado
        setTimeout(function () {
            $.ajax({
                url: '/OrdenDeCompra/Editar',
                method: 'POST',
                data: $('#editarOrdenForm').serialize(),
                success: function (response) {
                    $('#loader').hide();
                    Swal.fire({
                        title: 'Guardada',
                        text: 'La orden se ha guardado exitosamente.',
                        icon: 'success',
                        confirmButtonText: 'Aceptar',
                        customClass: {
                            confirmButton: 'custom-confirm-alert',
                        }
                    }).then(function () {
                        window.location.href = '/OrdenDeCompra/Index';
                    });
                },
                error: function (xhr, status, error) {
                    $('#loader').hide();
                    if (xhr.status === 400) {
                        Swal.fire({
                            icon: 'warning',
                            title: 'Datos duplicados',
                            text: 'El número de orden ya existe. Por favor, verifique la información e intente nuevamente.',
                            confirmButtonText: 'Aceptar',
                            customClass: {
                                confirmButton: 'custom-confirm-alert',
                            }
                        }).then(function () {
                            let errors = JSON.parse(xhr.responseText);

                            for (let key in errors) {
                                let mensajeError = errors[key][0];
                                let span = $(`span[data-valmsg-for="${key}"]`);
                                span.text(mensajeError);
                            }
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: 'Ocurrió un error al guardar los datos.',
                            confirmButtonText: 'Aceptar',
                            customClass: {
                                confirmButton: 'custom-confirm-alert',
                            }
                        });
                    }
                }
            });
        }, 2000);  // Simula un retraso de 2 segundos
    });


});
