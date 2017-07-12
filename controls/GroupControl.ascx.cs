using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;

namespace Christoc.Modules.PMT_Admin
{
    public partial class GroupControl : PMT_AdminModuleBase, IActionable
    {
        public event EventHandler GroupUpdated;
        public event EventHandler GroupDeleted;
        public WOGroupInfo Group {
            get { return (WOGroupInfo)(ViewState["WOGroup"] ?? new WOGroupInfo()); }

            set { ViewState["WOGroup"] = value; }
        }
        public string GroupName { get; set; }
        public bool EditMode { get; set; }
        protected void Page_Init(object sender, EventArgs e)
        {
            addLibItemControls();
            addStationControls();
            fillServices();
            txtGroupNotes.Text = Group.Comments;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Group != null)
            {
                if (Group.GroupType == GroupTypeEnum.Non_Deliverable)
                {
                    pnlNonDeliver.Visible = true;
                    pnlDeliver.Visible = false;
                    if (txtNonDeliverGroupName.Text == "")
                    {
                        txtNonDeliverGroupName.Text = Group.GroupName;
                    }
                    txtNonDeliverableNotes.Text = Group.Comments;
                    if (EditMode)
                    {
                        pnlNonDeliverEdit.Visible = true;
                        pnlNonDeliverView.Visible = false;
                        txtNonDeliverGroupName.Enabled = true;
                        btnDeleteGroup.Visible = true;
                        txtNonDeliverableNotes.Enabled = true;
                    }
                    else
                    {
                        AdminController aCont = new AdminController();
                        pnlNonDeliverEdit.Visible = false;
                        pnlNonDeliverView.Visible = true;
                        txtNonDeliverGroupName.Enabled = false;
                        btnDeleteGroup.Visible = false;
                        txtNonDeliverableNotes.Enabled = false;
                        litServices.Text = "MASTER<br />";
                        litServices.Text += "MASTER ID: " + Group.Master.PMTMediaId + "<br />";
                        litServices.Text += "TITLE: " + Group.Master.Title + "<br />";
                        AdvertiserInfo ad = aCont.Get_AdvertiserById(Group.Master.AdvertiserId);
                        litServices.Text += "ADVERTISER: " + ad.AdvertiserName + "<br />";
                        litServices.Text += "AGENCIES: <br />";
                        List<AgencyInfo> ags = Group.Master.Agencies;
                        foreach(AgencyInfo ag in ags)
                        {
                            litServices.Text += "&nbsp;&nbsp;" + ag.AgencyName + "<br />";
                        }
                        litServices.Text += "<br /><br />SERVICES:<br />";
                        foreach(ServiceInfo serv in Group.Services)
                        {
                            litServices.Text += serv.ServiceName + "<br />";
                        }
                        litServices.Text += "<br />NOTES:<br />" + Group.Comments;
                    }
                    litMasterInfo.Text = "MasterId: " + Group.Master.PMTMediaId + " Title: " + Group.Master.Title + " Product/Description: " + Group.Master.Comment + "<br /><br />";
                    //TODO: Show advertiser and agency (How do we know which agency? Do they need to choose?)
                }
                else
                {
                    pnlNonDeliver.Visible = false;
                    pnlDeliver.Visible = true;
                    if(Group.GroupType == GroupTypeEnum.Customized || Group.GroupType == GroupTypeEnum.Bundle)
                    {
                        cklCustomizeServices.Visible = true;
                    }
                    else
                    {
                        cklCustomizeServices.Visible = false;
                    }
                    if (txtDeliverGroupName.Text == "")
                    {
                        txtDeliverGroupName.Text = Group.GroupName;
                    }
                    txtGroupNotes.Text = Group.Comments;
                    if (EditMode)
                    {
                        pnlDeliverEdit.Visible = true;
                        pnlDeliverView.Visible = false;
                        txtDeliverGroupName.Enabled = true;
                        btnDeleteGroup.Visible = true;
                        txtGroupNotes.Enabled = true;
                    }
                    else
                    {
                        pnlDeliverEdit.Visible = false;
                        pnlDeliverView.Visible = true;
                        txtDeliverGroupName.Enabled = false;
                        btnDeleteGroup.Visible = false;
                        txtGroupNotes.Enabled = false;
                        litCustomizeServices.Text = "SERVICES: <br />";
                        foreach (ServiceInfo serv in Group.Services)
                        {
                            litCustomizeServices.Text += serv.ServiceName + "<br />";
                        }
                        drawDeliveryDisplay();
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
        private void drawDeliveryDisplay()
        {
            string display = "";
            txtDisplayGroupNotes.Text = Group.Comments;
            if (Group.GroupType == GroupTypeEnum.Bundle || Group.GroupType == GroupTypeEnum.Delivery)
            {
                AdminController aCont = new AdminController();
                List<MarketInfo> markets = aCont.Get_MarketsByPortalId(PortalId);
                List<AdvertiserInfo> advertisers = aCont.Get_AdvertisersByPortalId(PortalId);
                List<CarrierTypeInfo> carriers = aCont.Get_CarrierTypesByPortalId(PortalId);
                List<DeliveryMethodInfo> dms = aCont.Get_DeliveryMethodsByPortalId(PortalId);
                List<TapeFormatInfo> tfs = aCont.Get_TapeFormatsByPortalId(PortalId);
                List<TaskInfo> tasks = aCont.Get_TasksByWOId(Group.WorkOrderId);
                if (Group.LibraryItems.Count > 0)
                {
                    foreach (LibraryItemInfo libItem in Group.LibraryItems)
                    {
                        display += "<div class=\"delDisStRow\"><div class=\"deliverDisplayColHeader delDisLiCol1\">MASTER ID</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisLiCol2\">ISCI</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisLiCol3\">TITLE</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisLiCol4\">DESCRIPTION</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisLiCol5\">ENCODE</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisLiCol6\">LENGTH</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisLiCol7\">CC</div></div>";

                        display += "<div class=\"delDisStRow\"><div class=\"deliverDislayColData delDisLiCol1\">" + libItem.PMTMediaId + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol2\">" + libItem.ISCICode + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol3\">" + libItem.Title + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol4\">" + libItem.ProductDescription + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol5\">" + libItem.Encode + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol6\">" + libItem.MediaLength + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol7\">" + libItem.ClosedCaptioned.ToString().ToUpper() + "</div></div>";

                        display += "<div class=\"delDisStRow\"><div class=\"deliverDisplayColHeader delDisStCol1\">CALL LETTER</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisStCol2\">DELIVERY METHOD</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisStCol3\">SHIPPING</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisStCol3a\">QUANTITY</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisStCol4\">PRIORITY</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisStCol5\">TRACKING NUMBER</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisStCol6\">DATE</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisStCol7\">STATUS</div></div>";
                        //display += "<div class=\"deliverDisplayColHeader delDisStCol8\">INVOICE #</div></div>";

                        foreach (WOGroupStationInfo station in Group.WOGroupStations)
                        {
                            TaskInfo thisTask = new TaskInfo();
                            foreach(TaskInfo task in tasks)
                            {
                                if(task.LibraryId==libItem.Id && task.StationId==station.Id && task.WOGroupId==Group.Id)
                                {
                                    thisTask = task;
                                    break;
                                }
                            }
                            DropDownList ddlStationDelivery = new DropDownList();
                            ddlStationDelivery.ID = "ddlStationsDelivery_" + this.ID + "_" + station.StationId.ToString();
                            ddlStationDelivery.CssClass = "woStationDelivery";
                            string[] deliveryMethods = station.Station.DeliveryMethods.Split(',');
                            string[] tapeFormats = station.Station.TapeFormat.Split(',');
                            foreach (DeliveryMethodInfo dm in dms)
                            {
                                foreach (string dmi in deliveryMethods)
                                {
                                    if (dm.Id.ToString() == dmi)
                                    {
                                        ddlStationDelivery.Items.Add(new ListItem(dm.DeliveryMethod, "dm_" + dmi));
                                    }
                                }
                            }
                            foreach (TapeFormatInfo tf in tfs)
                            {
                                foreach (string tfi in tapeFormats)
                                {
                                    if (tf.Id.ToString() == tfi)
                                    {
                                        ddlStationDelivery.Items.Add(new ListItem(tf.TapeFormat, "tf_" + tfi));
                                    }
                                }
                            }
                            ddlStationDelivery.SelectedValue = station.DeliveryMethod;
                            string shipping = "";
                            string priority = "";
                            string quantity = "";
                            if (station.DeliveryMethod.IndexOf("tf_") != -1)
                            {
                                DropDownList ddlStationShipping = new DropDownList();
                                ddlStationShipping.ID = "ddlStationsShipping_" + this.ID + "_" + station.Station.Id.ToString();
                                ddlStationShipping.CssClass = "woStationShipping";
                                //ddlStationShipping.Items.Add(new ListItem("--Carrier--", ""));
                                if (Group.LibraryItems.Count > 0)
                                {
                                    try
                                    {
                                        var advertiser = advertisers.FirstOrDefault(i => i.Id == Group.LibraryItems[0].AdvertiserId);
                                        var carrierType = carriers.FirstOrDefault(i => i.Id == advertiser.Carrier);
                                        ddlStationShipping.Items.Add(new ListItem(carrierType.CarrierType, carrierType.Id.ToString()));
                                    }
                                    catch { }
                                }
                                ddlStationShipping.Items.Add(new ListItem("PMT Fedex", "-1"));
                                try
                                {
                                    ddlStationShipping.SelectedValue = station.ShippingMethodId.ToString();
                                    shipping = ddlStationShipping.SelectedItem.Text;
                                }
                                catch { }

                                DropDownList ddlStationQuantity = new DropDownList();
                                ddlStationQuantity.ID = "ddlStationsQuantity_" + this.ID + "_" + station.Station.Id.ToString();
                                ddlStationQuantity.CssClass = "woStationPriority";
                                for(int i = 1; i<=10; i++)
                                {
                                    ddlStationQuantity.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                }
                                try
                                {
                                    ddlStationQuantity.SelectedValue = station.Quantity.ToString();
                                    quantity = ddlStationQuantity.SelectedItem.Text;
                                }
                                catch { }

                                DropDownList ddlStationPriority = new DropDownList();
                                ddlStationPriority.ID = "ddlStationsPriority_" + this.ID + "_" + station.Station.Id.ToString();
                                ddlStationPriority.CssClass = "woStationPriority";
                                if (Group.LibraryItems.Count > 0)
                                {
                                    try
                                    {
                                        var advertiser = advertisers.FirstOrDefault(i => i.Id == Group.LibraryItems[0].AdvertiserId);
                                        var carrierType = carriers.FirstOrDefault(i => i.Id == advertiser.Carrier);
                                        if (carrierType.Id == 1)
                                        {
                                            ddlStationPriority.Items.Add(new ListItem("--Service--", "-1"));
                                            ddlStationPriority.Items.Add(new ListItem("Priority", "1"));
                                            ddlStationPriority.Items.Add(new ListItem("Standard", "2"));
                                            ddlStationPriority.Items.Add(new ListItem("2-Day", "3"));
                                        }
                                        else if (carrierType.Id == 2)
                                        {
                                            ddlStationPriority.Items.Add(new ListItem("--Service--", "-1"));
                                            ddlStationPriority.Items.Add(new ListItem("Priority", "1"));
                                            ddlStationPriority.Items.Add(new ListItem("Standard", "2"));
                                            ddlStationPriority.Items.Add(new ListItem("2-Day", "3"));
                                            ddlStationPriority.Items.Add(new ListItem("2-Day AM", "4"));
                                            //ddlStationPriority.Items.Add(new ListItem("3-Day", "5"));
                                        }
                                    }
                                    catch { }
                                }
                                try
                                {
                                    ddlStationPriority.SelectedValue = station.PriorityId.ToString();
                                    priority = ddlStationPriority.SelectedItem.Text;
                                }
                                catch { }
                            }
                            

                            display += "<div class=\"delDisStRow\"><div class=\"deliverDislayColData delDisStCol1\">" + station.Station.CallLetter + "</div>";
                            display += "<div class=\"deliverDislayColData delDisStCol2\">" + ddlStationDelivery.SelectedItem.Text + "</div>";
                            display += "<div class=\"deliverDislayColData delDisStCol3\">" + shipping + "</div>";
                            display += "<div class=\"deliverDislayColData delDisStCol3a\">" + quantity + "</div>";
                            display += "<div class=\"deliverDislayColData delDisStCol4\">" + priority + "</div>";
                            if (thisTask.Quantity > 0 && thisTask.DeliveryMethodId == 1)
                            {
                                //fedex
                                display += "<div class=\"deliverDislayColData delDisStCol5\"><a href=\"https://www.fedex.com/apps/fedextrack/?action=track&action=track&language=english&cntry_code=us&tracknumbers=" + thisTask.DeliveryOrderId.ToString() + "\" target=\"_blank\">" + thisTask.DeliveryOrderId.ToString() + "</a></div>";
                            }
                            else
                            {
                                display += "<div class=\"deliverDislayColData delDisStCol5\">" + thisTask.DeliveryOrderId.ToString() + "</div>";
                            }
                            display += "<div class=\"deliverDislayColData delDisStCol6\">" + thisTask.LastModifiedDate.ToShortDateString() + "</div>";
                            display += "<div class=\"deliverDislayColData delDisStCol7\">" + thisTask.DeliveryStatus + "</div>";
                            //display += "<div class=\"deliverDislayColData delDisStCol8\">12356</div>";
                            display += "</div>";
                        }
                        //display += "<br clear=\"both\" /></div>";
                    }
                    //display += "<br clear=\"both\" /></div>";
                }                
            }
            else
            {
                if (Group.LibraryItems.Count > 0)
                {
                    display += "<div class=\"delDisStRow\"><div class=\"deliverDisplayColHeader delDisLiCol1\">MASTER ID</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisLiCol2\">ISCI</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisLiCol3\">TITLE</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisLiCol4\">DESCRIPTION</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisLiCol5\">ENCODE</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisLiCol6\">LENGTH</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisLiCol7\">CC</div></div>";

                    foreach (LibraryItemInfo libItem in Group.LibraryItems)
                    {
                        display += "<div class=\"delDisStRow\"><div class=\"deliverDislayColData delDisLiCol1\">" + libItem.PMTMediaId + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol2\">" + libItem.ISCICode + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol3\">" + libItem.Title + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol4\">" + libItem.ProductDescription + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol5\">" + libItem.Encode + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol6\">" + libItem.MediaLength + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol7\">" + libItem.ClosedCaptioned.ToString().ToUpper() + "</div></div>";
                    }
                    display += "<br clear=\"both\" /></div>";
                }
                display += "<br clear=\"both\" />";
            }
            //display += "</div>";
            litDeliverView.Text = display;
        }
        private void drawDeliveryDisplayOld()
        {
            string display = "";
            txtDisplayGroupNotes.Text = Group.Comments;
            if(Group.GroupType==GroupTypeEnum.Bundle || Group.GroupType==GroupTypeEnum.Delivery)
            {
                AdminController aCont = new AdminController();
                List<MarketInfo> markets = aCont.Get_MarketsByPortalId(PortalId);
                List<AdvertiserInfo> advertisers = aCont.Get_AdvertisersByPortalId(PortalId);
                List<CarrierTypeInfo> carriers = aCont.Get_CarrierTypesByPortalId(PortalId);
                List<DeliveryMethodInfo> dms = aCont.Get_DeliveryMethodsByPortalId(PortalId);
                List<TapeFormatInfo> tfs = aCont.Get_TapeFormatsByPortalId(PortalId);
                foreach(WOGroupStationInfo station in Group.WOGroupStations)
                {
                    DropDownList ddlStationDelivery = new DropDownList();
                    ddlStationDelivery.ID = "ddlStationsDelivery_" + this.ID + "_" + station.StationId.ToString();
                    ddlStationDelivery.CssClass = "woStationDelivery";
                    string[] deliveryMethods = station.Station.DeliveryMethods.Split(',');
                    string[] tapeFormats = station.Station.TapeFormat.Split(',');
                    foreach (DeliveryMethodInfo dm in dms)
                    {
                        foreach (string dmi in deliveryMethods)
                        {
                            if (dm.Id.ToString() == dmi)
                            {
                                ddlStationDelivery.Items.Add(new ListItem(dm.DeliveryMethod, "dm_" + dmi));
                            }
                        }
                    }
                    foreach (TapeFormatInfo tf in tfs)
                    {
                        foreach (string tfi in tapeFormats)
                        {
                            if (tf.Id.ToString() == tfi)
                            {
                                ddlStationDelivery.Items.Add(new ListItem(tf.TapeFormat, "tf_" + tfi));
                            }
                        }
                    }
                    ddlStationDelivery.SelectedValue = station.DeliveryMethod;
                    string shipping = "";
                    string priority = "";
                    if (station.DeliveryMethod.IndexOf("tf_") != -1)
                    {
                        DropDownList ddlStationShipping = new DropDownList();
                        ddlStationShipping.ID = "ddlStationsShipping_" + this.ID + "_" + station.Station.Id.ToString();
                        ddlStationShipping.CssClass = "woStationShipping";
                        //ddlStationShipping.Items.Add(new ListItem("--Carrier--", ""));
                        if (Group.LibraryItems.Count > 0)
                        {
                            try
                            {
                                var advertiser = advertisers.FirstOrDefault(i => i.Id == Group.LibraryItems[0].AdvertiserId);
                                var carrierType = carriers.FirstOrDefault(i => i.Id == advertiser.Carrier);
                                ddlStationShipping.Items.Add(new ListItem(carrierType.CarrierType, carrierType.Id.ToString()));
                            }
                            catch { }
                        }
                        ddlStationShipping.Items.Add(new ListItem("PMT Fedex", "-1"));
                        try
                        {
                            ddlStationShipping.SelectedValue = station.ShippingMethodId.ToString();
                            shipping = ddlStationShipping.SelectedItem.Text;
                        }
                        catch { }

                        DropDownList ddlStationPriority = new DropDownList();
                        ddlStationPriority.ID = "ddlStationsPriority_" + this.ID + "_" + station.Station.Id.ToString();
                        ddlStationPriority.CssClass = "woStationPriority";
                        if (Group.LibraryItems.Count > 0)
                        {
                            try
                            {
                                var advertiser = advertisers.FirstOrDefault(i => i.Id == Group.LibraryItems[0].AdvertiserId);
                                var carrierType = carriers.FirstOrDefault(i => i.Id == advertiser.Carrier);
                                if (carrierType.Id == 1)
                                {
                                    ddlStationPriority.Items.Add(new ListItem("--Service--", "-1"));
                                    ddlStationPriority.Items.Add(new ListItem("Priority", "1"));
                                    ddlStationPriority.Items.Add(new ListItem("Standard", "2"));
                                    ddlStationPriority.Items.Add(new ListItem("2-Day", "3"));
                                }
                                else if (carrierType.Id == 2)
                                {
                                    ddlStationPriority.Items.Add(new ListItem("--Service--", "-1"));
                                    ddlStationPriority.Items.Add(new ListItem("Priority", "1"));
                                    ddlStationPriority.Items.Add(new ListItem("Standard", "2"));
                                    ddlStationPriority.Items.Add(new ListItem("2-Day", "3"));
                                    ddlStationPriority.Items.Add(new ListItem("2-Day AM", "4"));
                                    ddlStationPriority.Items.Add(new ListItem("3-Day", "5"));
                                }
                            }
                            catch { }
                        }
                        try
                        {
                            ddlStationPriority.SelectedValue = station.PriorityId.ToString();
                            priority = ddlStationPriority.SelectedItem.Text;
                        }
                        catch { }
                    }
                    display += "<div class=\"delDisStRow\"><div class=\"deliverDisplayColHeader delDisStCol1\">CALL LETTER</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisStCol2\">DELIVERY METHOD</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisStCol3\">SHIPPING</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisStCol4\">PRIORITY</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisStCol5\">TRACKING NUMBER</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisStCol6\">DATE</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisStCol7\">STATUS</div>";
                    //display += "<div class=\"deliverDisplayColHeader delDisStCol8\">INVOICE #</div></div>";

                    display += "<div class=\"delDisStRow\"><div class=\"deliverDislayColData delDisStCol1\">" + station.Station.CallLetter + "</div>";
                    display += "<div class=\"deliverDislayColData delDisStCol2\">" + ddlStationDelivery.SelectedItem.Text + "</div>";
                    display += "<div class=\"deliverDislayColData delDisStCol3\">" + shipping + "</div>";
                    display += "<div class=\"deliverDislayColData delDisStCol4\">" + priority + "</div>";
                    display += "<div class=\"deliverDislayColData delDisStCol5\">11111111</div>";
                    display += "<div class=\"deliverDislayColData delDisStCol6\">" + station.LastModifiedDate.ToShortDateString() + "</div>";
                    display += "<div class=\"deliverDislayColData delDisStCol7\">COMPLETE</div>";
                    //display += "<div class=\"deliverDislayColData delDisStCol8\">12356</div>";
                    display += "</div>";

                    if (Group.LibraryItems.Count > 0)
                    {
                        display += "<div class=\"delDisStRow\"><div class=\"deliverDisplayColHeader delDisLiCol1\">MASTER ID</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisLiCol2\">ISCI</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisLiCol3\">TITLE</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisLiCol4\">DESCRIPTION</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisLiCol5\">ENCODE</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisLiCol6\">LENGTH</div>";
                        display += "<div class=\"deliverDisplayColHeader delDisLiCol7\">CC</div></div>";

                        foreach (LibraryItemInfo libItem in Group.LibraryItems)
                        {
                            display += "<div class=\"delDisStRow\"><div class=\"deliverDislayColData delDisLiCol1\">" + libItem.PMTMediaId + "</div>";
                            display += "<div class=\"deliverDislayColData delDisLiCol2\">" + libItem.ISCICode + "</div>";
                            display += "<div class=\"deliverDislayColData delDisLiCol3\">" + libItem.Title + "</div>";
                            display += "<div class=\"deliverDislayColData delDisLiCol4\">" + libItem.ProductDescription + "</div>";
                            display += "<div class=\"deliverDislayColData delDisLiCol5\">" + libItem.Encode + "</div>";
                            display += "<div class=\"deliverDislayColData delDisLiCol6\">" + libItem.MediaLength + "</div>";
                            display += "<div class=\"deliverDislayColData delDisLiCol7\">" + libItem.ClosedCaptioned.ToString().ToUpper() + "</div></div>";
                        }
                        display += "<br clear=\"both\" />";
                    }
                }
            }
            else
            {
                if (Group.LibraryItems.Count > 0)
                {
                    display += "<div class=\"delDisStRow\"><div class=\"deliverDisplayColHeader delDisLiCol1\">MASTER ID</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisLiCol2\">ISCI</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisLiCol3\">TITLE</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisLiCol4\">DESCRIPTION</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisLiCol5\">ENCODE</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisLiCol6\">LENGTH</div>";
                    display += "<div class=\"deliverDisplayColHeader delDisLiCol7\">CC</div></div>";

                    foreach (LibraryItemInfo libItem in Group.LibraryItems)
                    {
                        display += "<div class=\"delDisStRow\"><div class=\"deliverDislayColData delDisLiCol1\">" + libItem.PMTMediaId + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol2\">" + libItem.ISCICode + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol3\">" + libItem.Title + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol4\">" + libItem.ProductDescription + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol5\">" + libItem.Encode + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol6\">" + libItem.MediaLength + "</div>";
                        display += "<div class=\"deliverDislayColData delDisLiCol7\">" + libItem.ClosedCaptioned.ToString().ToUpper() + "</div></div>";
                    }
                    display += "<br clear=\"both\" />";
                }
            }
            litDeliverView.Text = display;
        }
        private void fillServices()
        {
            AdminController aCont = new AdminController();
            List<ServiceInfo> Services = aCont.Get_ServicesByPortalId(PortalId);
            foreach(ServiceInfo service in Services)
            {
                ListItem li = new ListItem(service.ServiceName, service.Id.ToString());
                ListItem li2 = new ListItem(service.ServiceName, service.Id.ToString());
                foreach(ServiceInfo chkServ in Group.Services)
                {
                    if(chkServ.Id==service.Id)
                    {
                        li.Selected = true;
                        li2.Selected = true;
                    }
                }
                cklServices.Items.Add(li);
                cklCustomizeServices.Items.Add(li2);
            }
        }
        private void addLibItemControls()
        {
            if(ViewState["libItemIds"]!=null)
            {
                plGroupLibItems.Controls.Clear();
                List<int> libItemIds = (List<int>)ViewState["libItemIds"];
                foreach(int libItemId in libItemIds)
                {
                    Literal lit1 = new Literal();
                    lit1.ID = "litLibItems1_" + libItemId.ToString();
                    plGroupLibItems.Controls.Add(lit1);
                    ImageButton img = new ImageButton();
                    img.ID = "imgLibItems_" + libItemId.ToString();
                    img.Click += new ImageClickEventHandler(removeLibItem_click);
                    plGroupLibItems.Controls.Add(img);
                    Literal lit2 = new Literal();
                    lit2.ID = "litLibItems2_" + libItemId.ToString();
                    plGroupLibItems.Controls.Add(lit2);
                }
            }
            else
            { drawLibItems(); }
        }
        private void addStationControls()
        {
            if (ViewState["stationIds"] != null)
            {
                plGroupLibItems.Controls.Clear();
                List<int> stationIds = (List<int>)ViewState["stationIds"];
                foreach (int stationId in stationIds)
                {
                    Literal lit1 = new Literal();
                    lit1.ID = "litStations1_" + this.ID + "_" + stationId.ToString();
                    plGroupStations.Controls.Add(lit1);
                    DropDownList ddlStationDelivery = new DropDownList();
                    ddlStationDelivery.ID = "ddlStationsDelivery_" + this.ID + "_" + stationId.ToString();
                    ddlStationDelivery.AutoPostBack = true;
                    ddlStationDelivery.SelectedIndexChanged += new EventHandler(ddlStationDelivery_indexChanged);
                    plGroupStations.Controls.Add(ddlStationDelivery);
                    Literal lit1a = new Literal();
                    lit1a.ID = "litStations1a_" + this.ID + "_" + stationId.ToString();
                    plGroupStations.Controls.Add(lit1a);
                    DropDownList ddlStationShipping = new DropDownList();
                    ddlStationShipping.ID = "ddlStationsShipping_" + this.ID + "_" + stationId.ToString();
                    plGroupStations.Controls.Add(ddlStationShipping);
                    Literal lit1a1 = new Literal();
                    lit1a1.ID = "litStations1a1_" + this.ID + "_" + stationId.ToString();
                    plGroupStations.Controls.Add(lit1a1);
                    DropDownList ddlStationQuantity = new DropDownList();
                    ddlStationQuantity.ID = "ddlStationsQuantity_" + this.ID + "_" + stationId.ToString();
                    plGroupStations.Controls.Add(ddlStationQuantity);
                    Literal lit1b = new Literal();
                    lit1b.ID = "litStations1b_" + this.ID + "_" + stationId.ToString();
                    plGroupStations.Controls.Add(lit1b);
                    DropDownList ddlStationPriority = new DropDownList();
                    ddlStationPriority.ID = "ddlStationsPriority_" + this.ID + "_" + stationId.ToString();
                    Literal lit1c = new Literal();
                    lit1c.ID = "litStations1b_" + this.ID + "_" + stationId.ToString();
                    plGroupStations.Controls.Add(lit1c);
                    ImageButton img = new ImageButton();
                    img.ID = "imgStations1_" + this.ID + "_" + stationId.ToString();
                    img.Click += new ImageClickEventHandler(removeStation_click);
                    plGroupStations.Controls.Add(img);
                    Literal lit2 = new Literal();
                    lit2.ID = "litStations2_" + this.ID + "_" + stationId.ToString();
                    plGroupStations.Controls.Add(lit2);
                }
                if(stationIds.Count==0)
                {
                    plStationsHeader.Visible = false;
                }
                else
                {
                    plStationsHeader.Visible = true;
                }
            }
            else
            {
                plStationsHeader.Visible = false;
                drawStations(); 
            }
        }
        private void drawLibItems()
        {
            List<int> liControlIds = new List<int>();
            if (Group != null)
            {
                foreach (LibraryItemInfo libItem in Group.LibraryItems)
                {
                    Literal lit1 = new Literal();
                    lit1.ID = "litLibItems1_" + libItem.Id.ToString();
                    lit1.Text = "<div class=\"mastersRow\">";
                    lit1.Text += "<div class=\"mastersRowCell groupLibItemCol1\">" + (libItem.MasterId != -1 ? libItem.MasterId.ToString() : libItem.PMTMediaId) + "</div>";
                    lit1.Text += "<div class=\"mastersRowCell groupLibItemCol2\">" + libItem.ISCICode + "</div>";
                    lit1.Text += "<div class=\"mastersRowCell groupLibItemCol3\">" + libItem.Title + "</div>";
                    lit1.Text += "<div class=\"mastersRowCell groupLibItemCol4\">" + libItem.ProductDescription + "</div>";
                    lit1.Text += "<div class=\"mastersRowCell groupLibItemCol5\">" + libItem.Encode + "</div>";
                    lit1.Text += "<div class=\"mastersRowCell groupLibItemCol6\">" + libItem.MediaLength + "</div>";
                    lit1.Text += "<div class=\"mastersRowCell groupLibItemCol7\">" + libItem.ClosedCaptioned.ToString() + "</div>";
                    lit1.Text += "<div class=\"mastersRowCell groupLibItemCol8\">";
                    plGroupLibItems.Controls.Add(lit1);
                    ImageButton img = new ImageButton();
                    img.ID = "imgLibItems_" + libItem.Id.ToString();
                    img.ImageUrl = "/desktopmodules/pmt_admin/images/remove.jpg";
                    img.Click += new ImageClickEventHandler(removeLibItem_click);
                    plGroupLibItems.Controls.Add(img);
                    Literal lit2 = new Literal();
                    lit2.ID = "litLibItems2_" + libItem.Id.ToString();
                    lit2.Text = "</div></div>";
                    plGroupLibItems.Controls.Add(lit2);
                    liControlIds.Add(libItem.Id);
                }
                ViewState["libItemIds"] = liControlIds;
            }
        }
        private void drawStations()
        {
            List<int> stationControlIds = new List<int>();
            if (Group != null)
            {                
                AdminController aCont = new AdminController();
                List<MarketInfo> markets = aCont.Get_MarketsByPortalId(PortalId);
                List<AdvertiserInfo> advertisers = aCont.Get_AdvertisersByPortalId(PortalId);
                List<CarrierTypeInfo> carriers = aCont.Get_CarrierTypesByPortalId(PortalId);
                List<DeliveryMethodInfo> dms = aCont.Get_DeliveryMethodsByPortalId(PortalId);
                List<TapeFormatInfo> tfs = aCont.Get_TapeFormatsByPortalId(PortalId);
                List<WOGroupStationInfo> tempStations = new List<WOGroupStationInfo>();
                
                foreach (WOGroupStationInfo station in Group.WOGroupStations)
                {
                    WOGroupStationInfo tempStation = station;
                    var market = markets.FirstOrDefault(i => i.Id == station.Station.MarketId);
                    string marketname = "";
                    if(market!=null)
                    {
                        marketname = market.MarketName;
                    }
                    Literal lit1 = new Literal();
                    lit1.ID = "litStations1_" + this.ID + "_" + station.StationId.ToString();
                    lit1.Text = "<div class=\"mastersRow\">";
                    lit1.Text += "<div class=\"mastersRowCell groupStationCol1\">" + station.Station.CallLetter + "</div>";
                    lit1.Text += "<div class=\"mastersRowCell groupStationCol2\">" + marketname + "</div>";
                    lit1.Text += "<div class=\"mastersRowCell groupStationCol3\">" + station.Station.City + "</div>";
                    lit1.Text += "<div class=\"mastersRowCell groupStationCol4\">";
                    plGroupStations.Controls.Add(lit1);

                    DropDownList ddlStationDelivery = new DropDownList();
                    ddlStationDelivery.ID = "ddlStationsDelivery_" + this.ID + "_" + station.StationId.ToString();
                    ddlStationDelivery.CssClass = "woStationDelivery";
                    string[] deliveryMethods = station.Station.DeliveryMethods.Split(',');
                    string[] tapeFormats = station.Station.TapeFormat.Split(',');
                    foreach (DeliveryMethodInfo dm in dms)
                    {
                        foreach (string dmi in deliveryMethods)
                        {
                            if (dm.Id.ToString() == dmi)
                            {
                                ddlStationDelivery.Items.Add(new ListItem(dm.DeliveryMethod, "dm_" + dmi));
                            }
                        }
                    }
                    foreach (TapeFormatInfo tf in tfs)
                    {
                        foreach (string tfi in tapeFormats)
                        {
                            if (tf.Id.ToString() == tfi)
                            {
                                ddlStationDelivery.Items.Add(new ListItem(tf.TapeFormat, "tf_" + tfi));
                            }
                        }
                    }
                    if (ddlStationDelivery.Items.Count == 0)
                    {
                        ddlStationDelivery.Items.Add(new ListItem("No Delivery Method Set", "-1"));
                    }
                    try
                    {
                        ddlStationDelivery.SelectedValue = station.DeliveryMethod;
                    }
                    catch { }
                    tempStation.DeliveryMethod = ddlStationDelivery.SelectedValue;
                    ddlStationDelivery.AutoPostBack = true;
                    ddlStationDelivery.SelectedIndexChanged += new EventHandler(ddlStationDelivery_indexChanged);
                    plGroupStations.Controls.Add(ddlStationDelivery);

                    Literal lit1a1 = new Literal();
                    lit1a1.ID = "litStations1a1_" + this.ID + "_" + station.Station.Id.ToString();
                    lit1a1.Text = "</div>";
                    lit1a1.Text += "<div class=\"mastersRowCell groupStationCol5a\">";
                    plGroupStations.Controls.Add(lit1a1);

                    DropDownList ddlStationQuantity = new DropDownList();
                    ddlStationQuantity.ID = "ddlStationsQuantity_" + this.ID + "_" + station.Station.Id.ToString();
                    ddlStationQuantity.CssClass = "woStationShipping";
                    for(int i=1; i<=10;i++)
                    {
                        ddlStationQuantity.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                    try { ddlStationQuantity.SelectedValue = station.Quantity.ToString(); }
                    catch { }
                    if (ddlStationDelivery.SelectedValue.IndexOf("tf_") != -1)
                    {
                        ddlStationQuantity.Visible = true;
                    }
                    else
                    {
                        ddlStationQuantity.Visible = false;
                    }
                    ddlStationQuantity.AutoPostBack = true;
                    ddlStationQuantity.SelectedIndexChanged += new EventHandler(ddlStationQuantity_indexChanged);
                    plGroupStations.Controls.Add(ddlStationQuantity);

                    Literal lit1a = new Literal();
                    lit1a.ID = "litStations1a_" + this.ID + "_" + station.Station.Id.ToString();
                    lit1a.Text = "</div>";
                    lit1a.Text += "<div class=\"mastersRowCell groupStationCol5\">";
                    plGroupStations.Controls.Add(lit1a);

                    DropDownList ddlStationShipping = new DropDownList();
                    ddlStationShipping.ID = "ddlStationsShipping_" + this.ID + "_" + station.Station.Id.ToString();
                    ddlStationShipping.CssClass = "woStationShipping";
                    ddlStationShipping.Items.Add(new ListItem("--Carrier--", ""));
                    ddlStationShipping.Items.Add(new ListItem("PMT Fedex", "-1"));
                    if (Group.LibraryItems.Count > 0)
                    {
                        try
                        {
                            var advertiser = advertisers.FirstOrDefault(i => i.Id == Group.LibraryItems[0].AdvertiserId);
                            var carrierType = carriers.FirstOrDefault(i => i.Id == advertiser.Carrier);
                            ddlStationShipping.Items.Add(new ListItem(carrierType.CarrierType, carrierType.Id.ToString()));
                        }
                        catch { }
                    }
                    try
                    {
                        ddlStationShipping.SelectedValue = station.ShippingMethodId.ToString();
                    }
                    catch { }
                    //if(station.ShippingMethod!="")
                    //{
                    //    ddlStationShipping.SelectedValue = station.ShippingMethod;
                    //}
                    if(ddlStationDelivery.SelectedValue.IndexOf("tf_")!=-1)
                    {
                        ddlStationShipping.Visible = true;
                    }
                    else
                    {
                        ddlStationShipping.Visible = false;
                    }
                    try
                    {
                        tempStation.ShippingMethodId = Convert.ToInt32(ddlStationShipping.SelectedValue);
                    }
                    catch { }
                    ddlStationShipping.AutoPostBack = true;
                    ddlStationShipping.SelectedIndexChanged += new EventHandler(ddlStationShipping_indexChanged);
                    plGroupStations.Controls.Add(ddlStationShipping);

                    Literal lit1b = new Literal();
                    lit1b.ID = "litStations1b_" + this.ID + "_" + station.Station.Id.ToString();
                    lit1b.Text = "</div>";
                    lit1b.Text += "<div class=\"mastersRowCell groupStationCol6\">";
                    plGroupStations.Controls.Add(lit1b);

                    DropDownList ddlStationPriority = new DropDownList();
                    ddlStationPriority.ID = "ddlStationsPriority_" + this.ID + "_" + station.Station.Id.ToString();
                    if (Group.LibraryItems.Count > 0)
                    {
                        try
                        {
                            var advertiser = advertisers.FirstOrDefault(i => i.Id == Group.LibraryItems[0].AdvertiserId);
                            //var carrierType = carriers.FirstOrDefault(i => i.Id == advertiser.Carrier);
                            if (ddlStationShipping.SelectedValue == "1" || ddlStationShipping.SelectedValue == "-1")
                            {
                                ddlStationPriority.Items.Add(new ListItem("--Service--", "-1"));
                                ddlStationPriority.Items.Add(new ListItem("Priority", "1"));
                                ddlStationPriority.Items.Add(new ListItem("Standard", "2"));
                                ddlStationPriority.Items.Add(new ListItem("2-Day", "3"));
                            }
                            else if (ddlStationShipping.SelectedValue == "2")
                            {
                                ddlStationPriority.Items.Add(new ListItem("--Service--", "-1"));
                                ddlStationPriority.Items.Add(new ListItem("Priority", "1"));
                                ddlStationPriority.Items.Add(new ListItem("Standard", "2"));
                                ddlStationPriority.Items.Add(new ListItem("2-Day", "3"));
                                ddlStationPriority.Items.Add(new ListItem("2-Day AM", "4"));
                                ddlStationPriority.Items.Add(new ListItem("3-Day", "5"));
                            }
                        }
                        catch { }
                        try { ddlStationPriority.SelectedValue = station.PriorityId.ToString(); }
                        catch { }
                    }
                    ddlStationPriority.CssClass = "woStationPriority";
                    
                    if(ddlStationDelivery.SelectedValue.IndexOf("tf_")!=-1)
                    {
                        ddlStationPriority.Visible = true;
                        try
                        {
                            tempStation.PriorityId = Convert.ToInt32(ddlStationPriority.SelectedValue);
                        }
                        catch { }
                    }
                    else
                    {
                        ddlStationPriority.Visible = false;
                    }
                    ddlStationPriority.AutoPostBack = true;
                    ddlStationPriority.SelectedIndexChanged += new EventHandler(ddlStationPriority_indexChanged);
                    plGroupStations.Controls.Add(ddlStationPriority);
                    

                    Literal lit1c = new Literal();
                    lit1c.ID = "litStations1c_" + this.ID + "_" + station.Station.Id.ToString();
                    lit1c.Text = "</div>";
                    lit1c.Text += "<div class=\"mastersRowCell groupLibItemCol7\">";
                    plGroupStations.Controls.Add(lit1c);

                    ImageButton img = new ImageButton();
                    img.ID = "imgStations_" + this.ID + "_" + station.Station.Id.ToString();
                    img.ImageUrl = "/desktopmodules/pmt_admin/images/remove.jpg";
                    img.Click += new ImageClickEventHandler(removeStation_click);
                    plGroupStations.Controls.Add(img);
                    Literal lit2 = new Literal();
                    lit2.ID = "litStations2_" + this.ID + "_" + station.Station.Id.ToString();
                    lit2.Text = "</div></div>";
                    plGroupStations.Controls.Add(lit2);
                    stationControlIds.Add(station.Station.Id);
                    tempStations.Add(tempStation);
                }
                if (Group.WOGroupStations.Count == 0)
                {
                    plStationsHeader.Visible = false;
                }
                else
                {
                    plStationsHeader.Visible = true;
                }
                ViewState["stationIds"] = stationControlIds;
                //update stations for default case
                Group.WOGroupStations = tempStations;
            }
        }
        protected void ddlStationPriority_indexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            int thisId = -1;
            try
            {
                thisId = Convert.ToInt32(ddl.ID.Replace("ddlStationsPriority_" + this.ID + "_", ""));
            }
            catch { }
            if (thisId != -1)
            {
                //update wostation
                WOGroupInfo newGroup = Group;
                List<WOGroupStationInfo> newStats = new List<WOGroupStationInfo>();
                foreach (WOGroupStationInfo stat in Group.WOGroupStations)
                {
                    if (stat.StationId == thisId)
                    {
                        stat.PriorityId = Convert.ToInt32(ddl.SelectedValue);
                    }
                    newStats.Add(stat);                    
                }
                newGroup.WOGroupStations = newStats;
                Group = newGroup;
                if (this.GroupUpdated != null)
                {
                    this.GroupUpdated(this, new EventArgs());
                }
            }
        }
        protected void ddlStationQuantity_indexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            int thisId = -1;
            try
            {
                thisId = Convert.ToInt32(ddl.ID.Replace("ddlStationsQuantity_" + this.ID + "_", ""));
            }
            catch { }
            if (thisId != -1)
            {
                //update wostation
                WOGroupInfo newGroup = Group;
                List<WOGroupStationInfo> newStats = new List<WOGroupStationInfo>();
                foreach (WOGroupStationInfo stat in Group.WOGroupStations)
                {
                    if (stat.StationId == thisId)
                    {
                        stat.Quantity = Convert.ToInt32(ddl.SelectedValue);
                        newStats.Add(stat);
                    }
                    else
                    {
                        newStats.Add(stat);
                    }
                }
                newGroup.WOGroupStations = newStats;
                Group = newGroup;
                if (this.GroupUpdated != null)
                {
                    this.GroupUpdated(this, new EventArgs());
                }
            }
        }
        protected void ddlStationShipping_indexChanged(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            DropDownList ddl = (DropDownList)sender;
            int thisId = -1;
            try
            {
                thisId = Convert.ToInt32(ddl.ID.Replace("ddlStationsShipping_" + this.ID + "_", ""));
            }
            catch { }
            if (thisId != -1)
            {
                if (ddl != null)
                {
                    WOGroupStationInfo station = aCont.Get_WorkOrderGroupStationById(thisId);
                    DropDownList ddl2 = (DropDownList)plGroupStations.FindControl("ddlStationsPriority_" + this.ID + "_" + thisId.ToString());
                    if (ddl2 != null)
                    {
                        ddl2.Items.Clear();
                        if (ddl.SelectedValue == "1" || ddl.SelectedValue == "-1")
                        {
                            ddl2.Items.Add(new ListItem("--Service--", "-1"));
                            ddl2.Items.Add(new ListItem("Priority", "1"));
                            ddl2.Items.Add(new ListItem("Standard", "2"));
                            ddl2.Items.Add(new ListItem("2-Day", "3"));
                        }
                        else if (ddl.SelectedValue == "2")
                        {
                            ddl2.Items.Add(new ListItem("--Service--", "-1"));
                            ddl2.Items.Add(new ListItem("Priority", "1"));
                            ddl2.Items.Add(new ListItem("Standard", "2"));
                            ddl2.Items.Add(new ListItem("2-Day", "3"));
                            ddl2.Items.Add(new ListItem("2-Day AM", "4"));
                            ddl2.Items.Add(new ListItem("3-Day", "5"));
                        }
                        ddl2.CssClass = "woStationPriority";
                        if (station.PriorityId != -2)
                        {
                            try
                            {
                                ddl2.SelectedValue = station.PriorityId.ToString();
                            }
                            catch { }
                        }
                        ddl2.AutoPostBack = true;
                        ddl2.SelectedIndexChanged += new EventHandler(ddlStationPriority_indexChanged);
                    }
                }
                //update wostation
                WOGroupInfo newGroup = Group;
                List<WOGroupStationInfo> newStats = new List<WOGroupStationInfo>();
                foreach (WOGroupStationInfo stat in Group.WOGroupStations)
                {
                    if (stat.StationId == thisId)
                    {
                        try
                        {
                            stat.ShippingMethodId = Convert.ToInt32(ddl.SelectedValue);
                        }
                        catch { }
                        newStats.Add(stat);
                    }
                    else
                    {
                        newStats.Add(stat);
                    }
                }
                newGroup.WOGroupStations = newStats;
                Group = newGroup;
                if (this.GroupUpdated != null)
                {
                    this.GroupUpdated(this, new EventArgs());
                }
            }
        }
        protected void ddlStationDelivery_indexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            int thisId = -1;
            try
            {
                thisId = Convert.ToInt32(ddl.ID.Replace("ddlStationsDelivery_" + this.ID + "_", ""));
            }
            catch { }
            if(thisId!=-1)
            {
                DropDownList ddl2 = (DropDownList)plGroupStations.FindControl("ddlStationsShipping_" + this.ID + "_" + thisId.ToString());
                DropDownList ddl3 = (DropDownList)plGroupStations.FindControl("ddlStationsPriority_" + this.ID + "_" + thisId.ToString());
                DropDownList ddl4 = (DropDownList)plGroupStations.FindControl("ddlStationsQuantity_" + this.ID + "_" + thisId.ToString());
                if(ddl2!=null && ddl.SelectedValue.IndexOf("tf_")!=-1)
                {
                    ddl2.Visible = true;
                    ddl3.Visible = true;
                    ddl4.Visible = true;
                }
                else if (ddl2!=null && ddl.SelectedValue.IndexOf("tf_")==-1)
                {
                    ddl2.Visible = false;
                    ddl3.Visible = false;
                    ddl4.Visible = false;
                }
                //update wostation
                WOGroupInfo newGroup = Group;
                List<WOGroupStationInfo> newStats = new List<WOGroupStationInfo>();
                foreach(WOGroupStationInfo stat in Group.WOGroupStations)
                {
                    if(stat.StationId == thisId)
                    {
                        stat.DeliveryMethod = ddl.SelectedValue;
                        try
                        {
                            stat.ShippingMethodId = Convert.ToInt32(ddl2.Items[0].Value);
                            stat.PriorityId = Convert.ToInt32(ddl3.Items[0].Value);
                            stat.Quantity = Convert.ToInt32(ddl4.Items[0].Value);
                        }
                        catch { }
                        newStats.Add(stat);
                    }
                    else
                    {
                        newStats.Add(stat);
                    }
                }
                newGroup.WOGroupStations = newStats;
                Group = newGroup;
                if (this.GroupUpdated != null)
                {
                    this.GroupUpdated(this, new EventArgs());
                }
            }
        }
        protected void removeLibItem_click(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            int remId = Convert.ToInt32(btn.ID.Replace("imgLibItems_", ""));
            var libItem = Group.LibraryItems.FirstOrDefault(i => i.Id == remId);
            Group.LibraryItems.Remove(libItem);
            if (this.GroupUpdated != null)
            {
                this.GroupUpdated(this, new EventArgs());
            }
        }
        protected void removeStation_click(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            int remId = Convert.ToInt32(btn.ID.Replace("imgStations_" + this.ID + "_", ""));
            var station = Group.WOGroupStations.FirstOrDefault(i => i.StationId == remId);
            Group.WOGroupStations.Remove(station);
            if (this.GroupUpdated != null)
            {
                this.GroupUpdated(this, new EventArgs());
            }
        }

        protected void txtDeliverGroupName_TextChanged(object sender, EventArgs e)
        {
            WOGroupInfo group = Group;
            group.GroupName = txtDeliverGroupName.Text;
            Group = group;
            if (this.GroupUpdated != null)
            {
                this.GroupUpdated(this, new EventArgs());
            }
        }

        protected void txtGroupNotes_TextChanged(object sender, EventArgs e)
        {
            WOGroupInfo group = Group;
            group.Comments = txtGroupNotes.Text;
            Group = group;
            if (this.GroupUpdated != null)
            {
                this.GroupUpdated(this, new EventArgs());
            }
        }

        protected void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            if (this.GroupDeleted != null)
            {
                this.GroupDeleted(this, new EventArgs());
            }
        }

