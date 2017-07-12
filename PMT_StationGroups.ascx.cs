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
    public partial class PMT_StationGroups : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            FillStationGroupList();
            FillStationGroups();
            fillMarketParents();
            FillStations();
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
        #region StationGroup Methods
        protected void btnStationGroupStationSearch_Click(object sender, System.EventArgs e)
        {
            FillStations();
        }
        private void FillStations()
        {
            AdminController aCont = new AdminController();
            List<MarketInfo> markets = aCont.Get_MarketsByPortalId(PortalId);
            List<StationInfo> stations = aCont.Get_StationsByPortalId(PortalId);

            //for the station group stations
            List<int> marks = new List<int>();
            List<StationInfo> stationsInMarket = new List<StationInfo>();
            List<StationInfo> stationsFiltered = new List<StationInfo>();
            if (ddlStationGroupMarketSearch.SelectedIndex != 0)
            {
                int selMarket = Convert.ToInt32(ddlStationGroupMarketSearch.SelectedValue);
                recMarkets(ref markets, ref marks, selMarket);
                foreach (StationInfo station in stations)
                {
                    foreach (int j in marks)
                    {
                        if (j == station.MarketId)
                        {
                            stationsInMarket.Add(station);
                        }
                    }
                }
            }
            else
            {
                stationsInMarket = stations;
            }
            if (txtStationGroupStationSearch.Text != "")
            {
                foreach (StationInfo station in stationsInMarket)
                {
                    if (station.StationName.ToLower().IndexOf(txtStationGroupStationSearch.Text.ToLower()) != -1
                        || station.CallLetter.ToLower().IndexOf(txtStationGroupStationSearch.Text.ToLower()) != -1)
                    {
                        stationsFiltered.Add(station);
                    }
                }
            }
            else
            {
                stationsFiltered = stationsInMarket;
            }
            if (ViewState["StationGroupStationsPage"] != null)
            {
                gvStationGroupStations.PageIndex = Convert.ToInt32(ViewState["StationGroupStationsPage"]);
            }
            gvStationGroupStations.DataSource = stationsFiltered;
            gvStationGroupStations.DataBind();
            lblStationGroupStationsModalMessage.Text = stationsFiltered.Count.ToString() + " stations found.";
        }
        private void fillMarketParents()
        {
            //ddlMarketParent.Items.Clear();
            //ddlMarket2Parent.Items.Clear();
            ddlStationGroupMarketSearch.Items.Clear();
            AdminController aCont = new AdminController();
            List<MarketInfo> markets = aCont.Get_MarketsByPortalId(PortalId);
            //ddlMarketParent.Items.Add(new ListItem("--Select Parent if Appropriate--", "-1"));
            //ddlMarket2Parent.Items.Add(new ListItem("--Select Parent if Appropriate--", "-1"));
            ddlStationGroupMarketSearch.Items.Add(new ListItem("--Select Market--", "-1"));
            foreach (MarketInfo market in markets)
            {
                ddlStationGroupMarketSearch.Items.Add(new ListItem(market.MarketName, market.Id.ToString()));
                //if (market.Id.ToString() != txtSelectedMarket.Value)
                //{
                //    ddlMarketParent.Items.Add(new ListItem(market.MarketName, market.Id.ToString()));
                //}
                //if (market.Id.ToString() != txtSelectedMarket2.Value)
                //{
                //    ddlMarket2Parent.Items.Add(new ListItem(market.MarketName, market.Id.ToString()));
                //}
            }
            //try
            //{
            //    ddlMarketParent.SelectedValue = txtSelectedMarket.Value.ToString();
            //    ddlMarket2Parent.SelectedValue = txtSelectedMarket2.Value.ToString();
            //}
            //catch { }
        }   
        private void FillStationGroups()
        {
            AdminController aCont = new AdminController();
            //get station groupd for current user
            //may need to add all station groups for admins
            List<StationGroupInfo> stationGroups = aCont.Get_StationGroupsByUserId(UserId);
            gvStationGroup.DataSource = stationGroups;
            if (ViewState["StationGroupPage"] != null)
            {
                gvStationGroup.PageIndex = Convert.ToInt32(ViewState["StationGroupPage"]);
            }
            gvStationGroup.DataBind();
        }
        protected void btnSaveStationGroup_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            StationGroupInfo StationGroup = new StationGroupInfo();
            StationGroup.PortalId = PortalId;
            StationGroup.StationGroupName = txtStationGroupName.Text;
            StationGroup.Description = txtStationGroupDescription.Text;
            try
            {
                //StationGroup.AgencyId = Convert.ToInt32(ddlStationGroupAgency.SelectedValue);
            }
            catch { }
            StationGroup.LastModifiedById = UserId;
            StationGroup.LastModifiedDate = DateTime.Now;
            if (txtSelectedStationGroup.Value == "-1")
            {
                //save new StationGroup
                StationGroup.CreatedById = UserId;
                StationGroup.DateCreated = DateTime.Now;
                int StationGroupId = aCont.Add_StationGroup(StationGroup);
                txtSelectedStationGroup.Value = StationGroupId.ToString();
                txtStationGroupCreatedBy.Value = UserId.ToString();
                txtStationGroupCreatedDate.Value = DateTime.Now.Ticks.ToString();
                lblStationGroupMessage.Text = "Station Group Saved.";
                btnDeleteStationGroup.Enabled = true;
                btnSaveStationGroupAs.Enabled = true;
                btnManageStationsInGroup.Enabled = true;
            }
            else
            {
                //update existing StationGroup
                StationGroup.CreatedById = Convert.ToInt32(txtStationGroupCreatedBy.Value);
                StationGroup.DateCreated = new DateTime(Convert.ToInt64(txtStationGroupCreatedDate.Value));
                StationGroup.Id = Convert.ToInt32(txtSelectedStationGroup.Value);
                aCont.Update_StationGroup(StationGroup);
                lblStationGroupMessage.Text = "Station Group Updated.";
                btnManageStationsInGroup.Enabled = true;
            }
            FillStationGroups();
            FillStationGroupList();
            //fillDropDowns("");
        }

        protected void btnSaveStationGroupAs_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            StationGroupInfo StationGroup = new StationGroupInfo();
            StationGroup.PortalId = PortalId;
            StationGroup.StationGroupName = txtStationGroupName.Text;
            StationGroup.Description = txtStationGroupDescription.Text;
            //StationGroup.AgencyId = Convert.ToInt32(ddlStationGroupAgency.SelectedValue);
            StationGroup.LastModifiedById = UserId;
            StationGroup.LastModifiedDate = DateTime.Now;

            //save new StationGroup
            StationGroup.CreatedById = UserId;
            StationGroup.DateCreated = DateTime.Now;
            int StationGroupId = aCont.Add_StationGroup(StationGroup);
            txtSelectedStationGroup.Value = StationGroupId.ToString();
            txtStationGroupCreatedBy.Value = UserId.ToString();
            txtStationGroupCreatedDate.Value = DateTime.Now.Ticks.ToString();
            lblStationGroupMessage.Text = "Station Group Saved.";
            btnManageStationsInGroup.Enabled = true;

            FillStationGroups();
            FillStationGroupList();
            //fillDropDowns("");
        }

        protected void btnDeleteStationGroup_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int StationGroupId = Convert.ToInt32(txtSelectedStationGroup.Value);
            StationGroupInfo StationGroup = new StationGroupInfo();
            StationGroup.Id = StationGroupId;
            aCont.Delete_StationGroup(StationGroup);
            lblStationGroupMessage.Text = "Station Group Deleted.";
            FillStationGroups();
            FillStationGroupList();
            clearStationGroup();
            //fillDropDowns("");
        }

        protected void btnClearStationGroup_Click(object sender, System.EventArgs e)
        {
            clearStationGroup();
        }
        private void clearStationGroup()
        {
            txtStationGroupName.Text = "";
            txtStationGroupDescription.Text = "";
            //ddlStationGroupAgency.SelectedIndex = 0;
            txtSelectedStationGroup.Value = "-1";
            txtStationGroupCreatedBy.Value = "";
            txtStationGroupCreatedDate.Value = "";
            btnDeleteStationGroup.Enabled = false;
            btnSaveStationGroupAs.Enabled = false;
            btnManageStationsInGroup.Enabled = false;
            lbxStationGroupStations.Items.Clear();
            lblStationGroupMessage.Text = "";
        }

        private void FillStationGroupList()
        {
            //AdminController aCont = new AdminController();
            //List<StationGroupInfo> StationGroups = aCont.Get_StationGroupsByPortalId(PortalId);
            //gvStationGroup.DataSource = StationGroups;
            //if (ViewState["StationGroupPage"] != null)
            //{
            //    gvStationGroup.PageIndex = Convert.ToInt32(ViewState["StationGroupPage"]);
            //}
            //gvStationGroup.DataBind();
        }

        protected void gvStationGroup_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            txtSelectedStationGroup.Value = (gvStationGroup.SelectedRow.FindControl("hdngvStationGroupId") as HiddenField).Value;
            StationGroupInfo StationGroup = aCont.Get_StationGroupById(Convert.ToInt32(txtSelectedStationGroup.Value));
            if (StationGroup.Id != -1)
            {
                btnDeleteStationGroup.Enabled = true;
                btnSaveStationGroupAs.Enabled = true;
                txtStationGroupCreatedBy.Value = StationGroup.CreatedById.ToString();
                txtStationGroupCreatedDate.Value = StationGroup.DateCreated.Ticks.ToString();
                txtStationGroupName.Text = StationGroup.StationGroupName;
                txtStationGroupDescription.Text = StationGroup.Description;
                //ddlStationGroupAgency.SelectedValue = StationGroup.AgencyId.ToString();
                lblStationGroupMessage.Text = "";
                btnManageStationsInGroup.Enabled = true;
                lbxStationGroupStations.Items.Clear();
                lbxStationsInGroupModal.Items.Clear();
                foreach (StationInfo station in StationGroup.stations)
                {
                    lbxStationGroupStations.Items.Add(new ListItem(station.StationName, station.Id.ToString()));
                    lbxStationsInGroupModal.Items.Add(new ListItem(station.StationName, station.Id.ToString()));
                }
            }
            else
            {
                btnDeleteStationGroup.Enabled = false;
                btnSaveStationGroupAs.Enabled = false;
                clearStationGroup();
                lblStationGroupMessage.Text = "There was an error loading this Station Group.";
            }
            //clearAll("StationGroup");
        }

        protected void gvStationGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["StationGroupPage"] = e.NewPageIndex.ToString();
            gvStationGroup.PageIndex = e.NewPageIndex;
            gvStationGroup.DataBind();
        }
        #endregion
        #region Station Group Modal Methods
        private void clearStationGroupModal()
        {
            //lbxStationsInGroupModal.Items.Clear();
            lblStationGroupStationsModalMessage.Text = "";
        }

        protected void btnRemoveStationFromGroup_Click(object sender, System.EventArgs e)
        {
            if (lbxStationsInGroupModal.SelectedItem == null)
            {
                lblStationGroupStationsModalMessage.Text = "Please select a station to remove.";
            }
            else
            {
                AdminController aCont = new AdminController();
                aCont.Delete_StationsInGroup(Convert.ToInt32(lbxStationsInGroupModal.SelectedValue), Convert.ToInt32(txtSelectedStationGroup.Value));
                StationGroupInfo StationGroup = aCont.Get_StationGroupById(Convert.ToInt32(txtSelectedStationGroup.Value));
                lblStationGroupStationsModalMessage.Text = "Station Removed from Group.";
                lbxStationGroupStations.Items.Clear();
                lbxStationsInGroupModal.Items.Clear();
                foreach (StationInfo station in StationGroup.stations)
                {
                    lbxStationGroupStations.Items.Add(new ListItem(station.StationName, station.Id.ToString()));
                    lbxStationsInGroupModal.Items.Add(new ListItem(station.StationName, station.Id.ToString()));
                }
            }
        }
        protected void gvStationGroupStations_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            txtSelectedStationGroupStations.Value = (gvStationGroupStations.SelectedRow.FindControl("hdngvStationGroupStationId") as HiddenField).Value;
            StationInfo station = aCont.Get_StationById(Convert.ToInt32(txtSelectedStationGroupStations.Value));
            bool dupe = false;
            if (station.Id != -1)
            {
                //first, check for dupes
                foreach (ListItem li in lbxStationsInGroupModal.Items)
                {
                    if (li.Value == station.Id.ToString())
                    {
                        dupe = true;
                    }
                }
                if (!dupe)
                {
                    aCont.Add_StationsInGroup(PortalId, station.Id, Convert.ToInt32(txtSelectedStationGroup.Value));
                    lbxStationsInGroupModal.Items.Add(new ListItem(station.StationName, station.Id.ToString()));
                    lbxStationGroupStations.Items.Add(new ListItem(station.StationName, station.Id.ToString()));
                    //btnDeleteTapeFormat.Enabled = true;
                    btnRemoveStationFromGroup.Enabled = true;
                    txtStationGroupStationsCreatedBy.Value = station.CreatedById.ToString();
                    txtStationGroupStationsCreatedDate.Value = station.DateCreated.Ticks.ToString();
                    lblStationGroupStationsModalMessage.Text = "Station added to Group.";
                }
                else
                {
                    lblStationGroupStationsModalMessage.Text = "That station is already in this group.";
                }
            }
            else
            {
                btnRemoveStationFromGroup.Enabled = false;
                lblStationGroupStationsModalMessage.Text = "There was an error loading this Station.";
            }
        }

        protected void gvStationGroupStations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["StationGroupStationsPage"] = e.NewPageIndex.ToString();
            FillStations();
        }
        #endregion

        #region modal methods        
        protected void btnManageStationsInGroup_Click(object sender, System.EventArgs e)
        {
            mpeStationGroupStationsPopup.Show();
        }
        protected void btnCancelStationGroupStationsPopup_Click(object sender, System.EventArgs e)
        {
            clearStationGroupModal();
            mpeStationGroupStationsPopup.Hide();
        }
        public void recChildMarkets(ref List<MarketInfo> markets, ref List<int> marks, int selMarket)
        {
            //recurse through markets and find child markets
            foreach (MarketInfo market in markets)
            {
                if (market.ParentId == selMarket)
                {
                    marks.Add(market.Id);
                    recChildMarkets(ref markets, ref marks, market.Id);
                }
            }
        }
        public void recMarkets(ref List<MarketInfo> markets, ref List<int> marks, int selMarket)
        {
            //recurse through markets to find parent markets
            //this was logically incorrect.  Need to find child markets not parent ones.
            foreach (MarketInfo market in markets)
            {
                if (market.Id == selMarket)
                {
                    marks.Add(market.Id);
                    recChildMarkets(ref markets, ref marks, market.Id);
                    //if (market.ParentId != -1)
                    //{
                    //    recMarkets(ref markets, ref marks, market.ParentId);
                    //}
                }
            }
        }

        #endregion
    }
}