using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reservas_vuelos.Models
{
    public class BusquedaVuelos
    {
        public List<Vuelo> listaVuelos { get; set; }
        public Vuelo form {  get; set; }
    }
}