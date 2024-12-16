using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MercWebExt.Models.DataBase;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MercWebExt.Controllers
{
	public class AboutUsController : Controller
	{
		private readonly DatabaseContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public AboutUsController(DatabaseContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _context.AboutUs.ToListAsync());
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Title,Description")] AboutUs aboutUs)
		{
            //if (ModelState.IsValid)
            //{


				aboutUs.CreatedAt = DateTime.Now;
				_context.Add(aboutUs);
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

			var aboutUs = await _context.AboutUs.FindAsync(id);
			if (aboutUs == null)
			{
				return NotFound();
			}
			return View(aboutUs);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description, CreatedAt")] AboutUs aboutUs)
		{
			if (id != aboutUs.ID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(aboutUs);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CategoriesExists(aboutUs.ID))
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
			return View(aboutUs);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var aboutUs = await _context.AboutUs
                .FirstOrDefaultAsync(m => m.ID == id);
			if (aboutUs == null)
			{
				return NotFound();
			}

			return View(aboutUs);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var aboutUs = await _context.AboutUs.FindAsync(id);
			_context.AboutUs.Remove(aboutUs);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool CategoriesExists(int id)
		{
			return _context.AboutUs.Any(e => e.ID == id);
		}
	}
}
