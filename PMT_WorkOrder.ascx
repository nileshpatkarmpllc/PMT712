<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMT_WorkOrder.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.PMT_WorkOrder" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %> 
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ register tagprefix="dnn" tagname="Label" src="~/controls/LabelControl.ascx" %>

<dnn:DnnCssInclude ID="DnnCssInclude3" runat="server" FilePath="/DesktopModules/PMT_Admin/styles/style.css" Priority="11" />

<script>
    $(function () {
        /*
         * this swallows backspace keys on any non-input element.
         * stops backspace -> back
         */
        var rx = /INPUT|SELECT|TEXTAREA/i;

        $(document).bind("keydown keypress", function (e) {
            if (e.which == 8) { // 8 == backspace
                if (!rx.test(e.target.tagName) || e.target.disabled || e.target.readOnly) {
                    e.preventDefault();
                }
            }
        });
    });
</script>

<style>
    .wostatus {
        width:11% !important;
        min-width:140px;
    }
.bulkTextarea {
    height:auto !important;
    width:50%;
    border:1px solid #dedede;
    margin-left:10px;
}
.add-button {
    
}
.add-buttonBulk {
  display: block;
  float: left;
  position:relative;
  clear: both;
  width: 80px;
  height: 25px;
  margin-top: 0px;
  border-radius: 3px;
  margin-left:10px;
  background-color: rgb(52, 152, 219);
  font-size: 0.857em;
  font-weight: 300;
  text-align: center;
  letter-spacing: 1px;
  color: rgb(255, 255, 255);
  z-index:1000;
}
</style>

