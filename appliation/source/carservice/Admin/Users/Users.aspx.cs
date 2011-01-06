using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using businesslogic;

namespace presentation
{
    public partial class AdminUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

        protected void CarServiceUsersGridView_RowCreated(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                object isApprovedObject = DataBinder.Eval(e.Row.DataItem, "IsActive");
                if(isApprovedObject != null)
                {
                    bool isApproved = (bool)isApprovedObject;
                    if (isApproved == false)
                    {
                        e.Row.CssClass = "inactive";
                    }
                }
            }
        }
    }
}
