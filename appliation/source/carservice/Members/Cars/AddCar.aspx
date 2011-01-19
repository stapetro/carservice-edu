<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddCar.aspx.cs" Inherits="presentation.MembersAddCar" %>

<asp:Content ID="addCarTitle" runat="server" ContentPlaceHolderID="pageTitle">Car Service - Add Car</asp:Content>

<asp:Content ID="carsBody" runat="server" ContentPlaceHolderID="pageBody">
    <h2>
	    Car Management
    </h2>
    <p>
        Use the form below to create or edit a car.						
    </p>
    <asp:Label ID="notificationMsg" runat="server" Visible="false" />
    <asp:ValidationSummary ID="AddAutomobileValidationSummary" runat="server" CssClass="failureNotification" 
            ValidationGroup="AddAutomobileValidationGroup"/>
	<div class="accountInfo">
		<fieldset class="register">
			<legend>Car information</legend>
			<p>
				<span>Vin:</span>
                <asp:TextBox ID="AutoVin" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="AutoVinRequired" runat="server" ControlToValidate="AutoVin" 
                        CssClass="failureNotification" ErrorMessage="Vin is required." ToolTip="Vin is required." 
                        ValidationGroup="AddAutomobileValidationGroup">*</asp:RequiredFieldValidator>
			</p>
			<p>
				<span>Chassis number:</span>
                <asp:TextBox ID="AutoChassisNumber" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="AutoChassisNumberRequired" runat="server" ControlToValidate="AutoChassisNumber" 
                        CssClass="failureNotification" ErrorMessage="Chassis number is required." ToolTip="Chassis number is required." 
                        ValidationGroup="AddAutomobileValidationGroup">*</asp:RequiredFieldValidator>
			</p>
			<p>
				<span>Engine number:</span>
                <asp:TextBox ID="AutoEngineNumber" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="AutoEngineNumberRequired" runat="server" ControlToValidate="AutoEngineNumber" 
                        CssClass="failureNotification" ErrorMessage="Engine number is required." ToolTip="Engine number is required." 
                        ValidationGroup="AddAutomobileValidationGroup">*</asp:RequiredFieldValidator>
			</p>
			<p>
				<span>Engine cub:</span>
                <asp:TextBox ID="AutoEngineCub" runat="server" CssClass="textEntry"></asp:TextBox>
			</p>					
			<p>
				<span>Make:</span>
                <asp:TextBox ID="AutoMake" runat="server" CssClass="textEntry"></asp:TextBox>
			</p>					
			<p>
				<span>Model:</span>
                <asp:TextBox ID="AutoModel" runat="server" CssClass="textEntry"></asp:TextBox>
			</p>										
			<p>
				<span>Make year:</span>
                <asp:TextBox ID="AutoMakeYear" runat="server" CssClass="textEntry"></asp:TextBox>
			</p>
			<p>
				<span>Colour:</span>
                <asp:TextBox ID="AutoColour" runat="server" CssClass="textEntry"></asp:TextBox>
			</p>
			<p>
				<span>Description:</span>                
                <asp:TextBox ID="AutoDescription" runat="server" CssClass="textEntry" mode="multiline"></asp:TextBox>
			</p>
			<p>
				<span>Owner:</span>
                <asp:TextBox ID="AutoOwner" runat="server" CssClass="textEntry"></asp:TextBox>
			</p>
			<p>
				<span>Phone number:</span>
                <asp:TextBox ID="AutoPhoneNumber" runat="server" CssClass="textEntry"></asp:TextBox>
			</p>											
		</fieldset>
        <p class="submitButton">
            <asp:Button ID="CancelAutoButton" runat="server" Text="Cancel" OnClick="CancelAuto_OnClick" />
            <asp:Button ID="CreateAutoButton" runat="server" Text="Save" OnClick="SaveAuto_OnClick"
                    ValidationGroup="AddAutomobileValidationGroup"/>
        </p>
	</div>	
</asp:Content>
