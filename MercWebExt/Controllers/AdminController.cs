using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MercWebExt.Services;
using MercWebExt.Models.DataBase;

namespace MercWebExt.Controllers
{
    public class AdminController : Controller
    {
        private readonly IContextService _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger, IContextService context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Induction
        public IActionResult Index()
        {
            return View();
        }

        // GET: Induction/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inductionQuestion = _context.GetDataBaseContext().InductionQuestion.First(m => m.Qid == id);

            if (inductionQuestion == null)
            {
                return NotFound();
            }

            return View(inductionQuestion);
        }

        // GET: Induction/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Induction/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Qid,Qnumber,Question,AnswerLabel1,AnswerLabel2")] InductionQuestion inductionQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.GetDataBaseContext().Add(inductionQuestion);
                _context.GetDataBaseContext().SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(inductionQuestion);
        }

        // GET: Induction/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inductionQuestion = _context.GetDataBaseContext().InductionQuestion.Find(id);

            if (inductionQuestion == null)
            {
                return NotFound();
            }

            return View(inductionQuestion);
        }

        // POST: Induction/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Qid,Qnumber,Question,AnswerLabel1,AnswerLabel2")] InductionQuestion inductionQuestion)
        {
            if (id != inductionQuestion.Qid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.GetDataBaseContext().Update(inductionQuestion);
                    _context.GetDataBaseContext().SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InductionQuestionExists(inductionQuestion.Qid))
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
            return View(inductionQuestion);
        }

        // GET: Induction/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inductionQuestion = _context.GetDataBaseContext().InductionQuestion.First(m => m.Qid == id);

            if (inductionQuestion == null)
            {
                return NotFound();
            }

            return View(inductionQuestion);
        }

        // POST: Induction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var inductionQuestion = _context.GetDataBaseContext().InductionQuestion.Find(id);
            _context.GetDataBaseContext().InductionQuestion.Remove(inductionQuestion);

            _context.GetDataBaseContext().SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private bool InductionQuestionExists(int id)
        {
            return _context.GetDataBaseContext().InductionQuestion.Any(e => e.Qid == id);
        }
    }
}
