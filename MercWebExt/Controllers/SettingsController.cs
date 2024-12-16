using MercWebExt.Models.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class SettingsController : Controller
{
    private readonly DatabaseContext _context;

    public SettingsController(DatabaseContext context)
    {
        _context = context;
    }

    // GET: Settings
    public IActionResult Index()
    {
        var result = _context.Settings.ToList();
        return View(result);
    }

    // GET: Settings/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Settings/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PhoneNumber,Email,Address,FacebookLink,TwitterLink,LogoImage,BackgroundImage,FrontAdImage")] Settings settings)
    {
        if (ModelState.IsValid)
        {
            if (settings.LogoImage != null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", settings.LogoImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await settings.LogoImage.CopyToAsync(stream);
                }
                settings.LogoImagePath = "/images/" + settings.LogoImage.FileName;
            }

            if (settings.BackgroundImage != null)
            {
                var backgroundPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", settings.BackgroundImage.FileName);
                using (var stream = new FileStream(backgroundPath, FileMode.Create))
                {
                    await settings.BackgroundImage.CopyToAsync(stream);
                }
                settings.BackgroundImagePath = "/images/" + settings.BackgroundImage.FileName;
            }

            if (settings.FrontAdImage != null)
            {
                var adPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", settings.FrontAdImage.FileName);
                using (var stream = new FileStream(adPath, FileMode.Create))
                {
                    await settings.FrontAdImage.CopyToAsync(stream);
                }
                settings.FrontAdImagePath = "/images/" + settings.FrontAdImage.FileName;
            }

            _context.Add(settings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(settings);
    }

    // GET: Settings/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var settings = await _context.Settings.FindAsync(id);
        if (settings == null)
        {
            return NotFound();
        }
        return View(settings);
    }

    // POST: Settings/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ID,PhoneNumber,Email,Address,FacebookLink,TwitterLink,LogoImage,LogoImagePath,BackgroundImage,BackgroundImagePath,FrontAdImage,FrontAdImagePath")] Settings settings)
    {
        if (id != settings.ID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingSettings = await _context.Settings.AsNoTracking().FirstOrDefaultAsync(s => s.ID == id);
                if (existingSettings == null)
                {
                    return NotFound();
                }

                if (settings.LogoImage != null)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", settings.LogoImage.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await settings.LogoImage.CopyToAsync(stream);
                    }
                    settings.LogoImagePath = "/images/" + settings.LogoImage.FileName;
                }
                else
                {
                    settings.LogoImagePath = existingSettings.LogoImagePath;
                }

                if (settings.BackgroundImage != null)
                {
                    var backgroundPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", settings.BackgroundImage.FileName);
                    using (var stream = new FileStream(backgroundPath, FileMode.Create))
                    {
                        await settings.BackgroundImage.CopyToAsync(stream);
                    }
                    settings.BackgroundImagePath = "/images/" + settings.BackgroundImage.FileName;
                }
                else
                {
                    settings.BackgroundImagePath = existingSettings.BackgroundImagePath;
                }

                if (settings.FrontAdImage != null)
                {
                    var adPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", settings.FrontAdImage.FileName);
                    using (var stream = new FileStream(adPath, FileMode.Create))
                    {
                        await settings.FrontAdImage.CopyToAsync(stream);
                    }
                    settings.FrontAdImagePath = "/images/" + settings.FrontAdImage.FileName;
                }
                else
                {
                    settings.FrontAdImagePath = existingSettings.FrontAdImagePath;
                }

                _context.Update(settings);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettingsExists(settings.ID))
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
        return View(settings);
    }

    // GET: Settings/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var settings = _context.Settings
            .FirstOrDefault(m => m.ID == id);
        if (settings == null)
        {
            return NotFound();
        }

        return View(settings);
    }

    // POST: Settings/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var settings = await _context.Settings.FindAsync(id);
        _context.Settings.Remove(settings);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SettingsExists(int id)
    {
        return _context.Settings.Any(e => e.ID == id);
    }
}


//using MercWebExt.Models.DataBase;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//public class SettingsController : Controller
//{
//	private readonly DatabaseContext _context;

//	public SettingsController(DatabaseContext context)
//	{
//		_context = context;
//	}

//	// GET: Settings
//	public IActionResult Index()
//	{
//		var result = _context.Settings.ToList();
//		return View(result);
//	}

//	// GET: Settings/Create
//	public IActionResult Create()
//	{
//		return View();
//	}

//	// POST: Settings/Create
//	[HttpPost]
//	[ValidateAntiForgeryToken]
//	public async Task<IActionResult> Create([Bind("PhoneNumber,Email,Address,FacebookLink,TwitterLink,LogoImage,BackgroundImage")] Settings settings)
//	{
//		if (ModelState.IsValid)
//		{
//			if (settings.LogoImage != null)
//			{
//				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", settings.LogoImage.FileName);
//				using (var stream = new FileStream(filePath, FileMode.Create))
//				{
//					await settings.LogoImage.CopyToAsync(stream);
//				}
//				settings.LogoImagePath = "/images/" + settings.LogoImage.FileName;
//			}

