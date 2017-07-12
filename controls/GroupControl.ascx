<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupControl.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.GroupControl" %>

<asp:UpdateProgress ID="updDeliver" AssociatedUpdatePanelID="pnlDeliver" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="/DesktopModules/PMT_Admin/images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdateProgress ID="updNonDeliver" AssociatedUpdatePanelID="pnlNonDeliver" runat="server">
    <ProgressTemplate>  
        <div class="loading-panel">
            <div class="loading-container">          
                <img alt="progress" src="/DesktopModules/PMT_Admin/images/loading.gif"/>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>


<asp:UpdatePanel ID="pnlDeliver" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div class="groupHeader"><asp:TextBox ID="txtDeliverGroupName" AutoPostBack="true" OnTextChanged="txtDeliverGroupName_TextChanged" runat="server"></asp:TextBox></div>
        <div class="groupHolder">
            <asp:Panel ID="pnlDeliverEdit" runat="server">
                <div id="groupLibItemsHeader">
                    <div class="mastersHeader groupLibItemCol1">MASTER ID</div>
                    <div class="mastersHeader groupLibItemCol2">ISCI</div>
                    <div class="mastersHeader groupLibItemCol3">TITLE</div>
                    <div class="mastersHeader groupLibItemCol4">DESCRIPTION</div>
                    <div class="mastersHeader groupLibItemCol5">ENCODE</div>
                    <div class="mastersHeader groupLibItemCol6">LENGTH</div>
                    <div class="mastersHeader groupLibItemCol7">CC</div>
                    <div class="mastersHeader groupLibItemCol8">REMOVE</div>
                </div>
                <asp:PlaceHolder ID="plGroupLibItems" runat="server"></asp:PlaceHolder>
                <br />
                <asp:PlaceHolder ID="plStationsHeader" runat="server"><div id="groupStationsHeader">
                    <div class="mastersHeader groupStationCol1">CALL LETTERS</div>
                    <div class="mastersHeader groupStationCol2">MARKET</div>
                    <div class="mastersHeader groupStationCol3">CITY</div>
                    <div class="mastersHeader groupStationCol4">DELIVERY METHOD</div>
                    <div class="mastersHeader groupStationCol5a">QUANTITY</div>
                    <div class="mastersHeader groupStationCol5">SHIPPING</div>
                    <div class="mastersHeader groupStationCol6">PRIORITY</div>
                    <div class="mastersHeader groupStationCol7">REMOVE</div>
                </div></asp:PlaceHolder>
                <asp:PlaceHolder ID="plGroupStations" runat="server"></asp:PlaceHolder>
                <br />
                <div class="groupNotesHeader">GROUP NOTES:</div>
                <div class="groupNotes"><asp:TextBox ID="txtGroupNotes" runat="server" TextMode="MultiLine" Rows="4" Wrap="true" placeholder="Notes: " ViewStateMode="Enabled" AutoPostBack="true" OnTextChanged="txtGroupNotes_TextChanged"></asp:TextBox></div><br />
                <asp:CheckBoxList ID="cklCustomizeServices" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cklCustomizeServices_SelectedIndexChanged" RepeatColumns="4"></asp:CheckBoxList>
                <asp:Button ID="btnDeleteGroup" runat="server" CssClass="groupDeleteButton" Text="DELETE GROUP" OnClick="btnDeleteGroup_Click" OnClientClick="return confirm('Are you certain you want to delete this Group?');" />
                <br clear="both" /><br />
            </asp:Panel>
            <asp:Panel ID="pnlDeliverView" runat="server">
                <asp:Literal ID="litDeliverView" runat="server"></asp:Literal>
                <br />
                <asp:Literal ID="litCustomizeServices" runat="server"></asp:Literal><br />
                <div class="groupNotesHeader">GROUP NOTES:</div>
                <div class="groupNotes"><asp:TextBox ID="txtDisplayGroupNotes" runat="server" TextMode="MultiLine" Rows="4" Wrap="true" Enabled="false"></asp:TextBox></div><br />
                
            </asp:Panel>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="pnlNonDeliver" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div class="groupHeader"><asp:TextBox ID="txtNonDeliverGroupName" AutoPostBack="true" OnTextChanged="txtNonDeliverGroupName_TextChanged" runat="server"></asp:TextBox></div>
        <div class="groupHolder">
            <asp:Panel ID="pnlNonDeliverEdit" runat="server">
                <asp:Literal ID="litMasterInfo" runat="server"></asp:Literal>

                <asp:CheckBoxList ID="cklServices" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cklServices_SelectedIndexChanged" RepeatColumns="2"></asp:CheckBoxList>
                <br />
                <div class="groupNotesHeader">GROUP NOTES:</div>
                <div class="groupNotes"><asp:TextBox ID="txtNonDeliverableNotes" runat="server" TextMode="MultiLine" Rows="4" Wrap="true" placeholder="Notes: " ViewStateMode="Enabled" AutoPostBack="true" OnTextChanged="txtNonDeliverableNotes_TextChanged"></asp:TextBox></div><br />
                <asp:Button ID="btnNonDeliverableDelete" runat="server" CssClass="groupDeleteButton" Text="DELETE GROUP" OnClick="btnDeleteGroup_Click" OnClientClick="return confirm('Are you certain you want to delete this Group?');" />
                <br clear="both" /><br />
            </asp:Panel>
            <asp:Panel ID="pnlNonDeliverView" runat="server">
                
                <asp:Literal ID="litServices" runat="server"></asp:Literal>
            </asp:Panel>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<br />