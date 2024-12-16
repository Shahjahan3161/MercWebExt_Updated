using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MercWebExt.Models.DataBase;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MercWebExt.Controllers
{
	public class DashboardCategories3Controller : Controller
	{
		private readonly DatabaseContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public DashboardCategories3Controller(DatabaseContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		// GET: DashboardCategories
		public async Task<IActionResult> Index()
		{
			return View(await _context.Categories3.ToListAsync());
		}

		// GET: DashboardCategories/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: DashboardCategories/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("CategoryName,Description,Image")] Categories3 categories)
		{
			//if (ModelState.IsValid)
			//{
				if (categories.Image != null)
				{
					string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
					string uniqueFileName = Guid.NewGuid().ToString() + "_" + categories.Image.FileName;
					string filePath = Path.Combine(uploadsFolder, uniqueFileName);
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await categories.Image.CopyToAsync(fileStream);
					}
					categories.ImagePath = "/images/" + uniqueFileName;
				}

				categories.CreatedAt = DateTime.Now;
				_context.Add(categories);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			//}
			//return View(categories);
		}

		// GET: DashboardCategories/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var categories = await _context.Categories3.FindAsync(id);
			if (categories == null)
			{
				return NotFound();
			}
			return View(categories);
		}

        // POST: DashboardCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CategoryName,Description,ImagePath,CreatedAt,Image")] Categories3 categories)
        {
            if (id != categories.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (categories.Image != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + categories.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await categories.Image.CopyToAsync(fileStream);
                    }
                    categories.ImagePath = "/images/" + uniqueFileName;
                }
                try
                {
                    _context.Update(categories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriesExists(categories.ID))
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
            return View(categories);
        }

        // GET: DashboardCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var categories = await _context.Categories3
				.FirstOrDefaultAsync(m => m.ID == id);
			if (categories == null)
			{
				return NotFound();
			}

			return View(categories);
		}

		// POST: DashboardCategories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var categories = await _context.Categories3.FindAsync(id);
			_context.Categories3.Remove(categories);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool CategoriesExists(int id)
		{
			return _context.Categories3.Any(e => e.ID == id);
		}
	}
}
