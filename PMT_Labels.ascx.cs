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
    public partial class PMT_Labels : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            FillLabelList();
            fillTapeFormats();
            FillAgencyList();
            FillAdvertiserList();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (txtSelectedLabel.Value == "-1")
            //{
            //    pnlLabelA.Visible = false;
            //    pnlLabelB.Visible = false;
            //}
            if(!Page.IsPostBack && Request.QueryString["tid"]!=null)
            {
                AdminController aCont = new AdminController();
                try
                {
                    TaskInfo task = aCont.Get_TaskById(Convert.ToInt32(Request.QueryString["tid"]));
                    if(task.Id!=-1)
                    {
                        LibraryItemInfo lib = aCont.Get_LibraryItemById(task.LibraryId);
                        WOGroupStationInfo station = aCont.Get_WorkOrderGroupStationById(task.StationId);
                        ddlLabelTapeFormat.SelectedValue = station.DeliveryMethod.Replace("tf_","");
                        ddlLabelAgency.SelectedValue = lib.AgencyId.ToString();
                        ddlLabelAdvertiser.SelectedValue = lib.AdvertiserId.ToString();
                        txtLabelTitle.Text = lib.Title;
                        txtLabelDescription.Text = lib.ProductDescription;
                        txtLabelISCI.Text = lib.ISCICode;
                        txtLabelPMTMediaId.Text = lib.PMTMediaId;
                        txtLabelLength.Text = lib.MediaLength;
                        ddlLabelStandard.SelectedValue = lib.Standard;
                        txtLabelWOId.Text = task.WorkOrderId.ToString();
                        txtSelectedLabel.Value = "-1";
                        txtLabelNumber.Text = (aCont.getMaxLabelNumber(PortalId) + 1).ToString();
                    }
                }
                catch
                {
                    lblLabelMessage.Text = "Task Not Found for Label Creation.";
                }
            }
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
            //try
            //{
            //    Label.LabelNumber = Convert.ToInt32(txtLabelNumber.Text);
            //}
            //catch {
            //}
            try {
                Label.CampaignId = Convert.ToInt32(txtLabelWOId.Text);
            }
            catch { }
            Label.LastModifiedById = UserId;
            Label.LastModifiedDate = DateTime.Now;
            if (txtSelectedLabel.Value == "-1")
            {
                //save new Label
                Label.CreatedById = UserId;
                Label.DateCreated = DateTime.Now;
                Label.LabelNumber = aCont.getMaxLabelNumber(PortalId)+1;
                int LabelId = aCont.Add_Label(Label);
                txtSelectedLabel.Value = LabelId.ToString();
                txtLabelCreatedBy.Value = UserId.ToString();
                txtLabelCreatedDate.Value = DateTime.Now.Ticks.ToString();
                lblLabelMessage.Text = "Label Saved.";
                btnDeleteLabel.Enabled = true;
                btnSaveLabelAs.Enabled = true;
                //btnManageStationsInGroup.Enabled = true;
            }
            else
            {
                //update existing Label
                Label.CreatedById = Convert.ToInt32(txtLabelCreatedBy.Value);
                Label.DateCreated = new DateTime(Convert.ToInt64(txtLabelCreatedDate.Value));
                Label.Id = Convert.ToInt32(txtSelectedLabel.Value);
                aCont.Update_Label(Label);
                lblLabelMessage.Text = "Label Updated.";
                //btnManageStationsInGroup.Enabled = true;
            }
            FillLabelList();
            //fillDropDowns("");
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
            try
            {
                //Label.LabelNumber = Convert.ToInt32(txtLabelNumber.Text);
                Label.CampaignId = Convert.ToInt32(txtLabelWOId.Text);
            }
            catch { }
            Label.Notes = txtLabelNotes.Text;
            Label.LastModifiedById = UserId;
            Label.LastModifiedDate = DateTime.Now;

            //save new Label
            Label.CreatedById = UserId;
            Label.DateCreated = DateTime.Now;
            Label.LabelNumber = aCont.getMaxLabelNumber(PortalId) + 1;
            int LabelId = aCont.Add_Label(Label);
            txtSelectedLabel.Value = LabelId.ToString();
            txtLabelCreatedBy.Value = UserId.ToString();
            txtLabelCreatedDate.Value = DateTime.Now.Ticks.ToString();
            lblLabelMessage.Text = "Label Saved.";
            //btnManageStationsInGroup.Enabled = true;

            FillLabelList();
            //fillDropDowns("");
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
            //fillDropDowns("");
        }

        protected void btnClearLabel_Click(object sender, System.EventArgs e)
        {
            clearLabel();
            pnlLabelA.Visible = false;
            pnlLabelB.Visible = false;
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
            txtLabelWOId.Text = "";
            txtLabelNumber.Text = "";
        }

        private void FillLabelList()
        {
            AdminController aCont = new AdminController();
            List<LabelInfo> Labels = aCont.Get_LabelsByPortalId(PortalId);
            List<LabelInfo> lblsbyAgency = new List<LabelInfo>();
            List<LabelInfo> lblsbyAdvertiser = new List<LabelInfo>();
            List<LabelInfo> lblsbyKeyword = new List<LabelInfo>();
            if (ddlLabelAgencySearch.SelectedIndex > 0)
            {
                foreach (LabelInfo label in Labels)
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
            if (ddlLabelAdvertiserSearch.SelectedIndex > 0)
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
            if (txtLabelSearch.Text != "")
            {
                foreach (LabelInfo label in lblsbyAdvertiser)
                {
                    if (label.LabelNumber.ToString().ToLower().IndexOf(txtLabelSearch.Text.ToLower()) != -1 ||
                        label.Title.ToLower().IndexOf(txtLabelSearch.Text.ToLower()) != -1 ||
                        label.ISCICode.ToLower().IndexOf(txtLabelSearch.Text.ToLower()) != -1 ||
                        label.Description.ToLower().IndexOf(txtLabelSearch.Text.ToLower()) != -1 ||
                        label.CampaignId.ToString().IndexOf(txtLabelSearch.Text.ToLower()) != -1)
                    {
                        lblsbyKeyword.Add(label);
                    }
                }
            }
            else
            {
                lblsbyKeyword = lblsbyAdvertiser;
            }

            if (ViewState["LabelPage"] != null)
            {
                gvLabel.PageIndex = Convert.ToInt32(ViewState["LabelPage"]);
            }
            lblLabelMessage.Text = lblsbyKeyword.Count.ToString() + " Labels found.";
            gvLabel.DataSource = lblsbyKeyword;
            gvLabel.DataBind();
            if (txtLabelSearch.Text != "")
            {
                pnlLabelA.Controls.Clear();
                pnlLabelB.Controls.Clear();
                List<LabelInfo> labels = new List<LabelInfo>();
                int woid = -1;
                try { woid = Convert.ToInt32(txtLabelSearch.Text); }
                catch { }
                if (woid != -1)
                {
                    labels = aCont.Get_LabelsByPortalId(PortalId).Where(o => o.CampaignId == woid).ToList();
                    foreach (LabelInfo label in labels)
                    {
                        printLabel(label);
                    }
                }
            }
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
                if (Label.AgencyId > -1)
                {
                    ddlLabelAgency.SelectedValue = Label.AgencyId.ToString();
                }
                else
                {
                    if (ddlLabelAgency.SelectedIndex >= 0)
                    { ddlLabelAgency.Items[ddlLabelAgency.SelectedIndex].Selected = false; }
                    try
                    {
                        ddlLabelAgency.Items.FindByText(Label.AgencyName).Selected = true;
                    }
                    catch { }
                }
                if (Label.AdvertiserId > -1)
                {
                    ddlLabelAdvertiser.SelectedValue = Label.AdvertiserId.ToString();
                }
                else
                {
                    if (ddlLabelAdvertiser.SelectedIndex >= 0)
                    { ddlLabelAdvertiser.Items[ddlLabelAdvertiser.SelectedIndex].Selected = false; }
                    try
                    {
                        ddlLabelAdvertiser.Items.FindByText(Label.AdvertiserName).Selected = true;
                    }
                    catch { }
                }
                txtLabelTitle.Text = Label.Title;
                txtLabelDescription.Text = Label.Description;
                txtLabelISCI.Text = Label.ISCICode;
                txtLabelPMTMediaId.Text = Label.PMTMediaId;
                txtLabelLength.Text = Label.MediaLength;
                ddlLabelStandard.SelectedValue = Label.Standard;
                txtLabelNotes.Text = Label.Notes;
                txtLabelNumber.Text = Label.LabelNumber.ToString();
                txtLabelWOId.Text = Label.CampaignId.ToString();
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
                printLabel(Label);
                //if(tf.Label=="A")
                //{
                //    pnlLabelA.Visible = true;
                //    pnlLabelB.Visible = false;
                //    lblCustomerA.Text = Label.AdvertiserName;
                //    litBuyerA.Text = Label.AgencyName;
                //    litTitleA.Text = Label.Title;
                //    litTapecodeA.Text = "Tape Code: " + tf.TapeFormat;
                //    litDescA.Text = Label.Description;
                //    litIsciA.Text = "ISCI Code: " + Label.ISCICode;
                //    litMasterIdA.Text = Label.PMTMediaId + " " + DateTime.Now.ToShortDateString() + " " + Label.MediaLength + " " + Label.Standard + " " + Label.LabelNumber.ToString() + " WO# " + Label.CampaignId;
                //    litSpineA.Text = Label.AdvertiserName + "<br /><br />" + Label.Title + "<br /><br />ISCI<br />" + Label.ISCICode + "<br /><br />" + Label.Description + "<br /><br />" + Label.PMTMediaId + "<br />" + Label.MediaLength + " " + Label.Standard;
                //    litBoxFrontA.Text = Label.AdvertiserName + "<br />" + Label.AgencyName + "<br />" + Label.Title + "<br />Tape Code: " + tf.TapeFormat + "&nbsp;&nbsp;&nbsp;&nbsp;" + Label.Description + "<br />ISCI: " + Label.ISCICode + "<br /><br /><br />" + Label.PMTMediaId + " " + DateTime.Now.ToShortDateString() + " " + Label.MediaLength + " " + Label.Standard + "<br />" + Label.LabelNumber.ToString() + " WO# " + Label.CampaignId;
                //}
                //else if (tf.Label == "B")
                //{
                //    pnlLabelA.Visible = false;
                //    pnlLabelB.Visible = true;
                //    litDVDTLB.Text = Label.PMTMediaId + "<br />" + Label.MediaLength + "<br />" + Label.Standard;
                //    UserInfo csr = UserController.GetUserById(PortalId, Label.CreatedById);
                //    litDVDTRB.Text = Label.LabelNumber.ToString() + "<br />" + csr.DisplayName + "<br />" + Label.DateCreated.ToShortDateString();
                //    litDVDBottB.Text = Label.AdvertiserName + "<br />" + Label.AgencyName + "<br />" + Label.Title + "<br />" + Label.Description + " " + Label.ISCICode + "<br />";
                //}
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
            //clearAll("Labels");
        }

        protected void gvLabel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["LabelPage"] = e.NewPageIndex.ToString();
            FillLabelList();
            //gvLabel.PageIndex = e.NewPageIndex;
            //gvLabel.DataBind();
        }
        protected void btnLabelSearch_Click(object sender, System.EventArgs e)
        {
            FillLabelList();
        }
        #endregion
        #region fill Methods
        private void fillTapeFormats()
        {
            AdminController aCont = new AdminController();
            List<TapeFormatInfo> TapeFormats = aCont.Get_TapeFormatsByPortalId(PortalId);
            ddlLabelTapeFormat.Items.Add(new ListItem("--Please Select--", "-1"));
            foreach (TapeFormatInfo TapeFormat in TapeFormats)
            {
                ddlLabelTapeFormat.Items.Add(new ListItem(TapeFormat.TapeFormat, TapeFormat.Id.ToString()));
            }
        }
        private void FillAgencyList()
        {
            AdminController aCont = new AdminController();
            List<AgencyInfo> agencies = aCont.Get_AgenciesByPortalId(PortalId);
            List<AgencyInfo> agenciesFiltered = new List<AgencyInfo>();
            //if (txtAgencySearch.Text != "")
            //{
            //    foreach (AgencyInfo agency in agencies)
            //    {
            //        if (agency.AgencyName.ToLower().IndexOf(txtAgencySearch.Text.ToLower()) != -1)
            //        {
            //            agenciesFiltered.Add(agency);
            //        }
            //    }
            //}
            //else
            //{
            //    agenciesFiltered = agencies;
            //}
            //gvAgency.DataSource = agencies;
            //if (ViewState["AgencyPage"] != null)
            //{
            //    gvAgency.PageIndex = Convert.ToInt32(ViewState["AgencyPage"]);
            //}
            //lblAgencyMessage.Text = agenciesFiltered.Count.ToString() + " Agencies found.";
            //gvAgency.DataBind();
            //ddlStationGroupAgency.Items.Clear();
            ddlLabelAgencySearch.Items.Clear();
            //ddlStationGroupAgency.Items.Add(new ListItem("--Select Agency--", "-1"));
            ddlLabelAgencySearch.Items.Add(new ListItem("--Select Agency--", "-1"));
            ddlLabelAgency.Items.Clear();
            ddlLabelAgency.Items.Add(new ListItem("--Select Agency--", "-1"));
            //ddlMasterItemAgencySearch.Items.Add(new ListItem("--Select Agency--", "-1"));
            List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            foreach (AgencyInfo agency in agencies)
            {
                //ddlStationGroupAgency.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                ddlLabelAgency.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                ddlLabelAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                //lbxUserAgencies.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                //if (isAdmin)
                //{
                //    ddlMasterItemAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                //}
                //else
                //{
                //    foreach (AgencyInfo ag in userAgs)
                //    {
                //        if (ag.Id == agency.Id)
                //        {
                //            ddlMasterItemAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
                //        }
                //    }
                //}
            }
        }
        private void FillAdvertiserList()
        {
            AdminController aCont = new AdminController();
            List<AdvertiserInfo> Advertisers = aCont.Get_AdvertisersByPortalId(PortalId);
            List<AdvertiserInfo> advertisersByAgency = new List<AdvertiserInfo>();
            List<AdvertiserInfo> advertisersFiltered = new List<AdvertiserInfo>();
            
            ddlLabelAdvertiser.Items.Clear();
            ddlLabelAdvertiserSearch.Items.Clear();
            ddlLabelAdvertiser.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            ddlLabelAdvertiserSearch.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            foreach (AdvertiserInfo adv in Advertisers)
            {
                ddlLabelAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                ddlLabelAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
            }
        }
        #endregion

        protected void txtLabelSearch_TextChanged(object sender, EventArgs e)
        {
            FillLabelList();
        }

        protected void ddlLabelAgencySearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillLabelList();
        }

        protected void ddlLabelAdvertiserSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillLabelList();
        }
        private void printLabel(LabelInfo label)
        {
            AdminController aCont = new AdminController();
            TapeFormatInfo tf = aCont.Get_TapeFormatById(label.TapeFormat);
            if (tf.Label == "A")
            {
                pnlLabelA.Visible = true;
                pnlLabelB.Visible = false;
                Literal lit = new Literal();
                lit.ID = "labelALit_" + label.Id.ToString();
                lit.Text = "<div class=\"LabelA\">";
                lit.Text += "<div class=\"customerA labelBox\">" + label.AdvertiserName + "<br /></div>";
                lit.Text += "<div class=\"buyerA labelBox\">" + label.AgencyName + "<br /></div>";
                lit.Text += "<div class=\"titleA labelBox\">" + label.Title + "<br /></div>";
                lit.Text += "<div class=\"tapecodeA labelBox\">Tape Code: " + tf.TapeFormat + "</div>";
                lit.Text += "<div class=\"descA labelBox\">" + label.Description + "<br /></div>";
                lit.Text += "<div class=\"isciA labelBox\">" + label.ISCICode + "<br /></div>";
                lit.Text += "<div class=\"masterIdA labelBox\">" + label.PMTMediaId + "<br /></div>";
                lit.Text += "<div class=\"spineA labelBox\">" + label.AdvertiserName + "<br /><br />" + label.Title + "<br /><br />ISCI<br />" + label.ISCICode + "<br /><br />" + label.Description + "<br /><br />" + label.PMTMediaId + "<br />" + label.MediaLength + " " + label.Standard + "</div>";
                lit.Text += "<div class=\"boxFrontA labelBox\">" + label.AdvertiserName + "<br />" + label.AgencyName + "<br />" + label.Title + "<br />Tape Code: " + tf.TapeFormat + "&nbsp;&nbsp;&nbsp;&nbsp;" + label.Description + "<br />ISCI: " + label.ISCICode + "<br /><br /><br />" + label.PMTMediaId + " " + DateTime.Now.ToShortDateString() + "<br />" + label.MediaLength + " " + label.Standard + "<br />" + label.LabelNumber.ToString() + " WO# " + label.CampaignId + "</div>";
                lit.Text += "</div>";
                pnlLabelA.Controls.Add(lit);
            }
            else if (tf.Label == "B")
            {
                pnlLabelA.Visible = false;
                pnlLabelB.Visible = true;
                Literal lit = new Literal();
                lit.ID = "labelBLit_" + label.Id.ToString();
                lit.Text = "<div class=\"LabelB\">";
                lit.Text += "<div class=\"DVDTLB labelBox\">" + label.PMTMediaId + "<br />" + label.MediaLength + "<br />" + label.Standard + "</div>";
                UserInfo csr = UserController.GetUserById(PortalId, label.CreatedById);
                lit.Text += "<div class=\"DVDTRB labelBox\">" + label.LabelNumber.ToString() + "<br />" + csr.DisplayName + "<br />" + label.DateCreated.ToShortDateString() + "</div>";
                lit.Text += "<div class=\"DVDBottB labelBox\">" + label.AdvertiserName + "<br />" + label.AgencyName + "<br />" + label.Title + "<br />" + label.Description + " " + label.ISCICode + "<br />" + "</div></div>";
                pnlLabelB.Controls.Add(lit);
            }
        }

        protected void btnPrintAll_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            if(txtLabelSearch.Text != "")
            {
                List<LabelInfo> labels = new List<LabelInfo>();
                int woid = -1;
                try { woid = Convert.ToInt32(txtLabelSearch.Text); }
                catch { }
                if(woid != -1)
                {
                    labels = aCont.Get_LabelsByPortalId(PortalId).Where(o => o.CampaignId == woid).ToList();
                    foreach(LabelInfo label in labels)
                    {
                        printLabel(label);
                    }
                }
                FillLabelList();
                lblLabelMessage.Text = labels.Count().ToString() + " Labels ready to print.";
            }
        }
    }
}