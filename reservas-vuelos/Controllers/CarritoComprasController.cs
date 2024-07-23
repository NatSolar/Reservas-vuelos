using reservas_vuelos.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace reservas_vuelos.Controllers
{
    public class CarritoComprasController : Controller
    {
        // GET: CarritoCompras
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Carrito()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ListarVuelosCarrito()
        {
            int idCliente = (int)Session["usuario"];
            List<Carrito> oLista = new List<Carrito>();

            bool conversion;

            oLista = new Carrito().ListarVuelos(idCliente).Select(oc => new Carrito()
            {
                idVuelo = new Vuelo(oc.idVuelo.flightNo, oc.idVuelo.oneWay, oc.idVuelo.duration,
                                oc.idVuelo.departureIataCode, oc.idVuelo.departureTerminal,
                                oc.idVuelo.departureDate, oc.idVuelo.arrivalIataCode, oc.idVuelo.arrivalDate,
                                oc.idVuelo.carrierCode, oc.idVuelo.carrierName,
                                oc.idVuelo.aircraftType, oc.idVuelo.stopsNo, oc.idVuelo.currency,
                                oc.idVuelo.total, oc.idVuelo.cabin, oc.idVuelo.cabinFare)
                ,
                cantidad = oc.cantidad

            }).ToList();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int idVuelo)
        {
            int idCliente = (int)Session["usuario"];

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new Carrito().EliminarCarrito(idCliente, idVuelo);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult OperacionCarrito(int idVuelo, bool sumar)
        {
            int idCliente = (int)Session["usuario"];

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new Carrito().OperacionCarrito(idCliente, idVuelo, true, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> olistaCarrito, Venta oVenta)
        {
            decimal total = 0;

            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = new System.Globalization.CultureInfo("en-US");
            detalle_venta.Columns.Add("IdVuelo", typeof(string));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Total", typeof(decimal));

            foreach (Carrito oCarrito in olistaCarrito)
            {
                decimal subtotal = Convert.ToDecimal(oCarrito.cantidad.ToString()) * oCarrito.idVuelo.total;

                total += subtotal;

                detalle_venta.Rows.Add(new object[]
                {
                    oCarrito.idVuelo.flightNo,
                    oCarrito.cantidad,
                    subtotal
                });
            }

            oVenta.MontoTotal = total;
            oVenta.IdCliente = (int)Session["usuario"];

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;

            return Json(new { Status = true, Link = "/CarritoCompras/PagoEfectuado?idTransaccion=code0001&status=true" }, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> PagoEfectuado()
        {
            string idtransaccion = Request.QueryString["idTransaccion"];
            bool status = Convert.ToBoolean(Request.QueryString["status"]);

            ViewData["Status"] = status;

            if (status)
            {
                Venta oVenta = (Venta)TempData["Venta"];
                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];

                oVenta.IdTransaccion = idtransaccion;

                string mensaje = string.Empty;

                bool respuesta = new Venta().Registrar(oVenta, detalle_venta, out mensaje);

                ViewData["IdTransaccion"] = oVenta.IdTransaccion;

            }

            return View();
        }

    }
}