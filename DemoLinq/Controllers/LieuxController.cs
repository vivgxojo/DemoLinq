using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoLinq.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace DemoLinq.Controllers
{
    public class LieuxController : Controller
    {
        private readonly LieuDBContext _context;

        public LieuxController(LieuDBContext context)
        {
            _context = context;
        }

        // GET: Lieux
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 10; // Nombre d'ingrédients par page
            return View(await PaginatedList<Lieu>.CreateAsync(_context.Lieux.AsNoTracking(),
                pageNumber ?? 1, pageSize)); // Créer une page avec une liste paginée

           //return View(await _context.Lieux.ToListAsync());
        }

        public async Task<IActionResult> Search(string recherche, int? pageNumber)
        {
            ViewBag.Search = recherche;

            // Préparer la requête LINQ
            IQueryable<Lieu> requete = from lieu in _context.Lieux
                                       where lieu.Nom.Contains(recherche)
                                       select lieu;

            int pageSize = 5; // Nombre d'objets par page
            return View("Index", await PaginatedList<Lieu>.CreateAsync(requete.AsNoTracking(),
                pageNumber ?? 1, pageSize)); // Créer une page avec les résultats

            // Exécuter la requête asynchrone
            //IEnumerable<Lieu> result = await requete.ToListAsync();
            //return View("Index", result);
        }
        public async Task<IActionResult> Filtre(string id, int? pageNumber)
        {
            ViewBag.Filtre = id;

            // Préparer la requête LINQ avec expression lambda
            IQueryable<Lieu> requete = _context.Lieux.Where(l => l.Type == id);

            int pageSize = 5; // Nombre d'objets par page
            return View("Index", await PaginatedList<Lieu>.CreateAsync(requete.AsNoTracking(),
                pageNumber ?? 1, pageSize)); // Créer une page avec les résultats

            // Exécuter la requête asynchrone
            //IEnumerable<Lieu> result = await requete.ToListAsync();

            //return View("Index", result);
        }

        public async Task<IActionResult> FiltreTaille(string id, int? pageNumber)
        {
            ViewBag.FiltreTaille = id;

            IQueryable<Lieu> requete;
            // Préparer la requête LINQ avec expression lambda
            if (id == "Grands")
            {
                requete = _context.Lieux.Where(l => l.Superficie >= 70000);
            }
            else if (id == "Moyens")
            {
                requete = _context.Lieux.Where(l => l.Superficie >= 30000 &&
                                                                l.Superficie < 70000);
            }
            else if(id == "Petits")
            {
                requete = _context.Lieux.Where(l => l.Superficie < 30000);
            }
            else
            {
                return NotFound();
            }

            int pageSize = 5; // Nombre d'objets par page
            return View("Index", await PaginatedList<Lieu>.CreateAsync(requete.AsNoTracking(),
                pageNumber ?? 1, pageSize)); // Créer une page avec les résultats

            // Exécuter la requête asynchrone
            //IEnumerable<Lieu> result = await requete.ToListAsync();

            //return View("Index", result);
        }

        // GET: Lieux/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lieu = await _context.Lieux
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lieu == null)
            {
                return NotFound();
            }

            return View(lieu);
        }

        // GET: Lieux/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lieux/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Description,Type,Latitude,Longitude,Superficie")] Lieu lieu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lieu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lieu);
        }

        // GET: Lieux/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lieu = await _context.Lieux.FindAsync(id);
            if (lieu == null)
            {
                return NotFound();
            }
            return View(lieu);
        }

        // POST: Lieux/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Description,Type,Latitude,Longitude,Superficie")] Lieu lieu)
        {
            if (id != lieu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LieuExists(lieu.Id))
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
            return View(lieu);
        }

        // GET: Lieux/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lieu = await _context.Lieux
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lieu == null)
            {
                return NotFound();
            }

            return View(lieu);
        }

        // POST: Lieux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lieu = await _context.Lieux.FindAsync(id);
            if (lieu != null)
            {
                _context.Lieux.Remove(lieu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LieuExists(int id)
        {
            return _context.Lieux.Any(e => e.Id == id);
        }
    }
}
