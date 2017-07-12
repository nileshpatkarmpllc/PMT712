<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMT_Advertisers.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.PMT_Advertisers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %> 
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ register tagprefix="dnn" tagname="Label" src="~/controls/LabelControl.ascx" %>

<dnn:DnnCssInclude ID="DnnCssInclude3" runat="server" FilePath="/DesktopModules/PMT_Admin/styles/style.css" Priority="11" />

<style>
@import url(https://fonts.googleapis.com/css?family=Roboto:400,500);

.pmt {
	font-family:'Roboto', sans-serif;
}

.pmt .group-hdr {
	position:relative;
	padding:5px 30px;
	color:#FFF;
	text-transform:uppercase;
	background:#6291fc;
	-webkit-box-shadow: 0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	-moz-box-shadow:    0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	box-shadow:         0px 2px 5px 0px rgba(50, 50, 50, 0.25);
}
.pmt .group-content {
	padding:15px;
	background:#f7f7f7;
	-webkit-box-shadow: 0px 1px 2px 0px rgba(50, 50, 50, 0.50);
	-moz-box-shadow:    0px 1px 2px 0px rgba(50, 50, 50, 0.50);
	box-shadow:         0px 1px 2px 0px rgba(50, 50, 50, 0.50);
}

.pmt .sub-hdr {
	position:relative;
	padding:5px 10px;
	color:#3576be;
	text-transform:uppercase;
	-webkit-box-shadow: 0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	-moz-box-shadow:    0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	box-shadow:         0px 2px 5px 0px rgba(50, 50, 50, 0.25);
}
.pmt .sub-content{
	background:#FFF;
	overflow:hidden;
}
.pmt .sub-search-options {
	padding:10px 20px;
}


.pmt .table-container{
	/*max-height:210px;
	overflow-y: scroll;*/
}
.pmt .sub-search-options select {
	min-width:15%;
    border:solid 1px #cccccc;
}
.pmt .sub-search-options .action-links {
	float:right;
}
.pmt .sub-search-options .action-links a {
	display:inline-block;
	padding:5px 10px; 
	font-size:12px;
	color:#666;
	cursor:pointer;
}
.pmt .btn.non-deliverable{
	margin:15px;
	float:right;
}
.pmt .pagination {
	margin:20px 0;
	color:#666;
	font-size:11px;
	list-style-type:none;
}
.pmt .pagination td {
	display:inline-block;
	padding:3px 5px;
}
.pmt .pagination li.cur {
	color:#FFF;
	background:#666;
}
.pmt .create-master-item {
	padding:15px 0; 
	background:#f0f0f0;
	color:#666;
	font-size:12px;
}
.pmt .create-master-item .col {
	padding: 1% 2%;
	display:inline-block;
	vertical-align:top;
}
.pmt .create-master-item label {
	text-transform:uppercase;
	color:#666;
	font-size:12px;
}
.pmt .create-master-item input, .pmt .create-master-item select, .pmt .create-master-item textarea {
	width: 50%;
    border:1px solid #cccccc;
    margin-bottom:5px;
}
.pmt .create-master-item textarea {
    width:100%;
    height:110px;
}
.pmt .create-master-item .checkbox {
	margin:0 20px 0 0;
}
.pmt .create-master-item .checkbox input {
	width:auto;
}
.pmt .create-master-item .full {
	width:100%;
}
.pmt .col25 {
	width:33%;
    float:left;
}
.pmt .col25b {
    float:left;
}
.pmt .create-master-item .col33 {
	width:28%;
}
.pmt .create-master-item .col50 {
	width:45%;
    float:left;
}
.pmt .create-master-item .col75 {
	width:70%;
}
.pmt .create-master-item .clear {
	padding:0;
	clear:both;
	display:block;
}
.pmt .cmi-col {
	display:inline-block;
	padding:0 1%;
	width:47%;
	vertical-align:top;
}
.pmt .create-master-item .bottom {
	color:#666;
	background:#FFF;
    width:100%;
    border:1px solid #cccccc;
}
.pmt .create-master-item .bottom .btitle {
	display:block;
	text-transform:uppercase;
}
.pmt .create-master-item .bottom textarea {
	margin:10px 0 0 0;
	width:100%;
	height:100px;
	border:none;
	color:#666;
	font-family:'Roboto', sans-serif;
}
.pmt .create-master-item .bottom .action-btns {
	padding:30px 5%;
	border-left:1px solid #CCC;
	text-align:center;
} 
.pmt .create-master-item .bottom .action-btns .btn {
	margin:10px;
	text-align:center;
    width:30%;
}
.pmt .table {
	display:table;
	width:100% !important;
	font-size:12px;
	line-height:12px;
	color:#747474;
	/*text-align:center;*/
	border-spacing: 1px;
	background:#dedede;
}
.pmt .table .head {
	display:table-row;
	text-transform:uppercase;
	color:#000;
	background:#edf7ff;
}
.pmt .table .row {
	display:table-row;
}
.pmt .table .row:nth-child(even) {
	background: #FFF
}
.pmt .table .row:nth-child(odd) {
	background: #f7f7f7
}
.pmt .table .cell {
	display:table-cell;
	padding:8px;
}
.pmt .table .cell.sm {
	width:10%;
}
.pmt .cmi-col .table-header {
	padding:8px;
	background:#FFF;
	border:1px solid #dedede;
	border-width:1px 1px 0 1px;
}
.table-header th {
    padding:8px;
	background:#edf7ff;
	border:1px solid #dedede;
	border-width:1px 1px 0 1px;
}
.pmt .kw-search{	
	display:inline-block;
}
.pmt .kw-search .icon {	
	float:left;
	display:inline-block;
	width:17px;
	height:17px;
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABEAAAARCAYAAAA7bUf6AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAbNJREFUeNp8071Lm1EUx/EnxlpfqlZjrZtQilAJ8QVpl26KOJgM4gtItyIIDhZLrYLoIqjoYJdC1kZdREScCr4gLk5VULDQUvAPqKVW0aDEfq/8Eg4h8cIH7pPknufcc0580WjUMyuADoQRQj5+YAsx/ELCS1u5Zv8C82hN+81TvEYvxrCCm0xBarGKGj2f4Q9u8QgVeI5FZRezGeWgHJ9MgH30ox5N6MEaruDHHBrgs0E60ay3HiCCr+hGUPXowhKuldWAMkwFiWh/jhlc4Ave45m+c4encKznNpTZmgSV2m+s442KHFLA5PqJI33+BCU2k4faJ3Tn75hIC+CZjJLn/DYT1/sqFKMOO17m5WpQbbp3bjPZ0N7dcShtdux6i0bt9zQCqSALONHhdnw27fZ093cY1d51cRan9jpurD8qmHvuwyvVJq4ruHkpNIHdBG8rYCr1ZeRpkCrVgVCWa7lOftDZcfeBPxx2/7W7iIfYVNUD5s2nGrhBtdllWYSXeIBdW0QX6JuKW6Zu5agLfxUs2bkRFGAYpZk68U8yrUtMa6aG1fZwtnbet+IK9BgtmPwvwADbz1tf2VH6ogAAAABJRU5ErkJggg==') no-repeat;
}
.pmt .kw-search input{	
	margin:0 0 0 5px;
	/*border:none;*/
    border:solid 1px #cccccc;
}
.pmt .play-btn {
	display:inline-block;
	width:18px;
	height:18px;
	cursor:pointer;
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABIAAAASCAMAAABhEH5lAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAADBQTFRF6enp+fn5////5ubm7+/v5eXl1dXV2dnZ0tLSz8/P9vb239/f2NjY8vLyzMzM////TtOJoAAAABB0Uk5T////////////////////AOAjXRkAAACNSURBVHjaXJCJEsMgCETpEfHg+P+/7YI0TcqMozxdZCGP0NXM2tJMCGuyVfDcaA4kXaRjGzNQEEmJSjJy7MsrFrROijd4XBT3SriA6rDxSi0k1Kx7IPzwxqFbo9QlQp1HKG9INroItYRn+eMsr9HKvYn/VqUM8TbEZahsM/9sg8l3OFLDuYzwGclHgAEA0HMQIKkPIY4AAAAASUVORK5CYII=') no-repeat;
}
.pmt .play-btn:hover {
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABIAAAASCAMAAABhEH5lAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAADBQTFRFYv8fWP8Qbf8vV/8PkP9gY/8gbv8w8P/qpf9/ef9Aj/9f3f/Pef8/0v+/Tf8A////MRZddgAAABB0Uk5T////////////////////AOAjXRkAAACcSURBVHjaVJCJEsMgCETxSjw4/v9vs6BtU2bU4emuAJmH1Ev1qhIJYa2hJ8baaCUkxEw40nLkhEMiHIwM57QTE1ojwRuzXjfDvRAuoOqaemghoaz+aYd1c0PSi0IXCD63K/8Qb5S1HURbmL/25WcvXor1+SrC9/oqlb2hgne7IbRfVrRdvKgxmgbZw+HPcPgMxyUzq+YptyePAAMAhtAPxt5Zn3sAAAAASUVORK5CYII=') no-repeat;
}
.pmt .edit-btn {
	display:inline-block;
	width:18px;
	height:18px;
	cursor:pointer;
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABEAAAAPCAMAAAA1b9QjAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAADBQTFRFz8/P3t7e7+/v0tLS7Ozs6enp9vb28/Pz1dXV5ubm2NjY8vLy/Pz85eXlzMzM////GqKacwAAABB0Uk5T////////////////////AOAjXRkAAABqSURBVHjaVM9REoAgCEXRpyZRIux/t6GZ2v08M6DAyjWLQtlw6IxFkgYkxDISEgJDb/siZsdN6M4azJY0YNnkhfbWkAGonxCuNlJ0StW+YxPW4LDLIX1bF//zOQv6v6ufZrBSV9mHHwEGAOJbC2/muJh9AAAAAElFTkSuQmCC') no-repeat;
}
.pmt input, .pmt select {
	padding:4px;
	color:#666;
}
.pmt option {
	background:#FFF;
	color:#666;
}
.pmt .btn {
	display:inline-block;
	padding:5px 10px;
	font-size:14px;
	text-transform:uppercase;
	cursor:pointer;
	-webkit-box-shadow: 0px 1px 2px 0px rgba(50, 50, 50, 0.25);
	-moz-box-shadow:    0px 1px 2px 0px rgba(50, 50, 50, 0.25);
	box-shadow:         0px 1px 2px 0px rgba(50, 50, 50, 0.25);
	-webkit-transition:	background-color 400ms linear;
    -moz-transition: 	background-color 400ms linear;
    -o-transition: 		background-color 400ms linear;
    -ms-transition: 	background-color 400ms linear;
    transition: 		background-color 400ms linear;
    margin-right:10px;
    margin-bottom:10px;
}
.pmt .btn-green {
	color:#FFF;
	background:#4dff00;
}
.pmt .btn-green:hover {
	background:#49f200;
}
.pmt .btn-blue {
	color:#FFF;
	background:#3498db;
}
.pmt .btn-blue:hover {
	background:#9fa8da;
}
.pmt .btn-default {
	color:#808080;
	background:#f7f7f7;
}
.pmt .btn-default:hover {
	background:#ebebeb;
}
.onoffswitch {
	display:inline-block;
    position: relative; top:2px; width: 40px;
    -webkit-user-select:none; -moz-user-select:none; -ms-user-select: none;
}
.onoffswitch-checkbox {
    display: none;
}
.onoffswitch-label {
    display: block; overflow: hidden; cursor: pointer;
    border: 1px solid #999999; border-radius: 3px;
}
.onoffswitch-inner {
    display: block; width: 200%; margin-left: -100%;
    transition: margin 0.3s ease-in 0s;
}
.onoffswitch-inner:before, .onoffswitch-inner:after {
    display: block; float: left; width: 50%; height: 15px; padding: 0; line-height: 15px;
    box-sizing: border-box;
}
.onoffswitch-inner:before {
    content: "";
    padding-left: 3px;
    background-color: #459EFA; color: #FFFFFF;
}
.onoffswitch-inner:after {
    content: "";
    padding-right: 3px;
    background-color: #EEEEEE; color: #999999;
    text-align: right;
}
.onoffswitch-switch {
    display: block; width: 15px; margin: 0px;
    background: #FFFFFF;
    position: absolute; top: 0; bottom: 0;
    right: 23px;
    border: 1px solid #999999; border-radius: 3px;
    transition: all 0.3s ease-in 0s; 
}
.onoffswitch-checkbox:checked + .onoffswitch-label .onoffswitch-inner {
    margin-left: 0;
}
.onoffswitch-checkbox:checked + .onoffswitch-label .onoffswitch-switch {
    right: 0px; 
}
.AdvertiserPulldown {
    width:100% !important;
}
.pmt-approved label {
    margin-left:10px;
}
.ccChk {
    width:10% !important;
    float:left;
}
.table table td {
    padding:4px;
    border:1px solid #dedede;
	border-width:1px 1px 0 1px;
}
.table table {
    background-color:white;
    width:100%;
    border:none;
}
.table table table {
    width:30%;
}
.table table table td {
    border:none;
}
#videoPlayer {
    width:100%;
    text-align:center;
}
.pmt .btnAddEdit {
    width:20% !important;
}
.pmt .dnnLabel {
    width:20%;
    text-align:left;
    min-width:100px;
    padding-left:4px;
}
.pmt .altRow {
        background-color:#e3e3e3;
        margin-bottom:5px;
        margin-top:5px;
        padding-top:5px;
    }
    .pmt .editHolder {
        margin-bottom:5px;
    }
     .pmt-SelectList {
        max-height:150px;
        overflow-y:scroll;
        overflow-x:hidden;
        max-width:300px;
        background-color:white;
        margin-bottom:10px;
        border:solid 1px #dedede;
        padding:10px;
    }
    .pmt-SelectList input {
        margin-right:10px;
    }
    .pmt-SelectList input[type="checkbox"] {
        width:10%;
    }
