using Microsoft.AspNetCore.Mvc;
using OrdenesDeCompras.Extensions;
using OrdenesDeCompras.Models;
using System.Collections.Generic;

namespace OrdenesDeCompras.Controllers
{
    public class OrdenDeCompraController : Controller
    {

        public IActionResult Index()
        {
            // Recuperar la lista de ordenes de compra desde la sesión
            var ordenes = ObtenerOrdenesDesdeSesion();

            if (ordenes == null || ordenes.Count == 0)
            {
                ViewData["Message"] = "No hay órdenes de compra guardadas.";
                return View();
            }

            return View(ordenes);
        }

        public IActionResult Crear()
        {

            return View(); 
        }


        [HttpPost]
        public IActionResult Crear(OrdenDeCompra orden)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                // Obtener la lista de órdenes almacenadas en la sesión
                var listaDeOrdenes = ObtenerOrdenesDesdeSesion();

                
                //if (listaDeOrdenes == null)
                //{
                //    listaDeOrdenes = new List<OrdenDeCompra>();
                //}
                

                orden.Id = listaDeOrdenes.Count > 0 ? listaDeOrdenes.Max(o => o.Id) + 1 : 1;
                orden.NumeroDeOrden = orden.NumeroDeOrden.Trim();
                orden.Proveedor = orden.Proveedor.Trim();
                listaDeOrdenes.Add(orden);

                // Guardar la lista actualizada en la sesión

                GuardarOrdenesEnSesion(listaDeOrdenes);

                return RedirectToAction("Index");
            }


            
        }

        public IActionResult Editar(int id)
        {
            var ordenes = ObtenerOrdenesDesdeSesion();
            var orden = ordenes.FirstOrDefault(o => o.Id == id);

            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }

        [HttpPost]
        public IActionResult Editar(OrdenDeCompra orden)
        {
            var listaDeOrdenes = ObtenerOrdenesDesdeSesion();
            if (!ModelState.IsValid)
            {
                // Verificar duplicidad (excepto para el mismo ID)
                if (listaDeOrdenes.Any(o => o.NumeroDeOrden.Trim() == orden.NumeroDeOrden.Trim() && o.Id != orden.Id))
                {
                    ModelState.AddModelError("NumeroDeOrden", "El número de orden ya existe. Por favor, ingrese un número único.");
                    return BadRequest(ModelState);
                }
            }


            var ordenExistente = listaDeOrdenes.FirstOrDefault(o => o.Id == orden.Id);

            if (ordenExistente != null)
            {
                ordenExistente.NumeroDeOrden = orden.NumeroDeOrden.Trim();
                ordenExistente.Fecha = orden.Fecha;
                ordenExistente.Proveedor = orden.Proveedor.Trim();
                ordenExistente.MontoTotal = orden.MontoTotal;
            }

            GuardarOrdenesEnSesion(listaDeOrdenes);
            return Json(new { success = true, message = "La orden se ha actualizado exitosamente." });
        }


        [HttpPost]
        public IActionResult ActivarDesactivarOrden(int id)
        {
            // Obtener la lista de órdenes desde la sesión
            var ordenes = ObtenerOrdenesDesdeSesion();

            // Buscar la orden por Id
            var orden = ordenes.FirstOrDefault(o => o.Id == id);

            if (orden == null)
            {
                return NotFound();
            }

            orden.IsActive = !orden.IsActive; 

            // Guardar la lista actualizada en la sesión
            GuardarOrdenesEnSesion(ordenes);

            return RedirectToAction("Index");
        }



        // Guardar la lista de órdenes en la sesión
        private void GuardarOrdenesEnSesion(List<OrdenDeCompra> ordenes)
        {
            HttpContext.Session.SetObject("OrdenesDeCompra", ordenes);
        }

        // Obtener la lista de órdenes desde la sesión
        private List<OrdenDeCompra> ObtenerOrdenesDesdeSesion()
        {
            return HttpContext.Session.GetObject<List<OrdenDeCompra>>("OrdenesDeCompra") ?? new List<OrdenDeCompra>();
        }

        [HttpPost]
        public IActionResult GenerarOrdenRandom(int cantidad)
        {
            var listaDeOrdenes = ObtenerOrdenesDesdeSesion();

			int nuevoId = listaDeOrdenes.Count > 0 ? listaDeOrdenes.Max(o => o.Id) + 1 : 1;

			List<OrdenDeCompra> ordenesGeneradas = new List<OrdenDeCompra>();

			for (int i = 0; i < cantidad; i++)
			{
				var orden = new OrdenDeCompra
				{
					Id = nuevoId++,
					NumeroDeOrden = $"ORD-{new Random().Next(1000, 9999)}",
					Fecha = GenerarFechaAleatoria(),
					Proveedor = GenerarProveedorAleatorio(),
					MontoTotal = Math.Round((decimal)(new Random().NextDouble() * 10000), 2),
					IsActive = new Random().Next(0, 2) == 1
				};

				ordenesGeneradas.Add(orden);
			}

			listaDeOrdenes.AddRange(ordenesGeneradas);

            GuardarOrdenesEnSesion(listaDeOrdenes);

			return Json(new { success = true, message = "Orden generada y guardada correctamente.", ordenesGeneradas });
		}


        // Generar una fecha aleatoria en los últimos 2 años
        private DateTime GenerarFechaAleatoria()
        {
            var fechaInicio = DateTime.Now.AddYears(-1);
            var fechaFin = DateTime.Now;

            int range = (fechaFin - fechaInicio).Days;
            return fechaInicio.AddDays(new Random().Next(range));
        }

        // Generar un nombre de proveedor aleatorio
        private string GenerarProveedorAleatorio()
        {
            var proveedores = new[] { "Proveedor A", "Proveedor B", "Proveedor C", "Proveedor D", "Proveedor E" };
            return proveedores[new Random().Next(proveedores.Length)];
        }



    }
}
