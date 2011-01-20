<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Cars.aspx.cs" Inherits="presentation.MembersCars" %>

<asp:Content ID="carsTitle" runat="server" ContentPlaceHolderID="pageTitle">Car Service - Cars</asp:Content>

<asp:Content ID="carsBody" runat="server" ContentPlaceHolderID="pageBody">
    <h2>
	    Cars Management
    </h2>
    <p>
        <asp:HyperLink ID="addCar" NavigateUrl="~/Members/Cars/AddCar.aspx" runat="server">Add car</asp:HyperLink>	 	
    </p>
    <p>
        <asp:GridView ID="automobilesGrid" AllowPaging="true" AutoGenerateColumns="false"
            CssClass="nicetable" runat="server" OnRowEditing="EditAutomobileEventHandler_RowEditing"
            OnPageIndexChanging="AutomobilesGridView_PageIndexChanging">
            <Columns>
                <asp:BoundField HeaderText="Id" DataField="AutomobileId" />
                <asp:BoundField HeaderText="Vin" DataField="Vin" />
                <asp:BoundField HeaderText="Chassis" DataField="ChassisNumber" />
                <asp:TemplateField HeaderText="Make/Model">
                    <ItemTemplate>
                        <%# Eval("Make")%>&nbsp;/&nbsp;<%# Eval("Model")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Owner" DataField="Owner" />
                <asp:CommandField ShowEditButton="true" />
            </Columns>
        </asp:GridView>				        
    </p>
</asp:Content>