</style>

<asp:UpdateProgress ID="updAdvertiser" AssociatedUpdatePanelID="pnlAdvertisers" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<div id="PMT_Advertisers">
        <asp:UpdatePanel ID="pnlAdvertisers" runat="server" UpdateMode="Always" visible="true">
            <ContentTemplate>

                <div class="pmt">
	
		<div class="sub-hdr">
			<asp:Label ID="lblAdvertiserMessage" runat="server"></asp:Label>
			<span class="icon"></span>
		</div>
		<div class="sub-content">
			
			<div class="sub-search-options">
				<!--<div class="action-links">
					<a>Select All</a>
					<a>Clear All</a>
				</div>-->
				
				<div class="kw-search">
					<span class="icon"></span>
					<asp:TextBox ID="txtAdvertiserSearch" runat="server" AutoPostBack="true" OnTextChanged="txtAdvertiserSearch_TextChanged" EnableViewState="true" placeholder="keyword"></asp:TextBox> <asp:DropDownList ID="ddlAdvertiserAgencySearch" AutoPostBack="true" OnSelectedIndexChanged="ddlAdvertiserAgencySearch_SelectedIndexChanged" runat="server"></asp:DropDownList>
				</div>
	                
				
			</div>
			
			<span class="table-container">
				<span class="table">
                    <asp:GridView ID="gvAdvertiser" OnSelectedIndexChanged="gvAdvertiser_SelectedIndexChanged" OnPageIndexChanging="gvAdvertiser_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#edf7ff" CssClass="table-header" />
                    <Columns>
	                <asp:TemplateField HeaderText="Name">
	                    <ItemTemplate>
		                <span class="cell"><asp:HiddenField ID="hdngvAdvertiserId" Value='<%#Eval("Id") %>' runat="server" /><asp:Label ID="lblgvAdvertiserName" Text='<%#Eval("AdvertiserName") %>' runat="server" /></span>
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:BoundField HeaderText="City" DataField="City" ItemStyle-CssClass="cell" SortExpression="City"></asp:BoundField>
	                <asp:BoundField HeaderText="State" DataField="State" ItemStyle-CssClass="cell" SortExpression="State"></asp:BoundField>
	                <asp:BoundField HeaderText="Phone" DataField="Phone" ItemStyle-CssClass="cell" SortExpression="Phone"></asp:BoundField>
	                <asp:BoundField HeaderText="Cust Ref" DataField="CustomerReference" ItemStyle-CssClass="cell" SortExpression="CustomerReference"></asp:BoundField>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
	                    </ItemTemplate>
	                </asp:TemplateField>
                    </Columns>
                </asp:GridView></span></span>
            <div class="sub-hdr">
			Create/Edit Advertiser
			<span class="icon"></span>
		</div>
		<div class="sub-content create-master-item">
            <div class="editHolder">

                    <div class="col50">
	                <dnn:label ID="lblAdvertiserName" runat="server" />
	                <asp:TextBox ID="txtAdvertiserName" placeholder="Advertiser Name" runat="server" />
                    </div>
                    <div class="col50">
	                <dnn:label ID="lblAdvertiserAgency" runat="server" />
	                <div class="pmt-SelectList"><asp:CheckBoxList ID="ddlAdvertiserAgency" runat="server"></asp:CheckBoxList></div>
                    </div>
                    <br clear="both" />
                    <div class="altRow"><div class="col50">
	                <dnn:label ID="lblAdvertiserCarrier" runat="server" />
	                <asp:DropDownList ID="ddlAdvertiserCarrier" runat="server"></asp:DropDownList> <asp:Button id="btnAddCarrier" runat="server" Text="Add/Edit" CssClass="btnAddEdit btn-blue" OnClick="btnAddCarrier_Click" />
                    </div>
                    <div class="col50">
	                <dnn:label ID="lblAdvertiserFreight" runat="server" />
	                <asp:DropDownList ID="ddlAdvertiserFreight" runat="server"></asp:DropDownList> <asp:Button id="btnAddFreight" runat="server" Text="Add/Edit" CssClass="btnAddEdit btn-blue" OnClick="btnAddFreight_Click" />
                    </div><br clear="both" /></div>
                    <div class="col25">
	                <dnn:label ID="lblAdvertiserCarrierNum" runat="server" />
	                <asp:TextBox ID="txtAdvertiserCarrierNum" placeholder="Carrier #" runat="server" />
                    </div>
                    <div class="col25">
	                <dnn:label ID="lblAdvertiserAddress1" runat="server" />
	                <asp:TextBox ID="txtAdvertiserAddress1" placeholder="Address1" runat="server" />
                    </div></div>
                    <div class="col25">
	                <dnn:label ID="lblAdvertiserAddress2" runat="server" />
	                <asp:TextBox ID="txtAdvertiserAddress2" placeholder="Address2" runat="server" />
                    </div><br clear="both" />
                    <div class="altRow"><div class="col25">
	                <dnn:label ID="lblAdvertiserCity" runat="server" />
	                <asp:TextBox ID="txtAdvertiserCity" placeholder="City" runat="server" />
                    </div>
                    <div class="col25">
	                <dnn:label ID="lblAdvertiserState" runat="server" />
	                <asp:TextBox ID="txtAdvertiserState" placeholder="State/Province" runat="server" />
                    </div>
                    <div class="col25">
	                <dnn:label ID="lblAdvertiserZip" runat="server" />
	                <asp:TextBox ID="txtAdvertiserZip" placeholder="Zip/PostalCode" runat="server" />
                    </div><br clear="both" /></div>
                    <div class="col25">
	                <dnn:label ID="lblAdvertiserCountry" runat="server" />
	                <asp:TextBox ID="txtAdvertiserCountry" placeholder="Country" runat="server" />
                    </div>
                    <div class="col25">
	                <dnn:label ID="lblAdvertiserPhone" runat="server" />
	                <asp:TextBox ID="txtAdvertiserPhone" placeholder="Phone" runat="server" />
                    </div>
                    <div class="col25">
	                <dnn:label ID="lblAdvertiserFax" runat="server" />
	                <asp:TextBox ID="txtAdvertiserFax" placeholder="Fax" runat="server" />
                    </div><br clear="both" />
                    <div class="altRow"><div class="col50">
	                <dnn:label ID="lblAdvertiserClientType" runat="server" />
	                <asp:DropDownList ID="ddlAdvertiserClientType" runat="server"></asp:DropDownList> <asp:Button id="btnAddClientType" runat="server" Text="Add/Edit" CssClass="btnAddEdit btn-blue" OnClick="btnAddClientType_Click" />
                    </div>
                    <div class="col25">
	                <dnn:label ID="lblAdvertiserCustomerReference" runat="server" />
	                <asp:TextBox ID="txtAdvertiserCustomerReference" placeholder="Customer Reference" runat="server" />
                    </div><br clear="both" /></div>
            </div>
	                <asp:HiddenField Visible="false" ID="txtSelectedAdvertiser" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtAdvertiserCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtAdvertiserCreatedDate" Value="" runat="server"></asp:HiddenField>
	                <div style="width:100%;text-align:center;padding-top:10px;"><div class="col25b"><asp:Button ID="btnSaveAdvertiser" runat="server" Text="Save Advertiser" CssClass="btn btn-blue" OnClick="btnSaveAdvertiser_Click" /></div>
	                <div class="col25b"><asp:Button ID="btnSaveAdvertiserAs" runat="server" Text="Save Advertiser As" Enabled="false" CssClass="btn btn-blue" OnClick="btnSaveAdvertiserAs_Click" ToolTip="Save a new Advertiser based on these settings." /></div>
	                <div class="col25b"><asp:Button ID="btnDeleteAdvertiser" runat="server" CssClass=" btn redButton" Enabled="false" Text="Delete Advertiser" OnClick="btnDeleteAdvertiser_Click" OnClientClick="return confirm('Are you certain you want to delete this Advertiser?');" /></div>
	                <div class="col25b"><asp:Button ID="btnClearAdvertiser" Text="Clear Advertiser" CssClass="btn btn-default" ToolTip="If you have already clicked on another Advertiser below, you must click this button first before you try to create a new Advertiser." runat="server" OnClick="btnClearAdvertiser_Click" /></div><br clear="both" /></div>
                                       
                </div></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

