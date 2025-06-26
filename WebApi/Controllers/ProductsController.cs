using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models; // Asegúrate de importar el namespace donde está tu Product

namespace WebApi.Controllers
{
    [ApiController] // Indica que este es un controlador de API sin vistas
    [Route("api/[controller]")] // Define la ruta base para este controlador (ej. /api/products)
    public class ProductsController : ControllerBase // Hereda de ControllerBase para controladores API
    {
        // --- Lista en memoria (simulando una base de datos) ---
       
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop Dell XPS", Price = 1200.50m, Quantity = 10 },
            new Product { Id = 2, Name = "Mouse Logitech MX Master", Price = 75.99m, Quantity = 50 },
            new Product { Id = 3, Name = "Teclado Mecánico HyperX", Price = 110.00m, Quantity = 25 }
        };

        private static int _nextProductId = 4; // Para simular el Id auto-incremental
        // -------------------------------------------------------------------

        // -------------------------------------------------------------------
        // 1. GET /api/products: Obtener una lista de todos los productos. (Síncrono)
        // -------------------------------------------------------------------
        /// <summary>
        /// Obtiene una lista de todos los productos disponibles.
        /// </summary>
        /// <returns>Una lista de objetos Product.</returns>
        /// <response code="200">Retorna la lista de productos.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Product>> GetAllProducts() // Método síncrono
        {
            return Ok(_products); // Devuelve un código de estado 200 OK y la lista de productos
        }

        // -------------------------------------------------------------------
        // 2. GET /api/products/{id}: Obtener un producto por su ID. (Síncrono)
        // -------------------------------------------------------------------
        /// <summary>
        /// Obtiene un producto específico por su identificador único.
        /// </summary>
        /// <param name="id">El ID del producto a buscar.</param>
        /// <returns>El producto solicitado si se encuentra.</returns>
        /// <response code="200">Retorna el producto encontrado.</response>
        /// <response code="404">Si el producto con el ID especificado no se encuentra.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> GetProductById(int id) // Método síncrono
        {
            var product = _products.FirstOrDefault(vproduct => vproduct.Id == id); // Busca el producto por ID

            if (product == null)
            {
                return NotFound($"Producto con ID {id} no encontrado.");
            }

            return Ok(product);
        }

        // -------------------------------------------------------------------
        // 3. POST /api/products: Crear un nuevo producto. (Síncrono)
        // -------------------------------------------------------------------
        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        /// <remarks>
        /// Ejemplo de cuerpo de solicitud:
        ///
        ///     POST /api/products
        ///     {
        ///        "name": "Nuevo Producto",
        ///        "price": 99.99,
        ///        "quantity": 5
        ///     }
        ///
        /// </remarks>
        /// <param name="product">El objeto Producto a crear. Requiere Name, Price y Quantity.</param>
        /// <returns>El producto creado con su nuevo ID.</returns>
        /// <response code="201">Retorna el producto recién creado.</response>
        /// <response code="400">Si el modelo de producto es inválido (ej. datos faltantes o incorrectos).</response>
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateProduct([FromBody] Product product) // Método síncrono
        {
            // Valida el modelo recibido del cuerpo de la solicitud.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Asigna un nuevo ID al producto (simulando auto-incremental).
            product.Id = _nextProductId++;
            _products.Add(product); // Agrega el nuevo producto a nuestra lista en memoria.

            // Devuelve un código de estado 201 Created y la ubicación del nuevo recurso.
            return CreatedAtAction(
                nameof(GetProductById), // Nombre de la acción GET para obtener el producto por ID
                new { id = product.Id }, // Parámetros para la URL (el ID del nuevo producto)
                product // El objeto del producto creado
            );
        }

        // -------------------------------------------------------------------
        // 4. PUT /api/products/{id}: Actualizar un producto existente. (Síncrono)
        // -------------------------------------------------------------------
        /// <summary>
        /// Actualiza un producto existente por su ID.
        /// </summary>
        /// <remarks>
        /// El ID en la URL debe coincidir con el ID del producto en el cuerpo de la solicitud.
        ///
        ///     PUT /api/products/1
        ///     {
        ///        "id": 1,
        ///        "name": "Laptop Dell XPS (Actualizado)",
        ///        "price": 1250.00,
        ///        "quantity": 8
        ///     }
        ///
        /// </remarks>
        /// <param name="id">El ID del producto a actualizar.</param>
        /// <param name="product">El objeto Product con los datos actualizados. El ID debe coincidir con el de la URL.</param>
        /// <returns>Estado de la operación.</returns>
        /// <response code="204">Si el producto fue actualizado exitosamente (no contenido).</response>
        /// <response code="400">Si el ID de la URL no coincide con el ID del producto en el cuerpo, o el modelo es inválido.</response>
        /// <response code="404">Si el producto con el ID especificado no se encuentra.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateProduct(int id, [FromBody] Product product) // Método síncrono
        {
            // Validaciones
            if (id != product.Id)
            {
                return BadRequest("El ID del producto en la URL no coincide con el ID del cuerpo de la solicitud.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Buscar y actualizar el producto en la lista en memoria
            var existingProduct = _products.FirstOrDefault(vproduct => vproduct.Id == id);
            if (existingProduct == null)
            {
                return NotFound($"Producto con ID {id} no encontrado para actualizar.");
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;

            // 204 No Content es una respuesta común para PUT exitoso cuando no se devuelve contenido.
            return NoContent();
        }

        // -------------------------------------------------------------------
        // 5. DELETE /api/products/{id}: Eliminar un producto por su ID. (Síncrono)
        // -------------------------------------------------------------------
        /// <summary>
        /// Elimina un producto existente por su ID.
        /// </summary>
        /// <param name="id">El ID del producto a eliminar.</param>
        /// <returns>Estado de la operación.</returns>
        /// <response code="204">Si el producto fue eliminado exitosamente (no contenido).</response>
        /// <response code="404">Si el producto con el ID especificado no se encuentra.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteProduct(int id) // Método síncrono
        {
            // Buscar y eliminar el producto de la lista en memoria
            var productToRemove = _products.FirstOrDefault(vproduct => vproduct.Id == id);
            if (productToRemove == null)
            {
                return NotFound($"Producto con ID {id} no encontrado para eliminar.");
            }

            _products.Remove(productToRemove);

            // 204 No Content es una respuesta común para DELETE exitoso cuando no se devuelve contenido.
            return NoContent();
        }
    }
}
