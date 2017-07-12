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
    public partial class PMT_Stations : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            fillStationMarketsList();
            FillMarketList();
            fillMarketParents();
            FillMarket2List();
            fillMarket2Parents();
            FillTapeFormatList();
            fillTapeFormats();
            FillDeliveryMethodList();
            fillDeliveryMethods();
            FillStationList();
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
        #region Station Methods
        
        protected void btnSaveStation_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            StationInfo Station = new StationInfo();
            Station.PortalId = PortalId;
            Station.LastModifiedById = UserId;
            Station.LastModifiedDate = DateTime.Now;

            Station.MarketId = Convert.ToInt32(ddlStationMarket.SelectedValue);
            Station.StationContact = txtStationContact.Text;
            Station.CallLetter = txtStationCallLetters.Text;
            Station.MediaType = Convert.ToInt32(ddlStationMediaType.SelectedValue);
            Station.ProgramFormat = ddlStationProgramFormat.SelectedValue;
            Station.AdDeliveryCallLetters = txtAdDeliveryCallLetters.Text;
            Station.OTSMHDCallLetters = txtStationOTSMHD.Text;
            Station.OTSMSDCallLetters = txtStationOTSMSD.Text;
            Station.JavelinCallLetters = txtJavelinCallLetters.Text;
            Station.backupRequired = chkBackupRequired.Checked;
            string tapeFormat = "";
            foreach (ListItem li in lbxStationTapeFormat.Items)
            {
                if (li.Selected)
                {
                    tapeFormat += li.Value + ",";
                }
            }
            if (tapeFormat.Length > 0)
            {
                tapeFormat = tapeFormat.Substring(0, tapeFormat.Length - 1);
            }
            Station.TapeFormat = tapeFormat;
            Station.Email = txtStationEmail.Text;
            Station.SpecialInstructions = txtStationSpecialInstruction.Text;
            Station.AttentionLine = txtStationAttentionLine.Text;
            string deliveryMethod = "";
            foreach (ListItem li in lbxStationDeliveryMethod.Items)
            {
                if (li.Selected)
                {
                    deliveryMethod += li.Value + ",";
                }
            }
            if (deliveryMethod.Length > 0)
            {
                deliveryMethod = deliveryMethod.Substring(0, deliveryMethod.Length - 1);
            }
            Station.DeliveryMethods = deliveryMethod;
            if (ddlStationOnline.SelectedValue == "1")
            {
                Station.Online = true;
            }
            else
            {
                Station.Online = false;
            }
            if (ddlStationStatus.SelectedValue == "1")
            {
                Station.Status = true;
            }
            else
            {
                Station.Status = false;
            }

            Station.StationName = txtStationName.Text;
            Station.Address1 = txtStationAddress1.Text;
            Station.Address2 = txtStationAddress2.Text;
            Station.City = txtStationCity.Text;
            Station.State = txtStationState.Text;
            Station.Zip = txtStationZip.Text;
            Station.Country = txtStationCountry.Text;
            Station.Phone = txtStationPhone.Text;
            Station.Fax = txtStationFax.Text;
            Station.AttentionLine = txtStationAttentionLine.Text;
            if (txtSelectedStation.Value == "-1")
            {
                //save new Station
                Station.CreatedById = UserId;
                Station.DateCreated = DateTime.Now;
                int StationId = aCont.Add_Station(Station);
                txtSelectedStation.Value = StationId.ToString();
                txtStationCreatedBy.Value = UserId.ToString();
                txtStationCreatedDate.Value = DateTime.Now.Ticks.ToString();
                lblStationMessage.Text = "Station Saved.";
                btnDeleteStation.Enabled = true;
                btnSaveStationAs.Enabled = true;
            }
            else
            {
                //update existing Station
                Station.CreatedById = Convert.ToInt32(txtStationCreatedBy.Value);
                Station.DateCreated = new DateTime(Convert.ToInt64(txtStationCreatedDate.Value));
                Station.Id = Convert.ToInt32(txtSelectedStation.Value);
                aCont.Update_Station(Station);
                lblStationMessage.Text = "Station Updated.";
            }
            FillStationList();
            //FillStations();
            //fillDropDowns("Station");
        }

        protected void btnSaveStationAs_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            StationInfo Station = new StationInfo();
            Station.PortalId = PortalId;
            Station.LastModifiedById = UserId;
            Station.LastModifiedDate = DateTime.Now;

            Station.MarketId = Convert.ToInt32(ddlStationMarket.SelectedValue);
            Station.StationContact = txtStationContact.Text;
            Station.CallLetter = txtStationCallLetters.Text;
            Station.MediaType = Convert.ToInt32(ddlStationMediaType.SelectedValue);
            Station.AttentionLine = txtStationAttentionLine.Text;
            Station.ProgramFormat = ddlStationProgramFormat.SelectedValue;
            Station.AdDeliveryCallLetters = txtAdDeliveryCallLetters.Text;
            Station.OTSMHDCallLetters = txtStationOTSMHD.Text;
            Station.OTSMSDCallLetters = txtStationOTSMSD.Text;
            Station.JavelinCallLetters = txtJavelinCallLetters.Text;
            Station.backupRequired = chkBackupRequired.Checked;
            string tapeFormat = "";
            foreach (ListItem li in lbxStationTapeFormat.Items)
            {
                if (li.Selected)
                {
                    tapeFormat += li.Value + ",";
                }
            }
            if (tapeFormat.Length > 0)
            {
                tapeFormat = tapeFormat.Substring(0, tapeFormat.Length - 1);
            }
            Station.TapeFormat = tapeFormat;
            Station.Email = txtStationEmail.Text;
            Station.SpecialInstructions = txtStationSpecialInstruction.Text;
            string deliveryMethod = "";
            foreach (ListItem li in lbxStationDeliveryMethod.Items)
            {
                if (li.Selected)
                {
                    deliveryMethod += li.Value + ",";
                }
            }
            if (deliveryMethod.Length > 0)
            {
                deliveryMethod = deliveryMethod.Substring(0, deliveryMethod.Length - 1);
            }
            Station.DeliveryMethods = deliveryMethod;
            if (ddlStationOnline.SelectedValue == "1")
            {
                Station.Online = true;
            }
            else
            {
                Station.Online = false;
            }
            if (ddlStationStatus.SelectedValue == "1")
            {
                Station.Status = true;
            }
            else
            {
                Station.Status = false;
            }

            Station.StationName = txtStationName.Text;
            Station.Address1 = txtStationAddress1.Text;
            Station.Address2 = txtStationAddress2.Text;
            Station.City = txtStationCity.Text;
            Station.State = txtStationState.Text;
            Station.Zip = txtStationZip.Text;
            Station.Country = txtStationCountry.Text;
            Station.Phone = txtStationPhone.Text;
            Station.Fax = txtStationFax.Text;

            //save new Station
            Station.CreatedById = UserId;
            Station.DateCreated = DateTime.Now;
            int StationId = aCont.Add_Station(Station);
            txtSelectedStation.Value = StationId.ToString();
            txtStationCreatedBy.Value = UserId.ToString();
            txtStationCreatedDate.Value = DateTime.Now.Ticks.ToString();
            lblStationMessage.Text = "Station Saved.";

            FillStationList();
            //FillStations();
            //fillDropDowns("Station");
        }

        protected void btnDeleteStation_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int StationId = Convert.ToInt32(txtSelectedStation.Value);
            StationInfo Station = new StationInfo();
            Station.Id = StationId;
            aCont.Delete_Station(Station);
            lblStationMessage.Text = "Station Deleted.";
            FillStationList();
            //fillDropDowns("");
            clearStation();
        }

        protected void btnClearStation_Click(object sender, System.EventArgs e)
        {
            clearStation();
        }
        private void clearStation()
        {
            ddlStationMarket.SelectedIndex = 0;
            txtStationContact.Text = "";
            txtStationCallLetters.Text = "";
            ddlStationMediaType.SelectedIndex = 0;
            foreach (ListItem li in lbxStationTapeFormat.Items)
            {
                li.Selected = false;
            }
            txtStationEmail.Text = "";
            txtStationSpecialInstruction.Text = "";
            foreach (ListItem li in lbxStationDeliveryMethod.Items)
            { li.Selected = false; }
            ddlStationOnline.SelectedIndex = 0;
            ddlStationStatus.SelectedIndex = 0;

            txtStationAddress1.Text = "";
            txtStationAddress2.Text = "";
            txtStationCity.Text = "";
            txtStationCountry.Text = "";
            txtStationCreatedBy.Value = "-1";
            txtStationCreatedDate.Value = "";
            txtStationFax.Text = "";
            txtStationName.Text = "";
            txtStationPhone.Text = "";
            txtStationState.Text = "";
            txtStationZip.Text = "";
            txtStationAttentionLine.Text = "";
            txtSelectedStation.Value = "-1";
            ddlStationProgramFormat.SelectedValue = "Short Form";
            txtAdDeliveryCallLetters.Text = "";
            txtStationOTSMHD.Text = "";
            txtStationOTSMSD.Text = "";
            txtJavelinCallLetters.Text = "";
            btnDeleteStation.Enabled = false;
            btnSaveStationAs.Enabled = false;
            lblStationMessage.Text = "";
        }

        private void FillStationList()
        {
            AdminController aCont = new AdminController();
            List<StationInfo> Stations = aCont.Get_StationsByPortalId(PortalId);
            List<StationInfo> stationsInMarket = new List<StationInfo>();
            List<StationInfo> stationsInProgramFormat = new List<StationInfo>();
            List<StationInfo> stationsFiltered = new List<StationInfo>();
            if (ddlStationMarketSearch.SelectedIndex <= 0)
            {
                stationsInMarket = Stations;
            }
            else
            {
                foreach (StationInfo station in Stations)
                {
                    if (station.MarketId.ToString() == ddlStationMarketSearch.SelectedValue)
                    {
                        stationsInMarket.Add(station);
                    }
                }
            }
            if (txtStationSearch.Text == "")
            {
                stationsFiltered = stationsInMarket;
            }
            else
            {
                foreach (StationInfo station in stationsInMarket)
                {
                    if (station.StationName.ToLower().IndexOf(txtStationSearch.Text.ToLower()) != -1
                    || station.CallLetter.ToLower().IndexOf(txtStationSearch.Text.ToLower()) != -1)
                    {
                        stationsFiltered.Add(station);
                    }
                }
            }
            if(ddlProgramFormatSearch.SelectedIndex==0)
            {
                stationsInProgramFormat = stationsFiltered;
            }
            else 
            {
                foreach (StationInfo station in stationsFiltered)
                {
                    if (ddlProgramFormatSearch.SelectedValue == station.ProgramFormat)
                    {
                        stationsInProgramFormat.Add(station);
                    }
                }
            }
            lblStationMessage.Text = stationsInProgramFormat.Count.ToString() + " Stations found.";
            gvStation.DataSource = stationsInProgramFormat;
            if (ViewState["StationPage"] != null)
            {
                gvStation.PageIndex = Convert.ToInt32(ViewState["StationPage"]);
            }
            gvStation.DataBind();
        }

        protected void gvStation_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillStationList();
            clearStation();
            AdminController aCont = new AdminController();
            txtSelectedStation.Value = (gvStation.SelectedRow.FindControl("hdngvStationId") as HiddenField).Value;
            StationInfo Station = aCont.Get_StationById(Convert.ToInt32(txtSelectedStation.Value));
            if (Station.Id != -1)
            {
                try
                {
                    ddlStationMarket.SelectedValue = Station.MarketId.ToString();
                }
                catch
                {
                    ddlStationMarket.SelectedIndex = 0;
                }
                txtStationContact.Text = Station.StationContact;
                txtStationCallLetters.Text = Station.CallLetter;
                try
                {
                    ddlStationMediaType.SelectedValue = Station.MediaType.ToString();
                }
                catch
                {
                    ddlStationMediaType.SelectedIndex = 0;
                }
                string[] tapeFormats = Station.TapeFormat.Split(',');
                foreach (string format in tapeFormats)
                {
                    foreach (ListItem li in lbxStationTapeFormat.Items)
                    {
                        if (format == li.Value)
                        {
                            li.Selected = true;
                        }
                    }
                }
                txtStationEmail.Text = Station.Email;
                txtStationSpecialInstruction.Text = Station.SpecialInstructions;
                string[] deliveryMethods = Station.DeliveryMethods.Split(',');
                foreach (string method in deliveryMethods)
                {
                    foreach (ListItem li in lbxStationDeliveryMethod.Items)
                    {
                        if (method == li.Value)
                        {
                            li.Selected = true;
                        }
                    }
                }
                if (Station.Online)
                {
                    ddlStationOnline.SelectedValue = "1";
                }
                else if (!Station.Online)
                {
                    ddlStationOnline.SelectedValue = "0";
                }
                else
                {
                    ddlStationOnline.SelectedIndex = 0;
                }
                if (Station.Status)
                {
                    ddlStationStatus.SelectedValue = "1";
                }
                else if (!Station.Status)
                {
                    ddlStationStatus.SelectedValue = "0";
                }
                else
                {
                    ddlStationStatus.SelectedIndex = 0;
                }

                btnDeleteStation.Enabled = true;
                btnSaveStationAs.Enabled = true;
                txtStationAddress1.Text = Station.Address1;
                txtStationAddress2.Text = Station.Address2;
                txtStationCity.Text = Station.City;
                txtStationCountry.Text = Station.Country;
                txtStationCreatedBy.Value = Station.CreatedById.ToString();
                txtStationCreatedDate.Value = Station.DateCreated.Ticks.ToString();
                txtStationFax.Text = Station.Fax;
                txtStationName.Text = Station.StationName;
                txtStationPhone.Text = Station.Phone;
                txtStationState.Text = Station.State;
                txtStationZip.Text = Station.Zip;
                txtStationAttentionLine.Text = Station.AttentionLine;
                ddlStationProgramFormat.SelectedValue = Station.ProgramFormat;
                txtAdDeliveryCallLetters.Text = Station.AdDeliveryCallLetters;
                txtStationOTSMHD.Text = Station.OTSMHDCallLetters;
                txtStationOTSMSD.Text = Station.OTSMSDCallLetters;
                txtJavelinCallLetters.Text = Station.JavelinCallLetters;
                chkBackupRequired.Checked = Station.backupRequired;
                //lblStationMessage.Text = "";
            }
            else
            {
                btnDeleteStation.Enabled = false;
                btnSaveStationAs.Enabled = false;
                clearStation();
                lblStationMessage.Text = "There was an error loading this Station.";
            }
            //clearAll("Station");
        }

        protected void gvStation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["StationPage"] = e.NewPageIndex.ToString();
            FillStationList();
            //gvStation.PageIndex = e.NewPageIndex;
            //gvStation.DataBind();
        }
        #endregion
        #region Market Methods
        

        private void FillMarketList()
        {
            AdminController aCont = new AdminController();
            List<MarketInfo> Markets = aCont.Get_MarketsByPortalId(PortalId);
            List<MarketInfo> marketsFiltered = new List<MarketInfo>();
            //string term = txtMarketSearch.Text.ToLower();
            //foreach (MarketInfo market in Markets)
            //{
            //    if (market.MarketName.ToLower().IndexOf(term) != -1 || market.Description.ToLower().IndexOf(term) != -1)
            //    {
            //        marketsFiltered.Add(market);
            //    }
            //}
            //gvMarket.DataSource = marketsFiltered;
            gvMarket2.DataSource = Markets;
            //lblMarketMessage.Text = marketsFiltered.Count.ToString() + " Markets found.";
            
            if (ViewState["Market2Page"] != null)
            {
                gvMarket2.PageIndex = Convert.ToInt32(ViewState["Market2Page"]);
            }
            //gvMarket.DataBind();
            gvMarket2.DataBind();

            //fill the markets in Station Market search
            ddlStationMarketSearch.Items.Clear();
            ddlStationMarketSearch.Items.Add(new ListItem("--Please Select Market--", "-1"));
            foreach (MarketInfo market in Markets)
            {
                ddlStationMarketSearch.Items.Add(new ListItem(market.MarketName, market.Id.ToString()));
            }
        }
        private void fillStationMarketsList()
        {
            ddlStationMarket.Items.Clear();
            AdminController aCont = new AdminController();
            List<MarketInfo> markets = aCont.Get_MarketsByPortalId(PortalId);
            ddlStationMarket.Items.Add(new ListItem("--Please Select Market--", "-1"));
            foreach (MarketInfo market in markets)
            {
                ddlStationMarket.Items.Add(new ListItem(market.MarketName, market.Id.ToString()));
            }
        }

        protected void btnMarketSearch_Click(object sender, System.EventArgs e)
        {
            FillMarketList();
        }

        protected void btnMarketSearchClear_Click(object sender, System.EventArgs e)
        {
            FillMarketList();
        }
        private void fillMarketParents()
        {
            //ddlMarketParent.Items.Clear();
            ddlMarket2Parent.Items.Clear();
            //ddlStationGroupMarketSearch.Items.Clear();
            AdminController aCont = new AdminController();
            List<MarketInfo> markets = aCont.Get_MarketsByPortalId(PortalId);
            //ddlMarketParent.Items.Add(new ListItem("--Select Parent if Appropriate--", "-1"));
            ddlMarket2Parent.Items.Add(new ListItem("--Select Parent if Appropriate--", "-1"));
            //ddlStationGroupMarketSearch.Items.Add(new ListItem("--Select Market--", "-1"));
            foreach (MarketInfo market in markets)
            {
                //ddlStationGroupMarketSearch.Items.Add(new ListItem(market.MarketName, market.Id.ToString()));
                //if (market.Id.ToString() != txtSelectedMarket.Value)
                //{
                //    ddlMarketParent.Items.Add(new ListItem(market.MarketName, market.Id.ToString()));
                //}
                if (market.Id.ToString() != txtSelectedMarket2.Value)
                {
                    ddlMarket2Parent.Items.Add(new ListItem(market.MarketName, market.Id.ToString()));
                }
            }
            try
            {
                //ddlMarketParent.SelectedValue = txtSelectedMarket.Value.ToString();
                ddlMarket2Parent.SelectedValue = txtSelectedMarket2.Value.ToString();
            }
            catch { }
        }
        #endregion
        #region Market2 Methods
        //Market2 refers to the modal popup version of Market CRUD accessed from the Stations interface
        protected void btnSaveMarket2_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            MarketInfo Market2 = new MarketInfo();
            Market2.PortalId = PortalId;
            Market2.LastModifiedById = UserId;
            Market2.LastModifiedDate = DateTime.Now;
            Market2.MarketName = txtMarket2Name.Text;
            Market2.Description = txtMarket2Description.Text;
            Market2.ParentId = Convert.ToInt32(ddlMarket2Parent.SelectedValue);
            if (txtSelectedMarket2.Value == "-1")
            {
                //save new Market2
                Market2.CreatedById = UserId;
                Market2.DateCreated = DateTime.Now;
                int Market2Id = aCont.Add_Market(Market2);
                txtSelectedMarket2.Value = Market2Id.ToString();
                txtMarket2CreatedBy.Value = UserId.ToString();
                txtMarket2CreatedDate.Value = DateTime.Now.Ticks.ToString();
                lblMarket2Message.Text = "Market Saved.";
                btnDeleteMarket2.Enabled = true;
                btnSaveMarket2As.Enabled = true;
            }
            else
            {
                //update existing Market2
                Market2.CreatedById = Convert.ToInt32(txtMarket2CreatedBy.Value);
                Market2.DateCreated = new DateTime(Convert.ToInt64(txtMarket2CreatedDate.Value));
                Market2.Id = Convert.ToInt32(txtSelectedMarket2.Value);
                aCont.Update_Market(Market2);
                lblMarket2Message.Text = "Market Updated.";
            }
            FillMarket2List();
            //FillMarketList();
            //fillDropDowns("");
            fillMarket2Parents();
            //fillMarketParents();
            fillStationMarketsList();
            ddlMarket2Parent.SelectedValue = Market2.ParentId.ToString();
        }

        protected void btnSaveMarket2As_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            MarketInfo Market2 = new MarketInfo();
            Market2.PortalId = PortalId;
            Market2.LastModifiedById = UserId;
            Market2.LastModifiedDate = DateTime.Now;
            Market2.MarketName = txtMarket2Name.Text;
            Market2.Description = txtMarket2Description.Text;
            Market2.ParentId = Convert.ToInt32(ddlMarket2Parent.SelectedValue);

            //save new Market2
            Market2.CreatedById = UserId;
            Market2.DateCreated = DateTime.Now;
            int Market2Id = aCont.Add_Market(Market2);
            txtSelectedMarket2.Value = Market2Id.ToString();
            txtMarket2CreatedBy.Value = UserId.ToString();
            txtMarket2CreatedDate.Value = DateTime.Now.Ticks.ToString();
            lblMarket2Message.Text = "Market Saved.";

            FillMarket2List();
            //FillMarketList();
            //fillDropDowns("");
            fillMarket2Parents();
            //fillMarketParents();
            fillStationMarketsList();
            ddlMarket2Parent.SelectedValue = Market2.ParentId.ToString();
        }

        protected void btnDeleteMarket2_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int Market2Id = Convert.ToInt32(txtSelectedMarket2.Value);
            MarketInfo Market2 = new MarketInfo();
            Market2.Id = Market2Id;
            aCont.Delete_Market(Market2);
            lblMarket2Message.Text = "Market Deleted.";
            FillMarket2List();
            //fillDropDowns("");
            fillMarket2Parents();
            clearMarket2();
        }

        protected void btnClearMarket2_Click(object sender, System.EventArgs e)
        {
            clearMarket2();
        }
        private void clearMarket2()
        {
            txtMarket2Name.Text = "";
            txtSelectedMarket2.Value = "-1";
            txtMarket2CreatedBy.Value = "";
            txtMarket2CreatedDate.Value = "";
            txtMarket2Description.Text = "";
            btnDeleteMarket2.Enabled = false;
            btnSaveMarket2As.Enabled = false;
            lblMarket2Message.Text = "";
            fillMarket2Parents();
        }

        private void FillMarket2List()
        {
            AdminController aCont = new AdminController();
            List<MarketInfo> Market2s = aCont.Get_MarketsByPortalId(PortalId);
            List<MarketInfo> Market2sFiltered = new List<MarketInfo>();
            string term = txtMarket2Search.Text.ToLower();
            if (term != "")
            {
                foreach (MarketInfo Market2 in Market2s)
                {
                    if (Market2.MarketName.ToLower().IndexOf(term) != -1 || Market2.Description.ToLower().IndexOf(term) != -1)
                    {
                        Market2sFiltered.Add(Market2);
                    }
                }
            }
            else
            {
                Market2sFiltered = Market2s;
            }
            gvMarket2.DataSource = Market2sFiltered;
            if (ViewState["Market2Page"] != null)
            {
                gvMarket2.PageIndex = Convert.ToInt32(ViewState["Market2Page"]);
            }
            gvMarket2.DataBind();
        }

        protected void gvMarket2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            txtSelectedMarket2.Value = (gvMarket2.SelectedRow.FindControl("hdngvMarket2Id") as HiddenField).Value;
            fillMarket2Parents();
            MarketInfo Market2 = aCont.Get_MarketById(Convert.ToInt32(txtSelectedMarket2.Value));
            if (Market2.Id != -1)
            {
                btnDeleteMarket2.Enabled = true;
                btnSaveMarket2As.Enabled = true;
                txtMarket2CreatedBy.Value = Market2.CreatedById.ToString();
                txtMarket2CreatedDate.Value = Market2.DateCreated.Ticks.ToString();
                txtMarket2Name.Text = Market2.MarketName;
                txtMarket2Description.Text = Market2.Description;
                try
                {
                    ddlMarket2Parent.SelectedValue = Market2.ParentId.ToString();
                }
                catch { }
                lblMarket2Message.Text = "";
            }
            else
            {
                btnDeleteMarket2.Enabled = false;
                btnSaveMarket2As.Enabled = false;
                clearMarket2();
                lblMarket2Message.Text = "There was an error loading this Market.";
            }
            //clearAll("Market");
        }

        protected void gvMarket2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["Market2Page"] = e.NewPageIndex.ToString();
            FillMarket2List();
            //gvMarket2.PageIndex = e.NewPageIndex;
            //gvMarket2.DataBind();
        }

        protected void btnMarket2Search_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            List<MarketInfo> Market2s = aCont.Get_MarketsByPortalId(PortalId);
            List<MarketInfo> Market2sFiltered = new List<MarketInfo>();
            string term = txtMarket2Search.Text.ToLower();
            foreach (MarketInfo Market2 in Market2s)
            {
                if (Market2.MarketName.ToLower().IndexOf(term) != -1 || Market2.Description.ToLower().IndexOf(term) != -1)
                {
                    Market2sFiltered.Add(Market2);
                }
            }
            gvMarket2.DataSource = Market2sFiltered;
            gvMarket2.DataBind();
            lblMarket2Message.Text = Market2sFiltered.Count.ToString() + " Result(s) found for " + term + ".";
        }

        protected void btnMarket2SearchClear_Click(object sender, System.EventArgs e)
        {
            txtMarket2Search.Text = "";
            FillMarket2List();
            clearMarket2();
        }
        private void fillMarket2Parents()
        {
            ddlMarket2Parent.Items.Clear();
            AdminController aCont = new AdminController();
            List<MarketInfo> Market2s = aCont.Get_MarketsByPortalId(PortalId);
            ddlMarket2Parent.Items.Add(new ListItem("--Select Parent if Appropriate--", "-1"));
            foreach (MarketInfo Market2 in Market2s)
            {
                if (Market2.Id.ToString() != txtSelectedMarket2.Value)
                {
                    ddlMarket2Parent.Items.Add(new ListItem(Market2.MarketName, Market2.Id.ToString()));
                }
            }
            try
            {
                ddlMarket2Parent.SelectedValue = txtSelectedMarket2.Value.ToString();
            }
            catch { }
        }
        protected void txtMarket2Search_TextChanged(object sender, EventArgs e)
        {
            //AdminController aCont = new AdminController();
            //List<MarketInfo> Market2s = aCont.Get_MarketsByPortalId(PortalId);
            //List<MarketInfo> Market2sFiltered = new List<MarketInfo>();
            //string term = txtMarket2Search.Text.ToLower();
            //foreach (MarketInfo Market2 in Market2s)
            //{
            //    if (Market2.MarketName.ToLower().IndexOf(term) != -1 || Market2.Description.ToLower().IndexOf(term) != -1)
            //    {
            //        Market2sFiltered.Add(Market2);
            //    }
            //}
            //gvMarket2.DataSource = Market2sFiltered;
            //gvMarket2.DataBind();
            //lblMarket2Message.Text = Market2sFiltered.Count.ToString() + " Result(s) found for " + term + ".";
            FillMarket2List();
        }
        #endregion
        #region TapeFormat Methods
        protected void btnSaveTapeFormat_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            TapeFormatInfo TapeFormat = new TapeFormatInfo();
            TapeFormat.PortalId = PortalId;
            TapeFormat.LastModifiedById = UserId;
            TapeFormat.LastModifiedDate = DateTime.Now;
            TapeFormat.TapeFormat = txtTapeFormat.Text;
            TapeFormat.Printer = ddlTapePrinter.SelectedValue;
            TapeFormat.Label = ddlTapeLabel.SelectedValue;
            try
            {
                TapeFormat.Weight = Convert.ToDouble(txtTapeWeight.Text);
            }
            catch { }
            try
            {
                TapeFormat.MaxPerPak = Convert.ToInt32(txtTapeMaxPerPak.Text);
            }
            catch { }
            if (txtSelectedTapeFormat.Value == "-1")
            {
                //save new TapeFormat
                TapeFormat.CreatedById = UserId;
                TapeFormat.DateCreated = DateTime.Now;
                int TapeFormatId = aCont.Add_TapeFormat(TapeFormat);
                txtSelectedTapeFormat.Value = TapeFormatId.ToString();
                txtTapeFormatCreatedBy.Value = UserId.ToString();
                txtTapeFormatCreatedDate.Value = DateTime.Now.Ticks.ToString();
                lblTapeFormatMessage.Text = "Tape Format Type Saved.";
                btnDeleteTapeFormat.Enabled = true;
                btnSaveTapeFormatAs.Enabled = true;
            }
            else
            {
                //update existing TapeFormat
                TapeFormat.CreatedById = Convert.ToInt32(txtTapeFormatCreatedBy.Value);
                TapeFormat.DateCreated = new DateTime(Convert.ToInt64(txtTapeFormatCreatedDate.Value));
                TapeFormat.Id = Convert.ToInt32(txtSelectedTapeFormat.Value);
                aCont.Update_TapeFormat(TapeFormat);
                lblTapeFormatMessage.Text = "Tape Format Updated.";
            }
            fillTapeFormats();
            FillTapeFormatList();
            //fillDropDowns("");
        }

        protected void btnSaveTapeFormatAs_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            TapeFormatInfo TapeFormat = new TapeFormatInfo();
            TapeFormat.PortalId = PortalId;
            TapeFormat.LastModifiedById = UserId;
            TapeFormat.LastModifiedDate = DateTime.Now;
            TapeFormat.TapeFormat = txtTapeFormat.Text;
            TapeFormat.Printer = ddlTapePrinter.SelectedValue;
            TapeFormat.Label = ddlTapeLabel.SelectedValue;
            try
            {
                TapeFormat.Weight = Convert.ToDouble(txtTapeWeight.Text);
            }
            catch { }
            try
            {
                TapeFormat.MaxPerPak = Convert.ToInt32(txtTapeMaxPerPak.Text);
            }
            catch { }

            //save new TapeFormat
            TapeFormat.CreatedById = UserId;
            TapeFormat.DateCreated = DateTime.Now;
            int TapeFormatId = aCont.Add_TapeFormat(TapeFormat);
            txtSelectedTapeFormat.Value = TapeFormatId.ToString();
            txtTapeFormatCreatedBy.Value = UserId.ToString();
            txtTapeFormatCreatedDate.Value = DateTime.Now.Ticks.ToString();
            lblTapeFormatMessage.Text = "Tape Format Type Saved.";

            fillTapeFormats();
            FillTapeFormatList();
            //fillDropDowns("");
        }

        protected void btnDeleteTapeFormat_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int TapeFormatId = Convert.ToInt32(txtSelectedTapeFormat.Value);
            TapeFormatInfo TapeFormat = new TapeFormatInfo();
            TapeFormat.Id = TapeFormatId;
            aCont.Delete_TapeFormat(TapeFormat);
            lblTapeFormatMessage.Text = "Tape Format Type Deleted.";
            fillTapeFormats();
            FillTapeFormatList();
            clearTapeFormat();
            //fillDropDowns("");
        }

        protected void btnClearTapeFormat_Click(object sender, System.EventArgs e)
        {
            clearTapeFormat();
        }
        private void clearTapeFormat()
        {
            txtTapeFormat.Text = "";
            ddlTapePrinter.SelectedIndex = 0;
            ddlTapeLabel.SelectedIndex = 0;
            txtSelectedTapeFormat.Value = "-1";
            txtTapeFormatCreatedBy.Value = "";
            txtTapeFormatCreatedDate.Value = "";
            btnDeleteTapeFormat.Enabled = false;
            btnSaveTapeFormatAs.Enabled = false;
            lblTapeFormatMessage.Text = "";
            txtTapeMaxPerPak.Text = "";
            txtTapeWeight.Text = "";
        }

        private void FillTapeFormatList()
        {
            AdminController aCont = new AdminController();
            List<TapeFormatInfo> TapeFormats = aCont.Get_TapeFormatsByPortalId(PortalId);
            gvTapeFormat.DataSource = TapeFormats;
            if (ViewState["TapeFormatPage"] != null)
            {
                gvTapeFormat.PageIndex = Convert.ToInt32(ViewState["TapeFormatPage"]);
            }
            gvTapeFormat.DataBind();
        }

        protected void gvTapeFormat_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            txtSelectedTapeFormat.Value = (gvTapeFormat.SelectedRow.FindControl("hdngvTapeFormatId") as HiddenField).Value;
            TapeFormatInfo TapeFormat = aCont.Get_TapeFormatById(Convert.ToInt32(txtSelectedTapeFormat.Value));
            if (TapeFormat.Id != -1)
            {
                btnDeleteTapeFormat.Enabled = true;
                btnSaveTapeFormatAs.Enabled = true;
                txtTapeFormatCreatedBy.Value = TapeFormat.CreatedById.ToString();
                txtTapeFormatCreatedDate.Value = TapeFormat.DateCreated.Ticks.ToString();
                txtTapeFormat.Text = TapeFormat.TapeFormat;
                ddlTapePrinter.SelectedValue = TapeFormat.Printer;
                ddlTapeLabel.SelectedValue = TapeFormat.Label;
                lblTapeFormatMessage.Text = "";
                txtTapeWeight.Text = TapeFormat.Weight.ToString();
                txtTapeMaxPerPak.Text = TapeFormat.MaxPerPak.ToString();
            }
            else
            {
                btnDeleteTapeFormat.Enabled = false;
                btnSaveTapeFormatAs.Enabled = false;
                clearTapeFormat();
                lblTapeFormatMessage.Text = "There was an error loading this Tape Format Type.";
            }
        }

        protected void gvTapeFormat_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["TapeFormatPage"] = e.NewPageIndex.ToString();
            gvTapeFormat.PageIndex = e.NewPageIndex;
            gvTapeFormat.DataBind();
        }
        private void fillTapeFormats()
        {
            lbxStationTapeFormat.Items.Clear();
            AdminController aCont = new AdminController();
            List<TapeFormatInfo> TapeFormats = aCont.Get_TapeFormatsByPortalId(PortalId);
            //ddlLabelTapeFormat.Items.Add(new ListItem("--Please Select--", "-1"));
            foreach (TapeFormatInfo TapeFormat in TapeFormats)
            {
                lbxStationTapeFormat.Items.Add(new ListItem(TapeFormat.TapeFormat, TapeFormat.Id.ToString()));
                //ddlLabelTapeFormat.Items.Add(new ListItem(TapeFormat.TapeFormat, TapeFormat.Id.ToString()));
            }
        }
        #endregion
        #region DeliveryMethod Methods
        protected void btnSaveDeliveryMethod_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            DeliveryMethodInfo DeliveryMethod = new DeliveryMethodInfo();
            DeliveryMethod.PortalId = PortalId;
            DeliveryMethod.LastModifiedById = UserId;
            DeliveryMethod.LastModifiedDate = DateTime.Now;
            DeliveryMethod.DeliveryMethod = txtDeliveryMethod.Text;
            DeliveryMethod.Priority = txtDeliveryPriority.Text;
            if (txtSelectedDeliveryMethod.Value == "-1")
            {
                //save new DeliveryMethod
                DeliveryMethod.CreatedById = UserId;
                DeliveryMethod.DateCreated = DateTime.Now;
                int DeliveryMethodId = aCont.Add_DeliveryMethod(DeliveryMethod);
                txtSelectedDeliveryMethod.Value = DeliveryMethodId.ToString();
                txtDeliveryMethodCreatedBy.Value = UserId.ToString();
                txtDeliveryMethodCreatedDate.Value = DateTime.Now.Ticks.ToString();
                lblDeliveryMethodMessage.Text = "Delivery Method Saved.";
                btnDeleteDeliveryMethod.Enabled = true;
                btnSaveDeliveryMethodAs.Enabled = true;
            }
            else
            {
                //update existing DeliveryMethod
                DeliveryMethod.CreatedById = Convert.ToInt32(txtDeliveryMethodCreatedBy.Value);
                DeliveryMethod.DateCreated = new DateTime(Convert.ToInt64(txtDeliveryMethodCreatedDate.Value));
                DeliveryMethod.Id = Convert.ToInt32(txtSelectedDeliveryMethod.Value);
                aCont.Update_DeliveryMethod(DeliveryMethod);
                lblDeliveryMethodMessage.Text = "Delivery Method Updated.";
            }
            fillDeliveryMethods();
            FillDeliveryMethodList();
            //fillDropDowns("");
        }

        protected void btnSaveDeliveryMethodAs_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            DeliveryMethodInfo DeliveryMethod = new DeliveryMethodInfo();
            DeliveryMethod.PortalId = PortalId;
            DeliveryMethod.LastModifiedById = UserId;
            DeliveryMethod.LastModifiedDate = DateTime.Now;
            DeliveryMethod.DeliveryMethod = txtDeliveryMethod.Text;
            DeliveryMethod.Priority = txtDeliveryPriority.Text;

            //save new DeliveryMethod
            DeliveryMethod.CreatedById = UserId;
            DeliveryMethod.DateCreated = DateTime.Now;
            int DeliveryMethodId = aCont.Add_DeliveryMethod(DeliveryMethod);
            txtSelectedDeliveryMethod.Value = DeliveryMethodId.ToString();
            txtDeliveryMethodCreatedBy.Value = UserId.ToString();
            txtDeliveryMethodCreatedDate.Value = DateTime.Now.Ticks.ToString();
            lblDeliveryMethodMessage.Text = "Delivery Method Saved.";

            fillDeliveryMethods();
            FillDeliveryMethodList();
            //fillDropDowns("");
        }

        protected void btnDeleteDeliveryMethod_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int DeliveryMethodId = Convert.ToInt32(txtSelectedDeliveryMethod.Value);
            DeliveryMethodInfo DeliveryMethod = new DeliveryMethodInfo();
            DeliveryMethod.Id = DeliveryMethodId;
            aCont.Delete_DeliveryMethod(DeliveryMethod);
            lblDeliveryMethodMessage.Text = "DeliveryMethod Type Deleted.";
            fillDeliveryMethods();
            FillDeliveryMethodList();
            clearDeliveryMethod();
            //fillDropDowns("");
        }

        protected void btnClearDeliveryMethod_Click(object sender, System.EventArgs e)
        {
            clearDeliveryMethod();
        }
        private void clearDeliveryMethod()
        {
            txtDeliveryMethod.Text = "";
            txtSelectedDeliveryMethod.Value = "-1";
            txtDeliveryMethodCreatedBy.Value = "";
            txtDeliveryMethodCreatedDate.Value = "";
            btnDeleteDeliveryMethod.Enabled = false;
            btnSaveDeliveryMethodAs.Enabled = false;
            lblDeliveryMethodMessage.Text = "";
            txtDeliveryPriority.Text = "";
        }

        private void FillDeliveryMethodList()
        {
            AdminController aCont = new AdminController();
            List<DeliveryMethodInfo> DeliveryMethods = aCont.Get_DeliveryMethodsByPortalId(PortalId);
            gvDeliveryMethod.DataSource = DeliveryMethods;
            if (ViewState["DeliveryMethodPage"] != null)
            {
                gvDeliveryMethod.PageIndex = Convert.ToInt32(ViewState["DeliveryMethodPage"]);
            }
            gvDeliveryMethod.DataBind();
        }

        protected void gvDeliveryMethod_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            txtSelectedDeliveryMethod.Value = (gvDeliveryMethod.SelectedRow.FindControl("hdngvDeliveryMethodId") as HiddenField).Value;
            DeliveryMethodInfo DeliveryMethod = aCont.Get_DeliveryMethodById(Convert.ToInt32(txtSelectedDeliveryMethod.Value));
            if (DeliveryMethod.Id != -1)
            {
                btnDeleteDeliveryMethod.Enabled = true;
                btnSaveDeliveryMethodAs.Enabled = true;
                txtDeliveryMethodCreatedBy.Value = DeliveryMethod.CreatedById.ToString();
                txtDeliveryMethodCreatedDate.Value = DeliveryMethod.DateCreated.Ticks.ToString();
                txtDeliveryMethod.Text = DeliveryMethod.DeliveryMethod;
                txtDeliveryPriority.Text = DeliveryMethod.Priority;
                lblDeliveryMethodMessage.Text = "";
            }
            else
            {
                btnDeleteDeliveryMethod.Enabled = false;
                btnSaveDeliveryMethodAs.Enabled = false;
                clearDeliveryMethod();
                lblDeliveryMethodMessage.Text = "There was an error loading this Delivery Method.";
            }
        }

        protected void gvDeliveryMethod_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["DeliveryMethodPage"] = e.NewPageIndex.ToString();
            gvDeliveryMethod.PageIndex = e.NewPageIndex;
            gvDeliveryMethod.DataBind();
        }
        private void fillDeliveryMethods()
        {
            lbxStationDeliveryMethod.Items.Clear();
            AdminController aCont = new AdminController();
            List<DeliveryMethodInfo> DeliveryMethods = aCont.Get_DeliveryMethodsByPortalId(PortalId);
            foreach (DeliveryMethodInfo DeliveryMethod in DeliveryMethods)
            {
                lbxStationDeliveryMethod.Items.Add(new ListItem(DeliveryMethod.DeliveryMethod, DeliveryMethod.Id.ToString()));
            }
        }
        #endregion
        protected void btnCancelTapeFormatPopup_Click(object sender, System.EventArgs e)
        {
            clearTapeFormat();
            mpeTapeFormatPopup.Hide();
        }
        protected void btnCancelDeliveryMethodPopup_Click(object sender, System.EventArgs e)
        {
            clearDeliveryMethod();
            mpeDeliveryMethodPopup.Hide();
        }
        protected void btnCancelMarket2Popup_Click(object sender, System.EventArgs e)
        {
            clearMarket2();
            mpeMarket2Popup.Hide();
        }

        protected void btnAddStationMarket_Click(object sender, System.EventArgs e)
        {
            mpeMarket2Popup.Show();
        }
        protected void btnAddStationTapeFormat_Click(object sender, System.EventArgs e)
        {
            mpeTapeFormatPopup.Show();
        }
        protected void btnAddStationDeliveryMethod_Click(object sender, System.EventArgs e)
        {
            mpeDeliveryMethodPopup.Show();
        }
        protected void btnStationSearch_Click(object sender, System.EventArgs e)
        {
            FillStationList();
        }

        protected void txtStationSearch_TextChanged(object sender, EventArgs e)
        {
            FillStationList();
        }

        protected void ddlStationMarketSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillStationList();
        }

        protected void ddlProgramFormatSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillStationList();
        }

        protected void btnCreateEzPostAddress_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            if (txtSelectedStation.Value != "-1")
            {
                StationInfo stat = aCont.Get_StationById(Convert.ToInt32(txtSelectedStation.Value));
                lblStationMessage.Text = aCont.createEasyPostAddress(stat).id;
            }
        }

        
    }
}