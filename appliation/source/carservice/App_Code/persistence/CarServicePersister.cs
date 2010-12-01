using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;

namespace persistence
{
    /// <summary>
    /// Summary description for CarServicePersister
    /// </summary>
    public class CarServicePersister
    {
        private Entities carServiceEntities;

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

        public void GetUnfinishedRepairCards(DateTime startRepair, string chassisNumber, string vin)
        {
            IQueryable<RepairCard> unfinishedRepairCards =
                from repairCard in this.carServiceEntities.RepairCards
                where repairCard.FinishRepair == null &&
                    repairCard.StartRepair == startRepair /*&&
                    (repairCard.Automobile.Vin.IndexOf(vin) >= 0 || 
                        repairCard.Automobile.ChassisNumber.IndexOf(chassisNumber) >= 0)*/
                select repairCard;
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
    }
}