<ajaxToolkit:ModalPopupExtender runat="server" 
	ID="mpeClientTypePopup" 
	TargetControlID="dummy2" 
	PopupControlID="pnlClientTypeHolder" 
	BackgroundCssClass="modalBackground"                        
	DropShadow="true"/> 
<input id="dummy2" type="button" style="display: none" runat="server" />
<asp:Panel ID="pnlClientTypeHolder" CssClass="modalPopup" runat="server">

    <asp:UpdatePanel ID="pnlClientTypeModal" runat="server" UpdateMode="Conditional"><ContentTemplate>
        <div class="pmt">
	
		<div class="sub-hdr">
			Client Types <asp:Label ID="lblClientTypeMessage" runat="server" Text=""></asp:Label>
			<span class="icon"></span>
		</div>
		<div class="sub-content">
			
			<div class="sub-search-options">
				
				<div class="kw-search">
					
				</div>
                </div>
            <div class="table-container">
				<div class="table">
<asp:GridView ID="gvClientType" OnSelectedIndexChanged="gvClientType_SelectedIndexChanged" OnPageIndexChanging="gvClientType_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#edf7ff" CssClass="table-header" />
    <Columns>
	<asp:TemplateField HeaderText="Client Type">
	    <ItemTemplate>
		<asp:HiddenField ID="hdngvClientTypeId" Value='<%#Eval("Id") %>' runat="server" /><asp:Label ID="lblgvClientType" Text='<%#Eval("ClientType") %>' runat="server" />
	    </ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
	    <ItemTemplate>
		<asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
	    </ItemTemplate>
	</asp:TemplateField>
    </Columns>
