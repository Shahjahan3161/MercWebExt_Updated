using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using MercWebExt.Services;
using MercWebExt.Models.ViewModels;

namespace MercWebExt.Controllers
{
    public class GateController : Controller
    {
        private readonly IContextService _context;
        private readonly ILogger<GateController> _logger;

        public GateController(ILogger<GateController> logger, IContextService context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Induction
        public IActionResult GateLog()
        {
            return View();
        }

        // GET: Induction Report List
        [Authorize(Roles = "Site Manager, Admin")]
        public IActionResult GateLogList()
        {
            ViewReportInduction viewModel = new ViewReportInduction();
            viewModel.driverList = _context.GetDataBaseContext().InductionInduction.OrderBy(O => O.DateCreated).Take(30).ToList();

            return View(viewModel);
        }

        // POST : Select Specific Date
        [HttpPost]
        public IActionResult ReportDriverList(ViewReportInduction postData)
        {
            ViewReportInduction returnModel = new ViewReportInduction();

            DateTime selectDate = postData.selectDate;

            returnModel.driverList = _context.GetDataBaseContext().InductionInduction.Where(w => w.DateCreated.Date.Equals(selectDate.Date)).ToList();

            return View("InductionList", returnModel);
        }
    }
}
