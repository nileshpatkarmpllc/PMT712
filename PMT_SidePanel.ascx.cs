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
    public partial class PMT_SidePanel : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblDisplaName.Text = UserInfo.DisplayName;
            fillMasters();
            fillWorkOrders();
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
        private void fillMasters()
        {
            List<MasterItemInfo> masters = getMastersByCriteria();
            gvMasterItem.DataSource = masters;
            gvMasterItem.DataBind();
        }
        private void fillWorkOrders()
        {
            List<WorkOrderInfo> wos = getWorkOrdersByCriteria();
            gvWorkOrders.DataSource = wos;
            gvWorkOrders.DataBind();
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
        private List<MasterItemInfo> getMastersByCriteria()
        {
            bool canSeeAll = false;
            int userCase = getUserCase();
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
            
            if (canSeeAll)
            {
                mstrsByUser = MasterItems;
            }
            else
            {
                foreach (MasterItemInfo master in MasterItems)
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
            List<MasterItemInfo> sortedList = mstrsByUser.Distinct(new MasterItemComparer()).OrderByDescending(o => o.DateCreated).ToList();
            return sortedList;
        }
        private List<WorkOrderInfo> getWorkOrdersByCriteria()
        {
            bool canSeeAll = false;
            int userCase = getUserCase();
            if (userCase == 0)
            {
                canSeeAll = true;
            }
            AdminController aCont = new AdminController();
            List<WorkOrderInfo> workorders = aCont.Get_WorkOrdersByPortalId(PortalId);
            List<WorkOrderInfo> wosByUser = new List<WorkOrderInfo>();
            List<AgencyInfo> agencies = aCont.Get_AgenciesByUser(UserId);
            List<AdvertiserInfo> advertisers = aCont.Get_AdvertisersByPortalId(PortalId);
            List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
            if (canSeeAll)
            {
                wosByUser = workorders;
            }
            else
            {
                foreach(WorkOrderInfo wo in workorders)
                {
                    foreach(AdvertiserInfo ad in userAds)
                    {
                        if(ad.Id==wo.AdvertiserId)
                        {
                            wosByUser.Add(wo);
                        }
                    }
                    foreach(AgencyInfo ag in agencies)
                    {
                        if(ag.Id==wo.AgencyId)
                        {
                            wosByUser.Add(wo);
                        }
                    }
                }
            }
            wosByUser = wosByUser.Distinct(new WorkOrderComparer()).OrderByDescending(o => o.DateCreated).ToList();
            foreach(WorkOrderInfo wo in wosByUser)
            {
                var advertiser = advertisers.FirstOrDefault(i => i.Id == wo.AdvertiserId);
                if (advertiser != null)
                {
                    wo.AdvertiserName = advertiser.AdvertiserName;
                }
                if (wo.Status == "")
                {
                    wo.Status = "NEW";
                }
            }
            return wosByUser;
        }
        protected void gvMasterItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            int userCase = getUserCase();
            if (userCase == 0)
            {
                Response.Redirect("/Master-Items.aspx?miid=" + gvMasterItem.SelectedDataKey.Value.ToString());
            }
            else
            {
                AdminController aCont = new AdminController();
                MasterItemInfo master = aCont.Get_MasterItemById(Convert.ToInt32(gvMasterItem.SelectedDataKey.Value.ToString()));
                Response.Redirect("/Reports/miid/" + master.PMTMediaId);
            }
        }

        protected void gvWorkOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect("/Work-Orders/woid/" + gvWorkOrders.SelectedDataKey.Value.ToString());
        }
    }
}