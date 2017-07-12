<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.Settings" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>

	<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings")%></a></h2>
	<fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblAgencyView" runat="server" />  
            <asp:ListBox ID="lbxAgencyView" runat="server" SelectionMode="Multiple" Rows="4"></asp:ListBox>
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblAdvertiserView" runat="server" />
            <asp:ListBox ID="lbxAdvertiserView" runat="server" SelectionMode="Multiple" Rows="4"></asp:ListBox>
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblMarketView" runat="server" />
            <asp:ListBox ID="lbxMarketView" runat="server" SelectionMode="Multiple" Rows="4"></asp:ListBox>
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblStationView" runat="server" />
            <asp:ListBox ID="lbxStationView" runat="server" SelectionMode="Multiple" Rows="4"></asp:ListBox>
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblStationGroupView" runat="server" />
            <asp:ListBox ID="lbxStationGroupView" runat="server" SelectionMode="Multiple" Rows="4"></asp:ListBox>
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblLabelView" runat="server" />
            <asp:ListBox ID="lbxLabelView" runat="server" SelectionMode="Multiple" Rows="4"></asp:ListBox>
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblMasterItemsView" runat="server" />
            <asp:ListBox ID="lbxMasterItemsView" runat="server" SelectionMode="Multiple" Rows="4"></asp:ListBox>
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblUsersView" runat="server" />
            <asp:ListBox ID="lbxUsersView" runat="server" SelectionMode="Multiple" Rows="4"></asp:ListBox>
        </div>
    </fieldset>

