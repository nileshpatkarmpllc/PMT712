<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMT_Stations.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.PMT_Stations" %>
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
.col25b input, .col25b select, .col25b textarea {
    width:100% !important;
}
.pmt .create-master-item textarea {
    
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
    margin-right:5px;
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
        max-height:110px;
        overflow-y:scroll;
        overflow-x:hidden;
        max-width:300px;
        background-color:white;
        margin-bottom:10px;
        border:solid 1px #dedede;
        padding:10px;
        float:left;
        margin-right:10px;
    }
    .pmt-SelectList input {
        margin-right:10px;
    }
    .pmt-SelectList input[type="checkbox"] {
        width:10%;
    }
    .pmt .create-master-item input[type="checkbox"] {
        width: 5%;
        text-align:left;
    }
</style>

<asp:UpdateProgress ID="updStations" AssociatedUpdatePanelID="pnlStations" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="/DesktopModules/PMT_Admin/images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<div id="PMT_Stations">
        <asp:UpdatePanel ID="pnlStations" runat="server" UpdateMode="Always" visible="true">
            <ContentTemplate>

                <div class="pmt">
	
		<div class="sub-hdr">
			<asp:Label ID="lblStationMessage" runat="server"></asp:Label>
			<span class="icon"></span>
		</div>
		<div class="sub-content">
			
			<div class="sub-search-options">
				
				<div class="kw-search">
					<span class="icon"></span>
					<asp:TextBox ID="txtStationSearch" runat="server" EnableViewState="true" AutoPostBack="true" OnTextChanged="txtStationSearch_TextChanged" placeholder="keyword"></asp:TextBox> <asp:DropDownList ID="ddlStationMarketSearch" AutoPostBack="true" OnSelectedIndexChanged="ddlStationMarketSearch_SelectedIndexChanged" runat="server"></asp:DropDownList>
                    <asp:DropDownList ID="ddlProgramFormatSearch" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlProgramFormatSearch_SelectedIndexChanged">
                        <asp:ListItem Text="--Select Program Format--"></asp:ListItem>
                        <asp:ListItem Text="Short Form" Value="Short Form"></asp:ListItem>
                        <asp:ListItem Text="Long Form" Value="Long Form"></asp:ListItem>
                    </asp:DropDownList>
				</div>
	                
				
			</div>
			
			<span class="table-container">
				<span class="table">
                    <asp:GridView ID="gvStation" OnSelectedIndexChanged="gvStation_SelectedIndexChanged" OnPageIndexChanging="gvStation_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#edf7ff" CssClass="table-header" />
                        <Columns>
	                    <asp:TemplateField HeaderText="Name">
	                        <ItemTemplate>
		                    <span class="cell"><asp:HiddenField ID="hdngvStationId" Value='<%#Eval("Id") %>' runat="server" /><asp:Label ID="lblgvStationName" Text='<%#Eval("StationName") %>' runat="server" /></span>
	                        </ItemTemplate>
	                    </asp:TemplateField>
                            <asp:TemplateField HeaderText="Call Letters">
	                        <ItemTemplate>
		                    <span class="cell"><asp:Label ID="lblgvStationCallLetters" Text='<%#Eval("CallLetter") %>' runat="server" /></span>
	                        </ItemTemplate>
	                    </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="Program Format">
	                        <ItemTemplate>
		                    <span class="cell"><asp:Label ID="lblgvStationProgramFormat" Text='<%#Eval("ProgramFormat") %>' runat="server" /></span>
	                        </ItemTemplate>
	                    </asp:TemplateField>
	                    <asp:TemplateField>
	                        <ItemTemplate>
		                    <span class="cell"><asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" /></span>
	                        </ItemTemplate>
	                    </asp:TemplateField>
                        </Columns>
                    </asp:GridView></span></span>
            <div class="sub-hdr">
			Create/Edit Station
			<span class="icon"></span>
		</div>
		<div class="sub-content create-master-item">
            <div class="editHolder">

                 <div class="col50">
	            <dnn:label ID="lblStationName" runat="server" />
	            <asp:TextBox ID="txtStationName" placeholder="Station Name" runat="server" />
                </div>
                <div class="col50">
	            <dnn:label ID="lblStationMarket" runat="server" />
	            <asp:DropDownList ID="ddlStationMarket" runat="server"></asp:DropDownList> <asp:Button id="btnAddStationMarket" CssClass="btnAddEdit btn-blue" runat="server" Text="Add/Edit" OnClick="btnAddStationMarket_Click" />
                </div>
                <br clear="both" />
                    <div class="altRow">             
                <div class="col25">
	            <dnn:label ID="lblStationCallLetters" runat="server" />
	            <asp:TextBox ID="txtStationCallLetters" placeholder="Call Letters" runat="server" />
                </div>
                        <div class="col25">
	            <dnn:label ID="lblStationContact" runat="server" />
	            <asp:TextBox ID="txtStationContact" placeholder="Station Contact" runat="server" />
                </div>
                        <div class="col25">
	            <dnn:label ID="lblProgramFormat" runat="server" />
	            <asp:DropDownList ID="ddlStationProgramFormat" runat="server">
                    <asp:ListItem Text="Short Form" Value="Short Form" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Long Form" Value="Long Form"></asp:ListItem>
	            </asp:DropDownList>
                </div><br clear="both" /></div>
                <div class="col50">
                    <dnn:label ID="lblStationOTSMHD" runat="server"></dnn:label>
                    <asp:TextBox ID="txtStationOTSMHD" runat="server" placeholder="On The Spot HD Call Letters"></asp:TextBox>
                </div>
                <div class="col50">
                    <dnn:label ID="lblStationOTSMSD" runat="server"></dnn:label>
                    <asp:TextBox ID="txtStationOTSMSD" runat="server" placeholder="On The Spot SD Call Letters"></asp:TextBox>
                </div><br clear="both" />
                <div class="altRow">
                    <div class="col50">
                    <dnn:label ID="lblAdDeliveryCallLetters" runat="server"></dnn:label>
                    <asp:TextBox ID="txtAdDeliveryCallLetters" runat="server" placeholder="Ad Delivery Call Letters"></asp:TextBox>
                </div>
                    <div class="col50">
                    <dnn:label ID="lblJavelinCallLetters" runat="server"></dnn:label>
                    <asp:TextBox ID="txtJavelinCallLetters" runat="server" placeholder="Javelin Call Letters"></asp:TextBox>
                </div><br clear="both" />
                </div>

                <div class="col25">
	            <dnn:label ID="lblStationMediaType" runat="server" />
	            <asp:DropDownList ID="ddlStationMediaType" runat="server">
                    <asp:ListItem Value="-1" Text="--Please Select--"></asp:ListItem>
                    <asp:ListItem Value="1" Text="ADDELIVERY YES" Enabled="false"></asp:ListItem>
                    <asp:ListItem Value="2" Text="HD"></asp:ListItem>
                    <asp:ListItem Value="3" Text="HD & SD"></asp:ListItem>
                    <asp:ListItem Value="4" Text="HD & SD (BACKUP REQUIRED)" Enabled="false"></asp:ListItem>
                    <asp:ListItem Value="5" Text="SD"></asp:ListItem>
	            </asp:DropDownList>
                </div>    
                <div class="col25">
                    <dnn:label ID="lblBackupRequired" runat="server" />
                    <asp:CheckBox ID="chkBackupRequired" runat="server" />
                </div>   
                <div class="col25">
	            <dnn:label ID="lblStationTapeFormat" runat="server" />
	            <div class="pmt-SelectList"><asp:CheckBoxList ID="lbxStationTapeFormat" runat="server" Rows="6" SelectionMode="Multiple"></asp:CheckBoxList></div> <asp:Button id="btnAddStationTapeFormat" CssClass="btnAddEdit btn-blue" runat="server" Text="Add/Edit" OnClick="btnAddStationTapeFormat_Click" />
                </div><br clear="both" />
                <div class="altRow"><div class="col25">
                <dnn:label ID="lblStationAddress1" runat="server" />
                <asp:TextBox ID="txtStationAddress1" placeholder="Address1" runat="server" />
                </div>
                <div class="col25">
                <dnn:label ID="lblStationAddress2" runat="server" />
                <asp:TextBox ID="txtStationAddress2" placeholder="Address2" runat="server" />
                </div>
                <div class="col25">
                <dnn:label ID="lblStationCity" runat="server" />
                <asp:TextBox ID="txtStationCity" placeholder="City" runat="server" />
                </div><br clear="both" /></div>
                <div class="col25">
                <dnn:label ID="lblStationState" runat="server" />
                <asp:TextBox ID="txtStationState" placeholder="State/Province" runat="server" />
                </div>
                <div class="col25">
                <dnn:label ID="lblStationZip" runat="server" />
                <asp:TextBox ID="txtStationZip" placeholder="Zip/PostalCode" runat="server" />
                </div>
                <div class="col25">
                <dnn:label ID="lblStationCountry" runat="server" />
                <asp:TextBox ID="txtStationCountry" placeholder="Country" runat="server" />
                </div><br clear="both" />
                <div class="altRow"><div class="col25">
                <dnn:label ID="lblStationPhone" runat="server" />
                <asp:TextBox ID="txtStationPhone" placeholder="Phone" runat="server" />
                </div>
                <div class="col25">
                <dnn:label ID="lblStationFax" runat="server" />
                <asp:TextBox ID="txtStationFax" placeholder="Fax" runat="server" />
                </div>
                <div class="col25">
                <dnn:label ID="lblStationEmail" runat="server" />
                <asp:TextBox ID="txtStationEmail" placeholder="Email" runat="server" />
                </div><br clear="both" /></div>
                <div class="col50">
                <dnn:label ID="lblStationSpecialInstruction" runat="server" />
                <asp:TextBox ID="txtStationSpecialInstruction" TextMode="MultiLine" Rows="6" placeholder="Special Instructions" runat="server" />
                </div>
                <div class="col50">
                    <dnn:label ID="lblStationDeliveryMethod" runat="server" />
                    <div class="pmt-SelectList"><asp:CheckBoxList ID="lbxStationDeliveryMethod" runat="server" Rows="6" SelectionMode="Multiple"></asp:CheckBoxList></div> <asp:Button id="btnAddStationDeliveryMethod" CssClass="btnAddEdit btn-blue" runat="server" Text="Add/Edit" OnClick="btnAddStationDeliveryMethod_Click" />
                </div><br clear="both" />
                <div class="altRow"><div class="col25">
	            <dnn:label ID="lblStationOnline" runat="server" />
	            <asp:DropDownList ID="ddlStationOnline" runat="server">
                    <asp:ListItem Value="-1" Text="--Please Select--"></asp:ListItem>
                    <asp:ListItem Value="1" Text="YES"></asp:ListItem>
                    <asp:ListItem Value="0" Text="NO"></asp:ListItem>
	            </asp:DropDownList>
                </div>
                <div class="col25">
                <dnn:label ID="lblStationStatus" runat="server" />
                <asp:DropDownList ID="ddlStationStatus" runat="server">
                    <asp:ListItem Text="--Please Select--" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                    <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                </asp:DropDownList>
                </div>
                <div class="col25">
                <dnn:label ID="lblStationAttentionLine" runat="server" />
                <asp:TextBox ID="txtStationAttentionLine" placeholder="Attention Line" runat="server" />
                </div><br clear="both" /></div>
                </div>

	            <asp:HiddenField Visible="false" ID="txtSelectedStation" Value="-1" runat="server"></asp:HiddenField>
	            <asp:HiddenField Visible="false" ID="txtStationCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	            <asp:HiddenField Visible="false" ID="txtStationCreatedDate" Value="" runat="server"></asp:HiddenField>
            </div>
	            <div style="width:100%;text-align:center;padding-top:10px;"><div class="col25b"><asp:Button ID="btnSaveStation" CssClass="btn btn-blue" runat="server" Text="Save Station" OnClick="btnSaveStation_Click" /></div>
	            <div class="col25b"><asp:Button ID="btnSaveStationAs" runat="server" Text="Save Station As" CssClass="btn btn-blue" Enabled="false" OnClick="btnSaveStationAs_Click" ToolTip="Save a new Station based on these settings." /></div>
	            <div class="col25b"><asp:Button ID="btnDeleteStation" runat="server" CssClass="btn redButton" Enabled="false" Text="Delete Station" OnClick="btnDeleteStation_Click" OnClientClick="return confirm('Are you certain you want to delete this Station? NOTE: Deleting a station may make old orders invalid. It is recommended that you set this Station to Inactive Status rather than delete it.');" /></div>
	            <div class="col25b"><asp:Button ID="btnClearStation" Text="Clear Station" CssClass="btn btn-default" ToolTip="If you have already clicked on another Station below, you must click this button first before you try to create a new Station." runat="server" OnClick="btnClearStation_Click" /></div>
                    <div class="col25b"><asp:Button ID="btnCreateEzPostAddress" Text="Create EasyPost Address" CssClass="btn btn-blue" runat="server" OnClick="btnCreateEzPostAddress_Click" /></div><br clear="both" />
	            </div>
            </div></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div><br clear="both" />

