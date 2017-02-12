using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Report.Models
{
    public class ReportData
    {
        public string GetDepartmentById(string id)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");
            var dataFilePath = GetDateFilePath();
            using (var reader = File.OpenText(dataFilePath))
            {
                var json = JToken.ReadFrom(new JsonTextReader(reader));
                var key = json.Value<string>(id.Trim());
                return key;
            }
        }

        private static string GetDateFilePath()
        {
            var dataFilePath = ConfigurationManager.AppSettings["dataFilePath"];
            var path = System.Web.Hosting.HostingEnvironment.MapPath(dataFilePath);
            return path;
        }

        public string GetPathByDate(DateTime date, string folder, string department)
        {
            var month = System.Globalization.DateTimeFormatInfo.InvariantInfo.GetMonthName(date.Month);
            var day = date.Day.ToString();
            var year = date.Year.ToString();
            var name = "end-user-manual.pdf";
            var path = Path.Combine(folder, department, year, month, day, name);
            return path;
        }

        public string Get404PdfPath()
        {
            var dataPath = ConfigurationManager.AppSettings["NoPdfFile"];
            var path = System.Web.Hosting.HostingEnvironment.MapPath(dataPath);
            return path;
        }
    }
}