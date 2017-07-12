<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMT_Admin.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.PMT_Admin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %> 
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ register tagprefix="dnn" tagname="Label" src="~/controls/LabelControl.ascx" %>

<dnn:DnnCssInclude ID="DnnCssInclude3" runat="server" FilePath="http://code.jquery.com/ui/1.11.3/themes/smoothness/jquery-ui.css" Priority="11" />

<script>
    $(document).ready(function () {
        $('#tabs-admin').tabs();
    });
</script>

<style>
    .redButton {
        background-color:#ff7777 !important;
        background: rgba(0, 0, 0, 0) -moz-linear-gradient(center top , #ff7777 0%, #dd5555 100%) repeat scroll 0 0 !important;
    }
    .modalBackground { 
            background-color:#333333; 
            filter:alpha(opacity=70); 
            opacity:0.7; 
        } 
        .modalPopup { 
            background-color:#FFFFFF; 
            border-width:1px; 
            border-style:solid; 
            border-color:#CCCCCC; 
            padding:10px; 
            width:600px; 
            Height:auto; 
        } 
        .loading-panel {
                background: rgba(0, 0, 0, 0.2) none repeat scroll 0 0;
                position: relative;
                width: 100%;
            }

            .loading-container {
                background: rgba(49, 133, 156, 0.4) none repeat scroll 0 0;
                color: #fff;
                font-size: 90px;
                height: 100%;
                left: 0;
                padding-top: 15%;
                position: fixed;
                text-align: center;
                top: 0;
                width: 100%;
                z-index: 999999;
            }
            .bckclr3 {
                background-color: #CCFFCC;
                color: #000000;

            }

            .bckclr6 {
                background-color: #999999;
                color: #fff;
                width: 14%;
            }
</style>
<asp:UpdateProgress ID="updAgencies" AssociatedUpdatePanelID="pnlAgencies" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdateProgress ID="updAdvertiser" AssociatedUpdatePanelID="pnlAdvertisers" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdateProgress ID="updMarkets" AssociatedUpdatePanelID="pnlMarkets" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdateProgress ID="updStations" AssociatedUpdatePanelID="pnlStations" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdateProgress ID="updStationGroups" AssociatedUpdatePanelID="pnlStationGroups" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdateProgress ID="updLabels" AssociatedUpdatePanelID="pnlLabels" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdateProgress ID="UpdateMasterItems" AssociatedUpdatePanelID="pnlMasterItems" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdateProgress ID="UpdateUsers" AssociatedUpdatePanelID="pnlUsers" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>


<div style="position:relative;">

<div id="tabs-admin">
    <ul>
        <li id="liAgency" runat="server" visible="false"><a href="#PMT_Agencies">Agencies</a></li>
        <li id="liAdvertiser" runat="server" visible="false"><a href="#PMT_Advertisers">Advertisers</a></li>
        <li id="liMarket" runat="server" visible="false"><a href="#PMT_Markets">Markets</a></li>
        <li id="liStation" runat="server" visible="false"><a href="#PMT_Stations">Stations</a></li>        
        <li id="liStationGroup" runat="server" visible="false"><a href="#PMT_StationGroups">Station Groups</a></li>
        <li id="liLabels" runat="server" visible="false"><a href="#PMT_Labels">Labels</a></li>
        <li id="liMasterItems" runat="server" visible="false"><a href="#PMT_MasterItems">Master Items</a></li>
        <li id="liUsers" runat="server" visible="false"><a href="#PMT_Users">Users</a></li>
    </ul>

    <asp:UpdatePanel ID="pnlAdmin" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            

    <div id="PMT_Agencies">
        <asp:UpdatePanel ID="pnlAgencies" runat="server" UpdateMode="Always" Visible="false">
            <ContentTemplate>
                <h3>Agencies</h3>
                <fieldset>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblAgencyName" runat="server" />
                        <asp:TextBox ID="txtAgencyName" placeholder="Agency Name" runat="server" />
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblAgencyAddress1" runat="server" />
                        <asp:TextBox ID="txtAgencyAddress1" placeholder="Address1" runat="server" />
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblAgencyAddress2" runat="server" />
                        <asp:TextBox ID="txtAgencyAddress2" placeholder="Address2" runat="server" />
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblAgencyCity" runat="server" />
                        <asp:TextBox ID="txtAgencyCity" placeholder="City" runat="server" />
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblAgencyState" runat="server" />
                        <asp:TextBox ID="txtAgencyState" placeholder="State/Province" runat="server" />
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblAgencyZip" runat="server" />
                        <asp:TextBox ID="txtAgencyZip" placeholder="Zip/PostalCode" runat="server" />
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblAgencyCountry" runat="server" />
                        <asp:TextBox ID="txtAgencyCountry" placeholder="Country" runat="server" />
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblAgencyPhone" runat="server" />
                        <asp:TextBox ID="txtAgencyPhone" placeholder="Phone" runat="server" />
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblAgencyFax" runat="server" />
                        <asp:TextBox ID="txtAgencyFax" placeholder="Fax" runat="server" />
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblAgencyClientType" runat="server" />
                        <asp:DropDownList ID="ddlAgencyClientType" runat="server"></asp:DropDownList> <asp:Button id="btnAddClientType2" runat="server" Text="Add/Edit" OnClick="btnAddClientType_Click" />
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblAgencyStatus" runat="server" />
                        <asp:DropDownList ID="ddlAgencyStatus" runat="server">
                            <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                            <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblAgencyCustomerReference" runat="server" />
                        <asp:TextBox ID="txtAgencyCustomerReference" placeholder="Customer Reference" runat="server" />
                    </div>
                    <div class="dnnFormItem">
                        <asp:HiddenField Visible="false" ID="txtSelectedAgency" Value="-1" runat="server"></asp:HiddenField>
                        <asp:HiddenField Visible="false" ID="txtAgencyCreatedBy" Value="-1" runat="server"></asp:HiddenField>
                        <asp:HiddenField Visible="false" ID="txtAgencyCreatedDate" Value="" runat="server"></asp:HiddenField>
                        <asp:Button ID="btnSaveAgency" runat="server" Text="Save Agency" OnClick="btnSaveAgency_Click" />
                        <asp:Button ID="btnSaveAgencyAs" runat="server" Text="Save Agency As" Enabled="false" OnClick="btnSaveAgencyAs_Click" ToolTip="Save a new Agency based on these settings." />
                        <asp:Button ID="btnDeleteAgency" runat="server" CssClass="redButton" Enabled="false" Text="Delete Agency" OnClick="btnDeleteAgency_Click" OnClientClick="return confirm('Are you certain you want to delete this Agency? NOTE: Deleting an Agency may make old orders invalid. It is recommended that you set this Agency to Inactive Status rather than delete it.');" />
                        <asp:Button ID="btnClearAgency" Text="Clear Agency" ToolTip="If you have already clicked on another Agency below, you must click this button first before you try to create a new Agency." runat="server" OnClick="btnClearAgency_Click" />
                    </div><br />
                    <asp:Label ID="lblAgencyMessage" runat="server"></asp:Label>
                    <asp:ValidationSummary ID="valAgencySummary" runat="server" />  
                </fieldset>
                <br /><br />
                <div class="dnnFormItem">
	                <dnn:label ID="lblAgencySearch" runat="server" />
	                <asp:TextBox ID="txtAgencySearch" runat="server" EnableViewState="true" placeholder="keyword"></asp:TextBox>
	                <asp:Button ID="btnAgencySearch" runat="server" Text="Search Agencies" OnClick="btnAgencySearch_Click" />
                </div>
                <asp:GridView ID="gvAgency" OnSelectedIndexChanged="gvAgency_SelectedIndexChanged" OnPageIndexChanging="gvAgency_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true"  CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#9a9a9a" ForeColor="White" Font-Bold="true" Height="30" />
                    <AlternatingRowStyle BackColor="#dddddd" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HiddenField ID="hdngvAgencyId" Value='<%#Eval("Id") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblgvAgencyName" Text='<%#Eval("AgencyName") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="City" DataField="City" SortExpression="City"></asp:BoundField>
                        <asp:BoundField HeaderText="State" DataField="State" SortExpression="State"></asp:BoundField>
                        <asp:BoundField HeaderText="Phone" DataField="Phone" SortExpression="Phone"></asp:BoundField>
                        <asp:TemplateField HeaderText="Active" SortExpression="Active">
                            <ItemTemplate><%# (Boolean.Parse(Eval("Status").ToString())) ? "Active" : "Not Active" %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Cust Ref" DataField="CustomerReference" SortExpression="CustomerReference"></asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="PMT_Advertisers">
        <asp:UpdatePanel ID="pnlAdvertisers" runat="server" UpdateMode="Always" visible="false">
            <ContentTemplate>
                <h3>Advertisers</h3>
                <fieldset>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserAgency" runat="server" />
	                <asp:ListBox ID="ddlAdvertiserAgency" runat="server" Rows="6" SelectionMode="Multiple"></asp:ListBox>
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserName" runat="server" />
	                <asp:TextBox ID="txtAdvertiserName" placeholder="Advertiser Name" runat="server" />
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserCarrier" runat="server" />
	                <asp:DropDownList ID="ddlAdvertiserCarrier" runat="server"></asp:DropDownList> <asp:Button id="btnAddCarrier" runat="server" Text="Add/Edit" OnClick="btnAddCarrier_Click" />
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserFreight" runat="server" />
	                <asp:DropDownList ID="ddlAdvertiserFreight" runat="server"></asp:DropDownList> <asp:Button id="btnAddFreight" runat="server" Text="Add/Edit" OnClick="btnAddFreight_Click" />
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserCarrierNum" runat="server" />
	                <asp:TextBox ID="txtAdvertiserCarrierNum" placeholder="Carrier #" runat="server" />
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserAddress1" runat="server" />
	                <asp:TextBox ID="txtAdvertiserAddress1" placeholder="Address1" runat="server" />
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserAddress2" runat="server" />
	                <asp:TextBox ID="txtAdvertiserAddress2" placeholder="Address2" runat="server" />
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserCity" runat="server" />
	                <asp:TextBox ID="txtAdvertiserCity" placeholder="City" runat="server" />
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserState" runat="server" />
	                <asp:TextBox ID="txtAdvertiserState" placeholder="State/Province" runat="server" />
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserZip" runat="server" />
	                <asp:TextBox ID="txtAdvertiserZip" placeholder="Zip/PostalCode" runat="server" />
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserCountry" runat="server" />
	                <asp:TextBox ID="txtAdvertiserCountry" placeholder="Country" runat="server" />
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserPhone" runat="server" />
	                <asp:TextBox ID="txtAdvertiserPhone" placeholder="Phone" runat="server" />
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserFax" runat="server" />
	                <asp:TextBox ID="txtAdvertiserFax" placeholder="Fax" runat="server" />
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserClientType" runat="server" />
	                <asp:DropDownList ID="ddlAdvertiserClientType" runat="server"></asp:DropDownList> <asp:Button id="btnAddClientType" runat="server" Text="Add/Edit" OnClick="btnAddClientType_Click" />
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserCustomerReference" runat="server" />
	                <asp:TextBox ID="txtAdvertiserCustomerReference" placeholder="Customer Reference" runat="server" />
                    </div>
                    <div class="dnnFormItem">
	                <asp:HiddenField Visible="false" ID="txtSelectedAdvertiser" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtAdvertiserCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtAdvertiserCreatedDate" Value="" runat="server"></asp:HiddenField>
	                <asp:Button ID="btnSaveAdvertiser" runat="server" Text="Save Advertiser" OnClick="btnSaveAdvertiser_Click" />
	                <asp:Button ID="btnSaveAdvertiserAs" runat="server" Text="Save Advertiser As" Enabled="false" OnClick="btnSaveAdvertiserAs_Click" ToolTip="Save a new Advertiser based on these settings." />
	                <asp:Button ID="btnDeleteAdvertiser" runat="server" CssClass="redButton" Enabled="false" Text="Delete Advertiser" OnClick="btnDeleteAdvertiser_Click" OnClientClick="return confirm('Are you certain you want to delete this Advertiser?');" />
	                <asp:Button ID="btnClearAdvertiser" Text="Clear Advertiser" ToolTip="If you have already clicked on another Advertiser below, you must click this button first before you try to create a new Advertiser." runat="server" OnClick="btnClearAdvertiser_Click" />
                    </div><br />
                    <asp:Label ID="lblAdvertiserMessage" runat="server"></asp:Label>
                    <asp:ValidationSummary ID="valAdvertiserSummary" runat="server" />  
                </fieldset>
                <br /><br />
                <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserAgencySearch" runat="server" />
	                <asp:DropDownList ID="ddlAdvertiserAgencySearch" runat="server"></asp:DropDownList>
                    </div>
                <div class="dnnFormItem">
	                <dnn:label ID="lblAdvertiserSearch" runat="server" />
	                <asp:TextBox ID="txtAdvertiserSearch" runat="server" EnableViewState="true" placeholder="keyword"></asp:TextBox>
	                <asp:Button ID="btnAdvertiserSearch" runat="server" Text="Search Advertisers" OnClick="btnAdvertiserSearch_Click" />
                </div>
                <asp:GridView ID="gvAdvertiser" OnSelectedIndexChanged="gvAdvertiser_SelectedIndexChanged" OnPageIndexChanging="gvAdvertiser_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true"  CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#9a9a9a" ForeColor="White" Font-Bold="true" Height="30" />
                    <AlternatingRowStyle BackColor="#dddddd" />
                    <Columns>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:HiddenField ID="hdngvAdvertiserId" Value='<%#Eval("Id") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="Name">
	                    <ItemTemplate>
		                <asp:Label ID="lblgvAdvertiserName" Text='<%#Eval("AdvertiserName") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:BoundField HeaderText="City" DataField="City" SortExpression="City"></asp:BoundField>
	                <asp:BoundField HeaderText="State" DataField="State" SortExpression="State"></asp:BoundField>
	                <asp:BoundField HeaderText="Phone" DataField="Phone" SortExpression="Phone"></asp:BoundField>
	                <asp:BoundField HeaderText="Cust Ref" DataField="CustomerReference" SortExpression="CustomerReference"></asp:BoundField>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
	                    </ItemTemplate>
	                </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="PMT_Markets">
        <asp:UpdatePanel ID="pnlMarkets" runat="server" UpdateMode="Always" visible="false">
            <ContentTemplate>
                <h3>Markets</h3>
                <fieldset>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblMarketName" runat="server" />
	                <asp:TextBox ID="txtMarketName" placeholder="Market Name" runat="server" />
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblMarketDescription" runat="server" />
	                <asp:TextBox ID="txtMarketDescription" TextMode="MultiLine" Rows="6" placeholder="Description" runat="server" />
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblMarketParent" runat="server" />
	                <asp:DropDownList ID="ddlMarketParent" runat="server"></asp:DropDownList>
                    </div>
                    <div class="dnnFormItem">
	                <asp:HiddenField Visible="false" ID="txtSelectedMarket" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtMarketCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtMarketCreatedDate" Value="" runat="server"></asp:HiddenField>
	                <asp:Button ID="btnSaveMarket" runat="server" Text="Save Market" OnClick="btnSaveMarket_Click" />
	                <asp:Button ID="btnSaveMarketAs" runat="server" Text="Save Market As" Enabled="false" OnClick="btnSaveMarketAs_Click" ToolTip="Save a new Market based on these settings." />
	                <asp:Button ID="btnDeleteMarket" runat="server" CssClass="redButton" Enabled="false" Text="Delete Market" OnClick="btnDeleteMarket_Click" OnClientClick="return confirm('Are you certain you want to delete this Market?');" />
	                <asp:Button ID="btnClearMarket" Text="Clear Market" ToolTip="If you have already clicked on another Market below, you must click this button first before you try to create a new Market." runat="server" OnClick="btnClearMarket_Click" />
                    </div><br />
                    <asp:Label ID="lblMarketMessage" runat="server"></asp:Label>
                    <div class="dnnFormItem">
                        <dnn:label id="lblMarketSearch" runat="server"></dnn:label>
                    <asp:TextBox ID="txtMarketSearch" placeholder="Search" runat="server"></asp:TextBox> <asp:Button ID="btnMarketSearch" runat="server" OnClick="btnMarketSearch_Click" Text="Search" />
                        </div>
                </fieldset>
                <br /><br />
                <asp:GridView ID="gvMarket" OnSelectedIndexChanged="gvMarket_SelectedIndexChanged" OnPageIndexChanging="gvMarket_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true"  CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#9a9a9a" ForeColor="White" Font-Bold="true" Height="30" />
                    <AlternatingRowStyle BackColor="#dddddd" />
                    <Columns>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:HiddenField ID="hdngvMarketId" Value='<%#Eval("Id") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="Name">
	                    <ItemTemplate>
		                <asp:Label ID="lblgvMarketName" Text='<%#Eval("MarketName") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
	                    </ItemTemplate>
	                </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="PMT_Stations">
        <asp:UpdatePanel ID="pnlStations" runat="server" UpdateMode="Always" visible="false">
            <ContentTemplate>
                <h3>Stations</h3>
                <fieldset>                    
                <div class="dnnFormItem">
	            <dnn:label ID="lblStationMarket" runat="server" />
	            <asp:DropDownList ID="ddlStationMarket" runat="server"></asp:DropDownList> <asp:Button id="btnAddStationMarket" runat="server" Text="Add/Edit" OnClick="btnAddStationMarket_Click" />
                </div>
                <div class="dnnFormItem">
	            <dnn:label ID="lblStationName" runat="server" />
	            <asp:TextBox ID="txtStationName" placeholder="Station Name" runat="server" />
                </div>
                <div class="dnnFormItem">
	            <dnn:label ID="lblStationContact" runat="server" />
	            <asp:TextBox ID="txtStationContact" placeholder="Station Contact" runat="server" />
                </div>             
                <div class="dnnFormItem">
	            <dnn:label ID="lblStationCallLetters" runat="server" />
	            <asp:TextBox ID="txtStationCallLetters" placeholder="Call Letters" runat="server" />
                </div>
                <div class="dnnFormItem">
	            <dnn:label ID="lblStationMediaType" runat="server" />
	            <asp:DropDownList ID="ddlStationMediaType" runat="server">
                    <asp:ListItem Value="-1" Text="--Please Select--"></asp:ListItem>
                    <asp:ListItem Value="1" Text="ADDELIVERY YES"></asp:ListItem>
                    <asp:ListItem Value="2" Text="HD"></asp:ListItem>
                    <asp:ListItem Value="3" Text="HD & SD"></asp:ListItem>
                    <asp:ListItem Value="4" Text="HD & SD (BACKUP REQUIRED)"></asp:ListItem>
                    <asp:ListItem Value="5" Text="SD"></asp:ListItem>
	            </asp:DropDownList>
                </div>       
                <div class="dnnFormItem">
	            <dnn:label ID="lblStationTapeFormat" runat="server" />
	            <asp:ListBox ID="lbxStationTapeFormat" runat="server" Rows="6" SelectionMode="Multiple"></asp:ListBox> <asp:Button id="btnAddStationTapeFormat" runat="server" Text="Add/Edit" OnClick="btnAddStationTapeFormat_Click" />
                </div> 
                    <div class="dnnFormItem">
                <dnn:label ID="lblStationAddress1" runat="server" />
                <asp:TextBox ID="txtStationAddress1" placeholder="Address1" runat="server" />
                </div>
                <div class="dnnFormItem">
                <dnn:label ID="lblStationAddress2" runat="server" />
                <asp:TextBox ID="txtStationAddress2" placeholder="Address2" runat="server" />
                </div>
                <div class="dnnFormItem">
                <dnn:label ID="lblStationCity" runat="server" />
                <asp:TextBox ID="txtStationCity" placeholder="City" runat="server" />
                </div>
                <div class="dnnFormItem">
                <dnn:label ID="lblStationState" runat="server" />
                <asp:TextBox ID="txtStationState" placeholder="State/Province" runat="server" />
                </div>
                <div class="dnnFormItem">
                <dnn:label ID="lblStationZip" runat="server" />
                <asp:TextBox ID="txtStationZip" placeholder="Zip/PostalCode" runat="server" />
                </div>
                <div class="dnnFormItem">
                <dnn:label ID="lblStationCountry" runat="server" />
                <asp:TextBox ID="txtStationCountry" placeholder="Country" runat="server" />
                </div>
                <div class="dnnFormItem">
                <dnn:label ID="lblStationPhone" runat="server" />
                <asp:TextBox ID="txtStationPhone" placeholder="Phone" runat="server" />
                </div>
                <div class="dnnFormItem">
                <dnn:label ID="lblStationFax" runat="server" />
                <asp:TextBox ID="txtStationFax" placeholder="Fax" runat="server" />
                </div>
                <div class="dnnFormItem">
                <dnn:label ID="lblStationEmail" runat="server" />
                <asp:TextBox ID="txtStationEmail" placeholder="Email" runat="server" />
                </div>
                <div class="dnnFormItem">
                <dnn:label ID="lblStationSpecialInstruction" runat="server" />
                <asp:TextBox ID="txtStationSpecialInstruction" TextMode="MultiLine" Rows="6" placeholder="Special Instructions" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:label ID="lblStationDeliveryMethod" runat="server" />
                    <asp:ListBox ID="lbxStationDeliveryMethod" runat="server" Rows="6" SelectionMode="Multiple"></asp:ListBox> <asp:Button id="btnAddStationDeliveryMethod" runat="server" Text="Add/Edit" OnClick="btnAddStationDeliveryMethod_Click" />
                </div>
                <div class="dnnFormItem">
	            <dnn:label ID="lblStationOnline" runat="server" />
	            <asp:DropDownList ID="ddlStationOnline" runat="server">
                    <asp:ListItem Value="-1" Text="--Please Select--"></asp:ListItem>
                    <asp:ListItem Value="1" Text="YES"></asp:ListItem>
                    <asp:ListItem Value="0" Text="NO"></asp:ListItem>
	            </asp:DropDownList>
                </div>
                <div class="dnnFormItem">
                <dnn:label ID="lblStationStatus" runat="server" />
                <asp:DropDownList ID="ddlStationStatus" runat="server">
                    <asp:ListItem Text="--Please Select--" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                    <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                </asp:DropDownList>
                </div>
                <div class="dnnFormItem">
                <dnn:label ID="lblStationAttentionLine" runat="server" />
                <asp:TextBox ID="txtStationAttentionLine" placeholder="Attention Line" runat="server" />
                </div>
                <div class="dnnFormItem">
	            <asp:HiddenField Visible="false" ID="txtSelectedStation" Value="-1" runat="server"></asp:HiddenField>
	            <asp:HiddenField Visible="false" ID="txtStationCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	            <asp:HiddenField Visible="false" ID="txtStationCreatedDate" Value="" runat="server"></asp:HiddenField>
	            <asp:Button ID="btnSaveStation" runat="server" Text="Save Station" OnClick="btnSaveStation_Click" />
	            <asp:Button ID="btnSaveStationAs" runat="server" Text="Save Station As" Enabled="false" OnClick="btnSaveStationAs_Click" ToolTip="Save a new Station based on these settings." />
	            <asp:Button ID="btnDeleteStation" runat="server" CssClass="redButton" Enabled="false" Text="Delete Station" OnClick="btnDeleteStation_Click" OnClientClick="return confirm('Are you certain you want to delete this Station? NOTE: Deleting a station may make old orders invalid. It is recommended that you set this Station to Inactive Status rather than delete it.');" />
	            <asp:Button ID="btnClearStation" Text="Clear Station" ToolTip="If you have already clicked on another Station below, you must click this button first before you try to create a new Station." runat="server" OnClick="btnClearStation_Click" />
                </div><br />
                <asp:Label ID="lblStationMessage" runat="server"></asp:Label>
            </fieldset>
            <br /><br />
                <div class="dnnFormItem">
	                <dnn:label ID="lblStationMarketSearch" runat="server" />
	                <asp:DropDownList ID="ddlStationMarketSearch" runat="server"></asp:DropDownList>
                    </div>
                <div class="dnnFormItem">
	                <dnn:label ID="lblStationSearch" runat="server" />
	                <asp:TextBox ID="txtStationSearch" runat="server" EnableViewState="true" placeholder="keyword"></asp:TextBox>
	                <asp:Button ID="btnStationSearch" runat="server" Text="Search Stations" OnClick="btnStationSearch_Click" />
                </div>
            <asp:GridView ID="gvStation" OnSelectedIndexChanged="gvStation_SelectedIndexChanged" OnPageIndexChanging="gvStation_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true"  CssClass="tblItems" runat="server">
                <HeaderStyle BackColor="#9a9a9a" ForeColor="White" Font-Bold="true" Height="30" />
                <AlternatingRowStyle BackColor="#dddddd" />
                <Columns>
	            <asp:TemplateField>
	                <ItemTemplate>
		            <asp:HiddenField ID="hdngvStationId" Value='<%#Eval("Id") %>' runat="server" />
	                </ItemTemplate>
	            </asp:TemplateField>
	            <asp:TemplateField HeaderText="Name">
	                <ItemTemplate>
		            <asp:Label ID="lblgvStationName" Text='<%#Eval("StationName") %>' runat="server" />
	                </ItemTemplate>
	            </asp:TemplateField>
                    <asp:TemplateField HeaderText="Call Letters">
	                <ItemTemplate>
		            <asp:Label ID="lblgvStationCallLetters" Text='<%#Eval("CallLetter") %>' runat="server" />
	                </ItemTemplate>
	            </asp:TemplateField>
	            <asp:TemplateField>
	                <ItemTemplate>
		            <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
	                </ItemTemplate>
	            </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </ContentTemplate></asp:UpdatePanel>

    <div id="PMT_StationGroups">
        <asp:UpdatePanel ID="pnlStationGroups" runat="server" UpdateMode="Always" visible="false">
            <ContentTemplate>
                <h3>StationGroups</h3>
                <fieldset>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblStationGroupName" runat="server" />
	                <asp:TextBox ID="txtStationGroupName" placeholder="Station Group Name" runat="server" />
                    </div>
                    <div class="dnnFormItem">
                    <dnn:label ID="lblStationGroupDescription" runat="server" />
                    <asp:TextBox ID="txtStationGroupDescription" TextMode="MultiLine" Rows="6" placeholder="Description" runat="server" />
                    </div>
                    <div class="dnnFormItem" style="display:none;">
	                <dnn:label ID="lblStationGroupAgency" runat="server" />
	                <asp:DropDownList ID="ddlStationGroupAgency" runat="server"></asp:DropDownList>
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblStationGroupStations" runat="server" />
	                <asp:ListBox ID="lbxStationGroupStations" runat="server" Rows="6"></asp:ListBox> <asp:Button id="btnManageStationsInGroup" runat="server" Text="Manage Stations in Group" OnClick="btnManageStationsInGroup_Click" Enabled="false" />
                    </div>
                    <div class="dnnFormItem">
	                <asp:HiddenField Visible="false" ID="txtSelectedStationGroup" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtStationGroupCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtStationGroupCreatedDate" Value="" runat="server"></asp:HiddenField>
	                <asp:Button ID="btnSaveStationGroup" runat="server" Text="Save StationGroup" OnClick="btnSaveStationGroup_Click" />
	                <asp:Button ID="btnSaveStationGroupAs" runat="server" Text="Save StationGroup As" Enabled="false" OnClick="btnSaveStationGroupAs_Click" ToolTip="Save a new StationGroup based on these settings." />
	                <asp:Button ID="btnDeleteStationGroup" runat="server" CssClass="redButton" Enabled="false" Text="Delete Station Group" OnClick="btnDeleteStationGroup_Click" OnClientClick="return confirm('Are you certain you want to delete this Station Group?');" />
	                <asp:Button ID="btnClearStationGroup" Text="Clear Station Group" ToolTip="If you have already clicked on another Station Group below, you must click this button first before you try to create a new Station Group." runat="server" OnClick="btnClearStationGroup_Click" />
                    </div><br />
                    <asp:Label ID="lblStationGroupMessage" runat="server"></asp:Label>
                </fieldset>
                <br /><br />
                
                <asp:GridView ID="gvStationGroup" OnSelectedIndexChanged="gvStationGroup_SelectedIndexChanged" OnPageIndexChanging="gvStationGroup_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true"  CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#9a9a9a" ForeColor="White" Font-Bold="true" Height="30" />
                    <AlternatingRowStyle BackColor="#dddddd" />
                    <Columns>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:HiddenField ID="hdngvStationGroupId" Value='<%#Eval("Id") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="Name">
	                    <ItemTemplate>
		                <asp:Label ID="lblgvStationGroupName" Text='<%#Eval("StationGroupName") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
	                    </ItemTemplate>
	                </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="PMT_Labels">
        <asp:UpdatePanel ID="pnlLabels" runat="server" UpdateMode="Always" visible="false">
            <ContentTemplate>
                <h3>Labels</h3>
                <fieldset>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblLabelTapeFormat" runat="server" />
	                <asp:DropDownList ID="ddlLabelTapeFormat" runat="server"></asp:DropDownList>
                    </div>
                    <div class="dnnFormItem">
                    <dnn:label ID="lblLabelAgency" runat="server" />
                    <asp:DropDownList ID="ddlLabelAgency" runat="server"></asp:DropDownList>
                    </div>
                    <div class="dnnFormItem">
                    <dnn:label ID="lblLabelAdvertiser" runat="server" />
                    <asp:DropDownList ID="ddlLabelAdvertiser" runat="server"></asp:DropDownList>
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblLabelTitle" runat="server" />
	                <asp:TextBox ID="txtLabelTitle"  runat="server"></asp:TextBox>
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblLabelDescription" runat="server" />
	                <asp:TextBox ID="txtLabelDescription"  runat="server"></asp:TextBox>
                    </div>   
                    <div class="dnnFormItem">             
	                <dnn:label ID="lblLabelISCI" runat="server" />
	                <asp:TextBox ID="txtLabelISCI"  runat="server"></asp:TextBox>
                    </div>    
                    <div class="dnnFormItem">    
	                <dnn:label ID="lblLabelPMTMediaId" runat="server" />
	                <asp:TextBox ID="txtLabelPMTMediaId"  runat="server"></asp:TextBox>
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblLabelLength" runat="server" />
	                <asp:TextBox ID="txtLabelLength"  runat="server"></asp:TextBox>
                    </div>
                    <div class="dnnFormItem">
                    <dnn:label ID="lblLabelStandard" runat="server" />
                    <asp:DropDownList ID="ddlLabelStandard" runat="server">
                        <asp:ListItem Value="NTSC" Text ="NTSC"></asp:ListItem>
                        <asp:ListItem Value="PAL" Text="PAL"></asp:ListItem>
                    </asp:DropDownList>
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblLabelNotes" runat="server" />
	                <asp:TextBox ID="txtLabelNotes"  runat="server"></asp:TextBox>
                    </div>
                    <div class="dnnFormItem">
	                <asp:HiddenField Visible="false" ID="txtSelectedLabel" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtLabelCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtLabelCreatedDate" Value="" runat="server"></asp:HiddenField>
	                <asp:Button ID="btnSaveLabel" runat="server" Text="Save Label" OnClick="btnSaveLabel_Click" />
	                <asp:Button ID="btnSaveLabelAs" runat="server" Text="Save Label As" Enabled="false" OnClick="btnSaveLabelAs_Click" ToolTip="Save a new Label based on these settings." />
	                <asp:Button ID="btnDeleteLabel" runat="server" CssClass="redButton" Enabled="false" Text="Delete Label" OnClick="btnDeleteLabel_Click" OnClientClick="return confirm('Are you certain you want to delete this Label?');" />
	                <asp:Button ID="btnClearLabel" Text="Clear Label" ToolTip="If you have already clicked on another Label below, you must click this button first before you try to create a new Label." runat="server" OnClick="btnClearLabel_Click" />
                    </div><br />
                    <asp:Label ID="lblLabelMessage" runat="server"></asp:Label>
                </fieldset>
                <br /><br />
                <div class="dnnFormItem">
	                <dnn:label ID="lblLabelAgencySearch" runat="server" />
	                <asp:DropDownList ID="ddlLabelAgencySearch" runat="server"></asp:DropDownList>
                    </div>
                <div class="dnnFormItem">
	                <dnn:label ID="lblLabelAdvertiserSearch" runat="server" />
	                <asp:DropDownList ID="ddlLabelAdvertiserSearch" runat="server"></asp:DropDownList>
                    </div>
                <div class="dnnFormItem">
	                <dnn:label ID="lblLabelSearch" runat="server" />
	                <asp:TextBox ID="txtLabelSearch" runat="server" EnableViewState="true" placeholder="keyword"></asp:TextBox>
	                <asp:Button ID="btnLabelSearch" runat="server" Text="Search Labels" OnClick="btnLabelSearch_Click" />
                </div>
                
                <asp:GridView ID="gvLabel" OnSelectedIndexChanged="gvLabel_SelectedIndexChanged" OnPageIndexChanging="gvLabel_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true"  CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#9a9a9a" ForeColor="White" Font-Bold="true" Height="30" />
                    <AlternatingRowStyle BackColor="#dddddd" />
                    <Columns>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:HiddenField ID="hdngvLabelId" Value='<%#Eval("Id") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="Advertiser">
	                    <ItemTemplate>
		                <asp:Label ID="lblgvAdvertiserName" Text='<%#Eval("AdvertiserName") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="ISCI">
	                    <ItemTemplate>
		                <asp:Label ID="lblgvISCICode" Text='<%#Eval("ISCICode") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="Title">
	                    <ItemTemplate>
		                <asp:Label ID="lblgvTitle" Text='<%#Eval("Title") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
	                    </ItemTemplate>
	                </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br /><br />
                <asp:Panel ID="pnlLabelView" runat="server" Visible ="false">
                    <div style="width:900px;margin-left:5px;">
                                        <table border="1" width="100%" style="text-align: center;">
                                            <tr>
                                                <th class="bckclr6">Label Number</th>
                                                <th class="bckclr6">Tape Format</th>
                                                <th class="bckclr6" style="width: 30%">Destination ID</th>
                                                <th class="bckclr6">Campaign ID</th>
                                                <th class="bckclr6">Campaign Status</th>
                                            </tr>
                                            <tr class="bckclr3">
                                                <td><asp:Literal ID="litLabelNumber" runat="server"></asp:Literal></td>
                                                <td><asp:Literal ID="litLabelTapeFormat" runat="server"></asp:Literal></td>
                                                <td><asp:Literal ID="litLabelDestinationId" runat="server"></asp:Literal></td>
                                                <td><asp:Literal ID="litLabelCampaignId" runat="server"></asp:Literal></td>
                                                <td><asp:Literal ID="litLabelCampaignStatus" runat="server"></asp:Literal></td>
                                            </tr>
                                            <tr>
                                                <th class="bckclr6" colspan="2">Advertiser</th>
                                                <th class="bckclr6" style="width: 30%">Agency</th>
                                                <th class="bckclr6" colspan="2">Title</th>
                                            </tr>
                                            <tr class="bckclr3">
                                                <td colspan="2"><asp:Literal ID="litLabelAdvertiser" runat="server"></asp:Literal></td>
                                                <td><asp:Literal ID="litLabelAgency" runat="server"></asp:Literal></td>
                                                <td colspan="2"><asp:Literal ID="litLabelTitle" runat="server"></asp:Literal></td>
                                            </tr>
                                            <tr>
                                                <th class="bckclr6" colspan="2">
                                                    Product/Description
                                                </th>
                                                <th class="bckclr6" style="width: 30%">ISCI</th>
                                                <th class="bckclr6" colspan="2">PMT Media ID</th>
                                            </tr>
                                            <tr class="bckclr3">
                                                <td colspan="2"><asp:Literal ID="litLabelDescription" runat="server"></asp:Literal></td>
                                                <td><asp:Literal ID="litLabelISCI" runat="server"></asp:Literal></td>
                                                <td colspan="2"><asp:Literal ID="litLabelPMTMediaId" runat="server"></asp:Literal></td>
                                            </tr>
                                            <tr>
                                                <th class="bckclr6" colspan="2">Length</th>
                                                <th class="bckclr6" style="width: 30%">
                                                    Standard
                                                </th>
                                                <th class="bckclr6" colspan="2">Campaign Created</th>
                                            </tr>
                                            <tr class="bckclr3">
                                                <td colspan="2"><asp:Literal ID="litLabelMediaLength" runat="server"></asp:Literal></td>
                                                <td><asp:Literal ID="litLabelStandard" runat="server"></asp:Literal></td>
                                                <td colspan="2"><asp:Literal ID="litLabelCampaignCreated" runat="server"></asp:Literal></td>
                                            </tr>
                                        </table>
                                    </div>
                    <br /><br />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    
    <div id="PMT_MasterItems">
            <asp:UpdatePanel ID="pnlMasterItems" runat="server" UpdateMode="Always" visible="false">
                <ContentTemplate>
                    <h3>Master Items</h3>
                    <fieldset>
                        <div class="dnnFormItem">
    	                <dnn:Label ID="lblMasterItemCustomerId" runat="server" />
    	                <asp:TextBox ID="txtMasterItemCustomerId" runat="server" placeholder="Customer Id"></asp:TextBox>
                        </div>
                        <div class="dnnFormItem">
                        <dnn:Label ID="lblMasterItemPMTMediaId" runat="server" />
                        <asp:TextBox ID="txtMasterItemPMTMediaId" runat="server" placeholder="PMT Media Id"></asp:TextBox>
                        </div>
                        <div class="dnnFormItem">
                        <dnn:Label ID="lblMasterItemTitle" runat="server" />
                        <asp:TextBox ID="txtMasterItemTitle" runat="server" placeholder="Title"></asp:TextBox>
                        </div>
                        <div class="dnnFormItem">
    	                <dnn:Label ID="lblMasterItemFile" runat="server" />
    	                <asp:TextBox ID="txtMasterItemFile"  runat="server" placeholder="Filename"></asp:TextBox>
                        </div>
                        <div class="dnnFormItem">
                        <dnn:label ID="lblMasterItemAdvertiser" runat="server" />
                        <asp:DropDownList ID="ddlMasterItemAdvertiser" runat="server" OnSelectedIndexChanged="ddlMasterItemAdvertiser_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="dnnFormItem">
    	                <dnn:Label ID="lblMasterItemDAgencies" runat="server" />
    	                <asp:ListBox ID="lbxMasterItemAgencies" SelectionMode="Multiple" Rows="6"  runat="server"></asp:ListBox>
                        </div>
                        <div class="dnnFormItem">             
    	                <dnn:Label ID="lblMasterItemClosedCaption" runat="server" />
    	                <asp:CheckBox ID="chkMasterItemClosedCaption" runat="server"></asp:CheckBox>
                        </div>
                        <div class="dnnFormItem"><br />
	                    <dnn:Label ID="lblMasterItemMediaType" runat="server" />
	                        <asp:DropDownList ID="ddlMasterItemMediaType" runat="server">
                                <asp:ListItem Value="-1" Text="--Please Select--"></asp:ListItem>
                                <asp:ListItem Value="1" Text="ADDELIVERY YES"></asp:ListItem>
                                <asp:ListItem Value="2" Text="HD"></asp:ListItem>
                                <asp:ListItem Value="3" Text="HD & SD"></asp:ListItem>
                                <asp:ListItem Value="4" Text="HD & SD (BACKUP REQUIRED)"></asp:ListItem>
                                <asp:ListItem Value="5" Text="SD"></asp:ListItem>
	                        </asp:DropDownList>
                        </div>    
                        <div class="dnnFormItem">
                        <dnn:Label ID="lblMasterItemEncode" runat="server" />
                        <asp:DropDownList ID="ddlMasterItemEncode" runat="server">
                            <asp:ListItem Value="-1" Text="--Please Select--"></asp:ListItem>
                            <asp:ListItem Value="1" Text ="SPOTTRAC"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Teletrax"></asp:ListItem>
                            <asp:ListItem Value="3" Text="VEIL"></asp:ListItem>
                        </asp:DropDownList>
                        </div> 
                        <div class="dnnFormItem">
                        <dnn:Label ID="lblMasterItemStandard" runat="server" />
                        <asp:DropDownList ID="ddlMasterItemStandard" runat="server">
                            <asp:ListItem Value="-1" Text="--Please Select--"></asp:ListItem>
                            <asp:ListItem Value="NTSC" Text ="NTSC"></asp:ListItem>
                            <asp:ListItem Value="PAL" Text="PAL"></asp:ListItem>
                        </asp:DropDownList>
                        </div>
                        <div class="dnnFormItem">
    	                <dnn:Label ID="lblMasterItemLength" runat="server" />
    	                <asp:TextBox ID="txtMasterItemLength" placeholder="00:00" runat="server"></asp:TextBox>
                        </div>
                        <div class="dnnFormItem">
    	                <dnn:Label ID="lblMasterItemReel" runat="server" />
    	                <asp:TextBox ID="txtMasterItemReel" placeholder="Reel #" runat="server"></asp:TextBox>
                        </div>
                        <div class="dnnFormItem">
    	                <dnn:Label ID="lblMasterItemTapeCode" runat="server" />
    	                <asp:TextBox ID="txtMasterItemTapeCode" placeholder="Tape Code" runat="server"></asp:TextBox>
                        </div>                        
                        <div class="dnnFormItem">
    	                <dnn:Label ID="lblMasterItemPosition" runat="server" />
    	                <asp:TextBox ID="txtMasterItemPostition" placeholder="Position #" runat="server"></asp:TextBox>
                        </div>                     
                        <div class="dnnFormItem">
    	                <dnn:Label ID="lblMasterItemVaultId" runat="server" />
    	                <asp:TextBox ID="txtMasterItemVaultId" placeholder="Vault Id" runat="server"></asp:TextBox>
                        </div>                     
                        <div class="dnnFormItem">
    	                <dnn:Label ID="lblMasterItemLocation" runat="server" />
    	                <asp:TextBox ID="txtMasterItemLocation" placeholder="Location" runat="server"></asp:TextBox>
                        </div>                     
                        <div class="dnnFormItem">
    	                <dnn:Label ID="lblMasterItemComment" runat="server" />
    	                <asp:TextBox ID="txtMasterItemComment" placeholder="Comment" TextMode="MultiLine" Rows="6" runat="server"></asp:TextBox>
                        </div>
                        <div class="dnnFormItem">
                            <asp:Button ID="btnMasterChecklist" runat="server" Enabled ="false" Text ="Checklist" /><br />
    	                <asp:HiddenField Visible="false" ID="txtSelectedMasterItem" Value="-1" runat="server"></asp:HiddenField>
    	                <asp:HiddenField Visible="false" ID="txtMasterItemCreatedBy" Value="-1" runat="server"></asp:HiddenField>
    	                <asp:HiddenField Visible="false" ID="txtMasterItemCreatedDate" Value="" runat="server"></asp:HiddenField>
    	                <asp:Button ID="btnSaveMasterItem" runat="server" Text="Save MasterItem" OnClick="btnSaveMasterItem_Click" />
    	                <asp:Button ID="btnSaveMasterItemAs" runat="server" Text="Save MasterItem As" Enabled="false" OnClick="btnSaveMasterItemAs_Click" ToolTip="Save a new MasterItem based on these settings." />
    	                <asp:Button ID="btnDeleteMasterItem" runat="server" CssClass="redButton" Enabled="false" Text="Delete MasterItem" OnClick="btnDeleteMasterItem_Click" OnClientClick="return confirm('Are you certain you want to delete this MasterItem?');" />
    	                <asp:Button ID="btnClearMasterItem" Text="Clear MasterItem" ToolTip="If you have already clicked on another MasterItem below, you must click this button first before you try to create a new MasterItem." runat="server" OnClick="btnClearMasterItem_Click" />
                        </div><br />
                        
                        <asp:Label ID="lblMasterItemMessage" runat="server"></asp:Label>
                    </fieldset>
                    <br /><br />
                    <div class="dnnFormItem">
    	                <dnn:Label ID="lblMasterItemAdvertiserSearch" runat="server" />
    	                <asp:DropDownList ID="ddlMasterItemAdvertiserSearch" OnSelectedIndexChanged="ddlMasterItemAgencySearch_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                        </div>
                    <div class="dnnFormItem">
    	                <dnn:Label ID="lblMasterItemAgencySearch" runat="server" />
    	                <asp:DropDownList ID="ddlMasterItemAgencySearch" runat="server"></asp:DropDownList>
                        </div>
                    <div class="dnnFormItem">
    	                <dnn:Label ID="lblMasterItemSearch" runat="server" />
    	                <asp:TextBox ID="txtMasterItemSearch" runat="server" EnableViewState="true" placeholder="keyword"></asp:TextBox>
    	                <asp:Button ID="btnMasterItemSearch" runat="server" Text="Search MasterItems" OnClick="btnMasterItemSearch_Click" />
                    </div>
                    
                    <asp:GridView ID="gvMasterItem" OnSelectedIndexChanged="gvMasterItem_SelectedIndexChanged" OnPageIndexChanging="gvMasterItem_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true"  CssClass="tblItems" runat="server">
                        <HeaderStyle BackColor="#9a9a9a" ForeColor="White" Font-Bold="true" Height="30" />
                        <AlternatingRowStyle BackColor="#dddddd" />
                        <Columns>
    	                <asp:TemplateField>
    	                    <ItemTemplate>
    		                <asp:HiddenField ID="hdngvMasterItemId" Value='<%#Eval("Id") %>' runat="server" />
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="PMTMediaId">
    	                    <ItemTemplate>
    		                <asp:Label ID="lblgvPMTMediaID" Text='<%#Eval("PMTMediaId") %>' runat="server" />
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Title">
    	                    <ItemTemplate>
    		                <asp:Label ID="lblgvTitle" Text='<%#Eval("Title") %>' runat="server" />
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Length">
    	                    <ItemTemplate>
    		                <asp:Label ID="lblgvLength" Text='<%#Eval("Length") %>' runat="server" />
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField>
    	                    <ItemTemplate>
    		                <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
    	                    </ItemTemplate>
    	                </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    <div id="PMT_Users">
            <asp:UpdatePanel ID="pnlUsers" runat="server" UpdateMode="Always" visible="false">
                <ContentTemplate>
                    <h3>Users</h3>
                    <fieldset>
                        <div class="dnnFormItem">
    	                    <dnn:Label ID="lblUsers" runat="server" />
    	                    <asp:DropDownList ID="ddlUsers" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUsers_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="dnnFormItem">
    	                    <dnn:Label ID="lblUserFirstName" runat="server" />
    	                    <asp:Label ID="lblUserFirstNameValue" runat="server"></asp:Label>
                        </div><br clear="both" />
                        <div class="dnnFormItem">
    	                    <dnn:Label ID="lblUserLastName" runat="server" />
    	                    <asp:Label ID="lblUserLastNameValue" runat="server"></asp:Label>
                        </div><br clear="both" />
                        <div class="dnnFormItem">
    	                    <dnn:Label ID="lblUserDisplayName" runat="server" />
    	                    <asp:Label ID="lblUserDisplayNameValue" runat="server"></asp:Label>
                        </div><br clear="both" />
                        <div class="dnnFormItem">
    	                    <dnn:Label ID="lblUserUsername" runat="server" />
    	                    <asp:Label ID="lblUserUsernameValue" runat="server"></asp:Label>
                        </div><br clear="both" />
                        <div class="dnnFormItem">
    	                    <dnn:Label ID="lblUserEmail" runat="server" />
    	                    <asp:Label ID="lblUserEmailValue" runat="server"></asp:Label>
                        </div><br clear="both" />
                        <div class="dnnFormItem">
    	                    <dnn:Label ID="lblUserRoles" runat="server" />
    	                    <asp:ListBox ID="lbxUserRoles" SelectionMode="Multiple" Rows="6"  runat="server"></asp:ListBox>
                        </div>
                        <div class="dnnFormItem">
    	                    <dnn:Label ID="lblUserAgencies" runat="server" />
    	                    <asp:ListBox ID="lbxUserAgencies" SelectionMode="Multiple" Rows="6"  runat="server"></asp:ListBox>
                        </div>
                        <div class="dnnFormItem">
    	                    <dnn:Label ID="lblUserAdvertisers" runat="server" />
    	                    <asp:ListBox ID="lbxUserAdvertisers" SelectionMode="Multiple" Rows="6"  runat="server"></asp:ListBox>
                        </div>
                        <div class="dnnFormItem">
                            <asp:Button ID="btnSaveUserPermissions" runat="server" Text="Save User Permissions" OnClick="btnSaveUserPermissions_Click" /><br />
                            <asp:Label ID="lblUserMessage" runat="server"></asp:Label>
                        </div>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
    </div>

    <ajaxToolkit:ModalPopupExtender runat="server" 
	ID="mpeCarrierTypePopup" 
	TargetControlID="dummy3" 
	PopupControlID="pnlCarrierTypeHolder" 
	BackgroundCssClass="modalBackground"                        
	DropShadow="true"/> 
<input id="dummy3" type="button" style="display: none" runat="server" />
<asp:Panel ID="pnlCarrierTypeHolder" CssClass="modalPopup" runat="server">
<asp:UpdatePanel ID="pnlCarrierTypeModal" runat="server" UpdateMode="Conditional"><ContentTemplate>
<h3>Carrier Types</h3>
<fieldset>
    <div class="dnnFormItem">
	<dnn:label ID="lblCarrierType" runat="server" />
	<asp:TextBox ID="txtCarrierType" placeholder="Carrier Name" runat="server" />
    </div>
    <div class="dnnFormItem">
	<asp:HiddenField Visible="false" ID="txtSelectedCarrierType" Value="-1" runat="server"></asp:HiddenField>
	<asp:HiddenField Visible="false" ID="txtCarrierTypeCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	<asp:HiddenField Visible="false" ID="txtCarrierTypeCreatedDate" Value="" runat="server"></asp:HiddenField>
	<asp:Button ID="btnSaveCarrierType" runat="server" Text="Save CarrierType" OnClick="btnSaveCarrierType_Click" />
	<asp:Button ID="btnSaveCarrierTypeAs" runat="server" Text="Save CarrierType As" Enabled="false" OnClick="btnSaveCarrierTypeAs_Click" ToolTip="Save a new CarrierType based on these settings." />
	<asp:Button ID="btnDeleteCarrierType" runat="server" CssClass="redButton" Enabled="false" Text="Delete CarrierType" OnClick="btnDeleteCarrierType_Click" OnClientClick="return confirm('Are you certain you want to delete this CarrierType?');" />
	<asp:Button ID="btnClearCarrierType" Text="Clear CarrierType" ToolTip="If you have already clicked on another CarrierType below, you must click this button first before you try to create a new CarrierType." runat="server" OnClick="btnClearCarrierType_Click" />
    </div><br />
    <asp:Label ID="lblCarrierTypeMessage" runat="server"></asp:Label>
</fieldset>
<br /><br />
<asp:GridView ID="gvCarrierType" OnSelectedIndexChanged="gvCarrierType_SelectedIndexChanged" OnPageIndexChanging="gvCarrierType_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true"  CssClass="tblItems" runat="server">
    <HeaderStyle BackColor="#9a9a9a" ForeColor="White" Font-Bold="true" Height="30" />
    <AlternatingRowStyle BackColor="#dddddd" />
    <Columns>
	<asp:TemplateField>
	    <ItemTemplate>
		<asp:HiddenField ID="hdngvCarrierTypeId" Value='<%#Eval("Id") %>' runat="server" />
	    </ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField HeaderText="Carrier Type">
	    <ItemTemplate>
		<asp:Label ID="lblgvCarrierType" Text='<%#Eval("CarrierType") %>' runat="server" />
	    </ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
	    <ItemTemplate>
		<asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
	    </ItemTemplate>
	</asp:TemplateField>
    </Columns>
</asp:GridView>

<br /><br />
<asp:Button ID="btnCancelCarrierTypePopup" OnClick="btnCancelCarrierTypePopup_Click" runat="server" Text="Close" /> 
</ContentTemplate></asp:UpdatePanel>
</asp:Panel>



    <ajaxToolkit:ModalPopupExtender runat="server" 
	ID="mpeClientTypePopup" 
	TargetControlID="dummy2" 
	PopupControlID="pnlClientTypeHolder" 
	BackgroundCssClass="modalBackground"                        
	DropShadow="true"/> 
<input id="dummy2" type="button" style="display: none" runat="server" />
<asp:Panel ID="pnlClientTypeHolder" CssClass="modalPopup" runat="server">

    <asp:UpdatePanel ID="pnlClientTypeModal" runat="server" UpdateMode="Conditional"><ContentTemplate>
<h3>Client Types</h3>
<fieldset>
    <div class="dnnFormItem">
	<dnn:label ID="lblClientType" runat="server" />
	<asp:TextBox ID="txtClientType" placeholder="Client Type" runat="server" />
    </div>
    <div class="dnnFormItem">
	<asp:HiddenField Visible="false" ID="txtSelectedClientType" Value="-1" runat="server"></asp:HiddenField>
	<asp:HiddenField Visible="false" ID="txtClientTypeCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	<asp:HiddenField Visible="false" ID="txtClientTypeCreatedDate" Value="" runat="server"></asp:HiddenField>
	<asp:Button ID="btnSaveClientType" runat="server" Text="Save ClientType" OnClick="btnSaveClientType_Click" />
	<asp:Button ID="btnSaveClientTypeAs" runat="server" Text="Save ClientType As" Enabled="false" OnClick="btnSaveClientTypeAs_Click" ToolTip="Save a new ClientType based on these settings." />
	<asp:Button ID="btnDeleteClientType" runat="server" CssClass="redButton" Enabled="false" Text="Delete ClientType" OnClick="btnDeleteClientType_Click" OnClientClick="return confirm('Are you certain you want to delete this ClientType?');" />
	<asp:Button ID="btnClearClientType" Text="Clear ClientType" ToolTip="If you have already clicked on another ClientType below, you must click this button first before you try to create a new ClientType." runat="server" OnClick="btnClearClientType_Click" />
    </div><br />
    <asp:Label ID="lblClientTypeMessage" runat="server"></asp:Label>
</fieldset>
<br /><br />
<asp:GridView ID="gvClientType" OnSelectedIndexChanged="gvClientType_SelectedIndexChanged" OnPageIndexChanging="gvClientType_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true"  CssClass="tblItems" runat="server">
    <HeaderStyle BackColor="#9a9a9a" ForeColor="White" Font-Bold="true" Height="30" />
    <AlternatingRowStyle BackColor="#dddddd" />
    <Columns>
	<asp:TemplateField>
	    <ItemTemplate>
		<asp:HiddenField ID="hdngvClientTypeId" Value='<%#Eval("Id") %>' runat="server" />
	    </ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField HeaderText="Client Type">
	    <ItemTemplate>
		<asp:Label ID="lblgvClientType" Text='<%#Eval("ClientType") %>' runat="server" />
	    </ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
	    <ItemTemplate>
		<asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
	    </ItemTemplate>
	</asp:TemplateField>
    </Columns>
</asp:GridView>

<br /><br />
<asp:Button ID="btnCancelClientTypePopup" OnClick="btnCancelClientTypePopup_Click" runat="server" Text="Close" /> 
</ContentTemplate></asp:UpdatePanel>
    </asp:Panel>


    <ajaxToolkit:ModalPopupExtender runat="server" 
                        ID="mpeFreightPopup" 
                        TargetControlID="dummy" 
                        PopupControlID="pnlFreightHolder" 
                        BackgroundCssClass="modalBackground"                        
                        DropShadow="true"/> 
                <input id="dummy" type="button" style="display: none" runat="server" />
                <asp:Panel ID="pnlFreightHolder" CssClass="modalPopup" runat="server">
                <h3>Freight Types</h3>
                    <asp:UpdatePanel ID="pnlFreightPopup" runat="server" UpdateMode="Conditional"><ContentTemplate>
                <fieldset>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblFreightType" runat="server" />
	                <asp:TextBox ID="txtFreightType" placeholder="Freight Type" runat="server" />
                    </div>
                    <div class="dnnFormItem">
	                <asp:HiddenField Visible="false" ID="txtSelectedFreightType" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtFreightTypeCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtFreightTypeCreatedDate" Value="" runat="server"></asp:HiddenField>
	                <asp:Button ID="btnSaveFreightType" runat="server" Text="Save FreightType" OnClick="btnSaveFreightType_Click" />
	                <asp:Button ID="btnSaveFreightTypeAs" runat="server" Text="Save FreightType As" Enabled="false" OnClick="btnSaveFreightTypeAs_Click" ToolTip="Save a new FreightType based on these settings." />
	                <asp:Button ID="btnDeleteFreightType" runat="server" CssClass="redButton" Enabled="false" Text="Delete FreightType" OnClick="btnDeleteFreightType_Click" OnClientClick="return confirm('Are you certain you want to delete this FreightType?');" />
	                <asp:Button ID="btnClearFreightType" Text="Clear FreightType" ToolTip="If you have already clicked on another FreightType below, you must click this button first before you try to create a new FreightType." runat="server" OnClick="btnClearFreightType_Click" />
                    </div><br />
                    <asp:Label ID="lblFreightTypeMessage" runat="server"></asp:Label>
                </fieldset>
                <br /><br />
                <asp:GridView ID="gvFreightType" OnSelectedIndexChanged="gvFreightType_SelectedIndexChanged" OnPageIndexChanging="gvFreightType_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true"  CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#9a9a9a" ForeColor="White" Font-Bold="true" Height="30" />
                    <AlternatingRowStyle BackColor="#dddddd" />
                    <Columns>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:HiddenField ID="hdngvFreightTypeId" Value='<%#Eval("Id") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="Freight Type">
	                    <ItemTemplate>
		                <asp:Label ID="lblgvFreightType" Text='<%#Eval("FreightType") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
	                    </ItemTemplate>
	                </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                        <br /><br />
                    <asp:Button ID="btnCancelFreightPopup" OnClick="btnCancelFreightPopup_Click" runat="server" Text="Close" /> 
                        </ContentTemplate></asp:UpdatePanel>
                    </asp:Panel>

            <ajaxToolkit:ModalPopupExtender runat="server" 
	        ID="mpeTapeFormatPopup" 
	        TargetControlID="dummytape" 
	        PopupControlID="pnlTapeFormatHolder" 
	        BackgroundCssClass="modalBackground"                        
	        DropShadow="true"/> 
        <input id="dummytape" type="button" style="display: none" runat="server" />
        <asp:Panel ID="pnlTapeFormatHolder" CssClass="modalPopup" runat="server">
        <h3>TapeFormat Types</h3>
            <asp:UpdatePanel ID="pnlTapeFormatPopup" runat="server" UpdateMode="Conditional"><ContentTemplate>
        <fieldset>
            <div class="dnnFormItem">
	        <dnn:label ID="lblTapeFormat" runat="server" />
	        <asp:TextBox ID="txtTapeFormat" placeholder="Tape Format" runat="server" />
            </div>
            <div class="dnnFormItem">
	        <dnn:label ID="lblTapePrinter" runat="server" />
	        <asp:DropDownList ID="ddlTapePrinter" runat="server">
                <asp:ListItem Text="--Please Select--" Value="-1"></asp:ListItem>
                <asp:ListItem Text="Printer A" Value="A"></asp:ListItem>
                <asp:ListItem Text="Printer B" Value="B"></asp:ListItem>
	        </asp:DropDownList>
            </div>
            <div class="dnnFormItem">
	        <dnn:label ID="lblTapeLabel" runat="server" />
	        <asp:DropDownList ID="ddlTapeLabel" runat="server">
                <asp:ListItem Text="--Please Select--" Value="-1"></asp:ListItem>
                <asp:ListItem Text="Label A" Value="A"></asp:ListItem>
                <asp:ListItem Text ="Label B" Value="B"></asp:ListItem>
                <asp:ListItem Text="Label C" Value="C"></asp:ListItem>
	        </asp:DropDownList>
            </div>
            <div class="dnnFormItem">
	        <asp:HiddenField Visible="false" ID="txtSelectedTapeFormat" Value="-1" runat="server"></asp:HiddenField>
	        <asp:HiddenField Visible="false" ID="txtTapeFormatCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	        <asp:HiddenField Visible="false" ID="txtTapeFormatCreatedDate" Value="" runat="server"></asp:HiddenField>
	        <asp:Button ID="btnSaveTapeFormat" runat="server" Text="Save TapeFormat" OnClick="btnSaveTapeFormat_Click" />
	        <asp:Button ID="btnSaveTapeFormatAs" runat="server" Text="Save TapeFormat As" Enabled="false" OnClick="btnSaveTapeFormatAs_Click" ToolTip="Save a new TapeFormat based on these settings." />
	        <asp:Button ID="btnDeleteTapeFormat" runat="server" CssClass="redButton" Enabled="false" Text="Delete TapeFormat" OnClick="btnDeleteTapeFormat_Click" OnClientClick="return confirm('Are you certain you want to delete this TapeFormat?');" />
	        <asp:Button ID="btnClearTapeFormat" Text="Clear TapeFormat" ToolTip="If you have already clicked on another TapeFormat below, you must click this button first before you try to create a new TapeFormat." runat="server" OnClick="btnClearTapeFormat_Click" />
            </div><br />
            <asp:Label ID="lblTapeFormatMessage" runat="server"></asp:Label>
        </fieldset>
        <br /><br />
        <asp:GridView ID="gvTapeFormat" OnSelectedIndexChanged="gvTapeFormat_SelectedIndexChanged" OnPageIndexChanging="gvTapeFormat_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true"  CssClass="tblItems" runat="server">
            <HeaderStyle BackColor="#9a9a9a" ForeColor="White" Font-Bold="true" Height="30" />
            <AlternatingRowStyle BackColor="#dddddd" />
            <Columns>
	        <asp:TemplateField>
	            <ItemTemplate>
		        <asp:HiddenField ID="hdngvTapeFormatId" Value='<%#Eval("Id") %>' runat="server" />
	            </ItemTemplate>
	        </asp:TemplateField>
	        <asp:TemplateField HeaderText="Tape Format">
	            <ItemTemplate>
		        <asp:Label ID="lblgvTapeFormat" Text='<%#Eval("TapeFormat") %>' runat="server" />
	            </ItemTemplate>
	        </asp:TemplateField>
                <asp:TemplateField HeaderText="Printer">
	            <ItemTemplate>
		        <asp:Label ID="lblgvTapeFormatPrinter" Text='<%#Eval("Printer") %>' runat="server" />
	            </ItemTemplate>
	        </asp:TemplateField>
                <asp:TemplateField HeaderText="Label">
	            <ItemTemplate>
		        <asp:Label ID="lblgvTapeFormatLabel" Text='<%#Eval("Label") %>' runat="server" />
	            </ItemTemplate>
	        </asp:TemplateField>
	        <asp:TemplateField>
	            <ItemTemplate>
		        <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
	            </ItemTemplate>
	        </asp:TemplateField>
            </Columns>
        </asp:GridView>
	        <br /><br />
            <asp:Button ID="btnCancelTapeFormatPopup" OnClick="btnCancelTapeFormatPopup_Click" runat="server" Text="Close" /> 
	        </ContentTemplate></asp:UpdatePanel>
            </asp:Panel>

    <ajaxToolkit:ModalPopupExtender runat="server" 
                        ID="mpeDeliveryMethodPopup" 
                        TargetControlID="dummydm" 
                        PopupControlID="pnlDeliveryMethodHolder" 
                        BackgroundCssClass="modalBackground"                        
                        DropShadow="true"/> 
                <input id="dummydm" type="button" style="display: none" runat="server" />
                <asp:Panel ID="pnlDeliveryMethodHolder" CssClass="modalPopup" runat="server">
                <h3>DeliveryMethod Types</h3>
                    <asp:UpdatePanel ID="pnlDeliveryMethodPopup" runat="server" UpdateMode="Conditional"><ContentTemplate>
                <fieldset>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblDeliveryMethod" runat="server" />
	                <asp:TextBox ID="txtDeliveryMethod" placeholder="Delivery Method" runat="server" />
                    </div>
                    <div class="dnnFormItem">
	                <asp:HiddenField Visible="false" ID="txtSelectedDeliveryMethod" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtDeliveryMethodCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtDeliveryMethodCreatedDate" Value="" runat="server"></asp:HiddenField>
	                <asp:Button ID="btnSaveDeliveryMethod" runat="server" Text="Save DeliveryMethod" OnClick="btnSaveDeliveryMethod_Click" />
	                <asp:Button ID="btnSaveDeliveryMethodAs" runat="server" Text="Save DeliveryMethod As" Enabled="false" OnClick="btnSaveDeliveryMethodAs_Click" ToolTip="Save a new DeliveryMethod based on these settings." />
	                <asp:Button ID="btnDeleteDeliveryMethod" runat="server" CssClass="redButton" Enabled="false" Text="Delete DeliveryMethod" OnClick="btnDeleteDeliveryMethod_Click" OnClientClick="return confirm('Are you certain you want to delete this DeliveryMethod?');" />
	                <asp:Button ID="btnClearDeliveryMethod" Text="Clear DeliveryMethod" ToolTip="If you have already clicked on another DeliveryMethod below, you must click this button first before you try to create a new DeliveryMethod." runat="server" OnClick="btnClearDeliveryMethod_Click" />
                    </div><br />
                    <asp:Label ID="lblDeliveryMethodMessage" runat="server"></asp:Label>
                </fieldset>
                <br /><br />
                <asp:GridView ID="gvDeliveryMethod" OnSelectedIndexChanged="gvDeliveryMethod_SelectedIndexChanged" OnPageIndexChanging="gvDeliveryMethod_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true"  CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#9a9a9a" ForeColor="White" Font-Bold="true" Height="30" />
                    <AlternatingRowStyle BackColor="#dddddd" />
                    <Columns>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:HiddenField ID="hdngvDeliveryMethodId" Value='<%#Eval("Id") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="DeliveryMethod Type">
	                    <ItemTemplate>
		                <asp:Label ID="lblgvDeliveryMethod" Text='<%#Eval("DeliveryMethod") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
	                    </ItemTemplate>
	                </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                        <br /><br />
                    <asp:Button ID="btnCancelDeliveryMethodPopup" OnClick="btnCancelDeliveryMethodPopup_Click" runat="server" Text="Close" /> 
                        </ContentTemplate></asp:UpdatePanel>
                    </asp:Panel>

    <ajaxToolkit:ModalPopupExtender runat="server" 
	ID="mpeMarket2Popup" 
	TargetControlID="dummysm2" 
	PopupControlID="pnlMarket2Holder" 
	BackgroundCssClass="modalBackground"                        
	DropShadow="true"/> 
<input id="dummysm2" type="button" style="display: none" runat="server" />
<asp:Panel ID="pnlMarket2Holder" CssClass="modalPopup" runat="server">
<h3>Markets</h3>
    <asp:UpdatePanel ID="pnlMarket2Modal" runat="server" UpdateMode="Always"><ContentTemplate>
	<fieldset>
	    <div class="dnnFormItem">
		<dnn:label ID="lblMarket2Name" runat="server" />
		<asp:TextBox ID="txtMarket2Name" placeholder="Market Name" runat="server" />
	    </div>
	    <div class="dnnFormItem">
		<dnn:label ID="lblMarket2Description" runat="server" />
		<asp:TextBox ID="txtMarket2Description" TextMode="MultiLine" Rows="6" placeholder="Description" runat="server" />
	    </div>
	    <div class="dnnFormItem">
		<dnn:label ID="lblMarket2Parent" runat="server" />
		<asp:DropDownList ID="ddlMarket2Parent" runat="server"></asp:DropDownList>
	    </div>
	    <div class="dnnFormItem">
		<asp:HiddenField Visible="false" ID="txtSelectedMarket2" Value="-1" runat="server"></asp:HiddenField>
		<asp:HiddenField Visible="false" ID="txtMarket2CreatedBy" Value="-1" runat="server"></asp:HiddenField>
		<asp:HiddenField Visible="false" ID="txtMarket2CreatedDate" Value="" runat="server"></asp:HiddenField>
		<asp:Button ID="btnSaveMarket2" runat="server" Text="Save Market" OnClick="btnSaveMarket2_Click" />
		<asp:Button ID="btnSaveMarket2As" runat="server" Text="Save Market As" Enabled="false" OnClick="btnSaveMarket2As_Click" ToolTip="Save a new Market based on these settings." />
		<asp:Button ID="btnDeleteMarket2" runat="server" CssClass="redButton" Enabled="false" Text="Delete Market" OnClick="btnDeleteMarket2_Click" OnClientClick="return confirm('Are you certain you want to delete this Market?');" />
		<asp:Button ID="btnClearMarket2" Text="Clear Market2" ToolTip="If you have already clicked on another Market below, you must click this button first before you try to create a new Market." runat="server" OnClick="btnClearMarket2_Click" />
	    </div><br />
	    <asp:Label ID="lblMarket2Message" runat="server"></asp:Label>
	    <asp:TextBox ID="txtMarket2Search" placeholder="Search" runat="server"></asp:TextBox> <asp:Button ID="btnMarket2Search" runat="server" OnClick="btnMarket2Search_Click" Text="Search" /><asp:Button ID="btnMarket2SearchClear" runat="server" Text ="Clear Search" OnClick ="btnMarket2SearchClear_Click" />
	</fieldset>
	<br /><br />
	<asp:GridView ID="gvMarket2" OnSelectedIndexChanged="gvMarket2_SelectedIndexChanged" OnPageIndexChanging="gvMarket2_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true"  CssClass="tblItems" runat="server">
	    <HeaderStyle BackColor="#9a9a9a" ForeColor="White" Font-Bold="true" Height="30" />
	    <AlternatingRowStyle BackColor="#dddddd" />
	    <Columns>
		<asp:TemplateField>
		    <ItemTemplate>
			<asp:HiddenField ID="hdngvMarket2Id" Value='<%#Eval("Id") %>' runat="server" />
		    </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Name">
		    <ItemTemplate>
			<asp:Label ID="lblgvMarketName" Text='<%#Eval("MarketName") %>' runat="server" />
		    </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
		    <ItemTemplate>
			<asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
		    </ItemTemplate>
		</asp:TemplateField>
	    </Columns>
	</asp:GridView>
	<br /><br />
    <asp:Button ID="btnCancelMarket2Popup" OnClick="btnCancelMarket2Popup_Click" runat="server" Text="Close" /> 
	</ContentTemplate></asp:UpdatePanel>
    </asp:Panel>


    <ajaxToolkit:ModalPopupExtender runat="server" 
	ID="mpeStationGroupStationsPopup" 
	TargetControlID="dummysg" 
	PopupControlID="pnlStationGroupStationsHolder" 
	BackgroundCssClass="modalBackground"                        
	DropShadow="true"/> 
<input id="dummysg" type="button" style="display: none" runat="server" />
<asp:Panel ID="pnlStationGroupStationsHolder" CssClass="modalPopup" runat="server">

    <asp:UpdatePanel ID="pnlStationGroupStationsModal" runat="server" UpdateMode="Always"><ContentTemplate>
<h3>Stations in this Station Group</h3>
<fieldset>
    <div class="dnnFormItem">
	<dnn:label ID="lblStationsInGroupModal" runat="server" />
	<asp:ListBox ID="lbxStationsInGroupModal" runat="server" Rows="6"></asp:ListBox>
    </div>
    <div class="dnnFormItem">
	<asp:HiddenField Visible="false" ID="txtSelectedStationGroupStations" Value="-1" runat="server"></asp:HiddenField>
	<asp:HiddenField Visible="false" ID="txtStationGroupStationsCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	<asp:HiddenField Visible="false" ID="txtStationGroupStationsCreatedDate" Value="" runat="server"></asp:HiddenField>
	<asp:Button ID="btnRemoveStationFromGroup" runat="server" CssClass="redButton" Enabled="true" Text="Remove Station From Group" OnClick="btnRemoveStationFromGroup_Click" />
    </div><br />
    <asp:Label ID="lblStationGroupStationsModalMessage" runat="server"></asp:Label>
</fieldset>
<br /><br />
            <div class="dnnFormItem">
	                <dnn:label ID="lblStationGroupMarketSearch" runat="server" />
	                <asp:DropDownList ID="ddlStationGroupMarketSearch" runat="server"></asp:DropDownList>
                    </div>
                    <div class="dnnFormItem">
	                <dnn:label ID="lblStationGroupStationSearch" runat="server" />
	                <asp:TextBox ID="txtStationGroupStationSearch" runat="server" placeholder="keyword"></asp:TextBox>
                        <asp:Button ID="btnStationGroupStationSearch" runat="server" Text="Search Stations" OnClick="btnStationGroupStationSearch_Click" />
                    </div>
<asp:GridView ID="gvStationGroupStations" OnSelectedIndexChanged="gvStationGroupStations_SelectedIndexChanged" OnPageIndexChanging="gvStationGroupStations_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true"  CssClass="tblItems" runat="server">
    <HeaderStyle BackColor="#9a9a9a" ForeColor="White" Font-Bold="true" Height="30" />
    <AlternatingRowStyle BackColor="#dddddd" />
    <Columns>
	<asp:TemplateField>
	    <ItemTemplate>
		<asp:HiddenField ID="hdngvStationGroupStationId" Value='<%#Eval("Id") %>' runat="server" />
	    </ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField HeaderText="Station Name">
	    <ItemTemplate>
		<asp:Label ID="lblgvStationName" Text='<%#Eval("StationName") %>' runat="server" />
	    </ItemTemplate>
	</asp:TemplateField>
        <asp:TemplateField HeaderText="Call Letters">
	    <ItemTemplate>
		<asp:Label ID="lblgvStationGroupCallLetters" Text='<%#Eval("CallLetter") %>' runat="server" />
	    </ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
	    <ItemTemplate>
		<asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Add" />
	    </ItemTemplate>
	</asp:TemplateField>
    </Columns>
</asp:GridView>

<br /><br />
<asp:Button ID="btnCancelStationGroupStationsPopup" OnClick="btnCancelStationGroupStationsPopup_Click" runat="server" Text="Close" /> 
</ContentTemplate></asp:UpdatePanel>
    </asp:Panel>



</div>