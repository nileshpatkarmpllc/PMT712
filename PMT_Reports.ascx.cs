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
    public partial class PMT_Reports : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            FillAdvertiserList();
            FillAgencyList();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack && Request.QueryString["miid"]!=null)
            {
                txtMasterItemSearch.Text = Request.QueryString["miid"].ToString();
                ddlReport.SelectedValue = "1";
                FillMasterItemList();
                ddlMasterStatus.Visible = true;
                gvMasterItem.Visible = true;
                gvLibraryItem.Visible = false;
                btnExport.Visible = true;
                pnlDelivery.Visible = false;
            }
            if(!Page.IsPostBack && Request.QueryString["rid"]!=null)
            {
                if(Request.QueryString["rid"]=="1")
                {
                    ddlMasterStatus.Visible = true;
                    gvMasterItem.Visible = true;
                    gvLibraryItem.Visible = false;
                    btnExport.Visible = true;
                    pnlDelivery.Visible = false;
                    txtStartDate.Visible = false;
                    txtEndDate.Visible = false;
                    lblEndDate.Visible = false;
                    lblStartDate.Visible = false;
                    if (Request.QueryString["adid"] != null)
                    {
                        try
                        {
                            ddlReportsAdvertiserSearch.SelectedValue = Request.QueryString["adid"].ToString();
                            ViewState["selAdvertiser"] = Request.QueryString["adid"].ToString();
                        }
                        catch { }
                    }
                    if (Request.QueryString["agid"] != null)
                    {
                        try
                        {
                            ddlReportsAgencySearch.SelectedValue = Request.QueryString["agid"].ToString();
                            ViewState["selAgency"] = Request.QueryString["agid"].ToString();
                        }
                        catch { }
                    }
                    if (Request.QueryString["key"] != null)
                    {
                        txtMasterItemSearch.Text = Request.QueryString["key"].ToString();
                    }
                    if (Request.QueryString["status"] != null)
                    {
                        try
                        {
                            ddlMasterStatus.SelectedValue = Request.QueryString["status"].ToString();
                        }
                        catch { }
                    }
                    FillMasterItemList();
                }
                else if (Request.QueryString["rid"]=="2")
                {
                    ddlMasterStatus.Visible = false;
                    gvMasterItem.Visible = false;
                    gvLibraryItem.Visible = true;
                    pnlDelivery.Visible = false;
                    btnExport.Visible = true;
                    txtStartDate.Visible = false;
                    txtEndDate.Visible = false;
                    lblEndDate.Visible = false;
                    lblStartDate.Visible = false;
                    if (Request.QueryString["adid"] != null)
                    {
                        try
                        {
                            ddlReportsAdvertiserSearch.SelectedValue = Request.QueryString["adid"].ToString();
                            ViewState["selAdvertiser"] = Request.QueryString["adid"].ToString();
                        }
                        catch { }
                    }
                    if (Request.QueryString["agid"] != null)
                    {
                        try
                        {
                            ddlReportsAgencySearch.SelectedValue = Request.QueryString["agid"].ToString();
                            ViewState["selAgency"] = Request.QueryString["agid"].ToString();
                        }
                        catch { }
                    }
                    if (Request.QueryString["key"] != null)
                    {
                        txtMasterItemSearch.Text = Request.QueryString["key"].ToString();
                    }
                    FillLibraryItemList();
                }
                else if(Request.QueryString["rid"]=="3")
                {
                    ddlReport.SelectedValue = "3";
                    ddlMasterStatus.Visible = false;
                    gvMasterItem.Visible = false;
                    gvLibraryItem.Visible = false;
                    pnlDelivery.Visible = true;
                    btnExport.Visible = true;
                    txtStartDate.Visible = true;
                    txtEndDate.Visible = true;
                    lblEndDate.Visible = true;
                    lblStartDate.Visible = true;
                    if(Request.QueryString["adid"]!=null)
                    {
                        try { ddlReportsAdvertiserSearch.SelectedValue = Request.QueryString["adid"].ToString();
                            ViewState["selAdvertiser"] = Request.QueryString["adid"].ToString();
                        }
                        catch { }
                    }
                    if(Request.QueryString["agid"]!=null)
                    {
                        try { ddlReportsAgencySearch.SelectedValue = Request.QueryString["agid"].ToString();
                            ViewState["selAgency"] = Request.QueryString["agid"].ToString();
                        }
                        catch { }
                    }
                    if(Request.QueryString["key"]!=null)
                    {
                        txtMasterItemSearch.Text = Request.QueryString["key"].ToString();
                    }
                    if(Request.QueryString["start"]!=null)
                    {
                        txtStartDate.Text = Request.QueryString["start"].ToString().Replace("-","/");
                    }
                    if(Request.QueryString["end"]!=null)
                    {
                        txtEndDate.Text = Request.QueryString["end"].ToString().Replace("-", "/");
                    }
                    fillDeliveries();
                }
                else
                {
                    ddlMasterStatus.Visible = false;
                    gvMasterItem.Visible = false;
                    gvLibraryItem.Visible = false;
                    btnExport.Visible = false;
                    pnlDelivery.Visible = false;
                    lblReportsMessage.Text = " ";
                    txtStartDate.Visible = false;
                    txtEndDate.Visible = false;
                    lblEndDate.Visible = false;
                    lblStartDate.Visible = false;
                }
            }
            if (UserId == -1)
            {
                ddlReport.Visible = false;
                ddlReportsAdvertiserSearch.Visible = false;
                ddlReportsAgencySearch.Visible = false;
                txtMasterItemSearch.Visible = false;
                ddlMasterStatus.Visible = false;
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
            if(!canSeeAll)//if canSeeAll is true, we pass back 0, which is already the value of userCase
            {
                AdminController aCont = new AdminController();
                List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
                List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
                bool isAd = userAds.Count>0? true : false; //are they in Advertiser Role?
                bool isAg = userAgs.Count>0? true : false; //are they in Agency Role?
                foreach (string ar in allRoles)
                {
                    if(ar.ToLower()=="advertiser")
                    {
                        isAd = true;
                    }
                    if(ar.ToLower()=="agency")
                    {
                        isAg = true;
                    }
                }
                //now, check for case 1=Ad/Ad
                if((isAd && !isAg && userAds.Count>0 && userAgs.Count==0) || (isAg && userAds.Count > 0 && userAgs.Count==0))
                {
                    userCase = 1;
                }
                //check for case 2=Ag/Ag
                else if ((!isAd && isAg && userAds.Count==0 && userAgs.Count>0) || (isAd && userAgs.Count > 0 && userAds.Count==0))
                {
                    userCase = 2;
                }
                //check for case 3=Both/Both
                else if((isAd && isAg && userAds.Count>0 && userAgs.Count>0) || (userAds.Count > 0 && userAgs.Count>0))
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
            if(selAd != "" && selAd != "-1")
            {
                adSel = true;
            }
            if(selAg != "" && selAg != "-1")
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
            if(userCase==1 && selCase==0)
            { drawCase = 1; }
            else if (userCase ==2 && selCase==0)
            { drawCase = 2; }
            else if ((userCase == 0 || userCase == 3) && selCase == 0)
            { drawCase = 3; }
            else if(selCase==1 && (userCase==0 || userCase==1 || userCase==2 || userCase==3))
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
            if (userCase==0)
            {
                canSeeAll = true;
            }
            AdminController aCont = new AdminController();
            List<AdvertiserInfo> Advertisers = getAdvertisers();

            ddlReportsAdvertiserSearch.Items.Clear();
            ddlReportsAdvertiserSearch.Items.Add(new ListItem("--Select Advertiser--", "-1"));
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
                        ddlReportsAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    }
                }
                else
                {
                    List<AdvertiserInfo> sortedList = Advertisers.Distinct(new AdvertiserComparer()).OrderBy(o => o.AdvertiserName).ToList();
                    foreach (AdvertiserInfo adv in sortedList)
                    {
                        ddlReportsAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    }
                }
            }
            else
            {
                List<AdvertiserInfo> selAds = new List<AdvertiserInfo>();
                if(selAg!="" && selAg !="-1")
                {
                    selAds = aCont.Get_AdvertisersByAgencyId(Convert.ToInt32(selAg));
                }
                if (userCase==1) //Ad/Ad  (drawCase == 1 || drawCase ==4)
                {
                    List<AdvertiserInfo> sortedList = userAds.Distinct(new AdvertiserComparer()).OrderBy(o => o.AdvertiserName).ToList();
                    foreach (AdvertiserInfo adv in sortedList)
                    {
                        ddlReportsAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    }
                }
                else if (userCase==2) //(drawCase==2)//get all advertisers from agencies user is connected to
                {
                    List<AdvertiserInfo> addAds = new List<AdvertiserInfo>();
                    foreach(AgencyInfo ag in userAgs)
                    {
                        List<AdvertiserInfo> agAds = aCont.Get_AdvertisersByAgencyId(ag.Id);
                        foreach(AdvertiserInfo ad in agAds)
                        {
                            addAds.Add(ad);
                        }
                    }
                    List<AdvertiserInfo> sortedList = addAds.Distinct().OrderBy(o => o.AdvertiserName).ToList();
                    if (selAg == "" || selAg == "-1")
                    {
                        foreach (AdvertiserInfo adv in sortedList)
                        {
                            ddlReportsAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                    else
                    {
                        foreach (AdvertiserInfo adv in sortedList)
                        {
                            ddlReportsAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                }
                else if (userCase==3) //(drawCase==3 || drawCase==6)//get all advertisers user is connected to as well as all the advertisers that are connected to the agencies the user is tied to
                {
                    List<AdvertiserInfo> addAds = new List<AdvertiserInfo>();
                    foreach(AdvertiserInfo ad in userAds)
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
                            ddlReportsAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                    else
                    {
                        foreach (AdvertiserInfo adv in selAds)
                        {
                            ddlReportsAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                }
            }
            if(ddlReportsAdvertiserSearch.Items.Count==2)
            {
                ddlReportsAdvertiserSearch.SelectedIndex = 1;
            }
            else if(selAd != "")
            {
                try
                {
                    ddlReportsAdvertiserSearch.SelectedValue = selAd;
                }
                catch { }
            }
        }
        private void FillAgencyList()
        {
            ddlReportsAgencySearch.Items.Clear();
            string selAd = ""; //was sel
            if(ViewState["selAdvertiser"]!=null)
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
            List<AgencyInfo> agencies = getAgencies();
            ddlReportsAgencySearch.Items.Add(new ListItem("--Select Agency--", "-1"));
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
                        ddlReportsAgencySearch.Items.Add(li);
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
                            ddlReportsAgencySearch.Items.Add(li);
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
                if (userCase==1) //ad/ad //(drawCase == 1 || drawCase == 4)
                {
                    List<AgencyInfo> addAgs = new List<AgencyInfo>();
                    foreach (AdvertiserInfo adv in userAds)
                    {
                        if ((selAd == "" || selAd == "-1"))
                        {
                            foreach(AgencyInfo ag in adv.Agencies)
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
                        ddlReportsAgencySearch.Items.Add(li);
                    }
                }
                else if (userCase==2) //(drawCase==2 || drawCase==5) //all agencies user is connected to
                {
                    List<AgencyInfo> sortedList = userAgs.Distinct(new AgencyComparer()).OrderBy(o => o.AgencyName).ToList();
                    if ((selAd == "" || selAd == "-1"))
                    {
                        foreach (AgencyInfo ag in sortedList)
                        {
                            ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                            ddlReportsAgencySearch.Items.Add(li);
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
                                    ddlReportsAgencySearch.Items.Add(li);
                                }
                            }
                        }
                    }
                }
                else if (userCase==3) //(drawCase==3) //all agencies user is connected to and all agencies connected to the advertisers the user is connected to
                {
                    List<AgencyInfo> ags = new List<AgencyInfo>();
                    foreach (AgencyInfo ag in userAgs)
                    {
                        ags.Add(ag);
                    }
                    foreach(AdvertiserInfo ad in userAds)
                    {
                        List<AgencyInfo> adAgs = aCont.Get_AgenciesByAdvertiserId(ad.Id);
                        foreach(AgencyInfo ag in adAgs)
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
                            ddlReportsAgencySearch.Items.Add(li);
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
                                    ddlReportsAgencySearch.Items.Add(li);
                                }
                            }
                        }
                    }
                }
            }
            if(ddlReportsAgencySearch.Items.Count==2)
            {
                ddlReportsAgencySearch.SelectedIndex = 1;
            }
            else if (selAg!="")
            {
                try
                {
                    ddlReportsAgencySearch.SelectedValue = selAg;
                }
                catch { }
            }
        }

        private List<MasterItemInfo> getMastersByCriteria()
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
            List<MasterItemInfo> MasterItems = new List<MasterItemInfo>();
            List<MasterItemInfo> mstrsByUser = new List<MasterItemInfo>();
            MasterItems = aCont.Get_MasterItemsByPortalId(PortalId);
            List<AgencyInfo> agencies = aCont.Get_AgenciesByUser(UserId);
            List<AdvertiserInfo> advertisers = aCont.Get_AdvertisersByUser(UserId, PortalId);
            bool isAgencyRole = UserInfo.IsInRole("Agency");
            bool isAdvertiserRole = UserInfo.IsInRole("Advertiser");

            List<MasterItemInfo> mstrsbyAgency = new List<MasterItemInfo>();
            List<MasterItemInfo> mstrsbyAdvertiser = new List<MasterItemInfo>();
            List<MasterItemInfo> mstrsbyKeyword = new List<MasterItemInfo>();
            List<MasterItemInfo> mstrsbyStatus = new List<MasterItemInfo>();

            if (ddlReportsAdvertiserSearch.SelectedIndex > 0)
            {
                foreach (MasterItemInfo MasterItem in MasterItems)
                {
                    if ((MasterItem.AdvertiserId == Convert.ToInt32(ddlReportsAdvertiserSearch.SelectedValue)))
                    {
                        mstrsbyAdvertiser.Add(MasterItem);
                    }
                }
            }
            else
            {
                mstrsbyAdvertiser = MasterItems;
            }
            if (ddlReportsAgencySearch.SelectedIndex > 0)
            {
                foreach (MasterItemInfo MasterItem in mstrsbyAdvertiser)
                {
                    if (MasterItem.Agencies != null)
                    {
                        foreach (AgencyInfo ag in MasterItem.Agencies)
                        {
                            if (ag.Id.ToString() == ddlReportsAgencySearch.SelectedValue)
                            {
                                mstrsbyAgency.Add(MasterItem);
                            }
                        }
                    }
                }
            }
            else
            {
                mstrsbyAgency = mstrsbyAdvertiser;
            }
            if (txtMasterItemSearch.Text != "")
            {
                foreach (MasterItemInfo MasterItem in mstrsbyAgency)
                {
                    if (MasterItem.PMTMediaId.ToString().ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                        MasterItem.Title.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                        MasterItem.Filename.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                        MasterItem.CustomerId.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1)
                    {
                        mstrsbyKeyword.Add(MasterItem);
                    }
                }
            }
            else
            {
                mstrsbyKeyword = mstrsbyAgency;
            }
            if (ddlMasterStatus.SelectedIndex > 0)
            {
                //0=None,1=new,2=pending,3=approved
                foreach (MasterItemInfo MasterItem in mstrsbyKeyword)
                {

                    if (ddlMasterStatus.SelectedValue == "1" && !MasterItem.hasChecklist)
                    {
                        mstrsbyStatus.Add(MasterItem);
                    }
                    else if (ddlMasterStatus.SelectedValue == "2" && MasterItem.hasChecklist && !MasterItem.isApproved)
                    {
                        mstrsbyStatus.Add(MasterItem);
                    }
                    else if (ddlMasterStatus.SelectedValue == "3" && MasterItem.hasChecklist && MasterItem.isApproved)
                    {
                        mstrsbyStatus.Add(MasterItem);
                    }
                }

            }
            else
            {
                mstrsbyStatus = mstrsbyKeyword;
            }
            if (canSeeAll)
            {
                mstrsByUser = mstrsbyStatus;
            }
            else
            {
                foreach (MasterItemInfo master in mstrsbyStatus)
                {
                    foreach (AdvertiserInfo ad in advertisers)
                    {
                        if (ad.Id == master.AdvertiserId)
                        {
                            mstrsByUser.Add(master);
                        }
                    }
                    foreach (AgencyInfo ag in agencies)
                    {
                        foreach (AgencyInfo ag2 in master.Agencies)
                        {
                            if (ag.Id == ag2.Id)
                            {
                                mstrsByUser.Add(master);
                            }
                        }
                    }
                }
            }
            List<MasterItemInfo> sortedList = mstrsByUser.Distinct(new MasterItemComparer()).OrderBy(o => o.Title).ToList();
            return sortedList;
        }
        private void FillMasterItemList()
        {
            AdminController aCont = new AdminController();
            List<MasterItemInfo> sortedList = getMastersByCriteria();
            //hydrate advertisers and agencies
            List<AdvertiserInfo> allAds = getAdvertisers();
            List<AgencyInfo> allAgs = getAgencies();
            List<AgencyInfo> agencies = aCont.Get_AgenciesByUser(UserId);
            List<AdvertiserInfo> advertisers = aCont.Get_AdvertisersByUser(UserId, PortalId);
            bool isAgencyRole = UserInfo.IsInRole("Agency");
            bool isAdvertiserRole = UserInfo.IsInRole("Advertiser");
            foreach (MasterItemInfo master in sortedList) //(MasterItemInfo master in mstrsbyKeyword)
            {
                string ags = "";
                var advertiser = allAds.FirstOrDefault(i => i.Id == master.AdvertiserId);
                if (advertiser != null)
                {
                    master.Advertiser = advertiser.AdvertiserName;
                }
                foreach (AgencyInfo ag in master.Agencies)
                {
                    var agency = allAgs.FirstOrDefault(i => i.Id == ag.Id);
                    if (agency != null)
                    {
                        if (isAgencyRole && !isAdvertiserRole)
                        {
                            var inag = agencies.FirstOrDefault(o => o.Id == ag.Id);
                            if (inag != null)
                            {
                                ags += agency.AgencyName + ", ";
                            }
                        }
                        else
                        {
                            ags += agency.AgencyName + ", ";
                        }
                    }
                }
                if (ags.Length > 2)
                {
                    ags = ags.Substring(0, ags.Length - 2);
                }
                master.AgencyNames = ags;
            }
            if (ViewState["MasterItemPage"] != null)
            {
                gvMasterItem.PageIndex = Convert.ToInt32(ViewState["MasterItemPage"]);
            }
            lblReportsMessage.Text = sortedList.Count.ToString() + " Master Items found.";
            gvMasterItem.DataSource = sortedList;
            gvMasterItem.DataBind();
            if (UserId == -1)
            {
                lblReportsMessage.Text = "Please login to see this report.";
            }
        }
        public List<AdvertiserInfo> getAdvertisers()
        {
            AdminController aCont = new AdminController();
            return aCont.getAdvertisers(PortalId);
        }
        public List<AgencyInfo> getAgencies()
        {
            AdminController aCont = new AdminController();
            return aCont.getAgencies(PortalId);
        }
        private List<LibraryItemInfo> getLibsByCriteria()
        {
            AdminController aCont = new AdminController();
            bool canViewAll = false;
            int userCase = getUserCase();
            if (userCase == 0)
            {
                canViewAll = true;
            }
            List<LibraryItemInfo> LibraryItems = new List<LibraryItemInfo>();
            List<LibraryItemInfo> libsbyAgency = new List<LibraryItemInfo>();
            List<LibraryItemInfo> libsbyAdvertiser = new List<LibraryItemInfo>();
            List<LibraryItemInfo> libsbyKeyword = new List<LibraryItemInfo>();
            List<LibraryItemInfo> libsbyStatus = new List<LibraryItemInfo>();
            List<LibraryItemInfo> libsByUser = new List<LibraryItemInfo>();
            LibraryItems = aCont.Get_LibraryItemsByPortalId(PortalId);
            List<AgencyInfo> agencies = aCont.Get_AgenciesByUser(UserId);
            List<AdvertiserInfo> advertisers = aCont.Get_AdvertisersByUser(UserId, PortalId);
            bool isAgencyRole = UserInfo.IsInRole("Agency");
            bool isAdvertiserRole = UserInfo.IsInRole("Advertiser");
            if (ddlReportsAgencySearch.SelectedIndex > 0)
            {
                foreach (LibraryItemInfo LibraryItem in LibraryItems)
                {
                    if (LibraryItem.AgencyId != -1)
                    {
                        if (LibraryItem.AgencyId.ToString() == ddlReportsAgencySearch.SelectedValue)
                        {
                            libsbyAgency.Add(LibraryItem);
                        }
                    }
                }
            }
            else
            {
                libsbyAgency = LibraryItems;
            }

            if (ddlReportsAdvertiserSearch.SelectedIndex > 0)
            {
                foreach (LibraryItemInfo LibraryItem in libsbyAgency)
                {
                    if ((LibraryItem.AdvertiserId == Convert.ToInt32(ddlReportsAdvertiserSearch.SelectedValue)))
                    {
                        libsbyAdvertiser.Add(LibraryItem);
                    }
                }
            }
            else
            {
                libsbyAdvertiser = libsbyAgency;
            }

            if (txtMasterItemSearch.Text != "")
            {
                foreach (LibraryItemInfo LibraryItem in libsbyAdvertiser)
                {
                    if (LibraryItem.PMTMediaId.ToString().ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                        LibraryItem.Title.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                        LibraryItem.Filename.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                        LibraryItem.AgencyId.ToString().ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                        LibraryItem.Id.ToString().ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                        LibraryItem.ISCICode.ToString().ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                        LibraryItem.ProductDescription.ToString().ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1)
                    {
                        libsbyKeyword.Add(LibraryItem);
                    }
                }
            }
            else
            {
                libsbyKeyword = libsbyAdvertiser;
            }

            if (canViewAll)
            {
                libsByUser = libsbyKeyword;
            }
            else
            {
                foreach (LibraryItemInfo lib in libsbyKeyword)
                {
                    foreach (AdvertiserInfo ad in advertisers)
                    {
                        if (ad.Id == lib.AdvertiserId)
                        {
                            libsByUser.Add(lib);
                        }
                    }
                    foreach (AgencyInfo ag in agencies)
                    {
                        if (ag.Id == lib.AgencyId)
                        {
                            libsByUser.Add(lib);
                        }
                    }
                }
            }

            List<LibraryItemInfo> sortedList = libsByUser.Distinct(new LibraryItemComparer()).OrderBy(o => o.Title).ToList();
            return sortedList;
        }
        private void FillLibraryItemList()
        {
            AdminController aCont = new AdminController();
            List<LibraryItemInfo> sortedList = getLibsByCriteria();
            //fill in Agency and Advertiser names
            List<AdvertiserInfo> allAds = aCont.Get_AdvertisersByPortalId(PortalId);
            List<AgencyInfo> allAgs = aCont.Get_AgenciesByPortalId(PortalId);
            foreach (LibraryItemInfo lib in sortedList)
            {
                var advertiser = allAds.FirstOrDefault(i => i.Id == lib.AdvertiserId);
                var agency = allAgs.FirstOrDefault(i => i.Id == lib.AgencyId);
                if (advertiser != null)
                {
                    lib.Advertiser = advertiser.AdvertiserName;
                }
                if (agency != null)
                {
                    lib.Agency = agency.AgencyName;
                }
            }

            if (Session["LibraryItemPage"] != null)
            {
                gvLibraryItem.PageIndex = Convert.ToInt32(Session["LibraryItemPage"]);
            }
            lblReportsMessage.Text = sortedList.Count.ToString() + " Library Items found.";
            gvLibraryItem.DataSource = sortedList;
            gvLibraryItem.DataBind();
            if(UserId == -1)
            {
                lblReportsMessage.Text = "Please login to see this report.";
            }
        }
        protected void ddlReportsAdvertiserSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["selAdvertiser"] = ddlReportsAdvertiserSearch.SelectedValue;
            if (ddlReport.SelectedValue == "1")
            {
                FillAgencyList();
                FillMasterItemList();
            }
            else if (ddlReport.SelectedValue == "2")
            {
                FillAgencyList();
                FillLibraryItemList();
            }
            else if (ddlReport.SelectedValue == "3")
            {
                fillDeliveries();
            }
            else
            {
                FillAgencyList();
            }
        }

        protected void ddlReportsAgencySearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["selAgency"] = ddlReportsAgencySearch.SelectedValue;
            FillAdvertiserList();
            if (ddlReport.SelectedValue == "1")
            {
                FillMasterItemList();
            }
            else if (ddlReport.SelectedValue == "2")
            {
                FillLibraryItemList();
            }
            else if (ddlReport.SelectedValue == "3")
            {
                fillDeliveries();
            }
        }

        protected void gvLibraryItem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvLibraryItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["LibraryItemPage"] = e.NewPageIndex.ToString();
            FillLibraryItemList();
        }

        protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlReport.SelectedValue=="1")
            {
                FillMasterItemList();
                ddlMasterStatus.Visible = true;
                gvMasterItem.Visible = true;
                gvLibraryItem.Visible = false;
                btnExport.Visible = true;
                pnlDelivery.Visible = false;
                txtStartDate.Visible = false;
                txtEndDate.Visible = false;
                lblEndDate.Visible = false;
                lblStartDate.Visible = false;
            }
            else if (ddlReport.SelectedIndex==0)
            {
                ddlMasterStatus.Visible = false;
                gvMasterItem.Visible = false;
                gvLibraryItem.Visible = false;
                btnExport.Visible = false;
                pnlDelivery.Visible = false;
                lblReportsMessage.Text = " ";
                txtStartDate.Visible = false;
                txtEndDate.Visible = false;
                lblEndDate.Visible = false;
                lblStartDate.Visible = false;
            }
            else if(ddlReport.SelectedValue=="2")
            {
                FillLibraryItemList();
                ddlMasterStatus.Visible = false;
                gvMasterItem.Visible = false;
                gvLibraryItem.Visible = true;
                pnlDelivery.Visible = false;
                btnExport.Visible = true;
                txtStartDate.Visible = false;
                txtEndDate.Visible = false;
                lblEndDate.Visible = false;
                lblStartDate.Visible = false;
            }
            else if (ddlReport.SelectedValue == "3")
            {
                ddlMasterStatus.Visible = false;
                gvMasterItem.Visible = false;
                gvLibraryItem.Visible = false;
                pnlDelivery.Visible = true;
                btnExport.Visible = true;
                txtStartDate.Visible = true;
                txtEndDate.Visible = true;
                lblEndDate.Visible = true;
                lblStartDate.Visible = true;
                txtStartDate.Text = DateTime.Now.ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                fillDeliveries();
            }
        }

        private List<TaskInfo> getReportTasks()
        {
            AdminController aCont = new AdminController();
            List<TaskInfo> returnTasks = new List<TaskInfo>();
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
            int userCase = getUserCase();
            bool canSeeAll = false;
            if (userCase == 0)
            {
                canSeeAll = true;
            }
            else
            {
                userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
                userAgs = aCont.Get_AgenciesByUser(UserId);
            }
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
            if (selAd != "" && selAd!="-1")
            {
                foreach (WorkOrderInfo wo in wos)
                {
                    if(wo.AdvertiserId.ToString()==selAd)
                    {
                        wosByAdvertiser.Add(wo);
                    }
                }
            }
            else
            {
                wosByAdvertiser = wos;
            }
            if(selAg!="" && selAg!="-1")
            {
                foreach(WorkOrderInfo wo in wosByAdvertiser)
                {
                    if(wo.AgencyId.ToString() == selAg)
                    {
                        wosByAgency.Add(wo);
                    }
                }
            }
            else
            {
                wosByAgency = wosByAdvertiser;
            }
            //if(txtMasterItemSearch.Text!="")
            //{
            //    foreach(WorkOrderInfo wo in wosByAgency)
            //    {
            //        if(wo.Description.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower())!=-1 ||
            //            wo.PONumber.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
            //            wo.Notes.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
            //            wo.InvoiceNumber.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1)
            //        {
            //            wosByKeyword.Add(wo);
            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
                wosByKeyword = wosByAgency;
            //}
            //int maxShow = 100;
            int shown = 0;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            DateTime tempdate = DateTime.Now;
            try { tempdate = Convert.ToDateTime(txtStartDate.Text);
            startDate = new DateTime(tempdate.Year, tempdate.Month, tempdate.Day, 0, 0, 0);
            }
            catch { }
            try { tempdate = Convert.ToDateTime(txtEndDate.Text);
            endDate = new DateTime(tempdate.Year, tempdate.Month, tempdate.Day, 23, 59, 59);
            }
            catch { }
            foreach(WorkOrderInfo wo in wosByKeyword)
            {
                //if (shown >= maxShow)
                //{
                //    break;
                //}
                //else
                //{
                //Moving date detection to task level
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
                    if (Request.QueryString["rid"] != null)
                    {
                        showThis = true;
                    }
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
            return returnTasks;
        }

        private void fillDeliveries()
        {
            litDelivery.Text = "";
            AdminController aCont = new AdminController();
            List<TaskInfo> returnTasks = new List<TaskInfo>();
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
            List<TaskInfo> tasks = getReportTasks();
            int shown = 0;
            foreach (TaskInfo task in tasks)
            {
                if (task.TaskType == GroupTypeEnum.Delivery || task.TaskType == GroupTypeEnum.Bundle)
                {
                    var wo = wos.FirstOrDefault(o => o.Id == task.WorkOrderId);
                    if (wo != null)
                    {
                        if(wo.Status != "CANCELLED")
                        { 
                        var lib = libs.FirstOrDefault(o => o.Id == task.LibraryId);
                        WOGroupStationInfo groupStation = aCont.Get_WorkOrderGroupStationById(task.StationId);
                        var stat = stations.FirstOrDefault(o => o.Id == groupStation.StationId);
                        if (stat == null) { stat = new StationInfo(); }
                        if (lib != null)
                        {
                            if (wo.Description.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                wo.PONumber.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                wo.Notes.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                wo.InvoiceNumber.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                lib.ISCICode.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                lib.Title.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                lib.ProductDescription.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                stat.CallLetter.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                stat.StationName.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1)
                            {
                                litDelivery.Text += "<tr><td>" + task.WorkOrderId.ToString() + "</td>";
                                litDelivery.Text += "<td>" + lib.ISCICode + "</td>";
                                string ad = "";
                                string ag = "";
                                var thisad = ads.FirstOrDefault(o => o.Id == lib.AdvertiserId);
                                if (thisad != null) { ad = thisad.AdvertiserName; }
                                litDelivery.Text += "<td>" + ad + "</td>";
                                var thisag = ags.FirstOrDefault(o => o.Id == lib.AgencyId);
                                if (thisag != null) { ag = thisag.AgencyName; }
                                litDelivery.Text += "<td>" + ag + "</td>";
                                litDelivery.Text += "<td>" + lib.ProductDescription + "</td>";
                                litDelivery.Text += "<td>" + lib.Title + "</td>";
                                litDelivery.Text += "<td>";
                                if (task.DeliveryOrderId != "")
                                {
                                    litDelivery.Text += "<a href=\"https://www.fedex.com/apps/fedextrack/?action=track&action=track&language=english&cntry_code=us&tracknumbers=" + task.DeliveryOrderId.ToString() + "\" target=\"_blank\">" + task.DeliveryOrderId.ToString() + "</a>";
                                }
                                litDelivery.Text += "</td>";
                                //var station = stations.FirstOrDefault(o => o.Id == task.StationId);
                                string stationName = "";
                                //if (station != null)
                                //{
                                stationName = stat.StationName;
                                //}
                                litDelivery.Text += "<td>" + stationName + "</td>";
                                litDelivery.Text += "<td>" + wo.DateCreated.ToShortDateString() + "</td>";
                                litDelivery.Text += "<td>" + task.DeliveryMethod + "</td></tr>";
                                shown++;
                            }
                        }
                        }
                    }
                }
            }
            lblReportsMessage.Text = "Delivery Report: " + shown.ToString() + " Deliveries found.";
        }

        protected void txtMasterItemSearch_TextChanged(object sender, EventArgs e)
        {
            if (ddlReport.SelectedValue == "1")
            {
                FillMasterItemList();
            }
            else if(ddlReport.SelectedValue=="2")
            {
                FillLibraryItemList();
            }
            else if (ddlReport.SelectedValue == "3")
            {
                fillDeliveries();
            }
        }

        protected void ddlMasterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillMasterItemList();
        }

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int masterId = -1;
            try
            {
                masterId = Convert.ToInt32(btn.CommandArgument);
            }
            catch { }
            if (masterId != -1)
            {
                AdminController aCont = new AdminController();
                MasterItemInfo master = aCont.Get_MasterItemById(masterId);
                if (master.PMTMediaId != "")
                {
                    litVidSource.Text = "<source src=\"https://s3-pmt-bucket.s3.amazonaws.com/Viewers/" + master.PMTMediaId + ".mp4\" type=\"video/mp4\">";
                    mpeViewerPopup.Show();
                }
            }
        }
        protected void lbtnLibEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int masterId = -1;
            try
            {
                masterId = Convert.ToInt32(btn.CommandArgument);
            }
            catch { }
            if (masterId != -1)
            {
                AdminController aCont = new AdminController();
                LibraryItemInfo master = aCont.Get_LibraryItemById(masterId);
                int secs = 0;
                try
                {
                    string[] pcs = master.MediaLength.Split(':');
                    secs = 60 * Convert.ToInt32(pcs[0]) + Convert.ToInt32(pcs[1]);
                }
                catch { }
                if (master.ISCICode != "")
                {
                    if (secs < 299)
                    {
                        litVidSource.Text = "<source src=\"https://s3-pmt-bucket.s3.amazonaws.com/Viewers/" + master.ISCICode + ".mp4\" type=\"video/mp4\">";
                    }
                    else
                    {
                        string phone = master.ProductDescription.Replace("-", "");
                        if (phone.Length == 11)
                        {
                            phone = phone.Substring(1, 10);
                        }
                        litVidSource.Text = "<source src=\"https://s3-pmt-bucket.s3.amazonaws.com/Viewers/" + master.PMTMediaId + phone + ".mp4\" type=\"video/mp4\">";
                    }
                    mpeViewerPopup.Show();
                }
            }
        }

        protected void gvMasterItem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void gvMasterItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["MasterItemPage"] = e.NewPageIndex.ToString();
            //gvMasterItem.PageIndex = e.NewPageIndex;
            //gvMasterItem.DataBind();
            FillMasterItemList();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mpeViewerPopup.Hide();
            FillMasterItemList();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            List<AdvertiserInfo> ads = getAdvertisers();
            List<AgencyInfo> agencies = getAgencies();
            List<AgencyInfo> userAgencies = aCont.Get_AgenciesByUser(UserId);
            List<AdvertiserInfo> advertisers = aCont.Get_AdvertisersByUser(UserId, PortalId);
            bool isAgencyRole = UserInfo.IsInRole("Agency");
            bool isAdvertiserRole = UserInfo.IsInRole("Advertiser");
            if(ddlReport.SelectedValue=="1")
            {
                List<MasterItemInfo> masters = getMastersByCriteria();
                MemoryStream ms = new MemoryStream();
                TextWriter tw = new StreamWriter(ms);
                tw.WriteLine("PMTMediaId,Title,Agencies,Advertiser,Length,Status");
                foreach(MasterItemInfo master in masters)
                {
                    var advertiser = ads.FirstOrDefault(i => i.Id == master.AdvertiserId);
                    string line = "\"" + master.PMTMediaId + "\",\"" + master.Title + "\",\"";
                    string ags = "";
                    foreach(AgencyInfo ag in master.Agencies)
                    {
                        if (isAgencyRole && !isAdvertiserRole)
                        {
                            var inag = userAgencies.FirstOrDefault(o => o.Id == ag.Id);
                            if (inag != null)
                            {
                                ags += ag.AgencyName + ", ";
                            }
                        }
                        else
                        {
                            ags += ag.AgencyName + ", ";
                        }
                    }
                    ags = ags.Substring(0, ags.Length - 2);
                    line += ags;
                    line += "\",\"" + advertiser.AdvertiserName + "\",\"" + master.Length + "\",\"";
                    if(master.CheckListForm=="")
                    {
                        line += "NEW";
                    }
                    else if (master.CheckListForm!="" && !master.isApproved)
                    {
                        line += "PENDING";
                    }
                    else if (master.CheckListForm!="" && master.isApproved)
                    {
                        line += "APPROVED";
                    }
                    else
                    {
                        line += "ERROR";
                    }
                    line += "\"";
                    tw.WriteLine(line);
                }
                tw.Flush();
                byte[] bytes = ms.ToArray();
                ms.Close();

                Response.Clear();
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment;    filename=mastersreport.csv");
                Response.BinaryWrite(bytes);
                Response.End();
            }
            else if (ddlReport.SelectedValue == "2")
            {
                List<LibraryItemInfo> libs = getLibsByCriteria();
                MemoryStream ms = new MemoryStream();
                TextWriter tw = new StreamWriter(ms);
                tw.WriteLine("PMTMediaId,ISCI Code,Title,Description,Advertiser,Agency,Length");
                foreach (LibraryItemInfo lib in libs)
                {
                    var advertiser = ads.FirstOrDefault(i => i.Id == lib.AdvertiserId);
                    var agency = agencies.FirstOrDefault(i => i.Id == lib.AgencyId);
                    string line = "\"" + lib.PMTMediaId + "\",\"" + lib.ISCICode + "\",\"" + lib.Title + "\",\"" + lib.ProductDescription + "\",\"" + advertiser.AdvertiserName + "\",\"" + agency.AgencyName + "\",\"" + lib.MediaLength.Replace("00:60","01:00").Replace("00:90","01:30") + "\"";
                    tw.WriteLine(line);
                }
                tw.Flush();
                byte[] bytes = ms.ToArray();
                ms.Close();

                Response.Clear();
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment;    filename=libraryreport.csv");
                Response.BinaryWrite(bytes);
                Response.End();
            }
            else if (ddlReport.SelectedValue == "3" || (Request.QueryString["rid"] != null && Request.QueryString["rid"] == "3"))
            {
                MemoryStream ms = new MemoryStream();
                TextWriter tw = new StreamWriter(ms);
                tw.WriteLine("Campaign Id,Campaign date,Agency,Advertiser,PMT Media ID,TV Station Name,TV station Call Letters,TV Station Address,Delivery Method,Tracking Id,Date Signed,Time Signed,ISCI,Title,Description");

                List<LibraryItemInfo> libs = aCont.getLibs(PortalId);
                List<WorkOrderInfo> wos = aCont.Get_WorkOrdersByPortalId(PortalId);
                List<WorkOrderInfo> wosByAdvertiser = new List<WorkOrderInfo>();
                List<WorkOrderInfo> wosByAgency = new List<WorkOrderInfo>();
                List<WorkOrderInfo> wosByKeyword = new List<WorkOrderInfo>();
                List<AdvertiserInfo> userAds = new List<AdvertiserInfo>();
                List<AgencyInfo> userAgs = new List<AgencyInfo>();
                List<StationInfo> stations = aCont.Get_StationsByPortalId(PortalId);
                //int userCase = getUserCase();
                //bool canSeeAll = false;
                //if (userCase == 0)
                //{
                //    canSeeAll = true;
                //}
                //else
                //{
                //    userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
                //    userAgs = aCont.Get_AgenciesByUser(UserId);
                //}
                //string selAg = "";
                //string selAd = "";
                //if (ViewState["selAgency"] != null)
                //{
                //    selAg = ViewState["selAgency"].ToString();
                //}
                //if (ViewState["selAdvertiser"] != null)
                //{
                //    selAd = ViewState["selAdvertiser"].ToString();
                //}
                //if(Request.QueryString["adid"]!=null)
                //{
                //    selAd = Request.QueryString["adid"].ToString();
                //}
                //if(Request.QueryString["agid"]!=null)
                //{
                //    selAg = Request.QueryString["agid"].ToString();
                //}
                //if (selAd != "" && selAd != "-1")
                //{
                //    foreach (WorkOrderInfo wo in wos)
                //    {
                //        if (wo.AdvertiserId.ToString() == selAd)
                //        {
                //            wosByAdvertiser.Add(wo);
                //        }
                //    }
                //}
                //else
                //{
                //    wosByAdvertiser = wos;
                //}
                //if (selAg != "" && selAg != "-1")
                //{
                //    foreach (WorkOrderInfo wo in wosByAdvertiser)
                //    {
                //        if (wo.AgencyId.ToString() == selAg)
                //        {
                //            wosByAgency.Add(wo);
                //        }
                //    }
                //}
                //else
                //{
                //    wosByAgency = wosByAdvertiser;
                //}
                //if (txtMasterItemSearch.Text != "")
                //{
                //    foreach (WorkOrderInfo wo in wosByAgency)
                //    {
                //        if (wo.Description.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                //            wo.PONumber.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                //            wo.Notes.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                //            wo.InvoiceNumber.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1)
                //        {
                //            wosByKeyword.Add(wo);
                //        }
                //    }
                //}
                //else
                //{
                //    wosByKeyword = wosByAgency;
                //}
                //DateTime startDate = DateTime.Now;
                //DateTime endDate = DateTime.Now;
                //DateTime tempdate = DateTime.Now;
                //try
                //{
                //    tempdate = Convert.ToDateTime(txtStartDate.Text);
                //    startDate = new DateTime(tempdate.Year, tempdate.Month, tempdate.Day, 0, 0, 0);
                //}
                //catch { }
                //try
                //{
                //    tempdate = Convert.ToDateTime(txtEndDate.Text);
                //    endDate = new DateTime(tempdate.Year, tempdate.Month, tempdate.Day, 23, 59, 59);
                //}
                //catch { }
                //foreach (WorkOrderInfo wo in wosByKeyword)
                //{
                //    if (startDate <= wo.DateCreated && wo.DateCreated <= endDate)
                //    {
                //        bool showThis = canSeeAll;
                //        foreach (AdvertiserInfo ad in userAds)
                //        {
                //            if (ad.Id == wo.AdvertiserId)
                //            {
                //                showThis = true;
                //                break;
                //            }
                //        }
                //        if(UserId == -1)
                //        {
                //            showThis = true;
                //        }
                //        if (!showThis)
                //        {
                //            foreach (AgencyInfo ag in userAgs)
                //            {
                //                if (ag.Id == wo.AgencyId)
                //                {
                //                    showThis = true;
                //                    break;
                //                }
                //            }
                //        }
                //        if (showThis)
                //        {
                List<TaskInfo> tasks = getReportTasks(); //aCont.Get_TasksByWOId(wo.Id);)
                foreach (TaskInfo task in tasks)
                {
                    if (task.TaskType == GroupTypeEnum.Bundle || task.TaskType == GroupTypeEnum.Delivery)
                    {
                        var wo = wos.FirstOrDefault(o => o.Id == task.WorkOrderId);
                        if (wo != null)
                        {
                            if (wo.Status != "CANCELLED")
                            {
                                var lib = libs.FirstOrDefault(o => o.Id == task.LibraryId);
                                WOGroupStationInfo groupStation = aCont.Get_WorkOrderGroupStationById(task.StationId);
                                var station = stations.FirstOrDefault(o => o.Id == groupStation.StationId);
                                if (station == null) { station = new StationInfo(); }
                                if (lib != null)
                                {
                                    if (wo.Description.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                        wo.PONumber.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                        wo.Notes.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                        wo.InvoiceNumber.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                        lib.ISCICode.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                        lib.Title.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                        lib.ProductDescription.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                        station.CallLetter.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                                        station.StationName.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1)
                                    {
                                        //if (task.DeliveryStatus.ToLower() == "complete")
                                        //{
                                        string line = "\"" + wo.Id.ToString() + "\",\"" + wo.DateCreated.ToShortDateString() + "\",\"";
                                        var ag = agencies.FirstOrDefault(o => o.Id == wo.AgencyId);
                                        if (ag != null)
                                            line += ag.AgencyName + "\",\"";
                                        else
                                            line += "\",\"";
                                        var ad = ads.FirstOrDefault(o => o.Id == wo.AdvertiserId);
                                        if (ad != null)
                                            line += ad.AdvertiserName + "\",\"";
                                        else
                                            line += "\",\"";
                                        //var lib = libs.FirstOrDefault(o => o.Id == task.LibraryId);
                                        if (lib != null)
                                        { line += lib.PMTMediaId + "\",\""; }
                                        else
                                            line += "\",\"";
                                        //WOGroupStationInfo groupStation = aCont.Get_WorkOrderGroupStationById(task.StationId);
                                        //var station = stations.FirstOrDefault(o => o.Id == groupStation.StationId);
                                        if (station != null)
                                        {
                                            line += station.StationName + "\",\"" + station.CallLetter + "\",\"" + station.Address1 + " " + station.Address2 + "," + station.City + ", " + station.State + " " + station.Zip + ", " + station.Country + "\",\"";
                                        }
                                        else
                                            line += "\",\"\",\"\",\"";
                                        line += task.DeliveryMethod + "\",\"" + task.DeliveryOrderId + "\",\"";
                                        //try { line += task.DeliveryOrderDateComplete.ToShortDateString() + "\",\"" + task.DeliveryOrderDateComplete.ToShortTimeString() + "\",\""; }
                                        try { line += task.DateCreated.ToShortDateString() + "\",\"" + task.DateCreated.ToShortTimeString() + "\",\""; }
                                        catch { line += "\",\"\",\""; }
                                        //line += task.DeliveryStatus + "\",\"";
                                        if (lib != null)
                                        {
                                            line += lib.ISCICode + "\",\"" + lib.Title + "\",\"" + lib.ProductDescription + "\"";
                                        }
                                        else
                                            line += "\",\"\",\"";
                                        tw.WriteLine(line);
                                        //}
                                    }
                                }
                            }
                        }
                    }
                }
                tw.Flush();
                byte[] bytes = ms.ToArray();
                ms.Close();

                Response.Clear();
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment;    filename=deliveryreport.csv");
                Response.BinaryWrite(bytes);
                Response.End();
            }
            HttpContext.Current.Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void btnTestJavelin_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            lblTestJavelin.Text = aCont.AddJavelinOrder(17, PortalId);
            //lblTestJavelin.Text = aCont.GetJavelinOrderStatus(1002);
            //lblTestJavelin.Text = aCont.DeleteJavelinOrder(1002);
        }

        protected void btnJavelinStatus_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            //lblTestJavelin.Text = aCont.AddJavelinOrder(1002);
            lblTestJavelin.Text = aCont.GetJavelinOrderStatus(17);
            //lblTestJavelin.Text = aCont.DeleteJavelinOrder(1002);
        }

        protected void btnJavelinDelete_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            //lblTestJavelin.Text = aCont.AddJavelinOrder(1002);
            //lblTestJavelin.Text = aCont.GetJavelinOrderStatus(1002);
            lblTestJavelin.Text = aCont.DeleteJavelinOrder(17, PortalId);
        }

        protected void btnTestQB_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            List<QBCodeInfo> codes = aCont.FindQBCodesByTask(2009, PortalId);
            lblTestJavelin.Text = codes.Count().ToString() + " codes found.";
        }

        protected void txtStartDate_TextChanged(object sender, EventArgs e)
        {
            fillDeliveries();
        }

        protected void txtEndDate_TextChanged(object sender, EventArgs e)
        {
            fillDeliveries();
        }

        protected void btnGetComcastAuthToken_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            TaskInfo task = aCont.Get_TaskById(Convert.ToInt32(txtMasterItemSearch.Text));
            //TaskInfo task = new TaskInfo();
            //task.DeliveryOrderId = txtMasterItemSearch.Text;
            string token = aCont.addOTSMOrder(task);
            //string token = aCont.getOTSMToken();
            lblComcastMessage.Text = token;

            //string status = aCont.getComcastSpotStatus(txtMasterItemSearch.Text, token);
            //if (status == "")
            //{
            //    lblComcastMessage.Text = aCont.createComcastSpot(txtMasterItemSearch.Text, "Test Title", "", "", token); //aCont.getComcastSpotStatus(txtMasterItemSearch.Text); //aCont.getComcastAuthToken();
            //}
            //else { lblComcastMessage.Text = "Spot Already Exists."; }
        }
    }
    
}