using Microsoft.EntityFrameworkCore;

namespace ProfesionalesMVC.Models {
    public class Context : DbContext{

        public DbSet<Profesional> Profesionales { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<ProfesionalActividad> ProfesionalesAvtividades { get; set; }


        public Context () { }

        public Context (DbContextOptions<Context> options) : base(options) { }

        }
    }
