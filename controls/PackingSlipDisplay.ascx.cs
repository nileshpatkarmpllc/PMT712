using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Users;

namespace Christoc.Modules.PMT_Admin
{
    public partial class PackingSlipDisplay : PMT_AdminModuleBase, IActionable
    {
        public List<TaskInfo> Tasks
        {
            get { return (List<TaskInfo>)(ViewState["Tasks"] ?? new List<TaskInfo>()); }

            set { ViewState["Tasks"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            if (Tasks.Count() > 0)
            {
                lblWOId.Text = Tasks[0].WorkOrderId.ToString();
                lblCompletionDate.Text = Tasks[0].DeliveryOrderDateComplete.ToShortDateString();
                if (Tasks[0].CreatedById != -1)
                {
                    UserInfo user = UserController.GetUserById(PortalId, Convert.ToInt32(Tasks[0].CreatedById));
                    lblOrderUser.Text = user.DisplayName;
                }
                lblDeliveryMethod.Text = Tasks[0].DeliveryMethod;
                WorkOrderInfo wo = aCont.Get_WorkOrderById(Tasks[0].WorkOrderId);
                AdvertiserInfo billTo = new AdvertiserInfo();
                if (wo.BillToId != -1)
                {
                    billTo = aCont.Get_AdvertiserById(wo.BillToId);
                }
                else
                {
                    billTo = aCont.Get_AdvertiserById(wo.AdvertiserId);
                }
                litBillTo.Text = billTo.AdvertiserName + "<br />";
                litBillTo.Text += billTo.Address1 + "<br />";
                if (billTo.Address2 != "")
                {
                    litBillTo.Text += billTo.Address2 + "<br />";
                }
                litBillTo.Text += billTo.City + ", " + billTo.State + " " + billTo.Zip + "<br />";
                WOGroupStationInfo wogroupStation = aCont.Get_WorkOrderGroupStationById(Tasks[0].StationId);
                StationInfo station = aCont.Get_StationById(wogroupStation.StationId);
                litShipTo.Text = station.StationName + "<br />";
                litShipTo.Text += station.CallLetter + "<br />";
                litShipTo.Text += station.Address1 + "<br />";
                if (station.Address2 != "")
                {
                    litShipTo.Text += station.Address2 + "<br />";
                }
                if (station.Phone != "")
                {
                    litShipTo.Text += "Tel: " + station.Phone + "<br />";
                }
                if (station.Fax != "")
                {
                    litShipTo.Text += "Fax: " + station.Fax + "<br />";
                }
                if (station.Email != "")
                {
                    litShipTo.Text += "Email: " + station.Email + "<br />";
                }
                if (station.AttentionLine != "")
                {
                    litShipTo.Text += "ATTENTION: " + station.AttentionLine + "<br />";
                }

                litShipTo.Text += station.City + ", " + station.State + " " + station.Zip + "<br />";
                plTasks.Controls.Clear();
                foreach (TaskInfo Task in Tasks)
                {
                    LibraryItemInfo lib = aCont.Get_LibraryItemById(Task.LibraryId);
                    Literal lit = new Literal();
                    lit.Text = "<div class=\"pmtRow\"><div class=\"pmtCell2 outline\">" + Task.Quantity.ToString() + "</div>";
                    string desc = "";
                    try {
                        desc = Task.Description.Substring(Task.Description.IndexOf("Station:"), (Task.Description.IndexOf(", Delivery Method")) - Task.Description.IndexOf("Station:"));
                    }
                    catch { }
                    lit.Text += "<div class=\"pmtCell2 outline\">" + lib.Title + "</div>";
                    lit.Text += "<div class=\"pmtCell2 outline\">" + lib.ProductDescription + "</div>";
                    lit.Text += "<div class=\"pmtCell2 outline\">" + Task.DeliveryMethod + "</div>";
                    lit.Text += "<div class=\"pmtCell2 outline\">" + lib.MediaType.ToUpper() + "</div>";
                    lit.Text += "<div class=\"pmtCell2 outline\">" + lib.Standard + "</div>";
                    lit.Text += "<div class=\"pmtCell2 outline\">" + lib.MediaLength + "</div></div>";
                    plTasks.Controls.Add(lit);
                }
            }
        }
        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }
    }
}