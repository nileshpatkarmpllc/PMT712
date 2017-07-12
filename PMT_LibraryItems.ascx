<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMT_LibraryItems.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.PMT_LibraryItems" %>
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
	width: 70%;
    border:1px solid #cccccc;
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
.pmt .create-master-item .col25 {
	width:29%;
}
.pmt .create-master-item .col33 {
	width:28%;
}
.pmt .create-master-item .col50 {
	width:45%;
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
</style>

<asp:UpdateProgress ID="UpdateLibraryItems" AssociatedUpdatePanelID="pnlLibraryItems" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="/images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<div id="PMT_LibraryItems">
            <asp:UpdatePanel ID="pnlLibraryItems" runat="server" UpdateMode="Always" visible="true">
                <ContentTemplate>
<div class="pmt">
	
		<div class="sub-hdr">
			<asp:Label ID="lblLibraryItemMessage" runat="server"></asp:Label>
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
					<asp:TextBox ID="txtLibraryItemSearch" runat="server" AutoPostBack="true" OnTextChanged="txtLibraryItemSearch_TextChanged" EnableViewState="true" placeholder="keyword"></asp:TextBox>
				</div>
				<asp:DropDownList ID="ddlLibraryItemAdvertiserSearch" OnSelectedIndexChanged="ddlLibraryItemAdvertiserSearch_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                <asp:DropDownList ID="ddlLibraryItemAgencySearch" AutoPostBack="true" OnSelectedIndexChanged="ddlLibraryItemAgencySearch_SelectedIndexChanged" runat="server"></asp:DropDownList>
				
			</div>
			
			<div class="table-container">
				<div class="table">

                    <asp:GridView ID="gvLibraryItem" OnSelectedIndexChanged="gvLibraryItem_SelectedIndexChanged" OnPageIndexChanging="gvLibraryItem_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="tblItems" runat="server">
                        <HeaderStyle BackColor="#edf7ff" CssClass="table-header" />
                        <Columns>
    	                <asp:TemplateField HeaderText="PMTMediaId">
    	                    <ItemTemplate>
    		                <span class="cell"><asp:HiddenField ID="hdngvLibraryItemId" Value='<%#Eval("Id") %>' runat="server" /><asp:Label ID="lblgvPMTMediaID" Text='<%#Eval("PMTMediaId") %>' runat="server" /></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="ISCI Code">
    	                    <ItemTemplate>
    		                <span class="cell"><asp:Label ID="lblgvISCICode" Text='<%#Eval("ISCICode") %>' runat="server" /></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Title">
    	                    <ItemTemplate>
    		                <span class="cell"><asp:Label ID="lblgvTitle" Text='<%#Eval("Title") %>' runat="server" /></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Description">
    	                    <ItemTemplate>
    		                <span class="cell"><asp:Label ID="lblgvDescription" Text='<%#Eval("ProductDescription") %>' runat="server" /></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Length">
    	                    <ItemTemplate>
    		                <span class="cell"><asp:Label ID="lblgvLength" Text='<%#Eval("MediaLength") %>' runat="server" /></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date Created">
	                    <ItemTemplate>
		                <span class="cell"><%# Convert.ToDateTime(Eval("DateCreated")).ToString("d") %></span>
	                    </ItemTemplate>
	                </asp:TemplateField>
                            <asp:TemplateField HeaderText="View">
    	                    <ItemTemplate>
    		                <span class="cell"><asp:LinkButton ID="lbtnLibEdit" CommandArgument='<%#Eval("Id") %>' OnClick="lbtnLibEdit_Click" runat="server" Visible='<%# Eval("ISCICode").ToString() == "" ? false : true %>'><span class="play-btn"></span></asp:LinkButton></span>
    	                    </ItemTemplate>
                            </asp:TemplateField>
    	                <asp:TemplateField>
    	                    <ItemTemplate>
    		                <span class="cell"><asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" /></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
                            
                        </Columns>
                    </asp:GridView>

                    </div>
			</div><br />
                    <br />

		</div>
                    <div class="sub-hdr">
			Edit Library Item
			<span class="icon"></span>
		</div>
		<div class="sub-content create-master-item">

                    <div class="col col25">
    	                <asp:TextBox ID="txtLibraryItemCustomerId" runat="server" placeholder="Customer Id"></asp:TextBox>
                        </div>
                    <div class="col col25">
                        <asp:TextBox ID="txtISCICode" runat="server" placeholder="ISCI Code"></asp:TextBox>
                        </div>
                   <div class="col col25">
                        <asp:TextBox ID="txtLibraryItemPMTMediaId" runat="server" placeholder="PMT Media Id"></asp:TextBox>
                        </div>
                    <div class="col col25">
                        <asp:TextBox ID="txtLibraryItemTitle" runat="server" placeholder="Title"></asp:TextBox>
                        </div>
                    <div class="col col25">
    	                <asp:TextBox ID="txtLibraryItemFile"  runat="server" placeholder="Filename"></asp:TextBox>
                        </div>
                     <div class="col col25">
    	                <asp:TextBox ID="txtProductDescription"  runat="server" placeholder="Product Description"></asp:TextBox>
                        </div>
                    <div class="col col25">
                        <asp:DropDownList ID="ddlLibraryItemAdvertiser" runat="server" OnSelectedIndexChanged="ddlLibraryItemAdvertiser_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    <div class="col col25">
    	                <asp:DropDownList ID="ddlLibraryItemAgency" OnSelectedIndexChanged="ddlLibraryItemAgency_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                        </div>
                    <div class="col col25">
    	                Closed Captioned <asp:CheckBox ID="chkLibraryItemClosedCaption" CssClass="ccChk" runat="server"></asp:CheckBox>
                        </div>
                     <div class="col col25">
	                        <asp:DropDownList ID="ddlLibraryItemMediaType" runat="server">
                                <asp:ListItem Value="-1" Text="--Select Media Type--"></asp:ListItem>
                                <asp:ListItem Value="1" Text="ADDELIVERY YES" Enabled="false"></asp:ListItem>
                                <asp:ListItem Value="2" Text="HD"></asp:ListItem>
                                <asp:ListItem Value="3" Text="HD & SD"></asp:ListItem>
                                <asp:ListItem Value="4" Text="HD & SD (BACKUP REQUIRED)" Enabled="false"></asp:ListItem>
                                <asp:ListItem Value="5" Text="SD"></asp:ListItem>
	                        </asp:DropDownList>
                        </div>    
                     <div class="col col25">
                        <asp:DropDownList ID="ddlLibraryItemEncode" runat="server">
                            <asp:ListItem Value="-1" Text="--Select Encode--"></asp:ListItem>
                            <asp:ListItem Value="1" Text ="SPOTTRAC"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Teletrax"></asp:ListItem>
                            <asp:ListItem Value="3" Text="VEIL"></asp:ListItem>
                        </asp:DropDownList>
                        </div> 
                     <div class="col col25">
                        <asp:DropDownList ID="ddlLibraryItemStandard" runat="server">
                            <asp:ListItem Value="-1" Text="--Select Standard--"></asp:ListItem>
                            <asp:ListItem Value="NTSC" Text ="NTSC"></asp:ListItem>
                            <asp:ListItem Value="PAL" Text="PAL"></asp:ListItem>
                        </asp:DropDownList>
                        </div>
                     <div class="col col25">
    	                <asp:TextBox ID="txtLibraryItemLength" placeholder="00:00" runat="server"></asp:TextBox>
                        </div>
                    <div class="col col25">
    	                <asp:TextBox ID="txtLibraryItemReel" placeholder="Reel #" runat="server"></asp:TextBox>
                        </div>
                    <div class="col col25">
    	                <asp:TextBox ID="txtLibraryItemTapeCode" placeholder="Tape Code" runat="server"></asp:TextBox>
                        </div>                        
                     <div class="col col25">
    	                <asp:TextBox ID="txtLibraryItemPostition" placeholder="Position #" runat="server"></asp:TextBox>
                        </div>                     
                   <div class="col col25">
    	                <asp:TextBox ID="txtLibraryItemVaultId" placeholder="Vault Id" runat="server"></asp:TextBox>
                        </div>                     
                    <div class="col col25">
    	                <asp:TextBox ID="txtLibraryItemLocation" placeholder="Location" runat="server"></asp:TextBox>
                        </div>                     
                    <div class="col col25">
    	                <asp:TextBox ID="txtLibraryItemComment" placeholder="Comment" TextMode="MultiLine" Rows="6" runat="server"></asp:TextBox>
                        </div>

    	                <asp:HiddenField Visible="false" ID="txtChecklistForm" Value="" runat="server"></asp:HiddenField>
    	                <asp:HiddenField Visible="false" ID="txtSelectedLibraryItem" Value="-1" runat="server"></asp:HiddenField>
    	                <asp:HiddenField Visible="false" ID="txtLibraryItemCreatedBy" Value="-1" runat="server"></asp:HiddenField>
    	                <asp:HiddenField Visible="false" ID="txtLibraryItemCreatedDate" Value="" runat="server"></asp:HiddenField>
            <div class="col col75"><div class="bottom">
				<div class="action-btns">
    	                <asp:Button ID="btnSaveLibraryItem" runat="server" CssClass="btn btn-blue" Text="Save Library Item" OnClick="btnSaveLibraryItem_Click" />
    	                <asp:Button ID="btnSaveLibraryItemAs" runat="server" CssClass="btn btn-blue" Text="Save Library Item As" Enabled="false" OnClick="btnSaveLibraryItemAs_Click" ToolTip="Save a new Library Item based on these settings." />
    	                <asp:Button ID="btnDeleteLibraryItem" runat="server" CssClass="btn redButton" Enabled="false" Text="Delete Library Item" OnClick="btnDeleteLibraryItem_Click" OnClientClick="return confirm('Are you certain you want to delete this Library Item?');" />
    	                <asp:Button ID="btnClearLibraryItem" Visible="false" Text="Clear Library Item" ToolTip="If you have already clicked on another LibraryItem below, you must click this button first before you try to create a new Library Item." CssClass="btn btn-default" runat="server" OnClick="btnClearLibraryItem_Click" />
                    </div></div></div>
                        <br />
                        
                        
                    </fieldset>
                    
                    
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </div></div></div></div>

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