<ajaxToolkit:ModalPopupExtender runat="server" 
	ID="mpeMarket2Popup" 
	TargetControlID="dummysm2" 
	PopupControlID="pnlMarket2Holder" 
	BackgroundCssClass="modalBackground"                        
	DropShadow="true"/> 
<input id="dummysm2" type="button" style="display: none" runat="server" />
<asp:Panel ID="pnlMarket2Holder" CssClass="modalPopup" runat="server">

    <asp:UpdatePanel ID="pnlMarket2Modal" runat="server" UpdateMode="Always"><ContentTemplate>
	<div class="pmt">
	
		<div class="sub-hdr">
			Markets <asp:Label ID="lblMarket2Message" runat="server"></asp:Label>
			<span class="icon"></span>
		</div>
		<div class="sub-content">
			
			<div class="sub-search-options">
				
				<div class="kw-search">
					<span class="icon"></span>
                    <asp:TextBox ID="txtMarket2Search" placeholder="Search" AutoPostBack="true" OnTextChanged="txtMarket2Search_TextChanged" runat="server"></asp:TextBox>
				</div>
                </div>
            <div class="table-container">
				<div class="table">
                    <asp:GridView ID="gvMarket2" OnSelectedIndexChanged="gvMarket2_SelectedIndexChanged" OnPageIndexChanging="gvMarket2_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#edf7ff" CssClass="table-header" />
	                    <Columns>
		                <asp:TemplateField HeaderText="Name">
		                    <ItemTemplate>
			                <asp:HiddenField ID="hdngvMarket2Id" Value='<%#Eval("Id") %>' runat="server" /><asp:Label ID="lblgvMarketName" Text='<%#Eval("MarketName") %>' runat="server" />
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
		<dnn:label ID="lblMarket2Name" runat="server" />
		<asp:TextBox ID="txtMarket2Name" placeholder="Market Name" runat="server" />
	    </div>
	    <div class="col50">
		<dnn:label ID="lblMarket2Description" runat="server" />
		<asp:TextBox ID="txtMarket2Description" TextMode="MultiLine" Rows="6" placeholder="Description" runat="server" />
	    </div><br clear="both" />
	    <div class="altRow" style="display:none;"><div class="col50">
		<dnn:label ID="lblMarket2Parent" runat="server" />
		<asp:DropDownList ID="ddlMarket2Parent" runat="server"></asp:DropDownList>
	    </div><br clear="both" /></div>


		<asp:HiddenField Visible="false" ID="txtSelectedMarket2" Value="-1" runat="server"></asp:HiddenField>
		<asp:HiddenField Visible="false" ID="txtMarket2CreatedBy" Value="-1" runat="server"></asp:HiddenField>
		<asp:HiddenField Visible="false" ID="txtMarket2CreatedDate" Value="" runat="server"></asp:HiddenField>
                
		<div class="col25b"><asp:Button ID="btnSaveMarket2" runat="server" CssClass="btn btn-blue" Text="Save Market" OnClick="btnSaveMarket2_Click" /></div>
		<div class="col25b"><asp:Button ID="btnSaveMarket2As" runat="server" CssClass="btn btn-blue" Text="Save Market As" Enabled="false" OnClick="btnSaveMarket2As_Click" ToolTip="Save a new Market based on these settings." /></div>
		<div class="col25b"><asp:Button ID="btnDeleteMarket2" runat="server" CssClass="btn redButton" Enabled="false" Text="Delete Market" OnClick="btnDeleteMarket2_Click" OnClientClick="return confirm('Are you certain you want to delete this Market?');" /></div>
		<div class="col25b"><asp:Button ID="btnClearMarket2" Text="Clear Market" CssClass="btn btn-default" ToolTip="If you have already clicked on another Market below, you must click this button first before you try to create a new Market." runat="server" OnClick="btnClearMarket2_Click" /></div><br clear="both" />
	    
	    
	</div><br clear="both" />
    
