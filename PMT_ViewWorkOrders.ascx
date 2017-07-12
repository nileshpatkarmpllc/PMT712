<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMT_ViewWorkOrders.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.PMT_ViewWorkOrders" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %> 
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ register tagprefix="dnn" tagname="Label" src="~/controls/LabelControl.ascx" %>

<dnn:DnnCssInclude ID="DnnCssInclude3" runat="server" FilePath="/DesktopModules/PMT_Admin/styles/style.css" Priority="11" />

<asp:UpdateProgress ID="updWorkOrders" AssociatedUpdatePanelID="pnlWorkOrders" runat="server">
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

.pmt-dsah {
	font-family:'Roboto', sans-serif;
}

.pmt-dsah .group-hdr {
	position:relative;
	padding:5px 30px;
	color:#FFF;
	text-transform:uppercase;
	background:#6291fc;
	-webkit-box-shadow: 0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	-moz-box-shadow:    0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	box-shadow:         0px 2px 5px 0px rgba(50, 50, 50, 0.25);
}
.pmt-dsah .group-content {
	padding:15px;
	background:#f7f7f7;
	-webkit-box-shadow: 0px 1px 2px 0px rgba(50, 50, 50, 0.50);
	-moz-box-shadow:    0px 1px 2px 0px rgba(50, 50, 50, 0.50);
	box-shadow:         0px 1px 2px 0px rgba(50, 50, 50, 0.50);
	overflow:hidden;
}

.pmt-dsah .sub-hdr {
	position:relative;
	padding:5px 10px;
	color:#3576be;
	text-transform:uppercase;
	-webkit-box-shadow: 0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	-moz-box-shadow:    0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	box-shadow:         0px 2px 5px 0px rgba(50, 50, 50, 0.25);
}
.pmt-dsah .sub-content{
	background:#FFF;
	overflow:hidden;
}
.pmt-dsah .sub-search-options {
	padding:10px 20px;
}


.pmt-dsah table-container{
	max-height:210px;
	overflow-y: scroll;
}
.pmt-dsah .sub-search-options label {
	margin:0 15px 0 0; 
	font-size:14px; 
	color:#666;
}
.pmt-dsah .sub-search-options input {
	min-width:25%;
	margin:0 15px 0 0;
}
.pmt-dsah .action-links {
	float:right;
	padding:15px 15px 0 15px;
}
.pmt-dsah .action-links a {
	padding:10px 15px; 
}

.pmt-dsah .pagination {
	margin:20px 0;
	color:#666;
	font-size:11px;
	list-style-type:none;
}
.pmt-dsah .pagination li {
	display:inline-block;
	padding:3px 5px;
}
.pmt-dsah .pagination li.cur {
	color:#FFF;
	background:#666;
}

