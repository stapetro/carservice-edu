using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using presentation.utils;
using constants;
using persistence;
using System.Data.Objects;

namespace presentation
{   
    public partial class AdminSpareParts : System.Web.UI.Page
    {
        private ICarServicePersister persister;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(persister == null) 
            {
                persister = new CarServicePersister();      
            }
            if (IsPostBack == false)
            {
                BindSparePartsGrid();
            }
        }

        protected void SparePartsGridView_RowCreated(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                object isApprovedObject = DataBinder.Eval(e.Row.DataItem, "IsActive");
                if (isApprovedObject != null)
                {
                    bool isApproved = (bool)isApprovedObject;
                    if (isApproved == false)
                    {
                        e.Row.CssClass = CarServiceConstants.INACTIVE_CLASS_NAME;
                    }
                }
            }
        }        

        protected void EditSparePartventHandler_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int rowIndex = e.NewEditIndex;
            string partId = CarServicePresentationUtility.GetGridCellContent(this.sparePartsGrid, rowIndex, 0);
            if (string.IsNullOrEmpty(partId) == false)
            {
                string editUserPageUrl = "~/Admin/SpareParts/EditSparePart.aspx?"
                    + CarServiceConstants.SPARE_PART_ID_REQUEST_PARAM_NAME + "=" + partId;
                Response.Redirect(editUserPageUrl, false);
            }
        }

        protected void DeactivateSparePartEventHandler_RowDeliting(object sender, GridViewDeleteEventArgs e)
        {
            int rowIndex = e.RowIndex;
            string partId = CarServicePresentationUtility.GetGridCellContent(this.sparePartsGrid, rowIndex, 0);
            if (string.IsNullOrEmpty(partId) == false)
            {
                int partIdNum;
                if (Int32.TryParse(partId, out partIdNum) == true)
                {
                    SparePart sparePart = this.persister.GetSparePartById(partIdNum);
                    if (sparePart != null && sparePart.IsActive)
                    {
                        sparePart.IsActive = false;
                        this.persister.SaveChanges();
                    }
                }               
            }
            BindSparePartsGrid();
        }

        private void BindSparePartsGrid()
        {
            ObjectSet<SparePart> spareParts = this.persister.GetSpareParts();
            this.sparePartsGrid.DataSource = spareParts;
            this.sparePartsGrid.DataBind();
        }
    }
}