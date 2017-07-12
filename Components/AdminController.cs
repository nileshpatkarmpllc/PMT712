using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.IO;

using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Search;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Mail;
using Newtonsoft.Json;
using EasyPost;

namespace Christoc.Modules.PMT_Admin
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for PMT_Admin
    /// 
    /// The FeatureController class is defined as the BusinessController in the manifest file (.dnn)
    /// DotNetNuke will poll this class to find out which Interfaces the class implements. 
    /// 
    /// The IPortable interface is used to import/export content from a DNN module
    /// 
    /// The ISearchable interface is used by DNN to index the content of a module
    /// 
    /// The IUpgradeable interface allows module developers to execute code during the upgrade 
    /// process for a module.
    /// 
    /// Below you will find stubbed out implementations of each, uncomment and populate with your own data
    /// </summary>
    /// -----------------------------------------------------------------------------

    //uncomment the interfaces to add the support.
    public class AdminController //: IPortable, ISearchable, IUpgradeable
    {
        #region DataCalls
        public int Add_Agency(AgencyInfo Agency)
        {
            HttpContext.Current.Application["Agencies"] = null;
            return DataProvider.Instance().Add_Agency(Agency);
        }
        public int Add_AgencyForImport(AgencyInfo Agency)
        {
            HttpContext.Current.Application["Agencies"] = null;
            return DataProvider.Instance().Add_AgencyForImport(Agency);
        }
        public void ClearAgencies()
        {
            DataProvider.Instance().ClearAgencies();
        }
        public void ClearAdvertiserAgencies()
        {
            DataProvider.Instance().Delete_AdvertiserAgenciesForImport();
        }
        public void Update_Agency(AgencyInfo Agency)
        {
            if (Agency.Id != -1)
            {
                HttpContext.Current.Application["Agencies"] = null;
                DataProvider.Instance().Update_Agency(Agency);
            }
        }
        public void Delete_Agency(AgencyInfo Agency)
        {
            if (Agency.Id != -1)
            {
                HttpContext.Current.Application["Agencies"] = null;
                DataProvider.Instance().Delete_Agency(Agency);
            }
        }
        public List<AgencyInfo> Get_AgenciesByPortalId(int PortalId)
        {
            List<AgencyInfo> agencies = new List<AgencyInfo>();
            if (HttpContext.Current.Application["Agencies"] != null)
            {
                agencies = (List<AgencyInfo>)HttpContext.Current.Application["Agencies"];
            }
            else
            {
                agencies = CBO.FillCollection<AgencyInfo>(DataProvider.Instance().Get_AgenciesByPortalId(PortalId));
                HttpContext.Current.Application["Agencies"] = agencies;
            }
            return agencies;
        }
        public AgencyInfo Get_AgencyById(int Id)
        {
            AgencyInfo agency = new AgencyInfo();
            List<AgencyInfo> agencies = CBO.FillCollection<AgencyInfo>(DataProvider.Instance().Get_AgencyById(Id));
            if (agencies.Count > 0)
            {
                return agencies[0];
            }
            else
            {
                return agency;
            }
        }
        public int Add_Advertiser(AdvertiserInfo Advertiser)
        {
            HttpContext.Current.Application["Advertisers"] = null;
            return DataProvider.Instance().Add_Advertiser(Advertiser);
        }
        public int Add_AdvertiserForImport(AdvertiserInfo Advertiser)
        {
            return DataProvider.Instance().Add_AdvertiserForImport(Advertiser);
        }
        public void ClearAdvertisers()
        {
            DataProvider.Instance().ClearAdvertisers();
        }
        public void Update_Advertiser(AdvertiserInfo Advertiser)
        {
            if (Advertiser.Id != -1)
            {
                HttpContext.Current.Application["Advertisers"] = null;
                DataProvider.Instance().Update_Advertiser(Advertiser);
            }
        }
        public void Delete_Advertiser(AdvertiserInfo Advertiser)
        {
            if (Advertiser.Id != -1)
            {
                HttpContext.Current.Application["Advertisers"] = null;
                DataProvider.Instance().Delete_Advertiser(Advertiser);
            }
        }
        public List<AdvertiserInfo> Get_AdvertisersByPortalId(int PortalId)
        {
            //TODO: speed up with Linq
            List<AdvertiserInfo> advs = new List<AdvertiserInfo>();
            if (HttpContext.Current.Application["Advertisers"] != null)
            {
                advs = (List<AdvertiserInfo>)HttpContext.Current.Application["Advertisers"];
            }
            else
            {
                advs = CBO.FillCollection<AdvertiserInfo>(DataProvider.Instance().Get_AdvertisersByPortalId(PortalId));
                List<AgencyInfo> ags = Get_AgenciesByPortalId(PortalId);
                List<AdvertiserAgencyInfo> adags = Get_AdvertiserAgencies();
                foreach (AdvertiserInfo adv in advs)
                {
                    foreach (AgencyInfo ag in ags)
                    {
                        foreach (AdvertiserAgencyInfo adag in adags)
                        {
                            if (ag.Id == adag.AgencyId && adv.Id == adag.AdvertiserId)
                            {
                                adv.Agencies.Add(ag);
                            }
                        }
                    }
                }
                HttpContext.Current.Application["Advertisers"] = advs;
            }
            return advs;
        }
        public AdvertiserInfo Get_AdvertiserById(int Id)
        {
            AdvertiserInfo advertiser = new AdvertiserInfo();
            List<AdvertiserInfo> advertisers = CBO.FillCollection<AdvertiserInfo>(DataProvider.Instance().Get_AdvertiserById(Id));
            if (advertisers.Count > 0)
            {
                List<AgencyInfo> ags = Get_AgenciesByAdvertiserId(Id);
                advertisers[0].Agencies = ags;
                return advertisers[0];
            }
            else
            {
                return advertiser;
            }
        }
        public int Add_Market(MarketInfo Market)
        {
            return DataProvider.Instance().Add_Market(Market);
        }
        public void Update_Market(MarketInfo Market)
        {
            if (Market.Id != -1)
            {
                DataProvider.Instance().Update_Market(Market);
            }
        }
        public void Delete_Market(MarketInfo Market)
        {
            if (Market.Id != -1)
            {
                DataProvider.Instance().Delete_Market(Market);
            }
        }
        public List<MarketInfo> Get_MarketsByPortalId(int PortalId)
        {
            return CBO.FillCollection<MarketInfo>(DataProvider.Instance().Get_MarketsByPortalId(PortalId));
        }
        public MarketInfo Get_MarketById(int Id)
        {
            MarketInfo Market = new MarketInfo();
            List<MarketInfo> Markets = CBO.FillCollection<MarketInfo>(DataProvider.Instance().Get_MarketById(Id));
            if (Markets.Count > 0)
            {
                return Markets[0];
            }
            else
            {
                return Market;
            }
        }
        public int Add_ClientType(ClientTypeInfo ClientType)
        {
            return DataProvider.Instance().Add_ClientType(ClientType);
        }
        public void Update_ClientType(ClientTypeInfo ClientType)
        {
            if (ClientType.Id != -1)
            {
                DataProvider.Instance().Update_ClientType(ClientType);
            }
        }
        public void Delete_ClientType(ClientTypeInfo ClientType)
        {
            if (ClientType.Id != -1)
            {
                DataProvider.Instance().Delete_ClientType(ClientType);
            }
        }
        public List<ClientTypeInfo> Get_ClientTypesByPortalId(int PortalId)
        {
            return CBO.FillCollection<ClientTypeInfo>(DataProvider.Instance().Get_ClientTypesByPortalId(PortalId));
        }
        public ClientTypeInfo Get_ClientTypeById(int Id)
        {
            ClientTypeInfo ClientType = new ClientTypeInfo();
            List<ClientTypeInfo> ClientTypes = CBO.FillCollection<ClientTypeInfo>(DataProvider.Instance().Get_ClientTypeById(Id));
            if (ClientTypes.Count > 0)
            {
                return ClientTypes[0];
            }
            else
            {
                return ClientType;
            }
        }
        public int Add_CarrierType(CarrierTypeInfo CarrierType)
        {
            return DataProvider.Instance().Add_CarrierType(CarrierType);
        }
        public void Update_CarrierType(CarrierTypeInfo CarrierType)
        {
            if (CarrierType.Id != -1)
            {
                DataProvider.Instance().Update_CarrierType(CarrierType);
            }
        }
        public void Delete_CarrierType(CarrierTypeInfo CarrierType)
        {
            if (CarrierType.Id != -1)
            {
                DataProvider.Instance().Delete_CarrierType(CarrierType);
            }
        }
        public List<CarrierTypeInfo> Get_CarrierTypesByPortalId(int PortalId)
        {
            return CBO.FillCollection<CarrierTypeInfo>(DataProvider.Instance().Get_CarrierTypesByPortalId(PortalId));
        }
        public CarrierTypeInfo Get_CarrierTypeById(int Id)
        {
            CarrierTypeInfo CarrierType = new CarrierTypeInfo();
            List<CarrierTypeInfo> CarrierTypes = CBO.FillCollection<CarrierTypeInfo>(DataProvider.Instance().Get_CarrierTypeById(Id));
            if (CarrierTypes.Count > 0)
            {
                return CarrierTypes[0];
            }
            else
            {
                return CarrierType;
            }
        }
        public int Add_FreightType(FreightTypeInfo FreightType)
        {
            return DataProvider.Instance().Add_FreightType(FreightType);
        }
        public void Update_FreightType(FreightTypeInfo FreightType)
        {
            if (FreightType.Id != -1)
            {
                DataProvider.Instance().Update_FreightType(FreightType);
            }
        }
        public void Delete_FreightType(FreightTypeInfo FreightType)
        {
            if (FreightType.Id != -1)
            {
                DataProvider.Instance().Delete_FreightType(FreightType);
            }
        }
        public List<FreightTypeInfo> Get_FreightTypesByPortalId(int PortalId)
        {
            return CBO.FillCollection<FreightTypeInfo>(DataProvider.Instance().Get_FreightTypesByPortalId(PortalId));
        }
        public FreightTypeInfo Get_FreightTypeById(int Id)
        {
            FreightTypeInfo FreightType = new FreightTypeInfo();
            List<FreightTypeInfo> FreightTypes = CBO.FillCollection<FreightTypeInfo>(DataProvider.Instance().Get_FreightTypeById(Id));
            if (FreightTypes.Count > 0)
            {
                return FreightTypes[0];
            }
            else
            {
                return FreightType;
            }
        }
        public int Add_TapeFormat(TapeFormatInfo TapeFormat)
        {
            return DataProvider.Instance().Add_TapeFormat(TapeFormat);
        }
        public void Update_TapeFormat(TapeFormatInfo TapeFormat)
        {
            if (TapeFormat.Id != -1)
            {
                DataProvider.Instance().Update_TapeFormat(TapeFormat);
            }
        }
        public void Delete_TapeFormat(TapeFormatInfo TapeFormat)
        {
            if (TapeFormat.Id != -1)
            {
                DataProvider.Instance().Delete_TapeFormat(TapeFormat);
            }
        }
        public List<TapeFormatInfo> Get_TapeFormatsByPortalId(int PortalId)
        {
            return CBO.FillCollection<TapeFormatInfo>(DataProvider.Instance().Get_TapeFormatsByPortalId(PortalId));
        }
        public TapeFormatInfo Get_TapeFormatById(int Id)
        {
            TapeFormatInfo TapeFormat = new TapeFormatInfo();
            List<TapeFormatInfo> TapeFormats = CBO.FillCollection<TapeFormatInfo>(DataProvider.Instance().Get_TapeFormatById(Id));
            if (TapeFormats.Count > 0)
            {
                return TapeFormats[0];
            }
            else
            {
                return TapeFormat;
            }
        }
        public int Add_DeliveryMethod(DeliveryMethodInfo DeliveryMethod)
        {
            return DataProvider.Instance().Add_DeliveryMethod(DeliveryMethod);
        }
        public void Update_DeliveryMethod(DeliveryMethodInfo DeliveryMethod)
        {
            if (DeliveryMethod.Id != -1)
            {
                DataProvider.Instance().Update_DeliveryMethod(DeliveryMethod);
            }
        }
        public void Delete_DeliveryMethod(DeliveryMethodInfo DeliveryMethod)
        {
            if (DeliveryMethod.Id != -1)
            {
                DataProvider.Instance().Delete_DeliveryMethod(DeliveryMethod);
            }
        }
        public List<DeliveryMethodInfo> Get_DeliveryMethodsByPortalId(int PortalId)
        {
            return CBO.FillCollection<DeliveryMethodInfo>(DataProvider.Instance().Get_DeliveryMethodsByPortalId(PortalId));
        }
        public DeliveryMethodInfo Get_DeliveryMethodById(int Id)
        {
            DeliveryMethodInfo DeliveryMethod = new DeliveryMethodInfo();
            List<DeliveryMethodInfo> DeliveryMethods = CBO.FillCollection<DeliveryMethodInfo>(DataProvider.Instance().Get_DeliveryMethodById(Id));
            if (DeliveryMethods.Count > 0)
            {
                return DeliveryMethods[0];
            }
            else
            {
                return DeliveryMethod;
            }
        }
        public int Add_Station(StationInfo Station)
        {
            HttpContext.Current.Application["Stations"] = null;
            return DataProvider.Instance().Add_Station(Station);
        }
        public int Add_StationForImport(StationInfo Station)
        {
            HttpContext.Current.Application["Stations"] = null;
            return DataProvider.Instance().Add_StationForImport(Station);
        }
        public void ClearStations()
        {
            HttpContext.Current.Application["Stations"] = null;
            DataProvider.Instance().ClearStations();
        }
        public void Update_Station(StationInfo Station)
        {
            if (Station.Id != -1)
            {
                HttpContext.Current.Application["Stations"] = null;
                DataProvider.Instance().Update_Station(Station);
            }
        }
        public void Delete_Station(StationInfo Station)
        {
            if (Station.Id != -1)
            {
                HttpContext.Current.Application["Stations"] = null;
                DataProvider.Instance().Delete_Station(Station);
            }
        }
        public List<StationInfo> Get_StationsByPortalId(int PortalId)
        {
            List<StationInfo> stations = new List<StationInfo>();
            if(HttpContext.Current.Application["Stations"]!=null)
            {
                stations = (List<StationInfo>)HttpContext.Current.Application["Stations"];
            }
            else
            {
                stations = CBO.FillCollection<StationInfo>(DataProvider.Instance().Get_StationsByPortalId(PortalId));
                HttpContext.Current.Application["Stations"] = stations;
            }
            return stations;
        }
        public List<StationInfo> Get_StationsByPortalIdActive(int PortalId)
        {
            List<StationInfo> stations = new List<StationInfo>();
            if (HttpContext.Current.Application["Stations"] != null)
            {
                stations = (List<StationInfo>)HttpContext.Current.Application["Stations"];
            }
            else
            {
                stations = CBO.FillCollection<StationInfo>(DataProvider.Instance().Get_StationsByPortalIdActive(PortalId));
                HttpContext.Current.Application["Stations"] = stations;
            }
            return stations;
        }
        public StationInfo Get_StationById(int Id)
        {
            StationInfo Station = new StationInfo();
            List<StationInfo> Stations = CBO.FillCollection<StationInfo>(DataProvider.Instance().Get_StationById(Id));
            if (Stations.Count > 0)
            {
                return Stations[0];
            }
            else
            {
                return Station;
            }
        }
        public int Add_StationGroup(StationGroupInfo StationGroup)
        {
            return DataProvider.Instance().Add_StationGroup(StationGroup);
        }
        public void Update_StationGroup(StationGroupInfo StationGroup)
        {
            if (StationGroup.Id != -1)
            {
                DataProvider.Instance().Update_StationGroup(StationGroup);
            }
        }
        public void Delete_StationGroup(StationGroupInfo StationGroup)
        {
            if (StationGroup.Id != -1)
            {
                DataProvider.Instance().Delete_StationGroup(StationGroup);
            }
        }
        public List<StationGroupInfo> Get_StationGroupsByPortalId(int PortalId)
        {
            return CBO.FillCollection<StationGroupInfo>(DataProvider.Instance().Get_StationGroupsByPortalId(PortalId));
        }
        public StationGroupInfo Get_StationGroupById(int Id)
        {
            StationGroupInfo StationGroup = new StationGroupInfo();
            List<StationGroupInfo> StationGroups = CBO.FillCollection<StationGroupInfo>(DataProvider.Instance().Get_StationGroupById(Id));
            if (StationGroups.Count > 0)
            {
                // get stations
                List<StationInfo> stations = Get_StationsinGroupById(Id);
                StationGroups[0].stations = stations;
                return StationGroups[0];
            }
            else
            {
                return StationGroup;
            }
        }
        public List<StationGroupInfo> Get_StationGroupsByUserId(int UserId)
        {
            return CBO.FillCollection<StationGroupInfo>(DataProvider.Instance().Get_StationGroupsByUserId(UserId));
        }
        public List<StationInfo> Get_StationsinGroupById(int StationGroupId)
        {
            return CBO.FillCollection<StationInfo>(DataProvider.Instance().Get_StationsinGroupById(StationGroupId));
        }
        public void Delete_StationsInGroup(int StationId, int StationGroupId)
        {
            if (StationId != -1 && StationGroupId != -1)
            {
                DataProvider.Instance().Delete_StationsInGroup(StationId, StationGroupId);
            }
        }
        public void Delete_StationsInGroupByGroup(int StationGroupId)
        {
            if (StationGroupId != -1)
            {
                DataProvider.Instance().Delete_StationsInGroupByGroup(StationGroupId);
            }
        }
        public int Add_StationsInGroup(int PortalId, int StationId, int StationGroupId)
        {
            return DataProvider.Instance().Add_StationsInGroup(PortalId, StationId, StationGroupId);
        }
        public int Add_Label(LabelInfo Label)
        {
            return DataProvider.Instance().Add_Label(Label);
        }
        public int Add_LabelForImport(LabelInfo Label)
        {
            return DataProvider.Instance().Add_LabelForImport(Label);
        }
        public void ClearLabels()
        {
            DataProvider.Instance().ClearLabels();
        }
        public void Update_Label(LabelInfo Label)
        {
            if (Label.Id != -1)
            {
                DataProvider.Instance().Update_Label(Label);
            }
        }
        public void Delete_Label(LabelInfo Label)
        {
            if (Label.Id != -1)
            {
                DataProvider.Instance().Delete_Label(Label);
            }
        }
        public List<LabelInfo> Get_LabelsByPortalId(int PortalId)
        {
            return CBO.FillCollection<LabelInfo>(DataProvider.Instance().Get_LabelsByPortalId(PortalId));
        }
        public LabelInfo Get_LabelById(int Id)
        {
            LabelInfo Label = new LabelInfo();
            List<LabelInfo> Labels = CBO.FillCollection<LabelInfo>(DataProvider.Instance().Get_LabelById(Id));
            if (Labels.Count > 0)
            {
                return Labels[0];
            }
            else
            {
                return Label;
            }
        }
        public int Add_MasterItem(MasterItemInfo MasterItem)
        {
            HttpContext.Current.Application["MasterItems"] = null;
            return DataProvider.Instance().Add_MasterItem(MasterItem);
        }
        public int Add_MasterItemForImport(MasterItemInfo MasterItem)
        {
            HttpContext.Current.Application["MasterItems"] = null;
            return DataProvider.Instance().Add_MasterItemForImport(MasterItem);
        }
        public  void ClearMasterItems()
        {
            DataProvider.Instance().ClearMasterItems();
        }
        public void ClearMasterItemAgencies()
        {
            DataProvider.Instance().ClearMasterItemAgencies();
        }
        public void Update_MasterItem(MasterItemInfo MasterItem)
        {
            if (MasterItem.Id != -1)
            {
                HttpContext.Current.Application["MasterItems"] = null;
                DataProvider.Instance().Update_MasterItem(MasterItem);
            }
        }
        public void Delete_MasterItem(MasterItemInfo MasterItem)
        {
            if (MasterItem.Id != -1)
            {
                HttpContext.Current.Application["MasterItems"] = null;
                DataProvider.Instance().Delete_MasterItem(MasterItem);
            }
        }
        public List<MasterItemInfo> Get_MasterItemsByPortalId(int PortalId)
        {
            List<MasterItemInfo> masters  = new List<MasterItemInfo>();
            if (HttpContext.Current.Application["MasterItems"] != null)
            {
                masters = (List<MasterItemInfo>)HttpContext.Current.Application["MasterItems"];
            }
            else
            {
                masters = CBO.FillCollection<MasterItemInfo>(DataProvider.Instance().Get_MasterItemsByPortalId(PortalId));
                List<AgencyInfo> ags = Get_AgenciesByPortalId(PortalId);
                List<MasterItemAgencyInfo> miags = Get_MasterItemAgencies();
                
                //flattens AgencyInfo
                var allAgencyInfos = ags.Join(miags, a => a.Id, m => m.AgencyId, (info, agencyInfo) => new
                {
                    AgencyId = info.Id,
                    AgencyName = info.AgencyName,
                    MasterItemId = agencyInfo.MasterItemId
                }).ToList();

                foreach (var item in allAgencyInfos)
                {
                    var item1 = item;
                    foreach (var master in masters.Where(m => m.Id == item1.MasterItemId))
                    {
                        master.Agencies.Add(new AgencyInfo { Id = item.AgencyId, AgencyName = item.AgencyName });
                    }
                }
                HttpContext.Current.Application["MasterItems"] = masters;
            }
            return masters;
        }
        public MasterItemInfo Get_MasterItemById(int Id)
        {
            MasterItemInfo MasterItem = new MasterItemInfo();
            List<MasterItemInfo> MasterItems = CBO.FillCollection<MasterItemInfo>(DataProvider.Instance().Get_MasterItemById(Id));
            if (MasterItems.Count > 0)
            {
                List<AgencyInfo> ags = Get_AgenciesByMasterItemId(MasterItems[0].Id);
                MasterItems[0].Agencies = ags;
                return MasterItems[0];
            }
            else
            {
                return MasterItem;
            }
        }
        public void Add_AdvertiserAgency(int AdvertiserId, int AgencyId)
        {
            DataProvider.Instance().Add_AdvertiserAgency(AdvertiserId, AgencyId);
        }
        public void Delete_AdvertiserAgencyByAdvertiserId(int AdvertiserId)
        {
            DataProvider.Instance().Delete_AdvertiserAgencyByAdvertiserId(AdvertiserId);
        }
        public List<AgencyInfo> Get_AgenciesByAdvertiserId(int AdvertiserId)
        {
            return CBO.FillCollection<AgencyInfo>(DataProvider.Instance().Get_AgenciesByAdvertiserId(AdvertiserId));
        }
        public List<int> Get_AgencyIdsByAdvertiserId(int AdvertiserId)
        {
            return CBO.FillCollection<int>(DataProvider.Instance().Get_AgencyIdsByAdvertiserId(AdvertiserId));
        }
        public void Add_MasterItemAgency(int MasterItemId, int AgencyId)
        {
            DataProvider.Instance().Add_MasterItemAgency(MasterItemId, AgencyId);
        }
        public void Delete_MasterItemAgencyByMasterItemId(int MasterItemId)
        {
            DataProvider.Instance().Delete_MasterItemAgencyByMasterItemId(MasterItemId);
        }
        public List<AgencyInfo> Get_AgenciesByMasterItemId(int MasterItemId)
        {
            return CBO.FillCollection<AgencyInfo>(DataProvider.Instance().Get_AgenciesByMasterItemId(MasterItemId));
        }
        public List<int> Get_AgencyIdsByMasterItemId(int MasterItemId)
        {
            return CBO.FillCollection<int>(DataProvider.Instance().Get_AgencyIdsByMasterItemId(MasterItemId));
        }
        public List<AdvertiserAgencyInfo> Get_AdvertiserAgencies()
        {
            return CBO.FillCollection<AdvertiserAgencyInfo>(DataProvider.Instance().Get_AdvertiserAgencies());
        }
        public List<MasterItemAgencyInfo> Get_MasterItemAgencies()
        {
            return CBO.FillCollection<MasterItemAgencyInfo>(DataProvider.Instance().Get_MasterItemAgencies());
        }
        public void Add_UserInAgency(int UserId, int AgencyId)
        {
            DataProvider.Instance().Add_UserInAgency(UserId, AgencyId);
        }
        public void Delete_UserInAgencies(int UserId)
        {
            if (UserId != -1)
            {
                DataProvider.Instance().Delete_UserInAgencies(UserId);
            }
        }
        public void Add_UserInAdvertiser(int UserId, int AdvertiserId)
        {
            DataProvider.Instance().Add_UserInAdvertiser(UserId, AdvertiserId);
        }
        public void Delete_UserInAdvertisers(int UserId)
        {
            if (UserId != -1)
            {
                DataProvider.Instance().Delete_UserInAdvertisers(UserId);
            }
        }
        public List<AgencyInfo> Get_AgenciesByUser(int UserId)
        {
            List<AgencyInfo> ags = CBO.FillCollection<AgencyInfo>(DataProvider.Instance().Get_AgenciesByUser(UserId));
            return ags;
        }
        public List<AdvertiserInfo> Get_AdvertisersByUser(int UserId, int PortalId)
        {
            List<AdvertiserInfo> advs = new List<AdvertiserInfo>();
            advs = CBO.FillCollection<AdvertiserInfo>(DataProvider.Instance().Get_AdvertisersByUser(UserId));
            List<AgencyInfo> ags = Get_AgenciesByPortalId(PortalId);
            List<AdvertiserAgencyInfo> adags = Get_AdvertiserAgencies();
            foreach (AdvertiserInfo adv in advs)
            {
                foreach (AgencyInfo ag in ags)
                {
                    foreach (AdvertiserAgencyInfo adag in adags)
                    {
                        if (ag.Id == adag.AgencyId && adv.Id == adag.AdvertiserId)
                        {
                            adv.Agencies.Add(ag);
                        }
                    }
                }
            }
            return advs;
        }
        public int getMaxLabelNumber(int PortalId)
        {
            int returnMe = -1;
            string maxLabelNo = DataProvider.Instance().GetMaxLabelNumber(PortalId);
            try
            {
                returnMe = Convert.ToInt32(maxLabelNo);
            }
            catch { }
            return returnMe;
        }
        public string GetNextMediaId(int PortalId)
        {
            try
            {
                string currentMax = DataProvider.Instance().GetMaxPMTMediaId(PortalId);
                //string currentMax = "SB5555";
                string prefix = currentMax.Substring(0, 2);
                string numeral = currentMax.Substring(2, 4);
                if (numeral == "9999")
                {
                    numeral = "0001";
                    char pre1 = Convert.ToChar(currentMax.Substring(0, 1));
                    char pre2 = Convert.ToChar(currentMax.Substring(1, 1));
                    if (pre2 == 'Z')
                    {
                        pre2 = 'A';
                        pre1 = (char)((int)pre1 + 1);
                    }
                    else
                    {
                        pre2 = (char)((int)pre2 + 1);
                    }
                    prefix = pre1.ToString() + pre2.ToString();
                }
                else
                {
                    numeral = (Convert.ToInt32(numeral) + 1).ToString();
                }
                return prefix + numeral;
            }
            catch(Exception ex) {
                return ex.Message;
            }
        }
        public int Add_LibraryItem(LibraryItemInfo LibraryItem)
        {
            return DataProvider.Instance().Add_LibraryItem(LibraryItem);
        }
        public int Add_LibraryItemForImport(LibraryItemInfo LibraryItem)
        {
            return DataProvider.Instance().Add_LibraryItemForImport(LibraryItem);
        }
        public void ClearLibraryItems()
        {
            DataProvider.Instance().ClearLibraryItems();
        }
        public void Update_LibraryItem(LibraryItemInfo LibraryItem)
        {
            if (LibraryItem.Id != -1)
            {
                DataProvider.Instance().Update_LibraryItem(LibraryItem);
            }
        }
        public void Delete_LibraryItem(LibraryItemInfo LibraryItem)
        {
            if (LibraryItem.Id != -1)
            {
                DataProvider.Instance().Delete_LibraryItem(LibraryItem);
            }
        }
        public List<LibraryItemInfo> Get_LibraryItemsByPortalId(int PortalId)
        {
            return CBO.FillCollection<LibraryItemInfo>(DataProvider.Instance().Get_LibraryItemsByPortalId(PortalId));
        }
        public LibraryItemInfo Get_LibraryItemById(int Id)
        {
            LibraryItemInfo LibraryItem = new LibraryItemInfo();
            List<LibraryItemInfo> LibraryItems = CBO.FillCollection<LibraryItemInfo>(DataProvider.Instance().Get_LibraryItemById(Id));
            if (LibraryItems.Count > 0)
            {
                return LibraryItems[0];
            }
            else
            {
                return LibraryItem;
            }
        }
        public LibraryItemInfo Get_LibraryItemByISCI(string ISCI)
        {
            LibraryItemInfo LibraryItem = new LibraryItemInfo();
            List<LibraryItemInfo> LibraryItems = CBO.FillCollection<LibraryItemInfo>(DataProvider.Instance().Get_LibraryItemByISCI(ISCI));
            if (LibraryItems.Count > 0)
            {
                return LibraryItems[0];
            }
            else
            {
                return LibraryItem;
            }
        }
        public int Add_Service(ServiceInfo Service)
        {
            return DataProvider.Instance().Add_Service(Service);
        }
        public void Update_Service(ServiceInfo Service)
        {
            if (Service.Id != -1)
            {
                DataProvider.Instance().Update_Service(Service);
            }
        }
        public void Delete_Service(ServiceInfo Service)
        {
            if (Service.Id != -1)
            {
                DataProvider.Instance().Delete_Service(Service);
            }
        }
        public List<ServiceInfo> Get_ServicesByPortalId(int PortalId)
        {
            return CBO.FillCollection<ServiceInfo>(DataProvider.Instance().Get_ServicesByPortalId(PortalId));
        }
        public ServiceInfo Get_ServiceById(int Id)
        {
            ServiceInfo Service = new ServiceInfo();
            List<ServiceInfo> Services = CBO.FillCollection<ServiceInfo>(DataProvider.Instance().Get_ServiceById(Id));
            if (Services.Count > 0)
            {
                return Services[0];
            }
            else
            {
                return Service;
            }
        }
        public int Add_WorkOrder(WorkOrderInfo WorkOrder)
        {
            return DataProvider.Instance().Add_WorkOrder(WorkOrder);
        }
        public void Update_WorkOrder(WorkOrderInfo WorkOrder)
        {
            if (WorkOrder.Id != -1)
            {
                DataProvider.Instance().Update_WorkOrder(WorkOrder);
            }
        }
        public void Delete_WorkOrder(WorkOrderInfo WorkOrder)
        {
            if (WorkOrder.Id != -1)
            {
                DataProvider.Instance().Delete_WorkOrder(WorkOrder);
            }
        }
        public List<WorkOrderInfo> Get_WorkOrdersByPortalId(int PortalId)
        {
            return CBO.FillCollection<WorkOrderInfo>(DataProvider.Instance().Get_WorkOrdersByPortalId(PortalId));
        }
        public WorkOrderInfo Get_WorkOrderById(int Id)
        {
            WorkOrderInfo WorkOrder = new WorkOrderInfo();
            List<WorkOrderInfo> WorkOrders = CBO.FillCollection<WorkOrderInfo>(DataProvider.Instance().Get_WorkOrderById(Id));
            if (WorkOrders.Count > 0)
            {
                List<WOGroupInfo> Groups = Get_WorkOrderGroupsByWorkOrderId(WorkOrders[0].Id);
                List<WOGroupInfo> newGroups = new List<WOGroupInfo>();
                foreach(WOGroupInfo group in Groups)
                {
                    WOGroupInfo fullGroup = Get_WorkOrderGroupById(group.Id);
                    newGroups.Add(fullGroup);
                }
                WorkOrders[0].Groups = newGroups;
                return WorkOrders[0];
            }
            else
            {
                return WorkOrder;
            }
        }
        public int Add_WorkOrderGroupStation(WOGroupStationInfo WorkOrderGroupStation)
        {
            return DataProvider.Instance().Add_WorkOrderGroupStation(WorkOrderGroupStation);
        }
        public void Update_WorkOrderGroupStation(WOGroupStationInfo WorkOrderGroupStation)
        {
            if (WorkOrderGroupStation.Id != -1)
            {
                DataProvider.Instance().Update_WorkOrderGroupStation(WorkOrderGroupStation);
            }
        }
        public void Delete_WorkOrderGroupStation(WOGroupStationInfo WorkOrderGroupStation)
        {
            if (WorkOrderGroupStation.Id != -1)
            {
                DataProvider.Instance().Delete_WorkOrderGroupStation(WorkOrderGroupStation);
            }
        }
        public void Delete_WorkOrderGroupStationByGroupId(int GroupId)
        {
            if (GroupId != -1)
            {
                DataProvider.Instance().Delete_WorkOrderGroupStationByGroupId(GroupId);
            }
        }
        public void Delete_WorkOrderGroupStationByWOId(int WOId)
        {
            if (WOId != -1)
            {
                DataProvider.Instance().Delete_WorkOrderGroupStationByWOId(WOId);
            }
        }
        public List<WOGroupStationInfo> Get_WorkOrderGroupStationsByGroupId(int GroupId)
        {
            return CBO.FillCollection<WOGroupStationInfo>(DataProvider.Instance().Get_WorkOrderGroupStationsByGroupId(GroupId));
        }
        public WOGroupStationInfo Get_WorkOrderGroupStationById(int Id)
        {
            WOGroupStationInfo WorkOrderGroupStation = new WOGroupStationInfo();
            List<WOGroupStationInfo> WorkOrderGroupStations = CBO.FillCollection<WOGroupStationInfo>(DataProvider.Instance().Get_WorkOrderGroupStationById(Id));
            if (WorkOrderGroupStations.Count > 0)
            {
                return WorkOrderGroupStations[0];
            }
            else
            {
                return WorkOrderGroupStation;
            }
        }
        public int Add_WorkOrderGroup(WOGroupInfo WorkOrderGroup)
        {
            return DataProvider.Instance().Add_WorkOrderGroup(WorkOrderGroup);
        }
        public void Update_WorkOrderGroup(WOGroupInfo WorkOrderGroup)
        {
            if (WorkOrderGroup.Id != -1)
            {
                DataProvider.Instance().Update_WorkOrderGroup(WorkOrderGroup);
            }
        }
        public void Delete_WorkOrderGroup(WOGroupInfo WorkOrderGroup)
        {
            if (WorkOrderGroup.Id != -1)
            {
                DataProvider.Instance().Delete_WorkOrderGroup(WorkOrderGroup);
            }
        }
        public List<WOGroupInfo> Get_WorkOrderGroupsByWorkOrderId(int WorkOrderId)
        {
            return CBO.FillCollection<WOGroupInfo>(DataProvider.Instance().Get_WorkOrderGroupsByWOId(WorkOrderId));
        }
        public WOGroupInfo Get_WorkOrderGroupById(int Id)
        {
            WOGroupInfo WorkOrderGroup = new WOGroupInfo();
            List<WOGroupInfo> WorkOrderGroups = CBO.FillCollection<WOGroupInfo>(DataProvider.Instance().Get_WorkOrderGroupById(Id));
            if (WorkOrderGroups.Count > 0)
            {
                WorkOrderGroups[0].WOGroupStations = Get_WorkOrderGroupStationsByGroupId(WorkOrderGroups[0].Id);
                foreach(WOGroupStationInfo wogs in WorkOrderGroups[0].WOGroupStations)
                {
                    wogs.Station = Get_StationById(wogs.StationId);
                }

                WorkOrderGroups[0].Services = GetServicesByWOGroupId(WorkOrderGroups[0].Id);
                WorkOrderGroups[0].LibraryItems = GetLibItemsByWOGroupId(WorkOrderGroups[0].Id);
                if (WorkOrderGroups[0].MasterId != -1)
                {
                    WorkOrderGroups[0].Master = Get_MasterItemById(WorkOrderGroups[0].MasterId);
                }
                return WorkOrderGroups[0];
            }
            else
            {
                return WorkOrderGroup;
            }
        }
        public void AddWOGroupLibItem(int WorkOrderGroupId, int LibraryItemId)
        {
            DataProvider.Instance().AddWOGroupLibItem(WorkOrderGroupId, LibraryItemId);
        }
        public void DeleteWOGroupLibItemsByWOGroupId(int WorkOrderGroupId)
        {
            DataProvider.Instance().DeleteWOGroupLibItemsByWOGroupId(WorkOrderGroupId);
        }
        public List<LibraryItemInfo> GetLibItemsByWOGroupId(int WOGroupId)
        {
            return CBO.FillCollection<LibraryItemInfo>(DataProvider.Instance().GetLibItemsByWOGroupId(WOGroupId));
        }
        public void AddWOGroupService(int WorkOrderGroupId, int ServiceId)
        {
            DataProvider.Instance().AddWOGroupService(WorkOrderGroupId, ServiceId);
        }
        public void DeleteWOGroupServicesByWOGroupId(int WorkOrderGroupId)
        {
            DataProvider.Instance().DeleteWOGroupServicesByWOGroupId(WorkOrderGroupId);
        }
        public List<ServiceInfo> GetServicesByWOGroupId(int WOGroupId)
        {
            return CBO.FillCollection<ServiceInfo>(DataProvider.Instance().GetServicesByWOGroupId(WOGroupId));
        }
        public List<AdvertiserInfo> Get_AdvertisersByAgencyId(int AgencyId)
        {
            return CBO.FillCollection<AdvertiserInfo>(DataProvider.Instance().Get_AdvertisersByAgencyId(AgencyId));
        }
        public int Add_WOComment(WOCommentInfo WOComment)
        {
            return DataProvider.Instance().Add_WOComment(WOComment);
        }
        public void Update_WOComment(WOCommentInfo WOComment)
        {
            if (WOComment.Id != -1)
            {
                DataProvider.Instance().Update_WOComment(WOComment);
            }
        }
        public void Delete_WOComment(WOCommentInfo WOComment)
        {
            if (WOComment.Id != -1)
            {
                DataProvider.Instance().Delete_WOComment(WOComment);
            }
        }
        public List<WOCommentInfo> Get_WOCommentsByWOId(int WorkOrderId)
        {
            return CBO.FillCollection<WOCommentInfo>(DataProvider.Instance().Get_WOCommentsByWOId(WorkOrderId));
        }
        public WOCommentInfo Get_WOCommentById(int Id)
        {
            WOCommentInfo WOComment = new WOCommentInfo();
            List<WOCommentInfo> WOComments = CBO.FillCollection<WOCommentInfo>(DataProvider.Instance().Get_WOCommentById(Id));
            if (WOComments.Count > 0)
            {
                return WOComments[0];
            }
            else
            {
                return WOComment;
            }
        }
        public int Add_Task(TaskInfo Task)
        {
            return DataProvider.Instance().Add_Task(Task);
        }
        public int Add_TaskForImport(TaskInfo Task)
        {
            return DataProvider.Instance().Add_TaskForImport(Task);
        }
        public void Update_Task(TaskInfo Task)
        {
            if (Task.Id != -1)
            {
                DataProvider.Instance().Update_Task(Task);
            }
        }
        public void Delete_Task(TaskInfo Task)
        {
            if (Task.Id != -1)
            {
                DataProvider.Instance().Delete_Task(Task);
            }
        }
        public List<TaskInfo> Get_TasksByWOId(int WorkOrderId)
        {
            List<TaskInfo> tasks = CBO.FillCollection<TaskInfo>(DataProvider.Instance().Get_TasksByWOId(WorkOrderId));
            foreach(TaskInfo task in tasks)
            {
                task.QBCodes = Get_QBCodesByTaskId(task.Id);
            }
            return tasks;
        }
        public List<TaskInfo> Get_OpenTasks()
        {
            List<TaskInfo> tasks = CBO.FillCollection<TaskInfo>(DataProvider.Instance().Get_OpenTasks());
            foreach (TaskInfo task in tasks)
            {
                task.QBCodes = Get_QBCodesByTaskId(task.Id);
            }
            return tasks;
        }
        public TaskInfo Get_TaskById(int Id)
        {
            TaskInfo Task = new TaskInfo();
            List<TaskInfo> Tasks = CBO.FillCollection<TaskInfo>(DataProvider.Instance().Get_TaskById(Id));
            if (Tasks.Count > 0)
            {
                Tasks[0].QBCodes = Get_QBCodesByTaskId(Tasks[0].Id);
                return Tasks[0];
            }
            else
            {
                return Task;
            }
        }
        public List<AgencyInfo> getAgencies(int PortalId)
        {
            List<AgencyInfo> ags = new List<AgencyInfo>();
            if (HttpContext.Current.Application["Agencies"] != null)
            {
                ags = (List<AgencyInfo>)HttpContext.Current.Application["Agencies"];
            }
            else
            {
                AdminController aCont = new AdminController();
                ags = aCont.Get_AgenciesByPortalId(PortalId);
                HttpContext.Current.Application["Agencies"] = ags;
            }
            return ags;
        }
        public List<AdvertiserInfo> getAdvertisers(int PortalId)
        {
            List<AdvertiserInfo> ads = new List<AdvertiserInfo>();
            if (HttpContext.Current.Application["Advertisers"] != null)
            {
                ads = (List<AdvertiserInfo>)HttpContext.Current.Application["Advertisers"];
            }
            else
            {
                AdminController aCont = new AdminController();
                ads = aCont.Get_AdvertisersByPortalId(PortalId);
                HttpContext.Current.Application["Advertisers"] = ads;
            }
            return ads;
        }
        public List<MasterItemInfo> getMasters(int PortalId)
        {
            List<MasterItemInfo> masters = new List<MasterItemInfo>();
            if (HttpContext.Current.Application["MasterItems"] != null)
            {
                masters = (List<MasterItemInfo>)HttpContext.Current.Application["MasterItems"];
            }
            else
            {
                AdminController aCont = new AdminController();
                masters = aCont.Get_MasterItemsByPortalId(PortalId);
                HttpContext.Current.Application["MasterItems"] = masters;
            }
            return masters;
        }
        public List<LibraryItemInfo> getLibs(int PortalId)
        {
            List<LibraryItemInfo> libs = new List<LibraryItemInfo>();
            if (HttpContext.Current.Application["LibraryItems"] != null)
            {
                libs = (List<LibraryItemInfo>)HttpContext.Current.Application["LibraryItems"];
            }
            else
            {
                AdminController aCont = new AdminController();
                libs = aCont.Get_LibraryItemsByPortalId(PortalId);
                HttpContext.Current.Application["LibraryItems"] = libs;
            }
            return libs;
        }
        public List<LibraryItemInfo> getLibsByAdId(int AdvertiserId, int PortalId)
        {
            List<LibraryItemInfo> libs = getLibs(PortalId);
            List<LibraryItemInfo> libsByAd = libs.Where(n => n.AdvertiserId == AdvertiserId).ToList();
            return (libsByAd);
        }
        public int Add_QBCode(QBCodeInfo QBCode)
        {
            return DataProvider.Instance().Add_QBCode(QBCode);
        }
        public void Update_QBCode(QBCodeInfo QBCode)
        {
            if (QBCode.Id != -1)
            {
                DataProvider.Instance().Update_QBCode(QBCode);
            }
        }
        public void Delete_QBCode(QBCodeInfo QBCode)
        {
            if (QBCode.Id != -1)
            {
                DataProvider.Instance().Delete_QBCode(QBCode);
                DeleteQBCodeDeliveryMethodsByQBCodeId(QBCode.Id);
                DeleteQBCodeServicesByQBCodeId(QBCode.Id);
                DeleteQBCodeTapeFormatsByQBCodeId(QBCode.Id);
            }
        }
        public List<QBCodeInfo> Get_QBCodesByPortalId(int PortalId)
        {
            List<QBCodeInfo> QBCodes = CBO.FillCollection<QBCodeInfo>(DataProvider.Instance().Get_QBCodesByPortalId(PortalId));
            foreach(QBCodeInfo QBCode in QBCodes)
            {
                QBCode.DeliveryMethods = CBO.FillCollection<DeliveryMethodInfo>(DataProvider.Instance().Get_DeliveryMethodsByQBCodeId(QBCode.Id));
                QBCode.TapeFormats = CBO.FillCollection<TapeFormatInfo>(DataProvider.Instance().Get_TapeFormatsByQBCodeId(QBCode.Id));
                QBCode.Services = CBO.FillCollection<ServiceInfo>(DataProvider.Instance().Get_ServicesByQBCodeId(QBCode.Id));
            }
            return QBCodes;
        }
        public QBCodeInfo Get_QBCodeById(int Id)
        {
            QBCodeInfo QBCode = new QBCodeInfo();
            List<QBCodeInfo> QBCodes = CBO.FillCollection<QBCodeInfo>(DataProvider.Instance().Get_QBCodeById(Id));
            if (QBCodes.Count > 0)
            {
                QBCodes[0].DeliveryMethods = CBO.FillCollection<DeliveryMethodInfo>(DataProvider.Instance().Get_DeliveryMethodsByQBCodeId(QBCodes[0].Id));
                QBCodes[0].TapeFormats = CBO.FillCollection<TapeFormatInfo>(DataProvider.Instance().Get_TapeFormatsByQBCodeId(QBCodes[0].Id));
                QBCodes[0].Services = CBO.FillCollection<ServiceInfo>(DataProvider.Instance().Get_ServicesByQBCodeId(QBCodes[0].Id));
                return QBCodes[0];
            }
            else
            {
                return QBCode;
            }
        }
        public void AddQBCodeDeliveryMethod(int QBCodeId, int DeliveryMethodId)
        {
            DataProvider.Instance().Add_QBCodeDeliveryMethodsByQBCodeId(QBCodeId, DeliveryMethodId);
        }
        public void DeleteQBCodeDeliveryMethodsByQBCodeId(int QBCodeId)
        {
            DataProvider.Instance().Delete_QBCodeDeliveryMethodsByQBCodeId(QBCodeId);
        }
        public void AddQBCodeTapeFormat(int QBCodeId, int TapeFormatId)
        {
            DataProvider.Instance().Add_QBCodeTapeFormatsByQBCodeId(QBCodeId, TapeFormatId);
        }
        public void DeleteQBCodeTapeFormatsByQBCodeId(int QBCodeId)
        {
            DataProvider.Instance().Delete_QBCodeTapeFormatsByQBCodeId(QBCodeId);
        }
        public void AddQBCodeService(int QBCodeId, int ServiceId)
        {
            DataProvider.Instance().Add_QBCodeServicesByQBCodeId(QBCodeId, ServiceId);
        }
        public void DeleteQBCodeServicesByQBCodeId(int QBCodeId)
        {
            DataProvider.Instance().Delete_QBCodeServicesByQBCodeId(QBCodeId);
        }
        public void Add_TaskQBCode(int TaskId, int QBCodeId)
        {
            DataProvider.Instance().Add_TaskQBCode(TaskId, QBCodeId);
        }
        public void Delete_TaskQBCodeByTaskId(int TaskId)
        {
            DataProvider.Instance().Delete_TaskQBCodeByTaskId(TaskId);
        }
        public List<QBCodeInfo> Get_QBCodesByTaskId(int TaskId)
        {
            List<QBCodeInfo> QBCodes = CBO.FillCollection<QBCodeInfo>(DataProvider.Instance().Get_QBCodesByTaskId(TaskId));
            foreach (QBCodeInfo QBCode in QBCodes)
            {
                QBCode.DeliveryMethods = CBO.FillCollection<DeliveryMethodInfo>(DataProvider.Instance().Get_DeliveryMethodsByQBCodeId(QBCode.Id));
                QBCode.TapeFormats = CBO.FillCollection<TapeFormatInfo>(DataProvider.Instance().Get_TapeFormatsByQBCodeId(QBCode.Id));
                QBCode.Services = CBO.FillCollection<ServiceInfo>(DataProvider.Instance().Get_ServicesByQBCodeId(QBCode.Id));
            }
            return QBCodes;
        }
        public int Add_Report(ReportInfo Report)
        {
            HttpContext.Current.Application["Reports"] = null;
            return DataProvider.Instance().Add_Report(Report);
        }
        public void Update_Report(ReportInfo Report)
        {
            if (Report.Id != -1)
            {
                HttpContext.Current.Application["Reports"] = null;
                DataProvider.Instance().Update_Report(Report);
            }
        }
        public void Delete_Report(ReportInfo Report)
        {
            if (Report.Id != -1)
            {
                HttpContext.Current.Application["Reports"] = null;
                DataProvider.Instance().Delete_Report(Report);
            }
        }
        public List<ReportInfo> Get_ReportsByPortalId(int PortalId)
        {
            List<ReportInfo> Reports = new List<ReportInfo>();
            if (HttpContext.Current.Application["Reports"] != null)
            {
                Reports = (List<ReportInfo>)HttpContext.Current.Application["Reports"];
            }
            else
            {
                Reports = CBO.FillCollection<ReportInfo>(DataProvider.Instance().Get_ReportsByPortalId(PortalId));
                HttpContext.Current.Application["Reports"] = Reports;
            }
            return Reports;
        }
        public ReportInfo Get_ReportById(int Id)
        {
            ReportInfo Report = new ReportInfo();
            List<ReportInfo> Reports = CBO.FillCollection<ReportInfo>(DataProvider.Instance().Get_ReportById(Id));
            if (Reports.Count > 0)
            {
                return Reports[0];
            }
            else
            {
                return Report;
            }
        }
        public int Add_EasySpotObject(EasySpotObjectInfo EasySpotObject)
        {
            return DataProvider.Instance().Add_EasySpotObject(EasySpotObject);
        }
        public EasySpotObjectInfo Get_EasySpotObjectByIdAndType(int PMTObjectId, EasySpotTypeEnum ObjectType)
        {
            EasySpotObjectInfo EasySpotObject = new EasySpotObjectInfo();
            List<EasySpotObjectInfo> EasySpotObjects = CBO.FillCollection<EasySpotObjectInfo>(DataProvider.Instance().Get_EasySpotByIdAndType(PMTObjectId, ObjectType));
            if (EasySpotObjects.Count > 0)
            {
                return EasySpotObjects[0];
            }
            else
            {
                return EasySpotObject;
            }
        }
        public int Add_Invoice(InvoiceInfo Invoice)
        {
            HttpContext.Current.Application["Invoices"] = null;
            return DataProvider.Instance().Add_Invoice(Invoice);
        }
        public void Update_Invoice(InvoiceInfo Invoice)
        {
            if (Invoice.Id != -1)
            {
                HttpContext.Current.Application["Invoices"] = null;
                DataProvider.Instance().Update_Invoice(Invoice);
            }
        }
        public void Delete_Invoice(InvoiceInfo Invoice)
        {
            if (Invoice.Id != -1)
            {
                HttpContext.Current.Application["Invoices"] = null;
                DataProvider.Instance().Delete_Invoice(Invoice);
            }
        }
        public List<InvoiceInfo> Get_InvoicesByPortalId(int PortalId)
        {
            List<InvoiceInfo> Invoices = new List<InvoiceInfo>();
            if (HttpContext.Current.Application["Invoices"] != null)
            {
                Invoices = (List<InvoiceInfo>)HttpContext.Current.Application["Invoices"];
            }
            else
            {
                Invoices = CBO.FillCollection<InvoiceInfo>(DataProvider.Instance().Get_InvoicesByPortalId(PortalId));
                HttpContext.Current.Application["Invoices"] = Invoices;
            }
            return Invoices;
        }
        public InvoiceInfo Get_InvoiceById(int Id)
        {
            InvoiceInfo Invoice = new InvoiceInfo();
            List<InvoiceInfo> Invoices = CBO.FillCollection<InvoiceInfo>(DataProvider.Instance().Get_InvoiceById(Id));
            if (Invoices.Count > 0)
            {
                return Invoices[0];
            }
            else
            {
                return Invoice;
            }
        }
        public void Add_WOInInvoice(int InvoiceId, int WOId)
        {
            DataProvider.Instance().Add_WOInInvoice(InvoiceId, WOId);
        }
        public void Delete_WOInInvoiceByInvoiceId(int InvoiceId)
        {
            DataProvider.Instance().Delete_WOInInvoiceByInvoiceId(InvoiceId);
        }
        public List<int> Get_WOInsByInvoiceId(int InvoiceId)
        {
            //IDataReader idr = DataProvider.Instance().Get_WOInsByInvoiceId(InvoiceId);
            List<int> s = DataProvider.Instance().Get_WOInsByInvoiceId(InvoiceId);
            return s;
        }
        public List<InvoiceInfo> Get_InvoicesByToSend()
        {
            List<InvoiceInfo> Invoices = new List<InvoiceInfo>();
            //if (HttpContext.Current.Application["Invoices"] != null)
            //{
            //    Invoices = (List<InvoiceInfo>)HttpContext.Current.Application["Invoices"];
            //}
            //else
            //{
                List<int> invs = DataProvider.Instance().Get_InvoicesByToSend();
                foreach(int inv in invs)
                {
                    Invoices.Add(Get_InvoiceById(inv));
                }
                //Invoices =  //CBO.FillCollection<InvoiceInfo>(DataProvider.Instance().Get_InvoicesByToSend());
                //HttpContext.Current.Application["Invoices"] = Invoices;
            //}
            return Invoices;
        }
        #endregion

        #region Utilities
        public string SendReport(ReportInfo Report, string BaseUrl, bool sendMail)
        {
            string link = BaseUrl + "/rid/" + Report.ReportType.ToString();
            if (Report.AdvertiserId != -1)
                link += "/adid/" + Report.AdvertiserId.ToString();
            if (Report.AgencyId != -1)
                link += "/agid/" + Report.AgencyId.ToString();
            if (Report.Keyword != "")
                link += "/key/" + HttpUtility.UrlEncode(Report.Keyword);
            if (Report.Status != -1)
                link += "/status/" + Report.Status.ToString();
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            DateTime today = DateTime.Now;
            //today = today.AddDays(-1);
            if(Report.Frequency == "daily")
            {
                start = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
                end = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59);
            }
            else if (Report.Frequency == "weekly")
            {
                start = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0).AddDays(-7);
                end = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59);
            }
            else if (Report.Frequency == "monthly")
            {
                start = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0).AddMonths(-1);
                end = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59).AddDays(-1);
            }
            link += "/start/" + start.ToShortDateString().Replace("/","-");
            link += "/end/" + end.ToShortDateString().Replace("/", "-");

            if(sendMail)
            {
                string msg = Report.EmailMessage.Replace("[link]", link).Replace("[report_frequency]", Report.Frequency).Replace("[report_name]", Report.ReportName).Replace("[report_date]", DateTime.Now.ToShortDateString());
                if(Report.ReportType == 1)
                { msg = msg.Replace("[report_type]", "Master Items"); }
                else if (Report.ReportType == 2)
                { msg = msg.Replace("[report_type]", "Library Items"); }
                else if (Report.ReportType == 3)
                { msg = msg.Replace("[report_type]", "Delivery"); }
                Mail.SendEmail("notifications@pmtmedia.tv", Report.EmailTo, "Your Report from Pacific Media Technologies, Inc.", msg);
                DotNetNuke.Services.Log.EventLog.EventLogController eCont = new DotNetNuke.Services.Log.EventLog.EventLogController();
                eCont.AddLog("ReportSent", "Report Sent: " + msg, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.HOST_ALERT);
            }

            return link;
        }
        public bool areTasksEqual(TaskInfo Task1, TaskInfo Task2)
        {
            bool isEqual = false;
            if(Task1.DeliveryMethod==Task2.DeliveryMethod &&
                Task1.DeliveryMethodId==Task2.DeliveryMethodId &&
                Task1.Description ==Task2.Description &&
                Task1.LibraryId==Task2.LibraryId &&
                Task1.MasterId==Task2.MasterId &&
                Task1.PortalId==Task2.PortalId &&
                Task1.StationId==Task2.StationId &&
                Task1.TaskType==Task2.TaskType &&
                Task1.WOGroupId==Task2.WOGroupId &&
                Task1.WorkOrderId==Task2.WorkOrderId)
            {
                isEqual = true;
            }

            return isEqual;
        }
        public bool haveTasksChanged(TaskInfo Task1, TaskInfo Task2)
        {
            bool hasChanged = false;
            if ((Task1.DeliveryMethod != Task2.DeliveryMethod ||
                Task1.DeliveryMethodId != Task2.DeliveryMethodId ||
                Task1.Description != Task2.Description) &&
                (Task1.LibraryId == Task2.LibraryId &&
                Task1.MasterId == Task2.MasterId &&
                Task1.PortalId == Task2.PortalId &&
                Task1.StationId == Task2.StationId &&
                Task1.TaskType == Task2.TaskType &&
                Task1.WOGroupId == Task2.WOGroupId &&
                Task1.WorkOrderId == Task2.WorkOrderId))
            {
                hasChanged = true;
            }

            return hasChanged;
        }
        public int getSecondsFromLength(string length)
        {
            int returnMe = 0;
            string[] pcs = length.Split(':');
            if (pcs.Length == 1)
            {
                try
                {
                    returnMe = Convert.ToInt32(pcs[0]);
                }
                catch { }
            }
            else if (pcs.Length==2)
            {
                try
                {
                    returnMe = 60 * Convert.ToInt32(pcs[0]) + Convert.ToInt32(pcs[1]);
                }
                catch { }
            }
            return returnMe;
        }

        public string fixLength(string length)
        {
            string[] pcs = length.Split(':');
            string returnMe = "";
            int secs = 0;
            int mins = 0;
            if(pcs.Count()==1)
            {
                //value is just seconds
                try
                {
                    secs = Convert.ToInt32(pcs[0]);
                    mins = Convert.ToInt32(Math.Floor((double)(secs/60)));
                    secs = secs - (mins * 60);
                }
                catch { }
            }
            else if(pcs.Count()==2)
            {
                try
                {
                    mins = Convert.ToInt32(pcs[0]);
                    secs = Convert.ToInt32(pcs[1]);
                    if(secs>59)
                    {
                        mins += Convert.ToInt32(Math.Floor((double)(secs / 60)));
                        secs = secs - 60* Convert.ToInt32(Math.Floor((double)(secs / 60)));
                    }
                }
                catch { }
            }
            string min = mins.ToString();
            if (min.Length == 0)
            {
                min = "00";
            }
            else if (min.Length == 1)
            {
                min = "0" + min;
            }
            string sec = secs.ToString();
            if (sec.Length == 0)
            {
                sec = "00";
            }
            else if (sec.Length == 1)
            {
                sec = "0" + sec;
            }
            return min + ":" + sec;
        }
        public List<QBCodeInfo> FindQBCodesByTask(int TaskId, int PortalId, bool isServ = false)
        {
            List<QBCodeInfo> ReturnQBCodes = new List<QBCodeInfo>();
            TaskInfo task = Get_TaskById(TaskId);
            List<QBCodeInfo> codes = Get_QBCodesByPortalId(PortalId);
            if (task.TaskType == GroupTypeEnum.Non_Deliverable || task.TaskType == GroupTypeEnum.Customized || task.TaskType == GroupTypeEnum.Bundle)
            {
                WOGroupInfo group = Get_WorkOrderGroupById(task.WOGroupId);
                foreach(ServiceInfo serv in group.Services)
                {
                    foreach(QBCodeInfo code in codes)
                    {
                        foreach(ServiceInfo codeServ in code.Services)
                        {
                            if(codeServ.Id==serv.Id && code.Type==task.TaskType)
                            {
                                ReturnQBCodes.Add(code);
                            }
                        }
                    }
                }
            }
            //else if (task.TaskType == GroupTypeEnum.Customized)
            //{
            //    //Not sure what to do here?
            //    foreach(QBCodeInfo code in codes)
            //    {
            //        foreach (ServiceInfo codeServ in code.Services)
            //        {
            //            if (codeServ.Id == serv.Id)
            //            {
            //                ReturnQBCodes.Add(code);
            //            }
            //        }
            //    }
            //}
            if (!isServ)
            {
                if (task.TaskType == GroupTypeEnum.Bundle || task.TaskType == GroupTypeEnum.Delivery || task.TaskType == GroupTypeEnum.Customized)//bundle and delivery
                {
                    LibraryItemInfo lib = Get_LibraryItemById(task.LibraryId);
                    foreach (QBCodeInfo code in codes)
                    {
                        //check task type
                        if (code.Type == task.TaskType)
                        {
                            int minSecs = getSecondsFromLength(code.MinLength);
                            int maxSecs = getSecondsFromLength(code.MaxLength);
                            int libSecs = getSecondsFromLength(lib.MediaLength);
                            //check length
                            if (minSecs <= libSecs && libSecs <= maxSecs)
                            {
                                if (task.TaskType == GroupTypeEnum.Customized)
                                {
                                    if ((lib.MediaType.ToLower() != "hd" && lib.MediaType.ToLower() != "sd" && lib.MediaType.ToLower() != "hd & sd") || (lib.MediaType.ToLower() == "hd" && (code.MediaType == MediaTypeEnum.HD || code.MediaType == MediaTypeEnum.HD___SD)) ||
                                                (lib.MediaType.ToLower() == "sd" && (code.MediaType == MediaTypeEnum.SD || code.MediaType == MediaTypeEnum.HD___SD)) ||
                                                (lib.MediaType.ToLower() == "hd & sd" && (code.MediaType == MediaTypeEnum.HD || code.MediaType == MediaTypeEnum.SD || code.MediaType == MediaTypeEnum.HD___SD)))
                                    {
                                        ReturnQBCodes.Add(code);
                                    }
                                }
                                else
                                {
                                    WOGroupStationInfo station = Get_WorkOrderGroupStationById(task.StationId);
                                    station.Station = Get_StationById(station.StationId);
                                    if (station.DeliveryMethod.IndexOf("dm_") != -1)
                                    {
                                        //digital delivery
                                        //DeliveryMethodInfo del = Get_DeliveryMethodById(Convert.ToInt32(station.DeliveryMethod.Replace("dm_", "")));
                                        int del = Convert.ToInt32(station.DeliveryMethod.Replace("dm_", ""));
                                        foreach (DeliveryMethodInfo delm in code.DeliveryMethods)
                                        {
                                            if (delm.Id == del)
                                            {
                                                //check Media Type - Need to confirm this logic //2 = HD, 3 = HD&SD, 5=SD
                                                //if (station.Station.MediaType == -1 || (station.Station.MediaType == 2 && (code.MediaType == MediaTypeEnum.HD || code.MediaType == MediaTypeEnum.HD___SD)) ||
                                                //(station.Station.MediaType == 5 && (code.MediaType == MediaTypeEnum.SD || code.MediaType == MediaTypeEnum.HD___SD)) ||
                                                //(station.Station.MediaType == 3 && (code.MediaType == MediaTypeEnum.HD || code.MediaType == MediaTypeEnum.SD || code.MediaType == MediaTypeEnum.HD___SD)))
                                                if ((lib.MediaType.ToLower() != "hd" && lib.MediaType.ToLower() != "sd" && lib.MediaType.ToLower() != "hd & sd") || (lib.MediaType.ToLower() == "hd" && (code.MediaType == MediaTypeEnum.HD || code.MediaType == MediaTypeEnum.HD___SD)) ||
                                                    (lib.MediaType.ToLower() == "sd" && (code.MediaType == MediaTypeEnum.SD || code.MediaType == MediaTypeEnum.HD___SD)) ||
                                                    (lib.MediaType.ToLower() == "hd & sd" && (code.MediaType == MediaTypeEnum.HD || code.MediaType == MediaTypeEnum.SD || code.MediaType == MediaTypeEnum.HD___SD)))
                                                {
                                                    ReturnQBCodes.Add(code);
                                                }
                                            }
                                        }
                                    }
                                    else if (station.DeliveryMethod.IndexOf("tf_") != -1)
                                    {
                                        //tape delivery
                                        TapeFormatInfo tape = Get_TapeFormatById(Convert.ToInt32(station.DeliveryMethod.Replace("tf_", "")));
                                        foreach (TapeFormatInfo tfm in code.TapeFormats)
                                        {
                                            if (tfm.Id == tape.Id)
                                            {
                                                //check media type - Need to confirm this logic
                                                //if ((station.Station.MediaType == 2 && (code.MediaType == MediaTypeEnum.HD || code.MediaType == MediaTypeEnum.HD___SD)) ||
                                                //    (station.Station.MediaType == 5 && (code.MediaType == MediaTypeEnum.SD || code.MediaType == MediaTypeEnum.HD___SD)) ||
                                                //    (station.Station.MediaType == 3 && (code.MediaType == MediaTypeEnum.HD || code.MediaType == MediaTypeEnum.SD || code.MediaType == MediaTypeEnum.HD___SD)))
                                                if ((lib.MediaType.ToLower() != "hd" && lib.MediaType.ToLower() != "sd" && lib.MediaType.ToLower() != "hd & sd") || (lib.MediaType.ToLower() == "hd" && (code.MediaType == MediaTypeEnum.HD || code.MediaType == MediaTypeEnum.HD___SD)) ||
                                                    (lib.MediaType.ToLower() == "sd" && (code.MediaType == MediaTypeEnum.SD || code.MediaType == MediaTypeEnum.HD___SD)) ||
                                                    (lib.MediaType.ToLower() == "hd & sd" && (code.MediaType == MediaTypeEnum.HD || code.MediaType == MediaTypeEnum.SD || code.MediaType == MediaTypeEnum.HD___SD)))
                                                {
                                                    ReturnQBCodes.Add(code);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Delete_TaskQBCodeByTaskId(task.Id);
                foreach (QBCodeInfo code in ReturnQBCodes)
                {
                    Add_TaskQBCode(task.Id, code.Id);
                }
            }
            return ReturnQBCodes;
        }
        public void MakeComment(int PortalId, int WOId, int TaskId, int UserId, string DisplayName, string Comment, CommentTypeEnum CommentType = CommentTypeEnum.Comment)
        {
            WOCommentInfo comment = new WOCommentInfo();
            TaskInfo task = Get_TaskById(TaskId);
            comment.PortalId = PortalId;
            comment.WorkOrderId = WOId;
            comment.WOTaskId = task.Id;
            comment.CreatedById = UserId;
            comment.DateCreated = DateTime.Now;
            comment.DisplayName = DisplayName;
            comment.Comment = Comment;
            comment.LastModifiedById = UserId;
            comment.CommentType = CommentType;
            Add_WOComment(comment);
        }
        #endregion
        #region Comcast API Calls
        public string MakeComcastRequest(string xmlString)
        {
            string ComcastEndpoint = ConfigurationManager.AppSettings["ComcastEndpoint"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(ComcastEndpoint);
            byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(xmlString);
            req.Method = "POST";
            req.ContentType = "text/xml;charset=utf-8";
            req.ContentLength = requestBytes.Length;
            Stream requestStream = req.GetRequestStream();
            requestStream.Write(requestBytes, 0, requestBytes.Length);
            requestStream.Close();

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);
            string ComcastResponse = sr.ReadToEnd();
            sr.Close();
            res.Close();
            return ComcastResponse;
        }
        public string getComcastAuthToken()
        {
            string un = ConfigurationManager.AppSettings["ComcastUsername"];
            string pw = ConfigurationManager.AppSettings["ComcastPassword"];
            string xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><authRequest username=\"" + un + "\" password=\"" + pw + "\"></authRequest>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(MakeComcastRequest(xmlString));
            XmlElement xelRoot = doc.DocumentElement;
            string token = "";
            if (xelRoot != null)
            {
                token = xelRoot.Attributes["authToken"].Value;
            }
            return token;
        }
        public string getComcastSpotStatus(string isciCode, string token = "")
        {
            if (token == "")
            {
                token = getComcastAuthToken();
            }
            string status = "";
            if (token != "")
            {                
                string xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><spotRequest authToken=\"" + token + "\"><spot isci=\"" + isciCode + "\"></spot></spotRequest>";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(MakeComcastRequest(xmlString));
                XmlElement xelRoot = doc.DocumentElement;
                XmlNodeList nodes = xelRoot.GetElementsByTagName("spot");
                if (nodes[0] != null)
                {
                    status = nodes[0].Attributes["status"].Value;
                }
            }
            return status;
        }
        public string createComcastSpot(string isciCode, string title, string AdvertiserName, string BrandName, string AgencyName, int durationValue, string spotFormat, string token = "")
        {
            string spotStatus = getComcastSpotStatus(isciCode, token);
            string spotId = "";
            if(spotFormat=="" || spotFormat.Length>2)
            {
                spotFormat = "SD";
                if(isciCode.Substring(isciCode.Length-1,1).ToUpper()=="H")
                {
                    spotFormat = "HD";
                }
            }
            if (spotStatus == "")
            {
                if (token == "")
                    token = getComcastAuthToken();
                string xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><storeSpotRequest authToken=\"" + token + "\"><spot isci=\"" + isciCode + "\" title=\"" + title + "\" advertiserName=\"" + AdvertiserName + "\" brandName=\"" + BrandName + "\" agencyName=\"" + AgencyName + "\" durationValue=\"" + durationValue + "\" spotFormat=\"" + spotFormat.ToUpper() + "\" ></spot></storeSpotRequest>";                
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.LoadXml(MakeComcastRequest(xmlString));
                    XmlElement xelRoot = doc.DocumentElement;
                    if (xelRoot != null && xelRoot.InnerXml.ToLower().IndexOf("error") == -1)
                    {
                        spotId = xelRoot.Attributes["id"].Value;
                    }
                }
                catch { }
            }
            return spotId;
        }
        public string createComcastOrder(List<TaskInfo> tasks, string token="", int PortalId = 0)
        {
            string orderId = "";
            int c = 0;
            if (tasks.Count() > 0)
            {
                WorkOrderInfo wo = Get_WorkOrderById(tasks[0].WorkOrderId);
                //WOGroupInfo group = Get_WorkOrderGroupById(tasks[0].WOGroupId);
                AdvertiserInfo ad = Get_AdvertiserById(wo.AdvertiserId);
                AgencyInfo ag = Get_AgencyById(wo.AgencyId);
                List<LibraryItemInfo> libs = getLibs(PortalId);
                List<StationInfo> stats = Get_StationsByPortalId(PortalId);
                //List<WOGroupStationInfo> groupStats = Get_WorkOrderGroupStationsByGroupId(group.Id);
                if (token == "")
                    token = getComcastAuthToken();
                string xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><storeOrderRequest authToken=\"" + token + "\"><order name=\"" + wo.Description + "\" advertiserName=\"" + ad.AdvertiserName + "\" poNumber=\"" + wo.Id + "\" agencyName=\"" + ag.AgencyName + "\">"; //<group name=\"" + group.GroupName + "\">
                
                foreach (WOGroupInfo group in wo.Groups)
                {
                    List<LibraryItemInfo> iscis = new List<LibraryItemInfo>();
                    List<string> dests = new List<string>();
                    xmlString += "<group name=\"" + group.GroupName + "\">";
                    List<TaskInfo> groupTasks = new List<TaskInfo>();
                    foreach(TaskInfo task in tasks)
                    {
                        if(task.WOGroupId == group.Id)
                        {
                            groupTasks.Add(task);
                        }
                    }
                    foreach (TaskInfo task in groupTasks)
                    {
                        if (task.DeliveryOrderId == "" && task.DeliveryStatus != "COMPLETE" && !task.isComplete && !task.isDeleted && task.DeliveryStatus.ToLower() != "closed manually")
                        {
                            if (task.DeliveryMethod.ToLower() == "addelivery")
                            {
                                c++;
                                var lib = libs.FirstOrDefault(o => o.Id == task.LibraryId);
                                if (lib != null)
                                {
                                    iscis.Add(lib);
                                }
                                var wogroupStat = group.WOGroupStations.FirstOrDefault(o => o.Id == task.StationId);
                                if (wogroupStat != null)
                                {
                                    var stat = stats.FirstOrDefault(o => o.Id == wogroupStat.StationId);
                                    if (stat != null)
                                    {
                                        if (stat.AdDeliveryCallLetters != "")
                                        {
                                            dests.Add(stat.AdDeliveryCallLetters);
                                        }
                                        else
                                        {
                                            dests.Add(stat.CallLetter);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    List<LibraryItemInfo> iscisFinal = iscis.Distinct().ToList();
                    List<string> destsFinal = dests.Distinct().ToList();
                    foreach (LibraryItemInfo isci in iscisFinal)
                    {
                        string dur = fixLength(isci.MediaLength);
                        string[] parts = dur.Split(':');
                        int length = 0;
                        if (parts.Length == 2)
                        {
                            length = Convert.ToInt32(parts[0]) * 60 + Convert.ToInt32(parts[1]);
                        }
                        createComcastSpot(isci.ISCICode, isci.Title + " - " + isci.ProductDescription, ad.AdvertiserName, "", ag.AgencyName, length, isci.MediaType, token);
                        xmlString += "<spotIsci>" + isci.ISCICode + "</spotIsci>";
                    }
                    foreach (string dest in destsFinal)
                    {
                        xmlString += "<destinationCode>" + dest + "</destinationCode>";
                    }
                    xmlString += "</group>";
                }
                xmlString += "</order></storeOrderRequest>";
                if(c>0)
                {
                    XmlDocument doc = new XmlDocument();
                    string returned = MakeComcastRequest(xmlString);
                    if (returned.IndexOf("errorCode") == -1)
                    {
                        doc.LoadXml(returned);
                        XmlElement xelRoot = doc.DocumentElement;
                        if (xelRoot != null)
                        {
                            orderId = xelRoot.Attributes["id"].Value;
                            foreach (TaskInfo task in tasks)
                            {
                                if (task.DeliveryMethod.ToLower() == "addelivery")
                                {
                                    task.DeliveryMethodResponse = returned;
                                    task.DeliveryOrderId = orderId;
                                    task.DeliveryOrderDateCreated = DateTime.Now;
                                    task.DeliveryStatus = "PENDING";
                                    Update_Task(task);
                                    WOCommentInfo comment = new WOCommentInfo();
                                    comment.CommentType = CommentTypeEnum.SystemMessage;
                                    comment.PortalId = PortalId;
                                    comment.WorkOrderId = wo.Id;
                                    comment.WOTaskId = task.Id;
                                    comment.Comment = "Comcast order created: " + returned;
                                    Add_WOComment(comment);
                                }
                            }
                        }
                        xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><submitOrderRequest authToken=\"" + token + "\" id=\"" + orderId + "\"></submitOrderRequest>";
                        MakeComcastRequest(xmlString);
                    }
                    else
                    {
                        orderId = "API CALL FAILED";
                        WOCommentInfo comment = new WOCommentInfo();
                        comment.CommentType = CommentTypeEnum.Error;
                        comment.Comment = "AdDelivery API Call Failed: " + returned;
                        comment.WorkOrderId = wo.Id;
                        comment.WOTaskId = tasks[0].Id;
                        Add_WOComment(comment);
                    }
                }
            }
            return orderId;
        }
        public string getComcastOrderStatus(TaskInfo task, string token = "")
        {
            if (token == "")
                token = getComcastAuthToken();
            string xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><orderRequest authToken=\"" + token + "\"><order id=\"" + task.DeliveryOrderId + "\"></order></orderRequest>";
            string status = "";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(MakeComcastRequest(xmlString));
            XmlElement xelRoot = doc.DocumentElement;
            if (xelRoot != null && xelRoot.InnerXml.ToLower().IndexOf("error") == -1)
            {
                XmlNodeList nodes = xelRoot.GetElementsByTagName("order");
                if (nodes[0] != null)
                {
                    status = nodes[0].Attributes["status"].Value;
                    if(status=="COMPLETED")
                    {
                        task.DeliveryStatus = "COMPLETE";
                        task.DeliveryOrderDateComplete = DateTime.Now;
                        task.LastModifiedDate = DateTime.Now;
                        Update_Task(task);
                    }
                }
            }
            return status;
        }
        public string deleteComcastOrder(TaskInfo task, string token = "")
        {
            if (token == "")
                token = getComcastAuthToken();
            string xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><deleteOrderRequest authToken=\"" + token + "\"><order id=\"" + task.DeliveryOrderId + "\"></order></deleteOrderRequest>";
            string status = "";
            XmlDocument doc = new XmlDocument();
            string returned = MakeComcastRequest(xmlString);
            doc.LoadXml(returned);
            
            return status;
        }
        #endregion
        #region Javelin API Calls
        public string AddJavelinOrder(int TaskId, int PortalId)
        {
            string returnMe = "";
            string JavelinEndpoint = ConfigurationManager.AppSettings["JavelinEndpoint"];
            string JavelinLogin = ConfigurationManager.AppSettings["JavelinLogin"];
            string JavelinUsername = ConfigurationManager.AppSettings["JavelinUsername"];
            string JavelinPassword = ConfigurationManager.AppSettings["JavelinPassword"];
            string vendor = "PMT";
            int JavelinDeliveryId = 5;            
            string service = "SubmitOrder";
            TaskInfo task = Get_TaskById(TaskId);
            WorkOrderInfo wo = Get_WorkOrderById(task.WorkOrderId);
            if (task.DeliveryMethodId != JavelinDeliveryId)
            {
                returnMe = "Error: Non Javelin order routed to Javelin API call.";
            }
            else
            {
                //get session guid
                string requestData = "username=" + JavelinUsername + "&password=" + JavelinPassword;
                WebClient loginClient = new WebClient();
                loginClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string loginResp = loginClient.UploadString(JavelinLogin, requestData);
                loginClient.Dispose();
                dynamic stuff = JsonConvert.DeserializeObject(loginResp);
                string sessionGUID = stuff.session_guid;

                StationInfo station = Get_StationById(task.StationId);
                LibraryItemInfo lib = Get_LibraryItemById(task.LibraryId);
                AdvertiserInfo advertiser = Get_AdvertiserById(lib.AdvertiserId);
                string[] durs = lib.MediaLength.Split(':');
                int seconds = 0;
                try
                {
                    seconds = (Convert.ToInt32(durs[0]) * 60) + Convert.ToInt32(durs[1]);
                }
                catch { }
                string callLetters = station.CallLetter;
                if(station.JavelinCallLetters!=null && station.JavelinCallLetters!="")
                {
                    callLetters = station.JavelinCallLetters;
                }
                AgencyInfo agency = Get_AgencyById(lib.AgencyId);
                string query = "callletters=[\"" + callLetters + "\"]&omsordernumber=" + task.Id.ToString() + "&vendor=" + vendor + "&isci=" + lib.ISCICode + "&name=" + lib.Title + "&duration=" + seconds.ToString() + "&agency=" + agency.AgencyName + "&project=" + wo.Description + "&wholesaler=" + vendor + "&advertiser=" + advertiser.AdvertiserName + "&service=2&billable=on&brand=" + advertiser.AdvertiserName;
                string url = JavelinEndpoint + service;
                WebClient client = new WebClient();
                client.Headers.Add("session_guid", sessionGUID);
                string resp = client.UploadString(url, query);
                try
                {
                    dynamic orderResponse = JsonConvert.DeserializeObject(resp);
                    task.DeliveryMethodResponse = resp;
                    dynamic record = orderResponse.records[0];
                    task.DeliveryOrderId = record.jav_order_num;
                }
                catch { }
                task.DeliveryOrderDateCreated = DateTime.Now;
                task.LastModifiedDate = DateTime.Now;
                task.DeliveryStatus = "PENDING";
                Update_Task(task);

                returnMe = resp;//orderResponse.success.ToString();
                client.Dispose();
            }
            MakeComment(PortalId, task.WorkOrderId, task.Id, 1, "System", "Javelin call made for Task Id: " + TaskId.ToString() + ". Result: " + returnMe, CommentTypeEnum.SystemMessage);
            return returnMe;
        }
        public string GetJavelinOrderStatus(int TaskId)
        {
            string returnMe = "";
            string JavelinEndpoint = ConfigurationManager.AppSettings["JavelinEndpoint"];
            string JavelinLogin = ConfigurationManager.AppSettings["JavelinLogin"];
            string JavelinUsername = ConfigurationManager.AppSettings["JavelinUsername"];
            string JavelinPassword = ConfigurationManager.AppSettings["JavelinPassword"];
            int JavelinDeliveryId = 5;
            string service = "GetOrderDetail";
            TaskInfo task = Get_TaskById(TaskId);
            if (task.DeliveryMethodId != JavelinDeliveryId)
            {
                returnMe = "Error: Non Javelin order routed to Javelin API call.";
            }
            else
            {
                //get session guid
                string requestData = "username=" + JavelinUsername + "&password=" + JavelinPassword;
                WebClient loginClient = new WebClient();
                loginClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string loginResp = loginClient.UploadString(JavelinLogin, requestData);
                loginClient.Dispose();
                dynamic stuff = JsonConvert.DeserializeObject(loginResp);
                string sessionGUID = stuff.session_guid;
                string query = "&javOrderId=" + task.DeliveryOrderId;
                string url = JavelinEndpoint + service;

                WebClient client = new WebClient();
                client.Headers.Add("session_guid", sessionGUID);
                string resp = client.DownloadString(url + query);
                dynamic status = JsonConvert.DeserializeObject(resp);
                try
                {
                    task.DeliveryStatus = status.records[0].status;
                }
                catch { MakeComment(0, task.WorkOrderId, task.Id, 1, "System", "Get Status Failed for Task# " + task.Id.ToString() + ", Javelin Order# " + task.DeliveryMethodId.ToString(), CommentTypeEnum.Error); }
                if(task.DeliveryStatus=="COMPLETE")
                {
                    if(!task.isComplete)
                    {
                        task.DeliveryOrderDateComplete = DateTime.Now;
                    }
                    task.isComplete = true;
                }
                task.DeliveryMethodResponse = resp;
                //task.LastModifiedDate = Convert.ToDateTime(status.records[0].date);
                task.LastModifiedDate = DateTime.Now;
                Update_Task(task);
                try
                {
                    returnMe = status.records[0].status;
                }
                catch { }
            }
            return returnMe;
        }
        public string DeleteJavelinOrder(int TaskId, int PortalId)
        {
            string returnMe = "";
            string JavelinEndpoint = ConfigurationManager.AppSettings["JavelinEndpoint"];
            string JavelinLogin = ConfigurationManager.AppSettings["JavelinLogin"];
            string JavelinUsername = ConfigurationManager.AppSettings["JavelinUsername"];
            string JavelinPassword = ConfigurationManager.AppSettings["JavelinPassword"];
            int JavelinDeliveryId = 5;
            string service = "CancelExternalOrder";
            TaskInfo task = Get_TaskById(TaskId);
            if (task.DeliveryMethodId != JavelinDeliveryId)
            {
                returnMe = "Error: Non Javelin order routed to Javelin API call.";
            }
            else
            {
                //get session guid
                string requestData = "username=" + JavelinUsername + "&password=" + JavelinPassword;
                WebClient loginClient = new WebClient();
                loginClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string loginResp = loginClient.UploadString(JavelinLogin, requestData);
                loginClient.Dispose();
                dynamic stuff = JsonConvert.DeserializeObject(loginResp);
                string sessionGUID = stuff.session_guid;
                string query = "&id=" + task.DeliveryOrderId;
                string url = JavelinEndpoint + service;

                WebClient client = new WebClient();
                client.Headers.Add("session_guid", sessionGUID);
                string resp = client.DownloadString(url + query);
                dynamic status = JsonConvert.DeserializeObject(resp);
                task.DeliveryMethodResponse = resp;
                task.DeliveryStatus = "CANCELLED";
                Update_Task(task);
                if (status.iscancelled == "1")
                    returnMe = "Cancelled Successfully.";
                else
                    returnMe = resp;
            }
            MakeComment(PortalId, task.WorkOrderId, task.Id, 1, "System", "Javelin call made to cancel order for Task Id: " + TaskId.ToString() + ". Result: " + returnMe, CommentTypeEnum.SystemMessage);
            return returnMe;
        }
        #endregion
        #region OTSM API Calls
        public string getOTSMToken()
        {
            string OTSMEndpoint = ConfigurationManager.AppSettings["OTSMEndpoint"];
            string OTSMUsername = ConfigurationManager.AppSettings["OTSMUsername"];
            string OTSMPassword = ConfigurationManager.AppSettings["OTSMPassword"];
            string requestData = "grant_type=password&username=" + OTSMUsername + "&password=" + OTSMPassword;
            WebClient loginClient = new WebClient();
            loginClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            string loginResp = loginClient.UploadString(OTSMEndpoint + "Token", requestData);
            loginClient.Dispose();
            var jobject = Newtonsoft.Json.Linq.JObject.Parse(loginResp);
            return jobject.GetValue("access_token").ToString();
        }
        public string getOTSMSpotStatus(string isci, string token = "")
        {
            string OTSMEndpoint = ConfigurationManager.AppSettings["OTSMEndpoint"];
            if(token == "")
            {
                token = getOTSMToken();
            }
            WebClient loginClient = new WebClient();
            loginClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            loginClient.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
            string loginResp = loginClient.DownloadString(OTSMEndpoint + "api/Spots/" + isci);
            loginClient.Dispose();
            var jobject = Newtonsoft.Json.Linq.JObject.Parse(loginResp);
            string status = "";
            if(jobject.GetValue("Status") != null)
            {
                status = jobject.GetValue("Status").ToString();
            }
            return status;
        }
        public string addOTSMSpot(TaskInfo task, string token = "")
        {
            string OTSMEndpoint = ConfigurationManager.AppSettings["OTSMEndpoint"];
            WorkOrderInfo wo = Get_WorkOrderById(task.WorkOrderId);
            AdvertiserInfo ad = Get_AdvertiserById(wo.AdvertiserId);
            AgencyInfo ag = Get_AgencyById(wo.AgencyId);
            LibraryItemInfo lib = Get_LibraryItemById(task.LibraryId);
            if (token == "")
            {
                token = getOTSMToken();
            }
            string dur = fixLength(lib.MediaLength);
            string[] parts = dur.Split(':');
            int length = 0;
            if (parts.Length == 2)
            {
                length = Convert.ToInt32(parts[0]) * 60 + Convert.ToInt32(parts[1]);
                length = length * 1000;
                if(length>120000)
                {
                    length = 120000;
                }
            }
            string data = "{ \"Agency\" : \"" + ag.AgencyName + "\", \"Advertiser\" : \"" + ad.AdvertiserName + "\", \"Brand\" : \"" + ad.AdvertiserName + "\", \"ISCI\" : \"" + lib.ISCICode + "\", \"Title\" : \"" + lib.Title + " - " + lib.ProductDescription + "\", \"Duration\" : " + length + ", \"MediaType\" : \"V\" }";
            WebClient loginClient = new WebClient();
            loginClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            loginClient.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
            string loginResp = loginClient.UploadString(OTSMEndpoint + "api/Spots", data);
            loginClient.Dispose();
            var jobject = Newtonsoft.Json.Linq.JObject.Parse(loginResp);
            string status = "";
            if (jobject.GetValue("StatusDescription") != null)
            {
                status = jobject.GetValue("StatusDescription").ToString();
            }
            return status;
        }
        public string addOTSMOrder(TaskInfo task, string token = "")
        {
            string OTSMEndpoint = ConfigurationManager.AppSettings["OTSMEndpoint"];
            WorkOrderInfo wo = Get_WorkOrderById(task.WorkOrderId);
            WOGroupInfo wogroup = Get_WorkOrderGroupById(task.WOGroupId);
            AdvertiserInfo ad = Get_AdvertiserById(wo.AdvertiserId);
            AgencyInfo ag = Get_AgencyById(wo.AgencyId);
            LibraryItemInfo lib = Get_LibraryItemById(task.LibraryId);
            WOGroupStationInfo groupStation = Get_WorkOrderGroupStationById(task.StationId);
            StationInfo station = Get_StationById(groupStation.StationId);
            if (token == "")
            {
                token = getOTSMToken();
            }
            //string spotStatus = getOTSMSpotStatus(lib.ISCICode, token);
            //if(spotStatus == "")
            //{
            //    addOTSMSpot(task, token);
            //}
            string dur = fixLength(lib.MediaLength);
            string[] parts = dur.Split(':');
            int length = 0;
            if (parts.Length == 2)
            {
                length = Convert.ToInt32(parts[0]) * 60 + Convert.ToInt32(parts[1]);
                length = length * 1000;
                if (length > 120000)
                {
                    length = 120000;
                }
            }
            string callLetters = station.OTSMHDCallLetters;
            if(lib.MediaType=="SD")
            {
                callLetters = station.OTSMSDCallLetters;
            }
            if(callLetters == "")
            {
                callLetters = station.CallLetter;
            }
            string data = "{ \"OrderType\" : \"V\", \"Traffic\" : 2, \"Agency\" : \"" + ag.AgencyName + "\", \"Advertiser\" : \"" + ad.AdvertiserName + "\", \"Brand\" : \"" + ad.AdvertiserName + "\", \"OrderBy\" : \"PMT\", \"PoNumber\" : \"" + wo.PONumber + "\", \"RefId\" : \"" + wo.Id.ToString() + "\", \"Group\" : [{ \"Name\" : \"" + wogroup.GroupName + "\", \"Spot\" : [ { \"Agency\" : \"" + ag.AgencyName + "\", \"Advertiser\" : \"" + ad.AdvertiserName + "\", \"Brand\" : \"" + ad.AdvertiserName + "\", \"ISCI\" : \"" + lib.ISCICode + "\", \"Title\" : \"" + lib.Title.Replace(".","") + " - " + lib.ProductDescription + "\", \"Duration\" : " + length + " } ], \"Destination\": [ \"" + callLetters + "\" ] }] }";
            WebClient loginClient = new WebClient();
            loginClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            loginClient.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
            string loginResp = loginClient.UploadString(OTSMEndpoint + "api/Orders", data);
            loginClient.Dispose();
            var jobject = Newtonsoft.Json.Linq.JObject.Parse(loginResp);
            if(loginResp.ToLower().IndexOf("error")!=-1)
            {
                WOCommentInfo comment = new WOCommentInfo();
                comment.Comment = "Error submitting OTSM Order: " + loginResp;
                comment.CommentType = CommentTypeEnum.Error;
                comment.WorkOrderId = task.WorkOrderId;
                comment.WOTaskId = task.Id;
                Add_WOComment(comment);
            }
            string status = "";
            if (jobject.GetValue("OrderID") != null)
            {
                status = jobject.GetValue("OrderID").ToString();
            }
            task.DeliveryMethodResponse = loginResp;
            task.DeliveryOrderId = status;
            Update_Task(task);
            return status;
        }
        public string getOTSMOrderStatus(TaskInfo task, string token="")
        {
            string OTSMEndpoint = ConfigurationManager.AppSettings["OTSMEndpoint"];
            if (token == "")
            {
                token = getOTSMToken();
            }
            WebClient loginClient = new WebClient();
            loginClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            loginClient.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
            string loginResp = loginClient.DownloadString(OTSMEndpoint + "api/Orders/" + task.DeliveryOrderId);
            loginClient.Dispose();
            var jobject = Newtonsoft.Json.Linq.JObject.Parse(loginResp);
            string status = "";
            if (jobject.GetValue("OrderStatus") != null)
            {
                status = jobject.GetValue("OrderStatus").ToString();
            }
            return status;
        }
        public string deleteOTSMOrder(string orderId, string token = "")
        {
            string OTSMEndpoint = ConfigurationManager.AppSettings["OTSMEndpoint"];
            if (token == "")
            {
                token = getOTSMToken();
            }
            WebClient loginClient = new WebClient();
            loginClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            loginClient.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
            string loginResp = loginClient.UploadString(OTSMEndpoint + "api/Orders/" + orderId, "DELETE", "");
            loginClient.Dispose();
            var jobject = Newtonsoft.Json.Linq.JObject.Parse(loginResp);
            string status = "";
            if (jobject.GetValue("StatusDescription") != null)
            {
                status = jobject.GetValue("StatusDescription").ToString();
            }
            return status;
        }
        #endregion
        #region Shipping API Calls
        public Address createEasyPostAddress(StationInfo station, bool saveId = true)
        {
            string returnId = "";
            string mode = "test";
            string key = ConfigurationManager.AppSettings["EasyPostToken"];
            if(station.Phone == "")
            {
                station.Phone = ConfigurationManager.AppSettings["PMTPhone"];
            }
            Address address = new Address();
            try { mode=ConfigurationManager.AppSettings["EasyPostMode"]; }
            catch { }
            EasySpotObjectInfo epo = Get_EasySpotObjectByIdAndType(station.Id, EasySpotTypeEnum.Address);
            if (epo.EasySpotObjectId != "")
            {
                //returnId = epo.EasySpotObjectId;
                EasyPost.ClientManager.SetCurrent(key);
                address = Address.Retrieve(epo.EasySpotObjectId);
            }
            else
            {                
                string country = "US";
                if(station.Country!="")
                {
                    country = station.Country;
                }
                EasyPost.ClientManager.SetCurrent(key);
                try
                {
                    address = Address.Create(
                        new Dictionary<string, object>()
                    {
                        {"street1", station.Address1},
                        {"street2", station.Address2},
                        {"city", station.City},
                        {"state", station.State},
                        {"zip", station.Zip},
                        {"country", country},
                        {"company", station.StationName},
                        {"phone", station.Phone},
                        {"email", station.Email},
                        {"name", station.AttentionLine}
                    });//new Address();
                    epo.EasySpotObjectId = address.id;
                    epo.ObjectType = EasySpotTypeEnum.Address;
                    epo.PMTObjectId = station.Id;
                    if (saveId)
                    {
                        Add_EasySpotObject(epo);
                    }
                }
                catch (EasyPost.HttpException e)
                {
                    returnId = e.Code + ", " + e.StatusCode + ", " + e.Message;
                }
            }
            return address;
        }
        public Address getPMTAddress()
        {
            Address address = new Address();
            EasySpotObjectInfo epo = Get_EasySpotObjectByIdAndType(-1, EasySpotTypeEnum.Address);
            if(epo.EasySpotObjectId=="")
            {
                StationInfo stat = new StationInfo();
                stat.Address1 = ConfigurationManager.AppSettings["PMTStreet1"];
                stat.Address2 = ConfigurationManager.AppSettings["PMTStreet2"];
                stat.City = ConfigurationManager.AppSettings["PMTCity"];
                stat.State = ConfigurationManager.AppSettings["PMTState"];
                stat.Zip = ConfigurationManager.AppSettings["PMTZip"];
                stat.Phone = ConfigurationManager.AppSettings["PMTPhone"];
                stat.Email = ConfigurationManager.AppSettings["PMTEmail"];
                stat.AttentionLine = ConfigurationManager.AppSettings["PMTName"];
                address = createEasyPostAddress(stat);
                epo.EasySpotObjectId = address.id;
                epo.ObjectType = EasySpotTypeEnum.Address;
                epo.PMTObjectId = -1;
                Add_EasySpotObject(epo);
            }
            else
            {
                address = Address.Retrieve(epo.EasySpotObjectId);
            }
            return address;
        }

        public Parcel getEasyPostParcel(int id, double weight, string PackageName = "")
        {
            Parcel parcel = new Parcel();
            EasySpotObjectInfo epo = Get_EasySpotObjectByIdAndType(id, EasySpotTypeEnum.Parcel);
            string mode = "test";
            string key = ConfigurationManager.AppSettings["EasyPostToken"];
            try { mode = ConfigurationManager.AppSettings["EasyPostMode"]; }
            catch { }
            //if(mode == "test")
            //{
            //    PackageName = "Parcel";
            //}
            EasyPost.ClientManager.SetCurrent(key);
            if(epo.EasySpotObjectId=="")
            {
                //should only need to do this once per environment                
                parcel = Parcel.Create(new Dictionary<string, object>() {
                  {"weight", weight}, {"predefined_package", PackageName}
                });
                epo.EasySpotObjectId = parcel.id;
                epo.PMTObjectId = id;
                epo.ObjectType = EasySpotTypeEnum.Parcel;
                Add_EasySpotObject(epo);
            }
            else
            {
                parcel = Parcel.Retrieve(epo.EasySpotObjectId);
            }
            return parcel;
        }
        public Parcel getOnePoundFedExPak()
        {
            return getEasyPostParcel(-2, 1.0, "FedExPak");
        }
        public Parcel getTwoPoundFedExPak()
        {
            return getEasyPostParcel(-3, 2.0, "FedExPak");
        }
        public Parcel getOnePoundUPSPak()
        {
            return getEasyPostParcel(-4, 1.0, "Pak");
        }
        public Parcel getTwoPoundUPSPak()
        {
            return getEasyPostParcel(-5, 2.0, "Pak");
        }

        public string createEasySpotOrder(List<WOGroupInfo> groups)
        {
            WorkOrderInfo wo = new WorkOrderInfo();
            if(groups.Count()>0)
            {
                wo = Get_WorkOrderById(groups[0].WorkOrderId);
            }
            List<TaskInfo> woTasks = Get_TasksByWOId(wo.Id);
            string message = "";
            //need to figure out if using PMT's account or Bill To's
            AdvertiserInfo billTo = new AdvertiserInfo();
            if(wo.BillToId==-1)
            {
                billTo = Get_AdvertiserById(wo.AdvertiserId);
            }
            else
            {
                billTo = Get_AdvertiserById(wo.BillToId);
            }
            foreach(WOGroupInfo group in groups)
            {                
                if (group.GroupType == GroupTypeEnum.Bundle || group.GroupType == GroupTypeEnum.Delivery)
                {
                    foreach (WOGroupStationInfo station in group.WOGroupStations)
                    {
                        if (station.DeliveryMethod.IndexOf("tf_") != -1)
                        {
                            //make shipping order
                            TapeFormatInfo tapeformat = Get_TapeFormatById(Convert.ToInt32(station.DeliveryMethod.Replace("tf_", "")));
                            int tapesToSend = station.Quantity * group.LibraryItems.Count();
                            //int tapesLeft = tapesToSend;
                            //deliverymethod 1 = FedEx, 2 = UPS
                            int deliveryMethod = 1;
                            double totalWeight = tapesToSend * tapeformat.Weight;
                            var thisTask = woTasks.FirstOrDefault(o => o.StationId == station.Id && o.WOGroupId == group.Id);
                            if (thisTask != null && thisTask.DeliveryStatus != "COMPLETE" && thisTask.DeliveryStatus != "CANCELLED" || (thisTask.DeliveryStatus == "PENDING" && thisTask.DeliveryOrderId == ""))
                            {
                                //deliveryMethod = thisTask.DeliveryMethodId;
                                if(thisTask.DeliveryMethod.ToLower().IndexOf("ups")!=-1)
                                {
                                    deliveryMethod = 2;
                                }
                                Parcel parcel = new Parcel();
                                if (totalWeight <= 1.0)
                                {
                                    if (deliveryMethod == 1)
                                    {
                                        parcel = getOnePoundFedExPak();
                                    }
                                    else if (deliveryMethod == 2)
                                    {
                                        parcel = getOnePoundUPSPak();
                                    }
                                }
                                else
                                {
                                    if (deliveryMethod == 1)
                                    {
                                        parcel = getTwoPoundFedExPak();
                                    }
                                    else if (deliveryMethod == 2)
                                    {
                                        parcel = getTwoPoundUPSPak();
                                    }
                                }
                                //int twoPoundMax = (int)Math.Floor(2.0 / tapeformat.Weight);
                                //if(twoPoundMax>tapeformat.MaxPerPak)
                                //{
                                //    twoPoundMax = tapeformat.MaxPerPak;
                                //}
                                //int onePoundMax = (int)Math.Floor(1.0 / tapeformat.Weight);
                                //if (onePoundMax > tapeformat.MaxPerPak)
                                //{
                                //    onePoundMax = tapeformat.MaxPerPak;
                                //}
                                //int twoPoundPaks = 0;
                                //int onePoundPaks = 0;
                                //while(tapesLeft>0)
                                //{
                                //    if(tapesLeft>=twoPoundMax)
                                //    {
                                //        twoPoundPaks++;
                                //        tapesLeft = tapesLeft - twoPoundMax;
                                //    }
                                //    else
                                //    {
                                //        //need to see if it goes in 1 lb or 2 lb pak
                                //        if(tapesLeft * tapeformat.Weight > 1.0)
                                //        {
                                //            twoPoundPaks++;
                                //        }
                                //        else
                                //        {
                                //            onePoundPaks++;
                                //        } 
                                //        tapesLeft = 0;
                                //    }
                                //}
                                //message += "Station: " + station.Id.ToString() + ": Two Pound Paks: " + twoPoundPaks.ToString() + ", One Pound Paks: " + onePoundPaks.ToString() + ". ";
                                Address from = getPMTAddress();
                                bool saveId = false;
                                StationInfo refStation = Get_StationById(station.StationId);
                                if (refStation.Address1 == station.Station.Address1 &&
                                   refStation.Address2 == station.Station.Address2 &&
                                   refStation.City == station.Station.City &&
                                   refStation.State == station.Station.State &&
                                   refStation.Zip == station.Station.Zip &&
                                   refStation.AttentionLine == station.Station.AttentionLine && refStation.Phone == station.Station.Phone)
                                {
                                    saveId = true;
                                }
                                Address to = createEasyPostAddress(station.Station, saveId);
                                Shipment shipment = new Shipment();
                                shipment.to_address = to;
                                shipment.from_address = from;
                                shipment.parcel = parcel;
                                LibraryItemInfo lib = Get_LibraryItemById(thisTask.LibraryId);
                                Options options = new Options();
                                options.print_custom_1 = group.WorkOrderId.ToString();
                                options.print_custom_1_code = "PO";
                                options.print_custom_2 = lib.Title + " - " + tapeformat.TapeFormat;
                                // will need to implement carrier accounts when in production.  Doesn't work in testing
                                //if (station.DeliveryMethodId != -1)
                                if((station.DeliveryMethod.IndexOf("tf_")!=-1 && station.DeliveryMethod.Replace("tf_","")!="-1") || station.DeliveryMethodId != -1)
                                {
                                    //not using pmt's fedex
                                    if (billTo.CarrierNumber != "")
                                    {
                                        options.bill_third_party_account = billTo.CarrierNumber;
                                        if (deliveryMethod == 2)
                                        {
                                            //need to set UPS billto country and postalcode
                                            options.bill_third_party_country = billTo.Country;
                                            options.bill_third_party_postal_code = billTo.Zip;
                                        }
                                    }
                                }
                                shipment.options = options;
                                try { shipment.mode = ConfigurationManager.AppSettings["EasyPostMode"]; }
                                catch { }
                                //CarrierAccount ca = new CarrierAccount();
                                //ca.id = ConfigurationManager.AppSettings["EasyPostFedExAccountId"];
                                //List<CarrierAccount> cas = new List<CarrierAccount>();
                                //cas.Add(ca);
                                //shipment.carrier_accounts = cas;
                                shipment.Create();
                                //shipment.GetRates();
                                string ratename = "";
                                string intlRatename = "";
                                if (station.PriorityId == 1)
                                {
                                    if (station.ShippingMethodId == 1 || station.ShippingMethodId == -1)
                                    {
                                        //fedex
                                        ratename = "PRIORITY_OVERNIGHT";
                                        intlRatename = "INTERNATIONAL_PRIORITY";
                                    }
                                    else if (station.ShippingMethodId == 2)
                                    {
                                        //ups
                                        ratename = "NextDayAirEarlyAM";
                                        intlRatename = "Expedited";
                                    }
                                    //try
                                    //{
                                    //    //shipment.Buy(shipment.LowestRate());
                                    //}
                                    //catch (Exception e) { MakeComment(wo.PortalId, wo.Id, thisTask.Id, 0, "System", "Error buying shipment: " + e.Message, CommentTypeEnum.Error); }
                                    //catch { }
                                }
                                else if (station.PriorityId == 2)
                                {
                                    if (station.ShippingMethodId == 1 || station.ShippingMethodId == -1)
                                    {
                                        //fedex
                                        ratename = "STANDARD_OVERNIGHT";
                                        intlRatename = "INTERNATIONAL_ECONOMY";
                                    }
                                    else if (station.ShippingMethodId == 2)
                                    {
                                        //ups
                                        ratename = "NextDayAir";
                                        intlRatename = "UPSSaver";
                                    }
                                }
                                else if (station.PriorityId == 3)
                                {
                                    if (station.ShippingMethodId == 1 || station.ShippingMethodId == -1)
                                    {
                                        //fedex
                                        ratename = "FEDEX_2_DAY";
                                        intlRatename = "INTERNATIONAL_ECONOMY";
                                    }
                                    else if (station.ShippingMethodId == 2)
                                    {
                                        //ups
                                        ratename = "2ndDayAir";
                                        intlRatename = "UPSSaver";
                                    }
                                }
                                else if (station.PriorityId == 4)
                                {
                                    if (station.ShippingMethodId == 2)
                                    {
                                        //ups
                                        ratename = "2ndDayAirAM";
                                        intlRatename = "UPSSaver";
                                    }
                                }
                                string buyRate = "";
                                foreach (Rate rate in shipment.rates)
                                {
                                    if (rate.service == ratename || rate.service == intlRatename)
                                    {
                                        buyRate = rate.id;
                                        break;
                                    }
                                }
                                if (station.PriorityId == -1)
                                {
                                    MakeComment(wo.PortalId, wo.Id, thisTask.Id, 0, "System", "Task ID: " + thisTask.Id.ToString() + " does not have a Priority set.  Can't select rate and create shipment without priority.", CommentTypeEnum.Error);
                                }
                                else
                                {
                                    try
                                    {
                                        shipment.Buy(buyRate);
                                        thisTask.DeliveryMethodResponse = shipment.postage_label.label_url + "|";
                                        thisTask.DeliveryOrderId = shipment.tracking_code; // shipment.id;
                                        thisTask.DeliveryOrderDateCreated = DateTime.Now;
                                        foreach (Message msg in shipment.messages)
                                        {
                                            thisTask.DeliveryMethodResponse += msg.message + ". ";
                                        }
                                        thisTask.DeliveryStatus = "PENDING";
                                        Update_Task(thisTask);
                                        EasySpotObjectInfo epoNew = new EasySpotObjectInfo();
                                        epoNew.EasySpotObjectId = shipment.id;
                                        epoNew.PMTObjectId = thisTask.Id;
                                        epoNew.ObjectType = EasySpotTypeEnum.Shipment;
                                        Add_EasySpotObject(epoNew);
                                        MakeComment(wo.PortalId, wo.Id, thisTask.Id, 0, "System", "Shipment created: " + shipment.tracking_code, CommentTypeEnum.SystemMessage);
                                        message += "Success: Station " + station.Station.StationName + "<br />";
                                    }
                                    catch (Exception e)
                                    {
                                        MakeComment(wo.PortalId, wo.Id, thisTask.Id, 0, "System", "Error buying shipment: " + e.Message, CommentTypeEnum.Error);
                                        message += "Error: Station " + station.Station.StationName + ": " + e.Message + "<br />";
                                    }
                                }
                            }                            
                        } 
                    }
                }
            }
            return message;
        }

        public void deleteEasyPostShipment(TaskInfo task)
        {
            EasySpotObjectInfo epo = Get_EasySpotObjectByIdAndType(task.Id, EasySpotTypeEnum.Shipment);
        }

        #endregion
        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// -----------------------------------------------------------------------------
        //public string ExportModule(int ModuleID)
        //{
        //string strXML = "";

        //List<PMT_AdminInfo> colPMT_Admins = GetPMT_Admins(ModuleID);
        //if (colPMT_Admins.Count != 0)
        //{
        //    strXML += "<PMT_Admins>";

        //    foreach (PMT_AdminInfo objPMT_Admin in colPMT_Admins)
        //    {
        //        strXML += "<PMT_Admin>";
        //        strXML += "<content>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objPMT_Admin.Content) + "</content>";
        //        strXML += "</PMT_Admin>";
        //    }
        //    strXML += "</PMT_Admins>";
        //}

        //return strXML;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be imported</param>
        /// <param name="Content">The content to be imported</param>
        /// <param name="Version">The version of the module to be imported</param>
        /// <param name="UserId">The Id of the user performing the import</param>
        /// -----------------------------------------------------------------------------
        //public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        //{
        //XmlNode xmlPMT_Admins = DotNetNuke.Common.Globals.GetContent(Content, "PMT_Admins");
        //foreach (XmlNode xmlPMT_Admin in xmlPMT_Admins.SelectNodes("PMT_Admin"))
        //{
        //    PMT_AdminInfo objPMT_Admin = new PMT_AdminInfo();
        //    objPMT_Admin.ModuleId = ModuleID;
        //    objPMT_Admin.Content = xmlPMT_Admin.SelectSingleNode("content").InnerText;
        //    objPMT_Admin.CreatedByUser = UserID;
        //    AddPMT_Admin(objPMT_Admin);
        //}

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        /// -----------------------------------------------------------------------------
        //public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(DotNetNuke.Entities.Modules.ModuleInfo ModInfo)
        //{
        //SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();

        //List<PMT_AdminInfo> colPMT_Admins = GetPMT_Admins(ModInfo.ModuleID);

        //foreach (PMT_AdminInfo objPMT_Admin in colPMT_Admins)
        //{
        //    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objPMT_Admin.Content, objPMT_Admin.CreatedByUser, objPMT_Admin.CreatedDate, ModInfo.ModuleID, objPMT_Admin.ItemId.ToString(), objPMT_Admin.Content, "ItemId=" + objPMT_Admin.ItemId.ToString());
        //    SearchItemCollection.Add(SearchItem);
        //}

        //return SearchItemCollection;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpgradeModule implements the IUpgradeable Interface
        /// </summary>
        /// <param name="Version">The current version of the module</param>
        /// -----------------------------------------------------------------------------
        //public string UpgradeModule(string Version)
        //{
        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        #endregion

    }
    public static class ResponseHelper
    {
        public static void Redirect(this HttpResponse response, string url, string target, string windowFeatures)
        {

            if ((String.IsNullOrEmpty(target) || target.Equals("_self", StringComparison.OrdinalIgnoreCase)) && String.IsNullOrEmpty(windowFeatures))
            {
                response.Redirect(url);
            }
            else
            {
                Page page = (Page)HttpContext.Current.Handler;

                if (page == null)
                {
                    throw new InvalidOperationException("Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }
                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page, typeof(Page), "Redirect", script, true);
            }
        }
    }
}
