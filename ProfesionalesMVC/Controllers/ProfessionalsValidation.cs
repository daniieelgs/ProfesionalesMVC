using Microsoft.AspNetCore.Mvc;
using ProfesionalesMVC.Models;

namespace ProfesionalesMVC.Controllers {
    public class ProfessionalsValidation : Controller {

        private readonly Context _context;

        public ProfessionalsValidation (Context context) {
            _context = context;
        }

        //[AcceptVerbs("GET", "POST")]
        public IActionResult ValidateNIF (string nif) => _context.Profesionales.Any(x => x.NIF == nif) ? Json($"NIF '{nif}' ya registrado") : Json(true);

    }
}
