using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reservas_vuelos.Models
{
    public class Reserva
    {
        public int reserveNo { get; set; }
        public Vuelo vuelo { get; set; }
        public Usuario usuario { get; set; }
        public int passengersNo { get; set; }

        public Reserva(int reserve_no, Vuelo vuelo, Usuario usuario, int passengers_no) {
        
            this.reserveNo = reserve_no;
            this.vuelo = vuelo;
            this.usuario = usuario;
            this.passengersNo = passengers_no;
        }

    }
}