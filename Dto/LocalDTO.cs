using System;
using System.Collections.Generic;
using System.Text;

namespace Dto
{
    public class LocalDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Url { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
        public LocalDTO()
        {
        }
    }
}
