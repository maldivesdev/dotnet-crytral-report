using CrystalDecisions.CrystalReports.Engine;
using DS.Report.Models.Report.CA;
using DS.Report.Services.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace DS.Report.Services.CA
{
    public class CAService
    {
        #region [Report Method]

        public string SubmitReport()
        {
            string fileName = string.Empty;
            List<CAViewModel> caList = new List<CAViewModel>();
            try
            {
                #region GetReportData

                DataTable getDataReport = new DataTable();
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(HttpContext.Current.Server.MapPath("~/Reports/CA"), "CrystalReport1.rpt"));
                //Parameter
                caList.Add(new CAViewModel { CANo = "CA-123-4567890" });
                //Convert Data To Report
                getDataReport = ReportService.ConvertListToDatatable(caList);
                #endregion

                getDataReport.TableName = "DataTable1";
                rd.SetDataSource(getDataReport);

                #region Export pdf
                //Encoding FileName
                fileName = "CA-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
                ReportService.ExportPdfToDisk(rd, "CA", fileName);
                #endregion

                rd.Close();
                rd.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
            }
            return fileName;
        }

        #endregion
    }
}