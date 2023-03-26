using Microsoft.EntityFrameworkCore;

namespace ProfesionalesMVC.Models {
    [Index(nameof(Nombre), IsUnique = true)]
    public class Actividad {

        public int Id { get; set; }

        public string Nombre { get; set; }

        public IList<ProfesionalActividad> ProfesionalesActividades { get; set; } = new List<ProfesionalActividad>();

        public Actividad (string nombre) {
            Nombre = nombre;
        }

        public Actividad () { }
    }
}
