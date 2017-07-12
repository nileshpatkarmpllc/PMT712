<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMT_ImportTool.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.PMT_ImportTool" %>

<asp:TextBox ID="txtScript" TextMode="MultiLine" runat="server" Width="800" Height="500"></asp:TextBox>
<br />
<asp:Label ID="lblMessage" runat="server"></asp:Label><br />
<asp:Button ID="btnAdvertisers" Text="Import Advertisers" runat="server" OnClick="btnAdvertisers_Click" OnClientClick="return confirm('Are you certain you want to Clear and Import Advertisers?');" /><br />
<asp:Button ID="btnAgencies" Text="Import Agencies" runat="server" OnClick="btnAgencies_Click" OnClientClick="return confirm('Are you certain you want to Clear and Import Agencies?');" /><br />
<asp:Button ID="btnAdAgs" Text="Import AdvertiserAgencies" runat="server" OnClick="btnAdAgs_Click" OnClientClick="return confirm('Are you certain you want to Clear and Import AdvertiserAgencies?');" /><br />
<asp:Button ID="btnLabels" Text="Import Labels" runat="server" OnClick="btnLabels_Click" OnClientClick="return confirm('Are you certain you want to Clear and Import Labels?');" /><br />
<asp:Button ID="btnLibraryItems" Text="Import Library Items" runat="server" OnClick="btnLibraryItems_Click" OnClientClick="return confirm('Are you certain you want to Clear and Import Library Items?');" /><br />
<asp:Button ID="btnMasterItems" Text="Import Master Items" runat="server" OnClick="btnMasterItems_Click" OnClientClick="return confirm('Are you certain you want to Clear and Import Master Items?');" /><br />
<asp:Button ID="btnMasterItemAgencies" Text="Import Master Item Agencies" runat="server" OnClick="btnMasterItemAgencies_Click" OnClientClick="return confirm('Are you certain you want to Clear and Import Master Item Agencies?');" /><br />
<asp:Button ID="btnStations" Text="Import Stations" runat="server" OnClick="btnStations_Click" OnClientClick="return confirm('Are you certain you want to Clear and Import Stations?');" /><br />
<asp:Button ID="btnTasks" Text="Import Tasks" runat="server" OnClick="btnTasks_Click" OnClientClick="return confirm('Are you certain you want to Import Tasks?');" /><br />

<asp:Button ID="btnMakeCustomizeCodes" Text="Create Customize Codes for QB" runat="server" OnClick="btnMakeCustomizeCodes_Click" OnClientClick="return confirm('Are you certain you want to create QB Codes for Customize?');" /><br />
<asp:Button ID="btnMakeBundleCodes" Text="Create Bundle Codes for QB" runat="server" OnClick="btnMakeBundleCodes_Click" OnClientClick="return confirm('Are you certain you want to create QB Codes for Bundle?');" /><br />
<asp:Button ID="btnLongFormStations" Text="Import Long Form Stations" runat="server" OnClick="btnLongFormStations_Click" OnClientClick="return confirm('Are you certain you want to import Long Form Stations?');" /><br />
<asp:Button ID="btnLongFormCustomers" Text="Import Long Form Customers" runat="server" OnClick="btnLongFormCustomers_Click" OnClientClick="return confirm('Are you certain you want to import Long Form Customers?');" /><br />
<asp:Button ID="btnLongFormMasters" Text="Import Long Form Masters" runat="server" OnClick="btnLongFormMasters_Click" OnClientClick="return confirm('Are you certain you want to import Long Form Masters?');" /><br />
<asp:Button ID="btnLongFormLibraryItems" Text="Import Long Form Library Items" runat="server" OnClick="btnLongFormLibraryItems_Click" OnClientClick="return confirm('Are you certain you want to import Long Form Library Items?');" /><br />
<asp:Button ID="btnUpdateDeliveryDates" Text="Fix Delivery Dates" runat="server" OnClick="btnUpdateDeliveryDates_Click" OnClientClick="return confirm('Are you certain you want to Fix Delivery Dates?');" />