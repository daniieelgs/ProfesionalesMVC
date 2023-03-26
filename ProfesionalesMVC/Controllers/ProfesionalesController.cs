using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProfesionalesMVC.Models;

namespace ProfesionalesMVC.Controllers
{
    public class ProfesionalesController : Controller
    {
        private readonly Context _context;

        public ProfesionalesController(Context context)
        {
            _context = context;
        }

        // GET: Profesionales
        public async Task<IActionResult> Index(string? search, string? field)
        {
            
            search = search?.Trim();
            field = field?.Trim();

            search = search == "" ? null : search;
            field = field == "" || field == "Todos" ? null : field;

            ViewData ["Fields"] = new SelectList(typeof(Profesional).GetProperties().Where(x => x.PropertyType != typeof(bool) && !typeof(IEnumerable<Object>).IsAssignableFrom(x.PropertyType)).Select(x => x.Name).Append("Todos"), field ?? "Todos");

            return _context.Profesionales != null ? 
                          View(search == null ? await _context.Profesionales.ToListAsync() : 
                            (
                            field == null ?
                                await _context.Profesionales.Where(x => x.Nombre.Contains(search)).ToListAsync()
                                : await _context.SearchBy<Profesional>(field, search, _context.Profesionales)
                            ))
                          : Problem("Entity set 'Context.Profesionales'  is null.");
        }

        // GET: Profesionales/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.Profesionales == null)
            {
                return NotFound();
            }

            IEnumerable<ProfesionalActividad?> actividadesDone = _context.Profesionales.Include(x => x.ProfesionalesActividades).ThenInclude(x => x.Actividad).First(x => x.Id == id).ProfesionalesActividades;
            ViewData ["ActivitiesDone"] = actividadesDone;

            var actividades = await _context.Actividades.ToListAsync();

            ViewData ["Activities"] = new SelectList(actividades.Except(actividadesDone.Select(x => x.Actividad)), "Id", "Nombre");

            var profesional = await _context.Profesionales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profesional == null)
            {
                return NotFound();
            }

            return View(profesional);
        }

        [AcceptVerbs("POST")]
        public async Task<IActionResult> AddActivity ([Bind("Id,ProfesionalId,ActividadId,PrecioHora")] ProfesionalActividad profesionalActividad) {

            if (ModelState.IsValid) {
                return await new ProfesionalesActividadesController(_context).Create(profesionalActividad, RedirectToAction(nameof(Details), new { Id = profesionalActividad.ProfesionalId }));
                //return RedirectToAction("Create", "ProfesionalesActividades", new { profesionalActividad = profesionalActividad, redirect = RedirectToAction(nameof(Details), new { Id = profesionalActividad.ProfesionalId }) });
            }

            return RedirectToAction(nameof(Details), new { Id = profesionalActividad.ProfesionalId});
        }

        public async Task<IActionResult> RemoveActivity (int? id) {

            if (id == null) return NotFound();


            int idProfesional = _context.ProfesionalesAvtividades.Include(x => x.Profesional).FirstOrDefault(x => x.Id == id).Profesional.Id;

            return await new ProfesionalesActividadesController(_context).DeleteConfirmed((int)id, RedirectToAction(nameof(Details), new { Id = idProfesional }));
        }

        // GET: Profesionales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Profesionales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,NIF,Telefono,Mail,Activo")] Profesional profesional)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profesional);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profesional);
        }

        // GET: Profesionales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Profesionales == null)
            {
                return NotFound();
            }

            var profesional = await _context.Profesionales.FindAsync(id);
            if (profesional == null)
            {
                return NotFound();
            }
            return View(profesional);
        }

        // POST: Profesionales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,NIF,Telefono,Mail,Activo")] Profesional profesional)
        {
            if (id != profesional.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profesional);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfesionalExists(profesional.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(profesional);
        }

        // GET: Profesionales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Profesionales == null)
            {
                return NotFound();
            }

            var profesional = await _context.Profesionales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profesional == null)
            {
                return NotFound();
            }

            return View(profesional);
        }

        // POST: Profesionales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Profesionales == null)
            {
                return Problem("Entity set 'Context.Profesionales'  is null.");
            }
            var profesional = await _context.Profesionales.FindAsync(id);
            if (profesional != null)
            {
                _context.Profesionales.Remove(profesional);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfesionalExists(int id)
        {
          return (_context.Profesionales?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
