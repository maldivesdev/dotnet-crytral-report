using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Configuration;
using System.IO;

namespace DS.Report.Services.Shared
{
    public class ReportService
    {
        public static DataTable ConvertListToDatatable<T>(List<T> dataList)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in dataList)
            {
                var values = new object[props.Count];
                for (int i = 0; i < props.Count; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static void ExportExcel(ReportDocument reportDocument, string reportFileName = "")
        {
            ExportOptions options = new ExportOptions
            {
                ExportFormatType = ExportFormatType.Excel
            };
            reportDocument.ExportToHttpResponse(options, System.Web.HttpContext.Current.Response, false, reportFileName + '_' + DateTime.Now.ToString("yyyy-MM-dd_HHmmss",
                      CultureInfo.CreateSpecificCulture("en-US")));
        }
        public static void ExportWord(ReportDocument reportDocument, string reportFileName = "")
        {
            ExportOptions options = new ExportOptions
            {
                ExportFormatType = ExportFormatType.WordForWindows
            };
            reportDocument.ExportToHttpResponse(options, System.Web.HttpContext.Current.Response, false, reportFileName + '_' + DateTime.Now.ToString("yyyy-MM-dd_HHmmss",
                      CultureInfo.CreateSpecificCulture("en-US")));
        }
        public static void ExportPdf(ReportDocument reportDocument, string reportFileName = "")
        {
            ExportOptions options = new ExportOptions
            {
                ExportFormatType = ExportFormatType.PortableDocFormat
            };
            reportDocument.ExportToHttpResponse(options, System.Web.HttpContext.Current.Response, false, reportFileName + '_' + DateTime.Now.ToString("yyyy-MM-dd_HHmmss",
                      CultureInfo.CreateSpecificCulture("en-US")));
        }

        public static void ExportPdfToDisk(ReportDocument reportDocument, string process, string reportFileName = "")
        {
            string path = GetReportDirectory(reportFileName, process);
            reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, path);
        }

        public static byte[] ReadAllBytes(string path)
        {
            byte[] file = System.IO.File.ReadAllBytes(path);
            return file;
        }

        public static string GetReportDirectory(string reportFileName, string process)
        {
            string path = ConfigurationManager.AppSettings["DocumentFilePath"];
            path = path + "/Report/" + process;
            path = System.Web.HttpContext.Current.Server.MapPath(path);
            string fileName = reportFileName + ".pdf";
            path = Path.Combine(path, fileName);
            return path;
        }

        public static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<style>" +
                          ".allside{ border: 1px solid #000; }" +
                          ".leftright{ border-left: 1px solid #000; border-right: 1px solid #000; border-bottom: 1px dashed #ddd; }" +
                          ".leftrightbottom{ border-left: 1px solid #000; border-right: 1px solid #000; border-bottom: 1px dashed #ddd; }" +
                          "</style>" +
                          "<table>";
            //add header row
            html += "<tr class='allside'>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td class='allside' >" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td class='leftright'>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }
    }
}