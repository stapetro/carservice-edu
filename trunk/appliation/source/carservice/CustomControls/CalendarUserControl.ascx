<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CalendarUserControl.ascx.cs" Inherits="CustomControls_CalendarUserControl" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspe" %>

<asp:Panel ID="calendarControl" runat="server">
    <asp:TextBox ID="selectedDateTxt" runat="server" CssClass="textEntry" />
    <asp:ImageButton ID="calendarBtn" ImageUrl="~/Resources/Images/calendar_scheduleHS.png" runat="server" CssClass="calendar" />
    <aspe:CalendarExtender ID="CalendarExtender" TargetControlID="selectedDateTxt" PopupButtonID="calendarBtn" runat="server" />
    <aspe:ToolkitScriptManager ID="toolScriptMgr" runat="server" />
</asp:Panel>
