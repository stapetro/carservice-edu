using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using persistence;
using System.Data.Objects;
using presentation.utils;
using constants;

namespace presentation
{
    public partial class MembersCars : System.Web.UI.Page
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
                BindAutomobilesGrid();
            }
        }

        protected void EditAutomobileEventHandler_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int rowIndex = e.NewEditIndex;
            string autoId = CarServicePresentationUtility.GetGridCellContent(this.automobilesGrid, rowIndex, 0);
            if (string.IsNullOrEmpty(autoId) == false)
            {
                string editAutomobilePageUrl = "~/Members/Cars/AddCar.aspx?"
                    + CarServiceConstants.AUTOMOBILE_ID_REQUEST_PARAM_NAME + "=" + autoId;
                Response.Redirect(editAutomobilePageUrl);
            }
        }

        private void BindAutomobilesGrid()
        {
            ObjectSet<Automobile> automobiles = this.persister.GetAutomobiles();
            this.automobilesGrid.DataSource = automobiles;
            this.automobilesGrid.DataBind();
        }
    }
}