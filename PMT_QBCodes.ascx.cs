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
    public partial class PMT_QBCodes : PMT_AdminModuleBase, IActionable
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            fillServices();
            fillDeliveryMethods();
            fillTapeFormats();
            fillQBCodes();
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
        private void fillQBCodes()
        {
            string direction = "ASC";
            string column = "Code";
            if (ViewState["direction"] != null)
            {
                direction = ViewState["direction"].ToString();
            }
            if (ViewState["sort"] != null)
            {
                column = ViewState["sort"].ToString();
            }
            AdminController aCont = new AdminController();
            List<QBCodeInfo> qbcodes = aCont.Get_QBCodesByPortalId(PortalId);
            List<QBCodeInfo> qbcodesByType = new List<QBCodeInfo>();
            qbcodes = SortOrders(qbcodes, column, direction);
            List<QBCodeInfo> qbcodesFiltered = new List<QBCodeInfo>();
            if(ddlTypeSearch.SelectedIndex>0)
            {
                foreach(QBCodeInfo qbcode in qbcodes)
                {
                    if(qbcode.Type.ToString()==ddlTypeSearch.SelectedValue)
                    {
                        qbcodesByType.Add(qbcode);
                    }
                }
            }
            else
            {
                qbcodesByType = qbcodes;
            }
            if(txtQBCodeSearch.Text!="")
            {
                foreach(QBCodeInfo qbcode in qbcodesByType)
                {
                    if(qbcode.QBCode.ToLower().IndexOf(txtQBCodeSearch.Text.ToLower())!=-1)
                    {
                        qbcodesFiltered.Add(qbcode);
                    }
                }
            }
            else
            {
                qbcodesFiltered = qbcodesByType;
            }
            lblQBCodesMessage.Text = qbcodesFiltered.Count.ToString() + " QuickBooks codes found.";
            gvQBCodes.DataSource = qbcodesFiltered;
            if (ViewState["QBCodesPage"] != null)
            {
                gvQBCodes.PageIndex = Convert.ToInt32(ViewState["QBCodesPage"]);
            }
            gvQBCodes.DataBind();
        }
        protected void txtQBCodeSearch_TextChanged(object sender, EventArgs e)
        {
            fillQBCodes();
        }

        protected void gvQBCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            txtSelectedQBCode.Value = (gvQBCodes.SelectedRow.FindControl("hdngvQBCodeId") as HiddenField).Value;
            QBCodeInfo QBCode = aCont.Get_QBCodeById(Convert.ToInt32(txtSelectedQBCode.Value));
            if (QBCode.Id != -1)
            {
                btnDeleteQBCode.Enabled = true;
                btnSaveQBCodeAs.Enabled = true;
                txtQBCodeCreatedBy.Value = QBCode.CreatedById.ToString();
                txtQBCodeCreatedDate.Value = QBCode.DateCreated.Ticks.ToString();
                txtQBCode.Text = QBCode.QBCode;
                ddlQBCodeType.ClearSelection();
                ddlMediaType.ClearSelection();
                lbxStationDeliveryMethod.ClearSelection();
                lbxQBCodeTapeFormat.ClearSelection();
                foreach(ListItem li in ddlQBCodeType.Items)
                {
                    if(Convert.ToInt32(li.Value)==(int)QBCode.Type)
                    {
                        li.Selected = true;
                    }
                }
                if (QBCode.Type == GroupTypeEnum.Bundle || QBCode.Type == GroupTypeEnum.Delivery)
                {
                    pnlDelivery.Visible = true;
                    pnlNonDelivery.Visible = false;
                    foreach(ListItem li in ddlMediaType.Items)
                    {
                        if(Convert.ToInt32(li.Value)==(int)QBCode.MediaType)
                        {
                            li.Selected = true;
                        }
                    }
                    foreach(DeliveryMethodInfo dm in QBCode.DeliveryMethods)
                    {
                        foreach(ListItem li in lbxStationDeliveryMethod.Items)
                        {
                            if(li.Value==dm.Id.ToString())
                            {
                                li.Selected = true;
                            }
                        }
                    }
                    foreach(TapeFormatInfo tf in QBCode.TapeFormats)
                    {
                        foreach(ListItem li in lbxQBCodeTapeFormat.Items)
                        {
                            if(li.Value==tf.Id.ToString())
                            {
                                li.Selected = true;
                            }
                        }
                    }
                    txtMaxLength.Text = QBCode.MaxLength;
                    txtMinLength.Text = QBCode.MinLength;
                    txtQBCode.Text = QBCode.QBCode;
                }
                else if (QBCode.Type  == GroupTypeEnum.Non_Deliverable || QBCode.Type==GroupTypeEnum.Customized)
                {
                    pnlDelivery.Visible = false;
                    pnlNonDelivery.Visible = true;
                    cklServices.ClearSelection();
                    foreach(ServiceInfo serv in QBCode.Services)
                    {
                        foreach(ListItem li in cklServices.Items)
                        {
                            if (li.Value == serv.Id.ToString())
                            {
                                li.Selected = true;
                            }
                        }
                    }
                }
                else
                {
                    pnlDelivery.Visible = false;
                    pnlNonDelivery.Visible = false;
                    clearQBCOde();
                }
                lblQBCodesMessage.Text = "";
            }
            else
            {
                btnDeleteQBCode.Enabled = false;
                btnSaveQBCodeAs.Enabled = false;
                clearQBCOde();
                lblQBCodesMessage.Text = "There was an error loading this QuickBooks Code.";
            }
            fillQBCodes();
        }
        private void clearQBCOde()
        {
            txtMaxLength.Text = "";
            txtMinLength.Text = "";
            txtQBCode.Text = "";
            txtQBCodeCreatedBy.Value = "-1";
            txtQBCodeCreatedDate.Value = "";
            txtSelectedQBCode.Value = "-1";
            lbxQBCodeTapeFormat.ClearSelection();
            lbxStationDeliveryMethod.ClearSelection();
            cklServices.ClearSelection();
        }

        protected void gvQBCodes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["QBCodesPage"] = e.NewPageIndex.ToString();
            fillQBCodes();
        }

        protected void btnSaveQBCode_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            QBCodeInfo QBCode = new QBCodeInfo();
            if(txtSelectedQBCode.Value != "-1")
            {
                QBCode = aCont.Get_QBCodeById(Convert.ToInt32(txtSelectedQBCode.Value));
            }
            QBCode.PortalId = PortalId;
            QBCode.LastModifiedById = UserId;
            QBCode.LastModifiedDate = DateTime.Now;
            QBCode.QBCode = txtQBCode.Text;
            QBCode.Type = (GroupTypeEnum)Convert.ToInt32(ddlQBCodeType.SelectedValue);
            QBCode.MediaType = (MediaTypeEnum)Convert.ToInt32(ddlMediaType.SelectedValue);
            if (ddlQBCodeType.SelectedValue == "1" || ddlQBCodeType.SelectedValue == "2" || ddlQBCodeType.SelectedValue == "0" || QBCode.Type==GroupTypeEnum.Bundle || QBCode.Type == GroupTypeEnum.Customized || QBCode.Type == GroupTypeEnum.Delivery)
            {
                QBCode.MinLength = aCont.fixLength(txtMinLength.Text);
                QBCode.MaxLength = aCont.fixLength(txtMaxLength.Text);
            }
            if (txtSelectedQBCode.Value == "-1")
            {
                //save new code
                QBCode.CreatedById = UserId;
                QBCode.DateCreated = DateTime.Now;
                QBCode.Id = aCont.Add_QBCode(QBCode);
            }
            else
            {
                //update existing code
                QBCode.CreatedById = Convert.ToInt32(txtQBCodeCreatedBy.Value); ;
                QBCode.DateCreated = new DateTime(Convert.ToInt64(txtQBCodeCreatedDate.Value));
                QBCode.Id = Convert.ToInt32(txtSelectedQBCode.Value);
                aCont.Update_QBCode(QBCode);
            }
            //save delivery methods, tape formats and services
            if (ddlQBCodeType.SelectedValue == "1" || ddlQBCodeType.SelectedValue == "0")
            {
                aCont.DeleteQBCodeDeliveryMethodsByQBCodeId(QBCode.Id);
                foreach(ListItem li in lbxStationDeliveryMethod.Items)
                {
                    if(li.Selected)
                    {
                        aCont.AddQBCodeDeliveryMethod(QBCode.Id, Convert.ToInt32(li.Value));
                    }
                }
                aCont.DeleteQBCodeTapeFormatsByQBCodeId(QBCode.Id);
                foreach(ListItem li in lbxQBCodeTapeFormat.Items)
                {
                    if(li.Selected)
                    {
                        aCont.AddQBCodeTapeFormat(QBCode.Id, Convert.ToInt32(li.Value));
                    }
                }
            }
            if (ddlQBCodeType.SelectedValue == "3" || ddlQBCodeType.SelectedValue == "2" || ddlQBCodeType.SelectedValue == "1")
            {
                aCont.DeleteQBCodeServicesByQBCodeId(QBCode.Id);
                foreach(ListItem li in cklServices.Items)
                {
                    if(li.Selected)
                    {
                        aCont.AddQBCodeService(QBCode.Id, Convert.ToInt32(li.Value));
                    }
                }
            }
            lblQBCodesMessage.Text = "QuickBooks Code Saved.";
            fillQBCodes();
        }

        protected void btnSaveQBCodeAs_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            QBCodeInfo QBCode = new QBCodeInfo();
            QBCode.PortalId = PortalId;
            QBCode.LastModifiedById = UserId;
            QBCode.LastModifiedDate = DateTime.Now;
            QBCode.CreatedById = UserId;
            QBCode.DateCreated = DateTime.Now;
            QBCode.QBCode = txtQBCode.Text;
            QBCode.Type = (GroupTypeEnum)Convert.ToInt32(ddlQBCodeType.SelectedValue);
            QBCode.MediaType = (MediaTypeEnum)Convert.ToInt32(ddlMediaType.SelectedValue);
            if (ddlQBCodeType.SelectedValue == "1" || ddlQBCodeType.SelectedValue == "2" || ddlQBCodeType.SelectedValue == "0")
            {
                QBCode.MinLength = aCont.fixLength(txtMinLength.Text);
                QBCode.MaxLength = aCont.fixLength(txtMaxLength.Text);
            }
                //save new code
                QBCode.Id = aCont.Add_QBCode(QBCode);

            //save delivery methods, tape formats and services
            if (ddlQBCodeType.SelectedValue == "1" || ddlQBCodeType.SelectedValue == "0")
            {
                aCont.DeleteQBCodeDeliveryMethodsByQBCodeId(QBCode.Id);
                foreach (ListItem li in lbxStationDeliveryMethod.Items)
                {
                    if (li.Selected)
                    {
                        aCont.AddQBCodeDeliveryMethod(QBCode.Id, Convert.ToInt32(li.Value));
                    }
                }
                aCont.DeleteQBCodeTapeFormatsByQBCodeId(QBCode.Id);
                foreach (ListItem li in lbxQBCodeTapeFormat.Items)
                {
                    if (li.Selected)
                    {
                        aCont.AddQBCodeTapeFormat(QBCode.Id, Convert.ToInt32(li.Value));
                    }
                }
            }
            if (ddlQBCodeType.SelectedValue == "3" || ddlQBCodeType.SelectedValue == "2" || ddlQBCodeType.SelectedValue == "1")
            {
                aCont.DeleteQBCodeServicesByQBCodeId(QBCode.Id);
                foreach (ListItem li in cklServices.Items)
                {
                    if (li.Selected)
                    {
                        aCont.AddQBCodeService(QBCode.Id, Convert.ToInt32(li.Value));
                    }
                }
            }
            lblQBCodesMessage.Text = "QuickBooks Code Saved.";
            fillQBCodes();
        }

        protected void btnDeleteQBCode_Click(object sender, EventArgs e)
        {
            AdminController aCont = new AdminController();
            QBCodeInfo QBCode = new QBCodeInfo();
            QBCode.Id = Convert.ToInt32(txtSelectedQBCode.Value);
            aCont.Delete_QBCode(QBCode);
            lblQBCodesMessage.Text = "QuickBooks Code Deleted.";
            fillQBCodes();
            clearQBCOde();
        }

        protected void btnClearQBCode_Click(object sender, EventArgs e)
        {
            clearQBCOde();
            txtQBCodeCreatedBy.Value = "-1";
            txtQBCodeCreatedDate.Value = "";
            txtSelectedQBCode.Value = "-1";
        }

        protected void ddlQBCodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlQBCodeType.SelectedValue=="0")
            {
                pnlDelivery.Visible = true;
                pnlNonDelivery.Visible = false;
            }
            else if(ddlQBCodeType.SelectedValue=="2" || ddlQBCodeType.SelectedValue=="3")
            {
                pnlDelivery.Visible = false;
                pnlNonDelivery.Visible = true;
            }
            else
            {
                pnlDelivery.Visible = true;
                pnlNonDelivery.Visible = true;
            }
        }
        private void fillServices()
        {
            AdminController aCont = new AdminController();
            List<ServiceInfo> Services = aCont.Get_ServicesByPortalId(PortalId);
            cklServices.Items.Clear();
            foreach (ServiceInfo service in Services)
            {
                ListItem li = new ListItem(service.ServiceName, service.Id.ToString());
                cklServices.Items.Add(li);
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
            lbxQBCodeTapeFormat.Items.Clear();
            AdminController aCont = new AdminController();
            List<TapeFormatInfo> TapeFormats = aCont.Get_TapeFormatsByPortalId(PortalId);
            foreach (TapeFormatInfo TapeFormat in TapeFormats)
            {
                lbxQBCodeTapeFormat.Items.Add(new ListItem(TapeFormat.TapeFormat, TapeFormat.Id.ToString()));
            }
        }
        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }
        public static List<QBCodeInfo> SortOrders(List<QBCodeInfo> qbCodes, string sort, string direction)
        {
            List<QBCodeInfo> SortedList = qbCodes;
            if (direction == "ASC")
            {
                switch (sort)
                {
                    case "Type":
                        SortedList = qbCodes.OrderBy(o => o.Type).ToList();
                        break;
                    case "Code":
                        SortedList = qbCodes.OrderBy(o => o.QBCode).ToList();
                        break;
                    default:
                        break;
                }

            }
            else
            {
                switch (sort)
                {
                    case "Type":
                        SortedList = qbCodes.OrderByDescending(o => o.Type).ToList();
                        break;
                    case "Code":
                        SortedList = qbCodes.OrderByDescending(o => o.QBCode).ToList();
                        break;
                    default:
                        break;
                }
            }
            return SortedList;
        }

        protected void gvQBCodes_Sorting(object sender, GridViewSortEventArgs e)
        {
            string direction = GetSortDirection(e.SortExpression);
            string sort = e.SortExpression;
            ViewState["direction"] = direction;
            ViewState["sort"] = sort;
            fillQBCodes();
        }

        protected void ddlTypeSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillQBCodes();
        }
    }
}