using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// Representa un producto en el sistema.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// El identificador único del producto.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// El nombre del producto.
        /// </summary>
        /// <example>Laptop Dell XPS</example>
        [Required(ErrorMessage = "El nombre del producto es requerido.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Name { get; set; }

        /// <summary>
        /// El precio unitario del producto. Debe ser mayor que cero.
        /// </summary>
        /// <example>1200.50</example>
        [Required(ErrorMessage = "El precio del producto es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Price { get; set; }

        /// <summary>
        /// La cantidad disponible del producto en stock. Debe ser al menos 1.
        /// </summary>
        /// <example>10</example>
        [Required(ErrorMessage = "La cantidad del producto es requerida.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Quantity { get; set; }
    }
}
