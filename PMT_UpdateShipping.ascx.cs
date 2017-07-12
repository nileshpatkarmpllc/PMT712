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
    public partial class PMT_UpdateShipping : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if(Request.QueryString["woid"]==null)
            {
                lblMessage.Text = "Invalid Work Order";
                btnSubmitAddresses.Visible = false;
            }
            else
            {
                try
                {
                    if(!Page.IsPostBack)
                    {
                        ViewState["popupFirst"] = "true";
                    }
                    drawAddresses();
                }
                catch
                {
                    lblMessage.Text = "Invalid Work Order";
                    btnSubmitAddresses.Visible = false;
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
        private void drawAddresses()
        {
            if (Request.QueryString["woid"] != null)
            {
                AdminController aCont = new AdminController();
                WorkOrderInfo wo = aCont.Get_WorkOrderById(Convert.ToInt32(Request.QueryString["woid"]));
                List<TaskInfo> tasks = aCont.Get_TasksByWOId(wo.Id);
                bool hasShipping = false;
                if (ViewState["popupFirst"] == null)
                { ViewState["popupFirst"] = "false"; }
                foreach (WOGroupInfo group in wo.Groups)
                {
                    Literal litGroupTitle = new Literal();
                    litGroupTitle.ID = "litGroupTitle_" + group.Id.ToString();
                    litGroupTitle.Text = "<h3>" + group.GroupName + "</h3>";
                    pnlAddresses.Controls.Add(litGroupTitle);
                    foreach (WOGroupStationInfo station in group.WOGroupStations)
                    {
                        if (station.DeliveryMethod.IndexOf("tf_") != -1)
                        {
                            bool showThis = false;
                            var task = tasks.FirstOrDefault(o => o.WOGroupId == group.Id && o.StationId == station.Id);
                            if (task != null && task.DeliveryStatus != "COMPLETE" && task.DeliveryStatus != "CANCELLED" || (task.DeliveryStatus=="PENDING" && task.DeliveryOrderId=="" ))
                            {
                                showThis = true;
                            }
                            if (showThis)
                            {
                                hasShipping = true;
                                Literal lit1 = new Literal();
                                lit1.ID = "lit1_" + group.Id.ToString() + "_" + station.Id.ToString();
                                if (ViewState["popupFirst"].ToString() == "true")
                                { lit1.Text = "<div class=\"col25\">Station:</div><div class=\"col50\">" + station.Station.StationName + "</div><br clear=\"both\" /><div class=\"col25\">Street1:</div><div class=\"col50\">"; }
                                pnlAddresses.Controls.Add(lit1);
                                TextBox txtStreet1 = new TextBox();
                                txtStreet1.ID = "txtStreet1_" + group.Id.ToString() + "_" + station.Id.ToString();
                                if (ViewState["popupFirst"].ToString() == "true")
                                { txtStreet1.Text = station.Station.Address1; }
                                pnlAddresses.Controls.Add(txtStreet1);
                                Literal lit2 = new Literal();
                                lit2.ID = "lit2_" + group.Id.ToString() + "_" + station.Id.ToString();
                                if (ViewState["popupFirst"].ToString() == "true")
                                { lit2.Text = "</div><br clear=\"both\" /><div class=\"col25\">Street2:</div><div class=\"col50\">"; }
                                pnlAddresses.Controls.Add(lit2);
                                TextBox txtStreet2 = new TextBox();
                                txtStreet2.ID = "txtStreet2_" + group.Id.ToString() + "_" + station.Id.ToString();
                                if (ViewState["popupFirst"].ToString() == "true")
                                { txtStreet2.Text = station.Station.Address2; }
                                pnlAddresses.Controls.Add(txtStreet2);
                                Literal lit3 = new Literal();
                                lit3.ID = "lit3_" + group.Id.ToString() + "_" + station.Id.ToString();
                                if (ViewState["popupFirst"].ToString() == "true")
                                { lit3.Text = "</div><br clear=\"both\" /><div class=\"col25\">City:</div><div class=\"col50\">"; }
                                pnlAddresses.Controls.Add(lit3);
                                TextBox txtCity = new TextBox();
                                txtCity.ID = "txtCity_" + group.Id.ToString() + "_" + station.Id.ToString();
                                if (ViewState["popupFirst"].ToString() == "true")
                                { txtCity.Text = station.Station.City; }
                                pnlAddresses.Controls.Add(txtCity);
                                Literal lit4 = new Literal();
                                lit4.ID = "lit4_" + group.Id.ToString() + "_" + station.Id.ToString();
                                if (ViewState["popupFirst"].ToString() == "true")
                                { lit4.Text = "</div><br clear=\"both\" /><div class=\"col25\">State:</div><div class=\"col50\">"; }
                                pnlAddresses.Controls.Add(lit4);
                                TextBox txtState = new TextBox();
                                txtState.ID = "txtState_" + group.Id.ToString() + "_" + station.Id.ToString();
                                if (ViewState["popupFirst"].ToString() == "true")
                                { txtState.Text = station.Station.State; }
                                pnlAddresses.Controls.Add(txtState);
                                Literal lit5 = new Literal();
                                lit5.ID = "lit5_" + group.Id.ToString() + "_" + station.Id.ToString();
                                if (ViewState["popupFirst"].ToString() == "true")
                                { lit5.Text = "</div><br clear=\"both\" /><div class=\"col25\">Zip:</div><div class=\"col50\">"; }
                                pnlAddresses.Controls.Add(lit5);
                                TextBox txtZip = new TextBox();
                                txtZip.ID = "txtZip_" + group.Id.ToString() + "_" + station.Id.ToString();
                                if (ViewState["popupFirst"].ToString() == "true")
                                { txtZip.Text = station.Station.Zip; }
                                pnlAddresses.Controls.Add(txtZip);
                                Literal lit6 = new Literal();
                                lit6.ID = "lit6_" + group.Id.ToString() + "_" + station.Id.ToString();
                                if (ViewState["popupFirst"].ToString() == "true")
                                { lit6.Text = "</div><br clear=\"both\" /><div class=\"col25\">Attention:</div><div class=\"col50\">"; }
                                pnlAddresses.Controls.Add(lit6);
                                TextBox txtAttentionLine = new TextBox();
                                txtAttentionLine.ID = "txtAttentionLine_" + group.Id.ToString() + "_" + station.Id.ToString();
                                if (ViewState["popupFirst"].ToString() == "true")
                                { txtAttentionLine.Text = station.Station.AttentionLine; }
                                pnlAddresses.Controls.Add(txtAttentionLine);
                                Literal lit6b = new Literal();
                                lit6b.ID = "lit6b_" + group.Id.ToString() + "_" + station.Id.ToString();
                                if (ViewState["popupFirst"].ToString() == "true")
                                { lit6b.Text = "</div><br clear=\"both\" /><div class=\"col25\">Phone:</div><div class=\"col50\">"; }
                                pnlAddresses.Controls.Add(lit6b);
                                TextBox txtPhone = new TextBox();
                                txtPhone.ID = "txtPhone_" + group.Id.ToString() + "_" + station.Id.ToString();
                                if (ViewState["popupFirst"].ToString() == "true")
                                { txtPhone.Text = station.Station.Phone; }
                                pnlAddresses.Controls.Add(txtPhone);
                                Literal lit7 = new Literal();
                                lit7.ID = "lit7_" + group.Id.ToString() + "_" + station.Id.ToString();
                                if (ViewState["popupFirst"].ToString() == "true")
                                { lit7.Text = "</div><br clear=\"both\" /><br />"; }
                                pnlAddresses.Controls.Add(lit7);
                            }
                        }
                    }
                }
                if (ViewState["popupFirst"].ToString() == "true")
                { ViewState["popupFirst"] = "false"; }
            }
        }

        protected void btnSubmitAddresses_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();            
            WorkOrderInfo wo = aCont.Get_WorkOrderById(Convert.ToInt32(Request.QueryString["woid"]));
            foreach(WOGroupInfo group in wo.Groups)
            {
                foreach(WOGroupStationInfo groupStation in group.WOGroupStations)
                {
                    TextBox txtStreet1 = (TextBox)pnlAddresses.FindControl("txtStreet1_" + group.Id.ToString() + "_" + groupStation.Id.ToString());
                    if(txtStreet1 != null)
                    {
                        groupStation.Station.Address1 = txtStreet1.Text;
                    }
                    TextBox txtStreet2 =  (TextBox)pnlAddresses.FindControl("txtStreet2_" + group.Id.ToString() + "_" + groupStation.Id.ToString());
                    if(txtStreet2 != null)
                    {
                        groupStation.Station.Address2 = txtStreet2.Text;
                    }
                    TextBox txtCity =  (TextBox)pnlAddresses.FindControl("txtCity_" + group.Id.ToString() + "_" + groupStation.Id.ToString());
                    if(txtCity != null)
                    {
                        groupStation.Station.City = txtCity.Text;
                    }
                    TextBox txtState = (TextBox)pnlAddresses.FindControl("txtState_" + group.Id.ToString() + "_" + groupStation.Id.ToString());
                    if (txtState != null)
                    {
                        groupStation.Station.State = txtState.Text;
                    }
                    TextBox txtZip = (TextBox)pnlAddresses.FindControl("txtZip_" + group.Id.ToString() + "_" + groupStation.Id.ToString());
                    if (txtZip != null)
                    {
                        groupStation.Station.Zip = txtZip.Text;
                    }
                    TextBox txtAttentionLine = (TextBox)pnlAddresses.FindControl("txtAttentionLine_" + group.Id.ToString() + "_" + groupStation.Id.ToString());
                    if (txtAttentionLine != null)
                    {
                        groupStation.Station.AttentionLine = txtAttentionLine.Text;
                    }
                    TextBox txtPhone = (TextBox)pnlAddresses.FindControl("txtPhone_" + group.Id.ToString() + "_" + groupStation.Id.ToString());
                    if (txtPhone != null)
                    {
                        groupStation.Station.Phone = txtPhone.Text;
                    }
                }
            }
            lblMessage.Text += aCont.createEasySpotOrder(wo.Groups);
            if(lblMessage.Text.IndexOf("Error:")==-1)
            {
                Response.Redirect("/Work-Orders.aspx?woid=" + wo.Id.ToString());
            }            
        }
    }
}