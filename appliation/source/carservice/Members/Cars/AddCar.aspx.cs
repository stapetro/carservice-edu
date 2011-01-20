﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using constants;
using persistence;
using System.Globalization;

namespace presentation
{
    public partial class MembersAddCar : System.Web.UI.Page
    {
        private ICarServicePersister persister;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (persister == null)
            {
                persister = new CarServicePersister();
            }
            if (IsPostBack == false)
            {
                LoadAutomobilePage();
            }
            this.notificationMsg.Visible = false;
        }

        protected void CancelAuto_OnClick(object sender, EventArgs e)
        {
            string continueUrl = "~/Members/Cars/Cars.aspx";
            Response.Redirect(continueUrl);
        }

        protected void SaveAuto_OnClick(object sender, EventArgs e)
        {
            string vin = this.AutoVin.Text;
            string chassisNumber = this.AutoChassisNumber.Text;
            string engineNumberTxt = this.AutoEngineNumber.Text;
            string engineCubTxt = this.AutoEngineCub.Text;
            string make = this.AutoMake.Text;
            string model = this.AutoModel.Text;
            string makeYearTxt = this.AutoMakeYearCalendar.SelectedDate;
            string owner = this.AutoOwner.Text;
            string phoneNumber = this.AutoPhoneNumber.Text;
            string colour = this.AutoColour.Text;
            string description = this.AutoDescription.Text;
            HandleAutomobileInformation(vin, chassisNumber, engineNumberTxt,
                    engineCubTxt, make, model, makeYearTxt, owner, phoneNumber, colour, description);           
        }

        private void HandleAutomobileInformation(string vin, string chassisNumber, string engineNumberTxt, 
            string engineCubTxt, string make, string model, string makeYearTxt, string owner, 
            string phoneNumber, string colour, string description)
        {
            bool validVin = false;
            bool validChassisNumber = false;
            Automobile auto = null;
            object autoIdObject = Session[CarServiceConstants.AUTOMOBILE_ID_REQUEST_PARAM_NAME];
            // Updating existing automobile
            if (autoIdObject != null)
            {
                int autoId;
                if (Int32.TryParse(autoIdObject.ToString(), out autoId))
                {
                    auto = this.persister.GetAutomobilById(autoId);
                    string currentVin = auto.Vin;
                    bool isVinChanged = string.IsNullOrEmpty(currentVin) ||
                        currentVin.Equals(vin) == false;
                    if (isVinChanged)
                    {
                        validVin = IsVinValid(vin);
                    }
                    else
                    {                     
                        validVin = true;
                    }
                    string currentChassisNumber = auto.ChassisNumber;
                    bool isChassisNumberChanged = string.IsNullOrEmpty(currentChassisNumber) ||
                        currentChassisNumber.Equals(chassisNumber) == false;
                    if (isChassisNumberChanged)
                    {
                        validChassisNumber = IsChassisNumberValid(chassisNumber);
                    }
                    else
                    {
                        validChassisNumber = true;
                    }
                }
            }
            else //Creates new automobile
            {
                validVin = IsVinValid(vin);
                validChassisNumber = IsChassisNumberValid(chassisNumber);
            }
            int engineCubValue = -1;
            bool emptyEngineCub = string.IsNullOrEmpty(engineCubTxt);
            bool validEngineCub = emptyEngineCub || Int32.TryParse(engineCubTxt, out engineCubValue);
            DateTime makeYearValue = DateTime.Now;
            bool emptyMakeYear = string.IsNullOrEmpty(makeYearTxt);
            bool validMakeYear = emptyMakeYear || 
                DateTime.TryParseExact(makeYearTxt, CarServiceConstants.DATE_FORMAT,
                new CultureInfo(CarServiceConstants.ENGLISH_CULTURE_INFO), DateTimeStyles.None, out makeYearValue);
            string notificationMsg = GenerateNotificationErrorMsg(validVin, validChassisNumber, validEngineCub, validMakeYear);
            if (validVin && validChassisNumber && validEngineCub && validMakeYear)
            {
                DateTime? makeYear = null;
                if (emptyMakeYear == false)
                {
                    makeYear = makeYearValue;
                }
                int? engineCub = null;
                if (emptyEngineCub == false)
                {
                    engineCub = engineCubValue;
                }
                bool successfullySaved = SaveAutomobile(auto, vin, chassisNumber, engineNumberTxt,
                    engineCub, make, model, makeYear, owner, phoneNumber, colour, description);
                if (successfullySaved)
                {
                    notificationMsg += "Car is saved successfully.<br/>";
                }
            }            
            ShowNotificationMessage(notificationMsg);
        }

        private void ShowNotificationMessage(string notificationMsg)
        {
            if (string.IsNullOrEmpty(notificationMsg) == false)
            {
                this.notificationMsg.Text = notificationMsg;
                this.notificationMsg.Visible = true;
            }
        }
        
