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
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from PMT_AdminModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class PMT_Admin : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            showTabs();
            fillDropDowns("");
            FillAgencyList();
            FillAdvertiserList();
            FillMarketList();
            FillClientTypeList();
            FillCarrierTypeList();
            fillCarrierTypes();
            FillFreightTypeList();
            FillTapeFormatList();
            fillTapeFormats();
            fillStationMarketsList();
            fillDeliveryMethods();
            FillDeliveryMethodList();
            FillStationList();
            FillStations();
            FillStationGroups();
            FillLabelList();
            FillMasterItemList();
            fillUsers();
            fillUserRoles();
            if(!Page.IsPostBack)
            {
                clearAll("");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //AdminController aCont = new AdminController();
            //lblAgencyMessage.Text = aCont.GetNextMediaId(PortalId);
            try
            {
                if(ddlMasterItemAdvertiser.SelectedIndex>0)
                {
                    
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
        private string getSetting(string settingName, string defaultValue)
        {
            string returnMe = "";
            if(Settings.Contains(settingName))
            {
                returnMe = Settings[settingName].ToString();
            }
            else
            {
                returnMe = defaultValue;
            }
            return returnMe;
        }
        private void clearAll(string except)
        {
            if (except != "Carrier")
            { clearCarrierType(); }
            if (except != "Client")
            { clearClientType(); }
            if (except != "Market")
            {
                clearMarket();
                clearMarket2();
            }
            if (except != "Station")
            { clearStation(); }
            if (except != "StationGroup")
            { clearStationGroup(); }
            if (except != "Advertiser")
            { clearAdvertiser(); }
            if (except != "Agency")
            { clearAgency(); }
            if (except != "DeliveryMethod")
            { clearDeliveryMethod(); }
            if (except != "FreightType")
            { clearFreightType(); }
            if (except != "TapeFormat")
            { clearTapeFormat(); }
            if (except != "Labels")
            { clearLabel(); }
            if (except != "MasterItems")
            { clearMasterItem(); }
        }
        private void showTabs()
        {
            RoleController rCont = new RoleController();
            IList<RoleInfo> roles = rCont.GetRoles(PortalId);
            List<int> myRoles = new List<int>();
            bool isAdmin = false;
            if(UserInfo.IsInRole("Administators"))
            { isAdmin = true; }
            foreach (RoleInfo role in roles)
            {
                if(UserInfo.IsInRole(role.RoleName))
                {
                    myRoles.Add(role.RoleID);
                }
            }
            string agencyViewRoles = getSetting("AgencyViewRoles", "");
            string[] agencyRoles = agencyViewRoles.Split(',');
            foreach(string ar in agencyRoles)
            {
                foreach (int myRole in myRoles)
                {
                    if (ar == myRole.ToString() || isAdmin)
                    {
                        liAgency.Visible = true;
                        pnlAgencies.Visible = true;
                        break;
                    }
                }
            }
            string advertiserViewRoles = getSetting("AdvertiserViewRoles", "");
            string[] advertRoles = advertiserViewRoles.Split(',');
            foreach (string ar in advertRoles)
            {
                foreach (int myRole in myRoles)
                {
                    if (ar == myRole.ToString() || isAdmin)
                    {
                        liAdvertiser.Visible = true;
                        pnlAdvertisers.Visible = true;
                        break;
                    }
                }
            }
            string marketViewRoles = getSetting("MarketViewRoles", "");
            string[] marketRoles = marketViewRoles.Split(',');
            foreach (string ar in marketRoles)
            {
                foreach (int myRole in myRoles)
                {
                    if (ar == myRole.ToString() || isAdmin)
                    {
                        liMarket.Visible = true;
                        pnlMarkets.Visible = true;
                        break;
                    }
                }
            }
            string stationViewRoles = getSetting("StationViewRoles", "");
            string[] stationRoles = stationViewRoles.Split(',');
            foreach (string ar in stationRoles)
            {
                foreach (int myRole in myRoles)
                {
                    if (ar == myRole.ToString() || isAdmin)
                    {
                        liStation.Visible = true;
                        pnlStations.Visible = true;
                        break;
                    }
                }
            }
            string stationGroupViewRoles = getSetting("StationGroupViewRoles", "");
            string[] stationGroupRoles = stationGroupViewRoles.Split(',');
            foreach (string ar in stationGroupRoles)
            {
                foreach (int myRole in myRoles)
                {
                    if (ar == myRole.ToString() || isAdmin)
                    {
                        liStationGroup.Visible = true;
                        pnlStationGroups.Visible = true;
                        break;
                    }
                }
            }
            string LabelViewRoles = getSetting("LabelViewRoles", "");
            string[] LabelRoles = LabelViewRoles.Split(',');
            foreach (string ar in LabelRoles)
            {
                foreach (int myRole in myRoles)
                {
                    if (ar == myRole.ToString() || isAdmin)
                    {
                        liLabels.Visible = true;
                        pnlLabels.Visible = true;
                        break;
                    }
                }
            }
            string MasterItemViewRoles = getSetting("MasterItemsViewRoles", "");
            string[] MasterItemRoles = MasterItemViewRoles.Split(',');
            foreach (string ar in MasterItemRoles)
            {
                foreach (int myRole in myRoles)
                {
                    if (ar == myRole.ToString() || isAdmin)
                    {
                        liMasterItems.Visible = true;
                        pnlMasterItems.Visible = true;
                        break;
                    }
                }
            }
            string UserViewRoles = getSetting("UsersViewRoles", "");
            string[] UserRoles = UserViewRoles.Split(',');
            foreach (string ar in UserRoles)
            {
                foreach (int myRole in myRoles)
                {
                    if (ar == myRole.ToString() || isAdmin)
                    {
                        liUsers.Visible = true;
                        pnlUsers.Visible = true;
                        break;
                    }
                }
            }
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
        #region fill lists
        private void fillDropDowns(string fromPanel)
        {
            AdminController aCont = new AdminController();
            if (fromPanel != "Agency")
            {
                //Agency Client Type
                fillClientTypes();
                fillCarrierTypes();
            }

            if (fromPanel != "Advertiser")
            {
                //Advertiser Agency List
                List<AgencyInfo> agencies = aCont.Get_AgenciesByPortalId(PortalId);
                ddlAdvertiserAgency.Items.Clear();
                ddlAdvertiserAgencySearch.Items.Clear();
                //ddlAdvertiserAgency.Items.Add(new ListItem("--Select Agency--", "-1"));
                ddlAdvertiserAgencySearch.Items.Add(new ListItem("--Select Agency--", "-1"));
                foreach (AgencyInfo agency in agencies)
                {
                    ddlAdvertiserAgency.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                    ddlAdvertiserAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                }
                ddlAdvertiserAgency.ClearSelection();
            }

            if (fromPanel != "Advertiser")
            {
                //Advertiser Client Type
                fillClientTypes();
                fillCarrierTypes();
            }
            if (fromPanel != "Advertiser")
            {
                //Advertiser Carrier
                ddlAdvertiserCarrier.Items.Clear();
                ddlAdvertiserCarrier.Items.Add(new ListItem("--Select Carrier--", "-1"));
                List<CarrierTypeInfo> carriers = aCont.Get_CarrierTypesByPortalId(PortalId);
                foreach (CarrierTypeInfo carrier in carriers)
                {
                    ddlAdvertiserCarrier.Items.Add(new ListItem(carrier.CarrierType,carrier.Id.ToString()));
                }
            }
            if (fromPanel != "Advertiser")
            {
                //Advertiser Freight
                ddlAdvertiserFreight.Items.Clear();
                ddlAdvertiserFreight.Items.Add(new ListItem("--Select Freight--", "-1"));
                List<FreightTypeInfo> freights = aCont.Get_FreightTypesByPortalId(PortalId);
                foreach (FreightTypeInfo freight in freights)
                {
                    ddlAdvertiserFreight.Items.Add(new ListItem(freight.FreightType,freight.Id.ToString()));
                }
            }
            fillMarketParents();
            FillTapeFormatList();
            fillDeliveryMethods();
            fillStationMarketsList();
        }
        private void fillStationMarketsList()
        {
            ddlStationMarket.Items.Clear();
            AdminController aCont = new AdminController();
            List<MarketInfo> markets = aCont.Get_MarketsByPortalId(PortalId);
            ddlStationMarket.Items.Add(new ListItem("--Please Select Market--", "-1"));
            foreach(MarketInfo market in markets)
            {
                ddlStationMarket.Items.Add(new ListItem(market.MarketName, market.Id.ToString()));
            }
        }
        private void fillClientTypes()
        {
            ddlAdvertiserClientType.Items.Clear();
            ddlAgencyClientType.Items.Clear();
            ddlAgencyClientType.Items.Add(new ListItem("--Please Select Client Type--","-1"));
            ddlAdvertiserClientType.Items.Add(new ListItem("--Please Select Client Type--", "-1"));
            AdminController aCont = new AdminController();
            List<ClientTypeInfo> clientTypes = aCont.Get_ClientTypesByPortalId(PortalId);
            foreach (ClientTypeInfo clientType in clientTypes)
            {
                ddlAgencyClientType.Items.Add(new ListItem(clientType.ClientType, clientType.Id.ToString()));
                ddlAdvertiserClientType.Items.Add(new ListItem(clientType.ClientType, clientType.Id.ToString()));
            }
        }
        private void fillCarrierTypes()
        {
            ddlAdvertiserCarrier.Items.Clear();
            ddlAdvertiserCarrier.Items.Add(new ListItem("--Please Select Client Type--", "-1"));
            AdminController aCont = new AdminController();
            List<CarrierTypeInfo> CarrierTypes = aCont.Get_CarrierTypesByPortalId(PortalId);
            foreach (CarrierTypeInfo CarrierType in CarrierTypes)
            {
                ddlAdvertiserCarrier.Items.Add(new ListItem(CarrierType.CarrierType, CarrierType.Id.ToString()));
            }
        }
        private void fillFreightTypes()
        {
            ddlAdvertiserCarrier.Items.Clear();
            ddlAdvertiserCarrier.Items.Add(new ListItem("--Please Select Freight Type--", "-1"));
            AdminController aCont = new AdminController();
            List<FreightTypeInfo> FreightTypes = aCont.Get_FreightTypesByPortalId(PortalId);
            foreach (FreightTypeInfo FreightType in FreightTypes)
            {
                ddlAdvertiserCarrier.Items.Add(new ListItem(FreightType.FreightType, FreightType.Id.ToString()));
            }
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
        private void fillTapeFormats()
        {
            lbxStationTapeFormat.Items.Clear();
            AdminController aCont = new AdminController();
            List<TapeFormatInfo> TapeFormats = aCont.Get_TapeFormatsByPortalId(PortalId);
            ddlLabelTapeFormat.Items.Add(new ListItem("--Please Select--", "-1"));
            foreach (TapeFormatInfo TapeFormat in TapeFormats)
            {
                lbxStationTapeFormat.Items.Add(new ListItem(TapeFormat.TapeFormat, TapeFormat.Id.ToString()));
                ddlLabelTapeFormat.Items.Add(new ListItem(TapeFormat.TapeFormat, TapeFormat.Id.ToString()));
            }
        }
        #endregion

        #region Agency Methods
        protected void btnSaveAgency_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            AgencyInfo agency = new AgencyInfo();
            agency.PortalId = PortalId;
            agency.LastModifiedById = UserId;
            agency.LastModifiedDate = DateTime.Now;
            agency.AgencyName = txtAgencyName.Text;
            agency.Address1 = txtAgencyAddress1.Text;
            agency.Address2 = txtAgencyAddress2.Text;
            agency.City = txtAgencyCity.Text;
            agency.State = txtAgencyState.Text;
            agency.Zip = txtAgencyZip.Text;
            agency.Country = txtAgencyCountry.Text;
            agency.Phone = txtAgencyPhone.Text;
            agency.Fax = txtAgencyFax.Text;
            agency.ClientType = Convert.ToInt16(ddlAgencyClientType.SelectedValue);
            agency.CustomerReference = txtAgencyCustomerReference.Text;
            if (ddlAgencyStatus.SelectedValue == "1")
            {
                //only set if true, false is default value
                agency.Status = true;
            }
            if(txtSelectedAgency.Value=="-1")
            {
                //save new Agency
                agency.CreatedById = UserId;
                agency.DateCreated = DateTime.Now;
                int agencyId = aCont.Add_Agency(agency);
                txtSelectedAgency.Value = agencyId.ToString();
                txtAgencyCreatedBy.Value = UserId.ToString();
                txtAgencyCreatedDate.Value = DateTime.Now.Ticks.ToString();
                lblAgencyMessage.Text = "Agency Saved.";
                btnDeleteAgency.Enabled = true;
                btnSaveAgencyAs.Enabled = true;
            }
            else
            {
                //update existing Agency
                agency.CreatedById = Convert.ToInt32(txtAgencyCreatedBy.Value);
                agency.DateCreated = new DateTime(Convert.ToInt64(txtAgencyCreatedDate.Value));
                agency.Id = Convert.ToInt32(txtSelectedAgency.Value);
                aCont.Update_Agency(agency);
                lblAgencyMessage.Text = "Agency Updated.";
            }
            FillAgencyList();
            fillDropDowns("Agency");
            try
            {
                ddlAgencyClientType.SelectedValue = agency.ClientType.ToString();
            }
            catch
            {
                ddlAgencyClientType.SelectedIndex = 0;
            }
            clearAll("Agency");
        }

        protected void btnSaveAgencyAs_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            AgencyInfo agency = new AgencyInfo();
            agency.PortalId = PortalId;
            agency.LastModifiedById = UserId;
            agency.LastModifiedDate = DateTime.Now;
            agency.AgencyName = txtAgencyName.Text;
            agency.Address1 = txtAgencyAddress1.Text;
            agency.Address2 = txtAgencyAddress2.Text;
            agency.City = txtAgencyCity.Text;
            agency.State = txtAgencyState.Text;
            agency.Zip = txtAgencyZip.Text;
            agency.Country = txtAgencyCountry.Text;
            agency.Phone = txtAgencyPhone.Text;
            agency.Fax = txtAgencyFax.Text;
            agency.ClientType = Convert.ToInt16(ddlAgencyClientType.SelectedValue);
            agency.CustomerReference = txtAgencyCustomerReference.Text;
            if (ddlAgencyStatus.SelectedValue == "1")
            {
                //only set if true, false is default value
                agency.Status = true;
            }
            //save new Agency
            agency.CreatedById = UserId;
            agency.DateCreated = DateTime.Now;
            int agencyId = aCont.Add_Agency(agency);
            txtSelectedAgency.Value = agencyId.ToString();
            txtAgencyCreatedBy.Value = UserId.ToString();
            txtAgencyCreatedDate.Value = DateTime.Now.Ticks.ToString();
            lblAgencyMessage.Text = "Agency Saved.";
            
            FillAgencyList();
            fillDropDowns("Agency");
            clearAll("Agency");
        }

        protected void btnDeleteAgency_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int AgencyId = Convert.ToInt32(txtSelectedAgency.Value);
            AgencyInfo agency = new AgencyInfo();
            agency.Id = AgencyId;
            aCont.Delete_Agency(agency);
            lblAgencyMessage.Text = "Agency Deleted.";
            FillAgencyList();
            fillDropDowns("");
            clearAgency();
        }

        protected void btnClearAgency_Click(object sender, System.EventArgs e)
        {
            clearAgency();
        }
        private void clearAgency()
        {
            txtAgencyAddress1.Text = "";
            txtAgencyAddress2.Text = "";
            txtAgencyCity.Text = "";
            txtAgencyCountry.Text = "";
            txtAgencyCreatedBy.Value = "-1";
            txtAgencyCreatedDate.Value = "";
            txtAgencyCustomerReference.Text = "";
            txtAgencyFax.Text = "";
            txtAgencyName.Text = "";
            txtAgencyPhone.Text = "";
            txtAgencyState.Text = "";
            txtAgencyZip.Text = "";
            txtSelectedAgency.Value = "-1";
            ddlAgencyClientType.SelectedIndex = 0;
            ddlAgencyStatus.SelectedIndex = 0;
            btnDeleteAgency.Enabled = false;
            btnSaveAgencyAs.Enabled = false;
            lblAgencyMessage.Text = "";
            txtAgencySearch.Text = "";
        }

        private void FillAgencyList()
        {
            AdminController aCont = new AdminController();
            List<AgencyInfo> agencies = aCont.Get_AgenciesByPortalId(PortalId);
            List<AgencyInfo> agenciesFiltered = new List<AgencyInfo>();
            if(txtAgencySearch.Text!="")
            {
                foreach(AgencyInfo agency in agencies)
                {
                    if(agency.AgencyName.ToLower().IndexOf(txtAgencySearch.Text.ToLower()) != -1)
                    {
                        agenciesFiltered.Add(agency);
                    }
                }
            }
            else
            {
                agenciesFiltered = agencies;
            }
            gvAgency.DataSource = agenciesFiltered;
            if(Session["AgencyPage"]!=null)
            {
                gvAgency.PageIndex = Convert.ToInt32(Session["AgencyPage"]);
            }
            lblAgencyMessage.Text = agenciesFiltered.Count.ToString() + " Agencies found.";
            gvAgency.DataBind();
            ddlStationGroupAgency.Items.Clear();
            ddlLabelAgencySearch.Items.Clear();
            ddlStationGroupAgency.Items.Add(new ListItem("--Select Agency--", "-1"));
            ddlLabelAgencySearch.Items.Add(new ListItem("--Select Agency--", "-1"));
            ddlLabelAgency.Items.Clear();
            ddlLabelAgency.Items.Add(new ListItem("--Select Agency--", "-1"));
            ddlMasterItemAgencySearch.Items.Add(new ListItem("--Select Agency--", "-1"));
            List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            foreach (AgencyInfo agency in agencies)
            {
                ddlStationGroupAgency.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                ddlLabelAgency.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                ddlLabelAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                lbxUserAgencies.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                if (isAdmin)
                {
                    ddlMasterItemAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                }
                else
                {
                    foreach (AgencyInfo ag in userAgs)
                    {
                        if (ag.Id == agency.Id)
                        {
                            ddlMasterItemAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                        }
                    }
                }
            }
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
            if (Session["StationGroupStationsPage"] != null)
            {
                gvStationGroupStations.PageIndex = Convert.ToInt32(Session["StationGroupStationsPage"]);
            }
            gvStationGroupStations.DataSource = stationsFiltered;
            gvStationGroupStations.DataBind();
            lblStationGroupStationsModalMessage.Text = stationsFiltered.Count.ToString() + " stations found.";
        }
        private void FillStationGroups()
        {
            AdminController aCont = new AdminController();
            //get station groupd for current user
            //may need to add all station groups for admins
            List<StationGroupInfo> stationGroups = aCont.Get_StationGroupsByUserId(UserId);
            gvStationGroup.DataSource = stationGroups;
            if (Session["StationGroupPage"] != null)
            {
                gvStationGroup.PageIndex = Convert.ToInt32(Session["StationGroupPage"]);
            }
            gvStationGroup.DataBind();
        }

        protected void gvAgency_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillAgencyList();
            AdminController aCont = new AdminController();
            txtSelectedAgency.Value = (gvAgency.SelectedRow.FindControl("hdngvAgencyId") as HiddenField).Value;
            AgencyInfo agency = aCont.Get_AgencyById(Convert.ToInt32(txtSelectedAgency.Value));
            if(agency.Id != -1)
            {
                btnDeleteAgency.Enabled = true;
                btnSaveAgencyAs.Enabled = true;
                txtAgencyAddress1.Text = agency.Address1;
                txtAgencyAddress2.Text = agency.Address2;
                txtAgencyCity.Text = agency.City;
                txtAgencyCountry.Text = agency.Country;
                txtAgencyCreatedBy.Value = agency.CreatedById.ToString();
                txtAgencyCreatedDate.Value = agency.DateCreated.Ticks.ToString();
                txtAgencyCustomerReference.Text = agency.CustomerReference;
                txtAgencyFax.Text = agency.Fax;
                txtAgencyName.Text = agency.AgencyName;
                txtAgencyPhone.Text = agency.Phone;
                txtAgencyState.Text = agency.State;
                txtAgencyZip.Text = agency.Zip;
                try
                {
                    ddlAgencyClientType.SelectedValue = agency.ClientType.ToString();
                }
                catch {
                    ddlAgencyClientType.SelectedIndex = 0;
                }
                if (agency.Status)
                {
                    ddlAgencyStatus.SelectedIndex = 1;
                }
                else
                {
                    ddlAgencyStatus.SelectedIndex = 2;
                }
                //lblAgencyMessage.Text = "";
            }
            else
            {
                btnDeleteAgency.Enabled = false;
                btnSaveAgencyAs.Enabled = false;
                lblAgencyMessage.Text = "There was an error loading this agency.";
            }
            clearAll("Agency");
        }

        protected void gvAgency_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["AgencyPage"] = e.NewPageIndex.ToString();
            FillAgencyList();
        }

        #endregion

        #region Advertiser Methods
        protected void btnSaveAdvertiser_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            AdvertiserInfo Advertiser = new AdvertiserInfo();
            Advertiser.PortalId = PortalId;
            Advertiser.LastModifiedById = UserId;
            Advertiser.LastModifiedDate = DateTime.Now;
            Advertiser.AdvertiserName = txtAdvertiserName.Text;
            Advertiser.Carrier = Convert.ToInt32(ddlAdvertiserCarrier.SelectedValue);
            Advertiser.Freight = Convert.ToInt32(ddlAdvertiserFreight.SelectedValue);
            Advertiser.Address1 = txtAdvertiserAddress1.Text;
            Advertiser.Address2 = txtAdvertiserAddress2.Text;
            Advertiser.City = txtAdvertiserCity.Text;
            Advertiser.State = txtAdvertiserState.Text;
            Advertiser.Zip = txtAdvertiserZip.Text;
            Advertiser.Country = txtAdvertiserCountry.Text;
            Advertiser.Phone = txtAdvertiserPhone.Text;
            Advertiser.Fax = txtAdvertiserFax.Text;
            Advertiser.ClientType = Convert.ToInt16(ddlAdvertiserClientType.SelectedValue);
            Advertiser.CustomerReference = txtAdvertiserCustomerReference.Text;
            if (txtSelectedAdvertiser.Value == "-1")
            {
                //save new Advertiser
                Advertiser.CreatedById = UserId;
                Advertiser.DateCreated = DateTime.Now;
                int AdvertiserId = aCont.Add_Advertiser(Advertiser);
                //save advertiseragencies
                List<AgencyInfo> ags = new List<AgencyInfo>();
                //aCont.Delete_AdvertiserAgencyByAdvertiserId();
                foreach (ListItem li in ddlAdvertiserAgency.Items)
                {
                    if(li.Selected)
                    {
                        aCont.Add_AdvertiserAgency(AdvertiserId, Convert.ToInt32(li.Value));
                    }
                }
                txtSelectedAdvertiser.Value = AdvertiserId.ToString();
                txtAdvertiserCreatedBy.Value = UserId.ToString();
                txtAdvertiserCreatedDate.Value = DateTime.Now.Ticks.ToString();
                lblAdvertiserMessage.Text = "Advertiser Saved.";
                btnDeleteAdvertiser.Enabled = true;
                btnSaveAdvertiserAs.Enabled = true;
            }
            else
            {
                //update existing Advertiser
                AdvertiserInfo adv = aCont.Get_AdvertiserById(Convert.ToInt32(txtSelectedAdvertiser.Value));
                Advertiser.QuickbooksListId = adv.QuickbooksListId;
                Advertiser.QuickbooksEditSequence = adv.QuickbooksEditSequence;
                Advertiser.QuickbooksErrNum = adv.QuickbooksErrNum;
                Advertiser.QuickbooksErrMsg = adv.QuickbooksErrMsg;
                Advertiser.CreatedById = Convert.ToInt32(txtAdvertiserCreatedBy.Value);
                Advertiser.DateCreated = new DateTime(Convert.ToInt64(txtAdvertiserCreatedDate.Value));
                Advertiser.Id = Convert.ToInt32(txtSelectedAdvertiser.Value);
                aCont.Update_Advertiser(Advertiser);

                //save advertiseragencies
                List<AgencyInfo> ags = new List<AgencyInfo>();
                aCont.Delete_AdvertiserAgencyByAdvertiserId(Advertiser.Id);
                foreach (ListItem li in ddlAdvertiserAgency.Items)
                {
                    if(li.Selected)
                    {
                        aCont.Add_AdvertiserAgency(Advertiser.Id, Convert.ToInt32(li.Value));
                    }
                }

                lblAdvertiserMessage.Text = "Advertiser Updated.";
            }
            FillAdvertiserList();
            fillDropDowns("Advertiser");
            try
            {
                ddlAdvertiserClientType.SelectedValue = Advertiser.ClientType.ToString();
            }
            catch
            {
                ddlAdvertiserClientType.SelectedIndex = 0;
            }
            try
            {
                ddlAdvertiserCarrier.SelectedValue = Advertiser.Carrier.ToString();
            }
            catch
            {
                ddlAdvertiserCarrier.SelectedIndex = 0;
            }
            try
            {
                ddlAdvertiserFreight.SelectedValue = Advertiser.Freight.ToString();
            }
            catch
            {
                ddlAdvertiserFreight.SelectedIndex = 0;
            }
        }

        protected void btnSaveAdvertiserAs_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            AdvertiserInfo Advertiser = new AdvertiserInfo();
            Advertiser.PortalId = PortalId;
            Advertiser.LastModifiedById = UserId;
            Advertiser.LastModifiedDate = DateTime.Now;
            Advertiser.AdvertiserName = txtAdvertiserName.Text;
            Advertiser.Carrier = Convert.ToInt32(ddlAdvertiserCarrier.SelectedValue);
            Advertiser.Freight = Convert.ToInt32(ddlAdvertiserFreight.SelectedValue);
            Advertiser.Address1 = txtAdvertiserAddress1.Text;
            Advertiser.Address2 = txtAdvertiserAddress2.Text;
            Advertiser.City = txtAdvertiserCity.Text;
            Advertiser.State = txtAdvertiserState.Text;
            Advertiser.Zip = txtAdvertiserZip.Text;
            Advertiser.Country = txtAdvertiserCountry.Text;
            Advertiser.Phone = txtAdvertiserPhone.Text;
            Advertiser.Fax = txtAdvertiserFax.Text;
            Advertiser.ClientType = Convert.ToInt16(ddlAdvertiserClientType.SelectedValue);
            Advertiser.CustomerReference = txtAdvertiserCustomerReference.Text;

            //save new Advertiser
            Advertiser.CreatedById = UserId;
            Advertiser.DateCreated = DateTime.Now;
            int AdvertiserId = aCont.Add_Advertiser(Advertiser);

            //save advertiseragencies
            List<AgencyInfo> ags = new List<AgencyInfo>();
            //aCont.Delete_AdvertiserAgencyByAdvertiserId();
            foreach (ListItem li in ddlAdvertiserAgency.Items)
            {
                if(li.Selected)
                {
                    aCont.Add_AdvertiserAgency(AdvertiserId, Convert.ToInt32(li.Value));
                }
            }

            txtSelectedAdvertiser.Value = AdvertiserId.ToString();
            txtAdvertiserCreatedBy.Value = UserId.ToString();
            txtAdvertiserCreatedDate.Value = DateTime.Now.Ticks.ToString();
            lblAdvertiserMessage.Text = "Advertiser Saved.";

            FillAdvertiserList();
            fillDropDowns("Advertiser");
            try
            {
                ddlAdvertiserCarrier.SelectedValue = Advertiser.Carrier.ToString();
            }
            catch
            {
                ddlAdvertiserCarrier.SelectedIndex = 0;
            }
            try
            {
                ddlAdvertiserFreight.SelectedValue = Advertiser.Freight.ToString();
            }
            catch
            {
                ddlAdvertiserFreight.SelectedIndex = 0;
            }
        }

        protected void btnDeleteAdvertiser_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int AdvertiserId = Convert.ToInt32(txtSelectedAdvertiser.Value);
            aCont.Delete_AdvertiserAgencyByAdvertiserId(AdvertiserId);
            AdvertiserInfo Advertiser = new AdvertiserInfo();
            Advertiser.Id = AdvertiserId;
            aCont.Delete_Advertiser(Advertiser);
            lblAdvertiserMessage.Text = "Advertiser Deleted.";
            FillAdvertiserList();
            fillDropDowns("");
            clearAdvertiser();
        }

        protected void btnClearAdvertiser_Click(object sender, System.EventArgs e)
        {
            clearAdvertiser();
        }
        private void clearAdvertiser()
        {
            ddlAdvertiserAgency.ClearSelection();
            ddlAdvertiserCarrier.SelectedIndex = 0;
            ddlAdvertiserClientType.SelectedIndex = 0;
            ddlAdvertiserFreight.SelectedIndex = 0;
            txtAdvertiserAddress1.Text = "";
            txtAdvertiserAddress2.Text = "";
            txtAdvertiserCity.Text = "";
            txtAdvertiserCountry.Text = "";
            txtAdvertiserCreatedBy.Value = "-1";
            txtAdvertiserCreatedDate.Value = "";
            txtAdvertiserCustomerReference.Text = "";
            txtAdvertiserFax.Text = "";
            txtAdvertiserName.Text = "";
            txtAdvertiserPhone.Text = "";
            txtAdvertiserState.Text = "";
            txtAdvertiserZip.Text = "";
            txtSelectedAdvertiser.Value = "-1";
            ddlAdvertiserClientType.SelectedIndex = 0;
            ddlAdvertiserAgencySearch.SelectedIndex = 0;
            txtAdvertiserSearch.Text = "";
            lblAdvertiserMessage.Text = "";
            btnDeleteAdvertiser.Enabled = false;
            btnSaveAdvertiserAs.Enabled = false;
        }

        private void FillAdvertiserList()
        {
            AdminController aCont = new AdminController();
            List<AdvertiserInfo> Advertisers = aCont.Get_AdvertisersByPortalId(PortalId);
            List<AdvertiserInfo> advertisersByAgency = new List<AdvertiserInfo>();
            List<AdvertiserInfo> advertisersFiltered = new List<AdvertiserInfo>();
            if(ddlAdvertiserAgencySearch.SelectedIndex==0)
            {
                advertisersByAgency = Advertisers;
            }
            else
            {
                foreach(AdvertiserInfo advertiser in Advertisers)
                {
                   foreach(AgencyInfo ag in advertiser.Agencies)
                   {
                       if(ag.Id.ToString()==ddlAdvertiserAgencySearch.SelectedValue)
                       {
                           advertisersByAgency.Add(advertiser);
                       }
                   }
                }
            }
            if(txtAdvertiserSearch.Text=="")
            {
                advertisersFiltered = advertisersByAgency;
            }
            else
            {
                foreach(AdvertiserInfo advertiser in advertisersByAgency)
                {
                    if(advertiser.AdvertiserName.ToLower().IndexOf(txtAdvertiserSearch.Text.ToLower()) != -1)
                    {
                        advertisersFiltered.Add(advertiser);
                    }
                }
            }
            lblAdvertiserMessage.Text = advertisersFiltered.Count.ToString() + " Advertisers found.";
            gvAdvertiser.DataSource = advertisersFiltered;
            if (Session["AdvertiserPage"] != null)
            {
                gvAdvertiser.PageIndex = Convert.ToInt32(Session["AdvertiserPage"]);
            }
            gvAdvertiser.DataBind();
            ddlLabelAdvertiser.Items.Clear();
            ddlLabelAdvertiserSearch.Items.Clear(); 
            ddlMasterItemAdvertiser.Items.Clear();
            ddlLabelAdvertiser.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            ddlLabelAdvertiserSearch.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            ddlMasterItemAdvertiser.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            ddlMasterItemAdvertiserSearch.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            foreach(AdvertiserInfo adv in Advertisers)
            {
                ddlLabelAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                ddlLabelAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                ddlMasterItemAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    lbxUserAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                if (isAdmin)
                {
                    ddlMasterItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                }
                else
                {
                    foreach (AdvertiserInfo ad in userAds)
                    {
                        if (ad.Id == adv.Id)
                        {
                            ddlMasterItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                }
            }
            fillMasterItemAgencies();
        }

        protected void gvAdvertiser_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillAdvertiserList();
            AdminController aCont = new AdminController();
            txtSelectedAdvertiser.Value = (gvAdvertiser.SelectedRow.FindControl("hdngvAdvertiserId") as HiddenField).Value;
            AdvertiserInfo Advertiser = aCont.Get_AdvertiserById(Convert.ToInt32(txtSelectedAdvertiser.Value));
            if (Advertiser.Id != -1)
            {
                btnDeleteAdvertiser.Enabled = true;
                btnSaveAdvertiserAs.Enabled = true;
                //ddlAdvertiserAgency.SelectedValue = Advertiser.Agency.ToString();
                ddlAdvertiserAgency.ClearSelection();
                foreach(ListItem li in ddlAdvertiserAgency.Items)
                {
                    foreach(AgencyInfo adv in Advertiser.Agencies)
                    {
                        if(li.Value==adv.Id.ToString())
                        {
                            li.Selected = true;
                        }
                    }
                }
                try
                {
                    ddlAdvertiserCarrier.SelectedValue = Advertiser.Carrier.ToString();
                }
                catch
                {
                    ddlAdvertiserCarrier.SelectedIndex = 0;
                }
                try
                {
                    ddlAdvertiserFreight.SelectedValue = Advertiser.Freight.ToString();
                }
                catch
                {
                    ddlAdvertiserFreight.SelectedIndex = 0;
                }
                txtAdvertiserAddress1.Text = Advertiser.Address1;
                txtAdvertiserAddress2.Text = Advertiser.Address2;
                txtAdvertiserCity.Text = Advertiser.City;
                txtAdvertiserCountry.Text = Advertiser.Country;
                txtAdvertiserCreatedBy.Value = Advertiser.CreatedById.ToString();
                txtAdvertiserCreatedDate.Value = Advertiser.DateCreated.Ticks.ToString();
                txtAdvertiserCustomerReference.Text = Advertiser.CustomerReference;
                txtAdvertiserFax.Text = Advertiser.Fax;
                txtAdvertiserName.Text = Advertiser.AdvertiserName;
                txtAdvertiserPhone.Text = Advertiser.Phone;
                txtAdvertiserState.Text = Advertiser.State;
                txtAdvertiserZip.Text = Advertiser.Zip;
                try
                {
                    ddlAdvertiserClientType.SelectedValue = Advertiser.ClientType.ToString();
                }
                catch {
                    ddlAdvertiserClientType.SelectedIndex = 0;
                }
                //lblAdvertiserMessage.Text = "";
            }
            else
            {
                btnDeleteAdvertiser.Enabled = false;
                btnSaveAdvertiserAs.Enabled = false;
                clearAdvertiser();
                lblAdvertiserMessage.Text = "There was an error loading this Advertiser.";
            }
            clearAll("Advertiser");
        }

        protected void gvAdvertiser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["AdvertiserPage"] = e.NewPageIndex.ToString();
            FillAdvertiserList();
            //gvAdvertiser.PageIndex = e.NewPageIndex;
            //gvAdvertiser.DataBind();
        }
        #endregion

        #region Market Methods
        protected void btnSaveMarket_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            MarketInfo Market = new MarketInfo();
            Market.PortalId = PortalId;
            Market.LastModifiedById = UserId;
            Market.LastModifiedDate = DateTime.Now;
            Market.MarketName = txtMarketName.Text;
            Market.Description = txtMarketDescription.Text;
            Market.ParentId = Convert.ToInt32(ddlMarketParent.SelectedValue);
            if (txtSelectedMarket.Value == "-1")
            {
                //save new Market
                Market.CreatedById = UserId;
                Market.DateCreated = DateTime.Now;
                int MarketId = aCont.Add_Market(Market);
                txtSelectedMarket.Value = MarketId.ToString();
                txtMarketCreatedBy.Value = UserId.ToString();
                txtMarketCreatedDate.Value = DateTime.Now.Ticks.ToString();
                lblMarketMessage.Text = "Market Saved.";
                btnDeleteMarket.Enabled = true;
                btnSaveMarketAs.Enabled = true;
            }
            else
            {
                //update existing Market
                Market.CreatedById = Convert.ToInt32(txtMarketCreatedBy.Value);
                Market.DateCreated = new DateTime(Convert.ToInt64(txtMarketCreatedDate.Value));
                Market.Id = Convert.ToInt32(txtSelectedMarket.Value);
                aCont.Update_Market(Market);
                lblMarketMessage.Text = "Market Updated.";
            }
            FillMarketList();
            fillDropDowns("");
            fillMarketParents();
            fillStationMarketsList();
            ddlMarketParent.SelectedValue = Market.ParentId.ToString();
        }

        protected void btnSaveMarketAs_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            MarketInfo Market = new MarketInfo();
            Market.PortalId = PortalId;
            Market.LastModifiedById = UserId;
            Market.LastModifiedDate = DateTime.Now;
            Market.MarketName = txtMarketName.Text;
            Market.Description = txtMarketDescription.Text;
            Market.ParentId = Convert.ToInt32(ddlMarketParent.SelectedValue);

            //save new Market
            Market.CreatedById = UserId;
            Market.DateCreated = DateTime.Now;
            int MarketId = aCont.Add_Market(Market);
            txtSelectedMarket.Value = MarketId.ToString();
            txtMarketCreatedBy.Value = UserId.ToString();
            txtMarketCreatedDate.Value = DateTime.Now.Ticks.ToString();
            lblMarketMessage.Text = "Market Saved.";

            FillMarketList();
            FillMarket2List();
            fillDropDowns("");
            fillMarketParents();
            FillMarket2List();
            fillStationMarketsList();
            ddlMarketParent.SelectedValue = Market.ParentId.ToString();
        }

        protected void btnDeleteMarket_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int MarketId = Convert.ToInt32(txtSelectedMarket.Value);
            MarketInfo Market = new MarketInfo();
            Market.Id = MarketId;
            aCont.Delete_Market(Market);
            lblMarketMessage.Text = "Market Deleted.";
            FillMarketList();
            FillMarket2List();
            fillDropDowns("");
            fillMarketParents();
            fillMarket2Parents();
            clearMarket();
        }

        protected void btnClearMarket_Click(object sender, System.EventArgs e)
        {
            clearMarket();
        }
        private void clearMarket()
        {
            txtMarketName.Text = "";
            txtSelectedMarket.Value = "-1";
            txtMarketCreatedBy.Value = "";
            txtMarketCreatedDate.Value = "";
            txtMarketDescription.Text = "";
            btnDeleteMarket.Enabled = false;
            btnSaveMarketAs.Enabled = false;
            lblMarketMessage.Text = "";
            fillMarketParents();
        }

        private void FillMarketList()
        {
            AdminController aCont = new AdminController();
            List<MarketInfo> Markets = aCont.Get_MarketsByPortalId(PortalId);
            List<MarketInfo> marketsFiltered = new List<MarketInfo>();
            string term = txtMarketSearch.Text.ToLower();
            foreach (MarketInfo market in Markets)
            {
                if (market.MarketName.ToLower().IndexOf(term) != -1 || market.Description.ToLower().IndexOf(term) != -1)
                {
                    marketsFiltered.Add(market);
                }
            }
            gvMarket.DataSource = marketsFiltered;
            gvMarket2.DataSource = Markets;
            lblMarketMessage.Text = marketsFiltered.Count.ToString() + " Markets found.";
            if (Session["MarketPage"] != null)
            {
                gvMarket.PageIndex = Convert.ToInt32(Session["MarketPage"]);
            }
            if (Session["Market2Page"] != null)
            {
                gvMarket2.PageIndex = Convert.ToInt32(Session["Market2Page"]);
            }
            gvMarket.DataBind();
            gvMarket2.DataBind();

            //fill the markets in Station Market search
            ddlStationMarketSearch.Items.Clear();
            ddlStationMarketSearch.Items.Add(new ListItem("--Please Select--", "-1"));
            foreach(MarketInfo market in Markets)
            {
                ddlStationMarketSearch.Items.Add(new ListItem(market.MarketName, market.Id.ToString()));
            }
        }

        protected void gvMarket_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillMarketList();
            AdminController aCont = new AdminController();
            txtSelectedMarket.Value = (gvMarket.SelectedRow.FindControl("hdngvMarketId") as HiddenField).Value;
            fillMarketParents();
            MarketInfo Market = aCont.Get_MarketById(Convert.ToInt32(txtSelectedMarket.Value));
            if (Market.Id != -1)
            {
                btnDeleteMarket.Enabled = true;
                btnSaveMarketAs.Enabled = true;
                txtMarketCreatedBy.Value = Market.CreatedById.ToString();
                txtMarketCreatedDate.Value = Market.DateCreated.Ticks.ToString();
                txtMarketName.Text = Market.MarketName;
                txtMarketDescription.Text = Market.Description;
                try
                {
                    ddlMarketParent.SelectedValue = Market.ParentId.ToString();
                }
                catch { }
                //lblMarketMessage.Text = "";
            }
            else
            {
                btnDeleteMarket.Enabled = false;
                btnSaveMarketAs.Enabled = false;
                clearMarket();
                lblMarketMessage.Text = "There was an error loading this Market.";
            }
            clearAll("Market");
        }

        protected void gvMarket_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["MarketPage"] = e.NewPageIndex.ToString();
            gvMarket.PageIndex = e.NewPageIndex;
            gvMarket.DataBind();
        }

        protected void btnMarketSearch_Click(object sender, System.EventArgs e)
        {
            FillMarketList();
            //AdminController aCont = new AdminController();
            //List<MarketInfo> markets = aCont.Get_MarketsByPortalId(PortalId);
            //List<MarketInfo> marketsFiltered = new List<MarketInfo>();
            //string term = txtMarketSearch.Text.ToLower();
            //foreach(MarketInfo market in markets)
            //{
            //    if (market.MarketName.ToLower().IndexOf(term) != -1 || market.Description.ToLower().IndexOf(term) != -1)
            //    {
            //        marketsFiltered.Add(market);
            //    }
            //}
            //gvMarket.DataSource = marketsFiltered;
            //gvMarket.DataBind();
            //lblMarketMessage.Text = marketsFiltered.Count.ToString() + " Result(s) found for " + term + ".";
        }

        protected void btnMarketSearchClear_Click(object sender, System.EventArgs e)
        {
            txtMarketSearch.Text = "";
            FillMarketList();
            clearMarket();
        }
        private void fillMarketParents()
        {
            ddlMarketParent.Items.Clear();
            ddlMarket2Parent.Items.Clear();
            ddlStationGroupMarketSearch.Items.Clear();
            AdminController aCont = new AdminController();
            List<MarketInfo> markets = aCont.Get_MarketsByPortalId(PortalId);
            ddlMarketParent.Items.Add(new ListItem("--Select Parent if Appropriate--", "-1"));
            ddlMarket2Parent.Items.Add(new ListItem("--Select Parent if Appropriate--", "-1"));
            ddlStationGroupMarketSearch.Items.Add(new ListItem("--Select Market--", "-1"));
            foreach(MarketInfo market in markets)
            {
                ddlStationGroupMarketSearch.Items.Add(new ListItem(market.MarketName, market.Id.ToString()));
                if(market.Id.ToString() != txtSelectedMarket.Value)
                {
                    ddlMarketParent.Items.Add(new ListItem(market.MarketName, market.Id.ToString()));
                }
                if (market.Id.ToString() != txtSelectedMarket2.Value)
                {
                    ddlMarket2Parent.Items.Add(new ListItem(market.MarketName, market.Id.ToString()));
                }
            }
            try
            {
                ddlMarketParent.SelectedValue = txtSelectedMarket.Value.ToString();
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
            FillMarketList();
            fillDropDowns("");
            fillMarket2Parents();
            fillMarketParents();
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
            FillMarketList();
            fillDropDowns("");
            fillMarket2Parents();
            fillMarketParents();
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
            fillDropDowns("");
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
            gvMarket2.DataSource = Market2s;
            if (Session["Market2Page"] != null)
            {
                gvMarket2.PageIndex = Convert.ToInt32(Session["Market2Page"]);
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
            clearAll("Market");
        }

        protected void gvMarket2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["Market2Page"] = e.NewPageIndex.ToString();
            gvMarket2.PageIndex = e.NewPageIndex;
            gvMarket2.DataBind();
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
        #endregion

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
            string tapeFormat = "";
            foreach(ListItem li in lbxStationTapeFormat.Items)
            {
                if(li.Selected)
                {
                    tapeFormat += li.Value + ",";
                }
            }
            tapeFormat = tapeFormat.Substring(0, tapeFormat.Length - 1);
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
            deliveryMethod = deliveryMethod.Substring(0, deliveryMethod.Length - 1);
            Station.DeliveryMethods = deliveryMethod;
            if(ddlStationOnline.SelectedValue=="1")
            {
                Station.Online = true;
            }
            else
            {
                Station.Online = false;
            }
            if(ddlStationStatus.SelectedValue=="1")
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
            FillStations();
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
            string tapeFormat = "";
            foreach (ListItem li in lbxStationTapeFormat.Items)
            {
                if (li.Selected)
                {
                    tapeFormat += li.Value + ",";
                }
            }
            tapeFormat = tapeFormat.Substring(0, tapeFormat.Length - 1);
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
            deliveryMethod = deliveryMethod.Substring(0, deliveryMethod.Length - 1);
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
            FillStations();
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
            fillDropDowns("");
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
            btnDeleteStation.Enabled = false;
            btnSaveStationAs.Enabled = false;
            lblStationMessage.Text = "";
        }

        private void FillStationList()
        {
            AdminController aCont = new AdminController();
            List<StationInfo> Stations = aCont.Get_StationsByPortalId(PortalId);
            List<StationInfo> stationsInMarket = new List<StationInfo>();
            List<StationInfo> stationsFiltered = new List<StationInfo>();
            if (ddlStationMarketSearch.SelectedIndex == 0)
            {
                stationsInMarket = Stations;
            }
            else
            {
                foreach(StationInfo station in Stations)
                {
                    if(station.MarketId.ToString()==ddlStationMarketSearch.SelectedValue)
                    {
                        stationsInMarket.Add(station);
                    }
                }
            }
            if(txtStationSearch.Text=="")
            {
                stationsFiltered = stationsInMarket;
            }
            else
            {
                foreach(StationInfo station in stationsInMarket)
                {
                    if(station.StationName.ToLower().IndexOf(txtStationSearch.Text.ToLower()) != -1
                    || station.CallLetter.ToLower().IndexOf(txtStationSearch.Text.ToLower()) != -1)
                    {
                        stationsFiltered.Add(station);
                    }
                }
            }
            lblStationMessage.Text = stationsFiltered.Count.ToString() + " Stations found.";
            gvStation.DataSource = stationsFiltered;
            if (Session["StationPage"] != null)
            {
                gvStation.PageIndex = Convert.ToInt32(Session["StationPage"]);
            }
            gvStation.DataBind();
        }

        protected void gvStation_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillStationList();
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
                foreach(string format in tapeFormats)
                {
                    foreach(ListItem li in lbxStationTapeFormat.Items)
                    {
                        if(format==li.Value)
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
                if(Station.Online)
                {
                    ddlStationOnline.SelectedValue = "1";
                }
                else if(!Station.Online)
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
                //lblStationMessage.Text = "";
            }
            else
            {
                btnDeleteStation.Enabled = false;
                btnSaveStationAs.Enabled = false;
                clearStation();
                lblStationMessage.Text = "There was an error loading this Station.";
            }
            clearAll("Station");
        }

        protected void gvStation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["StationPage"] = e.NewPageIndex.ToString();
            FillStationList();
            //gvStation.PageIndex = e.NewPageIndex;
            //gvStation.DataBind();
        }
        #endregion

        #region ClientType Methods
        protected void btnSaveClientType_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            ClientTypeInfo ClientType = new ClientTypeInfo();
            ClientType.PortalId = PortalId;
            ClientType.LastModifiedById = UserId;
            ClientType.LastModifiedDate = DateTime.Now;
            ClientType.ClientType = txtClientType.Text;
            if (txtSelectedClientType.Value == "-1")
            {
                //save new ClientType
                ClientType.CreatedById = UserId;
                ClientType.DateCreated = DateTime.Now;
                int ClientTypeId = aCont.Add_ClientType(ClientType);
                txtSelectedClientType.Value = ClientTypeId.ToString();
                txtClientTypeCreatedBy.Value = UserId.ToString();
                txtClientTypeCreatedDate.Value = DateTime.Now.Ticks.ToString();
                lblClientTypeMessage.Text = "Client Type Saved.";
                btnDeleteClientType.Enabled = true;
                btnSaveClientTypeAs.Enabled = true;
            }
            else
            {
                //update existing ClientType
                ClientType.CreatedById = Convert.ToInt32(txtClientTypeCreatedBy.Value);
                ClientType.DateCreated = new DateTime(Convert.ToInt64(txtClientTypeCreatedDate.Value));
                ClientType.Id = Convert.ToInt32(txtSelectedClientType.Value);
                aCont.Update_ClientType(ClientType);
                lblClientTypeMessage.Text = "ClientType Updated.";
            }
            fillClientTypes();
            FillClientTypeList();
            fillDropDowns("");
        }

        protected void btnSaveClientTypeAs_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            ClientTypeInfo ClientType = new ClientTypeInfo();
            ClientType.PortalId = PortalId;
            ClientType.LastModifiedById = UserId;
            ClientType.LastModifiedDate = DateTime.Now;
            ClientType.ClientType = txtClientType.Text;

            //save new ClientType
            ClientType.CreatedById = UserId;
            ClientType.DateCreated = DateTime.Now;
            int ClientTypeId = aCont.Add_ClientType(ClientType);
            txtSelectedClientType.Value = ClientTypeId.ToString();
            txtClientTypeCreatedBy.Value = UserId.ToString();
            txtClientTypeCreatedDate.Value = DateTime.Now.Ticks.ToString();
            lblClientTypeMessage.Text = "Client Type Saved.";

            fillClientTypes();
            FillClientTypeList();
            fillDropDowns("");
        }

        protected void btnDeleteClientType_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int ClientTypeId = Convert.ToInt32(txtSelectedClientType.Value);
            ClientTypeInfo ClientType = new ClientTypeInfo();
            ClientType.Id = ClientTypeId;
            aCont.Delete_ClientType(ClientType);
            lblClientTypeMessage.Text = "Client Type Deleted.";
            fillClientTypes();
            FillClientTypeList();
            clearClientType();
            fillDropDowns("");
        }

        protected void btnClearClientType_Click(object sender, System.EventArgs e)
        {
            clearClientType();
        }
        private void clearClientType()
        {
            txtClientType.Text = "";
            txtSelectedClientType.Value = "-1";
            txtClientTypeCreatedBy.Value = "";
            txtClientTypeCreatedDate.Value = "";
            btnDeleteClientType.Enabled = false;
            btnSaveClientTypeAs.Enabled = false;
            lblClientTypeMessage.Text = "";
        }

        private void FillClientTypeList()
        {
            AdminController aCont = new AdminController();
            List<ClientTypeInfo> ClientTypes = aCont.Get_ClientTypesByPortalId(PortalId);
            gvClientType.DataSource = ClientTypes;
            if (Session["ClientTypePage"] != null)
            {
                gvClientType.PageIndex = Convert.ToInt32(Session["ClientTypePage"]);
            }
            gvClientType.DataBind();
        }

        protected void gvClientType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            txtSelectedClientType.Value = (gvClientType.SelectedRow.FindControl("hdngvClientTypeId") as HiddenField).Value;
            ClientTypeInfo ClientType = aCont.Get_ClientTypeById(Convert.ToInt32(txtSelectedClientType.Value));
            if (ClientType.Id != -1)
            {
                btnDeleteClientType.Enabled = true;
                btnSaveClientTypeAs.Enabled = true;
                txtClientTypeCreatedBy.Value = ClientType.CreatedById.ToString();
                txtClientTypeCreatedDate.Value = ClientType.DateCreated.Ticks.ToString();
                txtClientType.Text = ClientType.ClientType;
                lblClientTypeMessage.Text = "";
            }
            else
            {
                btnDeleteClientType.Enabled = false;
                btnSaveClientTypeAs.Enabled = false;
                clearClientType();
                lblClientTypeMessage.Text = "There was an error loading this Client Type.";
            }
        }

        protected void gvClientType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["ClientTypePage"] = e.NewPageIndex.ToString();
            gvClientType.PageIndex = e.NewPageIndex;
            gvClientType.DataBind();
        }
        #endregion

        #region CarrierType Methods
        protected void btnSaveCarrierType_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            CarrierTypeInfo CarrierType = new CarrierTypeInfo();
            CarrierType.PortalId = PortalId;
            CarrierType.LastModifiedById = UserId;
            CarrierType.LastModifiedDate = DateTime.Now;
            CarrierType.CarrierType = txtCarrierType.Text;
            if (txtSelectedCarrierType.Value == "-1")
            {
                //save new CarrierType
                CarrierType.CreatedById = UserId;
                CarrierType.DateCreated = DateTime.Now;
                int CarrierTypeId = aCont.Add_CarrierType(CarrierType);
                txtSelectedCarrierType.Value = CarrierTypeId.ToString();
                txtCarrierTypeCreatedBy.Value = UserId.ToString();
                txtCarrierTypeCreatedDate.Value = DateTime.Now.Ticks.ToString();
                lblCarrierTypeMessage.Text = "Client Type Saved.";
                btnDeleteCarrierType.Enabled = true;
                btnSaveCarrierTypeAs.Enabled = true;
            }
            else
            {
                //update existing CarrierType
                CarrierType.CreatedById = Convert.ToInt32(txtCarrierTypeCreatedBy.Value);
                CarrierType.DateCreated = new DateTime(Convert.ToInt64(txtCarrierTypeCreatedDate.Value));
                CarrierType.Id = Convert.ToInt32(txtSelectedCarrierType.Value);
                aCont.Update_CarrierType(CarrierType);
                lblCarrierTypeMessage.Text = "CarrierType Updated.";
            }
            fillDropDowns("");
            fillCarrierTypes();
            FillCarrierTypeList();
        }

        protected void btnSaveCarrierTypeAs_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            CarrierTypeInfo CarrierType = new CarrierTypeInfo();
            CarrierType.PortalId = PortalId;
            CarrierType.LastModifiedById = UserId;
            CarrierType.LastModifiedDate = DateTime.Now;
            CarrierType.CarrierType = txtCarrierType.Text;

            //save new CarrierType
            CarrierType.CreatedById = UserId;
            CarrierType.DateCreated = DateTime.Now;
            int CarrierTypeId = aCont.Add_CarrierType(CarrierType);
            txtSelectedCarrierType.Value = CarrierTypeId.ToString();
            txtCarrierTypeCreatedBy.Value = UserId.ToString();
            txtCarrierTypeCreatedDate.Value = DateTime.Now.Ticks.ToString();
            lblCarrierTypeMessage.Text = "Client Type Saved.";

            fillDropDowns("");
            fillCarrierTypes();
            FillCarrierTypeList();
        }

        protected void btnDeleteCarrierType_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int CarrierTypeId = Convert.ToInt32(txtSelectedCarrierType.Value);
            CarrierTypeInfo CarrierType = new CarrierTypeInfo();
            CarrierType.Id = CarrierTypeId;
            aCont.Delete_CarrierType(CarrierType);
            lblCarrierTypeMessage.Text = "Client Type Deleted.";
            fillCarrierTypes();
            FillCarrierTypeList();
            clearCarrierType();
            fillDropDowns("");
        }

        protected void btnClearCarrierType_Click(object sender, System.EventArgs e)
        {
            clearCarrierType();
        }
        private void clearCarrierType()
        {
            txtCarrierType.Text = "";
            txtSelectedCarrierType.Value = "-1";
            txtCarrierTypeCreatedBy.Value = "";
            txtCarrierTypeCreatedDate.Value = "";
            btnDeleteCarrierType.Enabled = false;
            btnSaveCarrierTypeAs.Enabled = false;
            lblCarrierTypeMessage.Text = "";
        }

        private void FillCarrierTypeList()
        {
            AdminController aCont = new AdminController();
            List<CarrierTypeInfo> CarrierTypes = aCont.Get_CarrierTypesByPortalId(PortalId);
            gvCarrierType.DataSource = CarrierTypes;
            if (Session["CarrierTypePage"] != null)
            {
                gvCarrierType.PageIndex = Convert.ToInt32(Session["CarrierTypePage"]);
            }
            gvCarrierType.DataBind();
        }

        protected void gvCarrierType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            txtSelectedCarrierType.Value = (gvCarrierType.SelectedRow.FindControl("hdngvCarrierTypeId") as HiddenField).Value;
            CarrierTypeInfo CarrierType = aCont.Get_CarrierTypeById(Convert.ToInt32(txtSelectedCarrierType.Value));
            if (CarrierType.Id != -1)
            {
                btnDeleteCarrierType.Enabled = true;
                btnSaveCarrierTypeAs.Enabled = true;
                txtCarrierTypeCreatedBy.Value = CarrierType.CreatedById.ToString();
                txtCarrierTypeCreatedDate.Value = CarrierType.DateCreated.Ticks.ToString();
                txtCarrierType.Text = CarrierType.CarrierType;
                lblCarrierTypeMessage.Text = "";
            }
            else
            {
                btnDeleteCarrierType.Enabled = false;
                btnSaveCarrierTypeAs.Enabled = false;
                clearCarrierType();
                lblCarrierTypeMessage.Text = "There was an error loading this Client Type.";
            }
        }

        protected void gvCarrierType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["CarrierTypePage"] = e.NewPageIndex.ToString();
            gvCarrierType.PageIndex = e.NewPageIndex;
            gvCarrierType.DataBind();
        }
        #endregion
        
        #region FreightType Methods
        protected void btnSaveFreightType_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            FreightTypeInfo FreightType = new FreightTypeInfo();
            FreightType.PortalId = PortalId;
            FreightType.LastModifiedById = UserId;
            FreightType.LastModifiedDate = DateTime.Now;
            FreightType.FreightType = txtFreightType.Text;
            if (txtSelectedFreightType.Value == "-1")
            {
                //save new FreightType
                FreightType.CreatedById = UserId;
                FreightType.DateCreated = DateTime.Now;
                int FreightTypeId = aCont.Add_FreightType(FreightType);
                txtSelectedFreightType.Value = FreightTypeId.ToString();
                txtFreightTypeCreatedBy.Value = UserId.ToString();
                txtFreightTypeCreatedDate.Value = DateTime.Now.Ticks.ToString();
                lblFreightTypeMessage.Text = "Freight Type Saved.";
                btnDeleteFreightType.Enabled = true;
                btnSaveFreightTypeAs.Enabled = true;
            }
            else
            {
                //update existing FreightType
                FreightType.CreatedById = Convert.ToInt32(txtFreightTypeCreatedBy.Value);
                FreightType.DateCreated = new DateTime(Convert.ToInt64(txtFreightTypeCreatedDate.Value));
                FreightType.Id = Convert.ToInt32(txtSelectedFreightType.Value);
                aCont.Update_FreightType(FreightType);
                lblFreightTypeMessage.Text = "FreightType Updated.";
            }
            fillFreightTypes();
            FillFreightTypeList();
            fillDropDowns("");
        }

        protected void btnSaveFreightTypeAs_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            FreightTypeInfo FreightType = new FreightTypeInfo();
            FreightType.PortalId = PortalId;
            FreightType.LastModifiedById = UserId;
            FreightType.LastModifiedDate = DateTime.Now;
            FreightType.FreightType = txtFreightType.Text;

            //save new FreightType
            FreightType.CreatedById = UserId;
            FreightType.DateCreated = DateTime.Now;
            int FreightTypeId = aCont.Add_FreightType(FreightType);
            txtSelectedFreightType.Value = FreightTypeId.ToString();
            txtFreightTypeCreatedBy.Value = UserId.ToString();
            txtFreightTypeCreatedDate.Value = DateTime.Now.Ticks.ToString();
            lblFreightTypeMessage.Text = "Freight Type Saved.";

            fillFreightTypes();
            FillFreightTypeList();
            fillDropDowns("");
        }

        protected void btnDeleteFreightType_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int FreightTypeId = Convert.ToInt32(txtSelectedFreightType.Value);
            FreightTypeInfo FreightType = new FreightTypeInfo();
            FreightType.Id = FreightTypeId;
            aCont.Delete_FreightType(FreightType);
            lblFreightTypeMessage.Text = "Freight Type Deleted.";
            fillFreightTypes();
            FillFreightTypeList();
            clearFreightType();
            fillDropDowns("");
        }

        protected void btnClearFreightType_Click(object sender, System.EventArgs e)
        {
            clearFreightType();
        }
        private void clearFreightType()
        {
            txtFreightType.Text = "";
            txtSelectedFreightType.Value = "-1";
            txtFreightTypeCreatedBy.Value = "";
            txtFreightTypeCreatedDate.Value = "";
            btnDeleteFreightType.Enabled = false;
            btnSaveFreightTypeAs.Enabled = false;
            lblFreightTypeMessage.Text = "";
        }

        private void FillFreightTypeList()
        {
            AdminController aCont = new AdminController();
            List<FreightTypeInfo> FreightTypes = aCont.Get_FreightTypesByPortalId(PortalId);
            gvFreightType.DataSource = FreightTypes;
            if (Session["FreightTypePage"] != null)
            {
                gvFreightType.PageIndex = Convert.ToInt32(Session["FreightTypePage"]);
            }
            gvFreightType.DataBind();
        }

        protected void gvFreightType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            txtSelectedFreightType.Value = (gvFreightType.SelectedRow.FindControl("hdngvFreightTypeId") as HiddenField).Value;
            FreightTypeInfo FreightType = aCont.Get_FreightTypeById(Convert.ToInt32(txtSelectedFreightType.Value));
            if (FreightType.Id != -1)
            {
                btnDeleteFreightType.Enabled = true;
                btnSaveFreightTypeAs.Enabled = true;
                txtFreightTypeCreatedBy.Value = FreightType.CreatedById.ToString();
                txtFreightTypeCreatedDate.Value = FreightType.DateCreated.Ticks.ToString();
                txtFreightType.Text = FreightType.FreightType;
                lblFreightTypeMessage.Text = "";
            }
            else
            {
                btnDeleteFreightType.Enabled = false;
                btnSaveFreightTypeAs.Enabled = false;
                clearFreightType();
                lblFreightTypeMessage.Text = "There was an error loading this Freight Type.";
            }
        }

        protected void gvFreightType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["FreightTypePage"] = e.NewPageIndex.ToString();
            gvFreightType.PageIndex = e.NewPageIndex;
            gvFreightType.DataBind();
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
            fillDropDowns("");
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
            fillDropDowns("");
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
            fillDropDowns("");
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
        }

        private void FillTapeFormatList()
        {
            AdminController aCont = new AdminController();
            List<TapeFormatInfo> TapeFormats = aCont.Get_TapeFormatsByPortalId(PortalId);
            gvTapeFormat.DataSource = TapeFormats;
            if (Session["TapeFormatPage"] != null)
            {
                gvTapeFormat.PageIndex = Convert.ToInt32(Session["TapeFormatPage"]);
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
            Session["TapeFormatPage"] = e.NewPageIndex.ToString();
            gvTapeFormat.PageIndex = e.NewPageIndex;
            gvTapeFormat.DataBind();
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
            fillDropDowns("");
        }

        protected void btnSaveDeliveryMethodAs_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            DeliveryMethodInfo DeliveryMethod = new DeliveryMethodInfo();
            DeliveryMethod.PortalId = PortalId;
            DeliveryMethod.LastModifiedById = UserId;
            DeliveryMethod.LastModifiedDate = DateTime.Now;
            DeliveryMethod.DeliveryMethod = txtDeliveryMethod.Text;

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
            fillDropDowns("");
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
            fillDropDowns("");
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
        }

        private void FillDeliveryMethodList()
        {
            AdminController aCont = new AdminController();
            List<DeliveryMethodInfo> DeliveryMethods = aCont.Get_DeliveryMethodsByPortalId(PortalId);
            gvDeliveryMethod.DataSource = DeliveryMethods;
            if (Session["DeliveryMethodPage"] != null)
            {
                gvDeliveryMethod.PageIndex = Convert.ToInt32(Session["DeliveryMethodPage"]);
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
            Session["DeliveryMethodPage"] = e.NewPageIndex.ToString();
            gvDeliveryMethod.PageIndex = e.NewPageIndex;
            gvDeliveryMethod.DataBind();
        }
        #endregion

        #region StationGroup Methods
        protected void btnSaveStationGroup_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            StationGroupInfo StationGroup = new StationGroupInfo();
            StationGroup.PortalId = PortalId;
            StationGroup.StationGroupName = txtStationGroupName.Text;
            StationGroup.Description = txtStationGroupDescription.Text;
            StationGroup.AgencyId = Convert.ToInt32(ddlStationGroupAgency.SelectedValue);
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
            fillDropDowns("");
        }

        protected void btnSaveStationGroupAs_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            StationGroupInfo StationGroup = new StationGroupInfo();
            StationGroup.PortalId = PortalId;
            StationGroup.StationGroupName = txtStationGroupName.Text;
            StationGroup.Description = txtStationGroupDescription.Text;
            StationGroup.AgencyId = Convert.ToInt32(ddlStationGroupAgency.SelectedValue);
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
            fillDropDowns("");
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
            fillDropDowns("");
        }

        protected void btnClearStationGroup_Click(object sender, System.EventArgs e)
        {
            clearStationGroup();
        }
        private void clearStationGroup()
        {
            txtStationGroupName.Text = "";
            txtStationGroupDescription.Text = "";
            ddlStationGroupAgency.SelectedIndex = 0;
            txtSelectedStationGroup.Value = "-1";
            txtStationGroupCreatedBy.Value = "";
            txtStationGroupCreatedDate.Value = "";
            btnDeleteStationGroup.Enabled = false;
            btnSaveStationGroupAs.Enabled = false;
            btnManageStationsInGroup.Enabled = false;
            lblStationGroupMessage.Text = "";
        }

        private void FillStationGroupList()
        {
            AdminController aCont = new AdminController();
            List<StationGroupInfo> StationGroups = aCont.Get_StationGroupsByPortalId(PortalId);
            gvStationGroup.DataSource = StationGroups;
            if (Session["StationGroupPage"] != null)
            {
                gvStationGroup.PageIndex = Convert.ToInt32(Session["StationGroupPage"]);
            }
            gvStationGroup.DataBind();
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
                ddlStationGroupAgency.SelectedValue = StationGroup.AgencyId.ToString();
                lblStationGroupMessage.Text = "";
                btnManageStationsInGroup.Enabled = true;
                lbxStationGroupStations.Items.Clear();
                lbxStationsInGroupModal.Items.Clear();
                foreach(StationInfo station in StationGroup.stations)
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
            clearAll("StationGroup");
        }

        protected void gvStationGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["StationGroupPage"] = e.NewPageIndex.ToString();
            gvStationGroup.PageIndex = e.NewPageIndex;
            gvStationGroup.DataBind();
        }
        #endregion

        #region Label Methods
        protected void btnSaveLabel_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            LabelInfo Label = new LabelInfo();
            Label.PortalId = PortalId;
            Label.TapeFormat = Convert.ToInt32(ddlLabelTapeFormat.SelectedValue);
            Label.AgencyId = Convert.ToInt32(ddlLabelAgency.SelectedValue);
            Label.AgencyName = ddlLabelAgency.SelectedItem.Text;
            Label.AdvertiserId = Convert.ToInt32(ddlLabelAdvertiser.SelectedValue);
            Label.AdvertiserName = ddlLabelAdvertiser.SelectedItem.Text;
            Label.Title = txtLabelTitle.Text;
            Label.Description = txtLabelDescription.Text;
            Label.ISCICode = txtLabelISCI.Text;
            Label.PMTMediaId = txtLabelPMTMediaId.Text;
            Label.MediaLength = txtLabelLength.Text;
            Label.Standard = ddlLabelStandard.SelectedValue;
            Label.Notes = txtLabelNotes.Text;
            Label.LastModifiedById = UserId;
            Label.LastModifiedDate = DateTime.Now;
            if (txtSelectedLabel.Value == "-1")
            {
                //save new Label
                Label.CreatedById = UserId;
                Label.DateCreated = DateTime.Now;
                int LabelId = aCont.Add_Label(Label);
                txtSelectedLabel.Value = LabelId.ToString();
                txtLabelCreatedBy.Value = UserId.ToString();
                txtLabelCreatedDate.Value = DateTime.Now.Ticks.ToString();
                lblLabelMessage.Text = "Label Saved.";
                btnDeleteLabel.Enabled = true;
                btnSaveLabelAs.Enabled = true;
                btnManageStationsInGroup.Enabled = true;
            }
            else
            {
                //update existing Label
                Label.CreatedById = Convert.ToInt32(txtLabelCreatedBy.Value);
                Label.DateCreated = new DateTime(Convert.ToInt64(txtLabelCreatedDate.Value));
                Label.Id = Convert.ToInt32(txtSelectedLabel.Value);
                aCont.Update_Label(Label);
                lblLabelMessage.Text = "Label Updated.";
                btnManageStationsInGroup.Enabled = true;
            }
            FillLabelList();
            fillDropDowns("");
        }

        protected void btnSaveLabelAs_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            LabelInfo Label = new LabelInfo();
            Label.PortalId = PortalId;
            Label.TapeFormat = Convert.ToInt32(ddlLabelTapeFormat.SelectedValue);
            Label.AgencyId = Convert.ToInt32(ddlLabelAgency.SelectedValue);
            Label.AgencyName = ddlLabelAgency.SelectedItem.Text;
            Label.AdvertiserId = Convert.ToInt32(ddlLabelAdvertiser.SelectedValue);
            Label.AdvertiserName = ddlLabelAdvertiser.SelectedItem.Text;
            Label.Title = txtLabelTitle.Text;
            Label.Description = txtLabelDescription.Text;
            Label.ISCICode = txtLabelISCI.Text;
            Label.PMTMediaId = txtLabelPMTMediaId.Text;
            Label.MediaLength = txtLabelLength.Text;
            Label.Standard = ddlLabelStandard.SelectedValue;
            Label.Notes = txtLabelNotes.Text;
            Label.LastModifiedById = UserId;
            Label.LastModifiedDate = DateTime.Now;

            //save new Label
            Label.CreatedById = UserId;
            Label.DateCreated = DateTime.Now;
            int LabelId = aCont.Add_Label(Label);
            txtSelectedLabel.Value = LabelId.ToString();
            txtLabelCreatedBy.Value = UserId.ToString();
            txtLabelCreatedDate.Value = DateTime.Now.Ticks.ToString();
            lblLabelMessage.Text = "Label Saved.";
            btnManageStationsInGroup.Enabled = true;

            FillLabelList();
            fillDropDowns("");
        }

        protected void btnDeleteLabel_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int LabelId = Convert.ToInt32(txtSelectedLabel.Value);
            LabelInfo Label = new LabelInfo();
            Label.Id = LabelId;
            aCont.Delete_Label(Label);
            lblLabelMessage.Text = "Label Deleted.";
            FillLabelList();
            clearLabel();
            fillDropDowns("");
        }

        protected void btnClearLabel_Click(object sender, System.EventArgs e)
        {
            clearLabel();
        }
        private void clearLabel()
        {
            ddlLabelTapeFormat.SelectedIndex = 0;
            ddlLabelAgency.SelectedIndex = 0;
            ddlLabelAdvertiser.SelectedIndex = 0;
            txtLabelTitle.Text = "";
            txtLabelDescription.Text = "";
            txtLabelISCI.Text = "";
            txtLabelPMTMediaId.Text = "";
            txtLabelLength.Text = "";
            ddlLabelStandard.SelectedIndex = 0;
            txtLabelNotes.Text = "";
            txtSelectedLabel.Value = "-1";
            txtLabelCreatedBy.Value = "";
            txtLabelCreatedDate.Value = "";
            btnDeleteLabel.Enabled = false;
            btnSaveLabelAs.Enabled = false;
            lblLabelMessage.Text = "";
            pnlLabelView.Visible = false;
        }

        private void FillLabelList()
        {
            AdminController aCont = new AdminController();
            List<LabelInfo> Labels = aCont.Get_LabelsByPortalId(PortalId);
            List<LabelInfo> lblsbyAgency = new List<LabelInfo>();
            List<LabelInfo> lblsbyAdvertiser = new List<LabelInfo>();
            List<LabelInfo> lblsbyKeyword = new List<LabelInfo>();
            if(ddlLabelAgencySearch.SelectedIndex>0)
            {
                foreach(LabelInfo label in Labels)
                {
                    if ((label.AgencyId == Convert.ToInt32(ddlLabelAgencySearch.SelectedValue)) || label.AgencyName == ddlLabelAgencySearch.SelectedItem.Text)
                    {
                        lblsbyAgency.Add(label);
                    }
                }
            }
            else
            {
                lblsbyAgency = Labels;
            }
            if(ddlLabelAdvertiserSearch.SelectedIndex>0)
            {
                foreach (LabelInfo label in lblsbyAgency)
                {
                    if ((label.AdvertiserId == Convert.ToInt32(ddlLabelAdvertiserSearch.SelectedValue)) || label.AdvertiserName == ddlLabelAdvertiserSearch.SelectedItem.Text)
                    {
                        lblsbyAdvertiser.Add(label);
                    }
                }
            }
            else
            {
                lblsbyAdvertiser = lblsbyAgency;
            }
            if(txtLabelSearch.Text!="")
            {
                foreach (LabelInfo label in lblsbyAdvertiser)
                {
                    if (label.LabelNumber.ToString().ToLower().IndexOf(txtLabelSearch.Text.ToLower()) != -1 ||
                        label.Title.ToLower().IndexOf(txtLabelSearch.Text.ToLower()) != -1 ||
                        label.ISCICode.ToLower().IndexOf(txtLabelSearch.Text.ToLower()) != -1 ||
                        label.Description.ToLower().IndexOf(txtLabelSearch.Text.ToLower()) != -1)
                    {
                        lblsbyKeyword.Add(label);
                    }
                }
            }
            else
            {
                lblsbyKeyword = lblsbyAdvertiser;
            }

            if (Session["LabelPage"] != null)
            {
                gvLabel.PageIndex = Convert.ToInt32(Session["LabelPage"]);
            }
            lblLabelMessage.Text = lblsbyKeyword.Count.ToString() + " Labels found.";
            gvLabel.DataSource = lblsbyKeyword;
            gvLabel.DataBind();
        }

        protected void gvLabel_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillLabelList();
            AdminController aCont = new AdminController();
            txtSelectedLabel.Value = (gvLabel.SelectedRow.FindControl("hdngvLabelId") as HiddenField).Value;
            LabelInfo Label = aCont.Get_LabelById(Convert.ToInt32(txtSelectedLabel.Value));
            if (Label.Id != -1)
            {
                btnDeleteLabel.Enabled = true;
                btnSaveLabelAs.Enabled = true;
                txtLabelCreatedBy.Value = Label.CreatedById.ToString();
                txtLabelCreatedDate.Value = Label.DateCreated.Ticks.ToString();
                ddlLabelTapeFormat.SelectedValue = Label.TapeFormat.ToString();
                if(Label.AgencyId>-1)
                {
                    ddlLabelAgency.SelectedValue = Label.AgencyId.ToString();
                }
                else
                {
                    if(ddlLabelAgency.SelectedIndex>=0)
                    { ddlLabelAgency.Items[ddlLabelAgency.SelectedIndex].Selected = false; }
                    ddlLabelAgency.Items.FindByText(Label.AgencyName).Selected = true;
                }
                if (Label.AdvertiserId > -1)
                {
                    ddlLabelAdvertiser.SelectedValue = Label.AdvertiserId.ToString();
                }
                else
                {
                    if (ddlLabelAdvertiser.SelectedIndex >= 0)
                    { ddlLabelAdvertiser.Items[ddlLabelAdvertiser.SelectedIndex].Selected = false; }
                    ddlLabelAdvertiser.Items.FindByText(Label.AdvertiserName).Selected = true;
                }
                txtLabelTitle.Text = Label.Title;
                txtLabelDescription.Text = Label.Description;
                txtLabelISCI.Text = Label.ISCICode;
                txtLabelPMTMediaId.Text = Label.PMTMediaId;
                txtLabelLength.Text = Label.MediaLength;
                ddlLabelStandard.SelectedValue = Label.Standard;
                txtLabelNotes.Text = Label.Notes;
                lblLabelMessage.Text = "";

                //populate and show actual label
                litLabelAdvertiser.Text = Label.AdvertiserName;
                litLabelAgency.Text = Label.AgencyName;
                litLabelCampaignCreated.Text = ""; //add value when campaigns are added
                litLabelCampaignId.Text = Label.CampaignId.ToString();
                litLabelCampaignStatus.Text = ""; //add value when campaigns are added
                litLabelDescription.Text = Label.Description;
                litLabelDestinationId.Text = ""; //add value when campaigns are added
                litLabelISCI.Text = Label.ISCICode;
                litLabelMediaLength.Text = Label.MediaLength;
                litLabelNumber.Text = Label.LabelNumber.ToString();
                litLabelPMTMediaId.Text = Label.PMTMediaId;
                litLabelStandard.Text = Label.Standard;
                TapeFormatInfo tf = aCont.Get_TapeFormatById(Label.TapeFormat);
                litLabelTapeFormat.Text = tf.TapeFormat;
                litLabelTitle.Text = Label.Title;
                pnlLabelView.Visible = true;
            }
            else
            {
                btnDeleteLabel.Enabled = false;
                btnSaveLabelAs.Enabled = false;
                clearLabel();
                lblLabelMessage.Text = "There was an error loading this Label.";
            }
            clearAll("Labels");
        }

        protected void gvLabel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["LabelPage"] = e.NewPageIndex.ToString();
            gvLabel.PageIndex = e.NewPageIndex;
            gvLabel.DataBind();
        }
        #endregion

        #region MasterItem Methods
        protected void btnSaveMasterItem_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            MasterItemInfo MasterItem = new MasterItemInfo();
            MasterItem.PortalId = PortalId;
            MasterItem.Filename = txtMasterItemFile.Text;
            MasterItem.AdvertiserId = Convert.ToInt32(ddlMasterItemAdvertiser.SelectedValue);
            MasterItem.Title = txtMasterItemTitle.Text;
            MasterItem.MediaType = Convert.ToInt32(ddlMasterItemMediaType.SelectedValue);
            MasterItem.Encode = Convert.ToInt32(ddlMasterItemEncode.SelectedValue);
            MasterItem.Standard = ddlMasterItemStandard.SelectedValue;
            MasterItem.Length = txtMasterItemLength.Text;
            MasterItem.CustomerId = txtMasterItemCustomerId.Text;
            MasterItem.PMTMediaId = txtMasterItemPMTMediaId.Text;
            try
            {
                MasterItem.Reel = Convert.ToInt32(txtMasterItemReel.Text);
            }
            catch { lblMasterItemMessage.Text += "Reel # must be an integer. "; }
            MasterItem.TapeCode = txtMasterItemTapeCode.Text;
            try
            {
                MasterItem.Position = Convert.ToInt32(txtMasterItemPostition.Text);
            }
            catch { lblMasterItemMessage.Text += "Position # must be an integer. "; }
            MasterItem.VaultId = txtMasterItemVaultId.Text;
            MasterItem.Location = txtMasterItemLocation.Text;
            MasterItem.Comment = txtMasterItemComment.Text;
            MasterItem.ClosedCaptioned = chkMasterItemClosedCaption.Checked;
            MasterItem.LastModifiedById = UserId;
            MasterItem.LastModifiedDate = DateTime.Now;
            int MasterItemId = -1;
            if (txtSelectedMasterItem.Value == "-1")
            {
                //save new MasterItem
                MasterItem.CreatedById = UserId;
                MasterItem.DateCreated = DateTime.Now;
                MasterItemId = aCont.Add_MasterItem(MasterItem);
                txtSelectedMasterItem.Value = MasterItemId.ToString();
                txtMasterItemCreatedBy.Value = UserId.ToString();
                txtMasterItemCreatedDate.Value = DateTime.Now.Ticks.ToString();
                lblMasterItemMessage.Text = "Master Item Saved.";
                btnDeleteMasterItem.Enabled = true;
                btnSaveMasterItemAs.Enabled = true;
                btnManageStationsInGroup.Enabled = true;
            }
            else
            {
                //update existing MasterItem
                MasterItemId = Convert.ToInt32(txtSelectedMasterItem.Value);
                MasterItem.CreatedById = Convert.ToInt32(txtMasterItemCreatedBy.Value);
                MasterItem.DateCreated = new DateTime(Convert.ToInt64(txtMasterItemCreatedDate.Value));
                MasterItem.Id = Convert.ToInt32(txtSelectedMasterItem.Value);
                aCont.Update_MasterItem(MasterItem);
                lblMasterItemMessage.Text = "MasterItem Updated.";
                btnManageStationsInGroup.Enabled = true;
            }
            //save agencies
            aCont.Delete_MasterItemAgencyByMasterItemId(MasterItemId);
            foreach(ListItem li in lbxMasterItemAgencies.Items)
            {
                if(li.Selected)
                {
                    aCont.Add_MasterItemAgency(MasterItemId, Convert.ToInt32(li.Value));
                }
            }
            FillMasterItemList();
            fillDropDowns("");
        }

        protected void btnSaveMasterItemAs_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            MasterItemInfo MasterItem = new MasterItemInfo();
            MasterItem.PortalId = PortalId;
            MasterItem.Filename = txtMasterItemFile.Text;
            MasterItem.AdvertiserId = Convert.ToInt32(ddlMasterItemAdvertiser.SelectedValue);
            MasterItem.Title = txtMasterItemTitle.Text;
            MasterItem.MediaType = Convert.ToInt32(ddlMasterItemMediaType.SelectedValue);
            MasterItem.Encode = Convert.ToInt32(ddlMasterItemEncode.SelectedValue);
            MasterItem.Standard = ddlMasterItemStandard.SelectedValue;
            MasterItem.Length = txtMasterItemLength.Text;
            MasterItem.CustomerId = txtMasterItemCustomerId.Text;
            MasterItem.PMTMediaId = txtMasterItemPMTMediaId.Text;
            MasterItem.ClosedCaptioned = chkMasterItemClosedCaption.Checked;
            try
            {
                MasterItem.Reel = Convert.ToInt32(txtMasterItemReel.Text);
            }
            catch { lblMasterItemMessage.Text += "Reel # must be an integer. "; }
            MasterItem.TapeCode = txtMasterItemTapeCode.Text;
            try
            {
                MasterItem.Position = Convert.ToInt32(txtMasterItemPostition.Text);
            }
            catch { lblMasterItemMessage.Text += "Position # must be an integer. "; }
            MasterItem.VaultId = txtMasterItemVaultId.Text;
            MasterItem.Location = txtMasterItemLocation.Text;
            MasterItem.Comment = txtMasterItemComment.Text;
            MasterItem.LastModifiedById = UserId;
            MasterItem.LastModifiedDate = DateTime.Now;

            //save new MasterItem
            MasterItem.CreatedById = UserId;
            MasterItem.DateCreated = DateTime.Now;
            int MasterItemId = aCont.Add_MasterItem(MasterItem);
            txtSelectedMasterItem.Value = MasterItemId.ToString();
            txtMasterItemCreatedBy.Value = UserId.ToString();
            txtMasterItemCreatedDate.Value = DateTime.Now.Ticks.ToString();
            lblMasterItemMessage.Text = "Master Item Saved.";
            btnManageStationsInGroup.Enabled = true;

            //save agencies
            aCont.Delete_MasterItemAgencyByMasterItemId(MasterItemId);
            foreach (ListItem li in lbxMasterItemAgencies.Items)
            {
                if (li.Selected)
                {
                    aCont.Add_MasterItemAgency(MasterItemId, Convert.ToInt32(li.Value));
                }
            }

            FillMasterItemList();
            fillDropDowns("");
        }

        protected void btnDeleteMasterItem_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int MasterItemId = Convert.ToInt32(txtSelectedMasterItem.Value);
            MasterItemInfo MasterItem = new MasterItemInfo();
            MasterItem.Id = MasterItemId;
            aCont.Delete_MasterItem(MasterItem);
            lblMasterItemMessage.Text = "MasterItem Deleted.";
            FillMasterItemList();
            clearMasterItem();
            fillDropDowns("");
        }

        protected void btnClearMasterItem_Click(object sender, System.EventArgs e)
        {
            clearMasterItem();
        }
        private void clearMasterItem()
        {
            txtMasterItemFile.Text = "";
            lbxMasterItemAgencies.Items.Clear();
            ddlMasterItemAdvertiser.SelectedIndex = 0;
            txtMasterItemTitle.Text = "";
            txtMasterItemComment.Text = "";
            ddlMasterItemMediaType.SelectedIndex = 0;
            txtMasterItemPMTMediaId.Text = "";
            txtMasterItemLength.Text = "";
            txtMasterItemCustomerId.Text = "";
            txtMasterItemTitle.Enabled = true;
            txtMasterItemPMTMediaId.Text = "";
            txtMasterItemPMTMediaId.Enabled = true;
            txtMasterItemReel.Text = "";
            txtMasterItemTapeCode.Text = "";
            txtMasterItemPostition.Text = "";
            txtMasterItemVaultId.Text = "";
            txtMasterItemLocation.Text = "";
            chkMasterItemClosedCaption.Checked = false;
            ddlMasterItemStandard.SelectedIndex = 0;
            ddlMasterItemEncode.SelectedIndex = 0;
            txtSelectedMasterItem.Value = "-1";
            txtMasterItemCreatedBy.Value = "";
            txtMasterItemCreatedDate.Value = "";
            btnDeleteMasterItem.Enabled = false;
            btnSaveMasterItemAs.Enabled = false;
            ddlMasterItemAdvertiserSearch.SelectedIndex = 0;
            ddlMasterItemAgencySearch.SelectedIndex = 0;
            txtMasterItemSearch.Text = "";
            lblMasterItemMessage.Text = "";
            btnMasterChecklist.Enabled = false;
            lblUserMessage.Text = "";
        }

        private void FillMasterItemList()
        {
            AdminController aCont = new AdminController();
            bool canViewAll = false;
            if(UserInfo.IsInRole("Administrators"))
            {
                canViewAll = true;
            }
            List<MasterItemInfo> MasterItems = new List<MasterItemInfo>();
            List<MasterItemInfo> mstrsByUser = new List<MasterItemInfo>();
            MasterItems = aCont.Get_MasterItemsByPortalId(PortalId);
            if (canViewAll)
            {
                mstrsByUser = MasterItems;
            }
            else
            {
                //TODO:Add specific user level view filtering
                //Should also filter ag and ad list by permissions
                List<AgencyInfo> agencies = aCont.Get_AgenciesByUser(UserId);
                List<AdvertiserInfo> advertisers = aCont.Get_AdvertisersByUser(UserId, PortalId);
                bool isAgencyRole = UserInfo.IsInRole("Agency");
                bool isAdvertiserRole = UserInfo.IsInRole("Advertiser");
                if(isAdvertiserRole && advertisers.Count>0 && agencies.Count==0)
                {
                    //in advertiser role and only tagged with advertisers.  Show all masters tagged with these advertisers
                    foreach(MasterItemInfo master in MasterItems)
                    {
                        foreach(AdvertiserInfo ad in advertisers)
                        { 
                            if(master.AdvertiserId==ad.Id)
                            {
                                mstrsByUser.Add(master);
                            }
                        }
                    }
                }
                else if (isAgencyRole && agencies.Count > 0 && advertisers.Count == 0)
                {
                    //in agency role and only tagged with agencies.  Show all masters tagged with these agencies
                    foreach (MasterItemInfo master in MasterItems)
                    {
                        foreach (AgencyInfo ag in agencies)
                        {
                            foreach(AgencyInfo ag1 in master.Agencies)
                            {
                                if(ag.Id==ag1.Id)
                                {
                                    mstrsByUser.Add(master);
                                }
                            }
                        }
                    }
                }
                else if(isAgencyRole && !isAdvertiserRole && agencies.Count>0 && advertisers.Count>0)
                {
                    //in agency role, not advertiser role, and tagged with both.  Only show intersection
                    foreach(MasterItemInfo master in MasterItems)
                    {
                        foreach (AgencyInfo ag in agencies)
                        {
                            foreach (AgencyInfo ag1 in master.Agencies)
                            {
                                foreach (AdvertiserInfo ad in advertisers)
                                {
                                    if (ag.Id == ag1.Id && ad.Id==master.AdvertiserId)
                                    {
                                        mstrsByUser.Add(master);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            List<MasterItemInfo> mstrsbyAgency = new List<MasterItemInfo>();
            List<MasterItemInfo> mstrsbyAdvertiser = new List<MasterItemInfo>();
            List<MasterItemInfo> mstrsbyKeyword = new List<MasterItemInfo>();
            if (ddlMasterItemAdvertiserSearch.SelectedIndex > 0)
            {
                foreach (MasterItemInfo MasterItem in MasterItems)
                {
                    if ((MasterItem.AdvertiserId == Convert.ToInt32(ddlMasterItemAdvertiserSearch.SelectedValue)))
                    {
                        mstrsbyAdvertiser.Add(MasterItem);
                    }
                }
            }
            else
            {
                mstrsbyAdvertiser = mstrsByUser;
            }
            if (ddlMasterItemAgencySearch.SelectedIndex > 0)
            {
                foreach (MasterItemInfo MasterItem in mstrsbyAdvertiser)
                {
                    if (MasterItem.Agencies != null)
                    {
                        foreach (AgencyInfo ag in MasterItem.Agencies)
                        {
                            if (ag.Id.ToString() == ddlMasterItemAgencySearch.SelectedValue)
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

            if (Session["MasterItemPage"] != null)
            {
                gvMasterItem.PageIndex = Convert.ToInt32(Session["MasterItemPage"]);
            }
            lblMasterItemMessage.Text = mstrsbyKeyword.Count.ToString() + " Master Items found.";
            gvMasterItem.DataSource = mstrsbyKeyword;
            gvMasterItem.DataBind();
        }

        protected void gvMasterItem_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillMasterItemList();
            btnMasterChecklist.Enabled = true;
            txtSelectedMasterItem.Value = (gvMasterItem.SelectedRow.FindControl("hdngvMasterItemId") as HiddenField).Value;
            btnMasterChecklist.OnClientClick = "window.open('check-list.aspx?Master=" + txtSelectedMasterItem.Value + "','_blank')";
            AdminController aCont = new AdminController();
            MasterItemInfo MasterItem = aCont.Get_MasterItemById(Convert.ToInt32(txtSelectedMasterItem.Value));
            if (MasterItem.Id != -1)
            {
                btnDeleteMasterItem.Enabled = true;
                btnSaveMasterItemAs.Enabled = true;
                txtMasterItemFile.Text = MasterItem.Filename;
                
                lbxMasterItemAgencies.Items.Clear();
                List<AgencyInfo> ags = aCont.Get_AgenciesByAdvertiserId(MasterItem.AdvertiserId);
                foreach(AgencyInfo ag in ags)
                {
                    ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                    foreach(AgencyInfo agSelected in MasterItem.Agencies)
                    {
                        if(agSelected.Id==ag.Id)
                        {
                            li.Selected = true;
                        }
                    }
                    lbxMasterItemAgencies.Items.Add(li);
                }

                ddlMasterItemAdvertiser.SelectedValue = MasterItem.AdvertiserId.ToString();
                txtMasterItemTitle.Text = MasterItem.Title;
                txtMasterItemComment.Text = MasterItem.Comment;
                ddlMasterItemMediaType.SelectedValue = MasterItem.MediaType.ToString();
                txtMasterItemPMTMediaId.Text = MasterItem.PMTMediaId;
                txtMasterItemLength.Text = MasterItem.Length;
                txtMasterItemCustomerId.Text = MasterItem.CustomerId;
                txtMasterItemTitle.Enabled = false;
                txtMasterItemPMTMediaId.Text = MasterItem.PMTMediaId;
                txtMasterItemPMTMediaId.Enabled = false;
                if (MasterItem.Reel > 0)
                {
                    txtMasterItemReel.Text = MasterItem.Reel.ToString();
                }
                txtMasterItemTapeCode.Text = MasterItem.TapeCode;
                if (MasterItem.Position > 0)
                {
                    txtMasterItemPostition.Text = MasterItem.Position.ToString();
                }
                txtMasterItemVaultId.Text = MasterItem.VaultId;
                txtMasterItemLocation.Text = MasterItem.Location;
                chkMasterItemClosedCaption.Checked = MasterItem.ClosedCaptioned;
                ddlMasterItemStandard.SelectedValue = MasterItem.Standard.ToString();
                ddlMasterItemEncode.SelectedValue = MasterItem.Encode.ToString();
                txtMasterItemCreatedBy.Value = MasterItem.CreatedById.ToString();
                txtMasterItemCreatedDate.Value = MasterItem.DateCreated.Ticks.ToString(); ;
                btnDeleteMasterItem.Enabled = true;
                btnSaveMasterItemAs.Enabled = true;
                lblMasterItemMessage.Text = "";                
            }
            else
            {
                btnDeleteMasterItem.Enabled = false;
                btnSaveMasterItemAs.Enabled = false;
                clearMasterItem();
                lblMasterItemMessage.Text = "There was an error loading this MasterItem.";
            }
            clearAll("MasterItems");
        }

        protected void gvMasterItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["MasterItemPage"] = e.NewPageIndex.ToString();
            //gvMasterItem.PageIndex = e.NewPageIndex;
            //gvMasterItem.DataBind();
            FillMasterItemList();
        }
        protected void ddlMasterItemAdvertiser_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillMasterItemAgencies();
        }

        private void fillMasterItemAgencies()
        {
            if (ddlMasterItemAdvertiser.SelectedIndex > 0)
            {
                AdminController aCont = new AdminController();
                List<AgencyInfo> ags = aCont.Get_AgenciesByAdvertiserId(Convert.ToInt32(ddlMasterItemAdvertiser.SelectedValue));
                List<AgencyInfo> agSelected = aCont.Get_AgenciesByMasterItemId(Convert.ToInt32(txtSelectedMasterItem.Value));
                List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
                lbxMasterItemAgencies.Items.Clear();
                foreach (AgencyInfo ag in ags)
                {
                    //foreach (AgencyInfo userAg in userAgs)
                    //{
                    //    if (userAg.Id == ag.Id)
                    //    {
                            ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                            foreach (AgencyInfo agsel in agSelected)
                            {
                                if (agsel.Id == ag.Id)
                                {
                                    li.Selected = true;
                                }
                            }
                            lbxMasterItemAgencies.Items.Add(li);
                    //    }
                    //}
                }
            }
            else
            {
                lbxMasterItemAgencies.Items.Clear();
            }
        }
        protected void ddlMasterItemAgencySearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMasterItemAgencySearch.Items.Clear();
            ddlMasterItemAgencySearch.Items.Add(new ListItem("--Select Agency--", ""));
            AdminController aCont = new AdminController();
            if (ddlMasterItemAdvertiserSearch.SelectedIndex > 0)
            {
                List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
                bool isAdmin = UserInfo.IsInRole("Administrators");
                List<AgencyInfo> ags = aCont.Get_AgenciesByAdvertiserId(Convert.ToInt32(ddlMasterItemAdvertiserSearch.SelectedValue));
                foreach (AgencyInfo ag in ags)
                {
                    ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                    ddlMasterItemAgencySearch.Items.Add(li);
                }
            }
            else
            {
                List<AgencyInfo> ags = aCont.Get_AgenciesByPortalId(PortalId);
                foreach (AgencyInfo ag in ags)
                {
                    ddlMasterItemAgencySearch.Items.Add(new ListItem(ag.AgencyName, ag.Id.ToString()));
                }
            }
            FillMasterItemList();
        }
        protected void btnMasterChecklist_Click(object sender, EventArgs e)
        {
            //Response.Redirect("check-list.aspx?Master=" + txtSelectedMasterItem.Value);
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
                foreach(ListItem li in lbxStationsInGroupModal.Items)
                {
                    if(li.Value==station.Id.ToString())
                    {
                        dupe = true;
                    }
                }
                if (!dupe)
                {
                    aCont.Add_StationsInGroup(PortalId, station.Id, Convert.ToInt32(txtSelectedStationGroup.Value));
                    lbxStationsInGroupModal.Items.Add(new ListItem(station.StationName, station.Id.ToString()));
                    lbxStationGroupStations.Items.Add(new ListItem(station.StationName, station.Id.ToString())); 
                    btnDeleteTapeFormat.Enabled = true;
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
            Session["StationGroupStationsPage"] = e.NewPageIndex.ToString();
            FillStations();
        }
        #endregion

        #region modal methods
        protected void btnAddFreight_Click(object sender, System.EventArgs e)
        {
            mpeFreightPopup.Show();
        }

        protected void btnCancelFreightPopup_Click(object sender, System.EventArgs e)
        {
            clearFreightType();
            mpeFreightPopup.Hide();
        }

        protected void btnAddClientType_Click(object sender, System.EventArgs e)
        {
            mpeClientTypePopup.Show();
        }

        protected void btnCancelClientTypePopup_Click(object sender, System.EventArgs e)
        {
            clearClientType();
            mpeClientTypePopup.Hide();
        }

        protected void btnCancelCarrierTypePopup_Click(object sender, System.EventArgs e)
        {
            clearCarrierType();
            mpeCarrierTypePopup.Hide();
        }

        protected void btnAddCarrier_Click(object sender, System.EventArgs e)
        {
            mpeCarrierTypePopup.Show();
        }

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

        #region search
        protected void btnStationGroupStationSearch_Click(object sender, System.EventArgs e)
        {
            FillStations();
        }
        protected void btnAgencySearch_Click(object sender, System.EventArgs e)
        {
            FillAgencyList();
        }
        protected void btnAdvertiserSearch_Click(object sender, System.EventArgs e)
        {
            FillAdvertiserList();
        }

        protected void btnStationSearch_Click(object sender, System.EventArgs e)
        {
            FillStationList();
        }
        protected void btnLabelSearch_Click(object sender, System.EventArgs e)
        {
            FillLabelList();
        }

        protected void ddlLabelAgencySearch_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        protected void btnMasterItemSearch_Click(object sender, EventArgs e)
        {
            FillMasterItemList();
        }
        #endregion  

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
            if(ddlUsers.SelectedValue!="-1")
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

            foreach(object user in users)
            {
                UserInfo thisUser = (UserInfo)user;
                ListItem li = new ListItem(thisUser.DisplayName, thisUser.UserID.ToString());
                ddlUsers.Items.Add(li);
            }
        }

        #endregion

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
                UserInfo thisUser =  UserController.GetUserById(PortalId, thisUserId);
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
                foreach(ListItem li in lbxUserAgencies.Items)
                {
                    if(li.Selected)
                    {
                        aCont.Add_UserInAgency(thisUserId, Convert.ToInt32(li.Value));
                    }
                }
                //save advertisers
                //first, delete all advertisers for this user
                aCont.Delete_UserInAdvertisers(thisUserId);
                //now add them back in
                foreach(ListItem li in lbxUserAdvertisers.Items)
                {
                    if(li.Selected)
                    {
                        aCont.Add_UserInAdvertiser(thisUserId, Convert.ToInt32(li.Value));
                    }
                }
                lblUserMessage.Text = "Permissions updated for this user.";
            }
        }
    }
}