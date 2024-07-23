using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reservas_vuelos.Models;
using Reservas_vuelos.Permisos;
using System.Runtime.Remoting.Messaging;


namespace reservas_vuelos.Controllers
{
    [ValidateSesion]
    public class HomeController : Controller
    {

        static string cadena = "Data Source=DESKTOP-0Q3AQFC;Initial Catalog=DB_RESERVAS_VUELOS;Integrated Security=true";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CerrarSesion()
        {
            Session["usuario"] = null;
            return RedirectToAction("InicioSesion", "Acceso");
        }

        public ActionResult Cuenta(int id)
        {

            Cuenta cuenta = new Cuenta();
            Usuario usuario = new Usuario();
            usuario.Cedula = id;

            usuario = getUsuario(id);

            if (usuario.Cedula != 0)
            {
                cuenta.usuario = usuario;
                cuenta.reservasFuturas = getReservasFuturas(usuario);
                cuenta.reservasViejas = getReservasViejas(usuario);

                return View(cuenta);
            }
            else
            {
                return View();
            }
        }


        public Usuario getUsuario(int userIndNo)
        {
            Usuario usuarioTemp = new Usuario();

            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_TraerInfoUsuario", cn);
                cmd.Parameters.AddWithValue("@UserIndNo", userIndNo);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            usuarioTemp.NombreCompleto = reader["name_user"].ToString();
                            usuarioTemp.Email = reader["email_user"].ToString();
                            usuarioTemp.Cedula = userIndNo;
                            usuarioTemp.FechaRegistro = reader["month_dt"].ToString() + ", " + reader["year_dt"].ToString();
                        }
                    }
                }

                cn.Close();
            }

            return usuarioTemp;
        }

        public List<Reserva> getReservasFuturas(Usuario usuario)
        {
            List<Reserva> reservasFuturas = new List<Reserva>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_getFutureReserves", cn);
                cmd.Parameters.AddWithValue("@user_ind_no", usuario.Cedula);
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
                                Vuelo vueloTemp = new Vuelo((int)reader["flight_no"], (Boolean)reader["oneWay"], (Decimal)reader["duration"],
                                reader["departure_iata_code"].ToString(), reader["departure_terminal"].ToString(),
                                (DateTime)reader["departure_date"], reader["arrival_iata_code"].ToString(), (DateTime)reader["arrival_date"],
                                reader["carrier_code"].ToString(), reader["carrier_name"].ToString(),
                                reader["aircraft_type"].ToString(), (int)reader["stops_no"], reader["currency"].ToString(),
                                (Decimal)reader["total"], reader["cabin"].ToString(), reader["cabin_fare"].ToString());

                                Reserva reservaTemp = new Reserva((int)reader["reserve_no"], vueloTemp, usuario, (int)reader["passengers_no"]);

                                reservasFuturas.Add(reservaTemp);
                            }
                        }
                    }
                }

                cn.Close();
            }

            return reservasFuturas;
        }

        public List<Reserva> getReservasViejas(Usuario usuario)
        {
            List<Reserva> reservasViejas = new List<Reserva>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_getPastReserves", cn);
                cmd.Parameters.AddWithValue("@user_ind_no", usuario.Cedula);
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
                                Vuelo vueloTemp = new Vuelo((int)reader["flight_no"], (Boolean)reader["oneWay"], (Decimal)reader["duration"],
                                reader["departure_iata_code"].ToString(), reader["departure_terminal"].ToString(),
                                (DateTime)reader["departure_date"], reader["arrival_iata_code"].ToString(), (DateTime)reader["arrival_date"],
                                reader["carrier_code"].ToString(), reader["carrier_name"].ToString(),
                                reader["aircraft_type"].ToString(), (int)reader["stops_no"], reader["currency"].ToString(),
                                (Decimal)reader["total"], reader["cabin"].ToString(), reader["cabin_fare"].ToString());

                                Reserva reservaTemp = new Reserva((int)reader["reserve_no"], vueloTemp, usuario, (int)reader["passengers_no"]);

                                reservasViejas.Add(reservaTemp);
                            }
                        }
                    }
                }

                cn.Close();
            }

            return reservasViejas;
        }


    }

}