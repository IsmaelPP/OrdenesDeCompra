using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace OrdenesDeCompras.Validations
{
    public class ValidationMontoTotal : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string valor = value.ToString();

                // Expresión regular para validar solo números con hasta 2 decimales
                if (!decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {
                    return new ValidationResult("El valor debe ser un número válido.");
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(valor, @"^\d+(\.\d{1,2})?$"))
                {
                    return new ValidationResult("El monto debe ser un valor numérico con hasta dos decimales.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
