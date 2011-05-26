<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PageLocalization.ascx.vb"
    Inherits="Apollo.DNN.Modules.PageLocalization.PageLocalization" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register Assembly="DotNetNuke.WebControls" Namespace="DotNetNuke.UI.WebControls"
    TagPrefix="DNN" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="DNN" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="DNN" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<div id="placeHolder" runat="server">
    <table style="width: 100%">
        <tr>
            <td style="vertical-align: top; text-align: left; white-space: nowrap" class="Normal">
                <DNN:DNNTreeView ID="TV" runat="server" Width="200px" style="white-space: normal;">
                </DNN:DNNTreeView>
            </td>
            <td style="vertical-align: top; text-align: left">
                <table class="Settings" cellspacing="2" cellpadding="2" summary="Manage Tabs Design Table"
                    border="0">
                    <tr>
                        <td class="SubHead" style="vertical-align: top; width: 200px">
                            <DNN:Label ID="plEditLanguage" runat="server" Text="Choose language for editing:"
                                ControlName="cboEditLanguage">
                            </DNN:Label>
                        </td>
                        <td class="NormalTextBox" style="vertical-align: top">
                            <dnn:DnnLanguageComboBox ID="cboEditLanguage" runat="server" AutoPostBack="true" Width="250"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="vertical-align: top; width: 200px">
                            <DNN:Label ID="lblTabName" runat="server" Suffix=":" ControlName="txtTabName">
                            </DNN:Label>
                        </td>
                        <td class="NormalTextBox" style="vertical-align: top">
                            <asp:TextBox ID="txtTabName" CssClass="NormalTextBox" runat="server" MaxLength="500"
                                Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="vertical-align: top; width: 200px">
                            <DNN:Label ID="lblTitle" runat="server" Suffix=":" ControlName="txtTitle">
                            </DNN:Label>
                        </td>
                        <td class="NormalTextBox" style="vertical-align: top">
                            <asp:TextBox ID="txtTitle" CssClass="NormalTextBox" runat="server" MaxLength="500"
                                Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="vertical-align: top; width: 200px">
                            <DNN:Label ID="lblDescription" runat="server" Suffix=":" ControlName="txtDescription">
                            </DNN:Label>
                        </td>
                        <td class="NormalTextBox" style="vertical-align: top">
                            <asp:TextBox ID="txtDescription" CssClass="NormalTextBox" runat="server" MaxLength="500"
                                Width="250px" TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="vertical-align: top; width: 200px">
                            <DNN:Label ID="lblKeywords" runat="server" Suffix=":" ControlName="txtKeyWords">
                            </DNN:Label>
                        </td>
                        <td class="NormalTextBox" style="vertical-align: top">
                            <asp:TextBox ID="txtKeyWords" CssClass="NormalTextBox" runat="server" MaxLength="500"
                                Width="250px" TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="vertical-align: top; width: 200px">
                            <DNN:Label ID="lblPageHeadText" runat="server" Suffix=":" ControlName="txtPageHeadText">
                            </DNN:Label>
                        </td>
                        <td class="NormalTextBox" style="vertical-align: top">
                            <asp:TextBox ID="txtPageHeadText" CssClass="NormalTextBox" runat="server" TextMode="MultiLine"
                                MaxLength="4000" Rows="4" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="vertical-align: top; width: 200px">
                            <DNN:Label ID="plIsVisible" runat="server" Suffix="?" ControlName="chkMenu">
                            </DNN:Label>
                        </td>
                        <td class="NormalTextBox" style="vertical-align: top">
                            <asp:CheckBox ID="chkMenu" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="vertical-align: top; width: 200px">
                            <DNN:Label ID="plURL" runat="server" Suffix=":" ControlName="ctlURL">
                            </DNN:Label>
                        </td>
                        <td class="NormalTextBox" style="vertical-align: top">
                            <DNN:DNNComboBox ID="ddlRedirTabs" runat="server" AutoPostBack="True">
                            </DNN:DNNComboBox>
                        </td>
                    </tr>
                </table>
                <p>
                    <DNN:CommandButton ID="cmdUpdate" runat="server" ImageUrl="~/images/save.gif" Text="Update"
                        CssClass="CommandButton" />
                </p>
            </td>
        </tr>
    </table>
</div>
