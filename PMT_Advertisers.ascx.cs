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
    public partial class PMT_Advertisers : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            FillAdvertiserList();
            FillClientTypeList();
            fillClientTypes();
            FillCarrierTypeList();
            fillCarrierTypes();
            FillFreightTypeList();
            fillFreightTypes();
            fillAgencies();
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
            Advertiser.CarrierNumber = txtAdvertiserCarrierNum.Text;
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
                    if (li.Selected)
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
                    if (li.Selected)
                    {
                        aCont.Add_AdvertiserAgency(Advertiser.Id, Convert.ToInt32(li.Value));
                    }
                }

                lblAdvertiserMessage.Text = "Advertiser Updated.";
            }
            if(Application["Advertisers"]!=null)
            {
                Application.Remove("Advertisers");
            }
            FillAdvertiserList();
            //fillDropDowns("Advertiser");
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
            Advertiser.CarrierNumber = txtAdvertiserCarrierNum.Text;
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
                if (li.Selected)
                {
                    aCont.Add_AdvertiserAgency(AdvertiserId, Convert.ToInt32(li.Value));
                }
            }

            txtSelectedAdvertiser.Value = AdvertiserId.ToString();
            txtAdvertiserCreatedBy.Value = UserId.ToString();
            txtAdvertiserCreatedDate.Value = DateTime.Now.Ticks.ToString();
            lblAdvertiserMessage.Text = "Advertiser Saved.";
            if (Application["Advertisers"] != null)
            {
                Application.Remove("Advertisers");
            }

            FillAdvertiserList();
            //fillDropDowns("Advertiser");
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
            if (Application["Advertisers"] != null)
            {
                Application.Remove("Advertisers");
            }
            FillAdvertiserList();
            //fillDropDowns("");
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
            txtAdvertiserCarrierNum.Text = "";
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
            if (ddlAdvertiserAgencySearch.SelectedIndex <= 0)
            {
                advertisersByAgency = Advertisers;
            }
            else
            {
                foreach (AdvertiserInfo advertiser in Advertisers)
                {
                    foreach (AgencyInfo ag in advertiser.Agencies)
                    {
                        if (ag.Id.ToString() == ddlAdvertiserAgencySearch.SelectedValue)
                        {
                            advertisersByAgency.Add(advertiser);
                        }
                    }
                }
            }
            if (txtAdvertiserSearch.Text == "")
            {
                advertisersFiltered = advertisersByAgency;
            }
            else
            {
                foreach (AdvertiserInfo advertiser in advertisersByAgency)
                {
                    if (advertiser.AdvertiserName.ToLower().IndexOf(txtAdvertiserSearch.Text.ToLower()) != -1)
                    {
                        advertisersFiltered.Add(advertiser);
                    }
                }
            }
            lblAdvertiserMessage.Text = advertisersFiltered.Count.ToString() + " Advertisers found.";
            gvAdvertiser.DataSource = advertisersFiltered;
            if (ViewState["AdvertiserPage"] != null)
            {
                gvAdvertiser.PageIndex = Convert.ToInt32(ViewState["AdvertiserPage"]);
            }
            gvAdvertiser.DataBind();
            //ddlLabelAdvertiser.Items.Clear();
            //ddlLabelAdvertiserSearch.Items.Clear();
            //ddlMasterItemAdvertiser.Items.Clear();
            //ddlLabelAdvertiser.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            //ddlLabelAdvertiserSearch.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            //ddlMasterItemAdvertiser.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            //ddlMasterItemAdvertiserSearch.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            foreach (AdvertiserInfo adv in Advertisers)
            {
                //ddlLabelAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                //ddlLabelAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                //ddlMasterItemAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                //lbxUserAdvertisers.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                if (isAdmin)
                {
                    //ddlMasterItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                }
                else
                {
                    foreach (AdvertiserInfo ad in userAds)
                    {
                        if (ad.Id == adv.Id)
                        {
                            //ddlMasterItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                        }
                    }
                }
            }
            //fillMasterItemAgencies();
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
                foreach (ListItem li in ddlAdvertiserAgency.Items)
                {
                    foreach (AgencyInfo adv in Advertiser.Agencies)
                    {
                        if (li.Value == adv.Id.ToString())
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
                txtAdvertiserCarrierNum.Text = Advertiser.CarrierNumber;
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
                catch
                {
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
            //clearAll("Advertiser");
        }

        protected void gvAdvertiser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["AdvertiserPage"] = e.NewPageIndex.ToString();
            FillAdvertiserList();
            //gvAdvertiser.PageIndex = e.NewPageIndex;
            //gvAdvertiser.DataBind();
        }
        #endregion
        #region ClientType Methods
        private void fillClientTypes()
        {
            ddlAdvertiserClientType.Items.Clear();
            //ddlAgencyClientType.Items.Clear();
            //ddlAgencyClientType.Items.Add(new ListItem("--Please Select Client Type--", "-1"));
            ddlAdvertiserClientType.Items.Add(new ListItem("--Please Select Client Type--", "-1"));
            AdminController aCont = new AdminController();
            List<ClientTypeInfo> clientTypes = aCont.Get_ClientTypesByPortalId(PortalId);
            foreach (ClientTypeInfo clientType in clientTypes)
            {
                //ddlAgencyClientType.Items.Add(new ListItem(clientType.ClientType, clientType.Id.ToString()));
                ddlAdvertiserClientType.Items.Add(new ListItem(clientType.ClientType, clientType.Id.ToString()));
            }
        }
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
            //fillDropDowns("");
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
            //fillDropDowns("");
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
            //fillDropDowns("");
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
            if (ViewState["ClientTypePage"] != null)
            {
                gvClientType.PageIndex = Convert.ToInt32(ViewState["ClientTypePage"]);
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
            ViewState["ClientTypePage"] = e.NewPageIndex.ToString();
            gvClientType.PageIndex = e.NewPageIndex;
            gvClientType.DataBind();
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
            //fillDropDowns("");
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

            //fillDropDowns("");
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
            //fillDropDowns("");
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
            if (ViewState["CarrierTypePage"] != null)
            {
                gvCarrierType.PageIndex = Convert.ToInt32(ViewState["CarrierTypePage"]);
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
            ViewState["CarrierTypePage"] = e.NewPageIndex.ToString();
            gvCarrierType.PageIndex = e.NewPageIndex;
            gvCarrierType.DataBind();
        }
        private void fillCarrierTypes()
        {
            ddlAdvertiserCarrier.Items.Clear();
            ddlAdvertiserCarrier.Items.Add(new ListItem("--Please Select Carrier Type--", "-1"));
            AdminController aCont = new AdminController();
            List<CarrierTypeInfo> CarrierTypes = aCont.Get_CarrierTypesByPortalId(PortalId);
            foreach (CarrierTypeInfo CarrierType in CarrierTypes)
            {
                ddlAdvertiserCarrier.Items.Add(new ListItem(CarrierType.CarrierType, CarrierType.Id.ToString()));
            }
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
            //fillDropDowns("");
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
            //fillDropDowns("");
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
            //fillDropDowns("");
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
            if (ViewState["FreightTypePage"] != null)
            {
                gvFreightType.PageIndex = Convert.ToInt32(ViewState["FreightTypePage"]);
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
            ViewState["FreightTypePage"] = e.NewPageIndex.ToString();
            gvFreightType.PageIndex = e.NewPageIndex;
            gvFreightType.DataBind();
        }
        private void fillFreightTypes()
        {
            ddlAdvertiserFreight.Items.Clear();
            ddlAdvertiserFreight.Items.Add(new ListItem("--Please Select Freight Type--", "-1"));
            AdminController aCont = new AdminController();
            List<FreightTypeInfo> FreightTypes = aCont.Get_FreightTypesByPortalId(PortalId);
            foreach (FreightTypeInfo FreightType in FreightTypes)
            {
                ddlAdvertiserFreight.Items.Add(new ListItem(FreightType.FreightType, FreightType.Id.ToString()));
            }
        }
        protected void btnAddFreight_Click(object sender, System.EventArgs e)
        {
            mpeFreightPopup.Show();
        }

        protected void btnCancelFreightPopup_Click(object sender, System.EventArgs e)
        {
            clearFreightType();
            mpeFreightPopup.Hide();
        }
        #endregion
        #region Agency Methods
        private void fillAgencies()
        {
            AdminController aCont = new AdminController();
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
        protected void btnAdvertiserSearch_Click(object sender, System.EventArgs e)
        {
            FillAdvertiserList();
        }
        #endregion

        protected void txtAdvertiserSearch_TextChanged(object sender, EventArgs e)
        {
            FillAdvertiserList();
        }

        protected void ddlAdvertiserAgencySearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillAdvertiserList();
        }

    }
}