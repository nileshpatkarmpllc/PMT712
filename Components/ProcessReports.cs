using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DotNetNuke.Services.Scheduling;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Mail;
using System.Net;
using System.IO;

namespace Christoc.Modules.PMT_Admin
{
    public class ProcessReports : SchedulerClient
    {
        public ProcessReports(ScheduleHistoryItem oItem)
            : base()
        {
            this.ScheduleHistoryItem = oItem;
        }
        public override void DoWork()
        {
            try
            {
                //Perform required items for logging
                this.Progressing();

                //process tasks
                string sURL;
                sURL = "http://localhost/DesktopModules/PMT_Admin/Services/RunReports.ashx";
                try { sURL = ConfigurationManager.AppSettings["ReportsEndpoint"]; }
                catch { }

                WebRequest wrGETURL;
                wrGETURL = WebRequest.Create(sURL);
                Stream objStream;
                objStream = wrGETURL.GetResponse().GetResponseStream();

                StreamReader objReader = new StreamReader(objStream);
                string result = objReader.ReadToEnd().ToString();

                //Show success
                this.ScheduleHistoryItem.Succeeded = true;
            }
            catch(Exception ex)
            {
                this.ScheduleHistoryItem.Succeeded = false;
                this.ScheduleHistoryItem.AddLogNote("Exception= " + ex.ToString());
                this.Errored(ref ex);
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
            }
        }
    }
}