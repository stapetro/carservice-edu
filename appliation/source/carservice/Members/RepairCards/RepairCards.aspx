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
    <asp:Label ID="notificationMsg" CssClass="negativeMsg" runat="server" Visible="false" />
    <p>
	    <div class="filterInfo">
	    <fieldset class="register">			
		    <legend>Filter</legend>				
		    <p>
                <asp:DropDownList ID="repairCardsFilterType" AutoPostBack="true" OnSelectedIndexChanged="ReportType_IndexChanged" runat="server">
                    <asp:ListItem Value="0" Text="Unfinished" Selected="True" />
                    <asp:ListItem Value="1" Text="Finished"  />
                </asp:DropDownList>		
		    </p>
            <asp:Panel ID="unfinishedRepairCardsFilter" runat="server">
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
                ValidationGroup="UnfinishedRepairCardsFilterValidationGroup" />
        </p>
	    </div>							    
    </p>
    <p>
        <asp:GridView ID="repairCardsGrid" AllowPaging="true" AutoGenerateColumns="false"
            CssClass="nicetable" AlternatingRowStyle-CssClass="alternaterow" runat="server" 
            OnRowEditing="EditRepairCardEventHandler_RowEditing"
            OnPageIndexChanging="RepairCardsGridView_PageIndexChanging">
            <Columns>
                <asp:BoundField HeaderText="Id" DataField="CardId" />
                <asp:TemplateField HeaderText="Vin/Chassis">
                    <ItemTemplate>
                        <%# Eval("Vin")%>&nbsp;/&nbsp;<%# Eval("ChassisNumber")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Start date" DataField="StartRepair" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField HeaderText="Finish date" DataField="FinishRepair" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField HeaderText="Repair price &#36;" DataField="CardPrice" />
                <asp:CommandField ShowEditButton="true" />
            </Columns>
        </asp:GridView>				        
    </p>
</asp:Content>
