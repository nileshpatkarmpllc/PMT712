<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMT_StationGroups.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.PMT_StationGroups" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %> 
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ register tagprefix="dnn" tagname="Label" src="~/controls/LabelControl.ascx" %>

<dnn:DnnCssInclude ID="DnnCssInclude3" runat="server" FilePath="/DesktopModules/PMT_Admin/styles/style.css" Priority="11" />

<asp:UpdateProgress ID="updStationGroups" AssociatedUpdatePanelID="pnlStationGroups" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<style>
@import url(https://fonts.googleapis.com/css?family=Roboto:400,500);

.pmt-station-groups {
	font-family:'Roboto', sans-serif;
}

.pmt-station-groups .group-hdr {
	position:relative;
	padding:5px 30px;
	color:#FFF;
	text-transform:uppercase;
	background:#6291fc;
	-webkit-box-shadow: 0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	-moz-box-shadow:    0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	box-shadow:         0px 2px 5px 0px rgba(50, 50, 50, 0.25);
}
.pmt-station-groups .group-content {
	padding:15px;
	background:#f7f7f7;
	-webkit-box-shadow: 0px 1px 2px 0px rgba(50, 50, 50, 0.50);
	-moz-box-shadow:    0px 1px 2px 0px rgba(50, 50, 50, 0.50);
	box-shadow:         0px 1px 2px 0px rgba(50, 50, 50, 0.50);
}

.pmt-station-groups .sub-hdr {
	position:relative;
	padding:5px 10px;
	color:#3576be;
	text-transform:uppercase;
	-webkit-box-shadow: 0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	-moz-box-shadow:    0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	box-shadow:         0px 2px 5px 0px rgba(50, 50, 50, 0.25);
}
.pmt-station-groups .sub-content{
	background:#FFF;
	overflow:hidden;
}
.pmt-station-groups .sub-content .sub-hdr{
	padding:15px; 
}
.pmt-station-groups .sub-search-options {
	display:inline-block;
	padding:0 20px;
}
.pmt-station-groups .sub-search-options input {
	margin:0 20px 0 0;
}

.pmt-station-groups table-container{
	max-height:210px;
	overflow-y: scroll;
}
.pmt-station-groups .sub-search-options select {
	min-width:15%;
}
.pmt-station-groups .sub-search-options .action-links {
	float:right;
}
.pmt-station-groups .sub-search-options .action-links a {
	display:inline-block;
	padding:5px 10px; 
	font-size:12px;
	color:#666;
	cursor:pointer;
}
.pmt-station-groups .btn.non-deliverable{
	margin:15px;
	float:right;
}
.pmt-station-groups .pagination {
	margin:20px 0;
	color:#666;
	font-size:11px;
	list-style-type:none;
}
.pmt-station-groups .pagination li {
	display:inline-block;
	padding:3px 5px;
}
.pmt-station-groups .pagination li.cur {
	color:#FFF;
	background:#666;
}
.pmt-station-groups .create-station-group {
	padding:15px 0; 
	background:#f0f0f0;
	color:#666;
	font-size:12px;
}
.pmt-station-groups .create-station-group .col {
	padding: 1% 2%;
	display:inline-block;
	vertical-align:top;
}
.pmt-station-groups .create-station-group label {
	text-transform:uppercase;
	color:#666;
	font-size:12px;
}


.pmt-station-groups .new-station-group {
	padding:15px;
}
.pmt-station-groups .new-station-group .col {
	display:inline-block;
	vertical-align:top;
}
.pmt-station-groups .new-station-group .col33 {
	width:32%;
}
.pmt-station-groups .new-station-group .col66 {
	width:66%;
}
.pmt-station-groups .new-station-group .clear {
	clear:both;
	padding:8px;
}
.pmt-station-groups .new-station-group .col33 label {
	display:inline-block;
	width:40%;
	font-size:14px;
	vertical-align:top;
	color:#666;
}
.pmt-station-groups .new-station-group .col33 input {
	width:58%;
}
.pmt-station-groups .new-station-group .col33 textarea {
	width:58%;
	height:100px;
}
.pmt-station-groups .new-station-group .col66 label {
	display:inline-block;
	padding:0 0 0 4%;
	width:25%;
	font-size:14px;
	vertical-align:top;
	color:#666;
}
.pmt-station-groups .new-station-group .col66 textarea {
	width:70%;
	height:150px;
}

.pmt-station-groups .new-station-group .action-btns {
	padding:30px 15px;
	text-align:right;
    overflow:hidden;
} 
.pmt-station-groups .new-station-group .action-btns .btn {
	margin-left:10px;
    overflow:hidden;
} 

.pmt-station-groups table {
	display:table;
	width:100%;
	font-size:12px;
	line-height:12px;
	color:#747474;
	text-align:center;
	border-spacing: 0px;
	background:#dedede;
}
.pmt-station-groups table .head {
	display:table-row;
	text-transform:uppercase;
	color:#000;
	background:#edf7ff;
}
.pmt-station-groups table tr {
	display:table-row;
}
.pmt-station-groups table tr:nth-child(even) {
	background: #FFF
}
.pmt-station-groups table tr:nth-child(odd) {
	background: #f7f7f7
}
.pmt-station-groups table td, th {
	display:table-cell;
	padding:8px;
    font-size:12px;
    border:solid 1px #cccccc;
}
    .pmt-station-groups table th {
        text-transform:uppercase;
        color:black;
    }
.pmt-station-groups table td.sm {
	width:10%;
}

.pmt-station-groups .kw-search{	
	display:inline-block;
}
.pmt-station-groups .kw-search .icon {	
	float:left;
	display:inline-block;
	width:17px;
	height:17px;
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABEAAAARCAYAAAA7bUf6AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAbNJREFUeNp8071Lm1EUx/EnxlpfqlZjrZtQilAJ8QVpl26KOJgM4gtItyIIDhZLrYLoIqjoYJdC1kZdREScCr4gLk5VULDQUvAPqKVW0aDEfq/8Eg4h8cIH7pPknufcc0580WjUMyuADoQRQj5+YAsx/ELCS1u5Zv8C82hN+81TvEYvxrCCm0xBarGKGj2f4Q9u8QgVeI5FZRezGeWgHJ9MgH30ox5N6MEaruDHHBrgs0E60ay3HiCCr+hGUPXowhKuldWAMkwFiWh/jhlc4Ave45m+c4encKznNpTZmgSV2m+s442KHFLA5PqJI33+BCU2k4faJ3Tn75hIC+CZjJLn/DYT1/sqFKMOO17m5WpQbbp3bjPZ0N7dcShtdux6i0bt9zQCqSALONHhdnw27fZ093cY1d51cRan9jpurD8qmHvuwyvVJq4ruHkpNIHdBG8rYCr1ZeRpkCrVgVCWa7lOftDZcfeBPxx2/7W7iIfYVNUD5s2nGrhBtdllWYSXeIBdW0QX6JuKW6Zu5agLfxUs2bkRFGAYpZk68U8yrUtMa6aG1fZwtnbet+IK9BgtmPwvwADbz1tf2VH6ogAAAABJRU5ErkJggg==') no-repeat;
}
.pmt-station-groups .kw-search input{	
	margin:0 0 0 5px;
	border:solid 1px #cccccc;
	background:transparent;
}
.pmt-station-groups .delete-btn {
	display:inline-block;
	width:18px;
	height:18px;
	cursor:pointer;
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAMAAAAoLQ9TAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAACFQTFRF9vb22dnZ+vr6wcHB1NTUz8/P8fHxt7e3////s7Oz////wFJVVAAAAAt0Uk5T/////////////wBKTwHyAAAAZklEQVR42mSPURLAMARECVa5/4G72v50mJiJZzdBmiFuVeby3JleX/gL2M1QjaRqAPtxRnxiNCKsL6AbuEhEvPIgE5Mny8UqGlnJg44yOrSHTN1KzwLLsh5d367B9uh7uf/6twADAMxdByX2qIaVAAAAAElFTkSuQmCC') no-repeat;
}

.pmt-station-groups .edit-btn {
	display:inline-block;
	width:18px;
	height:18px;
	cursor:pointer;
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABEAAAAPCAMAAAA1b9QjAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAADBQTFRFz8/P3t7e7+/v0tLS7Ozs6enp9vb28/Pz1dXV5ubm2NjY8vLy/Pz85eXlzMzM////GqKacwAAABB0Uk5T////////////////////AOAjXRkAAABqSURBVHjaVM9REoAgCEXRpyZRIux/t6GZ2v08M6DAyjWLQtlw6IxFkgYkxDISEgJDb/siZsdN6M4azJY0YNnkhfbWkAGonxCuNlJ0StW+YxPW4LDLIX1bF//zOQv6v6ufZrBSV9mHHwEGAOJbC2/muJh9AAAAAElFTkSuQmCC') no-repeat;
}
.pmt-station-groups input, .pmt-station-groups select, .pmt-station-groups textarea {
	padding:4px;
	color:#666;
    border:solid 1px #cccccc;
    width:30%;
}
    .pmt-station-groups select, .pmt-station-groups textarea {
        overflow-y: scroll;
    }
.pmt-station-groups option {
	background:#FFF;
	color:#666;
}
.pmt-station-groups .btn {
	display:inline-block;
	padding:5px 10px;
	font-size:14px;
	text-transform:uppercase;
    min-width:180px;
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

.pmt-station-groups .btn-blue {
	color:#FFF;
	background:#3498db;
    width:18%;
    overflow:hidden;
}
.pmt-station-groups .btn-red {
	color:#FFF;
	background:#ff7777;
    width:18%;
    min-width:180px;
    overflow:hidden;
}
.pmt-station-groups .btn-red2 {
    color:#FFF;
	background:#ff7777;
    width:18%;
    min-width:230px;
    overflow:hidden;
}
.pmt-station-groups .btn-blue:hover {
	background:#9fa8da;
    overflow:hidden;
}
.pmt-station-groups .btn-default {
	color:#808080;
	background:#f7f7f7;
    width:18%;
    overflow:hidden;
}
.pmt-station-groups .btn-default:hover {
	background:#ebebeb;
    overflow:hidden;
}
.table table td {
    padding:4px;
    border:none;
}
.table table {
    background-color:white;
    width:40%;
    border:none;
    border-spacing:0px;
}
.table table table {
    width:30%;
}
.table table table td {
    border:none;
}
.StationKeyword {
    width:120px !important;
}
</style>

<div id="PMT_StationGroups">
        <asp:UpdatePanel ID="pnlStationGroups" runat="server" UpdateMode="Always" visible="true">
            <ContentTemplate>
<div class="pmt-station-groups">
	
		<div class="sub-hdr">
			My Station Groups <asp:Label ID="lblStationGroupMessage" runat="server"></asp:Label>
			<!--<div class="sub-search-options">
				<input type="text" placeholder="Keyword CITY : TV : RADIO" />
				<div class="kw-search">
					<span class="icon"></span>
					<input type="text" placeholder="Keyword Search" />
				</div>
			</div>
			<span class="icon"></span>-->
		</div>
		<div class="sub-content">

			<div class="table-container">
                <asp:GridView ID="gvStationGroup" OnSelectedIndexChanged="gvStationGroup_SelectedIndexChanged" OnPageIndexChanging="gvStationGroup_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="table" runat="server">
                    <HeaderStyle BackColor="#f7f7f7" />
                    <AlternatingRowStyle BackColor="#f7f7f7" />
                    <Columns>
	                <asp:TemplateField HeaderText="Group Name">
	                    <ItemTemplate>
		                <asp:HiddenField ID="hdngvStationGroupId" Value='<%#Eval("Id") %>' runat="server" /><asp:Label ID="lblgvStationGroupName" Text='<%#Eval("StationGroupName") %>' runat="server" />
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField>
	                    <ItemTemplate>
		                <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="Edit" />
	                    </ItemTemplate>
	                </asp:TemplateField>
                    </Columns>
                </asp:GridView>
			</div>
			
			
		
		<div class="sub-hdr">
			Create New Station Group
			<span class="icon"></span>
		</div>
		<div class="sub-content new-station-group">
		
			<div class="col col33">
				<label>Group Name</label>
				<asp:TextBox ID="txtStationGroupName" placeholder="Station Group Name" runat="server" />
				
				<div class="clear"></div> 
				
				<label>Description</label>
				<asp:TextBox ID="txtStationGroupDescription" TextMode="MultiLine" Rows="6" placeholder="Description" runat="server" />
			</div>
			<div class="col col66">
				<label>Stations in Group</label>
				<asp:ListBox ID="lbxStationGroupStations" runat="server" Rows="6"></asp:ListBox> <asp:Button id="btnManageStationsInGroup" runat="server" Text="Manage Stations in Group" OnClick="btnManageStationsInGroup_Click" Enabled="false" />
			</div>
			
			<div class="action-btns">
				<asp:Button ID="btnClearStationGroup" Text="Clear Station Group" ToolTip="If you have already clicked on another Station Group below, you must click this button first before you try to create a new Station Group." runat="server" OnClick="btnClearStationGroup_Click" CssClass="btn btn-default" />
				<asp:Button ID="btnSaveStationGroupAs" runat="server" Text="Save StationGroup As" Enabled="false" OnClick="btnSaveStationGroupAs_Click" ToolTip="Save a new StationGroup based on these settings." CssClass="btn btn-blue" />
				<asp:Button ID="btnSaveStationGroup" runat="server" Text="Save StationGroup" OnClick="btnSaveStationGroup_Click" CssClass="btn btn-blue" />
                 <asp:Button ID="btnDeleteStationGroup" runat="server" CssClass="btn btn-red" Enabled="false" Text="Delete Station Group" OnClick="btnDeleteStationGroup_Click" OnClientClick="return confirm('Are you certain you want to delete this Station Group?');" />
			</div>
		</div>
		
</div>
	                <asp:HiddenField Visible="false" ID="txtSelectedStationGroup" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtStationGroupCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	                <asp:HiddenField Visible="false" ID="txtStationGroupCreatedDate" Value="" runat="server"></asp:HiddenField>
                    </div><br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

<ajaxToolkit:ModalPopupExtender runat="server" 
	ID="mpeStationGroupStationsPopup" 
	TargetControlID="dummysg" 
	PopupControlID="pnlStationGroupStationsHolder" 
	BackgroundCssClass="modalBackground"                        
	DropShadow="true"/> 
<input id="dummysg" type="button" style="display: none" runat="server" />
<asp:Panel ID="pnlStationGroupStationsHolder" CssClass="modalPopup" runat="server">

    <asp:UpdatePanel ID="pnlStationGroupStationsModal" runat="server" UpdateMode="Always"><ContentTemplate>
        <div class="pmt-station-groups">
	
		<div class="sub-hdr">
				Stations  
				<div class="sub-search-options">
					<div class="kw-search">
						<span class="icon"></span>
						<asp:TextBox ID="txtStationGroupStationSearch" runat="server" CssClass="StationKeyword" placeholder="keyword"></asp:TextBox>
					</div>
                    <asp:DropDownList ID="ddlStationGroupMarketSearch" runat="server"></asp:DropDownList>
                    <asp:Button ID="btnStationGroupStationSearch" runat="server" Text="Search Stations" OnClick="btnStationGroupStationSearch_Click" />
				</div>
				<span class="icon"></span>
			</div>
			
			<div class="table-container">
				<asp:GridView ID="gvStationGroupStations" OnSelectedIndexChanged="gvStationGroupStations_SelectedIndexChanged" PageSize="5" OnPageIndexChanging="gvStationGroupStations_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="table" runat="server">
                <HeaderStyle BackColor="#f7f7f7" />
                    <AlternatingRowStyle BackColor="#f7f7f7" />
                <Columns>
	            <asp:TemplateField HeaderText="Station Name">
	                <ItemTemplate>
		            <asp:HiddenField ID="hdngvStationGroupStationId" Value='<%#Eval("Id") %>' runat="server" /><asp:Label ID="lblgvStationName" Text='<%#Eval("StationName") %>' runat="server" />
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
			</div>
        
<h3>Stations in this Station Group - <asp:Label ID="lblStationGroupStationsModalMessage" runat="server"></asp:Label></h3>

	<asp:ListBox ID="lbxStationsInGroupModal" runat="server" Rows="6"></asp:ListBox>
    
	<asp:HiddenField Visible="false" ID="txtSelectedStationGroupStations" Value="-1" runat="server"></asp:HiddenField>
	<asp:HiddenField Visible="false" ID="txtStationGroupStationsCreatedBy" Value="-1" runat="server"></asp:HiddenField>
	<asp:HiddenField Visible="false" ID="txtStationGroupStationsCreatedDate" Value="" runat="server"></asp:HiddenField>
	<asp:Button ID="btnRemoveStationFromGroup" runat="server" CssClass="btn btn-red2" Enabled="true" Text="Remove Station From Group" OnClick="btnRemoveStationFromGroup_Click" /><br /><br />
<asp:Button ID="btnCancelStationGroupStationsPopup" OnClick="btnCancelStationGroupStationsPopup_Click" runat="server" Text="Close" /> 
            </div>
</ContentTemplate></asp:UpdatePanel>
    </asp:Panel>