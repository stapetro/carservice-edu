using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;
using System.Data.Objects;

namespace persistence
{
    /// <summary>
    /// Implements ICarServicePersister persistence facade.
    /// </summary>
    public class CarServicePersister : ICarServicePersister
    {
        private Entities carServiceEntities;

        /// <summary>
        /// Creates object context and opens DB connection.
        /// </summary>
        public CarServicePersister()
        {
            this.carServiceEntities = new Entities();
        }

        public void CreateAutomobile(Automobile automobile)
        {
            this.carServiceEntities.Automobiles.AddObject(automobile);
        }

        public Automobile GetAutomobilById(int automobileId)
        {
            Automobile foundAutomobile = this.carServiceEntities.Automobiles.FirstOrDefault(automobile => automobile.AutomobileId == automobileId);
            return foundAutomobile;
        }

        public ObjectSet<Automobile> GetAutomobiles()
        {
            return this.carServiceEntities.Automobiles;
        }

        public void DeleteAutomobile(Automobile automobile)
        {
            this.carServiceEntities.Automobiles.DeleteObject(automobile);
        }

        public void CreateSparePart(SparePart sparePart)
        {
            this.carServiceEntities.SpareParts.AddObject(sparePart);
        }

        public SparePart GetSparePartById(int sparePartId)
        {
            SparePart sparePart = this.carServiceEntities.SpareParts.FirstOrDefault(part => part.PartId == sparePartId);
            return sparePart;
        }

        public ObjectSet<SparePart> GetSpareParts()
        {
            return this.carServiceEntities.SpareParts;
        }

        public int GetSparePartMaxId()
        {
            int partId = this.carServiceEntities.SpareParts.Max(sp => sp.PartId);
            return partId;
        }

        public void DeleteSparePart(SparePart sparePart)
        {
            this.carServiceEntities.DeleteObject(sparePart);
        }

        public void CreateRepairCard(RepairCard repairCard)
        {
            this.carServiceEntities.RepairCards.AddObject(repairCard);
        }

        public void DeleteRepairCard(RepairCard repairCard)
        {
            this.carServiceEntities.RepairCards.DeleteObject(repairCard);
        }

        public RepairCard GetRepairCardById(int cardId)
        {
            RepairCard repairCard = this.carServiceEntities.RepairCards.FirstOrDefault(card => card.CardId == cardId);
            return repairCard;
        }

        public IQueryable<RepairCard> GetUnfinishedRepairCards(DateTime startRepair)
        {
            IQueryable<RepairCard> unfinishedRepairCards =
                from repairCard in this.carServiceEntities.RepairCards
                where (repairCard.StartRepair == startRepair &&
                    repairCard.FinishRepair == null)
                select repairCard;
            return unfinishedRepairCards;
        }

        public IQueryable<RepairCard> GetUnfinishedRepairCardsByVin(DateTime startRepair, string vin)
        {
            IQueryable<RepairCard> unfinishedRepairCards =
                from repairCard in this.carServiceEntities.RepairCards
                where (repairCard.StartRepair == startRepair &&
                    repairCard.FinishRepair == null &&
                    (repairCard.Automobile.Vin.IndexOf(vin) >= 0))
                select repairCard;
            return unfinishedRepairCards;
        }

        public IQueryable<RepairCard> GetUnfinishedRepairCardsByChassisNumber(DateTime startRepair, string chassisNumber)
        {
            IQueryable<RepairCard> unfinishedRepairCards =
                from repairCard in this.carServiceEntities.RepairCards
                where (repairCard.StartRepair == startRepair &&
                    repairCard.FinishRepair == null &&
                    (repairCard.Automobile.ChassisNumber.IndexOf(chassisNumber) >= 0))
                select repairCard;
            return unfinishedRepairCards;
        }

        public IQueryable<RepairCard> GetFinishedRepairCards(DateTime fromFinishRepair, DateTime toFinishRepair)
        {
            IQueryable<RepairCard> unfinishedRepairCards =
                from repairCard in this.carServiceEntities.RepairCards
                where (repairCard.FinishRepair >= fromFinishRepair &&
                    repairCard.FinishRepair <= toFinishRepair)
                select repairCard;
            return unfinishedRepairCards;
        }

        public void SaveChanges()
        {
            this.carServiceEntities.SaveChanges();
        }

        public void ReleaseConnection()
        {
            if (this.carServiceEntities != null)
            {
                this.carServiceEntities.Dispose();
            }
        }

