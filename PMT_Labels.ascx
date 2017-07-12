<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMT_Labels.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.PMT_Labels" %>
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
    /*width:20%;*/
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
   
</style>

<asp:UpdateProgress ID="updLabels" AssociatedUpdatePanelID="pnlLabels" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="/DesktopModules/PMT_Admin/images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<div id="divScreen">
<div id="PMT_Labels">
        <asp:UpdatePanel ID="pnlLabels" runat="server" UpdateMode="Always" visible="true">
            <ContentTemplate>
                <div class="pmt">
	
		<div class="sub-hdr">
			<asp:Label ID="lblLabelMessage" runat="server"></asp:Label>
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
					<asp:TextBox ID="txtLabelSearch" runat="server" EnableViewState="true" AutoPostBack="true" OnTextChanged="txtLabelSearch_TextChanged" placeholder="keyword"></asp:TextBox> <asp:DropDownList ID="ddlLabelAdvertiserSearch" AutoPostBack="true" OnSelectedIndexChanged="ddlLabelAdvertiserSearch_SelectedIndexChanged" runat="server"></asp:DropDownList> <asp:DropDownList ID="ddlLabelAgencySearch" AutoPostBack="true" OnSelectedIndexChanged="ddlLabelAgencySearch_SelectedIndexChanged" runat="server"></asp:DropDownList>
				</div>
	                
				
			</div>
			
			<span class="table-container">
				<span class="table">
                    <asp:GridView ID="gvLabel" OnSelectedIndexChanged="gvLabel_SelectedIndexChanged" OnPageIndexChanging="gvLabel_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#edf7ff" CssClass="table-header" />
                    <Columns>
	                <asp:TemplateField HeaderText="Advertiser">
	                    <ItemTemplate>
		                <asp:HiddenField ID="hdngvLabelId" Value='<%#Eval("Id") %>' runat="server" /><asp:Label ID="lblgvAdvertiserName" Text='<%#Eval("AdvertiserName") %>' runat="server" />
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
                </asp:GridView></span></span>
            <div class="sub-hdr">
			Create/Edit Label
			<span class="icon"></span>
		</div>
		<div class="sub-content create-master-item">
            <div class="editHolder">

                    <div class="col50">
	                <dnn:label ID="lblLabelTapeFormat" runat="server" />
	                <asp:DropDownList ID="ddlLabelTapeFormat" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col50">
                    <dnn:label ID="lblLabelAgency" runat="server" />
                    <asp:DropDownList ID="ddlLabelAgency" runat="server"></asp:DropDownList>
                    </div><br clear="both" />
                    
                    <div class="altRow"><div class="col50"><dnn:label ID="lblLabelAdvertiser" runat="server" />
                    <asp:DropDownList ID="ddlLabelAdvertiser" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col50">
	                <dnn:label ID="lblLabelTitle" runat="server" />
	                <asp:TextBox ID="txtLabelTitle"  runat="server"></asp:TextBox>
                    </div><br clear="both" /></div>
                    <div class="col50">
	                <dnn:label ID="lblLabelDescription" runat="server" />
	                <asp:TextBox ID="txtLabelDescription"  runat="server"></asp:TextBox>
                    </div>   
                    <div class="col50">             
	                <dnn:label ID="lblLabelISCI" runat="server" />
	                <asp:TextBox ID="txtLabelISCI"  runat="server"></asp:TextBox>
                    </div><br clear="both" />
                    <div class="altRow"><div class="col50">    
	                <dnn:label ID="lblLabelPMTMediaId" runat="server" />
	                <asp:TextBox ID="txtLabelPMTMediaId"  runat="server"></asp:TextBox>
                    </div>
                    <div class="col50">
	                <dnn:label ID="lblLabelLength" runat="server" />
	                <asp:TextBox ID="txtLabelLength"  runat="server"></asp:TextBox>
                    </div><br clear="both" /></div>
                    <div class="col50">
                    <dnn:label ID="lblLabelStandard" runat="server" />
                    <asp:DropDownList ID="ddlLabelStandard" runat="server">
                        <asp:ListItem Value="NTSC" Text ="NTSC"></asp:ListItem>
                        <asp:ListItem Value="PAL" Text="PAL"></asp:ListItem>
                    </asp:DropDownList>
                    </div>
                    <div class="col50">
	                <dnn:label ID="lblLabelNotes" runat="server" />
	                <asp:TextBox ID="txtLabelNotes"  runat="server"></asp:TextBox>
                    </div><br clear="both" />
                <div class="altRow"><div class="col50"><dnn:label ID="lblWorkOrderId" runat="server" />
                    <asp:TextBox ID="txtLabelWOId" runat="server"></asp:TextBox>
                    </div>
                    <div class="col50">
	                <dnn:label ID="lblLabelNumber" runat="server" />
	                <asp:TextBox ID="txtLabelNumber"  runat="server"></asp:TextBox>
                    </div><br clear="both" /></div>
                </div>

	                <asp:HiddenField Visible="false" ID="txtSelectedLabel" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtLabelCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtLabelCreatedDate" Value="" runat="server"></asp:HiddenField>
            </div>
	                <div style="width:100%;text-align:center;padding-top:10px;"><div class="col25b"><asp:Button ID="btnSaveLabel" CssClass="btn btn-blue" runat="server" Text="Save Label" OnClick="btnSaveLabel_Click" /></div>
	                <div class="col25b"><asp:Button ID="btnSaveLabelAs" runat="server" Text="Save Label As" Enabled="false" CssClass="btn btn-blue" OnClick="btnSaveLabelAs_Click" ToolTip="Save a new Label based on these settings." /></div>
	                <div class="col25b"><asp:Button ID="btnDeleteLabel" runat="server" CssClass="btn redButton" Enabled="false" Text="Delete Label" OnClick="btnDeleteLabel_Click" OnClientClick="return confirm('Are you certain you want to delete this Label?');" /></div>
	                <div class="col25b"><asp:Button ID="btnClearLabel" Text="Clear Label" CssClass="btn btn-default" ToolTip="If you have already clicked on another Label below, you must click this button first before you try to create a new Label." runat="server" OnClick="btnClearLabel_Click" /></div><br clear="both" /></div>
                    <div class="col25b"><asp:Button ID="btnPrintAll" Text="Print All Labels from WorkOrder" CssClass="btn btn-blue" ToolTip="Prints all labels from WO ID of current search." runat="server" OnClientClick="window.print()" /></div><br clear="both" /></div>
                   

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
            </div></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </div>

