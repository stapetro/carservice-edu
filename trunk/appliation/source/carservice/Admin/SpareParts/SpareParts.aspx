<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SpareParts.aspx.cs" Inherits="presentation.AdminSpareParts" %>

<asp:Content ID="sparePartsTitle" runat="server" ContentPlaceHolderID="pageTitle">Car Service - Spare Parts</asp:Content>

<asp:Content ID="sparePartsBody" runat="server" ContentPlaceHolderID="pageBody">
    <h2>
	    Spare parts management
    </h2>
    <p>
	    <asp:HyperLink ID="addSparePart" NavigateUrl="~/Admin/SpareParts/AddSparePart.aspx" runat="server">Add spare part</asp:HyperLink>			
    </p>
    <p>
        <asp:GridView ID="sparePartsGrid" AllowPaging="true" AutoGenerateColumns="false" 
            CssClass="nicetable" runat="server" OnRowCreated="SparePartsGridView_RowCreated" OnRowEditing="EditSparePartventHandler_RowEditing" OnRowDeleting="DeactivateSparePartEventHandler_RowDeliting">
            <Columns>
                <asp:BoundField HeaderText="Id" DataField="PartId" />
                <asp:BoundField HeaderText="Name" DataField="Name" />
                <asp:BoundField HeaderText="Price" DataField="Price" />
                <asp:BoundField HeaderText="Active" DataField="IsActive" />
                <asp:CommandField ShowEditButton="true" />
                <asp:CommandField ShowDeleteButton="true" />
            </Columns>
        </asp:GridView>				
    </p>
</asp:Content>
