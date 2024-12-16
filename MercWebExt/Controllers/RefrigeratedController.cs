using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MercWebExt.Models.DataBase;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MercWebExt.Controllers
{
	public class RefrigeratedController : Controller
	{
		private readonly DatabaseContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public RefrigeratedController(DatabaseContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _context.Refrigerated.ToListAsync());
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Title,Description")] Refrigerated refrigerated)
		{
            //if (ModelState.IsValid)
            //{


				refrigerated.CreatedAt = DateTime.Now;
				_context.Add(refrigerated);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			//}
			//return View(categories);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var refrigerated = await _context.Refrigerated.FindAsync(id);
			if (refrigerated == null)
			{
				return NotFound();
			}
			return View(refrigerated);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description, CreatedAt")] Refrigerated refrigerated)
		{
			if (id != refrigerated.ID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(refrigerated);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CategoriesExists(refrigerated.ID))
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
			return View(refrigerated);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var refrigerated = await _context.Refrigerated
                .FirstOrDefaultAsync(m => m.ID == id);
			if (refrigerated == null)
			{
				return NotFound();
			}

			return View(refrigerated);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var refrigerated = await _context.Refrigerated.FindAsync(id);
			_context.Refrigerated.Remove(refrigerated);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool CategoriesExists(int id)
		{
			return _context.Refrigerated.Any(e => e.ID == id);
		}
	}
}
