using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace persistence
{
    /// <summary>
    /// Car service persistence facade which gives access to the persistence layer.
    /// </summary>
    public interface ICarServicePersister
    {
        void CreateAutomobile(Automobile automobile);
        Automobile GetAutomobilById(int automobileId);
        ObjectSet<Automobile> GetAutomobiles();
        IQueryable<Automobile> GetAutomobilesByVinChassis(string vinChassis);
        void DeleteAutomobile(Automobile automobile);
        bool IsChassisNumberExists(string chasshisNumber);
        bool IsVinExists(string vin);

        void CreateSparePart(SparePart sparePart);
        SparePart GetSparePartById(int sparePartId);
        int GetSparePartMaxId();
        ObjectSet<SparePart> GetSpareParts();
        void DeleteSparePart(SparePart sparePart);

        void CreateRepairCard(RepairCard repairCard);
        void DeleteRepairCard(RepairCard repairCard);
        RepairCard GetRepairCardById(int cardId);
        int GetRepairCardMaxId();
        ObjectSet<RepairCard> GetRepairCards();
        IQueryable<RepairCard> GetUnfinishedRepairCards(DateTime? startRepair, string vinChassis);
        IQueryable<RepairCard> GetFinishedRepairCards(DateTime? fromFinishRepair, DateTime? toFinishRepair);

        void SaveChanges();
        void ReleaseConnection();
    }
}