.pmt-dsah table {
	display:table;
	width:100%;
	font-size:12px;
	line-height:12px;
	color:#747474;
	text-align:center;
	border-spacing: 1px;
	background:#dedede;
}
.pmt-dsah table .head {
	display:table-row;
	text-transform:uppercase;
	color:#000;
	background:#edf7ff;
}
.pmt-dsah table tr {
	display:table-row;
}
.pmt-dsah table tr:nth-child(even) {
	background: #FFF;
}
.pmt-dsah table tr:nth-child(odd) {
	background: #f7f7f7;
}
.pmt-dsah table td, th {
	display:table-cell;
	padding:8px;
    border:solid 1px #cccccc;
}
.pmt-dsah .kw-search{	
 
}
.pmt-dsah .kw-search .icon {	
	float:left;
	display:inline-block;
	width:17px;
	height:17px;
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABEAAAARCAYAAAA7bUf6AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAbNJREFUeNp8071Lm1EUx/EnxlpfqlZjrZtQilAJ8QVpl26KOJgM4gtItyIIDhZLrYLoIqjoYJdC1kZdREScCr4gLk5VULDQUvAPqKVW0aDEfq/8Eg4h8cIH7pPknufcc0580WjUMyuADoQRQj5+YAsx/ELCS1u5Zv8C82hN+81TvEYvxrCCm0xBarGKGj2f4Q9u8QgVeI5FZRezGeWgHJ9MgH30ox5N6MEaruDHHBrgs0E60ay3HiCCr+hGUPXowhKuldWAMkwFiWh/jhlc4Ave45m+c4encKznNpTZmgSV2m+s442KHFLA5PqJI33+BCU2k4faJ3Tn75hIC+CZjJLn/DYT1/sqFKMOO17m5WpQbbp3bjPZ0N7dcShtdux6i0bt9zQCqSALONHhdnw27fZ093cY1d51cRan9jpurD8qmHvuwyvVJq4ruHkpNIHdBG8rYCr1ZeRpkCrVgVCWa7lOftDZcfeBPxx2/7W7iIfYVNUD5s2nGrhBtdllWYSXeIBdW0QX6JuKW6Zu5agLfxUs2bkRFGAYpZk68U8yrUtMa6aG1fZwtnbet+IK9BgtmPwvwADbz1tf2VH6ogAAAABJRU5ErkJggg==') no-repeat;
}
.pmt-dsah .kw-search input{	
	margin:0 0 0 5px;
	border:solid 1px #cccccc;
}
.pmt-dsah .play-btn {
	display:inline-block;
	width:18px;
	height:18px;
	cursor:pointer;
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABIAAAASCAMAAABhEH5lAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAADBQTFRF6enp+fn5////5ubm7+/v5eXl1dXV2dnZ0tLSz8/P9vb239/f2NjY8vLyzMzM////TtOJoAAAABB0Uk5T////////////////////AOAjXRkAAACNSURBVHjaXJCJEsMgCETpEfHg+P+/7YI0TcqMozxdZCGP0NXM2tJMCGuyVfDcaA4kXaRjGzNQEEmJSjJy7MsrFrROijd4XBT3SriA6rDxSi0k1Kx7IPzwxqFbo9QlQp1HKG9INroItYRn+eMsr9HKvYn/VqUM8TbEZahsM/9sg8l3OFLDuYzwGclHgAEA0HMQIKkPIY4AAAAASUVORK5CYII=') no-repeat;
}
.pmt-dsah .play-btn:hover {
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABIAAAASCAMAAABhEH5lAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAADBQTFRFYv8fWP8Qbf8vV/8PkP9gY/8gbv8w8P/qpf9/ef9Aj/9f3f/Pef8/0v+/Tf8A////MRZddgAAABB0Uk5T////////////////////AOAjXRkAAACcSURBVHjaVJCJEsMgCETxSjw4/v9vs6BtU2bU4emuAJmH1Ev1qhIJYa2hJ8baaCUkxEw40nLkhEMiHIwM57QTE1ojwRuzXjfDvRAuoOqaemghoaz+aYd1c0PSi0IXCD63K/8Qb5S1HURbmL/25WcvXor1+SrC9/oqlb2hgne7IbRfVrRdvKgxmgbZw+HPcPgMxyUzq+YptyePAAMAhtAPxt5Zn3sAAAAASUVORK5CYII=') no-repeat;
}
.pmt-dsah input, .pmt-dsah select {
	padding:4px;
	color:#666;
    border:solid 1px #cccccc;
    margin-left:6px;
}
.pmt-dsah option {
	background:#FFF;
	color:#666;
}
.pmt-dsah .btn {
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

.pmt-dsah .btn-blue {
	color:#FFF;
	background:#3498db;
}
.pmt-dsah .btn-blue:hover {
	background:#9fa8da;
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
.col25 {
	width:49%;
    float:left;
}
.col33 {
	width:28%;
    float:left;
}
.col50 {
	width:45%;
    float:left;
}
.col75 {
	width:50%;
    float:left;
}
.col100 {
	width:95%;
    float:left;
}
.altRow {
        background-color:#e3e3e3;
        margin-bottom:5px;
        margin-top:5px;
        padding:5px;
    }
.Row {
        background-color:#ffffff;
        margin-bottom:5px;
        margin-top:5px;
        padding:5px;
    }
.cell1, .cell2 {
	display:table-cell;
    background-color:white;
	padding:2px;
    display:table-cell;
    width:20%;
    border:1px solid #e3e3e3;
}
.cell3{
	display:table-cell;
    background-color:white;
	padding:2px;
    display:table-cell;
    width:60%;
    border:1px solid #e3e3e3;
}
.cell10, .cell20, .cell30, .cell5{
	display:table-cell;
    background-color:white;
	padding:2px;
    border:1px solid #e3e3e3;
}

.table-header1, .table-header2 {
    background-color:#e3e3e3;
    border:1px solid white;
    display:table-cell;
    width:20%;
    padding:2px;
}
.table-header3 {
    background-color:#e3e3e3;
    border:1px solid white;
    display:table-cell;
    width:60%;
    padding:2px;
}
.table-header10, .table-header20, .table-header30, .table-header5 {
    background-color:#e3e3e3;
    border:1px solid white;
    padding:2px;
    display:table-cell;
}
.table-header5, .cell5 {
    width:3%;
}
.table-header10, .cell10 {
    width:8%;
}
.table-header10, .cell10 select {
    max-width:120px;
}
.table-header20, .cell20 {
    width:17%;
}
.table-header30, .cell30 {
    width:30%;
}
.rowRow {
    display:table-row;
}
.tableTable {
    display:table;
    width:100%;
    border-collapse:separate;
}
    .taskInput {
    width: 90%
    }
.Comment div {
    background-color: white;
}
.SystemMessage div {
    background-color: #ddffdd;
}
.Error div {
    background-color: #ffdddd;
}
.pmt-dsah .kw-search input[type="checkbox"] {
    width:12px;
    min-width:12px;
    margin-right:4px;
}
.imagePage {
    page-break-after: always;
    width: 5in;
    margin:0px;
    padding:0px;
}
.imagePage:last-of-type {
      	page-break-after: avoid;
   }
</style>
<div id="divScreen">
<div id="PMT_WorkOrders">
    <asp:UpdatePanel ID="pnlWorkOrders" runat="server" UpdateMode="Always" visible="true">
        <ContentTemplate>
<div class="pmt-dsah">
	
	
		
		<div class="sub-hdr">
			<asp:Label ID="lblMessage" runat="server"></asp:Label>
			<span class="icon"></span>
		</div>
		<div class="sub-content">
			
			<div class="sub-search-options">
				<div class="kw-search">
					<span class="icon"></span><asp:TextBox ID="txtKeyword" runat="server" placeholder="Keyword: Description, Invoice, PO #" AutoPostBack="true" OnTextChanged="txtKeyword_TextChanged"></asp:TextBox><asp:DropDownList ID="ddlAdvertisers" AutoPostBack="true" OnSelectedIndexChanged="ddlAdvertisers_SelectedIndexChanged" runat="server"></asp:DropDownList>
                <asp:DropDownList ID="ddlAgencies" AutoPostBack="true" OnSelectedIndexChanged="ddlAgencies_SelectedIndexChanged" runat="server"></asp:DropDownList>
                <asp:CheckBox ID="chkShowMine" runat="server" Text="Assigned to Me" AutoPostBack="true" OnCheckedChanged="chkShowMine_CheckedChanged" />
				</div>
			</div>
			
			<div class="table-container">
                <asp:GridView ID="gvMasters" OnSelectedIndexChanged="gvMasters_SelectedIndexChanged" OnPageIndexChanging="gvMasters_PageIndexChanging" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" PagerSettings-FirstPageText="<<" PagerSettings-PreviousPageText="<" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast" AllowSorting="true" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="table" runat="server">
                    <HeaderStyle BackColor="#f7f7f7 " CssClass="" />
                    <AlternatingRowStyle BackColor="#f7f7f7" />
                    <Columns>
	                <asp:TemplateField HeaderText="Id">
	                    <ItemTemplate>
		                <asp:HiddenField ID="hdngvMasterItemId" Value='<%#Eval("Id") %>' runat="server" /><span class="cell"><%#Eval("Id") %></span>
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="Advertiser">
	                    <ItemTemplate>
		                <span class="cell"><%#Eval("AdvertiserName") %></span>
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="Agency">
	                    <ItemTemplate>
		                <span class="cell"><%#Eval("AgencyName") %></span>
	                    </ItemTemplate>
	                </asp:TemplateField>
                        <asp:TemplateField HeaderText="InvoiceNumber">
	                    <ItemTemplate>
		                <span class="cell"><%#Eval("InvoiceNumber") %></span>
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="Status">
	                    <ItemTemplate>
		                <span class="cell"><%#Eval("Status") %></span>
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="Date Created">
	                    <ItemTemplate>
		                <span class="cell"><%# Convert.ToDateTime(Eval("DateCreated")).ToString("d") %></span>
	                    </ItemTemplate>
	                </asp:TemplateField>
                        <asp:TemplateField HeaderText="Add To Invoice">
    	                    <ItemTemplate>
    		                <span class="cell"><asp:LinkButton ID="lbtnAddToInvoice" CommandArgument='<%#Eval("Id") %>' OnClick="lbtnAddToInvoice_Click" runat="server" Visible='<%# Eval("InvoiceNumber")=="" ? true : false %>' Text="Add To Invoice"></asp:LinkButton></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="View">
	                    <ItemTemplate>
		                <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text="View" />
	                    </ItemTemplate>
	                </asp:TemplateField>
                    </Columns>
                </asp:GridView>
			</div>

		</div>
    <asp:HiddenField Visible="false" ID="txtSelectedWorkOrder" Value="-1" runat="server"></asp:HiddenField>
		<div class="action-links">
			<a class="btn btn-blue" href="/Work-Orders">Create New Work Order</a>
		</div>
		<br clear="both" /><br />
    <asp:Panel ID="pnlDetailView" runat="server" Visible="false">
        <div class="sub-hdr">
			WORK ORDER DETAILS
			<span class="icon"></span>
		</div><br />
		<div class="sub-content">
            <div class="Row"><div class="col33">Description: <asp:Label ID="lblDescription" runat="server"></asp:Label></div>
            <div class="col33">Advertiser: <asp:Label ID="lblAdvertiser" runat="server"></asp:Label></div>
            <div class="col33">Agency: <asp:Label ID="lblAgency" runat="server"></asp:Label></div><br clear="both" /></div>
            <div class="altRow"><div class="col33">Priority: <asp:DropDownList ID="ddlPriority" runat="server">
                <asp:ListItem Text="Normal" Value="Normal"></asp:ListItem>
                <asp:ListItem Text="High" Value="High"></asp:ListItem>
            </asp:DropDownList></div>
            <div class="col33">Assign To: <asp:DropDownList ID="ddlUsers" runat="server"></asp:DropDownList></div>
            <div class="col33">Status: <asp:DropDownList ID="ddlStatus" runat="server">
                <asp:ListItem Text="Ready For Review" Value="READY FOR REVIEW"></asp:ListItem>
                <asp:ListItem Text="New" Value="NEW"></asp:ListItem>
                <asp:ListItem Text="In Progress" Value="PENDING"></asp:ListItem>
                <asp:ListItem Text="Complete" Value="COMPLETE"></asp:ListItem>
                <asp:ListItem Text="Invoiced" Value="INVOICED"></asp:ListItem>
                <asp:ListItem Text="Cancelled" Value="CANCELLED"></asp:ListItem>
            </asp:DropDownList></div><br clear="both" /></div>
            <div class="Row">WORK ORDER TASKS<br />
                <div class="col100">
                    <div class="tableTable">
                    <div class="rowRow"><div class="table-header5">Task Id</div>
                    <div class="table-header10">PMT Id</div>
                    <div class="table-header10">Type</div>
                    <div class="table-header10">ISCI</div>
                    <div class="table-header10">Title</div>
                    <div class="table-header10">Description</div>
                    <div class="table-header10">Station</div>
                    <div class="table-header10">Delivery Method</div>
                    <div class="table-header10">Delivery_Method Id</div>
                    <div class="table-header10">Delivery Status</div>
                    <div class="table-header10">Task Status</div>
                    <div class="table-header10">QB Codes</div></div>
                    <asp:PlaceHolder ID="plTasks" runat="server"></asp:PlaceHolder>
                </div></div><br clear="both" /><br />
                <asp:Button ID="btnUpdateWorkOrder" runat="server" Text ="Update Work Order" CssClass="btn btn-blue" OnClick ="btnUpdateWorkOrder_Click" /> <asp:Button ID="btnViewWorkOrder" runat="server" Text ="View/Print WorkOrder" CssClass="btn btn-blue" OnClick="btnViewWorkOrder_Click" /> <asp:Button ID="btnCreateLabels" runat="server" Text="Create Labels" CssClass="btn btn-blue" OnClick="btnCreateLabels_Click" /> <asp:Button ID="btnProcessAPIs" runat="server" Text="Process API Tasks" CssClass="btn btn-blue" OnClick="btnProcessAPIs_Click" />
                <asp:Button ID="btnPrint" runat="server" Text="Print Shipping Labels" OnClientClick="window.print();" CssClass="btn btn-blue" Visible="false" /><br />
                <asp:Label ID="lblAPIMessages" runat="server"></asp:Label>
            </div>
            <div class="altRow">
                <div class="col100">
                Comments: <asp:TextBox ID="txtComment" runat="server"></asp:TextBox> <asp:Button ID="btnComment" runat="server" Text="ADD" OnClick="btnComment_Click" />
                Filter by Type: <asp:DropDownList ID="ddlCommentTypes" AutoPostBack="true" OnSelectedIndexChanged="ddlCommentTypes_SelectedIndexChanged" runat="server">
                    <asp:ListItem Text="ALL" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="User Comments" Value="Comment" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="System Messages" Value ="SystemMessage"></asp:ListItem>
                    <asp:ListItem Text="Error Messages" Value="Error"></asp:ListItem>
                      </asp:DropDownList>
                <!--Task: <asp:DropDownList ID="ddlTasks" AutoPostBack="true" OnSelectedIndexChanged="ddlTasks_SelectedIndexChanged" runat="server"></asp:DropDownList>-->
                User: <asp:DropDownList ID="ddlUserFilter" AutoPostBack="true" OnSelectedIndexChanged="ddlUserFilter_SelectedIndexChanged" runat="server"></asp:DropDownList>
                </div>
                <div class="col100" style="max-height:400px; overflow-y:scroll;overflow-x:hidden;">
                    <asp:Literal ID="litComments" runat="server"></asp:Literal>
                </div><br clear="both" />
            </div>
        </div>

    </asp:Panel>
    <h3>Create Invoice</h3>
    <asp:ListBox ID="lbxWOsToInvoice" runat="server" Width="300" Height="100"></asp:ListBox><br />
    <asp:Button ID="btnProcessInvoice" runat="server" Text="Create Invoice" CssClass="btn btn-blue" OnClick="btnProcessInvoice_Click" />
    <asp:Button ID="btnClearInvoiceItem" runat="server" OnClick="btnClearInvoiceItem_Click" Text="Clear WO From Invoice" CssClass="btn" /><br />
    <asp:Label ID="lblInvoiceMessage" runat="server"></asp:Label>
</div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </div>
<ajaxToolkit:ModalPopupExtender runat="server" 
	        ID="mpeStationAddressesPopup" 
	        TargetControlID="dummytape" 
	        PopupControlID="pnlStationAddressesHolder" 
	        BackgroundCssClass="modalBackground"                        
	        DropShadow="true"/> 
        <input id="dummytape" type="button" style="display: none" runat="server" />
        <asp:Panel ID="pnlStationAddressesHolder" CssClass="modalPopup" runat="server">
            <asp:UpdatePanel ID="pnlStationAddressesPopup" runat="server" UpdateMode="Conditional"><ContentTemplate>
        <div class="pmt">
	        
		<div class="sub-hdr">
			Verify Shipping Addresses <asp:Label ID="lblStationAddressesMessage" runat="server"></asp:Label>
			<span class="icon"></span>
		</div>
		<div class="sub-content">
            <asp:PlaceHolder ID="pnlAddresses" runat="server"></asp:PlaceHolder>
            </div>
            <br /><br />
            <asp:Button ID="btnCancelTapeFormatPopup" CssClass="btn btn-default" OnClick="btnCancelTapeFormatPopup_Click" runat="server" Text="Close" /> 
            <br clear="both" /><br /></div>
            </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
</div>

<div id="divPrint">
    <asp:UpdatePanel ID="pnlPrint" runat="server" UpdateMode ="Always"><ContentTemplate>
    <asp:PlaceHolder ID="plPrintShippingLabels" runat="server"></asp:PlaceHolder></ContentTemplate></asp:UpdatePanel>
    </div>