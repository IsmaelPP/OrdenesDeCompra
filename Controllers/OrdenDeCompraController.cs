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
                // Obtener la lista de órdenes almacenadas en la sesión (o base de datos)
                var listaDeOrdenes = ObtenerOrdenesDesdeSesion();

                // Si no hay duplicados, agregar la nueva orden a la lista
                if (listaDeOrdenes == null)
                {
                    listaDeOrdenes = new List<OrdenDeCompra>();
                }
                

                orden.Id = listaDeOrdenes.Count > 0 ? listaDeOrdenes.Max(o => o.Id) + 1 : 1;  // Generar un nuevo Id dinámicamente
                listaDeOrdenes.Add(orden);

                // Guardar la lista actualizada en la sesión

                GuardarOrdenesEnSesion(listaDeOrdenes);

                // Redirigir al índice o donde desees
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
                if (listaDeOrdenes.Any(o => o.NumeroDeOrden == orden.NumeroDeOrden && o.Id != orden.Id))
                {
                    ModelState.AddModelError("NumeroDeOrden", "El número de orden ya existe. Por favor, ingrese un número único.");
                    return BadRequest(ModelState);
                }
            }


            var ordenExistente = listaDeOrdenes.FirstOrDefault(o => o.Id == orden.Id);

            if (ordenExistente != null)
            {
                ordenExistente.NumeroDeOrden = orden.NumeroDeOrden;
                ordenExistente.Fecha = orden.Fecha;
                ordenExistente.Proveedor = orden.Proveedor;
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
                return NotFound();  // Si no se encuentra la orden
            }

            // Cambiar el estado de la orden (activar/desactivar)
            orden.IsActive = !orden.IsActive;  // Invertir el estado actual

            // Guardar la lista actualizada en la sesión
            GuardarOrdenesEnSesion(ordenes);

            return RedirectToAction("Index");  // Redirigir a la vista de listado (Index)
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



    }
}
