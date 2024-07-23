using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace reservas_vuelos.Models
{
    public class Carrito
    {
        public int idCarrito { get; set; }

        public Usuario idCliente { get; set; }

        public Vuelo idVuelo { get; set; }

        public int cantidad { get; set; }


        public bool ExisteCarrito(int idCliente, int idVuelo)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=DESKTOP-0Q3AQFC;Initial Catalog=DB_RESERVAS_VUELOS;Integrated Security=true"))
                {

                    SqlCommand cmd = new SqlCommand("sp_existeEnCarrito", cn);
                    cmd.Parameters.AddWithValue("@IdCliente", idCliente);
                    cmd.Parameters.AddWithValue("@IdVuelo", idVuelo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }


        public bool OperacionCarrito(int idCliente, int idVuelo, bool sumar, out string Mensaje)
        {
            bool resultado = true;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=DESKTOP-0Q3AQFC;Initial Catalog=DB_RESERVAS_VUELOS;Integrated Security=true"))
                {

                    SqlCommand cmd = new SqlCommand("sp_operacionCarrito", cn);
                    cmd.Parameters.AddWithValue("@IdCliente", idCliente);
                    cmd.Parameters.AddWithValue("@IdVuelo", idVuelo);
                    cmd.Parameters.AddWithValue("Sumar", sumar);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public int CantidadEnCarrito(int idCliente)
        {
            int resultado = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=DESKTOP-0Q3AQFC;Initial Catalog=DB_RESERVAS_VUELOS;Integrated Security=true"))
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from TBL_SHOPPING_CART where id_cliente = @idCliente ", cn);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.CommandType = CommandType.Text;

                    cn.Open();

                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }

            }
            catch (Exception ex)
            {
                resultado = 0;
            }
            return resultado;
        }

        public List<Carrito> ListarVuelos(int IdCliente)
        {
            List<Carrito> lista = new List<Carrito>();

            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=DESKTOP-0Q3AQFC;Initial Catalog=DB_RESERVAS_VUELOS;Integrated Security=true"))
                {
                    string query = "select * from fn_obtenerCarritoCliente(@IdCliente)";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                    cmd.CommandType = CommandType.Text;

                    cn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Carrito()
                            {
                                idVuelo = new Vuelo((int)reader["flight_no"], (Boolean)reader["oneWay"], (Decimal)reader["duration"],
                                reader["departure_iata_code"].ToString(), reader["departure_terminal"].ToString(),
                                (DateTime)reader["departure_date"], reader["arrival_iata_code"].ToString(), (DateTime)reader["arrival_date"],
                                reader["carrier_code"].ToString(), reader["carrier_name"].ToString(),
                                reader["aircraft_type"].ToString(), (int)reader["stops_no"], reader["currency"].ToString(),
                                (Decimal)reader["total"], reader["cabin"].ToString(), reader["cabin_fare"].ToString())
                                ,
                                cantidad = (int)reader["cantidad"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex) {
                lista = new List<Carrito>();
            }

            return lista;
        }


        public bool EliminarCarrito(int idCliente, int idVuelo)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=DESKTOP-0Q3AQFC;Initial Catalog=DB_RESERVAS_VUELOS;Integrated Security=true"))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarCarrito", cn);
                    cmd.Parameters.AddWithValue("@IdCliente", idCliente);
                    cmd.Parameters.AddWithValue("@IdVuelo", idVuelo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }


    }

}