<style>
     .LabelA {
        width:587px;
        height:360px;
        page-break-after: always;
        position:relative;
        left:1.25in;
        top:-0.5in;
        font-family: serif;
    }
     .LabelA:last-of-type  {
         page-break-after: auto;
     }
     .LabelB:last-of-type  {
         page-break-after: auto;
     }
    .LabelB {
        /*width:387px;
        height:315px;
        overflow:hidden;
        margin-left: 1.05in;
        font-size:16px;
        margin-top:-0.39in;
        page-break-after: always;*/
        
        width:1200px;
        height:450px;
        overflow:hidden;
        margin-left: -30px;
        font-size:19px;
        font-family: serif;
        margin-top:.375in;
        page-break-after: always;
    }
    .labelBox {
        position:relative;
        overflow:hidden;
        font-size:16px;
        text-align:center;
        line-height:18px;
        height:18px;
        letter-spacing:7px;
    }
    .customerA {
        min-width:209px;
        /*max-width:209px;*/
        top:3px;
        left:246px;
        z-index:10;
        height:auto;
    }
    .buyerA {
        min-width:209px;
        /*max-width:209px;*/
        top:5px;
        left:246px;
        z-index:11;
    }
    .titleA {
        min-width:209px;
        /*max-width:209px;*/
        top:8px;
        left:246px;
        z-index:12;
    }
    .tapecodeA {
        min-width:105px;
        /*max-width:105px;*/
        top:12px;
        left:316px;
        z-index:13;
        float:left;
    }
    .descA {
        min-width:85px;
        /*max-width:85px;*/
        top:12px;
        left:245px;
        z-index:14;
    }
    .isciA {
        min-width:209px;
        /*max-width:209px;*/
        top:15px;
        left:246px;
        z-index:15;
    }
    .masterIdA {
        min-width:250px;
        /*max-width:250px;*/
        top:19px;
        left:317px;
        text-align:left;
        z-index:16;
    }
    .spineA {
        min-width:134px;
        max-width:134px;
        top:2in;
        left:-28px;
        z-index:17;
        height:200px;
        font-size:13px;
        line-height:14px;
        overflow:visible;
        float:left;
        letter-spacing:5px;
    }
    .boxFrontA {
        min-width:262px;
        /*max-width:262px;*/
        top:-.2in;
        left:2.8in;
        z-index:18;
        height:150px;
        line-height:18px;
        font-size:16px;
        overflow:visible;
        float:left;
    }
    .DVDTLB {
        /*min-width:76px;
        max-width:76px;
        top:30px;
        left:24px;
        z-index:14;
        height:60px;
        float:left;
        font-size:14px;
        line-height:14px;*/
        
        min-width:60px;
        max-width:100px;
        top:130px;
        left:270px;
        z-index:14;
        height:80px;
        float:left;
        font-size:16px;
        line-height:18px;
        letter-spacing:4px; 
        overflow:visible;
    }
    .DVDTRB {
        /*min-width:66px;
        max-width:66px;
        top:30px;
        left:243px;
        z-index:15;
        height:60px;
        font-size:14px;
        line-height:14px;*/
        
        min-width:95px;
        max-width:150px;
        top:120px;
        left:730px;
        z-index:15;
        height:100px;
        font-size:16px;
        line-height:17px;  
        letter-spacing:5px;      
    }
    .DVDBottB {
        /*min-width:289px;
        max-width:289px;
        top:120px;
        left:44px;
        z-index:16;
        height:100px;
        font-size:14px;
        line-height:14px;*/
        
        min-width:580px;
	max-width:580px;
	top:193px;
	left:325px;
	z-index:16;
	height:250px;
	font-size:16px;
        line-height:19px;
        letter-spacing:5px;
    }
</style>

<div id="divPrint">
    <asp:UpdatePanel ID="divPrintPanel" runat="server"><ContentTemplate>
    <asp:Panel ID="pnlLabelA" runat="server">
        
    </asp:Panel>
    <asp:Panel ID="pnlLabelB" runat="server">
        
    </asp:Panel>
        </ContentTemplate></asp:UpdatePanel>
</div>