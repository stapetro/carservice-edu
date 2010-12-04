using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace persistence{
    /// <summary>
    /// Car service persistence facade which gives access to the persistence layer.
    /// </summary>
    public interface ICarServicePersister
    {
        void CreateAutomobile(Automobile automobile);
        Automobile GetAutomobilById(int automobileId);
        void DeleteAutomobile(Automobile automobile);

        void CreateSparePart(SparePart sparePart);
        SparePart GetSparePartById(int sparePartId);
        void DeleteSparePart(SparePart sparePart);

        void CreateRepairCard(RepairCard repairCard);
        void DeleteRepairCard(RepairCard repairCard);
        RepairCard GetRepairCardById(int cardId);
        IQueryable<RepairCard> GetUnfinishedRepairCards(DateTime startRepair);
        IQueryable<RepairCard> GetUnfinishedRepairCardsByVin(DateTime startRepair, string vin);
        IQueryable<RepairCard> GetUnfinishedRepairCardsByChassisNumber(DateTime startRepair, string chassisNumber);
        IQueryable<RepairCard> GetFinishedRepairCards(DateTime fromFinishRepair, DateTime toFinishRepair);

        void SaveChanges();
        void ReleaseConnection();
    }
}