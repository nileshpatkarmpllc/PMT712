<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMT_ReportsSettings.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.PMT_ReportsSettings" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>

<fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblAllView" runat="server" />  
            <asp:CheckBoxList ID="lbxAllView" runat="server" SelectionMode="Multiple" Rows="4"></asp:CheckBoxList>
        </div>
</fieldset>