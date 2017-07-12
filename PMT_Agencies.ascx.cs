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
    public partial class PMT_Agencies : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            FillAgencyList();
            fillClientTypes();
            FillClientTypeList();
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
            if (txtSelectedAgency.Value == "-1")
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
                lblMessage.Text = "Agency Updated.";
            }
            if (Application["Agencies"] != null)
            {
                Application.Remove("Agencies");
            }
            FillAgencyList();
            //fillDropDowns("Agency");
            try
            {
                ddlAgencyClientType.SelectedValue = agency.ClientType.ToString();
            }
            catch
            {
                ddlAgencyClientType.SelectedIndex = 0;
            }
            //clearAll("Agency");
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
            lblMessage.Text = "Agency Saved.";
            if (Application["Agencies"] != null)
            {
                Application.Remove("Agencies");
            }

            FillAgencyList();
            //fillDropDowns("Agency");
            //clearAll("Agency");
        }

        protected void btnDeleteAgency_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int AgencyId = Convert.ToInt32(txtSelectedAgency.Value);
            AgencyInfo agency = new AgencyInfo();
            agency.Id = AgencyId;
            aCont.Delete_Agency(agency);
            lblAgencyMessage.Text = "Agency Deleted.";
            if (Application["Agencies"] != null)
            {
                Application.Remove("Agencies");
            }
            FillAgencyList();
            //fillDropDowns("");
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
            if (txtAgencySearch.Text != "")
            {
                foreach (AgencyInfo agency in agencies)
                {
                    if (agency.AgencyName.ToLower().IndexOf(txtAgencySearch.Text.ToLower()) != -1)
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
            if (ViewState["AgencyPage"] != null)
            {
                gvAgency.PageIndex = Convert.ToInt32(ViewState["AgencyPage"]);
            }
            lblAgencyMessage.Text = agenciesFiltered.Count.ToString() + " Agencies found.";
            gvAgency.DataBind();
            //ddlStationGroupAgency.Items.Clear();
            //ddlLabelAgencySearch.Items.Clear();
            //ddlStationGroupAgency.Items.Add(new ListItem("--Select Agency--", "-1"));
            //ddlLabelAgencySearch.Items.Add(new ListItem("--Select Agency--", "-1"));
            //ddlLabelAgency.Items.Clear();
            //ddlLabelAgency.Items.Add(new ListItem("--Select Agency--", "-1"));
            //ddlMasterItemAgencySearch.Items.Add(new ListItem("--Select Agency--", "-1"));
            List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            foreach (AgencyInfo agency in agencies)
            {
                //ddlStationGroupAgency.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                //ddlLabelAgency.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                //ddlLabelAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                //lbxUserAgencies.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                if (isAdmin)
                {
                    //ddlMasterItemAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                }
                else
                {
                    foreach (AgencyInfo ag in userAgs)
                    {
                        if (ag.Id == agency.Id)
                        {
                            //ddlMasterItemAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                        }
                    }
                }
            }
        }
        protected void gvAgency_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillAgencyList();
            AdminController aCont = new AdminController();
            txtSelectedAgency.Value = (gvAgency.SelectedRow.FindControl("hdngvAgencyId") as HiddenField).Value;
            AgencyInfo agency = aCont.Get_AgencyById(Convert.ToInt32(txtSelectedAgency.Value));
            if (agency.Id != -1)
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
                catch
                {
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
            //clearAll("Agency");
        }

        protected void gvAgency_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["AgencyPage"] = e.NewPageIndex.ToString();
            FillAgencyList();
        }
        protected void btnAgencySearch_Click(object sender, System.EventArgs e)
        {
            FillAgencyList();
        }

        #endregion

        #region ClientType Methods
        protected void btnCancelClientTypePopup_Click(object sender, System.EventArgs e)
        {
            clearClientType();
            mpeClientTypePopup.Hide();
        }
        protected void btnAddClientType_Click(object sender, System.EventArgs e)
        {
            mpeClientTypePopup.Show();
        }
        private void fillClientTypes()
        {
            //ddlAdvertiserClientType.Items.Clear();
            ddlAgencyClientType.Items.Clear();
            ddlAgencyClientType.Items.Add(new ListItem("--Select Client Type--", "-1"));
            //ddlAdvertiserClientType.Items.Add(new ListItem("--Please Select Client Type--", "-1"));
            AdminController aCont = new AdminController();
            List<ClientTypeInfo> clientTypes = aCont.Get_ClientTypesByPortalId(PortalId);
            foreach (ClientTypeInfo clientType in clientTypes)
            {
                ddlAgencyClientType.Items.Add(new ListItem(clientType.ClientType, clientType.Id.ToString()));
                //ddlAdvertiserClientType.Items.Add(new ListItem(clientType.ClientType, clientType.Id.ToString()));
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
        #endregion

        protected void txtAgencySearch_TextChanged(object sender, EventArgs e)
        {
            FillAgencyList();
        }
    }
}