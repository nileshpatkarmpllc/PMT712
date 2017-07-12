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
    public partial class PMT_Users : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            fillUsers();
            fillUserRoles();
            FillAgencyList();
            FillAdvertiserList();
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
        private void FillAgencyList()
        {
            AdminController aCont = new AdminController();
            List<AgencyInfo> agencies = aCont.Get_AgenciesByPortalId(PortalId);
            
            List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            foreach (AgencyInfo agency in agencies)
            {
                lbxUserAgencies.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));                
            }
        }
        private void FillAdvertiserList()
        {
            AdminController aCont = new AdminController();
            List<AdvertiserInfo> Advertisers = aCont.Get_AdvertisersByPortalId(PortalId);

            List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            foreach (AdvertiserInfo adv in Advertisers)
            {
                lbxUserAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));                
            }
        }
        #region Users
        private void fillUserRoles()
        {
            lbxUserRoles.Items.Clear();
            RoleController rCont = new RoleController();
            IList<RoleInfo> roles = rCont.GetRoles(PortalId);
            foreach (RoleInfo role in roles)
            {
                if (role.RoleName.ToLower() != "administrators")
                {
                    ListItem li = new ListItem(role.RoleName, role.RoleID.ToString());
                    lbxUserRoles.Items.Add(li);
                }
            }
        }
        private void checkUserRoles()
        {
            lbxUserRoles.ClearSelection();
            lbxUserAgencies.ClearSelection();
            lbxUserAdvertisers.ClearSelection();
            int thisUserId = Convert.ToInt32(ddlUsers.SelectedValue);
            AdminController aCont = new AdminController();
            List<AdvertiserInfo> ads = aCont.Get_AdvertisersByUser(thisUserId, PortalId);
            List<AgencyInfo> ags = aCont.Get_AgenciesByUser(thisUserId);
            UserInfo selectedUser = UserController.GetUserById(PortalId, thisUserId);
            foreach (ListItem li in lbxUserRoles.Items)
            {
                if (selectedUser != null && selectedUser.IsInRole(li.Text))
                {
                    li.Selected = true;
                }
            }
            if (ads.Count > 0)
            {
                foreach (AdvertiserInfo ad in ads)
                {
                    foreach (ListItem li in lbxUserAdvertisers.Items)
                    {

                        if (ad.Id.ToString() == li.Value)
                        {
                            li.Selected = true;
                        }
                    }
                }
            }
            if (ags.Count > 0)
            {
                foreach (AgencyInfo ag in ags)
                {
                    foreach (ListItem li in lbxUserAgencies.Items)
                    {

                        if (ag.Id.ToString() == li.Value)
                        {
                            li.Selected = true;
                        }
                    }
                }
            }
        }
        protected void ddlUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkUserRoles();
            if (ddlUsers.SelectedValue != "-1")
            {
                RoleController rCont = new RoleController();
                UserInfo selectedUser = UserController.GetUserById(PortalId, Convert.ToInt32(ddlUsers.SelectedValue));
                IList<RoleInfo> roles = rCont.GetRoles(PortalId);
                lblUserEmailValue.Text = selectedUser.Email;
                lblUserFirstNameValue.Text = selectedUser.FirstName;
                lblUserLastNameValue.Text = selectedUser.LastName;
                lblUserUsernameValue.Text = selectedUser.Username;
                lblUserDisplayNameValue.Text = selectedUser.DisplayName;

                AdminController aCont = new AdminController();
                int thisUserId = -1;
                List<AgencyInfo> ags = new List<AgencyInfo>();
                List<AdvertiserInfo> ads = new List<AdvertiserInfo>();

                if (ddlUsers.SelectedIndex > 0)
                {
                    thisUserId = Convert.ToInt32(ddlUsers.SelectedValue);
                    ags = aCont.Get_AgenciesByUser(thisUserId);
                    ads = aCont.Get_AdvertisersByUser(thisUserId, PortalId);
                }
            }
            else
            {
                lblUserDisplayNameValue.Text = "";
                lblUserFirstNameValue.Text = "";
                lblUserLastNameValue.Text = "";
                lblUserUsernameValue.Text = "";
                lblUserEmailValue.Text = "";
            }
        }

        private void fillUsers()
        {
            ddlUsers.Items.Clear();
            ddlUsers.Items.Add(new ListItem("--Select User--", "-1"));
            ArrayList users = DotNetNuke.Entities.Users.UserController.GetUsers(PortalId);

            foreach (object user in users)
            {
                UserInfo thisUser = (UserInfo)user;
                ListItem li = new ListItem(thisUser.DisplayName, thisUser.UserID.ToString());
                ddlUsers.Items.Add(li);
            }
        }
        protected void btnSaveUserPermissions_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            int thisUserId = -1;
            if (ddlUsers.SelectedIndex > 0)
            {
                //save roles
                RoleController rCont = new RoleController();
                IList<RoleInfo> roles = rCont.GetRoles(PortalId);
                thisUserId = Convert.ToInt32(ddlUsers.SelectedValue);
                UserInfo thisUser = UserController.GetUserById(PortalId, thisUserId);
                foreach (ListItem li in lbxUserRoles.Items)
                {
                    RoleInfo rInfo = rCont.GetRoleByName(PortalId, li.Text);
                    if (rInfo != null)
                    {
                        if (li.Selected)
                        {
                            rCont.AddUserRole(PortalId, thisUserId, rInfo.RoleID, DateTime.MinValue, DateTime.MaxValue);
                        }
                        else
                        {
                            rCont.DeleteUserRole(PortalId, thisUserId, rInfo.RoleID);
                        }
                    }
                }
                //save agencies
                //first delete all agencies for this user
                aCont.Delete_UserInAgencies(thisUserId);
                //now add them back in
                foreach (ListItem li in lbxUserAgencies.Items)
                {
                    if (li.Selected)
                    {
                        aCont.Add_UserInAgency(thisUserId, Convert.ToInt32(li.Value));
                    }
                }
                //save advertisers
                //first, delete all advertisers for this user
                aCont.Delete_UserInAdvertisers(thisUserId);
                //now add them back in
                foreach (ListItem li in lbxUserAdvertisers.Items)
                {
                    if (li.Selected)
                    {
                        aCont.Add_UserInAdvertiser(thisUserId, Convert.ToInt32(li.Value));
                    }
                }
                lblUserMessage.Text = "Permissions updated for this user.";
            }
        }
        #endregion
    }
}