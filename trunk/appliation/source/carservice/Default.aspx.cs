using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using persistence;
using System.Web.Security;
using System.Text;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            StringBuilder sb = new StringBuilder();
            MembershipUserCollection users = Membership.GetAllUsers();
            foreach (MembershipUser user in users)
            {
                sb.Append(user.UserName + ", " + user.Email + "<br/>");
            }
            this.users.Text = sb.ToString();
        }
    }
}