        private bool IsChassisNumberValid(string chassisNumber)
        {
            if(string.IsNullOrEmpty(chassisNumber))
            {
                return false;
            }
            return this.persister.IsChassisNumberExists(chassisNumber);
        }

        private bool IsVinValid(string vin)
        {
            if (string.IsNullOrEmpty(vin))
            {
                return false;
            }
            return this.persister.IsVinExists(vin);
        }

        private bool SaveAutomobile(Automobile auto, string vin, string chassisNumber, string engineNumber, 
            int? engineCub, string make, string model, DateTime? makeYear, string owner, string phoneNumber, 
            string colour, string description)
        {
            if (auto == null)
            {
                auto = new Automobile();
                auto = InitAutomobile(auto, vin, chassisNumber, engineNumber, engineCub, make, model, makeYear, owner, phoneNumber, colour, description);
                this.persister.CreateAutomobile(auto);
            }
            else
            {
                auto = InitAutomobile(auto, vin, chassisNumber, engineNumber, engineCub, make, model, makeYear, owner, phoneNumber, colour, description);
            }
            this.persister.SaveChanges();
            this.notificationMsg.CssClass = CarServiceConstants.POSITIVE_CSS_CLASS_NAME;
            return true;
        }

        private Automobile InitAutomobile(Automobile auto, string vin, string chassisNumber, string engineNumber, 
            int? engineCub, string make, string model, DateTime? makeYear, string owner, string phoneNumber, 
            string colour, string description)
        {
            auto.Vin = vin;
            auto.ChassisNumber = chassisNumber;
            auto.EngineNumber = engineNumber;
            auto.EngineCub = engineCub;
            auto.MakeYear = makeYear;
            auto.Make = string.IsNullOrEmpty(make) ? null : make;
            auto.Model = string.IsNullOrEmpty(model) ? null : model;
            auto.Owner = string.IsNullOrEmpty(owner) ? null : owner;
            auto.PhoneNumber = string.IsNullOrEmpty(phoneNumber) ? null : phoneNumber;
            auto.Colour = string.IsNullOrEmpty(colour) ? null : colour;
            auto.Description = string.IsNullOrEmpty(description) ? null : description;
            return auto;
        }

        private string GenerateNotificationErrorMsg(bool validVin, bool validChassisNumber, 
            bool validEngineCub, bool validMakeYear)
        {
            string notificationMsg = string.Empty;
            if (validVin == false)
            {
                notificationMsg += "Vin is not valid or unique.<br/>";
            }
            if (validChassisNumber == false)
            {
                notificationMsg += "Chassis number is not valid or unique.<br/>";
            }
            if (validEngineCub == false)
            {
                notificationMsg += "Engine cub is not valid number.<br/>";
            }
            if (validMakeYear == false)
            {
                notificationMsg += "Make year is not valid.<br/>";
            }
            if (string.IsNullOrEmpty(notificationMsg) == false)
            {
                this.notificationMsg.CssClass = CarServiceConstants.NEGATIVE_CSS_CLASS_NAME;
            }
            return notificationMsg;
        }

        private void LoadAutomobileInformation(Automobile auto)
        {
            this.AutoVin.Text = auto.Vin;
            this.AutoChassisNumber.Text = auto.ChassisNumber;
            this.AutoEngineNumber.Text = auto.EngineNumber;
            this.AutoEngineCub.Text = auto.EngineCub.ToString();
            this.AutoMake.Text = auto.Make;
            this.AutoModel.Text = auto.Model;
            CultureInfo cultureInfo = new CultureInfo(CarServiceConstants.ENGLISH_CULTURE_INFO);
            DateTime? makeYear = auto.MakeYear;
            string makeYearTxt = string.Empty;
            if (makeYear.HasValue)
            {
                makeYearTxt = makeYear.Value.ToString(CarServiceConstants.DATE_FORMAT, cultureInfo);
            }
            this.AutoMakeYearCalendar.SelectedDate = makeYearTxt;
            this.AutoColour.Text = auto.Colour;
            this.AutoDescription.Text = auto.Description;
            this.AutoOwner.Text = auto.Owner;
            this.AutoPhoneNumber.Text = auto.PhoneNumber;
        }

        private void LoadAutomobilePage()
        {
            object autoIdObject = Session[CarServiceConstants.AUTOMOBILE_ID_REQUEST_PARAM_NAME];
            if (autoIdObject != null && string.IsNullOrEmpty(autoIdObject.ToString()) == false)
            {
                int autoId;
                if (Int32.TryParse(autoIdObject.ToString(), out autoId))
                {
                    Automobile auto = this.persister.GetAutomobilById(autoId);
                    if (auto != null)
                    {
                        LoadAutomobileInformation(auto);
                    }
                }
            }
            else
            {
                this.AutoMakeYearCalendar.SelectedDate = DateTime.Now.ToString(CarServiceConstants.DATE_FORMAT);
            }
        }
    }
}