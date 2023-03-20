﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElyriaAlumniAssociation.Data;
using ElyriaAlumniAssociation.Models;
using ElyriaAlumniAssociation.ViewModels;

namespace ElyriaAlumniAssociation.Controllers
{
    public class AlumniController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlumniController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Alumni
        public async Task<IActionResult> Index()
        {
            if (_context.Alumnus != null)
            {
                List<AlumnusViewModel> alumnusViewModels = new List<AlumnusViewModel>();
                var items = await _context.Alumnus.ToListAsync();
                foreach (var item in items)
                {
                    alumnusViewModels.Add(new AlumnusViewModel
                    {
                        Id = item.Id,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        MiddleInitial = item.MiddleInitial,
                        LastNameAtGraduation = item.LastNameAtGraduation,
                        School = item.School,
                        GraduationYear = item.GraduationYear,
                        StreetAddress = item.StreetAddress,
                        City = item.City,
                        Country = item.Country,
                        PostalCode = item.PostalCode,
                        EmailAddress = item.EmailAddress,
                        PhoneNumber = item.PhoneNumber,
                        ScholasticAward = item.ScholasticAward,
                        Athletics = item.Athletics,
                        Theatre = item.Theatre,
                        Band = item.Band,
                        Choir = item.Choir,
                        Clubs = item.Clubs,
                        ClassOfficer = item.ClassOfficer,
                        ROTC = item.ROTC,
                        OtherActivities = item.OtherActivities,
                        CurrentStatus = item.CurrentStatus

                    });
                }

                return View(alumnusViewModels);
            }

            return Problem("Entity set 'ApplicationDbContext.Alumnus'  is null.");
        }

        // GET: Alumni/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Alumnus == null)
            {
                return NotFound();
            }

            var alumnus = await _context.Alumnus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alumnus == null)
            {
                return NotFound();
            }

            return View(alumnus);
        }

        // GET: Alumni/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Alumni/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,MiddleInitial,LastNameAtGraduation,School,GraduationYear,StreetAddress,City,Country,PostalCode,EmailAddress,PhoneNumber,ScholasticAward,Athletics,Theatre,Band,Choir,Clubs,ClassOfficer,ROTC,OtherActivities,CurrentStatus")] Alumnus alumnus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alumnus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(alumnus);
        }

        // GET: Alumni/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Alumnus == null)
            {
                return NotFound();
            }

            var alumnus = await _context.Alumnus.FindAsync(id);
            if (alumnus == null)
            {
                return NotFound();
            }
            return View(alumnus);
        }

        // POST: Alumni/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,MiddleInitial,LastNameAtGraduation,School,GraduationYear,StreetAddress,City,Country,PostalCode,EmailAddress,PhoneNumber,ScholasticAward,Athletics,Theatre,Band,Choir,Clubs,ClassOfficer,ROTC,OtherActivities,CurrentStatus")] Alumnus alumnus)
        {
            if (id != alumnus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alumnus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlumnusExists(alumnus.Id))
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
            return View(alumnus);
        }

        // GET: Alumni/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Alumnus == null)
            {
                return NotFound();
            }

            var alumnus = await _context.Alumnus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alumnus == null)
            {
                return NotFound();
            }

            return View(alumnus);
        }

        // POST: Alumni/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Alumnus == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Alumnus'  is null.");
            }
            var alumnus = await _context.Alumnus.FindAsync(id);
            if (alumnus != null)
            {
                _context.Alumnus.Remove(alumnus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAlumni(List<AlumnusViewModel>? alumni)
        {
            List<Alumnus> alumniToDelete = new List<Alumnus>();
            foreach (var item in alumni)
            {
                var selectedAlumnus = await _context.Alumnus.FindAsync(item.Id);
                if (selectedAlumnus != null)
                {
                    alumniToDelete.Add(selectedAlumnus);
                }
            }

            _context.Alumnus.RemoveRange(alumniToDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AlumnusExists(int id)
        {
          return (_context.Alumnus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