<br />
    <asp:Button ID="btnCancelMarket2Popup" OnClick="btnCancelMarket2Popup_Click" CssClass="btn btn-default" runat="server" Text="Close" /> 
            <br clear="both" /><br /></div></div>
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
            <asp:UpdatePanel ID="pnlTapeFormatPopup" runat="server" UpdateMode="Conditional"><ContentTemplate>
        <div class="pmt">
	
		<div class="sub-hdr">
			Tape Formats <asp:Label ID="lblTapeFormatMessage" runat="server"></asp:Label>
			<span class="icon"></span>
		</div>
		<div class="sub-content">
			
			<div class="sub-search-options">
				
				<div class="kw-search">
				</div>
                </div>
            <div class="table-container">
				<div class="table">
                    <asp:GridView ID="gvTapeFormat" OnSelectedIndexChanged="gvTapeFormat_SelectedIndexChanged" OnPageIndexChanging="gvTapeFormat_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#edf7ff" CssClass="table-header" />
                        <Columns>
	                    <asp:TemplateField HeaderText="Tape Format">
	                        <ItemTemplate>
		                    <asp:HiddenField ID="hdngvTapeFormatId" Value='<%#Eval("Id") %>' runat="server" /><asp:Label ID="lblgvTapeFormat" Text='<%#Eval("TapeFormat") %>' runat="server" />
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
                            <asp:TemplateField HeaderText="Weight">
	                        <ItemTemplate>
		                    <asp:Label ID="lblgvTapeFormatWeight" Text='<%#Eval("Weight") %>' runat="server" />
	                        </ItemTemplate>
	                    </asp:TemplateField>
                            <asp:TemplateField HeaderText="MaxPerPak">
	                        <ItemTemplate>
		                    <asp:Label ID="lblgvTapeFormatMaxPerPak" Text='<%#Eval("MaxPerPak") %>' runat="server" />
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
	        <dnn:label ID="lblTapeFormat" runat="server" />
	        <asp:TextBox ID="txtTapeFormat" placeholder="Tape Format" runat="server" />
            </div>
            <div class="col50">
	        <dnn:label ID="lblTapePrinter" runat="server" />
	        <asp:DropDownList ID="ddlTapePrinter" runat="server">
                <asp:ListItem Text="--Please Select--" Value="-1"></asp:ListItem>
                <asp:ListItem Text="Printer A" Value="A"></asp:ListItem>
                <asp:ListItem Text="Printer B" Value="B"></asp:ListItem>
	        </asp:DropDownList>
            </div><br clear="both" />
            <div class="altRow"><div class="col50">
	        <dnn:label ID="lblTapeLabel" runat="server" />
	        <asp:DropDownList ID="ddlTapeLabel" runat="server">
                <asp:ListItem Text="--Please Select--" Value="-1"></asp:ListItem>
                <asp:ListItem Text="Label A" Value="A"></asp:ListItem>
                <asp:ListItem Text ="Label B" Value="B"></asp:ListItem>
                <asp:ListItem Text="Label C" Value="C"></asp:ListItem>
	        </asp:DropDownList>
            </div><br clear="both" /></div>
                <div class="col50">
	        <dnn:label ID="lblTapeWeight" runat="server" />
	        <asp:TextBox ID="txtTapeWeight" placeholder="Tape Weight" runat="server" />
            </div>
            <div class="col50">
	        <dnn:label ID="lblTapeMaxPerPak" runat="server" />
	        <asp:TextBox ID="txtTapeMaxPerPak" placeholder="Tape Max/Pak" runat="server" />
            </div><br clear="both" />


	        <asp:HiddenField Visible="false" ID="txtSelectedTapeFormat" Value="-1" runat="server"></asp:HiddenField>
	        <asp:HiddenField Visible="false" ID="txtTapeFormatCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	        <asp:HiddenField Visible="false" ID="txtTapeFormatCreatedDate" Value="" runat="server"></asp:HiddenField>


	        <div class="col25b"><asp:Button ID="btnSaveTapeFormat" CssClass="btn btn-blue" runat="server" Text="Save Tape Format" OnClick="btnSaveTapeFormat_Click" /></div>
	        <div class="col25b"><asp:Button ID="btnSaveTapeFormatAs" CssClass="btn btn-blue" runat="server" Text="Save Tape Format As" Enabled="false" OnClick="btnSaveTapeFormatAs_Click" ToolTip="Save a new Tape Format based on these settings." /></div>
	        <div class="col25b"><asp:Button ID="btnDeleteTapeFormat" runat="server" CssClass="btn redButton" Enabled="false" Text="Delete Tape Format" OnClick="btnDeleteTapeFormat_Click" OnClientClick="return confirm('Are you certain you want to delete this Tape Format?');" /></div>
	        <div class="col25b"><asp:Button ID="btnClearTapeFormat" CssClass="btn btn-default" Text="Clear Tape Format" ToolTip="If you have already clicked on another Tape Format below, you must click this button first before you try to create a new Tape Format." runat="server" OnClick="btnClearTapeFormat_Click" /></div><br clear="both" />
	    
	    
	</div><br clear="both" />
    
