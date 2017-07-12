<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMT_SidePanel.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.PMT_SidePanel" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %> 
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ register tagprefix="dnn" tagname="Label" src="~/controls/LabelControl.ascx" %>

<html><head>
<meta http-equiv="content-type" content="text/html; charset=windows-1252"><style>

.pmt-dashboard-panel {
	position:relative;
	font-family:'Open Sans', sans-serif;
	font-size:14px;
	line-height:17px;
	min-width:200px;
	background:#FFF;
}

.pmt-dashboard-panel .welcome {
	position:relative;
	margin:0 0 -1px 0;
	padding:15px 50px;
	color:#FFF;
	background:#578eec;
	-webkit-box-shadow: 0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	-moz-box-shadow:    0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	box-shadow:         0px 2px 5px 0px rgba(50, 50, 50, 0.25);
}

.pmt-dashboard-panel .welcome-title {
	margin:0;
	padding:0;
	font-size:20px;
	line-height:22px;
	font-weight:normal;
}
.pmt-dashboard-panel .welcome-user {
	margin:0;
	padding:0;
	font-size:14px;
	line-height:17px;
	font-weight:normal;
}

.pmt-dashboard-panel .toggle-hdr, .pmt-dashboard-panel .hdr {
	padding:10px;
	color:#FFF;
	text-transform:uppercase;
	text-align:center;
	cursor:pointer;
	border-top:1px solid #FFF;
}
.pmt-dashboard-panel .toggle-hdr .icon {
	float:right;
	display:inline-block;
	margin:3px 0 0 0;
	width: 12px;
	height:12px;	
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAwAAAAICAYAAADN5B7xAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAKBJREFUeNpi+P//fyQQ/wDiJ0DsD8SMQMyAhBWBeAdUzUaQwNz/CLAbiJWQFDMDcRNUMQj8BgnG/0cFG4FYHKqhAog/I8ltBwlyAXEDkuAfIF4JxDVA/ADNdlWY1bxA3IUk+RuIvwLxPyj/MBBrg9Qie04AiCf9xwQngNgYpo4BLUSEgHgOkuILQGyOHHLoGkBYFIg3A/ELIHZED2aAAAMAjYVOQLammzIAAAAASUVORK5CYII=') no-repeat;
}
.pmt-dashboard-panel .toggle-hdr.open .icon {	
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAcAAAAMCAYAAACulacQAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAJVJREFUeNpi/P//PwMSYARiayCOBeL1DCBJKGYEYicgvv0fAv4gS9gA8bX/CPAJJmkCxOeRJN4CcRhIQgeIjyNJvAfieLBbgMQBJIk3QJwGcwfDf1SwCog5YZJMQCcfR/KKERCHAzEzmAdUoQvEJ9HsjIUZC8LmQHwRScFLIA5GDgR7IL6B7DhkSRB2BeLHUMlvAAEGANAl/ckc0VquAAAAAElFTkSuQmCC') no-repeat;
}

.pmt-dashboard-panel .toggle-content .kw-search{	
	padding:5px 10px;
}
.pmt-dashboard-panel .toggle-content .kw-search .icon {	
	float:left;
	display:inline-block;
	width:17px;
	height:17px;
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABEAAAARCAYAAAA7bUf6AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAbNJREFUeNp8071Lm1EUx/EnxlpfqlZjrZtQilAJ8QVpl26KOJgM4gtItyIIDhZLrYLoIqjoYJdC1kZdREScCr4gLk5VULDQUvAPqKVW0aDEfq/8Eg4h8cIH7pPknufcc0580WjUMyuADoQRQj5+YAsx/ELCS1u5Zv8C82hN+81TvEYvxrCCm0xBarGKGj2f4Q9u8QgVeI5FZRezGeWgHJ9MgH30ox5N6MEaruDHHBrgs0E60ay3HiCCr+hGUPXowhKuldWAMkwFiWh/jhlc4Ave45m+c4encKznNpTZmgSV2m+s442KHFLA5PqJI33+BCU2k4faJ3Tn75hIC+CZjJLn/DYT1/sqFKMOO17m5WpQbbp3bjPZ0N7dcShtdux6i0bt9zQCqSALONHhdnw27fZ093cY1d51cRan9jpurD8qmHvuwyvVJq4ruHkpNIHdBG8rYCr1ZeRpkCrVgVCWa7lOftDZcfeBPxx2/7W7iIfYVNUD5s2nGrhBtdllWYSXeIBdW0QX6JuKW6Zu5agLfxUs2bkRFGAYpZk68U8yrUtMa6aG1fZwtnbet+IK9BgtmPwvwADbz1tf2VH6ogAAAABJRU5ErkJggg==') no-repeat;
}
.pmt-dashboard-panel .toggle-content .kw-search input{	
	margin:0 0 0 5px;
	width:80%;
	border:none;
}