<asp:UpdateProgress ID="updWorkOrder" AssociatedUpdatePanelID="pnlWorkOrder" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="/DesktopModules/PMT_Admin/images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<div id="divScreen">
<asp:UpdatePanel ID="pnlWorkOrder" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div class="work-orderinfo-header"><h3>WORK ORDER INFO</h3></div>
        <div class="workorder-info-panel">
    <div class="work-orderinfo-header"></div>
    <asp:TextBox ID="txtWorkOrderId" CssClass="work-order" Enabled="false" placeholder="WORK ORDER ID :" runat="server" />
    <asp:TextBox ID="txtWorkOrderDescription" CssClass="description" placeholder="DESCRIPTION :" runat="server" />
    <asp:TextBox ID="txtWorkOrderPONumber" CssClass="po-number" placeholder="P.O. NUMBER :" runat="server" />
    <asp:TextBox ID="txtWorkOrderQBBillingCode" CssClass="qb-billing" placeholder="INVOICE # :" runat="server" />
    <asp:DropDownList ID="ddlAdvertisers" CssClass="advertiser" ToolTip="Advertiser" AutoPostBack="true" OnSelectedIndexChanged="ddlAdvertisers_SelectedIndexChanged" runat="server"></asp:DropDownList>           
    <asp:DropDownList ID="ddlAgencies" CssClass="agencies" ToolTip="Agency" AutoPostBack="true" OnSelectedIndexChanged="ddlAgencies_SelectedIndexChanged" runat="server"></asp:DropDownList>            
            <asp:Label ID="lblStatus" runat="server" CssClass="agencies wostatus"></asp:Label>
            <asp:DropDownList ID="ddlBillTo" CssClass="agencies" ToolTip="Bill To" runat="server"></asp:DropDownList><br clear="both" />
  </div><br clear="both" />

        <asp:Panel ID="pnlEditMode" runat="server">
        <div class="master-items-panel">
    <div class="header-panel-grey header-panel-grey-1"></div>
    <p class="text masteritems">MASTER ITEMS</p>
    <div class="container keyword search">
      <asp:TextBox ID="txtMasterKeyword" AutoPostBack="true" OnTextChanged="txtMasterKeyword_TextChanged" CssClass="masterKeyword" placeholder="Search : ID & Title" runat="server"></asp:TextBox>
      <img class="search-image" src="/DesktopModules/PMT_Admin/images/SEARCH-IMAGE.png" />
        <div class="MastersTotal">&nbsp;<asp:Label ID="lblMastersTotal" runat="server"></asp:Label></div>
    </div>
    <div class="container select-stations">
      <div class="text selectall"><asp:Button Text="Select All" ID="btnMastersSelectAll" runat="server" OnClick="btnMastersSelectAll_Click" /></div>&nbsp;
      <div class="text selectall"><asp:Button Text="Clear All" ID="btnMastersClearAll" runat="server" OnClick="btnMastersClearAll_Click" /></div>
      <asp:ImageButton ID="imgExpandMasters" CssClass="imgMastersExpand" runat="server" ImageUrl="/DesktopModules/PMT_Admin/images/arrow_open.jpg" OnClick="imgExpandMasters_Click" />
    </div>

            <asp:Panel ID="pnlMasters" runat="server">
    <div class="masteritems-list" runat="server">
        <div id="mastersHeader">
            <div class="mastersHeader mastersCol1">MASTER ID</div>
            <div class="mastersHeader mastersCol2">TITLE</div>
            <div class="mastersHeader mastersCol3">AGENCY</div>
            <div class="mastersHeader mastersCol4">ADVERTISER</div>
            <div class="mastersHeader mastersCol5">LENGTH</div>
            <div class="mastersHeader mastersCol6">STATUS</div>
            <div class="mastersHeader mastersCol7">VIEW</div>
            <div class="mastersHeader mastersCol8">ADD</div>
        </div>
        <asp:PlaceHolder ID="plMasters" runat="server"></asp:PlaceHolder>
    </div>
    
                <div class="header-panel-grey header-panel-grey-2" style="background-color:white;"></div>
                <div class="container addtogroup">
                  <p class="text addto">Create New:</p>
                  <asp:Button ID="btnAddToNonDelivery" CssClass="nonDeliveryButton" OnClick="btnAddToNonDelivery_Click" runat="server" Text="NON-DELIVERY GROUP(S)" />
                </div>
            </asp:Panel>
    </div>

        <br clear="both" />
        <div class="library-items-panel">
    <div class="header-panel-grey header-panel-grey-1"></div>
    <p class="text libraryitems">LIBRARY ITEMS</p>
    <div class="container keyword search">
      <asp:TextBox ID="txtLibrarySearch" AutoPostBack="true" OnTextChanged="txtLibrarySearch_TextChanged" CssClass="masterKeyword" placeholder="Search : ID & Title" runat="server"></asp:TextBox>
      <img class="search-image" src="/DesktopModules/PMT_Admin/images/SEARCH-IMAGE.png">
        
        <div class="MastersTotal">&nbsp;<asp:Label ID="lblLibraryTotal" runat="server"></asp:Label></div>
    </div>
    <div class="container select-stations">
      <div class="text selectall"><asp:Button Text="Select All" ID="btnLibrarySelectAll" runat="server" OnClick="btnLibrarySelectAll_Click" /></div>&nbsp;
      <div class="text selectall"><asp:Button Text="Clear All" ID="btnLibraryClearAll" runat="server" OnClick="btnLibraryClearAll_Click" /></div>
      <asp:ImageButton ID="imgExpandLibrary" CssClass="imgMastersExpand" runat="server" ImageUrl="/DesktopModules/PMT_Admin/images/arrow_open.jpg" OnClick="imgExpandLibrary_Click" />
    </div>

            <asp:Panel ID="pnlLibrary" runat="server">
    <div class="masteritems-list" runat="server">
        <div id="mastersHeader1">
            <div class="mastersHeader libraryCol1">MASTER ID</div>
            <div class="mastersHeader libraryCol2">ISCI</div>
            <div class="mastersHeader libraryCol3">TITLE</div>
            <div class="mastersHeader libraryCol4">DESCRIPTION</div>
            <div class="mastersHeader libraryCol5">LENGTH</div>
            <div class="mastersHeader libraryCol6">ADVERTISER</div>
            <div class="mastersHeader libraryCol7">ADD</div>
        </div>
        <asp:PlaceHolder ID="plLibrary" runat="server"></asp:PlaceHolder>
    </div>
        <asp:Button ID="btnToggleBulk" runat="server" OnClick="btnToggleBulk_Click" CssClass="add-buttonBulk" Text="Bulk Add" />
        <asp:TextBox ID="txtAddBulk" runat="server" Rows="2" CssClass="bulkTextarea" TextMode="MultiLine" Visible ="false"></asp:TextBox><br clear="both" />