</asp:GridView>
            </div></div>
            <div class="create-master-item">
    <div class="col50">
	<dnn:label ID="lblClientType" runat="server" />
	<asp:TextBox ID="txtClientType" placeholder="Client Type" runat="server" />
    </div><br clear="both" /></div>

	<asp:HiddenField Visible="false" ID="txtSelectedClientType" Value="-1" runat="server"></asp:HiddenField>
	<asp:HiddenField Visible="false" ID="txtClientTypeCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	<asp:HiddenField Visible="false" ID="txtClientTypeCreatedDate" Value="" runat="server"></asp:HiddenField>

            <div class="col25b">
	<asp:Button ID="btnSaveClientType" runat="server" Text="Save Client Type" CssClass="btn btn-blue" OnClick="btnSaveClientType_Click" />
	<asp:Button ID="btnSaveClientTypeAs" runat="server" Text="Save Client Type As" CssClass="btn btn-blue" Enabled="false" OnClick="btnSaveClientTypeAs_Click" ToolTip="Save a new Client Type based on these settings." />
	<asp:Button ID="btnDeleteClientType" runat="server" CssClass="btn redButton" Enabled="false" Text="Delete Client Type" OnClick="btnDeleteClientType_Click" OnClientClick="return confirm('Are you certain you want to delete this Client Type?');" /><br />
	<asp:Button ID="btnClearClientType" Text="Clear Client Type" CssClass="btn btn-default" ToolTip="If you have already clicked on another Client Type below, you must click this button first before you try to create a new Client Type." runat="server" OnClick="btnClearClientType_Click" />
    </div><br clear="both" />
    
