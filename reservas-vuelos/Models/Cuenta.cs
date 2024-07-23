using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reservas_vuelos.Models
{
    public class Cuenta
    {
        public Usuario usuario { get; set; }
        public List<Reserva> reservasViejas { get; set; }
        public List<Reserva> reservasFuturas { get; set; }

    }
}