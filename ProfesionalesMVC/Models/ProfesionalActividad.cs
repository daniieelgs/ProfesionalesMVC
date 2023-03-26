using System.ComponentModel.DataAnnotations;

namespace ProfesionalesMVC.Models {
    public class ProfesionalActividad {

        public int Id { get; set; }

        public int ProfesionalId { get; set; }
        public int ActividadId { get; set; }

        public Profesional? Profesional { get; set; }
        public Actividad? Actividad { get; set; }
       private double precioHora;

        [Range(0, Double.MaxValue)]
        public double PrecioHora { get => precioHora; set {precioHora = value >= 0 ? value : throw new ArgumentException("El precio/hora no puede ser negativo"); } }

        public ProfesionalActividad (int profesionalId, int actividadId, double precioHora) {
            ProfesionalId = profesionalId;
            ActividadId = actividadId;
            PrecioHora = precioHora;
        }

        public ProfesionalActividad (Profesional profesional, Actividad actividad, double precioHora) {
            Profesional = profesional;
            Actividad = actividad;
            PrecioHora = precioHora;
        }

        public ProfesionalActividad () { }

    }
}
