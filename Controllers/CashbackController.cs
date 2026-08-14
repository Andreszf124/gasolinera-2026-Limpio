using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Gasolinera.Common;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Infrastructure.Repositories;
using Gasolinera.Models.Entidades;
using Microsoft.AspNet.Identity;

namespace Gasolinera.Controllers
{
    [Authorize]
    public class CashbackController : Controller
    {
        private readonly GasolineraContext _contexto;
        private readonly ICashbackRepository _cashbackRepo;
        private readonly IMovimientoCashbackRepository _movimientoCashbackRepo;

        private const decimal MONTO_POR_PUNTO = 200;

        public CashbackController()
        {
            _contexto = new GasolineraContext();
            _cashbackRepo = new CashbackRepository(_contexto);
            _movimientoCashbackRepo = new MovimientoCashbackRepository(_contexto);
        }

        // ========================================== //
        // CONSTRUCTOR PARA USO DESDE TIENDACONTROLLER //
        // ========================================== //
        public CashbackController(GasolineraContext contexto)
        {
            _contexto = contexto;
            _cashbackRepo = new CashbackRepository(_contexto);
            _movimientoCashbackRepo = new MovimientoCashbackRepository(_contexto);
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Clientes = new SelectList(_contexto.Clientes, "IdCliente", "NombreCompleto");
            return View();
        }

        [HttpGet]
        public ActionResult SaldoCliente(int idCliente)
        {
            var cashback = _cashbackRepo.ObtenerPorCliente(idCliente);

            if (cashback == null)
            {
                cashback = new Cashback
                {
                    IdCliente = idCliente,
                    PuntosAcumulados = 0,
                    PuntosCanjeados = 0,
                    PuntosDisponibles = 0,
                    FechaActualizacion = DateTime.Now
                };
            }

            return View(cashback);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CanjearPuntos(int idCliente, decimal puntos, int? idVenta)
        {
            if (puntos <= 0)
            {
                TempData["MensajeError"] = "Los puntos a canjear deben ser mayores a cero.";
                return RedirectToAction("SaldoCliente", new { idCliente });
            }

            var cashback = _cashbackRepo.ObtenerPorCliente(idCliente);

            if (cashback == null)
            {
                TempData["MensajeError"] = "El cliente no tiene puntos acumulados.";
                return RedirectToAction("SaldoCliente", new { idCliente });
            }

            if (cashback.PuntosDisponibles < puntos)
            {
                TempData["MensajeError"] = $"El cliente no tiene suficientes puntos. Tiene {cashback.PuntosDisponibles} puntos, necesita {puntos}.";
                return RedirectToAction("SaldoCliente", new { idCliente });
            }

            decimal descuento = puntos * MONTO_POR_PUNTO;

            cashback.PuntosCanjeados += puntos;
            cashback.PuntosDisponibles -= puntos;
            cashback.FechaActualizacion = DateTime.Now;
            _cashbackRepo.Actualizar(cashback);

            var movimiento = new MovimientoCashback
            {
                IdCliente = idCliente,
                IdVenta = idVenta,
                Monto = descuento,
                PuntosGenerados = -puntos,
                TipoMovimiento = TipoMovimientoCashback.Canje,
                FechaMovimiento = DateTime.Now,
                UsuarioResponsableId = User.Identity.GetUserId(),
                Observaciones = $"Canje de {puntos} puntos por ₡{descuento:N2} de descuento"
            };

            _movimientoCashbackRepo.Agregar(movimiento);
            _cashbackRepo.Guardar();
            _movimientoCashbackRepo.Guardar();

            TempData["MensajeExito"] = $"Se canjearon {puntos} puntos por ₡{descuento:N2} de descuento.";
            return RedirectToAction("SaldoCliente", new { idCliente });
        }

        [HttpGet]
        public ActionResult HistorialCliente(int idCliente)
        {
            var movimientos = _movimientoCashbackRepo.ObtenerPorCliente(idCliente);
            return View(movimientos);
        }

        // ========================================== //
        // ACUMULAR PUNTOS - CORREGIDO                //
        // ========================================== //
        public void AcumularPuntos(int idCliente, int idVenta, decimal monto)
        {
            if (monto <= 0) return;
            if (idVenta <= 0) return;

            decimal puntos = Math.Floor(monto / MONTO_POR_PUNTO);

            if (puntos == 0) return;

            var cashback = _cashbackRepo.ObtenerPorCliente(idCliente);

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
                _cashbackRepo.Agregar(cashback);
            }
            else
            {
                cashback.PuntosAcumulados += puntos;
                cashback.PuntosDisponibles += puntos;
                cashback.FechaActualizacion = DateTime.Now;
                _cashbackRepo.Actualizar(cashback);
            }

            var movimiento = new MovimientoCashback
            {
                IdCliente = idCliente,
                IdVenta = idVenta,
                Monto = monto,
                PuntosGenerados = puntos,
                TipoMovimiento = TipoMovimientoCashback.Acumulacion,
                FechaMovimiento = DateTime.Now,
                UsuarioResponsableId = User.Identity.GetUserId(),
                Observaciones = $"Acumulación de {puntos} puntos por compra de ₡{monto:N2}"
            };

            _movimientoCashbackRepo.Agregar(movimiento);
            _cashbackRepo.Guardar();
            _movimientoCashbackRepo.Guardar();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _contexto.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}