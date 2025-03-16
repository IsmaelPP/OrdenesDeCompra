using System.ComponentModel.DataAnnotations;
using OrdenesDeCompras.Validations;

namespace OrdenesDeCompras.Models
{
    public class OrdenDeCompra
    {
        [Key]
        [Required(ErrorMessage = "El campo Id es obligatorio.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Número de Orden es obligatorio.")]
        [ValidationNumeroDeOrden]
        public required string NumeroDeOrden { get; set; }
        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        [DataType(DataType.Date, ErrorMessage = "La fecha no tiene un formato válido.")]
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "El campo Proveedor es obligatorio.")]
        public required string Proveedor { get; set; }
        [Required(ErrorMessage = "El campo Monto Total es obligatorio.")]
        [ValidationMontoTotal]
        public decimal MontoTotal { get; set; }
        [Required]
        public bool IsActive { get; set; } = true; 

    }
}
