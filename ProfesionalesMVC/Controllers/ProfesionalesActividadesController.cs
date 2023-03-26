using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProfesionalesMVC.Models;

namespace ProfesionalesMVC.Controllers
{
    public class ProfesionalesActividadesController : Controller
    {
        private readonly Context _context;

        public ProfesionalesActividadesController(Context context)
        {
            _context = context;
        }

        // GET: ProfesionalesActividades
        public async Task<IActionResult> Index()
        {
            var context = _context.ProfesionalesAvtividades.Include(p => p.Actividad).Include(p => p.Profesional);
            return View(await context.ToListAsync());
        }

        // GET: ProfesionalesActividades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProfesionalesAvtividades == null)
            {
                return NotFound();
            }

            var profesionalActividad = await _context.ProfesionalesAvtividades
                .Include(p => p.Actividad)
                .Include(p => p.Profesional)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profesionalActividad == null)
            {
                return NotFound();
            }

            return View(profesionalActividad);
        }

        // GET: ProfesionalesActividades/Create
        public IActionResult Create()
        {
            ViewData["ActividadId"] = new SelectList(_context.Actividades, "Id", "Nombre");
            ViewData["ProfesionalId"] = new SelectList(_context.Profesionales, "Id", "Nombre");
            return View();
        }

        // POST: ProfesionalesActividades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProfesionalId,ActividadId,PrecioHora")] ProfesionalActividad profesionalActividad, IActionResult? redirect = null)
        {

            if (ModelState.IsValid)
            {
                _context.Add(profesionalActividad);
                await _context.SaveChangesAsync();
                if (redirect != null) return redirect;
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActividadId"] = new SelectList(_context.Actividades, "Id", "Nombre", profesionalActividad.ActividadId);
            ViewData["ProfesionalId"] = new SelectList(_context.Profesionales, "Id", "Nombre", profesionalActividad.ProfesionalId);
            return View(profesionalActividad);
        }

        // GET: ProfesionalesActividades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProfesionalesAvtividades == null)
            {
                return NotFound();
            }

            var profesionalActividad = await _context.ProfesionalesAvtividades.FindAsync(id);
            if (profesionalActividad == null)
            {
                return NotFound();
            }
            ViewData["ActividadId"] = new SelectList(_context.Actividades, "Id", "Nombre", profesionalActividad.ActividadId);
            ViewData["ProfesionalId"] = new SelectList(_context.Profesionales, "Id", "Nombre", profesionalActividad.ProfesionalId);
            return View(profesionalActividad);
        }

        // POST: ProfesionalesActividades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfesionalId,ActividadId,PrecioHora")] ProfesionalActividad profesionalActividad)
        {
            if (id != profesionalActividad.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profesionalActividad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfesionalActividadExists(profesionalActividad.Id))
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

            ViewData["ActividadId"] = new SelectList(_context.Actividades, "Id", "Nombre", profesionalActividad.ActividadId);
            ViewData["ProfesionalId"] = new SelectList(_context.Profesionales, "Id", "Nombre", profesionalActividad.ProfesionalId);
            return View(profesionalActividad);
        }

        // GET: ProfesionalesActividades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProfesionalesAvtividades == null)
            {
                return NotFound();
            }

            var profesionalActividad = await _context.ProfesionalesAvtividades
                .Include(p => p.Actividad)
                .Include(p => p.Profesional)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profesionalActividad == null)
            {
                return NotFound();
            }

            return View(profesionalActividad);
        }

        // POST: ProfesionalesActividades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, IActionResult? redirect = null)
        {
            if (_context.ProfesionalesAvtividades == null)
            {
                return Problem("Entity set 'Context.ProfesionalesAvtividades'  is null.");
            }
            var profesionalActividad = await _context.ProfesionalesAvtividades.FindAsync(id);
            if (profesionalActividad != null)
            {
                _context.ProfesionalesAvtividades.Remove(profesionalActividad);
            }
            
            await _context.SaveChangesAsync();

            if (redirect != null) return redirect;

            return RedirectToAction(nameof(Index));
        }

        private bool ProfesionalActividadExists(int id)
        {
          return (_context.ProfesionalesAvtividades?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
