using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class WhatsAppMessage
    {
        public string PhoneNumber { get; set; }
        public string TemplateName { get; set; }
        public string LanguageCode { get; set; }  // Ejemplo: "es" para español
        public string BodyText { get; set; }  // Texto que se incluirá en el template
    }
}
