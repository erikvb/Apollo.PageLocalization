<%@ Control Inherits="Apollo.DNN.Modules.PageLocalization.Settings" language="vb" AutoEventWireup="false" Explicit="true" Codebehind="Settings.ascx.vb" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellspacing="0" cellpadding="4" border="0" width=100%>
	<tr>
		<td class="SubHead" style="width:200px"><dnn:label id="plCheckPermissions" controlname="chkCheckPermissions" runat="server" /></td>
		<td valign="top">
		    <asp:CheckBox ID="chkCheckPermissions" runat="server" CssClass="NormalTextBox" Checked="false" />
		</td>
	</tr>
</table>

