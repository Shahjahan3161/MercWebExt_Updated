using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using MercWebExt.Services;
using MercWebExt.Models.ViewModels;
using MercWebExt.Models.DataBase;

namespace MercWebExt.Controllers
{
    public class CareerController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<CareerController> _logger;
        private readonly IContextService _context;

        public CareerController(ILogger<CareerController> logger, IContextService context, IEmailSender emailSender)
        {
            _logger = logger;
            _context = context;
            _emailSender = emailSender;
        }

        // GET: Career List
        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewCareer viewCareer = new ViewCareer(_context.GetDataBaseContext().Career.Where(w => w.IsActivated.Equals(true)).ToList());

            return View(viewCareer);
        }

        // GET: Career/Details/5
        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                _logger.LogError("NotFound");
                return NotFound();
            }

            try
            {
                var career = _context.GetDataBaseContext().Career.Where(w => w.Cid.Equals(id)).First();

                if (career == null)
                {
                    _logger.LogError("No Career");
                    return NotFound();
                }

                ViewApplication viewApplication = new ViewApplication();

                return View(viewApplication);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return RedirectToAction(nameof(Index));
            }
        }


        // GET: Career/Create
        [Authorize(Roles = "Job Admin, Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Career/Create
        [Authorize(Roles = "Job Admin, Admin")]
        [HttpPost]
        public IActionResult Create(Career career)
        {
            if(career.IsNonExpired.Value)
            { 
                career.DateClosing = DateTime.Now.AddYears(100);
            }
                
            var newCareer = new Career
            {
                JobTitle = career.JobTitle,
                Company = career.Company,
                JobDescription = career.JobDescription,
                IsDisplayed = career.IsDisplayed,
                IsNonExpired = career.IsNonExpired,
                DateClosing = career.DateClosing

            };

            _context.GetDataBaseContext().Add(newCareer);
            _context.GetDataBaseContext().SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Job Admin, Admin")]
        // GET: Career/Edit/5
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var career = _context.GetDataBaseContext().Career.Find(id);

                if (career == null)
                {
                    return NotFound();
                }

                return View(career);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Career/Edit/5
        [Authorize(Roles = "Job Admin, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Cid,JobTitle,Company,JobDescription,IsDisplayed,DateClosing,IsActivated,DateCreated,IsNonExpired")]Career career)
        {
            if (id != career.Cid)
            {
                return NotFound();
            }

            _context.GetDataBaseContext().Career.Update(career);
            _context.GetDataBaseContext().SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApplyJob([Bind("JobTitle,FirstName,LastName,ContactNumber,EmailAdd,AvailableDate,Resume")] ViewApplication application)
        {
            string to = "shabirhussain.6122@gmail.com";
            string from = application.EmailAdd;
            string emailName = application.FirstName + " " + application.LastName;
            string subject = "[Job Application for " + application.JobTitle + "] " + emailName;
            string preEnteredMessage = "Job Application:  \n\nI would like to apply for the " + application.JobTitle + " position.\n\nMy contact number is " +
                                   application.ContactNumber + "\n\nI am available to work from " + application.AvailableDate.ToString("dd/MMM/yyyy")
                                   + "\n\nPlease find the attached my resume.\n\nKind Regards\n\n" + emailName;
            string emailMessage = preEnteredMessage;

            try
            {
                _emailSender.SendApplication(from, to, subject, emailName, emailMessage, application.Resume);
                _logger.LogInformation("Sent Application at  " + DateTime.Now.ToShortDateString() + " by " + application.EmailAdd);
                ViewBag.Message = "Sent Application at " + DateTime.Now.ToShortDateString() + " by " + application.EmailAdd;

                ViewCareer viewCareer = new ViewCareer(_context.GetDataBaseContext().Career.Where(w => w.IsActivated.Equals(true)).ToList());

                return View("Index", viewCareer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                ViewBag.Message = "Invaild File or Details Please try it again";

                ViewCareer viewCareer = new ViewCareer(_context.GetDataBaseContext().Career.Where(w => w.IsActivated.Equals(true)).ToList());

                return View("Index", viewCareer);
            }
        }


        [Authorize(Roles = "Admin")]
        // GET: Career/Delete/5
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var career = _context.GetDataBaseContext().Career.First(m => m.Cid == id);
                if (career == null)
                {
                    return NotFound();
                }

                return View(career);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Admin")]
        // POST: Career/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var career = _context.GetDataBaseContext().Career.Find(id);
            _context.GetDataBaseContext().Career.Remove(career);
            _context.GetDataBaseContext().SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool CareerExists(int id)
        {
            return _context.GetDataBaseContext().Career.Any(e => e.Cid == id);
        }
    }
}
