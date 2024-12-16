using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MercWebExt.Models.DataBase;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MercWebExt.Migrations;

namespace MercWebExt.Controllers
{
	public class CertifiedController : Controller
	{
		private readonly DatabaseContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public CertifiedController(DatabaseContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _context.Certifieds.ToListAsync());
		}

		public IActionResult Create()
		{
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Images")] Certified certified)
        {
            if (certified.Images != null && certified.Images.Count > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                List<string> imagePaths = new List<string>();

                foreach (var image in certified.Images)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    imagePaths.Add("/images/" + uniqueFileName);
                }
                certified.ImagePaths = string.Join(",", imagePaths);
            }

            certified.CreatedAt = DateTime.Now;
            _context.Add(certified);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var certifieds = await _context.Certifieds.FindAsync(id);
			if (certifieds == null)
			{
				return NotFound();
			}
			return View(certifieds);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description,CreatedAt,Images")] Certified certified)
        {
            if (id != certified.ID)
            {
                return NotFound();
            }

            if (certified.Images != null && certified.Images.Count > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                List<string> imagePaths = new List<string>();

                foreach (var image in certified.Images)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    imagePaths.Add("/images/" + uniqueFileName);
                }
                certified.ImagePaths = string.Join(",", imagePaths);
            }
            else
            {
                // Preserve the existing image paths if no new images are uploaded
                _context.Entry(certified).Property(x => x.ImagePaths).IsModified = false;
            }

            try
            {
                _context.Update(certified);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertifiedExists(certified.ID))
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

        public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var certifieds = await _context.Certifieds
                .FirstOrDefaultAsync(m => m.ID == id);
			if (certifieds == null)
			{
				return NotFound();
			}

			return View(certifieds);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var certifieds = await _context.Certifieds.FindAsync(id);
			_context.Certifieds.Remove(certifieds);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool CertifiedExists(int id)
		{
			return _context.Certifieds.Any(e => e.ID == id);
		}
	}
}
