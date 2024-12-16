using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using MercWebExt.Services;
using MercWebExt.Models.ViewModels;
using MercWebExt.Models.DataBase;

namespace MercWebExt.Controllers
{
    public class PolicyController : Controller
    {
        private readonly IContextService _context;
        private readonly ILogger<PolicyController> _logger;
        [Obsolete]
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public PolicyController(ILogger<PolicyController> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IContextService context)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        // GET: PolicyList
        public IActionResult PolicyList()
        {
            ViewPolicies viewModel = new ViewPolicies();

            viewModel.policyList = new List<Policies>();
            viewModel.policyList = _context.GetDataBaseContext().Policies.ToList();

            return View(viewModel);
        }

        public IActionResult PolicyCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ViewPolicies policies)
        {
            var newPolicy = new Policies();

            newPolicy.Name = policies.policy.Name;
            newPolicy.IsUsed = true;
            newPolicy.DateCreated = DateTime.Now;
            newPolicy.Owner = policies.policy.Owner;
            newPolicy.DateUpdated = policies.policy.DateUpdated;
            newPolicy.FileName = policies.file.FileName.Split('.')[0];
            newPolicy.FileExt = policies.file.FileName.Split('.')[1];
            newPolicy.FileSize = policies.file.Length;
            newPolicy.FilePath = SaveFileToServer(policies.file);

            _context.GetDataBaseContext().Attach(newPolicy);

            _logger.LogError("End of method " + _context.GetDataBaseContext().SaveChanges());

            return RedirectToAction("PolicyList");
        }

        public IActionResult Edit(int? polId)
        {
            ViewPolicies viewModel = new ViewPolicies();
            Policies policy = new Policies();

            if(polId > 0)
            {
                policy = _context.GetDataBaseContext().Policies.Where(w=>w.Pid.Equals(polId)).First();
            }

            viewModel.policy = policy;

            return View("PolicyEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ViewPolicies policies)
        {
            Policies editPolicy = new Policies();
            editPolicy = _context.GetDataBaseContext().Policies.Where(w => w.Pid.Equals(policies.policy.Pid)).First();

            editPolicy.Name = policies.policy.Name;
            editPolicy.IsUsed = true;
            editPolicy.DateCreated = DateTime.Now;
            editPolicy.Owner = policies.policy.Owner;
            editPolicy.DateUpdated = policies.policy.DateUpdated;
            
            if(policies.file != null)
            {
                editPolicy.FileName = policies.file.FileName.Split('.')[0];
                editPolicy.FileExt = policies.file.FileName.Split('.')[1];
                editPolicy.FileSize = policies.file.Length;
                editPolicy.FilePath = SaveFileToServer(policies.file);
            }

            //_polContext.Update(editPolicy);
            _context.GetDataBaseContext().Policies.Update(editPolicy);

            _logger.LogError("End of method !!" + _context.GetDataBaseContext().SaveChanges());

            return RedirectToAction("PolicyList");
        }

        public string SaveFileToServer(IFormFile file)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string sFolderName = @"policies";
            string sDocuments = @"documents";
            string fileName = file.FileName;
            string newPath = Path.Combine(webRootPath, sDocuments, sFolderName);
            int fileCnt = 1;
            //Create Directory
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            
            newPath = Path.Combine(newPath, fileName);

            while (System.IO.File.Exists(newPath))
            {
                newPath = newPath.Replace(fileName.Split('.')[0], fileName.Split('.')[0] + "_" + fileCnt);

                fileName = fileName.Replace(fileName.Split('.')[0], fileName.Split('.')[0] + "_" + fileCnt);

                fileCnt++;
            }

            using (var stream = System.IO.File.Create(newPath))
            {
                file.CopyTo(stream);
            }

            //string returnPath = Path.Combine(sDocuments, sFolderName, fileName);

            return Path.Combine(sDocuments, sFolderName, fileName);
        }


    }
}
