using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using persistence;
using constants;

namespace presentation
{
    public partial class AdminAddSparePart : System.Web.UI.Page
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
                string partIdTxt = Request.QueryString[CarServiceConstants.SPARE_PART_ID_REQUEST_PARAM_NAME];                
                if(string.IsNullOrEmpty(partIdTxt))
                {
                    int maxPartId = this.persister.GetSparePartMaxId();
                    this.PartId.Text = (maxPartId + 1).ToString();
                }
                else
                {
                    this.PartId.Text = partIdTxt;
                    int partId;
                    if (Int32.TryParse(partIdTxt, out partId))
                    {
                        SparePart sparePart = this.persister.GetSparePartById(partId);
                        if (sparePart != null)
                        {
                            LoadSparePartInformation(sparePart);
                        }
                    }
                }
            }
            this.notificationMsg.Visible = false;
        }

        protected void CancelPart_OnClick(object sender, EventArgs e)
        {
            string continueUrl = "~/Admin/SpareParts/SpareParts.aspx";
            Response.Redirect(continueUrl);
        }        

        protected void AddPart_OnClick(object sender, EventArgs e)
        {
            string partIdTxt = this.PartId.Text;
            string partName = this.PartName.Text;
            string partPriceTxt = this.PartPrice.Text;
            int partId;
            decimal partPrice;
            int isPartActiveNum;
            bool isPartActive = false;
            bool validIdValue = Int32.TryParse(partIdTxt, out partId);
            string notificationMsg = string.Empty;
            this.notificationMsg.CssClass = "negativeMsg";
            if (validIdValue == false)
            {
                notificationMsg += "ID is not valid.<br/>";
            }
            bool validPriceValue = Decimal.TryParse(partPriceTxt, out partPrice);
            if (validPriceValue == false)
            {
                notificationMsg += "Price is not valid.<br/>";
            }
            if (validIdValue && validPriceValue
                && Int32.TryParse(this.PartActive.SelectedValue, out isPartActiveNum) == true)
            {                
                isPartActive = (isPartActiveNum == 1);
                SaveSparePart(partId, partName, partPrice, isPartActive);
                notificationMsg += "Part is saved successfully.<br/>";
                this.notificationMsg.CssClass = "positiveMsg";
            }
            if (string.IsNullOrEmpty(notificationMsg) == false)
            {
                this.notificationMsg.Text = notificationMsg;
                this.notificationMsg.Visible = true;
            }
        }

        private void LoadSparePartInformation(SparePart sparePart)
        {
            this.PartName.Text = sparePart.Name;
            this.PartPrice.Text = sparePart.Price.ToString();
            this.PartActive.SelectedValue = (sparePart.IsActive ? 1.ToString() : 0.ToString());
        }

        private void SaveSparePart(int partId, string partName, decimal partPrice, bool isPartActive)
        {
            SparePart updatedSparePart = this.persister.GetSparePartById(partId);
            if (updatedSparePart == null)
            {
                updatedSparePart = new SparePart()
                {
                    PartId = partId,
                    Name = partName,
                    Price = partPrice,
                    IsActive = isPartActive
                };
                this.persister.CreateSparePart(updatedSparePart);
            }
            else
            {
                updatedSparePart.Name = partName;
                updatedSparePart.Price = partPrice;
                updatedSparePart.IsActive = isPartActive;
            }
            this.persister.SaveChanges();
        }
    }
}