.pmt-dashboard-panel .toggle-content {
	display:none;
}
.pmt-dashboard-panel .toggle-content .sort .calendar {
	display:inline-block;
	padding:5px 2% 10px 2%;
	width:44%;
	font-size:12px;
	line-height:20px;
	color: #747474;
	text-align:center;
}
.pmt-dashboard-panel .toggle-content .sort .calendar .icon{
	display:inline-block;
	margin:0 5px;
	width: 20px;
	height:20px;
	vertical-align:middle;
}


.pmt-dashboard-panel .toggle-hdr.summary {
	background:#a0c3ff;
}
.pmt-dashboard-panel .toggle-hdr.summary:hover {
	background:#99b9f2;
}

.pmt-dashboard-panel .toggle-popup.summary {
	display:none;
	position:absolute;
	width:100%;
	-webkit-box-shadow: 0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	-moz-box-shadow:    0px 2px 5px 0px rgba(50, 50, 50, 0.25);
	box-shadow:         0px 2px 5px 0px rgba(50, 50, 50, 0.25);
}

.pmt-dashboard-panel .toggle-popup.summary .icon {
	float:left;
	display:inline-block;
	margin-right:10px;
	width:35px;
	height:35px;
}
.pmt-dashboard-panel .toggle-popup.summary .icon.work {
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACMAAAAgCAYAAACYTcH3AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAlhJREFUeNrMl79LHEEUx2929/ZEQWOKgEga0YTYSIKmMkWakEoIRkFsAiGpki6QxkILk5BCLQQL/wHhWkEkBCE2YiEphTSxE0IUFWI8z518B74Lw7HrzmRnYR98uF/D7Hfe+86bOSGlbKuUJAKDMRFoWsznA1mUmEvQAUTGOCX4nGLC/xGUJeYKPAVTwEsZIyn0L/gC6uAC1GwFBQYlegWeGc43AfrAJ4prsxGUJUZoK9wC66DKUqiH9YKXHLMP7oNZZvGDrSATz8Tm3QaLLb8NgOegG8yDEfAezPD3jxRUMxHjGYyJjetrn0O+r3IOtajfYA6sMhszxOcmcJKZJFF+wmLauZvesSyvKUb57jNfvbyZMY149afgLViiKOWhcZNelVeM0Oa5AwbBQ9AP1sAOszilZdNJmdKiSo8sa7tGMFMRP3ebLDzIkRG10kPwDTzgw4QmKKKIGptnIQZuavX/xT5zu6Uje8zYC/DG1A42YiIKugtugga/b2jmlczECfgJfrjswHqo5tXJRjZ6TepV+b6DaddnU+tDLrhDzvk+KWrMyB9bT9oMDpmdhYw7i2BJG9xlhYi5ohGHwK2MMh2DXYtLmbUYVZYbbO2PtR6SlJk9MMZyFlamM7ACvrJkaZ45AEfagepcTJNbuG7ZDgoRo+4uj+ibrCYm6al7Wmd2IibkZJPgieHkklnp1C7oucWoSTfBMCfs0r43PTrUztqguGtPbpHxJ07yjOkxTXVKHHI3ijxi4v5yWckXgYklTDzjm1yMXIRXKVGUSkyQ05jOxciyiPknwABCnIbWezzYxQAAAABJRU5ErkJggg==') center center no-repeat;
}
.pmt-dashboard-panel .toggle-popup.summary .icon.pending {
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABoAAAAZCAYAAAAv3j5gAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAo5JREFUeNq8lk+ITXEUx3/3vvveu1f5M6OoaRZGsaAs2IkiGkpYSVmwUpKdsrBRTJaztJANWc4wTdhQmhgNEbFAUbMSQ0QM5s2b63PG99bt131/8ianPs197/7mnN/5ne85vxekabrIOZe6zs18/NJzBUp6DuxdpAWdBDJHdZiBXlgMr/V9kG0i7DCLQAGMjXAFhmA3zOYXdhLIHP2Uj+MwDNthHRzyF4cF59yK36qF1WEv3IJBSGAUvsOS3LHNW6S/dsZz/suCTZVhDeyA/bBVwSfgJJiwNhfVPNIRWAH7oU8BfYuhBzbBWq3/Co/gAozAD/nIfNZzicw/1LSb023U5QO8godwE277DmWWWVWb/itz+iiAibS1PYCdsAKcqEBivajP/fAJvsApvTfiUHWptZHNJNyFKR1loroV9eEyGIBu+Z7LFkZtBFoq506qq+V6qcjGYDo7umwyTLURaAtcgmcwDvdzTZl4Tfwcjknq1WwEmZ3XMfSpuL1K2+UyMKcHhW3sJVyGa1JgFsT8vIG3CuIyMcRQgi5YCatgNCcCK+5RWA37YADG9W4aHsMRiWobfIbrEMm3iSV2eqjqhX3uhnu5QDe0iUxptm457IIhmBFX4QR8hJFGgeJcZuZkLBfoKaxXkNjbVCDJ2/qaMBv2A/mzrqQOn/TUVvWKHUniNu/uaFqfhW9S4xNf8lGDsW+j5bC+69JEfqFOD731icRyTiIwQV30B7alnuSiZ9N5gwZlJltT1QGNnGpB72RNP5sbwBX/hvX/oaydnYE9mlvv1Hxhk2s88q5v1ywj510bPcrgvQKVW1wljW7htFEgp6Ooe5mG/3jdp81mXFksiIXuP1m4QL/pWv3ec38EGAAOce4+EJ3KeAAAAABJRU5ErkJggg==') center center no-repeat;
}
.pmt-dashboard-panel .toggle-popup.summary .icon.billed {
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACMAAAAZCAYAAAC7OJeSAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAktJREFUeNrMls9LFkEYx3d0Xtv3tYLSU5gI2sGDQtAhELyImjclPIYEXSSSbkF0CfwbAhG6awTRoYMnvQgexBITOkkR8hqh8Bavr72tb98HvgvLy+7szPvuKz3wYWdnZ5955vkxM6pWq+W9/0Q0qFmOrYLAUb8CHXxaGWOj8A/bfaDNcgEy7gf4BfysjBGPXAEvwQiNczHmOdix8VCaMTLpXzAKFhpMhS/gM0OsmzVGXDzH9zOwC36CXMI/5/TCMLgOZsAy2GvWGPHKHTDF949gmnmgDcbIIl6AZ+AmGAf77FemuJpEFD8EeRr2FhSZR6cJSLL/Bu/AN+oRHV3U57kaI9ZXwC0wyb4DsMJ2O/+NQ/G5BTY4fghMcBHOxoQruA962f7AleYskjbHkLwBx+x7QA8HLsaE+8o15od44QSsRiZKk9Bz62CbfSOkmpQ3bQkVJNyje0Xeg022y8wNL0apYm6V6V1J9NegBC6DWS4msK2mcKBUUIHvn8AgS1aUHYFD/q/qqs9naDvo4SLHXwVjoJt9fv3mqRM8I9IT8d5jMM92O417Ar5HtvqAIZB9ZZHjAnKDY6SiOpN2cJ1yJoXP/rpvckatgVeRkIWTPWUVeoaFOm96aefPI5Z/iXokRLfB3UZ1aq9xkYmX6ibQtteFrI3J4n+n4+BCxWRMoQXz5U1zakMVyZE/YJnMtlfQr9wQYy9oChdyPybjA+4zgw7XTJsoHPIqcR7noThjQqm0KDUuJVWcqRr8jDwS9bgylb62iHNWkqrrnwADAFA0iTNdDlDbAAAAAElFTkSuQmCC')  center center no-repeat;
}


