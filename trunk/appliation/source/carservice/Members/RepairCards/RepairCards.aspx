<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RepairCards.aspx.cs" Inherits="presentation.MembersRepairCards" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspe" %>
<%@ Register Src="~/CustomControls/CalendarUserControl.ascx" TagName="CalendarUserControl" TagPrefix="ucCal" %>

<asp:Content ID="repairCardsTitle" runat="server" ContentPlaceHolderID="pageTitle">Car Service - Repair Cards</asp:Content>

<asp:Content ID="repairCardsBody" runat="server" ContentPlaceHolderID="pageBody">
    <aspe:ToolkitScriptManager ID="toolScriptMgr" runat="server" />
    <h2>
	    Repair Cards Management
    </h2>
    <p>
        <asp:HyperLink ID="addRepairCard" NavigateUrl="~/Members/RepairCards/AddRepairCard.aspx" runat="server">Add repair card</asp:HyperLink>
    </p>

    <asp:ValidationSummary ID="FinishedRepairCardsFilterValidationSummary" runat="server" CssClass="failureNotification" 
            ValidationGroup="FinishedRepairCardsFilterValidationGroup"/>
    <asp:ValidationSummary ID="UnfinishedRepairCardsFilterValidationSummary" runat="server" CssClass="failureNotification" 
            ValidationGroup="UnfinishedRepairCardsFilterValidationGroup"/>
    <asp:ValidationSummary ID="AllRepairCardsFilterValidationSummary" runat="server" CssClass="failureNotification" 
            ValidationGroup="AllRepairCardsFilterValidationGroup"/>
    <asp:BulletedList ID="notificationMsgList" runat="server" CssClass="failureNotification" Visible="false" />
	<div class="filterInfo">
	<fieldset class="register">			
		<legend>Filter</legend>				
		<p>
            <asp:DropDownList ID="repairCardsFilterType" AutoPostBack="true" OnSelectedIndexChanged="ReportType_IndexChanged" runat="server">
                <asp:ListItem Value="0" Text="All" Selected="True" />                    
                <asp:ListItem Value="1" Text="Unfinished" />
                <asp:ListItem Value="2" Text="Finished"  />
            </asp:DropDownList>		
		</p>
        <asp:Panel ID="allRepairCardsFilter" runat="server">
		<p>                
            <span>Vin / Chassis</span>
            <asp:TextBox ID="VinChassisAllRepairCardsTxt" runat="server" CssClass="textEntry" />
		</p>
        </asp:Panel>
        <asp:Panel ID="unfinishedRepairCardsFilter" Visible="false" runat="server">
		<p>                
			<span>Start repair&nbsp;&nbsp;&nbsp;</span>
            <ucCal:CalendarUserControl ID="startRepairDate" runat="server" />
                
            <span>Vin / Chassis</span>
            <asp:TextBox ID="VinChassisTxt" runat="server" CssClass="textEntry" />
		</p>
        </asp:Panel>
        <asp:Panel ID="finishedRepairCardsFilter" Visible="false" runat="server">
			<span>From&nbsp;&nbsp;&nbsp;</span>
            <ucCal:CalendarUserControl ID="fromFinishRepairDate" runat="server" />
			<span>&nbsp;To&nbsp;&nbsp;&nbsp;</span>
            <ucCal:CalendarUserControl ID="toFinishRepairDate" runat="server" />
        </asp:Panel>
	</fieldset>
    <p class="submitButton">
        <asp:Button ID="filterButton" runat="server" Text="Filter" OnClick="FilterRepairCards_OnClick" 
            ValidationGroup="AllRepairCardsFilterValidationGroup" />
    </p>
	</div>							    
    <p>
        <asp:GridView ID="repairCardsGrid" AllowPaging="true" AutoGenerateColumns="false"
            CssClass="nicetable" AlternatingRowStyle-CssClass="alternaterow" runat="server" 
            OnRowCreated="RepairCardsGridView_RowCreated" OnRowEditing="EditRepairCardEventHandler_RowEditing"
            OnPageIndexChanging="RepairCardsGridView_PageIndexChanging" OnSorting="RepairCardsGridView_Sorting" AllowSorting="true">
            <Columns>
                <asp:BoundField HeaderText="Id" DataField="CardId" SortExpression="CardId" />
                <asp:BoundField HeaderText="Vin" DataField="Vin" SortExpression="Vin" />
                <asp:BoundField HeaderText="Chassis" DataField="ChassisNumber" SortExpression="ChassisNumber" />                
                <asp:BoundField HeaderText="Start date" DataField="StartRepair" SortExpression="StartRepair" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField HeaderText="Finish date" DataField="FinishRepair" SortExpression="FinishRepair" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField HeaderText="Repair price &#36;" DataField="CardPrice" SortExpression="CardPrice" />
                <asp:BoundField HeaderText="UserId" DataField="UserId" Visible="false" />
                <asp:CommandField ShowEditButton="true" />
            </Columns>
        </asp:GridView>				        
    </p>
</asp:Content>