        #region Testing methods only
        //TODO: To be deleted
        public void TestCreateRepairCard()
        {
            string[] userNames = { "stanislav.petrov", "ScottBrown" };
            int[] autoIds = { 2, 4, 5, 6, 7, 8, 9, 10, 11, 2, 4, 5, 6, 7, 8, 9 };
            DateTime[] startReapirDates = { new DateTime(2010, 12, 2), new DateTime(2010, 12, 3), 
                                              new DateTime(2010, 11, 29),  new DateTime(2010, 10, 22),
                                              new DateTime(2010, 11, 15), new DateTime(2009, 5, 12), 
                                              new DateTime(2010, 1, 19), new DateTime(2009, 5, 13), 
                                              new DateTime(2010, 2, 18), new DateTime(2009, 5, 14), 
                                              new DateTime(2010, 1, 17), new DateTime(2009, 5, 15), 
                                              new DateTime(2010, 3, 16), new DateTime(2009, 12, 27), 
                                              new DateTime(2010, 4, 8), new DateTime(2010, 10, 3) };
            DateTime?[] finishRepairDates = { new DateTime(2010, 12, 5), new DateTime(2010, 12, 6), 
                                              new DateTime(2010, 12, 2),  new DateTime(2010, 10, 29),
                                              new DateTime(2010, 11, 18), new DateTime(2009, 5, 14), 
                                              new DateTime(2010, 1, 29), new DateTime(2009, 5, 23), 
                                              new DateTime(2010, 2, 28), new DateTime(2009, 5, 26),
                                              null, null,
                                              null, null,
                                              null, null
                                            };
            int[] partIds = { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
            for (int i = 0; i < autoIds.Length; i++)
            {
                Automobile auto = GetAutomobilById(autoIds[i]);
                MembershipUser user = Membership.GetUser(userNames[i % 2]);
                RepairCard card = new RepairCard()
                {
                    Automobile = auto,
                    UserId = (System.Guid)user.ProviderUserKey,
                    StartRepair = startReapirDates[i],
                };
                SparePart part = GetSparePartById(partIds[i]);
                card.SpareParts.Add(part);
                card.PartPrice = part.Price;
                card.CardPrice = part.Price;
                DateTime? finishRepair = finishRepairDates[i];
                if (finishRepair != null)
                {
                    card.FinishRepair = finishRepair;
                }
                this.carServiceEntities.RepairCards.AddObject(card);
            }

            //Automobile auto = GetAutomobilById(6);
            //MembershipUser user = Membership.GetUser("stanislav.petrov");
            //RepairCard card = new RepairCard()
            //{
            //    Automobile = auto,
            //    UserId = (System.Guid)user.ProviderUserKey,
            //    StartRepair = new DateTime(2010, 12, 01),
            //    Description = "Whole car repair"
            //};
            //RepairCard card = GetRepairCardById(2);
            //SparePart part = GetSparePartById(2);
            //card.SpareParts.Add(part);
            //card.PartPrice = part.Price;
            //IQueryable<SparePart> spareParts =
            //    from part in carServiceEntities.SpareParts
            //    where part.PartId == 1
            //    select part;
            //decimal partsPrice = 0m;
            //foreach (SparePart sp in spareParts)
            //{
            //    card.SpareParts.Add(sp);
            //    partsPrice += sp.Price;
            //}
            //card.PartPrice = partsPrice;
            //card.CardPrice = partsPrice;
            //this.carServiceEntities.RepairCards.AddObject(card);
        }

        //TODO: To be deleted
        public void TestCreateAutomobiles()
        {
            string[] vins = { "1M8GDM9", "AXKP04", "CX2788", "1G1FP22P", "XS2100001", "CA7972KK", "PK1234RT", "PA1839AC" };
            string[] makes = { "Audi", "Audi", "Citroen", "Citroen", "Opel", "Opel", "Toyota", "Toyota" };
            string[] models = { "A4", "R8", "Xsara", "Xantia", "Astra", "Insignia", "Corolla", "Avensis" };
            string[] chassisNumbers = { "XMCLRDA2A3F011237", "XABNRDA6A3F031227", "XMCLRDA2A3F011226", "XMCLRDA2A3F011225", "XMCLRDA2A3F011224", "XMCLRDA2A3F011223", "XMCLRDA2A3F011222", "YMCLRDA2A3F011222" };
            string[] engineNumbers = { "DGB060081U0017B", "DGB060081U0017C", "DGB060081U0017D", "DGB060081U0017F", "DGB060081U0016G", "DGB060081U0017H", "DGB060081U0018I", "FGB060081U0018I" };
            string[] colours = { "red", "black", "grey metallic", "blue", "yellow", "white", "black", "green" };
            int[] engineCubs = { 500, 1600, 1800, 1600, 1400, 1600, 2200, 1800 };
            string[] descriptions = { "Car with big priority", "Relatively in a good state", "Very comortable vehicle", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
            string[] owners = { "Ivan Petrov", "Georgi Tutkanov", "Stanislav Petrov", "Maria Trifonova", string.Empty, "Peter Vasilve", "Vasil Georgiev", "Katerina Ivanova" };
            string[] phoneNumbers = { "111-222-333", string.Empty, "987-654-321", string.Empty, "00359881320561", string.Empty, string.Empty, string.Empty };
            DateTime[] makeYears = { new DateTime(1995, 6, 1), new DateTime(1994, 5, 2), new DateTime(1993, 4, 3), new DateTime(1992, 3, 4), new DateTime(1991, 2, 5), new DateTime(1990, 1, 6), new DateTime(1995, 6, 10), new DateTime(1996, 8, 11) };

            for (int i = 0; i < vins.Length; i++)
            {
                Automobile auto = new Automobile()
                {
                    Vin = vins[i],
                    Make = makes[i],
                    Model = models[i],
                    ChassisNumber = chassisNumbers[i],
                    EngineNumber = engineNumbers[i],
                    Colour = colours[i],
                    EngineCub = engineCubs[i],
                    MakeYear = makeYears[i]
                };
                string desc = descriptions[i];
                string owner = owners[i];
                string phoneNumber = phoneNumbers[i];
                if (string.IsNullOrEmpty(desc) == false)
                {
                    auto.Description = desc;
                }
                if (string.IsNullOrEmpty(owner) == false)
                {
                    auto.Owner = owner;
                }
                if (string.IsNullOrEmpty(phoneNumber) == false)
                {
                    auto.PhoneNumber = phoneNumber;
                }
                this.carServiceEntities.Automobiles.AddObject(auto);
            }
            //IQueryable<Automobile> foundAutos =
            //    from auto in this.carServiceEntities.Automobiles
            //    where auto.AutomobileId != 2
            //    select auto;
            //int index = 0;
            //foreach (Automobile automobile in foundAutos)
            //{
            //    automobile.MakeYear = makeYears[index];
            //    index++;
            //}
        }
        #endregion
    }
}


