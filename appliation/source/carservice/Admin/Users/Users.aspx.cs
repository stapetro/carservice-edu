using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using businesslogic;
using constants;
using presentation.utils;

namespace presentation
{
    public partial class AdminUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                BindUsersGrid();
            }
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
                        e.Row.CssClass = CarServiceConstants.INACTIVE_CSS_CLASS_NAME;
                    }
                }
            }
        }

        protected void EditUserEventHandler_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int rowIndex = e.NewEditIndex;
            int userNameCellIndex = 0;
            string userName = CarServicePresentationUtility.GetGridCellContent(this.carServiceUsers, rowIndex, userNameCellIndex);
            if (string.IsNullOrEmpty(userName) == false)
            {
                string editUserPageUrl = "~/Admin/Users/EditUser.aspx?"
                    + CarServiceConstants.USER_NAME_REQUEST_PARAM_NAME + "=" + userName;
                Response.Redirect(editUserPageUrl, false);
            }             
        }

        protected void DeactivateUserEventHandler_RowDeliting(object sender, GridViewDeleteEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int userNameCellIndex = 0;
            string userName = CarServicePresentationUtility.GetGridCellContent(this.carServiceUsers, rowIndex, userNameCellIndex);
            if (string.IsNullOrEmpty(userName) == false)
            {
                MembershipUser user = Membership.GetUser(userName);
                if (user != null && user.IsApproved == true)
                {
                    user.IsApproved = false;
                    Membership.UpdateUser(user);                    
                }
            }
            BindUsersGrid();
        }

        protected void UsersGridView_PageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            this.carServiceUsers.PageIndex = e.NewPageIndex;
            BindUsersGrid();
        }

        private void BindUsersGrid()
        {
            List<CarServiceUser> carServiceUsers = GetUsers();
            this.carServiceUsers.DataSource = carServiceUsers;
            this.carServiceUsers.DataBind();
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
    }
}
