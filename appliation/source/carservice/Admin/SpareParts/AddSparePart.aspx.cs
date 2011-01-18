using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using persistence;

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
                int maxPartId = this.persister.GetSparePartMaxId();
                this.PartId.Text = (maxPartId+ 1).ToString();
            }
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
            if (validIdValue == false)
            {
                //TODO Error msg
            }
            bool validPriceValue = Decimal.TryParse(partPriceTxt, out partPrice);
            if (validPriceValue == false)
            {
                //TODO Error msg
            }
            if (validIdValue && validPriceValue
                && Int32.TryParse(this.PartActive.SelectedValue, out isPartActiveNum) == true)
            {
                isPartActive = (isPartActiveNum == 1);
                SparePart newSparePart = new SparePart()
                {
                    PartId = partId, Name = partName, Price = partPrice, IsActive = isPartActive
                };
                this.persister.CreateSparePart(newSparePart);
                this.persister.SaveChanges();
                //TODO Display msg for success.
            }
        }
    }
}