.pmt-dashboard-panel .toggle-popup.summary .table {
	border-spacing: 0 1px;
	background:#FFF;
}
.pmt-dashboard-panel .toggle-popup.summary .row {
	margin-top:1px;
	
}
.pmt-dashboard-panel .toggle-popup.summary .cell {
	padding:15px;
	color:#FFF;
	font-size:16px;
	line-height:35px;
	text-align:left;
	background:#76a7fa;
}
.pmt-dashboard-panel .toggle-popup.summary .amount {
	width:10%;
	background:#638de0;
}

.pmt-dashboard-panel .hdr.open-close{
	padding:0;
	background:#7a7676
}
.pmt-dashboard-panel .hdr.open-close a {
	display:inline-block;
	padding:10px 2%;
	width:44%;
	color:#FFF;
	text-align:center;
}
.pmt-dashboard-panel .hdr.open-close a:hover {
	background:#a6a6a6;
}

.pmt-dashboard-panel .toggle-hdr.new-pending{
	background:#8f48a5;
}
.pmt-dashboard-panel .toggle-hdr.new-pending:hover{
	background:#a82db6;
}


.pmt-dashboard-panel .toggle-hdr.work-orders{
	background:#38a173;
}
.pmt-dashboard-panel .toggle-hdr.work-orders:hover{
	background:#25c27e;
}
.pmt-dashboard-panel .toggle-content.work-orders .calendar .icon{
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAIAAAAC64paAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAU9JREFUeNpiLL2wgoFcwMRAARiimlmAWE9ANkbeCsL/8+/v219fuFnYeVg43v78AhQRZuf58ufH1z8/hdl4gFyI7Ian5y59eIxuM1Cu9+aO46/vANkL7h8GIiADyAUKAqWQZaE2IwOgqa5i2ko8YkC2pYgqRBDIdQVLAdkQ2ZfvPmHRDHStq6QOhG0logJhKPOKARGEDZE9/g5kObN1Rog4B/+XPz9XPTqpziv5/e+v6Xf2MjAwynIJLbh/5MKHRwaCcsfe3EGTff/768sfnygKbUZg8kQObSLBkofHEKF99/Or3c+vAKMEiIAMIBcoCHQtEOGSRSSSe19e7X51FRiZQARkALmgUHlzG4hwySKcHSBthJYMiEkkLKSlRyZmYNSgp22saQgzhWFJ21jTEK4Uhh5VfKycCtwiJLn/wdc3n35/B9kMpIC+JyORAAQYAMkO+PZN722HAAAAAElFTkSuQmCC') no-repeat;
}

