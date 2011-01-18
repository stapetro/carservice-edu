using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace presentation.utils
{
    /// <summary>
    /// Summary description for CarServicePresentationUtility
    /// </summary>
    public class CarServicePresentationUtility
    {
        public CarServicePresentationUtility()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string GetGridCellContent(GridView gridView, int rowIndex, int columnIndex)
        {
            GridViewRow selectedRow = gridView.Rows[rowIndex];
            if (selectedRow.RowType == DataControlRowType.DataRow)
            {
                TableCell selectedCell = selectedRow.Cells[columnIndex];
                //TableCell emailCell = rowToBeEdited.Cells[1];
                if (selectedCell != null)
                {
                    string cellContent = selectedCell.Text;
                    return cellContent;
                }
            }
            return string.Empty;
        }
    }
}
