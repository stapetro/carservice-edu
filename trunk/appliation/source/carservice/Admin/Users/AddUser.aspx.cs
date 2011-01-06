using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using businesslogic.utils;
using constants;


namespace presentation
{

    public partial class AdminAddUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /* *** Resetting user password without knowing old password ***
            MembershipUser user = Membership.GetUser("ScottBrown");
            user.ChangePassword(user.ResetPassword(), "ScottBrown");*/
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            FormsAuthentication.SetAuthCookie(RegisterUser.UserName, false /* createPersistentCookie */);
            if(UserRolesUtility.IsOperatorRoleExists() == false)
            {
                Roles.CreateRole(CarServiceConstants.OPERATOR_ROLE_NAME);
            }
            Roles.AddUserToRole(RegisterUser.UserName, CarServiceConstants.OPERATOR_ROLE_NAME);
            ProfileCommon profileCommon = Profile.GetProfile(RegisterUser.UserName);
            TextBox firstName = (TextBox)RegisterUserWizardStep.ContentTemplateContainer.FindControl("FirstName");
            if (firstName != null)
            {
                profileCommon.FirstName = firstName.Text;
            }
            TextBox lastName = (TextBox)RegisterUserWizardStep.ContentTemplateContainer.FindControl("LastName");
            if (lastName != null)
            {
                profileCommon.LastName = lastName.Text;
            }
            profileCommon.Save();
            string continueUrl = RegisterUser.ContinueDestinationPageUrl;
            if (String.IsNullOrEmpty(continueUrl))
            {
                continueUrl = "~/Admin/Users/Users.aspx";
            }
            Response.Redirect(continueUrl);
        }
    }
}