<br />
<asp:Button ID="btnCancelClientTypePopup" OnClick="btnCancelClientTypePopup_Click" CssClass="btn btn-default" runat="server" Text="Close" />
            <br clear="both" /><br /></div></div>
</ContentTemplate></asp:UpdatePanel>
    </asp:Panel>

<ajaxToolkit:ModalPopupExtender runat="server" 
	ID="mpeCarrierTypePopup" 
	TargetControlID="dummy3" 
	PopupControlID="pnlCarrierTypeHolder" 
	BackgroundCssClass="modalBackground"                        
	DropShadow="true"/> 
<input id="dummy3" type="button" style="display: none" runat="server" />
<asp:Panel ID="pnlCarrierTypeHolder" CssClass="modalPopup" runat="server">
<asp:UpdatePanel ID="pnlCarrierTypeModal" runat="server" UpdateMode="Conditional"><ContentTemplate>
<div class="pmt">
	
		<div class="sub-hdr">
			Carrier Types <asp:Label ID="lblCarrierTypeMessage" runat="server"></asp:Label>
			<span class="icon"></span>
		</div>
		<div class="sub-content">
			
			<div class="sub-search-options">
				
				<div class="kw-search">
					
				</div>
                </div>
            <div class="table-container">
				<div class="table">
                    <asp:GridView ID="gvCarrierType" OnSelectedIndexChanged="gvCarrierType_SelectedIndexChanged" OnPageIndexChanging="gvCarrierType_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#edf7ff" CssClass="table-header" />
                    <Columns>
	                <asp:TemplateField HeaderText="Carrier Type">
	                    <ItemTemplate>
		                <asp:HiddenField ID="hdngvCarrierTypeId" Value='<%#Eval("Id") %>' runat="server" /><asp:Label ID="lblgvCarrierType" Text='<%#Eval("CarrierType") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
	                    </ItemTemplate>
	                </asp:TemplateField>
                    </Columns>
                </asp:GridView>


    </div></div>
            <div class="create-master-item">
    <div class="col50">
	<dnn:label ID="lblCarrierType" runat="server" />
	<asp:TextBox ID="txtCarrierType" placeholder="Carrier Name" runat="server" />
    </div><br clear="both" /></div>

	<asp:HiddenField Visible="false" ID="txtSelectedCarrierType" Value="-1" runat="server"></asp:HiddenField>
	<asp:HiddenField Visible="false" ID="txtCarrierTypeCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	<asp:HiddenField Visible="false" ID="txtCarrierTypeCreatedDate" Value="" runat="server"></asp:HiddenField>
            
            <div class="col25b">
	<asp:Button ID="btnSaveCarrierType" runat="server" Text="Save Carrier Type" CssClass="btn btn-blue" OnClick="btnSaveCarrierType_Click" />
	<asp:Button ID="btnSaveCarrierTypeAs" runat="server" Text="Save Carrier Type As" CssClass="btn btn-blue" Enabled="false" OnClick="btnSaveCarrierTypeAs_Click" ToolTip="Save a new Carrier Type based on these settings." />
	<asp:Button ID="btnDeleteCarrierType" runat="server" CssClass="btn redButton" Enabled="false" Text="Delete Carrier Type" OnClick="btnDeleteCarrierType_Click" OnClientClick="return confirm('Are you certain you want to delete this Carrier Type?');" />
	<asp:Button ID="btnClearCarrierType" Text="Clear Carrier Type" CssClass="btn btn-default" ToolTip="If you have already clicked on another Carrier Type below, you must click this button first before you try to create a new Carrier Type." runat="server" OnClick="btnClearCarrierType_Click" />
    </div><br clear="both" />
    
