using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Xml;
using System.Text.RegularExpressions;
using System.Net;
using DotNetNuke.Services.Exceptions;

namespace Christoc.Modules.PMT_Admin
{
    /// <summary>
    /// Summary description for QBConnect
    /// </summary>
    [WebService(
     Namespace = "http://developer.intuit.com/",
     Name = "WCECommService",
     Description = "Sample WebService in ASP.NET to demonstrate " +
                      "QuickBooks WebConnector")]

    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class QBConnect : System.Web.Services.WebService
    {
        public AdminController aCont = new AdminController();

        [WebMethod]
        public string[] authenticate(string strUserName, string strPassword)
        {
            //try
            //{
            //    Session.Remove("req");                
            //}
            //catch { }
            //try
            //{
            //    Session.Remove("counter");
            //}
            //catch { }
            string evLogTxt = "WebMethod: authenticate() has been called by QBWebconnector" + Environment.NewLine;
            evLogTxt = evLogTxt + "Parameters received:" + Environment.NewLine;
            evLogTxt = evLogTxt + "string strUserName = " + strUserName + Environment.NewLine;
            evLogTxt = evLogTxt + "string strPassword = " + strPassword + Environment.NewLine;
            string[] authReturn = new string[2];
            // Code below uses a random GUID to use as session ticket
            // An example of a GUID is {85B41BEE-5CD9-427a-A61B-83964F1EB426}
            authReturn[0] = System.Guid.NewGuid().ToString();
            // For simplicity of sample, a hardcoded username/password is used.
            // In real world, you should handle authentication in using a standard way.
            // For example, you could validate the username/password against an LDAP
            // or a directory server
            string username = "PMTQBConnect";//ConfigurationManager.AppSettings["QBUserName"];
            string pwd = "pmtc0nn3ct!";//ConfigurationManager.AppSettings["QBPassword"];
            try
            {
                username = ConfigurationManager.AppSettings["QBUserName"];
                pwd = ConfigurationManager.AppSettings["QBPassword"];
            }
            catch { }
            evLogTxt = evLogTxt + "Password locally stored = " + pwd + Environment.NewLine;
            if (strUserName.ToUpper().Trim().Equals(username.ToUpper()) && strPassword.ToUpper().Trim().Equals(pwd.ToUpper()))
            {
                // An empty string for authReturn[1] means asking QBWebConnector
                // to connect to the company file that is currently openned in QB
                authReturn[1] = "";
            }
            else
            {
                authReturn[1] = "nvu";
            }
            // You could also return "none" to indicate there is no work to do
            // or a company filename in the format C:\full\path o\company.qbw
            // based on your program logic and requirements.
            evLogTxt = evLogTxt + "Return values: " + Environment.NewLine;
            evLogTxt = evLogTxt + "string[] authReturn[0] = " + authReturn[0].ToString() + Environment.NewLine;
            evLogTxt = evLogTxt + "string[] authReturn[1] = " + authReturn[1].ToString() + Environment.NewLine;
            logEvent(evLogTxt);
            return authReturn;
        }

        [WebMethod]
        public string closeConnection(string ticket)
        {
            string evLogTxt = "WebMethod: closeConnection() has been called by QBWebconnector" + Environment.NewLine;
            evLogTxt = evLogTxt + "Parameters received: " + Environment.NewLine;
            evLogTxt = evLogTxt + "string ticket = " + ticket + Environment.NewLine;
            evLogTxt = evLogTxt + Environment.NewLine;
            string retVal = null;
            retVal = "OK";
            evLogTxt = evLogTxt + Environment.NewLine;
            evLogTxt = evLogTxt + "Return values: " + Environment.NewLine;
            evLogTxt = evLogTxt + "string retVal= " + retVal + Environment.NewLine;
            logEvent(evLogTxt);
            return retVal;
        }
        [WebMethod]
        public string connectionError(string ticket, string hresult, string message)
        {
            string evLogTxt = "WebMethod: connectionError() has been called by QBWebconnector" + Environment.NewLine;
            evLogTxt = evLogTxt + "Parameters received:" + Environment.NewLine;
            evLogTxt = evLogTxt + "string ticket = " + ticket + Environment.NewLine;
            evLogTxt = evLogTxt + "string hresult = " + hresult + Environment.NewLine;
            evLogTxt = evLogTxt + "string message = " + message + Environment.NewLine;
            evLogTxt = evLogTxt + Environment.NewLine;
            string retVal = null;
            // 0x80040400 - QuickBooks found an error when parsing the provided XML text stream.
            const string QB_ERROR_WHEN_PARSING = "0x80040400";
            // 0x80040401 - Could not access QuickBooks.
            const string QB_COULDNT_ACCESS_QB = "0x80040401";
            // 0x80040402 - Unexpected error. Check the qbsdklog.txt file
            const string QB_UNEXPECTED_ERROR = "0x80040402";
            // Add more as you need...
            if (hresult.Trim().Equals(QB_ERROR_WHEN_PARSING))
            {
                evLogTxt = evLogTxt + "HRESULT = " + hresult + Environment.NewLine;
                evLogTxt = evLogTxt + "Message = " + message + Environment.NewLine;
                retVal = "DONE";
            }
            else if (hresult.Trim().Equals(QB_COULDNT_ACCESS_QB))
            {
                evLogTxt = evLogTxt + "HRESULT = " + hresult + Environment.NewLine;
                evLogTxt = evLogTxt + "Message = " + message + Environment.NewLine;
                retVal = "DONE";
            }
            else if (hresult.Trim().Equals(QB_UNEXPECTED_ERROR))
            {
                evLogTxt = evLogTxt + "HRESULT = " + hresult + Environment.NewLine;
                evLogTxt = evLogTxt + "Message = " + message + Environment.NewLine;
                retVal = "DONE";
            }
            else
            {
                // Depending on various hresults return different value
                // Try again with this company file
                evLogTxt = evLogTxt + "HRESULT = " + hresult + Environment.NewLine;
                evLogTxt = evLogTxt + "Message = " + message + Environment.NewLine;
                retVal = "";
            }
            evLogTxt = evLogTxt + Environment.NewLine;
            evLogTxt = evLogTxt + "Return values: " + Environment.NewLine;
            evLogTxt = evLogTxt + "string retVal = " + retVal + Environment.NewLine;
            logEvent(evLogTxt);
            return retVal;
        }
        [WebMethod]
        public string getLastError(string ticket)
        {
            string evLogTxt = "WebMethod: getLastError() has been called by QBWebconnector" + Environment.NewLine;
            evLogTxt = evLogTxt + "Parameters received:" + Environment.NewLine;
            evLogTxt = evLogTxt + "string ticket = " + ticket + Environment.NewLine;
            int errorCode = 0;
            string retVal = null;
            if (errorCode == -101)
            {
                retVal = "QuickBooks was not running!"; // just an example of custom user errors
            }
            else
            {
                retVal = "";
            }
            evLogTxt = evLogTxt + Environment.NewLine;
            evLogTxt = evLogTxt + "Return values: " + Environment.NewLine;
            evLogTxt = evLogTxt + "string retVal= " + retVal + Environment.NewLine;
            logEvent(evLogTxt);
            return retVal;
        }
        public string getServerVersion(string ticket)
        {
            return "1.0";
        }
        [WebMethod(Description = "response XML from QuickBooks", EnableSession = true)]
        public int receiveResponseXML(string ticket, string response, string hresult, string message)
        {
            string evLogTxt = "WebMethod: receiveResponseXML() called by QBWebconnector" + Environment.NewLine;
            evLogTxt = evLogTxt + "Parameters received:" + Environment.NewLine;
            evLogTxt = evLogTxt + "string ticket = " + ticket + Environment.NewLine;
            evLogTxt = evLogTxt + "string response = " + response + Environment.NewLine;
            evLogTxt = evLogTxt + "string hresult = " + hresult + Environment.NewLine;
            evLogTxt = evLogTxt + "string message = " + message + Environment.NewLine;
            int retVal = 0;
            //if (!hresult.ToString().Equals(""))
            //{
                // if error in the response, web service should return a negative int
                evLogTxt += "HRESULT = " + hresult + Environment.NewLine;
                evLogTxt += "Message = " + message + Environment.NewLine;
                 XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                XmlElement xelRoot = doc.DocumentElement;
                bool error = false;
                if (xelRoot != null)
                {
                    XmlNodeList nodes = xelRoot.GetElementsByTagName("InvoiceAddRs");
                    if (nodes[0] != null)
                    {
                        if(nodes[0].Attributes["statusSeverity"].Value.ToLower()=="error")
                        {
                            error = true;
                            evLogTxt += "Error: BillTo Not set up in Quickbooks. " + Environment.NewLine;
                            retVal = -101;
                            //string reqId = nodes[0].Attributes["requestID"].Value;
                            //Session["AddCustomer"] = reqId.Substring(0, reqId.IndexOf("_"));
                        }
                    }
                    if(!error)
                    {
                        //invoice created
                        //TODO: Update all invoices if multi add worked
                        XmlNodeList invNodes = xelRoot.GetElementsByTagName("InvoiceRet");
                        if (invNodes != null)
                        {
                            evLogTxt += "InvoiceRet: " + invNodes.Count.ToString() + Environment.NewLine;
                            foreach (XmlNode invNode in invNodes)
                            {
                                XmlNode txnNode = invNode.SelectSingleNode("RefNumber");
                                if (txnNode != null)
                                {
                                    evLogTxt += "TxnNumber: " + txnNode.InnerText + Environment.NewLine;
                                    XmlNode invNo = invNode.SelectSingleNode("Memo");
                                    if (invNo != null)
                                    {
                                        evLogTxt += "Memo: " + invNo.InnerText + Environment.NewLine;
                                        InvoiceInfo inv = aCont.Get_InvoiceById(Convert.ToInt32(invNo.InnerText));
                                        if (inv.Id != -1)
                                        {
                                            inv.SentToQB = true;
                                            inv.QBInvoiceNumber = txnNode.InnerText;
                                            inv.LastModifiedDate = DateTime.Now;
                                            aCont.Update_Invoice(inv);
                                            List<int> woIds = aCont.Get_WOInsByInvoiceId(inv.Id);
                                            foreach (int woId in woIds)
                                            {
                                                WorkOrderInfo wo = aCont.Get_WorkOrderById(woId);
                                                wo.InvoiceNumber = inv.QBInvoiceNumber;
                                                //wo.LastModifiedDate = DateTime.Now;
                                                wo.Status = "INVOICED";
                                                aCont.Update_WorkOrder(wo);
                                            }
                                            evLogTxt += "WO's updated." + Environment.NewLine;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    retVal = 100;
                }
            //}
            //else
            //{
            //    evLogTxt = evLogTxt + "Length of response received = " + response.Length + Environment.NewLine;
            //    //ArrayList req = buildRequest(ticket);
            //    //int total = req.Count;
            //    //int count = Convert.ToInt32(Session["counter"]);
            //    //int percentage = (count * 100) / total;
            //    //if (percentage >= 100)
            //    //{
            //    //    count = 0;
            //    //    Session["counter"] = 0;
            //    //}
            //    retVal = 100;
            //}
            evLogTxt = evLogTxt + Environment.NewLine;
            evLogTxt = evLogTxt + "Return values: " + Environment.NewLine;
            evLogTxt = evLogTxt + "int retVal= " + retVal.ToString() + Environment.NewLine;
            logEvent(evLogTxt);
            //TODO: Add update workorder to add invoice id
            return retVal;
        }
        [WebMethod(Description = "send request XML ", EnableSession = true)]
        public string sendRequestXML(string ticket, string strHCPResponse, string strCompanyFileName, string Country, int qbXMLMajorVers, int qbXMLMinorVers)
        {
            //if (Session["counter"] == null)
            //{
            //    Session["counter"] = 0;
            //}
            string evLogTxt = "WebMethod: sendRequestXML() has been called by QBWebconnector" + Environment.NewLine;
            evLogTxt = evLogTxt + "Parameters received:" + Environment.NewLine;
            evLogTxt = evLogTxt + "string ticket = " + ticket + Environment.NewLine;
            evLogTxt = evLogTxt + "string strHCPResponse = " + strHCPResponse + Environment.NewLine;
            evLogTxt = evLogTxt + "string strCompanyFileName = " + strCompanyFileName + Environment.NewLine;
            evLogTxt = evLogTxt + "string Country = " + Country + Environment.NewLine;
            evLogTxt = evLogTxt + "int qbXMLMajorVers = " + qbXMLMajorVers.ToString() + Environment.NewLine;
            evLogTxt = evLogTxt + "int qbXMLMinorVers = " + qbXMLMinorVers.ToString() + Environment.NewLine;
            evLogTxt = evLogTxt + Environment.NewLine;
            string req = buildRequest(ticket);
            string request = "";
            //int total = req.Count;
            //int count = Convert.ToInt32(Session["counter"]);
            if (req != "")
            {
                request = req;
                evLogTxt = evLogTxt + "sending request no = 1" + Environment.NewLine;
                //evLogTxt += req[count].ToString() + Environment.NewLine;
                logEvent("XML Sent: " + request);
                //Session["counter"] = ((int)Session["counter"]) + 1;
            }
            else
            {
                //count = 0;
                //Session["counter"] = 0;
                //try
                //{
                //    Session.Remove("req");
                //}
                //catch { }
                //try
                //{
                //    Session.Remove("requestID");
                //}
                //catch { }
                request = "";
            }
            evLogTxt = evLogTxt + Environment.NewLine;
            evLogTxt = evLogTxt + "Return values: " + Environment.NewLine;
            evLogTxt = evLogTxt + "string request = " + request + Environment.NewLine;
            if (req!= "")
            {
                logEvent(evLogTxt);
            }
            return request;
        }
        public string buildRequest(string ticket)
        {
            string req = "";
            string[] custCodes = new string[6] { "CUSTOMIZATION 30 MIN", "HD CUSTOMIZATION 30 MIN", "SPOT CUSTOMIZATION", "HD CUSTOMIZATION SPOT", "CUSTOMIZATION 5 MIN", "HD CUSTOMIZATION 5 MIN" };
            int custIndex = 2;

            int i = 1;
            string xmlString = "";
            List<InvoiceInfo> invs = aCont.Get_InvoicesByToSend();
            if (invs.Count > 0)
            {
                xmlString += "<?xml version=\"1.0\" encoding=\"utf-8\"?><?qbxml version=\"6.0\"?><QBXML><QBXMLMsgsRq onError=\"stopOnError\">";
                //build array of xml                
                foreach (InvoiceInfo inv in invs)
                {
                    xmlString += "<InvoiceAddRq requestID=\"" + inv.Id.ToString() + "\"><InvoiceAdd>";// 
                    List<int> woIds = aCont.Get_WOInsByInvoiceId(inv.Id);
                    if (woIds.Count > 0 && woIds[0] > 0)
                    {
                        WorkOrderInfo wo1 = aCont.Get_WorkOrderById(woIds[0]);
                        AdvertiserInfo billto = aCont.Get_AdvertiserById(wo1.BillToId);
                        if (billto.Id == -1)
                        {
                            billto = aCont.Get_AdvertiserById(wo1.AdvertiserId);
                        }
                        i++;
                        xmlString += "<CustomerRef><FullName>" + billto.AdvertiserName.Trim().Replace("&", "&#038;") + "</FullName></CustomerRef>";
                        xmlString += "<BillAddress><Addr1>" + billto.AdvertiserName.Trim().Replace("&", "&#038;") + "</Addr1><Addr2>" + billto.Address1.Trim().Replace("&", "&#038;");
                        if (billto.Address2.Trim() != "")
                        {
                            xmlString += " - " + billto.Address2.Trim().Replace("&", "&#038;");
                        }
                        xmlString += "</Addr2><City>" + billto.City.Trim().Replace("&", "&#038;") + "</City><State>" + billto.State.Trim().Replace("&", "&#038;") + "</State><PostalCode>" + billto.Zip.Trim().Replace("&", "&#038;") + "</PostalCode><Country>" + billto.Country.Trim().Replace("&", "&#038;") + "</Country></BillAddress>";
                        string po = wo1.PONumber;
                        if (po == "")
                        {
                            try
                            {
                                po = wo1.Groups[0].LibraryItems[0].PMTMediaId.Replace("&", "&#038;");
                            }
                            catch { }
                        }
                        xmlString += "<PONumber>" + po.Replace("&", "&#038;") + "</PONumber>";
                        if (woIds.Count == 1)
                        {
                            xmlString += "<FOB>WO: " + woIds[0].ToString() + "</FOB>";
                        }
                        else if (woIds.Count>1)
                        {
                            xmlString += "<FOB>SEE BELOW</FOB>";
                        }
                        xmlString += "<Memo>" + inv.Id.ToString() + "</Memo>";
                        AgencyInfo ag = aCont.Get_AgencyById(wo1.AgencyId);
                        xmlString += "<Other>" + ag.AgencyName.Trim().Replace("&", "&#038;") + "</Other>";
                        foreach (int woId in woIds)
                        {
                            WorkOrderInfo wo = aCont.Get_WorkOrderById(woId);
                            List<TaskInfo> tasks = aCont.Get_TasksByWOId(wo.Id);
                            int groupId = -1;
                            bool groupNew = true;
                            for (int j = 0; j < tasks.Count; j++)
                            {
                                TaskInfo task = tasks[j];
                                if(groupId==-1)
                                {
                                    groupId = task.WOGroupId;
                                }
                                if(task.WOGroupId != groupId)
                                {
                                    groupNew = true;
                                    groupId = task.WOGroupId;
                                }
                                if (task.DeliveryStatus.ToLower() != "cancelled" && !task.isDeleted)
                                {
                                    if (task.TaskType != GroupTypeEnum.Delivery)
                                    {
                                        WOGroupInfo group = aCont.Get_WorkOrderGroupById(task.WOGroupId);
                                        List<QBCodeInfo> servCodes = aCont.FindQBCodesByTask(task.Id, 0, true);
                                        LibraryItemInfo lib = aCont.Get_LibraryItemById(task.LibraryId);
                                        MasterItemInfo master = new MasterItemInfo();
                                        if (task.TaskType == GroupTypeEnum.Non_Deliverable)
                                        {
                                            master = aCont.Get_MasterItemById(task.MasterId);
                                        }
                                        //foreach (ServiceInfo serv in group.Services)
                                        if (groupNew)
                                        {
                                            foreach (QBCodeInfo code in servCodes)
                                            {

                                                if (task.TaskType != GroupTypeEnum.Non_Deliverable)
                                                {
                                                    xmlString += "<InvoiceLineAdd>";
                                                    xmlString += "<ItemRef><FullName>" + code.QBCode.Replace("&", "&#038;") + "</FullName></ItemRef>";
                                                    xmlString += "<Desc>" + lib.Title.Replace("&", "&#038;"); //code.QBCode.Replace("&", "&#038;");
                                                    if (woIds.Count > 1 && j == 0)
                                                    {
                                                        xmlString += " WO " + wo.Id.ToString();
                                                    }
                                                    xmlString += "</Desc>";
                                                    if (task.Quantity == 0)
                                                    {
                                                        task.Quantity = 1;
                                                    }
                                                    xmlString += "<Quantity>" + group.LibraryItems.Count.ToString() + "</Quantity>";
                                                    xmlString += "<Other1></Other1>";
                                                    xmlString += "<Other2></Other2></InvoiceLineAdd>";

                                                }
                                                else
                                                {
                                                    xmlString += "<InvoiceLineAdd>";
                                                    xmlString += "<ItemRef><FullName>" + code.QBCode.Replace("&", "&#038;") + "</FullName></ItemRef>";
                                                    xmlString += "<Desc>" + master.Title.Replace("&", "&#038;");
                                                    if (woIds.Count > 1 && j == 0)
                                                    {
                                                        xmlString += " WO " + wo.Id.ToString();
                                                    }
                                                    xmlString += "</Desc>";
                                                    if (task.Quantity == 0)
                                                    {
                                                        task.Quantity = 1;
                                                    }
                                                    xmlString += "<Quantity>" + task.Quantity + "</Quantity>";
                                                    xmlString += "<Other1>" + lib.ProductDescription.Trim().Replace("&", "&#038;") + "</Other1>";
                                                    xmlString += "<Other2>" + lib.ISCICode.Trim().Replace("&", "&#038;") + "</Other2></InvoiceLineAdd>";
                                                }

                                            }
                                            groupNew = false;
                                        }
                                    }
                                    if (task.TaskType == GroupTypeEnum.Bundle || task.TaskType == GroupTypeEnum.Delivery || task.TaskType == GroupTypeEnum.Customized)
                                    {
                                        LibraryItemInfo lib = aCont.Get_LibraryItemById(task.LibraryId);
                                        bool isHd = lib.MediaType.IndexOf("HD") != -1;
                                        string[] pcs = lib.MediaLength.Split(':');
                                        int secs = 0;
                                        if (pcs.Length == 2)
                                        {
                                            try
                                            {
                                                secs = 60 * Convert.ToInt32(pcs[0]) + Convert.ToInt32(pcs[1]);
                                            }
                                            catch { }
                                        }
                                        if(secs<=120 && !isHd)
                                        {
                                            custIndex = 2;
                                        }
                                        else if(secs<=120 && isHd)
                                        {
                                            custIndex = 3;
                                        }
                                        else if(secs>=1500 && secs<=1800 && !isHd)
                                        {
                                            custIndex = 0;
                                        }
                                        else if (secs >= 1500 && secs <= 1800 && isHd)
                                        {
                                            custIndex = 1;
                                        }
                                        else if (secs >= 180 && secs <= 300 && !isHd)
                                        {
                                            custIndex = 4;
                                        }
                                        else if (secs >= 180 && secs <= 300 && !isHd)
                                        {
                                            custIndex = 5;
                                        }
                                        xmlString += "<InvoiceLineAdd>";
                                        if (task.TaskType != GroupTypeEnum.Customized)
                                        {
                                            xmlString += "<ItemRef><FullName>" + task.QBCode.Trim().Replace("&", "&#038;") + "</FullName></ItemRef>";
                                        }
                                        else
                                        {
                                            xmlString += "<ItemRef><FullName>" + custCodes[custIndex] + "</FullName></ItemRef>";
                                        }
                                        xmlString += "<Desc>" + lib.Title.Trim().Replace("&", "&#038;");
                                        if (woIds.Count > 1 && j == 0)
                                        {
                                            xmlString += " WO " + wo.Id.ToString();
                                        }
                                        xmlString += "</Desc>";
                                        if (task.Quantity == 0)
                                        {
                                            task.Quantity = 1;
                                        }
                                        xmlString += "<Quantity>" + task.Quantity + "</Quantity>";
                                        xmlString += "<Other1>" + lib.ProductDescription.Trim().Replace("&", "&#038;") + "</Other1>";
                                        if (lib.TapeCode.Trim() == "")
                                        {
                                            xmlString += "<Other2>" + lib.ISCICode.Trim().Replace("&", "&#038;") + "</Other2></InvoiceLineAdd>";
                                        }
                                        else
                                        {
                                            xmlString += "<Other2>" + lib.TapeCode.Trim().Replace("&", "&#038;") + "</Other2></InvoiceLineAdd>";
                                        }
                                        WOGroupStationInfo station = aCont.Get_WorkOrderGroupStationById(task.StationId);
                                        if (station.DeliveryMethod.ToLower().IndexOf("tf_") != -1)
                                        {
                                            //check to see if we need to add shipping
                                            if (station.ShippingMethodId == -1)
                                            {
                                                int weight = 1;
                                                TapeFormatInfo tape = aCont.Get_TapeFormatById(Convert.ToInt32(station.DeliveryMethod.Replace("tf_", "")));
                                                if (station.Quantity * tape.Weight > 1.0)
                                                {
                                                    weight = 2;
                                                }
                                                string shippingPrice = "";
                                                if (weight == 1)
                                                {
                                                    if (station.PriorityId == 1)
                                                    {
                                                        shippingPrice = ConfigurationManager.AppSettings["FedEx1LbPriority"].ToString();
                                                    }
                                                    else if (station.PriorityId == 2)
                                                    {
                                                        shippingPrice = ConfigurationManager.AppSettings["FedEx1LbStandard"].ToString();
                                                    }
                                                    else if (station.PriorityId == 3)
                                                    {
                                                        shippingPrice = ConfigurationManager.AppSettings["FedEx1Lb2Day"].ToString();
                                                    }
                                                }
                                                else
                                                {
                                                    if (station.PriorityId == 1)
                                                    {
                                                        shippingPrice = ConfigurationManager.AppSettings["FedEx2LbPriority"].ToString();
                                                    }
                                                    else if (station.PriorityId == 2)
                                                    {
                                                        shippingPrice = ConfigurationManager.AppSettings["FedEx2LbStandard"].ToString();
                                                    }
                                                    else if (station.PriorityId == 3)
                                                    {
                                                        shippingPrice = ConfigurationManager.AppSettings["FedEx2Lb2Day"].ToString();
                                                    }
                                                }
                                                xmlString += "<InvoiceLineAdd><ItemRef><FullName>SHIPPING</FullName></ItemRef><Desc>P" + station.PriorityId.ToString() + " " + weight.ToString() + "LB SHIPPING CHARGE</Desc><Quantity>1</Quantity><Rate>" + shippingPrice + "</Rate></InvoiceLineAdd>";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    xmlString += "</InvoiceAdd></InvoiceAddRq>";
                    //xmlString += "";
                }
                xmlString += "</QBXMLMsgsRq></QBXML>";
                req = xmlString;
                //}
            }
            return req;
        }
        public string getCustomerAddXML()
        {
            string xmlString = "";
            if (Session["requestID"] != null)
            {
                AdvertiserInfo billTo = aCont.Get_AdvertiserById(Convert.ToInt32(Session["requestID"]));
                if (billTo.Id != -1)
                {
                    xmlString = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                    xmlString += "<?qbxml version=\"2.0\"?>";
                    xmlString += "<QBXML><QBXMLMsgsRq onError=\"stopOnError\">";
                    xmlString += "<CustomerAddRq requestID=\"add_" + billTo.Id.ToString() + "\"><CustomerAdd><Name>" + billTo.AdvertiserName + "</Name>";
                    xmlString += "<CompanyName>" + billTo.AdvertiserName + "</CompanyName><BillAddress>";
                    xmlString += "<Addr1>" + billTo.Address1 + "</Addr1>";
                    xmlString += "<Addr2>" + billTo.Address2 + "</Addr2>";
                    xmlString += "<City>" + billTo.City + "</City>";
                    xmlString += "<State>" + billTo.State + "</State>";
                    xmlString += "<PostalCode>" + billTo.Zip + "</PostalCode>";
                    xmlString += "<Country>" + billTo.Country + "</Country>";
                    xmlString += "</BillAddress><Phone>" + billTo.Phone + "</Phone>";
                    xmlString += "<Fax>" + billTo.Fax + "</Fax>";
                    xmlString += "</CustomerAdd></CustomerAddRq></QBXMLMsgsRq></QBXML>";
                }
            }
            return xmlString;
        }
        public void logEvent(string evLogTxt)
        {
            DotNetNuke.Services.Log.EventLog.EventLogController eCont = new DotNetNuke.Services.Log.EventLog.EventLogController();
            eCont.AddLog("QB Log Event", "QB Event Log: " + evLogTxt, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.HOST_ALERT);
        }
    }
}
