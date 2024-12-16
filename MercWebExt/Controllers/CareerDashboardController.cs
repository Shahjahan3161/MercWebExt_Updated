using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MercWebExt.Models.DataBase;
using MercWebExt.Models.ViewModels;
using MercWebExt.Services;

namespace MercWebExt.Controllers
{
    public class CareerDashboardController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<CareerDashboardController> _logger;
        private readonly IContextService _contextService;


        public CareerDashboardController(DatabaseContext context, ILogger<CareerDashboardController> logger, IContextService contextService)
        {
            _context = context;
            _logger = logger;
            _contextService = contextService;
        }

        public IActionResult Index()
        {
            var careers = _context.Career.ToList();
            return View(careers);
        }

     public IActionResult Applicants(string category)
{
    var inductionQuestions = _contextService.GetDataBaseContext().InductionInduction
        .Where(i => category == null || i.Category == category)
        .Select(i => new
        {
            LastName = i.LastName ?? string.Empty,
            FirstName = i.FirstName ?? string.Empty,
            DriverEmail = i.DriverEmail ?? string.Empty,
            DriverMobile = i.DriverMobile ?? string.Empty,
            RegoNumber = i.RegoNumber ?? string.Empty,
            DateCreated = i.DateCreated,
            UserId = i.UserId ?? string.Empty,
            ForkliftNo = i.ForkliftNo ?? string.Empty,
            Comments = i.Comments ?? string.Empty,
            Category = i.Category ?? string.Empty,
            Response1 = i.Response1 ?? string.Empty,
            Response2 = i.Response2 ?? string.Empty,
            Company = i.Company ?? string.Empty
        })
        .OrderByDescending(i => i.DateCreated)
        .ToList();

    ViewBag.Category = category; // Pass the selected category to the view

    return View(inductionQuestions);
}


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Career career)
        {
            if (ModelState.IsValid)
            {
                _context.Career.Add(career);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(career);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var career = _context.Career.Find(id);
            if (career == null)
            {
                return NotFound();
            }

            return View(career);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Cid,JobTitle,Company,JobDescription,IsDisplayed,DateClosing,IsActivated,DateCreated,IsNonExpired")] Career career)
        {
            if (id != career.Cid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(career);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CareerExists(career.Cid))
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
            return View(career);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var career = _context.Career.Find(id);
            if (career == null)
            {
                return NotFound();
            }

            return View(career);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var career = await _context.Career.FindAsync(id);
            if (career != null)
            {
                _context.Career.Remove(career);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CareerExists(int id)
        {
            return _context.Career.Any(e => e.Cid == id);
        }

        // Add the new action for listing applicants
        //public async Task<IActionResult> Applicants()
        //{
        //    var applicants = await _context.Career
        //                                   .Select(c => new ViewApplication
        //                                   {
        //                                       Career = c,
        //                                       FirstName = c.FirstName,
        //                                       LastName = c.LastName,
        //                                       ContactNumber = c.ContactNumber,
        //                                       EmailAdd = c.EmailAdd,
        //                                       AvailableDate = c.AvailableDate,
        //                                       ResumePath = null // Assuming the resume path is not displayed in the list view
        //                                   }).ToListAsync();

        //    return View(applicants);
        //}
    }
}
