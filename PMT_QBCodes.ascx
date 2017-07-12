<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMT_QBCodes.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.PMT_QBCodes" %>
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
	background:#ffffff;
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
    float:left;
}
.pmt .create-master-item .col50 {
	width:45%;
    float:left;
}
.pmt .create-master-item .col75 {
	width:70%;
    float:left;
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
.pmt .Row {
        background-color:#ffffff;
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
        max-width:500px;
        background-color:white;
        margin-bottom:10px;
        border:solid 1px #dedede;
        padding:10px;
        float:left;
        margin-right:10px;
    }
     .pmt-SelectList td {
         width:300px;
     }
    .pmt-SelectList input {
        margin-right:10px;
    }
    .pmt-SelectList input[type="checkbox"] {
        width:10%;
    }
</style>

<asp:UpdateProgress ID="updQBCodes" AssociatedUpdatePanelID="pnlQBCodes" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<div id="PMT_QBCodes">
        <asp:UpdatePanel ID="pnlQBCodes" runat="server" UpdateMode="Always" visible="true">
            <ContentTemplate>

                <div class="pmt">
	
		<div class="sub-hdr">
			<asp:Label ID="lblQBCodesMessage" runat="server"></asp:Label>
			<span class="icon"></span>
		</div>
		<div class="sub-content">
			
			<div class="sub-search-options">
				
				<div class="kw-search">
					<span class="icon"></span>
					<asp:TextBox ID="txtQBCodeSearch" runat="server" EnableViewState="true" AutoPostBack="true" OnTextChanged="txtQBCodeSearch_TextChanged" placeholder="keyword"></asp:TextBox> <asp:DropDownList ID="ddlTypeSearch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTypeSearch_SelectedIndexChanged">
                        <asp:ListItem Text="--Select Type--" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Delivery" Value="Delivery"></asp:ListItem>
                        <asp:ListItem Text="Bundle" Value="Bundle"></asp:ListItem>
                        <asp:ListItem Text="Customize" Value="Customized"></asp:ListItem>
                        <asp:ListItem Text="Non-Deliverable" Value="Non_Deliverable"></asp:ListItem>
					  </asp:DropDownList>
				</div>
	                
				
			</div>
			
			<span class="table-container">
				<span class="table">
                    <asp:GridView ID="gvQBCodes" OnSelectedIndexChanged="gvQBCodes_SelectedIndexChanged" OnPageIndexChanging="gvQBCodes_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" OnSorting="gvQBCodes_Sorting" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="tblItems" runat="server">
                    <HeaderStyle BackColor="#edf7ff" CssClass="table-header" />
                        <Columns>
	                    <asp:TemplateField HeaderText="QuickBooks Code" SortExpression="Code">
	                        <ItemTemplate>
		                    <span class="cell"><asp:HiddenField ID="hdngvQBCodeId" Value='<%#Eval("Id") %>' runat="server" /><asp:Label ID="lblgvQBCode" Text='<%#Eval("QBCode") %>' runat="server" /></span>
	                        </ItemTemplate>
	                    </asp:TemplateField>
                            <asp:TemplateField HeaderText="Length">
	                        <ItemTemplate>
		                    <span class="cell"><%#Eval("MinLength") %> - <%#Eval("MaxLength") %></span>
	                        </ItemTemplate>
	                    </asp:TemplateField>
                            <asp:TemplateField HeaderText="Type" SortExpression="Type">
	                        <ItemTemplate>
		                    <span class="cell"><%#Eval("Type").ToString() %></span>
	                        </ItemTemplate>
	                    </asp:TemplateField>
                            <asp:TemplateField HeaderText="Media Type">
	                        <ItemTemplate>
		                    <span class="cell"><%#Eval("MediaType").ToString().Replace("___"," & ") %></span>
	                        </ItemTemplate>
	                    </asp:TemplateField>
	                    <asp:TemplateField>
	                        <ItemTemplate>
		                    <span class="cell"><asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" /></span>
	                        </ItemTemplate>
	                    </asp:TemplateField>
                        </Columns>
                    </asp:GridView></span></span><br clear="both" />
            <div class="sub-hdr">
			Create/Edit QuickBooks Code
			<span class="icon"></span>
		</div>


            <div class="sub-content create-master-item">
            <div class="editHolder">

                <div class="Row"><div class="col50">
	            <dnn:label ID="lblQBCodeType" runat="server" />
	            <asp:DropDownList ID="ddlQBCodeType" AutoPostBack="true" OnSelectedIndexChanged="ddlQBCodeType_SelectedIndexChanged" runat="server">
                    <asp:ListItem Text="--Select Type--" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Delivery" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Bundle" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Customize" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Non-Deliverable" Value="3"></asp:ListItem>
	            </asp:DropDownList>
                </div>
                 <div class="col50">
	            <dnn:label ID="lblQBCode" runat="server" />
	            <asp:TextBox ID="txtQBCode" placeholder="QuickBooks Code" runat="server" />
                </div><br clear="both" /></div>
                
                <asp:Panel ID="pnlDelivery" Visible="false" runat="server">
                    <div class="altRow">
                        <div class="col33">
                            <dnn:label ID="lblMinLength" runat="server" />
	                        <asp:TextBox ID="txtMinLength" placeholder="Minimum Length" runat="server" />
                        </div>
                        <div class="col33">
                            <dnn:label ID="lblMaxLength" runat="server" />
	                        <asp:TextBox ID="txtMaxLength" placeholder="Maximum Length" runat="server" />
                        </div>
                        <div class="col33">
                            <dnn:label ID="lblMediaType" runat="server" />
	                        <asp:DropDownList ID="ddlMediaType" runat="server">
                                <asp:ListItem Text="--Select Media Type--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="HD" Value ="1"></asp:ListItem>
                                <asp:ListItem Text="SD" Value="4"></asp:ListItem>
                                <asp:ListItem Text="HD & SD" Value="2"></asp:ListItem>
	                        </asp:DropDownList>
                        </div>
                        <br clear="both" />
                    </div>
                    <div class="Row">
                        <div class="col50">
                            <dnn:label ID="lblStationDeliveryMethod" runat="server" />
                            <div class="pmt-SelectList"><asp:CheckBoxList ID="lbxStationDeliveryMethod" runat="server" Rows="6" SelectionMode="Multiple"></asp:CheckBoxList></div>
                        </div>
                        <div class="col50">
	                        <dnn:label ID="lblStationTapeFormat" runat="server" />
	                        <div class="pmt-SelectList"><asp:CheckBoxList ID="lbxQBCodeTapeFormat" runat="server" Rows="6" SelectionMode="Multiple"></asp:CheckBoxList></div>
                        </div>
                    <br clear="both" /></div>
                </asp:Panel>
                <asp:Panel ID="pnlNonDelivery" Visible="false" runat="server">
                    <div class="altRow"><div class="col50">
                        <dnn:label ID="lblServices" runat="server" />
                        <div class="pmt-SelectList"><asp:CheckBoxList ID="cklServices" runat="server" RepeatColumns="1"></asp:CheckBoxList></div>
                    </div><br clear="both" /></div>
                </asp:Panel>
                </div>
                <br clear="both" />

	            <asp:HiddenField Visible="false" ID="txtSelectedQBCode" Value="-1" runat="server"></asp:HiddenField>
	            <asp:HiddenField Visible="false" ID="txtQBCodeCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	            <asp:HiddenField Visible="false" ID="txtQBCodeCreatedDate" Value="" runat="server"></asp:HiddenField>
            </div>
	            <div style="width:100%;text-align:center;padding-top:10px;"><div class="col25b"><asp:Button ID="btnSaveQBCode" CssClass="btn btn-blue" runat="server" Text="Save QuickBooks Code" OnClick="btnSaveQBCode_Click" /></div>
                <div class="col25b"><asp:Button ID="btnSaveQBCodeAs" runat="server" Text="Save QuickBooks Code As" CssClass="btn btn-blue" Enabled="false" OnClick="btnSaveQBCodeAs_Click" ToolTip="Save a new QuickBooks Code based on these settings." /></div>
                <div class="col25b"><asp:Button ID="btnDeleteQBCode" runat="server" CssClass="btn redButton" Enabled="false" Text="Delete QuickBooks Code" OnClick="btnDeleteQBCode_Click" OnClientClick="return confirm('Are you certain you want to delete this QuickBooks Code? NOTE: Deleting a QuickBooks Code may make old orders invalid. It is recommended that you set this QuickBooks Code to Inactive Status rather than delete it.');" /></div>
                <div class="col25b"><asp:Button ID="btnClearQBCode" Text="Clear QuickBooks Code" CssClass="btn btn-default" ToolTip="If you have already clicked on another QuickBooks Code below, you must click this button first before you try to create a new QuickBooks Code." runat="server" OnClick="btnClearQBCode_Click" /></div><br clear="both" /></div>
            </div></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div><br clear="both" />