//			if (settings.BackgroundImage != null)
//			{
//				var backgroundPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", settings.BackgroundImage.FileName);
//				using (var stream = new FileStream(backgroundPath, FileMode.Create))
//				{
//					await settings.BackgroundImage.CopyToAsync(stream);
//				}
//				settings.BackgroundImagePath = "/images/" + settings.BackgroundImage.FileName;
//			}

//			_context.Add(settings);
//			await _context.SaveChangesAsync();
//			return RedirectToAction(nameof(Index));
//		}
//		return View(settings);
//	}

//	// GET: Settings/Edit/5
//	public async Task<IActionResult> Edit(int? id)
//	{
//		if (id == null)
//		{
//			return NotFound();
//		}

//		var settings = await _context.Settings.FindAsync(id);
//		if (settings == null)
//		{
//			return NotFound();
//		}
//		return View(settings);
//	}

//    // POST: Settings/Edit/5
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Edit(int id, [Bind("ID,PhoneNumber,Email,Address,FacebookLink,TwitterLink,LogoImage,LogoImagePath,BackgroundImage,BackgroundImagePath")] Settings settings)
//    {
//        if (id != settings.ID)
//        {
//            return NotFound();
//        }

//        if (ModelState.IsValid)
//        {
//            try
//            {
//                var existingSettings = await _context.Settings.AsNoTracking().FirstOrDefaultAsync(s => s.ID == id);
//                if (existingSettings == null)
//                {
//                    return NotFound();
//                }

//                if (settings.LogoImage != null)
//                {
//                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", settings.LogoImage.FileName);
//                    using (var stream = new FileStream(filePath, FileMode.Create))
//                    {
//                        await settings.LogoImage.CopyToAsync(stream);
//                    }
//                    settings.LogoImagePath = "/images/" + settings.LogoImage.FileName;
//                }
//                else
//                {
//                    settings.LogoImagePath = existingSettings.LogoImagePath;
//                }

//                if (settings.BackgroundImage != null)
//                {
//                    var backgroundPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", settings.BackgroundImage.FileName);
//                    using (var stream = new FileStream(backgroundPath, FileMode.Create))
//                    {
//                        await settings.BackgroundImage.CopyToAsync(stream);
//                    }
//                    settings.BackgroundImagePath = "/images/" + settings.BackgroundImage.FileName;
//                }
//                else
//                {
//                    settings.BackgroundImagePath = existingSettings.BackgroundImagePath;
//                }

//                _context.Update(settings);
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!SettingsExists(settings.ID))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//            return RedirectToAction(nameof(Index));
//        }
//        return View(settings);
//    }





//    // working
//    // POST: Settings/Edit/5
//    //[HttpPost]
//    //[ValidateAntiForgeryToken]
//    //public async Task<IActionResult> Edit(int id, [Bind("ID,PhoneNumber,Email,Address,FacebookLink,TwitterLink,LogoImage,LogoImagePath,BackgroundImage,BackgroundImagePath")] Settings settings)
//    //{
//    //	if (id != settings.ID)
//    //	{
//    //		return NotFound();
//    //	}

//    //	if (ModelState.IsValid)
//    //	{
//    //		try
//    //		{
//    //			if (settings.LogoImage != null)
//    //			{
//    //				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", settings.LogoImage.FileName);
//    //				using (var stream = new FileStream(filePath, FileMode.Create))
//    //				{
//    //					await settings.LogoImage.CopyToAsync(stream);
//    //				}
//    //				settings.LogoImagePath = "/images/" + settings.LogoImage.FileName;
//    //			}

//    //			if (settings.BackgroundImage != null)
//    //			{
//    //				var backgroundPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", settings.BackgroundImage.FileName);
//    //				using (var stream = new FileStream(backgroundPath, FileMode.Create))
//    //				{
//    //					await settings.BackgroundImage.CopyToAsync(stream);
//    //				}
//    //				settings.BackgroundImagePath = "/images/" + settings.BackgroundImage.FileName;
//    //			}

//    //			_context.Update(settings);
//    //			await _context.SaveChangesAsync();
//    //		}
//    //		catch (DbUpdateConcurrencyException)
//    //		{
//    //			if (!SettingsExists(settings.ID))
//    //			{
//    //				return NotFound();
//    //			}
//    //			else
//    //			{
//    //				throw;
//    //			}
//    //		}
//    //		return RedirectToAction(nameof(Index));
//    //	}
//    //	return View(settings);
//    //}

//    // GET: Settings/Delete/5
//    public IActionResult Delete(int? id)
//	{
//		if (id == null)
//		{
//			return NotFound();
//		}

//		var settings = _context.Settings
//			.FirstOrDefault(m => m.ID == id);
//		if (settings == null)
//		{
//			return NotFound();
//		}

//		return View(settings);
//	}

//	// POST: Settings/Delete/5
//	[HttpPost, ActionName("Delete")]
//	[ValidateAntiForgeryToken]
//	public async Task<IActionResult> DeleteConfirmed(int id)
//	{
//		var settings = await _context.Settings.FindAsync(id);
//		_context.Settings.Remove(settings);
//		await _context.SaveChangesAsync();
//		return RedirectToAction(nameof(Index));
//	}

//	private bool SettingsExists(int id)
//	{
//		return _context.Settings.Any(e => e.ID == id);
//	}
//}
