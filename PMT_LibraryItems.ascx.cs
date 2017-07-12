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
    public partial class PMT_LibraryItems : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            int lid = -1;
            if (Request.QueryString["lid"] != null)
            {
                lid = Convert.ToInt32(Request.QueryString["lid"]);
                txtLibraryItemSearch.Text = Request.QueryString["lid"].ToString();
            }
            //fillLibraryItemAgencies();
            FillAdvertiserList();
            FillAdvertiserListProp();
            FillAgencyList();
            FillAgencyListProp();
            FillLibraryItemList();

            if (lid != -1)
            {
                try
                {
                    LoadLibraryItem(lid);
                }
                catch { }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
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
        #region LibraryItem Methods
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

            ddlLibraryItemAdvertiserSearch.Items.Clear();
            ddlLibraryItemAdvertiserSearch.Items.Add(new ListItem("--Select Advertiser--", "-1"));
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
                        ddlLibraryItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    }
                }
                else
                {
                    List<AdvertiserInfo> sortedList = Advertisers.Distinct(new AdvertiserComparer()).OrderBy(o => o.AdvertiserName).ToList();
                    foreach (AdvertiserInfo adv in sortedList)
                    {
                        ddlLibraryItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
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
                        ddlLibraryItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
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
                            ddlLibraryItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                    else
                    {
                        foreach (AdvertiserInfo adv in sortedList)
                        {
                            ddlLibraryItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
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
                            ddlLibraryItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                    else
                    {
                        foreach (AdvertiserInfo adv in selAds)
                        {
                            ddlLibraryItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                }
            }
            if (ddlLibraryItemAdvertiserSearch.Items.Count == 2)
            {
                ddlLibraryItemAdvertiserSearch.SelectedIndex = 1;
            }
            else if (selAd != "")
            {
                try
                {
                    ddlLibraryItemAdvertiserSearch.SelectedValue = selAd;
                }
                catch { }
            }
        }
        private void FillAgencyList()
        {
            ddlLibraryItemAgencySearch.Items.Clear();
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
            ddlLibraryItemAgencySearch.Items.Add(new ListItem("--Select Agency--", "-1"));
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
                        ddlLibraryItemAgencySearch.Items.Add(li);
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
                        ddlLibraryItemAgencySearch.Items.Add(li);
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
                        ddlLibraryItemAgencySearch.Items.Add(li);
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
                            ddlLibraryItemAgencySearch.Items.Add(li);
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
                                    ddlLibraryItemAgencySearch.Items.Add(li);
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
                            ddlLibraryItemAgencySearch.Items.Add(li);
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
                                    ddlLibraryItemAgencySearch.Items.Add(li);
                                }
                            }
                        }
                    }
                }
            }
            if (ddlLibraryItemAgencySearch.Items.Count == 2)
            {
                ddlLibraryItemAgencySearch.SelectedIndex = 1;
            }
            else if (selAg != "")
            {
                try
                {
                    ddlLibraryItemAgencySearch.SelectedValue = selAg;
                }
                catch { }
            }
        }
        private void FillAdvertiserListProp()
        {
            string selAg = "";
            string selAd = "";
            if (ViewState["selAgencyProp"] != null)
            {
                selAg = ViewState["selAgencyProp"].ToString();
            }
            if (ViewState["selAdvertiserProp"] != null)
            {
                selAd = ViewState["selAdvertiserProp"].ToString();
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

            ddlLibraryItemAdvertiser.Items.Clear();
            ddlLibraryItemAdvertiser.Items.Add(new ListItem("--Select Advertiser--", "-1"));
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
                        ddlLibraryItemAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    }
                }
                else
                {
                    List<AdvertiserInfo> sortedList = Advertisers.Distinct(new AdvertiserComparer()).OrderBy(o => o.AdvertiserName).ToList();
                    foreach (AdvertiserInfo adv in sortedList)
                    {
                        ddlLibraryItemAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
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
                        ddlLibraryItemAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
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
                            ddlLibraryItemAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                    else
                    {
                        foreach (AdvertiserInfo adv in sortedList)
                        {
                            ddlLibraryItemAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
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
                            ddlLibraryItemAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                    else
                    {
                        foreach (AdvertiserInfo adv in selAds)
                        {
                            ddlLibraryItemAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                }
            }
            if (ddlLibraryItemAdvertiser.Items.Count == 2)
            {
                ddlLibraryItemAdvertiser.SelectedIndex = 1;
            }
            else if (selAd != "")
            {
                try
                {
                    ddlLibraryItemAdvertiser.SelectedValue = selAd;
                }
                catch { }
            }
        }
        private void FillAgencyListProp()
        {
            ddlLibraryItemAgency.Items.Clear();
            string selAd = ""; //was sel
            if (ViewState["selAdvertiserProp"] != null)
            {
                selAd = ViewState["selAdvertiserProp"].ToString();
            }
            string selAg = "";
            if (ViewState["selAgencyProp"] != null)
            {
                selAg = ViewState["selAgencyProp"].ToString();
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
            ddlLibraryItemAgency.Items.Add(new ListItem("--Select Agency--", "-1"));
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
                        ddlLibraryItemAgency.Items.Add(li);
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
                        ddlLibraryItemAgency.Items.Add(li);
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
                        ddlLibraryItemAgency.Items.Add(li);
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
                            ddlLibraryItemAgency.Items.Add(li);
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
                                    ddlLibraryItemAgency.Items.Add(li);
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
                            ddlLibraryItemAgency.Items.Add(li);
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
                                    ddlLibraryItemAgency.Items.Add(li);
                                }
                            }
                        }
                    }
                }
            }
            if (ddlLibraryItemAgency.Items.Count == 2)
            {
                ddlLibraryItemAgency.SelectedIndex = 1;
            }
            else if (selAg != "")
            {
                try
                {
                    ddlLibraryItemAgency.SelectedValue = selAg;
                }
                catch { }
            }
        }
        private void FillAgencyListOld()
        {
            AdminController aCont = new AdminController();
            List<AgencyInfo> agencies = aCont.getAgencies(PortalId); //aCont.Get_AgenciesByPortalId(PortalId);
            ddlLibraryItemAgency.Items.Clear();
            ddlLibraryItemAgencySearch.Items.Clear();
            ddlLibraryItemAgencySearch.Items.Add(new ListItem("--Select Agency--", "-1"));
            ddlLibraryItemAgency.Items.Add(new ListItem("--Select Agency--", "-1"));
            List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            List<AgencyInfo> liAgencies = agencies;
            if (ddlLibraryItemAdvertiser.SelectedIndex > 0)
            {
                liAgencies = aCont.Get_AgenciesByAdvertiserId(Convert.ToInt32(ddlLibraryItemAdvertiser.SelectedValue));
            }
            else
            {
                if (isAdmin)
                {
                    liAgencies = agencies;
                }
                else
                {
                    liAgencies = userAgs;
                }
            }
            List<AgencyInfo> searchAgencies = agencies;
            if(ddlLibraryItemAdvertiserSearch.SelectedIndex>0 && !isAdmin)
            {
                searchAgencies = aCont.Get_AgenciesByAdvertiserId(Convert.ToInt32(ddlLibraryItemAdvertiserSearch.SelectedValue));
            }
            else
            {
                if (isAdmin)
                {
                    searchAgencies = agencies;
                }
                else
                {
                    searchAgencies = userAgs;
                }
            }
            List<AgencyInfo> SortedList = liAgencies.OrderBy(o => o.AgencyName).ToList();
            foreach (AgencyInfo agency in SortedList)
            {
                if (isAdmin)
                {
                    ddlLibraryItemAgency.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                }
                else
                {
                    foreach (AgencyInfo ag in userAgs)
                    {
                        if (ag.Id == agency.Id)
                        {
                            ddlLibraryItemAgency.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                        }
                    }
                }
            }
            SortedList = searchAgencies.OrderBy(o => o.AgencyName).ToList();
            foreach (AgencyInfo agency in SortedList)
            {
                if (isAdmin)
                {
                    ddlLibraryItemAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                }
                else
                {
                    foreach (AgencyInfo ag in userAgs)
                    {
                        if (ag.Id == agency.Id)
                        {
                            ddlLibraryItemAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                        }
                    }
                }
            }
        }
        private void FillAdvertiserListOld()
        {
            AdminController aCont = new AdminController();
            List<AdvertiserInfo> Advertisers = aCont.getAdvertisers(PortalId); //aCont.Get_AdvertisersByPortalId(PortalId);

            ddlLibraryItemAdvertiser.Items.Clear();
            ddlLibraryItemAdvertiser.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            ddlLibraryItemAdvertiserSearch.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            List<AdvertiserInfo> SortedList = Advertisers.OrderBy(o => o.AdvertiserName).ToList();
            foreach (AdvertiserInfo adv in SortedList)
            {
                ddlLibraryItemAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                ddlLibraryItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
            }
            //fillLibraryItemAgencies();
            //FillAgencyList();
        }
        protected void btnLibraryItemSearch_Click(object sender, EventArgs e)
        {
            FillLibraryItemList();
        }
        protected void btnSaveLibraryItem_Click(object sender, System.EventArgs e)
        {
            if (Application["LibraryItems"] != null)
            {
                Application.Remove("LibraryItems");
            }
            if (txtSelectedLibraryItem.Value == "-1")
            {
                lblLibraryItemMessage.Text = "You must create a Library Item from a Master Item or another Library Item.";
            }
            else
            {
                AdminController aCont = new AdminController();
                LibraryItemInfo isciCheck = aCont.Get_LibraryItemByISCI(txtISCICode.Text);
                if (isciCheck.Id == -1 || isciCheck.Id == Convert.ToInt32(txtSelectedLibraryItem.Value))
                {
                    LibraryItemInfo LibraryItem = new LibraryItemInfo();
                    LibraryItem.PortalId = PortalId;
                    LibraryItem.Filename = txtLibraryItemFile.Text;
                    LibraryItem.AdvertiserId = Convert.ToInt32(ddlLibraryItemAdvertiser.SelectedValue);
                    LibraryItem.Title = txtLibraryItemTitle.Text;
                    LibraryItem.MediaType = ddlLibraryItemMediaType.SelectedItem.Text;
                    LibraryItem.Encode = ddlLibraryItemEncode.SelectedItem.Text;
                    LibraryItem.Standard = ddlLibraryItemStandard.SelectedItem.Text;
                    LibraryItem.MediaLength = aCont.fixLength(txtLibraryItemLength.Text);
                    LibraryItem.AgencyId = Convert.ToInt32(ddlLibraryItemAgency.SelectedValue);
                    LibraryItem.PMTMediaId = txtLibraryItemPMTMediaId.Text;
                    string desc = txtProductDescription.Text;
                    desc = desc.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
                    if (desc.IndexOf(".") == -1)
                    {
                        if (desc.Length == 11)
                        {
                            desc = desc.Substring(1, 10);
                        }
                        if (desc.Length == 10)
                        {
                            desc = "1-" + desc.Substring(0, 3) + "-" + desc.Substring(3, 3) + "-" + desc.Substring(6, 4);
                        }
                    }
                    LibraryItem.ProductDescription = desc;
                    LibraryItem.ISCICode = txtISCICode.Text;
                    try
                    {
                        LibraryItem.Reel = Convert.ToInt32(txtLibraryItemReel.Text);
                    }
                    catch { lblLibraryItemMessage.Text += "Reel # must be an integer. "; }
                    LibraryItem.TapeCode = txtLibraryItemTapeCode.Text;
                    try
                    {
                        LibraryItem.Position = Convert.ToInt32(txtLibraryItemPostition.Text);
                    }
                    catch { lblLibraryItemMessage.Text += "Position # must be an integer. "; }
                    LibraryItem.VaultId = txtLibraryItemVaultId.Text;
                    LibraryItem.Location = txtLibraryItemLocation.Text;
                    LibraryItem.Comment = txtLibraryItemComment.Text;
                    if (chkLibraryItemClosedCaption.Checked)
                    {
                        LibraryItem.ClosedCaptioned = "Yes";
                    }
                    else
                    {
                        LibraryItem.ClosedCaptioned = "No";
                    }
                    LibraryItem.LastModifiedById = UserId;
                    LibraryItem.LastModifiedDate = DateTime.Now;
                    int LibraryItemId = -1;
                    if (txtSelectedLibraryItem.Value == "-1")
                    {
                        //save new LibraryItem
                        LibraryItem.CreatedById = UserId;
                        LibraryItem.DateCreated = DateTime.Now;
                        LibraryItemId = aCont.Add_LibraryItem(LibraryItem);
                        txtSelectedLibraryItem.Value = LibraryItemId.ToString();
                        txtLibraryItemCreatedBy.Value = UserId.ToString();
                        txtLibraryItemCreatedDate.Value = DateTime.Now.Ticks.ToString();
                        lblLibraryItemMessage.Text = "Library Item Saved.";
                        btnDeleteLibraryItem.Enabled = true;
                        btnSaveLibraryItemAs.Enabled = true;
                        //btnManageStationsInGroup.Enabled = true;
                    }
                    else
                    {
                        //update existing LibraryItem
                        LibraryItemId = Convert.ToInt32(txtSelectedLibraryItem.Value);
                        LibraryItem.CreatedById = Convert.ToInt32(txtLibraryItemCreatedBy.Value);
                        LibraryItem.DateCreated = new DateTime(Convert.ToInt64(txtLibraryItemCreatedDate.Value));
                        LibraryItem.Id = Convert.ToInt32(txtSelectedLibraryItem.Value);
                        aCont.Update_LibraryItem(LibraryItem);
                        lblLibraryItemMessage.Text = "LibraryItem Updated.";
                        //btnManageStationsInGroup.Enabled = true;
                    }
                    FillLibraryItemList();
                }
                else
                {
                    lblLibraryItemMessage.Text = "That ISCI already exists in the system.";
                }
                //fillDropDowns("");
            }
        }

        protected void btnSaveLibraryItemAs_Click(object sender, System.EventArgs e)
        {
            if (Application["LibraryItems"] != null)
            {
                Application.Remove("LibraryItems");
            }
            AdminController aCont = new AdminController();
            LibraryItemInfo isciCheck = aCont.Get_LibraryItemByISCI(txtISCICode.Text);
            if (isciCheck.Id == -1)
            {
                LibraryItemInfo LibraryItem = new LibraryItemInfo();
                LibraryItem.PortalId = PortalId;
                LibraryItem.Filename = txtLibraryItemFile.Text;
                LibraryItem.AdvertiserId = Convert.ToInt32(ddlLibraryItemAdvertiser.SelectedValue);
                LibraryItem.Title = txtLibraryItemTitle.Text;
                LibraryItem.MediaType = ddlLibraryItemMediaType.SelectedItem.Text;
                LibraryItem.Encode = ddlLibraryItemEncode.SelectedItem.Text;
                LibraryItem.Standard = ddlLibraryItemStandard.SelectedItem.Text;
                LibraryItem.MediaLength = aCont.fixLength(txtLibraryItemLength.Text);
                LibraryItem.AgencyId = Convert.ToInt32(ddlLibraryItemAgency.SelectedValue);
                LibraryItem.PMTMediaId = txtLibraryItemPMTMediaId.Text;
                LibraryItem.ISCICode = txtISCICode.Text;
                LibraryItem.ProductDescription = txtProductDescription.Text;
                if (chkLibraryItemClosedCaption.Checked)
                {
                    LibraryItem.ClosedCaptioned = "Yes";
                }
                else
                {
                    LibraryItem.ClosedCaptioned = "No";
                }
                try
                {
                    LibraryItem.Reel = Convert.ToInt32(txtLibraryItemReel.Text);
                }
                catch { lblLibraryItemMessage.Text += "Reel # must be an integer. "; }
                LibraryItem.TapeCode = txtLibraryItemTapeCode.Text;
                try
                {
                    LibraryItem.Position = Convert.ToInt32(txtLibraryItemPostition.Text);
                }
                catch { lblLibraryItemMessage.Text += "Position # must be an integer. "; }
                LibraryItem.VaultId = txtLibraryItemVaultId.Text;
                LibraryItem.Location = txtLibraryItemLocation.Text;
                LibraryItem.Comment = txtLibraryItemComment.Text;
                LibraryItem.LastModifiedById = UserId;
                LibraryItem.LastModifiedDate = DateTime.Now;

                //save new LibraryItem
                LibraryItem.CreatedById = UserId;
                LibraryItem.DateCreated = DateTime.Now;
                int LibraryItemId = aCont.Add_LibraryItem(LibraryItem);
                txtSelectedLibraryItem.Value = LibraryItemId.ToString();
                txtLibraryItemCreatedBy.Value = UserId.ToString();
                txtLibraryItemCreatedDate.Value = DateTime.Now.Ticks.ToString();
                lblLibraryItemMessage.Text = "Library Item Saved.";
                //btnManageStationsInGroup.Enabled = true;

                FillLibraryItemList();
            }
            else
            {
                lblLibraryItemMessage.Text = "That ISCI already exists in the system.";
            }
            //fillDropDowns("");
        }

        protected void btnDeleteLibraryItem_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int LibraryItemId = Convert.ToInt32(txtSelectedLibraryItem.Value);
            txtLibraryItemSearch.Text = "";
            LibraryItemInfo LibraryItem = new LibraryItemInfo();
            LibraryItem.Id = LibraryItemId;
            aCont.Delete_LibraryItem(LibraryItem);
            lblLibraryItemMessage.Text = "LibraryItem Deleted.";
            if (Application["LibraryItems"] != null)
            {
                //Application.Remove("LibraryItems");
                Application["LibraryItems"] = null;
            }
            FillLibraryItemList();
            clearLibraryItem();
            //fillDropDowns("");
        }

        protected void btnClearLibraryItem_Click(object sender, System.EventArgs e)
        {
            clearLibraryItem();
        }
        private void clearLibraryItem()
        {
            txtLibraryItemFile.Text = "";
            ddlLibraryItemAgency.Items.Clear();
            ddlLibraryItemAdvertiser.SelectedIndex = 0;
            txtLibraryItemTitle.Text = "";
            txtLibraryItemComment.Text = "";
            txtChecklistForm.Value = "";
            ddlLibraryItemMediaType.SelectedIndex = 0;
            txtLibraryItemPMTMediaId.Text = "";
            txtLibraryItemLength.Text = "";
            txtLibraryItemCustomerId.Text = "";
            txtLibraryItemTitle.Enabled = true;
            txtLibraryItemPMTMediaId.Text = "";
            txtLibraryItemPMTMediaId.Enabled = true;
            txtLibraryItemReel.Text = "";
            txtLibraryItemTapeCode.Text = "";
            txtLibraryItemPostition.Text = "";
            txtLibraryItemVaultId.Text = "";
            txtLibraryItemLocation.Text = "";
            chkLibraryItemClosedCaption.Checked = false;
            ddlLibraryItemStandard.SelectedIndex = 0;
            ddlLibraryItemEncode.SelectedIndex = 0;
            txtSelectedLibraryItem.Value = "-1";
            txtLibraryItemCreatedBy.Value = "";
            txtLibraryItemCreatedDate.Value = "";
            btnDeleteLibraryItem.Enabled = false;
            btnSaveLibraryItemAs.Enabled = false;
            ddlLibraryItemAdvertiserSearch.SelectedIndex = 0;
            ddlLibraryItemAgencySearch.SelectedIndex = 0;
            txtLibraryItemSearch.Text = "";
            lblLibraryItemMessage.Text = "";
            txtISCICode.Text = "";
            txtProductDescription.Text = "";
            //lblUserMessage.Text = "";
        }

        private void FillLibraryItemList()
        {
            AdminController aCont = new AdminController();
            bool canSeeAll = false;
            int userCase = getUserCase();
            int drawCase = getDrawCase();
            if (userCase == 0)
            {
                canSeeAll = true;
            }
            List<LibraryItemInfo> LibraryItems = new List<LibraryItemInfo>();
            List<LibraryItemInfo> mstrsByUser = new List<LibraryItemInfo>();
            LibraryItems = aCont.getLibs(PortalId); //aCont.Get_LibraryItemsByPortalId(PortalId);
            if (canSeeAll)
            {
                mstrsByUser = LibraryItems;
            }
            else
            {
                //TODO:Add specific user level view filtering
                //Should also filter ag and ad list by permissions
                List<AgencyInfo> agencies = aCont.Get_AgenciesByUser(UserId);
                List<AdvertiserInfo> advertisers = aCont.Get_AdvertisersByUser(UserId, PortalId);
                bool isAgencyRole = UserInfo.IsInRole("Agency");
                bool isAdvertiserRole = UserInfo.IsInRole("Advertiser");
                if (isAdvertiserRole && advertisers.Count > 0 && agencies.Count == 0)
                {
                    //in advertiser role and only tagged with advertisers.  Show all Librarys tagged with these advertisers
                    foreach (LibraryItemInfo Library in LibraryItems)
                    {
                        foreach (AdvertiserInfo ad in advertisers)
                        {
                            if (Library.AdvertiserId == ad.Id)
                            {
                                mstrsByUser.Add(Library);
                            }
                        }
                    }
                }
                else
                {
                    foreach(LibraryItemInfo Library in LibraryItems)
                    {
                        foreach(AgencyInfo ag in agencies)
                        {
                            if(ag.Id==Library.AgencyId)
                            {
                                mstrsByUser.Add(Library);
                            }
                        }
                    }
                }
                //else if (isAgencyRole && agencies.Count > 0 && advertisers.Count == 0)
                //{
                //    //in agency role and only tagged with agencies.  Show all Librarys tagged with these agencies
                //    foreach (LibraryItemInfo Library in LibraryItems)
                //    {
                //        foreach (AgencyInfo ag in agencies)
                //        {
                //            //foreach (AgencyInfo ag1 in Library.Agencies)
                //            //{
                //                if (ag.Id == Library.AgencyId)
                //                {
                //                    mstrsByUser.Add(Library);
                //                }
                //            //}
                //        }
                //    }
                //}
                //else if (isAgencyRole && !isAdvertiserRole && agencies.Count > 0 && advertisers.Count > 0)
                //{
                //    //in agency role, not advertiser role, and tagged with both.  Only show intersection
                //    foreach (LibraryItemInfo Library in LibraryItems)
                //    {
                //        foreach (AgencyInfo ag in agencies)
                //        {
                //            foreach (AgencyInfo ag1 in Library.Agencies)
                //            {
                //                foreach (AdvertiserInfo ad in advertisers)
                //                {
                //                    if (ag.Id == ag1.Id && ad.Id == Library.AdvertiserId)
                //                    {
                //                        mstrsByUser.Add(Library);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
            }

            List<LibraryItemInfo> mstrsbyAgency = new List<LibraryItemInfo>();
            List<LibraryItemInfo> mstrsbyAdvertiser = new List<LibraryItemInfo>();
            List<LibraryItemInfo> mstrsbyKeyword = new List<LibraryItemInfo>();
            List<LibraryItemInfo> mstrsbyStatus = new List<LibraryItemInfo>();
            if (ddlLibraryItemAdvertiserSearch.SelectedIndex > 0)
            {
                mstrsbyAdvertiser = mstrsByUser.Where(n => n.AdvertiserId.ToString() == ddlLibraryItemAdvertiserSearch.SelectedValue).ToList();
                //foreach (LibraryItemInfo LibraryItem in LibraryItems)
                //{
                //    if ((LibraryItem.AdvertiserId == Convert.ToInt32(ddlLibraryItemAdvertiserSearch.SelectedValue)))
                //    {
                //        mstrsbyAdvertiser.Add(LibraryItem);
                //    }
                //}
            }
            else
            {
                mstrsbyAdvertiser = mstrsByUser;
            }
            if (ddlLibraryItemAgencySearch.SelectedIndex > 0)
            {
                mstrsbyAgency = mstrsbyAdvertiser.Where(n => n.AgencyId.ToString() == ddlLibraryItemAgencySearch.SelectedValue).ToList();
                //foreach (LibraryItemInfo LibraryItem in mstrsbyAdvertiser)
                //{
                //    if (LibraryItem.AgencyId != -1)
                //    {
                //        if (LibraryItem.AgencyId.ToString() == ddlLibraryItemAgencySearch.SelectedValue)
                //        {
                //            mstrsbyAgency.Add(LibraryItem);
                //        }                        
                //    }
                //}
            }
            else
            {
                mstrsbyAgency = mstrsbyAdvertiser;
            }
            if (txtLibraryItemSearch.Text != "")
            {
                foreach (LibraryItemInfo LibraryItem in mstrsbyAgency)
                {
                    if (LibraryItem.PMTMediaId.ToString().ToLower().IndexOf(txtLibraryItemSearch.Text.ToLower()) != -1 ||
                        LibraryItem.Title.ToLower().IndexOf(txtLibraryItemSearch.Text.ToLower()) != -1 ||
                        LibraryItem.Filename.ToLower().IndexOf(txtLibraryItemSearch.Text.ToLower()) != -1 ||
                        LibraryItem.AgencyId.ToString().ToLower().IndexOf(txtLibraryItemSearch.Text.ToLower()) != -1 ||
                        LibraryItem.Id.ToString().ToLower().IndexOf(txtLibraryItemSearch.Text.ToLower()) != -1 ||
                        LibraryItem.ISCICode.ToString().ToLower().IndexOf(txtLibraryItemSearch.Text.ToLower()) != -1 ||
                        LibraryItem.ProductDescription.ToString().ToLower().IndexOf(txtLibraryItemSearch.Text.ToLower()) != -1 ||
                        LibraryItem.MediaLength.ToString().ToLower().IndexOf(txtLibraryItemSearch.Text.ToLower()) != -1)
                    {
                        mstrsbyKeyword.Add(LibraryItem);
                    }
                }
            }
            else
            {
                mstrsbyKeyword = mstrsbyAgency;
            }
            if (Session["LibraryItemPage"] != null)
            {
                gvLibraryItem.PageIndex = Convert.ToInt32(Session["LibraryItemPage"]);
            }
            List<LibraryItemInfo> SortedList = mstrsbyKeyword.OrderBy(o => o.Title).ToList();
            lblLibraryItemMessage.Text = mstrsbyKeyword.Count.ToString() + " Library Items found.";
            gvLibraryItem.DataSource = mstrsbyKeyword;
            gvLibraryItem.DataBind();
        }

        protected void gvLibraryItem_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillLibraryItemList();
            GridView gv = (GridView)sender;
            txtSelectedLibraryItem.Value = (gv.SelectedRow.FindControl("hdngvLibraryItemId") as HiddenField).Value;
            //btnLibraryChecklist.OnClientClick = "window.open('check-list.aspx?Library=" + txtSelectedLibraryItem.Value + "','_blank')";
            LoadLibraryItem(Convert.ToInt32(txtSelectedLibraryItem.Value));
            //clearAll("LibraryItems");
        }
        private void LoadLibraryItem(int Id)
        {
            AdminController aCont = new AdminController();
            LibraryItemInfo LibraryItem = aCont.Get_LibraryItemById(Id);
            txtSelectedLibraryItem.Value = Id.ToString();
            if (LibraryItem.Id != -1)
            {
                btnDeleteLibraryItem.Enabled = true;
                btnSaveLibraryItemAs.Enabled = true;
                txtLibraryItemFile.Text = LibraryItem.Filename;
                ddlLibraryItemAdvertiser.SelectedIndex = 0;
                foreach(ListItem li in ddlLibraryItemAdvertiser.Items)
                {
                    if(li.Value==LibraryItem.AdvertiserId.ToString())
                    {
                        li.Selected = true;
                    }
                    else
                    {
                        li.Selected = false;
                    }
                }
                ViewState["selAdvertiserProp"] = LibraryItem.AdvertiserId.ToString();
                ddlLibraryItemAgency.SelectedIndex = 0;
                FillAgencyListProp();
                foreach (ListItem li in ddlLibraryItemAgency.Items)
                {
                    if (li.Value == LibraryItem.AgencyId.ToString())
                    {
                        li.Selected = true;
                    }
                    else
                    {
                        li.Selected = false;
                    }
                }
                txtLibraryItemTitle.Text = LibraryItem.Title;
                txtLibraryItemComment.Text = LibraryItem.Comment;
                foreach(ListItem li in ddlLibraryItemMediaType.Items)
                {
                    if(li.Text.ToLower()==LibraryItem.MediaType.ToLower())
                    {
                        li.Selected = true;
                    }
                    else
                    {
                        li.Selected = false;
                    }
                }
                foreach(ListItem li in ddlLibraryItemEncode.Items)
                {
                    if(li.Text.ToLower() == LibraryItem.Encode.ToLower())
                    {
                        li.Selected = true;
                    }
                    else
                    {
                        li.Selected = false;
                    }
                }

                txtLibraryItemPMTMediaId.Text = LibraryItem.PMTMediaId;
                txtLibraryItemLength.Text = LibraryItem.MediaLength;
                txtLibraryItemCustomerId.Text = LibraryItem.AgencyId.ToString();
                txtLibraryItemTitle.Enabled = false;
                txtLibraryItemPMTMediaId.Text = LibraryItem.PMTMediaId;
                txtISCICode.Text = LibraryItem.ISCICode;
                txtLibraryItemPMTMediaId.Enabled = false;
                txtProductDescription.Text = LibraryItem.ProductDescription;
                if (LibraryItem.Reel > 0)
                {
                    txtLibraryItemReel.Text = LibraryItem.Reel.ToString();
                }
                txtLibraryItemTapeCode.Text = LibraryItem.TapeCode;
                if (LibraryItem.Position > 0)
                {
                    txtLibraryItemPostition.Text = LibraryItem.Position.ToString();
                }
                txtLibraryItemVaultId.Text = LibraryItem.VaultId;
                txtLibraryItemLocation.Text = LibraryItem.Location;
                if (LibraryItem.ClosedCaptioned.ToLower() == "yes" || LibraryItem.ClosedCaptioned.ToLower() == "true")
                {
                    chkLibraryItemClosedCaption.Checked = true;
                }
                else
                {
                    chkLibraryItemClosedCaption.Checked = false;
                }
                foreach(ListItem li in ddlLibraryItemStandard.Items)
                {
                    if(li.Text.ToLower()==LibraryItem.Standard.ToLower())
                    {
                        li.Selected = true;
                    }
                    else
                    {
                        li.Selected = false;
                    }
                }
                //ddlLibraryItemStandard.SelectedItem.Text = LibraryItem.Standard.ToString();
                
                //ddlLibraryItemEncode.SelectedItem.Text = LibraryItem.Encode.ToString();
                txtLibraryItemCreatedBy.Value = LibraryItem.CreatedById.ToString();
                txtLibraryItemCreatedDate.Value = LibraryItem.DateCreated.Ticks.ToString(); ;
                btnDeleteLibraryItem.Enabled = true;
                btnSaveLibraryItemAs.Enabled = true;
                lblLibraryItemMessage.Text = "";
            }
            else
            {
                btnDeleteLibraryItem.Enabled = false;
                btnSaveLibraryItemAs.Enabled = false;
                clearLibraryItem();
                lblLibraryItemMessage.Text = "There was an error loading this Library Item.";
            }
        }

        protected void gvLibraryItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["LibraryItemPage"] = e.NewPageIndex.ToString();
            //gvLibraryItem.PageIndex = e.NewPageIndex;
            //gvLibraryItem.DataBind();
            FillLibraryItemList();
        }
        protected void ddlLibraryItemAdvertiser_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["selAdvertiserProp"] = ddlLibraryItemAdvertiser.SelectedValue;
            FillAgencyListProp();
            //fillLibraryItemAgencies();
            //FillAgencyList();
        }
        private void fillLibraryItemAgencies()
        {
            if (ddlLibraryItemAdvertiser.SelectedIndex != 0)
            {
                ddlLibraryItemAgency.Items.Clear();
                AdminController aCont = new AdminController();
                List<AgencyInfo> ags = aCont.Get_AgenciesByAdvertiserId(Convert.ToInt32(ddlLibraryItemAdvertiser.SelectedValue));
                ddlLibraryItemAgency.Items.Add(new ListItem("--Select Agency--", "-1"));
                List<AgencyInfo> SortedList = ags.OrderBy(o => o.AgencyName).ToList();
                foreach(AgencyInfo ag in SortedList)
                {
                    ddlLibraryItemAgency.Items.Add(new ListItem(ag.AgencyName, ag.Id.ToString()));
                }
            }
        }
        protected void ddlLibraryItemAgencySearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["selAgency"] = ddlLibraryItemAgencySearch.SelectedValue;
            FillAdvertiserList();
            FillLibraryItemList();
        }
        #endregion

        protected void txtLibraryItemSearch_TextChanged(object sender, EventArgs e)
        {
            FillLibraryItemList();
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

        protected void ddlLibraryItemAdvertiserSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["selAdvertiser"] = ddlLibraryItemAdvertiserSearch.SelectedValue;
            FillAgencyList();
            FillLibraryItemList();
        }

        protected void ddlLibraryItemAgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["selAgencyProp"] = ddlLibraryItemAgency.SelectedValue;
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
                try {
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
                        //if (master.Filename != "")
                        //{
                        //    litVidSource.Text = "<source src=\"https://s3-pmt-bucket.s3.amazonaws.com/Viewers/" + master.Filename + "\" type=\"video/mp4\">";
                        //}
                        //else
                        //{
                            string phone = master.ProductDescription.Replace("-", "");
                            if (phone.Length == 11)
                            {
                                phone = phone.Substring(1, 10);
                            }
                            litVidSource.Text = "<source src=\"https://s3-pmt-bucket.s3.amazonaws.com/Viewers/" + phone + master.PMTMediaId + ".mp4\" type=\"video/mp4\">";
                        //}
                    }
                    mpeViewerPopup.Show();
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mpeViewerPopup.Hide();
            FillLibraryItemList();
        }
    }
}