<div class="header-panel-grey header-panel-grey-2" style="background-color:white;"></div>
    <div class="container addtogroup">
                
        
      <p class="text addto">Add to :</p>
      <asp:DropDownList runat="server" ID="ddlLibraryAddToGroup" CssClass="name group" >
      </asp:DropDownList>
      <asp:Button runat="server" ID="btnLibraryAdd" OnClick="btnLibraryAdd_Click" CssClass="add-button" Text="Add" />
    </div>
            </asp:Panel>
    </div>

        <br clear="both" />
        <div class="library-items-panel">
    <div class="header-panel-grey header-panel-grey-1"></div>
    <p class="text libraryitems">ADD STATIONS</p>
    <div class="container keyword search">
       <div class="text selectall">Group Search: <asp:DropDownList ID="ddlStationGroups" runat="server" ToolTip="If you have no groups defined, feel free to create them on the Station Groups page.  They are for your convenience and are not a requirement." OnSelectedIndexChanged="ddlStationGroups_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></div>
      <asp:TextBox ID="txtStationSearch" AutoPostBack="true" OnTextChanged="txtStationSearch_TextChanged" CssClass="masterKeyword" placeholder="Search: Call Letters, City or Market" runat="server"></asp:TextBox>
      <asp:DropDownList ID="ddlStationFormat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStationFormat_SelectedIndexChanged"><asp:ListItem Text="--Select Format--" Value="-1"></asp:ListItem><asp:ListItem Value="Short Form" Text="Short Form"></asp:ListItem><asp:ListItem Value="Long Form" Text="Long Form"></asp:ListItem></asp:DropDownList> <img class="search-image" src="/DesktopModules/PMT_Admin/images/SEARCH-IMAGE.png">
        <div class="MastersTotal">&nbsp;<asp:Label ID="lblStationsTotal" runat="server"></asp:Label></div>
    </div>
    <div class="container select-stations">
      <div class="text selectall"><asp:Button Text="Select All" ID="btnStationsSelectAll" runat="server" OnClick="btnStationsSelectAll_Click" /></div>&nbsp;
      <div class="text selectall"><asp:Button Text="Clear All" ID="btnStationsClearAll" runat="server" OnClick="btnStationsClearAll_Click" /></div>
      <asp:ImageButton ID="imgExpandStations" CssClass="imgMastersExpand" runat="server" ImageUrl="/DesktopModules/PMT_Admin/images/arrow_open.jpg" OnClick="imgExpandStations_Click" />
    </div>

            <asp:Panel ID="pnlStations" runat="server">
    <div class="masteritems-list" runat="server">
        <div id="mastersHeader2">
            <div class="mastersHeader stationsCol1">CALL LETTERS</div>
            <div class="mastersHeader stationsCol2">MARKET</div>
            <div class="mastersHeader stationsCol3">CITY</div>
            <div class="mastersHeader stationsCol4">ADD</div>
        </div>
        <asp:PlaceHolder ID="plStations" runat="server"></asp:PlaceHolder>
    </div>
