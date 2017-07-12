using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Collections.Generic;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;

namespace Christoc.Modules.PMT_Admin
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// SQL Server implementation of the abstract DataProvider class
    /// 
    /// This concreted data provider class provides the implementation of the abstract methods 
    /// from data dataprovider.cs
    /// 
    /// In most cases you will only modify the Public methods region below.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class SqlDataProvider : DataProvider
    {

        #region Private Members

        private const string ProviderType = "data";
        private const string ModuleQualifier = "PMT_Admin_";

        private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
        private readonly string _connectionString;
        private readonly string _providerPath;
        private readonly string _objectQualifier;
        private readonly string _databaseOwner;

        #endregion

        #region Constructors

        public SqlDataProvider()
        {

            // Read the configuration specific information for this provider
            Provider objProvider = (Provider)(_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);

            // Read the attributes for this provider

            //Get Connection string from web.config
            _connectionString = Config.GetConnectionString();

            if (string.IsNullOrEmpty(_connectionString))
            {
                // Use connection string specified in provider
                _connectionString = objProvider.Attributes["connectionString"];
            }

            _providerPath = objProvider.Attributes["providerPath"];

            _objectQualifier = objProvider.Attributes["objectQualifier"];
            if (!string.IsNullOrEmpty(_objectQualifier) && _objectQualifier.EndsWith("_", StringComparison.Ordinal) == false)
            {
                _objectQualifier += "_";
            }

            _databaseOwner = objProvider.Attributes["databaseOwner"];
            if (!string.IsNullOrEmpty(_databaseOwner) && _databaseOwner.EndsWith(".", StringComparison.Ordinal) == false)
            {
                _databaseOwner += ".";
            }

        }

        #endregion

        #region Properties

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public string ProviderPath
        {
            get
            {
                return _providerPath;
            }
        }

        public string ObjectQualifier
        {
            get
            {
                return _objectQualifier;
            }
        }

        public string DatabaseOwner
        {
            get
            {
                return _databaseOwner;
            }
        }

        // used to prefect your database objects (stored procedures, tables, views, etc)
        private string NamePrefix
        {
            get { return DatabaseOwner + ObjectQualifier + ModuleQualifier; }
        }

        #endregion

        #region Private Methods

        private static object GetNull(object field)
        {
            return Null.GetNull(field, DBNull.Value);
        }

        #endregion

        #region Public Methods

        public override int Add_Agency(AgencyInfo Agency)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddAgency", Agency.PortalId, Agency.AgencyName, Agency.Address1, Agency.Address2, Agency.City, Agency.State, Agency.Zip, Agency.Country, Agency.Phone, Agency.Fax, Agency.ClientType, Agency.Status, Agency.CustomerReference, Agency.AttentionLine, Agency.CreatedById, Agency.DateCreated, Agency.LastModifiedById, Agency.LastModifiedDate).ToString());
        }
        public override int Add_AgencyForImport(AgencyInfo Agency)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddAgencyForImport", Agency.Id, Agency.PortalId, Agency.AgencyName, Agency.Address1, Agency.Address2, Agency.City, Agency.State, Agency.Zip, Agency.Country, Agency.Phone, Agency.Fax, Agency.ClientType, Agency.Status, Agency.CustomerReference, Agency.AttentionLine, Agency.CreatedById, Agency.DateCreated, Agency.LastModifiedById, Agency.LastModifiedDate).ToString());
        }
        public override void Update_Agency(AgencyInfo Agency)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateAgency", Agency.Id, Agency.PortalId, Agency.AgencyName, Agency.Address1, Agency.Address2, Agency.City, Agency.State, Agency.Zip, Agency.Country, Agency.Phone, Agency.Fax, Agency.ClientType, Agency.Status, Agency.CustomerReference, Agency.AttentionLine, Agency.CreatedById, Agency.DateCreated, Agency.LastModifiedById, Agency.LastModifiedDate).ToString();
        }
        public override void ClearAgencies()
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_ClearAgencies");
        }
        public override void Delete_AdvertiserAgenciesForImport()
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteAdvertiserAgenciesForImport");
        }
        public override void Delete_Agency(AgencyInfo Agency)
        {
            if (Agency.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteAgency", Agency.Id);
            }
        }
        public override IDataReader Get_AgenciesByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_AgenciesByPortalId", PortalId);
        }
        public override IDataReader Get_AgencyById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_AgencyById", Id);
        }

        public override int Add_Advertiser(AdvertiserInfo Advertiser)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddAdvertiser", Advertiser.PortalId, Advertiser.Carrier, Advertiser.Freight, Advertiser.CarrierNumber, Advertiser.AdvertiserName, Advertiser.Address1, Advertiser.Address2, Advertiser.City, Advertiser.State, Advertiser.Zip, Advertiser.Country, Advertiser.Phone, Advertiser.Fax, Advertiser.ClientType, Advertiser.QuickbooksListId, Advertiser.QuickbooksEditSequence, Advertiser.QuickbooksErrNum, Advertiser.QuickbooksErrMsg, Advertiser.CustomerReference, Advertiser.CreatedById, Advertiser.DateCreated, Advertiser.LastModifiedById, Advertiser.LastModifiedDate).ToString());
        }
        public override int Add_AdvertiserForImport(AdvertiserInfo Advertiser)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddAdvertiserForImport", Advertiser.Id, Advertiser.PortalId, Advertiser.Carrier, Advertiser.Freight, Advertiser.CarrierNumber, Advertiser.AdvertiserName, Advertiser.Address1, Advertiser.Address2, Advertiser.City, Advertiser.State, Advertiser.Zip, Advertiser.Country, Advertiser.Phone, Advertiser.Fax, Advertiser.ClientType, Advertiser.QuickbooksListId, Advertiser.QuickbooksEditSequence, Advertiser.QuickbooksErrNum, Advertiser.QuickbooksErrMsg, Advertiser.CustomerReference, Advertiser.CreatedById, Advertiser.DateCreated, Advertiser.LastModifiedById, Advertiser.LastModifiedDate).ToString());
        }
        public override void ClearAdvertisers()
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_ClearAdvertisers");
        }
        public override void Update_Advertiser(AdvertiserInfo Advertiser)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateAdvertiser", Advertiser.Id, Advertiser.PortalId, Advertiser.Carrier, Advertiser.Freight, Advertiser.CarrierNumber, Advertiser.AdvertiserName, Advertiser.Address1, Advertiser.Address2, Advertiser.City, Advertiser.State, Advertiser.Zip, Advertiser.Country, Advertiser.Phone, Advertiser.Fax, Advertiser.ClientType, Advertiser.QuickbooksListId, Advertiser.QuickbooksEditSequence, Advertiser.QuickbooksErrNum, Advertiser.QuickbooksErrMsg, Advertiser.CustomerReference, Advertiser.CreatedById, Advertiser.DateCreated, Advertiser.LastModifiedById, Advertiser.LastModifiedDate).ToString();
        }
        public override void Delete_Advertiser(AdvertiserInfo Advertiser)
        {
            if (Advertiser.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteAdvertiser", Advertiser.Id);
            }
        }
        public override IDataReader Get_AdvertisersByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_AdvertisersByPortalId", PortalId);
        }
        public override IDataReader Get_AdvertiserById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_AdvertiserById", Id);
        }
        public override int Add_Market(MarketInfo Market)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddMarket", Market.PortalId, Market.MarketName, Market.Description, Market.ParentId, Market.CreatedById, Market.DateCreated, Market.LastModifiedById, Market.LastModifiedDate).ToString());
        }
        public override void Update_Market(MarketInfo Market)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateMarket", Market.Id, Market.PortalId, Market.MarketName, Market.Description, Market.ParentId, Market.CreatedById, Market.DateCreated, Market.LastModifiedById, Market.LastModifiedDate);
        }
        public override void Delete_Market(MarketInfo Market)
        {
            if (Market.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteMarket", Market.Id);
            }
        }
        public override IDataReader Get_MarketsByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_MarketsByPortalId", PortalId);
        }
        public override IDataReader Get_MarketById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_MarketById", Id);
        }
        public override int Add_ClientType(ClientTypeInfo ClientType)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddClientType", ClientType.PortalId, ClientType.ClientType, ClientType.CreatedById, ClientType.DateCreated, ClientType.LastModifiedById, ClientType.LastModifiedDate).ToString());
        }
        public override void Update_ClientType(ClientTypeInfo ClientType)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateClientType", ClientType.Id, ClientType.PortalId, ClientType.ClientType, ClientType.CreatedById, ClientType.DateCreated, ClientType.LastModifiedById, ClientType.LastModifiedDate);
        }
        public override void Delete_ClientType(ClientTypeInfo ClientType)
        {
            if (ClientType.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteClientType", ClientType.Id);
            }
        }
        public override IDataReader Get_ClientTypesByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_ClientTypesByPortalId", PortalId);
        }
        public override IDataReader Get_ClientTypeById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_ClientTypeById", Id);
        }
        public override int Add_CarrierType(CarrierTypeInfo CarrierType)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddCarrierType", CarrierType.PortalId, CarrierType.CarrierType, CarrierType.CreatedById, CarrierType.DateCreated, CarrierType.LastModifiedById, CarrierType.LastModifiedDate).ToString());
        }
        public override void Update_CarrierType(CarrierTypeInfo CarrierType)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateCarrierType", CarrierType.Id, CarrierType.PortalId, CarrierType.CarrierType, CarrierType.CreatedById, CarrierType.DateCreated, CarrierType.LastModifiedById, CarrierType.LastModifiedDate);
        }
        public override void Delete_CarrierType(CarrierTypeInfo CarrierType)
        {
            if (CarrierType.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteCarrierType", CarrierType.Id);
            }
        }
        public override IDataReader Get_CarrierTypesByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_CarrierTypesByPortalId", PortalId);
        }
        public override IDataReader Get_CarrierTypeById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_CarrierTypeById", Id);
        }
        public override int Add_FreightType(FreightTypeInfo FreightType)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddFreightType", FreightType.PortalId, FreightType.FreightType, FreightType.CreatedById, FreightType.DateCreated, FreightType.LastModifiedById, FreightType.LastModifiedDate).ToString());
        }
        public override void Update_FreightType(FreightTypeInfo FreightType)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateFreightType", FreightType.Id, FreightType.PortalId, FreightType.FreightType, FreightType.CreatedById, FreightType.DateCreated, FreightType.LastModifiedById, FreightType.LastModifiedDate);
        }
        public override void Delete_FreightType(FreightTypeInfo FreightType)
        {
            if (FreightType.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteFreightType", FreightType.Id);
            }
        }
        public override IDataReader Get_FreightTypesByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_FreightTypesByPortalId", PortalId);
        }
        public override IDataReader Get_FreightTypeById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_FreightTypeById", Id);
        }
        public override int Add_TapeFormat(TapeFormatInfo TapeFormat)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddTapeFormat", TapeFormat.PortalId, TapeFormat.TapeFormat, TapeFormat.Printer, TapeFormat.Label, TapeFormat.CreatedById, TapeFormat.DateCreated, TapeFormat.LastModifiedById, TapeFormat.LastModifiedDate, TapeFormat.Weight, TapeFormat.MaxPerPak).ToString());
        }
        public override void Update_TapeFormat(TapeFormatInfo TapeFormat)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateTapeFormat", TapeFormat.Id, TapeFormat.PortalId, TapeFormat.TapeFormat, TapeFormat.Printer, TapeFormat.Label, TapeFormat.CreatedById, TapeFormat.DateCreated, TapeFormat.LastModifiedById, TapeFormat.LastModifiedDate, TapeFormat.Weight, TapeFormat.MaxPerPak);
        }
        public override void Delete_TapeFormat(TapeFormatInfo TapeFormat)
        {
            if (TapeFormat.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteTapeFormat", TapeFormat.Id);
            }
        }
        public override IDataReader Get_TapeFormatsByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_TapeFormatsByPortalId", PortalId);
        }
        public override IDataReader Get_TapeFormatById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_TapeFormatById", Id);
        }
        public override int Add_DeliveryMethod(DeliveryMethodInfo DeliveryMethod)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddDeliveryMethod", DeliveryMethod.PortalId, DeliveryMethod.DeliveryMethod,DeliveryMethod.Priority, DeliveryMethod.CreatedById, DeliveryMethod.DateCreated, DeliveryMethod.LastModifiedById, DeliveryMethod.LastModifiedDate).ToString());
        }
        public override void Update_DeliveryMethod(DeliveryMethodInfo DeliveryMethod)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateDeliveryMethod", DeliveryMethod.Id, DeliveryMethod.PortalId, DeliveryMethod.DeliveryMethod, DeliveryMethod.Priority, DeliveryMethod.CreatedById, DeliveryMethod.DateCreated, DeliveryMethod.LastModifiedById, DeliveryMethod.LastModifiedDate);
        }
        public override void Delete_DeliveryMethod(DeliveryMethodInfo DeliveryMethod)
        {
            if (DeliveryMethod.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteDeliveryMethod", DeliveryMethod.Id);
            }
        }
        public override IDataReader Get_DeliveryMethodsByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_DeliveryMethodsByPortalId", PortalId);
        }
        public override IDataReader Get_DeliveryMethodById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_DeliveryMethodById", Id);
        }
        public override int Add_Station(StationInfo Station)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddStation", Station.PortalId, Station.MarketId, Station.StationName, Station.StationContact, Station.CallLetter, Station.MediaType, Station.TapeFormat, Station.Address1, Station.Address2, Station.City, Station.State, Station.Zip, Station.Country, Station.Phone, Station.Fax, Station.Email, Station.SpecialInstructions, Station.DeliveryMethods, Station.Online, Station.Status, Station.AttentionLine, Station.CreatedById, Station.DateCreated, Station.LastModifiedById, Station.LastModifiedDate, Station.ProgramFormat, Station.AdDeliveryCallLetters, Station.OTSMHDCallLetters, Station.OTSMSDCallLetters, Station.JavelinCallLetters, Station.backupRequired).ToString());
        }
        public override void ClearStations()
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_ClearStations");
        }
        public override int Add_StationForImport(StationInfo Station)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddStationForImport", Station.Id, Station.PortalId, Station.MarketId, Station.StationName, Station.StationContact, Station.CallLetter, Station.MediaType, Station.TapeFormat, Station.Address1, Station.Address2, Station.City, Station.State, Station.Zip, Station.Country, Station.Phone, Station.Fax, Station.Email, Station.SpecialInstructions, Station.DeliveryMethods, Station.Online, Station.Status, Station.AttentionLine, Station.CreatedById, Station.DateCreated, Station.LastModifiedById, Station.LastModifiedDate, Station.ProgramFormat, Station.AdDeliveryCallLetters, Station.OTSMHDCallLetters, Station.OTSMSDCallLetters, Station.JavelinCallLetters, Station.backupRequired).ToString());
        }
        public override void Update_Station(StationInfo Station)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateStation", Station.Id, Station.PortalId, Station.MarketId, Station.StationName, Station.StationContact, Station.CallLetter, Station.MediaType, Station.TapeFormat, Station.Address1, Station.Address2, Station.City, Station.State, Station.Zip, Station.Country, Station.Phone, Station.Fax, Station.Email, Station.SpecialInstructions, Station.DeliveryMethods, Station.Online, Station.Status, Station.AttentionLine, Station.CreatedById, Station.DateCreated, Station.LastModifiedById, Station.LastModifiedDate, Station.ProgramFormat, Station.AdDeliveryCallLetters, Station.OTSMHDCallLetters, Station.OTSMSDCallLetters, Station.JavelinCallLetters, Station.backupRequired);
        }
        public override void Delete_Station(StationInfo Station)
        {
            if (Station.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteStation", Station.Id);
            }
        }
        public override IDataReader Get_StationsByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_StationsByPortalId", PortalId);
        }
        public override IDataReader Get_StationsByPortalIdActive(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_StationsByPortalIdActive", PortalId);
        }
        public override IDataReader Get_StationById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_StationById", Id);
        }
        public override int Add_StationGroup(StationGroupInfo StationGroup)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddStationGroup", StationGroup.PortalId, StationGroup.StationGroupName, StationGroup.Description, StationGroup.AgencyId, StationGroup.CreatedById, StationGroup.DateCreated, StationGroup.LastModifiedById, StationGroup.LastModifiedDate).ToString());
        }
        public override void Update_StationGroup(StationGroupInfo StationGroup)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateStationGroup", StationGroup.Id, StationGroup.PortalId, StationGroup.StationGroupName, StationGroup.Description, StationGroup.AgencyId, StationGroup.CreatedById, StationGroup.DateCreated, StationGroup.LastModifiedById, StationGroup.LastModifiedDate).ToString();
        }
        public override void Delete_StationGroup(StationGroupInfo StationGroup)
        {
            if (StationGroup.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteStationGroup", StationGroup.Id);
            }
        }
        public override IDataReader Get_StationGroupsByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_StationGroupsByPortalId", PortalId);
        }
        public override IDataReader Get_StationGroupById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_StationGroupById", Id);
        }
        public override IDataReader Get_StationGroupsByUserId(int UserId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_StationGroupsByUserId", UserId);
        }
        public override IDataReader Get_StationsinGroupById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_StationsInGroupById", Id);
        }
        public override void Delete_StationsInGroup(int StationId, int StationGroupId)
        {
            if (StationId != -1 && StationGroupId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteStationsInGroup", StationId, StationGroupId);
            }
        }
        public override void Delete_StationsInGroupByGroup(int StationGroupId)
        {
            if (StationGroupId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteStationsInGroupByGroup", StationGroupId);
            }
        }
        public override int Add_StationsInGroup(int PortalId, int StationId, int StationGroupId)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddStationsInGroup", PortalId, StationId, StationGroupId).ToString());
        }
        public override int Add_Label(LabelInfo Label)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddLabel", Label.PortalId, Label.UserType, Label.UserId, Label.CampaignId, Label.AgencyName, Label.AgencyId, Label.AdvertiserName, Label.AdvertiserId, Label.TapeFormat, Label.Title, Label.Description, Label.ISCICode, Label.PMTMediaId, Label.MediaLength, Label.Standard, Label.LabelNumber, Label.CampaignMediaId, Label.Notes, Label.CreatedById, Label.DateCreated, Label.LastModifiedById, Label.LastModifiedDate).ToString());
        }
        public override int Add_LabelForImport(LabelInfo Label)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddLabelForImport", Label.Id, Label.PortalId, Label.UserType, Label.UserId, Label.CampaignId, Label.AgencyName, Label.AgencyId, Label.AdvertiserName, Label.AdvertiserId, Label.TapeFormat, Label.Title, Label.Description, Label.ISCICode, Label.PMTMediaId, Label.MediaLength, Label.Standard, Label.LabelNumber, Label.CampaignMediaId, Label.Notes, Label.CreatedById, Label.DateCreated, Label.LastModifiedById, Label.LastModifiedDate).ToString());
        }
        public override void ClearLabels()
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_ClearLabels");
        }
        public override void Update_Label(LabelInfo Label)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateLabel", Label.Id, Label.PortalId, Label.UserType, Label.UserId, Label.CampaignId, Label.AgencyName, Label.AgencyId, Label.AdvertiserName, Label.AdvertiserId, Label.TapeFormat, Label.Title, Label.Description, Label.ISCICode, Label.PMTMediaId, Label.MediaLength, Label.Standard, Label.LabelNumber, Label.CampaignMediaId, Label.Notes, Label.CreatedById, Label.DateCreated, Label.LastModifiedById, Label.LastModifiedDate);
        }
        public override void Delete_Label(LabelInfo Label)
        {
            if (Label.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteLabel", Label.Id);
            }
        }
        public override IDataReader Get_LabelsByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_LabelsByPortalId", PortalId);
        }
        public override IDataReader Get_LabelById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_LabelById", Id);
        }
        public override int Add_MasterItem(MasterItemInfo MasterItem)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddMasterItem", MasterItem.PortalId, MasterItem.Filename, MasterItem.AdvertiserId, MasterItem.Title, MasterItem.MediaType, MasterItem.Encode, MasterItem.Standard, MasterItem.Length, MasterItem.CustomerId, MasterItem.PMTMediaId, MasterItem.Reel, MasterItem.TapeCode, MasterItem.Position, MasterItem.VaultId, MasterItem.Location, MasterItem.Comment, MasterItem.ClosedCaptioned, MasterItem.CreatedById, MasterItem.DateCreated, MasterItem.LastModifiedById, MasterItem.LastModifiedDate, MasterItem.CheckListForm, MasterItem.isApproved, MasterItem.BillToId, MasterItem.hasChecklist).ToString());
        }
        public override int Add_MasterItemForImport(MasterItemInfo MasterItem)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddMasterItemForImport", MasterItem.Id, MasterItem.PortalId, MasterItem.Filename, MasterItem.AdvertiserId, MasterItem.Title, MasterItem.MediaType, MasterItem.Encode, MasterItem.Standard, MasterItem.Length, MasterItem.CustomerId, MasterItem.PMTMediaId, MasterItem.Reel, MasterItem.TapeCode, MasterItem.Position, MasterItem.VaultId, MasterItem.Location, MasterItem.Comment, MasterItem.ClosedCaptioned, MasterItem.CreatedById, MasterItem.DateCreated, MasterItem.LastModifiedById, MasterItem.LastModifiedDate, MasterItem.CheckListForm, MasterItem.isApproved, MasterItem.BillToId).ToString());
        }
        public override void ClearMasterItems()
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_ClearMasterItems");
        }
        public override void Update_MasterItem(MasterItemInfo MasterItem)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateMasterItem", MasterItem.Id, MasterItem.PortalId, MasterItem.Filename, MasterItem.AdvertiserId, MasterItem.Title, MasterItem.MediaType, MasterItem.Encode, MasterItem.Standard, MasterItem.Length, MasterItem.CustomerId, MasterItem.PMTMediaId, MasterItem.Reel, MasterItem.TapeCode, MasterItem.Position, MasterItem.VaultId, MasterItem.Location, MasterItem.Comment, MasterItem.ClosedCaptioned, MasterItem.CreatedById, MasterItem.DateCreated, MasterItem.LastModifiedById, MasterItem.LastModifiedDate, MasterItem.CheckListForm, MasterItem.isApproved, MasterItem.BillToId, MasterItem.hasChecklist);
        }
        public override void Delete_MasterItem(MasterItemInfo MasterItem)
        {
            if (MasterItem.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteMasterItem", MasterItem.Id);
            }
        }
        public override IDataReader Get_MasterItemsByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_MasterItemsByPortalId", PortalId);
        }
        public override IDataReader Get_MasterItemById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_MasterItemById", Id);
        }
        public override void Add_AdvertiserAgency(int AdvertiserId, int AgencyId)
        {
            if (AdvertiserId != -1 && AgencyId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_AddAdvertiserAgency", AdvertiserId, AgencyId);
            }
        }
        public override void Delete_AdvertiserAgencyByAdvertiserId(int AdvertiserId)
        {
            if (AdvertiserId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteAdvertiserAgencyByAdvertiserId", AdvertiserId);
            }
        }
        public override IDataReader Get_AgenciesByAdvertiserId(int AdvertiserId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_AgenciesByAdvertiserId", AdvertiserId);
        }
        public override IDataReader Get_AgencyIdsByAdvertiserId(int AdvertiserId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_AgencyIdsByAdvertiserId", AdvertiserId);
        }
        public override void Add_MasterItemAgency(int MasterItemId, int AgencyId)
        {
            if (MasterItemId != -1 && AgencyId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_AddMasterItemAgency", MasterItemId, AgencyId);
            }
        }
        public override void ClearMasterItemAgencies()
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_ClearMasterItemAgencies");
        }
        public override void Delete_MasterItemAgencyByMasterItemId(int MasterItemId)
        {
            if (MasterItemId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteMasterItemAgencyByMasterItemId", MasterItemId);
            }
        }
        public override IDataReader Get_AgenciesByMasterItemId(int MasterItemId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_AgenciesByMasterItemId", MasterItemId);
        }
        public override IDataReader Get_AgencyIdsByMasterItemId(int MasterItemId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_AgencyIdsByMasterItemId", MasterItemId);
        }
        public override IDataReader Get_AdvertiserAgencies()
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_GetAdvertiserAgencies");
        }
        public override IDataReader Get_MasterItemAgencies()
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_GetMasterItemAgencies");
        }
        public override void Add_UserInAgency(int UserId, int AgencyId)
        {
            if (UserId != -1 && AgencyId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_AddUserInAgency", UserId, AgencyId);
            }
        }
        public override void Delete_UserInAgencies(int UserId)
        {
            if (UserId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteUserFromAgencies", UserId);
            }
        }
        public override void Add_UserInAdvertiser(int UserId, int AdvertiserId)
        {
            if (UserId != -1 && AdvertiserId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_AddUserInAdvertiser", UserId, AdvertiserId);
            }
        }
        public override void Delete_UserInAdvertisers(int UserId)
        {
            if (UserId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteUserFromAdvertisers", UserId);
            }
        }
        public override IDataReader Get_AgenciesByUser(int UserId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_GetAgenciesByUser", UserId);
        }
        public override IDataReader Get_AdvertisersByUser(int UserId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_GetAdvertisersByUser", UserId);
        }
        public override string GetMaxPMTMediaId(int PortalId)
        {
            return SqlHelper.ExecuteScalar(ConnectionString, "PMT_GetMaxPMTMediaId", PortalId).ToString();
        }
        public override string GetMaxLabelNumber(int PortalId)
        {
            return SqlHelper.ExecuteScalar(ConnectionString, "PMT_GetMaxLabelNumber", PortalId).ToString();
        }
        public override int Add_LibraryItem(LibraryItemInfo LibraryItem)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddLibraryItem", LibraryItem.PortalId, LibraryItem.AdvertiserId, LibraryItem.AgencyId, LibraryItem.ISCICode, LibraryItem.Filename, LibraryItem.Title, LibraryItem.ProductDescription, LibraryItem.MediaLength, LibraryItem.MediaType, LibraryItem.Encode, LibraryItem.Standard, LibraryItem.CustomerReference, LibraryItem.PMTMediaId, LibraryItem.MasterId, LibraryItem.DateCreated, LibraryItem.MediaIndex, LibraryItem.Reel, LibraryItem.TapeCode, LibraryItem.Position, LibraryItem.VaultId, LibraryItem.Location, LibraryItem.Status, LibraryItem.Comment, LibraryItem.ClosedCaptioned, LibraryItem.CreatedById, LibraryItem.LastModifiedById, LibraryItem.LastModifiedDate).ToString());
        }
        public override int Add_LibraryItemForImport(LibraryItemInfo LibraryItem)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddLibraryItemForImport", LibraryItem.Id, LibraryItem.PortalId, LibraryItem.AdvertiserId, LibraryItem.AgencyId, LibraryItem.ISCICode, LibraryItem.Filename, LibraryItem.Title, LibraryItem.ProductDescription, LibraryItem.MediaLength, LibraryItem.MediaType, LibraryItem.Encode, LibraryItem.Standard, LibraryItem.CustomerReference, LibraryItem.PMTMediaId, LibraryItem.MasterId, LibraryItem.DateCreated, LibraryItem.MediaIndex, LibraryItem.Reel, LibraryItem.TapeCode, LibraryItem.Position, LibraryItem.VaultId, LibraryItem.Location, LibraryItem.Status, LibraryItem.Comment, LibraryItem.ClosedCaptioned, LibraryItem.CreatedById, LibraryItem.LastModifiedById, LibraryItem.LastModifiedDate).ToString());
        }
        public override void ClearLibraryItems()
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_ClearLibraryItems");
        }
        public override void Update_LibraryItem(LibraryItemInfo LibraryItem)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateLibraryItem", LibraryItem.Id, LibraryItem.PortalId, LibraryItem.AdvertiserId, LibraryItem.AgencyId, LibraryItem.ISCICode, LibraryItem.Filename, LibraryItem.Title, LibraryItem.ProductDescription, LibraryItem.MediaLength, LibraryItem.MediaType, LibraryItem.Encode, LibraryItem.Standard, LibraryItem.CustomerReference, LibraryItem.PMTMediaId, LibraryItem.MasterId, LibraryItem.DateCreated, LibraryItem.MediaIndex, LibraryItem.Reel, LibraryItem.TapeCode, LibraryItem.Position, LibraryItem.VaultId, LibraryItem.Location, LibraryItem.Status, LibraryItem.Comment, LibraryItem.ClosedCaptioned, LibraryItem.CreatedById, LibraryItem.LastModifiedById, LibraryItem.LastModifiedDate);
        }
        public override void Delete_LibraryItem(LibraryItemInfo LibraryItem)
        {
            if (LibraryItem.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteLibraryItem", LibraryItem.Id);
            }
        }
        public override IDataReader Get_LibraryItemsByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_GetLibraryItemsByPortalId", PortalId);
        }
        public override IDataReader Get_LibraryItemById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_LibraryItemById", Id);
        }
        public override IDataReader Get_LibraryItemByISCI(string ISCI)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_LibraryItemsByISCI", ISCI);
        }
        public override int Add_Service(ServiceInfo Service)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddService", Service.PortalId, Service.ServiceName, Service.BillingCode, Service.CreatedById, Service.DateCreated, Service.LastModifiedById, Service.LastModifiedDate).ToString());
        }
        public override void Update_Service(ServiceInfo Service)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateService", Service.Id, Service.PortalId, Service.ServiceName, Service.BillingCode, Service.CreatedById, Service.DateCreated, Service.LastModifiedById, Service.LastModifiedDate);
        }
        public override void Delete_Service(ServiceInfo Service)
        {
            if (Service.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteService", Service.Id);
            }
        }
        public override IDataReader Get_ServicesByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_ServicesByPortalId", PortalId);
        }
        public override IDataReader Get_ServiceById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_ServiceById", Id);
        }
        public override int Add_WorkOrder(WorkOrderInfo WorkOrder)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddWorkOrder", WorkOrder.PortalId, WorkOrder.Description, WorkOrder.PONumber, WorkOrder.InvoiceNumber, WorkOrder.AdvertiserId, WorkOrder.AgencyId, WorkOrder.BillToId, WorkOrder.Notes, WorkOrder.CreatedById, WorkOrder.DateCreated, WorkOrder.LastModifiedById, WorkOrder.LastModifiedDate, WorkOrder.AssignedTo, WorkOrder.Status, WorkOrder.Priority).ToString());
        }
        public override void Update_WorkOrder(WorkOrderInfo WorkOrder)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateWorkOrder", WorkOrder.Id, WorkOrder.PortalId, WorkOrder.Description, WorkOrder.PONumber, WorkOrder.InvoiceNumber, WorkOrder.AdvertiserId, WorkOrder.AgencyId, WorkOrder.BillToId, WorkOrder.Notes, WorkOrder.CreatedById, WorkOrder.DateCreated, WorkOrder.LastModifiedById, WorkOrder.LastModifiedDate, WorkOrder.AssignedTo, WorkOrder.Status, WorkOrder.Priority);
        }
        public override void Delete_WorkOrder(WorkOrderInfo WorkOrder)
        {
            if (WorkOrder.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteWorkOrder", WorkOrder.Id);
            }
        }
        public override IDataReader Get_WorkOrdersByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_WorkOrdersByPortalId", PortalId);
        }
        public override IDataReader Get_WorkOrderById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_WorkOrderById", Id);
        }
        public override int Add_WorkOrderGroupStation(WOGroupStationInfo WorkOrderGroupStation)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddWorkOrderGroupStation", WorkOrderGroupStation.PortalId, WorkOrderGroupStation.WOGroupId, WorkOrderGroupStation.WorkOrderId, WorkOrderGroupStation.StationId, WorkOrderGroupStation.DeliveryMethod, WorkOrderGroupStation.ShippingMethodId, WorkOrderGroupStation.PriorityId, WorkOrderGroupStation.CreatedById, WorkOrderGroupStation.DateCreated, WorkOrderGroupStation.LastModifiedById, WorkOrderGroupStation.LastModifiedDate, WorkOrderGroupStation.Quantity).ToString());
        }
        public override void Update_WorkOrderGroupStation(WOGroupStationInfo WorkOrderGroupStation)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateWorkOrderGroupStation", WorkOrderGroupStation.Id, WorkOrderGroupStation.PortalId, WorkOrderGroupStation.WOGroupId, WorkOrderGroupStation.WorkOrderId, WorkOrderGroupStation.StationId, WorkOrderGroupStation.DeliveryMethod, WorkOrderGroupStation.ShippingMethodId, WorkOrderGroupStation.PriorityId, WorkOrderGroupStation.CreatedById, WorkOrderGroupStation.DateCreated, WorkOrderGroupStation.LastModifiedById, WorkOrderGroupStation.LastModifiedDate, WorkOrderGroupStation.Quantity);
        }
        public override void Delete_WorkOrderGroupStation(WOGroupStationInfo WorkOrderGroupStation)
        {
            if (WorkOrderGroupStation.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteWorkOrderGroupStation", WorkOrderGroupStation.Id);
            }
        }
        public override void Delete_WorkOrderGroupStationByGroupId(int GroupId)
        {
            if (GroupId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteWorkOrderGroupStationByGroupId", GroupId);
            }
        }
        public override void Delete_WorkOrderGroupStationByWOId(int WOId)
        {
            if (WOId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteWorkOrderGroupStationByWOId", WOId);
            }
        }
        public override IDataReader Get_WorkOrderGroupStationsByGroupId(int GroupId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_WorkOrderGroupStationsByGroupId", GroupId);
        }
        public override IDataReader Get_WorkOrderGroupStationById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_WorkOrderGroupStationById", Id);
        }
        public override int Add_WorkOrderGroup(WOGroupInfo WorkOrderGroup)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddWorkOrderGroup", WorkOrderGroup.PortalId, WorkOrderGroup.WorkOrderId, WorkOrderGroup.MasterId, WorkOrderGroup.GroupType, WorkOrderGroup.GroupName, WorkOrderGroup.Comments, WorkOrderGroup.CreatedById, WorkOrderGroup.DateCreated, WorkOrderGroup.LastModifiedById, WorkOrderGroup.LastModifiedDate).ToString());
        }
        public override void Update_WorkOrderGroup(WOGroupInfo WorkOrderGroup)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateWorkOrderGroup", WorkOrderGroup.Id, WorkOrderGroup.PortalId, WorkOrderGroup.WorkOrderId, WorkOrderGroup.MasterId, WorkOrderGroup.GroupType, WorkOrderGroup.GroupName, WorkOrderGroup.Comments, WorkOrderGroup.CreatedById, WorkOrderGroup.DateCreated, WorkOrderGroup.LastModifiedById, WorkOrderGroup.LastModifiedDate);
        }
        public override void Delete_WorkOrderGroup(WOGroupInfo WorkOrderGroup)
        {
            if (WorkOrderGroup.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteWorkOrderGroup", WorkOrderGroup.Id);
            }
        }
        public override IDataReader Get_WorkOrderGroupsByWOId(int WorkOrderId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_WorkOrderGroupsByWOId", WorkOrderId);
        }
        public override IDataReader Get_WorkOrderGroupById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_WorkOrderGroupById", Id);
        }
        public override void DeleteWOGroupLibItemsByWOGroupId(int WorkOrderId)
        {
            if (WorkOrderId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteWOGroupLibItemsByWOGroupId", WorkOrderId);
            }
        }
        public override void AddWOGroupLibItem(int WorkOrderGroupId, int LibraryItemId)
        {
            if (WorkOrderGroupId != -1 && LibraryItemId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_AddWOGroupLibItem", WorkOrderGroupId, LibraryItemId);
            }
        }
        public override IDataReader GetLibItemsByWOGroupId(int WOGroupId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_GetLibItemsByWOGroupId", WOGroupId);
        }
        public override void DeleteWOGroupServicesByWOGroupId(int WorkOrderGroupId)
        {
            if (WorkOrderGroupId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteWOGroupServicesByWOGroupId", WorkOrderGroupId);
            }
        }
        public override void AddWOGroupService(int WorkOrderGroupId, int ServiceId)
        {
            if (WorkOrderGroupId != -1 && ServiceId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_AddWOGroupService", WorkOrderGroupId, ServiceId);
            }
        }
        public override IDataReader GetServicesByWOGroupId(int WOGroupId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_GetServicesByWOGroupId", WOGroupId);
        }
        public override IDataReader Get_AdvertisersByAgencyId(int AgencyId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_AdvertisersByAgencyId", AgencyId);
        }
        public override int Add_WOComment(WOCommentInfo WOComment)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddWOComment", WOComment.PortalId, WOComment.WorkOrderId, WOComment.WOTaskId, WOComment.Comment, WOComment.DisplayName, WOComment.CreatedById, WOComment.DateCreated, WOComment.LastModifiedById, WOComment.LastModifiedDate, WOComment.CommentType).ToString());
        }
        public override void Update_WOComment(WOCommentInfo WOComment)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateWOComment", WOComment.Id, WOComment.PortalId, WOComment.WorkOrderId, WOComment.WOTaskId, WOComment.Comment, WOComment.DisplayName, WOComment.CreatedById, WOComment.DateCreated, WOComment.LastModifiedDate, WOComment.CommentType);
        }
        public override void Delete_WOComment(WOCommentInfo WOComment)
        {
            if (WOComment.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteWOComment", WOComment.Id);
            }
        }
        public override IDataReader Get_WOCommentsByWOId(int WorkOrderId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_WOCommentsByWOId", WorkOrderId);
        }
        public override IDataReader Get_WOCommentById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_WOCommentById", Id);
        }
        public override int Add_Task(TaskInfo Task)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddTask", Task.PortalId, Task.Description, Task.WorkOrderId, Task.WOGroupId, Task.TaskType, Task.MasterId, Task.LibraryId, Task.StationId, Task.DeliveryMethodId, Task.DeliveryMethod, Task.DeliveryMethodResponse, Task.DeliveryOrderId, Task.DeliveryOrderDateCreated, Task.DeliveryStatus, Task.isComplete, Task.isDeleted, Task.CreatedById, Task.DateCreated, Task.LastModifiedById, Task.LastModifiedDate, Task.QBCode, Task.QBCodeId, Task.DeliveryOrderDateComplete, Task.Quantity).ToString());
        }
        public override int Add_TaskForImport(TaskInfo Task)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddTaskForImport", Task.Id, Task.PortalId, Task.Description, Task.WorkOrderId, Task.WOGroupId, Task.TaskType, Task.MasterId, Task.LibraryId, Task.StationId, Task.DeliveryMethodId, Task.DeliveryMethod, Task.DeliveryMethodResponse, Task.DeliveryOrderId, Task.DeliveryOrderDateCreated, Task.DeliveryStatus, Task.isComplete, Task.isDeleted, Task.CreatedById, Task.DateCreated, Task.LastModifiedById, Task.LastModifiedDate, Task.QBCode, Task.QBCodeId, Task.DeliveryOrderDateComplete, Task.Quantity).ToString());
        }
        public override void Update_Task(TaskInfo Task)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateTask", Task.Id, Task.PortalId, Task.Description, Task.WorkOrderId, Task.WOGroupId, Task.TaskType, Task.MasterId, Task.LibraryId, Task.StationId, Task.DeliveryMethodId, Task.DeliveryMethod, Task.DeliveryMethodResponse, Task.DeliveryOrderId, Task.DeliveryOrderDateCreated, Task.DeliveryStatus, Task.isComplete, Task.isDeleted, Task.CreatedById, Task.DateCreated, Task.LastModifiedById, Task.LastModifiedDate, Task.QBCode, Task.QBCodeId, Task.DeliveryOrderDateComplete, Task.Quantity);
        }
        public override void Delete_Task(TaskInfo Task)
        {
            if (Task.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteTask", Task.Id);
            }
        }
        public override IDataReader Get_TasksByWOId(int WorkOrderId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_TasksByWOId", WorkOrderId);
        }
        public override IDataReader Get_OpenTasks()
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_OpenTasks");
        }
        public override IDataReader Get_TaskById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_TaskById", Id);
        }
        public override int Add_QBCode(QBCodeInfo QBCode)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddQBCode", QBCode.PortalId, QBCode.Type, QBCode.QBCode, QBCode.MediaType, QBCode.MinLength, QBCode.MaxLength, QBCode.CreatedById, QBCode.DateCreated, QBCode.LastModifiedById, QBCode.LastModifiedDate).ToString());
        }
        public override void Update_QBCode(QBCodeInfo QBCode)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateQBCode", QBCode.Id, QBCode.PortalId, QBCode.Type, QBCode.QBCode, QBCode.MediaType, QBCode.MinLength, QBCode.MaxLength, QBCode.CreatedById, QBCode.DateCreated, QBCode.LastModifiedById, QBCode.LastModifiedDate);
        }
        public override void Delete_QBCode(QBCodeInfo QBCode)
        {
            if (QBCode.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteQBCode", QBCode.Id);
            }
        }
        public override IDataReader Get_QBCodesByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_QBCodesByPortalId", PortalId);
        }
        public override IDataReader Get_QBCodeById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_QBCodeById", Id);
        }
        public override void Delete_QBCodeDeliveryMethodsByQBCodeId(int QBCodeId)
        {
            if (QBCodeId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteQBCodeDeliveryMethodsByQBCodeId", QBCodeId);
            }
        }
        public override void Add_QBCodeDeliveryMethodsByQBCodeId(int QBCodeId, int DeliveryMethodId)
        {
            if (QBCodeId != -1 && DeliveryMethodId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_AddQBCodeDeliveryMethod", QBCodeId, DeliveryMethodId);
            }
        }
        public override void Delete_QBCodeTapeFormatsByQBCodeId(int QBCodeId)
        {
            if (QBCodeId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteQBCodeTapeFormatsByQBCodeId", QBCodeId);
            }
        }
        public override void Add_QBCodeTapeFormatsByQBCodeId(int QBCodeId, int TapeFormatId)
        {
            if (QBCodeId != -1 && TapeFormatId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_AddQBCodeTapeFormat", QBCodeId, TapeFormatId);
            }
        }
        public override void Delete_QBCodeServicesByQBCodeId(int QBCodeId)
        {
            if (QBCodeId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteQBCodeServicesByQBCodeId", QBCodeId);
            }
        }
        public override void Add_QBCodeServicesByQBCodeId(int QBCodeId, int ServiceId)
        {
            if (QBCodeId != -1 && ServiceId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_AddQBCodeService", QBCodeId, ServiceId);
            }
        }
        public override IDataReader Get_DeliveryMethodsByQBCodeId(int QBCodeId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_GetDeliveryMethodsByQBCodeId", QBCodeId);
        }

        public override IDataReader Get_TapeFormatsByQBCodeId(int QBCodeId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_GetTapeFormatsByQBCodeId", QBCodeId);
        }

        public override IDataReader Get_ServicesByQBCodeId(int QBCodeId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_GetServicesByQBCodeId", QBCodeId);
        }
        public override void Delete_TaskQBCodeByTaskId(int TaskId)
        {
            if (TaskId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteTaskQBCodeByTaskId", TaskId);
            }
        }
        public override void Add_TaskQBCode(int TaskId, int QBCodeId)
        {
            if (TaskId != -1 && QBCodeId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_AddTaskQBCode", TaskId, QBCodeId);
            }
        }
        public override IDataReader Get_QBCodesByTaskId(int TaskId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_GetQBCodesByTaskId", TaskId);
        }
        public override int Add_Report(ReportInfo Report)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddReport", Report.PortalId, Report.ReportName, Report.ReportType, Report.AdvertiserId, Report.AgencyId, Report.Keyword, Report.Status, Report.Frequency, Report.FirstReportDate, Report.EmailTo, Report.EmailMessage, Report.isActive, Report.CreatedById, Report.DateCreated, Report.LastModifiedById, Report.LastModifiedDate).ToString());
        }
        public override void Update_Report(ReportInfo Report)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateReport", Report.Id, Report.PortalId, Report.ReportName, Report.ReportType, Report.AdvertiserId, Report.AgencyId, Report.Keyword, Report.Status, Report.Frequency, Report.FirstReportDate, Report.EmailTo, Report.EmailMessage, Report.isActive, Report.CreatedById, Report.DateCreated, Report.LastModifiedById, Report.LastModifiedDate);
        }
        public override void Delete_Report(ReportInfo Report)
        {
            if (Report.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteReport", Report.Id);
            }
        }
        public override IDataReader Get_ReportsByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_ReportsByPortalId", PortalId);
        }
        public override IDataReader Get_ReportById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_ReportById", Id);
        }
        public override int Add_EasySpotObject(EasySpotObjectInfo EasySpotObject)
        {
            string x = SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddEasySpotObject", EasySpotObject.PortalId, (int)EasySpotObject.ObjectType, EasySpotObject.PMTObjectId, EasySpotObject.EasySpotObjectId, EasySpotObject.CreatedById, EasySpotObject.DateCreated, EasySpotObject.LastModifiedById, EasySpotObject.LastModifiedDate).ToString();
            return int.Parse(x);
        }
        public override IDataReader Get_EasySpotByIdAndType(int PMTObjectId, EasySpotTypeEnum ObjectType)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_EasySpotByIdAndType", PMTObjectId, (int)ObjectType);
        }
        public override int Add_Invoice(InvoiceInfo Invoice)
        {
            return int.Parse(SqlHelper.ExecuteScalar(ConnectionString, "PMT_AddInvoice", Invoice.PortalId, Invoice.AdvertiserId, Invoice.AgencyId, Invoice.QBInvoiceNumber, Invoice.SentToQB, Invoice.CreatedById, Invoice.DateCreated, Invoice.LastModifiedById, Invoice.LastModifiedDate, Invoice.BillToId).ToString());
        }
        public override void Update_Invoice(InvoiceInfo Invoice)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_UpdateInvoice", Invoice.Id, Invoice.PortalId, Invoice.AdvertiserId, Invoice.AgencyId, Invoice.QBInvoiceNumber, Invoice.SentToQB, Invoice.CreatedById, Invoice.DateCreated, Invoice.LastModifiedById, Invoice.LastModifiedDate, Invoice.BillToId);
        }
        public override void Delete_Invoice(InvoiceInfo Invoice)
        {
            if (Invoice.Id != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteInvoice", Invoice.Id);
            }
        }
        public override IDataReader Get_InvoicesByPortalId(int PortalId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_InvoicesByPortalId", PortalId);
        }
        public override IDataReader Get_InvoiceById(int Id)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_InvoiceById", Id);
        }
        public override void Delete_WOInInvoiceByInvoiceId(int InvoiceId)
        {
            if (InvoiceId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_DeleteWOInInvoicesByInvoiceId", InvoiceId);
            }
        }
        public override void Add_WOInInvoice(int InvoiceId, int WOId)
        {
            if (InvoiceId != -1 && WOId != -1)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "PMT_AddWOInInvoice", InvoiceId, WOId);
            }
        }
        public override List<int> Get_WOInsByInvoiceId(int InvoiceId)
        {
            //return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_GetWOInInvoicesByInvoiceId", InvoiceId);
            List<int> s = new List<int>();
            SqlConnection sqlConn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandText = "select WOId from PMT_WOsInInvoices where InvoiceId = " + InvoiceId.ToString();
            cmd.Connection = sqlConn;
            sqlConn.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    s.Add(dr.GetInt32(0));
                }
            }
            sqlConn.Close();
            return s;
        }
        public override List<int> Get_InvoicesByToSend()
        {
            List<int> s = new List<int>();
            SqlConnection sqlConn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandText = "select * from   PMT_Invoices where  SentToQB = 0";
            cmd.Connection = sqlConn;
            sqlConn.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    s.Add(dr.GetInt32(0));
                }
            }
            sqlConn.Close();
            return s;
            //return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "PMT_Get_InvoicesByToSend");
        }
        #endregion

    }

}