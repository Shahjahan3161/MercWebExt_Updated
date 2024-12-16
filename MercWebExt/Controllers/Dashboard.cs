using MercWebExt.Models.DataBase;
using MercWebExt.Models.ViewModels;
using MercWebExt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity.Infrastructure;

public class Dashboard : Controller
{
    private readonly IContextService _contextService;
    private readonly DatabaseContext _dbContext;

    public Dashboard(IContextService contextService, DatabaseContext dbContext)
    {
        _contextService = contextService;
        _dbContext = dbContext; 
    }


    public IActionResult Index()
        {
            return View();
        }
    public IActionResult DriverInductionList()
    {
        var context = _contextService.GetDataBaseContext();
        var inductionQuestions = context.InductionQuestion
            .Where(w => w.IsActivated == true && w.Category == "Driver")
            .OrderBy(o => o.Qnumber)
            .Select(iq => new InductionQuestion
            {
                Qid = iq.Qid,
                Qnumber = iq.Qnumber,
                Question = iq.Question ?? string.Empty, // Handle null
                AnswerLabel1 = iq.AnswerLabel1 ?? string.Empty, // Handle null
                AnswerLabel2 = iq.AnswerLabel2 ?? string.Empty, // Handle null
                IsActivated = iq.IsActivated,
                Category = iq.Category ?? string.Empty, // Handle null
                AnswerLabel3 = iq.AnswerLabel3 ?? string.Empty, // Handle null
                FilePath = iq.FilePath ?? string.Empty, // Handle null
                FileName = iq.FileName ?? string.Empty, // Handle null
                IsNotUseFile = iq.IsNotUseFile
            })
            .ToList();

        var viewModel = new ViewInduction
        {
            InductionQuestions = inductionQuestions
        };

        return View(viewModel);
    }
    public IActionResult DriverInductionApplicantList()
    {
        var category = _contextService.GetDataBaseContext().InductionInduction
            .Where(w => w.Category == "Driver")         
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
            }).OrderByDescending(o => o.DateCreated).ToList();

		return View(category);
    }
    public IActionResult VisitorInductionApplicantList()
    {
        var category = _contextService.GetDataBaseContext().InductionInduction
			.Where(w => w.Category == "Visitor")
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
			}).OrderByDescending(o => o.DateCreated).ToList();

		return View(category);
    }

    // add driver induction questions
    public IActionResult AddDriverInductionQuestion()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddDriverInductionQuestion([Bind("Question,AnswerLabel1,AnswerLabel2,IsActivated,Category,isCorrectAnswer")] InductionQuestion inductionQuestion)
    {
        var questions = _dbContext.InductionQuestion
            .Where(i => i.Category == inductionQuestion.Category)
            .OrderByDescending(i => i.Qnumber)
            .ToList();

        int maxQnumber = questions.Any() ? questions.First().Qnumber : 0;

        inductionQuestion.Qnumber = maxQnumber + 1;

        inductionQuestion.AnswerLabel3 = "NA";
        inductionQuestion.FilePath = "SDF";
        inductionQuestion.FileName = "SDF";
        inductionQuestion.IsNotUseFile = false;

        _dbContext.InductionQuestion.Add(inductionQuestion);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(DriverInductionList));
    }
    // edit driver induction questions
    public IActionResult EditDriverInduction(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inductionQuestion = _contextService.GetDataBaseContext().InductionQuestion.Find(id);
        if (inductionQuestion == null)
        {
            return NotFound();
        }

        return View(inductionQuestion);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditDriverInduction(int id, [Bind("Qid,Qnumber,Question,AnswerLabel1,AnswerLabel2,AnswerLabel3,IsActivated,Category,FilePath,FileName,IsNotUseFile,isCorrectAnswer")] InductionQuestion inductionQuestion)
    {
        if (id != inductionQuestion.Qid)
        {
            return NotFound();
        }

        //if (ModelState.IsValid)
        //{
        try
        {
            _contextService.GetDataBaseContext().Update(inductionQuestion);
            _contextService.GetDataBaseContext().SaveChanges();
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
        return RedirectToAction(nameof(DriverInductionList));
        //}
        return View(inductionQuestion);
    }


    // edit driver induction questions

    [HttpPost]
    public IActionResult DeleteInductionDriverQuestion(int id)
    {

        var inductionQuestions = _contextService.GetDataBaseContext().InductionQuestion
            .Where(w => w.IsActivated == true)
            .Where(w => w.Category == "Driver" || w.Category == "ForkLift" || w.Category == "Visitor" || w.Category == "Response")
            .OrderBy(o => o.Qnumber)
            .ToList();
        var inductionQuestion = inductionQuestions.FirstOrDefault(w => w.Qid == id);

        if (inductionQuestion == null)
        {
            return NotFound();
        }
        _contextService.GetDataBaseContext().InductionQuestion.Remove(inductionQuestion);
        _contextService.GetDataBaseContext().SaveChanges();

        return RedirectToAction(nameof(DriverInductionList));
    }

    [HttpPost]
    public IActionResult DeleteInductionVisitorQuestion(int id)
    {

        var inductionQuestions = _contextService.GetDataBaseContext().InductionQuestion
            .Where(w => w.IsActivated == true)
            .Where(w => w.Category == "Driver" || w.Category == "ForkLift" || w.Category == "Visitor" || w.Category == "Response")
            .OrderBy(o => o.Qnumber)
            .ToList();
        var inductionQuestion = inductionQuestions.FirstOrDefault(w => w.Qid == id);

        if (inductionQuestion == null)
        {
            return NotFound();
        }
        _contextService.GetDataBaseContext().InductionQuestion.Remove(inductionQuestion);
        _contextService.GetDataBaseContext().SaveChanges();

        return RedirectToAction(nameof(VisitorInductionList));
    }





    public IActionResult AddVisitorInductionQuestion()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddVisitorInductionQuestion([Bind("Question,AnswerLabel1,AnswerLabel2,AnswerLabel3,IsActivated,Category,isCorrectAnswer")] InductionQuestion inductionQuestion, IFormFile uploadedFile)
    {
        var questions = _dbContext.InductionQuestion
          .Where(i => i.Category == inductionQuestion.Category)
          .OrderByDescending(i => i.Qnumber)
          .ToList();

        int maxQnumber = questions.Any() ? questions.First().Qnumber : 0;

        inductionQuestion.Qnumber = maxQnumber + 1;

        // Handle file upload
        if (uploadedFile != null && uploadedFile.Length > 0)
        {
            // Define a path to save the file
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Create a unique filename
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(uploadedFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file to the directory
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                uploadedFile.CopyTo(stream);
            }

            // Save file details to the model
            inductionQuestion.FilePath = "/uploads/" + uniqueFileName;  // Relative path to save in the DB
            inductionQuestion.FileName = uniqueFileName;
            inductionQuestion.IsNotUseFile = false; // Set to false since file is being used
            
        }
        else
        {
            // Handle the case where no file is uploaded
            inductionQuestion.FilePath = null;
            inductionQuestion.FileName = null;
            inductionQuestion.IsNotUseFile = true;  // Set to true since no file is uploaded
        }

        _dbContext.InductionQuestion.Add(inductionQuestion);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(VisitorInductionList));
    }


    public IActionResult EditVisitorInduction(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inductionQuestion = _contextService.GetDataBaseContext().InductionQuestion.Find(id);
        if (inductionQuestion == null)
        {
            return NotFound();
        }

        return View(inductionQuestion);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditVisitorInduction(int id, [Bind("Qid,Qnumber,Question,AnswerLabel1,AnswerLabel2,AnswerLabel3,IsActivated,Category,FilePath,FileName,IsNotUseFile")] InductionQuestion inductionQuestion)
    {
        if (id != inductionQuestion.Qid)
        {
            return NotFound();
        }

        //if (ModelState.IsValid)
        //{
        try
        {
            _contextService.GetDataBaseContext().Update(inductionQuestion);
            _contextService.GetDataBaseContext().SaveChanges();
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
        return RedirectToAction(nameof(VisitorInductionList));
        //}
        return View(inductionQuestion);
    }



    private bool InductionQuestionExists(int id)
    {
        return _contextService.GetDataBaseContext().InductionQuestion.Any(e => e.Qid == id);
    }

    // Visitor Induction 

    public IActionResult VisitorInductionList()
    {
        var context = _contextService.GetDataBaseContext();
        var inductionQuestions = context.InductionQuestion
            .Where(w => w.IsActivated == true && w.Category == "Visitor")
            .OrderBy(o => o.Qnumber)
            .Select(iq => new InductionQuestion
            {
                Qid = iq.Qid,
                Qnumber = iq.Qnumber,
                Question = iq.Question ?? string.Empty, // Handle null
                AnswerLabel1 = iq.AnswerLabel1 ?? string.Empty, // Handle null
                AnswerLabel2 = iq.AnswerLabel2 ?? string.Empty, // Handle null
                IsActivated = iq.IsActivated,
                Category = iq.Category ?? string.Empty, // Handle null
                AnswerLabel3 = iq.AnswerLabel3 ?? string.Empty, // Handle null
                FilePath = iq.FilePath ?? string.Empty, // Handle null
                FileName = iq.FileName ?? string.Empty, // Handle null
                IsNotUseFile = iq.IsNotUseFile
            })
            .ToList();

        var viewModel = new ViewInduction
        {
            InductionQuestions = inductionQuestions
        };

        return View(viewModel);
    }

  
  





}
