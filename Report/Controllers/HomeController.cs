using Report.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Report.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, DateTime date)
        {
            #region Future date, double check
            if (date > DateTime.Now)
            {
                ViewBag.ErrorType = "danger";
                ViewBag.Error = "Report not available yet.";
                return View("Index");
            }
            #endregion
            var data = new ReportData();
            var department = data.GetDepartmentById(id);
            #region If id is tempered
            if (string.IsNullOrEmpty(department))
            {
                ViewBag.ErrorType = "danger";
                ViewBag.Error = "Report not available yet.";
                return View("Index");
            }
            #endregion
            var folder = ConfigurationManager.AppSettings["fileServerFolder"];
            //date = new DateTime(2017, 01, 01);
            var path = data.GetPathByDate(date, folder, department);
            if (System.IO.File.Exists(path))
            {
                return RedirectToAction("Pdf", new { path = path });
            }
            else
            {

                if (date.Date == DateTime.Now.Date)
                    return RedirectToAction("Pdf", new { path = data.Get404PdfPath() });
                //return HttpNotFound("File not found");
                ViewBag.ErrorType = "danger";
                ViewBag.Error = String.Format("Report not found for {0} department for date {1}", department, date.ToShortDateString());
                return View("Index");
            }
        }

        //[OutputCache(Duration = 60, VaryByParam = "path")]
        public FileResult Pdf(string path)
        {
            return File(path, "application/pdf");
        }
    }
}