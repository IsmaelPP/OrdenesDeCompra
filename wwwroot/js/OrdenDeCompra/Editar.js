
document.getElementById("MontoTotal").addEventListener("input", function (event) {
    let value = event.target.value;

    // Permitir solo números con hasta 2 decimales
    if (!/^\d+(\.\d{0,2})?$/.test(value)) {
        event.target.value = value.slice(0, -1); // Eliminar el último carácter inválido
    }
});
$(document).ready(function () {
    $('#loader').hide();
    var crearOrdenDeCompraUrl = $('#crearOrdenDeCompraUrl').data('url');

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
        event.preventDefault();  // Evita que el formulario se envíe normalmente

        // Validación de todos los campos requeridos
        if (!$('#editarOrdenForm').valid()) {
            return; // Si no es válido, no se hace la solicitud AJAX
        }

        // Mostrar el loader
        $('#loader').show();

        // Simular un retraso o un proceso de guardado
        setTimeout(function () {
            // Aquí iría tu lógica para guardar los datos (AJAX, etc.)
            // Por ejemplo, una llamada AJAX
            $.ajax({
                url: '/OrdenDeCompra/Editar',
                method: 'POST',
                data: $('#editarOrdenForm').serialize(),  // Serializa los datos del formulario
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
                        // Redirigir al Index después de que el usuario cierre la alerta
                        window.location.href = '/OrdenDeCompra/Index';
                    });
                },
                error: function (xhr, status, error) {
                    // Aquí se oculta el loader si hay un error
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

                            // Mostrar los errores en los span correspondientes
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
