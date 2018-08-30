using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DS.Report.Services.CA;
using DS.Report.Services.Shared;
using System.Web.Mvc;

namespace DS.Report.Controllers.CA
{
    public class CAController : Controller
    {
        public FileResult SubmitReport()
        {
            var service = new CAService();
            string fileName = service.SubmitReport();
            string directory = ReportService.GetReportDirectory(fileName, "CA");
            var file = ReportService.ReadAllBytes(directory);
            Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);
            return File(file, "application/pdf");
            //return Json(new { content = file , contentType = "application/pdf" , fileName } , JsonRequestBehavior.AllowGet);
        }
    }
}