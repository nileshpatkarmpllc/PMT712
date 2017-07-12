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
    public partial class PMT_ReportDesigner : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            FillAdvertiserList();
            FillAgencyList();
            fillReports();
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
        private void fillReports()
        {
            AdminController aCont = new AdminController();
            List<ReportInfo> reports = aCont.Get_ReportsByPortalId(PortalId);
            List<ReportInfo> reportsByType = new List<ReportInfo>();
            List<ReportInfo> reportsByAdvertiser = new List<ReportInfo>();
            List<ReportInfo> reportsByAgency = new List<ReportInfo>();
            List<ReportInfo> reportsByKeyword = new List<ReportInfo>();
            List<ReportInfo> reportsByUser = new List<ReportInfo>();
            if(ddlReportTypeSearch.SelectedIndex != 0)
            {
                foreach(ReportInfo report in reports)
                {
                    if(report.ReportType.ToString()==ddlReportTypeSearch.SelectedValue)
                    {
                        reportsByType.Add(report);
                    }
                }
            }
            else
            {
                reportsByType = reports;
            }
            if(ddlReportsAdvertiserSearch.SelectedIndex != 0)
            {
                foreach(ReportInfo report in reportsByType)
                {
                    if(report.AdvertiserId.ToString()==ddlReportsAdvertiserSearch.SelectedValue)
                    {
                        reportsByAdvertiser.Add(report);
                    }
                }
            }
            else
            {
                reportsByAdvertiser = reportsByType;
            }
            if (ddlReportsItemAgencySearch.SelectedIndex != 0)
            {
                foreach (ReportInfo report in reportsByAdvertiser)
                {
                    if (report.AgencyId.ToString() == ddlReportsItemAgencySearch.SelectedValue)
                    {
                        reportsByAgency.Add(report);
                    }
                }
            }
            else
            {
                reportsByAgency = reportsByAdvertiser;
            }
            if (txtReportsSearch.Text != "")
            {
                foreach (ReportInfo report in reportsByAgency)
                {
                    if (report.ReportName.ToLower().IndexOf(txtReportsSearch.Text.ToLower()) != -1 ||
                        report.Keyword.ToLower().IndexOf(txtReportsSearch.Text.ToLower()) != -1 ||
                        report.EmailMessage.ToLower().IndexOf(txtReportsSearch.Text.ToLower()) != -1 ||
                        report.EmailTo.ToLower().IndexOf(txtReportsSearch.Text.ToLower()) != -1)
                    {
                        reportsByKeyword.Add(report);
                    }
                }
            }
            else
            {
                reportsByKeyword = reportsByAgency;
            }
            if (chkShowMine.Checked)
            {
                foreach (ReportInfo report in reportsByKeyword)
                {
                    if (report.CreatedById == UserId)
                    {
                        reportsByUser.Add(report);
                    }
                }
            }
            else
            {
                reportsByUser = reportsByKeyword;
            }

            if (ViewState["ReportsPage"] != null)
            {
                gvReports.PageIndex = Convert.ToInt32(ViewState["ReportsPage"]);
            }
            lblReportsMessage.Text = reportsByUser.Count.ToString() + " Reports found.";
            gvReports.DataSource = reportsByUser;
            gvReports.DataBind();
        }
        private void clearReport()
        {
            btnSendNow.Enabled = false;
            btnSendWithEmail.Enabled = false;
            btnSaveReportAs.Enabled = false;
            btnDeleteReport.Enabled = false;
            txtSelectedReport.Value = "-1";
            ddlReportType.SelectedIndex = 0;
            ddlReportAdvertiser.SelectedIndex = 0;
            ddlReportAgency.SelectedIndex = 0;
            ddlReportFrequency.SelectedIndex = 0;
            ddlReportStatus.SelectedIndex = 0;
            txtReportCreatedBy.Value = "-1";
            txtReportCreatedDate.Value = "";
            txtEmailMessage.Text = "Dear Customer,\r\n\r\nPacific Media Technologies, Inc. is pleased to send you this [report_frequency] [report_type] report entitled [report_name] for [report_date].\r\n\r\n[link]\r\n\r\nThank you";
            txtReportEmailTo.Text = "";
            txtReportKeyword.Text = "";
            txtReportName.Text = "";
            txtReportStartDate.Text = "";
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
        private int getSelCase()
        {
            //0= none selected, 1=Only Ad Selected, 2= Only Ag selected, 3=Both Selected
            string selAd = "";
            string selAg = "";
            bool adSel = false;
            bool agSel = false;
            if (ViewState["selAgency"] != null)
            {
                selAg = ViewState["selAgency"].ToString();
            }
            if (ViewState["selAdvertiser"] != null)
            {
                selAd = ViewState["selAdvertiser"].ToString();
            }
            if (selAd != "" && selAd != "-1")
            {
                adSel = true;
            }
            if (selAg != "" && selAg != "-1")
            {
                agSel = true;
            }
            int selCase = 0;
            if (adSel && !agSel)
            {
                selCase = 1;
            }
            else if (!adSel && agSel)
            {
                selCase = 2;
            }
            else if (adSel && agSel)
            {
                selCase = 3;
            }

            return selCase;
        }
        private int getDrawCase()
        {
            int drawCase = 0;
            int userCase = getUserCase();
            int selCase = getSelCase();
            if (userCase == 1 && selCase == 0)
            { drawCase = 1; }
            else if (userCase == 2 && selCase == 0)
            { drawCase = 2; }
            else if ((userCase == 0 || userCase == 3) && selCase == 0)
            { drawCase = 3; }
            else if (selCase == 1 && (userCase == 0 || userCase == 1 || userCase == 2 || userCase == 3))
            { drawCase = 4; }
            else if (selCase == 2 && (userCase == 0 || userCase == 1 || userCase == 2 || userCase == 3))
            { drawCase = 5; }
            else if (selCase == 3 && (userCase == 0 || userCase == 1 || userCase == 2 || userCase == 3))
            { drawCase = 6; }

            return drawCase;
        }
        private void FillAgencyList()
        {
            bool canSeeAll = false;
            int userCase = getUserCase();
            if (userCase == 0)
            {
                canSeeAll = true;
            }
            AdminController aCont = new AdminController();
            List<AgencyInfo> agencies = aCont.getAgencies(PortalId);
            ddlReportsItemAgencySearch.Items.Clear();
            ddlReportAgency.Items.Clear();
            ddlReportsItemAgencySearch.Items.Add(new ListItem("--Select Agency--", "-1"));
            ddlReportAgency.Items.Add(new ListItem("--Select Agency--", "-1"));
            List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
            if (canSeeAll)
            {
                foreach (AgencyInfo ag in agencies)
                {

                    ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                    ddlReportsItemAgencySearch.Items.Add(li);
                    ddlReportAgency.Items.Add(li);
                }
            }
            else
            {
                foreach (AgencyInfo userAg in userAgs)
                {
                    ListItem li = new ListItem(userAg.AgencyName, userAg.Id.ToString());
                    ddlReportsItemAgencySearch.Items.Add(li);
                    ddlReportAgency.Items.Add(li);
                }
            }
        }
        private void FillAdvertiserList()
        {
            bool canSeeAll = false;
            int userCase = getUserCase();
            if (userCase == 0)
            {
                canSeeAll = true;
            }
            AdminController aCont = new AdminController();
            List<AdvertiserInfo> Advertisers = aCont.getAdvertisers(PortalId); 

            ddlReportsAdvertiserSearch.Items.Clear();
            ddlReportAdvertiser.Items.Clear();
            ddlReportsAdvertiserSearch.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            ddlReportAdvertiser.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
            if (canSeeAll)
            {
                foreach (AdvertiserInfo adv in Advertisers)
                {
                    ddlReportsAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    ddlReportAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                }
            }
            else
            {
                foreach (AdvertiserInfo adv in userAds)
                {
                    ddlReportsAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    ddlReportAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                }
            }
        }
        protected void txtReportsSearch_TextChanged(object sender, EventArgs e)
        {
            fillReports();
        }

        protected void ddlReportsAdvertiserSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillReports();
        }

        protected void ddlReportsItemAgencySearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillReports();
        }

        protected void gvReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSelectedReport.Value = gvReports.SelectedDataKey.Value.ToString();
            AdminController aCont = new AdminController();
            ReportInfo Report = aCont.Get_ReportById(Convert.ToInt32(txtSelectedReport.Value));
            if(Report.Id==-1)
            {
                btnDeleteReport.Enabled = false;
                btnSaveReportAs.Enabled = false;
                btnSendNow.Enabled = false;
                btnSendWithEmail.Enabled = false;
                clearReport();
                lblEmailMessage.Text = "There was an error loading this Report.";
            }
            else
            {
                btnDeleteReport.Enabled = true;
                btnSaveReportAs.Enabled = true;
                btnSendNow.Enabled = true;
                btnSendWithEmail.Enabled = true;
                try { ddlReportType.SelectedValue = Report.ReportType.ToString(); }
                catch { }
                try { ddlReportAdvertiser.SelectedValue = Report.AdvertiserId.ToString(); }
                catch { }
                try { ddlReportAgency.SelectedValue = Report.AgencyId.ToString(); }
                catch { }
                txtReportKeyword.Text = Report.Keyword;
                try { ddlReportStatus.SelectedValue = Report.Status.ToString(); }
                catch { }
                try { ddlReportFrequency.SelectedValue = Report.Frequency; }
                catch { }
                txtReportStartDate.Text = Report.FirstReportDate.ToShortDateString();
                txtReportName.Text = Report.ReportName;
                txtReportEmailTo.Text = Report.EmailTo;
                txtEmailMessage.Text = Report.EmailMessage.Replace("<br />", Environment.NewLine);
                chkReportActive.Checked = Report.isActive;
                txtReportCreatedBy.Value = Report.CreatedById.ToString();
                txtReportCreatedDate.Value = Report.DateCreated.Ticks.ToString();
            }
            fillReports();
        }

        protected void gvReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["ReportsPage"] = e.NewPageIndex.ToString();
            fillReports();
        }

        protected void ddlReportTypeSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillReports();
        }

        protected void btnSaveReport_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            ReportInfo Report = new ReportInfo();
            Report.PortalId = PortalId;
            if (ddlReportType.SelectedIndex != 0)
                Report.ReportType = Convert.ToInt32(ddlReportType.SelectedValue);
            if (ddlReportAdvertiser.SelectedIndex != 0)
                Report.AdvertiserId = Convert.ToInt32(ddlReportAdvertiser.SelectedValue);
            if (ddlReportAgency.SelectedIndex != 0)
                Report.AgencyId = Convert.ToInt32(ddlReportAgency.SelectedValue);
            if (ddlReportFrequency.SelectedIndex != 0)
                Report.Frequency = ddlReportFrequency.SelectedValue;
            if (ddlReportStatus.SelectedIndex != 0)
                Report.Status = Convert.ToInt32(ddlReportStatus.SelectedValue);
            Report.EmailMessage = txtEmailMessage.Text;
            Report.EmailTo = txtReportEmailTo.Text;
            try
            { Report.FirstReportDate = Convert.ToDateTime(txtReportStartDate.Text); }
            catch { }
            Report.isActive = chkReportActive.Checked;
            Report.Keyword = txtReportKeyword.Text;
            Report.ReportName = txtReportName.Text;
            Report.LastModifiedById = UserId;
            Report.LastModifiedDate = DateTime.Now;
            int ReportId = -1;
            if (txtSelectedReport.Value == "-1")
            {
                //save new Report
                Report.CreatedById = UserId;
                Report.DateCreated = DateTime.Now;
                ReportId = aCont.Add_Report(Report);
                txtSelectedReport.Value = Report.ToString();
                txtReportCreatedBy.Value = UserId.ToString();
                txtReportCreatedBy.Value = DateTime.Now.Ticks.ToString();
                lblReportsMessage.Text = "Report Saved.";
                btnDeleteReport.Enabled = true;
                btnSaveReportAs.Enabled = true;
            }
            else
            {
                //update existing Report
                ReportId = Convert.ToInt32(txtSelectedReport.Value);
                Report.CreatedById = Convert.ToInt32(txtReportCreatedBy.Value);
                Report.DateCreated = new DateTime(Convert.ToInt64(txtReportCreatedDate.Value));
                Report.Id = Convert.ToInt32(txtSelectedReport.Value);
                aCont.Update_Report(Report);
                lblReportsMessage.Text = "Report Updated.";
            }
            fillReports();
        }

        protected void btnSaveReportAs_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            ReportInfo Report = new ReportInfo();
            Report.PortalId = PortalId;
            if (ddlReportType.SelectedIndex != 0)
                Report.ReportType = Convert.ToInt32(ddlReportType.SelectedValue);
            if (ddlReportAdvertiser.SelectedIndex != 0)
                Report.AdvertiserId = Convert.ToInt32(ddlReportAdvertiser.SelectedValue);
            if (ddlReportAgency.SelectedIndex != 0)
                Report.AgencyId = Convert.ToInt32(ddlReportAgency.SelectedValue);
            if (ddlReportFrequency.SelectedIndex != 0)
                Report.Frequency = ddlReportFrequency.SelectedValue;
            if (ddlReportStatus.SelectedIndex != 0)
                Report.Status = Convert.ToInt32(ddlReportStatus.SelectedValue);
            Report.EmailMessage = txtEmailMessage.Text;
            Report.EmailTo = txtReportEmailTo.Text;
            try
            { Report.FirstReportDate = Convert.ToDateTime(txtReportStartDate.Text); }
            catch { }
            Report.isActive = chkReportActive.Checked;
            Report.Keyword = txtReportKeyword.Text;
            Report.ReportName = txtReportName.Text;
            Report.LastModifiedById = UserId;
            Report.LastModifiedDate = DateTime.Now;
            Report.CreatedById = UserId;
            Report.DateCreated = DateTime.Now;
            int ReportId = aCont.Add_Report(Report);
            txtSelectedReport.Value = Report.ToString();
            txtReportCreatedBy.Value = UserId.ToString();
            txtReportCreatedBy.Value = DateTime.Now.Ticks.ToString();
            lblReportsMessage.Text = "Report Saved.";
            btnDeleteReport.Enabled = true;
            btnSaveReportAs.Enabled = true;
            fillReports();
        }

        protected void btnDeleteReport_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            int ReportId = Convert.ToInt32(txtSelectedReport.Value);
            ReportInfo report = new ReportInfo();
            report.Id = ReportId;
            lblReportsMessage.Text = "Report Deleted.";
            fillReports();
            clearReport();
        }

        protected void btnClearReport_Click(object sender, EventArgs e)
        {
            clearReport();
        }

        protected void btnSendNow_Click(object sender, EventArgs e)
        {
            if (txtSelectedReport.Value != "-1")
            {
                AdminController aCont = new AdminController();
                ReportInfo Report = aCont.Get_ReportById(Convert.ToInt32(txtSelectedReport.Value));
                string baseUrl = "http://" + Request.Url.Host + "/Reports";
                Response.Redirect(aCont.SendReport(Report, baseUrl,false));
            }
            else
            {
                lblReportsMessage.Text = "Please select a Report first.";
            }
        }

        protected void btnSendWithEmail_Click(object sender, EventArgs e)
        {
            if (txtSelectedReport.Value != "-1")
            {
                AdminController aCont = new AdminController();
                ReportInfo Report = aCont.Get_ReportById(Convert.ToInt32(txtSelectedReport.Value));
                string baseUrl = "http://" + Request.Url.Host + "/Reports";
                Response.Redirect(aCont.SendReport(Report, baseUrl, true));
            }
            else
            {
                lblReportsMessage.Text = "Please select a Report first.";
            }
        }

        protected void chkShowMine_CheckedChanged(object sender, EventArgs e)
        {
            fillReports();
        }
    }
}