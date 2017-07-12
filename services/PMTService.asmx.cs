using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DotNetNuke.Services.Exceptions;

namespace Christoc.Modules.PMT_Admin
{
    /// <summary>
    /// Summary description for PMTService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PMTService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public string AddMasterItem(string token, int PortalId, int ClientType, int ClientId, string Title, string Filename, string Length, string Format)
        {
            //client type: -1:don't know, 0:Advertiser, 1:Agency
            //Format should be "HD" or "SD"
            AdminController aCont = new AdminController();
            string MasterId = "";
            string Secret = "fEE3txVQkUSXiAC16vPeqdTTUwOYh99w";
            if(token==Secret)
            {
                try
                {
                    MasterItemInfo master = new MasterItemInfo();
                    master.PortalId = PortalId;
                    if (ClientType == 0 && ClientId > -1)
                    {
                        master.AdvertiserId = ClientId;
                    }
                    master.Title = Title;
                    master.Filename = Filename;
                    master.Length = Length;
                    master.PMTMediaId = aCont.GetNextMediaId(PortalId);
                    if (Format == "HD")
                    {
                        master.PMTMediaId += "H";
                    }
                    master.Id = aCont.Add_MasterItem(master);
                    if (ClientType == 1 && ClientId > -1)
                    {
                        aCont.Add_MasterItemAgency(master.Id, ClientId);
                    }
                    MasterId = master.PMTMediaId;
                }
                catch (Exception ex)
                {
                    DotNetNuke.Services.Log.EventLog.EventLogController eCont = new DotNetNuke.Services.Log.EventLog.EventLogController();
                    eCont.AddLog("Add MasterItem Failed", "Error: " + ex.Message, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.HOST_ALERT);
                    Exceptions.LogException(ex);
                    MasterId = ex.Message;
                }
            }
            else
            {
                MasterId = "Failed.";
            }
            return MasterId;
        }
    }
}
