using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using persistence;
using System.Collections;
using constants;
using System.Globalization;
using System.Web.Security;
using businesslogic.utils;
using System.Text;
using presentation.utils;

namespace presentation
{
    public partial class MembersAddRepairCard : System.Web.UI.Page
    {
        ICarServicePersister persister;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.persister == null)
            {
                this.persister = new CarServicePersister();
            }
            if (IsPostBack == false)
            {
                IQueryable<SparePart> spareParts = this.persister.GetSpareParts();
                object customSpareParts = CarServicePresentationUtility.GetSparePartsFormatForListBox(spareParts);
                CarServicePresentationUtility.BindListBox(this.unselectedSpareParts, customSpareParts);
                //int repairCardId = this.persister.GetRepairCardMaxId() + 1;
                //this.repairCardIdLbl.Text = repairCardId.ToString();
                this.startRepairDate.SelectedDate = 
                    DateTime.Now.ToString(CarServiceConstants.DATE_FORMAT, new CultureInfo(CarServiceConstants.ENGLISH_CULTURE_INFO));
                this.operatorLbl.Text = this.User.Identity.Name;
            }
            CarServicePresentationUtility.HideNotificationMsgList(this.notificationMsgList);
        }

        protected void SearchAutomobile_OnClick(object sender, EventArgs e)
        {
            string vinChassis = this.VinChassisTxt.Text;
            if (string.IsNullOrEmpty(vinChassis) == false)
            {
                IQueryable<Automobile> foundAutomobiles = this.persister.GetAutomobilesByVinChassis(vinChassis);
                var customAutomobileFormat = 
                    from auto in foundAutomobiles
                    select new
                    {
                        AutomobileId = auto.AutomobileId,
                        AutomobileRepresentation = auto.Vin + " / " + auto.ChassisNumber
                    };
                this.automobileDropDown.DataSource = customAutomobileFormat;
                this.automobileDropDown.DataBind();
            }
        }

        protected void SelectSpareParts_OnClick(object sender, EventArgs e)
        {
            decimal partsPrice = 0M;
            CarServicePresentationUtility.MoveListItems(this.unselectedSpareParts, this.selectedSpareParts, false, this.persister, out partsPrice);
            this.sparePartsPrice.Text = partsPrice.ToString();
            this.repairPrice.Text = partsPrice.ToString();
        }

        protected void UnselectSpareParts_OnClick(object sender, EventArgs e)
        {
            decimal partsPrice = 0M;
            CarServicePresentationUtility.MoveListItems(this.selectedSpareParts, this.unselectedSpareParts, true, this.persister, out partsPrice);
            this.sparePartsPrice.Text = partsPrice.ToString();
            this.repairPrice.Text = partsPrice.ToString();
        }

        protected void CancelRepairCard_OnClick(object sender, EventArgs e)
        {
            string continueUrl = "~/Members/RepairCards/RepairCards.aspx";
            Response.Redirect(continueUrl);
        }

        protected void SaveRepairCard_OnClick(object sender, EventArgs e)
        {
            CarServicePresentationUtility.ClearNotificationMsgList(this.notificationMsgList);

            DateTime? startRepairDate = null;
            string startRepairDateTxt = this.startRepairDate.SelectedDate;
            bool validStartRepairDate = CarServicePresentationUtility.ProcessStartRepairDate(startRepairDateTxt, 
                this.notificationMsgList, out startRepairDate);

            decimal sparePartsPrice = 0M;
            decimal repairPrice = 0M;
            string repairPriceTxt = this.repairPrice.Text;
            string sparePartsPriceTxt = this.sparePartsPrice.Text;
            bool validPrices = CarServicePresentationUtility.ProcessRepairPrices(sparePartsPriceTxt, repairPriceTxt, 
                this.notificationMsgList, out sparePartsPrice, out repairPrice);
            
            string automobileIdTxt = this.automobileDropDown.SelectedValue;
            Automobile automobile = CarServiceUtility.GetAutomobile(automobileIdTxt, this.persister);
            bool validAutomobileId = automobile != null;

            ListItemCollection selectedSparePartItems = this.selectedSpareParts.Items;
            bool validSpareParts = CarServicePresentationUtility.IsSparePartItemsValid(selectedSparePartItems, this.notificationMsgList);

            if (validAutomobileId && validPrices && validSpareParts &&
                (validStartRepairDate && startRepairDate.HasValue))
            {
                string description = this.repairCardDescription.Text;
                SaveRepairCard(automobile, startRepairDate.Value, description,
                    sparePartsPrice, repairPrice, selectedSparePartItems);
                CarServicePresentationUtility.AppendNotificationMsg("Repair card is saved successfully", this.notificationMsgList);
                this.notificationMsgList.CssClass = CarServiceConstants.POSITIVE_CSS_CLASS_NAME;
            }
            else
            {
                this.notificationMsgList.CssClass = CarServiceConstants.NEGATIVE_CSS_CLASS_NAME;
            }
            CarServicePresentationUtility.ShowNotificationMsgList(this.notificationMsgList);             
        }

        protected void Automobile_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            int automobileId;
            string automobileIdTxt = e.Value;
            bool validAutomobile = Int32.TryParse(automobileIdTxt, out automobileId) && (automobileId >= 0);
            e.IsValid = validAutomobile;
        }

        protected void Price_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            double price;
            e.IsValid = Double.TryParse(e.Value, out price);
        }

        #region Private methods

        private void SaveRepairCard(Automobile automobile, DateTime startRepairDate, string description, decimal sparePartsPrice, 
            decimal repairPrice, ListItemCollection sparePartItems)
        {
            MembershipUser currentUser = Membership.GetUser();
            RepairCard newRepairCard = new RepairCard()
            {
                Automobile = automobile,
                UserId = ((System.Guid)currentUser.ProviderUserKey),
                StartRepair = startRepairDate,
                Description = (string.IsNullOrEmpty(description) ? null : description),
                PartPrice = sparePartsPrice,
                CardPrice = repairPrice
            };
            CarServicePresentationUtility.AddSpareParts(newRepairCard, sparePartItems, this.persister);
            this.persister.CreateRepairCard(newRepairCard);
            this.persister.SaveChanges();
        }

        #endregion
    }
}