using reservas_vuelos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web.Mvc;


namespace reservas_vuelos.Controllers
{
    public class VueloController : Controller
    {
        static string cadena = "Data Source=DESKTOP-0Q3AQFC;Initial Catalog=DB_RESERVAS_VUELOS;Integrated Security=true";

        // GET: Vuelo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VuelosDisponibles()
        {
            return View();
        }


        public ActionResult TraerVuelos(Vuelo vueloTemp)
        {
            List<Vuelo> listaVuelos = new List<Vuelo>();
            BusquedaVuelos busqueda = new BusquedaVuelos();

            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_getAllFlights", cn);
                cmd.Parameters.AddWithValue("@departureIataCode", vueloTemp.departureIataCode);
                cmd.Parameters.AddWithValue("@arrivalIataCode", vueloTemp.arrivalIataCode);
                cmd.Parameters.AddWithValue("@departureDate", String.Format("{0:yyyy-MM-dd}", vueloTemp.departureDate));
                cmd.Parameters.AddWithValue("@arrivalDate", String.Format("{0:yyyy-MM-dd}", vueloTemp.arrivalDate));
                cmd.Parameters.AddWithValue("@one_way", vueloTemp.oneWay);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (!reader["flight_no"].ToString().Equals("0"))
                            {
                                Vuelo vueloT = new Vuelo((int)reader["flight_no"], (Boolean)reader["oneWay"], (Decimal)reader["duration"],
                                reader["departure_iata_code"].ToString(), reader["departure_terminal"].ToString(),
                                (DateTime)reader["departure_date"], reader["arrival_iata_code"].ToString(), (DateTime)reader["arrival_date"],
                                reader["carrier_code"].ToString(), reader["carrier_name"].ToString(),
                                reader["aircraft_type"].ToString(), (int)reader["stops_no"], reader["currency"].ToString(),
                                (Decimal)reader["total"], reader["cabin"].ToString(), reader["cabin_fare"].ToString());

                                listaVuelos.Add(vueloT);

                            }
                        }
                    }
                }

                cn.Close();
            }

            if (listaVuelos != null && listaVuelos.Count() > 0)
            {
                busqueda.listaVuelos = listaVuelos;
                busqueda.form = vueloTemp;

                return View(busqueda);
            }
            else
            {
                return View();
            }
        }


        [HttpPost]
        public JsonResult AgregarCarrito(int idVuelo)
        {
            int idCliente = (int)Session["usuario"];

            bool existe = new Carrito().ExisteCarrito(idCliente, idVuelo);

            bool respuesta = false;

            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito.";
            } else
            {
                respuesta = new Carrito().OperacionCarrito(idCliente, idVuelo, true, out mensaje);
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]

        public JsonResult CantidadEnCarrito()
        {
            int idCliente = (int)Session["usuario"];
            int cantidad = new Carrito().CantidadEnCarrito(idCliente);
            return Json(new {cantidad = cantidad}, JsonRequestBehavior.AllowGet);
        }

    }

}