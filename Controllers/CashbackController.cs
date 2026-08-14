using System;
using System.Linq;
using System.Web.Mvc;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Infrastructure.Repositories;
using Gasolinera.Models.Entidades;

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