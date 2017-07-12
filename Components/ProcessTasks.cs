using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNuke.Services.Scheduling;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Mail;

namespace Christoc.Modules.PMT_Admin
{
    public class ProcessTasks : SchedulerClient
    {
        public ProcessTasks(ScheduleHistoryItem oItem)
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
                AdminController aCont = new AdminController();
                List<TaskInfo> tasks = aCont.Get_OpenTasks();
                foreach(TaskInfo task in tasks)
                {
                    if(task.DeliveryMethod=="Javelin" && (task.DeliveryMethod!="COMPLETE" || task.DeliveryMethod!="CANCELLED"))
                    {
                        string status = aCont.GetJavelinOrderStatus(task.Id);
                        this.ScheduleHistoryItem.AddLogNote("Task# " + task.Id.ToString() + " status: " + status);
                    }
                }

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