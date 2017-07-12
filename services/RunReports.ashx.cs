using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Security.Roles;
using DotNetNuke.Entities.Users;

namespace Christoc.Modules.PMT_Admin
{
    /// <summary>
    /// Summary description for RunReports
    /// </summary>
    public class RunReports : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            SetPortalId(context.Request);
            context.Response.ContentType = "text/plain";
            string baseUrl = "http://" + context.Request.Url.Host + "/Reports";
            context.Response.Write("Success");
            RunReportsAsync(PortalId, baseUrl);
        }

        public async void RunReportsAsync(int PortalId,  string baseUrl)
        {
            AdminController aCont = new AdminController();
            List<ReportInfo> reports = aCont.Get_ReportsByPortalId(PortalId);
            string summary = "";
            foreach (ReportInfo report in reports)
            {
                summary += "Report ID: " + report.Id.ToString() + ", title: " + report.ReportName + ", Email to: " + report.EmailTo;
                if (report.isActive)
                {
                    List<TaskInfo> tasks = new List<TaskInfo>();
                    if(report.ReportType==3)
                    {
                        tasks = getReportTasks(PortalId, report);
                    }
                    if ((report.ReportType == 3 && tasks.Count > 0) || report.ReportType == 1 || report.ReportType == 2)
                    {
                        bool sent = false;
                        if (report.Frequency == "daily" && DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                        {
                            aCont.SendReport(report, baseUrl, true);
                            sent = true;
                        }
                        else if (report.Frequency == "weekly")
                        {
                            DateTime today = DateTime.Now;
                            TimeSpan t = today - report.FirstReportDate;
                            if (t.TotalDays % 7 == 0)
                            {
                                aCont.SendReport(report, baseUrl, true);
                                sent = true;
                            }
                        }
                        else if (report.Frequency == "monthly")
                        {
                            DateTime today = DateTime.Now;
                            if (today.Day == report.FirstReportDate.Day)
                            {
                                aCont.SendReport(report, baseUrl, true);
                                sent = true;
                            }
                        }
                        if (sent)
                        {
                            summary += " sent. ";
                        }
                        else
                        {
                            summary += " not sent. ";
                        }
                    }
                    else
                    {
                        summary += " not sent. ";
                    }
                }
                else
                {
                    summary += " not sent. ";
                }
            }
            DotNetNuke.Services.Log.EventLog.EventLogController eCont = new DotNetNuke.Services.Log.EventLog.EventLogController();
            eCont.AddLog("Reports Summary", summary, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.HOST_ALERT);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private void SetPortalId(HttpRequest request)
        {

            string domainName = DotNetNuke.Common.Globals.GetDomainName(request, true);

            string portalAlias = request.Url.Host;//domainName.Substring(0, domainName.IndexOf("/svc"));
            PortalAliasInfo pai = PortalSettings.GetPortalAliasInfo(portalAlias);
            if (pai != null)
                PortalId = pai.PortalID;
        }

        public static int PortalId { get; set; }

        private List<TaskInfo> getReportTasks(int PortalId, ReportInfo report)
        {
            List<TaskInfo> returnTasks = new List<TaskInfo>();
            if (report.ReportType == 3)
            {
                AdminController aCont = new AdminController();                
                List<LibraryItemInfo> libs = aCont.getLibs(PortalId);
                List<WorkOrderInfo> wos = aCont.Get_WorkOrdersByPortalId(PortalId);
                List<WorkOrderInfo> wosByAdvertiser = new List<WorkOrderInfo>();
                List<WorkOrderInfo> wosByAgency = new List<WorkOrderInfo>();
                List<WorkOrderInfo> wosByKeyword = new List<WorkOrderInfo>();
                List<AdvertiserInfo> userAds = new List<AdvertiserInfo>();
                List<AgencyInfo> userAgs = new List<AgencyInfo>();
                List<StationInfo> stations = aCont.Get_StationsByPortalId(PortalId);
                List<AdvertiserInfo> ads = aCont.getAdvertisers(PortalId);
                List<AgencyInfo> ags = aCont.getAgencies(PortalId);
                bool canSeeAll = true;
                string selAg = "";
                string selAd = "";
                selAg = report.AgencyId.ToString();
                selAd = report.AdvertiserId.ToString();
                if (selAd != "" && selAd != "-1")
                {
                    foreach (WorkOrderInfo wo in wos)
                    {
                        if (wo.AdvertiserId.ToString() == selAd)
                        {
                            wosByAdvertiser.Add(wo);
                        }
                    }
                }
                else
                {
                    wosByAdvertiser = wos;
                }
                if (selAg != "" && selAg != "-1")
                {
                    foreach (WorkOrderInfo wo in wosByAdvertiser)
                    {
                        if (wo.AgencyId.ToString() == selAg)
                        {
                            wosByAgency.Add(wo);
                        }
                    }
                }
                else
                {
                    wosByAgency = wosByAdvertiser;
                }
                wosByKeyword = wosByAgency;

                int shown = 0;
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now;
                DateTime tempdate = DateTime.Now;
                try
                {
                    tempdate = DateTime.Now; //Convert.ToDateTime(txtStartDate.Text);
                    startDate = new DateTime(tempdate.Year, tempdate.Month, tempdate.Day, 0, 0, 0);
                    if (report.Frequency == "weekly")
                    {
                        startDate = startDate.AddDays(-7);
                    }
                    else if (report.Frequency == "monthly")
                    {
                        startDate = startDate.AddMonths(-1);
                    }
                }
                catch { }
                try
                {
                    tempdate = DateTime.Now; //Convert.ToDateTime(txtEndDate.Text);
                    endDate = new DateTime(tempdate.Year, tempdate.Month, tempdate.Day, 23, 59, 59);
                }
                catch { }
                foreach (WorkOrderInfo wo in wosByKeyword)
                {
                    //if (shown >= maxShow)
                    //{
                    //    break;
                    //}
                    //else
                    //{
                    //if (startDate <= wo.LastModifiedDate && wo.LastModifiedDate <= endDate) //was datecreated
                    //{
                        bool showThis = canSeeAll;
                        foreach (AdvertiserInfo ad in userAds)
                        {
                            if (ad.Id == wo.AdvertiserId)
                            {
                                showThis = true;
                                break;
                            }
                        }
                        if (!showThis)
                        {
                            foreach (AgencyInfo ag in userAgs)
                            {
                                if (ag.Id == wo.AgencyId)
                                {
                                    showThis = true;
                                    break;
                                }
                            }
                        }
                        showThis = true;
                        if (showThis)
                        {
                            List<TaskInfo> tasks = aCont.Get_TasksByWOId(wo.Id);
                            foreach (TaskInfo task in tasks)
                            {
                                //if (task.DeliveryStatus.ToLower() == "complete")
                                //{
                                //if (startDate <= task.DeliveryOrderDateComplete && task.DeliveryOrderDateComplete <= endDate)
                                if (startDate <= task.DateCreated && task.DateCreated <= endDate)
                                {
                                    returnTasks.Add(task);
                                }
                                //}
                            }
                        }
                    //}
                }
            }
            return returnTasks;
        }
    }
}