<br />
            <asp:Button ID="btnCancelTapeFormatPopup" CssClass="btn btn-default" OnClick="btnCancelTapeFormatPopup_Click" runat="server" Text="Close" /> 
            <br clear="both" /><br /></div></div>
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
                
                    <asp:UpdatePanel ID="pnlDeliveryMethodPopup" runat="server" UpdateMode="Conditional"><ContentTemplate>
                        <div class="pmt">
	
		<div class="sub-hdr">
			Delivery Methods <asp:Label ID="lblDeliveryMethodMessage" runat="server"></asp:Label>
			<span class="icon"></span>
		</div>
		<div class="sub-content">
			
			<div class="sub-search-options">
				
				<div class="kw-search">
				</div>
                </div>
            <div class="table-container">
				<div class="table">
                    <asp:GridView ID="gvDeliveryMethod" OnSelectedIndexChanged="gvDeliveryMethod_SelectedIndexChanged" OnPageIndexChanging="gvDeliveryMethod_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#edf7ff" CssClass="table-header" />
                    <Columns>
	                <asp:TemplateField HeaderText="DeliveryMethod Type">
	                    <ItemTemplate>
		                <asp:HiddenField ID="hdngvDeliveryMethodId" Value='<%#Eval("Id") %>' runat="server" /><asp:Label ID="lblgvDeliveryMethod" Text='<%#Eval("DeliveryMethod") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="Priority">
	                    <ItemTemplate>
		                <asp:Label ID="lblgvDeliveryMethodPriority" Text='<%#Eval("Priority") %>' runat="server" />
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
	                <dnn:label ID="lblDeliveryMethod" runat="server" />
	                <asp:TextBox ID="txtDeliveryMethod" placeholder="Delivery Method" runat="server" />
                    </div>
                    <div class="col50">
	                <dnn:label ID="lblDeliveryPriority" runat="server" />
	                <asp:TextBox ID="txtDeliveryPriority" placeholder="Priority" runat="server" />
                    </div><br clear="both" />

	                <asp:HiddenField Visible="false" ID="txtSelectedDeliveryMethod" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtDeliveryMethodCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtDeliveryMethodCreatedDate" Value="" runat="server"></asp:HiddenField>

	                <div class="col25b"><asp:Button ID="btnSaveDeliveryMethod" runat="server" CssClass="btn btn-blue" Text="Save Delivery Method" OnClick="btnSaveDeliveryMethod_Click" /></div>
	                <div class="col25b"><asp:Button ID="btnSaveDeliveryMethodAs" runat="server" CssClass="btn btn-blue" Text="Save Delivery Method As" Enabled="false" OnClick="btnSaveDeliveryMethodAs_Click" ToolTip="Save a new Delivery Method based on these settings." /></div>
	                <div class="col25b"><asp:Button ID="btnDeleteDeliveryMethod" runat="server" CssClass="btn redButton" Enabled="false" Text="Delete Delivery Method" OnClick="btnDeleteDeliveryMethod_Click" OnClientClick="return confirm('Are you certain you want to delete this Delivery Method?');" /></div>
	                <div class="col25b"><asp:Button ID="btnClearDeliveryMethod" Text="Clear Delivery Method" CssClass="btn btn-default" ToolTip="If you have already clicked on another Delivery Method below, you must click this button first before you try to create a new Delivery Method." runat="server" OnClick="btnClearDeliveryMethod_Click" /></div><br clear="both" />
	    
	    
	</div><br clear="both" />
    
<br />
                    <asp:Button ID="btnCancelDeliveryMethodPopup" OnClick="btnCancelDeliveryMethodPopup_Click" CssClass="btn btn-default" runat="server" Text="Close" /> 
            <br clear="both" /><br /></div></div>
                        </ContentTemplate></asp:UpdatePanel>
                    </asp:Panel>