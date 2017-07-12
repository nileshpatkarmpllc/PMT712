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
    public partial class MasterItemCheckList : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["Master"] != null)
                {
                    try
                    {
                        int MasterId = Convert.ToInt32(Request.QueryString["Master"]);
                        AdminController aCont = new AdminController();
                        MasterItemInfo master = aCont.Get_MasterItemById(MasterId);
                        if (master.Id == -1)
                        {
                            pnlAuth.Visible = false;
                            pnlNotAuth.Visible = true;
                            lblError.Text = "Master not found.";
                        }
                        else
                        {
                            //check if user is authorized to view

                            //if authorized
                            loadForm(master);
                        }
                    }
                    catch
                    {
                        pnlAuth.Visible = false;
                        pnlNotAuth.Visible = true;
                        lblError.Text = "Master Id not in the correct format.";
                    }
                }
                else
                {
                    pnlAuth.Visible = false;
                    pnlNotAuth.Visible = true;
                    lblError.Text = "Invalid page request.";
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

        private void loadForm(MasterItemInfo master)
        {
            if(master.CheckListForm=="")
            {
                //never saved before
                txtDate.Text = "Date: " + DateTime.Now.ToShortDateString();
                txtPMTnumber.Text = "PMT #: " + master.PMTMediaId;
                txtVaultId.Text = "Vault Id: " + master.VaultId;
            }
            else
            {
                //load it up
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(master.CheckListForm);
                txtDate.Text = getXmlNodeText(ref doc, "date", "Date: ");
                txtVaultId.Text = getXmlNodeText(ref doc, "vaultid", "Vault Id: ");
                txtQc.Text= getXmlNodeText(ref doc, "qcby", "QC By: ");
                chkReceived.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "received", ""));
                txtPMTnumber.Text = getXmlNodeText(ref doc, "pmtmediaid", "PMT #: ");
                ddlResolution.SelectedValue = getXmlNodeText(ref doc, "resolution", "");
                ddlAudtio.SelectedValue = getXmlNodeText(ref doc, "audio", "");
                ddlLanguage.SelectedValue = getXmlNodeText(ref doc, "language", "");
                ddlTimecode.SelectedValue = getXmlNodeText(ref doc, "timecode", "");
                chkWatermark.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "watermark", ""));
                ddlType.SelectedValue = getXmlNodeText(ref doc, "type", "");
                chkDownConvert.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "downconvert", ""));
                chkCenterCutSafe.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "centercutsafe", ""));
                chkLetterbox.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "letterbox", ""));
                chkClosedCaptioned.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "closedcaptioned", ""));
                txtTitle.Text = getXmlNodeText(ref doc, "title", "Title: ");
                txtVersion.Text = getXmlNodeText(ref doc, "version", "Version: ");
                txtTRT.Text = getXmlNodeText(ref doc, "trt", "TRT: ");
                txtOffer.Text = getXmlNodeText(ref doc, "offer", "Offer: ");
                txtHost.Text = getXmlNodeText(ref doc, "host", "Host: ");
                ddlFormat.SelectedValue = getXmlNodeText(ref doc, "format", "");
                txtAKA.Text = getXmlNodeText(ref doc, "aka", "A.K.A.: ");
                txtOfferDate.Text = getXmlNodeText(ref doc, "labeldate", "Date: ");
                txtDisclaimerFor.Text = getXmlNodeText(ref doc, "for", "For: ");
                txtDisclaimerBy.Text = getXmlNodeText(ref doc, "by", "By: ");
                ddlDisclaimerOpen.SelectedValue = getXmlNodeText(ref doc, "open", "");
                ddlDisclaimerClose.SelectedValue = getXmlNodeText(ref doc, "close", "");
                chkExercise.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "exercise", ""));
                txtExerciseTimes.Text = getXmlNodeText(ref doc, "exercisetimes", "How Many Times: ");
                chkUIGEA.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "uigea", ""));
                txtUIGEATimes.Text = getXmlNodeText(ref doc, "uigeatimes", "How Many Times: ");
                chkStateTimes.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "staterestrictions", ""));
                txtStateTimes.Text = getXmlNodeText(ref doc, "statetimes", "How Many Times: ");
                chkResultsVary.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "actual", ""));
                txtResultsVaryTimes.Text = getXmlNodeText(ref doc, "actualtimes", "How Many Times: ");
                chkApple.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "apple", ""));
                txtAppleTimes.Text = getXmlNodeText(ref doc, "appletimes", "How Many Times: ");
                //price", ddlPrice.SelectedValue
                txtPrice2.Text = getXmlNodeText(ref doc, "price2", "");
                txtPrice3.Text = getXmlNodeText(ref doc, "price3", "$ ");
                txtSandH.Text = getXmlNodeText(ref doc, "sandh", "SHIPPING & HANDLING $: ");
                foreach(ListItem li in chkCCList.Items)
                {
                    li.Selected = Convert.ToBoolean(getXmlNodeText(ref doc, li.Value, ""));
                }
                chkMoneyBack.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "moneyback", ""));
                chkLessSandH.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "lesssandh", ""));
                chkUSFunds.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "usfunds", ""));
                chkResponseAddressLocation.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "responseaddresslocation", ""));
                txtResponseStreet.Text = getXmlNodeText(ref doc, "responsestreet","");
                txtResponseCity.Text = getXmlNodeText(ref doc, "responsecity", "");
                txtResponseState.Text = getXmlNodeText(ref doc, "responsestate", "");
                txtResponseZip.Text = getXmlNodeText(ref doc, "responsezip", "");
                chkWebsite.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "website", ""));
                txtWebsiteAddress.Text = getXmlNodeText(ref doc, "url", "Website Address: ");
                chkInCTA.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "incta", ""));
                chkWithPhone.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "withphone", ""));
                chkClub.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "club", ""));
                chkClock.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "clock", ""));
                chkEDLReq.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "edlreq", ""));
                chkGenericVO.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "genericvo", ""));
                chkScript.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "script", ""));
                txtDelivery.Text = getXmlNodeText(ref doc, "delivery", "Delivery: ");
                chk18Over.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "eightteen", ""));
                txtAttachments.Text = getXmlNodeText(ref doc, "attachments", "Attachments: ");
                chkSlate.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "slate", ""));
                chkJpeg.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "jpeg", ""));
                chkScripts.Checked = Convert.ToBoolean(getXmlNodeText(ref doc, "scripts", ""));
                ddlEDL.SelectedValue = getXmlNodeText(ref doc, "edl", "");
                txtComments.Text = getXmlNodeText(ref doc, "comments", "Comments/Notes: ");
            }
        }

        private string getXmlNodeText(ref XmlDocument doc, string nodeName, string label)
        {
            string returnMe = "";
            XmlNode root = doc.SelectSingleNode("root");
            try
            {
                XmlNode x = root.SelectSingleNode(nodeName);
                returnMe = x.InnerText;
                if(label!="" && returnMe.IndexOf(label)==-1)
                {
                    returnMe = label + returnMe;
                }
            }
            catch { }
            return returnMe;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            //XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            //XmlElement root = doc.DocumentElement;
            //doc.InsertBefore(xmlDeclaration, root);
            XmlNode rootNode = doc.CreateElement(string.Empty, "root", string.Empty);
            doc.AppendChild(rootNode);
                        
            makeXmlNode(ref doc, "date", txtDate.Text);
            makeXmlNode(ref doc, "vaultid", txtVaultId.Text);
            makeXmlNode(ref doc, "qcby", txtQc.Text);
            makeXmlNode(ref doc, "received", chkReceived.Checked.ToString());
            makeXmlNode(ref doc, "pmtmediaid", txtPMTnumber.Text);
            makeXmlNode(ref doc, "resolution", ddlResolution.SelectedValue);
            makeXmlNode(ref doc, "audio", ddlAudtio.SelectedValue);
            makeXmlNode(ref doc, "language", ddlLanguage.SelectedValue);
            makeXmlNode(ref doc, "timecode", ddlTimecode.SelectedValue);
            makeXmlNode(ref doc, "watermark", chkWatermark.Checked.ToString());
            makeXmlNode(ref doc, "type", ddlType.SelectedValue);
            makeXmlNode(ref doc, "downconvert", chkDownConvert.Checked.ToString());
            makeXmlNode(ref doc, "centercutsafe", chkCenterCutSafe.Checked.ToString());
            makeXmlNode(ref doc, "letterbox", chkLetterbox.Checked.ToString());
            makeXmlNode(ref doc, "closedcaptioned", chkClosedCaptioned.Checked.ToString());
            makeXmlNode(ref doc, "title", txtTitle.Text);
            makeXmlNode(ref doc, "version", txtVersion.Text);
            makeXmlNode(ref doc, "trt", txtTRT.Text);
            makeXmlNode(ref doc, "offer", txtOffer.Text);
            makeXmlNode(ref doc, "host", txtHost.Text);
            makeXmlNode(ref doc, "format", ddlFormat.SelectedValue);
            makeXmlNode(ref doc, "aka", txtAKA.Text);
            makeXmlNode(ref doc, "labeldate", txtOfferDate.Text);
            makeXmlNode(ref doc, "for", txtDisclaimerFor.Text);
            makeXmlNode(ref doc, "by", txtDisclaimerBy.Text);
            makeXmlNode(ref doc, "open", ddlDisclaimerOpen.SelectedValue);
            makeXmlNode(ref doc, "close", ddlDisclaimerClose.SelectedValue);
            makeXmlNode(ref doc, "exercise", chkExercise.Checked.ToString());
            makeXmlNode(ref doc, "exercisetimes", txtExerciseTimes.Text);
            makeXmlNode(ref doc, "uigea", chkUIGEA.Checked.ToString());
            makeXmlNode(ref doc, "uigeatimes", txtUIGEATimes.Text);
            makeXmlNode(ref doc, "staterestrictions", chkStateTimes.Checked.ToString());
            makeXmlNode(ref doc, "statetimes", txtStateTimes.Text);
            makeXmlNode(ref doc, "actual", chkResultsVary.Checked.ToString());
            makeXmlNode(ref doc, "actualtimes", txtResultsVaryTimes.Text);
            makeXmlNode(ref doc, "apple", chkApple.Checked.ToString());
            makeXmlNode(ref doc, "appletimes", txtAppleTimes.Text);
            makeXmlNode(ref doc, "price2", txtPrice2.Text);
            makeXmlNode(ref doc, "price3", txtPrice3.Text);
            makeXmlNode(ref doc, "sandh", txtSandH.Text);
            foreach(ListItem li in chkCCList.Items)
            {
                makeXmlNode(ref doc, li.Value, li.Selected.ToString());
            }
            makeXmlNode(ref doc, "moneyback", chkMoneyBack.Checked.ToString());
            makeXmlNode(ref doc, "lesssandh", chkLessSandH.Checked.ToString());
            makeXmlNode(ref doc, "usfunds", chkUSFunds.Checked.ToString());
            makeXmlNode(ref doc, "responseaddresslocation", chkResponseAddressLocation.Checked.ToString());
            makeXmlNode(ref doc, "responsestreet", txtResponseStreet.Text);
            makeXmlNode(ref doc, "responsecity", txtResponseCity.Text);
            makeXmlNode(ref doc, "responsestate", txtResponseState.Text);
            makeXmlNode(ref doc, "responsezip", txtResponseZip.Text);
            makeXmlNode(ref doc, "website", chkWebsite.Checked.ToString());
            makeXmlNode(ref doc, "url", txtWebsiteAddress.Text);
            makeXmlNode(ref doc, "incta", chkInCTA.Checked.ToString());
            makeXmlNode(ref doc, "withphone", chkWithPhone.Checked.ToString());
            makeXmlNode(ref doc, "club", chkClub.Checked.ToString());
            makeXmlNode(ref doc, "clock", chkClock.Checked.ToString());
            makeXmlNode(ref doc, "edlreq", chkEDLReq.Checked.ToString());
            makeXmlNode(ref doc, "genericvo", chkGenericVO.Checked.ToString());
            makeXmlNode(ref doc, "script", chkScript.Checked.ToString());
            makeXmlNode(ref doc, "delivery", txtDelivery.Text);
            makeXmlNode(ref doc, "eightteen", chk18Over.Checked.ToString());
            makeXmlNode(ref doc, "attachments", txtAttachments.Text);
            makeXmlNode(ref doc, "slate", chkSlate.Checked.ToString());
            makeXmlNode(ref doc, "jpeg", chkJpeg.Checked.ToString());
            makeXmlNode(ref doc, "scripts", chkScripts.Checked.ToString());
            makeXmlNode(ref doc, "edl", ddlEDL.SelectedValue);
            makeXmlNode(ref doc, "comments", txtComments.Text);
            if(Request.QueryString["Master"]!=null)
            {
                int MasterId = Convert.ToInt32(Request.QueryString["Master"]);
                AdminController aCont = new AdminController();
                MasterItemInfo master = aCont.Get_MasterItemById(Convert.ToInt32(Request.QueryString["Master"]));
                master.CheckListForm = doc.OuterXml.ToString();
                master.LastModifiedById = UserId;
                master.LastModifiedDate = DateTime.Now;
                aCont.Update_MasterItem(master);
                lblMessage.Text = "Checklist Saved.";
                Application["MasterItems"] = null;
            }
            
        }
        private void makeXmlNode(ref XmlDocument doc, string nodeName, string nodeText)
        {
            XmlNode root = doc.DocumentElement;
            XmlElement element1 = doc.CreateElement(string.Empty, nodeName, string.Empty);
            element1.InnerText = nodeText;
            //XmlText text1 = doc.CreateTextNode(nodeText);
            //element1.AppendChild(text1);
            root.AppendChild(element1);
        }
    }
}