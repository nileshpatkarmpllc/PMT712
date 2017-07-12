<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PackingSlipDisplay.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.PackingSlipDisplay" %>


<style>
    h1 {
        font-size:24px;
        font-weight:bold;
        line-height:normal;
    }
    .col1, .col2 {
        width:48%;
        float:left;
        font-size:16px;
    }
    .fullCol {
        width:100%;
        font-size:16px;
    }
    .col2left {
        width:50%;
        text-align:right;
        padding-right:5px;
        float:left;
    }
    .col2right {
        width:50%;
        text-align:left;
        float:left;
    }
    .outline {
        border: solid 1px black;
    }
    .pmtTable {
        display:table;
        width:100%;
    }
    .pmtRow {
        display:table-row;
        width:100%;
    }
    .pmtCell {
        display:table-cell;
        padding:10px;
        width:50%;
    }
    .pmtCell2 {
        padding:2px;
        display:table-cell;
    }
    .sigLine {
        border-bottom:1px solid black;
    }

</style>

<div class="packingSlipContainer">
    <div class="col1">
        <h1>Pacific Digital Distribution</h1>
        11112 Ventura Blvd.<br />
        Studio City,CA 91604<br />
        Tel: (818) 753-4700<br />
        Fax: (818) 753-1416<br />
    </div>
    <div class="col2">
        <div class="col2left">WorkOrder ID:</div><div class="col2right"><asp:Label ID="lblWOId" runat="server"></asp:Label></div><br clear="both" />
        <div class="col2left">Completion Date:</div><div class="col2right"><asp:Label ID="lblCompletionDate" runat="server"></asp:Label></div><br clear="both" />
        <div class="col2left">Order Submitted By:</div><div class="col2right"><asp:Label ID="lblOrderUser" runat="server"></asp:Label></div><br clear="both" />
        <div class="col2left">Delivery Method:</div><div class="col2right"><asp:Label ID="lblDeliveryMethod" runat="server"></asp:Label></div><br clear="both" />
    </div><br clear="both" /><br /><br />
    <div class="pmtTable">
        <div class="pmtRow">
            <div class="outline pmtCell">
                <h1>Bill To:</h1>
                <asp:Literal ID="litBillTo" runat="server"></asp:Literal>
            </div>
            <div class="outline pmtCell">
                <h1>Ship To:</h1>
                <asp:Literal ID="litShipTo" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <br /><br />
    <h1>MEDIA INFORMATION</h1>
    <div class="pmtTable">
        <div class="pmtRow">
            <div class="pmtCell2 outline">Qty</div>
            <div class="pmtCell2 outline">Title</div>
            <div class="pmtCell2 outline">Phone</div>
            <div class="pmtCell2 outline">Delivery Method</div>
            <div class="pmtCell2 outline">Media Type</div>
            <div class="pmtCell2 outline">Standard</div>
            <div class="pmtCell2 outline">TRT</div>
        </div>
        <asp:PlaceHolder ID="plTasks" runat="server"></asp:PlaceHolder>
        
    </div>
    <br clear="both" /><br />
    <div class="col1">&nbsp;</div>
    <div class="col2">
        <div class="col2left">Received:</div>
        <div class="col2right sigLine">&nbsp;</div>
    </div>
    <br clear="both" />
    <div class="col1">&nbsp;</div>
    <div class="col2">
        <div class="col2left">Date:</div>
        <div class="col2right sigLine" style="width:50%;"><div style="margin-left:50%;">Time:</div></div>
    </div>
</div>