<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MasterItemCheckList.ascx.cs" Inherits="Christoc.Modules.PMT_Admin.MasterItemCheckList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %> 
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ register tagprefix="dnn" tagname="Label" src="~/controls/LabelControl.ascx" %>

<dnn:DnnJsInclude ID="DnnJsInclude" runat="server" FilePath="/DesktopModules/PMT_Admin/jquery.switchButton.js" Priority="11" />
<dnn:DnnCssInclude ID ="DnnCssInclude" runat="server" FilePath="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" Priority="12" />


<script>
    $(document).ready(function () {
        $('#chkClosedCaptionFileRecd').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkReceived').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkWatermark').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkDownConvert').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkCenterCutSafe').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkLetterbox').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkClosedCaptioned').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkExercise').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkUIGEA').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkStateTimes').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkResultsVary').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkApple').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkMoneyBack').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkLessSandH').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkUSFunds').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkResponseAddressLocation').switchButton({ on_label: 'Tail', off_label: 'CTA' });
        $('#chkWebsite').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkInCTA').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkWithPhone').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkClub').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkClock').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkEDLReq').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkGenericVO').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkScript').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chk18Over').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkSlate').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkJpeg').switchButton({ on_label: 'Yes', off_label: 'No' });
        $('#chkScripts').switchButton({ on_label: 'Yes', off_label: 'No' });
        //$('.checkSwitches2').switchButton(
        //    {
        //        on_label: 'Tail',
        //        off_label: 'CTA'
        //    });
        $('#chkCCList').buttonset();
        //$('.<%= txtOfferDate.ClientID %>').datepicker();
        //$('.hasCalendar>').datepicker();
    });
    </script>

