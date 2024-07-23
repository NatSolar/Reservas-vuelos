using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace reservas_vuelos.Models
{
    public class Venta
    {
        public int IdCliente { get; set; }
        public int TotalProducto { get; set; }
        public decimal MontoTotal { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public string Nacionalidad { get; set; }
        public string Prefijo { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string IdTransaccion { get; set; }

        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=DESKTOP-0Q3AQFC;Initial Catalog=DB_RESERVAS_VUELOS;Integrated Security=true"))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarOrden", cn);
                    cmd.Parameters.AddWithValue("@IdCliente", obj.IdCliente);
                    cmd.Parameters.AddWithValue("@TotalProducto", obj.TotalProducto);
                    cmd.Parameters.AddWithValue("@MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", obj.Apellido);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", obj.FechaNacimiento.Date);
                    cmd.Parameters.AddWithValue("@Nacionalidad", obj.Nacionalidad);
                    cmd.Parameters.AddWithValue("@Prefijo", obj.Prefijo);
                    cmd.Parameters.AddWithValue("@Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("@CorreoElectronico", obj.CorreoElectronico);
                    cmd.Parameters.AddWithValue("@IdTransaccion", obj.IdTransaccion);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }

            return respuesta;
        }
    }
}