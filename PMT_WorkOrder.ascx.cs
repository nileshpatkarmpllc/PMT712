using System;
using System.Web.UI.WebControls;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;

using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Security.Roles;
using DotNetNuke.Entities.Modules.Definitions;

namespace Christoc.Modules.PMT_Admin
{
    public partial class PMT_WorkOrder : PMT_AdminModuleBase, IActionable
    {
        public List<WOGroupInfo> Groups
        {
            get { return (List<WOGroupInfo>)(ViewState["WOGroups"] ?? new List<WOGroupInfo>()); }

            set { ViewState["WOGroups"] = value; }
        }
        public bool WOEditMode
        {
            get { return (bool)(ViewState["editmode"] ?? true); }

            set { ViewState["editmode"] = value; }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            FillAdvertiserList();
            FillAgencyList();
            fillStationGroups();
            //addGroupControls();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            filterMedia();
            fillStations();
            if(!Page.IsPostBack)
            {
                pnlGroups.Controls.Clear();
                if(Request.QueryString["woid"]!=null)
                {
                    lblStatus.Visible = true;
                    btnPrint.Visible = true;
                    AdminController aCont = new AdminController();
                    try
                    {
                        string sWoid = Request.QueryString["woid"].ToString();
                        int woid = Convert.ToInt32(sWoid);
                        WorkOrderInfo WorkOrder = aCont.Get_WorkOrderById(woid);
                        
                        if (WorkOrder.Id != -1)
                        {
                            if(WorkOrder.Status.ToLower() == "invoiced")
                            {
                                btnSubmitWorkOrder.Visible = false;
                            }
                            if (WorkOrder.Status != "NEW" && WorkOrder.Status != "PENDING" && WorkOrder.Status != "COMPLETE" && WorkOrder.Status != "CANCELLED" && WorkOrder.Status.ToUpper() != "INVOICED")
                            {
                                lblStatus.Text = "Status: NEW";
                            }
                            else
                            {
                                lblStatus.Text = "Status: " + WorkOrder.Status.ToUpper();
                            }
                            List<WOGroupInfo> orderedGroups = new List<WOGroupInfo>();
                            int c = 0;
                            foreach (WOGroupInfo gr in WorkOrder.Groups)
                            {
                                gr.index = c;
                                c++;
                                orderedGroups.Add(gr);
                            }
                            Groups = orderedGroups;
                            txtWorkOrderId.Text = WorkOrder.Id.ToString();
                            txtWorkOrderId.Enabled = false;
                            txtWorkOrderDescription.Text = WorkOrder.Description;
                            txtWorkOrderPONumber.Text = WorkOrder.PONumber;
                            txtWorkOrderQBBillingCode.Text = WorkOrder.InvoiceNumber;
                            ddlAdvertisers.SelectedValue = WorkOrder.AdvertiserId.ToString();
                            ddlBillTo.SelectedValue = WorkOrder.BillToId.ToString();
                            ddlAgencies.SelectedValue = WorkOrder.AgencyId.ToString();
                            txtWONotes.Text = WorkOrder.Notes;
                            filterMedia();
                        }
                        else
                        {
                            Response.Redirect("~/Work-Orders");
                        }
                    }
                    catch { }
                }
                else
                {
                    lblStatus.Visible = false;
                }
                if (Request.QueryString["edit"] == null && Request.QueryString["woid"] == null)
                {
                    WOEditMode = true;
                    btnSubmitWorkOrder.Text = "CREATE WORK ORDER";
                    //txtWorkOrderId.Enabled = true;
                    txtWorkOrderDescription.Enabled = true;
                    txtWorkOrderPONumber.Enabled = true;
                    txtWorkOrderQBBillingCode.Enabled = true;
                    ddlAdvertisers.Enabled = true;
                    ddlBillTo.Enabled = true;
                    ddlAgencies.Enabled = true;
                    txtWONotes.Enabled = true;
                }
                else if (Request.QueryString["edit"] != null && Request.QueryString["woid"] != null)
                {
                    WOEditMode = true;
                    btnSubmitWorkOrder.Text = "UPDATE WORK ORDER";
                    //txtWorkOrderId.Enabled = true;
                    txtWorkOrderDescription.Enabled = true;
                    txtWorkOrderPONumber.Enabled = true;
                    txtWorkOrderQBBillingCode.Enabled = true;
                    ddlAdvertisers.Enabled = true;
                    ddlBillTo.Enabled = true;
                    ddlAgencies.Enabled = true;
                    txtWONotes.Enabled = true;
                }
                else
                {
                    WOEditMode = false;
                    txtWorkOrderId.Enabled = false;
                    txtWorkOrderDescription.Enabled = false;
                    txtWorkOrderPONumber.Enabled = false;
                    txtWorkOrderQBBillingCode.Enabled = false;
                    ddlAdvertisers.Enabled = false;
                    ddlBillTo.Enabled = false;
                    ddlAgencies.Enabled = false;
                    txtWONotes.Enabled = false;
                    drawPackingSlips();
                }
            }
            if(WOEditMode)
            {
                pnlEditMode.Visible = true;
                btnSubmitWorkOrder.Visible = true;
            }
            else
            {
                pnlEditMode.Visible = false;
                btnSubmitWorkOrder.Visible = true;
                btnSubmitWorkOrder.Text = "Edit";
            }
            drawGroups();
        }
        private string getSetting(string settingName, string defaultValue)
        {
            string returnMe = "";
            if (Settings.Contains(settingName))
            {
                returnMe = Settings[settingName].ToString();
            }
            else
            {
                returnMe = defaultValue;
            }
            return returnMe;
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
        //private void FillAdvertiserList()
        //{
        //    AdminController aCont = new AdminController();
        //    List<AdvertiserInfo> Advertisers = aCont.Get_AdvertisersByPortalId(PortalId);

        //    ddlAdvertisers.Items.Clear();
        //    ddlAdvertisers.Items.Add(new ListItem("--Select Advertiser--", "-1"));
        //    List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId);
        //    bool isAdmin = UserInfo.IsInRole("Administrators");
        //    foreach (AdvertiserInfo adv in Advertisers)
        //    {
        //        ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
        //    }
        //    //filterMedia();
        //}
        private int getUserCase()
        {
            //-1 = error (advertiser with no advertisers, etc.), 0 = see all (admin), 1=Ad/Ad, 2=Ag/Ag, 3=Both/Both
            int userCase = 0;
            bool canSeeAll = false;
            RoleController rCont = new RoleController();
            IList<RoleInfo> roles = rCont.GetRoles(PortalId);
            List<int> myRoles = new List<int>();
            bool isAdmin = false;
            if (UserInfo.IsInRole("Administators"))
            { isAdmin = true; }
            //check if user is in a role which has been allowed to see all in the settings
            foreach (RoleInfo role in roles)
            {
                if (UserInfo.IsInRole(role.RoleName))
                {
                    myRoles.Add(role.RoleID);
                }
            }
            string allViewRoles = getSetting("AllViewRoles", "");
            string[] allRoles = allViewRoles.Split(',');
            foreach (string ar in allRoles)
            {
                foreach (int myRole in myRoles)
                {
                    if (ar == myRole.ToString() || isAdmin)
                    {
                        canSeeAll = true;
                        break;
                    }
                }
            }
            if (!canSeeAll)//if canSeeAll is true, we pass back 0, which is already the value of userCase
            {
                AdminController aCont = new AdminController();
                List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
                List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
                bool isAd = userAds.Count > 0 ? true : false; //are they in Advertiser Role?
                bool isAg = userAgs.Count > 0 ? true : false; //are they in Agency Role?
                foreach (string ar in allRoles)
                {
                    if (ar.ToLower() == "advertiser")
                    {
                        isAd = true;
                    }
                    if (ar.ToLower() == "agency")
                    {
                        isAg = true;
                    }
                }
                //now, check for case 1=Ad/Ad
                if ((isAd && !isAg && userAds.Count > 0 && userAgs.Count == 0) || (isAg && userAds.Count > 0 && userAgs.Count == 0))
                {
                    userCase = 1;
                }
                //check for case 2=Ag/Ag
                else if ((!isAd && isAg && userAds.Count == 0 && userAgs.Count > 0) || (isAd && userAgs.Count > 0 && userAds.Count == 0))
                {
                    userCase = 2;
                }
                //check for case 3=Both/Both
                else if ((isAd && isAg && userAds.Count > 0 && userAgs.Count > 0) || (userAds.Count > 0 && userAgs.Count > 0))
                {
                    userCase = 3;
                }
                //else error
                else
                {
                    userCase = -1;
                }
            }

            return userCase;
        }
        private int getSelCase()
        {
            //0= none selected, 1=Only Ad Selected, 2= Only Ag selected, 3=Both Selected
            string selAd = "";
            string selAg = "";
            bool adSel = false;
            bool agSel = false;
            if (ViewState["selAgency"] != null)
            {
                selAg = ViewState["selAgency"].ToString();
            }
            if (ViewState["selAdvertiser"] != null)
            {
                selAd = ViewState["selAdvertiser"].ToString();
            }
            if (selAd != "" && selAd != "-1")
            {
                adSel = true;
            }
            if (selAg != "" && selAg != "-1")
            {
                agSel = true;
            }
            int selCase = 0;
            if (adSel && !agSel)
            {
                selCase = 1;
            }
            else if (!adSel && agSel)
            {
                selCase = 2;
            }
            else if (adSel && agSel)
            {
                selCase = 3;
            }

            return selCase;
        }
        private int getDrawCase()
        {
            int drawCase = 0;
            int userCase = getUserCase();
            int selCase = getSelCase();
            if (userCase == 1 && selCase == 0)
            { drawCase = 1; }
            else if (userCase == 2 && selCase == 0)
            { drawCase = 2; }
            else if ((userCase == 0 || userCase == 3) && selCase == 0)
            { drawCase = 3; }
            else if (selCase == 1 && (userCase == 0 || userCase == 1 || userCase == 2 || userCase == 3))
            { drawCase = 4; }
            else if (selCase == 2 && (userCase == 0 || userCase == 1 || userCase == 2 || userCase == 3))
            { drawCase = 5; }
            else if (selCase == 3 && (userCase == 0 || userCase == 1 || userCase == 2 || userCase == 3))
            { drawCase = 6; }

            return drawCase;
        }
        public void drawPackingSlips()
        {
            plPackingSlips.Controls.Clear();
            AdminController aCont = new AdminController();
            if (Request.QueryString["woid"] != null)
            {
                //draw workorder for print
                WorkOrderInfo wo = aCont.Get_WorkOrderById(Convert.ToInt32(Request.QueryString["woid"]));

                lblOrderNumber.Text = wo.Id.ToString();
                lblOrderNumber2.Text = wo.Id.ToString();
                if (wo.CreatedById != -1)
                {
                    UserInfo createdBy = UserController.GetUserById(PortalId, wo.CreatedById);
                    lblCustRep.Text = createdBy.DisplayName;
                    lblCustRep2.Text = createdBy.DisplayName;
                }
                lblDueDate.Text = wo.DateCreated.ToShortDateString();
                lblDueDate2.Text = wo.DateCreated.ToShortDateString();
                //lblCustRef = ?
                AdvertiserInfo ad = aCont.Get_AdvertiserById(wo.AdvertiserId);
                AgencyInfo ag = aCont.Get_AgencyById(wo.AgencyId);
                Literal litWO = new Literal();
                litWO.Text = "<h1>" + ad.AdvertiserName + "</h1><hr />";
                List<TaskInfo> tasks = aCont.Get_TasksByWOId(Convert.ToInt32(Request.QueryString["woid"]));
                Hashtable stations = new Hashtable();

                foreach (WOGroupInfo group in wo.Groups)
                {
                    litWO.Text += "<div style=\"font-size:18px;\">Group: " + group.GroupName + "<br /><br />";
                    if(group.GroupType == GroupTypeEnum.Customized)
                    {
                        LibraryItemInfo lib = aCont.Get_LibraryItemById(tasks[0].LibraryId);
                        litWO.Text += "<div class=\"libCol\">" + lib.PMTMediaId + "</div>";
                        litWO.Text += "<div class=\"libCol\">" + lib.Title + "<br />" + ag.AgencyName + "</div>";
                        litWO.Text += "<div class=\"libCol\">" + lib.Standard + "</div>";
                        litWO.Text += "<div class=\"libCol\">" + lib.MediaLength + "</div><br clear=\"both\" /><br />";
                        litWO.Text += "<div class=\"libCol\" style=\"text-decoration: underline;\">Product Description</div>";
                        litWO.Text += "<div class=\"libCol\" style=\"text-decoration: underline;\">ISCI Code</div>";
                        litWO.Text += "<div class=\"libCol\" style=\"text-decoration: underline;\">Tape Code</div><br clear=\"both\" />";
                    }
                    foreach (TaskInfo task in tasks)
                    {
                        if ((task.TaskType == GroupTypeEnum.Bundle || task.TaskType == GroupTypeEnum.Delivery || task.TaskType == GroupTypeEnum.Customized) && !task.isDeleted && task.WOGroupId == group.Id)
                        {
                            stations[task.StationId.ToString()] = "1";
                            LibraryItemInfo lib = aCont.Get_LibraryItemById(task.LibraryId);

                            if (task.TaskType != GroupTypeEnum.Customized)
                            {
                                litWO.Text += "<div class=\"libCol\">" + lib.PMTMediaId + "</div>";
                                litWO.Text += "<div class=\"libCol\">" + lib.Title + "</div><br clear=\"both\" />";
                                litWO.Text += "<div class=\"libCol\">&nbsp;</div>";
                                litWO.Text += "<div class=\"libCol\">" + lib.ProductDescription + "</div>";
                                litWO.Text += "<div class=\"libCol\">" + lib.Standard + " " + lib.MediaLength + "</div><br clear=\"both\" />";
                                litWO.Text += "<div class=\"libCol\">&nbsp;</div>";
                                litWO.Text += "<div class=\"libCol\">ISCI: " + lib.ISCICode + "</div><br clear=\"both\" />";
                                litWO.Text += "<div class=\"libCol\">&nbsp;</div>";
                                litWO.Text += "<div class=\"libCol\">Tape Code: " + lib.TapeCode + "</div><br clear=\"both\" />";
                                litWO.Text += "<div class=\"libCol\">&nbsp;</div>";
                                litWO.Text += "<div class=\"libCol\">Encode: " + lib.Encode + "</div>";
                                litWO.Text += "<div class=\"libCol\">Closed Captioned: " + lib.ClosedCaptioned + "</div><br clear=\"both\" />";
                                litWO.Text += "<div class=\"libCol\">&nbsp;</div>";
                                litWO.Text += "<div class=\"libCol\">" + ag.AgencyName + "</div><br clear=\"both\" />";
                                litWO.Text += "<div class=\"libCol\">&nbsp;</div>";
                                litWO.Text += "<div class=\"libCol\">Delivery Method: " + task.DeliveryMethod + "</div><br clear=\"both\" />";
                                litWO.Text += "<br /><hr /><br />";
                            }
                            else
                            {
                                litWO.Text += "<div class=\"libCol\">" + lib.ProductDescription + "</div>";
                                litWO.Text += "<div class=\"libCol\">" + lib.ISCICode + "</div>";
                                litWO.Text += "<div class=\"libCol\">" + lib.TapeCode + "</div><br clear=\"both\" />";
                            }
                        }
                    }
                    if (group.GroupType == GroupTypeEnum.Non_Deliverable)
                    {
                        litWO.Text += "MasterId: " + group.Master.PMTMediaId + "<br />Title: " + group.Master.Title + "<br />Product/Description: " + group.Master.Comment + "<br /><br />";
                    }
                    if (group.Services.Count() > 0)
                    {
                        litWO.Text += "<h3>Services</h3>";
                        foreach (ServiceInfo serv in group.Services)
                        {
                            litWO.Text += serv.ServiceName + "<br />";
                        }
                        litWO.Text += "<br /><br />";
                    }
                    if (group != null && group.Comments != "")
                    {
                        litWO.Text += "Notes:<br />" + group.Comments;
                    }
                    litWO.Text += "</div>";
                    if (group.GroupType == GroupTypeEnum.Customized)
                    {
                        litWO.Text += "<br /><hr /><br />";
                    }
                }
                plPrintWorkOrder.Controls.Add(litWO);
                Literal litWO2 = new Literal();
                litWO2.Text = litWO.Text;
                plPrintWorkOrder2.Controls.Add(litWO2);
                foreach (string key in stations.Keys)
                {
                    PackingSlipDisplay psd = (PackingSlipDisplay)LoadControl("controls/PackingSlipDisplay.ascx");
                    psd.ID = "psd_" + key;
                    var theseTasks = tasks.Where(o => o.StationId.ToString() == key).ToList();
                    if (theseTasks != null)
                    {
                        psd.Tasks = theseTasks;
                        plPackingSlips.Controls.Add(psd);
                    }
                }
            }
        }
        private void FillAdvertiserList()
        {
            string selAg = "";
            string selAd = "";
            if (ViewState["selAgency"] != null)
            {
                selAg = ViewState["selAgency"].ToString();
            }
            if (ViewState["selAdvertiser"] != null)
            {
                selAd = ViewState["selAdvertiser"].ToString();
            }
            bool canSeeAll = false;
            int userCase = getUserCase();
            int drawCase = getDrawCase();
            if (userCase == 0)
            {
                canSeeAll = true;
            }
            AdminController aCont = new AdminController();
            List<AdvertiserInfo> Advertisers = aCont.getAdvertisers(PortalId);

            ddlAdvertisers.Items.Clear();
            ddlAdvertisers.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            ddlBillTo.Items.Clear();
            ddlBillTo.Items.Add(new ListItem("--Select Bill To--", "-1"));
            List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
            List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            if (canSeeAll)
            {
                if (selAg != "" && selAg != "-1")
                {
                    List<AdvertiserInfo> ads = aCont.Get_AdvertisersByAgencyId(Convert.ToInt32(selAg));
                    List<AdvertiserInfo> sortedList = ads.Distinct(new AdvertiserComparer()).OrderBy(o => o.AdvertiserName).ToList();
                    foreach (AdvertiserInfo adv in sortedList)
                    {
                        ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        ddlBillTo.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    }
                }
                else
                {
                    List<AdvertiserInfo> sortedList = Advertisers.Distinct(new AdvertiserComparer()).OrderBy(o => o.AdvertiserName).ToList();
                    foreach (AdvertiserInfo adv in sortedList)
                    {
                        ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        ddlBillTo.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    }
                }
            }
            else
            {
                List<AdvertiserInfo> selAds = new List<AdvertiserInfo>();
                if (selAg != "" && selAg != "-1")
                {
                    selAds = aCont.Get_AdvertisersByAgencyId(Convert.ToInt32(selAg));
                }
                if (userCase == 1) //Ad/Ad  (drawCase == 1 || drawCase ==4)
                {
                    List<AdvertiserInfo> sortedList = userAds.Distinct(new AdvertiserComparer()).OrderBy(o => o.AdvertiserName).ToList();
                    foreach (AdvertiserInfo adv in sortedList)
                    {
                        ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        ddlBillTo.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    }
                }
                else if (userCase == 2) //(drawCase==2)//get all advertisers from agencies user is connected to
                {
                    List<AdvertiserInfo> addAds = new List<AdvertiserInfo>();
                    foreach (AgencyInfo ag in userAgs)
                    {
                        List<AdvertiserInfo> agAds = aCont.Get_AdvertisersByAgencyId(ag.Id);
                        foreach (AdvertiserInfo ad in agAds)
                        {
                            addAds.Add(ad);
                        }
                    }
                    List<AdvertiserInfo> sortedList = addAds.Distinct().OrderBy(o => o.AdvertiserName).ToList();
                    if (selAg == "" || selAg == "-1")
                    {
                        foreach (AdvertiserInfo adv in sortedList)
                        {
                            ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                            ddlBillTo.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                    else
                    {
                        foreach (AdvertiserInfo adv in sortedList)
                        {
                            ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                            ddlBillTo.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                }
                else if (userCase == 3) //(drawCase==3 || drawCase==6)//get all advertisers user is connected to as well as all the advertisers that are connected to the agencies the user is tied to
                {
                    List<AdvertiserInfo> addAds = new List<AdvertiserInfo>();
                    foreach (AdvertiserInfo ad in userAds)
                    {
                        addAds.Add(ad);
                    }
                    foreach (AgencyInfo ag in userAgs)
                    {
                        List<AdvertiserInfo> agAds = aCont.Get_AdvertisersByAgencyId(ag.Id);
                        foreach (AdvertiserInfo ad in agAds)
                        {
                            addAds.Add(ad);
                        }
                    }
                    List<AdvertiserInfo> sortedList = addAds.Distinct(new AdvertiserComparer()).OrderBy(o => o.AdvertiserName).ToList();
                    if (selAg == "" || selAg == "-1")
                    {
                        foreach (AdvertiserInfo adv in sortedList)
                        {
                            ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                            ddlBillTo.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                    else
                    {
                        foreach (AdvertiserInfo adv in selAds)
                        {
                            ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                            ddlBillTo.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                }
            }
            if (ddlAdvertisers.Items.Count == 2)
            {
                ddlAdvertisers.SelectedIndex = 1;
            }
            else if (selAd != "")
            {
                try
                {
                    ddlAdvertisers.SelectedValue = selAd;
                }
                catch { }
            }
        }
        private void FillAdvertiserListOld()
        {
            bool canSeeAll = false;
            //TODO:Make these roles a setting
            if (UserInfo.IsInRole("Administrators") || UserInfo.IsInRole("PMT Admin") || UserInfo.IsInRole("PMT ADMIN"))
            {
                canSeeAll = true;
            }
            AdminController aCont = new AdminController();
            List<AdvertiserInfo> Advertisers = aCont.Get_AdvertisersByPortalId(PortalId);

            ddlAdvertisers.Items.Clear();
            ddlAdvertisers.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            //ddlMasterItemAdvertiserSearch.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            //ddlMasterItemBillTo.Items.Add(new ListItem("--Select Bill To--", "-1"));
            List<AdvertiserInfo> userAds = aCont.getAdvertisers(PortalId); //aCont.Get_AdvertisersByUser(UserId, PortalId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            if (canSeeAll)
            {
                foreach (AdvertiserInfo adv in Advertisers)
                {
                    ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    //ddlMasterItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    //ddlMasterItemBillTo.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                }
            }
            else
            {
                foreach (AdvertiserInfo adv in userAds)
                {
                    ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    //ddlMasterItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    //ddlMasterItemBillTo.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                }
            }
            //filterMedia();
        }
        private void FillAgencyList()
        {
            ddlAgencies.Items.Clear();
            string selAd = ""; //was sel
            if (ViewState["selAdvertiser"] != null)
            {
                selAd = ViewState["selAdvertiser"].ToString();
            }
            string selAg = "";
            if (ViewState["selAgency"] != null)
            {
                selAg = ViewState["selAgency"].ToString();
            }
            bool canSeeAll = false;
            int userCase = getUserCase();
            int drawCase = getDrawCase();
            if (userCase == 0)
            {
                canSeeAll = true;
            }
            AdminController aCont = new AdminController();
            List<AgencyInfo> agencies = aCont.getAgencies(PortalId);
            ddlAgencies.Items.Add(new ListItem("--Select Agency--", "-1"));
            List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
            List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            if (canSeeAll)
            {
                if (selAd == "" || selAd == "-1")
                {
                    foreach (AgencyInfo ag in agencies)
                    {
                        ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                        ddlAgencies.Items.Add(li);
                    }
                }
                else
                {
                    AdvertiserInfo adv = aCont.Get_AdvertiserById(Convert.ToInt32(selAd));
                    List<AgencyInfo> sortedList = adv.Agencies.Distinct(new AgencyComparer()).OrderBy(o => o.AgencyName).ToList();
                    foreach (AgencyInfo ag in sortedList)
                    {
                        //if (selAd == ag.Id.ToString())
                        //{
                        ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                        ddlAgencies.Items.Add(li);
                        //}
                    }
                }
            }
            else
            {
                List<AgencyInfo> agSels = new List<AgencyInfo>();
                if ((selAd != "" && selAd != "-1"))
                {
                    agSels = aCont.Get_AgenciesByAdvertiserId(Convert.ToInt32(selAd));
                }
                if (userCase == 1) //ad/ad //(drawCase == 1 || drawCase == 4)
                {
                    List<AgencyInfo> addAgs = new List<AgencyInfo>();
                    foreach (AdvertiserInfo adv in userAds)
                    {
                        if ((selAd == "" || selAd == "-1"))
                        {
                            foreach (AgencyInfo ag in adv.Agencies)
                            {
                                addAgs.Add(ag);
                            }
                        }
                        else
                        {
                            foreach (AgencyInfo ag in adv.Agencies)
                            {
                                if (adv.Id.ToString() == selAd)
                                {
                                    addAgs.Add(ag);
                                }
                            }
                        }
                    }
                    List<AgencyInfo> sortedList = addAgs.Distinct(new AgencyComparer()).OrderBy(o => o.AgencyName).ToList();
                    foreach (AgencyInfo ag in sortedList)
                    {
                        ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                        ddlAgencies.Items.Add(li);
                    }
                }
                else if (userCase == 2) //(drawCase==2 || drawCase==5) //all agencies user is connected to
                {
                    List<AgencyInfo> sortedList = userAgs.Distinct(new AgencyComparer()).OrderBy(o => o.AgencyName).ToList();
                    if ((selAd == "" || selAd == "-1"))
                    {
                        foreach (AgencyInfo ag in sortedList)
                        {
                            ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                            ddlAgencies.Items.Add(li);
                        }
                    }
                    else
                    {
                        foreach (AgencyInfo ag in sortedList)
                        {
                            foreach (AgencyInfo agSel in agSels)
                            {
                                if (agSel.Id == ag.Id)
                                {
                                    ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                                    ddlAgencies.Items.Add(li);
                                }
                            }
                        }
                    }
                }
                else if (userCase == 3) //(drawCase==3) //all agencies user is connected to and all agencies connected to the advertisers the user is connected to
                {
                    List<AgencyInfo> ags = new List<AgencyInfo>();
                    foreach (AgencyInfo ag in userAgs)
                    {
                        ags.Add(ag);
                    }
                    foreach (AdvertiserInfo ad in userAds)
                    {
                        List<AgencyInfo> adAgs = aCont.Get_AgenciesByAdvertiserId(ad.Id);
                        foreach (AgencyInfo ag in adAgs)
                        {
                            ags.Add(ag);
                        }
                    }
                    List<AgencyInfo> sortedList = ags.Distinct(new AgencyComparer()).OrderBy(o => o.AgencyName).ToList();
                    if ((selAd == "" || selAd == "-1"))
                    {
                        foreach (AgencyInfo ag in sortedList)
                        {
                            ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                            ddlAgencies.Items.Add(li);
                        }
                    }
                    else
                    {
                        foreach (AgencyInfo ag in sortedList)
                        {
                            foreach (AgencyInfo agSel in agSels)
                            {
                                if (agSel.Id == ag.Id)
                                {
                                    ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                                    ddlAgencies.Items.Add(li);
                                }
                            }
                        }
                    }
                }
            }
            if (ddlAgencies.Items.Count == 2)
            {
                ddlAgencies.SelectedIndex = 1;
            }
            else if (selAg != "")
            {
                try
                {
                    ddlAgencies.SelectedValue = selAg;
                }
                catch { }
            }
        }
        private void FillAgencyListOld()
        {
            ddlAgencies.Items.Clear();
            bool canSeeAll = false;
            //TODO:Make these roles a setting
            if (UserInfo.IsInRole("Administrators") || UserInfo.IsInRole("PMT Admin") || UserInfo.IsInRole("PMT ADMIN"))
            {
                canSeeAll = true;
            }
            AdminController aCont = new AdminController();
            List<AgencyInfo> agencies = aCont.getAgencies(PortalId); //aCont.Get_AgenciesByPortalId(PortalId);
            ddlAgencies.Items.Add(new ListItem("--Select Agency--", "-1"));
            List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
            List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            if (canSeeAll)
            {
                foreach (AgencyInfo ag in agencies)
                {

                    ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                    ddlAgencies.Items.Add(li);
                }
            }
            else if(!canSeeAll && UserInfo.IsInRole("Advertiser"))
            {
                List<AgencyInfo> tempAgs = new List<AgencyInfo>();
                foreach(AdvertiserInfo ad in userAds)
                {
                    foreach(AgencyInfo ag in ad.Agencies)
                    {
                        bool isInThere = false;
                        foreach(AgencyInfo tempAg in tempAgs)
                        {
                            if(tempAg.Id==ag.Id)
                            {
                                isInThere = true;
                            }
                        }
                        if(!isInThere)
                        {
                            tempAgs.Add(ag);
                        }
                    }
                }
                foreach (AgencyInfo ag in tempAgs)
                {
                    ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                    ddlAgencies.Items.Add(li);
                }
            }
            else
            {
                foreach (AgencyInfo userAg in userAgs)
                {
                    ListItem li = new ListItem(userAg.AgencyName, userAg.Id.ToString());
                    ddlAgencies.Items.Add(li);
                }
            }
            //foreach (AgencyInfo agency in agencies)
            //{
            //    if (isAdmin)
            //    {
            //        ddlMasterItemAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
            //    }
            //    else
            //    {
            //        foreach (AgencyInfo ag in userAgs)
            //        {
            //            if (ag.Id == agency.Id)
            //            {
            //                ddlMasterItemAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
            //            }
            //        }
            //    }
            //}
        }
        //private void FillAgencyList()
        //{
        //    AdminController aCont = new AdminController();
        //    List<AgencyInfo> agencies = new List<AgencyInfo>();
        //    if (ddlAdvertisers.SelectedIndex == 0)
        //    { agencies = aCont.Get_AgenciesByPortalId(PortalId); }
        //    else
        //    {
        //        agencies = aCont.Get_AgenciesByAdvertiserId(Convert.ToInt32(ddlAdvertisers.SelectedValue));
        //    }
        //    ddlAgencies.Items.Clear();
        //    ddlAgencies.Items.Add(new ListItem("--Select Agency--", "-1"));
        //    List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
        //    bool isAdmin = UserInfo.IsInRole("Administrators");
        //    foreach (AgencyInfo agency in agencies)
        //    {
        //        if (isAdmin)
        //        {
        //            ddlAgencies.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
        //        }
        //        else
        //        {
        //            foreach (AgencyInfo ag in userAgs)
        //            {
        //                if (ag.Id == agency.Id)
        //                {
        //                    ddlAgencies.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
        //                }
        //            }
        //        }
        //    }
        //    //filterMedia();
        //}
        private void redrawItems()
        {
            if (ViewState["masterIds"] != null)
            {
                List<int> masterIds = (List<int>)ViewState["masterIds"];
                foreach (int masterId in masterIds)
                {
                    Literal lit1 = new Literal();
                    lit1.ID = "lit1_" + masterId.ToString();
                    plMasters.Controls.Add(lit1);
                    ImageButton img = new ImageButton();
                    img.ID = "img_" + masterId.ToString();
                    //img.ImageUrl = "images/play.jpg";
                    //img.Click += new ImageClickEventHandler(imgPlay_click);
                    plMasters.Controls.Add(img);
                    Literal lit2 = new Literal();
                    lit2.ID = "lit2_" + masterId.ToString();
                    //lit2.Text = "</div>";
                    //lit2.Text += "<div class=\"mastersRowCell mastersCol8\">";
                    plMasters.Controls.Add(lit2);
                    CheckBox chk = new CheckBox();
                    chk.ID = "chk_" + masterId.ToString();
                    //chk.AutoPostBack = true;
                    //chk.CheckedChanged += new EventHandler(masterChecked_check);
                    plMasters.Controls.Add(chk);
                    Literal lit3 = new Literal();
                    lit3.ID = "lit3_" + masterId.ToString();
                    //lit3.Text = "</div></div>";
                    plMasters.Controls.Add(lit3);
                }
                filterMedia(true);                
            }
            if (ViewState["libraryIds"] != null)
                {
                    List<int> libraryIds = (List<int>)ViewState["libraryIds"];
                    foreach (int libraryId in libraryIds)
                    {
                        Literal lit1 = new Literal();
                        lit1.ID = "lit1lib_" + libraryId.ToString();
                        plLibrary.Controls.Add(lit1);
                        CheckBox chk = new CheckBox();
                        chk.ID = "chklib_" + libraryId.ToString();
                        plLibrary.Controls.Add(chk);
                        Literal lit3 = new Literal();
                        lit3.ID = "lit3lib_" + libraryId.ToString();
                        //lit3.Text = "</div></div>";
                        plLibrary.Controls.Add(lit3);
                    }
                }
        }
        private void addGroupControls()
        {
            pnlGroups.Controls.Clear();
            if(ViewState["GroupControlIds"]!=null)
            {
                List<string> groupIds = (List<string>)ViewState["GroupControlIds"];
                foreach (string groupId in groupIds)
                {
                    GroupControl gc = (GroupControl)LoadControl("controls/GroupControl.ascx");
                    gc.ID = groupId;
                    pnlGroups.Controls.Add(gc);
                    gc.GroupUpdated += new EventHandler(gc_Updated);
                }
            }
        }
        protected void gc_Updated(object sender, EventArgs e)
        {
            drawGroups();
        }
        
        private List<StationInfo> getStations()
        {
            List<StationInfo> stats = new List<StationInfo>();
            if (Application["Stations"] != null)
            {
                stats = (List<StationInfo>)Application["Stations"];
            }
            else
            {
                AdminController aCont = new AdminController();
                stats = aCont.Get_StationsByPortalId(PortalId);
                Application["Stations"] = stats;
            }
            return stats;
        }
        private void filterMedia(bool libraryOnly = false)
        {
            AdminController aCont = new AdminController();
            List<MasterItemInfo> masters = aCont.getMasters(PortalId);
            if (!libraryOnly)
            {
                plMasters.Controls.Clear();
                List<int> MasterIds = new List<int>();
                if (ddlAdvertisers.SelectedIndex > 0 || ddlAgencies.SelectedIndex > 0 || txtMasterKeyword.Text != "")
                {                    
                    List<MasterItemInfo> mastersByAdvertiser = new List<MasterItemInfo>();
                    List<MasterItemInfo> mastersByAgency = new List<MasterItemInfo>();
                    List<MasterItemInfo> mastersByKeyword = new List<MasterItemInfo>();
                    List<AgencyInfo> agencies = aCont.Get_AgenciesByPortalId(PortalId);
                    List<AdvertiserInfo> advertisers = aCont.Get_AdvertisersByPortalId(PortalId);
                    if (ddlAdvertisers.SelectedIndex > 0)
                    {
                        mastersByAdvertiser = masters.Where(n => n.AdvertiserId.ToString() == ddlAdvertisers.SelectedValue).ToList();
                        //foreach (MasterItemInfo mast in masters)
                        //{
                        //    if (mast.AdvertiserId.ToString() == ddlAdvertisers.SelectedValue)
                        //    {
                        //        mastersByAdvertiser.Add(mast);
                        //    }
                        //}
                    }
                    else
                    {
                        mastersByAdvertiser = masters;
                    }
                    if (ddlAgencies.SelectedIndex > 0)
                    {
                        foreach (MasterItemInfo mast in mastersByAdvertiser)
                        {
                            foreach (AgencyInfo ag in mast.Agencies)
                            {
                                if (ag.Id.ToString() == ddlAgencies.SelectedValue)
                                {
                                    mastersByAgency.Add(mast);
                                }
                            }
                        }
                    }
                    else
                    {
                        mastersByAgency = mastersByAdvertiser;
                    }
                    if (txtMasterKeyword.Text != "")
                    {
                        foreach (MasterItemInfo master in mastersByAgency)
                        {
                            if (master.PMTMediaId.ToString().ToLower().IndexOf(txtMasterKeyword.Text.ToLower()) != -1 || master.Title.ToString().ToLower().IndexOf(txtMasterKeyword.Text.ToLower()) != -1)
                            {
                                mastersByKeyword.Add(master);
                            }
                        }
                    }
                    else
                    {
                        mastersByKeyword = mastersByAgency;
                    }
                    foreach (MasterItemInfo master in mastersByKeyword)
                    {
                        string agencyList = "";
                        foreach (AgencyInfo ag in master.Agencies)
                        {
                            var agency = agencies.FirstOrDefault(i => i.Id == ag.Id);
                            agencyList += agency.AgencyName + " ";
                        }
                        var advertiser = advertisers.FirstOrDefault(i => i.Id == master.AdvertiserId);
                        string status = "NEW";
                        if (master.CheckListForm != "" && !master.isApproved)
                        {
                            status = "PENDING";
                        }
                        else if (master.CheckListForm != "" && master.isApproved)
                        {
                            status = "APPROVED";
                        }
                        string ad = "";
                        try
                        {
                            ad = advertiser.AdvertiserName;
                        }
                        catch { }
                        Literal lit1 = new Literal();
                        lit1.ID = "lit1_" + master.Id.ToString();
                        lit1.Text = "<div class=\"mastersRow\">";
                        lit1.Text += "<div class=\"mastersRowCell mastersCol1\">" + master.PMTMediaId + "</div>";
                        lit1.Text += "<div class=\"mastersRowCell mastersCol2\">" + master.Title + "</div>";
                        lit1.Text += "<div class=\"mastersRowCell mastersCol3\">" + agencyList + "</div>";
                        lit1.Text += "<div class=\"mastersRowCell mastersCol4\">" + ad + "</div>";
                        lit1.Text += "<div class=\"mastersRowCell mastersCol5\">" + master.Length + "</div>";
                        lit1.Text += "<div class=\"mastersRowCell mastersCol6\">" + status + "</div>";
                        lit1.Text += "<div class=\"mastersRowCell mastersCol7\">";
                        plMasters.Controls.Add(lit1);
                        if (master.Filename.IndexOf(".mp4") != -1)
                        {
                            ImageButton img = new ImageButton();
                            img.ID = "img_" + master.Id.ToString();
                            img.ImageUrl = "images/play.jpg";
                            img.Click += new ImageClickEventHandler(imgPlay_click);
                            plMasters.Controls.Add(img);
                        }
                        Literal lit2 = new Literal();
                        lit2.ID = "lit2_" + master.Id.ToString();
                        lit2.Text = "</div>";
                        lit2.Text += "<div class=\"mastersRowCell mastersCol8\">";
                        plMasters.Controls.Add(lit2);
                        CheckBox chk = new CheckBox();
                        chk.ID = "chk_" + master.Id.ToString();
                        chk.AutoPostBack = true;
                        chk.CheckedChanged += new EventHandler(masterChecked_check);
                        plMasters.Controls.Add(chk);
                        Literal lit3 = new Literal();
                        lit3.ID = "lit3_" + master.Id.ToString();
                        lit3.Text = "</div></div>";
                        plMasters.Controls.Add(lit3);
                        MasterIds.Add(master.Id);
                    }
                    //Literal litd = new Literal();
                    //litd.Text = "</div>";
                    //plMasters.Controls.Add(litd);
                    ViewState["masterIds"] = MasterIds;
                    lblMastersTotal.Text = mastersByKeyword.Count.ToString() + " Master Items Found";
                }
                else
                {
                    lblMastersTotal.Text = "";
                }
            }
            //library items
            plLibrary.Controls.Clear();
            List<int> LibraryIds = new List<int>();
            if (ddlAdvertisers.SelectedIndex > 0 || ddlAgencies.SelectedIndex > 0 || txtLibrarySearch.Text != "" || txtMasterKeyword.Text != "")
            {
                List<int> MastersSelected = new List<int>();
                if (ViewState["masterIds"] != null)
                {
                    List<int> masterIds = (List<int>)ViewState["masterIds"];
                    foreach (int masterId in masterIds)
                    {
                        CheckBox chk = (CheckBox)plMasters.FindControl("chk_" + masterId.ToString());
                        if (chk != null && chk.Checked)
                        {
                            MastersSelected.Add(masterId);
                        }
                    }
                }
                if (!(ddlAdvertisers.SelectedIndex == 0 && ddlAgencies.SelectedIndex == 0 && txtLibrarySearch.Text == "" && txtMasterKeyword.Text != "" && MastersSelected.Count == 0))
                {

                    List<LibraryItemInfo> libs =aCont.getLibs(PortalId);
                    List<LibraryItemInfo> libsByAdvertiser = new List<LibraryItemInfo>();
                    List<LibraryItemInfo> libsByAgency = new List<LibraryItemInfo>();
                    List<LibraryItemInfo> libsByKeyword = new List<LibraryItemInfo>();
                    List<LibraryItemInfo> libsByMasters = new List<LibraryItemInfo>();
                    List<AgencyInfo> agencies = aCont.Get_AgenciesByPortalId(PortalId);
                    List<AdvertiserInfo> advertisers = aCont.Get_AdvertisersByPortalId(PortalId);
                    if (ddlAdvertisers.SelectedIndex > 0)
                    {
                        libsByAdvertiser = libs.Where(n => n.AdvertiserId.ToString() == ddlAdvertisers.SelectedValue).ToList();
                        //foreach (LibraryItemInfo lib in libs)
                        //{
                        //    if (lib.AdvertiserId.ToString() == ddlAdvertisers.SelectedValue)
                        //    {
                        //        libsByAdvertiser.Add(lib);
                        //    }
                        //}
                    }
                    else
                    {
                        libsByAdvertiser = libs;
                    }
                    if (ddlAgencies.SelectedIndex > 0)
                    {
                        foreach (LibraryItemInfo lib in libsByAdvertiser)
                        {
                            if (lib.AgencyId.ToString() == ddlAgencies.SelectedValue)
                            {
                                libsByAgency.Add(lib);
                            }
                        }
                    }
                    else
                    {
                        libsByAgency = libsByAdvertiser;
                    }
                    if (txtLibrarySearch.Text != "")
                    {
                        foreach (LibraryItemInfo lib in libsByAgency)
                        {
                            if (lib.PMTMediaId.ToString().ToLower().IndexOf(txtLibrarySearch.Text.ToLower()) != -1 || lib.Title.ToString().ToLower().IndexOf(txtLibrarySearch.Text.ToLower()) != -1 || lib.ProductDescription.ToString().ToLower().IndexOf(txtLibrarySearch.Text.ToLower()) != -1 || lib.ISCICode.ToString().ToLower().IndexOf(txtLibrarySearch.Text.ToLower()) != -1)
                            {
                                libsByKeyword.Add(lib);
                            }
                        }
                    }
                    else
                    {
                        libsByKeyword = libsByAgency;
                    }
                    //filter by selected Master Items

                    if (MastersSelected.Count > 0)
                    {
                        foreach (int MasterSelected in MastersSelected)
                        {
                            var master = masters.FirstOrDefault(i => i.Id == MasterSelected);
                            foreach (LibraryItemInfo lib in libsByKeyword)
                            {
                                if (lib.MasterId == MasterSelected || lib.PMTMediaId == master.PMTMediaId)
                                {
                                    libsByMasters.Add(lib);
                                }
                            }
                        }
                    }
                    else
                    {
                        libsByMasters = libsByKeyword;
                    }

                    foreach (LibraryItemInfo lib in libsByMasters)
                    {
                        var agency = agencies.FirstOrDefault(i => i.Id == lib.AgencyId);
                        var advertiser = advertisers.FirstOrDefault(i => i.Id == lib.AdvertiserId);

                        string ad = "";
                        try
                        {
                            ad = advertiser.AdvertiserName;
                        }
                        catch { }
                        Literal lit1 = new Literal();
                        lit1.ID = "lit1lib_" + lib.Id.ToString();
                        lit1.Text = "<div class=\"mastersRow\">";
                        lit1.Text += "<div class=\"mastersRowCell libraryCol1\">" + lib.PMTMediaId + "</div>";
                        lit1.Text += "<div class=\"mastersRowCell libraryCol2\">" + lib.ISCICode + "</div>";
                        lit1.Text += "<div class=\"mastersRowCell libraryCol3\">" + lib.Title + "</div>";
                        lit1.Text += "<div class=\"mastersRowCell libraryCol4\">" + lib.ProductDescription + "</div>";
                        lit1.Text += "<div class=\"mastersRowCell libraryCol5\">" + lib.MediaLength + "</div>";
                        lit1.Text += "<div class=\"mastersRowCell libraryCol6\">" + ad + "</div>";
                        lit1.Text += "<div class=\"mastersRowCell libraryCol7\">";
                        plLibrary.Controls.Add(lit1);
                        //ImageButton img = new ImageButton();
                        //img.ID = "img_" + master.Id.ToString();
                        //img.ImageUrl = "images/play.jpg";
                        //img.Click += new ImageClickEventHandler(imgPlay_click);
                        //plMasters.Controls.Add(img);
                        //Literal lit2 = new Literal();
                        //lit2.ID = "lit2_" + master.Id.ToString();
                        //lit2.Text = "</div>";
                        //lit2.Text += "<div class=\"mastersRowCell mastersCol8\">";
                        //plMasters.Controls.Add(lit2);
                        CheckBox chk = new CheckBox();
                        chk.ID = "chklib_" + lib.Id.ToString();
                        plLibrary.Controls.Add(chk);
                        Literal lit3 = new Literal();
                        lit3.ID = "lit3lib_" + lib.Id.ToString();
                        lit3.Text = "</div></div>";
                        plLibrary.Controls.Add(lit3);
                        LibraryIds.Add(lib.Id);
                    }
                    Literal litd = new Literal();
                    litd.Text = "</div>";
                    plLibrary.Controls.Add(litd);
                    ViewState["libraryIds"] = LibraryIds;
                    lblLibraryTotal.Text = libsByMasters.Count.ToString() + " Library Items Found";
                }
            }
            else
            {
                lblLibraryTotal.Text = "";
            }
        }
        private void fillStationGroups()
        {
            AdminController aCont = new AdminController();
            List<StationGroupInfo> stationGroups = new List<StationGroupInfo>();
            if (UserInfo.IsInRole("Agency") || UserInfo.IsInRole("Advertiser"))
            {
                stationGroups = aCont.Get_StationGroupsByUserId(UserId);
            }
            else
            {
                stationGroups = aCont.Get_StationGroupsByPortalId(PortalId);
            }
            ddlStationGroups.Items.Clear();
            ddlStationGroups.Items.Add("--Please Select Station Group--");
            foreach(StationGroupInfo sg in stationGroups)
            {
                ListItem li = new ListItem(sg.StationGroupName,sg.Id.ToString());
                ddlStationGroups.Items.Add(li);
            }
        }
        //private List<int> getMarketsAndParents(int marketId)
        //{
        //    List<int> marketsAndParents = new List<int>();


        //    return marketsAndParents;
        //}
        private void fillStations()
        {
            AdminController aCont = new AdminController();
            plStations.Controls.Clear();
            List<MarketInfo> markets = aCont.Get_MarketsByPortalId(PortalId);
            List<StationInfo> stations = getStations();
            List<StationInfo> statsByGroup = new List<StationInfo>();
            List<StationInfo> statsByKeyword = new List<StationInfo>();
            List<StationInfo> statsByFormat = new List<StationInfo>();
            if(ddlStationGroups.SelectedIndex>0)
            {
                statsByGroup = aCont.Get_StationsinGroupById(Convert.ToInt32(ddlStationGroups.SelectedValue));
                if (txtStationSearch.Text != "")
                {
                    foreach (StationInfo stat in stations)
                    {
                        if (stat.Status)
                        {
                            var market = markets.FirstOrDefault(i => i.Id == stat.MarketId);
                            string marketName = "";
                            try { marketName = market.MarketName; }
                            catch { }
                            if (stat.CallLetter.ToLower().IndexOf(txtStationSearch.Text.ToLower()) != -1 || stat.City.ToLower().IndexOf(txtStationSearch.Text.ToLower()) != -1 || marketName.ToLower().IndexOf(txtStationSearch.Text.ToLower()) != -1)
                            {
                                if (ddlStationFormat.SelectedValue == "-1")
                                {
                                    statsByGroup.Add(stat);
                                }
                                else if (ddlStationFormat.SelectedValue != "-1" && ddlStationFormat.SelectedValue == stat.ProgramFormat)
                                {
                                    statsByGroup.Add(stat);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (txtStationSearch.Text != "")
                {
                    foreach (StationInfo stat in stations)
                    {
                        if (stat.Status)
                        {
                            var market = markets.FirstOrDefault(i => i.Id == stat.MarketId);
                            string marketName = "";
                            try { marketName = market.MarketName; }
                            catch { }
                            if (stat.CallLetter.ToLower().IndexOf(txtStationSearch.Text.ToLower()) != -1 || stat.City.ToLower().IndexOf(txtStationSearch.Text.ToLower()) != -1 || marketName.ToLower().IndexOf(txtStationSearch.Text.ToLower()) != -1)
                            {
                                if (ddlStationFormat.SelectedValue == "-1")
                                {
                                    statsByGroup.Add(stat);
                                }
                                else if (ddlStationFormat.SelectedValue != "-1" && ddlStationFormat.SelectedValue == stat.ProgramFormat)
                                {
                                    statsByGroup.Add(stat);
                                }
                            }
                        }
                    }
                }
            }
            int c = 0;
            List<int> stationIds = new List<int>();
            foreach (StationInfo stat in statsByGroup)
            {
                if (stat.Status)
                {
                    var market = markets.FirstOrDefault(i => i.Id == stat.MarketId);
                    string marketName = "";
                    try { marketName = market.MarketName; }
                    catch { }
                    Literal lit1 = new Literal();
                    lit1.ID = "lit1stat_" + stat.Id.ToString();
                    lit1.Text = "<div class=\"mastersRow\">";
                    lit1.Text += "<div class=\"mastersRowCell stationsCol1\">" + stat.CallLetter + "</div>";
                    lit1.Text += "<div class=\"mastersRowCell stationsCol2\">" + marketName + "</div>";
                    lit1.Text += "<div class=\"mastersRowCell stationsCol3\">" + stat.City + "</div>";
                    lit1.Text += "<div class=\"mastersRowCell stationsCol4\">";
                    plStations.Controls.Add(lit1);
                    CheckBox chk = new CheckBox();
                    chk.ID = "chkstat_" + stat.Id.ToString();
                    plStations.Controls.Add(chk);
                    Literal lit3 = new Literal();
                    lit3.ID = "lit3stat_" + stat.Id.ToString();
                    lit3.Text = "</div></div>";
                    plStations.Controls.Add(lit3);
                    stationIds.Add(stat.Id);
                    c++;
                }
            }
            ViewState["stationIds"] = stationIds;
            lblStationsTotal.Text = c.ToString() + " Stations Found";
        }
        protected void imgPlay_click(object sender, EventArgs e)
        {
        }
        protected void masterChecked_check(object sender, EventArgs e)
        {
            filterMedia(true);
        }
        protected void ddlAdvertisers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["selAdvertiser"] = ddlAdvertisers.SelectedValue;
            FillAgencyList();
        }

        protected void ddlAgencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["selAgency"] = ddlAgencies.SelectedValue;
            filterMedia();
        }

        protected void txtMasterKeyword_TextChanged(object sender, EventArgs e)
        {
            filterMedia();
        }

        protected void imgExpandMasters_Click(object sender, ImageClickEventArgs e)
        {
            if(pnlMasters.Visible)
            {
                pnlMasters.Visible = false;
                imgExpandMasters.ImageUrl = "/DesktopModules/PMT_Admin/images/arrow_closed.jpg";
            }
            else
            {
                pnlMasters.Visible = true;
                imgExpandMasters.ImageUrl = "/DesktopModules/PMT_Admin/images/arrow_open.jpg";
                filterMedia();
            }
        }

        protected void btnMastersSelectAll_Click(object sender, EventArgs e)
        {
            filterMedia();
            if(ViewState["masterIds"]!=null)
            {
                List<int> masterIds = (List<int>)ViewState["masterIds"];
                foreach(int masterId in masterIds)
                {
                    CheckBox chk = (CheckBox)plMasters.FindControl("chk_" + masterId.ToString());
                    if (chk != null)
                    {
                        chk.Checked = true;
                    }
                }
            }
            filterMedia(true);
        }

        protected void btnMastersClearAll_Click(object sender, EventArgs e)
        {
            filterMedia();
            if (ViewState["masterIds"] != null)
            {
                List<int> masterIds = (List<int>)ViewState["masterIds"];
                foreach (int masterId in masterIds)
                {
                    CheckBox chk = (CheckBox)plMasters.FindControl("chk_" + masterId.ToString());
                    if (chk != null)
                    {
                        chk.Checked = false;
                    }
                }
            }
            filterMedia(true);
        }

        protected void txtLibrarySearch_TextChanged(object sender, EventArgs e)
        {
            filterMedia(true);
        }

        protected void btnLibrarySelectAll_Click(object sender, EventArgs e)
        {
            filterMedia(true);
            if (ViewState["libraryIds"] != null)
            {
                List<int> libraryIds = (List<int>)ViewState["libraryIds"];
                foreach (int libraryId in libraryIds)
                {
                    CheckBox chk = (CheckBox)plMasters.FindControl("chklib_" + libraryId.ToString());
                    if (chk != null)
                    {
                        chk.Checked = true;
                    }
                }
            }
        }

        protected void btnLibraryClearAll_Click(object sender, EventArgs e)
        {
            filterMedia(true);
            if (ViewState["libraryIds"] != null)
            {
                List<int> libraryIds = (List<int>)ViewState["libraryIds"];
                foreach (int libraryId in libraryIds)
                {
                    CheckBox chk = (CheckBox)plMasters.FindControl("chklib_" + libraryId.ToString());
                    if (chk != null)
                    {
                        chk.Checked = false;
                    }
                }
            }
        }

        protected void imgExpandLibrary_Click(object sender, ImageClickEventArgs e)
        {
            if (pnlLibrary.Visible)
            {
                pnlLibrary.Visible = false;
                imgExpandLibrary.ImageUrl = "/DesktopModules/PMT_Admin/images/arrow_closed.jpg";
            }
            else
            {
                pnlLibrary.Visible = true;
                imgExpandLibrary.ImageUrl = "/DesktopModules/PMT_Admin/images/arrow_open.jpg";
                filterMedia(true);
            }
        }

        protected void btnMasterAdd_Click(object sender, EventArgs e)
        {
            if (ViewState["masterIds"] != null)
            {
                List<int> masterIds = (List<int>)ViewState["masterIds"];
                foreach (int masterId in masterIds)
                {
                    CheckBox chk = (CheckBox)plMasters.FindControl("chk_" + masterId.ToString());
                    if (chk != null && chk.Checked)
                    {
                        //create new NonDeliverable group
                        WOGroupInfo ndGroup = new WOGroupInfo();
                        ndGroup.GroupType = GroupTypeEnum.Non_Deliverable;
                        ndGroup.PortalId = PortalId;
                        ndGroup.index = Groups.Count;
                        ndGroup.CreatedById = UserId;
                        ndGroup.DateCreated = DateTime.Now;
                        ndGroup.GroupName = "Group " + (Groups.Count + 1).ToString() + " " + ndGroup.GroupType.ToString();
                        ndGroup.LastModifiedById = UserId;
                        ndGroup.LastModifiedDate = DateTime.Now;
                        List<WOGroupInfo> groups = Groups;
                        groups.Add(ndGroup);
                        Groups = groups;
                    }
                }
                drawGroups();
            }
        }

        protected void btnLibraryAdd_Click(object sender, EventArgs e)
        {
            List<WOGroupInfo> groups = Groups;
            List<LibraryItemInfo> libsToAdd = new List<LibraryItemInfo>();
            AdminController aCont = new AdminController();
            List<LibraryItemInfo> libItems = aCont.getLibs(PortalId);

            foreach(WOGroupInfo group in groups)
            {
                if(group.GroupName == ddlLibraryAddToGroup.SelectedItem.Text)
                {
                    //see if this a bulk add
                    if(txtAddBulk.Visible && txtAddBulk.Text != "")
                    {
                        string[] addLines = txtAddBulk.Text.Split('\n');
                        foreach(string addLine in addLines)
                        {
                            //string[] vals = addLine.Split('|');
                            //if(vals.Count()>0)
                            //{
                                var lib = libItems.FirstOrDefault(o => o.ISCICode == addLine);
                                if(lib!=null)
                                {
                                    bool inThere = false;
                                    foreach (LibraryItemInfo libItem in group.LibraryItems)
                                    {
                                        if (libItem.Id == lib.Id)
                                        {
                                            inThere = true;
                                        }
                                    }
                                    if (!inThere)
                                    {
                                        group.LibraryItems.Add(lib);
                                    }
                                }
                            //}
                        }
                    }
                    //get checked lib items
                    if (ViewState["libraryIds"] != null)
                    {                        
                        List<int> libraryIds = (List<int>)ViewState["libraryIds"];
                        foreach (int libraryId in libraryIds)
                        {
                            CheckBox chk = (CheckBox)plMasters.FindControl("chklib_" + libraryId.ToString());
                            if (chk != null && chk.Checked)
                            {
                                var libItem = libItems.FirstOrDefault(i => i.Id == libraryId);
                                //now, see if that lib item is already in the group
                                bool inThere = false;
                                foreach(LibraryItemInfo lib in group.LibraryItems)
                                {
                                    if(lib.Id==libraryId)
                                    {
                                        inThere = true;
                                    }
                                }
                                if(!inThere)
                                {
                                    group.LibraryItems.Add(libItem);
                                }
                            }
                        }
                        Groups = groups;
                        drawGroups();
                    }
                }
            }
        }

        protected void txtStationSearch_TextChanged(object sender, EventArgs e)
        {
            fillStations();
        }

        protected void imgExpandStations_Click(object sender, ImageClickEventArgs e)
        {
            if (pnlStations.Visible)
            {
                pnlStations.Visible = false;
                imgExpandLibrary.ImageUrl = "/DesktopModules/PMT_Admin/images/arrow_closed.jpg";
            }
            else
            {
                pnlStations.Visible = true;
                imgExpandLibrary.ImageUrl = "/DesktopModules/PMT_Admin/images/arrow_open.jpg";
            }
        }

        protected void ddlStationGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillStations();
        }

        protected void btnStationsAdd_Click(object sender, EventArgs e)
        {
            List<WOGroupInfo> groups = Groups;
            List<WOGroupStationInfo> statsToAdd = new List<WOGroupStationInfo>();

            foreach (WOGroupInfo group in groups)
            {
                if (group.GroupName == ddlStationsAddToGroup.SelectedItem.Text)
                {
                    //get checked station items
                    if (ViewState["stationIds"] != null)
                    {
                        List<StationInfo> statItems = getStations();
                        List<int> stationIds = (List<int>)ViewState["stationIds"];
                        foreach (int stationId in stationIds)
                        {
                            CheckBox chk = (CheckBox)plMasters.FindControl("chkstat_" + stationId.ToString());
                            if (chk != null && chk.Checked)
                            {
                                var stat = statItems.FirstOrDefault(i => i.Id == stationId);
                                //now, see if that station is already in the group
                                bool inThere = false;
                                foreach (WOGroupStationInfo wostat in group.WOGroupStations)
                                {
                                    if (wostat.StationId == stationId)
                                    {
                                        inThere = true;
                                    }
                                }
                                if (!inThere)
                                {
                                    WOGroupStationInfo newStation = new WOGroupStationInfo();
                                    newStation.PortalId = PortalId;
                                    newStation.Station = stat;
                                    newStation.CreatedById = UserId;
                                    newStation.DateCreated = DateTime.Now;
                                    newStation.LastModifiedById = UserId;
                                    newStation.LastModifiedDate = DateTime.Now;
                                    newStation.StationId = stat.Id;
                                    group.WOGroupStations.Add(newStation);
                                }
                            }
                        }
                        Groups = groups;
                        drawGroups();
                    }
                }
            }
        }

        protected void btnStationsSelectAll_Click(object sender, EventArgs e)
        {
            fillStations();
            if (ViewState["stationIds"] != null)
            {
                List<int> stationIds = (List<int>)ViewState["stationIds"];
                foreach (int stationId in stationIds)
                {
                    CheckBox chk = (CheckBox)plMasters.FindControl("chkstat_" + stationId.ToString());
                    if (chk != null)
                    {
                        chk.Checked = true;
                    }
                }
            }
        }

        protected void btnStationsClearAll_Click(object sender, EventArgs e)
        {
            fillStations();
            if (ViewState["stationIds"] != null)
            {
                List<int> stationIds = (List<int>)ViewState["stationIds"];
                foreach (int stationId in stationIds)
                {
                    CheckBox chk = (CheckBox)plMasters.FindControl("chkstat_" + stationId.ToString());
                    if (chk != null)
                    {
                        chk.Checked = false;
                    }
                }
            }
        }
        private void drawGroups()
        {
            string LibGroup = "";
            if (ddlLibraryAddToGroup.SelectedItem != null)
            {
                LibGroup = ddlLibraryAddToGroup.SelectedItem.Text;
            }
            string StatGroup = "";
            if(ddlStationsAddToGroup.SelectedItem!=null)
            {
                StatGroup = ddlStationsAddToGroup.SelectedItem.Text;
            }
            ddlLibraryAddToGroup.Items.Clear();
            //ddlMastersAddToGroup.Items.Clear();
            ddlStationsAddToGroup.Items.Clear();
            pnlGroups.Controls.Clear();
            List<string> controlIds = new List<string>();
            int c = 0;
            int groupCount = 0;
            int libItemsCount = 0;
            int statCount = 0;
            int masterCount = 0;
            int serviceCount = 0;
            foreach(WOGroupInfo group in Groups)
            {
                if(group.GroupType==GroupTypeEnum.Non_Deliverable)
                {
                    //ddlMastersAddToGroup.Items.Add(new ListItem(group.GroupName, group.index.ToString()));
                }
                else
                {
                    if (group.CreatedById != -1)
                    {
                        ListItem li = new ListItem(group.GroupName, group.index.ToString());
                        if(LibGroup!="" && group.GroupName==LibGroup)
                        {
                            li.Selected = true;
                        }
                        ddlLibraryAddToGroup.Items.Add(li);
                        ListItem li2 = new ListItem(group.GroupName, group.index.ToString());
                        if (StatGroup != "" && group.GroupName == StatGroup)
                        {
                            li2.Selected = true;
                        }
                        ddlStationsAddToGroup.Items.Add(li2);
                    }
                }
                GroupControl gc = (GroupControl)LoadControl("controls/GroupControl.ascx");
                gc.EditMode = WOEditMode;
                gc.ID = "group_" + c.ToString();
                c++;
                controlIds.Add(gc.ID);
                gc.Group = group;
                gc.GroupUpdated += new EventHandler(gcControl_GroupUpdated);
                gc.GroupDeleted += new EventHandler(gcControl_GroupDeleted);
                WOGroupInfo newGroup = new WOGroupInfo();
                if(group.CreatedById==-1)
                {
                    gc.Visible = false;
                }
                else
                {
                    gc.Visible = true;
                    groupCount++;
                    libItemsCount += group.LibraryItems.Count;
                    statCount += group.WOGroupStations.Count;
                    if(group.GroupType==GroupTypeEnum.Non_Deliverable)
                    {
                        masterCount++;
                        serviceCount += group.Services.Count;
                    }
                }
                pnlGroups.Controls.Add(gc);
            }
            ViewState["GroupControlIds"] = controlIds;
            litWOSummary.Text = "<div class=\"woSummaryCol1\">GROUPS</div><div class=\"woSummaryCol2\">" + groupCount.ToString() + "</div>";
            litWOSummary.Text += "<div class=\"woSummaryCol1\">LIBRARY ITEMS</div><div class=\"woSummaryCol2\">" + libItemsCount.ToString() + "</div>";
            litWOSummary.Text += "<div class=\"woSummaryCol1\">STATIONS</div><div class=\"woSummaryCol2\">" + statCount.ToString() + "</div>";
            if(masterCount>0)
            {
                litWOSummary.Text += "<div class=\"woSummaryCol1\">MASTERS</div><div class=\"woSummaryCol2\">" + masterCount.ToString() + "</div>";
                litWOSummary.Text += "<div class=\"woSummaryCol1\">SERVICES</div><div class=\"woSummaryCol2\">" + serviceCount.ToString() + "</div>";
            }
            litWOSummary.Text += "</div><br clear=\"both\" />";
        }
        public void gcControl_GroupUpdated(object sender, EventArgs e)
        {
            GroupControl gc = (GroupControl)sender;
            var group = Groups.FirstOrDefault(i => i.index == gc.Group.index);
            List<WOGroupInfo> groups = Groups;
            groups[group.index] = group;
            Groups = groups;
            drawGroups();
        }
        public void gcControl_GroupDeleted(object sender, EventArgs e)
        {
            GroupControl gc = (GroupControl)sender;
            var group = Groups.FirstOrDefault(i => i.index == gc.Group.index);
            List<WOGroupInfo> groups = Groups;
            groups[group.index] = new WOGroupInfo();
            Groups = groups;
            drawGroups();
        }
        protected void btnDeliveryGroup_Click(object sender, EventArgs e)
        {
            createGroup(GroupTypeEnum.Delivery);
        }

        protected void btnBundleGroup_Click(object sender, EventArgs e)
        {
            createGroup(GroupTypeEnum.Bundle);
        }

        protected void btnCustomizeGroup_Click(object sender, EventArgs e)
        {
            createGroup(GroupTypeEnum.Customized);
        }
        private void createGroup (GroupTypeEnum groupType)
        {
            AdminController aCont = new AdminController();
            List<LibraryItemInfo> libItems = aCont.getLibs(PortalId); //aCont.getLibs(PortalId);
            List<WOGroupInfo> groups = Groups;
            int index = Groups.Count;
            WOGroupInfo newGroup = new WOGroupInfo();
            newGroup.PortalId = PortalId;
            newGroup.index = index;
            newGroup.CreatedById = UserId;
            newGroup.DateCreated = DateTime.Now;
            newGroup.GroupName = "Group " + (Groups.Count + 1).ToString() + " " + groupType.ToString();
            newGroup.GroupType = groupType;
            newGroup.LastModifiedById = UserId;
            newGroup.LastModifiedDate = DateTime.Now;
            if(ViewState["libraryIds"]!=null)
            {                
                List<int> libraryIds = (List<int>)ViewState["libraryIds"];
                foreach(int libraryId in libraryIds)
                {
                    CheckBox chk = (CheckBox)plMasters.FindControl("chklib_" + libraryId.ToString());
                    if (chk != null && chk.Checked)
                    {
                        var libItem = libItems.FirstOrDefault(i => i.Id == libraryId);
                        newGroup.LibraryItems.Add(libItem);
                    }
                }
            }
            //see if this a bulk add
            if (txtAddBulk.Visible && txtAddBulk.Text != "")
            {
                string[] addLines = txtAddBulk.Text.Split('\n');
                foreach (string addLine in addLines)
                {
                    //string[] vals = addLine.Split('|');
                    //if (vals.Count() > 0)
                    //{
                        var lib = libItems.FirstOrDefault(o => o.ISCICode == addLine);
                        if (lib != null)
                        {
                            newGroup.LibraryItems.Add(lib);
                        }
                    //}
                }
            }
            if (ViewState["stationIds"] != null && groupType!= GroupTypeEnum.Customized)
            {
                List<StationInfo> stations = getStations();
                List<int> stationIds = (List<int>)ViewState["stationIds"];
                foreach (int stationId in stationIds)
                {
                    CheckBox chk = (CheckBox)plMasters.FindControl("chkstat_" + stationId.ToString());
                    if (chk != null && chk.Checked)
                    {
                        var station = stations.FirstOrDefault(i => i.Id == stationId);
                        WOGroupStationInfo newStation = new WOGroupStationInfo();
                        newStation.PortalId = PortalId;
                        newStation.Station = station;
                        newStation.CreatedById = UserId;
                        newStation.DateCreated = DateTime.Now;
                        newStation.LastModifiedById = UserId;
                        newStation.LastModifiedDate = DateTime.Now;
                        newStation.StationId = station.Id;
                        newGroup.WOGroupStations.Add(newStation);
                    }
                }
            }
            groups.Add(newGroup);
            Groups = groups;
            ViewState["bob"] = "bob";
            drawGroups();
        }

        protected void btnAddToNonDelivery_Click(object sender, EventArgs e)
        {
            if (ViewState["masterIds"] != null)
            {
                AdminController aCont = new AdminController();
                List<int> masterIds = (List<int>)ViewState["masterIds"];
                List<MasterItemInfo> Masters = aCont.getMasters(PortalId);
                foreach (int masterId in masterIds)
                {
                    var master = Masters.FirstOrDefault(i => i.Id == masterId);
                    CheckBox chk = (CheckBox)plMasters.FindControl("chk_" + masterId.ToString());
                    if (chk != null && chk.Checked)
                    {
                        //create new NonDeliverable group
                        WOGroupInfo ndGroup = new WOGroupInfo();
                        ndGroup.GroupType = GroupTypeEnum.Non_Deliverable;
                        ndGroup.PortalId = PortalId;
                        ndGroup.index = Groups.Count;
                        ndGroup.CreatedById = UserId;
                        ndGroup.MasterId = masterId;
                        ndGroup.Master = master;
                        ndGroup.DateCreated = DateTime.Now;
                        ndGroup.GroupName = "Group " + (Groups.Count + 1).ToString() + " " + ndGroup.GroupType.ToString().Replace("_","-");
                        ndGroup.LastModifiedById = UserId;
                        ndGroup.LastModifiedDate = DateTime.Now;
                        List<WOGroupInfo> groups = Groups;
                        groups.Add(ndGroup);
                        Groups = groups;
                    }
                }
                drawGroups();
            }
        }

        protected void btnSubmitWorkOrder_Click(object sender, EventArgs e)
        {
            lblWOMessage.Text = "";
            if (!WOEditMode)
            {
                if(Request.QueryString["woid"]!=null)
                {
                    Response.Redirect("~/Work-Orders/woid/" + Request.QueryString["woid"].ToString() + "/edit/1");
                }
            }
            else
            {
                bool hasGroups = false;
                foreach (WOGroupInfo group in Groups)
                {
                    if (group.CreatedById != -1)
                    {
                        hasGroups = true;
                    }
                    foreach(WOGroupStationInfo stat in group.WOGroupStations)
                    {
                        if(stat.DeliveryMethod.ToLower().IndexOf("tf_")!=-1 && stat.PriorityId==-1)
                        {
                            lblWOMessage.Text += " Group: " + group.GroupName + ". You have selected a shipping method but no priority.";
                        }
                    }
                }
                if (!hasGroups)
                {
                    lblWOMessage.Text += " Please add at least one group to the order.";
                }
                if(ddlAdvertisers.SelectedIndex==0)
                {
                    lblWOMessage.Text += " You must select and Advertiser.";
                }
                if(ddlAgencies.SelectedIndex==0)
                {
                    lblWOMessage.Text += " You must select an Agency.";
                }
                if(lblWOMessage.Text=="")
                {
                    AdminController aCont = new AdminController();
                    WorkOrderInfo WorkOrder = new WorkOrderInfo();
                    if (Request.QueryString["woid"] != null)
                    {
                        WorkOrder = aCont.Get_WorkOrderById(Convert.ToInt32(Request.QueryString["woid"]));
                    }
                    WorkOrder.PortalId = PortalId;
                    WorkOrder.LastModifiedById = UserId;
                    WorkOrder.LastModifiedDate = DateTime.Now;
                    if (ddlAdvertisers.SelectedIndex > 0)
                    {
                        WorkOrder.AdvertiserId = Convert.ToInt32(ddlAdvertisers.SelectedValue);
                    }
                    else
                    {
                        //get AdvertiserId from first Master or Library Item
                        if (Groups[0].GroupType == GroupTypeEnum.Non_Deliverable)
                        {
                            WorkOrder.AdvertiserId = Groups[0].Master.AdvertiserId;
                        }
                        else
                        {
                            WorkOrder.AdvertiserId = Groups[0].LibraryItems[0].AdvertiserId;
                        }
                    }
                    WorkOrder.BillToId = WorkOrder.AdvertiserId;
                    if(ddlBillTo.SelectedIndex>0)
                    {
                        WorkOrder.BillToId = Convert.ToInt32(ddlBillTo.SelectedValue);
                    }
                    if (ddlAgencies.SelectedIndex > 0)
                    {
                        WorkOrder.AgencyId = Convert.ToInt32(ddlAgencies.SelectedValue);
                    }
                    else
                    {
                        //get AgencyId from first Master or Library Item
                        //if (Groups[0].GroupType == GroupTypeEnum.Non_Deliverable)
                        //{
                        //    ///TODO: What to do here?
                        //    //WorkOrder.AgencyId = Groups[0].Master;
                        //}
                        //else
                        //{
                        //    WorkOrder.AgencyId = Groups[0].LibraryItems[0].AgencyId;
                        //}
                    }
                    //Get billtoid from first Master
                    //if (Groups[0].GroupType == GroupTypeEnum.Non_Deliverable)
                    //{
                    //    WorkOrder.BillToId = Groups[0].Master.BillToId;
                    //}
                    //else
                    //{
                    //    MasterItemInfo billTo = aCont.Get_MasterItemById(Groups[0].LibraryItems[0].MasterId);
                    //    WorkOrder.BillToId = billTo.BillToId;
                    //}

                    WorkOrder.Description = txtWorkOrderDescription.Text;
                    WorkOrder.InvoiceNumber = txtWorkOrderQBBillingCode.Text;
                    WorkOrder.Notes = txtWONotes.Text;
                    WorkOrder.PONumber = txtWorkOrderPONumber.Text;
                    bool passedCheck = true;
                    if (Request.QueryString["woid"] != null)
                    {
                        try
                        {
                            WorkOrder.Id = Convert.ToInt32(Request.QueryString["woid"]);
                            aCont.Update_WorkOrder(WorkOrder);
                            aCont.MakeComment(PortalId, WorkOrder.Id, -1, UserId, UserInfo.DisplayName, "WorkOrder #" + WorkOrder.Id.ToString() + " updated.", CommentTypeEnum.SystemMessage);
                        }
                        catch { passedCheck = false; }
                    }
                    if (passedCheck)
                    {
                        //aCont.Delete_WorkOrderGroupStationByWOId(WorkOrder.Id);
                        //aCont.DeleteWOGroupLibItemsByWOId(WorkOrder.Id);
                        if (Request.QueryString["woid"] == null)
                        {
                            WorkOrder.CreatedById = UserId;
                            WorkOrder.DateCreated = DateTime.Now;
                            WorkOrder.Id = aCont.Add_WorkOrder(WorkOrder);
                            aCont.MakeComment(PortalId, WorkOrder.Id, -1, UserId, UserInfo.DisplayName, "WorkOrder #" + WorkOrder.Id.ToString() + " created.", CommentTypeEnum.SystemMessage);
                        }
                        foreach (WOGroupInfo group in Groups)
                        {
                            if (group.Id != -1)
                            {
                                aCont.DeleteWOGroupServicesByWOGroupId(group.Id);
                                aCont.DeleteWOGroupLibItemsByWOGroupId(group.Id);
                            }
                            group.WorkOrderId = WorkOrder.Id;
                            if (Request.QueryString["woid"] != null)
                            {
                                aCont.Update_WorkOrderGroup(group);
                            }
                            else
                            {
                                group.Id = aCont.Add_WorkOrderGroup(group);
                            }
                            //TODO: Account for change in number of libitems when editing
                            //if (Request.QueryString["woid"] == null)
                            //{

                            foreach (LibraryItemInfo libItem in group.LibraryItems)
                            {
                                try
                                {
                                    aCont.AddWOGroupLibItem(group.Id, libItem.Id);
                                }
                                catch { }
                            }
                            //}
                            foreach (WOGroupStationInfo station in group.WOGroupStations)
                            {
                                station.WorkOrderId = WorkOrder.Id;
                                station.WOGroupId = group.Id;
                                if (Request.QueryString["woid"] == null)
                                {
                                    station.Id = aCont.Add_WorkOrderGroupStation(station);
                                }
                                else
                                {
                                    if (station.Id == -1)
                                    {
                                        station.Id = aCont.Add_WorkOrderGroupStation(station);
                                    }
                                    else
                                    {
                                        aCont.Update_WorkOrderGroupStation(station);
                                    }
                                }
                            }
                            foreach (ServiceInfo service in group.Services)
                            {
                                try
                                {
                                    aCont.AddWOGroupService(group.Id, service.Id);
                                }
                                catch { }
                            }
                            //Add jobs
                            List<TaskInfo> ExistingTasks = aCont.Get_TasksByWOId(WorkOrder.Id);
                            List<TaskInfo> NewTasks = new List<TaskInfo>();
                            List<TaskInfo> TasksToAdd = new List<TaskInfo>();
                            List<TaskInfo> TasksToDelete = new List<TaskInfo>();
                            List<TaskInfo> TasksToUpdate = new List<TaskInfo>();
                            //if (Request.QueryString["woid"] == null)
                            //{
                                if (group.GroupType == GroupTypeEnum.Delivery || group.GroupType == GroupTypeEnum.Bundle)
                                {
                                    List<DeliveryMethodInfo> dels = aCont.Get_DeliveryMethodsByPortalId(PortalId);
                                    List<TapeFormatInfo> tapes = aCont.Get_TapeFormatsByPortalId(PortalId);
                                    List<CarrierTypeInfo> carrs = aCont.Get_CarrierTypesByPortalId(PortalId);
                                    foreach (WOGroupStationInfo stat in group.WOGroupStations)
                                    {
                                        foreach (LibraryItemInfo lib in group.LibraryItems)
                                        {
                                            TaskInfo task = new TaskInfo();
                                            task.CreatedById = UserId;
                                            task.DateCreated = DateTime.Now;
                                            if (stat.DeliveryMethod.IndexOf("dm_") != -1)
                                            {
                                                var del = dels.FirstOrDefault(i => i.Id.ToString() == stat.DeliveryMethod.Replace("dm_", ""));
                                                if (del != null)
                                                {
                                                    task.DeliveryMethod = del.DeliveryMethod;
                                                }
                                            }
                                            else if (stat.DeliveryMethod.IndexOf("tf_") != -1)
                                            {
                                                var tape = tapes.FirstOrDefault(i => i.Id.ToString() == stat.DeliveryMethod.Replace("tf_", ""));
                                                task.DeliveryMethod = tape.TapeFormat + ", ";
                                                task.Quantity = stat.Quantity;
                                                var carr = carrs.FirstOrDefault(i => i.Id == stat.ShippingMethodId);
                                                if(carr!=null)
                                                {
                                                    task.DeliveryMethod += carr.CarrierType + ", ";
                                                }
                                                else { task.DeliveryMethod += "PMT FedEx, "; }
                                                if(stat.PriorityId==1)
                                                { task.DeliveryMethod += "Priority"; }
                                                else if (stat.PriorityId==2)
                                                { task.DeliveryMethod += "Standard"; }
                                                else { task.DeliveryMethod += "2-Day"; }
                                                task.DeliveryMethod += ", Qty: " + task.Quantity.ToString();
                                            }
                                            task.DeliveryMethodId = Convert.ToInt32(stat.DeliveryMethod.Replace("dm_", "").Replace("tf_", ""));
                                            task.Description = group.GroupType.ToString() + " task: Station: " + stat.Station.CallLetter + ", ISCICode: " + lib.ISCICode + ", Delivery Method: " + task.DeliveryMethod + ", ";
                                            if (group.Services.Count() > 0)
                                            {
                                                task.Description += " Services: (";
                                                foreach (ServiceInfo serv in group.Services)
                                                {
                                                    task.Description += serv.ServiceName + ", ";
                                                }
                                                task.Description = task.Description.Substring(0, task.Description.Length - 2) + "), ";
                                            }
                                            task.Description += ", Group Comments: " + group.Comments;
                                            task.LastModifiedById = UserId;
                                            task.LastModifiedDate = DateTime.Now;
                                            task.LibraryId = lib.Id;
                                            task.PortalId = PortalId;
                                            task.StationId = stat.Id;
                                            task.TaskType = group.GroupType;
                                            task.WOGroupId = group.Id;
                                            task.WorkOrderId = WorkOrder.Id;
                                            NewTasks.Add(task);
                                            //task.Id = aCont.Add_Task(task);
                                            //aCont.MakeComment(PortalId, WorkOrder.Id, task.Id, UserId, UserInfo.DisplayName, task.TaskType.ToString() + " task created: " + task.Description, CommentTypeEnum.SystemMessage);
                                            //Log errors in QBcodes
                                            //task.QBCodes = aCont.FindQBCodesByTask(task.Id, PortalId);
                                            //if(task.QBCodes.Count()==0)
                                            //{
                                            //    aCont.MakeComment(PortalId, WorkOrder.Id, task.Id, UserId, UserInfo.DisplayName, "NOTE: Task# " + task.Id.ToString() + " returned no QuickBooks codes.", CommentTypeEnum.Error);
                                            //}
                                            //else if(task.QBCodes.Count()>1)
                                            //{
                                            //    aCont.MakeComment(PortalId, WorkOrder.Id, task.Id, UserId, UserInfo.DisplayName, "NOTE: Task# " + task.Id.ToString() + " returned more than 1 QuickBooks codes.  This may be an error.", CommentTypeEnum.Error);
                                            //}

                                            ////submit orders
                                            //if(task.DeliveryMethod == "Javelin")
                                            //{
                                            //    aCont.AddJavelinOrder(task.Id, PortalId);
                                            //}
                                        }
                                    }
                                }
                                if (group.GroupType == GroupTypeEnum.Customized)
                                {
                                    foreach (LibraryItemInfo lib in group.LibraryItems)
                                    {
                                        TaskInfo task = new TaskInfo();
                                        task.CreatedById = UserId;
                                        task.DateCreated = DateTime.Now;
                                        task.Description = group.GroupType.ToString() + " task: ISCICode: " + lib.ISCICode + ", Delivery Method: " + task.DeliveryMethod;
                                        if(group.Services.Count()>0)
                                        {
                                            task.Description += ", Services: (";
                                            foreach (ServiceInfo serv in group.Services)
                                            {
                                                task.Description += serv.ServiceName + ", ";
                                            }
                                            task.Description = task.Description.Substring(0, task.Description.Length - 2) + "), ";
                                        }
                                        task.Description += ", Group Comments: " + group.Comments;
                                        task.LastModifiedById = UserId;
                                        task.LastModifiedDate = DateTime.Now;
                                        task.LibraryId = lib.Id;
                                        task.PortalId = PortalId;
                                        task.TaskType = group.GroupType;
                                        task.WOGroupId = group.Id;
                                        task.WorkOrderId = WorkOrder.Id;
                                        NewTasks.Add(task);
                                        //task.Id = aCont.Add_Task(task);
                                        //aCont.MakeComment(PortalId, WorkOrder.Id, task.Id, UserId, UserInfo.DisplayName, task.TaskType.ToString() + " task created: " + task.Description, CommentTypeEnum.SystemMessage);
                                        //task.QBCodes = aCont.FindQBCodesByTask(task.Id, PortalId);
                                        ////Log qbcode errors
                                        //if (task.QBCodes.Count() == 0)
                                        //{
                                        //    aCont.MakeComment(PortalId, WorkOrder.Id, task.Id, UserId, UserInfo.DisplayName, "NOTE: Task# " + task.Id.ToString() + " returned no QuickBooks codes.", CommentTypeEnum.Error);
                                        //}
                                        //else if (task.QBCodes.Count() > 1)
                                        //{
                                        //    aCont.MakeComment(PortalId, WorkOrder.Id, task.Id, UserId, UserInfo.DisplayName, "NOTE: Task# " + task.Id.ToString() + " returned more than 1 QuickBooks codes.  This may be an error.", CommentTypeEnum.Error);
                                        //}
                                    }
                                }
                                if (group.GroupType == GroupTypeEnum.Non_Deliverable)
                                {
                                    TaskInfo task = new TaskInfo();
                                    task.CreatedById = UserId;
                                    task.DateCreated = DateTime.Now;
                                    task.Description = group.GroupType.ToString() + " task: PMT Media Id: " + group.Master.PMTMediaId + ", Services: (";
                                    foreach (ServiceInfo serv in group.Services)
                                    {
                                        task.Description += serv.ServiceName + ", ";
                                    }
                                    task.Description = task.Description.Substring(0, task.Description.Length - 2);
                                    task.Description += "), Group Comments: " + group.Comments;
                                    task.LastModifiedById = UserId;
                                    task.LastModifiedDate = DateTime.Now;
                                    task.MasterId = group.MasterId;
                                    task.PortalId = PortalId;
                                    task.TaskType = group.GroupType;
                                    task.WOGroupId = group.Id;
                                    task.WorkOrderId = WorkOrder.Id;
                                    NewTasks.Add(task);
                                    //task.Id = aCont.Add_Task(task);
                                    //aCont.MakeComment(PortalId, WorkOrder.Id, task.Id, UserId, UserInfo.DisplayName, task.TaskType.ToString() + " task created: " + task.Description, CommentTypeEnum.SystemMessage);
                                    //task.QBCodes = aCont.FindQBCodesByTask(task.Id, PortalId);
                                    ////log qbcodes error
                                    //if (task.QBCodes.Count() == 0)
                                    //{
                                    //    aCont.MakeComment(PortalId, WorkOrder.Id, task.Id, UserId, UserInfo.DisplayName, "NOTE: Task# " + task.Id.ToString() + " returned no QuickBooks codes.", CommentTypeEnum.Error);
                                    //}
                                }
                            //}
                            //check existingtasks against newtasks and process accordingly
                            foreach(TaskInfo existingTask in ExistingTasks)
                            {
                                if (existingTask.WOGroupId == group.Id)
                                {
                                    bool stillExists = false;
                                    bool hasChanged = false;
                                    TaskInfo changedTask = new TaskInfo();
                                    foreach (TaskInfo newTask in NewTasks)
                                    {
                                        if (aCont.areTasksEqual(newTask, existingTask))
                                        {
                                            stillExists = true;
                                        }
                                        else if (aCont.haveTasksChanged(newTask, existingTask))
                                        {
                                            hasChanged = true;
                                            changedTask = newTask;
                                            changedTask.Id = existingTask.Id;
                                        }
                                    }
                                    if (!stillExists && !hasChanged)
                                    {
                                        TasksToDelete.Add(existingTask);
                                    }
                                    else if (hasChanged)
                                    {
                                        TasksToUpdate.Add(changedTask);
                                    }
                                }
                            }
                            foreach(TaskInfo newTask in NewTasks)
                            {
                                if (newTask.WOGroupId == group.Id)
                                {
                                    bool isNew = true;
                                    foreach (TaskInfo existingTask in ExistingTasks)
                                    {
                                        if (aCont.areTasksEqual(newTask, existingTask))
                                        {
                                            isNew = false;
                                        }
                                        else if (aCont.haveTasksChanged(newTask, existingTask))
                                        {
                                            isNew = false;
                                        }
                                    }
                                    if (isNew)
                                    {
                                        TasksToAdd.Add(newTask);
                                    }
                                }
                            }
                            foreach(TaskInfo task in TasksToAdd)
                            {
                                if (task.WOGroupId == group.Id)
                                {
                                    task.Id = aCont.Add_Task(task);
                                    aCont.MakeComment(PortalId, WorkOrder.Id, task.Id, UserId, UserInfo.DisplayName, task.TaskType.ToString() + " task created: " + task.Description, CommentTypeEnum.SystemMessage);
                                    //if (task.DeliveryMethod.ToLower() == "addelivery")
                                    //{
                                    //    List<TaskInfo> tasks = new List<TaskInfo>();
                                    //    tasks.Add(task);
                                    //    aCont.createComcastOrder(tasks);
                                    //}
                                    //if (task.DeliveryMethod == "Javelin")
                                    //{
                                    //    aCont.AddJavelinOrder(task.Id, PortalId);
                                    //}
                                    //if (task.DeliveryMethod.ToLower() == "on the spot")
                                    //{
                                    //    aCont.deleteOTSMOrder(task.DeliveryOrderId);
                                    //}
                                    task.QBCodes = aCont.FindQBCodesByTask(task.Id, PortalId);
                                    ////log qbcodes error
                                    string msg = "";
                                    CommentTypeEnum commenttype = CommentTypeEnum.Error;
                                    if (task.QBCodes.Count() == 0)
                                    {
                                        msg = "NOTE: Task# " + task.Id.ToString() + " returned no QuickBooks codes.";
                                    }
                                    else if (task.QBCodes.Count() > 1 && task.TaskType != GroupTypeEnum.Non_Deliverable)
                                    {
                                        msg = "NOTE: Task# " + task.Id.ToString() + " returned more than 1 QuickBooks codes.  This may be an error.";
                                    }
                                    if (msg != "")
                                    {
                                        aCont.MakeComment(PortalId, WorkOrder.Id, task.Id, UserId, UserInfo.DisplayName, msg, commenttype);
                                    }
                                }
                            }
                            foreach (TaskInfo task in TasksToUpdate)
                            {
                                if (task.WOGroupId == group.Id)
                                {
                                    if (task.DeliveryMethod == "Javelin")
                                    {
                                        aCont.DeleteJavelinOrder(task.Id, PortalId);
                                        aCont.AddJavelinOrder(task.Id, PortalId);
                                    }
                                    if (task.DeliveryMethod.ToLower() == "addelivery")
                                    {
                                        aCont.deleteComcastOrder(task);
                                        List<TaskInfo> tasks = new List<TaskInfo>();
                                        tasks.Add(task);
                                        aCont.createComcastOrder(tasks);
                                    }
                                    if (task.DeliveryMethod.ToLower() == "on the spot")
                                    {
                                        aCont.deleteOTSMOrder(task.DeliveryOrderId);
                                        aCont.addOTSMOrder(task);
                                    }
                                    aCont.Update_Task(task);
                                    aCont.MakeComment(PortalId, WorkOrder.Id, task.Id, UserId, UserInfo.DisplayName, "Task# " + task.Id.ToString() + " Updated.", CommentTypeEnum.SystemMessage);
                                    task.QBCodes = aCont.FindQBCodesByTask(task.Id, PortalId);
                                    ////log qbcodes error
                                    string msg = "";
                                    CommentTypeEnum commenttype = CommentTypeEnum.Error;
                                    if (task.QBCodes.Count() == 0)
                                    {
                                        msg = "NOTE: Task# " + task.Id.ToString() + " returned no QuickBooks codes.";
                                    }
                                    else if (task.QBCodes.Count() > 1 && task.TaskType != GroupTypeEnum.Non_Deliverable)
                                    {
                                        msg = "NOTE: Task# " + task.Id.ToString() + " returned more than 1 QuickBooks codes.  This may be an error.";
                                    }
                                    if (msg != "")
                                    {
                                        aCont.MakeComment(PortalId, WorkOrder.Id, task.Id, UserId, UserInfo.DisplayName, msg, commenttype);
                                    }
                                }
                            }
                            foreach (TaskInfo task in TasksToDelete)
                            {
                                if (task.WOGroupId == group.Id)
                                {
                                    task.isDeleted = true;
                                    aCont.Update_Task(task);
                                    if (task.TaskType == GroupTypeEnum.Bundle || task.TaskType == GroupTypeEnum.Delivery)
                                    {
                                        WOGroupStationInfo gsi = new WOGroupStationInfo();
                                        gsi.Id = task.StationId;
                                        aCont.Delete_WorkOrderGroupStation(gsi);
                                    }
                                    if (task.DeliveryMethod.ToLower() == "addelivery")
                                    {
                                        aCont.deleteComcastOrder(task);
                                    }
                                    if (task.DeliveryMethod == "Javelin")
                                    {
                                        aCont.DeleteJavelinOrder(task.DeliveryMethodId, PortalId);
                                    }
                                    if(task.DeliveryMethod.ToLower()=="on the spot")
                                    {
                                        aCont.deleteOTSMOrder(task.DeliveryOrderId);
                                    }
                                    aCont.MakeComment(PortalId, WorkOrder.Id, task.Id, UserId, UserInfo.DisplayName, "Task# " + task.Id.ToString() + " marked as Cancelled.", CommentTypeEnum.SystemMessage);
                                }
                            }
                        }

                        if (Request.QueryString["woid"] != null)
                        {
                            aCont.MakeComment(PortalId, WorkOrder.Id, -1, UserId, UserInfo.DisplayName, "Workorder# " + WorkOrder.Id.ToString() + " updated.", CommentTypeEnum.SystemMessage);
                        }
                        lblWOMessage.Text = "Work Order saved successfully.";
                        Response.Redirect("/Work-Orders/woid/" + WorkOrder.Id.ToString());
                    }
                    else
                    {
                        lblWOMessage.Text = "Invalid Work Order Id";
                    }
                }
            }
        }

        protected void btnToggleBulk_Click(object sender, EventArgs e)
        {
            if(txtAddBulk.Visible)
            {
                txtAddBulk.Visible = false;
            }
            else
            {
                txtAddBulk.Visible = true;
            }
        }

        protected void ddlStationFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillStations();
        }
    }
}