<style>
    .pmt-btn {
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
    .pmt-btn-default {
	color:#808080;
	background:#f7f7f7;
    width:15%;
}
.pmt-btn-default:hover {
	background:#ebebeb;
}
</style>

<asp:Panel ID="pnlAuth" runat="server">

<div class="formTop"><div class="formTopDiv"><p class="formTopP"><span class="formTopText">MASTER ASSET CHECK LIST</span></p></div></div>

<div class="formBg">
    <br />
    <div class="sectionBg">
        <div class="sectionInner">
        <div class="sectionContent">
            <div class="sectionColumn">
            <asp:TextBox CssClass="pmtInput" runat="server" ID="txtDate" Text="Date: " ></asp:TextBox><ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDate"></ajaxToolkit:CalendarExtender><br />
                <asp:TextBox CssClass="pmtInput" runat="server" ID="txtQc" Text="QC By: " ></asp:TextBox><br />
                <asp:TextBox CssClass="pmtInput" runat="server" ID="txtPMTnumber" Text="PMT #: " ></asp:TextBox>
            </div>
            <div class="sectionColumn">
                <asp:TextBox CssClass="pmtInput" runat="server" ID="txtVaultId" Text="Vault Id: " ></asp:TextBox><br />
                <span class="fieldHeader" style="display:inline-block;width:60px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">Received: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkReceived" /></div>
            </div>
            <br clear="both" />
           </div>
        </div>
    </div>

    <div class="sectionBg">
        <div class="sectionInner">
        <div class="sectionContent">
            <div class="rowDark">
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;"><span style="position:relative;top:3px;">Resolution: </span></span>
                <asp:DropDownList ID="ddlResolution" runat="server">
                    <asp:ListItem Value="HD 720" Text="HD 720" />
                    <asp:ListItem Value="HD 1080" Text="HD 1080" />
                    <asp:ListItem Value="SD" Text="SD" />
                </asp:DropDownList>
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-left:20px;"><span style="position:relative;top:3px;">Audio: </span></span>
                <asp:DropDownList ID="ddlAudtio" runat="server">
                    <asp:ListItem Value="Stereo" Text="Stereo" />
                    <asp:ListItem Value="Mono" Text="Mono" />
                </asp:DropDownList>
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-left:20px;"><span style="position:relative;top:3px;">Language: </span></span>
                <asp:DropDownList ID="ddlLanguage" runat="server">
                    <asp:ListItem Value="English" Text="English" />
                    <asp:ListItem Value="Spanish" Text="Spanish" />
                    <asp:ListItem Value="Other" Text="Other" />
                </asp:DropDownList>
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-left:20px;"><span style="position:relative;top:3px;">Timecode: </span></span>
                <asp:DropDownList ID="ddlTimecode" runat="server">
                    <asp:ListItem Value="NDFTC" Text="NDFTC" />
                    <asp:ListItem Value="DFTC" Text="DFTC" />
                </asp:DropDownList>
            </div>
            <div class="rowLight">
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">Watermark Encoded: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkWatermark" /></div>
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-left:20px;"><span style="position:relative;top:3px;">Type: </span></span>
                <asp:DropDownList ID="ddlType" runat="server">
                    <asp:ListItem Value="Neilson" Text="Neilson" />
                    <asp:ListItem Value="Veil" Text="Veil" />
                    <asp:ListItem Value="Teletrax" Text="Teletrax" />
                </asp:DropDownList>
            </div>
            <div class="rowDark">
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">Down Convert: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkDownConvert" /></div>
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-left:20px;margin-right:5px;"><span style="position:relative;top:3px;">Center Cut Safe: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkCenterCutSafe" /></div>
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-left:20px;margin-right:5px;"><span style="position:relative;top:3px;">Letterbox: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkLetterbox" /></div>
            </div>
            <div class="rowLight">
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">Closed Captioned: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkClosedCaptioned" /></div>
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-left:20px;margin-right:5px;"><span style="position:relative;top:3px;">Caption File Rec'd: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkClosedCaptionFileRecd" /></div>
            </div>
           </div>
        </div>
    </div>

    <div class="subHeadDiv">
        <span style="position:relative;top:4px;left:20px;">MASTER LABELED AS:</span>
    </div>
    <div class="sectionBg">
        <div class="sectionInner">
        <div class="sectionContent">
            <div class="rowDark">
                <asp:TextBox CssClass="pmtInput fieldBig" runat="server" ID="txtTitle" Text="Title: " ></asp:TextBox>
                <asp:TextBox CssClass="pmtInput fieldBig" runat="server" ID="txtVersion" Text="Version: " ></asp:TextBox>
                <asp:TextBox CssClass="pmtInput fieldBig" runat="server" ID="txtTRT" Text="TRT: " ></asp:TextBox>
                <asp:TextBox CssClass="pmtInput fieldBig" runat="server" ID="txtOffer" Text="Offer: " ></asp:TextBox>
            </div>
            <div class="rowLight">
                <asp:TextBox CssClass="pmtInput fieldBig" runat="server" ID="txtHost" Text="Host: " ></asp:TextBox>
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;"><span style="position:relative;top:3px;">Format: </span></span>
                <asp:DropDownList ID="ddlFormat" runat="server">
                    <asp:ListItem Value="Digibeta" Text="Digibeta" />
                    <asp:ListItem Value="D2" Text="D2" />
                    <asp:ListItem Value="Digital File" Text="Digital File" />
                    <asp:ListItem Value="HDCam" Text="HDCam" />
                    <asp:ListItem Value="HDCamSR" Text="HDCamSR" />
                </asp:DropDownList>
                <asp:TextBox CssClass="pmtInput leftSpace fieldBig" runat="server" ID="txtAKA" Text="A.K.A.: " ></asp:TextBox>
                <asp:TextBox CssClass="pmtInput fieldBig hasDatepicker" runat="server" ID="txtOfferDate" Text="Date: " ></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtOfferDate"></ajaxToolkit:CalendarExtender>
            </div>
        </div>
        </div>
    </div>

    <div class="subHeadDiv">
        <span style="position:relative;top:4px;left:20px;">DISCLAIMERS : THE FOLLOWING IS (THE PROCEDING WAS) A PAID PROGRAM</span>
    </div>
    <div class="sectionBg">
        <div class="sectionInner">
        <div class="sectionContent">
            <div class="rowDark">
                <asp:TextBox CssClass="pmtInput fieldBig" runat="server" ID="txtDisclaimerFor" Text="For: " ></asp:TextBox>
                <asp:TextBox CssClass="pmtInput fieldBig" runat="server" ID="txtDisclaimerBy" Text="By: " ></asp:TextBox>
            </div>
            <div class="rowLight">
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;"><span style="position:relative;top:3px;">Open: </span></span>
                <asp:DropDownList ID="ddlDisclaimerOpen" runat="server">
                    <asp:ListItem Value="Audio" Text="Audio" />
                    <asp:ListItem Value="Video" Text="Video" />
                    <asp:ListItem Value="Audio / Video" Text="Audio / Video" />
                </asp:DropDownList>
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;"><span style="position:relative;top:3px;">Close: </span></span>
                <asp:DropDownList ID="ddlDisclaimerClose" runat="server">
                    <asp:ListItem Value="Audio" Text="Audio" />
                    <asp:ListItem Value="Video" Text="Video" />
                    <asp:ListItem Value="Audio / Video" Text="Audio / Video" />
                </asp:DropDownList>
            </div>
        </div>
        </div>
    </div>

    <div class="subHeadDiv">
        <span style="position:relative;top:4px;left:20px;">IN PROGRAM DISCLAIMERS :</span>
    </div>
    <div class="sectionBg">
        <div class="sectionInner">
        <div class="sectionContent">

            <div class="subSectionHeadDiv">
                <span style="position:relative;top:4px;left:20px;">EXERCISE OR INGESTABLES</span>
            </div>
            <div class="subsectionBg">
                <div class="sectionInner">
                <div class="sectionContent">
                    <div class="rowDark">
                        <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-left:20px;margin-right:5px;"><span style="position:relative;top:3px;">Exercise: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkExercise" /></div>
                        <asp:TextBox CssClass="pmtInput fieldBig leftSpace" runat="server" ID="txtExerciseTimes" Text="How Many Times: " ></asp:TextBox>
                    </div>
                </div>
                </div>
            </div>

            <div class="subSectionHeadDiv">
                <span style="position:relative;top:4px;left:20px;">GAMBLE</span>
            </div>
            <div class="subsectionBg">
                <div class="sectionInner">
                <div class="sectionContent">
                    <div class="rowDark">
                        <span class="fieldHeader" style="display:inline-block;width:220px;vertical-align:middle;margin-left:20px;margin-right:5px;"><span style="position:relative;top:3px;">UIGEA: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkUIGEA" /></div>
                        <asp:TextBox CssClass="pmtInput fieldBig leftSpace" runat="server" ID="txtUIGEATimes" Text="How Many Times: " ></asp:TextBox>
                    </div>
                    <div class="rowLight">
                        <span class="fieldHeader" style="display:inline-block;width:220px;vertical-align:middle;margin-left:20px;margin-right:5px;"><span style="position:relative;top:3px;">STATE RESTRICTIONS: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkStateTimes" /></div>
                        <asp:TextBox CssClass="pmtInput fieldBig leftSpace" runat="server" ID="txtStateTimes" Text="How Many Times: " ></asp:TextBox>
                    </div>
                    <div class="rowDark">
                        <span class="fieldHeader" style="display:inline-block;width:220px;vertical-align:middle;margin-left:20px;margin-right:5px;"><span style="position:relative;top:3px;">ACTUAL RESULTS MAY VARY: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkResultsVary" /></div>
                        <asp:TextBox CssClass="pmtInput fieldBig leftSpace" runat="server" ID="txtResultsVaryTimes" Text="How Many Times: " ></asp:TextBox>
                    </div>
                    <div class="rowLight">
                        <span class="fieldHeader" style="display:inline-block;width:220px;vertical-align:middle;margin-left:20px;margin-right:5px;"><span style="position:relative;top:3px;">APPLE TRADEMARK: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkApple" /></div>
                        <asp:TextBox CssClass="pmtInput fieldBig leftSpace" runat="server" ID="txtAppleTimes" Text="How Many Times: " ></asp:TextBox>
                    </div>
                </div>
                </div>
            </div>

        </div>
        </div>
    </div>

    <div class="sectionBg">
        <div class="sectionInner">
        <div class="sectionContent">
            <div class="rowDark">
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;"><span style="position:relative;top:3px;">Price: </span></span>
                <asp:TextBox CssClass="pmtInput fieldBig leftSpace" runat="server" ID="txtPrice2" Text="" ></asp:TextBox> Payments of 
                <asp:TextBox CssClass="pmtInput" runat="server" ID="txtPrice3" Text="$ " ></asp:TextBox>
            </div>
            <div class="rowLight">
                <asp:TextBox CssClass="pmtInput fieldBig" runat="server" ID="txtSandH" Text="SHIPPING & HANDLING $: " ></asp:TextBox>
            </div>
            <div class="rowDark">
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;"><span style="position:relative;top:3px;">Credit Cards: </span></span>
                <div style="display:inline-block;position:relative;top:7px;"><asp:CheckBoxList ID="chkCCList" ClientIDMode="Static" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="VISA">VISA</asp:ListItem>
                    <asp:ListItem Value="MasterCard">Master Card</asp:ListItem>
                    <asp:ListItem Value="Discover">Discover</asp:ListItem>
                    <asp:ListItem Value="AMEX">AMEX</asp:ListItem>
                    <asp:ListItem Value="Other">Other</asp:ListItem>
                </asp:CheckBoxList></div>
                <span class="fieldHeader" style="display:inline-block;width:220px;vertical-align:middle;margin-left:20px;margin-right:5px;"><span style="position:relative;top:3px;">DAY MONEY BACK GUARANTEE : </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkMoneyBack" /></div>
            </div>
            <div class="rowLight">
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">LESS S&H: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkLessSandH" /></div>
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-left:20px;margin-right:5px;"><span style="position:relative;top:3px;">US FUNDS: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkUSFunds" /></div>
            </div>
            <div class="rowDark">
                <span class="fieldHeader" style="display:inline-block;width:220px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">RESPONSE ADDRESS LOCATION: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches2" runat="server" ID="chkResponseAddressLocation" /></div>
            </div>
            <div class="rowLight">
                <div class="sectionColumn">
                    <span class="fieldHeader" style="display:inline-block;width:220px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">RESPONSE ADDRESS: </span></span>
                </div>
                <div class="sectionColumn" style="width:70%;">
                    <asp:TextBox CssClass="pmtInput fieldBigger" runat="server" ID="txtResponseStreet" placeholder="Street" ></asp:TextBox><br />
                    <asp:TextBox CssClass="pmtInput fieldBig" runat="server" ID="txtResponseCity" placeholder="City" ></asp:TextBox>
                    <asp:TextBox CssClass="pmtInput" runat="server" ID="txtResponseState" placeholder="State" ></asp:TextBox>
                    <asp:TextBox CssClass="pmtInput leftSpace" runat="server" ID="txtResponseZip" placeholder="Zip" ></asp:TextBox>
                </div><br clear="both" />
            </div>
        </div>
        </div>
    </div>

    <div class="sectionBg">
        <div class="sectionInner">
        <div class="sectionContent">
            <div class="rowDark">
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">WEBSITE: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkWebsite" /></div>
                <asp:TextBox CssClass="pmtInput fieldBigger leftSpace" runat="server" ID="txtWebsiteAddress" Text="Website Address: " ></asp:TextBox>
            </div>
            <div class="rowLight">
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">IN CTA: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkInCTA" /></div>
                <span class="fieldHeader leftSpace" style="display:inline-block;width:220px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">WITH PHONE NUMBER: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkWithPhone" /></div>
            </div>
            <div class="rowDark">
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">CLUB: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkClub" /></div>
                <span class="fieldHeader leftSpace" style="display:inline-block;width:220px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">CLOCK (TIME LEFT TO ORDER): </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkClock" /></div>
            </div>
            <div class="rowLight">
                <span class="fieldHeader" style="display:inline-block;width:120px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">EDL REQUIRED: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkEDLReq" /></div>
                <span class="fieldHeader leftSpace" style="display:inline-block;width:120px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">GENERIC VO: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkGenericVO" /></div>
                <span class="fieldHeader leftSpace" style="display:inline-block;width:120px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">SCRIPT: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkScript" /></div>
            </div>
        </div>
        </div>
    </div>

    <div class="sectionBg">
        <div class="sectionInner">
        <div class="sectionContent">
            <div class="rowDark">
                <asp:TextBox CssClass="pmtInput fieldBigger" runat="server" ID="txtDelivery" Text="Delivery: " ></asp:TextBox>
                <span class="fieldHeader leftSpace" style="display:inline-block;width:120px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">18 and Over: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chk18Over" /></div>
            </div>
        </div>
        </div>
    </div>

    <div class="sectionBg">
        <div class="sectionInner">
        <div class="sectionContent">
            <div class="rowDark">
                <asp:TextBox CssClass="pmtInput" runat="server" ID="txtAttachments" Text="Attachments: " ></asp:TextBox>
                <span class="fieldHeader leftSpace" style="display:inline-block;width:60px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">Slate: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkSlate" /></div>
                <span class="fieldHeader leftSpace" style="display:inline-block;width:60px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">JPEG: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkJpeg" /></div>
                <span class="fieldHeader leftSpace" style="display:inline-block;width:60px;vertical-align:middle;margin-right:5px;"><span style="position:relative;top:3px;">Scripts: </span></span><div class="checkSwitchDiv"><asp:CheckBox  ClientIDMode="Static" CssClass="checkSwitches" runat="server" ID="chkScripts" /></div>
                <span class="fieldHeader leftSpace" style="display:inline-block;width:50px;vertical-align:middle;"><span style="position:relative;top:3px;">EDL: </span></span>
                <asp:DropDownList ID="ddlEDL" runat="server">
                    <asp:ListItem Value="WORD" Text="WORD" />
                    <asp:ListItem Value="Text" Text="Text" />
                </asp:DropDownList>
            </div>
        </div>
        </div>
    </div>
    <div class="sectionBg">
        <div class="sectionInner">
        <div class="sectionContent">
            <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="8" CssClass="commentsField" Text="Comments/Notes: "></asp:TextBox>
            </div></div></div>
    <br />
    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save Checklist" CssClass="pmt-btn pmt-btn-default" />
    <br />
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <br />
    <br />
</div>
    </asp:Panel>

<asp:Panel ID="pnlNotAuth" runat="server">
    <asp:Label ID="lblError" runat="server"></asp:Label>
</asp:Panel>