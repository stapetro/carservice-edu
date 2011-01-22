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
                object customSpareParts = GetSparePartsFormatForListBox(spareParts);
                BindListBox(this.unselectedSpareParts, customSpareParts);
                int repairCardId = this.persister.GetRepairCardMaxId() + 1;
                this.repairCardIdTxt.Text = repairCardId.ToString();
                this.startRepairDate.SelectedDate = 
                    DateTime.Now.ToString(CarServiceConstants.DATE_FORMAT, new CultureInfo(CarServiceConstants.ENGLISH_CULTURE_INFO));
                this.operatorLbl.Text = this.User.Identity.Name;
            }
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
            MoveListItems(this.unselectedSpareParts, this.selectedSpareParts, false);            
        }

        protected void UnselectSpareParts_OnClick(object sender, EventArgs e)
        {
            MoveListItems(this.selectedSpareParts, this.unselectedSpareParts, true);            
        }

        protected void CancelRepairCard_OnClick(object sender, EventArgs e)
        {
            string continueUrl = "~/Members/RepairCards/RepairCards.aspx";
            Response.Redirect(continueUrl);
        }

        protected void SaveRepairCard_OnClick(object sender, EventArgs e)
        {
            StringBuilder notificationMsgOutput = new StringBuilder();
            DateTime? startRepairDate = null;
            bool validStartRepairDate = ProcessStartRepairDate(startRepairDate, notificationMsgOutput);
            bool validSparePartsPrice = false;
            bool validRepairPrice = false;
            decimal sparePartsPrice = 0M;
            decimal repairPrice = 0M;            
            ProcessRepairPrices(out validSparePartsPrice, out validRepairPrice, out sparePartsPrice,
                out repairPrice, notificationMsgOutput);
            Automobile automobile = null;
            bool validAutomobileId = ProcessAutomobileId(out automobile, notificationMsgOutput);            
            ListItemCollection selectedSparePartItems = this.selectedSpareParts.Items;
            bool validSpareParts = IsSparePartItemsValid(selectedSparePartItems, notificationMsgOutput);
            if (validAutomobileId && validSparePartsPrice && validRepairPrice && validSpareParts &&
                (validStartRepairDate && startRepairDate.HasValue))
            {
                string description = this.repairCardDescription.Text;
                SaveRepairCard(automobile, startRepairDate.Value, description, 
                    sparePartsPrice, repairPrice, selectedSparePartItems);
            }
        }

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
            AddSpareParts(newRepairCard, sparePartItems);
            this.persister.CreateRepairCard(newRepairCard);
            this.persister.SaveChanges();
        }

        private object GetSparePartsFormatForListBox(IQueryable<SparePart> spareParts)
        {
            var customSpareParts =
                from sp in spareParts
                select new
                {
                    PartId = sp.PartId,
                    PartName = sp.Name
                };
            return customSpareParts;
        }

        private object GetSparePartsFormatForListBox(List<SparePart> spareParts)
        {
            var customSpareParts =
                from sp in spareParts
                select new
                {
                    PartId = sp.PartId,
                    PartName = sp.Name
                };
            return customSpareParts;
        }

        private void BindListBox(ListBox listBox, object listBoxDataSource)
        {
            listBox.DataSource = listBoxDataSource;
            listBox.DataBind();
        }

        private void AddSparePartId(List<int> sparePartIds, string partIdTxt)
        {
            int partId;
            if (Int32.TryParse(partIdTxt, out partId))
            {
                sparePartIds.Add(partId);
            }
        }

        private void MoveListItems(ListBox srcListBox, ListBox destListBox, bool srcPriceCalculation)
        {
            int[] srcSelectedIndices = srcListBox.GetSelectedIndices();
            ListItemCollection destListItems = destListBox.Items;
            int totalNumberOfDestItems = destListItems.Count + srcSelectedIndices.Length;
            List<int> destItemValues = new List<int>(totalNumberOfDestItems);
            foreach (ListItem item in destListItems)
            {
                AddSparePartId(destItemValues, item.Value);
            }
            foreach (int selectedIndex in srcSelectedIndices)
            {
                ListItem item = srcListBox.Items[selectedIndex];
                AddSparePartId(destItemValues, item.Value);
            }
            BindSparePartsLists(destItemValues, srcListBox, destListBox, srcPriceCalculation);
        }

        private void BindSparePartsLists(List<int> destItemSparePartIds, ListBox srcListBox, 
            ListBox destListBox, bool srcPriceCalculation)
        {
            IQueryable<SparePart> spareParts = this.persister.GetSpareParts();
            List<SparePart> srcSpareParts = new List<SparePart>();
            List<SparePart> destSpareParts = new List<SparePart>();
            decimal totalPartsPrice = 0.0M;
            foreach (SparePart currSP in spareParts)
            {
                if (destItemSparePartIds.Contains(currSP.PartId))
                {
                    destSpareParts.Add(currSP);
                    if (srcPriceCalculation == false)
                    {
                        totalPartsPrice += currSP.Price;
                    }
                }
                else
                {
                    srcSpareParts.Add(currSP);
                    if (srcPriceCalculation)
                    {
                        totalPartsPrice += currSP.Price;
                    }
                }
            }
            this.sparePartsPrice.Text = totalPartsPrice.ToString();
            this.repairPrice.Text = totalPartsPrice.ToString();
            object customSpareParts = GetSparePartsFormatForListBox(srcSpareParts);
            BindListBox(srcListBox, customSpareParts);
            customSpareParts = GetSparePartsFormatForListBox(destSpareParts);
            BindListBox(destListBox, customSpareParts);
        }

        private bool ProcessStartRepairDate(DateTime? startRepairDate, StringBuilder notificationMsgOutput)
        {
            string startRepairDateTxt = this.startRepairDate.SelectedDate;
            bool validStartRepairDate = string.IsNullOrEmpty(startRepairDateTxt);
            if (validStartRepairDate == false)
            {
                notificationMsgOutput.Append("Start repair date is required.<br/>");
            }
            else
            {
                DateTime startRepairDateValue = DateTime.Now;
                validStartRepairDate = CarServiceUtility.IsValidDate(startRepairDateTxt, out startRepairDateValue);
                if (validStartRepairDate == true)
                {
                    startRepairDate = startRepairDateValue;
                }
                else
                {
                    notificationMsgOutput.Append("Start repair date is not in valid format.<br/>");
                }
            }
            return validStartRepairDate;
        }

        private void ProcessRepairPrices(out bool validSparePartsPrice, out bool validRepairPrice,
            out decimal sparePartsPrice, out decimal repairPrice, StringBuilder notificationMsgOutput)
        {
            validSparePartsPrice = false;
            validRepairPrice = false;
            sparePartsPrice = 0M;
            repairPrice = 0M;
            string repairPriceTxt = this.repairPrice.Text;
            if (string.IsNullOrEmpty(repairPriceTxt))
            {
                notificationMsgOutput.Append("Repair price is required.<br/>");
                validRepairPrice = false;
            }
            else
            {
                string sparePartsPriceTxt = this.sparePartsPrice.Text;
                validSparePartsPrice = Decimal.TryParse(sparePartsPriceTxt, out sparePartsPrice);
                validRepairPrice = Decimal.TryParse(repairPriceTxt, out repairPrice);
                if (validSparePartsPrice == false)
                {
                    notificationMsgOutput.Append("Spare parts price is not valid.<br/>");
                }
                if (validRepairPrice == false)
                {
                    notificationMsgOutput.Append("Repair price is not valid.<br/>");
                }
                if (validSparePartsPrice == true && validRepairPrice == true)
                {
                    validRepairPrice = (repairPrice >= sparePartsPrice);
                    if (validRepairPrice == false)
                    {
                        notificationMsgOutput.Append("Repair price should be larger than or equal to spare parts price.<br/>");
                    }
                }
            }
        }

        private bool ProcessAutomobileId(out Automobile automobile, StringBuilder notificationMsgOutput)
        {
            automobile = null;
            int automobileId;
            string automobileIdTxt = this.automobileDropDown.SelectedValue;
            bool validAutomobile = Int32.TryParse(automobileIdTxt, out automobileId);
            if (validAutomobile)
            {
                if (automobileId >= 0)
                {
                    automobile = this.persister.GetAutomobilById(automobileId);
                    validAutomobile = (automobile != null);
                }
                else
                {
                    validAutomobile = false;
                }
            }
            if (validAutomobile == false)
            {
                notificationMsgOutput.Append("Please select valid car.<br/>");
            }
            return validAutomobile;
        }

        private bool IsSparePartItemsValid(ListItemCollection selectedSparePartItems, StringBuilder notificationMsgOutput)
        {
            bool validSpareParts = (selectedSparePartItems.Count == 0);
            if (validSpareParts == false)
            {
                notificationMsgOutput.Append("Spare parts are not selected.<br/>");
            }
            return validSpareParts;
        }

        private void AddSpareParts(RepairCard repairCard, ListItemCollection selectedSparePartItems)
        {
            foreach (ListItem item in selectedSparePartItems)
            {
                int sparePartId;
                if (Int32.TryParse(item.Value, out sparePartId))
                {
                    SparePart sparePart = this.persister.GetSparePartById(sparePartId);
                    if (sparePart != null)
                    {
                        repairCard.SpareParts.Add(sparePart);
                    }
                }
            }
        }
        
    }
}