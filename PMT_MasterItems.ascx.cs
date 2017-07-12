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
    public partial class PMT_MasterItems : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            FillAdvertiserList();
            fillMasterItemAgencies();
            FillAgencyList();
            FillMasterItemList();
            fillBulkAgencies();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            bool canSeeAdvertisers = false;
            int usercase = getUserCase();
            if (usercase == 0)
            {
                canSeeAdvertisers = true;
            }
            ddlMasterItemAdvertiserSearch.Visible = canSeeAdvertisers;
            if(!Page.IsPostBack)
            {
                if(Request.QueryString["miid"]!=null)
                {
                    AdminController aCont = new AdminController();
                    try
                    {
                        string sMid = Request.QueryString["miid"].ToString();
                        int mid = Convert.ToInt32(sMid);
                        MasterItemInfo master = aCont.Get_MasterItemById(mid);
                        txtSelectedMasterItem.Value = master.Id.ToString();
                        if (master.Id != -1)
                        {
                            btnDeleteMasterItem.Enabled = true;
                            btnSaveMasterItemAs.Enabled = true;
                            txtMasterItemFile.Text = master.Filename;

                            lbxMasterItemAgencies.Items.Clear();
                            List<AgencyInfo> ags = aCont.Get_AgenciesByAdvertiserId(master.AdvertiserId);
                            foreach (AgencyInfo ag in ags)
                            {
                                ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                                foreach (AgencyInfo agSelected in master.Agencies)
                                {
                                    if (agSelected.Id == ag.Id)
                                    {
                                        li.Selected = true;
                                    }
                                }
                                lbxMasterItemAgencies.Items.Add(li);
                            }

                            try
                            {
                                ddlMasterItemAdvertiser.SelectedValue = master.AdvertiserId.ToString();
                            }
                            catch { }
                            if (master.BillToId == -1)
                            {
                                try
                                {
                                    ddlMasterItemBillTo.SelectedValue = ddlMasterItemAdvertiser.SelectedValue;
                                }
                                catch { }
                            }
                            txtMasterItemTitle.Text = master.Title;
                            txtMasterItemComment.Text = master.Comment;
                            txtChecklistForm.Value = master.CheckListForm;
                            chkApproved.Checked = master.isApproved;
                            if (!master.hasChecklist)
                            {
                                btnMasterChecklist.Visible = false;
                                chkApproved.Enabled = false;
                                //btnMasterChecklist.CssClass = "btn btn-blue";
                            }
                            else
                            {
                                btnMasterChecklist.Visible = true;
                                chkApproved.Enabled = true;
                                btnMasterChecklist.CssClass = "btn greenButton";
                            }
                            if (master.isApproved)
                            {
                                btnCreateLibraryItem.Visible = true;
                                pnlBulk.Visible = true;
                            }
                            else
                            {
                                btnCreateLibraryItem.Visible = false;
                                pnlBulk.Visible = false;
                            }
                            ddlMasterItemMediaType.SelectedValue = master.MediaType.ToString();
                            txtMasterItemPMTMediaId.Text = master.PMTMediaId;
                            txtMasterItemLength.Text = master.Length;
                            //txtMasterItemCustomerId.Text = MasterItem.CustomerId;
                            txtMasterItemTitle.Enabled = false;
                            txtMasterItemPMTMediaId.Text = master.PMTMediaId;
                            txtMasterItemPMTMediaId.Enabled = false;
                            if (master.Reel > 0)
                            {
                                txtMasterItemReel.Text = master.Reel.ToString();
                            }
                            txtMasterItemTapeCode.Text = master.TapeCode;
                            if (master.Position > 0)
                            {
                                txtMasterItemPostition.Text = master.Position.ToString();
                            }
                            txtMasterItemVaultId.Text = master.VaultId;
                            txtMasterItemLocation.Text = master.Location;
                            chkMasterItemClosedCaption.Checked = master.ClosedCaptioned;
                            ddlMasterItemStandard.SelectedValue = master.Standard.ToString();
                            ddlMasterItemEncode.SelectedValue = master.Encode.ToString();
                            txtMasterItemCreatedBy.Value = master.CreatedById.ToString();
                            txtMasterItemCreatedDate.Value = master.DateCreated.Ticks.ToString(); ;
                            btnDeleteMasterItem.Enabled = true;
                            btnSaveMasterItemAs.Enabled = true;
                            lblMasterItemMessage.Text = "";
                            txtMasterItemSearch.Text = master.PMTMediaId;
                            FillMasterItemList();
                        }
                        else
                        {
                            Response.Redirect("~/Master-Items");
                        }
                    }
                    catch { }
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
        #region MasterItem Methods
        private void FillAgencyList()
        {
            bool canSeeAll = false;
            int userCase = getUserCase();
            //TODO:Make these roles a setting
            //if (UserInfo.IsInRole("Administrators") || UserInfo.IsInRole("PMT Admin") || UserInfo.IsInRole("PMT ADMIN"))
            //{
            if (userCase == 0)
            {
                canSeeAll = true;
            }
            //}
            AdminController aCont = new AdminController();
            List<AgencyInfo> agencies = aCont.getAgencies(PortalId); //aCont.Get_AgenciesByPortalId(PortalId);
            ddlMasterItemAgencySearch.Items.Add(new ListItem("--Select Agency--", "-1"));
            List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            //ddlMasterItemAgencySearch.Items.Clear();
            if (canSeeAll)
            {
                foreach (AgencyInfo ag in agencies)
                {

                    ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                    ddlMasterItemAgencySearch.Items.Add(li);
                }
            }
            else
            {
                foreach (AgencyInfo userAg in userAgs)
                {
                    ListItem li = new ListItem(userAg.AgencyName, userAg.Id.ToString());
                    ddlMasterItemAgencySearch.Items.Add(li);
                }
            }
            //foreach (AgencyInfo agency in agencies)
            //{
            //    if (isAdmin)
            //    {
            //        ddlMasterItemAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
            //    }
            //    else
            //    {
            //        foreach (AgencyInfo ag in userAgs)
            //        {
            //            if (ag.Id == agency.Id)
            //            {
            //                ddlMasterItemAgencySearch.Items.Add(new ListItem(agency.AgencyName, agency.Id.ToString()));
            //            }
            //        }
            //    }
            //}
        }
        private void FillAdvertiserList()
        {
            bool canSeeAll = false;
            int userCase = getUserCase();
            //TODO:Make these roles a setting
            //if (UserInfo.IsInRole("Administrators") || UserInfo.IsInRole("PMT Admin") || UserInfo.IsInRole("PMT ADMIN"))
            //{
            if (userCase == 0)
            {
                canSeeAll = true;
            }
            //}
            AdminController aCont = new AdminController();
            List<AdvertiserInfo> Advertisers = aCont.getAdvertisers(PortalId); //aCont.Get_AdvertisersByPortalId(PortalId);
            
            ddlMasterItemAdvertiser.Items.Clear();
            ddlMasterItemAdvertiser.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            ddlMasterItemAdvertiserSearch.Items.Add(new ListItem("--Select Advertiser--", "-1"));
            ddlMasterItemBillTo.Items.Add(new ListItem("--Select Bill To--", "-1"));
            List<AdvertiserInfo> userAds = aCont.Get_AdvertisersByUser(UserId, PortalId);
            bool isAdmin = UserInfo.IsInRole("Administrators");
            if (canSeeAll)
            {
                foreach (AdvertiserInfo adv in Advertisers)
                {
                    ddlMasterItemAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    ddlMasterItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    ddlMasterItemBillTo.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                }
            }
            else
            {
                foreach (AdvertiserInfo adv in userAds)
                {
                    ddlMasterItemAdvertiser.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    ddlMasterItemAdvertiserSearch.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                    ddlMasterItemBillTo.Items.Add(new ListItem(adv.AdvertiserName, adv.Id.ToString()));
                }
            }
            fillMasterItemAgencies();
        }
        protected void btnMasterItemSearch_Click(object sender, EventArgs e)
        {
            FillMasterItemList();
        }
        protected void btnSaveMasterItem_Click(object sender, System.EventArgs e)
        {
            if(Application["MasterItems"]!=null)
            {
                Application.Remove("MasterItems");
            }
            AdminController aCont = new AdminController();
            MasterItemInfo MasterItem = new MasterItemInfo();
            MasterItem.PortalId = PortalId;
            MasterItem.Filename = txtMasterItemFile.Text;
            MasterItem.AdvertiserId = Convert.ToInt32(ddlMasterItemAdvertiser.SelectedValue);
            MasterItem.Title = txtMasterItemTitle.Text;
            MasterItem.MediaType = Convert.ToInt32(ddlMasterItemMediaType.SelectedValue);
            MasterItem.Encode = Convert.ToInt32(ddlMasterItemEncode.SelectedValue);
            MasterItem.Standard = ddlMasterItemStandard.SelectedValue;
            MasterItem.Length = aCont.fixLength(txtMasterItemLength.Text);
            MasterItem.CheckListForm = txtChecklistForm.Value;
            MasterItem.hasChecklist = chkHasChecklist.Checked;
            //MasterItem.CustomerId = txtMasterItemCustomerId.Text;
            if(ddlMasterItemBillTo.SelectedIndex>0)
            {
                MasterItem.BillToId = Convert.ToInt32(ddlMasterItemBillTo.SelectedValue);
            }
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
            MasterItem.isApproved = chkApproved.Checked;
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
                //btnManageStationsInGroup.Enabled = true;
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
                if(MasterItem.isApproved)
                {
                    btnCreateLibraryItem.Visible = true;
                    pnlBulk.Visible = true;
                }
                else
                {
                    btnCreateLibraryItem.Visible = false;
                    pnlBulk.Visible = false;
                }
                if(MasterItem.hasChecklist)
                {
                    btnMasterChecklist.Visible = true;
                }
                else
                {
                    btnMasterChecklist.Visible = false;
                }
                //btnManageStationsInGroup.Enabled = true;
            }
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
            //fillDropDowns("");
        }

        protected void btnSaveMasterItemAs_Click(object sender, System.EventArgs e)
        {
            if (Application["MasterItems"] != null)
            {
                Application.Remove("MasterItems");
            }
            AdminController aCont = new AdminController();
            MasterItemInfo MasterItem = new MasterItemInfo();
            MasterItem.PortalId = PortalId;
            MasterItem.Filename = txtMasterItemFile.Text;
            MasterItem.AdvertiserId = Convert.ToInt32(ddlMasterItemAdvertiser.SelectedValue);
            MasterItem.Title = txtMasterItemTitle.Text;
            MasterItem.MediaType = Convert.ToInt32(ddlMasterItemMediaType.SelectedValue);
            MasterItem.Encode = Convert.ToInt32(ddlMasterItemEncode.SelectedValue);
            MasterItem.Standard = ddlMasterItemStandard.SelectedValue;
            MasterItem.Length = aCont.fixLength(txtMasterItemLength.Text);
            //MasterItem.CustomerId = txtMasterItemCustomerId.Text;
            MasterItem.PMTMediaId = txtMasterItemPMTMediaId.Text;
            MasterItem.CheckListForm = txtChecklistForm.Value;
            MasterItem.hasChecklist = chkHasChecklist.Checked;
            MasterItem.ClosedCaptioned = chkMasterItemClosedCaption.Checked;
            if (ddlMasterItemBillTo.SelectedIndex > 0)
            {
                MasterItem.BillToId = Convert.ToInt32(ddlMasterItemBillTo.SelectedValue);
            }
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
            MasterItem.isApproved = chkApproved.Checked;
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
            //btnManageStationsInGroup.Enabled = true;

            //save agencies
            aCont.Delete_MasterItemAgencyByMasterItemId(MasterItemId);
            foreach (ListItem li in lbxMasterItemAgencies.Items)
            {
                if (li.Selected)
                {
                    aCont.Add_MasterItemAgency(MasterItemId, Convert.ToInt32(li.Value));
                }
            }
            if (MasterItem.hasChecklist)
            {
                btnMasterChecklist.Visible = true;
            }
            else
            {
                btnMasterChecklist.Visible = false;
            }

            FillMasterItemList();
            //fillDropDowns("");
        }

        protected void btnDeleteMasterItem_Click(object sender, System.EventArgs e)
        {
            AdminController aCont = new AdminController();
            int MasterItemId = Convert.ToInt32(txtSelectedMasterItem.Value);
            MasterItemInfo MasterItem = new MasterItemInfo();
            MasterItem.Id = MasterItemId;
            aCont.Delete_MasterItem(MasterItem);
            lblMasterItemMessage.Text = "MasterItem Deleted.";
            if (Application["MasterItems"] != null)
            {
                Application.Remove("MasterItems");
            }
            clearMasterItem();
            FillMasterItemList();
            //fillDropDowns("");
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
            chkApproved.Checked = false;
            chkApproved.Enabled = false;
            txtChecklistForm.Value = "";
            ddlMasterItemMediaType.SelectedIndex = 0;
            txtMasterItemPMTMediaId.Text = "";
            txtMasterItemLength.Text = "";
            //txtMasterItemCustomerId.Text = "";
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
            ddlMasterStatus.SelectedIndex = 0;
            lblMasterItemMessage.Text = "";
            btnMasterChecklist.Visible = false;
            chkHasChecklist.Checked = false;
            //lblUserMessage.Text = "";
        }

        private void FillMasterItemList()
        {
            AdminController aCont = new AdminController();
            bool canSeeAll = false;
            int userCase = getUserCase();
            int drawCase = getDrawCase();
            if (userCase == 0)
            {
                canSeeAll = true;
            }
            List<MasterItemInfo> MasterItems = new List<MasterItemInfo>();
            List<MasterItemInfo> mstrsByUser = new List<MasterItemInfo>();
            List<AgencyInfo> agencies = aCont.Get_AgenciesByUser(UserId);
            List<AdvertiserInfo> advertisers = aCont.Get_AdvertisersByUser(UserId, PortalId);
            bool isAgencyRole = UserInfo.IsInRole("Agency");
            bool isAdvertiserRole = UserInfo.IsInRole("Advertiser");
            MasterItems = aCont.Get_MasterItemsByPortalId(PortalId);
            if (canSeeAll)
            {
                mstrsByUser = MasterItems;
            }
            else
            {
                //TODO:Add specific user level view filtering
                //Should also filter ag and ad list by permissions
                if (isAdvertiserRole && advertisers.Count > 0 && agencies.Count == 0)
                {
                    //in advertiser role and only tagged with advertisers.  Show all masters tagged with these advertisers
                    foreach (MasterItemInfo master in MasterItems)
                    {
                        foreach (AdvertiserInfo ad in advertisers)
                        {
                            if (master.AdvertiserId == ad.Id)
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
                            foreach (AgencyInfo ag1 in master.Agencies)
                            {
                                if (ag.Id == ag1.Id)
                                {
                                    mstrsByUser.Add(master);
                                }
                            }
                        }
                    }
                }
                else if (isAgencyRole && !isAdvertiserRole && agencies.Count > 0 && advertisers.Count > 0)
                {
                    //in agency role, not advertiser role, and tagged with both.  Only show intersection
                    foreach (MasterItemInfo master in MasterItems)
                    {
                        foreach (AgencyInfo ag in agencies)
                        {
                            foreach (AgencyInfo ag1 in master.Agencies)
                            {
                                foreach (AdvertiserInfo ad in advertisers)
                                {
                                    if (ag.Id == ag1.Id && ad.Id == master.AdvertiserId)
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
            List<MasterItemInfo> mstrsbyStatus = new List<MasterItemInfo>();
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
                    if (MasterItem.Length.IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                        (MasterItem.PMTMediaId.ToString().ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                        MasterItem.Title.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                        MasterItem.Filename.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1 ||
                        MasterItem.CustomerId.ToLower().IndexOf(txtMasterItemSearch.Text.ToLower()) != -1) ||
                        MasterItem.Id.ToString()==txtMasterItemSearch.Text)
                    {
                        mstrsbyKeyword.Add(MasterItem);
                    }
                }
            }
            else
            {
                mstrsbyKeyword = mstrsbyAgency;
            }
            if (ddlMasterStatus.SelectedIndex > 0)
            {
                //0=None,1=new,2=pending,3=approved
                foreach (MasterItemInfo MasterItem in mstrsbyKeyword)
                {

                    if (ddlMasterStatus.SelectedValue == "1" && !MasterItem.hasChecklist)
                    {
                        mstrsbyStatus.Add(MasterItem);
                    }
                    else if (ddlMasterStatus.SelectedValue == "2" && MasterItem.hasChecklist && !MasterItem.isApproved)
                    {
                        mstrsbyStatus.Add(MasterItem);
                    }
                    else if (ddlMasterStatus.SelectedValue == "3" && MasterItem.hasChecklist && MasterItem.isApproved)
                    {
                        mstrsbyStatus.Add(MasterItem);
                    }
                }

            }
            else
            {
                mstrsbyStatus = mstrsbyKeyword;
            }

            //hydrate advertisers and agencies
            List<AdvertiserInfo> allAds = getAdvertisers();
            List<AgencyInfo> allAgs = getAgencies();
            foreach(MasterItemInfo master in mstrsbyKeyword)
            {
                string ags = "";
                var advertiser = allAds.FirstOrDefault(i => i.Id == master.AdvertiserId);
                if (advertiser != null)
                {
                    master.Advertiser = advertiser.AdvertiserName;
                }
                foreach(AgencyInfo ag in master.Agencies)
                {
                    var agency = allAgs.FirstOrDefault(i => i.Id == ag.Id);
                    if (agency != null)
                    {
                        if (isAgencyRole && !isAdvertiserRole)
                        {
                            var inag = agencies.FirstOrDefault(o => o.Id == ag.Id);
                            if(inag != null)
                            {
                                ags += agency.AgencyName + ", ";
                            }
                        }
                        else
                        {
                            ags += agency.AgencyName + ", ";
                        }
                    }
                }
                if (ags.Length > 2)
                {
                    ags = ags.Substring(0, ags.Length - 2);
                }
                master.AgencyNames = ags;
            }

            if (ViewState["MasterItemPage"] != null)
            {
                gvMasterItem.PageIndex = Convert.ToInt32(ViewState["MasterItemPage"]);
            }
            //if(ViewState["masterspage"]!= null)
            //{
            //    int startindex = (int)ViewState["masterspage"];
            //    gvMasterItem. = startindex;
            //}
            lblMasterItemMessage.Text = mstrsbyStatus.Count.ToString() + " Master Items found.";
            gvMasterItem.DataSource = mstrsbyStatus;
            gvMasterItem.DataBind();
        }

        private void fillBulkAgencies()
        {
            ddlBulkAgencies.Items.Clear();
            AdminController aCont = new AdminController();
            if (txtSelectedMasterItem.Value != "-1")
            {
                MasterItemInfo MasterItem = aCont.Get_MasterItemById(Convert.ToInt32(txtSelectedMasterItem.Value));
                ddlBulkAgencies.Items.Add(new ListItem("--Select Agency--",""));
                foreach(AgencyInfo ag in MasterItem.Agencies)
                {
                    ddlBulkAgencies.Items.Add(new ListItem(ag.AgencyName, ag.Id.ToString()));
                }
                if(MasterItem.Agencies.Count==1)
                {
                    ddlBulkAgencies.SelectedIndex = 1;
                }
            }
        }

        protected void gvMasterItem_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillMasterItemList();
            btnMasterChecklist.Visible = true;
            txtSelectedMasterItem.Value = gvMasterItem.SelectedDataKey.Value.ToString();
            //btnMasterChecklist.OnClientClick = "window.open('check-list.aspx?Master=" + txtSelectedMasterItem.Value + "','_blank')";
            AdminController aCont = new AdminController();
            MasterItemInfo MasterItem = aCont.Get_MasterItemById(Convert.ToInt32(txtSelectedMasterItem.Value));
            if (MasterItem.Id != -1)
            {
                fillBulkAgencies();
                btnDeleteMasterItem.Enabled = true;
                btnSaveMasterItemAs.Enabled = true;
                txtMasterItemFile.Text = MasterItem.Filename;

                lbxMasterItemAgencies.Items.Clear();
                List<AgencyInfo> ags = aCont.Get_AgenciesByAdvertiserId(MasterItem.AdvertiserId);
                foreach (AgencyInfo ag in ags)
                {
                    ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                    foreach (AgencyInfo agSelected in MasterItem.Agencies)
                    {
                        if (agSelected.Id == ag.Id)
                        {
                            li.Selected = true;
                        }
                    }
                    lbxMasterItemAgencies.Items.Add(li);
                }
                try
                {
                    ddlMasterItemAdvertiser.SelectedValue = MasterItem.AdvertiserId.ToString();
                }
                catch { }
                if(MasterItem.BillToId==-1)
                {
                    try
                    {
                        ddlMasterItemBillTo.SelectedValue = ddlMasterItemAdvertiser.SelectedValue;
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        ddlMasterItemBillTo.SelectedValue = MasterItem.BillToId.ToString();
                    }
                    catch { }
                }
                txtMasterItemTitle.Text = MasterItem.Title;
                txtMasterItemComment.Text = MasterItem.Comment;
                txtChecklistForm.Value = MasterItem.CheckListForm;
                chkApproved.Checked = MasterItem.isApproved;
                chkHasChecklist.Checked = MasterItem.hasChecklist;
                if(!MasterItem.hasChecklist)
                {
                    btnMasterChecklist.Visible = false;
                    chkApproved.Enabled = false;
                    //btnMasterChecklist.CssClass = "btn btn-blue";
                }
                else
                {
                    btnMasterChecklist.Visible = true;
                    btnMasterChecklist.NavigateUrl = "https://s3-us-west-1.amazonaws.com/s3-pmt-bucket/Master_Checklists/" + MasterItem.PMTMediaId + ".png";
                    chkApproved.Enabled = true;
                    btnMasterChecklist.CssClass = "btn greenButton";
                }
                if(MasterItem.isApproved)
                {
                    btnCreateLibraryItem.Visible = true;
                    pnlBulk.Visible = true;
                }
                else
                {
                    btnCreateLibraryItem.Visible = false;
                    pnlBulk.Visible = false;
                }
                try
                {
                    ddlMasterItemMediaType.SelectedValue = MasterItem.MediaType.ToString();
                }
                catch { }
                txtMasterItemPMTMediaId.Text = MasterItem.PMTMediaId;
                txtMasterItemLength.Text = MasterItem.Length;
                //txtMasterItemCustomerId.Text = MasterItem.CustomerId;
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
                try
                {
                    ddlMasterItemStandard.SelectedValue = MasterItem.Standard.ToString();
                }
                catch { }
                try
                {
                    ddlMasterItemEncode.SelectedValue = MasterItem.Encode.ToString();
                }
                catch { }
                txtMasterItemCreatedBy.Value = MasterItem.CreatedById.ToString();
                txtMasterItemCreatedDate.Value = MasterItem.DateCreated.Ticks.ToString();
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
            //clearAll("MasterItems");
        }

        protected void gvMasterItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["MasterItemPage"] = e.NewPageIndex.ToString();
            //gvMasterItem.PageIndex = e.NewPageIndex;
            //gvMasterItem.DataBind();
            FillMasterItemList();
        }
        protected void ddlMasterItemAdvertiser_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMasterItemBillTo.SelectedValue = ddlMasterItemAdvertiser.SelectedValue;
            fillMasterItemAgencies();
        }

        private void fillMasterItemAgencies()
        {
            if (ddlMasterItemAdvertiser.SelectedIndex > 0)
            {
                bool canSeeAll = false;
                int userCase = getUserCase();
                //TODO:Make these roles a setting
                //if (UserInfo.IsInRole("Administrators") || UserInfo.IsInRole("PMT Admin") || UserInfo.IsInRole("PMT ADMIN"))
                //{
                if (userCase == 0)
                {
                    canSeeAll = true;
                }
                //}
                AdminController aCont = new AdminController();
                List<AgencyInfo> ags = aCont.Get_AgenciesByAdvertiserId(Convert.ToInt32(ddlMasterItemAdvertiser.SelectedValue));
                List<AgencyInfo> agSelected = aCont.Get_AgenciesByMasterItemId(Convert.ToInt32(txtSelectedMasterItem.Value));
                List<AgencyInfo> userAgs = aCont.Get_AgenciesByUser(UserId);
                lbxMasterItemAgencies.Items.Clear();
                foreach (AgencyInfo ag in ags)
                {
                    if(canSeeAll)
                    {
                        ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                        foreach (AgencyInfo agsel in agSelected)
                        {
                            if (agsel.Id == ag.Id)
                            {
                                li.Selected = true;
                            }
                        }
                        lbxMasterItemAgencies.Items.Add(li);
                    }
                    else
                    {
                        foreach (AgencyInfo userAg in userAgs)
                        {
                            if (userAg.Id == ag.Id)
                            {
                                ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                                foreach (AgencyInfo agsel in agSelected)
                                {
                                    if (agsel.Id == ag.Id)
                                    {
                                        li.Selected = true;
                                    }
                                }
                                lbxMasterItemAgencies.Items.Add(li);
                            }
                        }
                    }
                }
            }
            else
            {
                lbxMasterItemAgencies.Items.Clear();
            }
        }
        protected void ddlMasterItemAgencySearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            string id = ddl.SelectedValue;
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
                    if(ag.Id.ToString()==id)
                    {
                        li.Selected = true;
                    }
                    ddlMasterItemAgencySearch.Items.Add(li);
                }
            }
            else
            {
                List<AgencyInfo> ags = aCont.Get_AgenciesByPortalId(PortalId);
                foreach (AgencyInfo ag in ags)
                {
                    ListItem li = new ListItem(ag.AgencyName, ag.Id.ToString());
                    if(ag.Id.ToString()==id)
                    {
                        li.Selected = true;
                    }
                    ddlMasterItemAgencySearch.Items.Add(li);
                }
            }
            FillMasterItemList();
        }
        //protected void btnMasterChecklist_Click(object sender, EventArgs e)
        //{
        //    FillMasterItemList();
        //    //Response.Redirect("/check-list.aspx?Master=" + txtSelectedMasterItem.Value, "_blank", "");
        //    AdminController aCont = new AdminController();
        //    MasterItemInfo master = aCont.Get_MasterItemById(Convert.ToInt32(txtSelectedMasterItem.Value));
        //    if(master.Id!=-1)
        //    {
        //        Response.Redirect("https://s3-us-west-1.amazonaws.com/s3-pmt-bucket/Master_Checklists/" + master.PMTMediaId + ".png");
        //    }
        //}
        #endregion

        protected void btnCreateLibraryItem_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            LibraryItemInfo libItem = new LibraryItemInfo();
            MasterItemInfo master = aCont.Get_MasterItemById(Convert.ToInt32(txtSelectedMasterItem.Value));
            libItem.AdvertiserId = master.AdvertiserId;
            if (master.ClosedCaptioned)
            {
                libItem.ClosedCaptioned = "Yes";
            }
            else
            {
                libItem.ClosedCaptioned = "No";
            }
            libItem.Comment = master.Comment;
            libItem.CreatedById = UserId;
            libItem.DateCreated = DateTime.Now;
            if (master.Encode == 1)
            {
                libItem.Encode = "SPOTTRAC";
            }
            else if (master.Encode == 2)
            {
                libItem.Encode = "Teletrax";
            }
            else if (master.Encode == 3)
            {
                libItem.Encode = "VEIL";
            }
            libItem.Filename = master.Filename;
            libItem.LastModifiedById = UserId;
            libItem.LastModifiedDate = DateTime.Now;
            libItem.Location = master.Location;
            libItem.MasterId = master.Id;
            libItem.MediaLength = master.Length;
            if (master.MediaType == 2)
            {
                libItem.MediaType = "hd";
            }
            else if (master.MediaType==3)
            {
                libItem.MediaType = "HD & SD";
            }
            else if (master.MediaType == 5)
            {
                libItem.MediaType = "sd";
            }
            libItem.PMTMediaId = master.PMTMediaId;
            libItem.PortalId = PortalId;
            libItem.Position = master.Position;
            libItem.Reel = master.Reel;
            libItem.Standard = master.Standard;
            libItem.TapeCode = master.TapeCode;
            libItem.Title = master.Title;
            libItem.VaultId = master.VaultId;
            libItem.Id = aCont.Add_LibraryItem(libItem);
            Response.Redirect("/Library-Items.aspx?lid=" + libItem.Id.ToString());
        }

        protected void ddlMasterItemAgencySearch_SelectedIndexChanged1(object sender, EventArgs e)
        {
            FillMasterItemList();
        }

        protected void txtMasterItemSearch_TextChanged(object sender, EventArgs e)
        {
            FillMasterItemList();
        }

        protected void ddlMasterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillMasterItemList();
        }
        public List<AdvertiserInfo> getAdvertisers()
        {
            List<AdvertiserInfo> ads = new List<AdvertiserInfo>();
            if (Application["Advertisers"] != null)
            {
                ads = (List<AdvertiserInfo>)Application["Advertisers"];
            }
            else
            {
                AdminController aCont = new AdminController();
                ads = aCont.Get_AdvertisersByPortalId(PortalId);
                Application["Advertisers"] = ads;
            }
            return ads;
        }
        public List<AgencyInfo> getAgencies()
        {
            List<AgencyInfo> ags = new List<AgencyInfo>();
            if (Application["Agencies"] != null)
            {
                ags = (List<AgencyInfo>)Application["Agencies"];
            }
            else
            {
                AdminController aCont = new AdminController();
                ags = aCont.Get_AgenciesByPortalId(PortalId);
                Application["Agencies"] = ags;
            }
            return ags;
        }

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int masterId = -1;
            try {
                masterId = Convert.ToInt32(btn.CommandArgument);
            }
            catch { }
            if(masterId!=-1)
            {
                AdminController aCont = new AdminController();
                MasterItemInfo master = aCont.Get_MasterItemById(masterId);
                if(master.PMTMediaId != "")
                {
                    litVidSource.Text = "<source src=\"https://s3-pmt-bucket.s3.amazonaws.com/Viewers/" + master.PMTMediaId + ".mp4\" type=\"video/mp4\">";
                    mpeViewerPopup.Show();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mpeViewerPopup.Hide();
            FillMasterItemList();
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

        protected void btnBulkLibrary_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            int c = 0;
            string[] descs = txtBulkLibraryDesc.Text.Split('\n');
            string[] iscis = txtBulkIscis.Text.Split('\n');
            string[] tcs = txtBulkTapeCode.Text.Split('\n');
            if (ddlBulkAgencies.SelectedIndex == 0)
            {
                lblBulkMessage.Text = "Please select an Agency.";
                mpeBulkError.Show();
            }
            else
            {
                if (descs.Count() == iscis.Count() && descs.Count() == tcs.Count())
                {
                    //first, check for duplicate ISCI's
                    List<string> dupes = new List<string>();
                    foreach(string isci in iscis)
                    {
                        LibraryItemInfo checkIsci = aCont.Get_LibraryItemByISCI(isci);
                        {
                            if(checkIsci.Id!=-1)
                            {
                                dupes.Add(isci);
                            }
                        }
                    }
                    if (dupes.Count > 0)
                    {
                        lblBulkMessage.Text = "The following ISCI's already exist in the OMS: ";
                        foreach (string dupe in dupes)
                        {
                            lblBulkMessage.Text += dupe + ", ";
                        }
                        lblBulkMessage.Text = lblBulkMessage.Text.Substring(0, lblBulkMessage.Text.Length - 2);
                        mpeBulkError.Show();
                    }
                    else
                    {
                        for (int i = 0; i < descs.Count(); i++)
                        {
                            LibraryItemInfo libItem = new LibraryItemInfo();
                            MasterItemInfo master = aCont.Get_MasterItemById(Convert.ToInt32(txtSelectedMasterItem.Value));
                            libItem.AdvertiserId = master.AdvertiserId;
                            if (ddlBulkAgencies.SelectedIndex > 0)
                            {
                                libItem.AgencyId = Convert.ToInt32(ddlBulkAgencies.SelectedValue);
                            }
                            if (master.ClosedCaptioned)
                            {
                                libItem.ClosedCaptioned = "Yes";
                            }
                            else
                            {
                                libItem.ClosedCaptioned = "No";
                            }
                            libItem.Comment = master.Comment;
                            libItem.CreatedById = UserId;
                            libItem.DateCreated = DateTime.Now;
                            if (master.Encode == 1)
                            {
                                libItem.Encode = "SPOTTRAC";
                            }
                            else if (master.Encode == 2)
                            {
                                libItem.Encode = "Teletrax";
                            }
                            else if (master.Encode == 3)
                            {
                                libItem.Encode = "VEIL";
                            }
                            libItem.Filename = master.Filename;
                            libItem.LastModifiedById = UserId;
                            libItem.LastModifiedDate = DateTime.Now;
                            libItem.Location = master.Location;
                            libItem.MasterId = master.Id;
                            libItem.MediaLength = master.Length;
                            if (master.MediaType == 2)
                            {
                                libItem.MediaType = "hd";
                            }
                            else if (master.MediaType == 3)
                            {
                                libItem.MediaType = "HD & SD";
                            }
                            else if (master.MediaType == 5)
                            {
                                libItem.MediaType = "sd";
                            }
                            libItem.PMTMediaId = master.PMTMediaId;
                            libItem.PortalId = PortalId;
                            libItem.Position = master.Position;
                            libItem.Reel = master.Reel;
                            libItem.Standard = master.Standard;
                            libItem.TapeCode = master.TapeCode;
                            libItem.Title = master.Title;
                            libItem.VaultId = master.VaultId;
                            descs[i] = descs[i].Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
                            if (descs[i].IndexOf(".") == -1)
                            {
                                if (descs[i].Length == 11)
                                {
                                    descs[i] = descs[i].Substring(1, 10);
                                }
                                if (descs[i].Length == 10)
                                {
                                    descs[i] = "1-" + descs[i].Substring(0, 3) + "-" + descs[i].Substring(3, 3) + "-" + descs[i].Substring(6, 4);
                                }
                            }
                            libItem.ProductDescription = descs[i];
                            libItem.ISCICode = iscis[i];
                            libItem.TapeCode = tcs[i];
                            libItem.Id = aCont.Add_LibraryItem(libItem);
                            c++;
                        }
                        lblMasterItemMessage.Text = c.ToString() + " Library Items created.";
                        Application["LibraryItems"] = null;
                    }
                }
                else
                {
                    lblBulkMessage.Text = "You must have the same number of lines in all 3 fields, even if they are blank.";
                    mpeBulkError.Show();
                }
            }
        }

        protected void btnBulkErrorClose_Click(object sender, EventArgs e)
        {
            mpeBulkError.Hide();
        }
    }
}