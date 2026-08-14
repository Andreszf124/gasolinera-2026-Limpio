using Gasolinera.Common;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Infrastructure.Repositories;
using Gasolinera.Models.Entidades;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Gasolinera.Controllers
{
    [Authorize]
    public class CashbackController : Controller
    {
        private readonly GasolineraContext _contexto;
        private readonly ICashbackRepository _cashbackRepo;
        private readonly IMovimientoCashbackRepository _movimientoCashbackRepo;

        public CashbackController()
        {
            _contexto = new GasolineraContext();
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
        public void AcumularPuntos(int idCliente, int idVenta, decimal monto)
        {
            if (monto <= 0) return;

            decimal puntos = Math.Floor(monto / 200);

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
                Observaciones = $"Acumulación de {puntos} puntos por compra de ₡{monto:N2}"
            };

            _movimientoCashbackRepo.Agregar(movimiento);
            _cashbackRepo.Guardar();
            _movimientoCashbackRepo.Guardar();
        }



        [HttpGet]
        public ActionResult HistorialCliente(int idCliente)
        {
            var movimientos = _movimientoCashbackRepo.ObtenerPorCliente(idCliente);
            return View(movimientos);
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