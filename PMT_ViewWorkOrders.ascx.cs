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
    public partial class PMT_ViewWorkOrders : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            FillAdvertiserList();
            FillAgencyList();
            fillMasters();
            fillAssignUsers();
            drawWoidsInInvoice();
            //drawAddresses();
                //fillTasksFilter();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["selectedWO"] != null)
            {
                drawTasks(Convert.ToInt32(ViewState["selectedWO"]));
            }
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
                    }
                }
                else
                {
                    List<AdvertiserInfo> sortedList = Advertisers.Distinct(new AdvertiserComparer()).OrderBy(o => o.AdvertiserName).ToList();
                    foreach (AdvertiserInfo adv in sortedList)
                    {
                        ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
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
                        }
                    }
                    else
                    {
                        foreach (AdvertiserInfo adv in sortedList)
                        {
                            ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
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
                        }
                    }
                    else
                    {
                        foreach (AdvertiserInfo adv in selAds)
                        {
                            ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
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
            //AdminController aCont = new AdminController();
            //List<AdvertiserInfo> Advertisers = aCont.Get_AdvertisersByPortalId(PortalId);

            //ddlAdvertisers.Items.Clear();
            //ddlAdvertisers.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            //List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
            //bool isAdmin = UserInfo.IsInRole("Administrators");
            //foreach (AdvertiserInfo adv in Advertisers)
            //{
            //    ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
            //}
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
            List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            if (canSeeAll)
            {
                foreach (AdvertiserInfo adv in Advertisers)
                {
                    ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                }
            }
            else
            {
                foreach (AdvertiserInfo adv in userAds)
                {
                    ddlAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                }
            }
            FillAgencyList();
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
            List<AgencyInfo> agencies = aCont.Get_AgenciesByPortalId(PortalId);
            ddlAgencies.Items.Add(new ListItem("--Select Agency--", "-1"));
            List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
            bool isAd = UserInfo.IsInRole("Advertiser");
            bool isAg = UserInfo.IsInRole("Agency");
            //ddlMasterItemAgencySearch.Items.Clear();
            if (canSeeAll)
            {
                foreach (AgencyInfo ag in agencies)
                {

                    ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                    ddlAgencies.Items.Add(li);
                }
            }
            else if (isAd && !isAg)
            {
                List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
                List<AgencyInfo> tempAgs = new List<AgencyInfo>();
                if(ddlAdvertisers.SelectedIndex==0)
                {
                    foreach(AdvertiserInfo ad in userAds)
                    {
                        List<AgencyInfo> adAgs = aCont.Get_AgenciesByAdvertiserId(ad.Id);
                        foreach(AgencyInfo ag in adAgs)
                        {
                            tempAgs.Add(ag);
                        }
                    }
                }
                else
                {
                    tempAgs = aCont.Get_AgenciesByAdvertiserId(Convert.ToInt32(ddlAdvertisers.SelectedValue));
                }
                List<AgencyInfo> agsDD = tempAgs.Distinct().ToList();
                foreach (AgencyInfo userAg in agsDD)
                {
                    ListItem li = new ListItem(userAg.AgencyName, userAg.Id.ToString());
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
        }

        protected void ddlAdvertisers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["selAdvertiser"] = ddlAdvertisers.SelectedValue;
            FillAgencyList();
            fillMasters();
        }

        protected void ddlAgencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["selAgency"] = ddlAgencies.SelectedValue;
            FillAdvertiserList();
            fillMasters();
        }

        private void fillMasters()
        {
            AdminController aCont = new AdminController();
            int uc = getUserCase();
            bool canViewAll = false;
            if (uc == 0)
            {
                canViewAll = true;
            }
            //if (UserInfo.IsInRole("Administrators") || UserInfo.IsInRole("PMT Admin") || UserInfo.IsInRole("PMT ADMIN"))
            //{
            //    canViewAll = true;
            //}
            List<WorkOrderInfo> wos = aCont.Get_WorkOrdersByPortalId(PortalId);
            List<WorkOrderInfo> wosByMine = new List<WorkOrderInfo>();
            List<WorkOrderInfo> wosByAdvertiser = new List<WorkOrderInfo>();
            List<WorkOrderInfo> wosByAgency = new List<WorkOrderInfo>();
            List<WorkOrderInfo> wosByKeyword = new List<WorkOrderInfo>();
            List<WorkOrderInfo> wosByUser = new List<WorkOrderInfo>();
            List<AgencyInfo> agencies = aCont.Get_AgenciesByPortalId(PortalId);
            List<AdvertiserInfo> advertisers = aCont.Get_AdvertisersByPortalId(PortalId);
            if (chkShowMine.Checked)
            {
                foreach (WorkOrderInfo wo in wos)
                {
                    if (wo.AssignedTo == UserId)
                    {
                        wosByMine.Add(wo);
                    }
                }
            }
            else
            {
                wosByMine = wos;
            }
            if (ddlAdvertisers.SelectedIndex > 0)
            {
                foreach (WorkOrderInfo mast in wosByMine)
                {
                    if (mast.AdvertiserId.ToString() == ddlAdvertisers.SelectedValue)
                    {
                        wosByAdvertiser.Add(mast);
                    }
                }
            }
            else
            {
                wosByAdvertiser = wosByMine;
            }
            if (ddlAgencies.SelectedIndex > 0)
            {
                foreach (WorkOrderInfo wo in wosByAdvertiser)
                {
                    if (wo.AgencyId.ToString() == ddlAgencies.SelectedValue)
                    {
                        wosByAgency.Add(wo);
                    }
                }
            }
            else
            {
                wosByAgency = wosByAdvertiser;
            }
            if (txtKeyword.Text != "")
            {
                foreach (WorkOrderInfo wo in wosByAgency)
                {
                    if (wo.InvoiceNumber.ToString().ToLower().IndexOf(txtKeyword.Text.ToLower()) != -1 || wo.PONumber.ToString().ToLower().IndexOf(txtKeyword.Text.ToLower()) != -1 || wo.Description.ToString().ToLower().IndexOf(txtKeyword.Text.ToLower()) != -1 || wo.Id.ToString().ToLower().IndexOf(txtKeyword.Text.ToLower()) != -1)
                    {
                        wosByKeyword.Add(wo);
                    }
                }
            }
            else
            {
                wosByKeyword = wosByAgency;
            }
            foreach (WorkOrderInfo wo in wosByKeyword)
            {
                var agency = agencies.FirstOrDefault(i => i.Id == wo.AgencyId);
                var advertiser = advertisers.FirstOrDefault(i => i.Id == wo.AdvertiserId);
                if (agency != null)
                {
                    wo.AgencyName = agency.AgencyName;
                }
                else { wo.AgencyName = ""; }
                if (advertiser != null)
                {
                    wo.AdvertiserName = advertiser.AdvertiserName;
                }
                else { wo.AdvertiserName = ""; }
                if (wo.Status == "")
                {
                    wo.Status = "NEW";
                }
            }
            //filter by user
            if(!canViewAll)
            {
                List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
                List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
                bool isAd = UserInfo.IsInRole("Advertiser");
                bool isAg = UserInfo.IsInRole("Agency");
                foreach (WorkOrderInfo wo in wosByKeyword)
                {
                    foreach (AdvertiserInfo ad in userAds)
                    {
                        if (isAd && wo.AdvertiserId == ad.Id)
                        {
                            wosByUser.Add(wo);
                        }
                    }
                }
            }
            else
            {
                wosByUser = wosByKeyword;
            }
            gvMasters.DataSource = wosByUser;
            if (ViewState["MastersPage"] != null)
            {
                gvMasters.PageIndex = Convert.ToInt32(ViewState["MastersPage"]);
            }
            lblMessage.Text = wosByUser.Count.ToString() + " WORK ORDERS FOUND";
            gvMasters.DataBind();
        }

        protected void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            fillMasters();
        }

        protected void gvMasters_SelectedIndexChanged(object sender, EventArgs e)
        {
            string thisId = (gvMasters.SelectedRow.FindControl("hdngvMasterItemId") as HiddenField).Value;
            ViewState["selectedWO"] = thisId;
            int UserCase = getUserCase();
            if (UserCase != 0)
            {
                Response.Redirect("~/Work-Orders/woid/" + thisId);
            }
            else
            {
                pnlDetailView.Visible = true;
                AdminController aCont = new AdminController();
                int woid = Convert.ToInt32(thisId);
                WorkOrderInfo wo = aCont.Get_WorkOrderById(woid);
                List<TaskInfo> tasks = aCont.Get_TasksByWOId(woid);
                List<WOCommentInfo> comments = aCont.Get_WOCommentsByWOId(woid);
                lblDescription.Text = wo.Id.ToString() + ": " + wo.Description;
                AdvertiserInfo ad = aCont.Get_AdvertiserById(wo.AdvertiserId);
                AgencyInfo ag = aCont.Get_AgencyById(wo.AgencyId);
                lblAdvertiser.Text = ad.AdvertiserName;
                lblAgency.Text = ag.AgencyName;
                if (wo.Priority != "Normal" && wo.Priority != "High")
                {
                    ddlPriority.SelectedValue = "Normal";
                }
                else
                {
                    ddlPriority.SelectedValue = wo.Priority;
                }
                if(wo.Status!="NEW" && wo.Status!="PENDING" && wo.Status!="COMPLETE" && wo.Status!="CANCELLED" && wo.Status!="READY FOR REVIEW")
                {
                    ddlStatus.SelectedValue = "READY FOR REVIEW";
                }
                else
                {
                    ddlStatus.SelectedValue = wo.Status;
                }
                if(ddlStatus.SelectedValue=="INVOICED" || ddlStatus.SelectedValue=="CANCELLED")
                {
                    ddlStatus.Enabled = false;
                    btnUpdateWorkOrder.Visible = false;
                }
                else
                {
                    ddlStatus.Enabled = true;
                    btnUpdateWorkOrder.Visible = true;
                }
                try
                {
                    ddlUsers.SelectedValue = wo.AssignedTo.ToString();
                }
                catch { }
                drawComments(woid);
                drawTasks(woid);
                fillMasters();
                //drawAddresses();
            }
        }

        protected void gvMasters_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["MastersPage"] = e.NewPageIndex.ToString();
            fillMasters();
            //gvMasters.PageIndex = e.NewPageIndex;
            //gvMasters.DataBind();
        }
        private void fillAssignUsers()
        {
            RoleController rCont = new RoleController();
            UserController uCont = new UserController();
            ArrayList users = DotNetNuke.Entities.Users.UserController.GetUsers(PortalId);            
            IList<RoleInfo> roles = rCont.GetRoles(PortalId);
            bool isAdmin = false;
            if (UserInfo.IsInRole("Administators"))
            { isAdmin = true; }
            string allViewRoles = getSetting("AllViewRoles", "");
            string[] allRoles = allViewRoles.Split(',');
            ddlUsers.Items.Clear();
            ddlUsers.Items.Add(new ListItem("--Select User--", ""));
            ddlUserFilter.Items.Add(new ListItem("--Select User--", ""));
            foreach (object user in users)
            {
                UserInfo thisUser = (UserInfo)user;
                foreach (string ar in allRoles)
                {
                    var role = roles.FirstOrDefault(i => i.RoleID.ToString() == ar);
                    if(thisUser.IsInRole(role.RoleName))
                    {
                        ListItem li = new ListItem(thisUser.DisplayName, thisUser.UserID.ToString());
                        ddlUsers.Items.Add(li);
                        ddlUserFilter.Items.Add(new ListItem(thisUser.DisplayName, thisUser.UserID.ToString()));
                        break;
                    }
                }                
            }            
        }
        private void drawComments(int woid)
        {
            AdminController aCont = new AdminController();
            List<WOCommentInfo> comments = aCont.Get_WOCommentsByWOId(woid);
            List<WOCommentInfo> commentsByType = new List<WOCommentInfo>();
            //List<WOCommentInfo> commentsByTask = new List<WOCommentInfo>();
            List<WOCommentInfo> commentsByUser = new List<WOCommentInfo>();
            if(ddlCommentTypes.SelectedIndex>0)
            {
                foreach(WOCommentInfo comment in comments)
                {
                    if(comment.CommentType.ToString()==ddlCommentTypes.SelectedValue)
                    {
                        commentsByType.Add(comment);
                    }
                }
            }
            else
            {
                commentsByType = comments;
            }
            //if(ddlTasks.SelectedIndex>0)
            //{
            //    foreach (WOCommentInfo comment in commentsByType)
            //    {
            //        if (comment.WOTaskId.ToString() == ddlTasks.SelectedValue)
            //        {
            //            commentsByTask.Add(comment);
            //        }
            //    }
            //}
            //else
            //{
            //    commentsByTask = commentsByType;
            //}
            if(ddlUserFilter.SelectedIndex>0)
            {
                foreach (WOCommentInfo comment in commentsByType)
                {
                    if (comment.CreatedById.ToString() == ddlUserFilter.SelectedValue)
                    {
                        commentsByUser.Add(comment);
                    }
                }
            }
            else
            {
                commentsByUser = commentsByType;
            }
            litComments.Text = "";
            litComments.Text += "<div class=\"rowRow\"><div class=\"table-header1\">DATE</div>";
            litComments.Text += "<div class=\"table-header2\">USER</div>";
            litComments.Text += "<div class=\"table-header3\">COMMENT</div></div>";
            foreach(WOCommentInfo comment in commentsByUser)
            {
                litComments.Text += "<div class=\"rowRow ";
                litComments.Text += comment.CommentType.ToString(); // Enum.GetName(typeof(CommentTypeEnum), comment.CommentType);
                litComments.Text += " \"><div class=\"cell1\">" + comment.DateCreated.ToLongDateString() + " " + comment.DateCreated.ToShortTimeString() + "</div>";
                litComments.Text += "<div class=\"cell2\">" + comment.DisplayName + "</div>";
                litComments.Text += "<div class=\"cell3\">" + comment.Comment.Replace("\"stations\"", "\"stations\" ") + "</div></div>";
            }
            litComments.Text += "<br clear=\"both\" />";
        }        
        private void drawTasks(int woid)
        {
            plTasks.Controls.Clear();
            plPrintShippingLabels.Controls.Clear();
            AdminController aCont = new AdminController();
            List<TaskInfo> tasks = aCont.Get_TasksByWOId(woid);
            bool hasShipping = false;

            foreach(TaskInfo task in tasks)
            {
                LibraryItemInfo lib = aCont.Get_LibraryItemById(task.LibraryId);
                MasterItemInfo master = aCont.Get_MasterItemById(task.MasterId);
                Literal lit1 = new Literal();
                lit1.ID = "lit1_" + task.Id.ToString();
                lit1.Text = "<div class=\"rowRow\"><div class=\"cell5\">" + task.Id.ToString() + "</div><div class=\"cell10\">";
                if (task.TaskType == GroupTypeEnum.Delivery || task.TaskType == GroupTypeEnum.Bundle || task.TaskType == GroupTypeEnum.Customized)
                {
                    lit1.Text += lib.PMTMediaId;
                }
                else if(task.TaskType == GroupTypeEnum.Non_Deliverable)
                {
                    lit1.Text += master.PMTMediaId;
                }
                lit1.Text +="</div><div class=\"cell10\">" + task.TaskType.ToString() + "</div><div class=\"cell10\">";
                if (task.TaskType == GroupTypeEnum.Delivery || task.TaskType == GroupTypeEnum.Bundle || task.TaskType == GroupTypeEnum.Customized)
                {
                    lit1.Text += lib.ISCICode;
                }
                lit1.Text += "</div><div class=\"cell10\">";
                if (task.TaskType == GroupTypeEnum.Delivery || task.TaskType == GroupTypeEnum.Bundle || task.TaskType == GroupTypeEnum.Customized)
                {
                    lit1.Text += lib.Title;
                }
                else if (task.TaskType == GroupTypeEnum.Non_Deliverable)
                {
                    lit1.Text += master.Title;
                }
                lit1.Text += "</div><div class=\"cell10\">";
                if (task.TaskType == GroupTypeEnum.Delivery || task.TaskType == GroupTypeEnum.Bundle || task.TaskType == GroupTypeEnum.Customized)
                {
                    lit1.Text += lib.ProductDescription + "<br />";
                }
                if (task.TaskType == GroupTypeEnum.Non_Deliverable || task.TaskType == GroupTypeEnum.Customized || task.TaskType == GroupTypeEnum.Bundle)
                {
                    lit1.Text += task.Description;
                }
                lit1.Text += "</div><div class=\"cell10\">";
                if (task.TaskType == GroupTypeEnum.Delivery || task.TaskType == GroupTypeEnum.Bundle)
                {
                    WOGroupStationInfo wostat = aCont.Get_WorkOrderGroupStationById(task.StationId);
                    StationInfo stat = aCont.Get_StationById(wostat.StationId);
                    lit1.Text += stat.CallLetter;
                }
                lit1.Text += "</div><div class=\"cell10\">" + task.DeliveryMethod + "</div>";

                if (task.DeliveryMethod == "Javelin" || task.DeliveryMethod.ToLower() == "addelivery" || task.DeliveryMethod.ToLower().Trim() == "on the spot")
                {
                    lit1.Text += "<div class=\"cell10\">" + task.DeliveryOrderId.ToString() + "</div>";
                    lit1.Text += "<div class=\"cell10\">" + task.DeliveryStatus;
                    plTasks.Controls.Add(lit1);
                    Button btnGetAPIStatus = new Button();
                    btnGetAPIStatus.ID = "btnGetAPIStatus_" + task.Id.ToString();
                    btnGetAPIStatus.CssClass = "btn btn-blue";
                    btnGetAPIStatus.Text = "Process Task";
                    btnGetAPIStatus.Click += new EventHandler(btnGetAPIStatus_Click);
                    plTasks.Controls.Add(btnGetAPIStatus);

                    Literal lit1b = new Literal();
                    lit1b.ID = "lit1b_" + task.Id.ToString();
                    lit1b.Text = "</div><div class=\"cell10\">";
                    if (!task.isComplete && !task.isDeleted)
                    {
                        lit1b.Text += "OPEN";
                    }
                    else if(task.isComplete && !task.isDeleted)
                    {
                        lit1b.Text += "COMPLETE";
                    }
                    else if (task.isDeleted)
                    {
                        lit1b.Text += "CANCELLED";
                    }
                    lit1b.Text += "</div><div class=\"cell10\">";
                    //int count = task.QBCodes.Count();
                    //int c = 0;
                    //foreach(QBCodeInfo code in task.QBCodes)
                    //{
                    //    lit1.Text += code.QBCode;
                    //    c++;
                    //    if(c<count)
                    //    {
                    //        lit1.Text += ", ";
                    //    }
                    //}
                    plTasks.Controls.Add(lit1b);
                }
                else
                {
                    lit1.Text += "<div class=\"cell10\">";
                    plTasks.Controls.Add(lit1);
                    if (task.TaskType != GroupTypeEnum.Non_Deliverable && task.TaskType != GroupTypeEnum.Customized)
                    {
                        TextBox txt = new TextBox();
                        txt.ID = "txtDeliveryId_" + task.Id.ToString();
                        txt.CssClass = "taskInput";
                        txt.Attributes.Add("placeholder", "Delivery Id");
                        //if (task.DeliveryOrderId != -1)
                        //{
                            txt.Text = task.DeliveryOrderId.ToString();
                        //}
                        plTasks.Controls.Add(txt);
                    }
                    Literal lit1a = new Literal();
                    lit1a.ID = "lit1a_" + task.Id.ToString();
                    lit1a.Text = "</div><div class=\"cell10\">";
                    plTasks.Controls.Add(lit1a);
                    if (task.TaskType == GroupTypeEnum.Delivery || task.TaskType == GroupTypeEnum.Bundle)
                    {
                        //if (task.DeliveryStatus.ToLower() != "closed manually")
                        //{
                            DropDownList ddl = new DropDownList();
                            ddl.ID = "ddlDeliveryStatus_" + task.Id.ToString();
                            ddl.Items.Add(new ListItem("--Select Status--", ""));
                            ListItem liPending = new ListItem("PENDING", "PENDING");
                            if (task.DeliveryStatus == "PENDING")
                                liPending.Selected = true;
                            ddl.Items.Add(liPending);
                            ListItem liComplete = new ListItem("COMPLETE", "COMPLETE");
                            if (task.DeliveryStatus == "COMPLETE" || task.DeliveryStatus.ToLower() == "closed manually")
                                liComplete.Selected = true;
                            ddl.Items.Add(liComplete);
                            ListItem liCancelled = new ListItem("CANCELLED", "CANCELLED");
                            if (task.DeliveryStatus == "CANCELLED")
                                liCancelled.Selected = true;
                            ddl.Items.Add(liCancelled);
                            plTasks.Controls.Add(ddl);
                        //}
                        //else
                        //{
                        //    Literal litDs = new Literal();
                        //    litDs.Text = "Closed Manually";
                        //    plTasks.Controls.Add(litDs);
                        //}
                    }
                    Literal lit1b = new Literal();
                    lit1b.ID = "lit1b_" + task.Id.ToString();
                    lit1b.Text = "</div><div class=\"cell10\">";
                    plTasks.Controls.Add(lit1b);
                    DropDownList ddlTaskStatus = new DropDownList();
                    ddlTaskStatus.ID = "ddlTaskStatus_" + task.Id.ToString();
                    if (!task.isDeleted)
                    {
                        ddlTaskStatus.Items.Add(new ListItem("--Select Status--", ""));
                        ListItem liOPEN = new ListItem("OPEN", "OPEN");
                        if (!task.isComplete && !task.isDeleted)
                            liOPEN.Selected = true;
                        ddlTaskStatus.Items.Add(liOPEN);
                        ListItem li2Complete = new ListItem("COMPLETE", "COMPLETE");
                        if (task.isComplete && !task.isDeleted)
                            li2Complete.Selected = true;
                        ddlTaskStatus.Items.Add(li2Complete);
                    }
                    else
                    {
                        ListItem li2Cancelled = new ListItem("CANCELLED", "CANCELLED");
                        if (task.isDeleted)
                            li2Cancelled.Selected = true;
                        ddlTaskStatus.Items.Add(li2Cancelled);
                    }
                    plTasks.Controls.Add(ddlTaskStatus);
                    WOGroupStationInfo stat = aCont.Get_WorkOrderGroupStationById(task.StationId);
                    if (stat.DeliveryMethod.IndexOf("tf_") != -1)
                    {
                        Button btnMakeLabel = new Button();
                        btnMakeLabel.ID = "btnMakeLabel_" + task.Id.ToString();
                        btnMakeLabel.CssClass = "btn btn-blue";
                        btnMakeLabel.Text = "Make Label";
                        btnMakeLabel.Click += new EventHandler(btnMakeLabel_Click);
                        plTasks.Controls.Add(btnMakeLabel);
                    }
                    Literal lit1c = new Literal();
                    lit1c.ID = "lit1c_" + task.Id.ToString();
                    lit1c.Text = "</div><div class=\"cell10\">";
                    plTasks.Controls.Add(lit1c);
                }
                if (task.QBCodes.Count() > 0)
                {
                    if (task.TaskType != GroupTypeEnum.Non_Deliverable && task.TaskType != GroupTypeEnum.Customized)
                    {
                        DropDownList ddlQBCodes = new DropDownList();
                        ddlQBCodes.ID = "ddlQBCodes_" + task.Id.ToString();
                        ddlQBCodes.Items.Add(new ListItem("--Select Code--", ""));
                        foreach (QBCodeInfo code in task.QBCodes)
                        {
                            ListItem li = new ListItem(code.QBCode, code.Id.ToString());
                            if (task.QBCodeId == code.Id)
                            {
                                li.Selected = true;
                            }
                            ddlQBCodes.Items.Add(li);
                        }
                        if(ddlQBCodes.Items.Count==2)
                        {
                            ddlQBCodes.Items[1].Selected = true;
                        }
                        plTasks.Controls.Add(ddlQBCodes);
                    }
                    else
                    {
                        Literal lit1d = new Literal();
                        lit1d.ID = "lit1d_" + task.Id.ToString();
                        int count = task.QBCodes.Count();
                        int c = 0;
                        foreach (QBCodeInfo code in task.QBCodes)
                        {
                            lit1d.Text += code.QBCode;
                            c++;
                            if (c < count)
                            {
                                lit1d.Text += ", ";
                            }
                        }
                        plTasks.Controls.Add(lit1d);
                    }
                }
                Button btnRefreshCodes = new Button();
                btnRefreshCodes.ID = "btnRefreshCodes_" + task.Id.ToString();
                btnRefreshCodes.CssClass = "btn btn-blue";
                btnRefreshCodes.Text = "Refresh QB Codes";
                btnRefreshCodes.Click += new EventHandler(btnRefreshCodes_Click);
                plTasks.Controls.Add(btnRefreshCodes);
                Literal lit2 = new Literal();
                lit2.ID = "lit2_" + task.Id.ToString();                
                lit2.Text += "</div></div>";
                plTasks.Controls.Add(lit2);
                if((task.TaskType == GroupTypeEnum.Bundle || task.TaskType == GroupTypeEnum.Delivery) && task.Quantity > 0)
                {
                    //draw fedex label
                    hasShipping = true;
                    Image img = new Image();
                    try
                    {
                        img.ImageUrl = task.DeliveryMethodResponse.Substring(0, task.DeliveryMethodResponse.IndexOf("|"));
                        img.CssClass = "imagePage";
                        img.ID = "img_" + task.Id.ToString();
                        plPrintShippingLabels.Controls.Add(img);
                    }
                    catch { }                    
                }
            }
            if(hasShipping)
            {
                btnPrint.Visible = true;
            }
        }
        protected void btnMakeLabel_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            Button btn = (Button)sender;
            int taskId = Convert.ToInt32(btn.ID.Replace("btnMakeLabel_", ""));
            TaskInfo task = aCont.Get_TaskById(taskId);
            Response.Redirect("/Labels/tid/" + task.Id.ToString(), "_blank", "");
        }

        protected void btnGetAPIStatus_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            Button btn = (Button)sender;
            int taskId = Convert.ToInt32(btn.ID.Replace("btnGetAPIStatus_", ""));
            TaskInfo task = aCont.Get_TaskById(taskId);
            if((task.TaskType == GroupTypeEnum.Bundle || task.TaskType == GroupTypeEnum.Delivery))
            {
                if (task.DeliveryOrderId == "" && task.DeliveryStatus != "COMPLETE" && task.DeliveryStatus.ToLower() != "closed manually" && !task.isComplete && !task.isDeleted)
                {
                    if (task.DeliveryMethod.ToLower() == "javelin")
                    {
                        aCont.AddJavelinOrder(task.Id, PortalId);
                    }
                    else if (task.DeliveryMethod.ToLower() == "addelivery")
                    {
                        List<TaskInfo> tempTasks = new List<TaskInfo>();
                        tempTasks.Add(task);
                        aCont.createComcastOrder(tempTasks, "", PortalId);
                    }
                    else if (task.DeliveryMethod.ToLower() == "on the spot")
                    {
                        aCont.addOTSMOrder(task);
                    }
                }
                else if (task.DeliveryStatus != "COMPLETE" && !task.isComplete && !task.isDeleted && task.DeliveryStatus.ToLower() != "closed manually")
                {
                    if (task.DeliveryMethod.ToLower() == "javelin")
                    {
                        aCont.GetJavelinOrderStatus(task.Id);
                    }
                    else if (task.DeliveryMethod.ToLower() == "addelivery")
                    {
                        aCont.getComcastOrderStatus(task);
                    }
                    else if (task.DeliveryMethod.ToLower() == "on the spot")
                    {
                        aCont.getOTSMOrderStatus(task);
                    }
                }
            }
            drawTasks(task.WorkOrderId);
            drawComments(task.WorkOrderId);
        }
        protected void btnRefreshCodes_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            Button btn = (Button)sender;
            int taskId = Convert.ToInt32(btn.ID.Replace("btnRefreshCodes_", ""));
            List<QBCodeInfo> codes = aCont.FindQBCodesByTask(taskId, PortalId);
            TaskInfo task = aCont.Get_TaskById(taskId);
            if(task.TaskType == GroupTypeEnum.Bundle || task.TaskType == GroupTypeEnum.Delivery)
            {
                if (codes.Count() == 1)
                {
                    task.QBCodeId = codes[0].Id;
                    task.QBCode = codes[0].QBCode;
                    task.LastModifiedById = UserId;
                    task.LastModifiedDate = DateTime.Now;
                    aCont.Update_Task(task);
                }
                else
                {
                    aCont.MakeComment(PortalId, task.WorkOrderId, task.Id, UserId, UserInfo.DisplayName, "NOTE: Task# " + task.Id.ToString() + " returned " + codes.Count().ToString() + " QuickBooks codes.  This may be an error.", CommentTypeEnum.Error);
                }
            }
            drawTasks(task.WorkOrderId);
            drawComments(task.WorkOrderId);
        }

        protected void btnComment_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            string thisId = (gvMasters.SelectedRow.FindControl("hdngvMasterItemId") as HiddenField).Value;
            WOCommentInfo comment = new WOCommentInfo();
            comment.PortalId = PortalId;
            comment.WorkOrderId = Convert.ToInt32(thisId);
            //comment.WOTaskId = task.Id;
            comment.CreatedById = UserId;
            comment.DateCreated = DateTime.Now;
            comment.DisplayName = UserInfo.DisplayName;
            comment.Comment = txtComment.Text;
            comment.LastModifiedById = UserId;
            aCont.Add_WOComment(comment);
            drawComments(comment.WorkOrderId);
        }
        private void fillTasksFilter()
        {
            AdminController aCont = new AdminController();
            if(ViewState["selectedWO"]!=null)
            {
                ddlTasks.Items.Clear();
                ddlTasks.Items.Add(new ListItem("--Select Task--", ""));
                List<TaskInfo> tasks = aCont.Get_TasksByWOId(Convert.ToInt32(ViewState["selectedWO"]));
                foreach(TaskInfo task in tasks)
                {
                    ddlTasks.Items.Add(new ListItem(task.Id.ToString(), task.Id.ToString()));
                }
            }
        }
        protected void btnUpdateWorkOrder_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            string changes = "";
            if (ViewState["selectedWO"] != null)
            {
                WorkOrderInfo workOrder = aCont.Get_WorkOrderById(Convert.ToInt32(ViewState["selectedWO"]));
                List<TaskInfo> tasks = aCont.Get_TasksByWOId(workOrder.Id);
                foreach (TaskInfo task in tasks)
                {
                    bool updated = false;
                    if (task.DeliveryMethod != "Javelin" && task.DeliveryMethod.ToLower() != "addelivery" && task.DeliveryMethod.ToLower().Trim() != "on the spot")
                    {
                        TextBox txt = (TextBox)plTasks.FindControl("txtDeliveryId_" + task.Id.ToString());
                        if (txt != null)
                        {
                            task.DeliveryOrderId = txt.Text;
                        }
                        DropDownList ddl = (DropDownList)plTasks.FindControl("ddlDeliveryStatus_" + task.Id.ToString());
                        if (ddl != null)
                        {
                            if (ddl.SelectedValue != task.DeliveryStatus)
                            {
                                updated = true;
                                changes += "Task #" + task.Id.ToString() + " delivery status changed to " + ddl.SelectedValue + ". ";
                            }
                            if (ddl.SelectedIndex == 0)
                            {
                                task.DeliveryStatus = "";
                            }
                            else
                            {
                                task.DeliveryStatus = ddl.SelectedItem.Text;
                            }
                        }
                        DropDownList ddlTaskStatus = (DropDownList)plTasks.FindControl("ddlTaskStatus_" + task.Id.ToString());
                        if (ddlTaskStatus.SelectedItem.Text == "OPEN")
                        {
                            if (task.isComplete || task.isDeleted)
                            {
                                updated = true;
                                changes += "Task #" + task.Id.ToString() + " status changed to " + ddlTaskStatus.SelectedItem.Text + ". ";
                            }
                            task.isComplete = false;
                            task.isDeleted = false;
                        }
                        else if (ddlTaskStatus.SelectedItem.Text == "COMPLETE")
                        {
                            if (!task.isComplete)
                            {
                                if (!task.isDeleted)
                                {
                                    if (task.DeliveryStatus.ToLower() != "cancelled")
                                    {
                                        updated = true;
                                        task.DeliveryOrderDateComplete = DateTime.Now;
                                        changes += "Task #" + task.Id.ToString() + " status changed to " + ddlTaskStatus.SelectedItem.Text + ". ";
                                    }
                                }
                            }
                            task.isComplete = true;
                            task.isDeleted = false;
                        }
                        else if (ddlTaskStatus.SelectedItem.Text == "CANCELLED")
                        {
                            if (!task.isDeleted && task.DeliveryStatus.ToLower() != "cancelled")
                            {
                                updated = true;
                                changes += "Task #" + task.Id.ToString() + " status changed to " + ddlTaskStatus.SelectedItem.Text + ". ";
                                if (task.DeliveryMethod.ToLower() == "addelivery")
                                {
                                    aCont.deleteComcastOrder(task);
                                }
                                if (task.DeliveryMethod.ToLower() == "javelin")
                                {
                                    aCont.DeleteJavelinOrder(task.Id, PortalId);
                                }
                            }
                            task.isDeleted = true;
                        }
                    }
                    if (task.QBCodes.Count() > 0)
                    {
                        DropDownList ddlQBCodes = (DropDownList)plTasks.FindControl("ddlQBCodes_" + task.Id.ToString());
                        if (ddlQBCodes != null && ddlQBCodes.SelectedValue != task.QBCodeId.ToString())
                        {
                            task.QBCode = ddlQBCodes.SelectedItem.Text;
                            try
                            {
                                task.QBCodeId = Convert.ToInt32(ddlQBCodes.SelectedValue);
                            }
                            catch { }
                            updated = true;
                            changes += "Task #" + task.Id.ToString() + " QBCode changed to " + ddlQBCodes.SelectedItem.Text + ". ";
                        }
                        else if (ddlQBCodes != null && (task.QBCodeId == -1 || task.QBCode == ""))
                        {
                            task.QBCode = ddlQBCodes.SelectedItem.Text;
                            task.QBCodeId = Convert.ToInt32(ddlQBCodes.SelectedValue);
                            updated = true;
                            changes += "Task #" + task.Id.ToString() + " QBCode saved as " + ddlQBCodes.SelectedItem.Text + ". ";
                        }
                    }
                    task.LastModifiedById = UserId;
                    task.LastModifiedDate = DateTime.Now;
                    if (updated)
                    {
                        aCont.Update_Task(task);
                        aCont.MakeComment(PortalId, workOrder.Id, task.Id, UserId, UserInfo.DisplayName, "Task: " + task.Id.ToString() + " Updated. " + changes, CommentTypeEnum.SystemMessage);
                    }

                }
                bool woUpdated = false;
                changes = "";
                if (ddlUsers.SelectedIndex > 0)
                {
                    if (ddlUsers.SelectedValue != workOrder.AssignedTo.ToString())
                    { 
                        woUpdated = true;
                        changes += "Work Order #" + workOrder.Id.ToString() + " assigned to " + ddlUsers.SelectedItem.Text + ". ";
                    }
                    workOrder.AssignedTo = Convert.ToInt32(ddlUsers.SelectedValue);
                }
                bool allClosed = true;
                if (ddlStatus.SelectedValue.ToLower() == "complete")
                {
                    tasks = aCont.Get_TasksByWOId(workOrder.Id);
                    //changing behavior to close tasks when WO is closed
                    foreach(TaskInfo task in tasks)
                    {
                        if (task.DeliveryStatus.ToLower() != "complete" && task.DeliveryStatus.ToLower() != "cancelled" && task.DeliveryStatus.ToLower() != "closed manually")
                        {
                            task.DeliveryStatus = "Closed Manually";
                            task.DeliveryOrderDateComplete = DateTime.Now;
                            task.isComplete = true;
                            task.LastModifiedById = UserId;
                            task.LastModifiedDate = DateTime.Now;
                            aCont.Update_Task(task);
                            changes += "Task Id: " + task.Id.ToString() + " closed manually. ";
                        }
                    }
                    //Code below was used to not allow WO to be closed when there are open tasks
                    //int thisId = -1;
                    //foreach (TaskInfo task in tasks)
                    //{
                    //    if (task.TaskType != GroupTypeEnum.Non_Deliverable)
                    //    {
                    //        if (task.DeliveryStatus != "COMPLETE" && task.DeliveryStatus != "CANCELLED")
                    //        {
                    //            allClosed = false;
                    //            thisId = task.Id;
                    //            break;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if(!task.isComplete)
                    //        {
                    //            allClosed = false;
                    //            thisId = task.Id;
                    //            break;
                    //        }
                    //    }
                    //}
                    //if (!allClosed)
                    //{
                    //    ddlStatus.SelectedValue = "PENDING";
                    //    aCont.MakeComment(PortalId, workOrder.Id, thisId, UserId, UserInfo.DisplayName, "Could not mark WorkOrder Complete due to open tasks." , CommentTypeEnum.Error);
                    //}
                    //else
                    //{
                    //    if (ddlStatus.SelectedValue != workOrder.Status)
                    //    { woUpdated = true; }
                    //    workOrder.Status = ddlStatus.SelectedValue;
                    //}
                }
                //else
                //{
                    if (ddlStatus.SelectedValue != workOrder.Status)
                    {
                        woUpdated = true;
                        changes += "Work Order #" + workOrder.Id.ToString() + " status changed to " + ddlStatus.SelectedValue + ". ";
                    }
                    workOrder.Status = ddlStatus.SelectedValue;
                //}
                if (ddlPriority.SelectedValue != workOrder.Priority)
                {
                    woUpdated = true;
                    changes += "Work Order #" + workOrder.Id.ToString() + " priority changed to " + ddlPriority.SelectedValue + ". ";
                }
                workOrder.Priority = ddlPriority.SelectedItem.Text;
                workOrder.LastModifiedById = UserId;
                workOrder.LastModifiedDate = DateTime.Now;
                if (woUpdated)
                {
                    aCont.Update_WorkOrder(workOrder);
                    aCont.MakeComment(PortalId, workOrder.Id, -1, UserId, UserInfo.DisplayName, "WorkOrder " + workOrder.Id.ToString() + " Updated. " + changes, CommentTypeEnum.SystemMessage);
                }
                lblMessage.Text = "Work Order Updated.";
                drawComments(workOrder.Id);
            }
            fillMasters();
        }

        protected void ddlCommentTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["selectedWO"] != null)
            {
                drawComments(Convert.ToInt32(ViewState["selectedWO"]));
            }
        }

        protected void ddlTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["selectedWO"] != null)
            {
                drawComments(Convert.ToInt32(ViewState["selectedWO"]));
            }
        }

        protected void ddlUserFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["selectedWO"] != null)
            {
                drawComments(Convert.ToInt32(ViewState["selectedWO"]));
            }
        }

        protected void btnViewWorkOrder_Click(object sender, EventArgs e)
        {
            if (ViewState["selectedWO"] != null)
            {
                Response.Redirect("~/Work-Orders/woid/" + ViewState["selectedWO"].ToString());
            }
            else
            {
                lblMessage.Text = "No WorkOrder Selected.";
            }
        }

        protected void chkShowMine_CheckedChanged(object sender, EventArgs e)
        {
            fillMasters();
        }

        protected void btnCreateLabels_Click(object sender, EventArgs e)
        {
            if(ViewState["selectedWO"] != null)
            {
                AdminController aCont = new AdminController();
                List<TapeFormatInfo> tapes = aCont.Get_TapeFormatsByPortalId(PortalId);
                WorkOrderInfo wo = aCont.Get_WorkOrderById(Convert.ToInt32(ViewState["selectedWO"]));
                List<LibraryItemInfo> libs = aCont.getLibs(PortalId);
                List<TaskInfo> tasks = aCont.Get_TasksByWOId(wo.Id);
                int c = 0;
                foreach(TaskInfo task in tasks)
                {
                    foreach(TapeFormatInfo tape in tapes)
                    {
                        if(task.DeliveryMethod.ToLower().IndexOf(tape.TapeFormat.ToLower())!=-1)
                        {
                            LabelInfo label = new LabelInfo();
                            label.PortalId = PortalId;
                            label.AdvertiserId = wo.AdvertiserId;
                            label.AdvertiserName = wo.AdvertiserName;
                            label.AgencyId = wo.AgencyId;
                            label.AgencyName = wo.AgencyName;
                            label.CampaignId = wo.Id;
                            label.CampaignMediaId = task.LibraryId;
                            label.CreatedById = UserId;
                            var lib = libs.FirstOrDefault(o => o.Id == task.LibraryId);
                            if (lib != null)
                            {
                                label.Description = lib.ProductDescription;
                                label.ISCICode = lib.ISCICode;
                                label.MediaLength = lib.MediaLength;
                                label.PMTMediaId = lib.PMTMediaId;
                                label.Standard = lib.Standard;
                                label.Title = lib.Title;
                            }
                            label.TapeFormat = tape.Id;
                            label.LabelNumber = aCont.getMaxLabelNumber(PortalId) + 1;
                            aCont.Add_Label(label);
                            c++;
                        }
                    }
                }
                lblMessage.Text = c.ToString() + " Labels created.";
            }
        }

        //private void drawAddresses()
        //{
        //    if (ViewState["selectedWO"] != null)
        //    {
        //        AdminController aCont = new AdminController();
        //        WorkOrderInfo wo = aCont.Get_WorkOrderById(Convert.ToInt32(ViewState["selectedWO"]));
        //        bool hasShipping = false;
        //        if (ViewState["popupFirst"] == null)
        //        { ViewState["popupFirst"] = "false"; }
        //        foreach (WOGroupInfo group in wo.Groups)
        //        {
        //            Literal litGroupTitle = new Literal();
        //            litGroupTitle.ID = "litGroupTitle_" + group.Id.ToString();
        //            litGroupTitle.Text = "<h3>" + group.GroupName + "</h3>";
        //            pnlAddresses.Controls.Add(litGroupTitle);
        //            foreach (WOGroupStationInfo station in group.WOGroupStations)
        //            {
        //                if (station.DeliveryMethod.IndexOf("tf_") != -1)
        //                {
        //                    hasShipping = true;
        //                    Literal lit1 = new Literal();
        //                    lit1.ID = "lit1_" + group.Id.ToString() + "_" + station.Id.ToString();
        //                    if (ViewState["popupFirst"]=="true")
        //                    { lit1.Text = "<div class=\"col25\">Station:</div><div class=\"col50\">" + station.Station.StationName + "</div><br /><div class=\"col25\">Street1:</div><div class=\"col50\">"; }
        //                    pnlAddresses.Controls.Add(lit1);
        //                    TextBox txtStreet1 = new TextBox();
        //                    txtStreet1.ID = "txtStreet1" + group.Id.ToString() + "_" + station.Id.ToString();
        //                    if (ViewState["popupFirst"] == "true")
        //                    { txtStreet1.Text = station.Station.Address1; }
        //                    pnlAddresses.Controls.Add(txtStreet1);
        //                    Literal lit2 = new Literal();
        //                    lit2.ID = "lit2_" + group.Id.ToString() + "_" + station.Id.ToString();
        //                    if (ViewState["popupFirst"] == "true")
        //                    { lit2.Text = "</div><br /><div class=\"col25\">Street2:</div><div class=\"col50\">"; }
        //                    pnlAddresses.Controls.Add(lit2);
        //                    TextBox txtStreet2 = new TextBox();
        //                    txtStreet2.ID = "txtStreet2" + group.Id.ToString() + "_" + station.Id.ToString();
        //                    if (ViewState["popupFirst"] == "true")
        //                    { txtStreet2.Text = station.Station.Address2; }
        //                    pnlAddresses.Controls.Add(txtStreet2);
        //                    Literal lit3 = new Literal();
        //                    lit3.ID = "lit3_" + group.Id.ToString() + "_" + station.Id.ToString();
        //                    if (ViewState["popupFirst"] == "true")
        //                    { lit3.Text = "</div><br /><div class=\"col25\">City:</div><div class=\"col50\">"; }
        //                    pnlAddresses.Controls.Add(lit3);
        //                    TextBox txtCity = new TextBox();
        //                    txtCity.ID = "txtCity" + group.Id.ToString() + "_" + station.Id.ToString();
        //                    if (ViewState["popupFirst"] == "true")
        //                    { txtCity.Text = station.Station.City; }
        //                    pnlAddresses.Controls.Add(txtCity);
        //                    Literal lit4 = new Literal();
        //                    lit4.ID = "lit4_" + group.Id.ToString() + "_" + station.Id.ToString();
        //                    if (ViewState["popupFirst"] == "true")
        //                    { lit4.Text = "</div><br /><div class=\"col25\">State:</div><div class=\"col50\">"; }
        //                    pnlAddresses.Controls.Add(lit4);
        //                    TextBox txtState = new TextBox();
        //                    txtState.ID = "txtState" + group.Id.ToString() + "_" + station.Id.ToString();
        //                    if (ViewState["popupFirst"] == "true")
        //                    { txtState.Text = station.Station.State; }
        //                    pnlAddresses.Controls.Add(txtState);
        //                    Literal lit5 = new Literal();
        //                    lit5.ID = "lit5_" + group.Id.ToString() + "_" + station.Id.ToString();
        //                    if (ViewState["popupFirst"] == "true")
        //                    { lit5.Text = "</div><br /><div class=\"col25\">Zip:</div><div class=\"col50\">"; }
        //                    pnlAddresses.Controls.Add(lit5);
        //                    TextBox txtZip = new TextBox();
        //                    txtZip.ID = "txtZip" + group.Id.ToString() + "_" + station.Id.ToString();
        //                    if (ViewState["popupFirst"] == "true")
        //                    { txtZip.Text = station.Station.Zip; }
        //                    pnlAddresses.Controls.Add(txtZip);
        //                    Literal lit6 = new Literal();
        //                    lit6.ID = "lit6_" + group.Id.ToString() + "_" + station.Id.ToString();
        //                    if (ViewState["popupFirst"] == "true")
        //                    { lit6.Text = "</div><br /><div class=\"col25\">Attention:</div><div class=\"col50\">"; }
        //                    pnlAddresses.Controls.Add(lit6);
        //                    TextBox txtAttentionLine = new TextBox();
        //                    txtAttentionLine.ID = "txtAttentionLine" + group.Id.ToString() + "_" + station.Id.ToString();
        //                    if (ViewState["popupFirst"] == "true")
        //                    { txtAttentionLine.Text = station.Station.AttentionLine; }
        //                    pnlAddresses.Controls.Add(txtAttentionLine);
        //                    Literal lit7 = new Literal();
        //                    lit7.ID = "lit7_" + group.Id.ToString() + "_" + station.Id.ToString();
        //                    if (ViewState["popupFirst"] == "true")
        //                    { lit7.Text = "</div><br />"; }
        //                    pnlAddresses.Controls.Add(lit7);
        //                    if (ViewState["popupFirst"]=="true")
        //                    { ViewState["popupFirst"] = "false"; }
        //                }
        //            }
        //        }
        //    }
        //}

        protected void btnProcessAPIs_Click(object sender, EventArgs e)
        {
            if (ViewState["selectedWO"] != null)
            {
                AdminController aCont = new AdminController();
                List<TaskInfo> tasks = aCont.Get_TasksByWOId(Convert.ToInt32(ViewState["selectedWO"]));
                WorkOrderInfo wo = aCont.Get_WorkOrderById(Convert.ToInt32(ViewState["selectedWO"]));
                List<TaskInfo> comcastTasks = new List<TaskInfo>();
                bool hasShipping = false;
                foreach (WOGroupInfo group in wo.Groups)
                {
                    Literal litGroupTitle = new Literal();
                    litGroupTitle.ID = "litGroupTitle_" + group.Id.ToString();
                    litGroupTitle.Text = "<h3>" + group.GroupName + "</h3>";
                    pnlAddresses.Controls.Add(litGroupTitle);
                    foreach (WOGroupStationInfo station in group.WOGroupStations)
                    {
                        if (station.DeliveryMethod.IndexOf("tf_") != -1)
                        {
                            hasShipping = true;
                            //Literal lit1 = new Literal();
                            //lit1.ID = "lit1_" + group.Id.ToString() + "_" + station.Id.ToString();
                            //lit1.Text = "<div class=\"col25\">Station:</div><div class=\"col50\">" + station.Station.StationName + "</div><br /><div class=\"col25\">Street1:</div><div class=\"col50\">";
                            //pnlAddresses.Controls.Add(lit1);
                            //TextBox txtStreet1 = new TextBox();
                            //txtStreet1.ID = "txtStreet1" + group.Id.ToString() + "_" + station.Id.ToString();
                            //txtStreet1.Text = station.Station.Address1;
                            //pnlAddresses.Controls.Add(txtStreet1);
                            //Literal lit2 = new Literal();
                            //lit2.ID = "lit2_" + group.Id.ToString() + "_" + station.Id.ToString();
                            //lit2.Text = "</div><br /><div class=\"col25\">Street2:</div><div class=\"col50\">";
                            //pnlAddresses.Controls.Add(lit2);
                            //TextBox txtStreet2 = new TextBox();
                            //txtStreet2.ID = "txtStreet2" + group.Id.ToString() + "_" + station.Id.ToString();
                            //txtStreet2.Text = station.Station.Address2;
                            //pnlAddresses.Controls.Add(txtStreet2);
                            //Literal lit3 = new Literal();
                            //lit3.ID = "lit3_" + group.Id.ToString() + "_" + station.Id.ToString();
                            //lit3.Text = "</div><br /><div class=\"col25\">City:</div><div class=\"col50\">";
                            //pnlAddresses.Controls.Add(lit3);
                            //TextBox txtCity = new TextBox();
                            //txtCity.ID = "txtCity" + group.Id.ToString() + "_" + station.Id.ToString();
                            //txtCity.Text = station.Station.City;
                            //pnlAddresses.Controls.Add(txtCity);
                            //Literal lit4 = new Literal();
                            //lit4.ID = "lit4_" + group.Id.ToString() + "_" + station.Id.ToString();
                            //lit4.Text = "</div><br /><div class=\"col25\">State:</div><div class=\"col50\">";
                            //pnlAddresses.Controls.Add(lit4);
                            //TextBox txtState = new TextBox();
                            //txtState.ID = "txtState" + group.Id.ToString() + "_" + station.Id.ToString();
                            //txtState.Text = station.Station.State;
                            //pnlAddresses.Controls.Add(txtState);
                            //Literal lit5 = new Literal();
                            //lit5.ID = "lit5_" + group.Id.ToString() + "_" + station.Id.ToString();
                            //lit5.Text = "</div><br /><div class=\"col25\">Zip:</div><div class=\"col50\">";
                            //pnlAddresses.Controls.Add(lit5);
                            //TextBox txtZip = new TextBox();
                            //txtZip.ID = "txtZip" + group.Id.ToString() + "_" + station.Id.ToString();
                            //txtZip.Text = station.Station.Zip;
                            //pnlAddresses.Controls.Add(txtZip);
                            //Literal lit6 = new Literal();
                            //lit6.ID = "lit6_" + group.Id.ToString() + "_" + station.Id.ToString();
                            //lit6.Text = "</div><br /><div class=\"col25\">Attention:</div><div class=\"col50\">";
                            //pnlAddresses.Controls.Add(lit6);
                            //TextBox txtAttentionLine = new TextBox();
                            //txtAttentionLine.ID = "txtAttentionLine" + group.Id.ToString() + "_" + station.Id.ToString();
                            //txtAttentionLine.Text = station.Station.AttentionLine;
                            //pnlAddresses.Controls.Add(txtAttentionLine);
                            //Literal lit7 = new Literal();
                            //lit7.ID = "lit7_" + group.Id.ToString() + "_" + station.Id.ToString();
                            //lit7.Text = "</div><br />";
                            //pnlAddresses.Controls.Add(lit7);
                        }
                    }
                }
                foreach(TaskInfo task in tasks)
                {
                    if (task.DeliveryOrderId == "" && task.DeliveryStatus != "COMPLETE" && !task.isComplete && !task.isDeleted && task.DeliveryStatus.ToLower() != "closed manually")
                    {
                        if (task.DeliveryMethod.ToLower() == "javelin")
                        {
                            aCont.AddJavelinOrder(task.Id, PortalId);
                        }
                        else if (task.DeliveryMethod.ToLower() == "addelivery")
                        {
                            //List<TaskInfo> tempTasks = new List<TaskInfo>();
                            //tempTasks.Add(task);
                            //aCont.createComcastOrder(tempTasks, "", PortalId);
                            comcastTasks.Add(task);
                        }
                        else if (task.DeliveryMethod.ToLower() == "on the spot")
                        {
                            aCont.addOTSMOrder(task);
                        }
                    }
                    else if (task.DeliveryStatus != "COMPLETE" && !task.isComplete && !task.isDeleted && task.DeliveryStatus.ToLower() != "closed manually")
                    {
                        if (task.DeliveryMethod.ToLower() == "javelin")
                        {
                            aCont.GetJavelinOrderStatus(task.Id);
                        }
                        else if (task.DeliveryMethod.ToLower() == "addelivery")
                        {
                            aCont.getComcastOrderStatus(task);
                        }
                        else if (task.DeliveryMethod.ToLower() == "on the spot")
                        {
                            aCont.getOTSMOrderStatus(task);
                        }
                    }
                }
                if (comcastTasks.Count() > 0)
                {
                    //foreach(WOGroupInfo group in wo.Groups)
                    //{
                    //    if(group.GroupType == GroupTypeEnum.Bundle || group.GroupType == GroupTypeEnum.Delivery)
                    //    {
                    //        List<TaskInfo> theseTasks = new List<TaskInfo>();
                    //        foreach(TaskInfo thisTask in comcastTasks)
                    //        {
                    //            if(thisTask.WOGroupId == group.Id)
                    //            {
                    //                theseTasks.Add(thisTask);
                    //            }
                    //        }
                    //        aCont.createComcastOrder(theseTasks);
                    //    }
                    //}                    
                    aCont.createComcastOrder(comcastTasks);
                }
                if(hasShipping)
                {
                    //lblAPIMessages.Text = aCont.createEasySpotOrder(wo.Groups);
                    ViewState["popupFirst"] = "true";
                    //mpeStationAddressesPopup.Show();
                    Response.Redirect("/update-shipping.aspx?woid=" + wo.Id.ToString());
                }
                drawTasks(Convert.ToInt32(ViewState["selectedWO"]));
            }
        }

        protected void btnCancelTapeFormatPopup_Click(object sender, EventArgs e)
        {
            mpeStationAddressesPopup.Hide();
        }

        protected void lbtnAddToInvoice_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int woId = -1;
            try
            {
                woId = Convert.ToInt32(btn.CommandArgument);
            }
            catch { }
            List<int> woids = new List<int>();
            if(ViewState["woidsininvoice"]!=null)
            {
                woids = (List<int>)ViewState["woidsininvoice"];
            }
            if(woId != -1)
            {
                woids.Add(woId);
                ViewState["woidsininvoice"] = woids;
            }
            drawWoidsInInvoice();
            btn.Visible = false;
        }
        private void drawWoidsInInvoice()
        {
            List<int> woids = new List<int>();
            if (ViewState["woidsininvoice"] != null)
            {
                woids = (List<int>)ViewState["woidsininvoice"];
            }
            lbxWOsToInvoice.Items.Clear();
            foreach(int woid in woids)
            {
                lbxWOsToInvoice.Items.Add(new ListItem(woid.ToString(), woid.ToString()));
            }
        }

        protected void btnProcessInvoice_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            List<int> woids = new List<int>();
            if (ViewState["woidsininvoice"] != null)
            {
                woids = (List<int>)ViewState["woidsininvoice"];
            }
            string invoiceMessage = "";
            int adId = -1;
            int agId = -1;
            int billtoId = -1;
            List<WorkOrderInfo> wos = new List<WorkOrderInfo>();
            foreach(int woid in woids)
            {
                wos.Add(aCont.Get_WorkOrderById(woid));
            }
            if(woids.Count()>0)
            {
                //will probably need to check if they are all from same advertiser
                adId = wos[0].AdvertiserId;
                agId = wos[0].AgencyId;
                billtoId = wos[0].BillToId;
                if(billtoId == -1)
                {
                    billtoId = wos[0].AdvertiserId;
                }

                //make sure all tasks have a QBCode
                foreach(WorkOrderInfo wo in wos)
                {
                    int thisBillToId = -1;
                    thisBillToId = wo.BillToId;
                    if(thisBillToId==-1)
                    {
                        thisBillToId = wo.AdvertiserId;
                    }
                    if(thisBillToId!=billtoId)
                    {
                        invoiceMessage += "WO: " + wo.Id.ToString() + " does not have the same Bill To or Advertiser as the other work orders.<br />";
                    }
                    if(wo.Status=="CANCELLED")
                    {
                        invoiceMessage += "WO: " + wo.Id.ToString() + " has been cancelled and cannot be invoiced.<br />";
                    }
                    else if(wo.Status!="COMPLETE")
                    {
                        invoiceMessage += "WO: " + wo.Id.ToString() + " has not been marked COMPLETE and cannot be invoiced.<br />";
                    }
                    List<TaskInfo> tasks = aCont.Get_TasksByWOId(wo.Id);
                    int c = 0;
                    foreach(TaskInfo task in tasks)
                    {
                        if ((task.QBCode == "" || task.QBCodeId == -1) && (task.TaskType == GroupTypeEnum.Bundle || task.TaskType == GroupTypeEnum.Delivery) && task.DeliveryStatus.ToLower()!="cancelled" && !task.isDeleted)
                        {
                            invoiceMessage += "WO: " + wo.Id.ToString() + ", Task: " + task.Id.ToString() + " does not have a QB Code.<br />";
                        }
                        List<QBCodeInfo> servCodes = aCont.FindQBCodesByTask(task.Id, PortalId, true);
                        WOGroupInfo group = aCont.Get_WorkOrderGroupById(task.WOGroupId);
                        if(servCodes.Count != group.Services.Count)
                        {
                            invoiceMessage += "WO: " + wo.Id.ToString() + ", Task: " + task.Id.ToString() + ", not all services returned a QB Code.<br />QB Codes found: ";
                            foreach(QBCodeInfo servCode in servCodes)
                            {
                                invoiceMessage += servCode.QBCode + ", ";
                            }
                            invoiceMessage += "<br />Services found: ";
                            foreach(ServiceInfo serv in group.Services)
                            {
                                invoiceMessage += serv.ServiceName + ", ";
                            }
                            invoiceMessage += "<br />";
                        }
                        if (task.DeliveryStatus.ToLower() != "cancelled")
                        {
                            c++;
                        }
                    }
                    if(c==0)
                    {
                        invoiceMessage += "WO: " + wo.Id.ToString() + ", has no billable tasks.<br />";
                    }
                }
            }
            else
            {
                invoiceMessage = "No Work Orders added to Invoice.<br />";
            }

            if(invoiceMessage!="")
            {
                lblInvoiceMessage.Text = invoiceMessage;
            }
            else
            {
                //create invoice
                InvoiceInfo invoice = new InvoiceInfo();
                invoice.PortalId = PortalId;
                invoice.AdvertiserId = adId;
                invoice.AgencyId = agId;
                invoice.BillToId = billtoId;
                invoice.CreatedById = UserId;
                invoice.LastModifiedById = UserId;
                invoice.Id = aCont.Add_Invoice(invoice);
                foreach(WorkOrderInfo wo in wos)
                {
                    aCont.Add_WOInInvoice(invoice.Id, wo.Id);
                    wo.InvoiceNumber = "PENDING";
                    wo.LastModifiedById = UserId;
                    wo.LastModifiedDate = DateTime.Now;
                    aCont.Update_WorkOrder(wo);
                }
                lblInvoiceMessage.Text = "Invoice ready for QB Import.";
                lbxWOsToInvoice.Items.Clear();
                ViewState["woidsininvoice"] = new List<int>();
                fillMasters();
            }
        }

        protected void btnClearInvoiceItem_Click(object sender, EventArgs e)
        {
            lbxWOsToInvoice.Items.Remove(lbxWOsToInvoice.SelectedItem);
            List<int> woids = new List<int>();
            foreach(ListItem li in lbxWOsToInvoice.Items)
            {
                woids.Add(Convert.ToInt32(li.Value));
            }
            ViewState["woidsininvoice"] = woids;
        }
    }
}