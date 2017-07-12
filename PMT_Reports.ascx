<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMT_Reports.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.PMT_Reports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %> 
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ register tagprefix="dnn" tagname="Label" src="~/controls/LabelControl.ascx" %>

<dnn:DnnCssInclude ID="DnnCssInclude3" runat="server" FilePath="/DesktopModules/PMT_Admin/styles/style.css" Priority="11" />

<style>
@import url(https://fonts.googleapis.com/css?family=Roboto:400,500);

.pmt-media-master {
	font-family:'Roboto', sans-serif;
}

.pmt-media-master .group-hdr {
	position:relative;
	padding:5px 30px;
	color:#FFF;
	text-transform:uppercase;
	background:#6291fc;
	-webkit-box-shadow: 0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	-moz-box-shadow:    0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	box-shadow:         0px 2px 5px 0px rgba(50, 50, 50, 0.25);
}
.pmt-media-master .group-content {
	padding:15px;
	background:#f7f7f7;
	-webkit-box-shadow: 0px 1px 2px 0px rgba(50, 50, 50, 0.50);
	-moz-box-shadow:    0px 1px 2px 0px rgba(50, 50, 50, 0.50);
	box-shadow:         0px 1px 2px 0px rgba(50, 50, 50, 0.50);
}

.pmt-media-master .sub-hdr {
	position:relative;
	padding:5px 10px;
	color:#3576be;
	text-transform:uppercase;
	-webkit-box-shadow: 0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	-moz-box-shadow:    0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	box-shadow:         0px 2px 5px 0px rgba(50, 50, 50, 0.25);
}
.pmt-media-master .sub-content{
	background:#FFF;
	overflow:hidden;
}
.pmt-media-master .sub-search-options {
	padding:10px 20px;
}


.pmt-media-master .table-container{
	/*max-height:210px;
	overflow-y: scroll;*/
}
.pmt-media-master .sub-search-options select {
	width:25%;
    border:solid 1px #cccccc;
}
.pmt-media-master .sub-search-options .action-links {
	float:right;
}
.pmt-media-master .sub-search-options .action-links a {
	display:inline-block;
	padding:5px 10px; 
	font-size:12px;
	color:#666;
	cursor:pointer;
}
.pmt-media-master .btn.non-deliverable{
	margin:15px;
	float:right;
}
.pmt-media-master .pagination {
	margin:20px 0;
	color:#666;
	font-size:11px;
	list-style-type:none;
}
.pmt-media-master .pagination td {
	display:inline-block;
	padding:3px 5px;
}
.pmt-media-master .pagination li.cur {
	color:#FFF;
	background:#666;
}
.pmt-media-master .create-master-item {
	padding:15px 0; 
	background:#f0f0f0;
	color:#666;
	font-size:12px;
}
.pmt-media-master .create-master-item .col {
	padding: 1% 2%;
	display:inline-block;
	vertical-align:top;
}
.pmt-media-master .create-master-item label {
	text-transform:uppercase;
	color:#666;
	font-size:12px;
}
.pmt-media-master .create-master-item input, .pmt-media-master .create-master-item select {
	width:48%;
}
.pmt-media-master .create-master-item .checkbox {
	margin:0 20px 0 0;
}
.pmt-media-master .create-master-item .checkbox input {
	width:auto;
}
.pmt-media-master .create-master-item .full {
	width:100%;
}
.pmt-media-master .create-master-item .col25 {
	width:49%;
}
.pmt-media-master .create-master-item .col33 {
	width:28%;
}
.pmt-media-master .create-master-item .col50 {
	width:45%;
}
.pmt-media-master .create-master-item .col75 {
	width:50%;
}
.pmt-media-master .create-master-item .clear {
	padding:0;
	clear:both;
	display:block;
}
.pmt-media-master .cmi-col {
	display:inline-block;
	padding:0 1%;
	width:47%;
	vertical-align:top;
}
.pmt-media-master .create-master-item .bottom {
	color:#666;
	background:#FFF;
}
.pmt-media-master .create-master-item .bottom .btitle {
	display:block;
	text-transform:uppercase;
}
.pmt-media-master .create-master-item .bottom textarea {
	margin:10px 0 0 0;
	width:100%;
	height:100px;
	border:none;
	color:#666;
	font-family:'Roboto', sans-serif;
}
.pmt-media-master .create-master-item .bottom .action-btns {
	padding:30px 5%;
	width:40%;
	border-left:1px solid #CCC;
	text-align:center;
} 
.pmt-media-master .create-master-item .bottom .action-btns .btn {
	margin:10px;
	width:100%;
	text-align:center;
}
.pmt-media-master .table {
	display:table;
	width:100% !important;
	font-size:12px;
	line-height:12px;
	color:#747474;
	/*text-align:center;*/
	border-spacing: 1px;
	background:#dedede;
}
.pmt-media-master .table .head {
	display:table-row;
	text-transform:uppercase;
	color:#000;
	background:#edf7ff;
}
.pmt-media-master .table .row {
	display:table-row;
}
.pmt-media-master .table .row:nth-child(even) {
	background: #FFF
}
.pmt-media-master .table .row:nth-child(odd) {
	background: #f7f7f7
}
.pmt-media-master .table .cell {
	display:table-cell;
	padding:8px;
}
.pmt-media-master .table .cell.sm {
	width:10%;
}
.pmt-media-master .cmi-col .table-header {
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
.pmt-media-master .kw-search{	
	display:inline-block;
}
.pmt-media-master .kw-search .icon {	
	float:left;
	display:inline-block;
	width:17px;
	height:17px;
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABEAAAARCAYAAAA7bUf6AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAbNJREFUeNp8071Lm1EUx/EnxlpfqlZjrZtQilAJ8QVpl26KOJgM4gtItyIIDhZLrYLoIqjoYJdC1kZdREScCr4gLk5VULDQUvAPqKVW0aDEfq/8Eg4h8cIH7pPknufcc0580WjUMyuADoQRQj5+YAsx/ELCS1u5Zv8C82hN+81TvEYvxrCCm0xBarGKGj2f4Q9u8QgVeI5FZRezGeWgHJ9MgH30ox5N6MEaruDHHBrgs0E60ay3HiCCr+hGUPXowhKuldWAMkwFiWh/jhlc4Ave45m+c4encKznNpTZmgSV2m+s442KHFLA5PqJI33+BCU2k4faJ3Tn75hIC+CZjJLn/DYT1/sqFKMOO17m5WpQbbp3bjPZ0N7dcShtdux6i0bt9zQCqSALONHhdnw27fZ093cY1d51cRan9jpurD8qmHvuwyvVJq4ruHkpNIHdBG8rYCr1ZeRpkCrVgVCWa7lOftDZcfeBPxx2/7W7iIfYVNUD5s2nGrhBtdllWYSXeIBdW0QX6JuKW6Zu5agLfxUs2bkRFGAYpZk68U8yrUtMa6aG1fZwtnbet+IK9BgtmPwvwADbz1tf2VH6ogAAAABJRU5ErkJggg==') no-repeat;
}
.pmt-media-master .kw-search input{	
	margin:0 0 0 5px;
	/*border:none;*/
    border:solid 1px #cccccc;
}
.pmt-media-master .play-btn {
	display:inline-block;
	width:18px;
	height:18px;
	cursor:pointer;
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABIAAAASCAMAAABhEH5lAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAADBQTFRF6enp+fn5////5ubm7+/v5eXl1dXV2dnZ0tLSz8/P9vb239/f2NjY8vLyzMzM////TtOJoAAAABB0Uk5T////////////////////AOAjXRkAAACNSURBVHjaXJCJEsMgCETpEfHg+P+/7YI0TcqMozxdZCGP0NXM2tJMCGuyVfDcaA4kXaRjGzNQEEmJSjJy7MsrFrROijd4XBT3SriA6rDxSi0k1Kx7IPzwxqFbo9QlQp1HKG9INroItYRn+eMsr9HKvYn/VqUM8TbEZahsM/9sg8l3OFLDuYzwGclHgAEA0HMQIKkPIY4AAAAASUVORK5CYII=') no-repeat;
}
.pmt-media-master .play-btn:hover {
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABIAAAASCAMAAABhEH5lAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAADBQTFRFYv8fWP8Qbf8vV/8PkP9gY/8gbv8w8P/qpf9/ef9Aj/9f3f/Pef8/0v+/Tf8A////MRZddgAAABB0Uk5T////////////////////AOAjXRkAAACcSURBVHjaVJCJEsMgCETxSjw4/v9vs6BtU2bU4emuAJmH1Ev1qhIJYa2hJ8baaCUkxEw40nLkhEMiHIwM57QTE1ojwRuzXjfDvRAuoOqaemghoaz+aYd1c0PSi0IXCD63K/8Qb5S1HURbmL/25WcvXor1+SrC9/oqlb2hgne7IbRfVrRdvKgxmgbZw+HPcPgMxyUzq+YptyePAAMAhtAPxt5Zn3sAAAAASUVORK5CYII=') no-repeat;
}
.pmt-media-master .checklist {
    min-width:22px;
    min-height:18px;
    background-repeat:no-repeat;
    background-image:url('/DesktopModules/PMT_Admin/images/checklist_grey.png');
}
.pmt-media-master .checklist:hover {
    background-image:url('/DesktopModules/PMT_Admin/images/checklist_red.png');
}
.pmt-media-master .checklistGreen {
    min-width:22px;
    min-height:18px;
    background-repeat:no-repeat;
    background-image:url('/DesktopModules/PMT_Admin/images/checklist_green.png');
}
.pmt-media-master .checklistGreen:hover {
    background-image:url('/DesktopModules/PMT_Admin/images/checklist_red.png');
}
.pmt-media-master .edit-btn {
	display:inline-block;
	width:18px;
	height:18px;
	cursor:pointer;
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABEAAAAPCAMAAAA1b9QjAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAADBQTFRFz8/P3t7e7+/v0tLS7Ozs6enp9vb28/Pz1dXV5ubm2NjY8vLy/Pz85eXlzMzM////GqKacwAAABB0Uk5T////////////////////AOAjXRkAAABqSURBVHjaVM9REoAgCEXRpyZRIux/t6GZ2v08M6DAyjWLQtlw6IxFkgYkxDISEgJDb/siZsdN6M4azJY0YNnkhfbWkAGonxCuNlJ0StW+YxPW4LDLIX1bF//zOQv6v6ufZrBSV9mHHwEGAOJbC2/muJh9AAAAAElFTkSuQmCC') no-repeat;
}
.pmt-media-master .export {
    margin-right:6px;
}
.pmt-media-master input, .pmt-media-master select {
	padding:4px;
	color:#666;
}
.pmt-media-master option {
	background:#FFF;
	color:#666;
}
.pmt-media-master .btn {
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
}
.pmt-media-master .btn-green {
	color:#FFF;
	background:#4dff00;
}
.pmt-media-master .btn-green:hover {
	background:#49f200;
}
.pmt-media-master .btn-blue {
	color:#FFF;
	background:#3498db;
}
.pmt-media-master .btn-blue:hover {
	background:#9fa8da;
}
.pmt-media-master .btn-default {
	color:#808080;
	background:#f7f7f7;
}
.pmt-media-master .btn-default:hover {
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
</style>

<asp:UpdateProgress ID="UpdateReports" AssociatedUpdatePanelID="pnlReports" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="/DesktopModules/PMT_Admin/images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<div class="pmt-media-master">
	
<div id="PMT_MasterItems">
            <asp:UpdatePanel ID="pnlReports" runat="server" UpdateMode="Always" visible="true">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExport" />
                </Triggers>
                <ContentTemplate> 
		
		<div class="sub-hdr">
			<asp:Label ID="lblReportsMessage" runat="server"></asp:Label>
			<span class="icon"></span>
		</div>
		<div class="sub-content">
			
			<div class="sub-search-options">
				<!--<div class="action-links">
					<a>Select All</a>
					<a>Clear All</a>
				</div>-->
				
                <asp:DropDownList ID="ddlReport" AutoPostBack="true" OnSelectedIndexChanged="ddlReport_SelectedIndexChanged" runat="server">
                    <asp:ListItem Text="--Select Report--" Value="-1" />
                    <asp:ListItem Text="Master Items" Value="1" />
                    <asp:ListItem Text="Library Items" Value="2" />
                    <asp:ListItem Text="Delivery Report" Value="3" />
                </asp:DropDownList>
				<div class="kw-search">
					<span class="icon"></span>
					<asp:TextBox ID="txtMasterItemSearch" runat="server" EnableViewState="true" AutoPostBack="true" OnTextChanged="txtMasterItemSearch_TextChanged" placeholder="Keyword Search"></asp:TextBox>
				</div>
                
                <asp:DropDownList ID="ddlReportsAdvertiserSearch" OnSelectedIndexChanged="ddlReportsAdvertiserSearch_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
				<asp:DropDownList ID="ddlReportsAgencySearch" AutoPostBack="true" OnSelectedIndexChanged="ddlReportsAgencySearch_SelectedIndexChanged" runat="server"></asp:DropDownList>
                <asp:DropDownList ID="ddlMasterStatus" AutoPostBack="true" OnSelectedIndexChanged="ddlMasterStatus_SelectedIndexChanged" Visible="false" runat="server">
                            <asp:ListItem Text="--Select Status--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="New" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Approved" Value="3"></asp:ListItem>
    	                </asp:DropDownList><br />
                <div class="kw-search"><asp:Label ID="lblStartDate" runat="server" Visible="false"> Start Date: </asp:Label><asp:TextBox ID="txtStartDate" AutoPostBack="true" OnTextChanged="txtStartDate_TextChanged" runat="server" Visible="false"></asp:TextBox><ajaxToolkit:CalendarExtender ID="CalendarExtender" runat="server" TargetControlID="txtStartDate"></ajaxToolkit:CalendarExtender>
                <asp:Label ID="lblEndDate" runat="server" Visible="false">End Date: </asp:Label><asp:TextBox ID="txtEndDate" runat="server" AutoPostBack="true" OnTextChanged="txtEndDate_TextChanged" Visible="false"></asp:TextBox><ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtEndDate"></ajaxToolkit:CalendarExtender>
                </div></div>

            <div style="display:none;"><asp:Button ID="btnTestJavelin" runat="server" Text="Create Javelin Order" OnClick="btnTestJavelin_Click" /><br />
            <asp:Button ID="btnJavelinStatus" runat="server" Text="Get Order Status" OnClick="btnJavelinStatus_Click" /><br />
            <asp:Button ID="btnJavelinDelete" runat="server" Text="Delete Order" OnClick="btnJavelinDelete_Click" /><br />
            <asp:Button ID="btnTestQB" runat="server" Text="Test QB" OnClick="btnTestQB_Click" /><br />
            <asp:Label ID="lblTestJavelin" runat="server"></asp:Label>

            <asp:Button ID="btnGetComcastAuthToken" runat="server" Text="Get OTSM Auth Token" OnClick="btnGetComcastAuthToken_Click" /><br />
            <asp:Label ID="lblComcastMessage" runat="server"></asp:Label><br /></div>
			
			<div class="table-container">
				<div class="table">

                    <asp:GridView ID="gvMasterItem" OnSelectedIndexChanged="gvMasterItem_SelectedIndexChanged" OnPageIndexChanging="gvMasterItem_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" Visible="false" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="tblItems" runat="server">
                        <HeaderStyle BackColor="#edf7ff" CssClass="table-header" />
                        <Columns>
                        
    	                <asp:TemplateField HeaderText="PMTMediaId">
    	                    <ItemTemplate>
    		                <asp:HiddenField ID="hdngvMasterItemId" Value='<%#Eval("Id") %>' runat="server" /><span class="cell"><%#Eval("PMTMediaId") %></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Title">
    	                    <ItemTemplate>
    		                <span class="cell"><%#Eval("Title") %></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Agencies">
    	                    <ItemTemplate>
    		                <span class="cell"><%#Eval("AgencyNames") %></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Advertiser">
    	                    <ItemTemplate>
    		                <span class="cell"><%#Eval("Advertiser") %></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Length">
    	                    <ItemTemplate>
    		                <span class="cell"><%#Eval("Length") %></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Status">
    	                    <ItemTemplate>
    		                <span class="cell"><%# !Boolean.Parse(Eval("hasCheckList").ToString()) ? "NEW" : "" %><%# (Boolean.Parse(Eval("hasCheckList").ToString()) && !Boolean.Parse(Eval("isApproved").ToString())) ? "PENDING" : "" %><%# (Boolean.Parse(Eval("hasCheckList").ToString()) && Boolean.Parse(Eval("isApproved").ToString())) ? "APPROVED" : "" %></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Checklist">
    	                    <ItemTemplate>
    		                <span class="cell"><asp:HyperLink NavigateUrl='<%# "https://s3-us-west-1.amazonaws.com/s3-pmt-bucket/Master_Checklists/" + Eval("PMTMediaId").ToString() + ".png" %>' runat="server"    Target="_blank">
                             <div class="<%# !Boolean.Parse(Eval("hasCheckList").ToString()) ? "checklist" : "checklistGreen" %>"></div></asp:HyperLink></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="View">
    	                    <ItemTemplate>
    		                <span class="cell"><asp:LinkButton ID="lbtnEdit" CommandArgument='<%#Eval("Id") %>' OnClick="lbtnEdit_Click" runat="server" Visible='<%# Eval("PMTMediaId") == "" ? false : true %>'><span class="play-btn"></span></asp:LinkButton></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
                        </Columns>
                    </asp:GridView>


                    <asp:GridView ID="gvLibraryItem" OnSelectedIndexChanged="gvLibraryItem_SelectedIndexChanged" OnPageIndexChanging="gvLibraryItem_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" Visible="false" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="tblItems" runat="server">
                        <HeaderStyle BackColor="#edf7ff" CssClass="table-header" />
                       
                        <Columns>
    	                <asp:TemplateField HeaderText="PMTMediaId">
    	                    <ItemTemplate>
    		                <asp:HiddenField ID="hdngvLibraryItemId" Value='<%#Eval("Id") %>' runat="server" /><span class="cell"><%#Eval("PMTMediaId") %></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="ISCI Code">
    	                    <ItemTemplate>
    		                <span class="cell"><%#Eval("ISCICode") %></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Title">
    	                    <ItemTemplate>
    		                <span class="cell"><%#Eval("Title") %></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Description">
    	                    <ItemTemplate>
    		                <span class="cell"><%#Eval("ProductDescription") %></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Advertiser">
    	                    <ItemTemplate>
    		                <span class="cell"><%#Eval("Advertiser") %></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>                            
    	                <asp:TemplateField HeaderText="Agency">
    	                    <ItemTemplate>
    		                <span class="cell"><%#Eval("Agency") %></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Length">
    	                    <ItemTemplate>
    		                <span class="cell"><%#Eval("MediaLength") %></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="View">
    	                    <ItemTemplate>
    		                <span class="cell"><asp:LinkButton ID="lbtnLibEdit" CommandArgument='<%#Eval("Id") %>' OnClick="lbtnLibEdit_Click" runat="server" Visible='<%# Eval("ISCICode").ToString() == "" ? false : true %>'><span class="play-btn"></span></asp:LinkButton></span>
    	                    </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Panel ID="pnlDelivery" runat="server" Visible="false">
    <div class="table-container">
<table>
	<tr class="head">
        <td>WO #</td>
		<td>ISCI</td>
        <td>Advertiser</td>
        <td>Agency</td>
		<td>Product/Description</td>
		<td>Title</td>
		<td>Tracking ID</td>
		<td>Ship to Station</td>
        <td>Date Created</td>
		<td>Shipping Method</td>
	</tr>
	<asp:Literal ID="litDelivery" runat="server"></asp:Literal>
	
	
</table></div>

</asp:Panel>
				</div>
			</div><br />
                    
            <div style="width:100%;text-align:right;margin-right:6px;"><asp:Button ID="btnExport" CssClass="btn btn-blue export" runat="server" Text="Export to Excel" Visible="false" OnClick="btnExport_Click" /></div><br />
            </ContentTemplate>
                </asp:UpdatePanel>
    </div></div></div>




<ajaxToolkit:ModalPopupExtender runat="server" 
	ID="mpeViewerPopup" 
	TargetControlID="dummysg" 
	PopupControlID="pnlViewerHolder" 
	BackgroundCssClass="modalBackground"                        
	DropShadow="true"/> 
<input id="dummysg" type="button" style="display: none" runat="server" />
<asp:Panel ID="pnlViewerHolder" CssClass="modalPopup" runat="server">

    <asp:UpdatePanel ID="pnlStationGroupStationsModal" runat="server" UpdateMode="Always"><ContentTemplate>
        <div id="videoPlayer">
        <video width="320" height="240" controls="controls">
            <asp:Literal ID="litVidSource" runat="server"></asp:Literal>
        </video>
            <br /><br />
            <asp:Button ID="btnCancel" runat="server" Text="Close" OnClick="btnCancel_Click" />
    </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>