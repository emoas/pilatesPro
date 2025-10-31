using System;
using System.Collections.Generic;
using System.Text;

namespace Dto.Clases
{
    public sealed class ClaseLightDTO
    {
            public int Id { get; set; }
            public DateTime HorarioInicio { get; set; }
            public bool Activo { get; set; }
            public int CuposTotales { get; set; }
            public int CuposOtorgados { get; set; }
            public int CuposConfirmados { get; set; }
            public ActividadMini Actividad { get; set; }
            public ProfesorMini Profesor { get; set; }
            public LocalMini Local { get; set; }
        }

        public sealed class ActividadMini { public string Nombre { get; set; } }
        public sealed class ProfesorMini { public int Id { get; set; } public string Sobrenombre { get; set; } }
        public sealed class LocalMini { public string Nombre { get; set; } }
}