.pmt-dashboard-panel .toggle-hdr.orders-pending{
	background:#ed9d97;
}
.pmt-dashboard-panel .toggle-hdr.orders-pending:hover{
	background:#e09590;
}

.pmt-dashboard-panel .toggle-hdr.invoices{
	background:#5a57b1;
}
.pmt-dashboard-panel .toggle-hdr.invoices:hover{
	background:#6659e0;
}
.pmt-dashboard-panel .toggle-content.invoices .calendar .icon{
	background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAIAAAAC64paAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAATdJREFUeNpinL/8DgO5gImBAjBENbMAsYIst4O1BIT/9++/z19+c7Azc3CwfPryCyjCx8P248efHz//8vKwArkQ2RNn3zx4/BXdZqDchu1Prt/5BGTvPfQSiIAMIBcoCJRCloXajAyApurrCEqIcQLZmqp8EEEQVwckBWRDZD9+/IVNMweLoY4QhK2hyg9hSIpxSoKNAwKI7M3bHxEBduP2x/XbHgM9CURAxg2w3N5Dz4EIlyyloc0ITJ7IoU0kOHD0BSK0n7/6fv7KO1CU/PgDZAC5ENdCXIhVFuHsF6++X7zyHhiZQARkvABLX7/9CYhwySKcbWEsgpYMiEkkLCR5lZmZSYCfHT1tY01DmCkMS9rGmoZwpTD0qOLiZBETYSfJ/a/e/Pz2/Q/IZiD14PEfMhIJQIABAFou/zhdBGMWAAAAAElFTkSuQmCC') no-repeat;
}


