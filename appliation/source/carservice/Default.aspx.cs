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
            //MembershipUser createdUser = Membership.CreateUser("stanislav.petrov", "stanislav.petrov", "stanislav.petrov@aspnet.net");
            //ProfileCommon profileCommon = Profile.GetProfile(createdUser.UserName);
            //profileCommon.FirstName = "Stanislav";
            //profileCommon.LastName = "Petrov";
            //profileCommon.Save();
            //Membership.DeleteUser("Test");
            //Membership.DeleteUser("test1");
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
            //persister.TestCreateRepairCard();           
            //persister.SaveChanges();
            //TestRepairCardQueries(persister);
            persister.ReleaseConnection();
        }
    }

    //TODO: To be deleted
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

    //TODO: To be deleted
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

    //TODO: To be deleted
    private void TestRepairCardQueries(CarServicePersister persister)
    {
        StringBuilder output = new StringBuilder("Finished repair cards between 2010-12-01 and 2010-12-06<br/>");
        IQueryable<RepairCard> foundRepairCards = persister.GetFinishedRepairCards(new DateTime(2010, 12, 1), new DateTime(2010, 12, 6));
        foreach (RepairCard card in foundRepairCards)
        {
            output.Append(card.CardId + ", ");
        }
        output.Append("<br/>Unfinished repair cards for 2010-12-03<br/>");
        foundRepairCards = persister.GetUnfinishedRepairCards(new DateTime(2010, 12, 3));
        foreach (RepairCard card in foundRepairCards)
        {
            output.Append(card.CardId + ", ");
        }
        output.Append("<br/>Unfinished repair cards for 2010-12-03 by chassis number<br/>");
        foundRepairCards = persister.GetUnfinishedRepairCardsByChassisNumber(new DateTime(2010, 12, 3), "XMCLRDA2A3F011227");
        foreach (RepairCard card in foundRepairCards)
        {
            output.Append(card.CardId + ", ");
        }
        output.Append("<br/>Unfinished repair cards for 2010-12-03 by part of chassis number<br/>");
        foundRepairCards = persister.GetUnfinishedRepairCardsByChassisNumber(new DateTime(2010, 12, 3), "RDA2A3F");
        foreach (RepairCard card in foundRepairCards)
        {
            output.Append(card.CardId + ", ");
        }
        output.Append("<br/>Unfinished repair cards for 2010-12-03 by vin<br/>");
        foundRepairCards = persister.GetUnfinishedRepairCardsByVin(new DateTime(2010, 12, 3), "PV2222");
        foreach (RepairCard card in foundRepairCards)
        {
            output.Append(card.CardId + ", ");
        }
        output.Append("<br/>Unfinished repair cards for 2010-12-03 by part of vin<br/>");
        foundRepairCards = persister.GetUnfinishedRepairCardsByVin(new DateTime(2010, 12, 3), "V22");
        foreach (RepairCard card in foundRepairCards)
        {
            output.Append(card.CardId + ", ");
        }
        output.Append("<br/>");
        //this.users.Text = output.ToString();
    }
}