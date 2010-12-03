using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;
using System.Data.Objects.DataClasses;
using persistence;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            //Creates new user
            //MembershipUser createdUser = Membership.CreateUser("test1", "test1@test.test", "test1@test.test");
            //ProfileCommon profileCommon = Profile.GetProfile(createdUser.UserName);
            //profileCommon.FirstName = "Stanislav";
            //profileCommon.LastName = "Petrov";
            //profileCommon.Save();
            //Deactivate user
            //createdUser.IsApproved = false;
            //StringBuilder sb = new StringBuilder();
            //MembershipUserCollection users = Membership.GetAllUsers();
            //foreach (MembershipUser user in users)
            //{
            //    sb.Append(user.UserName + ", " + user.Email + "<br/>");
            //}
            //this.users.Text = sb.ToString();
            CarServicePersister persister = new CarServicePersister();
            //using (persistence.Entities entities = new persistence.Entities())
            //{
                //    Automobile automobile = new Automobile()
                //    {
                //        Vin = "PA6504",
                //        ChassisNumber = "XMCLRDA2A3F011227",
                //        EngineNumber = "DGB 06 0081 U 0017 B"
                //    };
                //    entities.Automobiles.AddObject(automobile);
                //    entities.SaveChanges();                                
            //}
            //MembershipUser user = Membership.GetUser();
            //RepairCard repairCard = new RepairCard()
            //{
            //    Automobile = auto,
            //    UserId = (System.Guid)user.ProviderUserKey,
            //    StartRepair = DateTime.Now
            //};            
            persister.TestCreateRepairCard();
            //RepairCard card = persister.GetRepairCardById(2);
            //persister.DeleteRepairCard(card);
            persister.SaveChanges();
            persister.ReleaseConnection();
        }
    }

    private void CreateAutomobile(Entities carServiceEntities)
    {
        Automobile automobile = new Automobile()
        {
            Vin = "PA6504",
            ChassisNumber = "XMCLRDA2A3F011227",
            EngineNumber = "DGB060081U0017B"
        };
        carServiceEntities.Automobiles.AddObject(automobile);
        carServiceEntities.SaveChanges();
    }

    private void CreateSpareParts()
    {
        //string[] sparePartNames = { "Bumper", "Bonnet/Hood", "Cowl screen", "Fascia rear and support", 
        //                              "Fender (wing or mudguard)", "Spoiler", "Trim package", "Glass" };
        //decimal[] sparePartPrices = {99.9m,  134.14m, 888.7m, 45m, 452.63m, 500m, 1343.47m, 140.29m};
        //for (int i = 0; i < sparePartNames.Length; i++)
        //{
        //    SparePart sparePart = new SparePart()
        //    {
        //        Name = sparePartNames[i],
        //        Price = sparePartPrices[i],
        //        IsActive = true
        //    };
        //    persister.CreateSparePart(sparePart);
        //}
    }

    private void CreateRepairCards()
    {

    }

    private void CreateAutomobiles()
    {
        string[] vins = { "1M8GDM9", "AXKP04", "CX2788", "1G1FP22P", "XS2100001", "CA7972KK", "PK1234RT" };
        string[] makes = { };
        string[] models = { };
        string[] chassisNumbers = { "XMCLRDA2A3F011227", "XABNRDA6A3F031227", "XMCLRDA2A3F011226", "XMCLRDA2A3F011225", "XMCLRDA2A3F011224", "XMCLRDA2A3F011223", "XMCLRDA2A3F011222" };
        string[] engineNumbers = { "DGB060081U0017B", "DGB060081U0017C", "DGB060081U0017D", "DGB060081U0017F", "DGB060081U0016G", "DGB060081U0017H", "DGB060081U0018I" };
        string[] colours = { };
        int[] engineCubs = { };
        string[] descriptions = { };
        string[] owners = { };
        string[] phoneNumbers = { };
    }
}