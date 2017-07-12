<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMT_ReportDesigner.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.PMT_ReportDesigner" %>
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

<asp:UpdateProgress ID="updReports" AssociatedUpdatePanelID="pnlReports" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="/DesktopModules/PMT_Admin/images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<div id="PMT_Stations">
        <asp:UpdatePanel ID="pnlReports" runat="server" UpdateMode="Always" visible="true">
            <ContentTemplate>

                <div class="pmt">
	
		<div class="sub-hdr">
			<asp:Label ID="lblReportsMessage" runat="server"></asp:Label>
			<span class="icon"></span>
		</div>
		<div class="sub-content">
			
			<div class="sub-search-options">
				
				<div class="kw-search">
					<span class="icon"></span>
					<asp:TextBox ID="txtReportsSearch" runat="server" EnableViewState="true" AutoPostBack="true" OnTextChanged="txtReportsSearch_TextChanged" placeholder="keyword"></asp:TextBox>
                    <asp:DropDownList ID="ddlReportsAdvertiserSearch" OnSelectedIndexChanged="ddlReportsAdvertiserSearch_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
				    <asp:DropDownList ID="ddlReportsItemAgencySearch" AutoPostBack="true" OnSelectedIndexChanged="ddlReportsItemAgencySearch_SelectedIndexChanged" runat="server"></asp:DropDownList>
                    <asp:DropDownList ID="ddlReportTypeSearch" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlReportTypeSearch_SelectedIndexChanged">
                        <asp:ListItem Text="--Select Report Type--"></asp:ListItem>
                        <asp:ListItem Text="Master Items" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Library Items" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Delivery" Value="3"></asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:CheckBox ID="chkShowMine" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowMine_CheckedChanged" Text="Show My Reports Only" />
				</div>
	                
				
			</div>
			
			<span class="table-container">
				<span class="table">
                    <asp:GridView ID="gvReports" OnSelectedIndexChanged="gvReports_SelectedIndexChanged" OnPageIndexChanging="gvReports_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#edf7ff" CssClass="table-header" />
                        <Columns>
	                    <asp:TemplateField HeaderText="Name">
	                        <ItemTemplate>
		                    <span class="cell"><asp:HiddenField ID="hdngvStationId" Value='<%#Eval("Id") %>' runat="server" /><asp:Label ID="lblgvReportName" Text='<%#Eval("ReportName") %>' runat="server" /></span>
	                        </ItemTemplate>
	                    </asp:TemplateField>
                            <asp:TemplateField HeaderText="Report Type">
	                        <ItemTemplate>
		                    <span class="cell"><%# (Eval("ReportType").ToString()=="1") ? "Master Items" : "" %><%# (Eval("ReportType").ToString()=="2") ? "Library Items" : "" %><%# (Eval("ReportType").ToString()=="3") ? "Delivery" : "" %></span>
	                        </ItemTemplate>
	                    </asp:TemplateField>
                            <asp:TemplateField HeaderText="Frequency">
	                        <ItemTemplate>
		                    <span class="cell"><asp:Label ID="lblgvReportFrequency" Text='<%#Eval("Frequency") %>' runat="server" /></span>
	                        </ItemTemplate>
	                    </asp:TemplateField>
                            <asp:TemplateField HeaderText="Active">
	                        <ItemTemplate>
		                    <span class="cell"><%# Boolean.Parse(Eval("isActive").ToString()) ? "Yes" : "No" %></span>
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
			Create/Edit Report
			<span class="icon"></span>
		</div>
		<div class="sub-content create-master-item">
            <div class="editHolder">

                 <div class="col25">
	            <dnn:label ID="lblReportType" runat="server" />
	            <asp:DropDownList ID="ddlReportType" runat="server">
                    <asp:ListItem Text="--Select Report Type--"></asp:ListItem>
                        <asp:ListItem Text="Master Items" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Library Items" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Delivery" Value="3"></asp:ListItem>
	            </asp:DropDownList>
                </div>
                <div class="col25">
	            <dnn:label ID="lblReportAdvertiser" runat="server" />
	            <asp:DropDownList ID="ddlReportAdvertiser" runat="server"></asp:DropDownList>
                </div>
                <div class="col25">
                    <dnn:label ID="lblReportAgency" runat="server" />
                    <asp:DropDownList ID="ddlReportAgency" runat="server"></asp:DropDownList>
                </div>
                <br clear="both" />
                    <div class="altRow">             
                <div class="col25">
	            <dnn:label ID="lblReportKeyword" runat="server" />
	            <asp:TextBox ID="txtReportKeyword" placeholder="Keyword" runat="server" />
                </div>
                        <div class="col25">
	            <dnn:label ID="lblReportStatus" runat="server" />
	            <asp:DropDownList ID="ddlReportStatus" runat="server">
                    <asp:ListItem Text="--Select Status--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="New" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Approved" Value="3"></asp:ListItem>
	            </asp:DropDownList>
                </div>
                        <div class="col25">
	            <dnn:label ID="lblReportFrequency" runat="server" />
	            <asp:DropDownList ID="ddlReportFrequency" runat="server">
                    <asp:ListItem Text="--Select Frequency--" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Daily" Value="daily"></asp:ListItem>
                    <asp:ListItem Text="Weekly" Value="weekly"></asp:ListItem>
                    <asp:ListItem Text="Monthly" Value="monthly"></asp:ListItem>
	            </asp:DropDownList>
                </div><br clear="both" /></div>

                <div class="col25">
                    <dnn:label ID="lblReportStartDate" runat="server"></dnn:label>
                    <asp:TextBox ID="txtReportStartDate" runat="server"></asp:TextBox><ajaxToolkit:CalendarExtender ID="CalendarExtender" runat="server" TargetControlID="txtReportStartDate"></ajaxToolkit:CalendarExtender>
                </div>
                <div class="col25">
                    <dnn:label id="lblReportName" runat="server"></dnn:label>
                    <asp:TextBox ID="txtReportName" runat="server"></asp:TextBox>
                </div>
                <div class="col25">
                    <dnn:label id="lblReportActive" runat="server"></dnn:label>
                    <asp:CheckBox ID="chkReportActive" runat="server" Text="Active" />
                </div><br clear="both" />

                <div class="altRow"> 
                    <div class="col25">
                        <dnn:label id="lblReportEmailTo" runat="server"></dnn:label>
                        <asp:TextBox ID="txtReportEmailTo" runat="server"></asp:TextBox>
                    </div>
                    <div class="col50">
                        <dnn:label id="lblEmailMessage" runat="server"></dnn:label>
                        <asp:TextBox ID="txtEmailMessage" runat="server" Text="Dear Customer,&#013;&#010;&#013;&#010;Pacific Media Technologies, Inc. is pleased to send you this [report_frequency] [report_type] report entitled [report_name] for [report_date].&#013;&#010;&#013;&#010;[link]&#013;&#010;&#013;&#010;Thank you" Rows="8" TextMode="MultiLine"></asp:TextBox>
                    </div>
                <br clear="both" /></div>

                </div>

	            <asp:HiddenField Visible="false" ID="txtSelectedReport" Value="-1" runat="server"></asp:HiddenField>
	            <asp:HiddenField Visible="false" ID="txtReportCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	            <asp:HiddenField Visible="false" ID="txtReportCreatedDate" Value="" runat="server"></asp:HiddenField>
            </div>
	            <div style="width:100%;text-align:center;padding-top:10px;"><div class="col25b"><asp:Button ID="btnSaveReport" CssClass="btn btn-blue" runat="server" Text="Save Report" OnClick="btnSaveReport_Click" /></div>
	            <div class="col25b"><asp:Button ID="btnSaveReportAs" runat="server" Text="Save Report As" CssClass="btn btn-blue" Enabled="false" OnClick="btnSaveReportAs_Click" ToolTip="Save a new Report based on these settings." /></div>
	            <div class="col25b"><asp:Button ID="btnDeleteReport" runat="server" CssClass="btn redButton" Enabled="false" Text="Delete Report" OnClick="btnDeleteReport_Click" OnClientClick="return confirm('Are you certain you want to delete this Report?');" /></div>
	            <div class="col25b"><asp:Button ID="btnClearReport" Text="Clear Report" CssClass="btn btn-default" ToolTip="If you have already clicked on another Report below, you must click this button first before you try to create a new Report." runat="server" OnClick="btnClearReport_Click" /></div>
                <div class="col25b"><asp:Button ID="btnSendNow" Text="Send Now" Enabled="false" ToolTip="Send Report Now" CssClass="btn btn-blue" OnClick="btnSendNow_Click" runat="server" /></div>
                    <div class="col25b"><asp:Button ID="btnSendWithEmail" Text="Send Now With Email" Enabled="false" ToolTip="Send Report Now with Email" CssClass="btn btn-blue" OnClick="btnSendWithEmail_Click" runat="server" /></div>
                    <br clear="both" /></div>
            </div></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div><br clear="both" />