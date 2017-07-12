using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace Christoc.Modules.PMT_Admin
{
    public class ModelObjects
    {
    }
    public enum MediaTypeEnum
    {
        ADDELIVERY_YES,
        HD,
        HD___SD,
        HD___SD_BACKUP_REQUIRED,
        SD
    }
    public enum GroupTypeEnum
    {
        Delivery,
        Bundle,
        Customized,
        Non_Deliverable
    }
    public enum CommentTypeEnum
    {
        Comment,
        SystemMessage,
        Error
    }
    public enum EasySpotTypeEnum
    {
        Address,
        Parcel,
        Shipment,
        CarrierAccount,
        Tracker
    }
    [Serializable]
    public class AgencyInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public string AgencyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public int ClientType { get; set; }
        public bool Status { get; set; }
        public string CustomerReference { get; set; }
        public string AttentionLine { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public AgencyInfo()
        {
            Id = -1;
            PortalId = -1;
            AgencyName = "";
            Address1 = "";
            Address2 = "";
            City = "";
            State = "";
            Zip = "";
            Country = "";
            Phone = "";
            Fax = "";
            ClientType = -1;
            Status = false;
            CustomerReference = "";
            AttentionLine = "";
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }        
    }
    [Serializable]
    public class AdvertiserInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public List<AgencyInfo> Agencies { get; set; }
        public int Carrier { get; set; }
        public int Freight { get; set; }
        public string CarrierNumber { get; set; }
        public string AdvertiserName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public int ClientType { get; set; }
        public string CustomerReference { get; set; }
        public string QuickbooksListId { get; set; }
        public string QuickbooksEditSequence { get; set; }
        public string QuickbooksErrNum { get; set; }
        public string QuickbooksErrMsg { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        
        public AdvertiserInfo()
        {
            Id = -1;
            PortalId = -1;
            Agencies = new List<AgencyInfo>();
            Carrier = -1;
            Freight = -1;
            CarrierNumber = "";
            AdvertiserName = "";
            Address1 = "";
            Address2 = "";
            City = "";
            State = "";
            Zip = "";
            Country = "";
            Phone = "";
            Fax = "";
            ClientType = -1;
            CustomerReference = "";
            QuickbooksListId = "";
            QuickbooksEditSequence = "";
            QuickbooksErrNum = "";
            QuickbooksErrMsg = "";
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class MarketInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public string MarketName { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public MarketInfo()
        {
            Id = -1;
            PortalId = -1;
            MarketName = "";
            Description = "";
            ParentId = -1;
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class StationInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public int MarketId { get; set; }
        public string StationName { get; set; }
        public string StationContact { get; set; }
        public string CallLetter { get; set; }
        public string ProgramFormat { get; set; }
        public string AdDeliveryCallLetters { get; set; }
        public string OTSMHDCallLetters { get; set; }
        public string OTSMSDCallLetters { get; set; }
        public string JavelinCallLetters { get; set; }
        public int MediaType { get; set; }
        public bool backupRequired { get; set; }
        public string TapeFormat { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string SpecialInstructions { get; set; }
        public string DeliveryMethods { get; set; }
        public bool Online { get; set; }
        public bool Status { get; set; }
        public string AttentionLine { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public StationInfo()
        {
            Id =-1;
            PortalId=-1;
            MarketId=-1;
            StationName="";
            StationContact="";
            CallLetter="";
            ProgramFormat = "Short Form";
            AdDeliveryCallLetters = "";
            OTSMHDCallLetters = "";
            OTSMSDCallLetters = "";
            JavelinCallLetters = "";
            MediaType=-1;
            backupRequired = false;
            TapeFormat="";
            Address1="";
            Address2="";
            City="";
            State="";
            Zip="";
            Country="";
            Phone="";
            Fax="";
            Email="";
            SpecialInstructions="";
            DeliveryMethods="";
            Online=false;
            Status=false;
            AttentionLine = "";
            CreatedById = -1;
            DateCreated=DateTime.Now;
            LastModifiedById=-1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class ClientTypeInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public string ClientType { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public ClientTypeInfo()
        {
            Id = -1;
            PortalId = -1;
            ClientType = "";
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class CarrierTypeInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public string CarrierType { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public CarrierTypeInfo()
        {
            Id = -1;
            PortalId = -1;
            CarrierType = "";
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class FreightTypeInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public string FreightType { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public FreightTypeInfo()
        {
            Id = -1;
            PortalId = -1;
            FreightType = "";
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class DeliveryMethodInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public string DeliveryMethod { get; set; }
        public string Priority { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DeliveryMethodInfo()
        {
            Id = -1;
            PortalId = -1;
            DeliveryMethod = "";
            Priority = "";
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class TapeFormatInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public string TapeFormat { get; set; }
        public string Printer { get; set; }
        public string Label { get; set; }
        public double Weight { get; set; }
        public int MaxPerPak { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public TapeFormatInfo()
        {
            Id = -1;
            PortalId = -1;
            TapeFormat = "";
            Printer = "";
            Label = "";
            Weight = 1.0;
            MaxPerPak = 1;
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class StationGroupInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public string StationGroupName { get; set; }
        public string Description { get; set; }
        public int AgencyId { get; set; }
        public List<StationInfo> stations { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public StationGroupInfo()
        {
            Id = -1;
            PortalId = -1;
            StationGroupName = "";
            Description = "";
            AgencyId = -1;
            stations = new List<StationInfo>();
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class LabelInfo
    {
        //not AdvertiserName and AgencyName are legacy fields needed to reconstitute id fields
        public int Id { get; set; }
        public int PortalId { get; set; }
        public string UserType { get; set; }
        public int UserId { get; set; }
        public int CampaignId { get; set; }
        public string AgencyName { get; set; }
        public int AgencyId { get; set; }
        public string AdvertiserName { get; set; }
        public int AdvertiserId { get; set; }
        public int TapeFormat { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ISCICode { get; set; }
        public string PMTMediaId { get; set; }
        public string MediaLength { get; set; }
        public string Standard { get; set; }
        public Int64 LabelNumber { get; set; }
        public int CampaignMediaId { get; set; }
        public string Notes { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public LabelInfo()
        {
            Id = -1;
            PortalId = -1;
            UserType = "";
            UserId = -1;
            CampaignId = -1;
            AgencyName = "";
            AgencyId = -1;
            AdvertiserName = "";
            AdvertiserId = -1;
            TapeFormat = -1;
            Title = "";
            Description = "";
            ISCICode = "";
            PMTMediaId = "";
            MediaLength = "";
            Standard = "";
            LabelNumber = -1;
            CampaignMediaId = -1;
            Notes = "";
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class MasterItemInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public string Filename { get; set;}
        public int AdvertiserId { get; set; }
        public string Advertiser { get; set; }
        public List<AgencyInfo> Agencies { get; set; }
        public string AgencyNames { get; set; }
        public int BillToId { get; set; }
        public string Title { get; set; }
        public int MediaType { get; set; }
        public int Encode { get; set; }
        public string Standard { get; set; }
        public string Length { get; set; }
        public string CustomerId { get; set; }
        public string PMTMediaId { get; set; }
        public int Reel { get; set; }
        public string TapeCode { get; set; }
        public int Position { get; set; }
        public string VaultId { get; set; }
        public string Location { get; set; }
        public string Comment { get; set; }
        public bool ClosedCaptioned { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string CheckListForm { get; set; }
        public bool isApproved { get; set; }
        public bool hasChecklist { get; set; }
        public MasterItemInfo()
        {
            Id = -1;
            PortalId = -1;
            Filename = "";
            AdvertiserId = -1;
            Advertiser = "";
            Agencies = new List<AgencyInfo>();
            AgencyNames = "";
            BillToId = -1;
            Title = "";
            MediaType = -1;
            Encode = -1;
            Standard = "";
            Length = "";
            CustomerId = "";
            PMTMediaId = "";
            //Reel = -1;
            TapeCode = "";
            //Position = -1;
            VaultId = "";
            Location = "";
            Comment = "";
            ClosedCaptioned = false;
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
            CheckListForm = "";
            isApproved = false;
            hasChecklist = false;
        }        
    }
    [Serializable]
    public class LibraryItemInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public int AdvertiserId { get; set; }
        public string Advertiser { get; set; }
        public int AgencyId { get; set; }
        public string Agency { get; set; }
        public string ISCICode { get; set; }
        public string Filename { get; set; }
        public string Title { get; set; }
        public string ProductDescription { get; set; }
        public string MediaLength { get; set; }
        public string MediaType { get; set; }
        public string Encode { get; set; }
        public string Standard { get; set; }
        public string CustomerReference { get; set; }
        public string PMTMediaId { get; set; }
        public int MasterId { get; set; }
        public DateTime DateCreated { get; set; }
        public int MediaIndex { get; set; }
        public int Reel { get; set; }
        public string TapeCode { get; set; }
        public int Position { get; set; }
        public string VaultId { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string ClosedCaptioned { get; set; }
        public int CreatedById { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public LibraryItemInfo()
        {
            Id = -1;
            AdvertiserId = -1;
            Advertiser = "";
            AgencyId = -1;
            Agency = "";
            ISCICode = "";
            Filename = "";
            Title = "";
            ProductDescription = "";
            MediaLength = "00:00";
            MediaType = "";
            Encode = "";
            Standard = "";
            CustomerReference = "";
            PMTMediaId = "";
            MasterId = -1;
            DateCreated = DateTime.Now;
            MediaIndex = -1;
            Reel = -1;
            TapeCode = "";
            Position = -1;
            VaultId = "";
            Location = "";
            Status = "";
            Comment = "";
            ClosedCaptioned = "No";
            CreatedById = -1;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class AdvertiserAgencyInfo
    {
        public int AdvertiserId { get; set; }
        public int AgencyId { get; set; }
        public AdvertiserAgencyInfo()
        {
            AdvertiserId = -1;
            AgencyId = -1;
        }
    }
    [Serializable]
    public class MasterItemAgencyInfo
    {
        public int MasterItemId { get; set; }
        public int AgencyId { get; set; }
        public MasterItemAgencyInfo()
        {
            MasterItemId = -1;
            AgencyId = -1;
        }
    }
    [Serializable]
    public class WOGroupInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public int WorkOrderId { get; set; }
        public int index { get; set; }
        public GroupTypeEnum GroupType { get; set; }
        public string GroupName { get; set; }
        public string Comments { get; set; }
        public int MasterId { get; set; }
        public MasterItemInfo Master { get; set; }
        public List<ServiceInfo> Services { get; set; }
        public List<LibraryItemInfo> LibraryItems { get; set; }
        public List<WOGroupStationInfo> WOGroupStations { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public WOGroupInfo()
        {
            Id = -1;
            PortalId = -1;
            WorkOrderId = -1;
            index = 0;
            GroupType = GroupTypeEnum.Bundle;
            GroupName = "";
            Comments = "";
            MasterId = -1;
            Master = new MasterItemInfo();
            Services = new List<ServiceInfo>();
            LibraryItems = new List<LibraryItemInfo>();
            WOGroupStations = new List<WOGroupStationInfo>();
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
        
    }
    [Serializable]
    public class WOGroupStationInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public int WOGroupId { get; set; }
        public int WorkOrderId { get; set; }
        public int StationId { get; set; }
        public StationInfo Station { get; set; }
        public int DeliveryMethodId { get; set; }
        public string DeliveryMethod { get; set; }
        public int ShippingMethodId { get; set; }
        public string ShippingMethod { get; set; }
        public int PriorityId { get; set; }
        public string Priority { get; set; }
        public int Quantity { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public WOGroupStationInfo()
        {
            Id = -1;
            PortalId = -1;
            WOGroupId = -1;
            WorkOrderId = -1;
            StationId = -1;
            Station = new StationInfo();
            DeliveryMethodId = -1;
            DeliveryMethod = "";
            ShippingMethodId = -2;
            ShippingMethod = "";
            PriorityId = -1;
            Priority = "";
            Quantity = 1;
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class WorkOrderJobInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public int WorkOrderId { get; set; }
        public int WOStationId { get; set; }
        public WOGroupStationInfo WOStation { get; set; }
        public int LibraryItemId { get; set; }
        public LibraryItemInfo LibraryItem { get; set; }
        public DateTime DateDelivered { get; set; }
        public int DeliveryMethodId { get; set; }
        public DeliveryMethodInfo DeliveryMethod { get; set; }
        public int PriorityId { get; set; }
        public string Priority { get; set; }
        public int QuickBookBillingCodeId { get; set; }
        public string QuickBookBillingCode { get; set; }
        public string Status { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public WorkOrderJobInfo()
        {
            Id = -1;
            PortalId = -1;
            WorkOrderId = -1;
            WOStationId = -1;
            WOStation = new WOGroupStationInfo();
            LibraryItemId = -1;
            LibraryItem = new LibraryItemInfo();
            DeliveryMethodId = -1;
            DeliveryMethod = new DeliveryMethodInfo();
            PriorityId = -1;
            Priority = "";
            QuickBookBillingCodeId = -1;
            QuickBookBillingCode = "";
            Status = "";
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    
    [Serializable]
    public class ServiceInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public string ServiceName { get; set; }
        public string BillingCode { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public ServiceInfo()
        {
            Id = -1;
            PortalId = -1;
            ServiceName = "";
            BillingCode = "";
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class WorkOrderInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public string Description { get; set; }
        public string PONumber { get; set; }
        public string InvoiceNumber { get; set; }
        public int AdvertiserId { get; set; }
        public string AdvertiserName {get;set;}
        public int AgencyId { get; set; }
        public string AgencyName { get; set; }
        public int BillToId { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public List<WOGroupInfo> Groups { get; set; }
        public int AssignedTo { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public WorkOrderInfo()
        {
            Id = -1;
            PortalId = -1;
            Description = "";
            PONumber = "";
            InvoiceNumber = "";
            AdvertiserId = -1;
            AdvertiserName = "";
            AgencyId = -1;
            AgencyName = "";
            Groups = new List<WOGroupInfo>();
            BillToId = -1;
            Notes = "";
            Status = "READY FOR REVIEW";
            Priority = "Normal";
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class WOCommentInfo
    {
        public int Id { get; set; }
        public int PortalId {get;set;}
        public int WorkOrderId { get; set; }
        public int WOTaskId { get; set; }
        public string Comment { get; set; }
        public string DisplayName { get; set; }
        public CommentTypeEnum CommentType { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public WOCommentInfo()
        {
            Id = -1;
            PortalId = -1;
            WorkOrderId = -1;
            WOTaskId = -1;
            Comment = "";
            DisplayName = "";
            CommentType = CommentTypeEnum.Comment;
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class TaskInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public string Description { get; set; }
        public int WorkOrderId { get; set; }
        public int WOGroupId { get; set; }
        public GroupTypeEnum TaskType { get; set; }
        public int MasterId { get; set; }
        public int LibraryId { get; set; }
        public int StationId { get; set; }
        public int DeliveryMethodId { get; set; }
        public string DeliveryMethod { get; set; }
        public string DeliveryMethodResponse { get; set; }
        public string DeliveryOrderId { get; set; }
        public DateTime DeliveryOrderDateCreated { get; set; }
        public DateTime DeliveryOrderDateComplete { get; set; }
        public string DeliveryStatus { get; set; }
        public bool isComplete { get; set; }
        public bool isDeleted { get; set; }
        public string QBCode { get; set; }
        public int QBCodeId { get; set; }
        public List<QBCodeInfo> QBCodes { get; set; }
        public int Quantity { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public TaskInfo()
        {
            Id = -1;
            PortalId = -1;
            WorkOrderId = -1;
            WOGroupId = -1;
            TaskType = GroupTypeEnum.Delivery;
            MasterId = -1;
            LibraryId = -1;
            StationId = -1;
            DeliveryMethodId = -1;
            DeliveryMethod = "";
            DeliveryMethodResponse = "";
            DeliveryOrderId = "";
            DeliveryOrderDateCreated = DateTime.Now;
            DeliveryOrderDateComplete = DateTime.Now;
            DeliveryStatus = "";
            isComplete = false;
            isDeleted = false;
            QBCode = "";
            QBCodeId = -1;
            QBCodes = new List<QBCodeInfo>();
            Quantity = 0;
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class QBCodeInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public GroupTypeEnum Type { get; set; }
        public string QBCode { get; set; }
        public MediaTypeEnum MediaType { get; set; }
        public string MinLength { get; set; }
        public string MaxLength { get; set; }
        public List<DeliveryMethodInfo> DeliveryMethods { get; set; }
        public List<TapeFormatInfo> TapeFormats { get; set; }
        public List<ServiceInfo> Services { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public QBCodeInfo()
        {
            Id = -1;
            PortalId = -1;
            Type = GroupTypeEnum.Delivery;
            QBCode = "";
            MediaType = MediaTypeEnum.HD;
            MinLength = "00:00";
            MaxLength = "00:00";
            DeliveryMethods = new List<DeliveryMethodInfo>();
            TapeFormats = new List<TapeFormatInfo>();
            Services = new List<ServiceInfo>();
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class statAssoc
    {
        public int Id { get; set; }
        public int campaignId { get; set; }
        public int groupId { get; set; }
        public int mediaId { get; set; }
        public string trackingId { get; set; }
        public string service { get; set; }
        public string dateSigned { get; set; }
        public string timeSigned { get; set; }
        public string signedBy { get; set; }
        public string DeliveryMethod { get; set; }
        public string QBCode { get; set; }
        public string DeliveryType { get; set; }
        public statAssoc()
        {
            Id = -1;
            campaignId = -1;
            groupId = -1;
            mediaId = -1;
            trackingId = "";
            service = "";
            dateSigned = "";
            timeSigned = "";
            signedBy = "";
            DeliveryMethod = "";
            QBCode = "";
            DeliveryType = "";
        }
    }
    [Serializable]
    public class ReportInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public string ReportName { get; set; }
        public int ReportType { get; set; }
        public int AdvertiserId { get; set; }
        public int AgencyId { get; set; }
        public string Keyword { get; set; }
        public int Status { get; set; }
        public string Frequency { get; set; }
        public DateTime FirstReportDate { get; set; }
        public string EmailTo { get; set; }
        public string EmailMessage { get; set; }
        public bool isActive { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public ReportInfo()
        {
            Id = -1;
            PortalId = -1;
            ReportName = "";
            ReportType = 3;
            AdvertiserId = -1;
            AgencyId = -1;
            Keyword = "";
            Status = -1;
            Frequency = "daily";
            FirstReportDate = DateTime.Now.AddDays(1);
            EmailTo = "";
            EmailMessage = "";
            isActive = true;
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class OtsmSpot
    {
        public string Agency { get; set; }
        public string Advertiser { get; set; }
        public string Brand { get; set; }
        public string MediaType { get; set; }
        public string ISCI { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Status { get; set; }
        public string ProxyURL { get; set; }
        public List<OtsmSpotOrder> Otsm { get; set; }
    }
    [Serializable]
    public class OtsmSpotOrder
    {
        public int OrderID { get; set; }
        public string Status { get; set; }
        public DateTime LaunchDate { get; set; }
        public string OtsmPO { get; set; }
        public string Group { get; set; }
    }
    [Serializable]
    public class EasySpotObjectInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public EasySpotTypeEnum ObjectType { get; set; }
        public int PMTObjectId { get; set; }
        public string EasySpotObjectId { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public EasySpotObjectInfo()
        {
            Id = -1;
            PortalId = -1;
            ObjectType = EasySpotTypeEnum.Address;
            PMTObjectId = -1;
            EasySpotObjectId = "";
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    [Serializable]
    public class InvoiceInfo
    {
        public int Id { get; set; }
        public int PortalId { get; set; }
        public List<WorkOrderInfo> WorkOrders { get; set; }
        public int AdvertiserId { get; set; }
        public int AgencyId { get; set; }
        public int BillToId { get; set; }
        public string QBInvoiceNumber { get; set; }
        public bool SentToQB { get; set; }
        public string QBMessage { get; set; }
        public int CreatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public InvoiceInfo()
        {
            Id = -1;
            PortalId = -1;
            WorkOrders = new List<WorkOrderInfo>();
            AdvertiserId = -1;
            AgencyId = -1;
            BillToId = -1;
            QBInvoiceNumber = "";
            SentToQB = false;
            QBMessage = "";
            CreatedById = -1;
            DateCreated = DateTime.Now;
            LastModifiedById = -1;
            LastModifiedDate = DateTime.Now;
        }
    }
    public class AdvertiserComparer : IEqualityComparer<AdvertiserInfo>
    {
        // Advertisers are equal if their names and Advertiser numbers are equal.
        public bool Equals(AdvertiserInfo x, AdvertiserInfo y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the Advertisers' properties are equal.
            return x.Id == y.Id;
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(AdvertiserInfo Advertiser)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(Advertiser, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashAdvertiserId = Advertiser.Id == null ? 0 : Advertiser.Id.GetHashCode();

            //Calculate the hash code for the Advertiser.
            return hashAdvertiserId;
        }
    }
    public class AgencyComparer : IEqualityComparer<AgencyInfo>
    {
        public bool Equals(AgencyInfo x, AgencyInfo y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
            return x.Id == y.Id;
        }
        public int GetHashCode(AgencyInfo Agency)
        {
            if (Object.ReferenceEquals(Agency, null)) return 0;
            int hashAgencyId = Agency.Id == null ? 0 : Agency.Id.GetHashCode();
            return hashAgencyId;
        }
    }
    public class MasterItemComparer : IEqualityComparer<MasterItemInfo>
    {
        public bool Equals(MasterItemInfo x, MasterItemInfo y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
            return x.Id == y.Id;
        }
        public int GetHashCode(MasterItemInfo MasterItem)
        {
            if (Object.ReferenceEquals(MasterItem, null)) return 0;
            int hashMasterItemId = MasterItem.Id == null ? 0 : MasterItem.Id.GetHashCode();
            return hashMasterItemId;
        }
    }
    public class LibraryItemComparer : IEqualityComparer<LibraryItemInfo>
    {
        public bool Equals(LibraryItemInfo x, LibraryItemInfo y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
            return x.Id == y.Id;
        }
        public int GetHashCode(LibraryItemInfo LibraryItem)
        {
            if (Object.ReferenceEquals(LibraryItem, null)) return 0;
            int hashLibraryItemId = LibraryItem.Id == null ? 0 : LibraryItem.Id.GetHashCode();
            return hashLibraryItemId;
        }
    }
    public class WorkOrderComparer : IEqualityComparer<WorkOrderInfo>
    {
        public bool Equals(WorkOrderInfo x, WorkOrderInfo y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.Id == y.Id;
        }

        public int GetHashCode(WorkOrderInfo WorkOrder)
        {
            if (Object.ReferenceEquals(WorkOrder, null)) return 0;
            int hashWorkOrderId = WorkOrder.Id == null ? 0 : WorkOrder.Id.GetHashCode();
            return hashWorkOrderId;
        }
    }    
}