.pmt-dashboard-panel .toggle-hdr.contact-pnl{
	background:#8f8c8c;
}
.pmt-dashboard-panel .toggle-hdr.contact-pnl:hover{
	background:#a6a6a6;
}

.pmt-dashboard-panel .table {
	display:table;
	width:100%;
	font-size:10px;
	line-height:12px;
	color:#747474;
	text-align:center;
	border-spacing: 1px;
	border:1px solid #dedede;
}
.pmt-dashboard-panel .table .head {
	display:table-row;
	text-transform:uppercase;
	color:#FFF;
	background:#a3a3a3 !important;
}
.pmt-dashboard-panel .table .head th {
    padding: 5px !important;
    border:1px solid white;
    text-align:center;
}
.pmt-dashboard-panel .table .row {
	display:table-row;
}
.pmt-dashboard-panel .table .row:nth-child(even) {
	background: #FFF
}
.pmt-dashboard-panel .table .row:nth-child(odd) {
	background: #edf7ff
}
.pmt-dashboard-panel .table .cell {
	display:table-cell;
	padding:5px;
}
.pmt-dashboard-panel .table td {
	display:table-cell;
    border:1px solid white;
}
.caption, th, td
{
    border:1px solid white;
}
</style>


</head><body><div class="pmt-dashboard-panel">
	
	
	<!-- WELCOME HDR -->
	<div class="welcome">
		<span class="welcome-title">Hello <asp:Label ID="lblDisplaName" runat="server"></asp:Label></span>
	</div>
	
	

	
	
	<!-- OPEN/CLOSE ALL -->
	<div class="hdr open-close">
		<a class="open-all">Open All</a>
		<a class="close-all">Close All</a>
	</div>
	
	
	<!-- NEW MASTERS PENDING -->
	<div class="toggle-hdr new-pending open">
		Recent Masters
		<span class="icon"></span>
	</div>
	<div style="display: block;" class="toggle-content new-pending">
		<div class="table">
            <asp:GridView ID="gvMasterItem" OnSelectedIndexChanged="gvMasterItem_SelectedIndexChanged" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" AllowSorting="false" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="table" PagerSettings-Visible="false" runat="server">
                        <HeaderStyle BackColor="#edf7ff" CssClass="head" />
                <RowStyle CssClass="row" />
                        <Columns>                        
    	                <asp:TemplateField HeaderText="Id">
    	                    <ItemTemplate>
    		                <span class="cell"><asp:HiddenField ID="hdngvMasterItemId" Value='<%#Eval("Id") %>' runat="server" /><asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text='<%#Eval("PMTMediaId") %>'></asp:LinkButton></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Title">
    	                    <ItemTemplate>
    		                <span class="cell"><asp:LinkButton ID="lbtnSelect2" runat="server" CommandName="Select" Text='<%#Eval("Title") %>'></asp:LinkButton></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
    	                <asp:TemplateField HeaderText="Status">
    	                    <ItemTemplate>
    		                <span class="cell"><%# Eval("CheckListForm")=="" ? "NEW" : "" %><%# Eval("CheckListForm")!="" && !Boolean.Parse(Eval("isApproved").ToString()) ? "PENDING" : "" %><%# Boolean.Parse(Eval("isApproved").ToString()) ? "APPROVED" : "" %></span>
    	                    </ItemTemplate>
    	                </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
			
		</div>
	</div>
	
	
	<!-- ALL WORK ORDERS -->
	<div class="toggle-hdr work-orders open">
		Recent Work Orders
		<span class="icon"></span>
	</div>
	<div style="display: block;" class="toggle-content work-orders">
		<div class="table">
            <asp:GridView ID="gvWorkOrders" OnSelectedIndexChanged="gvWorkOrders_SelectedIndexChanged" DataKeyNames="Id" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" AllowSorting="false" RowStyle-Height="2" RowStyle-Wrap="true" ShowHeader="true" CssClass="table" PagerSettings-Visible="false" runat="server">
                        <HeaderStyle BackColor="#edf7ff" CssClass="head" />
                <RowStyle CssClass="row" />
                    <Columns>
	                <asp:TemplateField HeaderText="Id">
	                    <ItemTemplate>
		                <asp:HiddenField ID="hdngvMasterItemId" Value='<%#Eval("Id") %>' runat="server" /><span class="cell"><asp:LinkButton ID="lbtnSelect3" runat="server" CommandName="Select" Text='<%#Eval("Id") %>'></asp:LinkButton></span>
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="Advertiser">
	                    <ItemTemplate>
		                <span class="cell"><asp:LinkButton ID="lbtnSelect4" runat="server" CommandName="Select" Text='<%#Eval("AdvertiserName") %>'></asp:LinkButton></span>
	                    </ItemTemplate>
	                </asp:TemplateField>
	                <asp:TemplateField HeaderText="Status">
	                    <ItemTemplate>
		                <span class="cell"><%#Eval("Status") %></span>
	                    </ItemTemplate>
	                </asp:TemplateField>
                    </Columns>
                </asp:GridView></div>
	</div>

	<!-- CONTACTS contact-pnl-->
	<div class="toggle-hdr invoices open">
		Contacts
		<span class="icon"></span>
	</div>
	<div style="display: block;" class="toggle-content contact-pnl">
		<div class="table">
			<div class="head">
				<span class="cell">Name</span>
				<span class="cell">Number</span>
				<span class="cell">Email</span>
			</div>
			<div class="row">
				<span class="cell">Yeni Birrueta</span>
				<span class="cell">818-643-6370</span>
				<span class="cell"><a href="mailto:yenib@pmtmedia.tv">yenib@pmtmedia.tv</a></span>
			</div>
			<div class="row">
				<span class="cell">Tom Stavropoulos</span>
				<span class="cell">818-643-6364</span>
				<span class="cell"><a href="mailto:tomS@pmtmedia.tv">tomS@pmtmedia.tv</a></span>
			</div>
			<div class="row">
				<span class="cell">Leo Magana</span>
				<span class="cell">818-643-6373</span>
				<span class="cell"><a href="mailto:LeoM@pmtmedia.tv">LeoM@pmtmedia.tv</a></span>
			</div>
			<!--<div class="row">
				<span class="cell">Jessica Smith</span>
				<span class="cell">818-643-6362</span>
				<span class="cell"><a href="mailto:jessicas@pmtmedia.tv">jessicas@pmtmedia.tv</a></span>
			</div>-->
		</div>
	</div>


	<script type="text/javascript">
	    $(function () {

	        var $elm = $('.pmt-dashboard-panel');

	        $('.toggle-hdr', $elm).click(function () {
	            $(this).toggleClass('open');
	            $(this).next().toggle();
	        });

	        $('.open-all', $elm).click(function () {
	            $('.toggle-hdr', $elm).addClass('open');
	            $('.toggle-content', $elm).show();
	        });

	        $('.close-all', $elm).click(function () {
	            $('.toggle-hdr', $elm).removeClass('open');
	            $('.toggle-content', $elm).hide();
	        });

	    });
	</script>

</div></body></html>