        protected void txtNonDeliverGroupName_TextChanged(object sender, EventArgs e)
        {
            WOGroupInfo group = Group;
            group.GroupName = txtNonDeliverGroupName.Text;
            Group = group;
            if (this.GroupUpdated != null)
            {
                this.GroupUpdated(this, new EventArgs());
            }
        }

        protected void txtNonDeliverableNotes_TextChanged(object sender, EventArgs e)
        {
            WOGroupInfo group = Group;
            group.Comments = txtNonDeliverableNotes.Text;
            Group = group;
            if (this.GroupUpdated != null)
            {
                this.GroupUpdated(this, new EventArgs());
            }
        }

        protected void cklServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            WOGroupInfo group = Group;
            AdminController aCont = new AdminController();
            List<ServiceInfo> services = aCont.Get_ServicesByPortalId(PortalId);
            group.Services.Clear();
            foreach(ListItem li in cklServices.Items)
            {
                if(li.Selected)
                {
                    var service = services.FirstOrDefault(i => i.Id == Convert.ToInt32(li.Value));
                    group.Services.Add(service);
                }
            }
            Group = group;
            if (this.GroupUpdated != null)
            {
                this.GroupUpdated(this, new EventArgs());
            }
        }

        protected void btnToggleServices_Click(object sender, EventArgs e)
        {
            cklCustomizeServices.Visible = !cklCustomizeServices.Visible;
            ViewState["ServicesVisible"] = cklCustomizeServices.Visible;
        }

        protected void cklCustomizeServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            WOGroupInfo group = Group;
            AdminController aCont = new AdminController();
            List<ServiceInfo> services = aCont.Get_ServicesByPortalId(PortalId);
            group.Services.Clear();
            foreach (ListItem li in cklCustomizeServices.Items)
            {
                if (li.Selected)
                {
                    var service = services.FirstOrDefault(i => i.Id == Convert.ToInt32(li.Value));
                    group.Services.Add(service);
                }
            }
            Group = group;
            if (this.GroupUpdated != null)
            {
                this.GroupUpdated(this, new EventArgs());
            }
        }
    }
}