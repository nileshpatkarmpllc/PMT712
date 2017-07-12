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
    public partial class PMT_ImportTool : PMT_AdminModuleBase, IActionable
    {
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

        protected void btnAdvertisers_Click(object sender, EventArgs e)
        {
            int c = 0;
            AdminController aCont = new AdminController();
            if(txtScript.Text.IndexOf("<!-- Table advertisers -->")==-1)
            {
                lblMessage.Text = "This appears to be the wrong file.";
            }
            else
            {
                aCont.ClearAdvertisers();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(txtScript.Text);
                string[] nodeNames = { "id", "name", "address1", "address2", "city", "state", "zipcode", "country", "phone", "fax", "client_type", "customer_reference", "carrier_id", "freight_id", "carrier_no", "quickbooks_listid", "quickbooks_editsequence", "quickbooks_errnum", "quickbooks_errmsg" };
                XmlElement xelRoot = doc.DocumentElement;
                XmlNodeList xmlNodes = xelRoot.SelectNodes("/pmt_db01/advertisers");
                List<ClientTypeInfo> types = aCont.Get_ClientTypesByPortalId(PortalId);
                foreach(XmlNode node in xmlNodes)
                {
                    c++;
                    AdvertiserInfo ad = new AdvertiserInfo();
                    ad.PortalId = PortalId;
                    foreach(string nodeName in nodeNames)
                    {
                        if(node[nodeName]!=null)
                        {
                            switch (nodeName)
                            {
                                case "id":
                                    ad.Id = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "name":
                                    ad.AdvertiserName = node[nodeName].InnerText;
                                    break;
                                case "address1":
                                    ad.Address1 = node[nodeName].InnerText;
                                    break;
                                case "address2":
                                    ad.Address2 = node[nodeName].InnerText;
                                    break;
                                case "city":
                                    ad.City = node[nodeName].InnerText;
                                    break;
                                case "state":
                                    ad.State = node[nodeName].InnerText;
                                    break;
                                case "zipcode":
                                    ad.Zip = node[nodeName].InnerText;
                                    break;
                                case "country":
                                    ad.Country = node[nodeName].InnerText;
                                    break;
                                case "phone":
                                    ad.Phone = node[nodeName].InnerText;
                                    break;
                                case "fax":
                                    ad.Fax = node[nodeName].InnerText;
                                    break;
                                case "client_type":
                                    foreach(ClientTypeInfo clType in types)
                                    {
                                        if(clType.ClientType.ToLower()==node[nodeName].InnerText.ToLower())
                                        {
                                            ad.ClientType = clType.Id;
                                        }
                                    }                                    
                                    break;
                                case "customer_reference":
                                    ad.CustomerReference = node[nodeName].InnerText;
                                    break;
                                case "carrier_id":
                                    ad.Carrier = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "freight_id":
                                    ad.Freight = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "carrier_no":
                                    ad.CarrierNumber = node[nodeName].InnerText;
                                    break;
                                case "quickbooks_listid":
                                    ad.QuickbooksListId = node[nodeName].InnerText;
                                    break;
                                case "quickbooks_editsequence":
                                    ad.QuickbooksEditSequence = node[nodeName].InnerText;
                                    break;
                                case "quickbooks_errnum":
                                    ad.QuickbooksErrNum = node[nodeName].InnerText;
                                    break;
                                case "quickbooks_errmsg":
                                    ad.QuickbooksErrMsg = node[nodeName].InnerText;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    ad.CreatedById = 1;
                    ad.DateCreated = DateTime.Now;
                    ad.LastModifiedById = 1;
                    ad.LastModifiedDate = DateTime.Now;
                    aCont.Add_AdvertiserForImport(ad);
                }
                lblMessage.Text = c.ToString() + " records imported.";
            }
        }

        protected void btnAgencies_Click(object sender, EventArgs e)
        {
            int c = 0;
            AdminController aCont = new AdminController();
            if (txtScript.Text.IndexOf("<!-- Table customers -->") == -1)
            {
                lblMessage.Text = "This appears to be the wrong file.";
            }
            else
            {
                aCont.ClearAgencies();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(txtScript.Text);
                string[] nodeNames = { "id", "status", "name", "address1", "address2", "city", "state", "zipcode", "country", "phone", "fax", "customer_type", "customer_reference" };
                XmlElement xelRoot = doc.DocumentElement;
                XmlNodeList xmlNodes = xelRoot.SelectNodes("/pmt_db01/customers");
                List<ClientTypeInfo> types = aCont.Get_ClientTypesByPortalId(PortalId);
                foreach (XmlNode node in xmlNodes)
                {
                    c++;
                    AgencyInfo ad = new AgencyInfo();
                    ad.PortalId = PortalId;
                    foreach (string nodeName in nodeNames)
                    {
                        if (node[nodeName] != null)
                        {
                            switch (nodeName)
                            {
                                case "id":
                                    ad.Id = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "status":
                                    if (node[nodeName].InnerText.ToLower() == "active")
                                        ad.Status = true;
                                    break;
                                case "name":
                                    ad.AgencyName = node[nodeName].InnerText;
                                    break;
                                case "address1":
                                    ad.Address1 = node[nodeName].InnerText;
                                    break;
                                case "address2":
                                    ad.Address2 = node[nodeName].InnerText;
                                    break;
                                case "city":
                                    ad.City = node[nodeName].InnerText;
                                    break;
                                case "state":
                                    ad.State = node[nodeName].InnerText;
                                    break;
                                case "zipcode":
                                    ad.Zip = node[nodeName].InnerText;
                                    break;
                                case "country":
                                    ad.Country = node[nodeName].InnerText;
                                    break;
                                case "phone":
                                    ad.Phone = node[nodeName].InnerText;
                                    break;
                                case "fax":
                                    ad.Fax = node[nodeName].InnerText;
                                    break;
                                case "customer_type":
                                    foreach (ClientTypeInfo clType in types)
                                    {
                                        if (clType.ClientType.ToLower() == node[nodeName].InnerText.ToLower())
                                        {
                                            ad.ClientType = clType.Id;
                                        }
                                    }
                                    break;
                                case "customer_reference":
                                    ad.CustomerReference = node[nodeName].InnerText;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    ad.CreatedById = 1;
                    ad.DateCreated = DateTime.Now;
                    ad.LastModifiedById = 1;
                    ad.LastModifiedDate = DateTime.Now;
                    aCont.Add_AgencyForImport(ad);
                }
                lblMessage.Text = c.ToString() + " records imported.";
                Application["Advertisers"] = null;
            }
        }

        protected void btnAdAgs_Click(object sender, EventArgs e)
        {
            int c = 0;
            AdminController aCont = new AdminController();
            if (txtScript.Text.IndexOf("<!-- Table advertiser_agencies -->") == -1)
            {
                lblMessage.Text = "This appears to be the wrong file.";
            }
            else
            {
                aCont.ClearAdvertiserAgencies();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(txtScript.Text);
                string[] nodeNames = { "advertiser_id", "agency_id" };
                XmlElement xelRoot = doc.DocumentElement;
                XmlNodeList xmlNodes = xelRoot.SelectNodes("/pmt_db01/advertiser_agencies");
                foreach (XmlNode node in xmlNodes)
                {
                    c++;
                    int adId = -1;
                    int agId = -1;
                    foreach (string nodeName in nodeNames)
                    {
                        if (node[nodeName] != null)
                        {
                            switch (nodeName)
                            {
                                case "advertiser_id":
                                    adId = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "agency_id":
                                    agId = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    aCont.Add_AdvertiserAgency(adId, agId);
                }
                lblMessage.Text = c.ToString() + " records imported.";
                Application["Agencies"] = null;
            }
        }

        protected void btnLabels_Click(object sender, EventArgs e)
        {
            int c = 0;
            AdminController aCont = new AdminController();
            if (txtScript.Text.IndexOf("<!-- Table label_stations -->") == -1)
            {
                lblMessage.Text = "This appears to be the wrong file.";
            }
            else
            {
                aCont.ClearLabels();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(txtScript.Text);
                string[] nodeNames = { "id", "user_type", "user_id", "campaign_id", "agency", "advertiser", "tape_format", "title", "product_desc", "isci_code", "pmt_media_id", "media_length", "standard", "label_number", "camp_media_id", "creation_date", "notes" };
                XmlElement xelRoot = doc.DocumentElement;
                XmlNodeList xmlNodes = xelRoot.SelectNodes("/pmt_db01/label_stations");
                List<AdvertiserInfo> ads = aCont.getAdvertisers(PortalId);
                List<AgencyInfo> ags = aCont.getAgencies(PortalId);
                foreach (XmlNode node in xmlNodes)
                {
                    c++;
                    LabelInfo ad = new LabelInfo();
                    ad.PortalId = PortalId;
                    foreach (string nodeName in nodeNames)
                    {
                        if (node[nodeName] != null)
                        {
                            switch (nodeName)
                            {
                                case "id":
                                    ad.Id = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "user_type":
                                    ad.UserType = node[nodeName].InnerText;
                                    break;
                                case "user_id":
                                    ad.UserId = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "campaign_id":
                                    ad.CampaignId = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "agency":
                                    var thisag = ags.FirstOrDefault(o => o.AgencyName.ToLower() == node[nodeName].InnerText.ToLower());
                                    if (thisag != null)
                                    {
                                        ad.AgencyName = thisag.AgencyName;
                                        ad.AgencyId = thisag.Id;
                                    }
                                    break;
                                case "advertiser":
                                    var thisad = ads.FirstOrDefault(o => o.AdvertiserName.ToLower() == node[nodeName].InnerText.ToLower());
                                    if (thisad != null)
                                    {
                                        ad.AdvertiserName = thisad.AdvertiserName;
                                        ad.AdvertiserId = thisad.Id;
                                    }
                                    break;
                                case "tape_format":
                                    ad.TapeFormat = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "title":
                                    ad.Title = node[nodeName].InnerText;
                                    break;
                                case "product_desc":
                                    ad.Description = node[nodeName].InnerText;
                                    break;
                                case "isci_code":
                                    ad.ISCICode = node[nodeName].InnerText;
                                    break;
                                case "pmt_media_id":
                                    ad.PMTMediaId = node[nodeName].InnerText;
                                    break;
                                case "media_length":
                                    ad.MediaLength = aCont.fixLength(node[nodeName].InnerText);
                                    break;
                                case "standard":
                                    ad.Standard = node[nodeName].InnerText;
                                    break;
                                case "label_number":
                                    ad.LabelNumber = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "camp_media_id":
                                    ad.CampaignMediaId = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "creation_date":
                                    ad.DateCreated = Convert.ToDateTime(node[nodeName].InnerText);
                                    break;
                                case "notes":
                                    ad.Notes = node[nodeName].InnerText;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    ad.CreatedById = 1;
                    ad.DateCreated = DateTime.Now;
                    ad.LastModifiedById = 1;
                    ad.LastModifiedDate = DateTime.Now;
                    aCont.Add_LabelForImport(ad);
                }
                lblMessage.Text = c.ToString() + " records imported.";
            }
        }

        protected void btnLibraryItems_Click(object sender, EventArgs e)
        {
            int c = 0;
            AdminController aCont = new AdminController();
            if (txtScript.Text.IndexOf("<!-- Table medias -->") == -1)
            {
                lblMessage.Text = "This appears to be the wrong file.";
            }
            else
            {
                //aCont.ClearLibraryItems();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(txtScript.Text);
                string[] nodeNames = { "id", "advertiser_id", "cust_id", "isci_code", "file_name", "title", "product_desc", "media_length", "media_type", "encode", "standard", "customer_ref", "spin_media_id", "created_date", "media_index", "reel", "tape_code", "position", "valut_id", "location", "status", "comment", "close_cap" };
                XmlElement xelRoot = doc.DocumentElement;
                XmlNodeList xmlNodes = xelRoot.SelectNodes("/pmt_db01/medias");
                List<AdvertiserInfo> ads = aCont.getAdvertisers(PortalId);
                List<AgencyInfo> ags = aCont.getAgencies(PortalId);
                List<MasterItemInfo> masters = aCont.getMasters(PortalId);
                foreach (XmlNode node in xmlNodes)
                {
                    c++;
                    LibraryItemInfo ad = new LibraryItemInfo();
                    ad.PortalId = PortalId;
                    foreach (string nodeName in nodeNames)
                    {
                        if (node[nodeName] != null)
                        {
                            switch (nodeName)
                            {
                                case "id":
                                    ad.Id = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "advertiser_id":
                                    var thisad = ads.FirstOrDefault(o => o.Id.ToString() == node[nodeName].InnerText);
                                    if (thisad != null)
                                    {
                                        ad.AdvertiserId = thisad.Id;
                                        ad.Advertiser = thisad.AdvertiserName;
                                    }
                                    break;
                                case "cust_id":
                                    var thisag = ags.FirstOrDefault(o => o.Id.ToString() == node[nodeName].InnerText);
                                    if (thisag != null)
                                    {
                                        ad.AgencyId = thisag.Id;
                                        ad.Agency = thisag.AgencyName;
                                    }
                                    break;
                                case "isci_code":
                                    ad.ISCICode = node[nodeName].InnerText;
                                    break;
                                case "file_name":
                                    ad.Filename = node[nodeName].InnerText;
                                    break;
                                case "title":
                                    ad.Title = node[nodeName].InnerText;
                                    break;
                                case "product_desc":
                                    ad.ProductDescription = node[nodeName].InnerText;
                                    break;
                                case "media_length":
                                    ad.MediaLength = aCont.fixLength(node[nodeName].InnerText);
                                    break;
                                case "media_type":
                                    ad.MediaType = node[nodeName].InnerText;
                                    break;
                                case "encode":
                                    ad.Encode = node[nodeName].InnerText;
                                    break;
                                case "pmt_media_id":
                                    ad.PMTMediaId = node[nodeName].InnerText;
                                    break;
                                case "standard":
                                    ad.Standard = node[nodeName].InnerText;
                                    break;
                                case "customer_ref":
                                    ad.CustomerReference = node[nodeName].InnerText;
                                    break;
                                case "spin_media_id":
                                    var thisMaster = masters.FirstOrDefault(o => o.PMTMediaId == node[nodeName].InnerText);
                                    ad.PMTMediaId = node[nodeName].InnerText;
                                    if(thisMaster!=null)
                                    {
                                        ad.MasterId = thisMaster.Id;
                                    }
                                    break;
                                case "created_date":
                                    string[] datePcs = node[nodeName].InnerText.Split('-');
                                    try
                                    {
                                        ad.DateCreated = new DateTime(Convert.ToInt16(datePcs[0]),Convert.ToInt16(datePcs[1]),Convert.ToInt16(datePcs[2]));
                                    }
                                    catch { }
                                    break;
                                case "media_index":
                                    ad.MediaIndex = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "reel":
                                    ad.Reel =Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "tape_code":
                                    ad.TapeCode = node[nodeName].InnerText;
                                    break;
                                case "position":
                                    ad.Position = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "valut_id":
                                    ad.VaultId = node[nodeName].InnerText;
                                    break;
                                case "location":
                                    ad.Location = node[nodeName].InnerText;
                                    break;
                                case "status":
                                    ad.Status = node[nodeName].InnerText;
                                    break;
                                case "comment":
                                    ad.Comment = node[nodeName].InnerText;
                                    break;
                                case "close_cap":
                                    ad.ClosedCaptioned = node[nodeName].InnerText;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    ad.CreatedById = 1;
                    ad.LastModifiedById = 1;
                    ad.LastModifiedDate = DateTime.Now;
                    aCont.Add_LibraryItemForImport(ad);
                }
                lblMessage.Text = c.ToString() + " records imported.";
                Application["LibraryItems"] = null;
            }
        }

        protected void btnMasterItems_Click(object sender, EventArgs e)
        {
            int c = 0;
            AdminController aCont = new AdminController();
            if (txtScript.Text.IndexOf("<!-- Table master_items -->") == -1)
            {
                lblMessage.Text = "This appears to be the wrong file.";
            }
            else
            {
                //aCont.ClearMasterItems();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(txtScript.Text);
                string[] nodeNames = { "id", "filename", "advertiser_id", "title", "media_type", "encode", "standard", "length", "customer_id", "pmt_media_id", "reel", "tape_code", "position", "valut_id", "location", "comment", "close_cap" };
                XmlElement xelRoot = doc.DocumentElement;
                XmlNodeList xmlNodes = xelRoot.SelectNodes("/pmt_db01/master_items");
                List<AdvertiserInfo> ads = aCont.getAdvertisers(PortalId);
                List<AgencyInfo> ags = aCont.getAgencies(PortalId);
                List<MasterItemInfo> masters = aCont.getMasters(PortalId);
                foreach (XmlNode node in xmlNodes)
                {
                    c++;
                    MasterItemInfo ad = new MasterItemInfo();
                    ad.PortalId = PortalId;
                    foreach (string nodeName in nodeNames)
                    {
                        if (node[nodeName] != null)
                        {
                            switch (nodeName)
                            {
                                case "id":
                                    ad.Id = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "advertiser_id":
                                    var thisad = ads.FirstOrDefault(o => o.Id.ToString() == node[nodeName].InnerText);
                                    if (thisad != null)
                                    {
                                        ad.AdvertiserId = thisad.Id;
                                        ad.Advertiser = thisad.AdvertiserName;
                                    }
                                    break;
                                case "filename":
                                    ad.Filename = node[nodeName].InnerText;
                                    break;
                                case "title":
                                    ad.Title = node[nodeName].InnerText;
                                    break;
                                case "length":
                                    ad.Length = aCont.fixLength(node[nodeName].InnerText);
                                    break;
                                case "media_type":
                                    if (node[nodeName].InnerText.ToLower() == "hd")
                                        ad.MediaType = 2;
                                    else if (node[nodeName].InnerText.ToLower() == "hd & sd")
                                        ad.MediaType = 3;
                                    else if (node[nodeName].InnerText.ToLower() == "sd")
                                        ad.MediaType = 5;
                                    break;
                                case "encode":
                                    if (node[nodeName].InnerText.ToLower() == "spottrac")
                                        ad.Encode = 1;
                                    else if (node[nodeName].InnerText.ToLower() == "teletrax")
                                        ad.Encode = 2;
                                    else if (node[nodeName].InnerText.ToLower() == "veil")
                                        ad.Encode = 2;
                                    break;
                                case "pmt_media_id":
                                    ad.PMTMediaId = node[nodeName].InnerText;
                                    break;
                                case "standard":
                                    ad.Standard = node[nodeName].InnerText;
                                    break;
                                case "customer_id":
                                    ad.CustomerId = node[nodeName].InnerText;
                                    break;
                                case "reel":
                                    ad.Reel = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "tape_code":
                                    ad.TapeCode = node[nodeName].InnerText;
                                    break;
                                case "position":
                                    ad.Position = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "valut_id":
                                    ad.VaultId = node[nodeName].InnerText;
                                    break;
                                case "location":
                                    ad.Location = node[nodeName].InnerText;
                                    break;
                                case "comment":
                                    ad.Comment = node[nodeName].InnerText;
                                    break;
                                case "close_cap":
                                    if(node[nodeName].InnerText.ToLower() == "yes")
                                    ad.ClosedCaptioned = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    ad.CreatedById = 1;
                    ad.DateCreated = DateTime.Now;
                    ad.LastModifiedById = 1;
                    ad.LastModifiedDate = DateTime.Now;
                    aCont.Add_MasterItemForImport(ad);
                }
                lblMessage.Text = c.ToString() + " records imported.";
                Application["MasterItems"] = null;
            }
        }

        protected void btnMasterItemAgencies_Click(object sender, EventArgs e)
        {
            int c = 0;
            AdminController aCont = new AdminController();
            if (txtScript.Text.IndexOf("<!-- Table master_item_agencies -->") == -1)
            {
                lblMessage.Text = "This appears to be the wrong file.";
            }
            else
            {
                aCont.ClearMasterItemAgencies();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(txtScript.Text);
                string[] nodeNames = { "master_item_id", "agency_id" };
                XmlElement xelRoot = doc.DocumentElement;
                XmlNodeList xmlNodes = xelRoot.SelectNodes("/pmt_db01/master_item_agencies");
                foreach (XmlNode node in xmlNodes)
                {
                    c++;
                    int adId = -1;
                    int agId = -1;
                    foreach (string nodeName in nodeNames)
                    {
                        if (node[nodeName] != null)
                        {
                            switch (nodeName)
                            {
                                case "master_item_id":
                                    adId = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "agency_id":
                                    agId = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    aCont.Add_MasterItemAgency(adId, agId);
                }
                lblMessage.Text = c.ToString() + " records imported.";
                Application["MasterItems"] = null;
            }
        }

        protected void btnStations_Click(object sender, EventArgs e)
        {
            int c = 0;
            AdminController aCont = new AdminController();
            if (txtScript.Text.IndexOf("<!-- Table tv_stations -->") == -1)
            {
                lblMessage.Text = "This appears to be the wrong file.";
            }
            else
            {
                aCont.ClearStations();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(txtScript.Text);
                string[] stationNodeNames = { "id", "market_id", "call_letter", "media_type", "station_name", "station_contact", "address1", "address2", "city", "state", "zip_code", "country", "phone_no", "fax", "email_address", "special_instruction", "online", "status", "attention_line" };
                string[] tapeFormatNodeNames = { "station_id", "tape_format" };
                string[] deliveryMethodNodeNames = { "station_id", "delivery_method" };
                string[] javelineNodeNames = { "call_letter", "station_id" }; 
                XmlElement xelRoot = doc.DocumentElement;
                XmlNodeList xmlNodes = xelRoot.SelectNodes("/pmt_db01/tv_stations");
                XmlNodeList tapeFormatNodes = xelRoot.SelectNodes("/pmt_db01/station_formats");
                XmlNodeList deliveryMethodNodes = xelRoot.SelectNodes("/pmt_db01/delivery_methods");
                XmlNodeList javelinNodes = xelRoot.SelectNodes("/pmt_db01/call_letters");
                List<DeliveryMethodInfo> dms = aCont.Get_DeliveryMethodsByPortalId(PortalId);
                List<TapeFormatInfo> tfs = aCont.Get_TapeFormatsByPortalId(PortalId);
                foreach (XmlNode node in xmlNodes)
                {
                    StationInfo station = new StationInfo();
                    c++;
                    foreach (string nodeName in stationNodeNames)
                    {
                        if (node[nodeName] != null)
                        {
                            switch (nodeName)
                            {
                                case "id":
                                    station.Id = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "market_id":
                                    station.MarketId = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "call_letter":
                                    station.CallLetter = node[nodeName].InnerText;
                                    break;
                                case "media_type":
                                    if (node[nodeName].InnerText.ToLower() == "hd")
                                        station.MediaType = 2;
                                    else if (node[nodeName].InnerText.ToLower().IndexOf("addelivery") != -1)
                                        station.MediaType = 2;
                                    else if (node[nodeName].InnerText.ToLower() == "hd & sd" || node[nodeName].InnerText.ToLower() == "hd &amp; sd")
                                        station.MediaType = 3;
                                    else if (node[nodeName].InnerText.ToLower() == "hd & sd (backup required)" || node[nodeName].InnerText.ToLower() == "hd &amp; sd (backup required)")
                                    {
                                        station.MediaType = 3;
                                        station.backupRequired = true;
                                    }
                                    else if (node[nodeName].InnerText.ToLower() == "sd")
                                        station.MediaType = 5;
                                    break;
                                case "station_name":
                                    station.StationName = node[nodeName].InnerText;
                                    break;
                                case "station_contact":
                                    station.StationContact = node[nodeName].InnerText;
                                    break;
                                case "address1":
                                    station.Address1 = node[nodeName].InnerText;
                                    break;
                                case "address2":
                                    station.Address2 = node[nodeName].InnerText;
                                    break;
                                case "city":
                                    station.City = node[nodeName].InnerText;
                                    break;
                                case "state":
                                    station.State = node[nodeName].InnerText;
                                    break;
                                case "zip_code":
                                    station.Zip = node[nodeName].InnerText;
                                    break;
                                case "country":
                                    station.Country = node[nodeName].InnerText;
                                    break;
                                case "phone_no":
                                    station.Phone = node[nodeName].InnerText;
                                    break;
                                case "fax":
                                    station.Fax = node[nodeName].InnerText;
                                    break;
                                case "email_address":
                                    station.Email = node[nodeName].InnerText;
                                    break;
                                case "special_instruction":
                                    station.SpecialInstructions = node[nodeName].InnerText;
                                    break;
                                case "online":
                                    if(node[nodeName].InnerText.ToLower() == "yes")
                                        station.Online = true;
                                    break;
                                case "status":
                                    if(node[nodeName].InnerText.ToLower() == "active")
                                        station.Status = true;
                                    break;
                                case "attention_line":
                                    station.AttentionLine = node[nodeName].InnerText;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    //compute delivery methods, tapeformats and javelin call letters
                    foreach (XmlNode tfnode in tapeFormatNodes)
                    {
                        if(tfnode[tapeFormatNodeNames[0]] !=null && tfnode[tapeFormatNodeNames[0]].InnerText==station.Id.ToString())
                        {
                            var tf = tfs.FirstOrDefault(o => o.TapeFormat.ToLower() == tfnode[tapeFormatNodeNames[1]].InnerText.ToLower());
                            if(tf!=null)
                                station.TapeFormat += tf.Id.ToString() + ",";
                        }
                    }
                    if (station.TapeFormat.IndexOf(",") != -1)
                    {
                        station.TapeFormat = station.TapeFormat.Substring(0, station.TapeFormat.Length - 1);
                    }
                    foreach (XmlNode dmnode in deliveryMethodNodes)
                    {
                        if (dmnode[deliveryMethodNodeNames[0]] != null && dmnode[deliveryMethodNodeNames[0]].InnerText == station.Id.ToString())
                        {
                            var dm = dms.FirstOrDefault(o => o.DeliveryMethod.ToLower() == dmnode[deliveryMethodNodeNames[1]].InnerText.ToLower());
                            if(dm!=null)
                                station.DeliveryMethods += dm.Id.ToString() + ",";
                        }
                    }
                    foreach (XmlNode tfnode in javelinNodes)
                    {
                        if (tfnode[javelineNodeNames[1]] != null && tfnode[javelineNodeNames[1]].InnerText == station.Id.ToString())
                        {
                            station.JavelinCallLetters = tfnode[javelineNodeNames[0]].InnerText;
                            break;
                        }
                    }
                    station.PortalId = PortalId;
                    station.CreatedById = 1;
                    station.DateCreated = DateTime.Now;
                    station.LastModifiedById = 1;
                    station.LastModifiedDate = DateTime.Now;
                    aCont.Add_StationForImport(station);
                }
                lblMessage.Text = c.ToString() + " records imported.";
            }
        }

        protected void btnTasks_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            if (txtScript.Text.IndexOf("<!-- Table campaigns -->") == -1)
            {
                lblMessage.Text = "This appears to be the wrong file.";
            }
            else
            {
                aCont.ClearMasterItemAgencies();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(txtScript.Text);
                string[] nodeNames = { "id", "cust_id", "advertiser_id", "user_id", "user_type", "po_number", "group_type", "campaign_ref", "campaign_name", "product_name", "video_delivery", "media_delivery", "traffic_delivery", "reference", "estimate", "special_instruction", "created_date" };
                string[] assocNames = { "campaign_id", "group_id", "media_id" };
                string[] stationNames = { "id", "campaign_id", "group_id", "station_id" };
                string[] statusNames = { "id", "cust_id", "campaign_id", "station_group_id", "tracking_id", "service_provide", "date_signed", "time_signed", "signed_by" };
                string[] mediaNames = { "id", "campaign_id", "group_id", "media_id", "delivery_method", "quantity", "network_type", "packing_slip_id", "billing_code", "order_type", "production_lable", "delivery_type" };
                XmlElement xelRoot = doc.DocumentElement;
                XmlNodeList xmlNodes = xelRoot.SelectNodes("/pmt_db01/campaigns");
                XmlNodeList AssocXmlNodes = xelRoot.SelectNodes("/pmt_db01/camp_stat_associations");
                XmlNodeList StationXmlNodes = xelRoot.SelectNodes("/pmt_db01/campaign_stations");
                XmlNodeList StatusXmlNodes = xelRoot.SelectNodes("/pmt_db01/order_statuses");
                XmlNodeList MediaXmlNodes = xelRoot.SelectNodes("/pmt_db01/campaign_media_items");
                List<AdvertiserInfo> ads = aCont.getAdvertisers(PortalId);
                List<AgencyInfo> ags = aCont.getAgencies(PortalId);
                List<WorkOrderInfo> wos = new List<WorkOrderInfo>();
                List<QBCodeInfo> qbCodes = aCont.Get_QBCodesByPortalId(PortalId);
                List<statAssoc> mediaStats = new List<statAssoc>();
                foreach (XmlNode MediaXmlNode in MediaXmlNodes)
                {
                    statAssoc stat = new statAssoc();
                    foreach (string mediaName in mediaNames)
                    {
                        if (MediaXmlNode[mediaName] != null)
                        {
                            switch (mediaName)
                            {
                                case "id":
                                    stat.Id = Convert.ToInt32(MediaXmlNode[mediaName].InnerText);
                                    break;
                                case "campaign_id":
                                    stat.campaignId = Convert.ToInt32(MediaXmlNode[mediaName].InnerText);
                                    break;
                                case "group_id":
                                    stat.groupId = Convert.ToInt32(MediaXmlNode[mediaName].InnerText);
                                    break;
                                case "media_id":
                                    stat.mediaId = Convert.ToInt32(MediaXmlNode[mediaName].InnerText);
                                    break;
                                case "delivery_method":
                                    stat.DeliveryMethod = MediaXmlNode[mediaName].InnerText;
                                    break;
                                case "billing_code":
                                    stat.QBCode = MediaXmlNode[mediaName].InnerText;
                                    break;
                                case "delivery_type":
                                    stat.DeliveryType = MediaXmlNode[mediaName].InnerText;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    mediaStats.Add(stat);
                }
                foreach (XmlNode node in xmlNodes)
                {
                    WorkOrderInfo wo = new WorkOrderInfo();
                    foreach (string nodeName in nodeNames)
                    {
                        if (node[nodeName] != null)
                        {
                            switch (nodeName)
                            {
                                case "id":
                                    wo.Id = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "cust_id":
                                    wo.AgencyId = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "advertiser_id":
                                    wo.AdvertiserId = Convert.ToInt32(node[nodeName].InnerText);
                                    wo.BillToId = Convert.ToInt32(node[nodeName].InnerText);
                                    break;
                                case "created_date":
                                    wo.DateCreated = Convert.ToDateTime(node[nodeName].InnerText);
                                    break;
                                case "campaign_name":
                                    wo.Description = node[nodeName].InnerText;
                                    break;
                                case "po_number":
                                    wo.PONumber = node[nodeName].InnerText;
                                    break;
                                case "special_instruction":
                                    wo.Notes = node[nodeName].InnerText;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    wos.Add(wo);
                }
                List<statAssoc> stats = new List<statAssoc>();
                foreach(XmlNode assocNode in AssocXmlNodes)
                {
                    statAssoc stat = new statAssoc();
                    foreach (string assocNodeName in assocNames)
                    {
                        if (assocNode[assocNodeName] != null)
                        {
                            switch (assocNodeName)
                            {
                                case "campaign_id":
                                    stat.campaignId = Convert.ToInt32(assocNode[assocNodeName].InnerText);
                                    break;
                                case "group_id":
                                    stat.groupId = Convert.ToInt32(assocNode[assocNodeName].InnerText);
                                    break;
                                case "media_id":
                                    stat.mediaId = Convert.ToInt32(assocNode[assocNodeName].InnerText);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    stats.Add(stat);
                }
                List<statAssoc> stations = new List<statAssoc>();
                foreach (XmlNode StationXmlNode in StationXmlNodes)
                {
                    statAssoc stat = new statAssoc();
                    foreach (string stationName in stationNames)
                    {
                        if (StationXmlNode[stationName] != null)
                        {
                            switch (stationName)
                            {
                                case "id":
                                    stat.Id = Convert.ToInt32(StationXmlNode[stationName].InnerText);
                                    break;
                                case "campaign_id":
                                    stat.campaignId = Convert.ToInt32(StationXmlNode[stationName].InnerText);
                                    break;
                                case "group_id":
                                    stat.groupId = Convert.ToInt32(StationXmlNode[stationName].InnerText);
                                    break;
                                case "station_id":
                                    stat.mediaId = Convert.ToInt32(StationXmlNode[stationName].InnerText);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    stations.Add(stat);
                }
                List<statAssoc> statuses = new List<statAssoc>();
                foreach (XmlNode StatusXmlNode in StatusXmlNodes)
                {
                    statAssoc stat = new statAssoc();
                    foreach (string statusName in statusNames)
                    {
                        if (StatusXmlNode[statusName] != null)
                        {
                            switch (statusName)
                            {
                                case "campaign_id":
                                    stat.campaignId = Convert.ToInt32(StatusXmlNode[statusName].InnerText);
                                    break;
                                case "station_group_id":
                                    stat.groupId = Convert.ToInt32(StatusXmlNode[statusName].InnerText);
                                    break;
                                case "cust_id":
                                    stat.mediaId = Convert.ToInt32(StatusXmlNode[statusName].InnerText);
                                    break;
                                case "tracking_id":
                                    stat.trackingId = StatusXmlNode[statusName].InnerText;
                                    break;
                                case "service_provide":
                                    stat.service = StatusXmlNode[statusName].InnerText;
                                    break;
                                case "date_signed":
                                    stat.dateSigned = StatusXmlNode[statusName].InnerText;
                                    break;
                                case "time_signed":
                                    stat.timeSigned = StatusXmlNode[statusName].InnerText;
                                    break;
                                case "signed_by":
                                    stat.signedBy = StatusXmlNode[statusName].InnerText;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    statuses.Add(stat);
                }
                foreach(WorkOrderInfo wo in wos)
                {
                    List<statAssoc> thesestats = stats.Where(o => o.campaignId == wo.Id).ToList();
                    foreach(statAssoc thisstat in thesestats)
                    {
                        bool groupThere = false;
                        foreach(WOGroupInfo group in wo.Groups)
                        {
                            if(group.Id == thisstat.groupId)
                            {
                                groupThere = true;
                                LibraryItemInfo lib = new LibraryItemInfo();
                                lib.Id = thisstat.mediaId;
                                group.LibraryItems.Add(lib);
                            }
                        }
                        if(!groupThere)
                        {
                            WOGroupInfo group = new WOGroupInfo();
                            group.Id = thisstat.groupId;
                            LibraryItemInfo lib = new LibraryItemInfo();
                            lib.Id = thisstat.mediaId;
                            group.LibraryItems.Add(lib);
                            wo.Groups.Add(group);
                        }
                    }
                    foreach (WOGroupInfo group in wo.Groups)
                    {
                        List<statAssoc> theseStations = stations.Where(o => o.groupId == group.Id).ToList();
                        foreach(statAssoc thisStation in theseStations)
                        {
                            WOGroupStationInfo station = new WOGroupStationInfo();
                            station.StationId = thisStation.mediaId;
                            station.Id = thisStation.Id;
                            group.WOGroupStations.Add(station);
                        }
                    }
                } 
                //now, save tasks and wogroupstations
                foreach(WorkOrderInfo wo in wos)
                {
                    //check if this wo has only customize tasks
                    bool addThis = true;
                    for(int i=0; i<wo.Groups.Count(); i++)
                    {
                        if (wo.Groups[i].WOGroupStations.Count() == 0 || wo.Groups[i].LibraryItems.Count() == 0)
                        {
                            wo.Groups.Remove(wo.Groups[i]);
                        }
                    }
                    if(wo.Groups.Count==0)
                    {
                        addThis = false;
                    }
                    if (addThis)
                    {
                        int origWOId = wo.Id;
                        wo.PortalId = PortalId;
                        wo.CreatedById = UserId;
                        wo.LastModifiedById = UserId;
                        wo.Id = aCont.Add_WorkOrder(wo);
                        int x = 1;
                        foreach (WOGroupInfo group in wo.Groups)
                        {
                            bool isComplete = true;
                            int origGroupId = group.Id;
                            group.WorkOrderId = wo.Id;
                            group.PortalId = PortalId;
                            group.CreatedById = UserId;
                            group.LastModifiedById = UserId;
                            group.GroupName = "Group " + x.ToString();
                            x++;
                            group.GroupType = GroupTypeEnum.Delivery;
                            group.Id = aCont.Add_WorkOrderGroup(group);
                            foreach (WOGroupStationInfo station in group.WOGroupStations)
                            {
                                statAssoc thisStat = statuses.FirstOrDefault(o => o.groupId == station.Id);
                                if (thisStat != null)
                                {
                                    station.DeliveryMethod = thisStat.trackingId;
                                }
                                station.WOGroupId = group.Id;
                                station.CreatedById = UserId;
                                station.LastModifiedById = UserId;
                                station.WorkOrderId = wo.Id;
                                station.PortalId = PortalId;
                                station.Id = aCont.Add_WorkOrderGroupStation(station);
                                foreach (LibraryItemInfo lib in group.LibraryItems)
                                {
                                    statAssoc thisMedia = mediaStats.FirstOrDefault(o => o.mediaId == lib.Id && o.groupId == origGroupId);
                                    try
                                    {
                                        aCont.AddWOGroupLibItem(group.Id, lib.Id);
                                    }
                                    catch { }
                                    TaskInfo task = new TaskInfo();
                                    task.StationId = station.Id;
                                    task.PortalId = PortalId;
                                    if (thisStat != null)
                                    {
                                        task.DeliveryMethod = thisStat.trackingId;
                                        task.DeliveryMethodResponse = thisStat.signedBy;
                                        task.DeliveryStatus = thisStat.signedBy;
                                        if (thisStat.signedBy != "")
                                        {
                                            task.DeliveryStatus = "COMPLETE";
                                            task.isComplete = true;
                                        }
                                    }
                                    if (thisMedia != null)
                                    {
                                        task.DeliveryMethod = thisMedia.DeliveryMethod;
                                        task.QBCode = thisMedia.QBCode;
                                        var thisCode = qbCodes.FirstOrDefault(o => o.QBCode.ToLower() == thisMedia.QBCode.ToLower());
                                        if (thisCode != null)
                                        {
                                            task.QBCodeId = thisCode.Id;
                                        }
                                        if (thisMedia.DeliveryType.ToLower() == "bundle")
                                        {
                                            task.TaskType = GroupTypeEnum.Bundle;
                                        }
                                    }
                                    task.Description = "Imported";
                                    task.WorkOrderId = wo.Id;
                                    task.WOGroupId = group.Id;
                                    task.TaskType = GroupTypeEnum.Delivery;
                                    task.LibraryId = lib.Id;
                                    task.CreatedById = UserId;
                                    task.LastModifiedById = UserId;
                                    try
                                    {
                                        task.DeliveryOrderDateComplete = Convert.ToDateTime(thisStat.dateSigned);
                                    }
                                    catch { }
                                    try
                                    {
                                        bool isPM = false;
                                        if (thisStat.timeSigned.ToLower().IndexOf("pm") != -1)
                                        {
                                            isPM = true;
                                        }
                                        string timepart = thisStat.timeSigned.ToLower().Replace(" am", "").Replace(" pm", "");
                                        string[] timepieces = timepart.Split(':');
                                        int hours = 0;
                                        try { hours = Convert.ToInt16(timepieces[0]); }
                                        catch { }
                                        if (isPM)
                                            hours += 12;
                                        int minutes = 0;
                                        try { minutes = Convert.ToInt16(timepieces[1]); }
                                        catch { }
                                        task.DeliveryOrderDateComplete = task.DeliveryOrderDateComplete.AddHours(hours);
                                        task.DeliveryOrderDateComplete = task.DeliveryOrderDateComplete.AddMinutes(minutes);
                                    }
                                    catch { }
                                    task.DateCreated = wo.DateCreated;

                                    task.Id = aCont.Add_Task(task);
                                    if (!task.isComplete)
                                    {
                                        isComplete = false;
                                    }
                                }
                            }
                            if (isComplete)
                            {
                                wo.Status = "COMPLETE";
                                aCont.Update_WorkOrder(wo);
                            }
                        }
                    }     
                }
                lblMessage.Text = wos.Count.ToString() + " records imported.";
            }
        }

        protected void btnStationsLongform_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            int c = 0;
            string[] lines = txtScript.Text.Split('\n');
            foreach(string line in lines)
            {
                string[] vals = line.Split('|');
                StationInfo station = new StationInfo();
                station.PortalId = PortalId;
                station.CreatedById = UserId;
                station.Address1 = vals[3];
                station.Address2 = vals[4];
                station.AttentionLine = vals[5];
                station.CallLetter = vals[2];
                station.StationName = vals[2];
                station.City = vals[6];
                station.Phone = (vals[7] + " " + vals[8]).Trim();
                station.State = vals[9];
                station.Zip = vals[10];
                station.ProgramFormat = "Long Form";
                station.Email = vals[1];
                station.Status = true;
                station.Online = false;
                aCont.Add_Station(station);
                c++;
            }
            lblMessage.Text = c.ToString() + " Stations Imported.";
        }

        protected void btnMakeCustomizeCodes_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            List<QBCodeInfo> codes = aCont.Get_QBCodesByPortalId(PortalId);
            int c = 0;
            foreach(QBCodeInfo code in codes)
            {
                if(code.Type == GroupTypeEnum.Non_Deliverable)
                {
                    code.Type = GroupTypeEnum.Customized;
                    code.Id = -1;
                    code.QBCode = code.QBCode + " Customized";
                    code.Id = aCont.Add_QBCode(code);
                    foreach(ServiceInfo serv in code.Services)
                    {
                        aCont.AddQBCodeService(code.Id, serv.Id);
                    }
                    c++;
                }
            }
            lblMessage.Text = c.ToString() + " QB Codes created.";
        }

        protected void btnMakeBundleCodes_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            List<QBCodeInfo> codes = aCont.Get_QBCodesByPortalId(PortalId);
            int c = 0;
            foreach (QBCodeInfo code in codes)
            {
                if (code.Type == GroupTypeEnum.Non_Deliverable)
                {
                    code.Type = GroupTypeEnum.Bundle;
                    code.Id = -1;
                    code.QBCode = code.QBCode + " Bundle";
                    code.Id = aCont.Add_QBCode(code);
                    foreach (ServiceInfo serv in code.Services)
                    {
                        aCont.AddQBCodeService(code.Id, serv.Id);
                    }
                    c++;
                }
            }
            lblMessage.Text = c.ToString() + " QB Codes created.";
        }

        protected void btnLongFormStations_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            int c = 0;
            string[] lines = txtScript.Text.Split('\n');
            List<DeliveryMethodInfo> dels = aCont.Get_DeliveryMethodsByPortalId(PortalId);
            List<TapeFormatInfo> tapes = aCont.Get_TapeFormatsByPortalId(PortalId);
            foreach (string line in lines)
            {
                string[] fields = line.Split('|');
                StationInfo station = new StationInfo();
                station.PortalId = PortalId;
                station.CreatedById = UserId;
                station.LastModifiedById = UserId;
                station.CallLetter = fields[3];
                station.StationName = fields[5];
                station.StationContact = fields[6];
                station.Address1 = fields[7];
                station.Address2 = fields[8];
                station.City = fields[9];
                station.State = fields[10];
                station.Zip = fields[11];
                station.Country = fields[12];
                station.Phone = fields[13];
                station.Fax = fields[14];
                station.Email = fields[15];
                station.SpecialInstructions = fields[16];
                station.ProgramFormat = "Long Form";
                if(fields[26].Trim()!="")
                {
                    var del = dels.FirstOrDefault(o => o.DeliveryMethod.ToLower() == "ftp");
                    if (del != null)
                    {
                        station.DeliveryMethods += del.Id.ToString() + ",";
                    }
                }
                if (fields[27].Trim() != "")
                {
                    var del = dels.FirstOrDefault(o => o.DeliveryMethod.ToLower() == "pmt ftp");
                    if (del == null)
                    {
                        DeliveryMethodInfo dm = new DeliveryMethodInfo();
                        dm.DeliveryMethod = fields[27].Trim();
                        dm.PortalId = PortalId;
                        dm.Priority = "z";
                        dm.Id = aCont.Add_DeliveryMethod(dm);
                        del = dm;
                        dels.Add(dm);
                    }
                    station.DeliveryMethods += del.Id.ToString() + ",";
                }
                if (fields[28].Trim() != "")
                {
                    var del = dels.FirstOrDefault(o => o.DeliveryMethod.ToLower() == "pmt wave");
                    if (del == null)
                    {
                        DeliveryMethodInfo dm = new DeliveryMethodInfo();
                        dm.DeliveryMethod = fields[28].Trim();
                        dm.PortalId = PortalId;
                        dm.Priority = "z";
                        dm.Id = aCont.Add_DeliveryMethod(dm);
                        del = dm;
                        dels.Add(dm);
                    }
                    station.DeliveryMethods += del.Id.ToString() + ",";
                }
                if (fields[29].Trim() != "")
                {
                    var del = dels.FirstOrDefault(o => o.DeliveryMethod.ToLower() == fields[29].Trim().ToLower());
                    if (del == null)
                    {
                        DeliveryMethodInfo dm = new DeliveryMethodInfo();
                        dm.DeliveryMethod = fields[29].Trim();
                        dm.PortalId = PortalId;
                        dm.Priority = "z";
                        dm.Id = aCont.Add_DeliveryMethod(dm);
                        del = dm;
                        dels.Add(dm);
                    }
                    station.DeliveryMethods += del.Id.ToString() + ",";
                }
                if (fields[30].Trim() != "")
                {
                    var del = dels.FirstOrDefault(o => o.DeliveryMethod.ToLower() == fields[30].Trim().ToLower());
                    if (del == null)
                    {
                        DeliveryMethodInfo dm = new DeliveryMethodInfo();
                        dm.DeliveryMethod = fields[30].Trim();
                        dm.PortalId = PortalId;
                        dm.Priority = "z";
                        dm.Id = aCont.Add_DeliveryMethod(dm);
                        del = dm;
                        dels.Add(dm);
                    }
                    station.DeliveryMethods += del.Id.ToString() + ",";
                }
                string[] sds = fields[32].Trim().Split(',');
                foreach (string sd in sds)
                {
                    string tempsd = sd.Trim();
                    if (tempsd.ToLower() == "dvc pro" || tempsd.ToLower() == "dvc pro25")
                    {
                        tempsd = "DVCPRO25";
                    }
                    if (fields[31].Trim() != "" && tempsd.Trim() != "")
                    {
                        station.MediaType = 3;
                        var tapehd = tapes.FirstOrDefault(o => o.TapeFormat.ToLower() == fields[31].Trim().ToLower());
                        if(tapehd == null)
                        {
                            TapeFormatInfo tf = new TapeFormatInfo();
                            tf.Label = "A";
                            tf.PortalId = PortalId;
                            tf.Printer = "A";
                            tf.TapeFormat = fields[31].Trim();
                            tf.Id = aCont.Add_TapeFormat(tf);
                            tapehd = tf;
                            tapes.Add(tf);
                        }
                        //if (tapehd != null)
                        //{
                            if (station.TapeFormat.Length > 0)
                            { station.TapeFormat += ","; }
                            station.TapeFormat += tapehd.Id.ToString();
                        //}
                        var tapesd = tapes.FirstOrDefault(o => o.TapeFormat.ToLower() == tempsd.ToLower());
                        if (tapesd == null)
                        {
                            TapeFormatInfo tf = new TapeFormatInfo();
                            tf.Label = "A";
                            tf.PortalId = PortalId;
                            tf.Printer = "A";
                            tf.TapeFormat = fields[32].Trim();
                            tf.Id = aCont.Add_TapeFormat(tf);
                            tapesd = tf;
                            tapes.Add(tf);
                        }
                        //if (tapesd != null)
                        //{
                            if (station.TapeFormat.Length > 0)
                            { station.TapeFormat += ","; }
                            station.TapeFormat += tapesd.Id.ToString();
                        //}

                    }
                    else if (fields[31].Trim() != "")
                    {
                        station.MediaType = 2;
                        var tapehd = tapes.FirstOrDefault(o => o.TapeFormat.ToLower() == fields[31].Trim().ToLower());
                        if (tapehd == null)
                        {
                            TapeFormatInfo tf = new TapeFormatInfo();
                            tf.Label = "A";
                            tf.PortalId = PortalId;
                            tf.Printer = "A";
                            tf.TapeFormat = fields[31].Trim();
                            tf.Id = aCont.Add_TapeFormat(tf);
                            tapehd = tf;
                            tapes.Add(tf);
                        }
                        //if (tapehd != null)
                        //{
                            if (station.TapeFormat.Length > 0)
                            { station.TapeFormat += ","; }
                            station.TapeFormat += tapehd.Id.ToString();
                        //}
                    }
                    else if (tempsd.Trim() != "")
                    {
                        station.MediaType = 5;
                        var tapesd = tapes.FirstOrDefault(o => o.TapeFormat.ToLower() == tempsd.ToLower());
                        if (tapesd == null)
                        {
                            TapeFormatInfo tf = new TapeFormatInfo();
                            tf.Label = "A";
                            tf.PortalId = PortalId;
                            tf.Printer = "A";
                            tf.TapeFormat = tempsd.Trim();
                            tf.Id = aCont.Add_TapeFormat(tf);
                            tapesd = tf;
                            tapes.Add(tf);
                        }
                        //if (tapesd != null)
                        //{
                            if (station.TapeFormat.Length > 0)
                            { station.TapeFormat += ","; }
                            station.TapeFormat += tapesd.Id.ToString();
                        //}
                    }
                }
                station.Status = true;
                aCont.Add_Station(station);
                c++;
            }
            lblMessage.Text = c.ToString() + " Long Form Stations created.";
        }

        protected void btnLongFormCustomers_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            int c = 0;
            int d = 0;
            string[] lines = txtScript.Text.Split('\n');
            List<AdvertiserInfo> ads = aCont.getAdvertisers(PortalId);
            List<AgencyInfo> ags = aCont.getAgencies(PortalId);
            foreach (string line in lines)
            {                
                string[] fields = line.Split('|');
                if (fields[0].Trim() != "")
                {
                    try
                    {
                        if (fields[3] == "1")
                        {
                            //advertiser
                            //first, see if they already exist
                            var ad = ads.FirstOrDefault(o => o.AdvertiserName.ToLower() == cleanField(fields[0]).ToLower());
                            if (ad == null)
                            {
                                //add new advertiser
                                AdvertiserInfo newAd = new AdvertiserInfo();
                                newAd.AdvertiserName = cleanField(fields[0]);
                                newAd.LastModifiedById = Convert.ToInt32(fields[1].Trim());
                                newAd.Phone = (cleanField(fields[4]) + " " + cleanField(fields[5])).Trim();
                                newAd.Fax = cleanField(fields[6]);
                                newAd.Address1 = cleanField(fields[8]);
                                newAd.Address2 = cleanField(fields[9]);
                                newAd.City = cleanField(fields[10]);
                                newAd.State = cleanField(fields[11]);
                                newAd.Zip = cleanField(fields[12]);
                                newAd.ClientType = 3;
                                newAd.PortalId = PortalId;
                                if (cleanField(fields[13]).ToLower().IndexOf("fedx") != -1)
                                {
                                    newAd.Carrier = 1;
                                }
                                else if (cleanField(fields[13]).ToLower().IndexOf("airb") != -1 || cleanField(fields[13]).ToLower().IndexOf("ups") != -1)
                                {
                                    newAd.Carrier = 2;
                                }
                                if (cleanField(fields[14]).ToLower() == "prepaid")
                                {
                                    newAd.Freight = 7;
                                }
                                else if (cleanField(fields[14]).ToLower() == "3rd-party")
                                {
                                    newAd.Freight = 6;
                                }
                                newAd.CarrierNumber = cleanField(fields[15]);
                                newAd.CustomerReference = (cleanField(fields[2]) + " ").Trim() + cleanField(fields[18]);
                                aCont.Add_Advertiser(newAd);
                                ads.Add(newAd);
                                c++;
                            }
                        }
                        else
                        {
                            //agency
                            //first, see if they already exist
                            var ag = ags.FirstOrDefault(o => o.AgencyName.ToLower() == cleanField(fields[0]).ToLower());
                            if (ag == null)
                            {
                                //add new agency
                                AgencyInfo newAg = new AgencyInfo();
                                newAg.AgencyName = cleanField(fields[0]);
                                newAg.LastModifiedById = Convert.ToInt32(cleanField(fields[1]));
                                newAg.AttentionLine = cleanField(fields[2]);
                                newAg.Phone = (cleanField(fields[4]) + " " + cleanField(fields[5])).Trim();
                                newAg.Fax = cleanField(fields[6]);
                                newAg.Address1 = cleanField(fields[8]);
                                newAg.Address2 = cleanField(fields[9]);
                                newAg.City = cleanField(fields[10]);
                                newAg.State = cleanField(fields[11]);
                                newAg.Zip = cleanField(fields[12]);
                                newAg.ClientType = 3;
                                newAg.CustomerReference = cleanField(fields[18]);
                                newAg.Status = true;
                                newAg.PortalId = PortalId;
                                aCont.Add_Agency(newAg);
                                ags.Add(newAg);
                                d++;
                            }
                        }
                    }
                    catch { }
                }
            }
            lblMessage.Text = c.ToString() + " Advertisers added. " + d.ToString() + " Agencies added.";
            Application["Advertisers"] = null;
            Application["Agencies"] = null;
        }
        private string cleanField(string field)
        {
            return field.Replace("\"", "").Trim();
        }

        protected void btnLongFormMasters_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            int c = 0;
            string[] lines = txtScript.Text.Split('\n');
            List<AdvertiserInfo> ads = aCont.getAdvertisers(PortalId);
            List<AgencyInfo> ags = aCont.getAgencies(PortalId);
            List<TapeFormatInfo> tapes = aCont.Get_TapeFormatsByPortalId(PortalId);
            DateTime startDate = DateTime.Now.AddYears(-3);
            foreach (string line in lines)
            {
                string[] fields = line.Split('|');
                try
                {
                    int secs = getDuration(cleanField(fields[14]));
                    if (secs >= 1710)
                    {
                        //28:30 or longer, now check creation date
                        if (cleanField(fields[6]) != "" && cleanField(fields[15]) != "")
                        {
                            string[] dateParts = cleanField(fields[6]).Split(' ');
                            if (dateParts.Length == 2)
                            {
                                string[] mdy = dateParts[0].Split('/');
                                int month = Convert.ToInt32(cleanField(mdy[0]));
                                int day = Convert.ToInt32(cleanField(mdy[1]));
                                int year = Convert.ToInt32(cleanField(mdy[2]));
                                DateTime created = new DateTime(year, month, day);
                                if (created >= startDate)
                                {
                                    //add this master
                                    MasterItemInfo newMaster = new MasterItemInfo();
                                    newMaster.PortalId = PortalId;
                                    newMaster.DateCreated = created;
                                    newMaster.Title = cleanField(fields[25]);
                                    newMaster.Comment = cleanField(fields[5]);
                                    var ad = ads.FirstOrDefault(o => o.AdvertiserName.ToLower() == cleanField(fields[7]).ToLower());
                                    if (ad != null)
                                    {
                                        newMaster.AdvertiserId = ad.Id;
                                    }
                                    newMaster.Comment += " " + cleanField(fields[8]);
                                    newMaster.Length = cleanField(fields[14]);
                                    newMaster.PMTMediaId = cleanField(fields[15]);
                                    newMaster.Comment += " " + cleanField(fields[20]);
                                    newMaster.Standard = cleanField(fields[22]);
                                    aCont.Add_MasterItem(newMaster);
                                    c++;
                                }
                            }

                        }
                    }
                }
                catch (Exception ex) { lblMessage.Text = ex.Message; }
            }
            lblMessage.Text = c.ToString() + " Masters Added.";
            Application["MasterItems"] = null;
        }
        private int getDuration(string length)
        {
            int secs = 0;
            try
            {
                if (length.IndexOf("/") == -1)
                {
                    string[] parts = length.Split(':');
                    if (parts[0] != "")
                    {
                        secs = 60 * Convert.ToInt32(parts[0]);
                    }
                    if (parts.Length > 1 && parts[1] != "")
                    {
                        secs += Convert.ToInt32(parts[1]);
                    }
                }
            }
            catch { }
            return secs;
        }

        protected void btnLongFormLibraryItems_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            int c = 0;
            string[] lines = txtScript.Text.Split('\n');
            List<AdvertiserInfo> ads = aCont.getAdvertisers(PortalId);
            List<AgencyInfo> ags = aCont.getAgencies(PortalId);
            List<TapeFormatInfo> tapes = aCont.Get_TapeFormatsByPortalId(PortalId);
            List<MasterItemInfo> masters = aCont.getMasters(PortalId);
            DateTime startDate = DateTime.Now.AddYears(-3);
            foreach (string line in lines)
            {
                string[] fields = line.Split('|');
                try
                {
                    int secs = getDuration(cleanField(fields[21]));
                    if (secs >= 1710)
                    {
                        //28:30 or longer, now check creation date
                        if (cleanField(fields[13]) != "" && cleanField(fields[22]) != "")
                        {
                            string[] dateParts = cleanField(fields[13]).Split(' ');
                            if (dateParts.Length == 2)
                            {
                                string[] mdy = dateParts[0].Split('/');
                                int month = Convert.ToInt32(cleanField(mdy[0]));
                                int day = Convert.ToInt32(cleanField(mdy[1]));
                                int year = Convert.ToInt32(cleanField(mdy[2]));
                                DateTime created = new DateTime(year, month, day);
                                if (created >= startDate)
                                {
                                    //add this LibraryItem
                                    LibraryItemInfo newLibraryItem = new LibraryItemInfo();
                                    newLibraryItem.PortalId = PortalId;
                                    newLibraryItem.DateCreated = created;
                                    newLibraryItem.Title = cleanField(fields[35]);
                                    newLibraryItem.ProductDescription = cleanField(fields[17]);
                                    newLibraryItem.ISCICode = cleanField(fields[20]);
                                    newLibraryItem.Comment = cleanField(fields[27]);
                                    var ad = ads.FirstOrDefault(o => o.AdvertiserName.ToLower() == cleanField(fields[15]).ToLower());
                                    if (ad != null)
                                    {
                                        newLibraryItem.AdvertiserId = ad.Id;
                                    }
                                    else
                                    {
                                        ad = ads.FirstOrDefault(o => o.AdvertiserName.ToLower() == cleanField(fields[3]).ToLower());
                                        if (ad != null)
                                        {
                                            newLibraryItem.AdvertiserId = ad.Id;
                                        }
                                    }
                                    var ag = ags.FirstOrDefault(o => o.AgencyName.ToLower() == cleanField(fields[15]).ToLower());
                                    if(ag != null)
                                    {
                                        newLibraryItem.AgencyId = ag.Id;
                                    }
                                    var master = masters.FirstOrDefault(o => o.PMTMediaId.ToLower() == cleanField(fields[22]).ToLower());
                                    if(master != null)
                                    {
                                        newLibraryItem.MasterId = master.Id;
                                    }
                                    newLibraryItem.TapeCode = cleanField(fields[34]);
                                    newLibraryItem.MediaLength = cleanField(fields[21]);
                                    newLibraryItem.PMTMediaId = cleanField(fields[22]);
                                    newLibraryItem.Standard = cleanField(fields[31]);
                                    aCont.Add_LibraryItem(newLibraryItem);
                                    c++;
                                }
                            }

                        }
                    }
                }
                catch (Exception ex) { lblMessage.Text = ex.Message; }
            }
            lblMessage.Text = c.ToString() + " LibraryItems Added.";
            Application["LibraryItems"] = null;
        }

        protected void btnUpdateDeliveryDates_Click(object sender, EventArgs e)
        {
            int c = 0;
            AdminController aCont = new AdminController();
            List<WorkOrderInfo> wos = aCont.Get_WorkOrdersByPortalId(PortalId);
            foreach(WorkOrderInfo wo in wos)
            {
                DateTime start = new DateTime(2016, 6, 28, 0, 0, 0);
                if(wo.DateCreated >= start)
                {
                    List<TaskInfo> tasks = aCont.Get_TasksByWOId(wo.Id);
                    foreach(TaskInfo task in tasks)
                    {
                        task.DeliveryOrderDateComplete = task.DateCreated;
                        aCont.Update_Task(task);
                        c++;
                    }
                }
            }
            lblMessage.Text = c.ToString() + " tasks updated.";
        }
    }
}