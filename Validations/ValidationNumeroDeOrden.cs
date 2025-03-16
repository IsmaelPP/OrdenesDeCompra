using Newtonsoft.Json;
using OrdenesDeCompras.Models;
using System.ComponentModel.DataAnnotations;

namespace OrdenesDeCompras.Validations
{
    public class ValidationNumeroDeOrden : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var httpContextAccessor = validationContext.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;

                if (httpContextAccessor != null)
                {
                    // Recuperar las órdenes de compra de la sesión
                    var session = httpContextAccessor.HttpContext.Session;
                    var ordenesJson = session.GetString("OrdenesDeCompra");

                    if (!string.IsNullOrEmpty(ordenesJson))
                    {
                        var listaDeOrdenes = JsonConvert.DeserializeObject<List<OrdenDeCompra>>(ordenesJson);

                        // Verificar si el número de orden ya existe
                        if (listaDeOrdenes != null && listaDeOrdenes.Any(o => o.NumeroDeOrden.Trim() == value.ToString().Trim()))
                        {
                            return new ValidationResult("El número de orden ya existe. Por favor, ingrese un número de orden único.");
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }

    }
}
