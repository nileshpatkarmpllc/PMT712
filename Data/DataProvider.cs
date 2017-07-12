/*
' Copyright (c) 2015 Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;


namespace Christoc.Modules.PMT_Admin
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// An abstract class for the data access layer
    /// 
    /// The abstract data provider provides the methods that a control data provider (sqldataprovider)
    /// must implement. You'll find two commented out examples in the Abstract methods region below.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public abstract class DataProvider
    {

        #region Shared/Static Methods

        private static DataProvider provider;

        // return the provider
        public static DataProvider Instance()
        {
            if (provider == null)
            {
                const string assembly = "Christoc.Modules.PMT_Admin.SqlDataprovider,PMT_Admin";
                Type objectType = Type.GetType(assembly, true, true);

                provider = (DataProvider)Activator.CreateInstance(objectType);
                DataCache.SetCache(objectType.FullName, provider);
            }

            return provider;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not returning class state information")]
        public static IDbConnection GetConnection()
        {
            const string providerType = "data";
            ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);

            Provider objProvider = ((Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);
            string _connectionString;
            if (!String.IsNullOrEmpty(objProvider.Attributes["connectionStringName"]) && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]]))
            {
                _connectionString = System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]];
            }
            else
            {
                _connectionString = objProvider.Attributes["connectionString"];
            }

            IDbConnection newConnection = new System.Data.SqlClient.SqlConnection();
            newConnection.ConnectionString = _connectionString.ToString();
            newConnection.Open();
            return newConnection;
        }

        #endregion

        #region Abstract methods

        public abstract int Add_Agency(AgencyInfo Agency);
        public abstract int Add_AgencyForImport(AgencyInfo Agency);
        public abstract void ClearAgencies();
        public abstract void Update_Agency(AgencyInfo Agency);
        public abstract void Delete_Agency(AgencyInfo Agency);
        public abstract IDataReader Get_AgenciesByPortalId(int PortalId);
        public abstract IDataReader Get_AgencyById(int Id);
        public abstract int Add_Advertiser(AdvertiserInfo Advertiser);
        public abstract int Add_AdvertiserForImport(AdvertiserInfo Advertiser);
        public abstract void ClearAdvertisers();
        public abstract void Update_Advertiser(AdvertiserInfo Advertiser);
        public abstract void Delete_Advertiser(AdvertiserInfo Advertiser);
        public abstract IDataReader Get_AdvertisersByPortalId(int PortalId);
        public abstract IDataReader Get_AdvertiserById(int Id);
        public abstract int Add_Market(MarketInfo Market);
        public abstract void Update_Market(MarketInfo Market);
        public abstract void Delete_Market(MarketInfo Market);
        public abstract IDataReader Get_MarketsByPortalId(int PortalId);
        public abstract IDataReader Get_MarketById(int Id);
        public abstract int Add_ClientType(ClientTypeInfo ClientType);
        public abstract void Update_ClientType(ClientTypeInfo ClientType);
        public abstract void Delete_ClientType(ClientTypeInfo ClientType);
        public abstract IDataReader Get_ClientTypesByPortalId(int PortalId);
        public abstract IDataReader Get_ClientTypeById(int Id);
        public abstract int Add_CarrierType(CarrierTypeInfo CarrierType);
        public abstract void Update_CarrierType(CarrierTypeInfo CarrierType);
        public abstract void Delete_CarrierType(CarrierTypeInfo CarrierType);
        public abstract IDataReader Get_CarrierTypesByPortalId(int PortalId);
        public abstract IDataReader Get_CarrierTypeById(int Id);
        public abstract int Add_FreightType(FreightTypeInfo FreightType);
        public abstract void Update_FreightType(FreightTypeInfo FreightType);
        public abstract void Delete_FreightType(FreightTypeInfo FreightType);
        public abstract IDataReader Get_FreightTypesByPortalId(int PortalId);
        public abstract IDataReader Get_FreightTypeById(int Id);
        public abstract int Add_TapeFormat(TapeFormatInfo TapeFormat);
        public abstract void Update_TapeFormat(TapeFormatInfo TapeFormat);
        public abstract void Delete_TapeFormat(TapeFormatInfo TapeFormat);
        public abstract IDataReader Get_TapeFormatsByPortalId(int PortalId);
        public abstract IDataReader Get_TapeFormatById(int Id);
        public abstract int Add_DeliveryMethod(DeliveryMethodInfo DeliveryMethod);
        public abstract void Update_DeliveryMethod(DeliveryMethodInfo DeliveryMethod);
        public abstract void Delete_DeliveryMethod(DeliveryMethodInfo DeliveryMethod);
        public abstract IDataReader Get_DeliveryMethodsByPortalId(int PortalId);
        public abstract IDataReader Get_DeliveryMethodById(int Id);
        public abstract int Add_Station(StationInfo Station);
        public abstract int Add_StationForImport(StationInfo Station);
        public abstract void ClearStations();
        public abstract void Update_Station(StationInfo Station);
        public abstract void Delete_Station(StationInfo Station);
        public abstract IDataReader Get_StationsByPortalId(int PortalId);
        public abstract IDataReader Get_StationsByPortalIdActive(int PortalId);
        public abstract IDataReader Get_StationById(int Id);
        public abstract int Add_StationGroup(StationGroupInfo StationGroup);
        public abstract void Update_StationGroup(StationGroupInfo StationGroup);
        public abstract void Delete_StationGroup(StationGroupInfo StationGroup);
        public abstract IDataReader Get_StationGroupsByPortalId(int PortalId);
        public abstract IDataReader Get_StationGroupById(int Id);
        public abstract IDataReader Get_StationGroupsByUserId(int UserId);
        public abstract IDataReader Get_StationsinGroupById(int Id);
        public abstract void Delete_StationsInGroup(int StationId, int StationGroupId);
        public abstract void Delete_StationsInGroupByGroup(int StationGroupId);
        public abstract int Add_StationsInGroup(int PortalId, int StationId, int StationGroupId);
        public abstract int Add_Label(LabelInfo Label);
        public abstract int Add_LabelForImport(LabelInfo Label);
        public abstract void ClearLabels();
        public abstract void Update_Label(LabelInfo Label);
        public abstract void Delete_Label(LabelInfo Label);
        public abstract IDataReader Get_LabelsByPortalId(int PortalId);
        public abstract IDataReader Get_LabelById(int Id);
        public abstract int Add_MasterItem(MasterItemInfo MasterItem);
        public abstract int Add_MasterItemForImport(MasterItemInfo MasterItem);
        public abstract void ClearMasterItems();
        public abstract void Update_MasterItem(MasterItemInfo MasterItem);
        public abstract void Delete_MasterItem(MasterItemInfo MasterItem);
        public abstract IDataReader Get_MasterItemsByPortalId(int PortalId);
        public abstract IDataReader Get_MasterItemById(int Id);
        public abstract void Add_AdvertiserAgency(int AdvertiserId, int AgencyId);
        public abstract void Delete_AdvertiserAgencyByAdvertiserId(int AdvertiserId);
        public abstract void Delete_AdvertiserAgenciesForImport();
        public abstract IDataReader Get_AgenciesByAdvertiserId(int AdvertiserId);
        public abstract IDataReader Get_AgencyIdsByAdvertiserId(int AdvertiserId);
        public abstract void Add_MasterItemAgency(int MasterItemId, int AgencyId);
        public abstract void ClearMasterItemAgencies();
        public abstract void Delete_MasterItemAgencyByMasterItemId(int MasterItemId);
        public abstract IDataReader Get_AgenciesByMasterItemId(int MasterItemId);
        public abstract IDataReader Get_AgencyIdsByMasterItemId(int MasterItemId);
        public abstract IDataReader Get_AdvertiserAgencies();
        public abstract IDataReader Get_MasterItemAgencies();
        public abstract void Add_UserInAgency(int UserId, int AgencyId);
        public abstract void Delete_UserInAgencies(int UserId);
        public abstract void Add_UserInAdvertiser(int UserId, int AdvertiserId);
        public abstract void Delete_UserInAdvertisers(int UserId);
        public abstract IDataReader Get_AgenciesByUser(int UserId);
        public abstract IDataReader Get_AdvertisersByUser(int UserId);
        public abstract string GetMaxPMTMediaId(int PortalId);
        public abstract string GetMaxLabelNumber(int PortalId);
        public abstract int Add_LibraryItem(LibraryItemInfo LibraryItem);
        public abstract int Add_LibraryItemForImport(LibraryItemInfo LibraryItem);
        public abstract void ClearLibraryItems();
        public abstract void Update_LibraryItem(LibraryItemInfo LibraryItem);
        public abstract void Delete_LibraryItem(LibraryItemInfo LibraryItem);
        public abstract IDataReader Get_LibraryItemsByPortalId(int PortalId);
        public abstract IDataReader Get_LibraryItemById(int Id);
        public abstract IDataReader Get_LibraryItemByISCI(string ISCI);
        public abstract int Add_Service(ServiceInfo Service);
        public abstract void Update_Service(ServiceInfo Service);
        public abstract void Delete_Service(ServiceInfo Service);
        public abstract IDataReader Get_ServicesByPortalId(int PortalId);
        public abstract IDataReader Get_ServiceById(int Id);
        public abstract int Add_WorkOrder(WorkOrderInfo WorkOrder);
        public abstract void Update_WorkOrder(WorkOrderInfo WorkOrder);
        public abstract void Delete_WorkOrder(WorkOrderInfo WorkOrder);
        public abstract IDataReader Get_WorkOrdersByPortalId(int PortalId);
        public abstract IDataReader Get_WorkOrderById(int Id);
        public abstract int Add_WorkOrderGroupStation(WOGroupStationInfo WorkOrderGroupStation);
        public abstract void Update_WorkOrderGroupStation(WOGroupStationInfo WorkOrderGroupStation);
        public abstract void Delete_WorkOrderGroupStation(WOGroupStationInfo WorkOrderGroupStation);
        public abstract void Delete_WorkOrderGroupStationByGroupId(int GroupId);
        public abstract void Delete_WorkOrderGroupStationByWOId(int WOId);
        public abstract IDataReader Get_WorkOrderGroupStationsByGroupId(int PortalId);
        public abstract IDataReader Get_WorkOrderGroupStationById(int Id);
        public abstract int Add_WorkOrderGroup(WOGroupInfo WorkOrderGroup);
        public abstract void Update_WorkOrderGroup(WOGroupInfo WorkOrderGroup);
        public abstract void Delete_WorkOrderGroup(WOGroupInfo WorkOrderGroup);
        public abstract IDataReader Get_WorkOrderGroupsByWOId(int WorkOrderId);
        public abstract IDataReader Get_WorkOrderGroupById(int Id);
        public abstract void DeleteWOGroupLibItemsByWOGroupId(int WorkOrderGroupId);
        public abstract void AddWOGroupLibItem(int WorkOrderGroupId, int LibraryItemId);
        public abstract IDataReader GetLibItemsByWOGroupId(int WOGroupId);
        public abstract void DeleteWOGroupServicesByWOGroupId(int WorkOrderGroupId);
        public abstract void AddWOGroupService(int WorkOrderGroupId, int ServiceId);
        public abstract IDataReader GetServicesByWOGroupId(int WOGroupId);
        public abstract IDataReader Get_AdvertisersByAgencyId(int AgencyId);
        public abstract int Add_WOComment(WOCommentInfo WOComment);
        public abstract void Update_WOComment(WOCommentInfo WOComment);
        public abstract void Delete_WOComment(WOCommentInfo WOComment);
        public abstract IDataReader Get_WOCommentsByWOId(int WorkOrderId);
        public abstract IDataReader Get_WOCommentById(int Id);
        public abstract int Add_Task(TaskInfo Task);
        public abstract int Add_TaskForImport(TaskInfo Task);
        public abstract void Update_Task(TaskInfo Task);
        public abstract void Delete_Task(TaskInfo Task);
        public abstract IDataReader Get_TasksByWOId(int WorkOrderId);
        public abstract IDataReader Get_OpenTasks();
        public abstract IDataReader Get_TaskById(int Id);
        public abstract int Add_QBCode(QBCodeInfo QBCode);
        public abstract void Update_QBCode(QBCodeInfo QBCode);
        public abstract void Delete_QBCode(QBCodeInfo QBCode);
        public abstract IDataReader Get_QBCodesByPortalId(int PortalId);
        public abstract IDataReader Get_QBCodeById(int Id);
        public abstract void Delete_QBCodeDeliveryMethodsByQBCodeId(int QBCodeId);
        public abstract void Add_QBCodeDeliveryMethodsByQBCodeId(int QBCodeId, int DeliveryMethodId);
        public abstract void Delete_QBCodeTapeFormatsByQBCodeId(int QBCodeId);
        public abstract void Add_QBCodeTapeFormatsByQBCodeId(int QBCodeId, int TapeFormatId);
        public abstract void Delete_QBCodeServicesByQBCodeId(int QBCodeId);
        public abstract void Add_QBCodeServicesByQBCodeId(int QBCodeId, int ServiceId);
        public abstract IDataReader Get_DeliveryMethodsByQBCodeId(int QBCodeId);
        public abstract IDataReader Get_TapeFormatsByQBCodeId(int QBCodeId);
        public abstract IDataReader Get_ServicesByQBCodeId(int QBCodeId);
        public abstract void Delete_TaskQBCodeByTaskId(int TaskId);
        public abstract void Add_TaskQBCode(int TaskId, int QBCodeId);
        public abstract IDataReader Get_QBCodesByTaskId(int TaskId);
        public abstract int Add_Report(ReportInfo Report);
        public abstract void Update_Report(ReportInfo Report);
        public abstract void Delete_Report(ReportInfo Report);
        public abstract IDataReader Get_ReportsByPortalId(int PortalId);
        public abstract IDataReader Get_ReportById(int Id);
        public abstract int Add_EasySpotObject(EasySpotObjectInfo EasySpotObject);
        public abstract IDataReader Get_EasySpotByIdAndType(int PMTObjectId, EasySpotTypeEnum ObjectType);
        public abstract int Add_Invoice(InvoiceInfo Invoice);
        public abstract void Update_Invoice(InvoiceInfo Invoice);
        public abstract void Delete_Invoice(InvoiceInfo Invoice);
        public abstract IDataReader Get_InvoicesByPortalId(int PortalId);
        public abstract IDataReader Get_InvoiceById(int Id);
        public abstract void Delete_WOInInvoiceByInvoiceId(int InvoiceId);
        public abstract void Add_WOInInvoice(int InvoiceId, int WOId);
        public abstract List<int> Get_WOInsByInvoiceId(int InvoiceId);
        public abstract List<int> Get_InvoicesByToSend();
        #endregion

    }

}