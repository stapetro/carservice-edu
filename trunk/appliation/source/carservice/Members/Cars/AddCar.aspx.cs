using System;
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
    //TODO To be made custom control with attribute create = true | false
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
                string autoIdTxt = Request.QueryString[CarServiceConstants.AUTOMOBILE_ID_REQUEST_PARAM_NAME];
                if (string.IsNullOrEmpty(autoIdTxt) == false)
                {
                    int autoId;
                    if(Int32.TryParse(autoIdTxt, out autoId))
                    {
                        Automobile auto = this.persister.GetAutomobilById(autoId);
                        if (auto != null)
                        {
                            LoadAutomobileInformation(auto);
                        }
                    }
                }
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
            string makeYearTxt = string.Empty;
            string owner = this.AutoOwner.Text;
            string phoneNumber = this.AutoPhoneNumber.Text;
            string colour = this.AutoColour.Text;
            string description = this.AutoDescription.Text;
            bool validVin = IsVinValid(vin);
            bool isChassisNumberExists = IsChassisNumberExists(chassisNumber);
            bool validChassisNumber = string.IsNullOrEmpty(chassisNumber) == false && isChassisNumberExists == false;
            int engineCub;
            bool validEngineCub = Int32.TryParse(engineCubTxt, out engineCub);
            bool validEngineNumber = string.IsNullOrEmpty(engineNumberTxt) == false;
            string notificationMsg = string.Empty;
            this.notificationMsg.CssClass = CarServiceConstants.NEGATIVE_CSS_CLASS_NAME;
            if (validVin == false)
            {
                notificationMsg += "Vin is not valid or unique.<br/>";
            }
            if (isChassisNumberExists == false)
            {
                notificationMsg += "Chassis number is not valid or unique.<br/>";
            }
            if (validEngineCub == false)
            {
                notificationMsg += "Engine cub is not valid number.<br/>";
            }
            if (validVin && validChassisNumber && validEngineCub)
            {
                SaveAutomobile(vin, chassisNumber, engineNumberTxt, engineCub, make, 
                    model, makeYearTxt, owner, phoneNumber, colour, description);
                this.notificationMsg.CssClass = CarServiceConstants.POSITIVE_CSS_CLASS_NAME;
            }
            if (string.IsNullOrEmpty(notificationMsg) == false)
            {
                this.notificationMsg.Text = notificationMsg;
                this.notificationMsg.Visible = true;
            }
        }

        private void LoadAutomobileInformation(Automobile auto)
        {
            this.AutoVin.Text = auto.Vin;
            this.AutoChassisNumber.Text = auto.ChassisNumber;
            this.AutoEngineNumber.Text = auto.EngineNumber;
            this.AutoEngineCub.Text = auto.EngineCub.ToString();
            this.AutoMake.Text = auto.Make;
            this.AutoModel.Text = auto.Model;
            CultureInfo cultureInfo = new CultureInfo("bg-BG");
            DateTime? makeYear = auto.MakeYear;
            string makeYearTxt = string.Empty;
            if (makeYear.HasValue)
            {
                makeYearTxt = makeYear.Value.ToString("d", cultureInfo);
            }
            //this.AutoMakeYear.Text = string.Empty;
            this.AutoColour.Text = auto.Colour;
            this.AutoDescription.Text = auto.Description;
            this.AutoOwner.Text = auto.Owner;
            this.AutoPhoneNumber.Text = auto.PhoneNumber;
        }
        
        private bool IsChassisNumberExists(string chassisNumber)
        {
            //check for uniqueness
            return false;
        }

        private bool IsVinValid(string vin)
        {
            return false;
        }

        private void SaveAutomobile(string vin, string chassisNumber, string engineNumber, int engineCub, string make, 
                    string model, string makeYearTxt, string owner, string phoneNumber, string colour, string description)
        {
            Automobile auto = new Automobile();
            auto.Vin = vin;
            auto.ChassisNumber = chassisNumber;
            auto.EngineNumber = engineNumber;
            auto.EngineCub = engineCub;
            auto.Make = make;
            auto.Model = model;
            auto.MakeYear = DateTime.Now;
            auto.Owner = owner;
            auto.PhoneNumber = phoneNumber;
            auto.Colour = colour;
            auto.Description = description;
            this.persister.CreateAutomobile(auto);
            this.persister.SaveChanges();
        }
    }
}