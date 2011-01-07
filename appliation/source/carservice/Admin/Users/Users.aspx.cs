using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using businesslogic;
using constants;

namespace presentation
{
    public partial class AdminUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                List<CarServiceUser> carServiceUsers = GetUsers();
                this.carServiceUsers.DataSource = carServiceUsers;
                this.carServiceUsers.DataBind();
            }
        }

        private List<CarServiceUser> GetUsers()
        {
            List<CarServiceUser> carServiceUsers = new List<CarServiceUser>();
            MembershipUserCollection membershipUsers = Membership.GetAllUsers();
            foreach (MembershipUser currentUser in membershipUsers)
            {
                ProfileCommon profile = Profile.GetProfile(currentUser.UserName);
                if (profile != null)
                {
                    carServiceUsers.Add(new CarServiceUser(currentUser, profile));
                }
            }
            return carServiceUsers;
        }

        protected void CarServiceUsersGridView_RowCreated(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                object isApprovedObject = DataBinder.Eval(e.Row.DataItem, "IsActive");
                if (isApprovedObject != null)
                {
                    bool isApproved = (bool)isApprovedObject;
                    if (isApproved == false)
                    {
                        e.Row.CssClass = "inactive";
                    }
                }
            }
        }

        protected void EditUserEventHandler_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int rowIndex = e.NewEditIndex;
            GridViewRow rowToBeEdited = this.carServiceUsers.Rows[rowIndex];
            if (rowToBeEdited.RowType == DataControlRowType.DataRow)
            {
                TableCell userNameCell = rowToBeEdited.Cells[0];
                //TableCell emailCell = rowToBeEdited.Cells[1];
                if (userNameCell != null)
                {
                    string userName = userNameCell.Text;
                    //string email = emailCell.Text;
                    if (string.IsNullOrEmpty(userName) == false)
                    {
                        string editUserPageUrl = "~/Admin/Users/EditUser.aspx?"
                            + CarServiceConstants.USER_NAME_REQUEST_PARAM_NAME + "=" + userName;
                        Response.Redirect(editUserPageUrl, false);
                    }
                }
            }
        }
    }
}
