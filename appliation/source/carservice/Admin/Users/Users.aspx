<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="presentation.AdminUsers" %>

<asp:Content ID="carServiceTitle" runat="server" ContentPlaceHolderID="pageTitle">Car Service - Users</asp:Content>

<asp:Content ID="usersBody" runat="server" ContentPlaceHolderID="pageBody">
    <h2>User management</h2>
    <p>
	    <asp:HyperLink NavigateUrl="~/Admin/Users/AddUser.aspx" runat="server">Add user</asp:HyperLink>			
    </p>
    <p>
	    <table class="nicetable" >
		    <tbody>
			    <tr>
				    <th>Username</th>
				    <th>Email</th>
				    <th>First name</th>
				    <th>Last name</th>
				    <th>Active</th>
				    <th>Actions</th>
			    </tr>
			    <tr class="inactive">
				    <td>ScottBrown</td>
				    <td>scott@example.com</td>
				    <td>Scott</td>
				    <td>Brown</td>
				    <td>No</td>
				    <td><a href="addUser.htm">edit</a></td>
			    </tr>
			    <tr>
				    <td>stanislav.petrov</td>
				    <td>stanislav.petrov@aspnet.net</td>
				    <td>Stanislav</td>
				    <td>Petrov</td>
				    <td>Yes</td>
				    <td><a href="addUser.htm">edit</a></td>
			    </tr>												
		    </tbody>
	    </table>	    
    </p>
</asp:Content>