<br />
<asp:Button ID="btnCancelCarrierTypePopup" OnClick="btnCancelCarrierTypePopup_Click" CssClass="btn btn-default" runat="server" Text="Close" /> 
            <br clear="both" /><br /></div></div>
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
                <asp:UpdatePanel ID="pnlFreightPopup" runat="server" UpdateMode="Conditional"><ContentTemplate>
                <div class="pmt">
	
		<div class="sub-hdr">
			Freight Types <asp:Label ID="lblFreightTypeMessage" runat="server"></asp:Label>
			<span class="icon"></span>
		</div>
		<div class="sub-content">
			
			<div class="sub-search-options">
				
				<div class="kw-search">
					
				</div>
                </div>
            <div class="table-container">
				<div class="table">
                    <asp:GridView ID="gvFreightType" OnSelectedIndexChanged="gvFreightType_SelectedIndexChanged" OnPageIndexChanging="gvFreightType_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#edf7ff" CssClass="table-header" />
                    <Columns>
	                <asp:TemplateField HeaderText="Freight Type">
	                    <ItemTemplate>
		                <asp:HiddenField ID="hdngvFreightTypeId" Value='<%#Eval("Id") %>' runat="server" /><asp:Label ID="lblgvFreightType" Text='<%#Eval("FreightType") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
	                    </ItemTemplate>
	                </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                        </div></div>
            <div class="create-master-item">
    <div class="col50">
	                <dnn:label ID="lblFreightType" runat="server" />
	                <asp:TextBox ID="txtFreightType" placeholder="Freight Type" runat="server" />
                    </div><br clear="both" /></div>

	                <asp:HiddenField Visible="false" ID="txtSelectedFreightType" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtFreightTypeCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtFreightTypeCreatedDate" Value="" runat="server"></asp:HiddenField>
            <div class="col25b">
	                <asp:Button ID="btnSaveFreightType" runat="server" CssClass="btn btn-blue" Text="Save Freight Type" OnClick="btnSaveFreightType_Click" />
	                <asp:Button ID="btnSaveFreightTypeAs" runat="server" CssClass="btn btn-blue" Text="Save Freight Type As" Enabled="false" OnClick="btnSaveFreightTypeAs_Click" ToolTip="Save a new Freight Type based on these settings." />
	                <asp:Button ID="btnDeleteFreightType" runat="server" CssClass="btn redButton" Enabled="false" Text="Delete Freight Type" OnClick="btnDeleteFreightType_Click" OnClientClick="return confirm('Are you certain you want to delete this Freight Type?');" />
	                <asp:Button ID="btnClearFreightType" Text="Clear Freight Type" CssClass="btn btn-default" ToolTip="If you have already clicked on another Freight Type below, you must click this button first before you try to create a new Freight Type." runat="server" OnClick="btnClearFreightType_Click" />
                    </div><br clear="both" />
                    
                <br />
                    <asp:Button ID="btnCancelFreightPopup" OnClick="btnCancelFreightPopup_Click" CssClass="btn btn-default" runat="server" Text="Close" /> 
            <br clear="both" /><br /></div></div>
                        </ContentTemplate></asp:UpdatePanel>
                    </asp:Panel>