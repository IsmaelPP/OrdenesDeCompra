using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace OrdenesDeCompras.Models
{
    public class OrdenDeCompra
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string NumeroDeOrden { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public string Proveedor { get; set; }
        [Required]
        public decimal MontoTotal { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;  // Nuevo campo para estado de la orden

    }
}