<div class="header-panel-grey header-panel-grey-2" style="background-color:white;"></div>
    <div class="container addtogroup">
      <p class="text addto">Add to :</p>
      <asp:DropDownList runat="server" ID="ddlStationsAddToGroup" CssClass="name group" >
      </asp:DropDownList>
      <asp:Button runat="server" ID="btnStationsAdd" OnClick="btnStationsAdd_Click" CssClass="add-button" Text="Add" />
    </div>

                <div class="header-panel-grey header-panel-grey-2" style="background-color:white;"></div>
                <div class="container addtogroup3">
                  <p class="text2 addto">Create New:</p>
                  <asp:Button ID="btnDeliveryGroup" CssClass="nonDeliveryButton" OnClick="btnDeliveryGroup_Click" runat="server" Text="DELIVERY GROUP" />
                    <asp:Button ID="btnBundleGroup" CssClass="nonDeliveryButton" OnClick="btnBundleGroup_Click" runat="server" Text="BUNDLE GROUP" />
                    <asp:Button ID="btnCustomizeGroup" CssClass="nonDeliveryButton" OnClick="btnCustomizeGroup_Click" runat="server" Text="CUSTOMIZED GROUP" />
                </div>
            </asp:Panel>
    <br clear="both" />

            </asp:Panel><br clear="both" />

        <asp:Panel ID="pnlGroups" runat="server"></asp:Panel>
        <br clear="both" />
        <div style="float:left;width:50%;" class="noprint"><div class="groupNotesHeader woNotes">WORKORDER NOTES:</div>
                <div class="groupNotes woNotes"><asp:TextBox ID="txtWONotes" runat="server" TextMode="MultiLine" Rows="4" Wrap="true" placeholder="Notes: " ViewStateMode="Enabled"></asp:TextBox></div></div>
        <div style="float:left;width:50%;" class="noprint"><div class="groupNotesHeader woNotes">WORKORDER SUMMARY:</div>
                <div class="groupNotes woNotes"><asp:Literal ID="litWOSummary" runat="server"></asp:Literal></div>
        <br clear="both" />
        <asp:Button ID="btnSubmitWorkOrder" runat="server" CssClass="createOrderButton" Text="CREATE ORDER" OnClick="btnSubmitWorkOrder_Click" />
        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="window.print();" CssClass="createOrderButton" Visible="false" /><br clear="both" />
        <asp:Label ID="lblWOMessage" runat="server"></asp:Label></div>
    </ContentTemplate>
</asp:UpdatePanel>
            <br clear="both" />
    </div></div><br clear="both" />
<div id="divPrint">
    <asp:Panel ID="plPrintWorkOrder" CssClass="WorkOrderPrint" runat="server">
        <div class="col1">
            <h1>Pacific Digital Distribution</h1>
            11112 Ventura Blvd.<br />
            Studio City, CA 91604<br />
            Tel: (818) 753-4700<br />
            Fax: (818) 753-1416
        </div>
        <div class="col2">
            <strong>Order: <asp:Label ID="lblOrderNumber" runat="server"></asp:Label></strong><br />
            Cust Rep: <strong><asp:Label ID="lblCustRep" runat="server"></asp:Label></strong><br />
            Due Date: <strong><asp:Label ID="lblDueDate" runat="server"></asp:Label></strong><br />
            Cust Ref: <strong><asp:Label ID="lblCustRef" runat="server"></asp:Label></strong>
        </div><br clear="both" /><br />

    </asp:Panel>
    <asp:Panel ID="plPrintWorkOrder2" CssClass="WorkOrderPrint" runat="server">
        <div class="col1">
            <h1>Pacific Digital Distribution</h1>
            11112 Ventura Blvd.<br />
            Studio City, CA 91604<br />
            Tel: (818) 753-4700<br />
            Fax: (818) 753-1416
        </div>
        <div class="col2">
            <strong>Order: <asp:Label ID="lblOrderNumber2" runat="server"></asp:Label></strong><br />
            Cust Rep: <strong><asp:Label ID="lblCustRep2" runat="server"></asp:Label></strong><br />
            Due Date: <strong><asp:Label ID="lblDueDate2" runat="server"></asp:Label></strong><br />
            Cust Ref: <strong><asp:Label ID="lblCustRef2" runat="server"></asp:Label></strong>
        </div><br clear="both" /><br />

    </asp:Panel>
            <asp:PlaceHolder ID="plPackingSlips" runat="server"></asp:PlaceHolder>
</div>