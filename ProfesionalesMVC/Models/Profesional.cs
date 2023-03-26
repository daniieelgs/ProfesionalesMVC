using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProfesionalesMVC.Models {
    [Index(nameof(NIF), IsUnique = true)]
    [Index(nameof(Mail), IsUnique = true)]
    public class Profesional {
        public int Id { get; set; }

        public string Nombre { get; set; }

        [RegularExpression(@"^[0-9]{8}[A-z]{1}$", ErrorMessage = "Please enter a valid NIF"), Remote(action: "ValidateNIF", controller: "ProfessionalsValidation")]

        public string NIF { get; set; }

        [RegularExpression(@"^(\+?[0-9]{2})?[0-9]{9}", ErrorMessage = "Please enter a valid phone number")]
        public string Telefono { get; set; }

        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](<?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")]
        public string Mail { get; set; }

        public bool Activo { get; set; }

        public IList<ProfesionalActividad> ProfesionalesActividades { get; set; } = new List<ProfesionalActividad>();

        public Profesional (string nombre, string nIF, string telefono, string mail, bool activo) {
            Nombre = nombre;
            NIF = nIF;
            Telefono = telefono;
            Mail = mail;
            Activo = activo;
        }

        public Profesional () { }
    }
}
