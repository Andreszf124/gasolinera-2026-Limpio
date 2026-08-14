using System;
using System.Linq;
using System.Web.Mvc;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Models;
using Gasolinera.Models.Entidades;
using Microsoft.AspNet.Identity;

namespace Gasolinera.Controllers
{
    [Authorize]
    public class TiendaController : Controller
    {
        private readonly GasolineraContext _contexto;
        private readonly ApplicationDbContext _identityContext;

        public TiendaController()
        {
            _contexto = new GasolineraContext();
            _identityContext = new ApplicationDbContext();
        }

        // ========================================== //
        // LISTA DE PRODUCTOS PARA COMPRAR           //
        // ========================================== //
        [HttpGet]
        public ActionResult Index()
        {
            var productos = _contexto.Productos
                .Include("Categoria")
                .Where(p => p.Stock > 0)
                .ToList();

            var cliente = ObtenerOCrearCliente();

            if (cliente != null)
            {
                var cashback = _contexto.Cashbacks.FirstOrDefault(c => c.IdCliente == cliente.IdCliente);
                ViewBag.PuntosDisponibles = cashback?.PuntosDisponibles ?? 0;
            }
            else
            {
                ViewBag.PuntosDisponibles = 0;
            }

            return View(productos);
        }

        // ========================================== //
        // COMPRAR PRODUCTO                           //
        // ========================================== //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comprar(int idProducto, int cantidad, string metodoPago)
        {
            if (cantidad <= 0)
            {
                TempData["MensajeError"] = "La cantidad debe ser mayor a cero.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(metodoPago))
            {
                TempData["MensajeError"] = "Debe seleccionar un método de pago.";
                return RedirectToAction("Index");
            }

            var producto = _contexto.Productos.Find(idProducto);

            if (producto == null)
            {
                TempData["MensajeError"] = "El producto no existe.";
                return RedirectToAction("Index");
            }

            if (producto.Stock < cantidad)
            {
                TempData["MensajeError"] = $"No hay suficiente stock. Disponible: {producto.Stock}";
                return RedirectToAction("Index");
            }

            var cliente = ObtenerOCrearCliente();

            if (cliente == null)
            {
                TempData["MensajeError"] = "No se encontró un cliente asociado a su cuenta. Por favor, complete su perfil.";
                return RedirectToAction("Index");
            }

            // ========================================== //
            // OBTENER O CREAR EMPLEADO POR DEFECTO       //
            // ========================================== //
            var empleado = _contexto.Empleados.FirstOrDefault();
            if (empleado == null)
            {
                empleado = new Empleado
                {
                    NombreCompleto = "Empleado por Defecto",
                    Correo = "empleado@defecto.com",
                    Telefono = "8888-0000",
                    Cargo = "Cajero"
                };
                _contexto.Empleados.Add(empleado);
                _contexto.SaveChanges();
            }

            decimal total = producto.Precio * cantidad;
            decimal descuento = 0;
            decimal puntosUsados = 0;

            // ========================================== //
            // PROCESAR PAGO CON PUNTOS                   //
            // ========================================== //
            if (metodoPago == "Puntos")
            {
                var cashback = _contexto.Cashbacks.FirstOrDefault(c => c.IdCliente == cliente.IdCliente);
                if (cashback == null || cashback.PuntosDisponibles <= 0)
                {
                    TempData["MensajeError"] = "No tiene puntos disponibles para esta compra.";
                    return RedirectToAction("Index");
                }

                decimal puntosNecesarios = Math.Ceiling(total / 200);

                if (cashback.PuntosDisponibles < puntosNecesarios)
                {
                    TempData["MensajeError"] = $"No tiene suficientes puntos. Tiene {cashback.PuntosDisponibles} puntos, necesita {puntosNecesarios}.";
                    return RedirectToAction("Index");
                }

                descuento = total;
                total = 0;
                puntosUsados = puntosNecesarios;

                cashback.PuntosDisponibles -= puntosNecesarios;
                cashback.PuntosCanjeados += puntosNecesarios;
                cashback.FechaActualizacion = DateTime.Now;
                _contexto.SaveChanges();

                TempData["MensajeExito"] = $"Compra realizada con {puntosNecesarios} puntos. ¡Total: ₡0!";
            }
            else
            {
                TempData["MensajeExito"] = $"Compra realizada con {metodoPago}.";
            }

            // ========================================== //
            // CREAR VENTA                                //
            // ========================================== //
            var venta = new Venta
            {
                Fecha = DateTime.Now,
                IdCliente = cliente.IdCliente,
                IdEmpleado = empleado.IdEmpleado,
                TipoVenta = "Productos",
                TipoPago = metodoPago,
                Subtotal = producto.Precio * cantidad,
                Descuento = descuento,
                Impuesto = 0,
                Total = total,
                MetodoPago = metodoPago == "Puntos" ? "Puntos" : metodoPago,
                Estado = "Activa",
                PuntosUsados = puntosUsados
            };

            _contexto.Ventas.Add(venta);
            _contexto.SaveChanges();

            // ========================================== //
            // CREAR FACTURA                              //
            // ========================================== //
            var numeroFactura = GenerarNumeroFactura();

            var factura = new Factura
            {
                IdVenta = venta.IdVenta,
                FechaEmision = DateTime.Now,
                NumeroFactura = numeroFactura,
                Total = total,
                Estado = "Aprobada",
                Observaciones = $"Compra de {cantidad} unidad(es) de {producto.Nombre}. Pago: {metodoPago}"
            };

            _contexto.Facturas.Add(factura);

            // ========================================== //
            // ACTUALIZAR STOCK                           //
            // ========================================== //
            producto.Stock -= cantidad;

            _contexto.SaveChanges();

            // ========================================== //
            // ACUMULAR PUNTOS (si no es pago con puntos) //
            // ========================================== //
            if (metodoPago != "Puntos")
            {
                decimal puntosGenerados = Math.Floor((producto.Precio * cantidad) / 200);
                if (puntosGenerados > 0)
                {
                    // ========================================== //
                    // ACUMULAR PUNTOS DIRECTAMENTE AQUÍ          //
                    // ========================================== //
                    AcumularPuntos(cliente.IdCliente, venta.IdVenta, producto.Precio * cantidad);
                }
                TempData["MensajeExito"] += $" Se acumularon {Math.Floor((producto.Precio * cantidad) / 200)} puntos. Factura: {factura.NumeroFactura}";
            }
            else
            {
                TempData["MensajeExito"] = $"Compra realizada con puntos. Factura: {factura.NumeroFactura}";
            }

            return RedirectToAction("Index");
        }

        // ========================================== //
        // ACUMULAR PUNTOS (MÉTODO PRIVADO)           //
        // ========================================== //
        private void AcumularPuntos(int idCliente, int idVenta, decimal monto)
        {
            if (monto <= 0) return;
            if (idVenta <= 0) return;

            decimal puntos = Math.Floor(monto / 200);

            if (puntos == 0) return;

            // Buscar o crear registro de cashback
            var cashback = _contexto.Cashbacks.FirstOrDefault(c => c.IdCliente == idCliente);

            if (cashback == null)
            {
                cashback = new Cashback
                {
                    IdCliente = idCliente,
                    PuntosAcumulados = puntos,
                    PuntosCanjeados = 0,
                    PuntosDisponibles = puntos,
                    FechaActualizacion = DateTime.Now
                };
                _contexto.Cashbacks.Add(cashback);
            }
            else
            {
                cashback.PuntosAcumulados += puntos;
                cashback.PuntosDisponibles += puntos;
                cashback.FechaActualizacion = DateTime.Now;
            }

            // Crear movimiento
            var movimiento = new MovimientoCashback
            {
                IdCliente = idCliente,
                IdVenta = idVenta,
                Monto = monto,
                PuntosGenerados = puntos,
                TipoMovimiento = Common.TipoMovimientoCashback.Acumulacion,
                FechaMovimiento = DateTime.Now,
                UsuarioResponsableId = User.Identity.GetUserId(),
                Observaciones = $"Acumulación de {puntos} puntos por compra de ₡{monto:N2}"
            };

            _contexto.MovimientosCashback.Add(movimiento);
            _contexto.SaveChanges();
        }

        // ========================================== //
        // MÉTODO PARA OBTENER O CREAR CLIENTE        //
        // ========================================== //
        private Cliente ObtenerOCrearCliente()
        {
            var email = User.Identity.Name;

            var cliente = _contexto.Clientes.FirstOrDefault(c => c.Correo == email);

            if (cliente == null)
            {
                var userId = User.Identity.GetUserId();
                var user = _identityContext.Users.Find(userId);

                if (user != null)
                {
                    cliente = new Cliente
                    {
                        NombreCompleto = user.NombreCompleto ?? email,
                        Correo = email,
                        Telefono = "",
                        Direccion = ""
                    };

                    _contexto.Clientes.Add(cliente);
                    _contexto.SaveChanges();
                }
            }

            return cliente;
        }

        private string GenerarNumeroFactura()
        {
            var anio = DateTime.Now.Year;
            var mes = DateTime.Now.Month.ToString("D2");
            var consecutivo = _contexto.Facturas.Count() + 1;
            return $"F-{anio}{mes}-{consecutivo:D4}";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _contexto.Dispose();
                _identityContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}