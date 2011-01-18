<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="presentation.AdminUsers" %>

<asp:Content ID="carServiceTitle" runat="server" ContentPlaceHolderID="pageTitle">Car Service - Users</asp:Content>

<asp:Content ID="usersBody" runat="server" ContentPlaceHolderID="pageBody">
    <h2>User management</h2>
    <p>
	    <asp:HyperLink NavigateUrl="~/Admin/Users/AddUser.aspx" runat="server">Add user</asp:HyperLink>			
    </p>
    <p>
        <asp:GridView ID="carServiceUsers" AllowPaging="true" AutoGenerateColumns="false" 
            CssClass="nicetable" runat="server" OnRowCreated="CarServiceUsersGridView_RowCreated" OnRowEditing="EditUserEventHandler_RowEditing" OnRowDeleting="DeactivateUserEventHandler_RowDeliting">
            <Columns>
                <asp:BoundField HeaderText="User Name" DataField="UserName" />
                <asp:BoundField HeaderText="Email" DataField="Email" />
                <asp:BoundField HeaderText="First Name" DataField="FirstName" />
                <asp:BoundField HeaderText="Last Name" DataField="LastName" />
                <asp:BoundField HeaderText="Active" DataField="IsActive" />
                <asp:CommandField ShowEditButton="true" />
                <asp:CommandField ShowDeleteButton="true" />
            </Columns>
        </asp:GridView>
    </p>
</asp:Content>