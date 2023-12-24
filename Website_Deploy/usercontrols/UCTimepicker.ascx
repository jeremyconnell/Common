<%@ Control EnableViewState="false" EnableTheming="false" Language="C#" AutoEventWireup="true" CodeFile="UCTimepicker.ascx.cs" Inherits="usercontrols_UCTimepicker" %>
<table style="width:auto" cellpadding="0" cellspacing="0" border="0">
    <tr style="padding:0px">
        <td>
            <fieldset style="margin:0px; padding-top:0px">
                <legend style="margin:0px;">Hour</legend>
                <asp:DropDownList ID="ddHours" runat="server" style="width:auto" />
            </fieldset>            
        </td>
        <td id="cellMins" runat="server">
            <fieldset style="margin:0px; padding-top:0px">
                <legend style="margin:0px;">Mins</legend>
                <asp:DropDownList ID="ddMins" runat="server" style="width:auto" />
            </fieldset>            
        </td>
        <td id="cellSecs" runat="server">
            <fieldset style="margin:0px; padding-top:0px">
                <legend style="margin:0px;">Secs</legend>
                <asp:DropDownList ID="ddSecs" runat="server" style="width:auto" />
            </fieldset>            
        </td>
    </tr>
</table>
