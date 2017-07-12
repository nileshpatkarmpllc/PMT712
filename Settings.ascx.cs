/*
' Copyright (c) 2015  Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

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
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Settings class manages Module Settings
    /// 
    /// Typically your settings control would be used to manage settings for your module.
    /// There are two types of settings, ModuleSettings, and TabModuleSettings.
    /// 
    /// ModuleSettings apply to all "copies" of a module on a site, no matter which page the module is on. 
    /// 
    /// TabModuleSettings apply only to the current module on the current page, if you copy that module to
    /// another page the settings are not transferred.
    /// 
    /// If you happen to save both TabModuleSettings and ModuleSettings, TabModuleSettings overrides ModuleSettings.
    /// 
    /// Below we have some examples of how to access these settings but you will need to uncomment to use.
    /// 
    /// Because the control inherits from PMT_AdminSettingsBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Settings : PMT_AdminModuleSettingsBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            RoleController rCont = new RoleController();
            IList<RoleInfo> roles = rCont.GetRoles(PortalId);
            foreach (RoleInfo role in roles)
            {
                lbxAgencyView.Items.Add(new ListItem(role.RoleName, role.RoleID.ToString()));
                lbxAdvertiserView.Items.Add(new ListItem(role.RoleName, role.RoleID.ToString()));
                lbxMarketView.Items.Add(new ListItem(role.RoleName, role.RoleID.ToString()));
                lbxStationView.Items.Add(new ListItem(role.RoleName, role.RoleID.ToString()));
                lbxStationGroupView.Items.Add(new ListItem(role.RoleName, role.RoleID.ToString()));
                lbxLabelView.Items.Add(new ListItem(role.RoleName, role.RoleID.ToString()));
                lbxMasterItemsView.Items.Add(new ListItem(role.RoleName, role.RoleID.ToString()));
                lbxUsersView.Items.Add(new ListItem(role.RoleName, role.RoleID.ToString()));
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
                    if(Settings.Contains("AgencyViewRoles"))
                    {
                        string[] roles = Settings["AgencyViewRoles"].ToString().Split(',');
                        foreach(string role in roles)
                        {
                            foreach(ListItem li in lbxAgencyView.Items)
                            {
                                if(role==li.Value)
                                {
                                    li.Selected = true;
                                }
                            }
                        }
                    }
                    if (Settings.Contains("AdvertiserViewRoles"))
                    {
                        string[] roles = Settings["AdvertiserViewRoles"].ToString().Split(',');
                        foreach (string role in roles)
                        {
                            foreach (ListItem li in lbxAdvertiserView.Items)
                            {
                                if (role == li.Value)
                                {
                                    li.Selected = true;
                                }
                            }
                        }
                    }
                    if (Settings.Contains("MarketViewRoles"))
                    {
                        string[] roles = Settings["MarketViewRoles"].ToString().Split(',');
                        foreach (string role in roles)
                        {
                            foreach (ListItem li in lbxMarketView.Items)
                            {
                                if (role == li.Value)
                                {
                                    li.Selected = true;
                                }
                            }
                        }
                    }
                    if (Settings.Contains("StationViewRoles"))
                    {
                        string[] roles = Settings["StationViewRoles"].ToString().Split(',');
                        foreach (string role in roles)
                        {
                            foreach (ListItem li in lbxStationView.Items)
                            {
                                if (role == li.Value)
                                {
                                    li.Selected = true;
                                }
                            }
                        }
                    }
                    if (Settings.Contains("StationGroupViewRoles"))
                    {
                        string[] roles = Settings["StationGroupViewRoles"].ToString().Split(',');
                        foreach (string role in roles)
                        {
                            foreach (ListItem li in lbxStationGroupView.Items)
                            {
                                if (role == li.Value)
                                {
                                    li.Selected = true;
                                }
                            }
                        }
                    }
                    if (Settings.Contains("LabelViewRoles"))
                    {
                        string[] roles = Settings["LabelViewRoles"].ToString().Split(',');
                        foreach (string role in roles)
                        {
                            foreach (ListItem li in lbxLabelView.Items)
                            {
                                if (role == li.Value)
                                {
                                    li.Selected = true;
                                }
                            }
                        }
                    } 
                    if (Settings.Contains("MasterItemsViewRoles"))
                    {
                        string[] roles = Settings["MasterItemsViewRoles"].ToString().Split(',');
                        foreach (string role in roles)
                        {
                            foreach (ListItem li in lbxMasterItemsView.Items)
                            {
                                if (role == li.Value)
                                {
                                    li.Selected = true;
                                }
                            }
                        }
                    }
                    if (Settings.Contains("UsersViewRoles"))
                    {
                        string[] roles = Settings["UsersViewRoles"].ToString().Split(',');
                        foreach (string role in roles)
                        {
                            foreach (ListItem li in lbxUsersView.Items)
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

                string agencyRoles = "";
                foreach(ListItem li in lbxAgencyView.Items)
                {
                    if(li.Selected)
                    {
                        agencyRoles += li.Value + ",";
                    }
                }
                agencyRoles = agencyRoles.Substring(0, agencyRoles.Length - 1);
                modules.UpdateTabModuleSetting(TabModuleId, "AgencyViewRoles", agencyRoles);
                string AdvertiserRoles = "";
                foreach (ListItem li in lbxAdvertiserView.Items)
                {
                    if (li.Selected)
                    {
                        AdvertiserRoles += li.Value + ",";
                    }
                }
                AdvertiserRoles = AdvertiserRoles.Substring(0, AdvertiserRoles.Length - 1);
                modules.UpdateTabModuleSetting(TabModuleId, "AdvertiserViewRoles", AdvertiserRoles);

                string MarketRoles = "";
                foreach (ListItem li in lbxMarketView.Items)
                {
                    if (li.Selected)
                    {
                        MarketRoles += li.Value + ",";
                    }
                }
                MarketRoles = MarketRoles.Substring(0, MarketRoles.Length - 1);
                modules.UpdateTabModuleSetting(TabModuleId, "MarketViewRoles", MarketRoles);
                string StationRoles = "";
                foreach (ListItem li in lbxStationView.Items)
                {
                    if (li.Selected)
                    {
                        StationRoles += li.Value + ",";
                    }
                }
                StationRoles = StationRoles.Substring(0, StationRoles.Length - 1);
                modules.UpdateTabModuleSetting(TabModuleId, "StationViewRoles", StationRoles);
                string StationGroupRoles = "";
                foreach (ListItem li in lbxStationGroupView.Items)
                {
                    if (li.Selected)
                    {
                        StationGroupRoles += li.Value + ",";
                    }
                }
                StationGroupRoles = StationGroupRoles.Substring(0, StationGroupRoles.Length - 1);
                modules.UpdateTabModuleSetting(TabModuleId, "StationGroupViewRoles", StationGroupRoles);
                string LabelRoles = "";
                foreach (ListItem li in lbxLabelView.Items)
                {
                    if (li.Selected)
                    {
                        LabelRoles += li.Value + ",";
                    }
                }
                LabelRoles = LabelRoles.Substring(0, LabelRoles.Length - 1);
                modules.UpdateTabModuleSetting(TabModuleId, "LabelViewRoles", LabelRoles);
                string MasterItemsRoles = "";
                foreach (ListItem li in lbxMasterItemsView.Items)
                {
                    if (li.Selected)
                    {
                        MasterItemsRoles += li.Value + ",";
                    }
                }
                MasterItemsRoles = MasterItemsRoles.Substring(0, MasterItemsRoles.Length - 1);
                modules.UpdateTabModuleSetting(TabModuleId, "MasterItemsViewRoles", MasterItemsRoles);
                string UsersRoles = "";
                foreach (ListItem li in lbxUsersView.Items)
                {
                    if (li.Selected)
                    {
                        UsersRoles += li.Value + ",";
                    }
                }
                UsersRoles = UsersRoles.Substring(0, UsersRoles.Length - 1);
                modules.UpdateTabModuleSetting(TabModuleId, "UsersViewRoles", UsersRoles);

                //the following are two sample Module Settings, using the text boxes that are commented out in the ASCX file.
                //module settings
                //modules.UpdateModuleSetting(ModuleId, "Setting1", txtSetting1.Text);
                //modules.UpdateModuleSetting(ModuleId, "Setting2", txtSetting2.Text);

                //tab module settings
                //modules.UpdateTabModuleSetting(TabModuleId, "Setting1",  txtSetting1.Text);
                //modules.UpdateTabModuleSetting(TabModuleId, "Setting2",  txtSetting2.Text);
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion
    }
}