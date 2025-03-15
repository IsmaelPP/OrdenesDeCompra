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
        public IActionResult CrearOrdenDeCompra(OrdenDeCompra orden)
        {

            // Obtener la lista de órdenes almacenadas en la sesión (o base de datos)
            var listaDeOrdenes = ObtenerOrdenesDesdeSesion();

            // Validación: verificar si ya existe el número de orden
            if (listaDeOrdenes != null && listaDeOrdenes.Any(o => o.NumeroDeOrden == orden.NumeroDeOrden))
            {
                // Si el número de orden ya existe, agregar un mensaje de error
                ModelState.AddModelError("NumeroDeOrden", "El número de orden ya existe. Por favor, ingrese un número de orden único.");

                // Retornar la vista con el modelo y los errores de validación
                return View("Crear",orden);
            }

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

        [HttpGet]
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
        public IActionResult EditarOrdenDeCompra(OrdenDeCompra orden)
        {
            if (ModelState.IsValid)
            {
                // Obtener la lista de órdenes desde la sesión
                var ordenes = ObtenerOrdenesDesdeSesion();

                // Encontrar la orden que se va a actualizar
                var ordenExistente = ordenes.FirstOrDefault(o => o.Id == orden.Id);
                if (ordenExistente != null)
                {
                    // Actualizar los datos de la orden
                    ordenExistente.NumeroDeOrden = orden.NumeroDeOrden;
                    ordenExistente.Fecha = orden.Fecha;
                    ordenExistente.Proveedor = orden.Proveedor;
                    ordenExistente.MontoTotal = orden.MontoTotal;

                    // Guardar la lista actualizada en la sesión
                    GuardarOrdenesEnSesion(ordenes);

                    return RedirectToAction("Index");
                }

                return NotFound();
            }

            return View("Editar",orden);
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
