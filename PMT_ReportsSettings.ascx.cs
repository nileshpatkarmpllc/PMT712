using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;

using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Security.Roles;

namespace Christoc.Modules.PMT_Admin
{
    public partial class PMT_ReportsSettings : PMT_AdminModuleSettingsBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            RoleController rCont = new RoleController();
            IList<RoleInfo> roles = rCont.GetRoles(PortalId);
            foreach (RoleInfo role in roles)
            {
                lbxAllView.Items.Add(new ListItem(role.RoleName, role.RoleID.ToString()));
            }
        }
        #region Base Method Implementations

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// LoadSettings loads the settings from the Database and displays them
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void LoadSettings()
        {
            try
            {
                if (Page.IsPostBack == false)
                {
                    if (Settings.Contains("AllViewRoles"))
                    {
                        string[] roles = Settings["AllViewRoles"].ToString().Split(',');
                        foreach (string role in roles)
                        {
                            foreach (ListItem li in lbxAllView.Items)
                            {
                                if (role == li.Value)
                                {
                                    li.Selected = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpdateSettings saves the modified settings to the Database
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void UpdateSettings()
        {
            try
            {
                var modules = new ModuleController();

                string allRoles = "";
                foreach (ListItem li in lbxAllView.Items)
                {
                    if (li.Selected)
                    {
                        allRoles += li.Value + ",";
                    }
                }
                allRoles = allRoles.Substring(0, allRoles.Length - 1);
                modules.UpdateTabModuleSetting(TabModuleId, "AllViewRoles", allRoles);
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion
    }
}