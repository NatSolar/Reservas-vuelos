using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reservas_vuelos.Models
{
    public class Usuario
    {
        public int Cedula { get; set; } //Cédula de identificación
        public string NombreCompleto { get; set; } //Nombre completo
        public string Email { get; set; } //Correo electrónico
        public string Contrasena { get; set; } //Contraseña

        public string FechaRegistro { get; set; } //Fecha de registro

    }

}