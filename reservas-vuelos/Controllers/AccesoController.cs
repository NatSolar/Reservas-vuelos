using reservas_vuelos.Models;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;

namespace reservas_vuelos.Controllers
{
    public class AccesoController : Controller
    {

        static string cadena = "Data Source=DESKTOP-0Q3AQFC;Initial Catalog=DB_RESERVAS_VUELOS;Integrated Security=true";

        public ActionResult InicioSesion()
        {
            return View();
        }

        public ActionResult CrearCuenta()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CrearCuenta(Usuario pUsuario)
        {
            bool registrado;
            string mensaje;

            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn);
                cmd.Parameters.AddWithValue("name_user", pUsuario.NombreCompleto);
                cmd.Parameters.AddWithValue("email_user", pUsuario.Email);
                cmd.Parameters.AddWithValue("password_user", pUsuario.Contrasena);
                cmd.Parameters.AddWithValue("user_ind_no", pUsuario.Cedula);

                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                cn.Close();

            }

            if (registrado)
            {
                ViewData["Mensaje"] = mensaje;
                return RedirectToAction("InicioSesion", "Acceso");
            }
            else
            {
                return View();
            }

        }


        [HttpPost]
        public ActionResult InicioSesion(Usuario pUsuario)
        {

            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", cn);
                cmd.Parameters.AddWithValue("email_user", pUsuario.Email);
                cmd.Parameters.AddWithValue("password_user", pUsuario.Contrasena);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                pUsuario.Cedula = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                cn.Close();
            }

            if (pUsuario.Cedula != 0)
            {

                Session["usuario"] = pUsuario.Cedula;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "Usuario no encontrado.";
                return View();
            }
        }

    }
}